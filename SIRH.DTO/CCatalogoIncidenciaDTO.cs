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
    public class CCatalogoIncidenciaDTO : CBaseDTO
    {
        [DataMember]
        public CPerfilDTO Perfil { get; set; }
        [DataMember]
        [DisplayName("Nombre Catalogo")]
        public string NomCatalogo { get; set; }
        [DataMember]
        [DisplayName("Prioridad de Incidencia")]
        public int Prioridad { get; set; }
        [DataMember]        
        public String DetallePrioridad { get; set; }
    }
}