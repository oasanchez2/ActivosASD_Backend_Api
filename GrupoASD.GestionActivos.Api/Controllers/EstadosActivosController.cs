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
    public class EstadosActivosController : Controller
    {
        private readonly ILogger<ActivosController> _logger;
        private readonly ILogsErrorReposotorio _logsErrorReposotorio;
        private readonly IEstadosActivosRepositorio _estadosActivosRepositorio;

        public EstadosActivosController(ILogger<ActivosController> logger,
                                        IEstadosActivosRepositorio estadosActivosRepositorio,
                                        ILogsErrorReposotorio logsErrorReposotorio)
        {
            _logger = logger;
            _logsErrorReposotorio = logsErrorReposotorio;
            _estadosActivosRepositorio = estadosActivosRepositorio;
        }

        /// <summary>
        /// Devuelve la lista de Estados para los activos.
        /// </summary>
        /// <returns>IEnumerable<Activos></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadosActivos>>> GetEstadoActivos()
        {
            try
            {
                var listaEstados = await _estadosActivosRepositorio.BuscarTodos();
                if (listaEstados.Count() <= 0)
                {
                    return NotFound(new { mensaje = "No se encontraron registros en la base de datos." });
                }
                return listaEstados;
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
