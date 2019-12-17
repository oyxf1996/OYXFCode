using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public class MySqlModel
    {
        public string sql;

        public MySqlParameter[] parameters;

        public MySqlModel(string sql, MySqlParameter[] parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }

    }
}
