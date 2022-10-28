using SIRH.DTO;
using SIRH.Web.PuestoService;
//using SIRH.Web.PuestoLocal;
using SIRH.Web.Reports.MovimientoPuesto;
using SIRH.Web.Reports.PDF;
using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using SIRH.Web.Helpers;
using System.Security.Principal;
using SIRH.Web.UserValidation;
//
namespace SIRH.Web.Controllers
{
    public class MovimientoPuestoController : Controller
    {
        CPuestoServiceClient puestoService = new CPuestoServiceClient();
        //Comentario para actualizar
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        // GET: MovimientoPuesto
        public ActionResult Index(string codpuesto, string codcedula, string FechaDesde, string FechaHasta, string motivoSeleccionado, int? page)
        {
            Session["errorM"] = null;
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                }
                else
                {
                    MovimientoPuestoVM model = new MovimientoPuestoVM();
                    int paginaActual = page ?? 1;

                    List<CBaseDTO> motivosMv = new List<CBaseDTO>();
                    CMotivoMovimientoDTO dato1 = new CMotivoMovimientoDTO
                    {
                        IdEntidad = -1,
                        DesMotivo = "Todos los ceses"
                    };
                    CMotivoMovimientoDTO dato2 = new CMotivoMovimientoDTO
                    {
                        IdEntidad = -2,
                        DesMotivo = "Todos los nombramientos"
                    };
                    CMotivoMovimientoDTO dato3 = new CMotivoMovimientoDTO
                    {
                        IdEntidad = -3,
                        DesMotivo = "Todos los permisos"
                    };
                    CMotivoMovimientoDTO dato4 = new CMotivoMovimientoDTO
                    {
                        IdEntidad = -4,
                        DesMotivo = "Todos los movimientos"
                    };
                    CMotivoMovimientoDTO dato5 = new CMotivoMovimientoDTO
                    {
                        IdEntidad = -5,
                        DesMotivo = "Movimientos que generan vacantes"
                    };


                    motivosMv.Add(dato1);
                    motivosMv.Add(dato2);
                    motivosMv.Add(dato3);
                    motivosMv.Add(dato4);
                    motivosMv.Add(dato5);
                    motivosMv.AddRange(puestoService.ListarMotivosMovimiento().ToList());

                    var motivosMovimiento = motivosMv
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CMotivoMovimientoDTO)Q).IdEntidad.ToString(),
                            Text = ((CMotivoMovimientoDTO)Q).DesMotivo.ToString()
                        });
                    model.MotivosMovimiento = new SelectList(motivosMovimiento, "Value", "Text");

                    if (String.IsNullOrEmpty(codpuesto) && String.IsNullOrEmpty(codcedula) && String.IsNullOrEmpty(FechaDesde) && String.IsNullOrEmpty(FechaHasta)
                        && String.IsNullOrEmpty(motivoSeleccionado))
                    {
                        return View(model);
                    }
                    else
                    {
                        context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                        if (!Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                            model.PermisoVacantes = true;
                        else
                            model.PermisoVacantes = false;

                        model.MotivoSeleccionado = motivoSeleccionado ?? "";
                        model.CodCedula = codcedula ?? "";
                        model.CodPuesto = codpuesto ?? "";
                        model.FechaDesde = FechaDesde ?? "";
                        model.FechaHasta = FechaHasta ?? "";

                        DateTime? FechaD = FechaDesde == "" ? (DateTime?)null : DateTime.Parse(FechaDesde);
                        DateTime? FechaH = FechaHasta == "" ? (DateTime?)null : DateTime.Parse(FechaHasta);

                        int motivo = motivoSeleccionado == "" ? 0 : Convert.ToInt32(motivoSeleccionado);
                        var movimientosPuestos = puestoService.BuscarMovimientoPuestosFiltros(codpuesto, codcedula, motivo, FechaH, FechaD);

                        Session["movimientos"] = movimientosPuestos.ToList();
                        model.TotalMovimientos = movimientosPuestos.Count();
                        model.TotalPaginas = (int)Math.Ceiling((double)model.TotalMovimientos / 10);
                        model.PaginaActual = paginaActual;
                        if ((((paginaActual - 1) * 10) + 10) > model.TotalMovimientos)
                        {
                            model.MovimientosPuesto = movimientosPuestos.ToList().GetRange(((paginaActual - 1) * 10), (model.TotalMovimientos) - (((paginaActual - 1) * 10))).ToList(); ;
                        }
                        else
                        {
                            model.MovimientosPuesto = movimientosPuestos.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList();
                        }

                        return PartialView("Index_Result", model);
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorMovimientoPuesto");
            }
        }

        public ActionResult GenerarReporteMovimiento()
        {
            Session["errorM"] = null;
            try
            {

                MovimientoPuestoVM modelo = new MovimientoPuestoVM();
                List<MovimientoPuestoRPT> modelo1 = new List<MovimientoPuestoRPT>();

                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                if (!Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                    modelo.PermisoVacantes = true;
                else
                    modelo.PermisoVacantes = false;

                //DateTime? FechaD = model.FechaDesde == null ? (DateTime?)null : DateTime.Parse(model.FechaDesde);
                //DateTime? FechaH = model.FechaHasta == null ? (DateTime?)null : DateTime.Parse(model.FechaHasta);

                //int motivo = model.MotivoSeleccionado == null ? 0 : Convert.ToInt32(model.MotivoSeleccionado);

                //var movimientosPuestos = puestoService.BuscarMovimientoPuestosFiltros(model.CodPuesto, model.CodCedula, motivo, FechaH, FechaD);
                var movimientosPuestos = (List<CMovimientoPuestoDTO>)Session["movimientos"];

                string style = @"<style> TD { mso-number-format:\@; } </style>";
                //var headerTable = @"<Table><tr><td><img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNqlnu1iMrzA69ctdlBJvavSQ8d8bKAUtilw&usqp=CAU"" \></td></tr></Table>";
                var products = new System.Data.DataTable("teste");

                //products.Rows.Add("SIRH", "Fecha:", DateTime.Now.ToShortDateString(), "Usuario:", principal.Identity.Name.Split('\\')[1], "", "", "", "", "");

                products.Columns.Add("CANT", typeof(string));
                products.Columns.Add("PUESTO", typeof(string));
                products.Columns.Add("CLASE", typeof(string));
                products.Columns.Add("ESPECIALIDAD", typeof(string));
                products.Columns.Add("SUBESPECIALIDAD", typeof(string));
                products.Columns.Add("CEDULA", typeof(string));
                products.Columns.Add("NOMBRE", typeof(string));
                products.Columns.Add("CODIGO PRESUPUESTARIO", typeof(string));
                products.Columns.Add("DEPENDENCIA PRESUPUESTARIA", typeof(string));
                products.Columns.Add("RIGE", typeof(string));
                products.Columns.Add("VENCE", typeof(string));
                products.Columns.Add("MOTIVO", typeof(string));
                var contador = 0;

                foreach (var item in movimientosPuestos)
                {
                    contador++;
                    products.Rows.Add(contador.ToString(),
                                      item.Puesto.CodPuesto,
                                      item.Puesto.DetallePuesto.Clase != null ? item.Puesto.DetallePuesto.Clase.DesClase : "NO TIENE",
                                      item.Puesto.DetallePuesto.Especialidad != null ? item.Puesto.DetallePuesto.Especialidad.DesEspecialidad : "NO TIENE",
                                      item.Puesto.DetallePuesto.SubEspecialidad != null ? item.Puesto.DetallePuesto.SubEspecialidad.DesSubEspecialidad : "NO TIENE",
                                      item.Mensaje.Split('-')[0],
                                      item.Mensaje.Split('-')[1],
                                      item.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto,
                                      item.Puesto.UbicacionAdministrativa.Seccion.NomSeccion,
                                      item.FecMovimiento.ToShortDateString(),
                                      item.FechaVencimiento.Year > 1 ? item.FechaVencimiento.ToShortDateString() : "",
                                      item.MotivoMovimiento.DesMotivo
                                      );
                }

                products.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "");

                products.Rows.Add("SIRH", "Fecha:", DateTime.Now.ToShortDateString(), "Usuario:", principal.Identity.Name.Split('\\')[1], "", "", "", "", "", "", "");

                var grid = new GridView();
                grid.DataSource = products;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.AddHeader("Transfer-Encoding", "identity");
                Response.ContentType = "application/ms-excel";

                //Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);
                Response.Write(style);
                //Response.Write(headerTable);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.Close();
                Response.End();
                return View();


                //foreach (var item in movimientosPuestos)
                //{
                //    if(item.Puesto.EstadoPuesto.IdEntidad != 24 || (item.Puesto.EstadoPuesto.IdEntidad == 24 && modelo.PermisoVacantes))
                //    {
                //        MovimientoPuestoVM modelR = new MovimientoPuestoVM
                //        {
                //            FechaMovimiento = item.FecMovimiento,
                //            FechaVencimiento = item.FechaVencimiento,
                //            MotivoMovimiento = new CMotivoMovimientoDTO
                //            {
                //                DesMotivo = item.MotivoMovimiento?.DesMotivo
                //            },
                //            Puesto = new CPuestoDTO
                //            {
                //                CodPuesto = item.Puesto.CodPuesto,
                //                DetallePuesto = new CDetallePuestoDTO
                //                {
                //                    Clase = new CClaseDTO
                //                    {
                //                        DesClase = item.Puesto?.DetallePuesto?.Clase?.DesClase
                //                    },
                //                    Especialidad = new CEspecialidadDTO
                //                    {
                //                        DesEspecialidad = item.Puesto?.DetallePuesto?.Especialidad?.DesEspecialidad
                //                    },
                //                    SubEspecialidad = new CSubEspecialidadDTO
                //                    {
                //                        DesSubEspecialidad = item.Puesto?.DetallePuesto?.SubEspecialidad?.DesSubEspecialidad
                //                    }
                //                }
                //            }
                //        };
                //        modelo1.Add(MovimientoPuestoRPT.GenerarReporteMovimientoPuesto(modelR));
                //    }

                //}
                //return ReporteMovimientos(modelo1, "ReporteMovimientoPuestoGeneral.rpt", "EXCEL");
            }
            catch (Exception error)
            {
                Session["errorM"] = "error";
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte de funcionarios, ponerse en contacto con el personal autorizado. \n");
                return PartialView("_ErrorMovimientoPuesto");
            }

        }

        public ActionResult GenerarReporteCosto()
        {
            Session["errorM"] = null;
            try
            {

                MovimientoPuestoVM modelo = new MovimientoPuestoVM();
                List<MovimientoPuestoRPT> modelo1 = new List<MovimientoPuestoRPT>();

                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                if (!Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                    modelo.PermisoVacantes = true;
                else
                    modelo.PermisoVacantes = false;

                //DateTime? FechaD = model.FechaDesde == null ? (DateTime?)null : DateTime.Parse(model.FechaDesde);
                //DateTime? FechaH = model.FechaHasta == null ? (DateTime?)null : DateTime.Parse(model.FechaHasta);

                //int motivo = model.MotivoSeleccionado == null ? 0 : Convert.ToInt32(model.MotivoSeleccionado);

                //var movimientosPuestos = puestoService.BuscarMovimientoPuestosFiltros(model.CodPuesto, model.CodCedula, motivo, FechaH, FechaD);
                var movimientosPuestos = (List<CMovimientoPuestoDTO>)Session["movimientos"];

                string style = @"<style> TD { mso-number-format:\@; } </style>";
                //var headerTable = @"<Table><tr><td><img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNqlnu1iMrzA69ctdlBJvavSQ8d8bKAUtilw&usqp=CAU"" \></td></tr></Table>";
                var products = new System.Data.DataTable("teste");

                //products.Rows.Add("SIRH", "Fecha:", DateTime.Now.ToShortDateString(), "Usuario:", principal.Identity.Name.Split('\\')[1], "", "", "", "", "");

                products.Columns.Add("PROGRAMA", typeof(string));
                products.Columns.Add("N° DE PUESTO", typeof(string));
                products.Columns.Add("CODIGO DE CLASE", typeof(string));
                products.Columns.Add("DESCRIPCIÓN DE LA CLASE", typeof(string));
                products.Columns.Add("FECHA REAL DE LA VACANTE", typeof(string));
                products.Columns.Add("VACANTE DESDE", typeof(string));
                products.Columns.Add("VACANTE HASTA", typeof(string));
                products.Columns.Add("SALARIO BASE", typeof(string));
                products.Columns.Add("097-CARRERA PROFESIONAL", typeof(string));
                products.Columns.Add("103-DEDICACIÓN EXCLUSIVA", typeof(string));
                products.Columns.Add("115-PROHIBICIÓN", typeof(string));
                products.Columns.Add("123-RECONOCIMIENTO POR RIESGO PENITENCIARIO", typeof(string));
                products.Columns.Add("125-ANUALIDADES", typeof(string));
                products.Columns.Add("TOTAL PLUSES", typeof(string));
                products.Columns.Add("00401-CONTRIBUCIÓN PATRONAL AL SEGURO SOCIAL CCSS", typeof(string));
                products.Columns.Add("00405-CONTRIBUCIÓN PATRONAL BP", typeof(string));
                products.Columns.Add("00501-CONTRIBUCIÓN PATRONAL AL SEGURO DE PENSIONES CCSS", typeof(string));
                products.Columns.Add("00502-APORTE PATRONAL ROP", typeof(string));
                products.Columns.Add("00503-APORTE PATRONAL FCL", typeof(string));
                products.Columns.Add("60103-200-CCSS CONTRIBUCIÓN ESTATAL SEGURO", typeof(string));
                products.Columns.Add("60103-202-CCSS CONTRIBUCIÓN ESTATAL SEGURO", typeof(string));
                products.Columns.Add("TOTAL CONTRIBUCIONES", typeof(string));
                products.Columns.Add("SUBTOTAL REMUNERACIONES Y CONTRIBUCIONES SOCIALES", typeof(string));
                products.Columns.Add("AGUINALDO", typeof(string));
                products.Columns.Add("COSTO TOTAL PARA REBAJO EN EL PRESUPUESTO EXTRAORDINARIO", typeof(string));
                products.Columns.Add("COSTO ANUALIZADO", typeof(string));
                products.Columns.Add("VACANTE EXCEPTUADA (SI/NO)", typeof(string));
                products.Columns.Add("EXCEPCIÓN 1-DOCENTE 2-POLICIAL 3-SNG", typeof(string));
                products.Columns.Add("VACANTE UTILIZADA (SI/NO)", typeof(string));
                products.Columns.Add("NÚMERO ACUERDO AP", typeof(string));
                products.Columns.Add("FECHA DEL NOMBRAMIENTO", typeof(string));
                products.Columns.Add("OBSERVACIONES", typeof(string));

                foreach (var item in movimientosPuestos)
                {
                    products.Rows.Add(item.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto,
                                      item.Puesto.CodPuesto,
                                      item.Puesto.DetallePuesto.Clase != null ? item.Puesto.DetallePuesto.Clase.IdEntidad.ToString() : "NO TIENE",
                                      item.Puesto.DetallePuesto.Clase != null ? item.Puesto.DetallePuesto.Clase.DesClase : "NO TIENE",
                                      item.FecMovimiento.ToShortDateString(),
                                      item.FecMovimiento.ToShortDateString(),
                                      item.FechaVencimiento.Year > 1 ? item.FechaVencimiento.ToShortDateString() : "",
                                      item.MotivoMovimiento.DesMotivo
                                      );
                }

                products.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "");

                products.Rows.Add("SIRH", "Fecha:", DateTime.Now.ToShortDateString(), "Usuario:", principal.Identity.Name.Split('\\')[1], "", "", "", "", "", "", "");

                var grid = new GridView();
                grid.DataSource = products;
                grid.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                Response.AddHeader("Transfer-Encoding", "identity");
                Response.ContentType = "application/ms-excel";

                //Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);
                Response.Write(style);
                //Response.Write(headerTable);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.Close();
                Response.End();
                return View();
            }
            catch (Exception error)
            {
                Session["errorM"] = "error";
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte de funcionarios, ponerse en contacto con el personal autorizado. \n");
                return PartialView("_ErrorMovimientoPuesto");
            }

        }

        private CrystalReportPdfResult ReporteMovimientos(List<MovimientoPuestoRPT> modelo, string reporte, string tipo)
        {
            string reportPath = Path.Combine(Server.MapPath("~/Reports/MovimientoPuesto"), reporte);
            return new CrystalReportPdfResult(reportPath, modelo, tipo);
        }
    }
}