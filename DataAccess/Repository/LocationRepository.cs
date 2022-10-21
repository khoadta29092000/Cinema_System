using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DAO;

namespace DataAccess.Repository
{
    public class LocationRepository : ILocationRepository
    {
        public Task<List<Location>> GetLocations() => LocationDAO.GetLocations();

        public Task<Location> GetLocationById(int Id) => LocationDAO.GetLocationById(Id);
    }
}
