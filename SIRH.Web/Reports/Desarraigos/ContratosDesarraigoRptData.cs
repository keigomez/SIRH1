using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.Reports.Desarraigos
{
    public class ContratosDesarraigoRptData
    {
        public string Codigo { set; get; }
        public string Emisor { set; get; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string Monto { get; set; }

        internal static ContratosDesarraigoRptData GenerarDatosReporte(CContratoArrendamientoDTO contrato)
        {
            return new ContratosDesarraigoRptData
            {
                Codigo = contrato.CodigoContratoArrendamiento,
                Emisor = contrato.EmisorContrato,
                FechaInicio = contrato.FechaInicio.ToShortDateString(),
                FechaFinal = contrato.FechaFin.ToShortDateString(),
                Monto = contrato.MontoContrato.ToString("₡ #,#.00#;(#,#.00#)")
            };
        }
    }
}
