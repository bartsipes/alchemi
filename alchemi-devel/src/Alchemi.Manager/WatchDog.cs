using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Manager.Storage;
using Alchemi.Manager.Storage;
using Alchemi.Core.Utility;

namespace Alchemi.Manager
{
    internal class WatchDog
    {
        private static readonly Logger logger = new Logger();

        private bool _stop = false;
        private Thread _WatchDogThread = null;

        //TODO: check : do we need this here?
        private MExecutorCollection _Executors = new MExecutorCollection();
        private MApplicationCollection _Applications = new MApplicationCollection();

        internal WatchDog()
        {
        }

        internal void Start()
        {
            _stop = false;
            _WatchDogThread = new Thread(new ThreadStart(StartWatch));
            _WatchDogThread.Name = "WatchdogThread";
            _WatchDogThread.Start();
        }

        internal void Stop()
        {
            _stop = true;
            if (_WatchDogThread != null)
            {
                logger.Info("Stopping the watchdog thread...");
                _stop = true; //cleaner way of aborting.
                _WatchDogThread.Join();
            }
        }

        private void StartWatch()
        {
            logger.Info("WatchDog thread started.");
            try
            {
                while (!_stop)
                {
                    try
                    {
                        Thread.Sleep(7000);

                        // ping dedicated executors running threads and reset executor and thread if can't ping
                        ThreadStorageView[] dedicatedRunningThreadsStorage = ManagerStorageFactory.ManagerStorage().GetExecutorThreads(true,
                            Alchemi.Core.Owner.ThreadState.Scheduled, Alchemi.Core.Owner.ThreadState.Started);

                        foreach (ThreadStorageView threadStorage in dedicatedRunningThreadsStorage)
                        {
                            MExecutor me = _Executors[threadStorage.ExecutorId];
                            try
                            {
                                me.RemoteRef.PingExecutor();
                            }
                            catch
                            {
                                me.Disconnect();
                                new MThread(threadStorage.ApplicationId, threadStorage.ThreadId).Reset();
                            }
                        }

                        // disconnect nde if not recd alive notification in the last 90 seconds
                        // TODO: make time interval configurable
                        int secondsToTimeout = 90;
                        DisconnectTimedOutExecutors(new TimeSpan(0, 0, secondsToTimeout));

                        // reset threads whose executors have been disconnected
                        ThreadStorageView[] nonDedicatedLostThreadsStorage = ManagerStorageFactory.ManagerStorage().GetExecutorThreads(
                            false,
                            false,
                            Alchemi.Core.Owner.ThreadState.Scheduled,
                            Alchemi.Core.Owner.ThreadState.Started,
                            Alchemi.Core.Owner.ThreadState.Finished,
                            Alchemi.Core.Owner.ThreadState.Dead);

                        foreach (ThreadStorageView threadStorage in nonDedicatedLostThreadsStorage)
                        {
                            new MThread(threadStorage.ApplicationId, threadStorage.ThreadId).Reset();
                            new MExecutor(threadStorage.ExecutorId).Disconnect();
                        }

                    }
                    catch (ThreadAbortException)
                    {
                        logger.Debug("StartWatch thread aborting...");
                        Thread.ResetAbort();
                    }
                    catch (Exception ex)
                    {
                        logger.Debug("Error in WatchDog thread. Continuing after error...", ex);
                    }
                } //while
            }
            catch (ThreadAbortException)
            {
                logger.Debug("StartWatch thread aborting...");
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                logger.Error("WatchDog thread error. WatchDog thread stopped.", e);
            }

            logger.Info("WatchDog thread exited.");
        }

        /// <summary>
        /// Disconnect executors if the alive notification was not received in the alloted time.
        /// </summary>
        /// <param name="time"></param>
        private void DisconnectTimedOutExecutors(TimeSpan time)
        {
            DateTime currentTime = DateTime.Now; // storing the current time so we don't unfairly disconnect executors in case this process takes some time to complete
            ExecutorStorageView[] connectedExecutorsStorage = ManagerStorageFactory.ManagerStorage().GetExecutors(TriStateBoolean.Undefined, TriStateBoolean.True);

            foreach (ExecutorStorageView executorStorage in connectedExecutorsStorage)
            {
                if (executorStorage.PingTime.Add(time) < currentTime)
                {
                    executorStorage.Connected = false;
                    ManagerStorageFactory.ManagerStorage().UpdateExecutor(executorStorage);
                }
            }

        }

    }
}
