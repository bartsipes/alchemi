using System;
using System.Collections.Generic;
using System.Text;

namespace Alchemi.Core
{
    /// <summary>
    /// Enum containing all supported WCF bindings.
    /// </summary>
    public enum WCFBinding
    {
        None = 0,
        BasicHttpBinding = 1,
        WSHttpBinding = 2,
        WSDualHttpBinding = 3, //probably redundant
        WSFederationHttpBinding = 4,
        NetTcpBinding = 5,
        NetNamedPipeBinding = 6,
        NetMsmqBinding = 7, //probably redundant
        NetPeerTcpBinding = 8, //probably redundant
        MsmqIntegrationBinding = 9 //probably redundant
    }
}
