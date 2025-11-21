using VisitSystem.Models;

namespace VisitSystem.Services
{
    public interface ITicketService
    {
        Ticket GenerarTicket(VisitRecord visita);
    }
}
