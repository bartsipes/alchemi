#region Alchemi copyright notice
/*
  Alchemi [.NET Grid Computing Framework]
  http://www.alchemi.net
  
  Copyright (c)  Akshay Luther (2002-2004) & Rajkumar Buyya (2003-to-date), 
  GRIDS Lab, The University of Melbourne, Australia.
  
  Maintained and Updated by: Krishna Nadiminti (2005-to-date)
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

using System.Data;

namespace Alchemi.Core.Manager
{
	/// <summary>
	/// Represents a collection of the MExecutor objects held by the manager
	/// </summary>
    public class MExecutorCollection
    {
		// Create a logger for use in this class
		private static readonly Logger logger = new Logger();

		/// <summary>
		/// Gets the MExecutor object with the given executorId
		/// </summary>
        public MExecutor this[string executorId]
        {
            get
            {
                return new MExecutor(executorId);
            }
        }

		/// <summary>
		/// Registers a new executor with the manager
		/// </summary>
		/// <param name="sc">security credentials used to perform this operation.</param>
		/// <param name="cpuPower">cpu power of the executor</param>
		/// <returns></returns>
        public string RegisterNew(SecurityCredentials sc, int cpuPower)
        {
            string executorId = InternalShared.Instance.Database.ExecSql_Scalar("Executor_Insert 0, '{0}', {1}", sc.Username, cpuPower).ToString();
            logger.Debug("Registered new executor id="+executorId);
			return executorId;
        }

		/// <summary>
		/// Initialise the properties of this executor collection.
		/// This involves verfiying  the connection to all the dedicated executors in the database.
		/// </summary>
        public void Init()
        {
			logger.Debug("Init-ing executor collection from db");

			DataTable dt = InternalShared.Instance.Database.ExecSql_DataTable("select * from executor where is_dedicated = 1");

			logger.Debug("# of dedicated executors = " + dt.Rows.Count);
            foreach (DataRow dr in dt.Rows)
            {
                string executorId = dr["executor_id"].ToString();
                RemoteEndPoint ep = new RemoteEndPoint((string) dr["host"], (int) dr["port"], RemotingMechanism.TcpBinary);
                try
                {
					logger.Debug("Creating a MExecutor and connecting-dedicated to it");
                    new MExecutor(executorId).ConnectDedicated(ep);
                }
                catch
				{
					//logger.Error("ExecutorCommException while init-ing exec.collection",ece);
				}
            }

			logger.Debug("Executor collection init done");
        }

		/// <summary>
		/// Gets a DataTable containing all the available dedicated executors.
		/// </summary>
        public DataTable AvailableDedicatedExecutors
        {
            get 
            {
                return InternalShared.Instance.Database.ExecSql_DataTable("Executor_SelectAvailableDedicated");
            }
        }
    }
}