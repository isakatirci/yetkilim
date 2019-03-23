// Decompiled with JetBrains decompiler
// Type: Yetkilim.Business.Services.ServiceCollectionExtensions
// Assembly: Yetkilim.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 14D1D813-227D-40DF-9953-925BA95C932F
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Business.dll

using Microsoft.Extensions.DependencyInjection;
using Yetkilim.Infrastructure.Data.UnitOfWork;
using Yetkilim.Infrastructure.Email;

namespace Yetkilim.Business.Services
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBusinessServices(
      this IServiceCollection services)
    {
      services.AddScoped<IYetkilimUnitOfWork, YetkilimUnitOfWork>();
      services.AddScoped<IPlaceService, PlaceService>();
      services.AddScoped<IFeedbackService, FeedbackService>();
      services.AddScoped<ICompanyService, CompanyService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IPanelUserService, PanelUserService>();
      services.AddScoped<IEmailSender, EmailSender>();
      return services;
    }
  }
}
