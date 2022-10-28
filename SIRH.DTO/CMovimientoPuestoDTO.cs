using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

//
namespace SIRH.DTO
{
    [DataContract]
    public class CMovimientoPuestoDTO : CBaseDTO
    {
        [DataMember]
        [Required(ErrorMessage="Debe existir un oficio que respalde la declaración del puesto como vacante")]
        [DisplayName("N° Oficio")]
        public string CodOficio { get; set; }
        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecMovimiento { get; set; }
        [DataMember]
        [DisplayName("Fecha vencimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaVencimiento { get; set; }
        [DataMember]
        [DisplayName("Explicacion")]
        public string Explicacion { get; set; }
        [DataMember]
        public CMotivoMovimientoDTO MotivoMovimiento { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }
        [DataMember]
        public CEstadoMovimientoPuestoDTO EstadoMovimientoPuesto { get; set; }    
    
    }
}
