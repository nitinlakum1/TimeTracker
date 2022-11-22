using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using static TimeTrackerService.Enums;

namespace TimeTrackerService.Data
{
    public class Dac
    {
        #region Private Data Members
        private static string _connectionString;
        #endregion

        #region Constructor
        /// <summary>
        /// Parameterised Constructor.
        /// </summary>
        /// <param name="connectionString">DB Connection String.</param>
        public Dac(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            { throw new Exception("Invalid Connection String."); }

            _connectionString = connectionString;
        }
        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string commandText,
                                           List<SqlParameter> lstParameter,
                                           bool addReturnParameter)
        {
            return ExecuteNonQuery(commandText, lstParameter, CommandType.StoredProcedure, addReturnParameter);
        }

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string commandText,
                                           List<SqlParameter> lstParameter,
                                           CommandType commandType)
        {
            return ExecuteNonQuery(commandText, lstParameter, commandType, false);
        }

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string commandText,
                                           List<SqlParameter> lstParameter)
        {
            return ExecuteNonQuery(commandText, lstParameter, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public static bool ExecuteNonQuery(string commandText,
                                           List<SqlParameter> lstParameter,
                                           CommandType commandType,
                                           bool addReturnParameter)
        {
            bool returnValue = false;
            try
            {
                if (addReturnParameter)
                {
                    lstParameter.Add(new SqlParameter()
                    {
                        ParameterName = "@ErrorMessage",
                        DbType = DbType.String,
                        Value = null,
                        Size = 2000,
                        Direction = ParameterDirection.Output,
                    });
                    lstParameter.Add(new SqlParameter()
                    {
                        ParameterName = "@ReturnParam",
                        DbType = DbType.Int32,
                        Value = 0,
                        Direction = ParameterDirection.ReturnValue
                    });
                }

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (SqlConnection dbConnection = new SqlConnection(_connectionString))
                    {
                        try
                        {
                            //Open the connection.
                            using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
                            {
                                dbCommand.CommandType = commandType;

                                foreach (SqlParameter dbParameter in lstParameter)
                                {
                                    dbCommand.Parameters.Add(dbParameter);
                                }

                                dbConnection.Open();

                                //Execute the query. 
                                dbCommand.ExecuteNonQuery();

                                if (dbCommand.Parameters.Contains("@ReturnParam"))
                                {
                                    CheckReturnParameter(lstParameter, dbCommand.Parameters["@ReturnParam"]);
                                }
                            }
                        }
                        finally
                        {
                            //Close the connection.
                            if (dbConnection != null &&
                                dbConnection.State == ConnectionState.Open)
                            { dbConnection.Close(); }
                        }
                    }
                    //Complete the transaction.
                    transactionScope.Complete();
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        #endregion

        #region ExecuteNonQueryAsync

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public async static Task<bool> ExecuteNonQueryAsync(string commandText,
                                           List<SqlParameter> lstParameter,
                                           bool addReturnParameter)
        {
            return await ExecuteNonQueryAsync(commandText, lstParameter, CommandType.StoredProcedure, addReturnParameter);
        }

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async static Task<bool> ExecuteNonQueryAsync(string commandText,
                                           List<SqlParameter> lstParameter,
                                           CommandType commandType)
        {
            return await ExecuteNonQueryAsync(commandText, lstParameter, commandType, false);
        }

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <returns></returns>
        public async static Task<bool> ExecuteNonQueryAsync(string commandText,
                                           List<SqlParameter> lstParameter)
        {
            return await ExecuteNonQueryAsync(commandText, lstParameter, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Method to execute stored procedure.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public async static Task<bool> ExecuteNonQueryAsync(string commandText,
                                           List<SqlParameter> lstParameter,
                                           CommandType commandType,
                                           bool addReturnParameter)
        {
            bool returnValue = false;
            try
            {
                if (addReturnParameter)
                {
                    lstParameter.Add(new SqlParameter()
                    {
                        ParameterName = "@ErrorMessage",
                        DbType = DbType.String,
                        Value = null,
                        Size = 2000,
                        Direction = ParameterDirection.Output,
                    });
                    lstParameter.Add(new SqlParameter()
                    {
                        ParameterName = "@ReturnParam",
                        DbType = DbType.Int32,
                        Value = 0,
                        Direction = ParameterDirection.ReturnValue
                    });
                }

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (SqlConnection dbConnection = new SqlConnection(_connectionString))
                    {
                        try
                        {
                            //Open the connection.
                            using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
                            {
                                dbCommand.CommandType = commandType;

                                foreach (SqlParameter dbParameter in lstParameter)
                                {
                                    dbCommand.Parameters.Add(dbParameter);
                                }

                                dbConnection.Open();

                                //Execute the query. 
                                await dbCommand.ExecuteNonQueryAsync();

                                if (dbCommand.Parameters.Contains("@ReturnParam"))
                                {
                                    CheckReturnParameter(lstParameter, dbCommand.Parameters["@ReturnParam"]);
                                }
                            }
                        }
                        finally
                        {
                            //Close the connection.
                            if (dbConnection != null &&
                                dbConnection.State == ConnectionState.Open)
                            { dbConnection.Close(); }
                        }
                    }
                    //Complete the transaction.
                    transactionScope.Complete();
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        #endregion

        #region GetDataAsDataset

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet GetDataAsDataset(String commandText)
        {
            return GetDataAsDataset(commandText, "", null, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               string tableName)
        {
            return GetDataAsDataset(commandText, tableName, null, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               List<SqlParameter> lstParameter)
        {
            return GetDataAsDataset(commandText, "", lstParameter, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter)
        {
            return GetDataAsDataset(commandText, tableName, lstParameter, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               string tableName,
                                               CommandType commandType)
        {
            return GetDataAsDataset(commandText, tableName, null, commandType, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter,
                                               CommandType commandType)
        {
            return GetDataAsDataset(commandText, tableName, lstParameter, commandType, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter,
                                               bool addReturnParameter)
        {
            return GetDataAsDataset(commandText, tableName, lstParameter, CommandType.StoredProcedure, addReturnParameter);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public static DataSet GetDataAsDataset(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter,
                                               CommandType commandType,
                                               bool addReturnParameter)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection dbConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
                    {
                        try
                        {
                            dbCommand.CommandType = commandType;

                            if (lstParameter != null)
                            {
                                foreach (SqlParameter dbParameter in lstParameter)
                                {
                                    dbCommand.Parameters.Add(dbParameter);
                                }
                            }

                            dbConnection.Open();

                            SqlDataReader sqlDataReader = dbCommand.ExecuteReader();
                            if (sqlDataReader.HasRows)
                            {
                                if (!string.IsNullOrWhiteSpace(tableName))
                                {
                                    int i = 0;
                                    foreach (var item in tableName.Split(','))
                                    {
                                        dataSet.Tables.Add(item);

                                        //Load DataReader into the DataTable.
                                        dataSet.Tables[i++].Load(sqlDataReader);
                                    }
                                }
                                else
                                {
                                    dataSet.Tables.Add("Table1");

                                    //Load DataReader into the DataTable.
                                    dataSet.Tables[0].Load(sqlDataReader);
                                }
                            }
                        }
                        finally
                        {
                            //Close the connection.
                            if (dbConnection != null &&
                                dbConnection.State == ConnectionState.Open)
                            { dbConnection.Close(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataSet;
        }
        #endregion

        #region GetDataAsDatasetAsync

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText)
        {
            return await GetDataAsDatasetAsync(commandText, "", null, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               string tableName)
        {
            return await GetDataAsDatasetAsync(commandText, tableName, null, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="lstParameter"></param>
        /// <returns>DataSet</returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               List<SqlParameter> lstParameter)
        {
            return await GetDataAsDatasetAsync(commandText, "", lstParameter, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <returns>DataSet</returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter)
        {
            return await GetDataAsDatasetAsync(commandText, tableName, lstParameter, CommandType.StoredProcedure, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               string tableName,
                                               CommandType commandType)
        {
            return await GetDataAsDatasetAsync(commandText, tableName, null, commandType, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter,
                                               CommandType commandType)
        {
            return await GetDataAsDatasetAsync(commandText, tableName, lstParameter, commandType, false);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter,
                                               bool addReturnParameter)
        {
            return await GetDataAsDatasetAsync(commandText, tableName, lstParameter, CommandType.StoredProcedure, addReturnParameter);
        }

        /// <summary>
        /// Get data as dataset.
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableName"></param>
        /// <param name="lstParameter"></param>
        /// <param name="commandType"></param>
        /// <param name="addReturnParameter"></param>
        /// <returns></returns>
        public async static Task<DataSet> GetDataAsDatasetAsync(String commandText,
                                               string tableName,
                                               List<SqlParameter> lstParameter,
                                               CommandType commandType,
                                               bool addReturnParameter)
        {
            DataSet dataSet = new DataSet();
            try
            {
                using (SqlConnection dbConnection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand dbCommand = new SqlCommand(commandText, dbConnection))
                    {
                        try
                        {
                            dbCommand.CommandType = commandType;

                            if (lstParameter != null)
                            {
                                foreach (SqlParameter dbParameter in lstParameter)
                                {
                                    dbCommand.Parameters.Add(dbParameter);
                                }
                            }

                            dbConnection.Open();

                            SqlDataReader sqlDataReader = await dbCommand.ExecuteReaderAsync();
                            if (sqlDataReader.HasRows)
                            {
                                if (!string.IsNullOrWhiteSpace(tableName))
                                {
                                    int i = 0;
                                    foreach (var item in tableName.Split(','))
                                    {
                                        dataSet.Tables.Add(item);

                                        //Load DataReader into the DataTable.
                                        dataSet.Tables[i++].Load(sqlDataReader);
                                    }
                                }
                                else
                                {
                                    dataSet.Tables.Add("Table1");

                                    //Load DataReader into the DataTable.
                                    dataSet.Tables[0].Load(sqlDataReader);
                                }
                            }
                        }
                        finally
                        {
                            //Close the connection.
                            if (dbConnection != null &&
                                dbConnection.State == ConnectionState.Open)
                            { dbConnection.Close(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataSet;
        }
        #endregion

        #region IsServerConnected
        public static async Task<bool> IsServerConnected()
        {
            using (SqlConnection dbConnection = new SqlConnection(_connectionString))
            {
                try
                {
                    await dbConnection.OpenAsync();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        } 
        #endregion

        #region MakeDbParameter
        /// <summary>
        /// Method to create SqlParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">Datatype of the parameter.</param>
        /// <param name="parameterValue">Value of the parameter.</param>
        /// <returns>dbParameter</returns>
        public static SqlParameter MakeDbParameter(String parameterName,
                                                  DbType parameterType,
                                                  Object parameterValue)
        {
            SqlParameter dbParameter = new SqlParameter();
            try
            {
                //Set the ParameterName.
                dbParameter.ParameterName = parameterName;
                //Set the parameterType.
                dbParameter.DbType = parameterType;
                //Set the parameterValue.
                if (parameterValue == null) { dbParameter.Value = DBNull.Value; }
                else { dbParameter.Value = parameterValue; }
            }
            catch
            {
                throw;
            }
            return dbParameter;
        }
        #endregion

        #region MakeDbParameter
        /// <summary>
        /// Method to create SqlParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">Datatype of the parameter.</param>
        /// <param name="parameterValue">Value of the parameter.</param>
        /// <param name="parameterSize">Size of the parameter.</param>
        /// <returns>dbParameter</returns>
        public static SqlParameter MakeDbParameter(string parameterName,
                                                   DbType parameterType,
                                                   Object parameterValue,
                                                   int parameterSize)
        {
            SqlParameter dbParameter = new SqlParameter();
            try
            {
                //Set the ParameterName.
                dbParameter.ParameterName = parameterName;
                //Set the parameterType.
                dbParameter.DbType = parameterType;
                //Set the parameterValue.
                if (parameterValue == null) { dbParameter.Value = DBNull.Value; }
                else { dbParameter.Value = parameterValue; }
                //Set the parameterSize.
                dbParameter.Size = parameterSize;
            }
            catch
            {
                throw;
            }
            return dbParameter;
        }
        #endregion

        #region MakeDbParameter
        /// <summary>
        /// Method to create SqlParameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">Datatype of the parameter.</param>
        /// <param name="parameterValue">Value of the parameter.</param>
        /// <param name="parameterSize">Size of the parameter.</param>
        /// <returns>dbParameter</returns>
        public static SqlParameter MakeDbParameter(string parameterName,
                                                   DbType parameterType,
                                                   Object parameterValue,
                                                   int parameterSize,
                                                   ParameterDirection parameterDirection)
        {
            SqlParameter dbParameter = new SqlParameter();
            try
            {
                //Set the ParameterName.
                dbParameter.ParameterName = parameterName;
                //Set the parameterType.
                dbParameter.DbType = parameterType;
                //Set the parameterValue.
                if (parameterValue == null) { dbParameter.Value = DBNull.Value; }
                else { dbParameter.Value = parameterValue; }
                //Set the parameterSize.
                dbParameter.Size = parameterSize;
                dbParameter.Direction = parameterDirection;
            }
            catch
            {
                throw;
            }
            return dbParameter;
        }
        #endregion

        #region GetParameterValue
        public static string GetParameterValue(List<SqlParameter> lstParameter, string parameterName)
        {
            try
            {
                var data = lstParameter.Find(em => em.ParameterName.Equals("@" + parameterName)).Value;
                if (data != null)//data.GetType()
                {
                    return data.ToString();
                }
                return "";
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region CheckReturnParameter
        /// <summary>
        /// Method to check the return value of the StoredProcedure
        /// </summary>
        /// <param name="lstParameter"></param>
        /// <param name="dbParameter"></param>
        private static void CheckReturnParameter(List<SqlParameter> lstParameter, SqlParameter dbParameter)
        {
            if (Convert.ToInt32(dbParameter.Value) != (int)SPExceptions.Success)
            {
                SqlParameter errorMessage = lstParameter.Find(em => em.ParameterName.Equals("@ErrorMessage"));

                if (errorMessage != null
                    && errorMessage.Value != null
                    && !string.IsNullOrWhiteSpace(errorMessage.Value.ToString()))
                {
                    throw new Exception(errorMessage.Value.ToString());
                }
                else
                {
                    throw new Exception("Error executing the current process at Database.");
                }
            }
        }
        #endregion
    }
}
