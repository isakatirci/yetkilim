using AutoMapper;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Yetkilim.Domain.DTO;
using Yetkilim.Web.Areas.Admin.Models;
using Yetkilim.Web.Models;

namespace Yetkilim.Web.MapperProfile
{
    public class WebMapperProfile : Profile
    {
        public WebMapperProfile()
            //: this()
        {
            this.CreateMap<CompanyFormModel, CompanyDetailDTO>().ReverseMap();
            this.CreateMap<PlaceFormModel, PlaceDTO>().ReverseMap();
            this.CreateMap<PanelUserFormModel, PanelUserDTO>().ReverseMap();
            this.CreateMap<FeedbackRequestModel, FeedbackDTO>().ForPath<int>((Expression<Func<FeedbackDTO, int>>)((FeedbackDTO f) => f.Detail.AdviseRate), (Action<IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int>>)delegate (IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int> c)
            {
                c.MapFrom<int>((Expression<Func<FeedbackRequestModel, int>>)((FeedbackRequestModel x) => x.AdviseRate));
            }).ForPath<int>((Expression<Func<FeedbackDTO, int>>)((FeedbackDTO f) => f.Detail.PriceRate), (Action<IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int>>)delegate (IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int> c)
            {
                c.MapFrom<int>((Expression<Func<FeedbackRequestModel, int>>)((FeedbackRequestModel x) => x.PriceRate));
            })
                .ForPath<int>((Expression<Func<FeedbackDTO, int>>)((FeedbackDTO f) => f.Detail.CleaningRate), (Action<IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int>>)delegate (IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int> c)
                {
                    c.MapFrom<int>((Expression<Func<FeedbackRequestModel, int>>)((FeedbackRequestModel x) => x.CleaningRate));
                })
                .ForPath<int>((Expression<Func<FeedbackDTO, int>>)((FeedbackDTO f) => f.Detail.FlavorRate), (Action<IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int>>)delegate (IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int> c)
                {
                    c.MapFrom<int>((Expression<Func<FeedbackRequestModel, int>>)((FeedbackRequestModel x) => x.FlavorRate));
                })
                .ForPath<int>((Expression<Func<FeedbackDTO, int>>)((FeedbackDTO f) => f.Detail.EmployeeRate), (Action<IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int>>)delegate (IPathConfigurationExpression<FeedbackRequestModel, FeedbackDTO, int> c)
                {
                    c.MapFrom<int>((Expression<Func<FeedbackRequestModel, int>>)((FeedbackRequestModel x) => x.EmployeeRate));
                })
                .ReverseMap();
        }
    }
}