using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.Reports.Desarraigos
{
    public class DesarraigoFacturaRptData
    {
        public string Codigo { set; get; }
        public string Emisor { set; get; }
        public string Monto { set; get; }
        public string Fecha { set; get; }

        internal static DesarraigoFacturaRptData GenerarDatosReporte(CFacturaDesarraigoDTO factura) {
            return new DesarraigoFacturaRptData
            {
                Codigo = factura.CodigoFactura,
                Emisor = factura.Emisor,
                Monto = factura.MontoFactura.ToString("₡ #,#.00#;(#,#.00#)"),
                Fecha = factura.FechaFacturacion.ToShortDateString()
            };
        }




    }
}
