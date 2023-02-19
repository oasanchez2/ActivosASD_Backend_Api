using GrupoASD.GestionActivos.Api.Models;
using GrupoASD.GestionActivos.Api.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoActivoController : Controller
    {
        private readonly ILogger<ActivosController> _logger;
        private readonly ILogsErrorReposotorio _logsErrorReposotorio;
        private readonly ITipoActivoRepositorio _tipoActivoRepositorio;
        public TipoActivoController(ILogger<ActivosController> logger,
                                ITipoActivoRepositorio tipoActivoRepositorio,
                                ILogsErrorReposotorio logsErrorReposotorio)
        {
            _logger = logger;
            _logsErrorReposotorio = logsErrorReposotorio;
            _tipoActivoRepositorio = tipoActivoRepositorio;
        }

        /// <summary>
        /// Devuelve la lista de tipos para los activos.
        /// </summary>
        /// <returns>IEnumerable<Activos></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoActivo>>> GetTipoActivos()
        {
            try
            {
                var listaTipos = await _tipoActivoRepositorio.BuscarTodos();
                if (listaTipos.Count() <= 0)
                {
                    return NotFound(new { mensaje = "No se encontraron registros en la base de datos." });
                }
                return listaTipos;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(0, "Exception. {0}", ex.Message);
                long id = await _logsErrorReposotorio.InsertAndSaveAsync(ex);
                return StatusCode(500, new { mensaje = "Se ha genera un error interno consulte para mas detalle con el identificador", idlog = id });
            }
        }
    }
}
