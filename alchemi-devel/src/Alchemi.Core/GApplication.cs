#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c) 2002-2004 Akshay Luther & 2003-2004 Rajkumar Buyya 
---------------------------------------------------------------------------

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
#endregion

using System;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using Alchemi.Core.Utility;

namespace Alchemi.Core
{
    public class GApplication : GNode
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        //----------------------------------------------------------------------------------------------- 
        // member variables
        //----------------------------------------------------------------------------------------------- 
        
        private System.ComponentModel.Container components = null;
        private FileDependencyCollection _Manifest = new FileDependencyCollection();
        private ThreadCollection _Threads = new ThreadCollection();
        private string _Id = "";
        private int _LastThreadId = -1;
        private int _NumThreadsFinished = 0;
        private Thread _GetFinishedThreadsThread;
        private bool _Running = false;
        private bool _Initted = false;

        public event GThreadFinish ThreadFinish;
        public event GThreadFailed ThreadFailed;
        public event GApplicationFinish ApplicationFinish;

        //----------------------------------------------------------------------------------------------- 
        // properties
        //----------------------------------------------------------------------------------------------- 

        public FileDependencyCollection Manifest
        {
            get { return _Manifest; }
        }

        public ThreadCollection Threads
        {
            get { return _Threads; }
        }

        public string Id
        {
            get { return _Id; }
        }

        public bool Running
        {
            get { return _Running; }
        }

        internal ThreadState GetThreadState(GThread thread)
        {
            EnsureRunning();
            return (ThreadState) Manager.Owner_GetThreadState(Credentials, new ThreadIdentifier(_Id, thread.Id));
        }

        //----------------------------------------------------------------------------------------------- 
        // constructors and disposal
        //----------------------------------------------------------------------------------------------- 

        public GApplication(System.ComponentModel.IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public GApplication()
        {
            InitializeComponent();
        }

        public GApplication(GConnection connection) : base(connection) {}

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if (_Running & (_GetFinishedThreadsThread != null))
                {
                    _GetFinishedThreadsThread.Abort();
                    _GetFinishedThreadsThread.Join();
                }

                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        //----------------------------------------------------------------------------------------------- 
        // public methods
        //----------------------------------------------------------------------------------------------- 

        public void Start()
        {
            Init();

            foreach (GThread thread in _Threads)
            {
                if (thread.Id == -1)
                {
                    SetThreadOnManager(thread);
                }
            }

            StartGetFinishedThreads();
        }
        
        //----------------------------------------------------------------------------------------------- 
        
        public void StartThread(GThread thread)
        {
            Init();
            Threads.Add(thread);
            SetThreadOnManager(thread);
            StartGetFinishedThreads();
        }
        
        //----------------------------------------------------------------------------------------------- 

        public void Stop()
        {
            EnsureRunning();

            Manager.Owner_StopApplication(Credentials, this._Id);
            
            if (_GetFinishedThreadsThread != null)
            {
                _GetFinishedThreadsThread.Abort();
                _GetFinishedThreadsThread.Join();
            }

            _Running = false;
        }
        
        //----------------------------------------------------------------------------------------------- 
        
        internal void AbortThread(GThread thread)
        {
            EnsureRunning();
            Manager.Owner_AbortThread(Credentials, new ThreadIdentifier(_Id, thread.Id));
        }
        
        //----------------------------------------------------------------------------------------------- 
        // private methods
        //----------------------------------------------------------------------------------------------- 

        new private void Init()
        {
            base.Init();

            if (!_Initted)
            {
                if (Connection == null)
                {
                    throw new InvalidOperationException("No connection specified.");
                }

                _Id = Manager.Owner_CreateApplication(Credentials);
                Manager.Owner_SetApplicationManifest(Credentials, _Id, _Manifest);
                _Initted = true;
            }
        }

        //----------------------------------------------------------------------------------------------- 

        private void SetThreadOnManager(GThread thread)
        {
            thread.SetId(++_LastThreadId);
            thread.SetApplication(this);
      
            Manager.Owner_SetThread(
                Credentials, 
                new ThreadIdentifier(_Id, thread.Id, thread.Priority),
                Utils.SerializeToByteArray(thread));
        }

        //----------------------------------------------------------------------------------------------- 
        
        private void GetFinishedThreads()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(1000);
        
                    byte[][] FinishedThreads = Manager.Owner_GetFinishedThreads(Credentials, _Id);

                    _NumThreadsFinished += FinishedThreads.Length;

                    for (int i=0; i<FinishedThreads.Length; i++)
                    {
                        GThread th = (GThread) Utils.DeserializeFromByteArray(FinishedThreads[i]);
                        // assign [NonSerialized] members from the old local copy
                        th.SetApplication(this);
                        // HACK: need to change this if the user is allowed to set the id
                        _Threads[th.Id] = th;
                        Exception ex = Manager.Owner_GetFailedThreadException(Credentials, new ThreadIdentifier(_Id, th.Id));
                        
                        if (ex == null)
                        {
                            try
                            {
                                ThreadFinish(th);
                            }
                            catch (NullReferenceException) {}
                        }
                        else
                        {
                            try
                            {
                                ThreadFailed(th, ex);
                            }
                            catch (NullReferenceException) {}
                        }
                    }
        
                    if ((_NumThreadsFinished) == Threads.Count)
                    {
                        if (ApplicationFinish != null)
                        {
                            _Running = false;
                            ApplicationFinish.BeginInvoke(null, null);
                            break;
                        }
                    }
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                // lost connection to Manager
                System.Threading.Thread.Sleep(1000);
            }
            catch (ThreadAbortException)
            {
                System.Threading.Thread.ResetAbort();
            }
        }

        //----------------------------------------------------------------------------------------------- 

        private void EnsureRunning()
        {
            if (!_Running)
            {
                throw new InvalidOperationException("The grid application is not running.");
            }
        }
        
        //----------------------------------------------------------------------------------------------- 
        
        private void EnsureNotRunning()
        {
            if (_Running)
            {
                throw new InvalidOperationException("The grid application is running.");
            }
        }
        
        //----------------------------------------------------------------------------------------------- 
        
        private void StartGetFinishedThreads()
        {
            if (!_Running)
            {
                _GetFinishedThreadsThread = new Thread(new ThreadStart(GetFinishedThreads));
                _GetFinishedThreadsThread.Start();
            }

            _Running = true;
        }
    }
}

