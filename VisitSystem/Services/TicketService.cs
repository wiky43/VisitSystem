using VisitSystem.Models;

namespace VisitSystem.Services
{
    public class TicketService : ITicketService
    {
        public Ticket GenerarTicket(VisitRecord visita)
        {
            var ticket = new Ticket
            {
                Titulo = "REGISTRO DE VISITA",
                CodigoQR = visita.Codigo
            };

            ticket.Lineas.Add($"Nombre: {visita.Visitor.Nombre}");
            ticket.Lineas.Add($"Motivo: {visita.Motivo}");
            ticket.Lineas.Add($"Departamento: {visita.Departamento}");
            ticket.Lineas.Add($"Fecha: {visita.FechaHora:dd/MM/yyyy hh:mm tt}");
            ticket.Lineas.Add($"Código: {visita.Codigo}");

            return ticket;
        }
    }
}

