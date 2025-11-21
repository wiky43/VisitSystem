using VisitSystem.Models;

namespace VisitSystem.Services
{
    public class PrintManager : IPrintManager
    {
        public async Task EnviarAImprimirAsync(Ticket ticket)
        {
            try
            {
                // Simulación de impresión en consola
                Console.WriteLine("---- Ticket ----");
                Console.WriteLine(ticket.Titulo);
                foreach (var linea in ticket.Lineas)
                {
                    Console.WriteLine(linea);
                }
                Console.WriteLine("----------------");

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al imprimir ticket: {ex.Message}");
                
            }
        }
    }
}
