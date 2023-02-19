using GrupoASD.GestionActivos.Api.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Servicios
{
    public interface ITipoActivoRepositorio
    {
        Task<List<TipoActivo>> BuscarTodos();
    }
    public class TipoActivoRepositorio : ITipoActivoRepositorio
    {
        private readonly ActivosASDContext _context;
        public TipoActivoRepositorio(ActivosASDContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve el listado de los tipos  de un activo
        /// </summary>
        /// <returns></returns>
        public async Task<List<TipoActivo>> BuscarTodos()
        {
            return await _context.TipoActivo.Select(x =>
            new TipoActivo
            {
                IdTipoActivo = x.IdTipoActivo,
                Nombre = x.Nombre,                
                Estado = x.Estado
            }).ToListAsync();
        }

    }
}
