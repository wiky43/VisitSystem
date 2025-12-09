namespace VisitSystem.DTOs
{
    public class VisitRecordDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreVisitante { get; set; }
        public string CedulaVisitante { get; set; }
        public string EmpresaVisitante { get; set; }
        public DateTime HoraEntrada { get; set; }
        public string Visitado { get; set; }
        public string Departamento { get; set; }
        public string Motivo { get; set; }
    }
}
