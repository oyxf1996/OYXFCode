﻿using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace ADO.NET
{

    public class DbHelperSqlServer
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        private static readonly string connectionString = Config.SqlServerConnString;

        #region 查询
        
        public static DataSet GetDataSetBySql(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                PrepareCommand(connection, command, null, sql, paras);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        adapter.Fill(ds);
                        command.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "GetDataSetBySql", ex);
                    }
                    return ds;
                }
            }
        }
        
        public static SqlDataReader GetDataReaderBySql(string sql, params SqlParameter[] paras)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(connection, cmd, null, sql, paras);
                SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (Exception ex)
            {
                LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "GetDataReaderBySql", ex);
                return null;
            }

        }
        
        public static object GetObjectBySql(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "GetObjectBySql", ex);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region 修改
        
        public static int ExecuteSql(string sql, params SqlParameter[] paras)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
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
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "ExecuteSql", ex);
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
        public static int ExecuteSqlListByTran(List<SqlServerModel> sqlList)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.Transaction = trans;
                    try
                    {
                        int count = 0;
                        foreach (SqlServerModel item in sqlList)
                        {
                            cmd.CommandText = item.sql;

                            //当参数值赋值为null时，会报错，得赋值为DBNull.Value。
                            //if (item.parameters != null) cmd.Parameters.AddRange(item.parameters);
                            foreach (SqlParameter parameter in item.parameters)
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
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "ExecuteSqlListByTran", ex);
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

                sql = string.Format(@"SELECT {0} FROM (
                                            SELECT ROW_NUMBER() OVER(ORDER BY {1}) RowIndex,{0} FROM {2} WHERE {3}
                                        ) A WHERE A.RowIndex BETWEEN {4} AND {5}"
                                            , fields
                                            , order
                                            , tableName
                                            , condition
                                            , (pageIndex - 1) * pageSize + 1
                                            , pageIndex * pageSize);

                return GetDataSetBySql(sql, null);
            }
            catch (Exception ex)
            {
                LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "GetPagingData", ex);
                return new DataSet();
            }
        }

        /// <summary>
        /// 获取分页数据（通过执行存储过程 SP_GetPageRecords）
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="strFields">列，用逗号分隔，*代表全部</param>
        /// <param name="strTableName">表名【非空】</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序表达式【非空】</param>
        /// <param name="recordsCount"></param>
        /// <returns></returns>
        public static DataSet GetPagingDataByStoredProcedure(int pageIndex, int pageSize, string fields, string tableName, string condition, string order, out int recordsCount)
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

                SqlParameter[] paras = new SqlParameter[]{
                    new SqlParameter("@PageIndex",SqlDbType.Int),
                    new SqlParameter("@PageSize",SqlDbType.Int),
                    new SqlParameter("@Fields",SqlDbType.NVarChar),
                    new SqlParameter("@TableName",SqlDbType.NVarChar),
                    new SqlParameter("@Where",SqlDbType.NVarChar),
                    new SqlParameter("@Order",SqlDbType.NVarChar),
                    new SqlParameter("@RecordsCount", SqlDbType.Int)
                };
                paras[0].Value = pageIndex;
                paras[1].Value = pageSize;
                paras[2].Value = fields;
                paras[3].Value = tableName;
                paras[4].Value = condition;
                paras[5].Value = order;
                paras[6].Direction = ParameterDirection.Output;

                DataSet ds = GetDataSetByStoredProcedure("SP_GetPageRecords", paras);
                recordsCount = Convert.ToInt32(paras[6].Value);

                return ds;

            }
            catch (Exception ex)
            {
                LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "GetPagingDataByStoredProcedure", ex);
                return new DataSet();
            }
        }
        
        #endregion

        #region 存储过程
        
        public static DataSet GetDataSetByStoredProcedure(string strStoredProcName, SqlParameter[] paras)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                PrepareCommand(connection, command, null, strStoredProcName, paras, CommandType.StoredProcedure);
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        adapter.Fill(ds);
                        command.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteExceptionLog("ADO.NET", "DbHelperSqlServer", "GetDataSetByStoredProcedure", ex);
                    }
                    return ds;
                }
            }
        } 
        
        #endregion

        #region 私有方法

        private static void PrepareCommand(SqlConnection conn, SqlCommand cmd, SqlTransaction trans, string cmdText, SqlParameter[] parms, CommandType cmdType = CommandType.Text)
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
                foreach (SqlParameter parameter in parms)
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
