using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class LocationDAO
    {
        public static async Task<List<Location>> GetLocations()
        {
            var locations = new List<Location>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                    locations = await context.Locations.ToListAsync();

                }
                return locations;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public static async Task<Location> GetLocationById(int Id)
        {

            IEnumerable<Location> locations = await GetLocations();
            Location location = locations.SingleOrDefault(mb => mb.Id == Id);
            return location;
        }
    }
}
