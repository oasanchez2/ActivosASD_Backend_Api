using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Entidades
{
    public class ActivosBusquedaModel
    {
        public int IdTipoActivo { get; set; }
        public DateTime? FechaCompraInicio { get; set; }
        public DateTime? FechaCompraFin { get; set; }
        public string Serial { get; set; }
    }
}
