using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private readonly IWorldRepository _repository;
        private readonly ILogger<StopsController> _logger;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromRoute] string tripName)
        {
            try
            {
                var trip = await _repository.GetTripByName(tripName);
                return Ok(trip.Stops.Select(Mapper.Map<StopViewModel>).OrderBy(s => s.Order));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get stops: {ex}");
            }
            return BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromRoute] string tripName, [FromBody] StopViewModel stop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(stop);
                    _repository.AddStop(tripName, newStop);
                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"/api/trips{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                    }
                    else
                    {
                        return BadRequest("Failed to save changes");
                    }
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save stop: {ex}");
            }
            return BadRequest("Failed to save stop");
        }
    }
}
