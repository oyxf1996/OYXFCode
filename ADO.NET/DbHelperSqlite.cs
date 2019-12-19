using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using Utility;

namespace ADO.NET
{

    public class DbHelperSqlite
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        private static readonly string connectionString = Config.SqliteConnString;

        #region 查询

        public static DataSet GetDataSetBySql(string sql, params SQLiteParameter[] paras)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand();
                PrepareCommand(connection, command, null, sql, paras);
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        adapter.Fill(ds);
                        command.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlite", "GetDataSetBySql", ex);
                    }
                    return ds;
                }
            }
        }
        
        public static SQLiteDataReader GetDataReaderBySql(string sql, params SQLiteParameter[] paras)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(connection, cmd, null, sql, paras);
                SQLiteDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (Exception ex)
            {
                LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlite", "GetDataReaderBySql", ex);
                return null;
            }

        }
        
        public static object GetObjectBySql(string sql, params SQLiteParameter[] paras)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(connection, cmd, null, sql, paras);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlite", "GetObjectBySql", ex);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region 修改
        
        public static int ExecuteSql(string sql, params SQLiteParameter[] paras)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(connection, cmd, null, sql, paras);
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlite", "ExecuteSql", ex);
                        return 0;
                    }
                }
            }
        }

        #endregion

        #region 事务

        /// <summary>
        /// 执行多条SQL语句，返回影响的记录数（一个事务最多只能执行256条sql）
        /// </summary>
        /// <param name="sqlList">sql语句和参数化数组封装类的集合</param>
        /// <returns></returns>
        public static int ExecuteSqlListByTran(List<SqliteModel> sqlList)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteTransaction trans = connection.BeginTransaction())
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = connection;
                    cmd.Transaction = trans;
                    try
                    {
                        int count = 0;
                        foreach (SqliteModel item in sqlList)
                        {
                            cmd.CommandText = item.sql;

                            //当参数值赋值为null时，会报错，得赋值为DBNull.Value。
                            //if (item.parameters != null) cmd.Parameters.AddRange(item.parameters);
                            foreach (SQLiteParameter parameter in item.parameters)
                            {
                                if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                                    (parameter.Value == null))
                                {
                                    parameter.Value = DBNull.Value;
                                }
                                cmd.Parameters.Add(parameter);
                            }

                            int rows = cmd.ExecuteNonQuery();
                            count += rows;
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                        return count;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlite", "ExecuteSqlListByTran", ex);
                        return 0;
                    }
                }
            }
        }
        #endregion

        #region 分页

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="strFields">列，用逗号分隔，*代表全部</param>
        /// <param name="strTableName">表名【非空】</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序表达式【非空】</param>
        /// <returns></returns>
        public static DataSet GetPagingData(int pageIndex, int pageSize, string fields, string tableName, string condition, string order, out int recordsCount)
        {
            recordsCount = 0;
            try
            {
                //页索引和页大小必须大于0，表名和排序字段不为空
                if (pageIndex <= 0 || pageSize <= 0 || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(order))
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(fields))
                {
                    fields = "*";
                }

                if (string.IsNullOrWhiteSpace(condition))
                {
                    condition = "1=1";
                }

                string sql = string.Format("SELECT COUNT(1) FROM {0} WHERE {1}", tableName, condition);
                recordsCount = Convert.ToInt32(GetObjectBySql(sql, null));

                sql = string.Format(@"SELECT {0} FROM {2} WHERE {3} ORDER BY {1} LIMIT {4} OFFSET {5}"
                                            , fields
                                            , order
                                            , tableName
                                            , condition
                                            , pageSize
                                            , (pageIndex - 1) * pageSize);

                return GetDataSetBySql(sql, null);
            }
            catch (Exception ex)
            {
                LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlite", "GetPagingData", ex);
                return new DataSet();
            }
        }
        
        #endregion
        
        #region 私有方法

        private static void PrepareCommand(SQLiteConnection conn, SQLiteCommand cmd, SQLiteTransaction trans, string cmdText, SQLiteParameter[] parms, CommandType cmdType = CommandType.Text)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (parms != null)
            {
                foreach (SQLiteParameter parameter in parms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion
        
    }
}
