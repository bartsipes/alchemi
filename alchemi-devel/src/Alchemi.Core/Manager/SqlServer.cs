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
using System.Data;
using System.Data.SqlClient;

namespace Alchemi.Core.Manager
{
    // TODO: verify if returning a new connection for each request is
    // fine from a connection pooling point of view
    public class SqlServer
    {
        protected string _ConnStr = "";

        //-----------------------------------------------------------------------------------------------    

        public SqlServer(string connStr)
        {
            _ConnStr = connStr;
            this.ExecSql("VerifyConnection");
        }

        //-----------------------------------------------------------------------------------------------        
   
        protected SqlConnection NewConn()
        {
            return new SqlConnection(_ConnStr);
        }

        //-----------------------------------------------------------------------------------------------        

        // executes a sql statement against the db
        public void ExecSql (string sql)
        {
            SqlConnection cn = NewConn();
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Dispose();
        }

        public void ExecSql (string sql, params object[] args)
        {
            ExecSql(string.Format(sql, args));
        }

        //-----------------------------------------------------------------------------------------------        

        // executes a sql statement against the db and returns a scalar
        public object ExecSql_Scalar (string sql)
        {
            SqlConnection cn = NewConn();
            object result;
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();
            result = cmd.ExecuteScalar();
            cn.Dispose();
            return result;
        }

        public object ExecSql_Scalar (string sql, params object[] args)
        {
            return ExecSql_Scalar(string.Format(sql, args));
        }

        //-----------------------------------------------------------------------------------------------    

        // executes a sql statement against the db and returns a datatable
        public DataTable ExecSql_DataTable (string sql) 
        {
            SqlConnection cn = NewConn();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, cn);
            da.SelectCommand = cmd;
            cn.Open();
            da.Fill(ds);
            cn.Dispose();
            return ds.Tables[0];
        }

        public DataTable ExecSql_DataTable (string sql, params object[] args)
        {
            return ExecSql_DataTable(string.Format(sql, args));
        }

        //-----------------------------------------------------------------------------------------------    

        // executes a sql statement against the db and returns a dataset
        public DataSet ExecSql_DataSet (string sql) 
        {
            SqlConnection cn = NewConn();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, cn);
            da.SelectCommand = cmd;
            cn.Open();
            da.Fill(ds);
            cn.Dispose();
            return ds;
        }

        public DataSet ExecSql_DataSet (string sql, params object[] args)
        {
            return ExecSql_DataSet(string.Format(sql, args));
        }

    }
}
