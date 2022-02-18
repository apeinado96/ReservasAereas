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
    public class AirportController : ControllerBase
    {
        // GET: AirportController

        private readonly IAirportRepository _airportRespository;

        public AirportController(IAirportRepository airportRespository)
        {
            _airportRespository = airportRespository;
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {

                var airports = await _airportRespository.GetAll();
                var airportsPayload = JsonConvert.SerializeObject(airports);
                var response = JsonConvert.DeserializeObject<List<AirportsPayload>>(airportsPayload);

                var data = new
                {
                    airports
                };
                object result = Responses.ParseResponse(200, "Airport fetched", data);
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

                var airports = await _airportRespository.GetById(id);
                var airportsPayload = JsonConvert.SerializeObject(airports);
                var response = JsonConvert.DeserializeObject<AirportsPayload>(airportsPayload);

                var data = new
                {
                    airports
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
        /// <param name="airports"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Airports airports)
        {
            try
            {
                var id = await _airportRespository.Save(airports);
                var data = new
                {
                    id
                };
                object result = Responses.ParseResponse(200, "Airport save", data);
                return Ok("Airport save");
            }
            catch (Exception ex)
            {
                object result = Responses.ParseResponse(404, ex.Message, null);
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Edit entity
        /// </summary>
        /// <param name="airports"></param>
        /// <returns></returns>
        [HttpPut("id")]
        public async Task<IActionResult> Edit(int id, Airports airports)
        {
            try
            {
                airports.id = id;
                await _airportRespository.Edit(airports);
                object result = Responses.ParseResponse(200, "Airport edit", null);
                return Ok(("OK"));
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
                await _airportRespository.Delete(id);
                object result = Responses.ParseResponse(200, "Airport delete", null);
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
