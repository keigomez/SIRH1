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
    public class ReporteEncabezadoRptData
    {
        public string Funcionario { get; set; }
        public string TipoJornada { get; set; }
        public string FechaI { get; set; }
        public string FechaF { get; set; }
        public string EstadoMarcacion { set; get; }
        public string Departamento { set; get; }
        public string Codigoempleado { set; get; }
        public string Autor { get; set; }
        public string Filtros { get; set; }
        public string AusenciasTotal { get; set; }
        public string AusenciasMedia { get; set; }
        public string Tardia5 { get; set; }
        public string Tardia20 { get; set; }
        public string InconsistenciasTotal { get; set; }
        public string LaboradoTotal { get; set; }

        internal static ReporteEncabezadoRptData GenerarDatosReporteAsistencia(FormularioReporteMarcas modelo,CFuncionarioDTO fun, CTipoJornadaDTO jornada,
                                                                               CDetalleContratacionDTO detalle, CUbicacionAdministrativaDTO ubicacion, 
                                                                                string filtros, int ausT,int ausM,int tar5,int tar20, string lab)
        {

            return new ReporteEncabezadoRptData
            {
                Funcionario = fun.Cedula+" - "+fun.Nombre + " " + fun.PrimerApellido + " " + fun.SegundoApellido,
                TipoJornada = jornada != null?jornada.InicioJornada + " - " + jornada.FinJornada : "INDEF",
                Codigoempleado = fun.Mensaje == null ? "INDEF" : fun.Mensaje,
                FechaI = modelo.FechaI.ToShortDateString(),
                FechaF = modelo.FechaF.ToShortDateString(),
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                EstadoMarcacion = detalle.EstadoMarcacion == false?"NO":(detalle.EstadoMarcacion == true?"SI":"INDEF"),
                Departamento = ubicacion != null && ubicacion.Departamento != null ? ubicacion.Departamento.NomDepartamento: "INDEF",
                Filtros = filtros,
                AusenciasTotal = ausT.ToString(),
                AusenciasMedia = ausM.ToString(),
                Tardia5 = tar5.ToString(),
                Tardia20 = tar20.ToString(),
                InconsistenciasTotal = jornada != null?(tar5+tar20+ausM).ToString():"-1",
                LaboradoTotal = lab
            };
        }
    }
}