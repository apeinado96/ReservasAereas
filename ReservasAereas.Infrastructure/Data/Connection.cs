using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

/// <summary>
/// CONNECTION DATABASE USING ADO.NET
/// </summary>
public class Connection
{
    #region "CONNECTION METHODS"

    /// AUTHOR: ANDRES PEINADO MAZZILLI
    /// DATE: 2022/02/18
    /// <summary>
    /// Get data of store procedure
    /// </summary>
    /// <param name="pConnectionString"></param>
    /// <param name="pNameSp"></param>
    /// <param name="pParameters"></param>
    /// <returns></returns>
    public async Task<DataTable> ExecuteStoreProcedure(string pConnectionString, string pNameSp, List<SqlParameter> pParameters)
    {
        DataTable TableData = new DataTable();
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(pConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = pNameSp;

                    if (pParameters != null)
                    {
                        cmd.Parameters.AddRange(pParameters.ToArray());
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        TableData.Load(reader);
                        return TableData;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return TableData;
    }

    /// AUTHOR: ANDRES PEINADO MAZZILLI
    /// DATE: 2022/02/18
    /// <summary>
    /// Get Data of Store Procedure
    /// </summary>
    /// <param name="pConnectionString"></param>
    /// <param name="pNameSp"></param>
    /// <param name="pParameters"></param>
    /// <returns></returns>
    public DataTable ExecuteStoreProcedureSync(string pConnectionString, string pNameSp, List<SqlParameter> pParameters)
    {
        DataTable TableData = new DataTable();
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(pConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = pNameSp;

                    if (pParameters != null)
                    {
                        cmd.Parameters.AddRange(pParameters.ToArray());
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        TableData.Load(reader);
                        return TableData;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return TableData;
    }

    /// AUTHOR: ANDRES PEINADO MAZZILLI
    /// DATE: 2022/02/18
    /// <summary>
    /// Get boolean of Store Procedure when update, delete or insert
    /// </summary>
    /// <param name="pConnectionString"></param>
    /// <param name="pNameSp"></param>
    /// <param name="pParameters"></param>
    /// <returns></returns>
    public async Task ExecuteStoreProcedureValidate(string pConnectionString, string pNameSp, List<SqlParameter> pParameters)
    {
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(pConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = pNameSp;

                    if (pParameters != null)
                    {
                        cmd.Parameters.AddRange(pParameters.ToArray());
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.RecordsAffected == 0)
                        {
                            throw new InvalidOperationException("Non rows were affected");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// AUTHOR: ANDRES PEINADO MAZZILLI
    /// DATE: 2022/02/18
    /// <summary>
    /// Get boolean of Store Procedure when update, delete or insert
    /// </summary>
    /// <param name="pConnectionString"></param>
    /// <param name="pNameSp"></param>
    /// <param name="pParameters"></param>
    /// <returns></returns>
    public void ExecuteStoreProcedureValidateSync(string pConnectionString, string pNameSp, List<SqlParameter> pParameters)
    {
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(pConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = pNameSp;

                    if (pParameters != null)
                    {
                        cmd.Parameters.AddRange(pParameters.ToArray());
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = sqlConnection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.RecordsAffected == 0)
                        {
                            throw new InvalidOperationException("Non rows were affected");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #endregion
}