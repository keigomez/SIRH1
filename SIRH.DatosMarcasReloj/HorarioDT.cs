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
    
    public partial class HorarioDT
    {
        public int IdDia { get; set; }
        public int IdCorrelaHorarioDia { get; set; }
        public int IdHorario { get; set; }
        public int SalidaDiaSig { get; set; }
        public System.DateTime HoraEntrada { get; set; }
        public System.DateTime ToleranciaAHoraE { get; set; }
        public System.DateTime ToleranciaDHoraE { get; set; }
        public System.DateTime HoraSalida { get; set; }
        public System.DateTime ToleranciaAHoraS { get; set; }
        public System.DateTime ToleranciaDHoraS { get; set; }
        public short Almuerzo { get; set; }
        public System.DateTime TiempoAlmuerzo { get; set; }
        public System.DateTime HoraITAlmuerzo { get; set; }
        public System.DateTime HoraFTAlmuerzo { get; set; }
        public System.DateTime SalidaMatutino { get; set; }
        public System.DateTime EntradaMatutino { get; set; }
        public System.DateTime SalidaVespertino { get; set; }
        public System.DateTime EntradaVespertino { get; set; }
        public System.DateTime IniciaHorasExtras { get; set; }
        public System.DateTime IniciaHorasSimples { get; set; }
        public System.DateTime IniciaHorasDobles { get; set; }
    }
}