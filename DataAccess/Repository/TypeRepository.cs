using BusinessObject.Models;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class TypeRepository : ITypeRepository
    {
        public Task<List<Types>> GetTypes() => TypeDAO.GetTypes();
 
        public Task<Types> GetTypeById(int TypeId) => TypeDAO.Instance.GetTypeById(TypeId);
        public Task DeleteType(int m) => TypeDAO.DeleteType(m);
    
        public Task UpdateActive(int TypeId, bool? active) => TypeDAO.Instance.UpdateActive(TypeId, active);
        public Task AddType(Types m) => TypeDAO.AddType(m);
        public Task UpdateType(Types m) => TypeDAO.UpdateType(m);
        public Task<List<Types>> SearchByTitle(string search, int page, int pageSize) => TypeDAO.Instance.SearchByTitle(search, page, pageSize);
    }
}
