using GrupoASD.GestionActivos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ActivosASDContext context)
        {
            context.Database.EnsureCreated();

            // Busque Tipos activo.
            if (context.TipoActivo.Any())
            {
                return;   //DB has been seeded
            }

            using (var transaccion = context.Database.BeginTransaction())
            {
                var tiposActivo = new TipoActivo[]
                {
                    new TipoActivo{ IdTipoActivo = 1, Nombre = "Bienes Inmuebles", Estado = true },
                    new TipoActivo{ IdTipoActivo = 2, Nombre = "Maquinaria", Estado = true },
                    new TipoActivo{ IdTipoActivo = 3, Nombre = "Material de Oficiana", Estado = true },
                };
                foreach (TipoActivo item in tiposActivo)
                {
                    context.TipoActivo.Add(item);
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.TipoActivo ON;");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.TipoActivo OFF;");
                transaccion.Commit();
            };

            using (var transaccion = context.Database.BeginTransaction())
            {
                var estadoActivo = new EstadosActivos[]
                {
                    new EstadosActivos{  IdEstado = 1, NombreEstado = "Activo", Estado = true },
                    new EstadosActivos{  IdEstado = 2, NombreEstado = "Disponible", Estado = true },
                    new EstadosActivos{  IdEstado = 3, NombreEstado = "Asignado", Estado = true },
                    new EstadosActivos{  IdEstado = 4, NombreEstado = "Reparacion", Estado = true },
                    new EstadosActivos{  IdEstado = 5, NombreEstado = "Dado de baja", Estado = true }
                };
                foreach(EstadosActivos item in estadoActivo)
                {
                    context.EstadosActivos.Add(item);
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.EstadosActivos ON;");
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.EstadosActivos OFF;");
                transaccion.Commit();
            };
        }
    }
}
