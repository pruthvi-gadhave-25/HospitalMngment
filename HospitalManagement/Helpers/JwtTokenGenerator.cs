using HospitalManagement.Data;
using HospitalManagement.DTO;
using HospitalManagement.Helpers.Interface;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagement.Helpers
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    { 

        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {           
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
               {
                  new  Claim(ClaimTypes.NameIdentifier , user.RoleId.ToString()),
                   new Claim(ClaimTypes.Name ,user.UserName),
                   new Claim(ClaimTypes.Email ,user.Email),
                   new Claim(ClaimTypes.Role ,user.Role?.RoleName ?? "User"),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       

    }
}

