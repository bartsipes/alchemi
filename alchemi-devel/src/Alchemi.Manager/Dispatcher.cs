using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Alchemi.Core;
using System.Data.Common;

namespace Alchemi.Manager
{
    internal class Dispatcher
    {
        private static readonly Logger logger = new Logger();

        private bool _stop = false;
        private Thread _DispatcherThread;

        internal Dispatcher()
        {
        }

        internal void Start()
        {
            _stop = false;
            _DispatcherThread = new Thread(new ThreadStart(StartDispatch));
            _DispatcherThread.Name = "DispatcherThread";
            _DispatcherThread.Start();
        }

        internal void Stop()
        {
            _stop = true;
            InternalShared.Instance.DedicatedSchedulerActive.Set();
            _DispatcherThread.Join(2000);
        }

        private void StartDispatch()
        {
            logger.Info("Dispatcher thread started.");
            try
            {
                // TODO: allow scheduling of multiple threads in one go
                while (!_stop)
                {
                    try
                    {
                        //logger.Debug("WaitOne for 1000 millis on DedicatedSchedulerActive");
                        InternalShared.Instance.DedicatedSchedulerActive.WaitOne(1000, false);

                        //logger.Debug("Getting a dedicated schedule");
                        DedicatedSchedule ds = InternalShared.Instance.Scheduler.ScheduleDedicated();

                        if (ds == null)
                        {
                            //to avoid blocking again if stop has been called.
                            if (!_stop)
                            {
                                InternalShared.Instance.DedicatedSchedulerActive.Reset();
                                //logger.Debug("Dedicated schedule is null. Reset the DedicatedSchedulerActive waithandle");
                            }
                        }
                        else
                        {
                            logger.Debug(String.Format("Scheduler mapped thread {0} to executor: {1}", ds.TI.ThreadId, ds.ExecutorId));
                            MExecutor me = new MExecutor(ds.ExecutorId); 
                            try
                            {
                                // dispatch thread
                                me.ExecuteThread(ds);

                                /// tb@tbiro.com - Feb 28, 2006:
                                /// updating the thread's status is done inside ExecuteThread after it was decided 
                                /// whether this executor can take it or not
                                /// this prevents race conditions if the Executor finishes very quickly
                                /// and we overwrite here the Executor's status back to Scheduled after it was finished
                                //		// update thread state 'after' it is dispatched. (kna changed this: aug19,05). to prevent the scheduler from hanging here.
                                //		mt.CurrentExecutorId = ds.ExecutorId;
                                //		mt.State = ThreadState.Scheduled;
                                //		logger.Debug("Scheduled thread " + ds.TI.ThreadId + " to executor:"+ds.ExecutorId);
                            }
                            catch (Exception e)
                            {
                                logger.Error("Some error occured trying to schedule. Reset-ing the thread to be scheduled. Continuing...", e);
                                MThread mt = new MThread(ds.TI);
                                mt.Reset(); // this should happen as part of the disconnection
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        logger.Debug("Dispatcher Thread resetting abort (1)...");
                        Thread.ResetAbort();
                    }
                    catch (Exception e)
                    {
                        if (e is DbException)
                        {
                            //some problem contacting database
                            //wait for a bit and try again
                            logger.Debug("Error contacting database:", e);
                            Thread.Sleep(30000);
                            //TODO: need to provide fault tolerance here: what if the database/storage cannot be contacted.?
                            //TODO: in that case, just raise an event, and let the Service/exec deal with it.
                        }
                        else
                        {
                            logger.Error("Dispatch thread error. Continuing...", e);
                        }
                    }
                } //while
            }
            catch (ThreadAbortException)
            {
                logger.Debug("Scheduler Thread resetting abort (2)...");
                Thread.ResetAbort();
            }
            catch (Exception e)
            {
                logger.Error("StartDispatch thread error. Scheduler thread stopped.", e);
            }
            logger.Info("Scheduler thread exited.");
        }
    }
}
