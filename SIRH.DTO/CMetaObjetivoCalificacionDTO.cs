using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    public class CMetaObjetivoCalificacionDTO : CBaseDTO
    {
        [DataMember]
        public CObjetivoCalificacionDTO Objetivo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        public DateTime FechaFinalizacion { get; set; }
        [DataMember]
        public int TipoMeta { get; set; }

        [DataMember]
        public string ProductoEspecifico { get; set; }

        [DataMember]
        public string FuenteDatos { get; set; }

        [DataMember]
        public string Supuestos { get; set; }

        [DataMember]
        public string NotasTecnicas { get; set; }

        [DataMember]
        public string TipoIndicador { get; set; }

        [DataMember]
        public string Indicador { get; set; }

        [DataMember]
        public string Dimension { get; set; }

        [DataMember]
        public string MetaAnual { get; set; }

        [DataMember]
        public int IndEstado { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public int Prioridad { get; set; }

        [DataMember]
        public decimal ValorAsignado { get; set; }

        [DataMember]
        public decimal ValorCumplido { get; set; }
    }
}
