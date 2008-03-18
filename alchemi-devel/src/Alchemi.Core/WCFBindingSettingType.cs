using System;
using System.Collections.Generic;
using System.Text;

namespace Alchemi.Core
{
    /// <summary>
    /// Determines how the details of the binding used for WCF comunication are set.
    /// </summary>
    public enum WCFBindingSettingType
    {
        None = 0,
        Default = 1, //use default settings that work
        UseConfigFile = 2 //use configuration in app.config
    }
}
