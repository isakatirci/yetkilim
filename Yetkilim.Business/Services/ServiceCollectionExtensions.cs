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
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            ServiceCollectionServiceExtensions.AddScoped<IYetkilimUnitOfWork, YetkilimUnitOfWork>(services);
            ServiceCollectionServiceExtensions.AddScoped<IPlaceService, PlaceService>(services);
            ServiceCollectionServiceExtensions.AddScoped<IFeedbackService, FeedbackService>(services);
            ServiceCollectionServiceExtensions.AddScoped<ICompanyService, CompanyService>(services);
            ServiceCollectionServiceExtensions.AddScoped<IUserService, UserService>(services);
            ServiceCollectionServiceExtensions.AddScoped<IPanelUserService, PanelUserService>(services);
            ServiceCollectionServiceExtensions.AddScoped<IPromotionService, PromotionService>(services);
            ServiceCollectionServiceExtensions.AddScoped<IEmailSender, EmailSender>(services);
            ServiceCollectionServiceExtensions.AddScoped<ICompanyFeedbackService, CompanyFeedbackService>(services);
            
            return services;
        }
    
  }
}
