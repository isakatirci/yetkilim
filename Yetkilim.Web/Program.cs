using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Yetkilim.Web
{
    public class Program
    {
        public static IConfiguration Configuration
        {
            get;
        }

        static Program()
        {
            Program.Configuration = (new ConfigurationBuilder()).SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true).AddJsonFile(string.Concat("appsettings.", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production", ".json"), true).Build();
        }

        public Program()
        {
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseConfiguration(Program.Configuration).ConfigureLogging((ILoggingBuilder logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });
        }

        public static void Main(string[] args)
        {
            Program.CreateWebHostBuilder(args).Build().Run();
        }
    }
}
