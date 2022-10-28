using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    public enum JornadaEnum { D, M, N, DD, MD, ND }
    public enum EstadoDetalleExtraEnum { Activo = 1, Anulado = 2, Cerrado = 3, Aprobado = 4, Rechazado = 5 }
    [DataContract]
    public class CDetalleTiempoExtraDTO : CBaseDTO
    {
        [DataMember]
        public JornadaEnum? Jornada { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de emisión")]
        public DateTime? FechaCarga { get; set; }
        [DataMember]
        public bool FechaInicioEspecial { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha inicial")]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha inicial")]
        public DateTime? FechaInicioDoble { get; set; }
        [DataMember]
        public bool FechaFinalEspecial { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha final")]
        public DateTime FechaFinal { get; set; }
        [DataMember]
        public DateTime HoraInicioDate { get; set; }
        [DataMember]
        public DateTime HoraFinalDate { get; set; }
        [DataMember]
        [DisplayName("Hora inicio")]
        public string HoraInicio { get; set; }
        [DataMember]
        [DisplayName("Hora final")]
        public string HoraFinal { get; set; }
        [DataMember]
        [DisplayName("Total horas H0")]
        public string HoraTotalH0 { get; set; }
        [DataMember]
        [DisplayName("Total horas H1")]
        public string HoraTotalH1 { get; set; }
        [DataMember]
        [DisplayName("Total horas H2")]
        public string HoraTotalH2 { get; set; }
        [DataMember]
        [DisplayName("Minuto inicio")]
        public string MinutoInicio { get; set; }
        [DataMember]
        [DisplayName("Minuto final")]
        public string MinutoFinal { get; set; }
        [DataMember]
        [DisplayName("Total minutos H0")]
        public string MinutoTotalH0 { get; set; }
        [DataMember]
        [DisplayName("Total minutos H1")]
        public string MinutoTotalH1 { get; set; }
        [DataMember]
        [DisplayName("Total minutos H2")]
        public string MinutoTotalH2 { get; set; }
        [DataMember]
        public CTipoExtraDTO TipoExtra { get; set; }
        [DataMember]
        public CRegistroTiempoExtraDTO RegistroTiempoExtra { get; set; }
        [DataMember]
        [DisplayName("H0")]
        public decimal H0 { get; set; }
        [DataMember]
        [DisplayName("H1")]
        public decimal H1 { get; set; }
        [DataMember]
        [DisplayName("H2")]
        public decimal H2 { get; set; }
        [DataMember]
        [DisplayName("Total línea")]
        public decimal TotalLinea { get; set; }
        [DataMember]
        public EstadoDetalleExtraEnum Estado { get; set; }

    }
}
