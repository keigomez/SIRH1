using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.BorradorAccionPersonalLocal;
using SIRH.Web.TipoAccionPersonalLocal;
using SIRH.Web.FuncionarioLocal;
using SIRH.Web.PuestoLocal;
using SIRH.Web.PerfilUsuarioLocal;
using SIRH.Web.EstadoBorradorLocal;
using SIRH.DTO; 
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Reports.BorradorAccionPersonal;
using SIRH.Web.Helpers;
using System.Security.Principal;
using System.Threading;
using System.Web.UI;
using SIRH.Web.UserValidation; 


namespace SIRH.Web.Controllers
{
    public class BorradorAccionPersonalController : Controller
    {
        //
        // GET: /BorradorAccionPersonal/

        CBorradorAccionPersonalServiceClient servicioBorrador = new CBorradorAccionPersonalServiceClient();
        //CDetalleBorradorAccionPersonalDTO servicioDetalle = new CDetalleBorradorAccionPersonalDTO();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();  
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CTipoAccionPersonalServiceClient servicioTipo = new CTipoAccionPersonalServiceClient();
        CPuestoServiceClient servicioPuesto = new CPuestoServiceClient();
        CEstadoBorradorServiceClient servicioEstado = new CEstadoBorradorServiceClient();

        CAccesoWeb context = new CAccesoWeb();

        EmailWebHelper em = new EmailWebHelper();
        
        WindowsIdentity principal = WindowsIdentity.GetCurrent();

        const string strAsunto = "SIRH. Módulo de Borradores de Acción de Personal";


        public enum EstadosBorrador
        {
            Indefinido = 0,
            ElaboracionBorrador = 1,
            EnAnalisis = 2,
            EnRegistro = 3,
            Finalizado = 4,
            Anulado = 5
        }


        //const int intPerfilBorrador = 8;
        
        //const int intFinalizarBorrador = 7; 
        //const int intAnularBorrador = 8; 

        public ActionResult Index()
        {
            context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
            }
            else
            {
                context.GuardarBitacora(principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CBorradorAccionPersonalDTO).Name));
                return View();
            }
        }

        // GET: /BorradorAccionPersonal/Details/1
        public ActionResult Details(int codigo)
        {
             context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

             if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
             {
                 return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
             }
             else
             {
                 if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                     Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)]) ||
                     Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Registro))] != null ||
                     Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista))] != null)
                 {
                     FormularioBorradorAccionPersonalVM model = new FormularioBorradorAccionPersonalVM();

                     var datos = servicioBorrador.ObtenerBorrador(codigo);
                     if (datos.Count() > 1)
                     {
                         model.Borrador = (CBorradorAccionPersonalDTO)datos.ElementAt(0);
                         model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                         model.Asignado = (CFuncionarioDTO)datos.ElementAt(2);

                         if (datos.ElementAt(3).GetType() != typeof(CErrorDTO))
                         {
                             model.Detalle = (CDetalleBorradorAccionPersonalDTO)datos.ElementAt(3);
                             model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(4);
                             model.Funcionario = (CFuncionarioDTO)datos.ElementAt(5);

                             var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                             if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                             {
                                 model.Puesto = (CPuestoDTO)datosFuncionario.ElementAt(1);
                                 model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario.ElementAt(2);
                                 model.Contrato = (CDetalleContratacionDTO)datosFuncionario.ElementAt(3);
                                 var puntos = (CRespuestaDTO)servicioFuncionario.BuscarFuncionarioPuntosCarreraProfesional(model.Funcionario.Cedula);
                                 model.PuntosCarrera = Convert.ToInt16(puntos.Contenido);
                             }

                             var datosClase = servicioPuesto.BuscarClaseParams(model.Detalle.CodClase, "");
                             if (datosClase.FirstOrDefault().GetType() != typeof(CErrorDTO))
                             {
                                 model.Clase = (CClaseDTO)datosClase.ElementAt(0);
                             }
                         }
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
                     return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
                 }
             }
        }


        // GET: /BorradorAccionPersonal/Create
        public ActionResult Create(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Registro))] != null)
                {
                    FormularioBorradorAccionPersonalVM model = new FormularioBorradorAccionPersonalVM();

                    var datos = servicioBorrador.ObtenerBorrador(codigo);
                    if (datos.Count() > 1)
                    {
                        model.Borrador = (CBorradorAccionPersonalDTO)datos.ElementAt(0);
                        model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                        model.Asignado = (CFuncionarioDTO)datos.ElementAt(2);
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
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
                }
            }
        }

        // POST: /BorradorAccionPersonal/Create
        [HttpPost]
        public ActionResult Create(FormularioBorradorAccionPersonalVM model, string SubmitButton)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Funcionario.Cedula))
                {
                    throw new Exception("Busqueda");
                }

                var listadoUsuarios = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista))
                .Select(Q => new SelectListItem
                {
                    Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                    Text = ((CFuncionarioDTO)Q[0]).Nombre.ToString() + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido.ToString() + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido.ToString()
                });

                var listadoTipos = servicioTipo.RetornarTipos()
                  .Select(Q => new SelectListItem
                  {
                      Value = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString(),
                      Text = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString() + " - " + ((CTipoAccionPersonalDTO)Q).DesTipoAccion
                  });

                var listadoDirecciones = servicioPuesto.DescargarDireccionGenerals(0, "")
                 .Select(Q => new SelectListItem
                 {
                     Value = ((CDireccionGeneralDTO)Q).IdEntidad.ToString(),
                     Text = ((CDireccionGeneralDTO)Q).IdEntidad.ToString() + " - " + ((CDireccionGeneralDTO)Q).NomDireccion
                 });

                var listadoSecciones = servicioPuesto.DescargarSeccions(0, "")
                    .Select(Q => new SelectListItem
                    {
                      Value = ((CSeccionDTO)Q).IdEntidad.ToString(),
                      Text = ((CSeccionDTO)Q).IdEntidad.ToString() + " - " + ((CSeccionDTO)Q).NomSeccion
                    });

                var listadoClases = servicioPuesto.DescargarClases(0, "")
                    .Select(Q => new SelectListItem
                    {
                        Value = ((CClaseDTO)Q).IdEntidad.ToString(),
                        Text = ((CClaseDTO)Q).IdEntidad.ToString() + " - " + ((CClaseDTO)Q).DesClase
                    });

                var listadoCategorias = servicioPuesto.ListarCategoriasEscalaSalarial()
                   .Select(Q => new SelectListItem
                   {
                       Value = Q.IdEntidad.ToString(),
                       Text = Q.IdEntidad.ToString()
                   });

               
                List<SelectListItem> listadoPorcentajes = new List<SelectListItem>();
                SelectListItem selListItem = new SelectListItem() { Value = "10", Text = "10" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "20", Text = "20" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "25", Text = "25" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "55", Text = "55" };
                listadoPorcentajes.Add(selListItem);


                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                            model.Contrato = (CDetalleContratacionDTO)datosFuncionario[3];

                            model.Tipos = new SelectList(listadoTipos, "Value", "Text");

                            model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");

                            var datosUbicacion = servicioPuesto.DescargarUbicacionAdministrativa(model.Funcionario.Cedula);
                            model.Programa = (CProgramaDTO)datosUbicacion[1];

                            model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                            model.DireccionSeleccionada = model.Puesto.UbicacionAdministrativa.DireccionGeneral.IdEntidad.ToString();

                            model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                            model.SeccionSeleccionada = model.Puesto.UbicacionAdministrativa.Seccion.IdEntidad.ToString();

                            model.Clases = new SelectList(listadoClases, "Value", "Text");
                            model.ClaseSeleccionada = model.DetallePuesto.Clase.IdEntidad.ToString();

                            model.Categorias = new SelectList(listadoCategorias, "Value", "Text");
                            model.CategoriaSeleccionada = model.DetallePuesto.EscalaSalarial.CategoriaEscala.ToString();

                            model.Porcentajes = new SelectList(listadoPorcentajes, "Value", "Text");


                            var puntos = (CRespuestaDTO)servicioFuncionario.BuscarFuncionarioPuntosCarreraProfesional(model.Funcionario.Cedula);
                            model.PuntosCarrera = Convert.ToInt16(puntos.Contenido);

                            decimal porProhDedicExc = 0;

                            if (model.DetallePuesto.PorProhibicion != null)
                                porProhDedicExc = model.DetallePuesto.PorProhibicion;

                            if (model.DetallePuesto.PorDedicacion != null && porProhDedicExc == 0)
                                porProhDedicExc = model.DetallePuesto.PorDedicacion;


                            model.Detalle = new CDetalleBorradorAccionPersonalDTO
                            { 
                                FecRige = DateTime.Today,
                                FecVence = DateTime.Today,
                                FecRigeIntegra = DateTime.Today,
                                FecVenceIntegra = DateTime.Today,
                                Programa = model.Programa,
                                CodPuesto = model.Puesto.CodPuesto,
                                Disfrutado = model.Contrato.NumeroAnualidades,
                                Autorizado = model.Contrato.NumeroAnualidades,
                                MtoSalarioBase = model.DetallePuesto.EscalaSalarial.SalarioBase,
                                MtoAnual = model.DetallePuesto.EscalaSalarial.MontoAumentoAnual,
                                MtoAumentosAnuales = Convert.ToDecimal(model.DetallePuesto.EscalaSalarial.MontoAumentoAnual * model.Contrato.NumeroAnualidades),
                                MtoRecargo = 0,
                                MtoPunto = model.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera,
                                NumGradoGrupo = model.PuntosCarrera,
                                MtoGradoGrupo = Convert.ToDecimal(model.PuntosCarrera * model.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera),
                                PorProhibicion = porProhDedicExc,
                                MtoProhibicion = Convert.ToDecimal(model.DetallePuesto.EscalaSalarial.SalarioBase * porProhDedicExc /100),
                                MtoOtros = 0
                            };

                            var datos = servicioBorrador.ObtenerBorrador(model.Borrador.IdEntidad);
                            if (datos.Count() > 1)
                            {
                                model.Borrador = (CBorradorAccionPersonalDTO)datos.ElementAt(0);
                                model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                                model.Asignado = (CFuncionarioDTO)datos.ElementAt(2);

                                if (datos.ElementAt(3).GetType() != typeof(CErrorDTO))
                                {
                                    model.Detalle = (CDetalleBorradorAccionPersonalDTO)datos.ElementAt(3);
                                    model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(4);
                                    model.Funcionario = (CFuncionarioDTO)datos.ElementAt(5);
                                }
                            }
                            else
                            {
                                model.Error = (CErrorDTO)datos.ElementAt(0);
                            }

                            return PartialView("_FormularioBorradorAccionPersonal", model);
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
                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                        model.Tipos = new SelectList(listadoTipos, "Value", "Text");
                        model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                        model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                        model.Clases = new SelectList(listadoClases, "Value", "Text");
                        model.Categorias = new SelectList(listadoCategorias, "Value", "Text");

                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    
                    if (!(model.Detalle.FecRige.Year > 0001) || !(model.Detalle.FecRigeIntegra.Year > 0001)
                        || !(model.Detalle.FecVence.Year > 0001) || !(model.Detalle.FecVenceIntegra.Year > 0001))
                    {
                        ModelState.AddModelError("formulario","Debe completar todos los campos");
                        throw new Exception("formulario");
                    }

                    if (ModelState.IsValid == true)
                    {
                        string codUsuarioEnvia = principal.Name;
                        string codUsuarioRecibe = model.UsuarioAsignado;

                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        CProgramaDTO programa = new CProgramaDTO
                        {
                            IdEntidad = model.Programa.IdEntidad
                        };

                        CSeccionDTO seccion = new CSeccionDTO
                        {
                            IdEntidad = Convert.ToInt32(model.SeccionSeleccionada)
                        };

                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                        {
                            IdEntidad = 1 // Registrado
                        };

                        CTipoAccionPersonalDTO tipo = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };

                        model.Borrador.UsuarioAsignado = model.UsuarioAsignado;

                        model.Detalle.Categoria = Convert.ToInt32(model.CategoriaSeleccionada);
                        model.Detalle.CodClase = Convert.ToInt32(model.ClaseSeleccionada);
                        model.Detalle.Programa = programa;
                        model.Detalle.Seccion = seccion;

                        
                        var respuesta = servicioBorrador.GuardarBorrador(model.Funcionario,
                                                                        estado,
                                                                        tipo, 
                                                                        model.Borrador,
                                                                        model.Detalle,
                                                                        codUsuarioEnvia,
                                                                        codUsuarioRecibe);


                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)respuesta).Codigo > 0)
                            {
                                List<string> entidades = new List<string>();
                                entidades.Add(typeof(CBorradorAccionPersonalDTO).Name);
                                entidades.Add(typeof(CDetalleBorradorAccionPersonalDTO).Name);

                                context.GuardarBitacora(principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones),
                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                        CAccesoWeb.ListarEntidades(entidades.ToArray()));

                                em.Asunto = strAsunto;
                                em.Destinos = codUsuarioRecibe.Replace("MOPT\\", "") + "@mopt.go.cr";
                                em.EmailBody = "Se le ha asignado el Borrador de Acción de Personal N° " + Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido) + " para su respectivo trámite";
                                em.EnviarCorreo();

                                return JavaScript("window.location = '/BorradorAccionPersonal/Details?codigo=" +
                                                    ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                            }
                            else
                            {
                                model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                                model.Tipos = new SelectList(listadoTipos, "Value", "Text");
                                model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                                model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                                model.Clases = new SelectList(listadoClases, "Value", "Text");
                                model.Categorias = new SelectList(listadoCategorias, "Value", "Text");
                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                            model.Tipos = new SelectList(listadoTipos, "Value", "Text");
                            model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                            model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                            model.Clases = new SelectList(listadoClases, "Value", "Text");
                            model.Categorias = new SelectList(listadoCategorias, "Value", "Text");
                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                        model.Tipos = new SelectList(listadoTipos, "Value", "Text");
                        model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                        model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                        model.Clases = new SelectList(listadoClases, "Value", "Text");
                        model.Categorias = new SelectList(listadoCategorias, "Value", "Text");
                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorBorrador");
                }
                else
                {
                    return PartialView("_ErrorBorrador", model);
                }
            }
        }



        // GET: /BorradorAccionPersonal/Solicitud
        public ActionResult Solicitud()
        {

            context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Oficio))] != null)
                {
                    FormularioBorradorAccionPersonalVM model = new FormularioBorradorAccionPersonalVM();

                    try
                    {
                        //var listado = servicioUsuario.CargarUsuariosPerfil(intPerfilBorrador);

                        var listadoUsuarios = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), Convert.ToInt32(ENivelesBorradorAccionPersonal.Tecnico))
                          .Select(Q => new SelectListItem
                          {
                              Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                              Text = ((CFuncionarioDTO)Q[0]).Nombre.ToString() + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido.ToString() + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido.ToString()
                          });

                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                    }
                    catch (Exception error)
                    {
                        if (error.Message == "Busqueda")
                        {
                            return PartialView("_ErrorBorradorAccionPersonal");
                        }
                        else
                        {
                            return PartialView("_FormularioSolicitud", model);
                        }
                    }
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
                }
            }
        }


        // POST: /BorradorAccionPersonal/Solicitud
        [HttpPost]
        public ActionResult Solicitud(FormularioBorradorAccionPersonalVM model, string SubmitButton)
        {
            try
            {
                
                var listadoUsuarios = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista))
                  .Select(Q => new SelectListItem
                  {
                      Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                      Text = ((CFuncionarioDTO)Q[0]).Nombre.ToString() + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido.ToString() + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido.ToString()
                  });

                if (SubmitButton == "Buscar")
                {
                    if (model.Funcionario.Cedula == null)
                    {
                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");

                        throw new Exception("BusquedaSolicitud");
                    }
                    
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                            model.Contrato = (CDetalleContratacionDTO)datosFuncionario[3];

                            model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");

                            return PartialView("_FormularioSolicitud", model);
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
                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");

                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    if (model.UsuarioAsignado == null  || model.Borrador.NumOficio == null || model.Borrador.ObsJustificacion == null)
                    {
                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                        ModelState.AddModelError("Busqueda","Debe completar todos los campos del formulario");
                        throw new Exception("BusquedaSolicitud");
                    }

                    if (ModelState.IsValid == true)
                    {
                        string codUsuarioEnvia = principal.Name;
                        string codUsuarioRecibe = model.UsuarioAsignado;

                        model.Borrador.UsuarioAsignado = model.UsuarioAsignado;
                      
                        var respuesta = servicioBorrador.GuardarSolicitud(model.Borrador, codUsuarioEnvia, codUsuarioRecibe);


                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)respuesta).Codigo > 0)
                            {
                                context.GuardarBitacora(principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones),
                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                        CAccesoWeb.ListarEntidades(typeof(CBorradorAccionPersonalDTO).Name));

                                em.Asunto = strAsunto;
                                em.Destinos = codUsuarioRecibe.Replace("MOPT\\", "") + "@mopt.go.cr";
                                em.EmailBody = "Se le ha asignado la Solicitud N° " + Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido) + " de Borrador de Acción de Personal";
                                em.EnviarCorreo();

                                return JavaScript("window.location = '/BorradorAccionPersonal/Details?codigo=" +
                                                    ((CRespuestaDTO)respuesta).Contenido + "&accion=solicitud" + "'");
                            }
                            else
                            {
                                model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorBorrador");
                }
                if(error.Message == "BusquedaSolicitud")
                {
                    return PartialView("_ErrorBorrador");
                }

                else
                {
                    return PartialView("_FormularioSolicitud", model);
                }
            }
        }



        public ActionResult Edit(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Tecnico))] != null)
                {
                    FormularioBorradorAccionPersonalVM model = new FormularioBorradorAccionPersonalVM();

                    var datos = servicioBorrador.ObtenerBorrador(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Borrador = (CBorradorAccionPersonalDTO)datos.ElementAt(0);
                        model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                        model.Asignado = (CFuncionarioDTO)datos.ElementAt(2);

                        if (datos.ElementAt(3).GetType() != typeof(CErrorDTO))
                        {
                            model.Detalle = (CDetalleBorradorAccionPersonalDTO)datos.ElementAt(3);
                            model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(4);
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(5);
                        }

                        int catPermisoAsignar = 0;
                        int catPermisoDevolver = 0;

                        if (model.Borrador.EstadoBorrador.IdEntidad == Convert.ToInt32(EstadosBorrador.ElaboracionBorrador))
                        {
                            catPermisoAsignar = Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista);
                            catPermisoDevolver = Convert.ToInt32(ENivelesBorradorAccionPersonal.Tecnico);
                        }

                        if (model.Borrador.EstadoBorrador.IdEntidad == Convert.ToInt32(EstadosBorrador.EnAnalisis))
                        {
                            catPermisoAsignar = Convert.ToInt32(ENivelesBorradorAccionPersonal.Registro);
                            catPermisoDevolver = Convert.ToInt32(ENivelesBorradorAccionPersonal.Tecnico);
                        }

                        if (model.Borrador.EstadoBorrador.IdEntidad == Convert.ToInt32(EstadosBorrador.EnRegistro))
                        {
                            catPermisoAsignar = Convert.ToInt32(ENivelesBorradorAccionPersonal.Registro);
                            catPermisoDevolver = Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista);
                        }

                        var listadoUsuarios = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), catPermisoAsignar)
                             .Select(Q => new SelectListItem
                             {
                                 Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                                 Text = ((CFuncionarioDTO)Q[0]).Nombre + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido
                             });


                        var listadoUsuariosDevolver = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), catPermisoDevolver)
                             .Select(Q => new SelectListItem
                             {
                                 Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                                 Text = ((CFuncionarioDTO)Q[0]).Nombre + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido
                             });

                        model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
                        model.UsuariosDevolver = new SelectList(listadoUsuariosDevolver, "Value", "Text");
                    }
                    else
                    {
                        var listado = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), 0)
                             .Select(Q => new SelectListItem
                             {
                                 Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                                 Text = ((CFuncionarioDTO)Q[0]).Nombre + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido
                             });

                        model.Error = (CErrorDTO)datos.ElementAt(0);
                        model.Usuarios = new SelectList(listado, "Value", "Text");
                        model.UsuariosDevolver = new SelectList(listado, "Value", "Text");
                    }

                    return View(model); 
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
                }
            }
        }


        [HttpPost]
        public ActionResult Edit(int codigo, FormularioBorradorAccionPersonalVM model, string SubmitButton)
        {
            string usrRecibe = "";

            var listadoUsuarios = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), 0)
                 .Select(Q => new SelectListItem
                 {
                     Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                     Text = ((CFuncionarioDTO)Q[0]).Nombre + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido
                 });

            var listadoUsuariosDevolver = servicioUsuario.CargarUsuariosPerfil(Convert.ToInt32(EModulosHelper.BorradorAcciones), Convert.ToInt32(ENivelesBorradorAccionPersonal.Tecnico))
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CUsuarioDTO)Q[1]).NombreUsuario,
                            Text = ((CFuncionarioDTO)Q[0]).Nombre + " " + ((CFuncionarioDTO)Q[0]).PrimerApellido + " " + ((CFuncionarioDTO)Q[0]).SegundoApellido
                        });

            model.Usuarios = new SelectList(listadoUsuarios, "Value", "Text");
            model.UsuariosDevolver = new SelectList(listadoUsuariosDevolver, "Value", "Text");

            try
            {
                if (model.Movimiento.ObsMovimiento != null)
                {
                    int codMovimiento = 0;

                    if (SubmitButton == "Finalizar Borrador")
                    {
                        codMovimiento = Convert.ToInt16(EstadosBorrador.Finalizado);
                        //model.UsuarioAsignado = model.Borrador.UsuarioAsignado;
                        usrRecibe = model.Borrador.UsuarioAsignado;
                    }
                    else if (SubmitButton == "Devolver Borrador")
                    {
                        codMovimiento = model.Estado.IdEntidad - 1;
                        usrRecibe = model.UsuarioDevolucion;
                    }
                    else
                    {
                        codMovimiento = model.Estado.IdEntidad + 1;
                        usrRecibe = model.UsuarioAsignado;
                    }
                                        

                    CMovimientoBorradorAccionPersonalDTO movimientoBD = new CMovimientoBorradorAccionPersonalDTO
                    {
                        Borrador = new CBorradorAccionPersonalDTO
                        {
                            IdEntidad = codigo
                        },
                        CodMovimiento = codMovimiento,
                        FecMovimiento = DateTime.Now,
                        UsuarioEnvia = model.Borrador.UsuarioAsignado,
                        UsuarioRecibe = usrRecibe,
                        ObsMovimiento = model.Movimiento.ObsMovimiento
                    };

                    model.Borrador.IdEntidad = codigo;
                    model.Borrador.EstadoBorrador = model.Estado;
                    model.Borrador.UsuarioAsignado = usrRecibe;


                    var respuesta = servicioBorrador.ActualizarEstado(model.Borrador, movimientoBD);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        context.GuardarBitacora(principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones),
                                Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                CAccesoWeb.ListarEntidades(typeof(CBorradorAccionPersonalDTO).Name));

                        em.Asunto = strAsunto;
                        em.Destinos = usrRecibe.Replace("MOPT\\","") + "@mopt.go.cr";
                        em.EmailBody = "Se le ha asignado el Borrador de Acción de Personal N° " + respuesta.IdEntidad + " para su respectivo trámite.";
                        em.EmailBody += " Observaciones: " + model.Movimiento.ObsMovimiento;

                        em.EnviarCorreo();

                        return RedirectToAction("Details", new { codigo = codigo, accion = "modificar" });
                    }
                    else
                    {
                        ModelState.AddModelError("modificar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar las Observaciones para realizar esta acción");
                    throw new Exception();
                }
            }
            catch
            {
                return View(model);
            }
        }


        public ActionResult Cancel(int codigo)
        {

            context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Tecnico))] != null ||
                                         Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Registro))] != null ||
                     Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista))] != null)
                {

                    FormularioBorradorAccionPersonalVM model = new FormularioBorradorAccionPersonalVM();

                    var datos = servicioBorrador.ObtenerBorrador(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Borrador = (CBorradorAccionPersonalDTO)datos.ElementAt(0);
                        model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                        model.Asignado = (CFuncionarioDTO)datos.ElementAt(2);

                        if (datos.ElementAt(3).GetType() != typeof(CErrorDTO))
                        {
                            model.Detalle = (CDetalleBorradorAccionPersonalDTO)datos.ElementAt(3);
                            model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(4);
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(5);
                        }
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
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
                }
            }
        }

        [HttpPost]
        public ActionResult Cancel(int codigo, FormularioBorradorAccionPersonalVM model)
        {
            try
            {
                if (model.Movimiento.ObsMovimiento != null)
                {
                    model.Borrador.IdEntidad = codigo;

                    model.Movimiento.CodMovimiento = Convert.ToInt16(EstadosBorrador.Anulado);
                    model.Movimiento.UsuarioEnvia = model.Borrador.UsuarioAsignado;
                    model.Movimiento.UsuarioRecibe = User.Identity.Name;


                    var respuesta = servicioBorrador.ActualizarEstado(model.Borrador, model.Movimiento);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {

                        context.GuardarBitacora(principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones),
                                Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                CAccesoWeb.ListarEntidades(typeof(CBorradorAccionPersonalDTO).Name));

                        return RedirectToAction("Details", new { codigo = codigo, accion = "cancel" });
                    }
                    else
                    {
                        ModelState.AddModelError("cancel", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para realizar esta acción");
                    throw new Exception();
                }
            }
            catch
            {
                return View(model);
            }
        }


        // GET: /BorradorAccionPersonal/List
        public ActionResult List()
        {
            try
            {
                List<FormularioBorradorAccionPersonalVM> model = new List<FormularioBorradorAccionPersonalVM>();
                var respuesta = servicioBorrador.FuncionariosConBorradores();

                if (respuesta.First().First().GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in respuesta)
                    {
                        FormularioBorradorAccionPersonalVM temp = new FormularioBorradorAccionPersonalVM();
                        temp.Funcionario = (CFuncionarioDTO)item[0];
                        temp.Puesto = (CPuestoDTO)item[1];
                        temp.DetallePuesto = (CDetallePuestoDTO)item[2];
                        if (item[3].IdEntidad != -1)
                        {
                            temp.Borrador = (CBorradorAccionPersonalDTO)item[3];
                        }
                        else
                        {
                            temp.Borrador = new CBorradorAccionPersonalDTO { IdEntidad = -1 };
                        }
                        model.Add(temp);
                        model = model.OrderBy(Q => Q.Funcionario.PrimerApellido).ToList();
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
                return RedirectToAction("Index", "Error", new { errorType = "carga", modulo = "borradoraccionpersonal" });
            }
        }



        // GET: /BorradorAccionPersonal/Buscar
        // Búsqueda de Solicitudes de Acción de Personal
        public ActionResult Buscar()
        {
            BusquedaBorradorAccionPersonalVM model = new BusquedaBorradorAccionPersonalVM();

            var listadoEstados = servicioEstado.RetornarEstados()
                 .Select(Q => new SelectListItem
                 {
                     Value = ((CEstadoBorradorDTO)Q).IdEntidad.ToString(),
                     Text = ((CEstadoBorradorDTO)Q).DesEstadoBorrador
                 });

            model.Estados = new SelectList(listadoEstados, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        public ActionResult Buscar(BusquedaBorradorAccionPersonalVM model)
        {
            try
            {
                if(String.IsNullOrEmpty(model.Borrador.NumOficio) && String.IsNullOrEmpty(model.EstadoSeleccionado))
                {
                    ModelState.AddModelError("Busqueda", "Debe completar alguno de los campos establecidos");
                    throw new Exception("Busqueda");
                }

                //  Estado
                if (Convert.ToInt32(model.EstadoSeleccionado) > 0)
                {
                    CEstadoBorradorDTO tipoEst = new CEstadoBorradorDTO
                    {
                        IdEntidad = Convert.ToInt32(model.EstadoSeleccionado)
                    };
                    model.Borrador.EstadoBorrador = tipoEst;
                }

                var datos = servicioBorrador.BuscarSolicitud(model.Borrador);


                if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                }
                else
                {
                    model.Borradores = new List<FormularioBorradorAccionPersonalVM>();

                    foreach (var item in datos)
                    {
                        FormularioBorradorAccionPersonalVM temp = new FormularioBorradorAccionPersonalVM();
                        temp.Borrador = (CBorradorAccionPersonalDTO)item.ElementAt(0);
                        temp.Estado = (CEstadoBorradorDTO)item.ElementAt(1);
                        temp.Asignado = (CFuncionarioDTO)item.ElementAt(2);
                        model.Borradores.Add(temp);
                    }

                    return PartialView("_BuscarResults", model.Borradores);
                }
               
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorBorrador");
                }
                else
                {
                    return PartialView("_ErrorBorrador");
                }
            }
        }


        // GET: /BorradorAccionPersonal/Search
        public ActionResult Search()
        {
            context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.BorradorAcciones), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.BorradorAcciones)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Registro))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.BorradorAcciones, Convert.ToInt32(ENivelesBorradorAccionPersonal.Analista))] != null)
                {

                    BusquedaBorradorAccionPersonalVM model = new BusquedaBorradorAccionPersonalVM();

                    var listadoTipos = servicioTipo.RetornarTipos()
                          .Select(Q => new SelectListItem
                          {
                              Value = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString(),
                              Text = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString() + " - " + ((CTipoAccionPersonalDTO)Q).DesTipoAccion
                          });

                    model.Tipos = new SelectList(listadoTipos, "Value", "Text");


                    var listadoEstados = servicioEstado.RetornarEstados()
                         .Select(Q => new SelectListItem
                         {
                             Value = ((CEstadoBorradorDTO)Q).IdEntidad.ToString(),
                             Text = ((CEstadoBorradorDTO)Q).DesEstadoBorrador
                         });

                    model.Estados = new SelectList(listadoEstados, "Value", "Text");

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.BorradorAcciones) });
                }
            }
        }


        [HttpPost]
        public ActionResult Search(BusquedaBorradorAccionPersonalVM model)
        {
            try
            {              
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                if (!String.IsNullOrEmpty(model.Funcionario.Cedula) || !String.IsNullOrEmpty(model.Borrador.NumOficio)
                    || (model.FechaRigeDesde.Year > 1 && model.FechaVenceHasta.Year > 1)
                    || (model.FechaRigeIntegraDesde.Year > 1 && model.FechaVenceIntegraHasta.Year > 1)
                    || !String.IsNullOrEmpty(model.EstadoSeleccionado) || model.TipoSeleccionado != 0)
                {
                    List<DateTime> fechas = new List<DateTime>();
                    List<DateTime> fechasIntegra = new List<DateTime>();


                    if (model.FechaRigeDesde.Year > 1 && model.FechaVenceHasta.Year > 1)
                    {
                        fechas.Add(model.FechaRigeDesde);
                        fechas.Add(model.FechaVenceHasta);
                    }

                    //if (model.FechaRigeIntegraDesde.Year > 1 && model.FechaRigeIntegraHasta.Year > 1)
                    //{
                    //    fechasRige.Add(model.FechaRigeIntegraDesde);
                    //    fechasRige.Add(model.FechaRigeIntegraHasta);
                    //}

                    //if (model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1)
                    //{
                    //    fechasVence.Add(model.FechaVenceDesde);
                    //    fechasVence.Add(model.FechaVenceHasta);
                    //}
                    if (model.FechaRigeIntegraDesde.Year > 1 && model.FechaVenceIntegraHasta.Year > 1)
                    {
                        fechasIntegra.Add(model.FechaRigeIntegraDesde);
                        fechasIntegra.Add(model.FechaVenceIntegraHasta);
                    }


                    //  Tipo de Acción
                    if (Convert.ToInt32(model.TipoSeleccionado) > 0)
                    {
                        CTipoAccionPersonalDTO tipoAcc = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };
                        model.Detalle = new CDetalleBorradorAccionPersonalDTO { TipoAccion = tipoAcc };  
                    }


                    //  Estado
                    if (Convert.ToInt32(model.EstadoSeleccionado) > 0)
                    {
                        CEstadoBorradorDTO tipoEst = new CEstadoBorradorDTO
                        {
                            IdEntidad = Convert.ToInt32(model.EstadoSeleccionado)
                        };
                        model.Borrador.EstadoBorrador = tipoEst;
                    }

                    var datos = servicioBorrador.BuscarBorrador(model.Funcionario,
                                                                model.Borrador,
                                                                model.Detalle,
                                                                fechas.ToArray(),
                                                                fechasIntegra.ToArray());


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.Borradores = new List<FormularioBorradorAccionPersonalVM>();

                        foreach (var item in datos)
                        {
                            FormularioBorradorAccionPersonalVM temp = new FormularioBorradorAccionPersonalVM();
                            temp.Borrador = (CBorradorAccionPersonalDTO)item.ElementAt(0);
                            temp.Detalle = (CDetalleBorradorAccionPersonalDTO)item.ElementAt(1);
                            temp.TipoAccion = (CTipoAccionPersonalDTO)item.ElementAt(2);
                            temp.Estado = (CEstadoBorradorDTO)item.ElementAt(3);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(4);
                            temp.Asignado = (CFuncionarioDTO)item.ElementAt(5);

                            model.Borradores.Add(temp);
                        }

                        return PartialView("_SearchResults", model.Borradores);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaRigeDesde.Year > 1 || model.FechaRigeHasta.Year > 1)
                    {
                        if (!(model.FechaRigeDesde.Year > 1 && model.FechaRigeHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Rige, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    if (model.FechaVenceDesde.Year > 1 || model.FechaVenceHasta.Year > 1)
                    {
                        if (!(model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados no son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorBorrador");
                }
                else
                {
                    return PartialView("_ErrorBorrador");
                }
            }
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleBorrador(FormularioBorradorAccionPersonalVM model)
        {
            List<BorradorRptData> modelo = new List<BorradorRptData>();

            modelo.Add(BorradorRptData.GenerarDatosReporteDetalle(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/AccionPersonal"), "BorradorRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo,"PDF");
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteBorradores(List<FormularioBorradorAccionPersonalVM> model)
        {
            List<BorradorRptData> modelo = new List<BorradorRptData>();

            foreach (var item in model)
            {
                modelo.Add(BorradorRptData.GenerarDatosReporte(item, String.Empty));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/AccionPersonal"), "ReporteBorradoresRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo,"PDF");
        }


        [HttpPost]
        public ActionResult GetCategoria(int? idCategoria)
        {
            try
            {
                if (idCategoria != null)
                {
                    var datos = servicioPuesto.BuscarEscalaCategoria(Convert.ToInt16(idCategoria));
                    var categoria = (CEscalaSalarialDTO)datos.ElementAt(0);
                    return Json(new { success = true, salario = categoria.SalarioBase, aumento = categoria.MontoAumentoAnual }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, salario = "0", aumento = "0 ", mensaje = "No existe la Categoría" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, salario = "0", aumento = "0 ", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}