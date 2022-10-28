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
using SIRH.DTO;

namespace SIRH.Web.Reports.RelojMarcador
{
    public class ReporteEncabezadoSimpleRptData{

        public string FechaI { get; set; }
        public string FechaF { get; set; }
        public string Departamento { set; get; }

        internal static ReporteEncabezadoSimpleRptData GenerarDatosReporteAsistencia(FormularioReporteMarcas modelo,CUbicacionAdministrativaDTO ubicacion)
        {
            return new ReporteEncabezadoSimpleRptData{
                FechaI = modelo.FechaI.ToShortDateString(),
                FechaF = modelo.FechaF.ToShortDateString(),
                Departamento = ubicacion.Departamento != null ? ubicacion.Departamento.NomDepartamento : "INDEF"
            };
        }
    }
}