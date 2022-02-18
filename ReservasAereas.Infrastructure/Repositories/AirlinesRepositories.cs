using ReservasAereas.Core.Entities;
using ReservasAereas.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ReservasAereas.Infrastructure.Repositories
{
    public class AirlinesRepositories : IAirlineRepository
    {
        private readonly string _dsapanelvtrDatabaseConnectionString;
        private readonly IConfiguration _config;

        public AirlinesRepositories(IConfiguration config)
        {
            _config = config;
            _dsapanelvtrDatabaseConnectionString = _config.GetConnectionString("ReservasAereasConnectionString");
        }

        private List<SqlParameter> ListParameters = new List<SqlParameter>();

        /// <summary>
        /// Get all airlines
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> GetAll()
        {
            try
            {
                var Airlines = await new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airlines_GetAll", null);
                return Airlines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns></returns>
        public async Task<Airlines> GetById(int id)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@id", DbType = DbType.Int32, Value =  id}
                };

                List<Airlines> Airlines = await ReservasAereas.Utilities.Utilities.ConvertDataTable<Airlines>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airlines_GetById", ListParameters));
                return Airlines.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save airlines
        /// </summary>
        /// <returns></returns>
        public async Task<int> Save(Airlines airlines)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@name", DbType = DbType.String, Value =  airlines.name},
                        new SqlParameter(){ParameterName = "@createdAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                        new SqlParameter(){ParameterName = "@updatedAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                };

                DataTable Airlines = await new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airlines_Save", ListParameters);
                return Airlines.Rows.Count > 0 ? Convert.ToInt32(Airlines.Rows[0]["id"]) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit airlines
        /// </summary>
        /// <returns></returns>
        public async Task Edit(Airlines airlines)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@id", DbType = DbType.Int32, Value =  airlines.id},
                        new SqlParameter(){ParameterName = "@name", DbType = DbType.String, Value =  airlines.name},
                        new SqlParameter(){ParameterName = "@updatedAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                };

                await new Connection().ExecuteStoreProcedureValidate(_dsapanelvtrDatabaseConnectionString, "sp_Airlines_Edit", ListParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete by id
        /// </summary>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@id", DbType = DbType.Int32, Value =  id}
                };

                await ReservasAereas.Utilities.Utilities.ConvertDataTable<Airlines>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airlines_Delete", ListParameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
