//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIRH.DatosMarcasReloj
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmpleadoHorario
    {
        public int IdHorarioEmpleado { get; set; }
        public string IdCodigoEmpleado { get; set; }
        public int IdHorario { get; set; }
        public System.DateTime FechaInicioHorario { get; set; }
        public string TipoHorario { get; set; }
        public int CantidadTiempo { get; set; }
        public short TipoTiempo { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public short Estatus { get; set; }
        public int NoCantidadTiempo { get; set; }
    }
}
