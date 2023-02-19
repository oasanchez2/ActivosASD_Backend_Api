using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrupoASD.GestionActivos.Api.Models
{
    public partial class EstadosActivos
    {
        public EstadosActivos()
        {
            Activos = new HashSet<Activos>();
        }

        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public bool Estado { get; set; }
        [JsonIgnore]
        public virtual ICollection<Activos> Activos { get; set; }
    }
}
