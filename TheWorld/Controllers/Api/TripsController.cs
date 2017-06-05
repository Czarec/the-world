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
    [Route("/api/trips")]
    public class TripsController : Controller
    {
        private readonly IWorldRepository _repository;
        private readonly ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _repository.GetAllTrips().ContinueWith(r => r.Result.Select(Mapper.Map<TripViewModel>)));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to load all trips: {ex}");
                return BadRequest("Error occurred");
            }            
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] TripViewModel trip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                _repository.AddTrip(newTrip);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }                
                else
                {
                    return BadRequest("Failed to save changes");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
