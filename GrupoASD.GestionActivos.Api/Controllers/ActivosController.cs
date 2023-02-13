using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrupoASD.GestionActivos.Api.Models;
using GrupoASD.GestionActivos.Api.Servicios;
using Microsoft.Extensions.Logging;

namespace GrupoASD.GestionActivos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivosController : ControllerBase
    {
        private readonly ActivosASDContext _context;
        private readonly ILogger<ActivosController> _logger;
        private readonly ILogsErrorReposotorio _logsErrorReposotorio;
        private readonly IActivosReposotorio _activosReposotorio;
        public ActivosController(ActivosASDContext context,
            ILogger<ActivosController> logger,
            ILogsErrorReposotorio logsErrorReposotorio,
            IActivosReposotorio activosReposotorio)
        {
            _context = context;
            _logger = logger;
            _logsErrorReposotorio = logsErrorReposotorio;
            _activosReposotorio = activosReposotorio;
            
        }

        /// <summary>
        /// Devuelve la lista de Activos encontrados en la base de datos
        /// </summary>
        /// <returns>IEnumerable<Activos></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activos>>> GetActivos()
        {
            try
            {
                List<Activos> listaActivos = await _activosReposotorio.BuscarTodos();
                if(listaActivos.Count() <= 0)
                {
                    return NotFound(new { mensaje = "No se encontraron registros en la base de datos."});
                }
                return listaActivos;
                
            }
            catch(Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);                
                long id = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = id });
            }
            
        }

        // GET: api/Activos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activos>> GetActivos(int id)
        {
            var activos = await _context.Activos.FindAsync(id);

            if (activos == null)
            {
                return NotFound();
            }

            return activos;
        }

        // PUT: api/Activos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivos(int id, Activos activos)
        {
            if (id != activos.IdActivo)
            {
                return BadRequest();
            }

            _context.Entry(activos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Activos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Activos>> PostActivos(Activos activos)
        {
            _context.Activos.Add(activos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivos", new { id = activos.IdActivo }, activos);
        }

        // DELETE: api/Activos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Activos>> DeleteActivos(int id)
        {
            var activos = await _context.Activos.FindAsync(id);
            if (activos == null)
            {
                return NotFound();
            }

            _context.Activos.Remove(activos);
            await _context.SaveChangesAsync();

            return activos;
        }

        private bool ActivosExists(int id)
        {
            return _context.Activos.Any(e => e.IdActivo == id);
        }
    }
}
