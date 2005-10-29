#region Alchemi copyright and license notice

/*
* Alchemi [.NET Grid Computing Framework]
* http://www.alchemi.net
*
* Title			:	SqlServer.cs
* Project		:	Alchemi Core
* Created on	:	2003
* Copyright		:	Copyright © 2005 The University of Melbourne
*					This technology has been developed with the support of 
*					the Australian Research Council and the University of Melbourne
*					research grants as part of the Gridbus Project
*					within GRIDS Laboratory at the University of Melbourne, Australia.
* Author         :  Akshay Luther (akshayl@cs.mu.oz.au) and Rajkumar Buyya (raj@cs.mu.oz.au)
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
using System.Data;
using System.Data.SqlClient;

namespace Alchemi.Core.Manager
{
    // TODO: verify if returning a new connection for each request is
    // fine from a connection pooling point of view
	/// <summary>
	/// Represents the sql server database
	/// </summary>
    public class SqlServer
    {
		/// <summary>
		/// Connection string
		/// </summary>
        protected string _ConnStr = "";
		private SqlConnection conn = null;

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Creates a new instance of the database object
		/// </summary>
		/// <param name="connStr">connection string used to connect to the database</param>
        public SqlServer(string connStr)
        {
            _ConnStr = connStr;
            this.ExecSql("VerifyConnection");
        }

        //-----------------------------------------------------------------------------------------------        
   
		/// <summary>
		/// Creates a new sql connection object
		/// </summary>
		/// <returns>SqlConntection</returns>
        protected SqlConnection NewConn()
        {
			//if (conn==null)
			conn = new SqlConnection(_ConnStr);
			//if (conn.State == ConnectionState.Closed) conn.Open();
            return conn;
        }

        //-----------------------------------------------------------------------------------------------        

        /// <summary>
        /// Executes a sql statement against the db
        /// </summary>
        /// <param name="sql">the SQL to execute</param>
        public void ExecSql (string sql)
        {
            SqlConnection cn = NewConn();
			cn.Open();
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.ExecuteNonQuery();
			cn.Dispose();
        }

		/// <summary>
		/// Executes a sql statement against the db
		/// </summary>
		/// <param name="sql">the SQL to execute</param>
		/// <param name="args">arguments to pass to the sql statement</param>
        public void ExecSql (string sql, params object[] args)
        {
            ExecSql(string.Format(sql, args));
        }

        //-----------------------------------------------------------------------------------------------        

		/// <summary>
		/// Executes a sql statement against the db and returns a scalar 
		/// </summary>
		/// <param name="sql">the sql statement to execute</param>
		/// <returns>a scalar object which is the result of execution</returns>
        public object ExecSql_Scalar (string sql)
        {
            SqlConnection cn = NewConn();
            object result;
			cn.Open();
            SqlCommand cmd = new SqlCommand(sql, cn);
            result = cmd.ExecuteScalar();
			cn.Dispose();
            return result;
        }

		/// <summary>
		/// Executes a sql statement against the db and returns a scalar 
		/// </summary>
		/// <param name="sql">the sql statement to execute</param>
		/// <param name="args">arguments to pass to the sql statement</param>
		/// <returns>a scalar object which is the result of execution</returns>
		public object ExecSql_Scalar (string sql, params object[] args)
        {
            return ExecSql_Scalar(string.Format(sql, args));
        }

        //-----------------------------------------------------------------------------------------------    

		/// <summary>
		/// Executes a sql statement against the db and returns a datatable
		/// </summary>
		/// <param name="sql">the sql to execute</param>
		/// <returns>DataTable</returns>
        public DataTable ExecSql_DataTable (string sql) 
        {
            SqlConnection cn = NewConn();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
			cn.Open();
			SqlCommand cmd = new SqlCommand(sql, cn);
            da.SelectCommand = cmd;
            da.Fill(ds);
			cn.Dispose();
			return ds.Tables[0];
        }

		/// <summary>
		/// Executes a sql statement against the db and returns a datatable
		/// </summary>
		/// <param name="sql">the sql to execute</param>
		/// <param name="args">arguments to pass to the sql statement</param>
		/// <returns>DataTable</returns>
        public DataTable ExecSql_DataTable (string sql, params object[] args)
        {
            return ExecSql_DataTable(string.Format(sql, args));
        }

        //-----------------------------------------------------------------------------------------------    

        /// <summary>
        /// Executes a sql statement against the db and returns a dataset
        /// </summary>
        /// <param name="sql">the sql to execute</param>
        /// <returns>DataSet</returns>
        public DataSet ExecSql_DataSet (string sql) 
        {
            SqlConnection cn = NewConn();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
			cn.Open();
            SqlCommand cmd = new SqlCommand(sql, cn);
            da.SelectCommand = cmd;
            da.Fill(ds);
			cn.Dispose();
            return ds;
        }

		/// <summary>
		/// Executes a sql statement against the db and returns a dataset
		/// </summary>
		/// <param name="sql">the sql to execute</param>
		/// <param name="args">arguments to pass to the sql statement</param>
		/// <returns>DataSet</returns>
        public DataSet ExecSql_DataSet (string sql, params object[] args)
        {
            return ExecSql_DataSet(string.Format(sql, args));
        }

    }
}
