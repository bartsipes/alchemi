using System;
using System.Collections.Generic;
using System.Text;
using Alchemi.Core.Manager.Storage;
using System.Collections;
using Db4objects.Db4o;
using System.IO;
using Alchemi.Core.Manager;
using Alchemi.Core;
using Alchemi.Core.Owner;
using Alchemi.Core.Utility;

namespace Alchemi.Manager.Storage
{
    public class db4oManagerStorage : ManagerStorageBase, IManagerStorage, IManagerStorageSetup, IDisposable
    {
        private string _DBPath = @"c:\temp\test.db";
        private IObjectServer _db4oServer;

        public db4oManagerStorage(string dbFile)
        {
            _DBPath = dbFile;
            //_db4oServer = Db4oFactory.OpenServer(_DBPath, 0);
        }

        public string DBPath
        {
            get { return _DBPath; }
        }

        #region IManagerStorage Members

        public bool VerifyConnection()
        {
            return File.Exists(_DBPath);
        }

        public bool CheckPermission(SecurityCredentials sc, Permission perm)
        {

            if (sc == null)
            {
                return false;
            }
            UserStorageView user = null;
            IObjectContainer container = GetStorage();
            Int32 groupId = -1;
            try
            {
                IList<UserStorageView> users = container.Query<UserStorageView>(delegate(UserStorageView userFinder)
                {
                    return String.Compare(userFinder.Username, sc.Username, true) == 0 && userFinder.PasswordMd5Hash == sc.Password;
                });
                if (users.Count > 0)
                    user = users[0];

                if (user == null)
                    return false;
                else
                    groupId = user.GroupId;
            }
            finally
            {
                container.Close();
            }

            foreach (Permission permission in GetGroupPermissions(groupId))
            {
                // in the SQL implementation the higher leverl permissions are considered to 
                // include the lower leverl permissions
                if ((int)permission >= (int)perm)
                {
                    return true;
                }
            }

            return false;
        }

        public bool AuthenticateUser(Alchemi.Core.SecurityCredentials sc)
        {
            if (sc == null)
            {
                return false;
            }
            bool isPresent = false;
            IObjectContainer container = GetStorage();
            try
            {
                IList<UserStorageView> users = container.Query<UserStorageView>(delegate(UserStorageView user)
                {
                    return user.Username == sc.Username && user.PasswordMd5Hash == sc.Password;
                });
                if (users.Count > 0)
                    isPresent = true;
            }
            finally
            {
                container.Close();
            }
            return isPresent;

        }

        public void AddUsers(UserStorageView[] users)
        {
            if (users == null)
                return;

            IObjectContainer container = GetStorage();
            try
            {
                foreach (UserStorageView user in users)
                    container.Set(user);
            }
            finally
            {
                container.Close();
            }
        }

        public void UpdateUsers(UserStorageView[] updates)
        {
            if (updates == null)
            {
                return;
            }

            IObjectContainer container = GetStorage();
            try
            {
                foreach (UserStorageView userInUpdates in updates)
                {
                    UserStorageView user = null;
                    IList<UserStorageView> users = container.Query<UserStorageView>(delegate(UserStorageView userFinder)
                    {
                        return userFinder.Username == userInUpdates.Username;
                    });
                    if (users.Count > 0)
                    {
                        user = users[0];
                        user.Password = userInUpdates.Password;
                        user.GroupId = userInUpdates.GroupId;
                        container.Set(user);
                    }
                }
            }
            finally
            {
                container.Close();
            }
        }

        public UserStorageView[] GetUsers()
        {
            UserStorageView[] allUsers;
            IObjectContainer container = GetStorage();
            try
            {
                IList<UserStorageView> users =
                    container.Query<UserStorageView>(delegate(UserStorageView userFinder)
                {
                    return true;
                });


                if (users.Count > 0)
                {
                    allUsers = new UserStorageView[users.Count];
                    users.CopyTo(allUsers, 0);
                }
                else
                    allUsers = new UserStorageView[0];
            }
            finally
            {
                container.Close();
            }
            return allUsers;
        }

        public void DeleteUser(UserStorageView userToDelete)
        {
            if (userToDelete == null)
            {
                return;
            }

            IObjectContainer container = GetStorage();
            try
            {
                IList<UserStorageView> users =
                    container.Query<UserStorageView>(delegate(UserStorageView userFinder)
                {
                    return userFinder.Username == userToDelete.Username;
                });

                if (users.Count > 0)
                    container.Delete(users[0]);
            }
            finally
            {
                container.Close();
            }
        }

        public void AddGroups(GroupStorageView[] groups)
        {
            if (groups == null)
                return;
            IObjectContainer container = GetStorage();
            try
            {
                foreach (GroupStorageView group in groups)
                    container.Set(group);
            }
            finally
            {
                container.Close();
            }
        }

        public GroupStorageView[] GetGroups()
        {
            GroupStorageView[] allGroups;
            IObjectContainer container = GetStorage();
            try
            {
                IList<GroupStorageView> groups =
                    container.Query<GroupStorageView>(delegate(GroupStorageView groupFinder)
                {
                    return true;
                });


                if (groups.Count > 0)
                {
                    allGroups = new GroupStorageView[groups.Count];
                    groups.CopyTo(allGroups, 0);
                }
                else
                    allGroups = new GroupStorageView[0];
            }
            finally
            {
                container.Close();
            }
            return allGroups;
        }

        public GroupStorageView GetGroup(int groupId)
        {
            GroupStorageView group = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<GroupStorageView> groups =
                    container.Query<GroupStorageView>(delegate(GroupStorageView groupFinder)
                {
                    return groupFinder.GroupId == groupId;
                });

                if (groups.Count > 0)
                    group = groups[0];
            }
            finally
            {
                container.Close();
            }
            return group;
        }

        public void AddGroupPermission(int groupId, Permission permission)
        {
            Hashtable groupPerms = null;
            ArrayList permissions = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<Hashtable> groupPermissionsList =
                    container.Query<Hashtable>(delegate(Hashtable groupPermissions)
                {
                    return true;
                });

                if (groupPermissionsList.Count == 0)
                    groupPerms = new Hashtable();
                else
                    groupPerms = groupPermissionsList[0];

                if (groupPerms[groupId] != null)
                {
                    permissions = (ArrayList)groupPerms[groupId];
                }
                else
                {
                    permissions = new ArrayList();

                    groupPerms.Add(groupId, permissions);
                }

                Int32 index = permissions.IndexOf(permission);

                // only add it if it is not already in the array
                if (index < 0)
                {
                    permissions.Add(permission);
                }

                groupPerms[groupId] = permissions;
                container.Set(groupPerms);
            }
            finally
            {
                container.Close();
            }
        }

        public Permission[] GetGroupPermissions(int groupId)
        {

            Hashtable groupPermissions = null;
            ArrayList permissions = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<Hashtable> groupPermissionsList =
                    container.Query<Hashtable>(delegate(Hashtable groupPermissionsFinder)
                {
                    return true;
                });

                if (groupPermissionsList.Count == 0)
                    groupPermissions = new Hashtable();
                else
                    groupPermissions = groupPermissionsList[0];

                if (groupPermissions[groupId] != null)
                {
                    permissions = (ArrayList)groupPermissions[groupId];
                }
                else
                {
                    permissions = new ArrayList();

                    groupPermissions.Add(groupId, permissions);
                }
            }
            finally
            {
                container.Close();
            }

            return (Permission[])permissions.ToArray(typeof(Permission));


        }

        public PermissionStorageView[] GetGroupPermissionStorageView(int groupId)
        {
            return PermissionStorageView.GetPermissionStorageView(GetGroupPermissions(groupId));
        }

        public void DeleteGroup(GroupStorageView groupToDelete)
        {
            if (groupToDelete == null)
            {
                return;
            }

            IObjectContainer container = GetStorage();
            try
            {
                IList<UserStorageView> users =
                    container.Query<UserStorageView>(delegate(UserStorageView userFinder)
                {
                    return userFinder.GroupId == groupToDelete.GroupId;
                });
                foreach (UserStorageView user in users)
                    container.Delete(user);

                IList<GroupStorageView> groups =
                    container.Query<GroupStorageView>(delegate(GroupStorageView groupFinder)
                {
                    return groupFinder.GroupId == groupToDelete.GroupId;
                });

                if (groups.Count > 0)
                    container.Delete(groups[0]);
            }
            finally
            {
                container.Close();
            }
        }

        public UserStorageView[] GetGroupUsers(int groupId)
        {
            UserStorageView[] groupUsers;
            IObjectContainer container = GetStorage();
            try
            {
                IList<UserStorageView> users =
                    container.Query<UserStorageView>(delegate(UserStorageView userFinder)
                {
                    return userFinder.GroupId == groupId;
                });


                if (users.Count > 0)
                {
                    groupUsers = new UserStorageView[users.Count];
                    users.CopyTo(groupUsers, 0);
                }
                else
                    groupUsers = new UserStorageView[0];
            }
            finally
            {
                container.Close();
            }
            return groupUsers;
        }

        public string AddExecutor(ExecutorStorageView executor)
        {
            if (executor == null)
            {
                return null;
            }

            String executorId;
            if (executor.ExecutorId == null)
            {
                executorId = Guid.NewGuid().ToString();
            }
            else
            {
                executorId = executor.ExecutorId;
            }

            executor.ExecutorId = executorId;

            IObjectContainer container = GetStorage();
            try
            {
                container.Set(executor);
            }
            finally
            {
                container.Close();
            }
            return executorId;
        }

        public void UpdateExecutor(ExecutorStorageView updatedExecutor)
        {
            ExecutorStorageView executor = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ExecutorStorageView> executors =
                    container.Query<ExecutorStorageView>(delegate(ExecutorStorageView executorFinder)
                {
                    return executorFinder.ExecutorId == updatedExecutor.ExecutorId;
                });

                if (executors.Count > 0)
                {
                    executor = executors[0];
                    container.Delete(executor);
                    container.Set(updatedExecutor);
                }
            }
            finally
            {
                container.Close();
            }
        }

        public void DeleteExecutor(ExecutorStorageView executor)
        {
            if (executor == null)
            {
                return;
            }

            IObjectContainer container = GetStorage();
            try
            {
                IList<ExecutorStorageView> executors =
                    container.Query<ExecutorStorageView>(delegate(ExecutorStorageView executorFinder)
                {
                    return executorFinder.ExecutorId == executor.ExecutorId;
                });

                if (executors.Count > 0)
                    container.Delete(executors[0]);
            }
            finally
            {
                container.Close();
            }
        }

        public ExecutorStorageView[] GetExecutors()
        {
            ExecutorStorageView[] allExecutors;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ExecutorStorageView> executors =
                    container.Query<ExecutorStorageView>(delegate(ExecutorStorageView executorFinder)
                {
                    return true;
                });


                if (executors.Count > 0)
                {
                    allExecutors = new ExecutorStorageView[executors.Count];
                    executors.CopyTo(allExecutors, 0);
                }
                else
                    allExecutors = new ExecutorStorageView[0];
            }
            finally
            {
                container.Close();
            }
            return allExecutors;
        }

        public ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated)
        {
            return GetExecutors(dedicated, TriStateBoolean.Undefined);
        }

        public ExecutorStorageView[] GetExecutors(TriStateBoolean dedicated, TriStateBoolean connected)
        {
            ExecutorStorageView[] allExecutors;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ExecutorStorageView> executors =
                    container.Query<ExecutorStorageView>(delegate(ExecutorStorageView executorFinder)
                {
                    return ((dedicated == TriStateBoolean.True && executorFinder.Dedicated)
                        || dedicated == TriStateBoolean.Undefined) &&
                        ((connected == TriStateBoolean.True && executorFinder.Connected)
                        || connected == TriStateBoolean.Undefined);
                });


                if (executors.Count > 0)
                {
                    allExecutors = new ExecutorStorageView[executors.Count];
                    executors.CopyTo(allExecutors, 0);
                }
                else
                    allExecutors = new ExecutorStorageView[0];
            }
            finally
            {
                container.Close();
            }
            return allExecutors;
        }

        public ExecutorStorageView GetExecutor(string executorId)
        {
            ExecutorStorageView executor = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ExecutorStorageView> executors =
                    container.Query<ExecutorStorageView>(delegate(ExecutorStorageView executorFinder)
                {
                    return executorFinder.ExecutorId == executorId;
                });

                if (executors.Count > 0)
                    executor = executors[0];
            }
            finally
            {
                container.Close();
            }
            return executor;
        }

        public string AddApplication(ApplicationStorageView application)
        {
            if (application == null)
            {
                return null;
            }

            String applicationId = Guid.NewGuid().ToString();

            application.ApplicationId = applicationId;

            IObjectContainer container = GetStorage();
            try
            {
                container.Set(application);
            }
            finally
            {
                container.Close();
            }
            return applicationId;
        }

        public void UpdateApplication(ApplicationStorageView updatedApplication)
        {
            ApplicationStorageView application = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ApplicationStorageView> apps =
                    container.Query<ApplicationStorageView>(delegate(ApplicationStorageView app)
                {
                    return app.ApplicationId == updatedApplication.ApplicationId;
                });

                if (apps.Count > 0)
                {
                    application = apps[0];
                    container.Delete(application);
                    container.Set(updatedApplication);
                }
            }
            finally
            {
                container.Close();
            }
        }

        public ApplicationStorageView[] GetApplications()
        {
            return GetApplications(false);
        }

        public ApplicationStorageView[] GetApplications(bool populateThreadCount)
        {
            return GetApplications(null, populateThreadCount);
        }

        public ApplicationStorageView[] GetApplications(string userName, bool populateThreadCount)
        {
            ApplicationStorageView[] allApplications;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ApplicationStorageView> applicationsList =
                    container.Query<ApplicationStorageView>(delegate(ApplicationStorageView applicationFinder)
                {
                    return string.IsNullOrEmpty(userName)
                        || String.Compare(applicationFinder.Username, userName, false) == 0;
                });

                foreach (ApplicationStorageView application in applicationsList)
                {
                    if (populateThreadCount)
                    {
                        Int32 totalThreads;
                        Int32 unfinishedThreads;

                        GetApplicationThreadCount(application.ApplicationId, out totalThreads, out unfinishedThreads);

                        application.TotalThreads = totalThreads;
                        application.UnfinishedThreads = unfinishedThreads;
                    }
                }
                if (applicationsList.Count > 0)
                {
                    allApplications = new ApplicationStorageView[applicationsList.Count];
                    applicationsList.CopyTo(allApplications, 0);
                }
                else
                    allApplications = new ApplicationStorageView[0];
            }
            finally
            {
                container.Close();
            }

            return allApplications;
        }

        public void DeleteApplication(ApplicationStorageView applicationToDelete)
        {
            if (applicationToDelete == null)
            {
                return;
            }

            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threads =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ApplicationId == applicationToDelete.ApplicationId;
                });
                foreach (ThreadStorageView thread in threads)
                    container.Delete(thread);

                IList<ApplicationStorageView> apps =
                    container.Query<ApplicationStorageView>(delegate(ApplicationStorageView appFinder)
                {
                    return appFinder.ApplicationId == applicationToDelete.ApplicationId;
                });
                foreach (ApplicationStorageView app in apps)
                    container.Delete(app);

            }
            finally
            {
                container.Close();
            }
        }


        public ApplicationStorageView GetApplication(string applicationId)
        {
            ApplicationStorageView application = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ApplicationStorageView> apps =
                    container.Query<ApplicationStorageView>(delegate(ApplicationStorageView app)
                {
                    return app.ApplicationId == applicationId;
                });

                if (apps.Count > 0)
                    application = apps[0];
            }
            finally
            {
                container.Close();
            }
            return application;
        }

        public int AddThread(ThreadStorageView thread)
        {
            if (thread == null)
            {
                return -1;
            }

            ArrayList threads = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ArrayList> threadsList =
                    container.Query<ArrayList>(delegate(ArrayList threadsFinder)
                {
                    return true;
                });
                if (threadsList.Count > 0)
                    threads = threadsList[0];
                else
                    threads = new ArrayList();

                thread.InternalThreadId = threads.Count;
                threads.Add(thread);
                container.Set(threads);
            }
            finally
            {
                container.Close();
            }
            return thread.InternalThreadId;
        }

        public void UpdateThread(ThreadStorageView updatedThread)
        {
            ThreadStorageView thread = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threads =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ThreadId == updatedThread.ThreadId;
                });

                if (threads.Count > 0)
                {
                    thread = threads[0];
                    container.Delete(thread);
                    container.Set(updatedThread);
                }
            }
            finally
            {
                container.Close();
            }
        }

        public ThreadStorageView GetThread(string applicationId, int threadId)
        {
            ThreadStorageView thread = null;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threads =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ApplicationId == applicationId && threadFinder.ThreadId == threadId;
                });

                if (threads.Count > 0)
                    thread = threads[0];
            }
            finally
            {
                container.Close();
            }
            return thread;
        }

        public ThreadStorageView[] GetThreads(ApplicationState appState, params ThreadState[] threadStates)
        {
            ArrayList threadList = new ArrayList();

            IObjectContainer container = GetStorage();
            try
            {
                IList<ApplicationStorageView> apps =
                    container.Query<ApplicationStorageView>(delegate(ApplicationStorageView app)
                {
                    return app.State == appState;
                });

                foreach (ApplicationStorageView application in apps)
                {
                    if (application.State == appState)
                    {
                        foreach (ThreadStorageView thread in GetThreads(application.ApplicationId, threadStates))
                        {
                            threadList.Add(thread);
                        }
                    }
                }
            }
            finally
            {
                container.Close();
            }

            return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
        }

        public ThreadStorageView[] GetThreads(params ThreadState[] state)
        {
            return GetThreads(null, state);
        }

        public ThreadStorageView[] GetThreads(String applicationId, params ThreadState[] threadStates)
        {
            ArrayList threadList = new ArrayList();

            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threadsList =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return applicationId == null || threadFinder.ApplicationId == applicationId;
                });

                foreach (ThreadStorageView thread in threadsList)
                {
                    bool threadStateCorrect = false;

                    if (threadStates == null || threadStates.Length == 0)
                    {
                        threadStateCorrect = true;
                    }
                    else
                    {
                        foreach (ThreadState state in threadStates)
                        {
                            if (state == thread.State)
                            {
                                threadStateCorrect = true;
                                break;
                            }
                        }
                    }

                    if (threadStateCorrect)
                    {
                        threadList.Add(thread);
                    }
                }
            }
            finally
            {
                container.Close();
            }

            return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
        }

        public ThreadStorageView[] GetExecutorThreads(String executorId, params ThreadState[] threadStates)
        {
            ArrayList threadList = new ArrayList();

            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threadsList =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ExecutorId == executorId;
                });
                foreach (ThreadStorageView thread in threadsList)
                {
                    bool threadStateCorrect = false;

                    if (threadStates == null || threadStates.Length == 0)
                    {
                        threadStateCorrect = true;
                    }
                    else
                    {
                        foreach (ThreadState state in threadStates)
                        {
                            if (state == thread.State)
                            {
                                threadStateCorrect = true;
                                break;
                            }
                        }
                    }

                    if (threadStateCorrect)
                    {
                        threadList.Add(thread);
                    }
                }
            }
            finally
            {
                container.Close();
            }

            return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
        }

        public ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, params ThreadState[] state)
        {
            ArrayList threadList = new ArrayList();

            foreach (ExecutorStorageView executor in GetExecutors())
            {
                if (executor.Dedicated == dedicatedExecutor)
                {
                    threadList.AddRange(GetExecutorThreads(executor.ExecutorId, state));
                }
            }

            return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
        }

        public ThreadStorageView[] GetExecutorThreads(bool dedicatedExecutor, bool connectedExecutor, params ThreadState[] state)
        {
            ArrayList threadList = new ArrayList();

            foreach (ExecutorStorageView executor in GetExecutors())
            {
                if (executor.Dedicated == dedicatedExecutor && executor.Connected == connectedExecutor)
                {
                    threadList.AddRange(GetExecutorThreads(executor.ExecutorId, state));
                }
            }

            return (ThreadStorageView[])threadList.ToArray(typeof(ThreadStorageView));
        }

        public void GetApplicationThreadCount(string applicationId, out int totalThreads, out int unfinishedThreads)
        {
            totalThreads = unfinishedThreads = 0;

            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threadsList =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ApplicationId == applicationId;
                });

                foreach (ThreadStorageView thread in threadsList)
                {
                    totalThreads++;

                    if (thread.State == ThreadState.Ready || thread.State == ThreadState.Scheduled || thread.State == ThreadState.Started)
                    {
                        unfinishedThreads++;
                    }
                }
            }
            finally
            {
                container.Close();
            }
        }

        public int GetApplicationThreadCount(string applicationId, ThreadState threadState)
        {
            Int32 threadCount = 0;
            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threadsList =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ApplicationId == applicationId && threadFinder.State == threadState;
                });

                threadCount = threadsList.Count;
            }
            finally
            {
                container.Close();
            }
            return threadCount;
        }

        public int GetExecutorThreadCount(string executorId, params ThreadState[] threadState)
        {
            Int32 threadCount = 0;

            if (threadState == null && threadState.Length == 0)
            {
                return threadCount;
            }

            ArrayList threadList = new ArrayList();

            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threadsList =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ExecutorId != null && threadFinder.ExecutorId == executorId;
                });

                foreach (ThreadStorageView thread in threadsList)
                {
                    foreach (ThreadState state in threadState)
                    {
                        if (thread.State == state)
                        {
                            threadCount++;
                            break; // no point in continuing since there is only one state for a thread
                        }
                    }
                }
            }
            finally
            {
                container.Close();
            }

            return threadCount;
        }

        public void DeleteThread(ThreadStorageView threadToDelete)
        {
            if (threadToDelete == null)
            {
                return;
            }

            IObjectContainer container = GetStorage();
            try
            {
                IList<ThreadStorageView> threads =
                    container.Query<ThreadStorageView>(delegate(ThreadStorageView threadFinder)
                {
                    return threadFinder.ThreadId == threadToDelete.ThreadId;
                });

                if (threads.Count > 0)
                    container.Delete(threads[0]);
            }
            finally
            {
                container.Close();
            }
        }

        public SystemSummary GetSystemSummary()
        {
            // calculate total number of executors
            ExecutorStorageView[] executors = GetExecutors();
            ApplicationStorageView[] applications = GetApplications();
            ThreadStorageView[] threads = GetThreads();

            Int32 totalExecutors = executors != null ? executors.Length : 0;

            // calculate number of unfinished applications
            Int32 unfinishedApps = 0;
            foreach (ApplicationStorageView application in applications)
            {
                if (application.State == ApplicationState.AwaitingManifest || application.State == ApplicationState.Ready)
                {
                    unfinishedApps++;
                }
            }

            Int32 unfinishedThreads = 0;
            foreach (ThreadStorageView thread in threads)
            {
                if (thread.State != ThreadState.Dead && thread.State != ThreadState.Finished)
                {
                    unfinishedThreads++;
                }
            }

            float maxPowerValue = 0;
            Int32 powerUsage = 0;
            Int32 powerAvailable = 0;
            float totalUsageValue = 0;

            int connectedExecutorCount = 0;

            foreach (ExecutorStorageView executor in executors)
            {
                if (executor.Connected)
                {
                    connectedExecutorCount++;
                    maxPowerValue += executor.MaxCpu;
                    powerAvailable += executor.AvailableCpu;
                    powerUsage += executor.CpuUsage;
                    totalUsageValue += executor.TotalCpuUsage * executor.MaxCpu / (3600 * 1000);
                }
            }

            if (connectedExecutorCount != 0)
            {
                powerAvailable /= connectedExecutorCount;
                powerUsage /= connectedExecutorCount;
            }
            else
            {
                powerAvailable = 0;
                powerUsage = 0;
            }

            String powerTotalUsage = String.Format("{0} GHz*Hr", Math.Round(totalUsageValue, 6));
            String maxPower = String.Format("{0} GHz", Math.Round(maxPowerValue / 1000, 6));

            SystemSummary summary = new SystemSummary(
                maxPower,
                totalExecutors,
                powerUsage,
                powerAvailable,
                powerTotalUsage,
                unfinishedApps,
                unfinishedThreads);

            return summary;
        }

        #endregion

        #region IManagerStorageSetup Members

        public void TearDownStorage()
        {
            if (_db4oServer != null)
                _db4oServer.Close();

            if (File.Exists(_DBPath))
            {
                File.Delete(_DBPath);
                while (File.Exists(_DBPath))
                    System.Threading.Thread.Sleep(5);
            }
        }

        public void CreateStorage(String databaseName)
        {
            if (_db4oServer != null)
                _db4oServer.Close();

            _DBPath = databaseName;

            File.Delete(_DBPath);
            while (File.Exists(_DBPath))
                System.Threading.Thread.Sleep(5);
        }

        public void InitializeStorageData()
        {
            CreateDefaultObjects(this);
        }

        public void SetUpStorage()
        {
            if (_db4oServer != null)
                _db4oServer.Close();

            if (File.Exists(_DBPath))
            {
                File.Delete(_DBPath);
                while (File.Exists(_DBPath))
                    System.Threading.Thread.Sleep(5);
            }
        }
        private IObjectContainer GetStorage()
        {
            if (_db4oServer == null)
                _db4oServer = Db4oFactory.OpenServer(_DBPath, 0);

            return _db4oServer.OpenClient();
        }



        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_db4oServer != null)
            {
                _db4oServer.Close();
                _db4oServer = null;
            }
        }

        #endregion
    }
}
