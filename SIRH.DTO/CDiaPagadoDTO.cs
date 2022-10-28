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
    public class CDiaPagadoDTO : CBaseDTO
    {
        [DataMember]
        public CCatalogoDiaDTO CatalogoDia { get; set; }
        [DataMember]
        public CPagoFeriadoDTO PagoFeriado { get; set; }
        [DataMember]
        [DisplayName("Cantidad horas")]
        [Required(ErrorMessage = "La cantidad de horas son requeridas.")]
        public int CantidadHoras { get; set; }
        [DataMember]
        [DisplayName("Salario hora")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoSalarioHora { get; set; }
        [DataMember]
        [DisplayName("Monto total")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal MontoTotal { get; set; }
        [DataMember]
        [DisplayName("Año")]
        public string Anio { get; set; }
    }
}