namespace VisitSystem.Models
{
    public class VisitRecord
    {
        public int Id { get; set; }
        public Visitor Visitor { get; set; }
        public string Visitado { get; set; }
        public string Motivo { get; set; }
        public string Departamento { get; set; }
        public DateTime HoraEntrada { get; set; } = DateTime.Now;
        public DateTime? HoraSalida { get; set; }

        public string Codigo { get; set; } // Ej: VST-1023
    }
}
