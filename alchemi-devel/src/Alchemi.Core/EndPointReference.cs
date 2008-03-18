using System;
using System.ServiceModel;

namespace Alchemi.Core
{
    /// <summary>
    /// A class that wraps the reference to the remote object.
    /// </summary>
    /// <remarks>Added so that any additional resources may be disposed.</remarks>
    public class EndPointReference : IDisposable
    {
        #region Private Members
        internal System.ServiceModel.Channels.IChannelFactory _fac = null;
        private object _Instance = null;
        #endregion

        #region Propertie - Instance
        /// <summary>
        /// The actual instance of the object.
        /// </summary>
        public object Instance
        {
            get { return _Instance; }
            set { _Instance = value; }
        }
        #endregion

        #region Method - Dispose
        public void Dispose()
        {
            if (_fac != null)
            {
                if (_fac.State == CommunicationState.Opened || _fac.State == CommunicationState.Opening)
                    _fac.Close();
                if (_fac.State == CommunicationState.Faulted)
                    _fac.Abort();
            }
        }
        #endregion

    }
}
