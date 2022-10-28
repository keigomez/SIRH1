using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    public class CObjetivoCalificacionDTO : CBaseDTO
    {
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public CPeriodoCalificacionDTO Periodo { get; set; }
        
        [DataMember]
        public CSeccionDTO Seccion { get; set; }

        [DataMember]
        public string ActividadPresupuestaria { get; set; }

        [DataMember]
        public string ProductoPrograma { get; set; }

        [DataMember]
        public int IndEstado { get; set; }

        [DataMember]
        public string Observaciones { get; set; }
    }
}
