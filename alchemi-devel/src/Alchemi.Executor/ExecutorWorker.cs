using System;
using System.Collections.Generic;
using System.Text;
using Alchemi.Core.Owner;
using Alchemi.Core;
using System.IO;
using System.Security.Policy;
using System.Security;
using System.Threading;
using Alchemi.Core.Utility;

using System.Reflection;
using System.IO.IsolatedStorage;

namespace Alchemi.Executor.Sandbox
{
    internal class ExecutorWorker
    {
        private static readonly Logger logger = new Logger();

        private ThreadIdentifier _CurTi;
        private GExecutor _executor;

        private Thread _execThread;

        internal ExecutorWorker(GExecutor executor, ThreadIdentifier ti)
        {
            _CurTi = ti;
            _executor = executor;
        }

        #region Property - _Manager
        private IManager _Manager
        {
            get
            {
                return _executor.Manager;
            }
        } 
        #endregion


        #region Property - _GridAppDomains
        private IDictionary<string, GridAppDomain> _GridAppDomains
        {
            get
            {
                return _executor._GridAppDomains;
            }
        } 
        #endregion


        #region Property - _Credentials
        private SecurityCredentials _Credentials
        {
            get
            {
                return _executor.Credentials;
            }
        } 
        #endregion


        #region Method - GetManifestAndSetupDomain
        // the caller of this method will lock the GridAppDomains, so no worries:
        // no need to lock it here.
        private void GetManifestAndSetupDomain()
        {
            string appDir = ExecutorUtil.GetApplicationDirectory(_CurTi.ApplicationId);
            logger.Debug("AppDir on executor=" + appDir);

            // make sure that by the time the lock was acquired the app domain is still not created
            if (!_GridAppDomains.ContainsKey(_CurTi.ApplicationId))
            {
                // create application domain for newly encountered grid application
                logger.Debug("app dir on executor: " + appDir);

                if (!Directory.Exists(appDir))
                    Directory.CreateDirectory(appDir);

                FileDependencyCollection manifest = _Manager.Executor_GetApplicationManifest(
                    _Credentials, _CurTi.ApplicationId);
                if (manifest != null)
                {
                    foreach (FileDependency dep in manifest)
                    {
                        logger.Debug("Unpacking file: " + dep.FileName + " to " + appDir);
                        dep.UnpackToFolder(appDir);
                    }
                }
                else
                {
                    logger.Warn("Executor_GetApplicationManifest from the Manager returned null");
                }

                CreateSandboxDomain(appDir);

                logger.Info("Created app domain, policy, got instance of GridAppDomain and added to hashtable...all done once for this application");
            }
            else
            {
                logger.Info("I got the lock but this app domain is already created.");
            }
        } 
        #endregion


        #region Method - ExecuteThreadInAppDomain
        // if we have an exception in the secondary appdomain, it will raise an exception in this method,
        // since the cross-app-domain call uses remoting internally, and it is just as if a remote method 
        // has caused an exception. we have a handler for that below anyway.
        private void ExecuteThreadInAppDomain()
        {
            byte[] rawThread = null;
            GridAppDomain gad = null;
            string threadDir = null;
            try
            {
                logger.Info("Started ExecuteThreadInAppDomain...");
                logger.Info(string.Format("executing grid thread # {0}", _CurTi.UniqueId));

                lock (_GridAppDomains)
                {
                    if (!_GridAppDomains.ContainsKey(_CurTi.ApplicationId))
                    {
                        GetManifestAndSetupDomain(); //do this only once per app.
                    }
                    gad = _GridAppDomains[_CurTi.ApplicationId];
                }

                //get thread from manager
                rawThread = _Manager.Executor_GetThread(_Credentials, _CurTi);
                logger.Debug("Got thread from manager. executing it: " + _CurTi.ThreadId);

                threadDir = Path.Combine(gad.Domain.BaseDirectory, _CurTi.ThreadId.ToString());
                Directory.CreateDirectory(threadDir);

                //execute it in its own thread-directory.           
                byte[] finishedThread = gad.Executor.ExecuteThread(rawThread, threadDir);

                logger.Info(string.Format("ExecuteThread returned for thread # {0}", _CurTi.UniqueId));

                //set its status to finished
                _Manager.Executor_SetFinishedThread(_Credentials, _CurTi, finishedThread, null);
                logger.Info(string.Format("Finished executing grid thread # {0}", _CurTi.UniqueId));

            }
            catch (ThreadAbortException)
            {
                if (_CurTi != null)
                    logger.Warn(string.Format("aborted grid thread # {0}", _CurTi.UniqueId));
                else
                    logger.Warn(string.Format("aborted grid thread # {0}", null));

                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                logger.Warn(string.Format("grid thread # {0} failed ({1})", _CurTi.UniqueId, e.GetType()), e);
                try
                {
                    _Manager.Executor_SetFinishedThread(_Credentials, _CurTi, rawThread, e);
                }
                catch (Exception ex1)
                {
                    if (_CurTi != null)
                    {
                        logger.Warn("Error trying to set failed thread for App: " + _CurTi.ApplicationId + ", thread=" + _CurTi.ThreadId + ". Original Exception = \n" + e, ex1);
                    }
                    else
                    {
                        logger.Warn("Error trying to set failed thread: Original exception = " + e, ex1);
                    }
                }
            }
            finally
            {
                try
                {
                    logger.Debug("Not Trying to delete thread dir: " + threadDir);
                    //if (threadDir != null)
                    //    Directory.Delete(threadDir, true);
                }
                catch { }
                logger.Info("Exited ExecuteThreadInAppDomain...");
            }
        } 
        #endregion


        #region Method - GetPermissionSetForApp
        private PermissionSet GetPermissionSetForApp(string readonlyDir, string appDir)
        {
            SecurityConfig scfg = null;
            try
            {
                //first load the config 
                scfg = SecurityConfig.Load();
            }
            catch (Exception ex)
            {
                logger.Warn("Error loading security config", ex);
                //load the default permission set, and save it to file.
                scfg = new SecurityConfig(true);
                try
                {
                    scfg.Save();
                }
                catch (Exception ex1)
                {
                    logger.Warn("Error saving security config", ex1);
                }
            }

            //return scfg.GetGThreadPermissions(readonlyDir, appDir);
            PermissionSet ps = (PolicyLevel.CreateAppDomainLevel()).GetNamedPermissionSet("FullTrust");
            return ps;
        } 
        #endregion


        #region Method - CreateSandboxDomain
        private void CreateSandboxDomain(string appDir)
        {
            Type appDomainExecutorType = typeof(AppDomainExecutor);
            AppDomainSetup info = new AppDomainSetup();

            //we want to seperate the app-base of the new sandboxed domain
            //so that it won't be able to touch the files in the executor itself.
            info.ApplicationBase = appDir;

            string readOnlyDir = Path.GetDirectoryName(appDomainExecutorType.Assembly.Location);
            PermissionSet grantSet = GetPermissionSetForApp(readOnlyDir, appDir);

            // need to load config file here
            string appConfigFile = Path.Combine(appDir, "App.config");
            if (File.Exists(appConfigFile))
                info.ConfigurationFile = "App.config";

            //we could have some assemblies run with higher trust here if needed.
            AppDomain domain = AppDomain.CreateDomain(_CurTi.ApplicationId, null, info, grantSet, null);

            //need to copy the core dll into the new app-base, so that its types can be accessed.
            //TODOLATER: is there a better way, than copying Alchemi.Core to appdir? perhaps put it into GAC?
            string src = typeof(Alchemi.Core.Logger).Assembly.Location;
            string dest = Path.Combine(info.ApplicationBase, Path.GetFileName(src));
            if (!File.Exists(dest))
            {
                File.Copy(src, dest);
            }

            AppDomainExecutor executor = (AppDomainExecutor)domain.CreateInstanceFromAndUnwrap(
                appDomainExecutorType.Assembly.Location,
                appDomainExecutorType.FullName);
            executor.LogWriter = logger;

            //_GridAppDomains is locked by caller, so we don't need to worry about that.
            _GridAppDomains.Add(_CurTi.ApplicationId, new GridAppDomain(domain, executor));
        } 
        #endregion


        #region Method - Start
        internal void Start()
        {
            _execThread = new Thread(new ThreadStart(ExecuteThreadInAppDomain));
            _execThread.Name = "ExecutorWorker-" + _CurTi.UniqueId;
            _execThread.Priority = ThreadPriority.Lowest;
            _execThread.Start();
        } 
        #endregion


        #region Method - Stop
        internal void Stop()
        {
            if (_execThread != null && _execThread.IsAlive)
            {
                _execThread.Abort();
            }
        } 
        #endregion
    }
}
