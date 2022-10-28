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
    public class CAccionPersonalHistoricoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Número de Acción")]
        public string NumAccion { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecVence { get; set; }
        
        [DataMember]
        [DisplayName("Código Acción")]
        public int CodAccion { get; set; }


        [DataMember]
        [DisplayName("Explicación")]
        public string Explicacion { get; set; }

        [DataMember]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        [DataMember]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DataMember]
        [DisplayName("Apellido1")]
        public string Apellido1 { get; set; }

        [DataMember]
        [DisplayName("Apellido2")]
        public string Apellido2 { get; set; }

        [DataMember]
        [DisplayName("Núm. Puesto")]
        public string CodPuesto { get; set; }

        [DataMember]
        [DisplayName("Núm. Puesto")]
        public string CodPuesto2 { get; set; }

        [DataMember]
        [DisplayName("Código Categoría")]
        public string Categoria { get; set; }

        [DataMember]
        [DisplayName("Código Categoría")]
        public string Categoria2 { get; set; }

        [DataMember]
        [DisplayName("Código Clase")]
        public string CodClase { get; set; }

        [DataMember]
        [DisplayName("Clase")]
        public string DesClase { get; set; }

        [DataMember]
        [DisplayName("Código Clase")]
        public string CodClase2 { get; set; }

        [DataMember]
        [DisplayName("Clase")]
        public string DesClase2 { get; set; }

        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Salario Base")]
        public string MtoSalarioBase { get; set; }

        [DataMember]
        [DisplayName("Aumentos Anuales")]
        public string MtoAumentosAnuales { get; set; }

        [DataMember]
        [DisplayName("Recargo de Funciones")]
        public string MtoRecargo { get; set; }

        [DataMember]
        [DisplayName("Monto Grado")]
        public string MtoGradoGrupo { get; set; }

        [DataMember]
        [DisplayName("Monto Prohib./Dedic.")]
        public string MtoProhibicion { get; set; }

        [DataMember]
        [DisplayName("Otros Sobresueldos")]
        public string MtoOtros { get; set; }

        [DataMember]
        [DisplayName("Total")]
        public decimal MtoTotal { get; set; }


        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Salario Base")]
        public string MtoSalarioBase2 { get; set; }

        [DataMember]
        [DisplayName("Aumentos Anuales")]
        public string MtoAumentosAnuales2 { get; set; }

        [DataMember]
        [DisplayName("Recargo de Funciones")]
        public string MtoRecargo2 { get; set; }

        [DataMember]
        [DisplayName("Monto Grado")]
        public string MtoGradoGrupo2 { get; set; }

        [DataMember]
        [DisplayName("Monto Prohib./Dedic.")]
        public string MtoProhibicion2 { get; set; }

        [DataMember]
        [DisplayName("Otros Sobresueldos")]
        public string MtoOtros2 { get; set; }

        [DataMember]
        [DisplayName("Total")]
        public decimal MtoTotal2 { get; set; }

        [DataMember]
        [DisplayName("Disfrutado")]
        public int Disfrutado { get; set; }

        [DataMember]
        [DisplayName("Disfrutado")]
        public int Disfrutado2 { get; set; }

        [DataMember]
        [DisplayName("Autorizado")]
        public int Autorizado { get; set; }

        [DataMember]
        [DisplayName("Autorizado")]
        public int Autorizado2 { get; set; }
       
    }
}