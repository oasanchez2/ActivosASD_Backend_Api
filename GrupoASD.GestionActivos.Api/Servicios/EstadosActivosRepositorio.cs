using GrupoASD.GestionActivos.Api.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrupoASD.GestionActivos.Api.Entidades;

namespace GrupoASD.GestionActivos.Api.Servicios
{
    public interface IEstadosActivosRepositorio
    {
        Task<List<EstadosActivos>> BuscarTodos();
    }
    public class EstadosActivosRepositorio : IEstadosActivosRepositorio
    {
        private readonly ActivosASDContext _context;

        public EstadosActivosRepositorio(ActivosASDContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve el listado de estados de un activo
        /// </summary>
        /// <returns></returns>
        public async Task<List<EstadosActivos>> BuscarTodos()
        {
            return await _context.EstadosActivos.Select(x =>
            new EstadosActivos
            {
                IdEstado = x.IdEstado,
                NombreEstado = x.NombreEstado,
                Estado = x.Estado
            }).ToListAsync();
        }
    }
}
