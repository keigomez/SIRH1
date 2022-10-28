using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using SIRH.DTO;

namespace SIRH.DeamonServicios
{
    public class ReporteEncabezadoSimpleRptData{

        public string FechaI { get; set; }
        public string FechaF { get; set; }
        public string Departamento { set; get; }

        internal static ReporteEncabezadoSimpleRptData GenerarDatosReporteAsistencia(DateTime FechaI, DateTime FechaF, CUbicacionAdministrativaDTO ubicacion)
        {
            return new ReporteEncabezadoSimpleRptData{
                FechaI = FechaI.ToShortDateString(),
                FechaF = FechaF.ToShortDateString(),
                Departamento = ubicacion.Departamento != null ? ubicacion.Departamento.NomDepartamento : "INDEF"
            };
        }
    }
}