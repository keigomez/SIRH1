using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.PerfilUsuarioLocal;
using SIRH.Web.ViewModels;
using SIRH.DTO;
using SIRH.Web.Helpers;
using System.Security.Principal;
using SIRH.Web.UserValidation;
//using SIRH.Web.FuncionarioService;
using SIRH.Web.FuncionarioLocal;

namespace SIRH.Web.Controllers
{
    public class PerfilUsuarioController : Controller
    {
        CPerfilUsuarioServiceClient perfilUsuario = new CPerfilUsuarioServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        //
        // GET: /PerfilUsuario/
        public ActionResult Index()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), Convert.ToInt32(EAccionesBitacora.Login), 0,
                        CAccesoWeb.ListarEntidades(typeof(CPerfilDTO).Name));
                    return View();
                }
            } catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType=error.Message, modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
        }
        
        // GET: /PerfilUsuario/Create
        public ActionResult Create()
        {
            try {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Usuarios)]) ||
                        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Usuarios, 4)] != null)
                    {
                        return View();
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                    }
                }
            }
            catch (Exception error) {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType=error.Message,modulo = Convert.ToInt32(EModulosHelper.Usuarios) });               
            }

        }

        [HttpPost]
        public ActionResult Create(PerfilUsuarioVM model, string SubmitButton)
        {
            try
            {
                if(ModelState.IsValid == true)
                {
                    if (!String.IsNullOrEmpty(model.Funcionario.Cedula) && !String.IsNullOrEmpty(model.Usuario.NombreUsuario))
                    {
                        var resultado = perfilUsuario.RegistrarUsuario(model.Usuario.NombreUsuario,model.Funcionario.Cedula);
                        if(resultado != null)
                        {
                            if (resultado.GetType() != typeof(CErrorDTO))
                            {
                                model.Usuario = (CUsuarioDTO)resultado;
                                return PartialView("_Success");
                            }

                            else
                            {
                                ModelState.AddModelError("Registro", ((CErrorDTO)resultado).MensajeError);
                                throw new Exception("Registro");
                            }
                        }else
                        {
                            throw new Exception("Ha ocurrido un error al registrar el usuario, favor comunicarse con el personal encargado.");
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                        throw new Exception("Busqueda");
                    }
                }else
                {
                    throw new Exception("Busqueda");
                }
                         
                
            }
            catch (Exception error) {
                return PartialView("_ErrorUsuario");
            }
                  
        }

        public ActionResult Browse(string username)
        {
            PerfilUsuarioVM perfilvm = new PerfilUsuarioVM();
            List<CBaseDTO> perfilDescarga = new List<CBaseDTO>();
            //perfilDescarga = perfilUsuario.RetornarPerfilUsuario(username).ToList();
            perfilvm.Funcionario = (CFuncionarioDTO)perfilDescarga.ElementAt(0);
            perfilvm.Usuario = (CUsuarioDTO)perfilDescarga.ElementAt(1);
            return View(perfilvm);
        }

        // GET: /PerfilUsuario/
        public ActionResult Search()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Usuarios)]) ||
                        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Usuarios, 4)] != null)
                    {
                        return View();
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                    }
                }
            }
            catch (Exception error)
            {
                ViewData["mode"] = "regreso";
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message , modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
        }

        [HttpPost]
        public ActionResult Search(PerfilUsuarioVM model)
        {
            return View();
        }

        public ActionResult AsignarPermisos()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Usuarios)]) ||
                        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Usuarios, 4)] != null)
                    {

                        return View();
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                    }
                }
            }
            catch (Exception error)
            {
                ViewData["mode"] = "regreso";
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
        }

        [HttpPost]
        public ActionResult AsignarPermisos(PerfilUsuarioVM modelo, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        if (!String.IsNullOrEmpty(modelo.Usuario.NombreUsuario))
                        {
                            Session["lista"] = null;
                            var resultado = perfilUsuario.RetornarPerfilUsuario("MOPT\\" + modelo.Usuario.NombreUsuario);
                            if (resultado != null)
                            {
                                if (resultado.Count() >= 2)
                                {
                                    modelo.Usuario = (CUsuarioDTO)resultado.ElementAt(0).ElementAt(0);
                                    modelo.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(0);

                                    var permisos = perfilUsuario.ListarPermisos();
                                    
                                    if (permisos != null) {
                                        if(permisos.Count() > 0)
                                        {
                                                                                     
                                            List<string> perfiles = new List<string>();
                                            foreach (var item in permisos) {
                                                if (!perfiles.Contains(((CCatPermisoDTO)item).Perfil))
                                                {
                                                    perfiles.Add(((CCatPermisoDTO)item).Perfil);
                                                }
                                            }
                                            Session["perfiles"] = perfiles;
                                            Session["permisos"] = permisos.ToList();
                                            modelo.Permiso = new SelectList(new List<Object>().Select(Q => new SelectListItem
                                            {
                                                Value = "",
                                                Text = ""
                                            }), "Value", "Text");

                                            modelo.Perfil = new SelectList(perfiles.Select(Q => new SelectListItem
                                            {
                                                Value = Q.ToString(),
                                                Text = Q.ToString()
                                            }), "Value", "Text");


                                        }

                                        return PartialView("_AsignarPermisosResult", modelo);
                                    }
                                    else
                                    {
                                        throw new Exception("Busqueda");
                                    }
                                    
                                }
                                else
                                {
                                    if (resultado.Count() == 0)
                                    {
                                        ModelState.AddModelError("Busqueda", "No se encontró el usuario ingresado");
                                        throw new Exception("Busqueda");
                                    }
                                    throw new Exception("Busqueda");
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.ElementAt(0).ElementAt(0)).MensajeError);
                                throw new Exception("Busqueda");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }else
                {
                    if (ModelState.IsValid == true)
                    {
                        var permisos = (List<CCatPermisoDTO>)Session["lista"];
                        if (permisos != null) { 
                            var resultado = perfilUsuario.AsignarAccesosUsuario(modelo.Usuario.NombreUsuario, permisos.ToArray());
                            if(resultado != null)
                            {
                                if (resultado.ElementAt(0).GetType() != typeof(CErrorDTO))
                                {
                                    
                                    return PartialView("_Success");
                                }else
                                {
                                    ModelState.AddModelError("Busqueda", (((CErrorDTO)resultado.ElementAt(0)).MensajeError));
                                    throw new Exception("Busqueda");
                                }
                                    
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de asignar los permisos, cantacte el personal a cargo");
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
                

            }
            catch (Exception error)
            {
                return PartialView("_ErrorUsuario");
            }
        }

        public ActionResult Perfil(string username) {
            try
            {
                PerfilUsuarioVM modelo = new PerfilUsuarioVM();
                var resultado = perfilUsuario.RetornarPerfilUsuario(username);
                if (resultado != null)
                {
                    if (resultado.Count() >= 3 )
                    {
                        List<CCatPermisoDTO> permisos = new List<CCatPermisoDTO>();
                        List<CPerfilDTO> perfiles = new List<CPerfilDTO>();
                        modelo.Usuario = (CUsuarioDTO)resultado.ElementAt(0).ElementAt(0);
                        modelo.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(0);

                        for (int j = 2; j < resultado.Count(); j++)
                        {
                            for (int i = 0; i < resultado.ElementAt(j).Count(); i++)
                            {
                                if (resultado.ElementAt(j).ElementAt(i).GetType() == typeof(CPerfilDTO))
                                    perfiles.Add((CPerfilDTO)resultado.ElementAt(j).ElementAt(i));
                                else
                                    permisos.Add((CCatPermisoDTO)resultado.ElementAt(j).ElementAt(i));
                            }
                        }
                        modelo.Permisos = permisos;
                        modelo.Perfiles = perfiles.OrderBy(Q => Q.IdEntidad).ToList();
                        foreach (var item in modelo.Perfiles)
                        {
                            item.Mensaje = DeterminarControllerPermiso(item.IdEntidad);
                        }

                        modelo.Perfiles = modelo.Perfiles.OrderBy(Q => Q.NomPerfil).ToList();

                        return View(modelo);
                    }
                    else
                    {    
                        ModelState.AddModelError("Busqueda", "Este usuario no tiene ningún permiso asociado al sistema");
                        throw new Exception("Busqueda");
                    }

                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.ElementAt(0).ElementAt(0)).MensajeError);
                    throw new Exception("Busqueda");
                }

            }
            catch (Exception error) {
                ViewData["mode"] = "regreso";
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
               
        }

        private string DeterminarControllerPermiso(int perfil)
        {
            switch (perfil)
            {
                case 1:
                    return "Reportes,PerfilUsuario,Caucion,Desarraigo,PagoFeriado,MarcasAsistencia,"
                            + "BorradorAccionPersonal,Carrera,Vacantes,Calificacion,AccionPersonal,"
                            + "RegistroIncapacidad,Planilla,ViaticoCorrido,RegistroTiempoExtra,"
                            + "Vacaciones,Archivo,ManualCargos,"
                            + "ComponentePresupuestario";
                case 2:
                    return "Reportes";
                case 3:
                    return "PerfilUsuario";
                case 4:
                    return "Caucion";
                case 5:
                    return "Desarraigo";
                case 6:
                    return "PagoFeriado";
                case 7:
                    return "MarcasAsistencia";
                case 8:
                    return "BorradorAccionPersonal";
                case 9:
                    return "Carrera";
                case 10:
                    return "Vacantes";
                case 11:
                    return "Calificacion";
                case 12:
                    return "AccionPersonal";
                case 13:
                    return "RegistroIncapacidad";
                case 14:
                    return "Planilla";
                case 15:
                    return "ViaticoCorrido";
                case 16:
                    return "RegistroTiempoExtra";
                case 17:
                    return "Vacaciones";
                case 18:
                    return "Archivo";
                case 23:
                    return "ManualCargos";
                default:
                    return "No tiene permisos";
            }
        }

        public ActionResult List() {

            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Usuarios)]) ||
                        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Usuarios, 4)] != null)
                    {
                        return View();
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                    }
                }
            }
            catch (Exception error)
            {
                ViewData["mode"] = "regreso";
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
        }

        [HttpPost]
        public ActionResult List(PerfilUsuarioVM modelo) {
            try
            {
                if (ModelState.IsValid == true)
                {
                    if (!String.IsNullOrEmpty(modelo.Usuario.NombreUsuario))
                    {
                        var resultado = perfilUsuario.RetornarPerfilUsuario("MOPT\\" + modelo.Usuario.NombreUsuario);
                        if (resultado != null)
                        {
                            if (resultado.Count() >= 3)
                            {
                                List<CCatPermisoDTO> permisos = new List<CCatPermisoDTO>();
                                List<CPerfilDTO> perfiles = new List<CPerfilDTO>();
                                modelo.Usuario = (CUsuarioDTO)resultado.ElementAt(0).ElementAt(0);
                                modelo.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(0);
                                for(int j= 2; j < resultado.Count(); j++)
                                {
                                    for (int i = 0; i < resultado.ElementAt(j).Count(); i++)
                                    {
                                        if (resultado.ElementAt(j).ElementAt(i).GetType() == typeof(CPerfilDTO))
                                            perfiles.Add((CPerfilDTO)resultado.ElementAt(j).ElementAt(i));
                                        else
                                            permisos.Add((CCatPermisoDTO)resultado.ElementAt(j).ElementAt(i));
                                    }
                                }
                                
                                modelo.Permisos = permisos;
                                modelo.Perfiles = perfiles;

                                return PartialView("_ListResult",modelo);
                            }
                            else
                            {
                                if(resultado.Count() == 2)
                                {
                                    ModelState.AddModelError("Busqueda", "El usuario está registrado en el sistema, pero no se le ha asignado ningún perfil para el uso del mismo");
                                    throw new Exception("Busqueda");
                                }
                                throw new Exception("Busqueda");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.ElementAt(0).ElementAt(0)).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    throw new Exception("Busqueda");
                }


            }
            catch (Exception error)
            {
                return PartialView("_ErrorUsuario");
            }
        }

        [HttpPost]
        public ActionResult Actualizar(PerfilUsuarioVM modelo, string SubmitButton)
        {
            if (String.IsNullOrEmpty(SubmitButton)) {
                
                var permisos = (List<CBaseDTO>)Session["permisos"];

                if (permisos != null)
                {
                    if (permisos.Count() > 0)
                    {
                        var perfiles = (List<string>)Session["perfiles"];
                       
                        List<CCatPermisoDTO> lista;
                        if (Session["lista"] != null)
                        {
                            lista = (List<CCatPermisoDTO>)Session["lista"];
                            modelo.Permisos = lista;
                        }
                        
                        modelo.Permiso = new SelectList(permisos.ToList().Where(Q => ((CCatPermisoDTO)Q).Perfil == modelo.PerfilSeleccionado).Select(Q => new SelectListItem
                        {
                            Value = ((CCatPermisoDTO)Q).IdEntidad.ToString(),
                            Text = ((CCatPermisoDTO)Q).NomPermiso.ToString()
                        }), "Value", "Text");

                        modelo.Perfil = new SelectList(perfiles.Select(Q => new SelectListItem
                        {
                            Value = Q.ToString(),
                            Text = Q.ToString()
                        }), "Value", "Text");

                    }

                    return PartialView("_AsignarPermisosResult", modelo);
                }
                else
                {
                    throw new Exception("Busqueda");
                }
            }else
            {
                List<CCatPermisoDTO> lista;
                if (Session["lista"] != null)
                {
                    lista = (List<CCatPermisoDTO>)Session["lista"];
                }
                else
                {
                    lista = new List<CCatPermisoDTO>();
                }
                var permisos = (List<CBaseDTO>)Session["permisos"];

                if (permisos != null)
                {
                    if (permisos.Count() > 0)
                    {
                        var perfiles = (List<string>)Session["perfiles"];
                        CCatPermisoDTO permiso = (CCatPermisoDTO)permisos.Find(P => ((CCatPermisoDTO)P).IdEntidad.ToString() == modelo.PermisoSeleccionado && ((CCatPermisoDTO)P).Perfil == modelo.PerfilSeleccionado);
                        lista.Add(permiso);
                        Session["lista"] = lista;
                        modelo.Permisos = lista;
                        modelo.Permiso = new SelectList(permisos.ToList().Where(Q => ((CCatPermisoDTO)Q).Perfil == modelo.PerfilSeleccionado).Select(Q => new SelectListItem
                        {
                            Value = ((CCatPermisoDTO)Q).IdEntidad.ToString(),
                            Text = ((CCatPermisoDTO)Q).NomPermiso.ToString()
                        }), "Value", "Text");

                        modelo.Perfil = new SelectList(perfiles.Select(Q => new SelectListItem
                        {
                            Value = Q.ToString(),
                            Text = Q.ToString()
                        }), "Value", "Text");

                        return PartialView("_AsignarPermisosResult", modelo);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }else
                {
                    throw new Exception();
                }
      
            }
            
        }

        public ActionResult Deshabilitar()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Usuarios)]) ||
                        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Usuarios, 4)] != null)
                    {

                        return View();
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                    }
                }
            }
            catch (Exception error)
            {
                ViewBag["mode"] = "regreso";
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
        }

        [HttpPost]
        public ActionResult Deshabilitar(PerfilUsuarioVM modelo)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    if (!String.IsNullOrEmpty(modelo.Usuario.NombreUsuario) && !String.IsNullOrEmpty(modelo.Observacion))
                    {
                        var resultado = perfilUsuario.DeshabilitarUsuario(modelo.Usuario.NombreUsuario,modelo.Observacion);
                        if (resultado != null) {
                            if (resultado.GetType() != typeof(CErrorDTO))
                            {
                                return PartialView("_Success");
                            }else
                            {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado).MensajeError);
                                throw new Exception("Busqueda");
                            }
                        }else
                        {
                            ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de deshabilitar el usuario, comunicarse con el encargado");
                            throw new Exception("Busqueda");
                        }
                       
                    }
                    else{
                        ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                        throw new Exception("Busqueda");        
                    }
                }else
                {
                    ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                    throw new Exception("Busqueda");
                }
                        
            }
            catch (Exception error) {
                return PartialView("_ErrorUsuario");
            }
            
        }

        public ActionResult AgregarPermiso()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Usuarios), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Usuarios)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Usuarios)]) ||
                        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Usuarios, 4)] != null)
                    {
                        PerfilUsuarioVM modelo = new PerfilUsuarioVM();
                        var perfiles = perfilUsuario.DescargarPerfiles();
                        if (perfiles == null)
                        {
                            perfiles = new List<CPerfilDTO>().ToArray();
                        }
                        Session["perfiles"] = perfiles.ToArray();
                        modelo.Perfil = new SelectList(perfiles.Select(Q => new SelectListItem
                        {
                            Value = ((CPerfilDTO)Q).IdEntidad.ToString(),
                            Text = ((CPerfilDTO)Q).NomPerfil.ToString()
                        }), "Value", "Text");

                        return View(modelo);
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
                    }
                }
            }
            catch (Exception error) {
                ViewData["mode"] = "regreso";
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Usuarios) });
            }
        }

        [HttpPost]
        public ActionResult AgregarPermiso(PerfilUsuarioVM modelo)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    if (!String.IsNullOrEmpty(modelo.PermisoNuevo.NomPermiso) && !String.IsNullOrEmpty(modelo.PerfilSeleccionado))
                    {
                        var perfiles = (CBaseDTO[])Session["perfiles"];
                        if (perfiles == null) {
                            perfiles = perfilUsuario.DescargarPerfiles();
                        }

                        CPerfilDTO perfil = (CPerfilDTO)perfiles.ToList().Find(P => P.IdEntidad.ToString() == modelo.PerfilSeleccionado);

                        var resultado = perfilUsuario.AgregarPermiso(modelo.PermisoNuevo,perfil);
                        
                        if(resultado != null)
                        {
                            if (resultado.GetType() != typeof(CErrorDTO))
                            {
                                modelo.Perfil = new SelectList(perfiles.Select(Q => new SelectListItem
                                {
                                    Value = ((CPerfilDTO)Q).IdEntidad.ToString(),
                                    Text = ((CPerfilDTO)Q).NomPerfil.ToString()
                                }), "Value", "Text");

                                return PartialView("_Success", modelo);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado).MensajeError);
                                throw new Exception();

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de insertar el permiso, cantacte al personal encargado.");
                            throw new Exception();
                        }
                                                 

                    }else
                    {
                        ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                        throw new Exception();
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                    throw new Exception();
                }
                    
            }
            catch (Exception error)
            {
                return PartialView("_ErrorUsuario");
            }
            
        }

        [HttpPost]
        public ActionResult AgregarPerfil(PerfilUsuarioVM modelo)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    if (!String.IsNullOrEmpty(modelo.PerfilNuevo.NomPerfil))
                    {
                        var perfiles = (CBaseDTO[])Session["perfiles"];
                        if (perfiles == null)
                        {
                            perfiles = perfilUsuario.DescargarPerfiles();
                        }

                        var resultado = perfilUsuario.AgregarPerfil(modelo.PerfilNuevo);
                        if (resultado != null)
                        {
                            if (resultado.GetType() != typeof(CErrorDTO))
                            {
                                modelo.Perfil = new SelectList(perfiles.Select(Q => new SelectListItem
                                {
                                    Value = ((CPerfilDTO)Q).IdEntidad.ToString(),
                                    Text = ((CPerfilDTO)Q).NomPerfil.ToString()
                                }), "Value", "Text");

                                return PartialView("_Success",modelo);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado).MensajeError);
                                throw new Exception();

                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de insertar el permiso, cantacte al personal encargado.");
                            throw new Exception();
                        }


                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                        throw new Exception();
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", "Los campos son obligatorios.");
                    throw new Exception();
                }

            }
            catch (Exception error)
            {
                return PartialView("_ErrorUsuario");
            }

        }

    }
}
