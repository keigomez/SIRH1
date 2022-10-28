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
    public class CCursoCapacitacionDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Título Escaneado")]
        public byte[] ImagenTitulo { get; set; }
        [DataMember]
        [DisplayName("Total de horas")]
        public int TotalHoras { get; set; }
        [DataMember]
        [DisplayName("Total de puntos")]
        public int TotalPuntos { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Título obtenido")]
        public string DescripcionCapacitacion { get; set; }
        [DataMember]
        [DisplayName("Número de resolución")]
        public string Resolucion { get; set; }
        [DataMember]
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        [DisplayName("Fecha de Finalización")]
        public DateTime FechaFinal { get; set; }
        [DataMember]
        public CModalidadDTO Modalidad { get; set; }
        [DataMember]
        public CFormacionAcademicaDTO FormacionAcademica { get; set; }
        [DataMember]
        public CEntidadEducativaDTO EntidadEducativa { get; set; }
        [DataMember]
        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember]
        public DateTime FecRegistro { get; set; }
        [DataMember]
        public DateTime FecVence { get; set; }

    }
}
    