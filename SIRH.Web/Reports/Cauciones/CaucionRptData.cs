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
using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Cauciones
{
    public class CaucionRptData
    {
        public string Funcionario { get; set; }
        public string NumeroPoliza { get; set; }
        public string EntidadSeguros { get; set; }
        public string MontoCaucion { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static CaucionRptData GenerarDatosReporte(FormularioCaucionVM dato, string filtros)
        {
            return new CaucionRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                NumeroPoliza = dato.Caucion.NumeroPoliza,
                EntidadSeguros = dato.EntidadSeguros.NombreEntidad,
                MontoCaucion = dato.MontoCaucion.Nivel + " - " + dato.MontoCaucion.Descripcion + " - ₡ " + 
                                dato.MontoCaucion.MontoColones.ToString("#,#.00#;(#,#.00#)"),
                FechaEmision = dato.Caucion.FechaEmision.ToShortDateString(),
                FechaVencimiento = dato.Caucion.FechaVence.ToShortDateString(),
                Estado = dato.Caucion.DetalleEstadoPoliza,
                Observaciones = dato.Caucion.ObservacionesPoliza,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros
            };
        }
    }
}
