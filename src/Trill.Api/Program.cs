using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Trill.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration.Enrich.FromLogContext()
                        .Enrich.WithProperty("Server", "local")
                        .MinimumLevel.Information()
                        .WriteTo.Console();
                    // .WriteTo.Seq("http://localhost:5341");
                });
    }
}
