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
    public class ZahteviController : ControllerBase
    {
        private readonly UserManager<Korisnik> userManager;
        private readonly OTSupplyDbContext context;

        public ZahteviController(UserManager<Korisnik> userManager, OTSupplyDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateZahtev([FromBody] CreateZahtevDto dto)
        {
            var oglas = await context.Oglasi
                .Include(o => o.Prodavac)
                .ThenInclude(p => p.Korisnik)
                .FirstOrDefaultAsync(o => o.Id == dto.OglasId);

            if (oglas == null)
            {
                return NotFound("Oglas ne postoji.");
            }

            var kupacId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(kupacId))
            {
                return Unauthorized();
            }

            var zahtev = new Zahtev
            {
                Oglas_Id = dto.OglasId,
                Grad_Id = dto.GradId,
                Poruka = dto.Poruka,
                Kolicina = dto.Kolicina,
                Telefon = dto.Telefon,
                Kupac_Id = Guid.Parse(kupacId),
                VlasnikOglasa_KorisnikId = oglas.Prodavac.Id_Korisnik
            };

            await context.Zahtevi.AddAsync(zahtev);
            await context.SaveChangesAsync();

            return Ok("Zahtev uspešno kreiran.");
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetZahteviForSeller(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 7,
    [FromQuery] bool? procitano = null)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (korisnikId == null)
                return Unauthorized();

            // Pronadji Prodavca vezanog za ovog korisnika
            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == korisnikId);
            if (prodavac == null)
                return Unauthorized("Niste registrovani kao prodavac.");

            var query = context.Zahtevi
                .Where(z => context.Oglasi
                    .Any(o => o.Id == z.Oglas_Id && o.Prodavac_Id == prodavac.Id))
                .AsQueryable();

            // Filtriranje po "Procitano"
            if (procitano.HasValue)
            {
                query = query.Where(z => z.Procitano == procitano.Value);
            }

            // Ukupan broj (opcionalno: za frontend paginaciju)
            var totalCount = await query.CountAsync();

            var zahtevi = await query
                .OrderByDescending(z => z.PoslatoVreme)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                pageNumber,
                pageSize,
                items = zahtevi
            });
        }
        [HttpPut("mark-as-read/{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid id)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (korisnikId == null)
                return Unauthorized();

            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == korisnikId);
            if (prodavac == null)
                return Unauthorized("Niste registrovani kao prodavac.");

            var zahtev = await context.Zahtevi.FirstOrDefaultAsync(z => z.Id == id);

            if (zahtev == null)
                return NotFound("Zahtev nije pronađen.");

            // Da li je zahtev poslat oglasu koji pripada prodavcu tom?
            var oglas = await context.Oglasi.FirstOrDefaultAsync(o => o.Id == zahtev.Oglas_Id && o.Prodavac_Id == prodavac.Id);
            if (oglas == null)
                return Forbid("Nemate pristup ovom zahtevu.");

            // Obeleži kao pregledan (seen) zahtev
            zahtev.Procitano = true;
            await context.SaveChangesAsync();

            return Ok("Zahtev uspešno obeležen kao pročitan.");
        }



    }
}
