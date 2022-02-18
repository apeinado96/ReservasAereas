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
    public class AirportsRepositories : IAirportRepository
    {
        private readonly string _dsapanelvtrDatabaseConnectionString;
        private readonly IConfiguration _config;

        public AirportsRepositories(IConfiguration config)
        {
            _config = config;
            _dsapanelvtrDatabaseConnectionString = _config.GetConnectionString("ReservasAereasConnectionString");
        }

        private List<SqlParameter> ListParameters = new List<SqlParameter>();

        /// <summary>
        /// Get all airports
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> GetAll()
        {
            try
            {
                DataTable Airports = await new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airports_GetAll", null);
                return Airports;
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
        public async Task<Airports> GetById(int id)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@id", DbType = DbType.Int32, Value =  id}
                };

                List<Airports> Airports = await ReservasAereas.Utilities.Utilities.ConvertDataTable<Airports>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airports_GetById", ListParameters));
                return Airports.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save airports
        /// </summary>
        /// <returns></returns>
        public async Task<int> Save(Airports airports)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@name", DbType = DbType.String, Value =  airports.name},
                        new SqlParameter(){ParameterName = "@createdAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                        new SqlParameter(){ParameterName = "@updatedAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                };

                DataTable Airports = await new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airports_Save", ListParameters);
                return Airports.Rows.Count > 0 ? Convert.ToInt32(Airports.Rows[0]["id"]) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit airports
        /// </summary>
        /// <returns></returns>
        public async Task Edit(Airports airports)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@id", DbType = DbType.Int32, Value =  airports.id},
                        new SqlParameter(){ParameterName = "@name", DbType = DbType.String, Value =  airports.name},
                        new SqlParameter(){ParameterName = "@updatedAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                };

                await new Connection().ExecuteStoreProcedureValidate(_dsapanelvtrDatabaseConnectionString, "sp_Airports_Edit", ListParameters);
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

                await ReservasAereas.Utilities.Utilities.ConvertDataTable<Airports>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Airports_Delete", ListParameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
