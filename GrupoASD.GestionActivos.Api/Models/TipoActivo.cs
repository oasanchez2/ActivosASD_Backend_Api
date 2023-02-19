using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrupoASD.GestionActivos.Api.Models
{
    public partial class TipoActivo
    {
        public TipoActivo()
        {
            Activos = new HashSet<Activos>();
        }

        public int IdTipoActivo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }

        [JsonIgnore]
        public virtual ICollection<Activos> Activos { get; set; }
    }
}
