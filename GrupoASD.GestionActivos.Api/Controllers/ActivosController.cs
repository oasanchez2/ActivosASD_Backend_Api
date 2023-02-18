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
                if (listaActivos.Count() <= 0)
                {
                    return NotFound(new { mensaje = "No se encontraron registros en la base de datos." });
                }
                return listaActivos;

            }
            catch (Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);
                long id = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = id });
            }

        }

        /// <summary>
        /// Devuelve un activo encontrado por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Activos>> GetActivos(int id)
        {
            try
            {
                if(id <= 0)
                {
                    return NotFound(new { mensaje = "Debe ingresar id mayor a 0" });
                }
                Activos activo = await _activosReposotorio.ObtenerActivo(id);
                if(activo == null)
                {
                    return NotFound(new { mensaje = "No se encontraron activo con ese id." });
                }
                               
                return activo;

            }
            catch (Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);
                long idlog = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = idlog });
            }
        }

        /// <summary>
        /// Devuelve los activos encontrados segun la busqueda enviada
        /// </summary>
        /// <param name="activoBusqueda"></param>
        /// <returns></returns>
        [Route("busqueda")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activos>>> GetActivos(ActivosBusquedaModel activoBusqueda)
        {
            try
            {
                //Validación para que venga un fecha inicio o fin y que la fecha inicio no sea mayor a la final
                if(activoBusqueda.FechaCompraInicio != null && activoBusqueda.FechaCompraFin == null)
                {
                    return NotFound(new { mensaje = "Debe ingresar una fecha final" });
                }
                if (activoBusqueda.FechaCompraInicio == null && activoBusqueda.FechaCompraFin != null)
                {
                    return NotFound(new { mensaje = "Debe ingresar una fecha Inicial" });
                }
                if (activoBusqueda.FechaCompraInicio != null && activoBusqueda.FechaCompraFin != null)
                {
                    if(activoBusqueda.FechaCompraInicio > activoBusqueda.FechaCompraFin)
                    {
                        return NotFound(new { mensaje = "La fecha de fin no puede ser menor a la fecha de inicio" });
                    }
                }

                List<Activos> activos = await _activosReposotorio.BuscarActivoPeronalizada(activoBusqueda);
                if (activos.Count() <= 0)
                {
                    return NotFound(new { mensaje = "No se encontraron registros en la base de datos." });
                }

                return activos;
            }
            catch(Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);
                long id = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = id });
            }
        }

        /// <summary>
        /// Metodo encargado de actualizar algunos campos de los activos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activos"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivos(int id, ActivosUpdateModel activoModel)
        {
            try
            {
                if (id != activoModel.IdActivo)
                {
                    return BadRequest(new { mensaje = "El id el activo no corresponde al id a modificar" });
                }

                Activos activo = await _activosReposotorio.ObtenerActivo(id);

                if (activo == null)
                {
                    return NotFound(new { mensaje = string.Format("No existe el activo con identificador {0}", id) });
                }

                if(activoModel.FechaBaja != null)
                {
                    if (activoModel.FechaBaja < activo.FechaCompra)
                    {
                        return NotFound(new { mensaje = "La fecha de baja no puede ser menor a la fecha de compra" });
                    }
                }

                activo.Serial = activoModel.Serial;
                activo.FechaBaja = activoModel.FechaBaja;
                await _activosReposotorio.SaveAsync();

                return Ok(new { mensaje = "Activo actualizado correctamente" });
            }
            catch(Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);
                long idLog = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = idLog });
            }
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
                    Activos activoPorNombre = await _activosReposotorio.ObtenerActivoPorNombre(activoModel.Nombre);
                    if(activoPorNombre != null)
                    {
                        return BadRequest(new { mensaje = string.Format("El activo {0} ya se encuentra registrado.", activoModel.Nombre) });
                    }

                    if (activoModel.FechaBaja != null)
                    {
                        if (activoModel.FechaBaja < activoModel.FechaCompra)
                        {
                            return NotFound(new { mensaje = "La fecha de baja no puede ser menor a la fecha de compra" });
                        }
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
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Activos>> DeleteActivos(int id)
        //{
        //    var activos = await _context.Activos.FindAsync(id);
        //    if (activos == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Activos.Remove(activos);
        //    await _context.SaveChangesAsync();

        //    return activos;
        //}
    }
}
