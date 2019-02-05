using AutoMapper;
using Yetkilim.Domain.DTO;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Models;

namespace Yetkilim.Web.MapperProfile
{
    public class WebMapperProfile : Profile
    {
        public WebMapperProfile()
        {

            CreateMap<CompanyFormModel, CompanyDetailDTO>()
                .ReverseMap();

            CreateMap<PlaceFormModel, PlaceDTO>()
                .ReverseMap();

            CreateMap<FeedbackRequestModel, FeedbackDTO>()
                .ForPath(f=> f.Detail.AdviseRate, c=> c.MapFrom(x=> x.AdviseRate))
                .ForPath(f => f.Detail.PriceRate, c => c.MapFrom(x => x.PriceRate))
                .ForPath(f => f.Detail.CleaningRate, c => c.MapFrom(x => x.CleaningRate))
                .ForPath(f => f.Detail.FlavorRate, c => c.MapFrom(x => x.FlavorRate))
                .ForPath(f => f.Detail.EmployeeRate, c => c.MapFrom(x => x.EmployeeRate))
                .ReverseMap();

        }
    }
}