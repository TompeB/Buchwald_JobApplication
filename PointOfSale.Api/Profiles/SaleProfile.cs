using AutoMapper;
using PointOfSale.Domain.Models;
using PointOfSale.Infrastructure.Entities;
using PointOfSale.Shared.Dto;

namespace PointOfSale.Api.Profiles
{
    public class SaleProfile : Profile
    {
        public SaleProfile() 
        {
            CreateMap<SaleDto, SaleBlo>();
            CreateMap<SaleBlo, SaleDto>();

            CreateMap<Sale, SaleDto>();
            CreateMap<SaleDto, Sale>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.TimeStampCreated,
                    opt => opt.Ignore()
                );
        }
    }
}
