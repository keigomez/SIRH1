using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    public class CMovimientoBorradorAccionPersonalDTO : CBaseDTO
    {
        [DataMember]
        public CBorradorAccionPersonalDTO Borrador { get; set; }
        [DataMember]
        [DisplayName("Fecha Movimiento")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecMovimiento { get; set; }
        [DataMember]
        [DisplayName("Movimiento")]
        public int CodMovimiento { get; set; }
        [DataMember]
        [DisplayName("Usuario")]
        public string UsuarioEnvia { get; set; }
        [DataMember]
        [DisplayName("Usuario Asignado")]
        public string UsuarioRecibe { get; set; }
        [DataMember]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observaciones")]
        public string ObsMovimiento { get; set; }
    }
}