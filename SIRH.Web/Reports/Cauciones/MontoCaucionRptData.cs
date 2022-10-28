using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;

namespace SIRH.Web.Reports.Cauciones
{
    public class MontoCaucionRptData
    {
        public string Nivel { get; set; }
        public string MontoColones { get; set; }
        public string Descripcion { get; set; }
        public string FechaRige { get; set; }
        public string JustificacionMonto { get; set; }
        public string FechaVencimiento { get; set; }
        public string JustificacionInactivo { get; set; }
        public string Estado { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static MontoCaucionRptData GenerarDatosReporteMontoCaucion(CMontoCaucionDTO dato, string filtros)
        {
            return new MontoCaucionRptData
            {
                Nivel = dato.Nivel,
                MontoColones = "₡ " + dato.MontoColones.ToString("#,#.00#;(#,#.00#)"),
                Descripcion = dato.Descripcion,
                FechaRige = dato.FechaRige.ToShortDateString(),
                JustificacionMonto = dato.Justificacion,
                FechaVencimiento = dato.FechaInactiva.Year > 1 ? dato.FechaInactiva.ToShortDateString() : "",
                Estado = dato.DetalleEstadoMonto,
                JustificacionInactivo = dato.JustificacionInactiva,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros
            };
        }
    }
}
