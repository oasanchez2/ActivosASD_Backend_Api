using GrupoASD.GestionActivos.Api.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Servicios
{
    public interface IActivosReposotorio
    {
        Task<List<Activos>> BuscarTodos();
    }
    public class ActivosReposotorio : IActivosReposotorio
    {
        private readonly ActivosASDContext _context;

        public ActivosReposotorio(ActivosASDContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devuelve todos los activos registrados en la aplicación.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Activos>> BuscarTodos()
        {
            return await _context.Activos.ToListAsync();            
        }
    }
}
