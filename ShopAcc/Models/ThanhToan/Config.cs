using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Config
/// </summary>
public class Config
{
    public Config()
    {

        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetConnectString()
    {
        string host = ConfigurationManager.AppSettings["HostSQL"];
        string dbname = ConfigurationManager.AppSettings["DBName"];
        string username = ConfigurationManager.AppSettings["Username"];
        string pass = ConfigurationManager.AppSettings["Password"];

        string connectString = string.Format("data source={0};uid={1}; pwd={2}; Database={3};Max Pool Size=2000", host,username,pass,dbname);

        return connectString;
    }
}