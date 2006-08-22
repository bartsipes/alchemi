using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using Alchemi.Core.Utility;
using System.Xml.Serialization;
using System.IO;
using System.Security.Permissions;
using Alchemi.Core;

namespace Alchemi.Executor.Sandbox
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityConfig
    {
        public const string ConfigFileName = "Alchemi.Executor.Security.config";

        private PermissionSet _GridThreadPermissions = null;
        private Logger logger;

        /// <summary>
		/// Creates an instance of the SecurityConfig class.
		/// </summary>
        public SecurityConfig()
        {
            logger = new Logger();
        }
        /// <summary>
        /// Creates an instance of the SecurityConfig 
        /// </summary>
        /// <param name="GetDefault"></param>
        public SecurityConfig(bool GetDefault) : this()
        {
            if (GetDefault)
            {
                _GridThreadPermissions = GetDefaultPermissionSet();
            }
        }

        public static SecurityConfig Load()
        {
            SecurityConfig scfg = new SecurityConfig();

            TextReader tr = null;
            try
            {
                tr = File.OpenText(DefaultConfigFile);
                string xml = tr.ReadToEnd();
                PermissionSet ps = new PermissionSet(null);
                ps.FromXml(SecurityElement.FromString(xml));
                scfg._GridThreadPermissions = ps;
            }
            finally
            {
                try
                {
                    if (tr != null)
                        tr.Close();
                }
                catch { }
            }
            return scfg;
        }

        public void Save()
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = File.OpenWrite(DefaultConfigFile);
                sw = new StreamWriter(fs);
                sw.Write(_GridThreadPermissions.ToXml().ToString());
            }
            finally
            {
                try
                {
                    if (sw != null)
                        sw.Close();
                    if (fs != null)
                        fs.Close();
                }
                catch { }
            }
        }

        private PermissionSet GetDefaultPermissionSet()
        {
            PermissionSet ps = new PermissionSet(null);
            
            //Add the execution, serialization permission.
            IPermission secPerm = new SecurityPermission(
                SecurityPermissionFlag.Execution | 
                SecurityPermissionFlag.SerializationFormatter);
            ps.AddPermission(secPerm);

            return ps;
        }

        public PermissionSet GetGThreadPermissions(string ReadOnlyDirectory, string SandboxedDirectory)
        {
            PermissionSet ps = new PermissionSet(_GridThreadPermissions);

            //Add file I/O permission for the grid thread app's sandboxed directory.
            IPermission filePerm = new FileIOPermission(FileIOPermissionAccess.AllAccess, SandboxedDirectory);
            ps.AddPermission(filePerm);

            //TODOLATER: if the Alchemi.Executor.dll is in the GAC, we may not need this.
            //Add a readonly file I/O permission for the ReadOnlyDir
            IPermission filePerm2 = new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery,
                ReadOnlyDirectory);
            ps.AddPermission(filePerm2);

            return ps;
        }

        private static string DefaultConfigFile
        {
            get
            {
                return Utils.GetFilePath(ConfigFileName, AlchemiRole.Executor, true);
            }
        }

    }
}
