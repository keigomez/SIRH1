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


namespace SIRH.Web.Reports.PagoFeriado
{
    public class PagoFeriadoRptData
    {
        //Datos del funcionario
        public string Funcionario { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }

        //Datos del trámite
        public string Observaciones { get; set; }
        public string Autor { get; set; }
        public string Consecutivo { get; set; }
        public string Filtros { get; set; }
        public string SalarioBruto { get; set; }
        public string EstadoTramite { get; set; }
        public string FechaTramite { get; set; }

        //Días pagados
        public string DiaPagado { get; set; }
        public string HorasLaboradas { get; set; }
        public string SalarioHoras { get; set; }
        public string MontoDia { get; set; }

        //Subtotales
        public string PorcentajeSalarioEscolar { get; set; }
        public string MontoSalarioEscolar { get; set; }
        public string SubtotalDias { get; set; }

        //Diferencias Efectuadas Obrero
        public string Diferencias { get; set; }
        public string DeduccionEfectuada { get; set; }
        public string PorcentajeDeduccion { get; set; }
        public string MontoDeduccion { get; set; }
        public string TotalDeduccion { get; set; }

        //Diferencias Efectuadas Patronal
        public string DeduccionEfectuadaPatronal { get; set; }
        public string PorcentajeDeduccionPatronal { get; set; }
        public string MontoDeduccionPatronal { get; set; }
        public string TotalDeduccionPatronal { get; set; }
        public string CodigoDeduccionPatronal { get; set; }

        //Totales
        public string DiferenciaLiquida { get; set; }
        public string AguinaldoProporcional { get; set; }
        public string MontoTotal { get; set; }

        internal static PagoFeriadoRptData GenerarDatosReporte(FormularioPagoFeriadoVM dato, string filtros, string tipo)
        {
            if (tipo.Equals("DetalleSearch"))
            {
                return new PagoFeriadoRptData
                {
                    Consecutivo = dato.PagoFeriados.IdEntidad.ToString(),
                    Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                    dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                    Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    Filtros = filtros,
                    FechaTramite = dato.PagoExtraordinario.FechaTramite.ToString(),
                    EstadoTramite = dato.EstadoTramite.DescripcionEstado,
                    Observaciones = dato.PagoFeriados.ObsevacionTramite,
                    MontoSalarioEscolar = "₡ " + dato.PagoFeriados.MontoSalarioEscolar.ToString("#,#.00#;(#,#.00#)"),
                    SubtotalDias = "₡ " + dato.PagoFeriados.MontoSubtotalDia.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccion = "₡ " + dato.PagoFeriados.MontoDeduccionObrero.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccionPatronal = "₡ " + dato.PagoFeriados.MontoDeduccionPatronal.ToString("#,#.00#;(#,#.00#)"),
                    DiferenciaLiquida = "₡ " + dato.PagoFeriados.MontoDiferenciaLiquida.ToString("#,#.00#;(#,#.00#)"),
                    AguinaldoProporcional = "₡ " + dato.PagoFeriados.MontoAguinaldoProporcional.ToString("#,#.00#;(#,#.00#)"),
                    MontoTotal = "₡ " + dato.PagoFeriados.MontoDeTotal.ToString("#,#.00#;(#,#.00#)"),

                    DiaPagado = dato.CatalogoDiaAuxiliar.DescripcionDia,
                    HorasLaboradas = dato.DiaPagadoAuxiliar.CantidadHoras.ToString(),
                    SalarioHoras = "₡ " + dato.DiaPagadoAuxiliar.MontoSalarioHora.ToString("#,#.00#;(#,#.00#)"),
                    MontoDia = "₡ " + dato.DiaPagadoAuxiliar.MontoTotal.ToString("#,#.00#;(#,#.00#)"),
                    SalarioBruto = "₡ " + dato.PagoFeriados.MontoSalaroBruto.ToString("#,#.00#;(#,#.00#)"),
                    PorcentajeSalarioEscolar = dato.SalEscolarEfectuado.PorcentajeEfectuado.ToString("#.00#") + "%"

                };
            }
            else if (tipo.Equals("DetalleGuardar"))
            {
                return new PagoFeriadoRptData
                {
                    Consecutivo = dato.PagoFeriados.IdEntidad.ToString(),
                    Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                    dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                    Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    Filtros = filtros,
                    FechaTramite = dato.PagoExtraordinario.FechaTramite.ToString(),
                    Observaciones = dato.PagoFeriados.ObsevacionTramite,
                    MontoSalarioEscolar = "₡ " + dato.PagoFeriados.MontoSalarioEscolar.ToString("#,#.00#;(#,#.00#)"),
                    SubtotalDias = "₡ " + dato.PagoFeriados.MontoSubtotalDia.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccion = "₡ " + dato.PagoFeriados.MontoDeduccionObrero.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccionPatronal = "₡ " + dato.PagoFeriados.MontoDeduccionPatronal.ToString("#,#.00#;(#,#.00#)"),
                    DiferenciaLiquida = "₡ " + dato.PagoFeriados.MontoDiferenciaLiquida.ToString("#,#.00#;(#,#.00#)"),
                    AguinaldoProporcional = "₡ " + dato.PagoFeriados.MontoAguinaldoProporcional.ToString("#,#.00#;(#,#.00#)"),
                    MontoTotal = "₡ " + dato.PagoFeriados.MontoDeTotal.ToString("#,#.00#;(#,#.00#)"),

                    DiaPagado = dato.CatalogoDiaAuxiliar.DescripcionDia,
                    HorasLaboradas = dato.DiaPagadoAuxiliar.CantidadHoras.ToString(),
                    SalarioHoras = "₡ " + dato.DiaPagadoAuxiliar.MontoSalarioHora.ToString("#,#.00#;(#,#.00#)"),
                    MontoDia = "₡ " + dato.DiaPagadoAuxiliar.MontoTotal.ToString("#,#.00#;(#,#.00#)"),

                    SalarioBruto = "₡ " + dato.PagoFeriados.MontoSalaroBruto.ToString("#,#.00#;(#,#.00#)"),
                    Division = dato.Puesto.UbicacionAdministrativa.Division.NomDivision,
                    Direccion = dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion,
                    Departamento = dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento,
                    Diferencias = "₡ " + (Math.Round(dato.PagoFeriados.MontoSalarioEscolar + dato.PagoFeriados.MontoSubtotalDia, 2)).ToString("#,#.00#;(#,#.00#)"),

                    DeduccionEfectuada = dato.DeduccionesObreroAuxiliar.DescripcionDeduccion,
                    PorcentajeDeduccion = dato.DeduccionesEfectuadasObreraAuxiliar.PorcentajeEfectuado + "%",
                    MontoDeduccion = "₡ " + dato.DeduccionesEfectuadasObreraAuxiliar.MontoDeduccion.ToString("#,#.00#;(#,#.00#)"),

                    DeduccionEfectuadaPatronal = dato.DeduccionesPatronalAuxiliar.DescripcionDeduccion,
                    PorcentajeDeduccionPatronal = dato.DeduccionesEfectuadasObreraAuxiliar.PorcentajeEfectuado + "%",
                    MontoDeduccionPatronal = "₡ " + dato.DeduccionesEfectuadasPatronalAuxiliar.MontoDeduccion.ToString("#,#.00#;(#,#.00#)"),
                    PorcentajeSalarioEscolar = dato.SalEscolarEfectuado.PorcentajeEfectuado.ToString("#.00#") + "%"
                };
            }
            else
            {
                return new PagoFeriadoRptData();
            }
        }



        internal static PagoFeriadoRptData GenerarDatosReporteBusqueda(BusquedaPagoFeriadoVM dato, string filtros)
        {
            if (dato.CatalogoDiaAuxiliar != null)
            {
                return new PagoFeriadoRptData
                {
                    Consecutivo = dato.PagoFeriado.IdEntidad.ToString(),
                    Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                    dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                    Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    Filtros = filtros,
                    FechaTramite = dato.PagoExtraordinario.FechaTramite.ToString(),
                    EstadoTramite = dato.EstadoTramite.DescripcionEstado,
                    Observaciones = dato.PagoFeriado.ObsevacionTramite,
                    MontoSalarioEscolar = "₡ " + dato.PagoFeriado.MontoSalarioEscolar.ToString("#,#.00#;(#,#.00#)"),
                    SubtotalDias = "₡ " + dato.PagoFeriado.MontoSubtotalDia.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccion = "₡ " + dato.PagoFeriado.MontoDeduccionObrero.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccionPatronal = "₡ " + dato.PagoFeriado.MontoDeduccionPatronal.ToString("#,#.00#;(#,#.00#)"),
                    DiferenciaLiquida = "₡ " + dato.PagoFeriado.MontoDiferenciaLiquida.ToString("#,#.00#;(#,#.00#)"),
                    AguinaldoProporcional = "₡ " + dato.PagoFeriado.MontoAguinaldoProporcional.ToString("#,#.00#;(#,#.00#)"),
                    MontoTotal = "₡ " + dato.PagoFeriado.MontoDeTotal.ToString("#,#.00#;(#,#.00#)"),
                    DiaPagado = dato.CatalogoDiaAuxiliar.DescripcionDia,
                    HorasLaboradas = dato.DiaPagadoAuxiliar.CantidadHoras.ToString(),
                    SalarioHoras = "₡ " + dato.DiaPagadoAuxiliar.MontoSalarioHora.ToString("#,#.00#;(#,#.00#)"),
                    MontoDia = "₡ " + dato.DiaPagadoAuxiliar.MontoTotal.ToString("#,#.00#;(#,#.00#)"),
                    SalarioBruto = "₡ " + dato.PagoFeriado.MontoSalaroBruto.ToString("#,#.00#;(#,#.00#)"),
                    PorcentajeSalarioEscolar = dato.SalEscolarEfectuado.PorcentajeEfectuado.ToString("#.00#") + "%"

                };
            }
            else
            {

                return new PagoFeriadoRptData
                {
                    Consecutivo = dato.PagoFeriado.IdEntidad.ToString(),
                    Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre + " " +
                                    dato.Funcionario.PrimerApellido + " " + dato.Funcionario.SegundoApellido,
                    Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    Filtros = filtros,
                    FechaTramite = dato.PagoExtraordinario.FechaTramite.ToString(),
                    EstadoTramite = dato.EstadoTramite.DescripcionEstado,
                    Observaciones = dato.PagoFeriado.ObsevacionTramite,
                    MontoSalarioEscolar = "₡ " + dato.PagoFeriado.MontoSalarioEscolar.ToString("#,#.00#;(#,#.00#)"),
                    SubtotalDias = "₡ " + dato.PagoFeriado.MontoSubtotalDia.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccion = "₡ " + dato.PagoFeriado.MontoDeduccionObrero.ToString("#,#.00#;(#,#.00#)"),
                    TotalDeduccionPatronal = "₡ " + dato.PagoFeriado.MontoDeduccionPatronal.ToString("#,#.00#;(#,#.00#)"),
                    DiferenciaLiquida = "₡ " + dato.PagoFeriado.MontoDiferenciaLiquida.ToString("#,#.00#;(#,#.00#)"),
                    AguinaldoProporcional = "₡ " + dato.PagoFeriado.MontoAguinaldoProporcional.ToString("#,#.00#;(#,#.00#)"),
                    MontoTotal = "₡ " + dato.PagoFeriado.MontoDeTotal.ToString("#,#.00#;(#,#.00#)"),


                };

            }

        }
    }
}
