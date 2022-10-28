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
using System.Globalization;

namespace SIRH.Web.Reports.ViaticoCorrido
{
    public class DeduccionPI1104RptData
    {
        public string NombreFormulario { get; set; }
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string Apell1Funcionario { get; set; }
        public string Apell2Funcionario { get; set; }
        public string NombreCompleto { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string UbicacionReal { get; set; }
        public string Tel { get; set; }
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
        public string CabinasYorN { get; set; }
        public string MesDeduccion { get; set; }

        internal static DeduccionPI1104RptData GenerarDatosReporte(FormularioViaticoCorridoVM dato, string filtros, int i)
        {
            DateTime fecha = dato.Deduccion[i].MovimientoViaticoCorridoDTO.FecMovimientoDTO;
            var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();

            string cabinas = "";

            if (dato.Deduccion[i].MovimientoViaticoCorridoDTO.ViaticoCorridoDTO.PernocteDTO != null)
                cabinas = "Sí";
            else
                cabinas = "No";

            return new DeduccionPI1104RptData
            {
                NombreFormulario = "PI-1103",
                CedulaFuncionario = dato.Funcionario.Cedula,
                Apell1Funcionario = dato.Funcionario.PrimerApellido.TrimEnd(),
                Apell2Funcionario = dato.Funcionario.SegundoApellido.TrimEnd(),
                NombreFuncionario = dato.Funcionario.Nombre.TrimEnd(),
                NombreCompleto = dato.Funcionario.Nombre.TrimEnd() + " " + dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(), 
                Provincia = dato.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia,
                Canton = dato.UbicacionTrabajo.Distrito.Canton.NomCanton,
                Distrito = dato.UbicacionTrabajo.Distrito.NomDistrito,
                Division = dato.Puesto.UbicacionAdministrativa.Division.NomDivision,
                Direccion = dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd(),
                Departamento = dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd(),
                UbicacionReal = dato.DetallePuesto.OcupacionReal.DesOcupacionReal,
                Tel = "Pendiente",
                ProgramaP = dato.Puesto.UbicacionAdministrativa.Presupuesto.IdUnidadPresupuestaria,
                Area = dato.Puesto.UbicacionAdministrativa.Presupuesto.Area,
                Actividad = dato.Puesto.UbicacionAdministrativa.Presupuesto.Actividad,
                Motivo = dato.Deduccion[i].DesMotivoDTO,
                Rige = dato.Deduccion[i].FecRigeDTO,
                Vence = dato.Deduccion[i].FecVenceDTO,
                Ndias = dato.Deduccion[i].NumNoDiaDTO.ToString(),
                MontB = Convert.ToDecimal(dato.Deduccion[i].MontMontoBajarDTO).ToString("#,#.00#;(#,#.00#)"),
                MontR = Convert.ToDecimal(dato.Deduccion[i].MontMontoRebajarDTO).ToString("#,#.00#;(#,#.00#)"),
                Total = Convert.ToDecimal(dato.Deduccion[i].TotRebajarDTO).ToString("#,#.00#;(#,#.00#)"),
                NAccion = dato.Deduccion[i].NumSolicitudAccionPDTO,
                CabinasYorN = cabinas,
                MesDeduccion = mes
            };
        }
    }
}
