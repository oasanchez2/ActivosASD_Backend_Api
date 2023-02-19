using GrupoASD.GestionActivos.Api.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrupoASD.GestionActivos.Api.Entidades;

namespace GrupoASD.GestionActivos.Api.Servicios
{
    public interface IActivosReposotorio
    {
        Task<List<Activos>> BuscarTodos();
        Task<List<Activos>> BuscarActivoPeronalizada(ActivosBusquedaModel activoBusqueda);
        Task<Activos> ObtenerActivo(int id);
        Task<Activos> ObtenerActivoPorNombre(string nombre);
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
            //return await _context.Activos.Include(x => x.IdEstadoActualNavigation).Include(x => x.IdTipoActivoNavigation).ToListAsync();
            return await _context.Activos.Select(x => new Activos
            {
                IdActivo = x.IdActivo,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                IdTipoActivo = x.IdTipoActivo,
                Serial = x.Serial,
                NumeroInternoInventario = x.NumeroInternoInventario,
                Peso = x.Peso,
                Alto = x.Alto,
                Ancho = x.Ancho,
                Largo = x.Largo,
                ValorCompra = x.ValorCompra,
                FechaCompra = x.FechaCompra,
                FechaBaja = x.FechaBaja,
                IdEstadoActual = x.IdEstadoActual,
                Color = x.Color,
                IdEstadoActualNavigation = new EstadosActivos
                {
                    IdEstado = x.IdEstadoActualNavigation.IdEstado,
                    NombreEstado = x.IdEstadoActualNavigation.NombreEstado,
                    Estado = x.IdEstadoActualNavigation.Estado
                },
                IdTipoActivoNavigation = new TipoActivo
                {
                    IdTipoActivo = x.IdTipoActivoNavigation.IdTipoActivo,
                    Nombre = x.IdTipoActivoNavigation.Nombre,
                    Estado = x.IdTipoActivoNavigation.Estado
                }
            }
             ).ToListAsync();
        }

        /// <summary>
        /// Busca los activos segun los paametros de busqueda insertados
        /// </summary>
        /// <param name="activoBusqueda"></param>
        /// <returns></returns>
        public async Task<List<Activos>> BuscarActivoPeronalizada(ActivosBusquedaModel activoBusqueda)
        {
            List<Activos> resultado = new List<Activos>();
            if (activoBusqueda.IdTipoActivo > 0 && activoBusqueda.FechaCompraInicio == null &&
               activoBusqueda.FechaCompraFin == null && string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.IdTipoActivo == activoBusqueda.IdTipoActivo).ToListAsync();
            }
            else if (activoBusqueda.IdTipoActivo > 0 && activoBusqueda.FechaCompraInicio != null &&
               activoBusqueda.FechaCompraFin != null && string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.IdTipoActivo == activoBusqueda.IdTipoActivo &&
                                                         x.FechaCompra > activoBusqueda.FechaCompraInicio &&
                                                         x.FechaCompra < activoBusqueda.FechaCompraFin).ToListAsync();
            }
            else if (activoBusqueda.IdTipoActivo > 0 && activoBusqueda.FechaCompraInicio != null &&
               activoBusqueda.FechaCompraFin != null && !string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.IdTipoActivo == activoBusqueda.IdTipoActivo &&
                                                         x.FechaCompra > activoBusqueda.FechaCompraInicio &&
                                                         x.FechaCompra < activoBusqueda.FechaCompraFin &&
                                                         x.Serial == activoBusqueda.Serial).ToListAsync();
            }
            else if (activoBusqueda.IdTipoActivo > 0 && activoBusqueda.FechaCompraInicio == null &&
               activoBusqueda.FechaCompraFin == null && !string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.IdTipoActivo == activoBusqueda.IdTipoActivo &&
                                                          x.Serial == activoBusqueda.Serial).ToListAsync();
            }
            else if (activoBusqueda.IdTipoActivo == 0 && activoBusqueda.FechaCompraInicio != null &&
               activoBusqueda.FechaCompraFin != null && string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.FechaCompra > activoBusqueda.FechaCompraInicio &&
                                                          x.FechaCompra < activoBusqueda.FechaCompraFin).ToListAsync();
            }
            else if (activoBusqueda.IdTipoActivo == 0 && activoBusqueda.FechaCompraInicio != null &&
               activoBusqueda.FechaCompraFin != null && !string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.FechaCompra > activoBusqueda.FechaCompraInicio &&
                                                         x.FechaCompra < activoBusqueda.FechaCompraFin &&
                                                         x.Serial == activoBusqueda.Serial).ToListAsync();
            }
            else if (activoBusqueda.IdTipoActivo == 0 && activoBusqueda.FechaCompraInicio == null &&
               activoBusqueda.FechaCompraFin == null && !string.IsNullOrEmpty(activoBusqueda.Serial))
            {
                resultado = await _context.Activos.Where(x => x.Serial == activoBusqueda.Serial).ToListAsync();
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene un activo de la base de datos por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Activos> ObtenerActivo(int id)
        {
            return await _context.Activos.Include(x => x.IdEstadoActualNavigation).Include(x => x.IdTipoActivoNavigation).FirstOrDefaultAsync(x => x.IdActivo == id);
        }

        /// <summary>
        /// Buscar un activo en base de datos por su nombre
        /// </summary>
        /// <returns></returns>
        public async Task<Activos> ObtenerActivoPorNombre(string nombre)
        {
            return await _context.Activos.Include(x => x.IdEstadoActualNavigation).Include(x => x.IdTipoActivoNavigation).FirstOrDefaultAsync(x => x.Nombre.Equals(nombre));
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
