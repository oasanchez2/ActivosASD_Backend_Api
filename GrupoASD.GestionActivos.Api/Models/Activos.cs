using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrupoASD.GestionActivos.Api.Models
{
    public partial class Activos
    {
        public int IdActivo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdTipoActivo { get; set; }
        public string Serial { get; set; }
        public string NumeroInternoInventario { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Alto { get; set; }
        public decimal? Ancho { get; set; }
        public decimal? Largo { get; set; }
        public decimal ValorCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int IdEstadoActual { get; set; }
        public string Color { get; set; }

        public virtual EstadosActivos IdEstadoActualNavigation { get; set; }
        public virtual TipoActivo IdTipoActivoNavigation { get; set; }
    }
}
