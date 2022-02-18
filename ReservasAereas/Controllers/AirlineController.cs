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
    public class AirlineController : ControllerBase
    {
        // GET: AirlineController

        private readonly IAirlineRepository _airlineRespository;

        public AirlineController(IAirlineRepository airlineRespository)
        {
            _airlineRespository = airlineRespository;
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

                var airlines = await _airlineRespository.GetAll();
                var airlinesPayload = JsonConvert.SerializeObject(airlines);
                var payload = JsonConvert.DeserializeObject<List<AirlinesPayload>>(airlinesPayload);

                var data = new
                {
                    airlines
                };
                object result = Responses.ParseResponse(200, "Airline fetched", data);
                return Ok(payload);
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

                var airlines = await _airlineRespository.GetById(id);
                var airlinesPayload = JsonConvert.SerializeObject(airlines);
                var payload = JsonConvert.DeserializeObject<AirlinesPayload>(airlinesPayload);

                var data = new
                {
                    airlines
                };
                return Ok(payload);
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
        /// <param name="airlines"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Airlines airlines)
        {
            try
            {
                var id = await _airlineRespository.Save(airlines);
                var data = new
                {
                    id
                };
                object result = Responses.ParseResponse(200, "Airline save", data);
                return Ok("Airline save");
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
        /// <param name="airlines"></param>
        /// <returns></returns>
        [HttpPut("id")]
        public async Task<IActionResult> Edit(int id, Airlines airlines)
        {
            try
            {
                airlines.id = id;
                await _airlineRespository.Edit(airlines);
                object result = Responses.ParseResponse(200, "Airline edit", null);
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
                await _airlineRespository.Delete(id);
                object result = Responses.ParseResponse(200, "Airline delete", null);
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
