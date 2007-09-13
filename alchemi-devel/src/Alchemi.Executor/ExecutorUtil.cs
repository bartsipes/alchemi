using System;
using System.Collections.Generic;
using System.Text;
using Alchemi.Core.Utility;
using System.IO;

namespace Alchemi.Executor
{
    internal sealed class ExecutorUtil
    {
        #region Private Constructor
        private ExecutorUtil()
        {
        } 
        #endregion



        #region Property - BaseDirectory
        /// <summary>
        /// Gets the base directory of the Executor.
        /// </summary>
        /// <value>The base directory.</value>
        internal static string BaseDirectory
        {
            get
            {
                return Utils.GetFilePath(string.Empty, AlchemiRole.Executor, false);
            }
        } 
        #endregion


        #region Property - DataDirectory
        /// <summary>
        /// Gets the data directory of the Executor.
        /// </summary>
        /// <value>The data directory.</value>
        internal static string DataDirectory
        {
            get
            {
                return Path.Combine(
                    ExecutorUtil.BaseDirectory,
                    "dat"
                    );
            }
        } 
        #endregion


        #region Method - GetApplicationDirectory
        /// <summary>
        /// Gets the application directory.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        internal static string GetApplicationDirectory(string applicationId)
        {
            return Path.Combine(
                ExecutorUtil.DataDirectory,
                String.Format("application_{0}", applicationId)
                );
        } 
        #endregion

    }
}
