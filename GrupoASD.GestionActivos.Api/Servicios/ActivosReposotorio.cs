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
        Task<Activos> BuscarActivoPorNombre(string nombre);
        void Insertar(Activos activos);
        Task SaveAsync();
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

        /// <summary>
        /// Buscar un activo en base de datos por su nombre
        /// </summary>
        /// <returns></returns>
        public async Task<Activos> BuscarActivoPorNombre(string nombre)
        {
            return await _context.Activos.FirstOrDefaultAsync(x => x.Nombre.Equals(nombre));
        }
        /// <summary>
        /// Inserta un activo.
        /// </summary>
        /// <param name="activos"></param>
        public void Insertar(Activos activos)
        {
            _context.Add(activos);
        }

        /// <summary>
        /// Guarda un log en la base de datos
        /// </summary>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
