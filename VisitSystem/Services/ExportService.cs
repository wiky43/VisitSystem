using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System;
using VisitSystem.Models; 

namespace VisitSystem.Services
{
    public class ExportService : IExportService
    {
        
        public byte[] GenerateExcelReport(List<VisitRecord> visits)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Historial Visitas");

                
                worksheet.Cell(1, 1).Value = "Código";
                worksheet.Cell(1, 2).Value = "Visitante";
                worksheet.Cell(1, 3).Value = "Cédula";
                worksheet.Cell(1, 4).Value = "Empresa Origen";
                worksheet.Cell(1, 5).Value = "Anfitrión (Visitado)";
                worksheet.Cell(1, 6).Value = "Motivo";
                worksheet.Cell(1, 7).Value = "Departamento";
                worksheet.Cell(1, 8).Value = "Hora Entrada";
                worksheet.Cell(1, 9).Value = "Hora Salida";

                worksheet.Row(1).Style.Font.Bold = true;

                
                int row = 2;
                foreach (var v in visits)
                {
                    worksheet.Cell(row, 1).Value = v.Codigo;
                    worksheet.Cell(row, 2).Value = v.Visitor?.Nombre ?? "N/A";
                    worksheet.Cell(row, 3).Value = v.Visitor?.Cedula ?? "N/A";
                    worksheet.Cell(row, 4).Value = v.Visitor?.Empresa ?? "-";
                    worksheet.Cell(row, 5).Value = v.Visitado ?? "N/A";
                    worksheet.Cell(row, 6).Value = v.Motivo;
                    worksheet.Cell(row, 7).Value = v.Departamento;
                    worksheet.Cell(row, 8).Value = v.HoraEntrada.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cell(row, 9).Value = v.HoraSalida.HasValue ? v.HoraSalida.Value.ToString("yyyy-MM-dd HH:mm") : "Activo";
                    row++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
        public byte[] GeneratePdfReport(List<VisitRecord> visits)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);
                    page.Size(PageSizes.A4);

                    page.Header().Text("Registro Detallado de Visitas").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // Nombre
                            columns.RelativeColumn(2); // Anfitrión
                            columns.RelativeColumn(2); // Motivo
                            columns.ConstantColumn(70); // Entrada
                            columns.ConstantColumn(70); // Salida
                        });

                       
                        table.Header(header =>
                        {
                            header.Cell().Element(EstiloHeader).Text("Visitante");
                            header.Cell().Element(EstiloHeader).Text("Anfitrión");
                            header.Cell().Element(EstiloHeader).Text("Motivo");
                            header.Cell().Element(EstiloHeader).Text("Entrada");
                            header.Cell().Element(EstiloHeader).Text("Salida");
                        });

                        
                        foreach (var v in visits)
                        {
                            table.Cell().Element(EstiloCelda).Text(v.Visitor?.Nombre ?? "-");
                            table.Cell().Element(EstiloCelda).Text(v.Visitado ?? "-");
                            table.Cell().Element(EstiloCelda).Text(v.Motivo);
                            table.Cell().Element(EstiloCelda).Text(v.HoraEntrada.ToString("HH:mm"));
                            table.Cell().Element(EstiloCelda).Text(v.HoraSalida.HasValue ? v.HoraSalida.Value.ToString("HH:mm") : "Activo");
                        }
                    });

                    
                    page.Footer().AlignRight().Text(x =>
                    {
                        x.Span("Página ").FontSize(9);
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }

      
        static IContainer EstiloHeader(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold().Color(Colors.White))
                            .Background(Colors.Blue.Medium)
                            .Padding(5);
        }

        static IContainer EstiloCelda(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
        }
    }
}