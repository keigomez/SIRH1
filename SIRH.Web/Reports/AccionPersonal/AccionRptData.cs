using System;
using SIRH.Web.Helpers;
using SIRH.Web.ViewModels;
using System.Globalization;

namespace SIRH.Web.Reports.AccionPersonal
{
    public class AccionRptData
    {//
        public string Funcionario { get; set; }
        public string Codigo { get; set; }
        public string TipoAccion { get; set; }
        public string FechaRige { get; set; }
        public string FechaVence { get; set; }
        public string FechaIntegraRige { get; set; }
        public string FechaIntegraVence { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }
        public string NumAccion { get; set; }
        public string NumExpediente { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }

        public string MonAnualidad { get; set; }

        public string Programa { get; set; }
        public string Seccion { get; set; }
        public string Clase { get; set; }
        public string Categoria { get; set; }
        public string Especialidad { get; set; }
        public string SubEspecialidad { get; set; }
        public string CodPuesto { get; set; }
        public string NumAnualidad { get; set; }
        public string MesAumento { get; set; }
        public string PorProh { get; set; }
        public string PorRiesgo { get; set; }
        public string PorDisponibilidad { get; set; }
        public string PorGradoPolicial { get; set; }
        public string PorCurso { get; set; }
        public string PorQuinquenio { get; set; }
        public string DetalleMonOtros { get; set; }
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
        public string EspecialidadNuevo { get; set; }
        public string SubEspecialidadNuevo { get; set; }
        public string CodPuestoNuevo { get; set; }
        public string NumAnualidadNuevo { get; set; }
        public string MesAumentoNuevo { get; set; }
        public string PorProhNuevo { get; set; }
        public string PorRiesgoNuevo { get; set; }
        public string PorDisponibilidadNuevo { get; set; }
        public string PorGradoPolicialNuevo { get; set; }
        public string PorCursoNuevo { get; set; }
        public string DetalleMonOtrosNuevo { get; set; }
        public string PorQuinquenioNuevo { get; set; }
        public string MonSalBaseNuevo { get; set; }
        public string MonAnualNuevo { get; set; }
        public string MonRecargoNuevo { get; set; }
        public string NumGradoNuevo { get; set; }
        public string MonGradoNuevo { get; set; }
        public string MonProhNuevo { get; set; }
        public string MonOtrosNuevo { get; set; }
        public string MonTotalNuevo { get; set; }


        internal static AccionRptData GenerarDatosReporte(FormularioAccionPersonalVM dato, string filtros)
        {
            return new AccionRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre.TrimEnd() + " " +
                              dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Codigo = dato.Accion.IdEntidad.ToString(),
                TipoAccion = dato.Accion.TipoAccion.DesTipoAccion,
                FechaRige = dato.Accion.FecRige.ToShortDateString(),
                FechaVence = dato.Accion.FecVence != null ?
                                (dato.Accion.FecVence.ToString() != "01/01/0001") ? Convert.ToDateTime(dato.Accion.FecVence).ToShortDateString() : ""  
                                : "",
                FechaIntegraRige = (dato.Accion.FecRigeIntegra.ToShortDateString() != "01/01/0001") ? Convert.ToDateTime(dato.Accion.FecRigeIntegra).ToShortDateString() : "",
                FechaIntegraVence = dato.Accion.FecVenceIntegra != null ?
                                        (dato.Accion.FecVenceIntegra.ToString() != "01/01/0001") ? Convert.ToDateTime(dato.Accion.FecVenceIntegra).ToShortDateString() : ""
                                        : "",
                Estado = dato.Accion.Estado.DesEstadoBorrador,
                Usuario = dato.Asignado.Nombre + " " + dato.Asignado.PrimerApellido + " " + dato.Asignado.SegundoApellido,
                NumAccion = dato.Accion.NumAccion,
                Observaciones = dato.Accion.Observaciones,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,
            };
        }


        internal static AccionRptData GenerarDatosReporteDetalle(FormularioAccionPersonalVM dato, string filtros)
        {
            var mesAnterior = 0;
            if (dato.TipoAccion.IdEntidad == Convert.ToInt32(ETipoAccionesHelper.ModificFechaAA))
                mesAnterior = Convert.ToInt16(dato.Contrato.FechaMesAumento) - Convert.ToInt16(dato.Accion.IndDato);
            else
                mesAnterior = Convert.ToInt16(dato.Contrato.FechaMesAumento);

            return new AccionRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre.TrimEnd() + " " +
                              dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Codigo = dato.Accion.IdEntidad.ToString(),
                TipoAccion = dato.TipoAccion.DesTipoAccion,
                FechaRige = dato.Accion.FecRige.ToShortDateString(),
                FechaVence = (dato.Accion.FecVence.ToString().Length > 0) ? dato.Accion.FecVence.ToString().Substring(0, 10) : "",
                FechaIntegraRige = dato.Accion.FecRigeIntegra.ToShortDateString(),
                FechaIntegraVence = (dato.Accion.FecVence.ToString().Length > 0) ? dato.Accion.FecVenceIntegra.ToString().Substring(0, 10) : "",
                Estado = (dato.Estado.IdEntidad == 8) ? "Anulado" : "",
                Usuario = "", //dato.Asignado.Nombre + " " + dato.Asignado.PrimerApellido + " " + dato.Asignado.SegundoApellido,
                NumExpediente = dato.Expediente.NumeroExpediente.ToString(),
                NumAccion = dato.Accion.NumAccion,
                Observaciones = dato.Accion.Observaciones,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,
                Programa = dato.Puesto.UbicacionAdministrativa.Presupuesto.Programa.IdEntidad + " " + dato.Puesto.UbicacionAdministrativa.Presupuesto.Programa.DesPrograma,
                Seccion = dato.Puesto.UbicacionAdministrativa.Seccion.IdEntidad + " " + dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion,
                //Clase = dato.DetallePuesto.Clase.IdEntidad + " " + dato.DetallePuesto.Clase.DesClase,
                Clase = dato.Detalle.DetallePuestoAnterior.Clase.IdEntidad + " " + dato.Detalle.DetallePuestoAnterior.Clase.DesClase,
                Categoria = dato.Detalle.IndCategoria > 0 ? dato.Detalle.IndCategoria.ToString() : dato.DetallePuesto.EscalaSalarial.CategoriaEscala.ToString(), //dato.DetallePuesto.EscalaSalarial.CategoriaEscala.ToString(),
                Especialidad = dato.Detalle.DetallePuestoAnterior.Especialidad.IdEntidad.ToString() + " " + dato.Detalle.DetallePuestoAnterior.Especialidad.DesEspecialidad,
                SubEspecialidad = (dato.Detalle.DetallePuestoAnterior.SubEspecialidad != null) ?
                                        dato.Detalle.DetallePuestoAnterior.SubEspecialidad.IdEntidad > -1 ?  
                                            dato.Detalle.DetallePuestoAnterior.SubEspecialidad.IdEntidad.ToString() + " " + dato.Detalle.DetallePuestoAnterior.SubEspecialidad.DesSubEspecialidad
                                            : " "
                                        : "",
                CodPuesto = dato.Detalle.DetallePuestoAnterior.Puesto.CodPuesto, //dato.Puesto.CodPuesto,
                MesAumento = CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[mesAnterior - 1].ToString().ToUpper(),
                NumAnualidad = (dato.TipoAccion.IdEntidad == 32) ? Convert.ToInt16((dato.Contrato.NumeroAnualidades - dato.Accion.IndDato)).ToString() : dato.Contrato.NumeroAnualidades.ToString(),
                MonAnualidad = dato.Detalle.DetallePuestoAnterior.EscalaSalarial.MontoAumentoAnual.ToString("#,#.00#;(#,#.00#)"), //dato.DetallePuesto.EscalaSalarial.MontoAumentoAnual.ToString("#,#.00#;(#,#.00#)"),
                PorProh = dato.Detalle.PorProhOriginal.ToString() + "%",
                MonSalBase = dato.Detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase.ToString("#,#.00#;(#,#.00#)"), //dato.DetallePuesto.EscalaSalarial.SalarioBase.ToString("#,#.00#;(#,#.00#)"),
                MonAnual = (dato.Contrato.NumeroAnualidades * dato.Detalle.DetallePuestoAnterior.EscalaSalarial.MontoAumentoAnual).ToString("#,#.00#;(#,#.00#)"), //(dato.Contrato.NumeroAnualidades * dato.DetallePuesto.EscalaSalarial.MontoAumentoAnual).ToString("#,#.00#;(#,#.00#)"),
                MonRecargo = "0,00",
                NumGrado = dato.PuntosCarrera.ToString(),
                MonGrado = (dato.PuntosCarrera * dato.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera).ToString("#,#.00#;(#,#.00#)"),
                MonProh = dato.Detalle.MtoProhibicionAnterior.ToString("#,#.00#;(#,#.00#)"), //(dato.DetallePuesto.EscalaSalarial.SalarioBase * dato.Detalle.PorProhOriginal / 100).ToString("#,#.00#;(#,#.00#)"),
                MonOtros = dato.Detalle.MtoOtrosAnterior.ToString("#,#.00#;(#,#.00#)"),
                MonTotal = dato.Detalle.MtoTotalAnterior.ToString("#,#.00#;(#,#.00#)"),

                DetalleMonOtros = (dato.Detalle.Accion.NumAccion != null) ?
                                   ((dato.Detalle.PorCarreraPolicialAnterior > 0) ? "Carrera Pol. (" + dato.Detalle.PorCarreraPolicialAnterior.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorCursoAnterior > 0) ? "Curso Básico (" + dato.Detalle.PorCursoAnterior.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorRiesgoAnterior > 0) ? "Riesgo (" + dato.Detalle.PorRiesgoAnterior.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorGradoPolicialAnterior > 0) ? "Grado Policial (" + dato.Detalle.PorGradoPolicialAnterior.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorDisponibilidadAnterior > 0) ? "Disponibilidad (" + dato.Detalle.PorDisponibilidadAnterior.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorQuinquenioAnterior > 0) ? "Quinquenio (" + dato.Detalle.PorQuinquenioAnterior.ToString() + " %)" + "\n\r" : "") +
                                   ((dato.Detalle.PorBonificacionAnterior > 0) ? "Bonificación Ext. (" + dato.Detalle.PorBonificacionAnterior.ToString() + " %)" + "\n\r" : "") +
                                   ((dato.Detalle.PorConsultaAnterior > 0) ? "Consulta Ext. (" + dato.Detalle.PorConsultaAnterior.ToString() + " %)" + "\n\r" : "") +
                                   ((dato.Detalle.PorPeligrosidadAnterior > 0) ? "Peligrosidad (" + dato.Detalle.PorPeligrosidadAnterior.ToString() + " %)" : "")
                                   : "",

                ProgramaNuevo = "", //(dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.Programa.IdEntidad + " " + dato.Detalle.Programa.DesPrograma : "",
                SeccionNuevo = "", //(dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.Seccion.IdEntidad + " " + dato.Detalle.Seccion.NomSeccion : "",
                ClaseNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Clase.IdEntidad.ToString() + " " + dato.Clase.DesClase : "",
                CategoriaNuevo = (dato.Detalle.Accion.NumAccion != null) ?
                                    (dato.TipoAccion.IdEntidad == 54) ? dato.Detalle.IndCategoria.ToString() : dato.DetallePuesto.EscalaSalarial.CategoriaEscala.ToString()
                                    : "",
                EspecialidadNuevo = dato.DetallePuesto.Especialidad.IdEntidad.ToString() + " " + dato.DetallePuesto.Especialidad.DesEspecialidad,
                SubEspecialidadNuevo = (dato.DetallePuesto.SubEspecialidad != null) ?
                                            dato.Detalle.DetallePuestoAnterior.SubEspecialidad.IdEntidad > -1 ?
                                                dato.DetallePuesto.SubEspecialidad.IdEntidad.ToString() + " " + dato.DetallePuesto.SubEspecialidad.DesSubEspecialidad
                                                : ""
                                            : "",
                CodPuestoNuevo = dato.Accion.Nombramiento.Puesto.CodPuesto,//(dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.CodPuesto : "",
                MesAumentoNuevo = dato.MesSeleccionado,
                NumAnualidadNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.NumAnualidad.ToString() : "",
                PorCursoNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.PorCurso.ToString() + "%" : "",
                PorRiesgoNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.PorRiesgo.ToString() + "%" : "",
                PorDisponibilidadNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.PorDisponibilidad.ToString() + "%" : "",
                PorQuinquenioNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.PorQuinquenio.ToString() + "%" : "",
                PorProhNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.PorProhibicion.ToString() + "%" : "",
                MonSalBaseNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.MtoSalarioBase.ToString("#,#.00#;(#,#.00#)") : "",
                //MonAnualNuevo = (dato.Detalle.Accion.NumAccion != null) ? (dato.Detalle.MtoAumentosAnuales  * dato.DetallePuesto.EscalaSalarial.MontoAumentoAnual).ToString("#,#.00#;(#,#.00#)") : "",
                MonAnualNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.MtoAumentosAnuales.ToString("#,#.00#;(#,#.00#)") : "",
                MonRecargoNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.MtoRecargo.ToString("#,#.00#;(#,#.00#)") : "",
                NumGradoNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.NumGradoGrupo.ToString() : "",
                MonGradoNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.MtoGradoGrupo.ToString("#,#.00#;(#,#.00#)") : "",
                MonProhNuevo = (dato.Detalle.Accion.NumAccion != null) ? (dato.Detalle.MtoSalarioBase * dato.Detalle.PorProhibicion / 100).ToString("#,#.00#;(#,#.00#)") : "",
                MonOtrosNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.MtoOtros.ToString("#,#.00#;(#,#.00#)") : "",

                DetalleMonOtrosNuevo = (dato.Detalle.Accion.NumAccion != null) ?
                                   ((dato.Detalle.PorCarreraPolicial > 0) ? "Carrera Pol. (" + dato.Detalle.PorCarreraPolicial.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorCurso > 0) ? "Curso Básico (" + dato.Detalle.PorCurso.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorRiesgo > 0) ? "Riesgo (" + dato.Detalle.PorRiesgo.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorGradoPolicial > 0) ? "Grado Policial (" + dato.Detalle.PorGradoPolicial.ToString() + "%)" + "\n\r" : "") +
                                   ((dato.Detalle.PorDisponibilidad > 0) ? "Disponibilidad (" + dato.Detalle.PorDisponibilidad.ToString() + "%)" + "\n\r" : "") +                                  
                                   ((dato.Detalle.PorQuinquenio > 0) ? "Quinquenio (" + dato.Detalle.PorQuinquenio.ToString() + " %)" + "\n\r" : "") +
                                   ((dato.Detalle.PorBonificacion > 0) ? "Bonificación Ext. (" + dato.Detalle.PorBonificacion.ToString() + " %)" + "\n\r" : "") +
                                   ((dato.Detalle.PorConsulta > 0) ? "Consulta Ext. (" + dato.Detalle.PorConsulta.ToString() + " %)" + "\n\r" : "") +
                                   ((dato.Detalle.PorPeligrosidad > 0) ? "Peligrosidad (" + dato.Detalle.PorPeligrosidad.ToString() + " %)" : "") 
                                   : "",

                //MonTotalNuevo = (dato.Detalle.Accion.NumAccion != null) ?
                //                    (dato.Detalle.MtoSalarioBase +
                //                    dato.Detalle.MtoAumentosAnuales +
                //                    dato.Detalle.MtoRecargo +
                //                    (dato.Detalle.MtoSalarioBase * dato.Detalle.PorProhibicion / 100) +
                //                    (dato.Detalle.NumGradoGrupo * dato.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera) +
                //                    dato.Detalle.MtoOtros).ToString("#,#.00#;(#,#.00#)") 
                //                : "",
                MonTotalNuevo = (dato.Detalle.Accion.NumAccion != null) ? dato.Detalle.MtoTotal.ToString("#,#.00#;(#,#.00#)") : "",

            };
        }


        internal static AccionRptData GenerarDatosHistoricoDetalle(FormularioAccionPersonalVM dato, string filtros)
        {
            return new AccionRptData
            {
                Funcionario = dato.Funcionario.Cedula + " - " + dato.Funcionario.Nombre.TrimEnd() + " " +
                              dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Codigo = dato.TipoAccion.IdEntidad.ToString(),
                TipoAccion = dato.TipoAccion.DesTipoAccion,
                FechaRige = dato.Historico.FecRige.ToShortDateString(),
                FechaVence = (dato.Historico.FecVence.ToShortDateString() != "01/01/0001") ? dato.Historico.FecVence.ToString().Substring(0, 10) : "",
                
                Usuario = "", //dato.Asignado.Nombre + " " + dato.Asignado.PrimerApellido + " " + dato.Asignado.SegundoApellido,
               // NumExpediente = dato.Expediente.NumeroExpediente.ToString(),
                NumAccion = dato.Historico.NumAccion,
                Observaciones = dato.Historico.Explicacion,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,
                
                Clase = dato.Historico.CodClase + " " + dato.Historico.DesClase,
                Categoria = dato.Historico.Categoria,
                
                CodPuesto = dato.Historico.CodPuesto,
                
                NumAnualidad = dato.Historico.Disfrutado.ToString(),
                                
                MonSalBase = dato.Historico.MtoSalarioBase,
                MonAnual = dato.Historico.MtoAumentosAnuales,
                MonRecargo = dato.Historico.MtoRecargo,
                MonGrado = dato.Historico.MtoGradoGrupo,
                MonProh = dato.Historico.MtoProhibicion,
                MonOtros = dato.Historico.MtoOtros,
                MonTotal = dato.Historico.MtoTotal.ToString("#,#.00#;(#,#.00#)"),

                ClaseNuevo = dato.Historico.CodClase2 != null ? dato.Historico.CodClase2 + " " + dato.Historico.DesClase2 : "",
                CategoriaNuevo = dato.Historico.Categoria2 != null ? dato.Historico.Categoria2 : "",
                CodPuestoNuevo = dato.Historico.CodPuesto2 != null ? dato.Historico.CodPuesto2 : "",
                NumAnualidadNuevo = dato.Historico.Disfrutado2.ToString(),
                MonSalBaseNuevo = dato.Historico.MtoSalarioBase2,
                MonAnualNuevo = dato.Historico.MtoAumentosAnuales2,
                MonRecargoNuevo = dato.Historico.MtoRecargo2,
                MonGradoNuevo = dato.Historico.MtoGradoGrupo2,
                MonProhNuevo = dato.Historico.MtoProhibicion2,
                MonOtrosNuevo = dato.Historico.MtoOtros2,
                MonTotalNuevo = dato.Historico.MtoTotal2.ToString("#,#.00#;(#,#.00#)")
            };
        }


        internal static AccionRptData GenerarDatosReporteHistorico(FormularioAccionPersonalVM dato, string filtros)
        {
            return new AccionRptData
            {
                Funcionario = dato.Historico.Cedula,
                Codigo = dato.Historico.IdEntidad.ToString(),
                CodPuesto = dato.Historico.CodPuesto,
                TipoAccion = dato.Historico.CodAccion + ' ' + dato.TipoAccion.DesTipoAccion,
                FechaRige = dato.Historico.FecRige.ToShortDateString(),
                FechaVence = (dato.Historico.FecVence != null) ? dato.Historico.FecVence.ToShortDateString() : "",
                NumAccion = dato.Historico.NumAccion,
                Observaciones = dato.Historico.Explicacion,
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                Filtros = filtros,
            };
        }
    }
}