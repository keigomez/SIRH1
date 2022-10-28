using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ComponentePresupuestarioLocal;
using SIRH.DTO;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Helpers;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.UserValidation;
using SIRH.Web.Reports.Planilla;

using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.PuestoService;
//using SIRH.Web.PlanillaService;
using SIRH.Web.FuncionarioService;
using SIRH.Web.AccionPersonalService;
//using SIRH.Web.PuestoLocal;
using SIRH.Web.PlanillaLocal;
//using SIRH.Web.FuncionarioLocal;

namespace SIRH.Web.Controllers
{
    public class PlanillaController : Controller
    {

        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CComponentePresupuestarioServiceClient servicioComponente = new CComponentePresupuestarioServiceClient();
        CPuestoServiceClient servicioPuesto = new CPuestoServiceClient();
        CPlanillaServiceClient servicioPlanilla = new CPlanillaServiceClient();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();

        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        // GET: Planilla
        public ActionResult Index()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Planilla)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
            }
            else
            {
                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CComponentePresupuestarioDTO).Name));
                return View();
            }
        }

        public ActionResult SearchHistoricoPlanilla()
        {
            CHistoricoPlanillaDTO model = new CHistoricoPlanillaDTO();
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchHistoricoPlanilla(CHistoricoPlanillaDTO model)
        {
            try
            {
                if (model.Cedula == null && (model.FechaFinal.Subtract(model.FechaInicio).TotalDays > 31))
                {
                    ModelState.AddModelError("Busqueda", "Para consultas de planilla mayores a un mes debe digitar la cédula del funcionario a consultar, ya que la consulta devolvería una gran cantidad de resultados.");
                    throw new Exception("Busqueda");
                }

                BusquedaHistoricoPlanillaVM modeloPartial = new BusquedaHistoricoPlanillaVM();
                var resultado = servicioPlanilla.BuscarDatosPlanilla(model.Cedula, model.FechaInicio, model.FechaFinal);

                if (resultado.ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    ModelState.AddModelError("Busqueda", "Entro al if");

                    modeloPartial.Historico = new List<CHistoricoPlanillaDTO>();
                    foreach (var item in resultado)
                    {
                        ModelState.AddModelError("Busqueda", "Entro al foreach");
                        if (item != null)
                        {
                            ModelState.AddModelError("Busqueda", "el salario no es nulo");
                            decimal quincena = ((CHistoricoPlanillaDTO)item).SalarioMensual != null ? Convert.ToDecimal(((CHistoricoPlanillaDTO)item).SalarioMensual.Replace(',', '.')) / 2 : 0;
                            CHistoricoPlanillaDTO temp = (CHistoricoPlanillaDTO)item;
                            temp.SalarioQuincenal = quincena.ToString();
                            modeloPartial.Historico.Add(temp);
                        }
                    }
                    modeloPartial.TotalRegistros = modeloPartial.Historico.Count;
                    modeloPartial.TotalPaginas = Convert.ToInt32(Math.Ceiling((double)modeloPartial.Historico.Count / 10));
                    modeloPartial.PaginaActual = 1;
                    ModelState.AddModelError("Busqueda", "armó el paginador");
                    Session["HistoricoPlanilla"] = modeloPartial;
                    ModelState.AddModelError("Busqueda", "hizo la sesión");
                    BusquedaHistoricoPlanillaVM modeloPartialFinal = new BusquedaHistoricoPlanillaVM();
                    modeloPartialFinal.PaginaActual = modeloPartial.PaginaActual;
                    modeloPartialFinal.TotalPaginas = modeloPartial.TotalPaginas;
                    modeloPartialFinal.TotalRegistros = modeloPartial.TotalRegistros;
                    modeloPartialFinal.Historico = modeloPartial.Historico.GetRange(((modeloPartial.PaginaActual - 1) * 10), 10).ToList();
                    ModelState.AddModelError("Busqueda", "genero el modelo parcial" + modeloPartial.ToString());
                    return PartialView("_SearchHistoricoPlanillaResults", modeloPartialFinal);
                    //throw new Exception("Busqueda");
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.ElementAt(0)).MensajeError);
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", error.Message);
                return PartialView("_ErrorPlanilla");
            }
        }

        public ActionResult MoverPaginadorHistoricoPlanilla(int page)
        {
            try
            {
                var modeloPartial = (BusquedaHistoricoPlanillaVM)Session["HistoricoPlanilla"];

                if (modeloPartial != null)
                {
                    modeloPartial.PaginaActual = page;

                    BusquedaHistoricoPlanillaVM modeloPartialFinal = new BusquedaHistoricoPlanillaVM();

                    modeloPartialFinal.PaginaActual = modeloPartial.PaginaActual;
                    modeloPartialFinal.TotalPaginas = modeloPartial.TotalPaginas;
                    modeloPartialFinal.TotalRegistros = modeloPartial.TotalRegistros;

                    if ((((modeloPartial.PaginaActual - 1) * 10) + 10) > modeloPartial.TotalRegistros)
                    {
                        modeloPartialFinal.Historico = modeloPartial.Historico.GetRange(((modeloPartial.PaginaActual - 1) * 10), (modeloPartial.TotalRegistros) - (((modeloPartial.PaginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modeloPartialFinal.Historico = modeloPartial.Historico.GetRange(((modeloPartial.PaginaActual - 1) * 10), 10).ToList();
                    }

                    return PartialView("_SearchHistoricoPlanillaResults", modeloPartialFinal);
                }
                else
                {
                    ModelState.AddModelError("Busqueda", "No se encontaron resultados para la búsqueda indicada");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", error.Message);
                return PartialView("_ErrorPlanilla");
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteBusquedaHistorico(string btnSubmit)
        {
            List<HistoricoPlanillaRptData> modelo = new List<HistoricoPlanillaRptData>();

            var modeloPartial = (BusquedaHistoricoPlanillaVM)Session["HistoricoPlanilla"];

            var Total = modeloPartial.Historico.Sum(Q => Convert.ToDecimal(Q.SalarioQuincenal));

            var PromedioQuincena = Total / modeloPartial.Historico.Count;

            var PromedioDia = PromedioQuincena / 15;

            foreach (var item in modeloPartial.Historico)
            {
                //comentario
                modelo.Add(HistoricoPlanillaRptData.GenerarDatosReporte(item, Total, PromedioQuincena, PromedioDia, String.Empty));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Planilla"), "ReporteBusquedaHistoricoRPT.rpt");
            if (btnSubmit.Contains("pdf"))
            {
                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            else
            {
                return new CrystalReportPdfResult(reportPath, modelo, "EXCEL");
            }
        }

        public ActionResult DetailsHistorico(int codigo)
        {
            CHistoricoPlanillaDTO modelo = new CHistoricoPlanillaDTO();
            try
            {
                var resultado = servicioPlanilla.ObtenerPagoID(codigo);

                if (resultado.GetType() != typeof(CErrorDTO))
                {
                    modelo = (CHistoricoPlanillaDTO)resultado;
                    decimal quincena = Convert.ToDecimal(((CHistoricoPlanillaDTO)modelo).SalarioMensual.Replace(',', '.')) / 2;
                    modelo.SalarioQuincenal = quincena.ToString();
                    return View(modelo);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado).MensajeError);
                }
            }
            catch (Exception error)
            {
                modelo.Mensaje = error.Message;
                return View(modelo);
            }
        }

        public ActionResult DetailsMovimiento(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Planilla)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Planilla)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesIncapacidades.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesIncapacidades.Operativo))] != null)
                {
                    FormularioPlanillaVM model = new FormularioPlanillaVM();

                    var datos = servicioComponente.ObtenerMovimientoPresupuesto(codigo);

                    if (datos.Count() > 0)
                    {
                        model.ComponentePresupuestario = (CComponentePresupuestarioDTO)datos.ElementAt(0);

                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
                }
            }
        }


        public ActionResult CreateMovimiento()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Planilla)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Planilla)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesPlanillas.Operativo))] != null)
                {
                    FormularioPlanillaVM model = new FormularioPlanillaVM();


                    var tiposMovimientos = servicioComponente.DescargarCatMovimientoPresupuesto()
                        .Where(Q => Q.IdEntidad != 2)
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CCatMovimientoPresupuestoDTO)Q).IdEntidad.ToString(),
                            Text = ((CCatMovimientoPresupuestoDTO)Q).DesMovimientoPresupuesto

                        });

                    var programas = servicioComponente.DescargarProgramas()
                                .Select(Q => new SelectListItem
                                {
                                    Value = ((CProgramaDTO)Q).IdEntidad.ToString(),
                                    Text = ((CProgramaDTO)Q).DesPrograma
                                });

                    var objetosGasto = servicioComponente.DescargarObjetosGasto()
                                .Select(Q => new SelectListItem
                                {
                                    Value = ((CObjetoGastoDTO)Q).IdEntidad.ToString(),
                                    Text = ((CObjetoGastoDTO)Q).CodObjGasto + " - " + ((CObjetoGastoDTO)Q).DesObjGasto
                                });


                    model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                    model.Programas = new SelectList(programas, "Value", "Text");
                    model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");


                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
                }
            }
        }


        // POST: /Planilla/Create
        [HttpPost]
        public ActionResult CreateMovimiento(FormularioPlanillaVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    //if (ModelState.IsValid == true)
                    //{
                    var datosMovimientos = servicioComponente.ListarMovimientosPresupuesto(model.ComponentePresupuestario.AnioPresupuesto);

                    if (datosMovimientos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        List<FormularioPlanillaVM> componentes = new List<FormularioPlanillaVM>();

                        foreach (var item in datosMovimientos)
                        {
                            FormularioPlanillaVM temp = new FormularioPlanillaVM();
                            temp.ComponentePresupuestario = (CComponentePresupuestarioDTO)item;


                            componentes.Add(temp);
                        }

                        return PartialView("_FormularioPlanilla", componentes);
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda",
                            ((CErrorDTO)datosMovimientos.FirstOrDefault()).MensajeError);
                        throw new Exception("Busqueda");
                    }
                    //}
                    //else
                    //{
                    //    throw new Exception("Busqueda");
                    //}
                }
                else
                {
                    //if (ModelState.IsValid == true)
                    //{
                    CCatMovimientoPresupuestoDTO tipoMovimiento = new CCatMovimientoPresupuestoDTO
                    {
                        IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                    };

                    CProgramaDTO programa = new CProgramaDTO
                    {
                        IdEntidad = Convert.ToInt32(model.ProgramaSeleccionado)
                    };

                    CObjetoGastoDTO objetoGasto = new CObjetoGastoDTO
                    {
                        IdEntidad = Convert.ToInt32(model.ObjetoGastoSeleccionado)
                    };


                    var respuesta = servicioComponente.GuardarComponentePresupuestario(programa, objetoGasto, tipoMovimiento, model.ComponentePresupuestario);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        if (((CRespuestaDTO)respuesta).Codigo > 0)
                        {
                            List<string> entidades = new List<string>();
                            entidades.Add(typeof(CComponentePresupuestarioDTO).Name);

                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla),
                                    Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                    CAccesoWeb.ListarEntidades(entidades.ToArray()));

                            return JavaScript("window.location = '/Planilla/DetailsMovimiento?codigo=" +
                                                ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                        }
                        else
                        {
                            //model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                            //model.Programas = new SelectList(programas, "Value", "Text");
                            //model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");

                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        //model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                        //model.Programas = new SelectList(programas, "Value", "Text");
                        //model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");

                        ModelState.AddModelError("Agregar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                    //}
                    //else
                    //{
                    //    //model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                    //    //model.Programas = new SelectList(programas, "Value", "Text");
                    //    //model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");

                    //    throw new Exception("Formulario");
                    //}
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorIncapacidad");
                }
                else
                {
                    return PartialView("_FormularioIncapacidad", model);
                }
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleMovimiento(FormularioPlanillaVM model)
        {
            List<DetalleMovimientoRptData> modelo = new List<DetalleMovimientoRptData>();

            modelo.Add(DetalleMovimientoRptData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Planilla"), "DetalleMovimientoRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo);

        }


        public ActionResult SearchI()
        {
            return View();
        }

        public ActionResult SearchSIRH()
        {
            return View();
        }

        public ActionResult Compare()
        {
            return View();
        }

        public ActionResult CalculateAP()
        {
            return View();
        }

        public ActionResult IncludeP()
        {
            return View();
        }

        public ActionResult Modify()
        {
            return View();
        }

        public ActionResult SearchPresupuesto()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Planilla)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Planilla)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesPlanillas.Operativo))] != null)
                {
                    FormularioPlanillaVM model = new FormularioPlanillaVM();


                    var tiposMovimientos = servicioComponente.DescargarCatMovimientoPresupuesto()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CCatMovimientoPresupuestoDTO)Q).IdEntidad.ToString(),
                            Text = ((CCatMovimientoPresupuestoDTO)Q).DesMovimientoPresupuesto

                        });

                    var programas = servicioComponente.DescargarProgramas()
                                .Select(Q => new SelectListItem
                                {
                                    Value = ((CProgramaDTO)Q).IdEntidad.ToString(),
                                    Text = ((CProgramaDTO)Q).DesPrograma
                                });

                    var objetosGasto = servicioComponente.DescargarObjetosGasto()
                                .Select(Q => new SelectListItem
                                {
                                    Value = ((CObjetoGastoDTO)Q).IdEntidad.ToString(),
                                    Text = ((CObjetoGastoDTO)Q).CodObjGasto + " - " + ((CObjetoGastoDTO)Q).DesObjGasto
                                });


                    model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                    model.Programas = new SelectList(programas, "Value", "Text");
                    model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");


                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Planilla) });
                }
            }
        }


        // POST: /Planilla/Create
        [HttpPost]
        public ActionResult SearchPresupuesto(FormularioPlanillaVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    //if (ModelState.IsValid == true)
                    //{
                    var datosMovimientos = servicioComponente.ListarMovimientosPresupuesto(model.ComponentePresupuestario.AnioPresupuesto);

                    if (datosMovimientos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        List<FormularioPlanillaVM> componentes = new List<FormularioPlanillaVM>();

                        foreach (var item in datosMovimientos)
                        {
                            FormularioPlanillaVM temp = new FormularioPlanillaVM();
                            temp.ComponentePresupuestario = (CComponentePresupuestarioDTO)item;


                            componentes.Add(temp);
                        }

                        return PartialView("_FormularioPlanilla", componentes);
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda",
                            ((CErrorDTO)datosMovimientos.FirstOrDefault()).MensajeError);
                        throw new Exception("Busqueda");
                    }
                    //}
                    //else
                    //{
                    //    throw new Exception("Busqueda");
                    //}
                }
                else
                {
                    if (ModelState.IsValid == true)
                    {
                        CCatMovimientoPresupuestoDTO tipoMovimiento = new CCatMovimientoPresupuestoDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };

                        CProgramaDTO programa = new CProgramaDTO
                        {
                            IdEntidad = Convert.ToInt32(model.ProgramaSeleccionado)
                        };

                        CObjetoGastoDTO objetoGasto = new CObjetoGastoDTO
                        {
                            IdEntidad = Convert.ToInt32(model.ObjetoGastoSeleccionado)
                        };

                        CComponentePresupuestarioDTO componentePresupuestario = new CComponentePresupuestarioDTO();

                        var respuesta = servicioComponente.GuardarComponentePresupuestario(programa, objetoGasto, tipoMovimiento, componentePresupuestario);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)respuesta).Codigo > 0)
                            {
                                List<string> entidades = new List<string>();
                                entidades.Add(typeof(CComponentePresupuestarioDTO).Name);

                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades),
                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                        CAccesoWeb.ListarEntidades(entidades.ToArray()));

                                return JavaScript("window.location = '/RegistroIncapacidad/Details?codigo=" +
                                                    ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                            }
                            else
                            {
                                //model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                                //model.Programas = new SelectList(programas, "Value", "Text");
                                //model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");

                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            //model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                            //model.Programas = new SelectList(programas, "Value", "Text");
                            //model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");

                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        //model.TiposMovimiento = new SelectList(tiposMovimientos, "Value", "Text");
                        //model.Programas = new SelectList(programas, "Value", "Text");
                        //model.ObjetoGasto = new SelectList(objetosGasto, "Value", "Text");

                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorIncapacidad");
                }
                else
                {
                    return PartialView("_FormularioIncapacidad", model);
                }
            }
        }

        #region DeduccionTemporal

        public ActionResult DeduccionSearch()
        {
            List<SelectListItem> listadoUsuarios = null;

            BusquedaDeduccionTemporalVM model = new BusquedaDeduccionTemporalVM();
            List<string> estadosPage = new List<string>();
            estadosPage.Add("Seleccionar Estado");
            estadosPage.Add("Registrada");
            estadosPage.Add("Aprobada");
            estadosPage.Add("Anulada");

            model.Estados = new SelectList(estadosPage);

            var tiposDeduccion = servicioPlanilla.ListarTiposDeduccion()
                                    .Select(Q => new SelectListItem
                                    {
                                        Value = ((CTipoDeduccionTemporalDTO)Q).IdEntidad.ToString(),
                                        Text = ((CTipoDeduccionTemporalDTO)Q).DetalleTipoDeduccionTemporal
                                    });

            model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");
            var usuario = principal.Identity.Name;
            context.IniciarSesionModulo(Session, usuario, Convert.ToInt32(EModulosHelper.Planilla), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Planilla)].ToString().StartsWith("Error"))
            {
                model.PermisoAdministrador = false;
                model.PermisoRegistrar = false;
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Planilla)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesPlanillas.Administrador))] != null)
                {
                    model.PermisoAdministrador = true;

                    listadoUsuarios = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.Planilla), Convert.ToInt32(ENivelesPlanillas.Operativo))
                      .Select(Q => new SelectListItem
                      {
                          Value = ((CUsuarioDTO)Q[1]).IdEntidad.ToString(),
                          Text = ((CFuncionarioDTO)Q[0]).Nombre.TrimEnd().ToString() + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido.TrimEnd().ToString() + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido.TrimEnd().ToString()
                      }).ToList();
                }
                else
                {
                    model.PermisoAdministrador = false;
                    if ( Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesPlanillas.Operativo))] != null)
                        model.PermisoRegistrar = true;
                    else
                        model.PermisoRegistrar = false;

                    var valor = "";
                    var texto = "";
                    var usr = servicioUsuario.ObtenerUsuarioPorNombre(usuario);
                    if (usr.ElementAt(0).GetType().Equals(typeof(CErrorDTO)) == false)
                    {
                        if (usr.ElementAt(1).ElementAt(0).GetType().Equals(typeof(CErrorDTO)) == false)
                        {
                            valor = ((CUsuarioDTO)usr[0][0]).IdEntidad.ToString();
                            texto = ((CFuncionarioDTO)usr[1][0]).Nombre.TrimEnd().ToString() + " " + ((CFuncionarioDTO)usr[1][0]).PrimerApellido.TrimEnd().ToString() + " " + ((CFuncionarioDTO)usr[1][0]).SegundoApellido.TrimEnd().ToString();
                        }
                    }

                    listadoUsuarios = new List<SelectListItem>();
                    listadoUsuarios.Add(new SelectListItem() { Value = valor, Text = texto });
                }
            }

            model.Usuarios  = new SelectList(listadoUsuarios, "Value", "Text");
            return View(model);
        }

        [HttpPost]
        public ActionResult DeduccionSearch(BusquedaDeduccionTemporalVM model)
        {
            ModelState.Clear();
            try
            {
                List<DateTime> fechas = new List<DateTime>();
                List<DateTime> fechasBitacora = new List<DateTime>();
                model.Deduccion = new CDeduccionTemporalDTO();
                model.Funcionario.Sexo = GeneroEnum.Indefinido;

                if ((model.Funcionario.Cedula != String.Empty && model.Funcionario.Cedula != null) ||
                        (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1)
                        || (model.FechaBitacoraHasta.Year > 1 && model.FechaBitacoraHasta.Year > 1)
                        || model.EstadoSeleccionado != "Seleccionar Estado"
                        || model.TipoSeleccionado != null
                        || model.UsuarioSeleccionado != null)

                {
                    if ((model.FechaEmisionDesde.Year > 1) != (model.FechaEmisionHasta.Year > 1))
                    {
                        ModelState.AddModelError("Fecha", "Debe seleccionar tanto la fecha de inicio como la fecha final, para realizar una búsqueda con rango de fechas");
                        throw new Exception("Busqueda");
                    }
                    else
                    {
                        if (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1)
                        {
                            fechas.Add(model.FechaEmisionDesde);
                            fechas.Add(model.FechaEmisionHasta);
                        }
                    }

                    if (model.FechaBitacoraDesde.Year > 1 && model.FechaBitacoraHasta.Year > 1)
                    {
                        fechasBitacora.Add(model.FechaBitacoraDesde);
                        fechasBitacora.Add(model.FechaBitacoraHasta);
                    }

                    model.Deduccion.DatoTipoDeduccionTemporal = model.TipoSeleccionado != null ?
                                                                new CTipoDeduccionTemporalDTO { IdEntidad = Convert.ToInt32(model.TipoSeleccionado) }
                                                                : null;

                    switch (model.EstadoSeleccionado)
                    {
                        case "Registrada" :
                            model.Deduccion.Estado = 0;
                            break;
                        case "Aprobada" :
                            model.Deduccion.Estado = 1;
                            break;
                        case "Anulada" :
                            model.Deduccion.Estado = 2;
                            break;
                        default:
                            model.Deduccion.Estado = -1;
                            break;
                    }
                    //model.Deduccion.Estado = model.EstadoSeleccionado != "Seleccionar Estado" ?
                    //                         model.EstadoSeleccionado == "Activa" ? 1 : 2
                    //                         : -1;

                    CBitacoraUsuarioDTO entidadBitacora = new CBitacoraUsuarioDTO
                    {
                        CodigoAccion = 1, // Guardar
                        CodigoModulo = Convert.ToInt32(EModulosHelper.Planilla),
                        Usuario = new CUsuarioDTO{ IdEntidad = Convert.ToInt32(model.UsuarioSeleccionado) }
                    };


                    var datos = servicioPlanilla.BuscarDeducciones(model.Funcionario, model.Deduccion, entidadBitacora, fechas.ToArray(), fechasBitacora.ToArray());

                    if (datos.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        model.Deducciones = new List<FormularioDeduccionTemporalVM>();
                        var listado = datos.OrderBy(Q => ((CFuncionarioDTO)Q.ElementAt(1)).PrimerApellido)
                                                   .ThenBy(Q => ((CFuncionarioDTO)Q.ElementAt(1)).SegundoApellido)
                                                   .ThenBy(Q => ((CFuncionarioDTO)Q.ElementAt(1)).Nombre)
                                                   .ToList();

                        if (model.CampoOrdenar == "Núm. Expediente")
                            listado = listado.OrderBy(Q => ((CExpedienteFuncionarioDTO)Q.ElementAt(4)).NumeroExpediente).ToList();

                        foreach (var item in listado)
                        {
                            FormularioDeduccionTemporalVM temp = new FormularioDeduccionTemporalVM
                            {
                                Deduccion = (CDeduccionTemporalDTO)item.ElementAt(0),
                                Funcionario = (CFuncionarioDTO)item.ElementAt(1),
                                Puesto = (CPuestoDTO)item.ElementAt(2),
                                Bitacora = (CBitacoraUsuarioDTO)item.ElementAt(3),
                                Expediente = (CExpedienteFuncionarioDTO)item.ElementAt(4),
                                MostrarDato = (model.PermisoAdministrador || model.PermisoRegistrar) ? true : false,
                                PermisoAprobar = model.PermisoAdministrador,
                                PermisoRegistrar = model.PermisoRegistrar
                            };

                            temp.DatoTipoDeduccion = temp.Deduccion.DatoTipoDeduccionTemporal;
                            model.Deducciones.Add(temp);
                        }

                        return PartialView("_SearchResultsDeduccion", model.Deducciones);
                    }
                    else
                    {
                        ModelState.AddModelError("Datos", ((CErrorDTO)datos.FirstOrDefault().FirstOrDefault()).MensajeError);
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.AddModelError("Datos", "Debe seleccionar al menos uno de los parámetros de búsqueda establecidos en el formulario.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorPlanilla");
                }
                else
                {
                    ModelState.AddModelError("Datos", error.Message);
                    return PartialView("_ErrorPlanilla");
                }
            }
        }

        public ActionResult CreateDeduccion()
        {
            FormularioDeduccionTemporalVM model = new FormularioDeduccionTemporalVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateDeduccion(FormularioDeduccionTemporalVM model, string SubmitButton)
        {
            try
            {
                var tiposDeduccion = servicioPlanilla.ListarTiposDeduccion()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CTipoDeduccionTemporalDTO)Q).IdEntidad.ToString(),
                            Text = ((CTipoDeduccionTemporalDTO)Q).DetalleTipoDeduccionTemporal,
                        });

                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        //var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        var datosFuncionario = servicioAccion.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                            model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");

                            var datosSalario = servicioFuncionario.BuscarFuncionarioSalario(model.Funcionario.Cedula);
                            if (datosSalario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                model.Salario = (CSalarioDTO)datosSalario[1];
                                return PartialView("_FormularioDeduccion", model);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)datosSalario.FirstOrDefault()).MensajeError);
                                throw new Exception("Busqueda");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda",
                                ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");

                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");

                    if (ModelState.IsValid == true)
                    {

                        if (model.Deduccion.NumeroDocumento == null || model.Deduccion.NumeroDocumento == "")
                        {
                            //model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");
                            ModelState.AddModelError("Formulario", "Debe digitar el Número de Documento");
                            throw new Exception("Formulario");
                        }

                        if (model.Deduccion.Periodo == null || model.Deduccion.Periodo == "")
                        {
                            //model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");
                            ModelState.AddModelError("Formulario", "Debe digitar el Periodo");
                            throw new Exception("Formulario");
                        }

                        CTipoDeduccionTemporalDTO entidad = new CTipoDeduccionTemporalDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };

                        model.Funcionario.Sexo = GeneroEnum.Indefinido;
                        model.Deduccion.DatoTipoDeduccionTemporal = entidad;

                        var respuesta = servicioPlanilla.AgregarDeduccionTemporal(model.Funcionario, model.Deduccion);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (respuesta.IdEntidad > 0)
                            {
                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla),
                                                        Convert.ToInt32(EAccionesBitacora.Guardar), respuesta.IdEntidad,
                                                        CAccesoWeb.ListarEntidades(typeof(CDeduccionTemporalDTO).Name));

                                //return JavaScript("window.location = '/Planilla/DetailsDeduccion?id=" +
                                //                     respuesta.IdEntidad + "&accion=guardar" + "';");

                                return new JavaScriptResult("CargarMensajeRegistro(" + respuesta.IdEntidad + ");");
                            }
                            else
                            {
                                // model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");

                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            // model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");

                            ModelState.AddModelError("Agregar", ((CErrorDTO)respuesta).MensajeError);

                            throw new Exception(((CErrorDTO)respuesta).MensajeError);
                        }
                    }
                    else
                    {
                        // model.Tipos = new SelectList(tiposDeduccion, "Value", "Text");

                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorPlanilla");
                }
                else
                {
                    return PartialView("_FormularioDeduccion", model);
                }
            }
        }

        [HttpGet]
        public ActionResult DetailsDeduccion(int id)
        {
            FormularioDeduccionTemporalVM model = new FormularioDeduccionTemporalVM();

            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Planilla)].ToString().StartsWith("Error"))
            {
                model.PermisoAprobar = false;
                model.PermisoRegistrar = false;
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Planilla)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesPlanillas.Administrador))] != null)
                {
                    model.PermisoAprobar = true;
                }
                if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Planilla, Convert.ToInt32(ENivelesPlanillas.Operativo))] != null)
                {
                    model.PermisoRegistrar = true;
                }
            }
            CDeduccionTemporalDTO deduccion = new CDeduccionTemporalDTO { IdEntidad = id };
            var datos = servicioPlanilla.DescargarDetalleDeduccion(deduccion);

            if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.Deduccion = (CDeduccionTemporalDTO)datos.ElementAt(0);
                model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                model.Puesto = (CPuestoDTO)datos.ElementAt(2);
                model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(3);
                model.DatoTipoDeduccion = model.Deduccion.DatoTipoDeduccionTemporal;
            }
            else
            {
                ModelState.AddModelError("Datos", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
                throw new Exception("Busqueda");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult EditDeduccion(int id, string accion)
        {
            FormularioDeduccionTemporalVM model = new FormularioDeduccionTemporalVM();
            CDeduccionTemporalDTO deduccion = new CDeduccionTemporalDTO { IdEntidad = id };
            var datos = servicioPlanilla.DescargarDetalleDeduccion(deduccion);

            if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.Deduccion = (CDeduccionTemporalDTO)datos.ElementAt(0);
                model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                model.Puesto = (CPuestoDTO)datos.ElementAt(2);
                model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(3);
                model.DatoTipoDeduccion = model.Deduccion.DatoTipoDeduccionTemporal;
                model.PermisoAprobar = (accion == "aprobar") ? true : false;
            }
            else
            {
                ModelState.AddModelError("Datos", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
                throw new Exception("Busqueda");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditDeduccion(FormularioDeduccionTemporalVM model, string SubmitButton)
        {
            if (SubmitButton == "Aprobar")
            {
                var datos = servicioPlanilla.AprobarDeduccionTemporal(model.Deduccion);

                if (datos.GetType() != typeof(CErrorDTO))
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla),
                                                       Convert.ToInt32(EAccionesBitacora.Editar), model.Deduccion.IdEntidad,
                                                       CAccesoWeb.ListarEntidades(typeof(CDeduccionTemporalDTO).Name));

                    return JavaScript("ObtenerDetalleAprobacion(" + model.Deduccion.IdEntidad + ");");
                }
                else
                {
                    ModelState.AddModelError("Datos", ((CErrorDTO)datos).MensajeError);
                    return PartialView(model);
                }
            }
            else
            {
                var datos = servicioPlanilla.AnularDeduccionTemporal(model.Deduccion);

                if (datos.GetType() != typeof(CErrorDTO))
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Planilla),
                                                       Convert.ToInt32(EAccionesBitacora.Anular), model.Deduccion.IdEntidad,
                                                       CAccesoWeb.ListarEntidades(typeof(CDeduccionTemporalDTO).Name));

                    return JavaScript("ObtenerDetalleAnulacion(" + model.Deduccion.IdEntidad + ");");
                    //return RedirectToAction("DetailsDeduccion", new { id = model.Deduccion.IdEntidad, accion = "modificar" });
                }
                else
                {
                    ModelState.AddModelError("Datos", ((CErrorDTO)datos).MensajeError);
                    return PartialView(model);
                }
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDeduccionTemporalDetalle(FormularioDeduccionTemporalVM model)
        {
            string reportPath = "0";
            try
            {
                List<DeduccionTemporalRptData> modelo = new List<DeduccionTemporalRptData>();

                modelo.Add(DeduccionTemporalRptData.GenerarDatosReporte(model, String.Empty));

                reportPath = Path.Combine(Server.MapPath("~/Reports/Planilla"), "DeduccionTemporalRpt.rpt");

                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(reportPath + " " + ex.Message);
            }
        }


        public CrystalReportPdfResult ReporteDeducciones(List<FormularioDeduccionTemporalVM> model)
        {
            string reportPath = "0";
            try
            {
                List<DeduccionTemporalRptData> modelo = new List<DeduccionTemporalRptData>();

                foreach (var item in model)
                {
                    modelo.Add(DeduccionTemporalRptData.GenerarDatosReporte(item, String.Empty));
                }

                reportPath = Path.Combine(Server.MapPath("~/Reports/Planilla"), "ReporteDeduccionesTemporales.rpt");

                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(reportPath + " " + ex.Message);
            }
        }

        [HttpPost]
        public ActionResult GetTipoDeduccion(int? idTipo)
        {
            try
            {
                if (idTipo != null)
                {
                    var datos = (CRespuestaDTO)servicioPlanilla.ObtenerTipoDeduccion(Convert.ToInt16(idTipo));
                    var tipo = ((CTipoDeduccionTemporalDTO)datos.Contenido);
                    return Json(new { success = true, tipo = tipo.IndConSalario }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, tipo = "0", mensaje = "No existe el tipo" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, tipo = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditExplicacion(int idDeduccion, string obsModificada)
        {
            try
            {
                var datoDeduccion = new CDeduccionTemporalDTO
                {
                    IdEntidad = idDeduccion,
                    Explicacion = obsModificada
                };
                var respuesta = servicioPlanilla.ModificarDeduccionExplicacion(datoDeduccion);
                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    return Json(new
                    {
                        success = true,
                        mensaje = "Se modificó la Explicación del registro de deducción"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta).MensajeError);
                }
            }
            catch (Exception error)
            {
                return Json(new { success = false, mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        #endregion
    }
}