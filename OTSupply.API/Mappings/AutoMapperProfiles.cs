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
            // Map AddOglasDto to Oglas, but ignore ImageURLs since we handle it manually
            CreateMap<AddOglasDto, Oglas>()
                .ForMember(dest => dest.ImageURLs, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Prodavac_Id, opt => opt.Ignore())
                .ForMember(dest => dest.Kategorija, opt => opt.Ignore())
                .ForMember(dest => dest.Grad, opt => opt.Ignore())
                .ForMember(dest => dest.Prodavac, opt => opt.Ignore());
            CreateMap<Oglas, GetOglasDto>().ReverseMap();
            

        }
    }
}
