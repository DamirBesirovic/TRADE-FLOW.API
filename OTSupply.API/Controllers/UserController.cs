using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;
using System.Security.Claims;

namespace OTSupply.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly OTSupplyDbContext context;
        private readonly UserManager<Korisnik> userManager;

        public UserController( OTSupplyDbContext context,UserManager<Korisnik> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Korisnik ne postoji");

            var roles = await userManager.GetRolesAsync(user);

            // Ako je prodavac, vraćamo SellerProfileDto
            if (roles.Contains("Seller"))
            {
                var seller = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userId);
                if (seller == null)
                    return NotFound("Prodavac nije pronađen.");

                var sellerDto = new SellerProfileDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Ime = user.Ime,
                    Prezime = user.Prezime,
                    DatumRegistracije = user.DatumRegistracije,
                    isFirstLogin = user.isFirstLogin,
                    PhoneNumber = seller.PhoneNumber, // uzimamo iz Prodavac jer može da se razlikuje
                    Roles = roles.ToList(),

                    ImeFirme = seller.ImeFirme,
                    Bio = seller.Bio,
                    PfpUrl = seller.PfpUrl,
                    Ocena = seller.Ocena,
                    IsVerified = seller.IsVerified
                };

                return Ok(sellerDto);
            }

            // Običan user
            var dto = new UserProfileDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Ime = user.Ime,
                Prezime = user.Prezime,
                DatumRegistracije = user.DatumRegistracije,
                isFirstLogin = user.isFirstLogin,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return Ok(dto);
        }


        // PUT: api/User/update-profile
        [HttpPut("update-profile")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Korisnik nije pronađen.");

            user.Ime = dto.Ime;
            user.Prezime = dto.Prezime;

            if (user.UserName != dto.Username)
            {
                var userNameResult = await userManager.SetUserNameAsync(user, dto.Username);
                if (!userNameResult.Succeeded)
                    return BadRequest(userNameResult.Errors);

                var emailResult = await userManager.SetEmailAsync(user, dto.Username);
                if (!emailResult.Succeeded)
                    return BadRequest(emailResult.Errors);
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }

        // PUT: api/User/update-seller-profile
        [HttpPut("update-seller-profile")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateSellerProfile([FromBody] UpdateSellerProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Korisnik nije pronađen.");

            user.Ime = dto.Ime;
            user.Prezime = dto.Prezime;

            if (user.UserName != dto.Username)
            {
                var userNameResult = await userManager.SetUserNameAsync(user, dto.Username);
                if (!userNameResult.Succeeded)
                    return BadRequest(userNameResult.Errors);

                var emailResult = await userManager.SetEmailAsync(user, dto.Username);
                if (!emailResult.Succeeded)
                    return BadRequest(emailResult.Errors);
            }

            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userId);
            if (prodavac == null)
                return NotFound("Prodavac nije pronađen.");

            prodavac.ImeFirme = dto.ImeFirme;
            prodavac.Bio = dto.Bio;
            //prodavac.Mesto = dto.Mesto;
            prodavac.PhoneNumber = dto.PhoneNumber;
            prodavac.PfpUrl = dto.PfpUrl;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("get-all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = userManager.Users.ToList();
            var response = new List<object>();

            foreach (var user in allUsers)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("Seller"))
                {
                    var seller = await context.Prodavci
                        .Include(p => p.Korisnik)
                        .FirstOrDefaultAsync(p => p.Id_Korisnik == user.Id);

                    if (seller != null)
                    {
                        var sellerDto = new GetSellerResponseDto
                        {
                            Id = user.Id,
                            Ime = user.Ime,
                            Prezime = user.Prezime,
                            Username = user.UserName,
                            Email = user.Email,
                            DatumRegistracije = user.DatumRegistracije,
                            Roles = roles.ToList(),

                            ImeFirme = seller.ImeFirme,
                            Bio = seller.Bio,
                            PhoneNumber = seller.PhoneNumber,
                            PfpUrl = seller.PfpUrl,
                            Ocena = seller.Ocena,
                            IsVerified = seller.IsVerified
                        };

                        response.Add(sellerDto);
                        continue;
                    }
                }

                // Obični korisnik
                var userDto = new GetUserResponseDto
                {
                    Id = user.Id,
                    Ime = user.Ime,
                    Prezime = user.Prezime,
                    Username = user.UserName,
                    Email = user.Email,
                    DatumRegistracije = user.DatumRegistracije,
                    Roles = roles.ToList()
                };

                response.Add(userDto);
            }

            return Ok(response);
        }


        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("Korisnik nije pronađen.");

            var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Lozinka je uspešno promenjena.");
        }


        // DELETE: api/User/delete-account
        [HttpDelete("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Korisnik nije pronađen.");

            // ako je prodavac, obriši i njegove oglase + entry iz Prodavci
            var seller = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == user.Id);
            if (seller != null)
            {
                // pronalazak svih oglasa tog prodavca
                var oglasi = await context.Oglasi
                    .Include(o => o.ImageURLs)
                    .Where(o => o.Prodavac_Id == seller.Id)
                    .ToListAsync();

                // obriši slike i oglase
                foreach (var oglas in oglasi)
                {
                    context.ImageUrls.RemoveRange(oglas.ImageURLs);
                }
                context.Oglasi.RemoveRange(oglasi);

                // obriši prodavca
                context.Prodavci.Remove(seller);
                await context.SaveChangesAsync();
            }

            // obriši samog korisnika
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Nalog i svi povezani podaci su uspešno obrisani.");
        }


    }
}
