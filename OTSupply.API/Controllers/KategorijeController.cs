using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.CustomActionFilter;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;

namespace OTSupply.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategorijeController : ControllerBase
    {
        private readonly OTSupplyDbContext dbContext;
        private readonly IMapper mapper;

        public KategorijeController(OTSupplyDbContext dbContext,IMapper mapper)
        {
                this.dbContext=dbContext;
            this.mapper = mapper;
        }



        // GET: https://localhost:portnumber/api/kategorije
        [HttpGet]
        public async  Task<IActionResult> GetAll()
        {

            //dobavljanje podataka iz baze
            var kategorije = await dbContext.Kategorije.ToListAsync(); //iz baze uzimamo domen model kategorija

            //mapiranje podataka u dto

          var kategorijeDto=  mapper.Map<List<KategorijaDto>>(kategorije);
            //var kategorijeDto = new List<KategorijaDto>();
            //foreach (var kategorija in kategorije)
            //{
            //    kategorijeDto.Add(new KategorijaDto()
            //    {
            //        Id = kategorija.Id,
            //        Name = kategorija.Name
            //    });
            //}

            return Ok(kategorijeDto);
        }


        //GET: https://localhost:portnumber/api/kategorije/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
         
            //Uzimamo kategoriju domain model iz baze
            var kategorija = await dbContext.Kategorije.FirstOrDefaultAsync(x => x.Id == id);

            if (kategorija == null)
            {
                return NotFound();
            }
            
            var kategorijaDto= mapper.Map<KategorijaDto>(kategorija);
          
            //vracamo dto nazad klientu
            return Ok(kategorijaDto);
        }


        //POST za pravljenje novog grada
        //POST: https://localhost:portnumber/api/gradovi

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles =("Admin"))]

        public async Task<IActionResult> Create([FromBody] AddKategorijaDto addKategorijaDto)
        {
           
                //mapiranje DTO u domain model

                var kategorija = mapper.Map<Kategorija>(addKategorijaDto);

                // Domen model stavljamo u bazu

                await dbContext.Kategorije.AddAsync(kategorija);
                await dbContext.SaveChangesAsync();

                //Mapiranje domen modela u dto

                var kategorijaDto = mapper.Map<KategorijaDto>(kategorija);

                return CreatedAtAction(nameof(GetById), new { id = kategorija.Id }, kategorijaDto);
         
            
        }


        //Update kategorije
        //UPDATE:  https://localhost:portnumber/api/kategorije{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateKategorijaDto updateKategorijaDto)
        {
            
                //Proveravamo dal postoji grad
                var kategorijaDomen = await dbContext.Kategorije.FirstOrDefaultAsync(x => x.Id == id);

                if (kategorijaDomen == null)
                {
                    return NotFound();
                }


                //mapiranje  Dto koji smo poslali u domen model

                kategorijaDomen.Name = updateKategorijaDto.Name;

                await dbContext.SaveChangesAsync();

                //Konvertovanje domen modela u dto

                var kategorijaDto = new KategorijaDto
                {
                    Id = kategorijaDomen.Id,
                    Name = kategorijaDomen.Name
                };
                return Ok(kategorijaDto);
           

        }




        //DELETE za brisanje grada
        //DELETE: https://localhost:portnumber/api/gradovi/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var kategorijaDomen =await dbContext.Kategorije.FirstOrDefaultAsync(x => x.Id == id);

            if (kategorijaDomen == null)
            {
                return NotFound();
            }

            //Brisanje kategorije

            dbContext.Kategorije.Remove(kategorijaDomen);
           await dbContext.SaveChangesAsync();


            //opcionalno da vratimo izbrisani grad nazad u ok response
            //mapiranje domen modela u dto
            var kategorijaDto = mapper.Map<KategorijaDto>(kategorijaDomen);
            
            return Ok(kategorijaDto);


        }


    }
}
