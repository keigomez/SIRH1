using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.DTO
{
    [DataContract]
    public class CCursoDTO : CBaseDTO
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string Nombre { get; set; }

        [DisplayName("Nombre de curso")]
        [DataMember]
        public string NombreCurso { get; set; }
        [DataMember]
        public decimal TotalHoras { get; set; }

        [DisplayName("Tipo de Curso")]
        [DataMember]
        public string TipoCurso { get; set; }
        [DataMember]
        public string ImpartidoEn { get; set; }
        [DataMember]
        public DateTime FecRige { get; set; }
        [DataMember]
        public DateTime FecFinal { get; set; }
        [DataMember]
        public string Resolucion { get; set; }

    }
}
