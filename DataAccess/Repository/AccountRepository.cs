using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using DataAccess.DAO;

namespace DataAccess.Repository
{
        public class AccountRepository : IAccountRepository
    {
        public  Task<List<Account>> GetMembers() => AccountDAO.GetMembers();
        public  Task<Account> LoginMember(string email, string password) => AccountDAO.Instance.Login(email, password);
        public  Task<Account> GetProfile(int AccountID) => AccountDAO.Instance.GetProfile(AccountID);
        public  Task DeleteMember(int m) => AccountDAO.DeleteAccount(m);
        public  Task ChangePassword(int AccountID, string password) => AccountDAO.Instance.ChangePassword(AccountID, password);
        public  Task UpdateActive(int AccountID, bool? active) => AccountDAO.Instance.UpdateActive(AccountID, active);
        public  Task AddMember(Account m) => AccountDAO.AddAccount(m);
        public  Task UpdateMember(Account m) => AccountDAO.UpdateAccount(m);
        public  Task<List<Account>> SearchByEmail(string search, int page, int pageSize) => AccountDAO.Instance.SearchByEmail(search, page, pageSize);
       
    }
}
