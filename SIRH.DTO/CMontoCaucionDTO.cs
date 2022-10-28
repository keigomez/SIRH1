using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CMontoCaucionDTO : CBaseDTO
    {
        [DataMember]
        [Required(ErrorMessage="Debe establecer el Nivel para el monto de caución")]
        [DisplayName("Nivel")]
        public string Nivel { get; set; }
        [DataMember]
        [DataType(DataType.Currency, ErrorMessage = "Debe ingresar un monto válido para el nivel")]
        [DisplayName("Monto en colones")]
        public decimal MontoColones { get; set; }
        [DataMember]
        [DisplayName("Cargos asociados")]
        public string Descripcion { get; set; }
        [DataMember]
        [DisplayName("Reglamento asociado")]
        [Required(ErrorMessage = "Debe establecer la base justificativa (Decreto, ley, etc) para el monto de caución")]
        public string Justificacion { get; set; }
        [DataMember]
        [DisplayName("Fecha rige")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaRige { get; set; }
        [DataMember]
        public int EstadoMonto { get; set; }
        [DataMember]
        [DisplayName("Estado")]
        public string DetalleEstadoMonto { get; set; }
        [DataMember]
        [DisplayName("Justificación de inactividad")]
        public string JustificacionInactiva { get; set; }
        [DataMember]
        [DisplayName("Fecha vence")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInactiva { get; set; }

    }
}
