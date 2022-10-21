using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetLocations();
        Task<Location> GetLocationById(int Id);
    }
}
