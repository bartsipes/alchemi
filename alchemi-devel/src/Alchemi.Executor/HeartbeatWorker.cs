using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Alchemi.Core;
using Alchemi.Core.Executor;
using System.Diagnostics;
using System.Net.Sockets;

namespace Alchemi.Executor
{
    internal class HeartbeatWorker
    {
        private Logger logger;
        /// <summary>
        /// Default heart beat interval (5 seconds)
        /// </summary>
        internal const int DEFAULT_INTERVAL = 5;

        private bool _stop;
        private int _HeartbeatInterval;

        private Thread _HeartbeatThread;

        private GExecutor _executor;

        internal HeartbeatWorker(GExecutor executor)
        {
            _stop = false;
            _HeartbeatInterval = DEFAULT_INTERVAL;
            _executor = executor;
            logger = new Logger();
        }

        /// <summary>
        /// Gets or sets the heartbeat interval for the executor (in seconds).
        /// </summary>
        internal int Interval
        {
            get
            {
                return _HeartbeatInterval;
            }
            set
            {
                _HeartbeatInterval = value;
            }
        }

        internal void Start()
        {
            _stop = false;
            _HeartbeatThread = new Thread(new ThreadStart(Heartbeat));
            _HeartbeatThread.Name = "HeartBeat";
            _HeartbeatThread.IsBackground = true;
            _HeartbeatThread.Priority = ThreadPriority.Lowest;
            _HeartbeatThread.Start();
        }

        internal void Stop()
        {
            //dont wait for it to stop, its a background thread :
            //so it will be killed with the process anyway.
            _stop = true; 
        }

        private void Heartbeat()
        {
            int pingFailCount = 0;

            logger.Info("HeartBeat Thread Started.");

            //heart-beat thread handles its own errors.
            try
            {
                HeartbeatInfo info = new HeartbeatInfo();
                info.Interval = _HeartbeatInterval;

                // init for cpu usage
                TimeSpan oldTime = Process.GetCurrentProcess().TotalProcessorTime;
                DateTime timeMeasured = DateTime.Now;
                TimeSpan newTime = new TimeSpan(0);

                //TODO: be language neutral here. how??
                // init for cpu avail
                PerformanceCounter cpuCounter = new PerformanceCounter();
                cpuCounter.ReadOnly = true;
                cpuCounter.CategoryName = "Processor";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = "_Total";

                while (!_stop)
                {
                    // calculate usage
                    newTime = Process.GetCurrentProcess().TotalProcessorTime;
                    TimeSpan absUsage = newTime - oldTime;
                    float flUsage = ((float)absUsage.Ticks / (DateTime.Now - timeMeasured).Ticks) * 100;
                    info.PercentUsedCpuPower = (int)flUsage > 100 ? 100 : (int)flUsage;
                    info.PercentUsedCpuPower = (int)flUsage < 0 ? 0 : (int)flUsage;
                    timeMeasured = DateTime.Now;
                    oldTime = newTime;

                    try
                    {
                        // calculate avail
                        info.PercentAvailCpuPower = 100 - (int)cpuCounter.NextValue() + info.PercentUsedCpuPower;
                        info.PercentAvailCpuPower = info.PercentAvailCpuPower > 100 ? 100 : info.PercentAvailCpuPower;
                        info.PercentAvailCpuPower = info.PercentAvailCpuPower < 0 ? 0 : info.PercentAvailCpuPower;
                    }
                    catch (Exception e)
                    {
                        logger.Debug("HeartBeat thread error: " + e.Message + Environment.NewLine + " Heartbeat continuing after error...");
                    }

                    //have a seperate try...block since we try 3 times before giving up
                    try
                    {
                        //send a heartbeat to the manager.
                        _executor.Manager.Executor_Heartbeat(_executor.Credentials, _executor.Id, info);
                        pingFailCount = 0;
                    }
                    catch (Exception e)
                    {
                        if (e is SocketException  || e is System.Runtime.Remoting.RemotingException)
                        {
                            pingFailCount++;
                            //we disconnect the executor if the manager cant be pinged three times
                            if (pingFailCount >= 3)
                            {
                                logger.Error("Failed to contact manager " + pingFailCount + " times...", e);

                                //the disconnect here should be started off on a seperate thread because:
                                //disconnect itself waits for HeartBeatThread to stop. If the method call
                                //to disconnect from HeartBeat wont return immediately, then there is a deadlock
                                //with disconnect waiting for the HeartBeatThread to stop and the HeartBeatThread waiting
                                //for the call to disconnect to return.

                                //raise the event to indicate that the Executor has got disconnected.
                                _executor.OnGotDisconnected();
                            }
                        }
                        else if (e is InvalidExecutorException)
                        {
                            //raise a disconnected event.
                            //so that we will reconnect if needed.
                            logger.Error("Invalid Executor exception : ", e);
                            _executor.OnGotDisconnected();
                        }
                        else
                        {
                            logger.Debug("Error during heartbeat. Continuing after error...", e);
                        }
                    }

                    Thread.Sleep(_HeartbeatInterval * 1000);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
                logger.Info("HeartBeat thread aborted.");
            }
            catch (Exception e)
            {
                logger.Error("HeartBeat Exception. Heartbeat thread stopping...", e);
            }

            logger.Info("HeartBeat Thread Exited.");
        }
    }
}
