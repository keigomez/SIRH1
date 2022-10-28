using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class CCursoGradoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Título Escaneado")]
        public byte[] ImagenTitulo { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Título obtenido")]
        public string CursoGrado { get; set; }
        [DataMember]
        [DisplayName("Incentivo")]
        public int Incentivo { get; set; }
        [DataMember]
        [DisplayName("Tipo de Curso académico")]
        public int TipoGrado { get; set; }
        [DataMember]
        [DisplayName("Fecha de emisión")]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        [DisplayName("Número de resolución")]
        public string Resolucion { get; set; }
        [DataMember]
        public CFormacionAcademicaDTO FormacionAcademica { get; set; }
        [DataMember]
        public CEntidadEducativaDTO EntidadEducativa { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [DataMember]
        public int Estado { get; set; }
    }


}