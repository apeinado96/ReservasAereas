using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReservasAereas.Core.Entities;
using ReservasAereas.Core.Interfaces;
using ReservasAereas.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaPedalea.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        // GET: BookingController

        private readonly IBookingRepository _bookingRespository;

        public BookingController(IBookingRepository bookingRespository)
        {
            _bookingRespository = bookingRespository;
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        [HttpGet("airportOrigin/airportDestination/airlinenName/flightNumber")]
        public async Task<IActionResult> List(string airportOrigin, string airportDestination, string airlinenName, string flightNumber)
        {
            try
            {

                var bookings = await _bookingRespository.GetAll(airportOrigin, airportDestination, airlinenName, flightNumber);
                var bookingsPayload = JsonConvert.SerializeObject(bookings);
                var response = JsonConvert.DeserializeObject<List<BookingsPayload>>(bookingsPayload);

                var data = new
                {
                    bookings
                };
                object result = Responses.ParseResponse(200, "Booking fetched", data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                object result = Responses.ParseResponse(404, ex.Message, null);
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {

                var bookings = await _bookingRespository.GetById(id);
                var bookingsPayload = JsonConvert.SerializeObject(bookings);
                var response = JsonConvert.DeserializeObject<BookingsPayload>(bookingsPayload);

                var data = new
                {
                    bookings
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                object result = Responses.ParseResponse(404, ex.Message, null);
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="bookings"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Bookings bookings)
        {
            try
            {
                var id = await _bookingRespository.Save(bookings);
                var data = new
                {
                    id
                };
                object result = Responses.ParseResponse(200, "Booking save", data);
                return Ok("Booking save");
            }
            catch (Exception ex)
            {
                object result = Responses.ParseResponse(404, ex.Message, null);
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookingRespository.Delete(id);
                object result = Responses.ParseResponse(200, "Booking delete", null);
                return Ok(("OK"));
            }
            catch (Exception ex)
            {
                object result = Responses.ParseResponse(404, ex.Message, null);
                return BadRequest(result);
            }
        }
    }
}
