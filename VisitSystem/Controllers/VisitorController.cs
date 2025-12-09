using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VisitSystem.Data;
using VisitSystem.DTOs;
using VisitSystem.Models;
using VisitSystem.Services; 

namespace VisitSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VisitantesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly IPrintManager _printManager;

        
        public VisitantesController(AppDbContext context, ITicketService ticketService, IPrintManager printManager)
        {
            _context = context;
            _ticketService = ticketService;
            _printManager = printManager;
        }
        [Authorize(Roles = "Admin, Recepcionista")]
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegisterVisitorDto dto)
        {
            if (dto == null)
                return BadRequest("Datos del visitante vacíos.");

            try
            {
               
                var visitor = new Visitor
                {
                    Nombre = dto.Nombre,
                    Cedula = dto.Cedula,
                    Empresa = dto.Empresa
                };
                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();

                
                var visita = new VisitRecord
                {
                    Visitor = visitor,
                    Visitado = dto.Visitado,
                    Motivo = dto.Motivo,
                    Departamento = dto.Departamento,
                    HoraEntrada = DateTime.Now, 
                    HoraSalida = null, 
                    Codigo = $"VST-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
                };
                _context.VisitRecords.Add(visita);
                await _context.SaveChangesAsync();

                
                var ticket = _ticketService.GenerarTicket(visita);
                await _printManager.EnviarAImprimirAsync(ticket);

                return Ok(new
                {
                    message = "Visitante registrado y ticket enviado",
                    codigo = visita.Codigo,
                    idVisitante = visita.Id
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { error = "Error al guardar en la base de datos", detalles = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error inesperado", detalles = ex.Message });
            }
        }

        
        [HttpGet("pendientes")]
        [Authorize(Roles = "Admin, Recepcionista")]
        public async Task<ActionResult<IEnumerable<VisitRecordDto>>> GetPendientes()
        {
            
            var pendingVisits = await _context.VisitRecords
                .Include(v => v.Visitor)
                .Where(v => v.HoraSalida == null)
                .Select(v => new VisitRecordDto
                {
                    Id = v.Id,
                    Codigo = v.Codigo,
                    NombreVisitante = v.Visitor.Nombre,
                    CedulaVisitante = v.Visitor.Cedula,
                    EmpresaVisitante = v.Visitor.Empresa,
                    HoraEntrada = v.HoraEntrada,
                    Visitado = v.Visitado,
                    Departamento = v.Departamento,
                    Motivo = v.Motivo
                })
                .ToListAsync();

            return Ok(pendingVisits);
        }

        
        [HttpPut("registrarSalida/{id}")]
        [Authorize(Roles = "Admin, Recepcionista")]
        public async Task<ActionResult> RegistrarSalida(int id)
        {
            var visitRecord = await _context.VisitRecords
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visitRecord == null)
            {
                return NotFound(new { message = $"No se encontró la visita con ID {id}." });
            }

            if (visitRecord.HoraSalida.HasValue)
            {
                return BadRequest(new { message = "Esta visita ya tiene una hora de salida registrada." });
            }

            visitRecord.HoraSalida = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, new { message = "Conflicto de actualización." });
            }

            return NoContent(); 
        }
    }
}