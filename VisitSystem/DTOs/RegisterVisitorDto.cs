namespace VisitSystem.DTOs
{
    public class RegisterVisitorDto
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Empresa { get; set; }
        public string Visitado { get; set; }
        public string Motivo { get; set; }
        public string Departamento { get; set; }
        public DateTime HoraEntrada { get; set; }
    }
}
