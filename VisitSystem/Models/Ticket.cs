namespace VisitSystem.Models
{
    public class Ticket
    {
        public string Titulo { get; set; }
        public List<string> Lineas { get; set; } = new();
        public string CodigoQR { get; set; }
    }
}
