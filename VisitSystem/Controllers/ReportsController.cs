using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitSystem.Data;
using VisitSystem.Services;

namespace VisitSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IExportService _exportService;

        public ReportsController(AppDbContext context, IExportService exportService)
        {
            _context = context;
            _exportService = exportService;
        }


       
        [HttpGet("download")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DownloadReport(
            [FromQuery] string format,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            
            var query = _context.VisitRecords 
                .Include(v => v.Visitor)      
                                              
                .AsQueryable();

            
            if (startDate.HasValue)
                query = query.Where(v => v.HoraEntrada >= startDate.Value);
            if (endDate.HasValue)
            {
              DateTime nextDay = endDate.Value.AddDays(1);
                query = query.Where(v => v.HoraEntrada < nextDay);
            }

            var data = await query.OrderByDescending(v => v.HoraEntrada).ToListAsync();

            if (!data.Any())
                return NotFound("No hay datos para generar el reporte en esas fechas.");

            byte[] fileBytes;
            string mimeType;
            string fileName;

            switch (format?.ToLower())
            {
                
                case "xlsx":
                    fileBytes = _exportService.GenerateExcelReport(data);
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = $"Reporte_Visitas_{DateTime.Now:yyyyMMdd}.xlsx";
                    break;

                case "pdf":
                    fileBytes = _exportService.GeneratePdfReport(data);
                    mimeType = "application/pdf";
                    fileName = $"Reporte_Visitas_{DateTime.Now:yyyyMMdd}.pdf";
                    break;

                default:
                    return BadRequest("Formato no soportado. Usa 'xlsx' o 'pdf'.");
            }

            
            return File(fileBytes, mimeType, fileName);
        }
    }
}
