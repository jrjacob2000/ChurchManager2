using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChurchManagerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChurchManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]    
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private UserManager<ApplicationUser> _signInManager;
        private ApplicationSettings _appSettings;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, UserManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appsettings)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._appSettings = appsettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<Object> PostApplicationUser(RegisterUserModel userModel)
        {
            var appUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = userModel.FullName,
                UserName = userModel.UserName,
                Email = userModel.Email,
                ClientId = Guid.NewGuid()                
            };

            if (appUser.UserName == null)
                appUser.UserName = appUser.Email;

            try
            {
                var result = await _userManager.CreateAsync(appUser, userModel.PasswordHash);
                if (userModel.IsAdmin)
                    await _userManager.AddToRoleAsync(appUser, RoleEnum.Admin.ToString());
                if(userModel.IsAccountant)
                    await _userManager.AddToRoleAsync(appUser, RoleEnum.Accountant.ToString());
                if (userModel.IsEncoder)
                    await _userManager.AddToRoleAsync(appUser, RoleEnum.Encoder.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        [Route("GetUser/{userId}")]
        public async Task<Object> UpdateApplicationUser(string userId)
        {
            if (userId == null || Guid.Parse(userId) == Guid.Empty)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid id");
                return Task.FromResult(response);
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.Content = new StringContent("User not found");
                return Task.FromResult(response);
            }


            try
            {
                

                var roles = Enum.GetNames(typeof(RoleEnum)).Cast<string>();

                var isAdmin = await _userManager.IsInRoleAsync(user, RoleEnum.Admin.ToString()).ConfigureAwait(false);
                var isAccountant = await _userManager.IsInRoleAsync(user, RoleEnum.Accountant.ToString()).ConfigureAwait(false);
                var isEncoder = await _userManager.IsInRoleAsync(user, RoleEnum.Encoder.ToString()).ConfigureAwait(false);

                var result = new RegisterUserModel() { 
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsAdmin = isAdmin,
                    IsAccountant = isAccountant,
                    IsEncoder = isEncoder
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task<Object> UpdateApplicationUser(RegisterUserModel userModel)
        {
            if (userModel.Id == null || Guid.Parse(userModel.Id) == Guid.Empty)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid id");
                return Task.FromResult(response);
            }

            var user = await _userManager.FindByIdAsync(userModel.Id.ToString()).ConfigureAwait(false);
            if(user == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.Content = new StringContent("User not found");
                return Task.FromResult(response);
            }

            //user.FullName = userModel.FullName;
            //user.UserName = userModel.UserName;
            //user.Email = userModel.Email;
            
            try
            {
                var result = await _userManager.UpdateAsync(user);

                var roles = Enum.GetNames(typeof(RoleEnum)).Cast<string>();

                //remove first all roles of the user
                await _userManager.RemoveFromRolesAsync(user, roles).ConfigureAwait(false);

                if (userModel.IsAdmin)
                    await _userManager.AddToRoleAsync(user, RoleEnum.Admin.ToString());
                else
                    await _userManager.RemoveFromRoleAsync(user, RoleEnum.Admin.ToString()).ConfigureAwait(false);

                if (userModel.IsAccountant)
                    await _userManager.AddToRoleAsync(user, RoleEnum.Accountant.ToString());
                else
                    await _userManager.RemoveFromRoleAsync(user, RoleEnum.Accountant.ToString()).ConfigureAwait(false);

                if (userModel.IsEncoder)
                    await _userManager.AddToRoleAsync(user, RoleEnum.Encoder.ToString());
                else
                    await _userManager.RemoveFromRoleAsync(user, RoleEnum.Encoder.ToString()).ConfigureAwait(false);

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public async Task<Object> DeleteApplicationUser(string userId)
        {
            if (userId == null || Guid.Parse(userId) == Guid.Empty)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid id");
                return Task.FromResult(response);
            }

            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.Content = new StringContent("User not found");
                return Task.FromResult(response);
            }


            try
            {
               var result = await _userManager.DeleteAsync(user);

                
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]    
        [Route("Users")]
        public async Task<object> GetApplicationUsers()
        {
            try
            {
                var result = await _userManager.Users.Select(x =>
                new RegisterUserModel() { 
                    Id = x.Id,
                    FullName = x.FullName,
                    UserName = x.UserName,
                    Email = x.Email
                }).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.PasswordHash))
            {

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._appSettings.JwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("Id", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);


                return Ok(new { token });
                
            }
            return Unauthorized();
        }
    }


   
}