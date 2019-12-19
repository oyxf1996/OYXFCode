using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Config
    {
        public static readonly string MySqlConnString = ConfigHelper.GetConnectionString("MySqlConnString");
        public static readonly string SqliteConnString = ConfigHelper.GetConnectionString("SqliteConnString");
        public static readonly string SqlServerConnString = ConfigHelper.GetConnectionString("SqlServerConnString");
    }
}
