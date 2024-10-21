using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Uni_Manager.Entity;
using System.Linq;


namespace Uni_Manager.Service
{
    public class DbManager
    {
        private  string _connectionString;
       

        public DbManager()
        {
            _connectionString = ConfigurationManager.AppSettings["DbConnectionString"];
        }

        public static string ConnectionString { get; internal set; }= ConfigurationManager.AppSettings["DbConnectionString"];

        public bool IsDbOnLine
        {
            get
            {
                try
                {
                    using ( var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
     

     

     

    }
}
