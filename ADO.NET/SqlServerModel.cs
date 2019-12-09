using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public class SqlServerModel
    {
        public string sql;

        public SqlParameter[] parameters;

        public SqlServerModel(string sql, SqlParameter[] parameters)
        {
            this.sql = sql;
            this.parameters = parameters;
        }

    }
}
