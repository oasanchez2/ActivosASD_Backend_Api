using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Entidades
{
    public class ActivosModel
    {
        public int IdActivo { get; set; }
        [Required(ErrorMessage = "{0} es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public int IdTipoActivo { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public string Serial { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public string NumeroInternoInventario { get; set; }        
        public decimal? Peso { get; set; }
        public decimal? Alto { get; set; }
        public decimal? Ancho { get; set; }
        public decimal? Largo { get; set; }
        
        [Required(ErrorMessage = "{0} es requerido")]
        public decimal ValorCompra { get; set; }
        
        [Required(ErrorMessage = "{0} es requerido")]
        public DateTime FechaCompra { get; set; }
        public DateTime? FechaBaja { get; set; }
        
        [Required(ErrorMessage = "{0} es requerido")]
        public int IdEstadoActual { get; set; }
        public string Color { get; set; }
    }
}
