using AutoMapper;
using Microsoft.Identity.Client;
using OTSupply.API.Models.Domain;
using OTSupply.API.Models.DTO;

namespace OTSupply.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<Kategorija,KategorijaDto>().ReverseMap();
            CreateMap<Kategorija, AddKategorijaDto>().ReverseMap();
            CreateMap<Kategorija, UpdateKategorijaDto>().ReverseMap();
            CreateMap<Oglas,AddOglasDto>().ReverseMap();
            CreateMap<Oglas, GetOglasDto>().ReverseMap();
            

        }
    }
}
