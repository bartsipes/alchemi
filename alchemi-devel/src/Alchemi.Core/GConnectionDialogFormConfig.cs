#region Alchemi Copyright and License Notice
/*
 * Alchemi [.NET Grid Computing Framework]
 * http://www.alchemi.net
 *
 * Title        :   GConnectionDialogFormConfig.cs
 * Project      :   Alchemi.Core
 * Created on   :   August 2005
 * Copyright    :   Copyright © 2006 The University of Melbourne
 *                  This technology has been developed with the support of 
 *                  the Australian Research Council and the University of Melbourne
 *                  research grants as part of the Gridbus Project
 *                  within GRIDS Laboratory at the University of Melbourne, Australia.
 * Author       :   Rajkumar Buyya (raj@csse.unimelb.edu.au)
 *                  Krishna Nadiminti (kna@csse.unimelb.edu.au)
 * License      :   GPL
 *                  This program is free software; you can redistribute it and/or 
 *                  modify it under the terms of the GNU General Public
 *                  License as published by the Free Software Foundation;
 *                  See the GNU General Public License 
 *                  (http://www.gnu.org/copyleft/gpl.html) for more details.
 *
 */
#endregion

#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	GConnectionDialogFormConfig.cs
* Project		:	Alchemi.Core
* Created on	:	August 2005
* Copyright		:	Copyright © 2006 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Krishna Nadiminti (kna@csse.unimelb.edu.au) and Rajkumar Buyya (raj@csse.unimelb.edu.au) 
* License        :  GPL
*					This program is free software; you can redistribute it and/or 
*					modify it under the terms of the GNU General Public
*					License as published by the Free Software Foundation;
*					See the GNU General Public License 
*					(http://www.gnu.org/copyleft/gpl.html) for more details.
*
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Alchemi.Core.Utility;

namespace Alchemi.Core
{
    /// <summary>
    /// Represents the login configuration information, to connect to a manager.
    /// Used for GConnection Dialog
    /// </summary>
    [Serializable]
    class GConnectionDialogFormConfig
    {
        // Create a logger for use in this class
        private static readonly Logger logger = new Logger();

        public const string Default_Config_File = "GConnectionDialogForm.dat";

        private string _Host     = "localhost";
        private int    _Port     = 9000;
        private string _Username = "user";
        private string _Password = "user";

        private string _Filename;

        /// <summary>
        /// Creates a new instance of the GConnectionDialogFormConfig class.
        /// </summary>
        /// <param name="filename">The file to read/write.</param>
        public GConnectionDialogFormConfig(string filename)
        {
            _Filename = filename;
        }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string Host
        {
            get { return _Host; }
            set { _Host = value; }
        }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        /// <summary>
        /// Reads the config from a file
        /// </summary>
        /// <param name="filename">file to read from</param>
        /// <returns>Config object read</returns>
        public static GConnectionDialogFormConfig Read(string filename)
        {
            string file = Utils.GetFilePath(filename, AlchemiRole.Owner, false);
            GConnectionDialogFormConfig c;
            //handle missing file exception / serialization exception etc... and create a default config.
            try
            {
                //open for read-only
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (fs.Length > 0)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        c = (GConnectionDialogFormConfig)bf.Deserialize(fs);
                    }
                    else
                    {
                        c = new GConnectionDialogFormConfig(file);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Error reading config from " + file + ", getting default config.", ex);
                c = new GConnectionDialogFormConfig(file);
            }
            return c;
        }


        /// <summary>
        /// Write the config to file
        /// </summary>
        public void Write()
        {
            string file = Utils.GetFilePath(Default_Config_File, AlchemiRole.Owner, true);
            try
            {
                using (Stream s = new FileStream(_Filename, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(s, this);
                    s.Close();
                }
            }
            catch
            {
                //ignore. if we have a call to "write" here again, we might end up in a 
                //infinite loop!
            }
        }
    }
}
