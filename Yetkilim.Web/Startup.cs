using AspNet.Security.OAuth.Instagram;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Yetkilim.Business.MapperProfile;
using Yetkilim.Business.Services;
using Yetkilim.Global.Configuration;
using Yetkilim.Global.Context;
using Yetkilim.Infrastructure.Data.Context;
using Yetkilim.Web;
using Yetkilim.Web.MapperProfile;
using Yetkilim.Web.Models;

namespace Yetkilim.Web
{
    public class Startup
    {
        private readonly ILogger _logger;

        public IConfiguration Configuration
        {
            get;
        }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.Configuration = configuration;
            this._logger = logger;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (!env.IsDevelopment())
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                this._logger.LogInformation("In Development environment", Array.Empty<object>());
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc((IRouteBuilder routes) => {
                routes.MapRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            this.InitializeMapperProfiles();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>((CookiePolicyOptions options) => {
                options.CheckConsentNeeded = (HttpContext context) => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<ConfigurationModel>(this.Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<YetkilimDbContext>((DbContextOptionsBuilder options) => options.UseSqlServer(this.Configuration.GetConnectionString("YetkilimDbContext"), YetkilimDbContext.SqlOptions), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            services.AddAuthentication((AuthenticationOptions options) => {
                options.DefaultAuthenticateScheme = "Cookies";
                options.DefaultChallengeScheme = "Cookies";
                options.DefaultSignInScheme = "Cookies";
                options.DefaultScheme = "Cookies";
            }).AddFacebook((FacebookOptions facebookOptions) => {
                facebookOptions.AppId = this.Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = this.Configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.CallbackPath = "/auth/externalcallback";
                facebookOptions.SignInScheme = "Cookies";
            }).AddInstagram((InstagramAuthenticationOptions instagramOptions) => {
                instagramOptions.ClientId = this.Configuration["Authentication:Instagram:ClientId"];
                instagramOptions.ClientSecret = this.Configuration["Authentication:Instagram:ClientSecret"];
                instagramOptions.CallbackPath = "/auth/signin-instagram";
            }).AddCookie("Cookies", (CookieAuthenticationOptions options) => {
                options.LoginPath = "/auth/signin";
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
            }).AddCookie("AdminAreaCookies", (CookieAuthenticationOptions options) => {
                options.LoginPath = "/admin/manage/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });
            services.AddInfrastructureServices();
            services.AddBusinessServices();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IGlobalContext, WebGlobalContext>();
        }

        private void InitializeMapperProfiles()
        {
            Mapper.Initialize((IMapperConfigurationExpression cfg) => {
                cfg.AddProfile<BusinessMapperProfile>();
                cfg.AddProfile<WebMapperProfile>();
            });
        }
    }
}
