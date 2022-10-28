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

namespace SIRH.Web.Reports.BorradorAccionPersonal
{
    public class BorradorRptData
    {
        public string Funcionario { get; set; }
        public string Codigo { get; set; }
        public string TipoAccion { get; set; }
        public string FechaRige { get; set; }
        public string FechaVence { get; set; }
        public string FechaIntegraRige { get; set; }
        public string FechaIntegraVence { get; set; }
        public string Estado { get; set; }
        public string Justificacion { get; set; }
        public string Usuario { get; set; }
        public string NumOficio { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        public string MonAnualidad { get; set; }

        public string Programa { get; set; }
        public string Seccion { get; set; }
        public string Clase { get; set; }
        public string Categoria { get; set; }
        public string CodPuesto { get; set; }
        public string Disfrutado { get; set; }
        public string Autorizado { get; set; }
        public string PorProh { get; set; }
        public string NumGrado { get; set; }
        public string MonSalBase { get; set; }
        public string MonAnual { get; set; }
        public string MonRecargo { get; set; }
        public string MonGrado { get; set; }
        public string MonProh { get; set; }
        public string MonOtros { get; set; }
        public string MonTotal { get; set; }


        public string ProgramaNuevo { get; set; }
        public string SeccionNuevo { get; set; }
        public string ClaseNuevo { get; set; }
        public string CategoriaNuevo { get; set; }
        public string CodPuestoNuevo { get; set; }
        public string DisfrutadoNuevo { get; set; }
        public string AutorizadoNuevo { get; set; }
        public string PorProhNuevo { get; set; }
        public string MonSalBaseNuevo { get; set; }
        public string MonAnualNuevo { get; set; }
        public string MonRecargoNuevo { get; set; }
        public string NumGradoNuevo { get; set; }
        public string MonGradoNuevo { get; set; }
        public string MonProhNuevo { get; set; }
        public string MonOtrosNuevo { get; set; }
        public string MonTotalNuevo { get; set; }


        internal static BorradorRptData GenerarDatosReporte(FormularioBorradorAccionPersonalVM dato, string filtros)
        {
            return new BorradorRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                              dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                Codigo = dato.Borrador.IdEntidad.ToString(),
                TipoAccion = dato.Detalle.TipoAccion.DesTipoAccion,
                FechaRige = dato.Detalle.FecRige.ToShortDateString(),
                FechaVence = dato.Detalle.FecVence.ToShortDateString(),
                FechaIntegraRige = dato.Detalle.FecRigeIntegra.ToShortDateString(),
                FechaIntegraVence = dato.Detalle.FecVenceIntegra.ToShortDateString(),
                Estado = dato.Borrador.EstadoBorrador.DesEstadoBorrador,
                Usuario = dato.Asignado.Nombre + " " + dato.Asignado.PrimerApellido + " " + dato.Asignado.SegundoApellido,
                NumOficio = dato.Borrador.NumOficio,
                Justificacion = dato.Borrador.ObsJustificacion,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,
            };
        }


        internal static BorradorRptData GenerarDatosReporteDetalle(FormularioBorradorAccionPersonalVM dato, string filtros)
        {
            return new BorradorRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                              dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                Codigo = dato.Borrador.IdEntidad.ToString(),
                TipoAccion = dato.Detalle.TipoAccion.DesTipoAccion,
                FechaRige = dato.Detalle.FecRige.ToShortDateString(),
                FechaVence = dato.Detalle.FecVence.ToShortDateString(),
                FechaIntegraRige = dato.Detalle.FecRigeIntegra.ToShortDateString(),
                FechaIntegraVence = dato.Detalle.FecVenceIntegra.ToShortDateString(),
                Estado = dato.Borrador.EstadoBorrador.DesEstadoBorrador,
                Usuario = dato.Asignado.Nombre + " " + dato.Asignado.PrimerApellido + " " + dato.Asignado.SegundoApellido,
                NumOficio = dato.Borrador.NumOficio,
                Justificacion = dato.Borrador.ObsJustificacion,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,

                Programa = dato.Puesto.UbicacionAdministrativa.Presupuesto.Programa.IdEntidad + " " + dato.Puesto.UbicacionAdministrativa.Presupuesto.Programa.DesPrograma,
                Seccion = dato.Puesto.UbicacionAdministrativa.Seccion.IdEntidad + " " + dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion,
                Clase = dato.DetallePuesto.Clase.IdEntidad + " " + dato.DetallePuesto.Clase.DesClase,
                Categoria = dato.DetallePuesto.EscalaSalarial.CategoriaEscala.ToString(),
                CodPuesto = dato.Puesto.CodPuesto,
                Disfrutado = dato.Contrato.NumeroAnualidades.ToString(),
                Autorizado = dato.Contrato.NumeroAnualidades.ToString(),
                MonAnualidad = dato.DetallePuesto.EscalaSalarial.MontoAumentoAnual.ToString("#,#.00#;(#,#.00#)"),
                PorProh = dato.DetallePuesto.PorProhibicion.ToString() + "%",
                MonSalBase = dato.DetallePuesto.EscalaSalarial.SalarioBase.ToString("#,#.00#;(#,#.00#)"),
                MonAnual = (dato.Contrato.NumeroAnualidades * dato.DetallePuesto.EscalaSalarial.MontoAumentoAnual).ToString("#,#.00#;(#,#.00#)"),
                MonRecargo = "0,00",
                NumGrado = dato.PuntosCarrera.ToString(),
                MonGrado = (dato.PuntosCarrera * dato.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera).ToString("#,#.00#;(#,#.00#)"),
                MonProh = (dato.DetallePuesto.EscalaSalarial.SalarioBase * dato.DetallePuesto.PorProhibicion / 100).ToString("#,#.00#;(#,#.00#)"),
                MonOtros = "0,00",

                MonTotal = (dato.DetallePuesto.EscalaSalarial.SalarioBase +
                           (dato.Contrato.NumeroAnualidades * dato.DetallePuesto.EscalaSalarial.MontoAumentoAnual) +
                           (dato.PuntosCarrera * dato.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera) +
                           (dato.DetallePuesto.EscalaSalarial.SalarioBase * dato.DetallePuesto.PorProhibicion / 100)).ToString("#,#.00#;(#,#.00#)"),

                ProgramaNuevo = (dato.Detalle.Programa != null) ? dato.Detalle.Programa.IdEntidad + " " + dato.Detalle.Programa.DesPrograma : "",
                SeccionNuevo = (dato.Detalle.Programa != null) ? dato.Detalle.Seccion.IdEntidad + " " + dato.Detalle.Seccion.NomSeccion : "",
                ClaseNuevo = dato.Detalle.CodClase.ToString() + " " + dato.Clase.DesClase,
                CategoriaNuevo = dato.Detalle.Categoria.ToString(),
                CodPuestoNuevo = dato.Detalle.CodPuesto,
                DisfrutadoNuevo = dato.Detalle.Disfrutado.ToString(),
                AutorizadoNuevo = dato.Detalle.Autorizado.ToString(),
                PorProhNuevo = dato.Detalle.PorProhibicion.ToString() + "%",
                MonSalBaseNuevo = dato.Detalle.MtoSalarioBase.ToString("#,#.00#;(#,#.00#)"),
                MonAnualNuevo = dato.Detalle.MtoAumentosAnuales.ToString("#,#.00#;(#,#.00#)"),
                MonRecargoNuevo = dato.Detalle.MtoRecargo.ToString("#,#.00#;(#,#.00#)"),
                NumGradoNuevo = dato.Detalle.NumGradoGrupo.ToString(),
                MonGradoNuevo = (dato.Detalle.NumGradoGrupo * dato.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera).ToString("#,#.00#;(#,#.00#)"),
                MonProhNuevo = (dato.Detalle.MtoSalarioBase * dato.Detalle.PorProhibicion / 100).ToString("#,#.00#;(#,#.00#)"),
                MonOtrosNuevo = dato.Detalle.MtoOtros.ToString("#,#.00#;(#,#.00#)"),

                MonTotalNuevo = (dato.Detalle.MtoSalarioBase +
                                dato.Detalle.MtoAumentosAnuales +
                                dato.Detalle.MtoRecargo +
                                (dato.Detalle.MtoSalarioBase * dato.Detalle.PorProhibicion / 100) +
                                (dato.Detalle.NumGradoGrupo * dato.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera) +
                                dato.Detalle.MtoOtros).ToString("#,#.00#;(#,#.00#)")
            };
        }
    }
}