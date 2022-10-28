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
using System.Collections.Generic;

namespace SIRH.Web.Reports.Historial
{
    public class HistorialRptDat
    {
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string Clasificacion { get; set; }
        public string UnidaddondeTrabaja { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public string Perido { get; set; }
        public string Expediente { get; set; }
        public string Calificacion { get; set; }
        public string Estado { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static HistorialRptDat GenerarDatosReporte(BusquedaHistorialCalificacionVM dato, string filtros)
        {
            return new HistorialRptDat
            {
                CedulaFuncionario = dato.Funcionario.Cedula,
                NombreFuncionario = dato.Funcionario.Nombre.TrimEnd() + " " +
                                dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Clasificacion = dato.DetallePuesto.Clase.DesClase,
                Expediente = dato.Expediente.NumeroExpediente.ToString(),
                UnidaddondeTrabaja = dato.Puesto.UbicacionAdministrativa.Division.NomDivision + "-" + dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion + "-" + dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento +
                "-" + dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion,
                Departamento= dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento,
                Puesto = dato.Puesto.CodPuesto.ToString(),
                Calificacion = dato.DetalleCNombramiento.CalificacionDTO.DesCalificacion,
                Perido = dato.DetalleCNombramiento.Periodo.IdEntidad.ToString(),
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()
                //Filtros = filtros
            };
        }
    }
}

