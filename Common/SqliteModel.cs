using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SqliteModel
    {
        public string sql;

        public SQLiteParameter[] parameters;

        public SqliteModel(string sql, SQLiteParameter[] parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }

    }
}
