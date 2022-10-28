using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.CaucionService;
//using SIRH.Web.CaucionesLocal;
using SIRH.Web.FuncionarioService;
//using SIRH.Web.FuncionarioLocal;
//using SIRH.Web.PuestoLocal;
using SIRH.Web.PuestoService;
using SIRH.DTO;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Reports.Cauciones;
using SIRH.Web.Helpers;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.UserValidation;
using System.DirectoryServices.AccountManagement;
//using SIRH.Web.PerfilUsuarioLocal;
using SIRH.Web.PerfilUsuarioService;
using System.Web.Hosting;


namespace SIRH.Web.Controllers
{
    public class CaucionController : Controller
    {
        //
        // GET: /Caucion/
        
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CCaucionesServiceClient servicioCaucion = new CCaucionesServiceClient();
        CPuestoServiceClient servicioPuesto = new CPuestoServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

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

        //
        // GET: /Caucion/DetalleNotificacion/5

        public ActionResult DetalleNotificacion(int codigo, string accion)
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
                    NotificacionUsuarioVM model = new NotificacionUsuarioVM();

                    var datos = servicioUsuario.ObtenerNotificacion(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Notificacion = (CNotificacionUsuarioDTO)datos.ElementAt(0);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
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
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // GET: /Caucion/Details/5

        public ActionResult Details(int codigo, string accion)
        {
            var datos = servicioCaucion.ObtenerCaucion(codigo);

            if (datos.ElementAt(0).GetType() != typeof(CErrorDTO))
            {
                //Consulta exitosa
                FormularioCaucionVM model = new FormularioCaucionVM();
                model.Caucion = (CCaucionDTO)datos.ElementAt(0);
                model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                model.EntidadSeguros = (CEntidadSegurosDTO)datos.ElementAt(2);
                model.MontoCaucion = (CMontoCaucionDTO)datos.ElementAt(3);

                return View(model);
            }
            else
            {
                //Consulta con errores
                FormularioCaucionVM model = new FormularioCaucionVM();
                model.Error = (CErrorDTO)datos.ElementAt(0);

                return View(model);
            }
            //context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.RelacionesLaborales), 0);

            //if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Cauciones)].ToString().StartsWith("Error"))
            //{
            //    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            //}
            //else
            //{
            //    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
            //        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Cauciones)]) ||
            //        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Consulta))] != null ||
            //        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
            //    {
            //        FormularioCaucionVM model = new FormularioCaucionVM();

            //        var datos = servicioCaucion.ObtenerCaucion(codigo);

            //        if (datos.Count() > 1)
            //        {
            //            model.Caucion = (CCaucionDTO)datos.ElementAt(0);
            //            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
            //            model.EntidadSeguros = (CEntidadSegurosDTO)datos.ElementAt(2);
            //            model.MontoCaucion = (CMontoCaucionDTO)datos.ElementAt(3);
            //        }
            //        else
            //        {
            //            model.Error = (CErrorDTO)datos.ElementAt(0);
            //        }

            //        return View(model);
            //    }
            //    else
            //    {
            //        CAccesoWeb.CargarErrorAcceso(Session);
            //        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            //    }
            //}
        }

        public ActionResult EnviarNotificacion(string cedula = null)
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
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {

                    NotificacionUsuarioVM model = new NotificacionUsuarioVM();
                    if (cedula != null)
                    {
                        model.Funcionario = new CFuncionarioDTO
                        {
                            Cedula = cedula
                        };
                    }
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        private string ContenidoCorreo(string funcionario)
        {
            string respuesta = "";
            respuesta += "Estimado (a) " + funcionario + "\r\r";
            respuesta += "La Ley N°8131, denominada “Ley de Administración Financiera de la República y Presupuestos Públicos”, propiamente en el artículo N°13 establece lo siguiente:\r";
            respuesta += "\r\r “…Artículo 13.- Garantías.Sin perjuicio de las previsiones que deba tomar la Administración, todo encargado de recaudar, custodiar o administrar fondos y valores públicos deberá rendir garantía con cargo a su propio peculio, en favor de la Hacienda Pública o la entidad respectiva, para asegurar el correcto cumplimiento de los deberes y las obligaciones de los funcionarios.Las leyes y los reglamentos determinarán las clases y los montos de las garantías, así como los procedimientos aplicables a este particular, tomando en consideración los niveles de responsabilidad, el monto administrado y el salario del funcionario.”.";
            respuesta += "\r\r Por otra parte, en el Decreto Ejecutivo N°41365 - MOPT, denominado “Reglamento para la rendición de cauciones de los funcionarios del Ministerio de Obras Públicas y Transportes”, en su artículo 5 señala lo siguiente:";
            respuesta += "\r\r … “5º—Que de conformidad con lo establecido por el artículo 120 de la Ley de la Administración Financiera de la República y Presupuestos Públicos, N°8131, la falta de presentación de la respectiva garantía por parte de los funcionarios públicos obligados a ello, constituye causal para el cese en el cargo sin responsabilidad patronal.”.";
            respuesta += "\r\rCon base en lo anterior, se le informa que la póliza de fidelidad suscrita por su persona, se encuentra vencida, por lo que se le solicita remitir el comprobante de la nueva caución, al Departamento de Gestión de Servicios del Personal, en un plazo no mayor a tres días a partir de la fecha de recibo del presente comunicado; según lo establece la Ley N°6227, denominada “Ley General de la Administración Pública”, artículo 262,  incisos a), b) y d).";
            return respuesta;
        }

        [HttpPost]
        public ActionResult EnviarNotificacion(NotificacionUsuarioVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario =
                                    servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            if (((CPuestoDTO)datosFuncionario[1]).PuestoConfianza == true)
                            {
                                model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                                model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];
                                model.Expediente = new CExpedienteFuncionarioDTO { NumeroExpediente = Convert.ToInt32(model.Funcionario.Mensaje) };
                                List<string> asuntosPage = new List<string>();
                                asuntosPage.Add("Seleccionar Asunto");
                                asuntosPage.Add("Notificación");
                                asuntosPage.Add("Recordatorio");
                                model.Asuntos = new SelectList(asuntosPage);

                                model.Notificacion = new CNotificacionUsuarioDTO();

                                using (HostingEnvironment.Impersonate())
                                {
                                    PrincipalSearcher searcher = new PrincipalSearcher();

                                    var searchPrinciple = new UserPrincipal(new PrincipalContext(ContextType.Domain));

                                    searchPrinciple.Description = model.Funcionario.Cedula;

                                    searcher.QueryFilter = searchPrinciple;

                                    var results = searcher.FindAll();
                                    var correo = "";
                                    if (results.Count() > 1)
                                    {
                                        correo = results.FirstOrDefault(Q => !Q.DistinguishedName.Contains("disabled")).UserPrincipalName;
                                    }
                                    else
                                    {
                                        correo = results.FirstOrDefault().UserPrincipalName;
                                    }

                                    model.Notificacion.Destinatario = correo;
                                }

                                model.Notificacion.Contenido = ContenidoCorreo(model.Funcionario.Nombre + " " + model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido);

                                return PartialView("_FormularioEnvioNotificacion", model);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda",
                                   ("El funcionario: " + ((CFuncionarioDTO)datosFuncionario[0]).Cedula + " - "
                                    + ((CFuncionarioDTO)datosFuncionario[0]).Nombre + " "
                                    + ((CFuncionarioDTO)datosFuncionario[0]).PrimerApellido + " "
                                    + ((CFuncionarioDTO)datosFuncionario[0]).SegundoApellido
                                    + " no se encuentra nombrado en un puesto sujeto a caución, por lo que no se le pueden enviar notificaciones desde este módulo."));
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
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.AsuntoSeleccionado.Contains("Asunto"))
                    {
                        ModelState.AddModelError("Busqueda", "Debe seleccionar el asunto de la notificación");
                    }
                    if (model.Notificacion.Contenido == "" || model.Notificacion.Contenido == null)
                    {
                        ModelState.AddModelError("Busqueda", "La notificación no puede ser enviada sin contenido");
                    }
                    if (model.Notificacion.Destinatario == "" || model.Notificacion.Destinatario == null)
                    {
                        ModelState.AddModelError("Busqueda", "Debe indicar el destinatario que recibirá la notificación");
                    }
                    if (ModelState.IsValid == true)
                    {
                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        model.Notificacion.CedulaReferencia = model.Funcionario.Cedula;
                        model.Notificacion.Asunto = model.AsuntoSeleccionado;
                        model.Notificacion.Modulo = 4;
                        model.Notificacion.FechaEnvio = DateTime.Now;
                        model.Notificacion.Usuario = new CUsuarioDTO();
                        model.Notificacion.UsuarioDestino = new CUsuarioDTO();
                        model.Notificacion.Usuario.NombreUsuario = principal.Identity.Name.Split('\\')[1];
                        model.Notificacion.UsuarioDestino.NombreUsuario = model.Notificacion.Destinatario.Split('@')[0];

                        //var respuesta = servicioUsuario.EnviarNotificacion(model.Notificacion);
                        var respuesta = new CErrorDTO();

                        var correo = new EmailWebHelper
                        {
                            Asunto = model.AsuntoSeleccionado + " - Prueba SendGrid",
                            Destinos = model.Notificacion.Destinatario + "," + model.Notificacion.Usuario.NombreUsuario + "@mopt.go.cr",
                            EmailBody = model.Notificacion.Contenido
                        };

                        var enviado = correo.EnviarCorreo();

                        return JavaScript("window.location = '/Caucion/DetalleNotificacion?codigo=" +
                                            1 + "&accion=guardar" + "'");

                        //if (respuesta.GetType() != typeof(CErrorDTO))
                        //{
                        //    if (((CBaseDTO)respuesta).IdEntidad > 0)
                        //    {
                        //        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones),
                        //                                Convert.ToInt32(EAccionesBitacora.Notificar), Convert.ToInt32(((CBaseDTO)respuesta).IdEntidad),
                        //                                CAccesoWeb.ListarEntidades(typeof(CCaucionDTO).Name));

                        //        var correo = new EmailWebHelper
                        //        {
                        //            Asunto = model.AsuntoSeleccionado + " - SIRH - Cauciones",
                        //            Destinos = model.Notificacion.Destinatario + "," + model.Notificacion.Usuario.NombreUsuario + "@mopt.go.cr",
                        //            EmailBody = model.Notificacion.Contenido
                        //        };

                        //        var enviado = correo.EnviarCorreo();

                        //        return JavaScript("window.location = '/Caucion/DetalleNotificacion?codigo=" +
                        //                            ((CBaseDTO)respuesta).IdEntidad + "&accion=guardar" + "'");
                        //    }
                        //    else
                        //    {
                        //        ModelState.AddModelError("Agregar", respuesta.Mensaje);
                        //        throw new Exception(respuesta.Mensaje);
                        //    }
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError("Agregar", respuesta.Mensaje);
                        //    throw new Exception(respuesta.Mensaje);
                        //}
                    }
                    else
                    {
                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("error", error.Message);
                List<string> asuntosPage = new List<string>();
                asuntosPage.Add("Seleccionar Asunto");
                asuntosPage.Add("Notificación");
                asuntosPage.Add("Recordatorio");
                model.Asuntos = new SelectList(asuntosPage);
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorCaucion");
                }
                else
                {
                    return PartialView("_FormularioEnvioNotificacion", model);
                }
            }
        }

        //
        // GET: /Caucion/Create

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
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Cauciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // POST: /Caucion/Create

        [HttpPost]
        public ActionResult Create(FormularioCaucionVM model, string SubmitButton)
        {
            try
            {
                var entidadesSeguros = servicioCaucion.ListarEntidadSeguros()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CEntidadSegurosDTO)Q).IdEntidad.ToString(),
                            Text = ((CEntidadSegurosDTO)Q).NombreEntidad
                        });

                var montosCaucion = servicioCaucion.ListarMontosCaucion()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CMontoCaucionDTO)Q).IdEntidad.ToString(),
                            Text = "₡ " + ((CMontoCaucionDTO)Q).MontoColones.ToString("#,#.00#;(#,#.00#)") + " - " + ((CMontoCaucionDTO)Q).Descripcion
                        });

                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario =
                                    servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            if (((CPuestoDTO)datosFuncionario[1]).PuestoConfianza == true)
                            {
                                model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                                model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];
                                model.Expediente = new CExpedienteFuncionarioDTO { NumeroExpediente = Convert.ToInt32(model.Funcionario.Mensaje) };

                                model.Aseguradoras = new SelectList(entidadesSeguros, "Value", "Text");

                                model.Montos = new SelectList(montosCaucion, "Value", "Text");

                                return PartialView("_FormularioCaucion", model);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda",
                                   ("El funcionario: " + ((CFuncionarioDTO)datosFuncionario[0]).Cedula + " - "
                                    + ((CFuncionarioDTO)datosFuncionario[0]).Nombre + " "
                                    + ((CFuncionarioDTO)datosFuncionario[0]).PrimerApellido + " "
                                    + ((CFuncionarioDTO)datosFuncionario[0]).SegundoApellido
                                    + " no se encuentra nombrado en un puesto de confianza, por lo que no se le pueden registrar pólizas de caución."));
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
                        model.Aseguradoras = new SelectList(entidadesSeguros, "Value", "Text");

                        model.Montos = new SelectList(montosCaucion, "Value", "Text");

                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    if (ModelState.ContainsKey("Expediente.DatoABuscar"))
                        ModelState["Expediente.DatoABuscar"].Errors.Clear();
                    if (ModelState.IsValid == true)
                    {
                        CEntidadSegurosDTO entidad = new CEntidadSegurosDTO
                        {
                            IdEntidad = Convert.ToInt32(model.AseguradoraSeleccionada)
                        };

                        CMontoCaucionDTO monto = new CMontoCaucionDTO
                        {
                            IdEntidad = Convert.ToInt32(model.MontoSeleccionado)
                        };

                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        var respuesta = servicioCaucion.AgregarCaucion(model.Funcionario, model.Caucion,
                                                                        entidad, monto);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)respuesta).Codigo > 0)
                            {
                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones),
                                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                                        CAccesoWeb.ListarEntidades(typeof(CCaucionDTO).Name));

                                return JavaScript("window.location = '/Caucion/Details?codigo=" +
                                                    ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                            }
                            else
                            {
                                model.Aseguradoras = new SelectList(entidadesSeguros, "Value", "Text");

                                model.Montos = new SelectList(montosCaucion, "Value", "Text");

                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            model.Aseguradoras = new SelectList(entidadesSeguros, "Value", "Text");

                            model.Montos = new SelectList(montosCaucion, "Value", "Text");

                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        model.Aseguradoras = new SelectList(entidadesSeguros, "Value", "Text");

                        model.Montos = new SelectList(montosCaucion, "Value", "Text");

                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorCaucion");
                }
                else
                {
                    return PartialView("_FormularioCaucion", model);
                }
            }
        }

        //
        // GET: /Caucion/GestionNotificacion

        public ActionResult GestionNotificacion()
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
                    NotificacionUsuarioVM model = new NotificacionUsuarioVM();

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        [HttpPost]
        public ActionResult GestionNotificacion(NotificacionUsuarioVM model)
        {
            try
            {
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                model.Notificacion = new CNotificacionUsuarioDTO();
                model.Notificacion.Modulo = 4;
                ModelState.Clear();
                if (model.Funcionario.Cedula != null ||
                    (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1))
                {
                    List<DateTime> fechasEmision = new List<DateTime>();

                    if (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1)
                    {
                        fechasEmision.Add(model.FechaEmisionDesde);
                        fechasEmision.Add(model.FechaEmisionHasta);
                    }

                    var datos = servicioUsuario.BuscarNotificaciones(model.Funcionario, model.Notificacion, fechasEmision.ToArray());


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        var modelo = new List<NotificacionUsuarioVM>();

                        foreach (var item in datos)
                        {
                            NotificacionUsuarioVM temp = new NotificacionUsuarioVM();
                            temp.Notificacion = (CNotificacionUsuarioDTO)item.ElementAt(0);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(1);
                            modelo.Add(temp);
                        }

                        return PartialView("_GestionNotificacionResult", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaEmisionDesde.Year > 1 || model.FechaEmisionHasta.Year > 1)
                    {
                        if (!(model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorCaucion");
                }
                else
                {
                    return PartialView("_ErrorCaucion");
                }
            }
        }

        //
        // GET: /Caucion/Search

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
                    BusquedaCaucionVM model = new BusquedaCaucionVM();
                    List<string> estadosPage = new List<string>();
                    estadosPage.Add("Seleccionar Estado");
                    estadosPage.Add("Activa");
                    estadosPage.Add("Por Activar");
                    estadosPage.Add("Anulada");
                    estadosPage.Add("Vencida");
                    model.Estados = new SelectList(estadosPage);

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // POST: /Caucion/Search

        [HttpPost]
        public ActionResult Search(BusquedaCaucionVM model)
        {
            try
            {
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                if (model.Funcionario.Cedula != null || model.Caucion.NumeroPoliza != null ||
                    (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1) ||
                    (model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1) || 
                    model.EstadoSeleccionado != null || model.Puesto.CodPuesto != null || 
                    model.NivelCaucion.Descripcion != null)
                {
                    List<DateTime> fechasEmision = new List<DateTime>();
                    List<DateTime> fechasVencimiento = new List<DateTime>();
                    if (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1)
                    {
                        fechasEmision.Add(model.FechaEmisionDesde);
                        fechasEmision.Add(model.FechaEmisionHasta);
                    }

                    if (model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1)
                    {
                        fechasVencimiento.Add(model.FechaVenceDesde);
                        fechasVencimiento.Add(model.FechaVenceHasta);
                    }

                    model.Caucion.DetalleEstadoPoliza = model.EstadoSeleccionado;

                    var datos = servicioCaucion.BuscarCauciones(model.Funcionario, model.Caucion,
                                                                    fechasEmision.ToArray(),
                                                                    fechasVencimiento.ToArray(), model.Puesto, model.NivelCaucion);
                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.Cauciones = new List<FormularioCaucionVM>();

                        foreach (var item in datos)
                        {
                            FormularioCaucionVM temp = new FormularioCaucionVM();
                            temp.Caucion = (CCaucionDTO)item.ElementAt(0);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(1);
                            temp.Puesto = (CPuestoDTO)item.ElementAt(2);
                            temp.EntidadSeguros = (CEntidadSegurosDTO)item.ElementAt(3);
                            temp.MontoCaucion = (CMontoCaucionDTO)item.ElementAt(4);
                            model.Cauciones.Add(temp);
                        }

                        return PartialView("_SearchResults", model.Cauciones);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaEmisionDesde.Year > 1 || model.FechaEmisionHasta.Year > 1)
                    {
                        if (!(model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    if (model.FechaVenceDesde.Year > 1 || model.FechaVenceHasta.Year > 1)
                    {
                        if (!(model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorCaucion");
                }
                else
                {
                    return PartialView("_ErrorCaucion");
                }
            }
        }

        //
        // GET: /Caucion/Edit/5

        public ActionResult Edit(int codigo)
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
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {
                    FormularioCaucionVM model = new FormularioCaucionVM();

                    var datos = servicioCaucion.ObtenerCaucion(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Caucion = (CCaucionDTO)datos.ElementAt(0);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                        model.EntidadSeguros = (CEntidadSegurosDTO)datos.ElementAt(2);
                        model.MontoCaucion = (CMontoCaucionDTO)datos.ElementAt(3);
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

                    if (model.Caucion.EstadoPoliza == 3)
                    {
                        model.Error = new CErrorDTO { MensajeError = "La póliza ya se encuentra anulada por lo que no es posible completar este proceso" };
                    }

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        //
        // POST: /Caucion/Edit/5

        [HttpPost]
        public ActionResult Edit(int codigo, FormularioCaucionVM model)
        {
            try
            {
                ModelState.Clear();
                if (model.Caucion.ObservacionesPoliza != null)
                {
                    if (model.Caucion.ObservacionesPoliza.Trim() != "")
                    {
                        model.Caucion.IdEntidad = codigo;
                        var respuesta = servicioCaucion.AnularCaucion(model.Caucion);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Cauciones),
                                    Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                    CAccesoWeb.ListarEntidades(typeof(CCaucionDTO).Name));
                            return JavaScript("window.location = '/Caucion/Details?codigo=" + respuesta.IdEntidad + "&accion=modificar" + "'");
                        }
                        else
                        {
                            ModelState.AddModelError("modificar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Debe digitar una justificación para anular esta póliza");
                        throw new Exception();
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para anular esta póliza");
                    throw new Exception();
                }
            }
            catch(Exception error)
            {
                ModelState.AddModelError("contenido", error.Message);
                return PartialView("_ErrorCaucion");
            }
        }

        //
        // GET: /Caucion/List
        public ActionResult List()
        {
            try
            {
                List<FormularioCaucionVM> model = new List<FormularioCaucionVM>();
                var respuesta = servicioFuncionario.FuncionariosConCauciones();

                if (respuesta.First().First().GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in respuesta)
                    {
                        FormularioCaucionVM temp = new FormularioCaucionVM();
                        temp.Funcionario = (CFuncionarioDTO)item[0];
                        temp.Puesto = (CPuestoDTO)item[1];
                        temp.DetallePuesto = (CDetallePuestoDTO)item[2];
                        if (item[3].IdEntidad != -1)
                        {
                            temp.Caucion = (CCaucionDTO)item[3];
                        }
                        else
                        {
                            temp.Caucion = new CCaucionDTO { IdEntidad = -1 };
                        }
                        model.Add(temp);
                        model = model.OrderBy(Q => Q.Funcionario.PrimerApellido).ToList();
                        var notificacion = servicioUsuario.ObtenerNotificacionCedula(temp.Funcionario.Cedula, 4);

                        if (notificacion.GetType() != typeof(CErrorDTO))
                        {
                            temp.Notificacion = (CNotificacionUsuarioDTO)notificacion;
                        }
                    }
                    return View(model);
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta.First().First()).Mensaje);
                }
            }
            catch (Exception error)
            {
                CAccesoWeb.CargarErrorSistema(error.Message, Session);
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
            }
        }

        public ActionResult SetPuestoCaucion()
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
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Cauciones, Convert.ToInt32(ENivelesCaucion.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }

        [HttpPost]
        public ActionResult SetPuestoCaucion(DetallePuestoVM model, string SubmitButton)
        {
            model.mensaje = "";
            try
            {
                if (SubmitButton == "Buscar")
                {
                    var resultado = servicioPuesto.DescargarPuestoCompleto(model.Puesto.CodPuesto);

                    if (resultado.ElementAt(0).GetType() != typeof(CErrorDTO))
                    {
                        model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                        model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                        model.UbicacionPuesto = ((CUbicacionAdministrativaDTO)resultado.ElementAt(3));
                        if (resultado.Count() > 4)
                        {
                            model.Funcionario = ((CFuncionarioDTO)resultado.ElementAt(4));
                        }
                        else
                        {
                            model.Funcionario = new CFuncionarioDTO { Mensaje = "Este puesto no está siendo ocupado por ningún funcionario al momento de esta consulta." };
                        }
                        return PartialView("_ResultadoSetPuestoCaucion", model);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.ElementAt(0)).MensajeError);
                    }
                }
                else
                {
                    var resultado = servicioCaucion.ActualizarObservacionesPuestoCaucion(model.Puesto.CodPuesto, model.Puesto.ObservacionesPuesto);
                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        model.mensaje = "Se modificó el puesto correctamente";
                        System.Threading.Thread.Sleep(1000);
                        var resultado2 = servicioPuesto.DescargarPuestoCompleto(model.Puesto.CodPuesto);

                        if (resultado2.ElementAt(0).GetType() != typeof(CErrorDTO))
                        {
                            model.Puesto = ((CPuestoDTO)resultado2.ElementAt(0));
                            model.DetallePuesto = ((CDetallePuestoDTO)resultado2.ElementAt(1));
                            model.UbicacionPuesto = ((CUbicacionAdministrativaDTO)resultado2.ElementAt(3));
                            if (resultado2.Count() > 4)
                            {
                                model.Funcionario = ((CFuncionarioDTO)resultado2.ElementAt(4));
                            }
                            else
                            {
                                model.Funcionario = new CFuncionarioDTO { Mensaje = "Este puesto no está siendo ocupado por ningún funcionario al momento de esta consulta." };
                            }
                            return PartialView("_ResultadoSetPuestoCaucion", model);
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)resultado2.ElementAt(0)).MensajeError);
                        }
                        //return JavaScript("window.location = '/Caucion/SetPuestoCaucion");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado).MensajeError);
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("buscar", (error.Message));
                return PartialView("_ErrorCaucion");
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleCaucion(FormularioCaucionVM model)
        {
            List<CaucionRptData> modelo = new List<CaucionRptData>();

            modelo.Add(CaucionRptData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Cauciones"), "CaucionRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseCaucion(List<FormularioCaucionVM> model)
        {
            List<CaucionRptData> modelo = new List<CaucionRptData>();

            foreach (var item in model)
            {
                modelo.Add(CaucionRptData.GenerarDatosReporte(item, String.Empty));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Cauciones"), "DesgloceCaucionesRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }
    }
}
