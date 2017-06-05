using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext _context;
        private readonly ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Trip>> GetAllTrips()
        {
            _logger.LogInformation("Getting trips from the database");
            return await _context.Trips.ToListAsync();
        }

        public async Task<Trip> GetTripByName(string tripName)
        {
            return await _context.Trips.Include(t => t.Stops).FirstOrDefaultAsync(t => t.Name == tripName);
        }

        public void AddTrip(Trip trip)
        {
            _context.Trips.Add(trip);
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var trip = GetTripByName(tripName).Result;
            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }            
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
