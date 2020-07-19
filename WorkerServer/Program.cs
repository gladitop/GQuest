using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerServer
{
    public class Program
    {
        public static void Main(string[] args) //Запуск приложения
        {
            CreateHostBuilder(args).Build().Run(); //Билд демона
        }

        public static IHostBuilder CreateHostBuilder(string[] args) //Сам билд демона
        {
            return Host.CreateDefaultBuilder(args)
                .UseSystemd() //Система инцилизации systemd!
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>(); //Вот и добавление демона
                });
        }
    }
}