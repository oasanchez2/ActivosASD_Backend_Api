using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrupoASD.GestionActivos.Api.Models
{
    public partial class LogsError
    {
        public long IdLogError { get; set; }
        public string OrignError { get; set; }
        public string MethodError { get; set; }
        public string MessageError { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public DateTime DayError { get; set; }
    }
}
