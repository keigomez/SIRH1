using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
using SIRH.DTO;
using SIRH.Web.Models;
//using SIRH.Web.PuestoService;
using SIRH.Web.PuestoLocal;
using SIRH.Web.Helpers;
using SIRH.Web.ServicioTSE;
//using SIRH.Web.FuncionarioLocal;
using SIRH.Web.FuncionarioService;
//using SIRH.Web.NombramientoLocal;
using SIRH.Web.NombramientoService;
using SIRH.Web.AntecedentesPenalesService;
using SIRH.Web.FormacionAcademicaLocal;
using SIRH.Web.UserValidation;
using System.Security.Principal;
using SIRH.Web.DesarraigoService;
using SIRH.Web.AccionPersonalService;
//using SIRH.Web.AccionPersonalLocal;

namespace SIRH.Web.Controllers
{
    public class VacantesController : Controller
    {
        CFormacionAcademicaServiceClient servicioCarrera = new CFormacionAcademicaServiceClient();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CPuestoServiceClient servicioPuesto = new CPuestoServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        CDesarraigoServiceClient ServicioDesarraigo = new CDesarraigoServiceClient();
        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();
        //
        // GET: /Vacantes/

        public ActionResult Index()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                }
                else
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), Convert.ToInt32(EAccionesBitacora.Login), 0,
                        CAccesoWeb.ListarEntidades(typeof(CPerfilDTO).Name));
                    return View();
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }
        }
        [HttpPost]
        public ActionResult Vacante_Save(MovimientoPuestoModel modelito)
        {
            try
            {
                CPuestoServiceClient puestoService = new CPuestoServiceClient();

                CMovimientoPuestoDTO movimiento = new CMovimientoPuestoDTO();

                movimiento.CodOficio = modelito.MovimientoPuesto.CodOficio;
                movimiento.FecMovimiento = Convert.ToDateTime(modelito.MovimientoPuesto.FecMovimiento);
                movimiento.FechaVencimiento = Convert.ToDateTime(modelito.MovimientoPuesto.FechaVencimiento);
                movimiento.MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = Convert.ToInt32(modelito.MotivoSeleccionado) };
                movimiento.Puesto = new CPuestoDTO { CodPuesto = modelito.MovimientoPuesto.Puesto.CodPuesto };
                movimiento.Explicacion = modelito.MovimientoPuesto.Explicacion;

                var resultado = puestoService.GuardarMovimientoPuesto(movimiento);

                //var resultado = new CBaseDTO { Mensaje = "sí" };

                if (resultado.Mensaje.Contains("error"))
                {
                    Session["errorVacante"] = resultado.Mensaje;
                    return JavaScript("window.location = '/Vacantes/Vacante_Prepare?error=true'");
                }
                else
                {
                    if (!((modelito.MotivoSeleccionado > 0 && modelito.MotivoSeleccionado < 7) || modelito.MotivoSeleccionado == 8 ||
                        modelito.MotivoSeleccionado == 18))
                    {
                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                        {
                            IdEntidad = 1 // Registrado
                        };

                        CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(DefinirMotivoParaAccion(modelito.MotivoSeleccionado))
                        };

                        CAccionPersonalDTO accion = new CAccionPersonalDTO
                        {
                            AnioRige = DateTime.Now.Year,
                            CodigoModulo = Convert.ToInt32(EModulosHelper.Vacantes),
                            CodigoObjetoEntidad = resultado.IdEntidad,
                            FecRige = movimiento.FecMovimiento,
                            FecVence = movimiento.FechaVencimiento.Year == 1 ? (DateTime?)null : movimiento.FechaVencimiento,
                            FecRigeIntegra = movimiento.FecMovimiento,
                            FecVenceIntegra = movimiento.FechaVencimiento.Year == 1 ? (DateTime?)null : movimiento.FechaVencimiento,
                            Nombramiento = modelito.Nombramiento,
                            Observaciones = movimiento.Explicacion
                        };

                        modelito.Funcionario.Sexo = GeneroEnum.Indefinido;

                        var respuestaAP = servicioAccion.AgregarAccion(modelito.Funcionario, //aquí tengo que enviar el funcionario para que haga la acción de personal
                                                         estado,
                                                         tipoAP,
                                                         accion,
                                                         null);
                    }

                    //string envia = principal.Identity.Name.Split('\\')[1];

                    //var correo = new EmailWebHelper
                    //{
                    //    Asunto = "SIRH - Movimiento de funcionario",
                    //    Destinos = "ana.rojas@mopt.go.cr, sally.monge@mopt.go.cr, " + envia + "@mopt.go.cr",
                    //    EmailBody = "Estimados(as) usuarios(as), <br/><br/> Se le informa que se ha registrado un movimiento para el funcionario: <b>" + modelito.Funcionario.Cedula + " - " + modelito.Funcionario.Nombre + " " + modelito.Funcionario.PrimerApellido + " " + modelito.Funcionario.SegundoApellido + "</b><br/><br /> Puede encontrar el detalle del funcionario y el movimiento realizado (estado del funcionario) <a href='http://sisrh.mopt.go.cr:84/Funcionario/Details?cedula=" + modelito.Funcionario.Cedula + "'>AQUÍ</a> <br/><br/> Saludos."
                    //};

                    //var enviado = correo.EnviarCorreo();

                    return JavaScript("window.location = '/Vacantes/Vacante_Prepare?codPuesto=" + movimiento.Puesto.CodPuesto + "&movimiento="+movimiento.MotivoMovimiento.DesMotivo+"'");
                }
            }
            catch (Exception error)
            {
                Session["errorVacante"] = error.Message;
                return JavaScript("window.location = '/Vacantes/Vacante_Prepare?error=true'");
            }
        }

        private ETipoAccionesHelper DefinirMotivoParaAccion(int motivo)
        {
            switch (motivo)
            {
                case 7:
                    return ETipoAccionesHelper.DespidoCausa;
                case 9:
                case 13:
                case 14:
                case 15:
                case 16:
                case 20:
                case 51:
                    return ETipoAccionesHelper.Renuncia;
                case 10:
                case 11:
                case 19:
                    return ETipoAccionesHelper.CeseFunciones;
                case 12:
                    return ETipoAccionesHelper.PermisoSinSalario;
                case 17:
                case 65:
                    return ETipoAccionesHelper.SuspensionIndefinida;
                case 21:
                case 66:
                    return ETipoAccionesHelper.SuspensionTemporal;
                case 22:
                    return ETipoAccionesHelper.PermisoSueldoTotal;
                case 35:
                case 52:
                    return ETipoAccionesHelper.CeseInterinidad;
                case 45:
                case 47:
                case 48:
                    return ETipoAccionesHelper.Traslado;
                case 46:
                case 49:
                case 50:
                    return ETipoAccionesHelper.TrasladoInterino;
                default:
                    throw new Exception("No es posible tramitar la acción de personal, el código " + motivo + "no es válido");
            }
        }

        public ActionResult Vacante_Prepare(string codPuesto, string error)
        {
            CPuestoDTO puesto = new CPuestoDTO();
            CPuestoDTO puestoExtra = new CPuestoDTO();
            if (error == null)
            {
                if (codPuesto.Contains("_"))
                {
                    puesto.CodPuesto = codPuesto.Split('_')[0];
                    puestoExtra.CodPuesto = codPuesto.Split('_')[1];
                    var perfilExtra = servicioPuesto.DescargarPerfilPuestoAcciones(puestoExtra.CodPuesto);
                    puestoExtra = (CPuestoDTO)perfilExtra.ElementAt(3).ElementAt(0);
                    ViewBag.PuestoExtra = puestoExtra.CodPuesto;
                    ViewBag.EstadoExtra = puestoExtra.EstadoPuesto.DesEstadoPuesto;
                }
                else
                {
                    puesto.CodPuesto = codPuesto;
                }

                var perfil = servicioPuesto.DescargarPerfilPuestoAcciones(puesto.CodPuesto);
                if (perfil.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    puesto = (CPuestoDTO)perfil.ElementAt(3).ElementAt(0);
                    /*if (puesto.EstadoPuesto.DesEstadoPuesto.StartsWith("VAC"))
                    {*/
                        string body = "";
                        string envia = principal.Identity.Name.Split('\\')[1];
                        if (codPuesto.Contains("_"))
                        {
                            if (puesto.CodPuesto != puestoExtra.CodPuesto)
                            {
                                body = "Estimados(as) usuarios(as), <br/><br/> Se le informa que se ha registrado un nuevo movimiento para el puesto: <b>" + puesto.CodPuesto + "</b> cuyo estado actual es <b>" + puesto.EstadoPuesto.DesEstadoPuesto + "</b><br/><br/> Además, en este movimiento se afectó también al puesto <b>" + puestoExtra.CodPuesto + "</b> cuyo estado actual es <b>" + puestoExtra.EstadoPuesto.DesEstadoPuesto + "</b><br/><br/>  Puede encontrar el detalle del primer puesto <a href='http://sisrh.mopt.go.cr:84/Puesto/Details?codPuesto=" + puesto.CodPuesto + "'>AQUÍ</a> <br/><br/> Y el detalle del segundo puesto <a href='http://sisrh.mopt.go.cr:84/Puesto/Details?codPuesto=" + puestoExtra.CodPuesto + "'>AQUÍ</a> <br/><br/> Saludos.";
                            }
                            else
                            {
                                body = "Estimados(as) usuarios(as), <br/><br/> Se le informa que se ha registrado un nuevo movimiento para el puesto: <b>" + puesto.CodPuesto + "</b> cuyo estado actual es <b>" + puesto.EstadoPuesto.DesEstadoPuesto + "</b><br/><br/> Puede encontrar el detalle del puesto <a href='http://sisrh.mopt.go.cr:84/Puesto/Details?codPuesto=" + puesto.CodPuesto + "'>AQUÍ</a> <br/><br/> Saludos.";
                            }
                        }
                        else
                        {
                            body = "Estimados(as) usuarios(as), <br/><br/> Se le informa que se ha registrado un nuevo movimiento para el puesto: <b>" + puesto.CodPuesto + "</b> cuyo estado actual es <b>" + puesto.EstadoPuesto.DesEstadoPuesto + "</b><br/><br/> Puede encontrar el detalle del puesto en el siguiente enlace: <a href='http://sisrh.mopt.go.cr:84/Puesto/Details?codPuesto=" + puesto.CodPuesto + "'>AQUÍ</a> <br/><br/> Saludos.";
                        }

                        var correo = new EmailWebHelper
                        {
                            Asunto = "SIRH - Movimiento de puesto",
                            Destinos = "deivert.guiltrichs@mopt.go.cr" + "," + envia + "@mopt.go.cr",
                            EmailBody = body
                        };

                        var enviado = correo.EnviarCorreo();
                    //}
                }
                else
                {
                    error = "No fue posible cargar la información del puesto";
                    puesto.Mensaje = error;
                }
                    //NotificacionesEmailHelper.SendEmail("deivert.guiltrichs@mopt.go.cr",
                    //                                    "Confección de nombramiento para el puesto # " + codPuesto,
                    //                                    "Buenas tardes, <br/> <br/> Se le informa que el puesto # " + codPuesto + " ha sido asignado a un nuevo funcionario."
                    //                                    + "<br/> <br/>  Para más información, puede ingresar a http://sisrh.mopt.go.cr:84/Puesto ."
                    //                                    + "<br/> <br/>  Por favor no responda a este correo ya que, fue generado automáticamente.");
            }
            else
            {
                error = Session["errorVacante"].ToString();
                puesto.Mensaje = error;
            }
            return View(puesto);
        }

        //
        // GET: /Vacantes/RegistrarCandidato
        public ActionResult RegistrarCandidato()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                }
                else
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), Convert.ToInt32(EAccionesBitacora.Login), 0,
                        CAccesoWeb.ListarEntidades(typeof(CPerfilDTO).Name));
                    return View();
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }
        }

        //
        // POST: /Vacantes/RegistrarCandidato

        [HttpPost]
        public ActionResult RegistrarCandidato(PerfilBasicoFuncionarioVM model, string SubmitButton)
        {
            try
            {              
                if (SubmitButton == "Buscar")
                {
                    if (!String.IsNullOrEmpty(model.Funcionario.Cedula))
                    {                      
                        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
                        wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();

                        model.MensajePoderJudicial = "La persona consultada no registra antecedentes penales en el Poder Judicial.";
                        var funcionario = servicioFuncionario.BuscarFuncionarioBase(model.Funcionario.Cedula);

                        if (funcionario != null)
                        {
                            if (funcionario.GetType() != typeof(CErrorDTO))
                            {
                                model.Funcionario = (CFuncionarioDTO)funcionario;   
                            }
                            else
                            {
                                //aquí debería tirar un error
                                var persona = servicioTSE.wsConsultaDatosPersona(0, FuncionarioHelper.CedulaEmulacionATSE(model.Funcionario.Cedula), true);
                                model.Funcionario = FuncionarioHelper.ConvertirPersonaTSEAFuncionario(persona);
                                model.Edad = Convert.ToInt32(persona.Edad.Substring(0, 2));
                                model.MensajeTSE = "La persona consultada no ha trabajado en el MOPT anteriormente, los datos mostrados provienen de los registros del Tribunal Supremo de Elecciones";
                                model.EstadoCivil = new CHistorialEstadoCivilDTO {CatEstadoCivil = new CCatEstadoCivilDTO {DesEstadoCivil = persona.Indice_Estado_Civil } };
                            }
                        }
                        else
                        {
                            //aquí debería tirar un error
                            var persona = servicioTSE.wsConsultaDatosPersona(0, model.Funcionario.Cedula, true);
                            model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);
                            model.Edad = Convert.ToInt32(persona.Edad.Substring(0, 2));
                            model.MensajeTSE = "La persona consultada no ha trabajado en el MOPT anteriormente, los datos mostrados provienen de los registros del Tribunal Supremo de Elecciones";
                            model.EstadoCivil = new CHistorialEstadoCivilDTO { CatEstadoCivil = new CCatEstadoCivilDTO { DesEstadoCivil = persona.Indice_Estado_Civil } };

                        }
                        var tipos = servicioFuncionario.ListarTiposContacto().ToList();
                        List<CTipoContactoDTO> list = new List<CTipoContactoDTO>();
                        model.DatosContacto = new List<CInformacionContactoDTO>();
                        if (tipos != null)
                        {
                            foreach(var item in tipos)
                            {
                                list.Add((CTipoContactoDTO)item);
                                model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = (CTipoContactoDTO)item });
                            }
                        }else
                        {
                            list = new List<CTipoContactoDTO>
                                {
                                    new CTipoContactoDTO { DesTipoContacto = "Telefono Domicilio" },
                                    new CTipoContactoDTO { DesTipoContacto = "Telefono Trabajo" },
                                    new CTipoContactoDTO { DesTipoContacto = "Telefono Celular" },
                                    new CTipoContactoDTO { DesTipoContacto = "Telefono Adicional" },
                                    new CTipoContactoDTO { DesTipoContacto = "Correo Electrónico" },
                                };
                            //model.DatosContacto = new List<CInformacionContactoDTO>();
                            model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = list[0] });
                            model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = list[1] });
                            model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = list[2] });
                            model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = list[3] });
                            model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = list[4] });
                        }
                        model.TiposContacto = list;


                        return PartialView("_FormularioCandidato", model);
                    }
                    else
                    {
                        //ModelState.AddModelError("Busqueda", "Los cédula es obligatoria.");
                        throw new Exception("Busqueda");
                    }

                }
                else
                {
                    if (SubmitButton.Contains("Continuar"))
                    {
                        bool isNull = true;
                        foreach (var item in model.DatosContacto)
                        {
                            if (!String.IsNullOrEmpty(item.DesContenido))
                            {
                                isNull = false;
                            }
                        }

                        if (isNull)
                        {
                            ModelState.AddModelError("Guardar", "Debe completar al menos un campo de los anteriores");
                            throw new Exception("Guardar");
                        }                                                

                        if (model.MensajeTSE != null)
                        {
                            model.Funcionario.EstadoFuncionario = new CEstadoFuncionarioDTO { IdEntidad = 20 };
                            model.Funcionario.Cedula = ConvertirCedula(model.Funcionario.Cedula);
                            var resultado = new List<object>();
                            //var resultado = servicioFuncionario.GuardarDatosPersonalesFuncionario(model.Funcionario,model.EstadoCivil,model.DatosContacto.ToArray());
                            if (resultado != null)
                            {
                                if (resultado.Count() > 0)
                                {
                                    foreach(var item in resultado)
                                    {
                                        if (item.GetType() == typeof(CErrorDTO))
                                        {
                                            ModelState.AddModelError("Guardar", ((CErrorDTO)item).MensajeError);

                                            throw new Exception("Guardar");
                                        }
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("Guardar", "Ha ocurrido un error al guardar los datos.");

                                    throw new Exception("Guardar");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Guardar", "Ha ocurrido un error al guardar los datos.");

                                throw new Exception("Guardar");
                            }

                            PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                            modelo.Funcionario = model.Funcionario;
                            modelo.InformacionContacto = model.DatosContacto;
                            for (int i = 0; i < modelo.InformacionContacto.Count(); i++)
                            {
                                modelo.InformacionContacto[i].TipoContacto = model.TiposContacto[i];
                            }
                            CDireccionDTO dir = new CDireccionDTO
                            {
                                DirExacta = "",
                                Distrito = new CDistritoDTO
                                {
                                    NomDistrito = "",
                                    Canton = new CCantonDTO
                                    {
                                        NomCanton = "",
                                        Provincia = new CProvinciaDTO
                                        {
                                            NomProvincia = ""
                                        }
                                    }
                                }
                            };
                            modelo.Direccion = dir;

                            return PartialView("_DesgloseCandidato", modelo);

                        }
                        else
                        {
                            ModelState.AddModelError("Guardar", "El funcionario que intenta guardar ya existe en la base de datos del MOPT. Presione en el botón siguiente para continuar");
                            throw new Exception("Guardar");
                        }
                
                    }
                    else
                    {
                        if (SubmitButton.Contains("Siguiente"))
                        {
                            PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                            modelo.Funcionario = model.Funcionario;
                            modelo.InformacionContacto = model.DatosContacto;
                            for (int i = 0; i < modelo.InformacionContacto.Count(); i++)
                            {
                                modelo.InformacionContacto[i].TipoContacto = model.TiposContacto[i];
                            }
                            CDireccionDTO dir = new CDireccionDTO
                            {
                                DirExacta = "",
                                Distrito = new CDistritoDTO
                                {
                                    NomDistrito = "",
                                    Canton = new CCantonDTO
                                    {
                                        NomCanton = "",
                                        Provincia = new CProvinciaDTO
                                        {
                                            NomProvincia = ""
                                        }
                                    }
                                }
                            };
                            var direccion = servicioFuncionario.DescargarDireccion(model.Funcionario.Cedula);

                            if (direccion != null)
                            {
                                if (direccion.Count() > 0)
                                {
                                    dir.DirExacta = ((CDireccionDTO)direccion[0]).DirExacta;
                                    dir.Distrito = ((CDistritoDTO)direccion[3]);
                                    dir.Distrito.Canton = ((CCantonDTO)direccion[2]);
                                    dir.Distrito.Canton.Provincia = ((CProvinciaDTO)direccion[1]);
                                }
                            }

                            modelo.Direccion = dir;

                            return PartialView("_DesgloseCandidato", modelo);

                        }
                        else
                        {
                            bool isNull = true;
                            foreach (var item in model.DatosContacto)
                            {
                                if (!String.IsNullOrEmpty(item.DesContenido))
                                {
                                    isNull = false;
                                }
                            }

                            if (isNull)
                            {
                                ModelState.AddModelError("Guardar", "Debe completar al menos un campo de los anteriores");
                                throw new Exception("Guardar");
                            }

                            if (model.MensajeTSE != null)
                            {
                                model.Funcionario.EstadoFuncionario = new CEstadoFuncionarioDTO { IdEntidad = 20 };
                                model.Funcionario.Cedula = ConvertirCedula(model.Funcionario.Cedula);
                                var resultado = new List<object>();
                                //var resultado = servicioFuncionario.GuardarDatosPersonalesFuncionario(model.Funcionario, model.EstadoCivil, model.DatosContacto.ToArray());
                                if (resultado != null)
                                {
                                    if (resultado.Count() > 0)
                                    {
                                        foreach (var item in resultado)
                                        {
                                            if (item.GetType() == typeof(CErrorDTO))
                                            {
                                                ModelState.AddModelError("Guardar", ((CErrorDTO)item).MensajeError);

                                                throw new Exception("Guardar");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Guardar", "Ha ocurrido un error al guardar los datos.");

                                        throw new Exception("Guardar");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("Guardar", "Ha ocurrido un error al guardar los datos.");

                                    throw new Exception("Guardar");
                                }
                                
                                return PartialView("_Success");

                            }
                            else
                            {
                                ModelState.AddModelError("Guardar", "El funcionario que intenta guardar ya existe en la base de datos del MOPT. Presione en el botón siguiente para continuar");
                                throw new Exception("Guardar");
                            }
                        }
                    }
                }
                                
            }
            catch (Exception error)
            {
                return PartialView("_ErrorVacantes");
            }
            
        }

        //
        // GET: /Vacantes/DetalleCandidato
        public ActionResult DetalleCandidato()
        {
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

                if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                }
                else
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), Convert.ToInt32(EAccionesBitacora.Login), 0,
                        CAccesoWeb.ListarEntidades(typeof(CPerfilDTO).Name));
                    
                        return View();
                    
                        
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }
        }

        //
        // POST: /Vacantes/DetalleCandidato
        [HttpPost]
        public ActionResult DetalleCandidato(PerfilFuncionarioVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if(model.Funcionario.Cedula != null)
                    {
                        wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();

                        var funcionario = servicioFuncionario.BuscarFuncionarioBase(model.Funcionario.Cedula);

                        if (funcionario != null)
                        {
                            if (funcionario.GetType() != typeof(CErrorDTO))
                            {
                                model.Funcionario = (CFuncionarioDTO)funcionario;
                            }
                            else
                            {
                                var persona = servicioTSE.wsConsultaDatosPersona(0, model.Funcionario.Cedula, true);
                                model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);

                            }
                        }
                        else
                        {
                            var persona = servicioTSE.wsConsultaDatosPersona(0, model.Funcionario.Cedula, true);
                            model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);
                        }

                        var informacion = servicioFuncionario.DescargarInformacionContacto(model.Funcionario.Cedula);
                        List<CInformacionContactoDTO> list = new List<CInformacionContactoDTO>();
                        if (informacion != null)
                        {
                            
                            if(informacion.Count() > 0)
                            {
                                foreach(var item in informacion)
                                {
                                    list.Add((CInformacionContactoDTO)item);
                                }
                            }                            
                        }
                        model.InformacionContacto = list;

                        var direccion = servicioFuncionario.DescargarDireccion(model.Funcionario.Cedula);

                        CDireccionDTO dir = new CDireccionDTO
                        {
                            DirExacta = "",
                            Distrito = new CDistritoDTO
                            {
                                NomDistrito = "",
                                Canton = new CCantonDTO
                                {
                                    NomCanton = "",
                                    Provincia = new CProvinciaDTO
                                    {
                                        NomProvincia = ""
                                    }
                                }
                            }
                        }; 
                        if (direccion != null) {
                            if(direccion.Count() > 0)
                            {
                                dir.DirExacta = ((CDireccionDTO)direccion[0]).DirExacta;
                                dir.Distrito = ((CDistritoDTO)direccion[3]);
                                dir.Distrito.Canton = ((CCantonDTO)direccion[2]);
                                dir.Distrito.Canton.Provincia = ((CProvinciaDTO)direccion[1]);
                            }
                        }

                        model.Direccion = dir;

                        return PartialView("_DesgloseCandidato", model);
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
          
                }


                return View(model);
            }
            catch (Exception error)
            {
                return PartialView("_ErrorVacantes");
            }
            
        }

        public ActionResult AsignarCandidatoPedimento(string cedula)
        {
            try
            {
                CandidatoPuestoVM model = new CandidatoPuestoVM();
                CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
                wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
                if(cedula != null)
                {
                    var funcionario = servicioFuncionario.BuscarFuncionarioBase(cedula);

                    if (funcionario != null)
                    {
                        if (funcionario.GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)funcionario;
                        }
                        else
                        {
                            throw new Exception("Antes de nombrar un candidato debe estar registrado como funcionario. Proceda a registrar el candidato.");
                        }
                    }
                    else
                    {
                        throw new Exception("Antes de nombrar un candidato debe estar registrado como funcionario. Proceda a registrar el candidato.");
                    }

                    return View(model);
                }else
                {
                    throw new Exception("El campo de la cédula está nulo.");
                }
                
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }
        }

        [HttpPost]
        public ActionResult AsignarCandidatoPedimento(CandidatoPuestoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (!String.IsNullOrEmpty(model.PedimentoPuesto.NumeroPedimento))
                    {
                        var resultado = servicioPuesto.DescargarPuestoPedimento(model.PedimentoPuesto.NumeroPedimento);
                        if (resultado != null) {
                            foreach(var item in resultado.ToList())
                            {
                                if(item.GetType() == typeof(CErrorDTO))
                                {
                                    ModelState.AddModelError("Buscar", ((CErrorDTO)item).MensajeError);
                                    throw new Exception("Buscar");
                                }
                            }

                            model.Puesto = (CPuestoDTO)resultado[0];
                            model.DetallePuesto = (CDetallePuestoDTO)resultado[1];
                            model.PedimentoPuesto = (CPedimentoPuestoDTO)resultado[4];
                            model.UbicacionAdministrativa = (CUbicacionAdministrativaDTO)resultado[5];

                            return PartialView("_DesglosePedimento", model);
                        }else
                        {
                            ModelState.AddModelError("Buscar", "Ha ocurrido un error al buscar el pedimento.");

                            throw new Exception("Buscar");
                        }

                    }else
                    {
                        ModelState.AddModelError("Buscar", "Debe completar todos los campos.");

                        throw new Exception("Buscar");
                    }                                                  
                    
                }
                else
                {
                    if(SubmitButton == "Asignar")
                    {
                        CNombramientoServiceClient servicioNombramiento = new CNombramientoServiceClient();

                        model.Nombramiento = new CNombramientoDTO {
                            FecNombramiento = DateTime.Now,
                            FecRige = DateTime.Now,
                            EstadoNombramiento = new CEstadoNombramientoDTO { IdEntidad = 12 },
                            Puesto = model.Puesto,
                            Funcionario = model.Funcionario
                        };

                        var resultado = servicioNombramiento.GuardarNombramiento(model.Nombramiento, null);

                        if (resultado == null || resultado.GetType() == typeof(CErrorDTO))
                        {
                            ModelState.AddModelError("Buscar", "Ha ocurrido un error a la hora de guardar el nombramiento.");
                            throw new Exception("Buscar");
                        }

                        return PartialView("_Success");
                    }else
                    {
                        return JavaScript("window.top.location.href ='" + Url.Action("DetalleCandidato") + "';");
                    }
                    
                }
            } catch (Exception error)
            {
                return PartialView("_ErrorVancantes");
            }
        }

        public ActionResult NombrarCandidato(string cedula)
        {
            try
            {
                FuncionarioDetalleContratacionVM model = new FuncionarioDetalleContratacionVM();
                wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();

                if (cedula != null)
                {
                    var funcionario = servicioFuncionario.BuscarFuncionarioBase(cedula);

                    if (funcionario != null)
                    {
                        if (funcionario.GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)funcionario;
                        }
                        else
                        {
                            throw new Exception("Antes de nombrar un candidato debe estar registrado como funcionario. Proceda a registrar el candidato.");
                        }
                    }
                    else
                    {
                        throw new Exception("Antes de nombrar un candidato debe estar registrado como funcionario. Proceda a registrar el candidato.");
                    }

                    var puesto = servicioPuesto.DetallePuestoPorCedula(cedula);
                    if (puesto != null && puesto.Count() > 0){
                        if (puesto.FirstOrDefault().GetType() == typeof(CErrorDTO)) {
                            throw new Exception(((CErrorDTO)puesto[0]).MensajeError);
                        }
                    }
                    else
                    {
                        throw new Exception("Ha ocurrido un error obteniendo el puesto asociado al candidato.");

                    }

                    

                    model.PedimentoPuesto = new CPedimentoPuestoDTO
                    {
                        FechaPedimento = DateTime.Now,
                        NumeroPedimento = "MOPT-DGIRH-DGSC-1220217",
                        ObservacionesPedimento = "Concurso externo para suplir vacante por fallecimiento"
                    };
                    model.Puesto = new CPuestoDTO
                    {
                        CodPuesto = "026577",
                        EstadoPuesto = new CEstadoPuestoDTO { DesEstadoPuesto = "Activo" }
                    };
                    model.DetallePuesto = new CDetallePuestoDTO
                    {
                        Clase = new CClaseDTO { DesClase = "Administración" },
                        Especialidad = new CEspecialidadDTO { DesEspecialidad = "Administración de Recursos Humanos" },
                    };
                    model.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
                    {
                        Division = new CDivisionDTO { NomDivision = "División Administrativa" },
                        DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = "Dirección de Gestión Institucional de Recursos Humanos" },
                        Departamento = new CDepartamentoDTO { NomDepartamento = "Gestión de Empleo" },
                        Seccion = new CSeccionDTO { NomSeccion = "Administración de Puestos Vacantes" },
                        Presupuesto = new CPresupuestoDTO { CodigoPresupuesto = "32600 05 05" }
                    };

                    Dictionary<int, string> adscrita = GenerarEntidadFinanciera();
                    model.EntidadesFinancieras = new SelectList(adscrita, "Key", "Value");

                    return View(model);

                }
                else
                {
                    throw new Exception("El campo de la cédula está nulo.");
                }
          
                
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });

            }
        }

        [HttpPost]
        public ActionResult NombrarCandidato(FuncionarioDetalleContratacionVM model)
        {
            try
            {
                if (model.DetalleContratación.FechaIngreso == null || String.IsNullOrEmpty(model.DetalleContratación.FechaMesAumento)
                    || model.DetalleContratación.NumeroAnualidades != 0 || String.IsNullOrEmpty(model.CuentaBancaria.CtaCliente)
                    || model.EntidadSeleccionada != 0)
                {
                    ModelState.AddModelError("Guardar", "Debe completar todos los campos.");

                    throw new Exception("Guardar");
                }

                var funcionario = servicioFuncionario.BuscarFuncionarioBase(model.Funcionario.Cedula);

                if (funcionario != null && funcionario.GetType() != typeof(CErrorDTO)) {
                    model.DetalleContratación.Funcionario = (CFuncionarioDTO)funcionario;
                }
                else
                {
                    ModelState.AddModelError("Guardar", "Ha ocurrido un error al obtener el funcionario.");
                    throw new Exception("Guardar");
                }
                model.CuentaBancaria.EntidadFinanciera = new CEntidadFinancieraDTO { CodEntidad = model.EntidadSeleccionada.ToString() };
            
                var detalle = servicioFuncionario.GuardarDetalleContratacion(model.DetalleContratación,model.CuentaBancaria);

                if(detalle != null)
                {
                    if(detalle.Count() > 0)
                    {
                        foreach(var item in detalle)
                        {
                            if(item.GetType() == typeof(CErrorDTO))
                            {
                                ModelState.AddModelError("Guardar", ((CErrorDTO)item).MensajeError);
                                throw new Exception("Guardar");
                            }
                        }
                    }
                }

                return PartialView("_Success");
            }
            catch (Exception error)
            {
                return PartialView("_ErrorVacantes");
            }
        }

        private Dictionary<int, string> GenerarEntidadFinanciera()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var entidades = servicioFuncionario.ListarEntidadesFinancieras().ToList();
            
            foreach (var item in entidades)
            {
                dic.Add(int.Parse(((CEntidadFinancieraDTO)item).CodEntidad), ((CEntidadFinancieraDTO)item).NomEntidad);
            }
            return dic;
        }

        //
        // GET: /Vacantes/RegistrarDireccionCandidato
        public ActionResult RegistrarDireccionCandidato(string cedula)
        {
            try
            {
                DireccionFuncionarioVM model = new DireccionFuncionarioVM();
                Dictionary<int, string> adscrita = GenerarProvincias();
                model.Provincias = new SelectList(adscrita, "Key", "Value");
                Dictionary<int, string> cantones = GenerarCantones();
                model.Cantones = new SelectList(new Dictionary<int, string>(), "Key", "Value");
                Dictionary<int, string> distritos = GenerarDistritos();
                model.Distritos = new SelectList(new Dictionary<int, string>(), "Key", "Value");

                var resultado = servicioFuncionario.BuscarFuncionarioBase(cedula);
                if (resultado != null)
                {
                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)resultado;
                    }
                    else
                    {
                        throw new Exception("El candidato con la cédula " + cedula + " debe estar registrado. Favor registrar primero el candidato");
                    }
                }
                else
                {
                    throw new Exception("El candidato con la cédula " + cedula + " debe estar registrado. Favor registrar primero el candidato");
                }

                return View(model);
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });

            }

        }

        private Dictionary<int, string> GenerarProvincias()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var localizacion = ServicioDesarraigo.GetLocalizacion(false, false, true);
            var provincias = localizacion[0].ToList();
            Session["provincias"] = provincias;
            foreach (var item in provincias)
            {
                dic.Add(((CProvinciaDTO)item).IdEntidad, ((CProvinciaDTO)item).NomProvincia);
            }

            Session["dicP"] = dic;
            return dic;
        }

        private Dictionary<int, string> GenerarCantones()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var localizacion = ServicioDesarraigo.GetLocalizacion(true, false, false);
            var cantones = localizacion[0].ToList();
            Session["cantones"] = cantones;
            foreach (var item in cantones)
            {
                dic.Add(((CCantonDTO)item).IdEntidad, ((CCantonDTO)item).NomCanton);
            }

            Session["dicC"] = dic;
            return dic;
        }

        private Dictionary<int, string> GenerarDistritos()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var localizacion = ServicioDesarraigo.GetLocalizacion(false, true, false);
            var distritos = localizacion[0].ToList();
            Session["distritos"] = distritos;
            foreach (var item in distritos)
            {
                dic.Add(((CDistritoDTO)item).IdEntidad, ((CDistritoDTO)item).NomDistrito);
            }
            Session["dicD"] = dic;

            return dic;
        }

        //
        // POST: /Vacantes/RegistrarDireccionCandidato
        [HttpPost]
        public ActionResult RegistrarDireccionCandidato(DireccionFuncionarioVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton != null)
                {
                    if (model.DistritoSeleccionado != 0 && !String.IsNullOrEmpty(model.Direccion.DirExacta))
                    {
                        model.Direccion.Distrito = new CDistritoDTO { IdEntidad = model.DistritoSeleccionado };
                        var resultado = servicioFuncionario.GuardarDireccionFuncionario(model.Funcionario, model.Direccion);
                        if (resultado != null)
                        {
                            if (((CRespuestaDTO)resultado).Contenido.GetType() != typeof(CErrorDTO))
                            {
                                return PartialView("_Success");
                            }
                            else
                            {
                                ModelState.AddModelError("Guardar", (((CErrorDTO)((CRespuestaDTO)resultado).Contenido)).MensajeError);
                                throw new Exception("Guardar");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Guardar", "Ha ocurrido un error al intentar guardar la dirección del funcionario");
                            throw new Exception("Guardar");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("Guardar", "Es necesario escoger un distrito y completar el campo de la dirección exacta");
                        throw new Exception("Guardar");
                    }

                    if (SubmitButton.Contains("Continuar"))
                    {
                        Session["funcionario"] = model.Funcionario;
                        //return RedirectToAction("RegistrarFormacionCandidato", "Vacantes", new { cedula = model.Funcionario.Cedula });
                        return JavaScript("window.top.location.href ='" + Url.Action("RegistrarFormacionCandidato", new { cedula = model.Funcionario.Cedula }) + "';");
                    }
                    else
                    {
                        //return RedirectToAction("DetalleCandidato", "Vacantes");
                        return JavaScript("window.top.location.href ='" + Url.Action("DetalleCandidato") + "';");
                    }
                }
                else
                {
                    int aux = model.ProvinciaSeleccionada;
                    if (Session["aux"] != null && (int)Session["aux"] == model.ProvinciaSeleccionada)
                        model.ProvinciaSeleccionada = 0;

                    if (model.ProvinciaSeleccionada != 0)
                    {
                        var cantones = ((List<CBaseDTO>)Session["cantones"]).ToList().Where(item => ((CCantonDTO)item).Provincia.IdEntidad == model.ProvinciaSeleccionada);
                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        foreach (var item in cantones)
                        {
                            dic.Add(((CCantonDTO)item).IdEntidad, ((CCantonDTO)item).NomCanton);
                        }
                        model.Provincias = new SelectList((Dictionary<int, string>)Session["dicP"], "Key", "Value");
                        model.Cantones = new SelectList(dic, "Key", "Value");
                        model.Distritos = new SelectList(new Dictionary<int, string>(), "Key", "Value");
                        Session["dicC"] = dic;
                        Session["aux"] = model.ProvinciaSeleccionada;
                        return PartialView("_FormularioDireccionCandidato", model);
                    }
                    else if (model.CantonSeleccionado != 0)
                    {
                        var distritos = ((List<CBaseDTO>)Session["distritos"]).ToList().Where(item => ((CDistritoDTO)item).Canton.IdEntidad == model.CantonSeleccionado);
                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        foreach (var item in distritos)
                        {
                            dic.Add(((CDistritoDTO)item).IdEntidad, ((CDistritoDTO)item).NomDistrito);
                        }

                        model.Provincias = new SelectList((Dictionary<int, string>)Session["dicP"], "Key", "Value");
                        model.Cantones = new SelectList((Dictionary<int, string>)Session["dicC"], "Key", "Value");
                        model.Distritos = new SelectList(dic, "Key", "Value");
                        model.ProvinciaSeleccionada = aux;
                        return PartialView("_FormularioDireccionCandidato", model);
                    }
                    else
                    {
                        return PartialView("_FormularioDireccionCandidato", model);
                    }
                }
            }
            catch (Exception error)
            {
                return PartialView("_ErrorVacantes");
            }
            
        }

        //
        // GET: /Vacantes/RegistrarFormacionCandidato
        public ActionResult RegistrarFormacionCandidato(string cedula)
        {
            try
            {
                FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                if (Session["funcionario"] != null && Session["funcionario"].GetType() == typeof(CFuncionarioDTO))
                {
                    model.Funcionario = (CFuncionarioDTO)Session["funcionario"];
                }else
                {
                    var resultado = servicioFuncionario.BuscarFuncionarioBase(cedula);

                    if (resultado != null && resultado.GetType() == typeof(CFuncionarioDTO)) {
                        model.Funcionario = (CFuncionarioDTO)resultado;
                    }else
                    {
                        wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
                        var persona = servicioTSE.wsConsultaDatosPersona(0, cedula, true);
                        model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);
                    }                   
                }
                               
                model.CursoGrado = new CCursoGradoDTO();
                //model.Funcionario.Mensaje = "Guardar y agregar otro curso de grado";
                model.Error = new CErrorDTO();
                //model.Error.Mensaje = "Guardar y agregar curso de capacitación";

                var entidadesEducativas = servicioCarrera.ListarEntidadEducativa()
                .Select(Q => new SelectListItem
                {
                    Value = ((CEntidadEducativaDTO)Q).IdEntidad.ToString(),
                    Text = ((CEntidadEducativaDTO)Q).DescripcionEntidad
                });

                model.EntidadesEducativas = new SelectList(entidadesEducativas, "Value", "Text");

                Dictionary<int, string> grados = GenerarDiccionario(true);

                model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                model.TituloFieldSet = "Curso de Grado";
                return View(model);
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }

        }

        public ActionResult RegistrarExperienciaCandidato(string cedula)
        {
            try
            {
                FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                if (Session["funcionario"] != null && Session["funcionario"].GetType() == typeof(CFuncionarioDTO))
                {
                    model.Funcionario = (CFuncionarioDTO)Session["funcionario"];
                }
                else
                {
                    var resultado = servicioFuncionario.BuscarFuncionarioBase(cedula);

                    if (resultado != null && resultado.GetType() == typeof(CFuncionarioDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)resultado;
                    }
                    else
                    {
                        wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
                        var persona = servicioTSE.wsConsultaDatosPersona(0, cedula, true);
                        model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);
                    }
                }

                model.CursoGrado = new CCursoGradoDTO();
                //model.Funcionario.Mensaje = "Guardar y agregar otro curso de grado";
                model.Error = new CErrorDTO();
                //model.Error.Mensaje = "Guardar y agregar curso de capacitación";

                Dictionary<int, string> entidadesEducativas = new Dictionary<int, string>
                {
                    { 1, "Seleccione..." },
                    { 2, "Entidad pública" },
                    { 3, "Empresa privada" }
                };

                //var entidadesEducativas = servicioCarrera.ListarEntidadEducativa()
                //.Select(Q => new SelectListItem
                //{
                //    Value = ((CEntidadEducativaDTO)Q).IdEntidad.ToString(),
                //    Text = ((CEntidadEducativaDTO)Q).DescripcionEntidad
                //});

                model.EntidadesEducativas = new SelectList(entidadesEducativas, "Key", "Value");

                Dictionary<int, string> grados = GenerarDiccionario(true);

                model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                model.TituloFieldSet = "Curso de Grado";
                return View(model);
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }

        }

        public ActionResult RegistrarEntrevistaCandidato(string cedula)
        {
            try
            {
                FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                if (Session["funcionario"] != null && Session["funcionario"].GetType() == typeof(CFuncionarioDTO))
                {
                    model.Funcionario = (CFuncionarioDTO)Session["funcionario"];
                }
                else
                {
                    var resultado = servicioFuncionario.BuscarFuncionarioBase(cedula);

                    if (resultado != null && resultado.GetType() == typeof(CFuncionarioDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)resultado;
                    }
                    else
                    {
                        wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
                        var persona = servicioTSE.wsConsultaDatosPersona(0, cedula, true);
                        model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);
                    }
                }

                model.CursoGrado = new CCursoGradoDTO();
                //model.Funcionario.Mensaje = "Guardar y agregar otro curso de grado";
                model.Error = new CErrorDTO();
                //model.Error.Mensaje = "Guardar y agregar curso de capacitación";

                Dictionary<int, string> entidadesEducativas = new Dictionary<int, string>
                {
                    { 1, "Seleccione..." },
                    { 2, "Entidad pública" },
                    { 3, "Empresa privada" }
                };

                //var entidadesEducativas = servicioCarrera.ListarEntidadEducativa()
                //.Select(Q => new SelectListItem
                //{
                //    Value = ((CEntidadEducativaDTO)Q).IdEntidad.ToString(),
                //    Text = ((CEntidadEducativaDTO)Q).DescripcionEntidad
                //});

                model.EntidadesEducativas = new SelectList(entidadesEducativas, "Key", "Value");

                Dictionary<int, string> grados = GenerarDiccionario(true);

                model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                model.TituloFieldSet = "Curso de Grado";
                return View(model);
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }

        }

        //
        // POST: /Vacantes/RegistrarFormacionCandidato
        [HttpPost]
        public ActionResult RegistrarFormacionCandidato(FuncionarioCarreraVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton != null) {
                    if (SubmitButton.Contains("grado"))
                    {                      
                        if (String.IsNullOrEmpty(model.CursoGrado.CursoGrado) || model.EntidadEducativaSeleccionada == 0 
                            || model.CursoGrado.FechaEmision == null || model.GradoAcademicoSeleccionado == 0)
                        {
                            ModelState.AddModelError("Guardar", "Es necesario completar todos los campos.");

                            throw new Exception("Guardar");
                        }else
                        {                            
                            var resultado = servicioCarrera.GuardarFormacionAcademica(model.CursoGrado,model.Funcionario);
                            if(resultado != null)
                            {
                                if (resultado.ToList().FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    return PartialView("_Success");
                                }
                                else
                                {
                                    ModelState.AddModelError("Guardar", ((CErrorDTO)resultado.ToList().FirstOrDefault()).MensajeError);
                                    throw new Exception("Guardar");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Guardar", "Ha ocurrido un error a la hora de guardar los datos de Formación Académica");
                                throw new Exception("Guardar");
                            }

                        }

                    }
                    else
                    {
                        if (SubmitButton.Contains("capacitación"))
                        {
                            if (String.IsNullOrEmpty(model.CursoCapacitacion.DescripcionCapacitacion) || model.EntidadEducativaSeleccionada == 0
                                || model.CursoCapacitacion.FechaInicio == null || model.CursoCapacitacion.FechaFinal == null)
                            {
                                throw new Exception("Es necesario completar todos los campos.");
                            }else
                            {
                                var resultado = servicioCarrera.GuardarFormacionAcademica(model.CursoCapacitacion, model.Funcionario);
                                if (resultado.ToList().FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    return PartialView("_Success");
                                }
                                else
                                {
                                    ModelState.AddModelError("Guardar", "Ha ocurrido un error a la hora de guardar los datos de Formación Académica");
                                    throw new Exception("Guardar");
                                }
                            }

                        }
                        else
                        {

                            //return RedirectToAction("DetalleCandidato", "Vacantes");
                            return JavaScript("window.top.location.href ='" + Url.Action("DetalleCandidato") + "';");
                        }
                    }
                }
                else {
                    throw new Exception("Ha ocurrido un error con el valor del SubmitButton");
                }
            }
            catch (Exception error)
            {
                return PartialView("_ErrorVacantes");
            }
        }

        private Dictionary<int, string> GenerarDiccionario(bool oficial)
        {
            if (oficial)
            {
                return new Dictionary<int, string>
                {
                    {1, "Diplomado"},
                    {2, "Bachiller"},
                    {3, "Licenciatura"},
                    {4, "Maestría"},
                    {5, "Doctorado"},
                    {6, "Curso de capacitación"}
                };
            }
            else
            {
                return new Dictionary<int, string>
                {
                    {1, "Bachiller"},
                    {2, "Licenciatura"},
                    {3, "Licenciatura adicional"},
                    {4, "Maestría"},
                    {5, "Maestría Adicional"},
                    {6, "Doctorado"},
                    {7, "Doctorado Adicional"},
                    {8, "Especialidad con base a Bachillerato"},
                    {9, "Especialidad con base a Licenciatura"},
                    {10, "Especialidad adicional"},
                };
            }
        }

        //
        // GET: /Vacantes/RegistrarDetalleContratacion
        public ActionResult RegistrarDetalleContratacion(string cedula)
        {
            wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
            var persona = servicioTSE.wsConsultaDatosPersona(0, "0207590466", true);
            FuncionarioDetalleContratacionVM model = new FuncionarioDetalleContratacionVM();
            model.Funcionario = ConvertirPersonaTSEAFuncionario(persona);
            //model.Funcionario = new CFuncionarioDTO { Cedula = "11111111", Nombre = "Ryan", PrimerApellido = "aaaaa", SegundoApellido = "bbbbb", Sexo = GeneroEnum.Masculino, FechaNacimiento = DateTime.Now };
            return View(model);
        }

        //
        // POST: /Vacantes/RegistrarDetalleContratacion
        [HttpPost]
        public ActionResult RegistrarDetalleContratacion(FuncionarioDetalleContratacionVM model)
        {
            return View();
        }

        // GET: /Vacantes/RegistrarPedimentoPuesto
        public ActionResult RegistrarPedimentoPuesto(string codpuesto)
        {
            try
            {
                return View();
                
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Vacantes) });

            }
        }

        //
        // POST: /Vacantes/RegistrarPedimentoPuesto
        [HttpPost]
        public ActionResult RegistrarPedimentoPuesto(PedimentoPuestoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (!String.IsNullOrEmpty(model.Puesto.CodPuesto))
                    {
                        var puesto = servicioPuesto.DescargarPuestoCompleto(model.Puesto.CodPuesto);

                        if (puesto != null)
                        {
                            foreach (var item in puesto.ToList())
                            {
                                if (item.GetType() == typeof(CErrorDTO))
                                {
                                    throw new Exception(((CErrorDTO)item).MensajeError);
                                }
                            }

                            model.Puesto = (CPuestoDTO)puesto[0];
                            model.DetallePuesto = (CDetallePuestoDTO)puesto[1];
                            model.UbicacionAdministrativa = (CUbicacionAdministrativaDTO)puesto[3];
                            if (puesto.Count() > 4)
                            {
                                model.Funcionario = ((CFuncionarioDTO)puesto.ElementAt(4));
                            }
                            else
                            {
                                model.Funcionario = new CFuncionarioDTO { Mensaje = "Este puesto no está siendo ocupado por ningún funcionario al momento de esta consulta." };
                            }
                            return PartialView("_DetalleRegistroPedimento", model);
                        }
                        else
                        {
                            throw new Exception("Ha ocurrido un error a la hora de buscar el puesto.");
                        }

                    }
                    else
                    {
                        throw new Exception("El codigo de puesto está vacío o es nulo.");

                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(model.PedimentoPuesto.NumeroPedimento) || model.PedimentoPuesto.FechaPedimento == null
                        || String.IsNullOrEmpty(model.PedimentoPuesto.ObservacionesPedimento))
                    {
                        ModelState.AddModelError("Guardar", "Debe completar todos los campos.");
                        throw new Exception("Guardar");
                    }
                    else
                    {
                        var resultado = servicioPuesto.GuardarPedimentoPuesto(model.Puesto, model.PedimentoPuesto);

                        if (resultado != null)
                        {
                            if (resultado.GetType() != typeof(CErrorDTO))
                            {
                                return JavaScript("window.location = '/Vacantes/DetallePedimento?idPedimento=" +
                                                    ((CRespuestaDTO)resultado).Contenido + "'");
                            }
                            else
                            {
                                ModelState.AddModelError("Guardar", ((CErrorDTO)resultado).MensajeError);
                                throw new Exception("Guardar");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Guardar", "Ha ocurrido un error a la hora de guardar el pedimento de puesto.");
                            throw new Exception("Guardar");
                        }

                    }
                }
            }
            catch (Exception error)
            {
                return PartialView("_ErrorVacantes");
            }

            //return RedirectToAction("Browse", "Puesto");
        }

        // GET: /Vacantes/RegistrarEstudioPuesto
        public ActionResult RegistrarEstudioPuesto(string codpuesto)
        {
            EstudioPuestoVM model = new EstudioPuestoVM();
            model.Puesto = new CPuestoDTO
            {
                CodPuesto = "026577",
                EstadoPuesto = new CEstadoPuestoDTO { DesEstadoPuesto = "Activo" }
            };
            model.DetallePuesto = new CDetallePuestoDTO
            {
                Clase = new CClaseDTO { DesClase = "Administración" },
                Especialidad = new CEspecialidadDTO { DesEspecialidad = "Administración de Recursos Humanos" },
            };
            model.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
            {
                Division = new CDivisionDTO { NomDivision = "División Administrativa" },
                DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = "Dirección de Gestión Institucional de Recursos Humanos" },
                Departamento = new CDepartamentoDTO { NomDepartamento = "Gestión de Empleo" },
                Seccion = new CSeccionDTO { NomSeccion = "Administración de Puestos Vacantes" },
                Presupuesto = new CPresupuestoDTO { CodigoPresupuesto = "32600 05 05" }
            };
            return View(model);
        }

        //
        // POST: /Vacantes/RegistrarEstudioPuesto
        [HttpPost]
        public ActionResult RegistrarEstudioPuesto(EstudioPuestoVM model)
        {
            return RedirectToAction("Browse", "Puesto");
        }

        // GET: /Vacantes/RegistrarPrestamoPuesto
        public ActionResult RegistrarPrestamoPuesto(string codpuesto)
        {
            PrestamoPuestoVM model = new PrestamoPuestoVM();
            model.Puesto = new CPuestoDTO
            {
                CodPuesto = "026577",
                EstadoPuesto = new CEstadoPuestoDTO { DesEstadoPuesto = "Activo" }
            };
            model.DetallePuesto = new CDetallePuestoDTO
            {
                Clase = new CClaseDTO { DesClase = "Administración" },
                Especialidad = new CEspecialidadDTO { DesEspecialidad = "Administración de Recursos Humanos" },
            };
            model.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
            {
                Division = new CDivisionDTO { NomDivision = "División Administrativa" },
                DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = "Dirección de Gestión Institucional de Recursos Humanos" },
                Departamento = new CDepartamentoDTO { NomDepartamento = "Gestión de Empleo" },
                Seccion = new CSeccionDTO { NomSeccion = "Administración de Puestos Vacantes" },
                Presupuesto = new CPresupuestoDTO { CodigoPresupuesto = "32600 05 05" }
            };

            Dictionary<int, string> gubernamental = GenerarEntidadGubernamental();
            model.EntidadesGubernamentales = new SelectList(gubernamental, "Key", "Value");

            Dictionary<int, string> adscrita = GenerarEntidadAdscrita();
            model.EntidadesAdscritas = new SelectList(adscrita, "Key", "Value");

            return View(model);
        }

        //
        // POST: /Vacantes/RegistrarPrestamoPuesto
        [HttpPost]
        public ActionResult RegistrarPrestamoPuesto(PrestamoPuestoVM model, string SubmitButton)
        {
            if (SubmitButton.StartsWith("Addendum"))
            {
                return RedirectToAction("RegistrarAddendumPrestamo", "Vacantes", new { codpuesto = "078667" });
            }
            else
            {
                if (SubmitButton.StartsWith("Rescisión"))
                {
                    return RedirectToAction("RegistrarRescisionPrestamo", "Vacantes", new { codpuesto = "078667" });
                }
                else
                {
                    return RedirectToAction("Browse", "Puesto");
                }
            }
        }

        private Dictionary<int, string> GenerarEntidadAdscrita()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var entidades = servicioFuncionario.ListarEntidadesAdscritas().ToList();

            foreach (var item in entidades)
            {
                dic.Add(((CEntidadAdscritaDTO)item).IdEntidad, ((CEntidadAdscritaDTO)item).EntidadAdscrita);
            }
            return dic;
        }

        private Dictionary<int, string> GenerarEntidadGubernamental()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            var entidades = servicioFuncionario.ListarEntidadesGubernamentales().ToList();

            foreach (var item in entidades)
            {
                dic.Add(((CEntidadGubernamentalDTO)item).IdEntidad, ((CEntidadGubernamentalDTO)item).EntidadGubernamental);
            }
            return dic;
        }

        // GET: /Vacantes/AsignarContenidoPresupuestario
        public ActionResult AsignarContenidoPresupuestario(string codpuesto)
        {
            ContenidoPresupuestarioPuestoVM model = new ContenidoPresupuestarioPuestoVM();
            model.Puesto = new CPuestoDTO
            {
                CodPuesto = "026577",
                EstadoPuesto = new CEstadoPuestoDTO { DesEstadoPuesto = "Activo" }
            };
            model.DetallePuesto = new CDetallePuestoDTO
            {
                Clase = new CClaseDTO { DesClase = "Administración" },
                Especialidad = new CEspecialidadDTO { DesEspecialidad = "Administración de Recursos Humanos" },
            };
            model.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
            {
                Division = new CDivisionDTO { NomDivision = "División Administrativa" },
                DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = "Dirección de Gestión Institucional de Recursos Humanos" },
                Departamento = new CDepartamentoDTO { NomDepartamento = "Gestión de Empleo" },
                Seccion = new CSeccionDTO { NomSeccion = "Administración de Puestos Vacantes" },
                Presupuesto = new CPresupuestoDTO { CodigoPresupuesto = "32600 05 05" }
            };
            return View(model);
        }

        //
        // POST: /Vacantes/AsignarContenidoPresupuestario
        [HttpPost]
        public ActionResult AsignarContenidoPresupuestario(ContenidoPresupuestarioPuestoVM model)
        {
            return RedirectToAction("Browse", "Puesto");
        }

        // GET: /Vacantes/RegistrarCaracteristicasPuesto
        public ActionResult RegistrarCaracteristicasPuesto(string codpuesto)
        {
            return View();
        }

        //
        // POST: /Vacantes/RegistrarCaracteristicasPuesto
        [HttpPost]
        public ActionResult RegistrarCaracteristicasPuesto(CaracteristicasPuestoVM model)
        {
            return View();
        }

        // GET: /Vacantes/RegistrarAddendumPrestamo
        public ActionResult RegistrarAddendumPrestamo(string codpuesto)
        {
            PrestamoPuestoVM model = new PrestamoPuestoVM();
            model.Puesto = new CPuestoDTO
            {
                CodPuesto = "026577",
                EstadoPuesto = new CEstadoPuestoDTO { DesEstadoPuesto = "Activo" }
            };
            model.DetallePuesto = new CDetallePuestoDTO
            {
                Clase = new CClaseDTO { DesClase = "Administración" },
                Especialidad = new CEspecialidadDTO { DesEspecialidad = "Administración de Recursos Humanos" },
            };
            model.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
            {
                Division = new CDivisionDTO { NomDivision = "División Administrativa" },
                DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = "Dirección de Gestión Institucional de Recursos Humanos" },
                Departamento = new CDepartamentoDTO { NomDepartamento = "Gestión de Empleo" },
                Seccion = new CSeccionDTO { NomSeccion = "Administración de Puestos Vacantes" },
                Presupuesto = new CPresupuestoDTO { CodigoPresupuesto = "32600 05 05" }
            };

            Dictionary<int, string> gubernamental = GenerarEntidadGubernamental();
            model.EntidadesGubernamentales = new SelectList(gubernamental, "Key", "Value");

            Dictionary<int, string> adscrita = GenerarEntidadAdscrita();
            model.EntidadesAdscritas = new SelectList(adscrita, "Key", "Value");

            return View(model);
        }

        //
        // POST: /Vacantes/RegistrarAddendumPrestamo
        [HttpPost]
        public ActionResult RegistrarAddendumPrestamo(PrestamoPuestoVM model)
        {
            return RedirectToAction("Browse", "Puesto");
        }

        // GET: /Vacantes/RegistrarRescisionPrestamo
        public ActionResult RegistrarRescisionPrestamo(string codpuesto)
        {
            PrestamoPuestoVM model = new PrestamoPuestoVM();
            model.Puesto = new CPuestoDTO
            {
                CodPuesto = "026577",
                EstadoPuesto = new CEstadoPuestoDTO { DesEstadoPuesto = "Activo" }
            };
            model.DetallePuesto = new CDetallePuestoDTO
            {
                Clase = new CClaseDTO { DesClase = "Administración" },
                Especialidad = new CEspecialidadDTO { DesEspecialidad = "Administración de Recursos Humanos" },
            };
            model.UbicacionAdministrativa = new CUbicacionAdministrativaDTO
            {
                Division = new CDivisionDTO { NomDivision = "División Administrativa" },
                DireccionGeneral = new CDireccionGeneralDTO { NomDireccion = "Dirección de Gestión Institucional de Recursos Humanos" },
                Departamento = new CDepartamentoDTO { NomDepartamento = "Gestión de Empleo" },
                Seccion = new CSeccionDTO { NomSeccion = "Administración de Puestos Vacantes" },
                Presupuesto = new CPresupuestoDTO { CodigoPresupuesto = "32600 05 05" }
            };

            Dictionary<int, string> gubernamental = GenerarEntidadGubernamental();
            model.EntidadesGubernamentales = new SelectList(gubernamental, "Key", "Value");

            Dictionary<int, string> adscrita = GenerarEntidadAdscrita();
            model.EntidadesAdscritas = new SelectList(adscrita, "Key", "Value");
            return View(model);
        }

        //
        // POST: /Vacantes/RegistrarRescisionPrestamo
        [HttpPost]
        public ActionResult RegistrarRescisionPrestamo(PrestamoPuestoVM model)
        {
            return RedirectToAction("Browse", "Puesto");
        }

        private CFuncionarioDTO ConvertirPersonaTSEAFuncionario(Persona persona)
        {
            return new CFuncionarioDTO
            {
                Cedula = persona.Identificacion,
                Nombre = persona.Nombre,
                PrimerApellido = persona.Apellido1,
                SegundoApellido = persona.Apellido2,
                FechaNacimiento = Convert.ToDateTime(persona.Fecha_Nacimiento),
                Sexo = persona.Genero == "FEMENINO" ? GeneroEnum.Femenino : GeneroEnum.Masculino
            };
        }


        private string ConvertirCedula(string cedula)
        {
            if (cedula.ElementAt(1) == '0') {
                cedula = cedula.Remove(1,1);
                cedula = cedula.Insert(0,"0");
                cedula = cedula.Insert(1, "0");
            }
            else
            {
                cedula = cedula.Insert(0, "0");
            }
            return cedula;

        }

        #region PEDIMENTO PUESTO

        public ActionResult SearchPedimento()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchPedimento(PedimentoPuestoVM model)
        {
            try
            {
                List<DateTime> fechas = new List<DateTime>();
                List<PedimentoPuestoVM> modelSearch = new List<PedimentoPuestoVM>();
                ModelState.Clear();
                if ((model.Puesto.CodPuesto != null && model.Puesto.CodPuesto.TrimEnd().Equals("")) 
                    && (model.PedimentoPuesto.NumeroPedimento != null && model.PedimentoPuesto.NumeroPedimento.TrimEnd().Equals(""))
                    && (model.FechaEmisionDesde.Year == 1 && model.FechaEmisionHasta.Year == 1))
                {
                    ModelState.AddModelError("Buscar", "Debe seleccionar al menos un parámetro de búsqueda para proceder");
                    throw new Exception("Buscar");
                }
                else
                {
                    if (model.FechaEmisionDesde.Year > 1)
                    {
                        if (model.FechaEmisionHasta.Year == 1)
                        {
                            ModelState.AddModelError("Buscar", "Debe seleccionar ambas fechas para proceder con la búsqueda por este rango");
                            throw new Exception("Buscar");
                        }
                    }
                    if (model.FechaEmisionHasta.Year > 1)
                    {
                        if (model.FechaEmisionDesde.Year == 1)
                        {
                            ModelState.AddModelError("Buscar", "Debe seleccionar ambas fechas para proceder con la búsqueda por este rango");
                            throw new Exception("Buscar");
                        }
                    }
                    if (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1)
                    {
                        fechas.Add(model.FechaEmisionDesde);
                        fechas.Add(model.FechaEmisionHasta);
                    }
                }

                var resultado = servicioPuesto.BuscarPedimentos(model.Puesto, model.PedimentoPuesto, fechas.ToArray()).ToList();

                if (resultado.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in resultado)
                    {
                        PedimentoPuestoVM temp = new PedimentoPuestoVM();
                        temp.PedimentoPuesto = (CPedimentoPuestoDTO)item.ElementAt(0);
                        temp.Puesto = (CPuestoDTO)item.ElementAt(1);
                        temp.DetallePuesto = (CDetallePuestoDTO)item.ElementAt(2);
                        modelSearch.Add(temp);
                    }
                    return PartialView("_ResultsSearchPedimento", modelSearch);
                }
                else
                {
                    ModelState.AddModelError("Buscar", ((CErrorDTO)resultado.ElementAt(0).ElementAt(0)).MensajeError);
                    throw new Exception("Buscar");
                }
            }
            catch (Exception)
            {
                return PartialView("_ErrorVacantes");
            }
        }

        public ActionResult DetallePedimento(int idPedimento)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();

            var resultado = servicioPuesto.BuscarPedimentoCodigo(idPedimento);

            if (resultado.ElementAt(0).GetType() != typeof(CErrorDTO))
            {
                model.Puesto = (CPuestoDTO)resultado.ElementAt(0);
                model.DetallePuesto = (CDetallePuestoDTO)resultado.ElementAt(1);
                model.PedimentoPuesto = (CPedimentoPuestoDTO)resultado.ElementAt(2);
            }

            return View(model);
        }

        #endregion
    }
}
