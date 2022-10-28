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
    public class DeduccionPI1105RptData
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

        public string Motivo { get; set; }
        public string Rige { get; set; }
        public string Vence { get; set; }
        public string Ndias { get; set; }
        public string MontB { get; set; }
        public string MontR { get; set; }
        public string Total { get; set; }
        public string NAccion { get; set; }





        internal static DeduccionPI1105RptData GenerarDatosReporte(FormularioGastoTransporteVM dato, string filtros, int i)
        {
            return new DeduccionPI1105RptData
            {
                NombreFormulario = "PI-1105",
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
                ProgramaP = dato.Puesto.UbicacionAdministrativa.Presupuesto.IdUnidadPresupuestaria,//Pendiente de agregar a la bd
                Area = dato.Puesto.UbicacionAdministrativa.Presupuesto.Area,//Pendiente de agregar a la bd
                Actividad = dato.Puesto.UbicacionAdministrativa.Presupuesto.Actividad,//Pendiente de agregar a la bd
                Motivo = dato.Deduccion[i].DesMotivoDTO,
                Rige = dato.Deduccion[i].FecRigeDTO,
                Vence = dato.Deduccion[i].FecVenceDTO,
                Ndias = dato.Deduccion[i].NumNoDiaDTO.ToString(),
                MontB = dato.Deduccion[i].MontMontoBajarDTO,
                MontR = dato.Deduccion[i].MontMontoRebajarDTO,
                Total = dato.Deduccion[i].TotRebajarDTO,
                NAccion = dato.Deduccion[i].NumSolicitudAccionPDTO.ToString(),


            };
        }
    }
}
