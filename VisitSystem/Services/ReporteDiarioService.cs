using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VisitSystem.Data;
using VisitSystem.Models;
using VisitSystem.Services;

namespace VisitSystem.Services
{
    public class ReporteDiarioService : BackgroundService
    {
        private readonly IServiceProvider _provider;

        public ReporteDiarioService(IServiceProvider provider)
        {
            _provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var ahora = DateTime.Now;
                if (ahora.Hour == 8 && ahora.Minute == 0)
                {
                    using var scope = _provider.CreateScope();
                    var reporteService = scope.ServiceProvider.GetRequiredService<IReporteService>();
                    await reporteService.EnviarReporteDiaAnteriorAsync();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }

}