using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OTSupply.API.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OTSupply.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {

        private readonly IConfiguration configuration;
        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //public string CreateJwtToken(IdentityUser user, List<string> roles)
        public string CreateJwtToken(Korisnik user, List<string> roles)
        {
            //pravimo claims iz rolova

            //var claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.Email,user.Email));
            var claims = new List<Claim>
             {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id) // ← OVO DODAJ
             };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credidentials= new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token= new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credidentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
