using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace DataAccess.Repository
{
    public interface IAccountRepository
    {
         Task<List<Account>>  GetMembers();
         Task<Account>  LoginMember(String email, String password);
         Task<Account>  GetProfile(int AccountID);
         Task ChangePassword(int AccountID, string password);
         Task UpdateActive(int AccountID, bool? active);
         Task DeleteMember(int m);

         Task UpdateMember(Account m);
         Task AddMember(Account m);
         Task<List<Account>> SearchByEmail(string search,int CinemaId, int page, int pageSize);
    
    }
}
