//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SIRH.DeamonServicios.MarcacionesService;
//using SIRH.DTO;
//using System.IO;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

//namespace SIRH.DeamonServicios
//{
//    public class CMarcacionesWinService
//    {
//        internal void EjecutarServicioInconsistenciasMarcaciones(CMarcacionServiceClient marcas, ServiceSIRH winservice)
//        {
//            winservice.EscribirEntrada("Intentando ejecutar el servicio de marcas de asistencia");
//            if (DateTime.Now.Day == 1){
//                winservice.EscribirEntrada("Ejecutando el servicio de marcas de asistencia");
//                var busqueda = DateTime.Now.AddMonths(-1);
//                List<DateTime> fechas = new List<DateTime>();
//                fechas.Add(new DateTime(busqueda.Year, busqueda.Month, 1, 0, 0, 0));
//                fechas.Add(new DateTime(busqueda.Year, busqueda.Month, DateTime.DaysInMonth(busqueda.Year, busqueda.Month), 23, 59, 0));
            
//                var departamentos = marcas.ListarDepartamentos();
//                if (departamentos.Length < 0) {
//                    foreach(var elem in departamentos){
//                        try
//                        {
//                            var datosReporte = marcas.ReporteConsolidadoPorDepartamento((CDepartamentoDTO)elem, fechas.ToArray());
//                            var reporte = InitReportePorCorreo(datosReporte, winservice, fechas);
//                            EnviarCorreo(reporte,winservice);
//                        }
//                        catch (Exception error) {
//                            winservice.EscribirEntrada(error.Message);
//                        }
//                    }
//                }else{
//                    throw new Exception("Servicio de marcas de asistencia NO encontró departamentos");
//                }
//            }else {
//                throw new Exception("Servicio de marcas de asistencia cancelado por la Fecha Actual");
//            }
//        }

//        private void EnviarCorreo(ReportDocument reporte, ServiceSIRH winservice)
//        {
//            CEmailHelper mensajero = new CEmailHelper();
//            winservice.EscribirEntrada("Construyó el email");
//            mensajero = this.EncabezadoCorreoReporte(mensajero);
//            winservice.EscribirEntrada("Adjuntando el reporte");
//            AdjuntarReporte(mensajero,reporte);
//            winservice.EscribirEntrada("Enviando Correo");
//            winservice.EscribirEntrada("Correo enviado: Resultado " + ((mensajero.EnviarCorreo())?"Exitos":"Fallido"));
//        }

//        private void AdjuntarReporte(CEmailHelper salida, ReportDocument reporte)
//        {
//            reporte.ExportToDisk(ExportFormatType.PortableDocFormat, "ReporteConsolidadoDepartamentoRPT.rpt");
//            salida.AttachmentFlag = true;
//        }

//        private CEmailHelper EncabezadoCorreoReporte(CEmailHelper salida)
//        {
//            salida.Asunto = "Reporte de Marcas de Asistencia";
//            salida.EmailBody = "Buenos días, <br><br>";
//            salida.EmailBody += "Revisar el siguiente reporte de marcas: <br><br>";
//            salida = EmailPie(salida);
//            return salida;
//        }

//        private CEmailHelper EmailPie(CEmailHelper salida)
//        {
//            salida.EmailBody += "</table><br><br>";
//            salida.EmailBody += "Para más información puede ingresar al Módulo de Administración de Marcas de Asistencia en la ubicación: http://sisrh.mopt.go.cr:82/Marcaciones/";
//            salida.EmailBody += "<br><br>Por favor no responder a este correo, ya que fue generado automáticamente";
//            salida.EmailBody += "<br><br>Atentamente,";
//            salida.EmailBody += "<br>Unidad de Informática. DGIRH.";
//            salida.EmailBody += "<br>Dirección de Gestión Institucional de Recursos Humanos.";
//            salida.Destinos = "dguiltrc@mopt.go.cr, dgrangeg@mopt.go.cr, vchavesc@mopt.go.cr";

//            return salida;
//        }

//        private ReportDocument InitReportePorCorreo(CBaseDTO[][] datos, ServiceSIRH winservice, List<DateTime> fechas)
//        {
//            winservice.EscribirEntrada("Creando el modelo del reporte");
//            List<ReporteEncabezadoSimpleRptData> modelo1 = new List<ReporteEncabezadoSimpleRptData>();
//            List<ReporteEncabezadoRptData> modelo2 = new List<ReporteEncabezadoRptData>();
//            List<ConsolidadoFuncionarioRptData> modelo3 = new List<ConsolidadoFuncionarioRptData>();
//            List<object> modelos;
//            //datos[0] = [CFuncionarioDTO?,CDetalleContratacionDTO,CTipoJornadaDTO?,CUbicacionAdministrativaDTO]
//            //el signo ? es que puede ser un CErrorDTO

//            for (int i = 0; i < datos.Length; )
//            {
//                if (datos[i][0].GetType() != typeof(CErrorDTO))
//                {
//                    if (modelo1.Count == 0)//Se genera el encabezado del reporte
//                        modelo1.Add(ReporteEncabezadoSimpleRptData.GenerarDatosReporteAsistencia(fechas[0], fechas[1], (CUbicacionAdministrativaDTO)datos[i][3]));
//                    if (((CDetalleContratacionDTO)datos[i][1]).EstadoMarcacion != false)
//                    {
//                        List<List<CMarcacionesDTO>> marcas = new List<List<CMarcacionesDTO>>();
//                        if (datos[i + 1][0].GetType() == typeof(CErrorDTO)){ //datos[i + 1][0] = Una marca o un CErrroDTO
//                            modelos = GenerarModelosConsolidados(fechas[0], fechas[1], datos[i][2].GetType() != typeof(CErrorDTO) ? (CTipoJornadaDTO)datos[i][2] : null, (CFuncionarioDTO)datos[i][0], (CDetalleContratacionDTO)datos[i][1], (CUbicacionAdministrativaDTO)datos[i][3], null);
//                            modelo2.Add(((List<ReporteEncabezadoRptData>)(modelos)[0])[0]);
//                            modelo3.AddRange(((List<ConsolidadoFuncionarioRptData>)(modelos)[1]).Select(M => { M.id = modelo2.Last().Funcionario; return M; }));
//                            i += 2;
//                        }else{
//                            for (int j = i + 1; j < datos.Length; j++){
//                                if (datos[j][0].GetType() == typeof(CMarcacionesDTO))
//                                    marcas.Add(datos[j].Select(E => (CMarcacionesDTO)E).ToList());
//                                else{
//                                    modelos = GenerarModelosConsolidados(fechas[0], fechas[1], datos[i][2].GetType() != typeof(CErrorDTO) ? (CTipoJornadaDTO)datos[i][2] : null, (CFuncionarioDTO)datos[i][0], (CDetalleContratacionDTO)datos[i][1], (CUbicacionAdministrativaDTO)datos[i][3], marcas);
//                                    modelo2.Add(((List<ReporteEncabezadoRptData>)(modelos)[0])[0]);
//                                    modelo3.AddRange(((List<ConsolidadoFuncionarioRptData>)(modelos)[1]).Select(M => { M.id = modelo2.Last().Funcionario; return M; }));
//                                    i = j;
//                                    break;
//                                }
//                                if (j + 1 == datos.Length){
//                                    i = datos.Length;
//                                }
//                            }
//                        }
//                    }
//                }
//                else i++; //No se encontro el funcionario se salta el funcionario   
//            }
//            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "ReporteConsolidadoDepartamentoRPT.rpt");
//            winservice.EscribirEntrada("Generando reporte para enviar");
//            return GenerarReporte(modelo1, reportPath, modelo2, modelo3);
//        }

//        private ReportDocument GenerarReporte(object dataSet, string reportPath, object group, object subreport)
//        {
//            ReportDocument reportDocument = new ReportDocument();
//            reportDocument.Load(reportPath);
//            reportDocument.Database.Tables[0].SetDataSource(dataSet);
//            reportDocument.Database.Tables[1].SetDataSource(group);
//            reportDocument.Subreports[0].SetDataSource(subreport);
//            return reportDocument;
//        }

//        private void SummaryConsolidadoFuncionario(out int ausT, out int ausM, out int tar05, out int tar20, out TimeSpan laborado, DateTime fechaF, List<ConsolidadoFuncionarioRptData> model)
//        {
//            laborado = new TimeSpan(0, 0, 0);
//            ausT = 0; ausM = 0; tar05 = 0; tar20 = 0;
//            foreach (var m in model)
//            {
//                if (m.Laborado != "xx:xx:xx*" && m.Laborado != "xx:xx:xx")
//                    laborado = laborado + Convert.ToDateTime(m.Laborado).TimeOfDay;
//                if (m.Falta == "AU-T")
//                    ausT++;
//                if (m.Falta == "AU-M")
//                    ausM++;
//                if (m.Falta == "T-05")
//                    tar05++;
//                if (m.Falta == "T-20")
//                    tar20++;
//            }
//        }


//        private List<object> GenerarModelosConsolidados(DateTime fechaI, DateTime fechaF, CTipoJornadaDTO jornada,
//                                                                     CFuncionarioDTO funcionario, CDetalleContratacionDTO detalle,
//                                                                     CUbicacionAdministrativaDTO ubicacion, List<List<CMarcacionesDTO>> marcas)
//        {
//            List<ReporteEncabezadoRptData> modelo1 = new List<ReporteEncabezadoRptData>();
//            List<ConsolidadoFuncionarioRptData> modelo2 = new List<ConsolidadoFuncionarioRptData>();
//            var FechaIJornada = jornada != null ? Convert.ToDateTime(jornada.InicioJornada) : default(DateTime);
//            var FechaFJornada = jornada != null ? Convert.ToDateTime(jornada.FinJornada) : default(DateTime);
//            var FechaI = fechaI.AddDays(0);
//            if (marcas != null)
//            {
//                marcas.Add(new List<CMarcacionesDTO> { new CMarcacionesDTO { FechaHoraMarca = fechaF.AddDays(2) } });
//                foreach (var elem in marcas)
//                {
//                    for (; FechaI != elem[0].FechaHoraMarca.Date && FechaI <= fechaF.Date; FechaI = FechaI.AddDays(1))
//                         modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistenciaSinMarca(FechaI, true));
//                    if (elem[0].FechaHoraMarca.Date == FechaI)
//                    {
//                        modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistencia(elem, FechaIJornada, FechaFJornada));
//                        FechaI = FechaI.AddDays(1);
//                    }
//                }
//            }
//            else
//            {
//                for (; FechaI <= fechaF.Date; FechaI = FechaI.AddDays(1))
//                    modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistenciaSinMarca(FechaI, true));
//            }
//            TimeSpan laborado = new TimeSpan(0, 0, 0);
//            int ausT = -1, ausM = -1, tar05 = -1, tar20 = -1;
//            if (jornada != null)
//                SummaryConsolidadoFuncionario(out ausT, out ausM, out tar05, out tar20, out laborado, fechaF, modelo2);

//            modelo1.Add(ReporteEncabezadoRptData.GenerarDatosReporteAsistencia(fechaI,fechaF, funcionario, jornada, detalle,
//                                                                               ubicacion, "", ausT, ausM, tar05, tar20,
//                                                                               String.Format("{0}:{1}", (int)laborado.TotalHours, laborado.Minutes)));
//            return new List<object> { modelo1, modelo2 };

//        }

//    }
//}
