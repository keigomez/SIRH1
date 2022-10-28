using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class FormularioContratoArrendamientoVM
    {
        [Required]
        [DisplayName("Código del Contrato")]
        public string CodigoContrato { get; set; }
        [DisplayName("Fecha de Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInicioContrato { get; set; }
        [DisplayName("Fecha de Vencimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFinalContrato { get; set; }
        [DisplayName("Emisor del Contrato")]
        public string EmisorContrato { get; set; }
        [DisplayName("Monto del Contrato")]
        public decimal MontoContrato { get; set; }
    }
}
