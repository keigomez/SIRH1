using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
using SIRH.Web.CaucionService;
using SIRH.DTO;
using SIRH.Web.Reports.PDF;
using SIRH.Web.Reports.Cauciones;
using System.IO;
using System.Security.Principal;
using SIRH.Web.Helpers;
using System.Threading;
using SIRH.Web.UserValidation;

namespace SIRH.Web.Controllers
{
    public class MontoCaucionController : Controller
    {
        CCaucionesServiceClient caucionService = new CCaucionesServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        //
        // GET: /MontoCaucion/Details/5

        public ActionResult Details(int id)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Cauciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Cauciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {
                    BusquedaMontosVM model = new BusquedaMontosVM();

                    var datos = caucionService.ObtenerMontoCaucion(id);

                    if (datos.GetType() != typeof(CErrorDTO))
                    {
                        model.MontoCaucion = (CMontoCaucionDTO)datos;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos;
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // GET: /MontoCaucion/Search

        public ActionResult Search()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Cauciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Cauciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {
                    BusquedaMontosVM model = new BusquedaMontosVM();
                    List<string> estadosPage = new List<string>();
                    estadosPage.Add("Seleccionar Estado");
                    estadosPage.Add("Activo");
                    estadosPage.Add("Inactivo");

                    model.Estados = new SelectList(estadosPage);

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // POST: /MontoCaucion/Search

        [HttpPost]
        public ActionResult Search(BusquedaMontosVM model)
        {
            try
            {
                ModelState.Clear();
                List<DateTime> fechas = new List<DateTime>();
                List<decimal> montos = new List<decimal>();


                // VALIDA CAMPOS DEL FORMULARIO (QUE NO ESTÉN VACÍOS)
                if (model.MontoCaucion.Descripcion != null ||
                    (model.FechaInicio.Year > 1 && model.FechaFin.Year > 1) ||
                    (model.MontoInicio > 0 && model.MontoFinal > 0) ||
                    model.EstadoSeleccionado != "Seleccionar Estado")
                {
                    if ((model.FechaInicio.Year > 1) != (model.FechaFin.Year > 1))
                    {
                        ModelState.AddModelError("Fecha", "Debe seleccionar tanto la fecha de inicio como la fecha final, para realizar una búsqueda con rango de fechas");
                        throw new Exception("Busqueda");
                    }
                    else
                    {
                        if (model.FechaInicio.Year > 1 && model.FechaFin.Year > 1)
                        {
                            fechas.Add(model.FechaInicio);
                            fechas.Add(model.FechaFin);
                        }
                    }

                    if ((model.MontoInicio > 0) != (model.MontoFinal > 0))
                    {
                        ModelState.AddModelError("Fecha", "Debe seleccionar tanto monto de inicio como el monto final, para realizar una búsqueda con rango de montos");
                        throw new Exception("Busqueda");
                    }
                    else
                    {
                        if (model.MontoInicio > 0 && model.MontoFinal > 0)
                        {
                            montos.Add(model.MontoInicio);
                            montos.Add(model.MontoFinal);
                        }
                    }

                    model.MontoCaucion.DetalleEstadoMonto = model.EstadoSeleccionado;

                    // LLAMA A LOS DATOS DE UN SERVICIO WEB (BD)
                    var datos = caucionService.BuscarMontosCaucion(model.MontoCaucion, fechas.ToArray(),
                                                                        montos.ToArray());

                    if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        List<CMontoCaucionDTO> resultsModel = new List<CMontoCaucionDTO>();

                        foreach (var item in datos)
                        {
                            resultsModel.Add((CMontoCaucionDTO)item);
                        }

                        return PartialView("_SearchResults", resultsModel);
                    }
                    else
                    {
                        //GENERACIÓN DE ERRORES
                        ModelState.AddModelError("Datos", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
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
                    return PartialView("_ErrorMontoCaucion");
                }
                else
                {
                    List<string> estadosPage = new List<string>();
                    estadosPage.Add("Seleccionar Estado");
                    estadosPage.Add("Activo");
                    estadosPage.Add("Inactivo");

                    model.Estados = new SelectList(estadosPage);
                    return View(model);
                }
            }
        }

        //
        // GET: /MontoCaucion/Create

        public ActionResult Create()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Cauciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Cauciones)]))
                {
                    BusquedaMontosVM model = new BusquedaMontosVM();
                    List<string> estadosPage = new List<string>();
                    estadosPage.Add("Seleccionar Estado");
                    estadosPage.Add("Activo");
                    estadosPage.Add("Inactivo");

                    model.Estados = new SelectList(estadosPage);

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // POST: /MontoCaucion/Create

        [HttpPost]
        public ActionResult Create(BusquedaMontosVM model)
        {
            try
            {
                if (model.EstadoSeleccionado == "Activo")
                {
                    ModelState.Where(S => S.Key == "MontoCaucion.FechaInactiva").FirstOrDefault().Value.Errors.Clear();
                    if (model.MontoCaucion.FechaRige > DateTime.Now)
                    {
                        ModelState.AddModelError("MontoCaucion.FechaInicio", "La fecha de rige para un nivel de caución, no puede ser superior al día de hoy");
                        throw new Exception("Validacion");
                    }
                }
                else
                {
                    if (model.EstadoSeleccionado == "Inactivo")
                    {
                        ModelState.Where(S => S.Key == "MontoCaucion.FechaInactiva").FirstOrDefault().Value.Errors.Clear();
                        if (model.MontoCaucion.FechaRige > DateTime.Now || model.MontoCaucion.FechaInactiva > DateTime.Now)
                        {
                            ModelState.AddModelError("MontoCaucion.FechaInicio", "La fecha de rige y la fecha de vence para un nivel de caución, no pueden ser superiores al día de hoy");
                            throw new Exception("Validacion");
                        }
                        if (model.MontoCaucion.FechaInactiva < model.MontoCaucion.FechaRige)
                        {
                            ModelState.AddModelError("MontoCaucion.FechaInicio", "La fecha de vencimiento para un nivel de caución, no puede ser anterior a la fecha de rige");
                            throw new Exception("Validacion");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("MontoCaucion.EstadoSeleccionado", "Debe seleccionar el estado del monto de caución");
                    }
                }

                if (ModelState.IsValid)
                {
                    model.MontoCaucion.EstadoMonto = model.EstadoSeleccionado == "Activo" ? 1 : 2;
                    var resultado = caucionService.AgregarMontoCaucion(model.MontoCaucion);

                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones),
                            Convert.ToInt32(EAccionesBitacora.Guardar), resultado.IdEntidad,
                            CAccesoWeb.ListarEntidades(typeof(CMontoCaucionDTO).Name));
                        return RedirectToAction("Details", new { id = resultado.IdEntidad, accion = "guardar" });
                        //return JavaScript("window.location = '/MontoCaucion/Details?id=" + resultado.IdEntidad + "&accion=guardar" + "'");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Validacion");
                }
            }
            catch (Exception error)
            {
                List<string> estadosPage = new List<string>();
                estadosPage.Add("Seleccionar Estado");
                estadosPage.Add("Activo");
                estadosPage.Add("Inactivo");
                model.Estados = new SelectList(estadosPage);

                if (error.Message != "Validacion")
                {
                    ModelState.AddModelError("BDError", error.Message);
                }

                return View(model);
            }
        }

        //
        // GET: /MontoCaucion/Edit/5

        public ActionResult Edit(int id)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Cauciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Cauciones)]))
                {
                    BusquedaMontosVM model = new BusquedaMontosVM();

                    var datos = caucionService.ObtenerMontoCaucion(id);

                    if (datos.GetType() != typeof(CErrorDTO))
                    {
                        model.MontoCaucion = (CMontoCaucionDTO)datos;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos;
                    }

                    if (model.MontoCaucion.EstadoMonto != 1)
                    {
                        model.Error = new CErrorDTO { MensajeError = "El nivel de caución no puede editarse debido a que su estado actual es inactivo." };
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // POST: /MontoCaucion/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, BusquedaMontosVM model)
        {
            try
            {
                if (model.MontoCaucion.FechaInactiva.Year > 1 && model.MontoCaucion.JustificacionInactiva != null)
                {
                    if (model.MontoCaucion.FechaInactiva > model.MontoCaucion.FechaRige)
                    {
                        model.MontoCaucion.IdEntidad = id;

                        var datos = caucionService.EditarMontoCaucion(model.MontoCaucion);

                        if (datos.GetType() != typeof(CErrorDTO))
                        {
                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones),
                                Convert.ToInt32(EAccionesBitacora.Editar), id,
                                CAccesoWeb.ListarEntidades(typeof(CMontoCaucionDTO).Name));
                            return JavaScript("window.location = '/MontoCaucion/Details?id=" + id + "&accion=guardar" + "'");
                        }
                        else
                        {
                            ModelState.AddModelError("Guardar", ((CErrorDTO)datos).MensajeError);
                            throw new Exception("Validacion");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Validacion", "La fecha de vencimiento del nivel no puede ser menor a la fecha de rige del mismo.");
                        throw new Exception("Validacion");
                    }
                }
                else
                {
                    ModelState.AddModelError("Validacion", "Debe digitar tanto la fecha de vencimiento como la justificación de inactividad para poder guardar el monto de caución.");
                    throw new Exception("Validacion");
                }
            }
            catch
            {
                return PartialView("_ErrorMontoCaucion");
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteMontoCaucion(List<CMontoCaucionDTO> model)
        {
            List<MontoCaucionRptData> modelo = new List<MontoCaucionRptData>();

            foreach (var item in model)
            {
                modelo.Add(MontoCaucionRptData.GenerarDatosReporteMontoCaucion(item, String.Empty));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Cauciones"), "MontoCaucionRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

    }
}
