using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HotelReservation.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            ////applying migrations at runtime
            //using (var scope = host.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<HotelContext>();
            //    db.Database.Migrate();
            //}

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
