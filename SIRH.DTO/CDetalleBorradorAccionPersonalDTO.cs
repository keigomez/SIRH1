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
    public class CDetalleBorradorAccionPersonalDTO : CBaseDTO
    {
        [DataMember]
        public CBorradorAccionPersonalDTO Borrador { get; set; }
        [DataMember]
        public CTipoAccionPersonalDTO TipoAccion { get; set; }
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }
        [DataMember]
        public CProgramaDTO Programa { get; set; }
        [DataMember]
        public CSeccionDTO Seccion { get; set; }

        [DataMember]
        [DisplayName("Fecha Rige")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[Required(ErrorMessage = "La fecha de rige es requerida.")]
        public DateTime FecRige { get; set; }
        
        [DataMember]
        [DisplayName("Fecha Vence")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[Required(ErrorMessage = "La fecha de vence es requerida.")]
        public DateTime FecVence { get; set; }
        [DataMember]
        [DisplayName("Fecha Rige Integra")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[Required(ErrorMessage = "La fecha de rige integra es requerida.")]
        public DateTime FecRigeIntegra { get; set; }
        [DataMember]
        [DisplayName("Fecha Vence Integra")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[Required(ErrorMessage = "La fecha de vence integra es requerida.")]
        public DateTime FecVenceIntegra { get; set; }

        [DataMember]
        [DisplayName("Código Clase")]
        public int CodClase { get; set; }

        [DataMember]
        [DisplayName("Núm. Puesto")]
        public string CodPuesto { get; set; }

        [DataMember]
        [DisplayName("Número de Horas")]
        public int NumHoras { get; set; }

        [DataMember]
        [DisplayName("Disfrutado")]
        public int Disfrutado { get; set; }

        [DataMember]
        [DisplayName("Autorizado")]
        public int Autorizado { get; set; }

        [DataMember]
        [DisplayName("Categoría")]
        public int Categoria { get; set; }

        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [DisplayName("Salario Base")]
        public decimal MtoSalarioBase { get; set; }

        [DataMember]
        [DisplayName("Monto Anualidad")]
        public decimal MtoAnual { get; set; }

        [DataMember]
        [DisplayName("Aumentos Anuales")]
        public decimal MtoAumentosAnuales { get; set; }

        [DataMember]
        [DisplayName("Recargo de Funciones")]
        public decimal MtoRecargo { get; set; }

        [DataMember]
        [DisplayName("Valor Punto")]
        public decimal MtoPunto { get; set; }

        [DataMember]
        [DisplayName("Grado o Grupo")]
        public decimal NumGradoGrupo { get; set; }

        [DataMember]
        [DisplayName("Monto Grado")]
        public decimal MtoGradoGrupo { get; set; }

        [DataMember]
        [DisplayName("Prohibición")]
        public decimal PorProhibicion { get; set; }

        [DataMember]
        [DisplayName("Monto Prohib./Dedic.")]
        public decimal MtoProhibicion { get; set; }

        [DataMember]
        [DisplayName("Otros Sobresueldos")]
        public decimal MtoOtros { get; set; }
    }
}