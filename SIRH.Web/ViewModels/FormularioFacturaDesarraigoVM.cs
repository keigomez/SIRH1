using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FormularioFacturaDesarraigoVM
    {
        [DisplayName("Fecha de Facturación")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFacturacion { get; set; }
        [Required]
        [DisplayName("Código de la Factura")]
        public string CodigoFactura { get; set; }
        [DisplayName("Emisor de la Factura")]
        public string EmisorFactura { get; set; }
        [DisplayName("Concepto de la Factura")]
        public string ConceptoFactura { get; set; }
        [DisplayName("Monto de la Factura")]
        public decimal MontoFactura { get; set; }
    }
}
