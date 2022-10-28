using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CPrestamoPuestoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Fecha de Traslado")]
        public DateTime FechaDeTraslado { get; set; }
        [DataMember]
        [DisplayName("N° de Resolución")]
        public string NumDeResolucion { get; set; }
        [DataMember]
        [DisplayName("N° Oficio Rescisión")]
        public string NumOficioDeRescision { get; set; }
        [DataMember]
        [DisplayName("Fecha Fin de Convenio")]
        public DateTime FechaFinalConvenio { get; set; }
        [DataMember]
        [DisplayName("N° de Rescisión")]
        public string NumDeRescision { get; set; }
        [DataMember]
        public CEntidadAdscritaDTO EntidadAdscrita { get; set; }
        [DataMember]
        public CEntidadGubernamentalDTO EntidadGubernamental { get; set; }
        [DataMember]
        public CUbicacionAdministrativaDTO UbicacionAdministrativa { get; set; }
        [DataMember]
        public CPuestoDTO Puesto { get; set; }        
    }
}
