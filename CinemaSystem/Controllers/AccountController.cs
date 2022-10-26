using Microsoft.AspNetCore.Mvc;
using BusinessObject.Models;
using DataAccess.Repository;
using DataAccess.DAO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using CinemaSystem.Models;


namespace CinemaSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository repositoryAccount;
        private readonly IConfiguration configuration;
        public AccountController(IAccountRepository _repositoryAccount, IConfiguration configuration)
        {
            repositoryAccount = _repositoryAccount;
            this.configuration = configuration;

        }
      

        [HttpGet]
        public async Task<IActionResult> GetAll(string search,int RoleId ,int page, int pageSize)
        {
      
            try
            {
                        var AccountList = await repositoryAccount.SearchByEmail(search, RoleId, page, pageSize);
                        var Count = AccountList.Count();
                        return Ok(new { StatusCode = 200, Message = "Load successful", data = AccountList, Count });
                     
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Getaccount(int id)
        {
            try
            {
                var Result = await repositoryAccount.GetProfile(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult> GetLogin(Account acc)
        {
            try
            {
          
                Account customer = await repositoryAccount.LoginMember(acc.Email, acc.Password);
                if (customer != null)
                {
                    if (customer.Active == true)
                    {
                       
                        return Ok(new { StatusCode = 200, Message = "Login succedfully" , data = GenerateToken(customer) });
                    }
                    else
                    {
                        return Ok(new { StatusCode = 409, Message = "Account Not Active"});
                    }
                }
                else
                {
                    return Ok(new { StatusCode = 409, Message = "Email or Password is valid" });
                }

                        
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        } 
        private string GenerateToken(Account acc)
        {
            var secretKey = configuration.GetSection("AppSettings").GetSection("SecretKey").Value;

            var jwtTokenHandler = new JwtSecurityTokenHandler();
             
            var secretKeyBytes =  Encoding.UTF8.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Email, acc.Email),
                    new Claim(ClaimTypes.Role, acc.RoleId.ToString()),
                    new Claim("Id", acc.Id.ToString()),
                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Account acc)
        {

            try
            {
                var newAcc = new Account
                {
                    Active = acc.Active,
                    Address = acc.Address,
                    Avatar = acc.Avatar,
                    CinemaId = acc.CinemaId,
                    Date = acc.Date,
                    Email = acc.Email,
                    FullName = acc.FullName,
                    Gender = acc.Gender,
                    RoleId = acc.RoleId,
                    IsLogged = true,
                    Password = acc.Password,
                    Phone = acc.Phone
                };
                await repositoryAccount.AddMember(newAcc);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

            [HttpPost("Register")]
      
            public async Task<IActionResult> Register(Register acc)
            {
                 
                try
                {
                if (acc.ConfirmPassword != acc.Password)
                {
                   
                    return StatusCode(409, new { StatusCode = 409, Message = "Confirm Password not correct password" });
                }
                var newAcc = new Account
                    {
                        Active = true ,
                        Address = null,
                        Avatar = "https://bloganchoi.com/wp-content/uploads/2022/02/avatar-trang-y-nghia.jpeg",
                        CinemaId = null,
                        Date = acc.Date,
                        Email = acc.Email,
                        FullName = acc.FullName,
                        Gender = acc.Gender,
                        RoleId = 2,
                        IsLogged = true,
                        Password = acc.Password,
                        Phone = acc.Phone
                    };
                    await repositoryAccount.AddMember(newAcc);
                    return Ok(new { StatusCode = 200, Message = "Register successful" });
                }
                catch (Exception ex)
                {
                    return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
                }

            }
        [HttpPost("Login_Google")]
        public async Task<ActionResult> GetLoginGoogle(Token token)
        {
            try
            {

                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token.token);
                string email = jsonToken.Claims.First(claim => claim.Type == "email").Value;
                string avatar = jsonToken.Claims.First(claim => claim.Type == "picture").Value;
                string name = jsonToken.Claims.First(claim => claim.Type == "name").Value;
                var AccountList = await repositoryAccount.GetMembers();
                var isExists = AccountList.SingleOrDefault(x => x.Email == email);
                if (isExists == null)
                {
                    var newAcc = new Account
                    {
                        Active = true,
                        Avatar = avatar,
                        Email = email,
                        FullName = name,
                        RoleId = 3,
                        IsLogged = false,
                    };
                    await repositoryAccount.AddMember(newAcc);
                    var member = AccountList.SingleOrDefault(x => x.Email == newAcc.Email);
                    return Ok(new { StatusCode = 200, Message = "Login SuccessFully", data = GenerateToken(newAcc) });
                }
                else
                {

                    return Ok(new { StatusCode = 200, Message = "Login SuccessFully", data = GenerateToken(isExists) });
                }
                




                

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> update(int id, Account acc)
        {
            if (id != acc.Id)
            {
                return BadRequest();
            }
           
            try
            {
                var Acc = new Account
                {
                    Id = acc.Id,    
                    Active = acc.Active,
                    Address = acc.Address,
                    Avatar = acc.Avatar,
                    CinemaId = acc.CinemaId,
                    Date = acc.Date,
                    Email = acc.Email,
                    FullName = acc.FullName,
                    Gender = acc.Gender,
                    RoleId = acc.RoleId,
                    IsLogged = true,
                    Password = acc.Password,
                    Phone = acc.Phone
                };
                await repositoryAccount.UpdateMember(Acc);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpPut("ChangePassword")]
        [Authorize()]
        public async Task<IActionResult> ChangPassword(int id, ChangePassword acc)
        {

            try
            {
                
                if(id == 0)
                {
                    return Ok(new { StatusCode = 400, Message = "Id is not Exits" });
                }
                else
                {
                       Account account = await repositoryAccount.GetProfile(id);
                        if(acc.OldPassword != account.Password)
                    {
                        return Ok(new { StatusCode = 400, Message = "Old Password not correct" });
                    }
                    else
                    {
                        await repositoryAccount.ChangePassword(id, acc.NewPassword);
                        return Ok(new { StatusCode = 200, Message = "ChangePassword successful" });
                    }
                                 
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
        [HttpPut("UpdateActive")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateActive(int id)
        {

            try
            {
                if (id == 0)
                {
                    return Ok(new { StatusCode = 400, Message = "Id is not Exits" });
                }
                else
                {
                    Account acc = await repositoryAccount.GetProfile(id);
                    if (acc == null)
                    {
                        return Ok(new { StatusCode = 400, Message = "Id not Exists" });

                    }
                    else
                    {
                        await repositoryAccount.UpdateActive(id, acc.Active);
                        return Ok(new { StatusCode = 200, Message = "Update Active successful" });
                    }                              
                }

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
         
               await repositoryAccount.DeleteMember(id);
              
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
