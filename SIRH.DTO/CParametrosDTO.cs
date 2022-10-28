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
    public class CParametrosDTO : CBaseDTO
    {
        // Campos para filtrar en los Reportes y Gráficos
        [DataMember]
        public string nomParametro { get; set; }
        [DataMember]
        public string[] valoresBuscar { get; set; }
        [DataMember]
        public string[] valoresDescartar { get; set; }
    }
}