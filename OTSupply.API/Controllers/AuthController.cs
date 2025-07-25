using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;
using OTSupply.API.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OTSupply.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenRepository tokenRepository;
        private readonly OTSupplyDbContext context;

        public AuthController(UserManager<Korisnik> userManager, RoleManager<IdentityRole> roleManager, ITokenRepository tokenRepository,OTSupplyDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.tokenRepository = tokenRepository;
            this.context = context;
        }

        //POST:/api/Auth/Register
        [HttpPost]
        [Route("Register")] 
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {   //osnovna registraija, uvek daje role User a za role Seller mora seller registracija
            // Check if username or email is already taken
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
            {
                return BadRequest("Vec postoji korisnik sa tim korisnickim imenom.");
            }

            // Create the new user
            var newUser = new Korisnik
            {
                UserName = request.Username,
                Email = request.Username,
                Ime = request.Ime,
                Prezime = request.Prezime,
                DatumRegistracije = DateTime.UtcNow,
                isFirstLogin = true,
                isDeleted = false
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // automatski dodeljujemo role user
            var userRoleExists = await _roleManager.RoleExistsAsync("User");
            if (!userRoleExists)
            {
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                if (!createRoleResult.Succeeded)
                    return StatusCode(500, "Neuspešno kreiranje role User.");
            }

            var addRoleResult = await _userManager.AddToRoleAsync(newUser, "User");
            if (!addRoleResult.Succeeded)
            {
                return StatusCode(500, "Neuspešno dodeljivanje role User korisniku.");
            }

            return Ok("Korisnik uspešno registrovan sa rolom 'User'.");
        }

        [HttpPost]
        [Route("RegisterSeller")]
        public async Task<IActionResult> RegisterSeller([FromBody] RegisterSellerRequestDto registerSellerRequestDto)
        {
            // 1. Izvući Id trenutno ulogovanog korisnika iz tokena
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Niste ulogovani.");

            // 2. Pronađi korisnika u bazi
            var korisnik = await _userManager.FindByIdAsync(userId);
            if (korisnik == null)
                return NotFound("Korisnik nije pronađen.");

            // 3. Proveri da li korisnik već ima profil Prodavca
            var postojiProdavac = await context.Prodavci.AnyAsync(p => p.Id_Korisnik == korisnik.Id);
            if (postojiProdavac)
                return BadRequest("Korisnik već ima registrovan prodavac profil.");

            // 4. Napravi novi Prodavac objekat
            var prodavac = new Prodavac
            {
                //Id = Guid.NewGuid(),
                Id_Korisnik = korisnik.Id,
                Bio = registerSellerRequestDto.Bio,
                ImeFirme = registerSellerRequestDto.ImeFirme,
                PfpUrl = registerSellerRequestDto.PfpUrl,
                Ocena = 0,
                IsVerified = false,
                PhoneNumber = registerSellerRequestDto.PhoneNumber
            };

            // 5. Dodaj novi Prodavac u bazu
            await context.Prodavci.AddAsync(prodavac);

            // 6. Dodaj korisniku rolu "Seller" ukoliko je nema
            if (!await _userManager.IsInRoleAsync(korisnik, "Seller"))
            {
                var roleExists = await _roleManager.RoleExistsAsync("Seller");
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("Seller"));
                    if (!roleResult.Succeeded)
                        return StatusCode(500, "Neuspešno kreiranje role Seller.");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(korisnik, "Seller");
                if (!addRoleResult.Succeeded)
                    return StatusCode(500, "Neuspešno dodeljivanje role Seller korisniku.");
            }

            // 7. Sačuvaj promene u bazi
            await context.SaveChangesAsync();

            // 8. Vrati uspešan odgovor
            return Ok("Seller profil uspešno registrovan.");
        }



        //POST:/api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user=await _userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
               var checkPasswordResult= await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    //Pokupimo rolove od korisnika
                    var roles=await _userManager.GetRolesAsync(user);

                    if(roles!=null)
                    {

                        //ako je sve ispravno pravimo jwt token
                        var jwtToken=tokenRepository.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);

                    }

                   
                }
            }
            return BadRequest("Username or password incorrect");
        }
    }
}
