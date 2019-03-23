// Decompiled with JetBrains decompiler
// Type: Yetkilim.Infrastructure.Data.Context.ServiceCollectionExtensions
// Assembly: Yetkilim.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 305AA2AA-C4F4-4601-8DDC-B6B97F702133
// Assembly location: C:\Users\isa\source\repos\yetkilim\Yetkilim.Web\bin\Debug\netcoreapp2.2\Yetkilim.Infrastructure.dll

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Yetkilim.Infrastructure.Data.Context
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services)
    {
      services.AddScoped<DbContext, YetkilimDbContext>();
      return services;
    }
  }
}
