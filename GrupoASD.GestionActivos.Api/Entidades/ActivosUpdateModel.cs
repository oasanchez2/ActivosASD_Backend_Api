using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoASD.GestionActivos.Api.Entidades
{
    public class ActivosUpdateModel
    {
        [Required(ErrorMessage = "{0} es requerido")]
        public int IdActivo { get; set; }

        [Required(ErrorMessage = "{0} es requerido")]
        public string Serial { get; set; }

        public DateTime? FechaBaja { get; set; }

    }
}
