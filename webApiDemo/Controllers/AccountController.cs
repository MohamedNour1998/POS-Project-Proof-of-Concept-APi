using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webApiDemo.Dto;
using webApiDemo.Models;

namespace webApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }
        //create Account New user Registration "Post"
        [HttpPost("Registration")]
        public async Task< IActionResult> Registration(RegistrationUserDto userDto)
        {
            //save
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userDto.UserName;
                user.Email = userDto.Email;
                IdentityResult result = await userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    return Ok("Account is crated");
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                //check and create Token
                ApplicationUser user =await userManager.FindByNameAsync(userDto.UserName);
                if (user!=null)
                {
                    bool found = await userManager.CheckPasswordAsync(user,userDto.Password);
                    if (found)
                    {
                        //claims token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name,user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        //get Role
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var roleitem in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role,roleitem));
                        }
                        //add securityKey
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
                        //add signingCredentials
                        SigningCredentials signing = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
                        //create Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],// url webApi
                            audience: config["JWT:ValidAudiance"],//url consumer angular
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials:signing
                            );
                        return Ok(new {
                            token=new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration=mytoken.ValidTo
                        });

                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }
    }
}
