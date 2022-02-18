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
    public class BookingRepositories : IBookingRepository
    {
        private readonly string _dsapanelvtrDatabaseConnectionString;
        private readonly IConfiguration _config;

        public BookingRepositories(IConfiguration config)
        {
            _config = config;
            _dsapanelvtrDatabaseConnectionString = _config.GetConnectionString("ReservasAereasConnectionString");
        }

        private List<SqlParameter> ListParameters = new List<SqlParameter>();

        /// <summary>
        /// Get all booking
        /// </summary>
        /// <returns></returns>
        public async Task<List<Bookings>> GetAll(string airportOrigin, string airportDestination, string airlinenName, string flightNumber)
        {
            try
            {

                ListParameters = new List<SqlParameter>(){
                            new SqlParameter(){ParameterName = "@airportOrigin", DbType = DbType.String, Value = string.IsNullOrEmpty(airportOrigin) ? null : airportOrigin},
                            new SqlParameter(){ParameterName = "@airportDestination", DbType = DbType.String, Value =  string.IsNullOrEmpty(airportDestination) ? null : airportDestination},
                            new SqlParameter(){ParameterName = "@airlinenName", DbType = DbType.String, Value =  string.IsNullOrEmpty(airlinenName) ? null : airlinenName},
                            new SqlParameter(){ParameterName = "@flightNumber", DbType = DbType.String, Value =  string.IsNullOrEmpty(flightNumber) ? null : flightNumber}
                };

                List<Bookings> Bookings = await ReservasAereas.Utilities.Utilities.ConvertDataTable<Bookings>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Bookings_GetAll", ListParameters));
                return Bookings;
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
        public async Task<Bookings> GetById(int id)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@id", DbType = DbType.Int32, Value =  id}
                };

                List<Bookings> Bookings = await ReservasAereas.Utilities.Utilities.ConvertDataTable<Bookings>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Bookings_GetById", ListParameters));
                return Bookings.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save booking
        /// </summary>
        /// <returns></returns>
        public async Task<int> Save(Bookings booking)
        {
            try
            {
                ListParameters = new List<SqlParameter>(){
                        new SqlParameter(){ParameterName = "@airportOrigintId", DbType = DbType.Int32, Value =  booking.airportOrigintId},
                        new SqlParameter(){ParameterName = "@airportDestinationtId", DbType = DbType.Int32, Value =  booking.airportDestinationtId},
                        new SqlParameter(){ParameterName = "@entryTime", DbType = DbType.DateTime, Value =  booking.entryTime},
                        new SqlParameter(){ParameterName = "@departureTime", DbType = DbType.DateTime, Value =  booking.departureTime},
                        new SqlParameter(){ParameterName = "@airlineId", DbType = DbType.Int32, Value =  booking.airlineId},
                        new SqlParameter(){ParameterName = "@flightNumber", DbType = DbType.String, Value =  booking.flightNumber},
                        new SqlParameter(){ParameterName = "@priceTypePassenger", SqlDbType = SqlDbType.Float, Value =  booking.priceTypePassenger},
                        new SqlParameter(){ParameterName = "@createdAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                        new SqlParameter(){ParameterName = "@updatedAt", DbType = DbType.DateTime, Value =  DateTime.Now},
                };

                DataTable Bookings = await new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Bookings_Save", ListParameters);
                return Bookings.Rows.Count > 0 ? Convert.ToInt32(Bookings.Rows[0]["id"]) : 0;
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

                await ReservasAereas.Utilities.Utilities.ConvertDataTable<Bookings>
                    (new Connection().ExecuteStoreProcedure(_dsapanelvtrDatabaseConnectionString, "sp_Bookings_Delete", ListParameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
