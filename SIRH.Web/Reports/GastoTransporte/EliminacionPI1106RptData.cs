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

namespace SIRH.Web.Reports.GastoTransporte
{
    public class EliminacionPI1106RptData
    {
        public string NombreFormulario { get; set; }
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string Apell1Funcionario { get; set; }
        public string Apell2Funcionario { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string UbicacionReal { get; set; }
        public string Tel { get; set; }
        public string CodPresupuestario { get; set; }
        public string ProgramaP { get; set; }
        public string Area { get; set; }
        public string Actividad { get; set; }


        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string Carta { get; set; }
        public string Justificacion { get; set; }
        public string Observacion { get; set; }



        internal static EliminacionPI1106RptData GenerarDatosReporte(FormularioGastoTransporteVM dato, string filtros)
        {
            return new EliminacionPI1106RptData
            {
                NombreFormulario = "PI-1106",
                CedulaFuncionario = dato.Funcionario.Cedula,
                Apell1Funcionario = dato.Funcionario.PrimerApellido,
                Apell2Funcionario = dato.Funcionario.SegundoApellido,
                NombreFuncionario = dato.Funcionario.Nombre,
                Provincia = dato.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia,
                Canton = dato.UbicacionTrabajo.Distrito.Canton.NomCanton,
                Distrito = dato.UbicacionTrabajo.Distrito.NomDistrito,
                Division = dato.Puesto.UbicacionAdministrativa.Division.NomDivision,
                Direccion = dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion,
                Departamento = dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento,
                UbicacionReal = dato.DetallePuesto.OcupacionReal.DesOcupacionReal,
                Tel = "Pendiente",
                CodPresupuestario = dato.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto,
                ProgramaP = dato.Puesto.UbicacionAdministrativa.Presupuesto.IdUnidadPresupuestaria,
                Area = dato.Puesto.UbicacionAdministrativa.Presupuesto.Area,
                Actividad = dato.Puesto.UbicacionAdministrativa.Presupuesto.Actividad,
                FechaInicio = dato.Eliminacion.FecInicioDTO.ToShortDateString(),
                FechaFinal = dato.Eliminacion.FecFinDTO.ToShortDateString(),
                Carta = dato.NumCartaPresentacion,
                Justificacion = dato.Eliminacion.ObsJustificacionDTO,
                Observacion = dato.MovimientoGastoTransporte.ObsObservacionesDTO

            };
        }
    }
}
