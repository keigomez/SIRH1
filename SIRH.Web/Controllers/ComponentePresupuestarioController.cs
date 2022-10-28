using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SIRH.DTO;
using SIRH.Web.ComponentePresupuestarioLocal;
using SIRH.Web.Helpers;
using SIRH.Web.UserValidation;
using SIRH.Web.ViewModels;

namespace SIRH.Web.Controllers
{
    public class ComponentePresupuestarioController : Controller
    {
        CComponentePresupuestarioServiceClient componentePresupuestarioService = new CComponentePresupuestarioServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        public ActionResult Details(int codigo, string accion)
        {
            var datos = componentePresupuestarioService.ObtenerMovimientoPresupuesto(codigo);

            if (datos.ElementAt(0).GetType() != typeof(CErrorDTO))
            {
                ComponentePresupuestarioVM model = new ComponentePresupuestarioVM();
                model.ComponentePresupuestario = (CComponentePresupuestarioDTO)datos.ElementAt(0);
                model.ObjetoGasto = (CObjetoGastoDTO)datos.ElementAt(1);
                model.Programa = (CProgramaDTO)datos.ElementAt(2);
                model.ComponentePresupuestario = (CComponentePresupuestarioDTO)datos.ElementAt(3);

                return View(model);
            }
            else
            {
                //Consulta con errores
                ComponentePresupuestarioVM model = new ComponentePresupuestarioVM();
                model.Error = (CErrorDTO)datos.ElementAt(0);

                return View(model);
            }
        }


            public ActionResult Index()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Cauciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            }
            else
            {
                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CCaucionDTO).Name));
                return View();
            }
        }



        // GET: /ComponentePresupuestario/Create

        public ActionResult Create()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.ComponentePresupuestario), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.ComponentePresupuestario)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.ComponentePresupuestario) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.ComponentePresupuestario)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {
                    ComponentePresupuestarioVM model = new ComponentePresupuestarioVM();
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }


        // POST: /ComponentePresupuestario/Create

        [HttpPost]
        public ActionResult Create(ComponentePresupuestarioVM model, string SubmitButton)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    //model.ComponentePresupuestario.TipoMovimiento = model.TipoMovimiento == "Decreto" ? 3 : 1;
                    //var resultado = caucionService.AgregarMontoCaucion(model.MontoCaucion);

                    var resultado = componentePresupuestarioService.AgregarDecretoComponentePresupuestario(model.ComponentePresupuestario.Programa, model.ComponentePresupuestario.ObjetoGasto, model.ComponentePresupuestario.TipoMovimiento, model.ComponentePresupuestario);

                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.ComponentePresupuestario),
                            Convert.ToInt32(EAccionesBitacora.Guardar), resultado.IdEntidad,
                            CAccesoWeb.ListarEntidades(typeof(CComponentePresupuestarioDTO).Name));
                        // return RedirectToAction("Details", new { id = resultado.IdEntidad, accion = "guardar" });
                        return JavaScript("Se Guardo Correctamente");
                        


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
                //List<string> estadosPage = new List<string>();
                //estadosPage.Add("Seleccionar Estado");
                //estadosPage.Add("Activo");
                //estadosPage.Add("Inactivo");
                //model.Estados = new SelectList(estadosPage);

                if (error.Message != "Validacion")
                {
                    ModelState.AddModelError("BDError", error.Message);
                }

                return View(model);
            }



        }

        //-------------------------AGUINALDO--------------------------------------------


        public ActionResult SearchAguinaldo()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null ||
                    Session["Permiso_Feriado_Consulta"] != null)
                {
                    BusquedaPagoFeriadoVM model = new BusquedaPagoFeriadoVM();

                    List<string> diasFeriados = new List<string>();
                    var auxiliar = componentePresupuestarioService.ListarDiasPorTipo(1).Select(
                                    P => new SelectListItem
                                    {
                                        Value = ((CCatalogoDiaDTO)P.ElementAt(0)).IdEntidad.ToString(),
                                        Text = ((CCatalogoDiaDTO)P.ElementAt(0)).DescripcionDia.ToString()
                                    }
                                    );

                    model.DiaFeriado = new SelectList(auxiliar, "Value", "Text");


                    var auxiliarA = componentePresupuestarioService.ListarDiasPorTipo(2).Select(
                                  P => new SelectListItem
                                  {
                                      Value = ((CCatalogoDiaDTO)P.ElementAt(0)).IdEntidad.ToString(),
                                      Text = ((CCatalogoDiaDTO)P.ElementAt(0)).DescripcionDia.ToString()
                                  }
                                  );

                    model.DiaAsueto = new SelectList(auxiliarA, "Value", "Text");
                    var estados = new List<string>
                                     {
                                         "Activo",
                                         "Anulado"
                                     };

                    var datoEstado = estados.Select(P => new SelectListItem
                    {
                        Value = P.ToString(),
                        Text = P.ToString()
                    });

                    model.Estados = new SelectList(datoEstado, "Value", "Text");

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }
        }




    }




}

        
                