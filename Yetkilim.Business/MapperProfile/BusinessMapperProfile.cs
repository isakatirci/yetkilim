// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.MapperProfile.BusinessMapperProfile
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using AutoMapper;
using System;
using System.Linq.Expressions;
using Yetkilim.Domain.DTO;
using Yetkilim.Domain.Entity;

namespace Yetkilim.Business.MapperProfile
{
  public class BusinessMapperProfile : Profile
  {
    public BusinessMapperProfile()
    {
      this.CreateMap<Place, PlaceDTO>().ReverseMap().ForMember<int>((Expression<Func<Place, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<PlaceDTO, Place, int>>) (c => c.Ignore()));
      this.CreateMap<Feedback, FeedbackDTO>().ReverseMap().ForMember<int>((Expression<Func<Feedback, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<FeedbackDTO, Feedback, int>>) (c => c.Ignore()));
      this.CreateMap<FeedbackDetail, FeedbackDetailDTO>().ReverseMap().ForMember<int>((Expression<Func<FeedbackDetail, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<FeedbackDetailDTO, FeedbackDetail, int>>) (c => c.Ignore()));
      this.CreateMap<Company, CompanyInfoDTO>().ReverseMap().ForMember<int>((Expression<Func<Company, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<CompanyInfoDTO, Company, int>>) (c => c.Ignore()));
      this.CreateMap<Company, CompanyDetailDTO>().ReverseMap().ForMember<int>((Expression<Func<Company, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<CompanyDetailDTO, Company, int>>) (c => c.Ignore()));
      this.CreateMap<User, UserDTO>().ReverseMap().ForMember<int>((Expression<Func<User, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<UserDTO, User, int>>) (c => c.Ignore()));
      this.CreateMap<PanelUser, PanelUserDTO>().ReverseMap().ForMember<int>((Expression<Func<PanelUser, int>>) (f => f.Id), (Action<IMemberConfigurationExpression<PanelUserDTO, PanelUser, int>>) (c => c.Ignore()));
      this.CreateMap<CompanyType, ParameterDTO>();
    }
  }
}
