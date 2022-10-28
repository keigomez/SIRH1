using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(object))]
    [KnownType(typeof(CEntidadSegurosDTO))]
    public class CRespuestaDTO : CBaseDTO
    {
        [DataMember]
        public int Codigo { get; set; }
        [DataMember]
        public string Resumen { get; set; }
        [DataMember]
        public object Contenido { get; set; }
    }
}
