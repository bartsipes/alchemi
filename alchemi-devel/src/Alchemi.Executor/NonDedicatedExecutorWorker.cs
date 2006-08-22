using System;
using System.Collections.Generic;
using System.Text;
using Alchemi.Core;
using System.Threading;
using Alchemi.Core.Owner;
using System.Net.Sockets;

namespace Alchemi.Executor
{
    internal class NonDedicatedExecutorWorker
    {
        internal const int DEFAULT_EMPTYTHREAD_INTERVAL = 2000;

        private static readonly Logger logger = new Logger();

        private bool _stop;
        private int _EmptyThreadInterval;
        private bool _ExecutingNonDedicated;

        private Thread _NonDedicatedMonitorThread;

        private GExecutor _executor;

        internal NonDedicatedExecutorWorker(GExecutor executor)
        {
            this._executor = executor;
            _stop = false;
            _EmptyThreadInterval = DEFAULT_EMPTYTHREAD_INTERVAL;
        }

        internal void Start()
        {
            _stop = false;
            _NonDedicatedMonitorThread = new Thread(new ThreadStart(NonDedicatedMonitor));
            _NonDedicatedMonitorThread.Name = "Non-dedicatedMonitor";
            _NonDedicatedMonitorThread.Priority = ThreadPriority.BelowNormal;
            _NonDedicatedMonitorThread.Start();
        }
        internal void Stop()
        {
            _stop = true; //clean way to abort thread
        }
        
        private void NonDedicatedMonitor()
        {
            bool gotDisconnected = false;
            logger.Info("NonDedicatedMonitor Thread Started.");
            try
            {
                _ExecutingNonDedicated = true;
                while (!gotDisconnected && !_stop)
                {
                    try
                    {
                        //_ReadyToExecute.WaitOne();

                        //get the next thread-id from the manager - only if we can execute something here.
                        //in non-dedicated mode...the executor pulls threads instead of the manager giving it threads to execute.
                        ThreadIdentifier ti = _executor.Manager.Executor_GetNextScheduledThreadIdentifier(_executor.Credentials, _executor.Id);

                        if (ti == null)
                        {
                            //if there is no thread to execute, sleep for a random time and ask again later
                            Random rnd = new Random();
                            Thread.Sleep(rnd.Next(_EmptyThreadInterval, _EmptyThreadInterval * 2));
                        }
                        else
                        {
                            logger.Debug("Non-dedicated mode: Calling own method to execute thread");
                            _executor.Manager_ExecuteThread(ti);
                        }
                    }
                    catch (SocketException se)
                    {
                        gotDisconnected = true;
                        logger.Error("Got disconnected. Non-dedicated mode.", se);
                    }
                    catch (System.Runtime.Remoting.RemotingException re)
                    {
                        gotDisconnected = true;
                        logger.Error("Got disconnected. Non-dedicated mode.", re);
                    }
                }

                // got disconnected
                _ExecutingNonDedicated = false;

                //_executor.OnNonDedicatedExecuting

                logger.Debug("Non-dedicated executor: Unremoting self");
                _executor.DisconnectNDE();

                //raise the event for non-dedicated mode only.
                _executor.OnGotDisconnected();

            }
            catch (ThreadAbortException)
            {
                _ExecutingNonDedicated = false;
                logger.Warn("ThreadAbortException. Non-dedicated executor");
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                logger.Error("Error in non-dedicated monitor: " + e.Message, e);
            }

            logger.Info("NonDedicatedMonitor Thread Exited.");
        }

        internal bool ExecutingNonDedicated
        {
            get
            {
                return _ExecutingNonDedicated;
            }
        }
    }
}
