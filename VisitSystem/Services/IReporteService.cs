using Microsoft.EntityFrameworkCore;
using VisitSystem.Data;

namespace VisitSystem.Services
{
    public interface IReporteService
    {
        Task EnviarReporteDiaAnteriorAsync();
    }

    public class ReporteService : IReporteService
    {
        private readonly IExportService _exportService;
        private readonly EmailSender _emailSender;
        private readonly AppDbContext _dbContext;

        public ReporteService(IExportService exportService, EmailSender emailSender, AppDbContext dbContext)
        {
            _exportService = exportService;
            _emailSender = emailSender;
            _dbContext = dbContext;
        }

        public async Task EnviarReporteDiaAnteriorAsync()
        {
            try
            {
              
                var fechaInicio = DateTime.Today.AddDays(-1); 
                var fechaFin = DateTime.Today;               

                Console.WriteLine($"[ReporteService] Buscando registros entre {fechaInicio} y {fechaFin}");

                var visitas = await _dbContext.VisitRecords
                                             .Include(v => v.Visitor)
                                             .Where(v => v.HoraEntrada >= fechaInicio && v.HoraEntrada < fechaFin)
                                             .OrderByDescending(v => v.HoraEntrada)
                                             .ToListAsync();

                Console.WriteLine($"[ReporteService] Registros encontrados: {visitas.Count}");

                if (!visitas.Any())
                {
                    Console.WriteLine("[ReporteService] No hay registros para el día anterior. Se enviará correo de prueba.");

                  
                    await _emailSender.EnviarReporteConAdjuntosAsync(
                        $"Reporte Diario {fechaInicio:yyyy-MM-dd}",
                        "No se encontraron registros para el día anterior.",
                        new Dictionary<string, byte[]>()
                    );
                    return;
                }

                
                Console.WriteLine("[ReporteService] Generando archivos Excel y PDF...");
                var excelBytes = _exportService.GenerateExcelReport(visitas);
                var pdfBytes = _exportService.GeneratePdfReport(visitas);

                
                string fechaString = fechaInicio.ToString("yyyyMMdd");
                var archivos = new Dictionary<string, byte[]>
                {
                    { $"Reporte_{fechaString}.xlsx", excelBytes },
                    { $"Reporte_{fechaString}.pdf", pdfBytes }
                };

                Console.WriteLine($"[ReporteService] Archivos generados: {string.Join(", ", archivos.Keys)}");

                
                Console.WriteLine("[ReporteService] Enviando correo...");
                await _emailSender.EnviarReporteConAdjuntosAsync(
                    $"Reporte Diario {fechaString}",
                    $"Adjunto el reporte del día {fechaInicio:yyyy-MM-dd}.",
                    archivos
                );

                Console.WriteLine("[ReporteService] Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ReporteService] Error al enviar reporte: {ex.Message}");
            }
        }
    }
}

