using VisitSystem.Models;

namespace VisitSystem.Services
{
    public interface IPrintManager
    {
        Task EnviarAImprimirAsync(Ticket ticket);
    }
}
