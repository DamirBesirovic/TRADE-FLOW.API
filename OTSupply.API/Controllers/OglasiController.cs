using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using OTSupply.API.CustomActionFilter;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;
using OTSupply.API.Models.DTO.OTSupply.API.Models.DTO;
using System.Security.Claims;

namespace OTSupply.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OglasiController : ControllerBase
    {
        private readonly OTSupplyDbContext context;
        private readonly UserManager<Korisnik> userManager;
        private readonly IMapper mapper;

        public OglasiController(OTSupplyDbContext context, UserManager<Korisnik> userManager,IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Seller,Admin")]
        [ValidateModel]
        public async Task<IActionResult> CreateOglas([FromBody] AddOglasDto addOglasDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userId);
            if (prodavac == null)
                return Unauthorized("Niste prodavac.");

            var oglas = mapper.Map<Oglas>(addOglasDto);
            oglas.Prodavac_Id = prodavac.Id;

            oglas.ImageURLs = addOglasDto.ImageUrls.Select(url => new ImageUrl
            {
                Id = Guid.NewGuid(),
                Url = url
            }).ToList();

            await context.Oglasi.AddAsync(oglas);
            await context.SaveChangesAsync();

            var response = new CreateOglasResponseDto
            {
                Id = oglas.Id,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                Materijal = oglas.Materijal,
                Cena = oglas.Cena,
                Mesto = oglas.Mesto,
                ImageUrls = oglas.ImageURLs.Select(i => i.Url).ToList(),
                Kategorija_Id = oglas.Kategorija_Id,
                Grad_Id = oglas.Grad_Id
            };

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] string? search = null,
    [FromQuery] Guid? kategorija = null,
    [FromQuery] Guid? grad = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null)
        {
            var query = context.Oglasi
                .Include(o => o.ImageURLs)
                .Include(o => o.Kategorija)
                .Include(o => o.Grad)
                .Include(o => o.Prodavac)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(o =>
                    o.Naslov.Contains(search) ||
                    o.Opis.Contains(search) ||
                    o.Materijal.Contains(search));
            }

            if (kategorija.HasValue)
            {
                query = query.Where(o => o.Kategorija_Id == kategorija.Value);
            }

            if (grad.HasValue)
            {
                query = query.Where(o => o.Grad_Id == grad.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(o => o.Cena >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(o => o.Cena <= maxPrice.Value);
            }

            var total = await query.CountAsync();

            var oglasi = await query
                .OrderByDescending(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = oglasi.Select(o => new GetOglasDto
            {
                Id = o.Id,
                Naslov = o.Naslov,
                Opis = o.Opis,
                Materijal = o.Materijal,
                Cena = o.Cena,
                Mesto = o.Mesto,
                ImageUrls = o.ImageURLs.Select(i => i.Url).ToList(),
                Kategorija = o.Kategorija?.Name,
                Grad = o.Grad?.Name,
                Prodavac = o.Prodavac?.ImeFirme,
                ProdavacId = o.Prodavac_Id,
                GradId = o.Grad_Id,
                KategorijaId = o.Kategorija_Id
            });

            return Ok(new
            {
                TotalItems = total,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)total / pageSize),
                Items = result
            });
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll([FromQuery] int page=1, [FromQuery] int pageSize=20)
        //{
        //    var query = context.Oglasi
        //        .Include(o => o.ImageURLs)
        //        .Include(o => o.Kategorija)
        //        .Include(o => o.Grad)
        //        .Include(o => o.Prodavac)
        //        .AsQueryable();

        //    var total = await query.CountAsync();

        //    var oglasi = await query
        //        .OrderByDescending(o => o.Id)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    var result = oglasi.Select(o => new GetOglasDto
        //    {
        //        Id = o.Id,
        //        Naslov = o.Naslov,
        //        Opis = o.Opis,
        //        Materijal = o.Materijal,
        //        Cena = o.Cena,
        //        Mesto = o.Mesto,
        //        ImageUrls = o.ImageURLs.Select(i => i.Url).ToList(),
        //        Kategorija = o.Kategorija?.Name,
        //        Grad = o.Grad?.Name,
        //        Prodavac = o.Prodavac?.ImeFirme,
        //        ProdavacId=o.Prodavac_Id
        //    });

        //    return Ok(new
        //    {
        //        TotalItems = total,
        //        Page = page,
        //        PageSize = pageSize,
        //        TotalPages = (int)Math.Ceiling((double)total / pageSize),
        //        Items = result
        //    });

        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var o = await context.Oglasi
                .Include(o => o.ImageURLs)
                .Include(o => o.Kategorija)
                .Include(o => o.Grad)
                .Include(o => o.Prodavac)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (o == null) return NotFound();

            var dto = new GetOglasDto
            {
                Id = o.Id,
                Naslov = o.Naslov,
                Opis = o.Opis,
                Materijal = o.Materijal,
                Cena = o.Cena,
                Mesto = o.Mesto,
                ImageUrls = o.ImageURLs.Select(i => i.Url).ToList(),
                Kategorija = o.Kategorija?.Name,
                Grad = o.Grad?.Name,
                Prodavac = o.Prodavac?.ImeFirme
            };

            return Ok(dto);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> UpdateOglas(Guid id, [FromBody] UpdateOglasDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userId);

            if (prodavac == null)
                return Unauthorized("Niste prijavljeni kao prodavac.");

            var oglas = await context.Oglasi
                .Include(o => o.ImageURLs)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oglas == null)
                return NotFound("Oglas nije pronađen.");

            if (oglas.Prodavac_Id != prodavac.Id)
                return Forbid("Nemate pravo da menjate ovaj oglas.");

            // Update podataka
            oglas.Naslov = dto.Naslov;
            oglas.Opis = dto.Opis;
            oglas.Materijal = dto.Materijal;
            oglas.Cena = dto.Cena;
            oglas.Mesto = dto.Mesto;
            oglas.Kategorija_Id = dto.Kategorija_Id;
            oglas.Grad_Id = dto.Grad_Id;

            // Resetovanje slika
            oglas.ImageURLs.Clear();
            foreach (var url in dto.ImageUrls)
            {
                oglas.ImageURLs.Add(new ImageUrl { Id = Guid.NewGuid(), Url = url });
            }

            await context.SaveChangesAsync();
            return NoContent(); // 204 OK no contentt
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> DeleteOglas(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var prodavac = await context.Prodavci.FirstOrDefaultAsync(p => p.Id_Korisnik == userId);

            if (prodavac == null)
                return Unauthorized("Niste prijavljeni kao prodavac.");

            var oglas = await context.Oglasi
                .Include(o => o.ImageURLs)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oglas == null)
                return NotFound("Oglas nije pronađen.");

            if (oglas.Prodavac_Id != prodavac.Id)
                return Forbid("Nemate pravo da obrišete ovaj oglas.");

            context.ImageUrls.RemoveRange(oglas.ImageURLs);
            context.Oglasi.Remove(oglas);
            await context.SaveChangesAsync();

            return NoContent();
        }



    }
}
