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
using SIRH.DTO;
using SIRH.Web.ViewModels;
using System.Collections.Generic;

namespace SIRH.Web.Reports.RegistroTiempoExtra
{
    //Comentario para subirlo.
    public class RegistroTiempoExtraRptData
    {
        public string Funcionario { get; set; }
        public string Periodo { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFinal { get; set; }
        public string FechaPago { get; set; }
        public string Autor { get; set; }
        public string Cedula { get; set; }
        public string Puesto { get; set; }
        public string H0 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string TotalH0 { get; set; }
        public string TotalH1 { get; set; }
        public string TotalH2 { get; set; }
        public string DiaMes { get; set; }
        public string CodigoPresupuestal { get; set; }
        public string Area { get; set; }
        public string Actividad { get; set; }
        public string Cargo { get; set; }
        public string MontoDiurna { get; set; }
        public string MontoMixta { get; set; }
        public string MontoNocturna { get; set; }
        public string MontoTotal { get; set; }
        public string Justificacion { get; set; }
        public string OficJustificacion { get; set; }
        public string Lugar_Trabajo { get; set; }
        public string Seccion { get; set; }
        public string Estado { get; set; }
        public string Doble { get; set; }
        public string Titulo { get; set; }
        public string TipoArchivo { get; set; } //Administravos: DPA-228(V4) - Oficiales: DPA-228-a(V4)

        #region FILTROS
        public string FiltroCedula { get; set; }
        public string FiltroDepartamento { get; set; }
        public string FiltroDireccion { get; set; }
        public string FiltroDivision { get; set; }
        public string FiltroFechaDesde { get; set; }
        public string FiltroFechaHasta { get; set; }
        public string FiltroSeccion { get; set; }
        public string FiltroEstado { get; set; }
        public string FiltroPagoDoble { get; set; }
        public string Codigo { get; set; }
        #endregion

        internal static RegistroTiempoExtraRptData GenerarDatosReporte(FuncionarioRegistroExtrasVM dato, string filtros, int index)
        {
            bool hayExtra = dato.DetalleExtras[index].FechaFinal >= dato.DetalleExtras[index].FechaInicio;
            string codigo = Convert.ToInt32(dato.RegistroTiempoExtra.Estado).ToString() + dato.RegistroTiempoExtra.IdEntidad + dato.Funcionario.Cedula.Substring(dato.Funcionario.Cedula.Length - 5, 4);
            string estado;
            string titulo;
            string h0 = "";
            string tipoarchivo = "";
            string montoDiurna = "";
            string montoMixta = "";
            string montoNocturna = "";
            if (dato.Doble)
            {
                estado = dato.EstadoDetalles.ToString();
                titulo = "INFORME DE JORNADAS DOBLES PARA AGENTES DE SEGURIDAD Y VIGILANCIA";
                if (estado != "Activo")
                {
                    titulo += " (" + estado.ToUpper() + ")";
                }
            }
            else
            {
                estado = dato.RegistroTiempoExtra.Estado.ToString();
                if(Convert.ToBoolean(dato.DetallePuesto.OcupacionReal?.DesOcupacionReal.Contains("GUARDA")) || Convert.ToBoolean(dato.RegistroTiempoExtra.Clase?.DesClase.Contains("OFIC.SEGUR.SERV.CIVIL")))
                {
                    titulo = "INFORME DE TIEMPO EXTRAORDINARIO PARA AGENTES DE SEGURIDAD Y VIGILANCIA";// DPA-228-a(V4)
                    montoDiurna = (dato.RegistroTiempoExtra.MontoDiurna* (decimal)1.5).ToString("0.00");
                    montoMixta = (dato.RegistroTiempoExtra.MontoMixta * (decimal)1.5)?.ToString("0.00");
                    montoNocturna = (dato.RegistroTiempoExtra.MontoNocturna * (decimal)1.5)?.ToString("0.00");
                    tipoarchivo = "DPA-228-a (V4)";
                    if (estado != "Activo")
                    {
                        titulo += " (" + estado.ToUpper() + ")";
                    }
                }
                else
                {
                    titulo = "INFORME DE TIEMPO EXTRAORDINARIO";//Administrativos  DPA-228(V4)
                    montoDiurna = (dato.RegistroTiempoExtra.MontoDiurna * (decimal)1.5).ToString("0.00");
                    //dato.RegistroTiempoExtra.MontoNocturna = dato.RegistroTiempoExtra.MontoNocturna * 2;
                    tipoarchivo = "DPA-228(V4)";

                    if (estado != "Activo")
                    {
                        titulo += " (" + estado.ToUpper() + ")";
                    }
                }
            }
            if (dato.DetalleExtras[index].Jornada == SIRH.DTO.JornadaEnum.D
                || dato.DetalleExtras[index].Jornada == SIRH.DTO.JornadaEnum.DD
                || dato.DetalleExtras[index].Jornada == SIRH.DTO.JornadaEnum.MD
                || dato.DetalleExtras[index].Jornada == SIRH.DTO.JornadaEnum.ND)
            {
                h0 = horas(dato.DetalleExtras[index].H0);
            }
            else if (dato.DetalleExtras[index].TipoExtra?.IdEntidad != null &&
                (dato.DetalleExtras[index].TipoExtra.IdEntidad == 4 || dato.DetalleExtras[index].TipoExtra.IdEntidad == 6 
                || dato.DetalleExtras[index].TipoExtra.IdEntidad == 7 || dato.DetalleExtras[index].TipoExtra.IdEntidad == 8))
            {
                if (dato.DetalleExtras[index].Jornada == SIRH.DTO.JornadaEnum.M)
                {
                    h0 = horas(dato.DetalleExtras[index].H0); // Aquí estaba H0+1
                }
                else if (dato.DetalleExtras[index].Jornada == SIRH.DTO.JornadaEnum.N)
                {
                    h0 = horas(dato.DetalleExtras[index].H0); //Aquí estaba H0+2
                }
            }
            return new RegistroTiempoExtraRptData
            {
                Funcionario = $"{dato.Funcionario.Nombre.Trim()} {dato.Funcionario.PrimerApellido.Trim()} {dato.Funcionario.SegundoApellido.Trim()}",
                Periodo = dato.RegistroTiempoExtra.Periodo,
                FechaInicial = dato.DetalleExtras[index].FechaInicio.ToShortDateString(),
                FechaFinal = hayExtra ? dato.DetalleExtras[index].FechaFinal.ToShortDateString() : "",
                HoraInicio = hayExtra ? tiempo(dato.DetalleExtras[index].HoraInicio, dato.DetalleExtras[index].MinutoInicio) : "",
                HoraFinal = hayExtra ? tiempo(dato.DetalleExtras[index].HoraFinal, dato.DetalleExtras[index].MinutoFinal) : "",
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Cedula = dato.Funcionario.Cedula,
                Puesto = dato.RegistroTiempoExtra.Clase?.DesClase != null ? dato.RegistroTiempoExtra.Clase.DesClase : "",
                H0 = h0,
                H1 = horas(dato.DetalleExtras[index].H1),
                H2 = horas(dato.DetalleExtras[index].H2),
                DiaMes = (index + 1).ToString(),
                Codigo = codigo,
                TotalH0 = dato.TotalHorasH0.ToString("0.00").Replace(".", ":"),
                TotalH1 = dato.TotalHorasH1.ToString("0.00").Replace(".", ":"),
                TotalH2 = dato.TotalHorasH2.ToString("0.00").Replace(".", ":"),
                CodigoPresupuestal = dato.RegistroTiempoExtra.Presupuesto?.CodigoPresupuesto != null ? dato.RegistroTiempoExtra.Presupuesto.CodigoPresupuesto.Substring(0, 3) : "",
                Area = dato.RegistroTiempoExtra.Area != null ? dato.RegistroTiempoExtra.Area : dato.Area, //verificación de area
                Actividad = dato.RegistroTiempoExtra.Actividad != null ? dato.RegistroTiempoExtra.Actividad : dato.Actividad, //Verificacion de actividad
                Cargo = dato.DetallePuesto.OcupacionReal?.DesOcupacionReal != null ? dato.DetallePuesto.OcupacionReal.DesOcupacionReal : "",
                MontoDiurna = montoDiurna,//string.Format("{0:0.00}", montoDiurna),//(dato.RegistroTiempoExtra.MontoDiurna)),
                MontoMixta = string.Format("{0:0.00}", montoMixta), //dato.RegistroTiempoExtra.MontoMixta),
                MontoNocturna = string.Format("{0:0.00}", montoNocturna), //dato.RegistroTiempoExtra.MontoNocturna),
                MontoTotal = string.Format("{0:0.00}", dato.RegistroTiempoExtra.MontoTotal),
                Justificacion = dato.RegistroTiempoExtra.Justificacion != null ? dato.RegistroTiempoExtra.Justificacion : "",
                OficJustificacion = dato.RegistroTiempoExtra.OficJustificacion != null ? dato.RegistroTiempoExtra.OficJustificacion : "",
                Lugar_Trabajo = elegir_lugar(dato),
                Titulo = titulo,
                TipoArchivo = tipoarchivo,
            };
        }

        internal static RegistroTiempoExtraRptData GenerarDatosReporteGeneral(CRegistroTiempoExtraDTO dato, List<string> filtros)
        {
            return new RegistroTiempoExtraRptData
            {
                Cedula = dato.Funcionario.Cedula,
                Funcionario = $"{dato.Funcionario.Nombre.Trim()} {dato.Funcionario.PrimerApellido.Trim()} {dato.Funcionario.SegundoApellido.Trim()}",
                Seccion = dato.Seccion.NomSeccion,
                FechaPago = filtros[8] == "Jornada Doble" ? dato.FecRegistroDetalles.ToShortDateString() : dato.FecPago.ToShortDateString(),
                MontoTotal = string.Format("{0:0.00}", dato.MontoTotal),
                Estado = filtros[8] == "Jornada Doble"? dato.EstadoDetalles.ToString() : dato.Estado.ToString(),
                FiltroCedula = filtros[0],
                FiltroDepartamento = filtros[1],
                FiltroDireccion = filtros[2],
                FiltroDivision = filtros[3],
                FiltroFechaDesde = filtros[4],
                FiltroFechaHasta = filtros[5],
                FiltroSeccion = filtros[6],
                FiltroEstado = filtros[7],
                FiltroPagoDoble = filtros[8],
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
            };
        }

        private static string tiempo(string hora, string min)
        {
            return (hora != null) ? $"{hora}:{min}" : "";
        }

        private static string horas(decimal H)
        {
            return (H != 0) ? Math.Round(H,2).ToString().Replace(".", ":") : "";
        }

        private static string elegir_lugar(FuncionarioRegistroExtrasVM dato)
        {
            if (dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion != null)
            {
                return dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion;
            }
            else if (dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento != null)
            {
                return dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento;
            }
            else if (dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion != null)
            {
                return dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion;
            }
            else if (dato.Puesto.UbicacionAdministrativa.Division.NomDivision != null)
            {
                return dato.Puesto.UbicacionAdministrativa.Division.NomDivision;
            }
            else
            {
                return "";
            }
        }
    }
}
