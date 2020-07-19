using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WorkerServer
{
    public class Worker : BackgroundService //Сам наш демон
    {
        private readonly ILogger<Worker> _logger; //Чтобы писать инфу

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        static void StartServer()
        {
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Working: {time}", DateTimeOffset.Now);
                await Task.Delay(3000, stoppingToken); //Будем писать лог каждый 3 секунды
                
                //Если будет сбой сервера, то будем перезапускать его

                bool serverRun = false;
                Process[] processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    if (Config.NameProcess == process.ProcessName)
                    {
                        serverRun = true;
                        break;
                    }
                }
                
                if()
            }
        }
    }
}