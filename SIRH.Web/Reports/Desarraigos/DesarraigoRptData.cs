using SIRH.Web.ViewModels;

namespace SIRH.Web.Reports.Desarraigos
{
    public class DesarraigoRptData {
        public string Funcionario { get; set; }
        public string CodigoDesarraigo { get; set; }
        public string MontoDesarraigo { get; set; }
        public string FechaEmision { get; set; }
        public string FechaVencimiento { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public string LugarTrabajo { get; set; }
        public string LugarContrato { get; set; }
        public string OcupacionReal { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        internal static DesarraigoRptData GenerarDatosReporte(FormularioDesarraigoVM dato,string filtros){
            return new DesarraigoRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                CodigoDesarraigo = dato.Desarraigo.CodigoDesarraigo,
                MontoDesarraigo = dato.Desarraigo.MontoDesarraigo.ToString("₡ #,#.00#;(#,#.00#)"),
                FechaEmision =  dato.Desarraigo.FechaInicio.ToShortDateString(),
                FechaVencimiento = dato.Desarraigo.FechaFin.ToShortDateString(),
                Estado = dato.Desarraigo.EstadoDesarraigo.NomEstadoDesarraigo,
                Observaciones = dato.Desarraigo.ObservacionesDesarraigo!=null? dato.Desarraigo.ObservacionesDesarraigo:"No hay observaciones",
                LugarTrabajo = dato.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia+", "
                               +dato.UbicacionTrabajo.Distrito.Canton.NomCanton+", "+dato.UbicacionTrabajo.Distrito.NomDistrito,
                LugarContrato = dato.UbicacionContrato.Distrito.Canton.Provincia.NomProvincia + ", " + dato.UbicacionContrato.Distrito.Canton.NomCanton + ", "
                               +dato.UbicacionContrato.Distrito.NomDistrito,
                OcupacionReal = dato.DetallePuesto.OcupacionReal.DesOcupacionReal,
                Autor =  System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros
            };
        }
    }
}