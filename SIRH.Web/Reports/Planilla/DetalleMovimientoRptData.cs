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

namespace SIRH.Web.Reports.Planilla
{
    public class DetalleMovimientoRptData
    {
        public string Anio { get; set; }
        public string Programa { get; set; }
        public string ObjetoGasto { get; set; }
        public string TipoMovimiento { get; set; }
        public string MontoComponente { get; set; }
        public string Detalle { get; set; }
        public string Filtros { get; set; }

        internal static DetalleMovimientoRptData GenerarDatosReporte(FormularioPlanillaVM dato, string filtros)
        {
            return new DetalleMovimientoRptData
            {
                Anio = dato.ComponentePresupuestario.AnioPresupuesto,
                Programa = dato.ComponentePresupuestario.Programa.IdEntidad.ToString() + "-" + dato.ComponentePresupuestario.Programa.DesPrograma,
                ObjetoGasto = dato.ComponentePresupuestario.ObjetoGasto.SubPartida.Partida.CodPartida + " " +
                                dato.ComponentePresupuestario.ObjetoGasto.SubPartida.CodSubPartida + " " +
                                dato.ComponentePresupuestario.ObjetoGasto.CodObjGasto + "-" +
                                dato.ComponentePresupuestario.ObjetoGasto.DesObjGasto,
                TipoMovimiento = dato.ComponentePresupuestario.TipoMovimiento.DesMovimientoPresupuesto,
                MontoComponente = dato.ComponentePresupuestario.MontoComponente.ToString(),
                Detalle = dato.ComponentePresupuestario.Detalle
            };
        }
    }
}