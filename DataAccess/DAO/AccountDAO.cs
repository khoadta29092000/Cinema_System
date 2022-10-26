using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();
        private AccountDAO() { }
        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }
        public  static async Task<List<Account>>  GetMembers()
        {
            var members = new List<Account>();

            try
            {
                using (var context = new CinemaManagementContext())
                {
                     members =  await context.Accounts.ToListAsync();
                  
                }
                return  members;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<Account>  Login(string email, string password)
        {

            IEnumerable<Account> members = await GetMembers();
            Account member =  members.SingleOrDefault(mb => mb.Email.Equals(email) && mb.Password.Equals(password));
            return member;
        }
        public static async Task AddAccount(Account m)
        {
            try
            {


                using (var context = new CinemaManagementContext())
                {
                    var p1 = await context.Accounts.FirstOrDefaultAsync(c => c.Email.Equals(m.Email));
                    var p2 = await context.Accounts.FirstOrDefaultAsync(c => c.Id.Equals(m.Id));
                    if (p1 == null)
                    {
                        if (p2 == null)
                        {
                            context.Accounts.Add(m);
                            await context.SaveChangesAsync();                         
                        }
                        else
                        {
                            throw new Exception("Id is Exits");
                        }
                    }
                    else
                    {
                        throw new Exception("Email is Exist");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task UpdateAccount(Account m)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                 
                    var p1 = await context.Accounts.FirstOrDefaultAsync(c => c.Email.Equals(m.Email));
                   
                    if (p1 == null )
                    {
                      
                            context.Entry<Account>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await context.SaveChangesAsync();
                                         
                    }
                    else
                     {
                    if (p1.Email == m.Email)
                    {
                            context.Entry<Account>(m).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await context.SaveChangesAsync();
                        }

                        throw new Exception("Email is Exits");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task DeleteAccount(int p)
        {
            try
            {
                using (var context = new CinemaManagementContext())
                {
                    var member = await context.Accounts.FirstOrDefaultAsync(c => c.Id == p);
                    if(member == null)
                    {
                        throw new Exception("Id is not Exits");
                    }
                    else
                    {
                        context.Accounts.Remove(member);
                        await context.SaveChangesAsync();
                    }
                   

                  
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<List<Account>> SearchByEmail(string search,int RoleId ,int page, int pageSize)
        {
            List<Account> searchResult = null;
            IEnumerable<Account> searchValues = await GetMembers();
            if (page == 0 || pageSize == 0)
            {
                page = 1;
                pageSize = 1000;
            }
             if(search == null)
            {
  
              
                if (RoleId != 0)
                {
                    searchValues = searchValues.Where(c => c.RoleId == RoleId).ToList();
                }
                searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                searchResult = searchValues.ToList();
            }
           
            else
            {
                using (var context = new CinemaManagementContext())
                {
                    searchValues = await (from member in context.Accounts
                                          where member.Email.ToLower().Contains(search.ToLower())
                                          select member).ToListAsync();
                    if (RoleId != 0)
                    {
                        searchValues = searchValues.Where(c => c.RoleId == RoleId).ToList();
                    }
                    searchValues = searchValues.Skip((page - 1) * pageSize).Take(pageSize);
                    searchResult = searchValues.ToList();


                }
            }
         
            return searchResult;
        }
        public async Task<Account> GetProfile(int AccountID)
        {

            IEnumerable<Account> members = await GetMembers();
            Account member = members.SingleOrDefault(mb => mb.Id == AccountID);
            return member;
        }
        public async Task ChangePassword(int AccountID, string password)
        {
            try
            {
                var user = new Account() { Id = AccountID, Password = password };
                using (var db = new CinemaManagementContext())
                {
                    db.Accounts.Attach(user);
                    db.Entry(user).Property(x => x.Password).IsModified = true;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateActive(int AccountID, bool? acticve)
        {

            try
            {

                var user = new Account() { Id = AccountID, Active = !acticve };
                using (var db = new CinemaManagementContext())
                {
                    db.Accounts.Attach(user);
                    db.Entry(user).Property(x => x.Active).IsModified = true;
                   await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
