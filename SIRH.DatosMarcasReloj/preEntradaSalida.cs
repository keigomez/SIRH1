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
    
    public partial class preEntradaSalida
    {
        public decimal IdEntradaSalida { get; set; }
        public string IdJornada { get; set; }
        public int IdHorario { get; set; }
        public int IdDiaSemana { get; set; }
        public string CodigoEmpleado { get; set; }
        public string TipoOperacion { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime Hora { get; set; }
        public string FormaCaptura { get; set; }
        public int IdJustificacion { get; set; }
        public string IdSupervisor { get; set; }
        public string UsuarioOpera { get; set; }
        public short Discrepancia { get; set; }
    }
}
