using SIRH.DTO;
//using SIRH.Web.FuncionarioLocal;
using SIRH.Web.FuncionarioService;
using SIRH.Web.Helpers;
using SIRH.Web.NombramientoService;
//using SIRH.Web.NombramientoLocal;
using SIRH.Web.Reports.PDF;
using SIRH.Web.Reports.Vacaciones;
using SIRH.Web.UserValidation;
using SIRH.Web.VacacionesService;
//using SIRH.Web.VacacionesLocal;
using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SIRH.Web.Controllers
{
    public class VacacionesController : Controller
    {
        CAccesoWeb context = new CAccesoWeb();
        CVacacionesServiceClient ServicioVacaciones = new CVacacionesServiceClient();
        CFuncionarioServiceClient ServicioFuncionario = new CFuncionarioServiceClient();
        private static double uploadProgresses = 0;



        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        // GET: Vacaciones
        public ActionResult Index()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
                else
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), Convert.ToInt32(EAccionesBitacora.Login), 0,
                        CAccesoWeb.ListarEntidades(typeof(CPerfilDTO).Name));
                    return View();
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
        }
        public ActionResult ReintegroVacaciones()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacaciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
            }
        }
        [HttpPost]
        public ActionResult ReintegroVacaciones(ReintegroVacacionesVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuesto(model.PeriodoVacaciones.Nombramiento.Funcionario.Cedula);

                        var datosRegistro = ServicioVacaciones.ObtenerRegistro(model.PeriodoVacaciones.Nombramiento.Funcionario.Cedula, model.RegistroVacaciones.NumeroTransaccion);



                        if (datosRegistro[0].GetType() != typeof(CErrorDTO) && datosFuncionario[0].GetType() != typeof(CErrorDTO))
                        {
                            model.PeriodoVacaciones = (CPeriodoVacacionesDTO)datosRegistro[1];
                            model.RegistroVacaciones = (CRegistroVacacionesDTO)datosRegistro[0];
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                            return PartialView("_FormularioReintegro", model);
                        }
                        else
                        {
                            if (datosRegistro[0].GetType() == typeof(CErrorDTO) && datosFuncionario[0].GetType() != typeof(CErrorDTO))
                            {
                                ModelState.AddModelError("BusquedaPeriodo",
                                   ((CErrorDTO)datosRegistro[0]).MensajeError);
                                throw new Exception("BusquedaPeriodo");
                            }
                            else
                                if (datosRegistro[0].GetType() != typeof(CErrorDTO) && datosFuncionario[0].GetType() == typeof(CErrorDTO))
                            {
                                ModelState.AddModelError("BusquedaPeriodo",
                                  ((CErrorDTO)datosFuncionario[0]).MensajeError);
                                throw new Exception("BusquedaFuncionario");
                            }
                            else
                            {
                                ModelState.AddModelError("BusquedaPeriodo",
                                   ((CErrorDTO)datosRegistro[0]).MensajeError);
                                ModelState.AddModelError("BusquedaPeriodo",
                                  ((CErrorDTO)datosFuncionario[0]).MensajeError);
                                throw new Exception("BusquedaFuncionario");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("BusquedaPeriodo");
                    }

                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                    if (ModelState.IsValid)
                    {
                        model.ReintegroVacaciones.Mensaje = model.PeriodoVacaciones.Periodo;
                        var reintegra = ServicioVacaciones.RegistraReintegroVacaciones(model.RegistroVacaciones.NumeroTransaccion, model.ReintegroVacaciones, model.Funcionario.Cedula);
                        if (((CRespuestaDTO)reintegra).Codigo > 0)
                        {
                            return JavaScript("window.location = '/Vacaciones/Details?cedula=" +
                                                                              model.Funcionario.Cedula + "&CodPeriodo=" +
                                                                               model.PeriodoVacaciones.Periodo + "&Codigo=" + ((CRespuestaDTO)reintegra).Codigo + "&detalle=3'");
                        }
                        else
                        {
                            ModelState.AddModelError("Error",
                              ((CRespuestaDTO)reintegra).Mensaje);
                            throw new Exception("Guardar");
                        }

                    }
                    else 
                    {
                        return PartialView("_ErrorVacaciones");
                    }
                }
            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }
        }
        public ActionResult RegistrarVacaciones()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacaciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
            }
        }
        [HttpPost]
        public ActionResult RegistrarVacaciones(FormularioRegistroVacacionesVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuesto(model.PeriodoVacaciones.Nombramiento.Funcionario.Cedula);

                        var datosPeriodo = ServicioVacaciones.ListarPeriodosActivos(model.PeriodoVacaciones.Nombramiento.Funcionario.Cedula).OrderBy(P => ((CPeriodoVacacionesDTO)P).Periodo).ToList();

                        List<CPeriodoVacacionesDTO> datosDescarga = new List<CPeriodoVacacionesDTO>();

                        if (datosPeriodo[0].GetType() != typeof(CErrorDTO))
                        {
                            foreach (CPeriodoVacacionesDTO item in datosPeriodo)
                            {
                                if (item.Saldo > 0)
                                {
                                    datosDescarga.Add(item);
                                }
                            }

                        }

                        if (datosDescarga.Count < 1)
                        {
                            datosDescarga.Add(new CPeriodoVacacionesDTO { IdEntidad = -1 });
                        }

                        if (datosFuncionario[0].GetType() != typeof(CErrorDTO))
                        {
                            if (datosDescarga[0].IdEntidad > -1)
                            {
                                //model.PeriodosActivos = new SelectList(datosPeriodo.Select(U => ((CPeriodoVacacionesDTO)U).Periodo));
                                model.PeriodoVacaciones = ((CPeriodoVacacionesDTO)datosDescarga.FirstOrDefault());
                                model.UltimoPeriodo = ((CPeriodoVacacionesDTO)datosDescarga.FirstOrDefault()).Periodo;
                                model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                                List<string> estadosTipoVacaciones = new List<string>();
                                estadosTipoVacaciones.Add("Vacaciones");
                                estadosTipoVacaciones.Add("Permiso con goce de salario");
                                estadosTipoVacaciones.Add("Proporcionales");
                                model.tipoDocumento = new SelectList(estadosTipoVacaciones);
                                return PartialView("_FormularioVacaciones", model);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "El funcionario no tiene periodos de vacaciones disponibles.");
                                throw new Exception("Error al cargar el modelo");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("BusquedaPeriodo",
                              ((CErrorDTO)datosFuncionario[0]).MensajeError);
                            throw new Exception("BusquedaFuncionario");
                        }
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }

                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                    if (ModelState.IsValid && model.UltimoPeriodo != null && model.tipoDocumentoSeleccion != null)
                    {
                        var vacaciones = 0;
                        if (model.tipoDocumentoSeleccion == "Vacaciones")
                        {
                            vacaciones = 1;
                        }
                        else
                            if (model.tipoDocumentoSeleccion == "Permiso sin goce de salario")
                        {
                            vacaciones = 6;
                        }
                        else
                        {
                            vacaciones = 5;
                        }
                        var guardarDatos = ServicioVacaciones.GuardarRegistroVacaciones(model.RegistroVacaciones, model.Funcionario.Cedula, model.UltimoPeriodo, vacaciones
                            );
                        if (((CRespuestaDTO)guardarDatos).Codigo > 0)
                        {
                            return JavaScript("window.location = '/Vacaciones/Details?cedula=" +
                                                                              model.Funcionario.Cedula + "&CodPeriodo=" +
                                                                               model.UltimoPeriodo + "&Codigo=" + ((CRespuestaDTO)guardarDatos).Codigo + "&detalle=4'");
                        }
                        else
                        {
                            ModelState.AddModelError("BusquedaPeriodo",
                              ((CRespuestaDTO)guardarDatos).Mensaje);
                            throw new Exception("Guardar");
                        }
                    }
                    else
                    {
                        if (model.UltimoPeriodo != null)
                        {
                            ModelState.AddModelError("Busqueda", "El periodo de vacaciones es obligatorio.");
                            throw new Exception("Error al cargar el modelo");
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", "El tipo de transacción es obligatorio.");
                            throw new Exception("Error al cargar el modelo");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }
        }
        public ActionResult Search()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacaciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
            }
        }
        [HttpPost]
        public ActionResult Search(DetalleVacacionesVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var detallePeriodos = ServicioVacaciones.ObtenerDetalleVacaciones(model.Funcionario.Cedula);
                        var periodosActivos = ServicioVacaciones.ListarPeriodosActivos(model.Funcionario.Cedula);
                        var periodosInactivos = ServicioVacaciones.ListarPeriodosNoActivos(model.Funcionario.Cedula);
                        var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuestoVacaciones(model.Funcionario.Cedula);

                        var periodosActuales = new List<string>();


                        if (periodosActivos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            foreach (var item in periodosActivos)
                            {
                                periodosActuales.Add(((CPeriodoVacacionesDTO)item).Periodo);
                            }
                        }

                        if (periodosInactivos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            foreach (var item in periodosInactivos)
                            {
                                periodosActuales.Add(((CPeriodoVacacionesDTO)item).Periodo);
                            }
                        }

                        var periodosEmulacion = ServicioVacaciones.BuscarHistorialPeriodoVacacionesSinActuales(model.Funcionario.Cedula, periodosActuales.ToArray());

                        if ((detallePeriodos[0][0].GetType() != typeof(CErrorDTO) 
                            || detallePeriodos[1][0].GetType() != typeof(CErrorDTO))
                            && datosFuncionario[0].GetType() != typeof(CErrorDTO))
                        {
                            model.DetalleContratacion = (CDetalleContratacionDTO)ServicioVacaciones.DetalleContratacion(model.Funcionario.Cedula);
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];

                            //model.PeriodosActivos = detallePeriodos.ElementAt(0).Select(F => (CPeriodoVacacionesDTO)F).ToList();
                            if (periodosActivos[0].GetType() != typeof(CErrorDTO))
                            {
                                model.PeriodosActivos = periodosActivos.Select(F => (CPeriodoVacacionesDTO)F).ToList();
                                foreach (var item in model.PeriodosActivos)
                                {
                                    var fechaCumplimiento = new DateTime(DateTime.Now.Year,
                                                                         model.DetalleContratacion.FechaIngreso.Month,
                                                                         model.DetalleContratacion.FechaIngreso.Day);
                                    if (item.DiasDerecho == 26)
                                    {
                                        item.ProporcionMes = Math.Round(Convert.ToDouble(item.DiasDerecho / 12), 2) - 0.01;
                                    }
                                    else
                                    {
                                        item.ProporcionMes = Math.Round(Convert.ToDouble(item.DiasDerecho / 12), 2);
                                    }
                                    item.Proporcion = Math.Round((DateTime.Now.Subtract(fechaCumplimiento).Days / 30) * item.ProporcionMes, 2);
                                }
                            }
                            if (periodosInactivos[0].GetType() != typeof(CErrorDTO))
                            {
                                model.PeriodosNoActivos = periodosInactivos.Select(F => (CPeriodoVacacionesDTO)F).ToList();
                            }
                            if (periodosEmulacion[0].GetType() != typeof(CErrorDTO))
                            {
                                model.PeriodosEmulacion = periodosEmulacion.Select(F => (CPeriodoVacacionesDTO)F).ToList();
                            }

                            return PartialView("_SearchDetails", model);
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda",
                                  ((CErrorDTO)detallePeriodos[0][0]).MensajeError
                              );
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }

        }

        [HttpPost]
        public ActionResult ReintegrarRegistro(FormCollection form)
        {
            try
            {
                var fechainicioOriginal = Convert.ToDateTime(form["fechaInicioOriginal"]);
                var fechafinOriginal = Convert.ToDateTime(form["fechaFinOriginal"]);

                if ((Convert.ToDateTime(form["fecInicio"]) < fechainicioOriginal || Convert.ToDateTime(form["fecInicio"]) > fechafinOriginal)
                    || (Convert.ToDateTime(form["fecFin"]) > fechafinOriginal || Convert.ToDateTime(form["fecFin"]) < fechainicioOriginal))
                {
                    ModelState.AddModelError("errorActualizar", "El rango de fechas seleccionado no coincide con el registro de vacaciones a reintegrar");
                    return PartialView("_ErrorVacaciones");
                }

                CReintegroVacacionesDTO reintegro = new CReintegroVacacionesDTO
                {
                    CantidadDias = Convert.ToDecimal(form["diasReintegro"]),
                    FechaActualizacion = DateTime.Now,
                    FechaRige = Convert.ToDateTime(form["fecInicio"]),
                    FechaVence = Convert.ToDateTime(form["fecFin"]),
                    Fuente = form["fuente"],
                    Motivo = 99,
                    Observaciones = form["obsReintegro"],
                    SolReintegro = form["docReintegro"],
                    RegistroVacaciones = new CRegistroVacacionesDTO { IdEntidad = Convert.ToInt32(form["identRegistro"]) }
                };

                var resultado = ServicioVacaciones.RegistrarReintegroRegistro(reintegro);

                if (resultado.GetType() != typeof(CErrorDTO))
                {
                    return JavaScript("window.location = '/Vacaciones/Details?cedula=" +
                            form["Funcionario.Cedula"] + "&codPeriodo=" +
                            form["Periodo.Periodo"] + "&codigo=" + resultado.IdEntidad + "&detalle=6'");
                }
                else
                {
                    ModelState.AddModelError("errorActualizar", ((CErrorDTO)resultado).MensajeError);
                    return PartialView("_ErrorVacaciones");
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("errorActualizar", error.Message);
                return PartialView("_ErrorVacaciones");
            }

        }

        public ActionResult Reportes()
        {
            if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacaciones)]) ||
                Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Consulta))] != null ||
                Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Operativo))] != null)
            {
                BusquedaVacacionesReportesVM model = new BusquedaVacacionesReportesVM();
                List<string> estados = new List<string>();
                estados.Add("Registros");
                estados.Add("Reintegros");
                estados.Add("Rebajo colectivo");
                List<string> estadosTipoVacaciones = new List<string>();
                estadosTipoVacaciones.Add("Vacaciones");
                estadosTipoVacaciones.Add("Permiso sin goce de salario");
                estadosTipoVacaciones.Add("Proporcionales");
                model.Direcciones = new SelectList(ServicioVacaciones.ListarDireccionGeneral().Select(U => ((CDireccionGeneralDTO)U).NomDireccion));
                model.Divisiones = new SelectList(ServicioVacaciones.ListarDivisiones().Select(U => ((CDivisionDTO)U).NomDivision));
                model.Departamentos = new SelectList(ServicioVacaciones.ListarDepartamentos().Select(U => ((CDepartamentoDTO)U).NomDepartamento));
                model.Secciones = new SelectList(ServicioVacaciones.ListarSecciones().Select(U => ((CSeccionDTO)U).NomSeccion));
                model.Estados = new SelectList(estados);
                model.tipoRegistroVacaciones = new SelectList(estadosTipoVacaciones);
                return View(model);
            }
            else
            {
                CAccesoWeb.CargarErrorAcceso(Session);
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
        }
        [HttpPost]
        public ActionResult Reportes(BusquedaVacacionesReportesVM model)
        {
            try
            {
                if (model.EstadoSeleccion != null && (model.Funcionario.Cedula != null || model.SeleccionTipoVacaciones != null || model.Periodo.Periodo != null || model.Registro.NumeroTransaccion != null
                    || model.DepartamentoSeleccion != null || model.DireccionSeleccion != null
                    || model.DivisionSeleccion != null || model.SeccionSeleccion != null || model.DepartamentoSeleccion != null || (model.FechaFinalVacaciones.Year > 1 &&
                    model.FechaInicioVacaciones.Year > 1)))
                {
                    var filtro = new List<string>();
                    List<DateTime> fechas = new List<DateTime>();
                    if (model.FechaInicioVacaciones.Year > 1 && model.FechaFinalVacaciones.Year > 1)
                    {
                        fechas.Add(model.FechaFinalVacaciones);
                        fechas.Add(model.FechaInicioVacaciones);
                        filtro.Add(" Fechas");
                    }
                    if (model.EstadoSeleccion == null)
                    {
                        model.EstadoSeleccion = "null";
                    }
                    if (model.SeleccionTipoVacaciones == null)
                    {
                        model.SeleccionTipoVacaciones = "null";
                    }
                    if (model.DepartamentoSeleccion == null)
                    {
                        model.DepartamentoSeleccion = "null";
                    }
                    if (model.DireccionSeleccion == null)
                    {
                        model.DireccionSeleccion = "null";
                    }
                    if (model.DivisionSeleccion == null)
                    {
                        model.DivisionSeleccion = "null";
                    }
                    if (model.SeccionSeleccion == null)
                    {
                        model.SeccionSeleccion = "null";
                    }
                    model.Funcionario.Sexo = GeneroEnum.Indefinido;
                    if (model.EstadoSeleccion == "Registros" || model.EstadoSeleccion == "Rebajo colectivo")
                    {
                        if (model.EstadoSeleccion == "Rebajo colectivo" && model.SeleccionTipoVacaciones != "null")
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Busqueda", "Para buscar por tipo de vacaciones debe seleccionar tipo de registro vacaciones.");
                            throw new Exception("Busqueda");
                        }
                        var datos = ServicioVacaciones.BuscarVacaciones(model.Funcionario, model.Periodo, model.Registro, fechas.ToArray(),
                            model.DireccionSeleccion, model.SeccionSeleccion, model.DivisionSeleccion, model.DepartamentoSeleccion, model.EstadoSeleccion, model.SeleccionTipoVacaciones);
                        if (datos[0][0].GetType() != typeof(CErrorDTO))
                        {
                            if (model.EstadoSeleccion == "Rebajo colectivo")
                            {
                                model.RegistroRebajoColectivo = true;
                            }

                            model.RegistroVacaciones = datos.ElementAt(0).Select(F => (CRegistroVacacionesDTO)F).ToList();

                            model.RegistroFuncionarios = datos.ElementAt(1).Select(F => (CFuncionarioDTO)F).ToList();
                            model.PeriodosVacaciones = datos.ElementAt(2).Select(F => (CPeriodoVacacionesDTO)F).ToList();
                            return PartialView("_Reportes", model);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Busqueda",
                                  ((CErrorDTO)datos[0][0]).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        if (model.EstadoSeleccion == "Reintegros" && model.SeleccionTipoVacaciones != "null")
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Busqueda", "Para buscar por tipo de vacaciones debe seleccionar tipo de registro vacaciones.");
                            throw new Exception("Busqueda");
                        }

                        var datosReintegros = ServicioVacaciones.BuscarReintegros(model.Funcionario, model.Periodo, model.Registro, fechas.ToArray(), model.DireccionSeleccion, model.SeccionSeleccion, model.DivisionSeleccion, model.DepartamentoSeleccion);

                        if (datosReintegros[0][0].GetType() != typeof(CErrorDTO))
                        {
                            model.ReintegrosVacaciones = datosReintegros.ElementAt(0).Select(F => (CReintegroVacacionesDTO)F).ToList();
                            model.RegistroFuncionarios = datosReintegros.ElementAt(1).Select(F => (CFuncionarioDTO)F).ToList();
                            model.PeriodosVacaciones = datosReintegros.ElementAt(2).Select(F => (CPeriodoVacacionesDTO)F).ToList();
                            return PartialView("_Reportes", model);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Busqueda",
                                  ((CErrorDTO)datosReintegros[0][0]).MensajeError);
                            throw new Exception("Busqueda");
                        }

                    }

                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaFinalVacaciones.Year < 2 && model.FechaInicioVacaciones.Year > 2 || model.FechaFinalVacaciones.Year > 2 && model.FechaInicioVacaciones.Year > 2)
                    {
                        ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                    }
                    if (model.EstadoSeleccion == null)
                    {
                        ModelState.AddModelError("Busqueda", "Debe ingresar un tipo de registo.");

                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    }
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }

        }
        public ActionResult CreatePeriodo()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacaciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
            }
        }
        [HttpPost]
        public ActionResult CreatePeriodo(FormularioPeriodoVacacionesVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid)
                    {
                        var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuestoVacaciones(model.Funcionario.Cedula);
                        if (datosFuncionario[0].GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                            return PartialView("_FormularioPeriodo", model);
                        }
                        else
                        {
                            ModelState.AddModelError("BusquedaPeriodo",
                                  ((CErrorDTO)datosFuncionario[0]).MensajeError);
                            throw new Exception("BusquedaPeriodo");
                        }

                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                    if (ModelState.IsValid && model.PeriodoVacaciones.Periodo != null)
                    {
                        var guardarDatos = ServicioVacaciones.GuardaRegistroPeriodo(model.PeriodoVacaciones, model.Funcionario.Cedula);
                        if (((CBaseDTO)guardarDatos).IdEntidad < 0)
                        {
                            ModelState.AddModelError("Error al ingresar", ((CRespuestaDTO)guardarDatos).Mensaje);
                            throw new Exception("Guardar");
                        }
                        else
                        {
                            return JavaScript("window.location = '/Vacaciones/Details?cedula=" +
                                                  model.Funcionario.Cedula + "&CodPeriodo=" +
                                                   model.PeriodoVacaciones.Periodo + "&Codigo=" + ((CBaseDTO)guardarDatos).IdEntidad + "&detalle=2'");
                        }
                    }

                    else
                    {
                        if (model.PeriodoVacaciones.Periodo == null)
                        {
                            ModelState.AddModelError("Periodo", "El periodo es obligatorio.");

                        }
                        throw new Exception("Error al cargar el modelo");

                    }

                }
            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }
        }
        public ActionResult Details(string cedula, string codPeriodo, int codigo, int detalle = 0)
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
                else
                {
                    DetalleVacacionesVM model = new DetalleVacacionesVM();
                    model.detalle = detalle;
                    var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuestoVacaciones(cedula);
                    var datosPeriodo = ServicioVacaciones.ObtenerPeriodo(cedula, codPeriodo, codigo);
                    var periodosActivos = ServicioVacaciones.ListarPeriodosActivos(cedula);
                    if (periodosActivos[0].GetType() != typeof(CErrorDTO))
                    {
                        var datosperiodos = periodosActivos.Where(P => P.IdEntidad != datosPeriodo.IdEntidad).ToList();
                        model.PeriodosActivos = datosperiodos.Select(F => (CPeriodoVacacionesDTO)F).ToList();
                    }
                    else
                    {
                        model.PeriodosActivos = null;
                    }
                    model.Periodo = (CPeriodoVacacionesDTO)datosPeriodo;
                    model.codigoDetalle = codigo;
                    if (codigo > 0)
                    {
                        var datosRegistroVacaciones = ServicioVacaciones.ListarRegistroVacaciones(cedula, codPeriodo, codigo);
                        var datosReintegro = ServicioVacaciones.ListarReintegrosPeriodos(cedula, codPeriodo, codigo);
                        if (datosRegistroVacaciones.ElementAt(0).GetType() != typeof(CErrorDTO))
                        {
                            model.RegistroVacaciones = datosRegistroVacaciones.Select(F => (CRegistroVacacionesDTO)F).ToList();
                            model.TotalRegistros = datosRegistroVacaciones.Sum(F => ((CRegistroVacacionesDTO)F).Dias);
                            if (model.TotalRegistros > model.Periodo.DiasDerecho)
                            {
                                model.Alerta = "¡ATENCIÓN! La sumatoria de días disfrutados de este periodo es mayor a los días a derecho, por favor revise la información para solventar esta inconsistencia.";
                            }
                        }
                        else
                        {
                            model.RegistroVacaciones = new List<CRegistroVacacionesDTO>();
                        }
                        if (datosReintegro.Count() > 0 && datosReintegro.ElementAt(0).GetType() != typeof(CErrorDTO))
                        {
                            model.ReintegroVacaciones = datosReintegro.Select(F => (CReintegroVacacionesDTO)F).ToList();
                            model.TotalReintegros = datosReintegro.Sum(F => ((CReintegroVacacionesDTO)F).CantidadDias);
                        }
                        else
                        {
                            model.ReintegroVacaciones = new List<CReintegroVacacionesDTO>();
                        }
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        model.codigoDetalle = codigo;
                        model.estadoPeriodo = DefinirEstadoPeriodo(model.Periodo.Estado);
                    }
                    return View(model);
                }
            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }
        }

        private string DefinirEstadoPeriodo(int estado)
        {
            switch (estado)
            {
                case 1:
                    return "Activo";
                case 2:
                    return "Vencido";
                case 3:
                    return "Anulado";
                default:
                    return "Indefinido";
            }
        }

        [HttpPost]
        public ActionResult ActualizarDatos(int idRegistro, string mensaje, FormCollection form)
        {
            try
            {
                var resultado = ServicioVacaciones.ActualizarDatosVacaciones(idRegistro, mensaje, form.Count > 4 ? Convert.ToDecimal(form[4]) : 0);
                //return RedirectToAction("Details", "Vacaciones", new { cedula = form[3], codPeriodo = form[2], codigo = idRegistro });
                if (resultado.GetType() != typeof(CErrorDTO))
                {
                    if (mensaje != "anular")
                    {
                        return JavaScript("window.location = '/Vacaciones/Details?cedula=" +
                          form[3] + "&CodPeriodo=" +
                           form[2] + "&Codigo=" + idRegistro + "&detalle=5'");
                    }
                    else
                    {
                        return JavaScript("window.location = '/Vacaciones/Search'");
                    }
                }
                else
                {
                    ModelState.AddModelError("errorActualizar",((CErrorDTO)resultado).MensajeError);
                    return PartialView("_ErrorVacaciones");
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("errorActualizar", error.Message);
                return PartialView("_ErrorVacaciones");
            }
        }

        public ActionResult DetalleHistorial(string cedula, string periodo, string ex = "no")
        {
            DetalleVacacionesVM model = new DetalleVacacionesVM();
            try
            {
                var respuesta = ServicioVacaciones.BuscarHistorialMovimientoVacaciones(cedula, periodo);

                if (respuesta.ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    if (ex == "si")
                    {
                        var datosFuncionario = ServicioFuncionario.BuscarExfuncionarioFiltros(cedula, "", "", "", "");
                        model.DetalleContratacion = new CDetalleContratacionDTO
                        {
                            Mensaje = datosFuncionario[0].NumeroExpediente,
                            FechaIngreso = Convert.ToDateTime(datosFuncionario[0].FechaIngreso),
                            FechaCese = Convert.ToDateTime(datosFuncionario[0].FechaCese)
                        };
                        model.Funcionario = new CFuncionarioDTO
                        {
                            Cedula = datosFuncionario[0].Cedula,
                            Nombre = datosFuncionario[0].Nombre,
                            PrimerApellido = datosFuncionario[0].PrimerApellido,
                            SegundoApellido = datosFuncionario[0].SegundoApellido,
                            EstadoFuncionario = new CEstadoFuncionarioDTO { DesEstadoFuncionario = datosFuncionario[0].DescEstado }
                        };
                        model.Puesto = new CPuestoDTO
                        {
                            CodPuesto = datosFuncionario[0].PuestoPropiedad,
                            UbicacionAdministrativa = new CUbicacionAdministrativaDTO
                            {
                                Division = new CDivisionDTO { NomDivision = datosFuncionario[0].Division },
                                DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = datosFuncionario[0].Direccion },
                                Departamento = new CDepartamentoDTO { NomDepartamento = datosFuncionario[0].Departamento },
                                Seccion = new CSeccionDTO { NomSeccion = datosFuncionario[0].Seccion }
                            }
                        };
                        model.DetallePuesto = new CDetallePuestoDTO
                        {
                            Clase = new CClaseDTO { DesClase = datosFuncionario[0].ClasePuesto }
                        };
                    }
                    else
                    {
                        var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuestoVacaciones(cedula);
                        model.RegistroVacaciones = new List<CRegistroVacacionesDTO>();
                        model.Periodo = new CPeriodoVacacionesDTO { Periodo = periodo };
                        model.DetalleContratacion = (CDetalleContratacionDTO)ServicioVacaciones.DetalleContratacion(cedula);
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                    }

                    model.RegistroVacaciones = new List<CRegistroVacacionesDTO>();
                    model.ReintegroVacaciones = new List<CReintegroVacacionesDTO>();
                    model.Periodo = new CPeriodoVacacionesDTO { Periodo = periodo };

                    foreach (var item in respuesta)
                    {
                        if (((CRegistroVacacionesDTO)item).TipoTransaccion != 99)
                        {
                            model.RegistroVacaciones.Add((CRegistroVacacionesDTO)item);
                            model.TotalRegistros += ((CRegistroVacacionesDTO)item).Dias;
                        }
                        else
                        {
                            model.ReintegroVacaciones.Add(new CReintegroVacacionesDTO
                            {
                                FechaRige = ((CRegistroVacacionesDTO)item).FechaRige,
                                SolReintegro = ((CRegistroVacacionesDTO)item).NumeroTransaccion,
                                CantidadDias = ((CRegistroVacacionesDTO)item).Dias
                            });

                            model.TotalReintegros += ((CRegistroVacacionesDTO)item).Dias;
                        }
                    }

                    return View(model);
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta.ElementAt(0)).MensajeError);
                }
            }
            catch (Exception error)
            {
                model.Error = new CErrorDTO { MensajeError = error.Message };
                return View("_ErrorVacaciones");
            }
        }


        [HttpPost]
        public ActionResult TrasladarRegistro(FormCollection form)
        {
            var cantidadPeriodos = Convert.ToInt32(form["PeriodosActivos.Count"]);
            int idRegistro = 0;
            decimal dias = 0;
            int periodoDestino = 0;

            for (int i = 0; i < cantidadPeriodos; i++)
            {
                var texto = form["textdiasTraslado" + i];
                if (texto != "")
                {
                    dias = Convert.ToDecimal(texto);
                    idRegistro = Convert.ToInt32(form["identRegistro"]);
                    periodoDestino = Convert.ToInt32(form["PeriodosActivos["+i+"].IdEntidad"]);
                    var respuesta = ServicioVacaciones.TrasladarRegistroVacaciones(idRegistro, dias, periodoDestino);
                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/Vacaciones/Details?cedula=" +
                                form["Funcionario.Cedula"] + "&CodPeriodo=" +
                                form["Periodo.Periodo"] + "&Codigo=" + form["Periodo.IdEntidad"] + "&detalle=5'");
                    }
                    else
                    {
                        ModelState.AddModelError("errorActualizar", ((CErrorDTO)respuesta).MensajeError);
                        return PartialView("_ErrorVacaciones");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalle(BusquedaVacacionesReportesVM model)
        {

            if (model.RegistroVacaciones != null)
            {
                List<ReporteRegistrosRpt> modelo = new List<ReporteRegistrosRpt>();
                for (int i = 0; i < model.RegistroVacaciones.Count(); i++)
                {
                    modelo.Add(ReporteRegistrosRpt.GenerarDatosReporte(model.RegistroVacaciones[i], model.RegistroFuncionarios[i], model.PeriodosVacaciones[i]));
                }
                string reportPath = Path.Combine(Server.MapPath("~/Reports/Vacaciones"), "ReporteRegistroRPT.rpt");
                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            else
            {
                List<ReporteReintegroRpt> modelo2 = new List<ReporteReintegroRpt>();
                for (int i = 0; i < model.ReintegrosVacaciones.Count(); i++)
                {
                    modelo2.Add(ReporteReintegroRpt.GenerarDatosReporte(model.ReintegrosVacaciones[i], model.RegistroFuncionarios[i], model.PeriodosVacaciones[i]));
                }
                string reportPath = Path.Combine(Server.MapPath("~/Reports/Vacaciones"), "ReporteReintegroRPT.rpt");
                return new CrystalReportPdfResult(reportPath, modelo2, "PDF");
            }
        }
        public ActionResult RebajoColectivo()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacaciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacaciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacaciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacaciones, Convert.ToInt32(ENivelesVacaciones.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacaciones) });
                }
            }
        }
        [HttpPost]
        public ActionResult RebajoColectivo(RebajoColectivoVM model, string SubmitButton)
        {
            try
            {
                CBaseDTO rebajo = new CBaseDTO();
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        uploadProgresses = 0;
                        var documento = ServicioVacaciones.ValidarNumeroDocumento(model.Registro.NumeroTransaccion);
                        if (((CRespuestaDTO)documento).Codigo > 0)
                        {
                            model.funcionariosInconsistencias = new List<CFuncionarioDTO>();
                            var funcionariosActivos = ServicioVacaciones.ListarFuncionariosActivos(model.seleccionSeguridad, model.seleccionPolicial);
                            if (funcionariosActivos[0].GetType() != typeof(CErrorDTO))
                            {
                                var porcentaje = 0;
                                foreach (var item in funcionariosActivos)
                                {
                                    porcentaje++;
                                    rebajo = ServicioVacaciones.GuardarRebajoColectivo((CFuncionarioDTO)item, model.Registro);
                                    //var inconsistencia = ServicioVacaciones.ObtenerInconsistencias(model.Registro
                                    //    .FechaRige, model.Registro.FechaVence, ((CFuncionarioDTO)item).Cedula);
                                    //if (inconsistencia[0][0].GetType() != typeof(CErrorDTO))
                                    //{
                                    //    model.registrosInconsistentes = inconsistencia.ElementAt(0).Select(F => (CRegistroVacacionesDTO)F).ToList();
                                    //    model.funcionariosInconsistencias = inconsistencia.ElementAt(1).Select(F => (CFuncionarioDTO)F).ToList();
                                    //}
                                    if (((CRespuestaDTO)rebajo).Codigo < 0)
                                    {
                                        uploadProgresses = -1;
                                        ModelState.AddModelError("Busqueda", "Ocurrio un error al realizar el rebajo colectivo");
                                        throw new Exception("Rebajo Colectivo");

                                    }
                                    else
                                    if (porcentaje == Convert.ToInt32((funcionariosActivos.Count() / 100)))
                                    {
                                        uploadProgresses++;
                                        porcentaje = 0;
                                        if (uploadProgresses > 100)
                                        {
                                            uploadProgresses = 100;
                                        }
                                    }
                                }
                                uploadProgresses = 100;
                                return PartialView("_RebajoColectivoInfo", model);
                            }
                            else
                            {
                                uploadProgresses = -1;
                                ModelState.AddModelError("Busqueda", "Ocurrio un error al intentar cargar los funcionarios");
                                throw new Exception("BusquedaPeriodo");
                            }
                        }
                        else
                        {
                            uploadProgresses = -1;
                            ModelState.AddModelError("DocumentoRepetido",
                          ((CRespuestaDTO)documento).Mensaje);
                            throw new Exception("BusquedaPeriodo");
                        }
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }

                }
                else
                {
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception e)
            {
                return PartialView("_ErrorVacaciones");
            }
        }
        [HttpPost]
        public ActionResult RebajoColectivoInfo(RebajoColectivoVM model, string SubmitButton)
        {
            try
            {
                CBaseDTO rebajo = new CBaseDTO();
                if (SubmitButton == "Anular")
                {
                    if (ModelState.IsValid == true)
                    {
                        var funcionariosActivos = ServicioVacaciones.ListarFuncionariosActivos(model.seleccionSeguridad, model.seleccionPolicial);
                        foreach (var item in funcionariosActivos)
                        {
                            ServicioVacaciones.AnularRebajoColectivo((CFuncionarioDTO)item, model.Registro.NumeroTransaccion);
                        }
                        return JavaScript("window.location = '/Vacaciones/AnularRebajo?numeroTransaccion=" +
                                                  model.Registro.NumeroTransaccion + "'");
                    }
                    else
                    {
                        throw new Exception("Anular");
                    }

                }
                else
                {
                    throw new Exception("Anular");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Busqueda", "Ocurrio un al intentar anular el rebajo colectivo.");
                return PartialView("_ErrorVacaciones");
            }
        }
        public ActionResult AnularRebajo(string numeroTransaccion)
        {
            RebajoColectivoVM model = new RebajoColectivoVM();
            model.numTransaccion = numeroTransaccion;
            return View(model);
        }
        public CrystalReportPdfResult ReporteFuncionario(string cedula)
        {
            List<PeriodoVacacionesRpt> modelo1 = new List<PeriodoVacacionesRpt>();
            List<DatosFuncionarioRpt> modelo2 = new List<DatosFuncionarioRpt>();
            BusquedaVacacionesReportesVM model = new BusquedaVacacionesReportesVM();
            var periodos = ServicioVacaciones.ListaPeriodos(cedula);
            if (periodos[0].GetType() != typeof(CErrorDTO))
            {
                model.PeriodosVacaciones = periodos.Select(F => (CPeriodoVacacionesDTO)F).ToList();
            }
            var datosFuncionario = ServicioFuncionario.BuscarFuncionarioDetallePuesto(cedula);
            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
            model.Funcionario.Mensaje = ((CDetalleContratacionDTO)datosFuncionario[3]).FechaVacaciones.ToShortDateString();
            modelo2.Add(DatosFuncionarioRpt.GenerarDatosReporte(model.Funcionario));
            if (model.PeriodosVacaciones != null)
            {
                for (int i = 0; i < model.PeriodosVacaciones.Count(); i++)
                {
                    var fechaCumplimiento = new DateTime(DateTime.Now.Year,
                                     ((CDetalleContratacionDTO)datosFuncionario[3]).FechaVacaciones.Month,
                                     ((CDetalleContratacionDTO)datosFuncionario[3]).FechaVacaciones.Day);
                    model.PeriodosVacaciones[i].ProporcionMes = Math.Round(Convert.ToDouble(model.PeriodosVacaciones[i].DiasDerecho / 12), 2);
                    model.PeriodosVacaciones[i].Proporcion = Math.Round((DateTime.Now.Subtract(fechaCumplimiento).Days / 30) * model.PeriodosVacaciones[i].ProporcionMes, 2);
                    modelo1.Add(PeriodoVacacionesRpt.GenerarDatosReporte(model.PeriodosVacaciones[i]));
                }
            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Vacaciones"), "DatosFuncionarioRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo2, modelo1);
        }

        public CrystalReportPdfResult ReporteInconsistencias(DateTime fechaInic, DateTime FechaVence, bool seguridad, bool transito)
        {
            List<InconsistenciaRebColRpt> modelo1 = new List<InconsistenciaRebColRpt>();
            var funcionarios = ServicioVacaciones.ListarFuncionariosActivos(seguridad, transito);
            foreach (var item in funcionarios)
            {
                var registros = ServicioVacaciones.ObtenerInconsistencias(fechaInic, FechaVence, ((CFuncionarioDTO)item).Cedula).ElementAt(0).Select(F => (CRegistroVacacionesDTO)F).ToList();
                foreach (var registro in registros)
                {
                    modelo1.Add(InconsistenciaRebColRpt.GenerarDatosReporte((CRegistroVacacionesDTO)registro, (CFuncionarioDTO)item));
                }
            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Vacaciones"), "InconsistenciaRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo1);
        }

        public ActionResult BuscarHistorial()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscarHistorial(DetalleVacacionesVM model)
        {
            try
            {
                var respuesta = ServicioVacaciones.BuscarHistorialPeriodoVacacionesSinActuales(model.Funcionario.Cedula, (new List<string>()).ToArray());
                var datosFuncionario = ServicioFuncionario.BuscarExfuncionarioFiltros(model.Funcionario.Cedula, "", "", "", "");

                if (respuesta.ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    model.DetalleContratacion = new CDetalleContratacionDTO
                    {
                        Mensaje = datosFuncionario[0].NumeroExpediente,
                        FechaIngreso = Convert.ToDateTime(datosFuncionario[0].FechaIngreso),
                        FechaCese = Convert.ToDateTime(datosFuncionario[0].FechaCese)
                    };
                    model.Funcionario = new CFuncionarioDTO
                    {
                        Cedula = datosFuncionario[0].Cedula,
                        Nombre = datosFuncionario[0].Nombre,
                        PrimerApellido = datosFuncionario[0].PrimerApellido,
                        SegundoApellido = datosFuncionario[0].SegundoApellido,
                        EstadoFuncionario = new CEstadoFuncionarioDTO { DesEstadoFuncionario = datosFuncionario[0].DescEstado }
                    };
                    model.Puesto = new CPuestoDTO
                    {
                        CodPuesto = datosFuncionario[0].PuestoPropiedad,
                        UbicacionAdministrativa = new CUbicacionAdministrativaDTO
                        {
                            Division = new CDivisionDTO { NomDivision = datosFuncionario[0].Division },
                            DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = datosFuncionario[0].Direccion },
                            Departamento = new CDepartamentoDTO { NomDepartamento = datosFuncionario[0].Departamento },
                            Seccion = new CSeccionDTO { NomSeccion = datosFuncionario[0].Seccion } 
                        }
                    };
                    model.DetallePuesto = new CDetallePuestoDTO
                    {
                        Clase = new CClaseDTO { DesClase = datosFuncionario[0].ClasePuesto }
                    };

                    model.PeriodosEmulacion = respuesta.Select(F => (CPeriodoVacacionesDTO)F).ToList();

                    return PartialView("_BuscarHistorialResult",model);
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta.ElementAt(0)).MensajeError);
                }
            }
            catch (Exception error)
            {
                model.Error = new CErrorDTO { MensajeError = error.Message };
                return PartialView("_BuscarHistorialResult", model);
            }
        }
    }
}