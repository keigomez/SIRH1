using System;
using System.Collections.Generic;
using System.Linq;
using SIRH.DTO;
using System.IO;
using System.Web;
using System.Web.Mvc;
//using SIRH.Web.RegistroIncapacidadLocal;
//using SIRH.Web.FuncionarioLocal;
//using SIRH.Web.AccionPersonalLocal;
using SIRH.Web.RegistroIncapacidadService;
using SIRH.Web.FuncionarioService;
using SIRH.Web.AccionPersonalService;
using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using SIRH.Web.Reports.RegistroIncapacidad;
using SIRH.Web.Helpers;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.UserValidation;

namespace SIRH.Web.Controllers
{
    public class RegistroIncapacidadController : Controller
    {
        //
        // GET: /RegistroIncapacidad/

        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CRegistroIncapacidadServiceClient servicioRegistro = new CRegistroIncapacidadServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();

        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        public ActionResult Index()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Incapacidades)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
            }
            else
            {
                return View();
            }
        }

        // GET: /RegistroIncapacidad/Details/1
        public ActionResult Details(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Incapacidades)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Incapacidades)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Incapacidades, Convert.ToInt32(ENivelesIncapacidades.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Incapacidades, Convert.ToInt32(ENivelesIncapacidades.Operativo))] != null)
                {
                    FormularioRegistroIncapacidadVM model = new FormularioRegistroIncapacidadVM();

                    var datos = servicioRegistro.ObtenerRegistroIncapacidad(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Incapacidad = (CRegistroIncapacidadDTO)datos.ElementAt(0);
                        model.TipoIncapacidad = (CTipoIncapacidadDTO)datos.ElementAt(1);
                        model.EntidadMedica = (CEntidadMedicaDTO)datos.ElementAt(2);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                        model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
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
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
                }
            }          
        }


        // GET: /RegistroIncapacidad/Create
        public ActionResult Create()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Incapacidades)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Incapacidades)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Incapacidades, Convert.ToInt32(ENivelesIncapacidades.Operativo))] != null)
                {
                    List<string> entidades = new List<string>();
                    entidades.Add(typeof(CRegistroIncapacidadDTO).Name);

                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades),
                                           Convert.ToInt32(EAccionesBitacora.Login), Convert.ToInt32(EModulosHelper.Incapacidades),
                                           CAccesoWeb.ListarEntidades(entidades.ToArray()));

                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
                }
            }
        }


        // POST: /RegistroIncapacidad/Create
        [HttpPost]
        public ActionResult Create(FormularioRegistroIncapacidadVM model, string SubmitButton)
        {
            var idEntidad = 0;
            var idTipo = 0;

            try
            {

                ModelState.Clear();

                var tiposIncapacidades = servicioRegistro.ListarTipoIncapacidad()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CTipoIncapacidadDTO)Q).IdEntidad.ToString(),
                            Text = ((CTipoIncapacidadDTO)Q).DescripcionTipoIncapacidad,
                        });

                var entidadesMedicas = servicioRegistro.ListarEntidadMedica()
                            .Select(Q => new SelectListItem
                            {
                                Value = ((CEntidadMedicaDTO)Q).IdEntidad.ToString(),
                                Text = ((CEntidadMedicaDTO)Q).DescripcionEntidadMedica
                            });

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
                            
                            model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");
                            model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");

                            model.Incapacidad.FecRige = DateTime.Today;
                            model.Incapacidad.FecVence = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                          
                            var respuesta = servicioFuncionario.BuscarFuncionarioSalario(model.Funcionario.Cedula);
                            model.MontoSalarioBruto = ((CSalarioDTO)respuesta.ElementAt(1)).MtoTotal;

                            return PartialView("_FormularioIncapacidad", model);
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
                        model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");
                        model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");

                        throw new Exception("Busqueda");
                    }
                }
               else
                {
                    if (SubmitButton == "Buscar Incapacidad")
                    {
                        var numDifDias = model.Incapacidad.FecVence.Subtract(model.Incapacidad.FecRige).TotalDays + 1;
                        var nombreModelo = "";

                        CBaseDTO[] datos = new CBaseDTO[0];

                        switch (model.TipoSeleccionado)
                        {
                            case 1: // Maternidad.  El máximo son 123 días
                                if(numDifDias > 123)
                                {
                                    ModelState.AddModelError("BuscarIncapacidad", "El máximo de días permitidos son 123 días");
                                    throw new Exception("Error");
                                }
                                break;
                            case 2:
                            case 3:
                                if(model.Incapacidad.CodNumeroCaso != null)
                                    datos = servicioRegistro.ObtenerRegistroIncapacidadCodigo(model.Incapacidad.CodNumeroCaso);
                                else
                                {
                                    ModelState.AddModelError("BuscarIncapacidad", "Debe digitar el Número de Caso");
                                    throw new Exception("Error");
                                }  
                                break;

                            case 4:
                            case 5:
                            case 6:
                                datos = servicioRegistro.ObtenerRegistroIncapacidadProrroga(model.Funcionario.Cedula, model.EntidadMedica.IdEntidad, model.TipoSeleccionado, model.Incapacidad.FecRige);
                                break;

                            default:
                                break;
                        }             

                        if (datos.Count() > 1)
                        {
                            var fechaRigeProrroga = model.Incapacidad.FecRige;
                            var fechaRigeFecVence = model.Incapacidad.FecVence;

                            model.Incapacidad = (CRegistroIncapacidadDTO)datos.ElementAt(0);
                            model.TipoIncapacidad = (CTipoIncapacidadDTO)datos.ElementAt(1);
                            model.EntidadMedica = (CEntidadMedicaDTO)datos.ElementAt(2);
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                            model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
                            var dia = model.Incapacidad.Detalles.Max(D => D.FecRige).ToList();
                            //model.Incapacidad.FecRige = Convert.ToDateTime(model.Incapacidad.Detalles.Max(D => D.FecRige)).AddDays(1);
                            //model.Incapacidad.FecVence = Convert.ToDateTime(model.Incapacidad.Detalles.Max(D => D.FecRige)).AddDays(1);
                            model.Incapacidad.FecRige = fechaRigeProrroga;
                            model.Incapacidad.FecVence = fechaRigeFecVence;

                            //var respuesta = servicioRegistro.ObtenerSalarioBruto(model.Funcionario.Cedula);
                            //model.MontoSalarioBruto = Convert.ToDecimal(((CRespuestaDTO)respuesta).Contenido);
                            var respuesta = servicioFuncionario.BuscarFuncionarioSalario(model.Funcionario.Cedula);
                            model.MontoSalarioBruto = ((CSalarioDTO)respuesta.ElementAt(1)).MtoTotal;

                            model.IndProrroga = 1;
                            model.Incapacidad.NumDiasOrigen = model.Incapacidad.Detalles.Count;
                            model.Incapacidad.NumDiasTotal = numDifDias;

                            //return PartialView("_FormularioProrroga", model);
                            nombreModelo = "_FormularioProrroga";
                        }
                        else
                        {
                            model.IndProrroga = 0;
                            model.Incapacidad.NumDiasOrigen = 0;
                            model.Incapacidad.NumDiasTotal = numDifDias;
                            //model.Incapacidad.FecRige =  DateTime.Today;
                            //model.Incapacidad.FecVence = DateTime.Today;
                            //model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");
                            //model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");
                            model.Incapacidad.CodIncapacidad = model.Incapacidad.CodNumeroCaso;
                            model.Incapacidad.ObsIncapacidad = "";
                            model.Incapacidad.Detalles= new List<CDetalleIncapacidadDTO>();
                            
                            var respuesta = servicioFuncionario.BuscarFuncionarioSalario(model.Funcionario.Cedula);
                            model.MontoSalarioBruto = ((CSalarioDTO)respuesta.ElementAt(1)).MtoTotal;

                            //return PartialView("_FormularioDetalle", model);
                            nombreModelo = "_FormularioDetalle";
                        }

                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        // Verificar que no exista una incapacidad para esas fechas
                        var datoInc = servicioRegistro.VerificarFechasIncapacidad(model.Funcionario, model.Incapacidad);

                        if (datoInc.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)datoInc).Codigo > 0)
                                return PartialView(nombreModelo, model);
                            else
                            {
                                ModelState.AddModelError("BuscarIncapacidad", ((CRespuestaDTO)datoInc).Contenido.ToString());
                                throw new Exception("Error");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("BuscarIncapacidad", ((CErrorDTO)datoInc).MensajeError);
                            throw new Exception("Error");
                        }
                    }
                    else
                    {     
                        if (ModelState.IsValid == true)
                        {
                            model.Incapacidad.MtoSalario = model.MontoSalarioBruto;
                            model.Incapacidad.IndProrroga = model.IndProrroga;

                            if (model.Incapacidad.CodIncapacidad == null || model.Incapacidad.CodIncapacidad == "")
                                model.Incapacidad.CodIncapacidad = model.Incapacidad.CodNumeroCaso;

                            idTipo = model.TipoSeleccionado;
                            idEntidad = model.EntidadSeleccionada;

                            CEntidadMedicaDTO entidad = new CEntidadMedicaDTO
                            {
                                IdEntidad = idEntidad
                            };

                            CTipoIncapacidadDTO tipo = new CTipoIncapacidadDTO
                            {
                                IdEntidad = idTipo,
                                EntidadMedica = entidad
                            };

                            model.Funcionario.Sexo = GeneroEnum.Indefinido;

                            var respuesta = servicioRegistro.GuardarRegistroIncapacidad(model.Funcionario, tipo, model.Incapacidad);

                            if (respuesta.GetType() != typeof(CErrorDTO))
                            {
                                if (((CRespuestaDTO)respuesta).Codigo > 0)
                                {
                                    //CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                                    //{
                                    //    IdEntidad = 1 // Registrado
                                    //};

                                    //CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                                    //{
                                    //    IdEntidad = Convert.ToInt32(ETipoAccionesHelper.Incapacidad) 
                                    //};

                                    //CAccionPersonalDTO accion = new CAccionPersonalDTO
                                    //{
                                    //    AnioRige = DateTime.Now.Year,
                                    //    CodigoModulo = Convert.ToInt32(EModulosHelper.Incapacidades),
                                    //    CodigoObjetoEntidad = Convert.ToInt16(((CRespuestaDTO)respuesta).Contenido),
                                    //    FecRige = model.Incapacidad.FecRige,
                                    //    FecVence = model.Incapacidad.FecVence,
                                    //    FecRigeIntegra = model.Incapacidad.FecRige,
                                    //    FecVenceIntegra = model.Incapacidad.FecVence,
                                    //    Observaciones = model.Incapacidad.ObsIncapacidad
                                    //};

                                    List<string> entidades = new List<string>();
                                    entidades.Add(typeof(CRegistroIncapacidadDTO).Name);

                                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades),
                                            Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                            CAccesoWeb.ListarEntidades(entidades.ToArray()));

                                    //var respuestaAP = servicioAccion.AgregarAccion(model.Funcionario,
                                    //                                                     estado,
                                    //                                                     tipoAP,
                                    //                                                     accion,
                                    //                                                     null);

                                    return RedirectToAction("Details", "RegistroIncapacidad", new { codigo = ((CRespuestaDTO)respuesta).Contenido, accion = "guardar" });

                                    //return JavaScript("window.location.href = '../RegistroIncapacidad/Details?codigo=" +
                                    //                     + "&accion=guardar" + "'");
                                }
                                else
                                {
                                    model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");
                                    model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");

                                    //ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                    throw new Exception(respuesta.Mensaje);
                                }
                            }
                            else
                            {
                                model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");
                                model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");

                                //ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");
                            model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");

                            //throw new Exception("Formulario");
                            return PartialView("_ErrorIncapacidad");
                        }
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Agregar", error.Message);
                return PartialView("_ErrorIncapacidad");
                //if (error.Message == "Busqueda")
                //{
                //    return PartialView("_ErrorIncapacidad");
                //}
                //else
                //{
                //    return PartialView("_ErrorIncapacidad");
                //    //return PartialView("_FormularioIncapacidad", model);
                //}
            }
        }


        public ActionResult Edit(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Incapacidades)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Incapacidades)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Incapacidades, Convert.ToInt32(ENivelesIncapacidades.Operativo))] != null)
                {
                    FormularioRegistroIncapacidadVM model = new FormularioRegistroIncapacidadVM();

                    var datos = servicioRegistro.ObtenerRegistroIncapacidad(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Incapacidad = (CRegistroIncapacidadDTO)datos.ElementAt(0);
                        model.TipoIncapacidad = (CTipoIncapacidadDTO)datos.ElementAt(1);
                        model.EntidadMedica = (CEntidadMedicaDTO)datos.ElementAt(2);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
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
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
                }
            }
        }


        [HttpPost]
        public ActionResult Edit(int codigo, FormularioRegistroIncapacidadVM model)
        {
            try
            {
                if (model.Incapacidad.ObsIncapacidad != null)
                {
                    model.Incapacidad.IdEntidad = codigo;
                    var respuesta = servicioRegistro.AnularRegistroIncapacidad(model.Incapacidad);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {

                        CAccionPersonalDTO accion = new CAccionPersonalDTO
                        {
                            CodigoModulo = Convert.ToInt32(EModulosHelper.Incapacidades),
                            CodigoObjetoEntidad = codigo,
                            Observaciones = model.Incapacidad.ObsIncapacidad,
                            TipoAccion = new CTipoAccionPersonalDTO
                            {
                                IdEntidad = Convert.ToInt32(ETipoAccionesHelper.Incapacidad)
                            }
                        };

                        var respuestaModulo = servicioAccion.AnularAccionModulo(accion);

                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), Convert.ToInt32(EAccionesBitacora.Editar), 0,
                                                CAccesoWeb.ListarEntidades(typeof(CRegistroIncapacidadDTO).Name));

                        return JavaScript("window.location = '/RegistroIncapacidad/Details?codigo=" + codigo + "&accion=modificar" + "'");

                        //return RedirectToAction("Details", new { codigo = codigo, accion = "modificar" });
                    }
                    else
                    {
                        ModelState.AddModelError("modificar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para anular esta incapacidad");
                    throw new Exception();
                }
            }
            catch
            {
                return View(model);
            }
        }


        public ActionResult Aprobar(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Incapacidades)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Incapacidades)]))
                {
                    FormularioRegistroIncapacidadVM model = new FormularioRegistroIncapacidadVM();

                    var datos = servicioRegistro.ObtenerRegistroIncapacidad(codigo);

                    if (datos.Count() > 1)
                    {
                        model.Incapacidad = (CRegistroIncapacidadDTO)datos.ElementAt(0);
                        model.TipoIncapacidad = (CTipoIncapacidadDTO)datos.ElementAt(1);
                        model.EntidadMedica = (CEntidadMedicaDTO)datos.ElementAt(2);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                        model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
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
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
                }
            }
        }


        [HttpPost]
        public ActionResult Aprobar(int codigo, FormularioRegistroIncapacidadVM model)
        {
            try
            {
                if (model.Incapacidad.ObsIncapacidad != null)
                {
                    model.Incapacidad.IdEntidad = codigo;
                    var respuesta = servicioRegistro.AprobarRegistroIncapacidad(model.Incapacidad);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        int tipoAccion = 0;

                        if (model.Incapacidad.IndProrroga == 0)
                            tipoAccion = Convert.ToInt32(ETipoAccionesHelper.Incapacidad);
                        else
                            tipoAccion = Convert.ToInt32(ETipoAccionesHelper.ProrrogaIncapacidad);

                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                            {
                                IdEntidad = 1 // Registrado
                            };

                        CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = tipoAccion
                        };

                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        CAccionPersonalDTO accion = new CAccionPersonalDTO
                        {
                            AnioRige = DateTime.Now.Year,
                            CodigoModulo = Convert.ToInt32(EModulosHelper.Incapacidades),
                            CodigoObjetoEntidad = codigo,
                            FecRige = model.Incapacidad.FecRige,
                            FecVence = model.Incapacidad.FecVence,
                            FecRigeIntegra = model.Incapacidad.FecRige,
                            FecVenceIntegra = model.Incapacidad.FecVence,
                            Observaciones = model.Incapacidad.ObsIncapacidad
                        };

                        List<string> entidades = new List<string>();
                        entidades.Add(typeof(CAccionPersonalDTO).Name);

                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades),
                                Convert.ToInt32(EAccionesBitacora.Editar), codigo,
                                CAccesoWeb.ListarEntidades(entidades.ToArray()));

                        var respuestaAP = servicioAccion.AgregarAccion(model.Funcionario,
                                                                             estado,
                                                                             tipoAP,
                                                                             accion,
                                                                             null);

                        //return RedirectToAction("Details", new { codigo = codigo, accion = "modificar" });
                        return JavaScript("window.location = '/RegistroIncapacidad/Details?codigo=" + codigo + "&accion=aprobar" + "'");
                    }
                    else
                    {
                        ModelState.AddModelError("modificar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para anular esta incapacidad");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }



        // GET: /RegistroIncapacidad/List
        public ActionResult List()
        {
            try
            {
                List<FormularioRegistroIncapacidadVM> model = new List<FormularioRegistroIncapacidadVM>();
                var respuesta = servicioRegistro.FuncionariosConIncapacidades();

                if (respuesta.First().First().GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in respuesta)
                    {
                        FormularioRegistroIncapacidadVM temp = new FormularioRegistroIncapacidadVM();
                        temp.Funcionario = (CFuncionarioDTO)item[0];
                        temp.Puesto = (CPuestoDTO)item[1];
                        temp.DetallePuesto = (CDetallePuestoDTO)item[2];
                        if (item[3].IdEntidad != -1)
                        {
                            temp.Incapacidad = (CRegistroIncapacidadDTO)item[3];
                        }
                        else
                        {
                            temp.Incapacidad = new CRegistroIncapacidadDTO { IdEntidad = -1 };
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
                return RedirectToAction("Index", "Error", new { errorType = "carga", modulo = "registroincapacidad" });
            }
        }


        // GET: /RegistroIncapacidad/Search
        public ActionResult Search()
        {
            BusquedaIncapacidadVM model = new BusquedaIncapacidadVM();

            var tiposIncapacidades = servicioRegistro.ListarTipoIncapacidad()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CTipoIncapacidadDTO)Q).IdEntidad.ToString(),
                            Text = ((CTipoIncapacidadDTO)Q).DescripcionTipoIncapacidad
                        });

            var entidadesMedicas = servicioRegistro.ListarEntidadMedica()
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CEntidadMedicaDTO)Q).IdEntidad.ToString(),
                            Text = ((CEntidadMedicaDTO)Q).DescripcionEntidadMedica
                        });

            model.Tipos = new SelectList(tiposIncapacidades, "Value", "Text");
            model.Entidades = new SelectList(entidadesMedicas, "Value", "Text");

            List<string> estadosPage = new List<string>();
            estadosPage.Add("Seleccionar Estado");
            estadosPage.Add("Activa");
            estadosPage.Add("Aprobada");
            estadosPage.Add("Anulada");
            model.Estados = new SelectList(estadosPage);

            return View(model);
        }


        [HttpPost]
        public ActionResult Search(BusquedaIncapacidadVM model)
        {
            try
            {
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                if (model.Funcionario.Cedula != null || model.RegistroIncapacidad.IdEntidad != null ||
                    (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1) ||
                    (model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1) ||
                    (model.FechaBitacoraHasta.Year > 1 && model.FechaBitacoraHasta.Year > 1) )
                {
                    List<DateTime> fechasEmision = new List<DateTime>();
                    List<DateTime> fechasVencimiento = new List<DateTime>();
                    List<DateTime> fechasBitacora = new List<DateTime>();

                    CEntidadMedicaDTO entidadM = new CEntidadMedicaDTO();

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

                    if (model.FechaBitacoraDesde.Year > 1 && model.FechaBitacoraHasta.Year > 1)
                    {
                        fechasBitacora.Add(model.FechaBitacoraDesde);
                        fechasBitacora.Add(model.FechaBitacoraHasta);
                    }

                    //  Tipo de Incapacidad
                    if (Convert.ToInt32(model.TipoSeleccionado) > 0)
                    {
                        CTipoIncapacidadDTO tipoI = new CTipoIncapacidadDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };
                        model.RegistroIncapacidad.TipoIncapacidad = tipoI;
                    }

                    //  Entidad Médica
                    if (Convert.ToInt32(model.EntidadSeleccionada) > 0)
                    {
                        entidadM = new CEntidadMedicaDTO
                        {
                            IdEntidad = Convert.ToInt32(model.EntidadSeleccionada)
                        };
                    }

                    CBitacoraUsuarioDTO entidadBitacora = new CBitacoraUsuarioDTO
                    {
                        CodigoAccion = 1, // Guardar
                        CodigoModulo = Convert.ToInt32(EModulosHelper.Incapacidades)
                    };


                    model.Incapacidades = new List<FormularioRegistroIncapacidadVM>();

                    var datos = servicioRegistro.BuscarRegistroIncapacidades(model.Funcionario,
                                                                            model.RegistroIncapacidad,
                                                                            entidadM,
                                                                            entidadBitacora,
                                                                            fechasEmision.ToArray(),
                                                                            fechasVencimiento.ToArray(),
                                                                            fechasBitacora.ToArray());
                    if(datos.Count() == 0)
                    {
                        return PartialView("_SearchResults", model.Incapacidades);
                    }

                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {                   
                        foreach (var item in datos)
                        {
                            FormularioRegistroIncapacidadVM temp = new FormularioRegistroIncapacidadVM();
                            temp.Incapacidad = (CRegistroIncapacidadDTO)item.ElementAt(0);
                            temp.TipoIncapacidad = (CTipoIncapacidadDTO)item.ElementAt(1);
                            temp.EntidadMedica = (CEntidadMedicaDTO)item.ElementAt(2);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(3);

                            model.Incapacidades.Add(temp);
                        }

                        return PartialView("_SearchResults", model.Incapacidades);
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
                    return PartialView("_ErrorIncapacidad");
                }
                else
                {
                    return PartialView("_ErrorIncapacidad");
                }
            }
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleIncapacidad(FormularioRegistroIncapacidadVM model)
        {
            
            List<IncapacidadRptData> modelo = new List<IncapacidadRptData>();
            for (int i = 0; i < model.Incapacidad.Detalles.Count(); i++)
            {
                modelo.Add(IncapacidadRptData.GenerarDatosReporte(model, i, String.Empty));
            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/RegistroIncapacidad"), "IncapacidadRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteIncapacidades(List<FormularioRegistroIncapacidadVM> model)
        {
            List<IncapacidadRptData> modelo = new List<IncapacidadRptData>();

            foreach (var item in model)
            {
                modelo.Add(IncapacidadRptData.GenerarDatosReporte(item, 0, String.Empty));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/RegistroIncapacidad"), "ReporteIncapacidadesRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }


        //[HttpPost]
        //public ActionResult GetPorcentajes(int? idEntidad, DateTime fecRige, DateTime fecVence)
        //{
        //    decimal porcentaje= 0;
        //    var timeSpan = fecVence - fecRige;
        //    int dias = timeSpan.Days + 1;
        //    try
        //    {
        //        switch (idEntidad)
        //        {
        //            case 1: // CCSS
        //                if (dias >= 1 && dias <= 3)
        //                    porcentaje = 80;

        //                if (dias >= 4 && dias <= 30)
        //                    porcentaje = 20;

        //                if (dias > 30)
        //                    porcentaje = 40;

        //                break;

        //            case 2: // INS
        //                if (dias >= 1 && dias <= 30)
        //                    porcentaje = 20;

        //                if (dias >= 31 && dias <= 45)
        //                    porcentaje = 40;

        //                if (dias >= 46 && dias <= 760)
        //                    porcentaje = 33;

        //                break;
        //        }

        //        return Json(new
        //        {
        //            success = true,
        //            porc = porcentaje
        //        }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception error)
        //    {
        //        return Json(new { success = false, porc = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        // GET: /RegistroIncapacidad/Prorroga
        public ActionResult Prorroga()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Incapacidades)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Incapacidades)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Incapacidades, Convert.ToInt32(ENivelesIncapacidades.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Incapacidades) });
                }
            }
        }


        // POST: /RegistroIncapacidad/Create
        [HttpPost]
        public ActionResult Prorroga(FormularioRegistroIncapacidadVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {

                        var datos = servicioRegistro.ObtenerRegistroIncapacidadCodigo(model.Incapacidad.CodIncapacidad);

                        if (datos.Count() > 1)
                        {
                            model.Incapacidad = (CRegistroIncapacidadDTO)datos.ElementAt(0);
                            model.TipoIncapacidad = (CTipoIncapacidadDTO)datos.ElementAt(1);
                            model.EntidadMedica = (CEntidadMedicaDTO)datos.ElementAt(2);
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                            model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);

                            model.Incapacidad.FecRige = Convert.ToDateTime(model.Incapacidad.Detalles.Max(D => D.FecRige));
                            model.Incapacidad.FecVence = Convert.ToDateTime(model.Incapacidad.Detalles.Max(D => D.FecRige));

                            var respuesta = servicioRegistro.ObtenerSalarioBruto(model.Funcionario.Cedula);
                            model.MontoSalarioBruto = Convert.ToDecimal(((CRespuestaDTO)respuesta).Contenido);
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.ElementAt(0);
                        }

                        return PartialView("_FormularioProrroga", model);
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    if (ModelState.IsValid == true)
                    {
                        model.Incapacidad.MtoSalario = model.MontoSalarioBruto;
                        model.Incapacidad.IndProrroga = 1;

                        CEntidadMedicaDTO entidad = new CEntidadMedicaDTO
                        {
                            IdEntidad = model.EntidadMedica.IdEntidad
                        };

                        CTipoIncapacidadDTO tipo = new CTipoIncapacidadDTO
                        {
                            IdEntidad = model.TipoIncapacidad.IdEntidad,
                            EntidadMedica = entidad
                        };

                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        var respuesta = servicioRegistro.GuardarRegistroIncapacidad(model.Funcionario, tipo, model.Incapacidad);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)respuesta).Codigo > 0)
                            {
                                CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                                {
                                    IdEntidad = 1 // Registrado
                                };

                                CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                                {
                                    IdEntidad = Convert.ToInt32(ETipoAccionesHelper.ProrrogaIncapacidad)
                                };

                                CAccionPersonalDTO accion = new CAccionPersonalDTO
                                {
                                    AnioRige = DateTime.Now.Year,
                                    CodigoModulo = Convert.ToInt32(EModulosHelper.Incapacidades),
                                    CodigoObjetoEntidad = Convert.ToInt16(((CRespuestaDTO)respuesta).Contenido),
                                    FecRige = model.Incapacidad.FecRige,
                                    FecVence = model.Incapacidad.FecVence,
                                    FecRigeIntegra = model.Incapacidad.FecRige,
                                    FecVenceIntegra = model.Incapacidad.FecVence,
                                    Observaciones = model.Incapacidad.ObsIncapacidad
                                };

                                List<string> entidades = new List<string>();
                                entidades.Add(typeof(CAccionPersonalDTO).Name);

                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Incapacidades),
                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                        CAccesoWeb.ListarEntidades(entidades.ToArray()));

                                //var respuestaAP = servicioAccion.AgregarAccion(model.Funcionario,
                                //                                                     estado,
                                //                                                     tipoAP,
                                //                                                     accion,
                                //                                                     null);

                                return JavaScript("window.location = '/RegistroIncapacidad/Details?codigo=" +
                                                    ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                            }
                            else
                            {
                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        //throw new Exception("Formulario");
                        return PartialView("_ErrorIncapacidad");
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
                    return PartialView("_ErrorIncapacidad");
                    //return PartialView("_FormularioIncapacidad", model);
                }
            }
        }



        [HttpPost]
        public ActionResult GetPorcentajes(int? idTipo, int? idEntidad, int diasOrigen, DateTime fecRige, DateTime fecVence, decimal montoSalario)
        {
            decimal porcentaje = 0;
            int maxDias = -1;
            int dias = 0;
            //bool prorroga = false;
            //diasOrigen += 1;

            //prorroga = diasOrigen == 1 ? false : true;

            List<CDetalleIncapacidadDTO> listadoDias = new List<CDetalleIncapacidadDTO>();
            try
            {

                var lista = Enumerable.Range(0, (fecVence - fecRige).Days + 1)
                                         .Select(d => fecRige.AddDays(d))
                                         .ToArray();

                foreach (DateTime fecha in lista)
                {
                    diasOrigen++;

                    switch (idEntidad)
                    {
                        case 1: // CCSS

                            switch (idTipo)
                            {
                                case 1: // // Maternidad
                                    porcentaje = 50;
                                    maxDias = 123;
                                    dias++;
                                    break;

                                case 4:  // Fase Terminal
                                case 5:  // Licencia
                                    porcentaje = 0;
                                    break;

                                default:
                                    if (diasOrigen >= 1 && diasOrigen <= 3)
                                        porcentaje = 80;

                                    if (diasOrigen >= 4 && diasOrigen <= 30)
                                        porcentaje = 20;

                                    if (diasOrigen > 30)
                                        porcentaje = 40;
                                    break;
                            }
                            break;

                        case 2: // INS

                            if (idTipo == 3) // SOA
                            {
                                porcentaje = 0;
                            }
                            else
                            {
                                if (diasOrigen >= 1 && diasOrigen <= 30)
                                    porcentaje = 20;

                                if (diasOrigen >= 31 && diasOrigen <= 45)
                                    porcentaje = 40;

                                if (diasOrigen >= 46 && diasOrigen <= 760)
                                    porcentaje = 33;
                            }

                            break;
                    }

                    listadoDias.Add(new CDetalleIncapacidadDTO
                    {
                        NumDia = diasOrigen, // prorroga == false ? diasOrigen : diasOrigen + 1,
                        FecRige = fecha.ToShortDateString(),
                        PorSubsidio = porcentaje,
                        MtoSubsidio = Math.Round(Convert.ToDecimal((montoSalario / 30) * (porcentaje / 100)),2),
                        PorRebaja = 100 - porcentaje,
                        MtoRebaja = Math.Round(Convert.ToDecimal((montoSalario / 30) * ((100 - porcentaje) / 100)),2)
                    });

                    if(dias == maxDias)
                        break;
                }

                return Json(new
                {
                    success = true,
                    porc = listadoDias
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception error)
            {
                return Json(new { success = false, porc = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetEntidad(int? idTipo)
        {
            try
            {
                if (idTipo != null)
                {
                    var datos = servicioRegistro.ObtenerTipoIncapacidad(Convert.ToInt16(idTipo));
                    var tipoInc = (CTipoIncapacidadDTO)datos.ElementAt(0);
                    return Json(new { success = true, idEntidad = tipoInc.EntidadMedica.IdEntidad }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, idEntidad = "0",  mensaje = "No existe el tipo" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, idEntidad = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}