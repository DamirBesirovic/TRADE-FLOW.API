using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTSupply.API.CustomActionFilter;
using OTSupply.API.Data;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;

namespace OTSupply.API.Controllers
{ //https://localhost:portnumber/api/gradovi
    [Route("api/[controller]")]
    [ApiController]
    public class GradoviController : ControllerBase
    {
        private readonly OTSupplyDbContext dbContext;
        public GradoviController(OTSupplyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET: https://localhost:portnumber/api/gradovi?filterOn=Name&filterQuery=Track
        [HttpGet]
        public  async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery) 
        {

            //dobavljanje podataka iz baze
            var gradovi = dbContext.Gradovi.AsQueryable();
            //var gradovi = await dbContext.Gradovi.ToListAsync();

            //filtriranje

            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name" ,StringComparison.OrdinalIgnoreCase))
                {
                    gradovi = gradovi.Where(x=> x.Name.Contains(filterQuery));
                }
            }

            //mapiranje podataka u dto

            var gradoviDto = new List<GradDto>();
            foreach (var grad in gradovi)
            {
                gradoviDto.Add(new GradDto()
                {
                    Id = grad.Id,
                    Name = grad.Name
                });
            }

            return Ok(gradoviDto);
        }

        //GET: https://localhost:portnumber/api/gradovi/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var grad = dbContext.Gradovi.Find(id);
            //Uzimamo grad domain model iz baze
            var grad = await dbContext.Gradovi.FirstOrDefaultAsync(x => x.Id == id);

            if (grad == null)
            {
                return NotFound();
            }

            var gradDto = new GradDto
            {
                Id = grad.Id,
                Name = grad.Name,
            };

            //vracamo dto nazad klientu
            return Ok(gradDto);
        }

        //POST za pravljenje novog grada
        //POST: https://localhost:portnumber/api/gradovi

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles ="Admin")]

        

        public async Task<IActionResult> Create([FromBody] AddGradDto addGradDto)
        {
            //mapiranje DTO u domain model

            var grad = new Grad
            {
                Name = addGradDto.Name
            };

            // Domen model stavljamo u bazu

            await dbContext.Gradovi.AddAsync(grad);
            await dbContext.SaveChangesAsync();

            //Mapiranje domen modela u dto

            var gradDto = new GradDto
            {
                Id = grad.Id,
                Name = grad.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = grad.Id }, gradDto);
        }


        //Update grada
        //UPDATE:  https://localhost:portnumber/api/gradovi/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGradDto updateGradDto)
        {
            //Proveravamo dal postoji grad
            var gradDomen = await dbContext.Gradovi.FirstOrDefaultAsync(x => x.Id == id);

            if(gradDomen == null)
            {
                return NotFound();
            }


            //mapiranje  Dto koji smo poslali u domen model
            
            gradDomen.Name = updateGradDto.Name;

           await dbContext.SaveChangesAsync();

            //Konvertovanje domen modela u dto

            var gradDto = new GradDto
            {
                Id=gradDomen.Id,
                Name=gradDomen.Name
            };
            return Ok(gradDto);

        }




        //DELETE za brisanje grada
        //DELETE: https://localhost:portnumber/api/gradovi/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var gradDomen= await dbContext.Gradovi.FirstOrDefaultAsync(x => x.Id == id);

            if (gradDomen == null)
            {
                return NotFound();
            }

            //Brisanje grada

            dbContext.Gradovi.Remove(gradDomen);
           await dbContext.SaveChangesAsync();


            //opcionalno da vratimo izbrisani grad nazad u ok response
            //mapiranje domen modela u dto
            var gradDto = new GradDto
            {
                Id = gradDomen.Id,
                Name = gradDomen.Name
            };
            return Ok(gradDto);


        }





        [HttpGet("debug-auth")]
        public IActionResult DebugAuth()
        {
            return Ok(new
            {
                User.Identity?.IsAuthenticated,
                User.Identity?.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }),
                Headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
            });
        }

      

    }
}
