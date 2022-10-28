using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ArchivoLocal;
//using SIRH.Web.FuncionarioLocal;
//using SIRH.Web.PerfilUsuarioLocal;
using SIRH.Web.FuncionarioService;
using SIRH.Web.PerfilUsuarioService;
//using SIRH.Web.ArchivoService;
using SIRH.DTO;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Helpers;
using System.Security.Principal;
using SIRH.Web.UserValidation;
using SIRH.Web.ServicioTSE;
using SIRH.Web.Reports.Boletas;

namespace SIRH.Web.Controllers
{
    public class ArchivoController : Controller
    {
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CArchivoServiceClient servicioArchivo = new CArchivoServiceClient();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        CAccesoWeb context = new CAccesoWeb();

        public ActionResult Index() {
            return View();
        }

        public ActionResult Details(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    FormularioBoletaPrestamoVM model = new FormularioBoletaPrestamoVM();

                    var temp = servicioArchivo.ObtenerBoleta(codigo); //trae Boleta, usuario  y funcionario DTO

                    model.BoletaPrestamo = (CBoletaPrestamoDTO)temp[0];
                    model.BoletaPrestamo.Usuario = (CUsuarioDTO)temp[1];
                    model.Funcionario = (CFuncionarioDTO)temp[2];
                    model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;

                    if (model.BoletaPrestamo.TipoUsuario == TipoUsuarioEnum.Interno)
                    {
                        //var busquedaFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                        //var busquedaSolicitante = servicioFuncionario.BuscarFuncionarioBase(model.BoletaPrestamo.CedulaSolicitante);
                        //var funcSolicitante = (CFuncionarioDTO)busquedaSolicitante;
                        var busquedaSolicitante = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.BoletaPrestamo.CedulaSolicitante);
                        var funcSolicitante = (CFuncionarioDTO)busquedaSolicitante[0];

                        //var puesto = (CPuestoDTO)busquedaFuncionario[1];
                        //var expediente = (CExpedienteFuncionarioDTO)temp[3];

                        model.BoletaPrestamo.NombreSolicitante = funcSolicitante.Nombre.TrimEnd();
                        model.BoletaPrestamo.ApellidoSolicitante = funcSolicitante.PrimerApellido.TrimEnd() + " " + funcSolicitante.SegundoApellido.TrimEnd();

                        //model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula.TrimEnd();
                        //model.BoletaPrestamo.NombreFuncionarioSolicitado = model.BoletaPrestamo.NombreFuncionarioSolicitado.TrimEnd();
                        //model.BoletaPrestamo.ApellidoFuncionarioSolicitado = model.Funcionario.PrimerApellido.TrimEnd() + " " + model.Funcionario.SegundoApellido.TrimEnd();

                        //model.BoletaPrestamo.DivisiónFuncionario = puesto.UbicacionAdministrativa.Division.NomDivision;
                        //model.BoletaPrestamo.DirecciónFuncionario = puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion;
                        //model.BoletaPrestamo.DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento;

                        //model.BoletaPrestamo.NumeroExpediente = expediente.NumeroExpediente.ToString();

                        //return View(model);
                    }
                    else {
                        wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
                        string cedulaFormateada = FuncionarioHelper.CedulaEmulacionATSE(model.BoletaPrestamo.CedulaSolicitante);
                        var persona = servicioTSE.wsConsultaDatosPersona(0, cedulaFormateada, true);

                        //var busquedaFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        //var puesto = (CPuestoDTO)busquedaFuncionario[1];
                        //var expediente = (CExpedienteFuncionarioDTO)temp[3];

                        model.BoletaPrestamo.NombreSolicitante = persona.Nombre;
                        model.BoletaPrestamo.ApellidoSolicitante = persona.Apellido1 + " " + persona.Apellido2;
                        //model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;
                        //model.BoletaPrestamo.ApellidoFuncionarioSolicitado = model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido;

                        //model.BoletaPrestamo.DivisiónFuncionario = puesto.UbicacionAdministrativa.Division.NomDivision;
                        //model.BoletaPrestamo.DirecciónFuncionario = puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion;
                        //model.BoletaPrestamo.DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento;

                        //model.BoletaPrestamo.NumeroExpediente = expediente.NumeroExpediente.ToString();

                        //return View(model);
                    }

                    var busquedaFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                    var puesto = (CPuestoDTO)busquedaFuncionario[1];
                    var expediente = (CExpedienteFuncionarioDTO)temp[3];

                    model.BoletaPrestamo.NombreFuncionarioSolicitado = model.BoletaPrestamo.NombreFuncionarioSolicitado.TrimEnd();
                    model.BoletaPrestamo.ApellidoFuncionarioSolicitado = model.Funcionario.PrimerApellido.TrimEnd() + " " + model.Funcionario.SegundoApellido.TrimEnd();

                    model.BoletaPrestamo.DivisiónFuncionario = puesto.UbicacionAdministrativa.Division.NomDivision;
                    model.BoletaPrestamo.DirecciónFuncionario = puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion;
                    model.BoletaPrestamo.DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento;

                    model.BoletaPrestamo.NumeroExpediente = expediente.NumeroExpediente.ToString();

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Cauciones) });
                }
            }
        }


        public ActionResult DetailsTrasladoArchivoCentral(int codigo) {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    BusquedaExpedienteVM model = new BusquedaExpedienteVM();

                    var temp = servicioArchivo.ObtenerExpedientePorNumeroExpediente(codigo);
                    model.Expediente = (CExpedienteFuncionarioDTO)temp.FirstOrDefault();

                        return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
                }
            }
        }

        public ActionResult DetailsCambioEstadoExpediente(int codigo) {
            BusquedaExpedienteVM model = new BusquedaExpedienteVM();

            var temp = servicioArchivo.ObtenerExpedientePorNumeroExpediente(codigo);
            model.Expediente = (CExpedienteFuncionarioDTO)temp.FirstOrDefault();

            return View(model);
        }


        public ActionResult CreateBoletaPrestamo() {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
                }
            }

        }

        public ActionResult CreateBusquedaPrestamo() {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
                }
            }
        }

        public ActionResult CreateBusquedaExpediente() {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
                }
            }
        }

        public ActionResult CreateFoleo() {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
                }
            }

        }

        public ActionResult CreateTrasladoArchivoCentral() {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Archivo), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Archivo)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Archivo)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Archivo, Convert.ToInt32(ENivelesArchivo.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Archivo) });
                }
            }
        }


        [HttpPost]
        public ActionResult CreateBoletaPrestamo(FormularioBoletaPrestamoVM model, string SubmitButton) {
            wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();

            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (model.UsuarioExterno)
                    { // Es un usuario externo.
                        string cedulaFormateada = FuncionarioHelper.CedulaEmulacionATSE(model.BoletaPrestamo.CedulaSolicitante);
                        var persona = servicioTSE.wsConsultaDatosPersona(0, cedulaFormateada, true);

                            var busquedaFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.BoletaPrestamo.CedulaFuncionario);

                            if (busquedaFuncionario[0].GetType() != typeof(CErrorDTO))
                            {
                                var busquedaExpediente = servicioArchivo.ObtenerExpedientePorCedula(model.BoletaPrestamo.CedulaFuncionario);
                                if (busquedaExpediente != null)
                                {                                    
                                    var expediente = (CExpedienteFuncionarioDTO)busquedaExpediente[0];
                                    if (expediente.Estado != EstadoEnum.Prestado)
                                    { //  Si no existe ya un préstamo con el expediente del funcionario consultado.

                                        if (expediente.Estado != EstadoEnum.TrasladadoArchivoCentral)
                                        {
                                            var puesto = (CPuestoDTO)busquedaFuncionario[1];

                                            model.BoletaPrestamo.NombreSolicitante = persona.Nombre;
                                            model.BoletaPrestamo.ApellidoSolicitante = persona.Apellido1 + "" + persona.Apellido2;
                                            model.Funcionario = (CFuncionarioDTO)busquedaFuncionario[0];
                                            model.BoletaPrestamo.NombreFuncionarioSolicitado = model.Funcionario.Nombre;
                                            model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;
                                            model.BoletaPrestamo.ApellidoFuncionarioSolicitado = model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido;
                                            model.BoletaPrestamo.DivisiónFuncionario = puesto.UbicacionAdministrativa.Division.NomDivision;
                                            model.BoletaPrestamo.DirecciónFuncionario = puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion;
                                            model.BoletaPrestamo.DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento;
                                            model.BoletaPrestamo.NumeroExpediente = expediente.NumeroExpediente.ToString();
                                            return PartialView("_FormularioBoleta", model);
                                        }
                                        else {
                                            ModelState.AddModelError("BuscarUsuario", "El funcionario consultado posee el expediente en archivo central, imposible realizar préstamo.");
                                            throw new Exception("BuscarUsuario");
                                        }
                                    }
                                    else {
                                        ModelState.AddModelError("BuscarUsuario", "El expediente del funcionario solicitado ya ha sido prestado.");
                                        throw new Exception("BuscarUsuario");
                                    }
                                }
                                else
                                { // Error
                                    CErrorDTO error = (CErrorDTO)busquedaExpediente[0];
                                    ModelState.AddModelError("BuscarUsuario", error.MensajeError);
                                    throw new Exception("BuscarUsuario");
                                }
                            }
                            else
                            { // Error
                                ModelState.AddModelError("BuscarUsuario", ((CErrorDTO)busquedaFuncionario[0]).MensajeError);
                                throw new Exception("BuscarUsuario");
                            }
                    }
                    else
                    { // Es un usuario Interno

                        var busquedaFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.BoletaPrestamo.CedulaFuncionario);
                        if (busquedaFuncionario[0].GetType() != typeof(CErrorDTO))
                        { // si el funcinario es válido.
                            var busquedaSolicitante = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.BoletaPrestamo.CedulaSolicitante);
                            if (busquedaSolicitante.GetType() != typeof(CErrorDTO))
                            { // si el funcionario solicitante existe.

                                var busquedaExpediente = servicioArchivo.ObtenerExpedientePorCedula(model.BoletaPrestamo.CedulaFuncionario);
                                if (busquedaExpediente[0].GetType() != typeof(CErrorDTO))
                                { // si el expediente existe.
                                    var puesto = (CPuestoDTO)busquedaFuncionario[1];
                                    var expediente = (CExpedienteFuncionarioDTO)busquedaExpediente[0];
                                    var funcSolicitante = (CFuncionarioDTO)busquedaSolicitante[0];
                                    var puestoSolicitante = (CPuestoDTO)busquedaSolicitante[1];
                                    if (expediente.Estado == EstadoEnum.NoPrestado)
                                    {
                                        //  Si no existe ya un prestamo conel expediente del funcionario consultado.
                                        model.Mensaje = null;
                                        model.BoletaPrestamo.NombreSolicitante = funcSolicitante.Nombre;
                                        model.BoletaPrestamo.ApellidoSolicitante = funcSolicitante.PrimerApellido + " " + funcSolicitante.SegundoApellido;

                                        model.Funcionario = (CFuncionarioDTO)busquedaFuncionario[0];

                                        model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;
                                        model.BoletaPrestamo.NombreFuncionarioSolicitado = model.Funcionario.Nombre;
                                        model.BoletaPrestamo.ApellidoFuncionarioSolicitado = model.Funcionario.PrimerApellido.TrimEnd() + "  " + model.Funcionario.SegundoApellido.TrimEnd();

                                        model.BoletaPrestamo.DivisiónFuncionario = puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd();
                                        model.BoletaPrestamo.DirecciónFuncionario = puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd();
                                        model.BoletaPrestamo.DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento != null ? puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "";

                                        model.BoletaPrestamo.NumeroExpediente = expediente.NumeroExpediente.ToString();

                                        model.BoletaPrestamo.LugarDeProcedencia = puestoSolicitante.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd() + " " + (puestoSolicitante.UbicacionAdministrativa.Departamento.NomDepartamento  != null ?  puestoSolicitante.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "") ;

                                        var informacionContacto = servicioFuncionario.DescargarInformacionContacto(model.BoletaPrestamo.CedulaSolicitante);

                                        foreach (var item in informacionContacto)
                                        {
                                            var info = (CInformacionContactoDTO)item;
                                            if (info.TipoContacto.IdEntidad == 1) // Correo
                                            {
                                                model.BoletaPrestamo.CorreoSolicitante = info.DesContenido;
                                            }
                                        }

                                        return PartialView("_FormularioBoleta", model);
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("BuscarUsuario", "El expediente del funcionario solicitado ya ha sido prestado.");
                                        throw new Exception("BuscarUsuario");
                                    }
                                }
                                else
                                {
                                    var puesto = (CPuestoDTO)busquedaFuncionario[1];
                                    var expediente = (CExpedienteFuncionarioDTO)busquedaExpediente[0];
                                    model.Mensaje = "El Funcionario buscado no posee un expediente registrado, por favor brindar el número de expediente.";
                                    model.Funcionario = (CFuncionarioDTO)busquedaFuncionario[0];
                                    model.BoletaPrestamo.NombreFuncionarioSolicitado = model.Funcionario.Nombre;
                                    model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;
                                    model.BoletaPrestamo.ApellidoFuncionarioSolicitado = model.Funcionario.PrimerApellido + "  " + model.Funcionario.SegundoApellido;
                                    model.BoletaPrestamo.DivisiónFuncionario = puesto.UbicacionAdministrativa.Division.NomDivision;
                                    model.BoletaPrestamo.DirecciónFuncionario = puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion;
                                    model.BoletaPrestamo.DepartamentoFuncionario = puesto.UbicacionAdministrativa.Departamento.NomDepartamento;
                                    model.BoletaPrestamo.NumeroExpediente = expediente.NumeroExpediente.ToString();
                                    return PartialView("_FormularioBoleta", model);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("BuscarUsuario", "EL usuario solicitante no posee registros dentro del MOPT.");
                                throw new Exception("BuscarUsuario");
                            }
                        }
                        else
                        { // Error
                            ModelState.AddModelError("BuscarUsuario", ((CErrorDTO)busquedaFuncionario[0]).MensajeError);
                            throw new Exception("BuscarUsuario");
                        }
                    }
                }
                else if (SubmitButton == "Guardar Boleta")
                {
                    if (model.BoletaPrestamo.Telefonolicitante == null || model.BoletaPrestamo.Telefonolicitante == "")
                    {
                        ModelState.AddModelError("BuscarUsuario", "Debe digitar el Teléfono");
                        throw new Exception("BuscarUsuario");
                    }

                    if (model.BoletaPrestamo.CorreoSolicitante == null || model.BoletaPrestamo.CorreoSolicitante == "")
                    {
                        ModelState.AddModelError("BuscarUsuario", "Debe digitar el Correo");
                        throw new Exception("BuscarUsuario");
                    }

                    if (model.BoletaPrestamo.MotivoPrestamo == null ||  model.BoletaPrestamo.MotivoPrestamo == "")
                    {
                        ModelState.AddModelError("BuscarUsuario", "Debe digitar el Motivo");
                        throw new Exception("BuscarUsuario");
                    }

                    if (ModelState.IsValid == false)
                        throw new Exception("BuscarUsuario");

                    var temp = servicioUsuario.ObtenerUsuarioPorNombre(principal.Identity.Name);

                    var busquedaFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.BoletaPrestamo.CedulaFuncionario);
                    if (model.UsuarioExterno)
                    { // Es un usuario externo.

                        model.Usuario = (CUsuarioDTO)temp[0][0];
                        model.Funcionario = (CFuncionarioDTO)busquedaFuncionario[0];
                        model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;
                        model.BoletaPrestamo.TipoUsuario = TipoUsuarioEnum.Externo;

                        var respuesta = servicioArchivo.AgregarBoletaPrestamo(model.Funcionario,
                            model.Usuario, model.BoletaPrestamo);

                        if (respuesta.GetType() != typeof(CErrorDTO)) {
                            return JavaScript("window.location = '/Archivo/Details?codigo=" +
                            ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                        }
                        else
                        {
                            CErrorDTO error = (CErrorDTO)respuesta;
                            ModelState.AddModelError("BuscarUsuario", error.MensajeError);
                            throw new Exception(error.MensajeError);
                        }
                    }
                    else
                    { // Es un usuario interno.

                        model.Usuario = (CUsuarioDTO)temp[0][0];
                        model.Funcionario = (CFuncionarioDTO)busquedaFuncionario[0];
                        model.BoletaPrestamo.CedulaFuncionario = model.Funcionario.Cedula;
                        model.BoletaPrestamo.TipoUsuario = TipoUsuarioEnum.Interno;

                        var respuesta = servicioArchivo.AgregarBoletaPrestamo(model.Funcionario,
                            model.Usuario, model.BoletaPrestamo);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            return JavaScript("window.location = '/Archivo/Details?codigo=" +
                            ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "'");
                        }
                        else {
                            CErrorDTO error = (CErrorDTO)respuesta;
                            ModelState.AddModelError("BuscarUsuario", error.MensajeError);
                            throw new Exception(error.MensajeError);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("BuscarUsuario", "Nada");
                    throw new Exception("BuscarUsuario");
                }
            }
            catch (Exception error)
            {
                if (error.Message == "BuscarUsuario")
                {
                    return PartialView("_ErrorBoletaPrestamo");
                }
                else
                {
                    return PartialView("_ErrorBoletaPrestamo");
                }
            }
        }

        [HttpPost]
        public ActionResult CreateBusquedaPrestamo(BusquedaExpedienteVM model, string SubmitButton) {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (model.Expediente.FiltroBusqueda != null)
                    { //si el usuario seleccinó una opción de los radio Buttons
                          var respuesta = servicioArchivo.BusquedaBoletaSegunParametros(model.Expediente);

                            if (respuesta[0].GetType() != typeof(CErrorDTO))
                            {
                                model.ListaBoletas = new List<BoletasRecuperadasVM>(); //inicializamos la lista de VM de el VM padre.

                                foreach (CBoletaPrestamoDTO item in respuesta)
                                {
                                    BoletasRecuperadasVM obj = new BoletasRecuperadasVM(); //creamos un nuevo VM.
                                    obj.ListaBoletas = new List<CBoletaPrestamoDTO>(); // inicializamos la lista de DTO del VM obj.
                                    obj.ListaBoletas.Add(item); //Agregamos a la lista de DTOs el item de tipo CBoletaPrestamoDTO.
                                    model.ListaBoletas.Add(obj); //agregamos el obj a la lista de VMs del VM padre.
                                }

                                return PartialView("_DetailsBusquedaPrestamo", model.ListaBoletas);
                            }
                            else
                            {
                                CErrorDTO error = (CErrorDTO)respuesta[0];
                                ModelState.AddModelError("BuscarExpediente", error.MensajeError);
                                throw new Exception("BuscarExpediente");
                            }
                    }
                    else {
                        ModelState.AddModelError("BuscarExpediente", "Debe seleccionar una opción de búsqueda primero.");
                        throw new Exception("BuscarExpediente");
                    }
                }
                else {
                    ModelState.AddModelError("BuscarExpediente", "Error.");
                    throw new Exception("BuscarExpediente");
                }
            }
            catch (Exception error) {
                if (error.Message == "BuscarExpediente")
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
                else
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
            }
        }


        [HttpPost]
        public ActionResult CreateBusquedaExpediente(BusquedaExpedienteVM model, string SubmitButton) {

            try
            {
                if (SubmitButton == "Buscar")
                {

                    if (model.Expediente.FiltroBusqueda != null)
                    { //si el usuario seleccinó una opción de los radio Buttons

                        CBaseDTO[] respuesta = new CBaseDTO[0];

                        if (model.Expediente.FiltroBusqueda == "Cédula Funcionario")
                        {
                          respuesta = servicioArchivo.ObtenerExpedientePorCedulaFuncionario(model.Expediente.DatoABuscar);
                        }
                        else if (model.Expediente.FiltroBusqueda == "Número Expediente")
                        {
                            respuesta = servicioArchivo.ObtenerExpedientePorNumeroExpediente(Convert.ToInt32(model.Expediente.DatoABuscar));
                        }

                        if (respuesta.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Expediente = (CExpedienteFuncionarioDTO)respuesta.FirstOrDefault();
                            return PartialView("_FormularioBusquedaExpediente", model);
                        }
                        else
                        {
                            ModelState.AddModelError("BuscarExpediente", ((CErrorDTO)respuesta[0]).MensajeError);
                            throw new Exception("BuscarExpediente");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("BuscarExpediente", "Debe seleccionar una opción de búsqueda primero.");
                        throw new Exception("BuscarExpediente");
                    }

                }
                if (SubmitButton == "Actualizar Estado Expediente Devuelto")
                {
                    var respuesta = servicioArchivo.ActualizarEstadoExpedienteEnPrestamo(model.Expediente);
                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/Archivo/DetailsCambioEstadoExpediente?codigo=" + model.Expediente.NumeroExpediente + "'"); //((CRespuestaDTO)respuesta).Contenido + "'");
                    }
                    else
                    {
                        ModelState.AddModelError("BuscarExpediente", ((CErrorDTO)respuesta).MensajeError);
                        throw new Exception("BuscarExpediente");
                    }
                }
                else
                {
                    return PartialView(model);
                }
            }
            catch (Exception error) {
                if (error.Message == "BuscarExpediente")
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
                else
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
            }
        }

        [HttpPost]
        public ActionResult CreateFoleo(FoleoVM model, string SubmitButton) {

            try
            {
                if (SubmitButton == "Buscar")
                {

                    if (model.CedulaABuscar != null)
                    {
                        var respuesta = servicioArchivo.RealizarFoleo(model.CedulaABuscar);

                        if (respuesta.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {

                            model.expediente = (CExpedienteFuncionarioDTO)respuesta.FirstOrDefault().FirstOrDefault();
                            model.expediente.listaTomos = respuesta.ElementAt(1).OfType<CTomoDTO>().ToList(); // convertimos el array de tipo CBaseDTO a una lista de lo que contiene ese array.

                            AsociarFoliosATomos(model.expediente.listaTomos, respuesta.ElementAt(2).OfType<CFolioDTO>().ToList()); // asociamos los folios a sus respectivos tomos.

                            return PartialView("_DetailsFoleo", model);
                        }
                        else
                        {
                            CErrorDTO error = (CErrorDTO)respuesta.FirstOrDefault().FirstOrDefault();
                            ModelState.AddModelError("BuscarExpediente", error.MensajeError);
                            throw new Exception("BuscarExpediente");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("BuscarExpediente", "Debe ingresar una cédula a buscar.");
                        throw new Exception("BuscarExpediente");
                    }
                }
                else
                {
                    ModelState.AddModelError("BuscarExpediente", "Error.");
                    throw new Exception("BuscarExpediente");
                }

            }
            catch (Exception error) {
                if (error.Message == "BuscarExpediente")
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
                else
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
            }

        }

        [HttpPost]
        public ActionResult CreateTrasladoArchivoCentral(BusquedaExpedienteVM model, string SubmitButton) {

            try
            {
                if (SubmitButton == "Buscar")
                {

                    if (model.Expediente.DatoABuscar != null)
                    {
                        var respuesta = servicioArchivo.ObtenerExpedientePorNumeroExpediente(Convert.ToInt32(model.Expediente.DatoABuscar));
                        if (respuesta.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Expediente = (CExpedienteFuncionarioDTO)respuesta.FirstOrDefault();
                            return PartialView("_FormularioTrasladoArchivoCentral", model);
                        }
                        else {

                            CErrorDTO error = (CErrorDTO)respuesta.FirstOrDefault();
                            ModelState.AddModelError("BuscarExpediente", error.MensajeError);
                            throw new Exception("BuscarExpediente");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("BuscarExpediente", "Debe ingresar una cédula a buscar.");
                        throw new Exception("BuscarExpediente");
                    }
                }

                if (SubmitButton == "Actualizar") {
                    if (model.Expediente.FechaTrasladoArchivoCentral != Convert.ToDateTime(DateTime.MinValue))
                    {// si tiene la fecha '01/01/0001 12:00:00 AM', quiere decir que el usuario no ingresó una fecha de traslado a archivo central.

                        if (model.Expediente.NumeroExpedienteEnArchivo != null)
                        {
                            if (model.Expediente.NumeroCaja != null)
                            {
                                var respuesta = servicioArchivo.ActualizarFechaTrasladoArchivoCentralExpediente(model.Expediente);
                                if (((CRespuestaDTO)respuesta).Contenido.GetType() != typeof(CErrorDTO))
                                {

                                    if (((CRespuestaDTO)respuesta).Contenido != null)
                                    { // si el contenido no es null, quiere decir que logró actualizar el expediente

                                        return JavaScript("window.location = '/Archivo/DetailsTrasladoArchivoCentral?codigo=" + ((CRespuestaDTO)respuesta).Contenido + "'");
                                    }
                                    else
                                    { // si la respuesta es que no actualizó datos nuevos de la tabla.
                                        ModelState.AddModelError("BuscarExpediente", "El expediente del funcionario solicitado, ya tiene la fecha de traslado que digitó.");
                                        throw new Exception("BuscarExpediente");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("BuscarExpediente", ((CErrorDTO)respuesta).MensajeError);
                                    throw new Exception("BuscarExpediente");
                                }
                            }
                            else {
                                ModelState.AddModelError("BuscarExpediente", "El número de caja es un campo obligatorio.");
                                throw new Exception("BuscarExpediente");
                            }
                        }
                        else {
                            ModelState.AddModelError("BuscarExpediente", "El número de expediente en archivo es un campo obligatorio.");
                            throw new Exception("BuscarExpediente");
                        }
                    }
                    else { 
                        throw new Exception("BuscarExpediente");
                    }
                }

                else
                {

                    ModelState.AddModelError("BuscarExpediente", "Error.");
                    throw new Exception("BuscarExpediente");
                }
            }
            catch (Exception error) {
                if (error.Message == "BuscarExpediente")
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
                else
                {
                    return PartialView("_ErrorBusquedaExpediente");
                }
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteBoletaPrestamo(FormularioBoletaPrestamoVM model)
        {
            List<BoletaRptData> modelo = new List<BoletaRptData>();

            modelo.Add(BoletaRptData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Archivo"), "BoletaRpt.rpt");
            return new CrystalReportPdfResult(reportPath, modelo,"PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteBusquedaPrestamo(List<BoletasRecuperadasVM> model)
        {
            List<BusquedaBoletaRptData> modelo = new List<BusquedaBoletaRptData>();
            foreach (var item in model) {
                modelo.Add(BusquedaBoletaRptData.GenerarReporteBusquedaBoletas(item, String.Empty));
            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Archivo"), "BusquedaBoletaRpt.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        public void AsociarFoliosATomos(List<CTomoDTO> tomos, List<CFolioDTO> folios)
        {

            foreach (CTomoDTO item in tomos)
            {
                
                foreach (CFolioDTO item2 in folios) {
                    if (item2.IdTomo == item.IdEntidad) {
                        item.ListaFolios.Add(item2);
                    }
                }

                folios.RemoveAll(r => r.IdTomo == item.IdEntidad); //removemos los folios de la lista 'folios' ya agregados a su respectivo tomo.
            }
        }

    }
}