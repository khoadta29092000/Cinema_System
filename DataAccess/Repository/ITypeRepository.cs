using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ITypeRepository
    {
        Task<List<Types>> GetTypes();
    
        Task<Types> GetTypeById(int TypeId);
        Task UpdateActive(int TypeId, bool? active);
        Task DeleteType(int m);

        Task UpdateType(Types m);
        Task AddType(Types m);
        Task<List<Types>> SearchByTitle(string search, int page, int pageSize);
    }
}
