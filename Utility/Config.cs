using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Config
    {
        public static readonly string MySqlConnString = GetConnectionString("MySqlConnString");
        public static readonly string SqliteConnString = GetConnectionString("SqliteConnString");
        public static readonly string SqlServerConnString = GetConnectionString("SqlServerConnString");

        public static readonly string 网站根路径 = GetAppSetting("网站根路径");

        #region 获取配置值的方法

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        } 
        #endregion
    }
}
