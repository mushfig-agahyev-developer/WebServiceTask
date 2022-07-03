using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebServiceTask.DAL;
using WebServiceTask.Interfaces;
using WebServiceTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var _db = services.GetRequiredService<AppDbContext>();
                    bool _hasAddress = _db.Personal.Any(r => r.Id > 0);


                    if (!_hasAddress)
                    {
                        var dbInitializer = services.GetRequiredService<ISeedDbContextInitialValues>();
                        dbInitializer.Initialize(_db).GetAwaiter().GetResult();
                    }
                }
                catch (Exception problem)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(problem, "An error occurred while seeding the database");
                }
                finally
                {
                    //...
                }

            }
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}
