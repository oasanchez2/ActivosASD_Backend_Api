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
using GrupoASD.GestionActivos.Api.Entidades;

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

        /// <summary>
        /// Crea un nuevo activo en la base de datos
        /// </summary>
        /// <param name="activoModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Activos>> PostActivos(ActivosModel activoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Activos activoPorNombre = await _activosReposotorio.BuscarActivoPorNombre(activoModel.Nombre);
                    if(activoPorNombre != null)
                    {
                        return NotFound(new { mensaje = string.Format("El activo {0} ya se encuentra registrado.", activoModel.Nombre) });
                    }

                    Activos activo = new Activos
                    {
                         Nombre = activoModel.Nombre,
                         Descripcion = activoModel.Descripcion,
                         IdTipoActivo = activoModel.IdTipoActivo,
                         Serial = activoModel.Serial,
                         NumeroInternoInventario = activoModel.NumeroInternoInventario,
                         Peso = activoModel.Peso,
                         Alto = activoModel.Alto,
                         Ancho = activoModel.Ancho,
                         Largo = activoModel.Largo,
                         ValorCompra = activoModel.ValorCompra,
                         FechaCompra = activoModel.FechaCompra,
                         FechaBaja = activoModel.FechaBaja,
                         IdEstadoActual = activoModel.IdEstadoActual,
                         Color = activoModel.Color
                    };
                    _activosReposotorio.Insertar(activo);
                    await _activosReposotorio.SaveAsync();
                    activoModel.IdActivo = activo.IdActivo;

                    return CreatedAtAction("GetActivos", new { id = activoModel.IdActivo }, activoModel);                    
                }
                else
                {                    
                    return new UnprocessableEntityObjectResult(ModelState);
                }

            }
            catch(Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);
                long id = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = id });
            }
 
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
