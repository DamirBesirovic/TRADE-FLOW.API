using Microsoft.AspNetCore.Identity;
using OTSupply.API.Models.Domain;

namespace OTSupply.API.Repositories
{
    public interface ITokenRepository
    {
        //  string CreateJwtToken(IdentityUser user,List<string> roles);
        string CreateJwtToken(Korisnik user, List<string> roles);

    }
}
