using VisitSystem.Models;
namespace VisitSystem.Services
{
    public interface IExportService
    {
        byte[] GenerateExcelReport(List<VisitRecord> visits);
        byte[] GeneratePdfReport(List<VisitRecord> visits);
    }
}
