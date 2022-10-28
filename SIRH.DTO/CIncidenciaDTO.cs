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
    public class CIncidenciaDTO : CBaseDTO
    {
        [DisplayName("Número de incidencia")]
        public string NumeroIncidencia { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioEmisor { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioEncargado { get; set; }

        [DataMember]
        public CCatalogoIncidenciaDTO Catalogo { get; set; }

        [DataMember]
        [DisplayName("Prioridad de Incidencia")]
        public String DetallePriIncidencia { get; set; }

        [DataMember]
        public int EstIncidencia { get; set; }

        [DataMember]
        [DisplayName("Estado de Incidencia")]
        public String DetalleEstIncidencia { get; set; }

        [DataMember]
        public string IpOrigen { get; set; }

        [DataMember]
        [MaxLength(300, ErrorMessage = "Aviso, el tamaño de la descripción del error debe ser no más de 300 caracteres.")]
        public string Error { get; set; }
        
        [DataMember]
        [MaxLength(300, ErrorMessage = "Aviso, el tamaño del motivo de rechazo debe ser no más de 300 caracteres.")]
        public string Motivo { get; set; }

        [DataMember]
        [DisplayName("Imagen de Error")]
        public byte[] ImagenError { get; set; }

        [DataMember]
        [DisplayName("Fecha Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecInicio { get; set; }

        [DataMember]
        [DisplayName("Fecha Fin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecFin { get; set; }
    }
}