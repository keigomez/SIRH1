using System;
using System.IO;
using SIRH.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.FuncionarioService;
using SIRH.Web.CalificacionService;
//using SIRH.Web.FuncionarioLocal;
//using SIRH.Web.CalificacionLocal;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.Helpers;
using SIRH.Web.Models;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using SIRH.Web.Reports.Calificacion;
using SIRH.Web.Reports.Historial;
using SIRH.Web.Reports.Datos;
using SIRH.Web.UserValidation;
using System.Xml;

namespace SIRH.Web.Controllers
{/// <summary>
/// 
/// </summary>
    public class CalificacionController : Controller
    {
        // GET: Evaluacion
         
        #region Variables
        CCalificacionServiceClient servicioCalificacion = new CCalificacionServiceClient();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();

        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        #endregion

        #region Control
        public ActionResult Index()
        {
            //var usuario = "MOPT\\jalvarag";
            //context.IniciarSesionModulo(Session, usuario, Convert.ToInt32(EModulosHelper.Calificacion), 0);
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));
                return View();
            }
        }

        public ActionResult Calificar(string cedula)
        {
            bool permiso = false;
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (cedula != null)
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                  Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                  Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.FueraDominio))] != null)
                    {
                        permiso = true;
                    }
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                       Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                       Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Operativo))] != null)
                    {
                        permiso = true;
                    }
                }


                if (permiso == true)
                {
                    var usuario = servicioUsuario.ObtenerUsuarioPorNombre(principal.Identity.Name);
                    if (usuario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        FormularioCalificacionVM modelF = new FormularioCalificacionVM();

                        // Verificar que exista para el Periodo para el año actual menos 1
                        DateTime Fecha = DateTime.Today;
                        int Periodo = Fecha.AddYears(-1).Year;
                        var dato = servicioCalificacion.ObtenerPeriodoCalificacion(Periodo);

                        if (dato.GetType() != typeof(CErrorDTO))
                        {
                            var periodo = (CPeriodoCalificacionDTO)dato;

                            // Verificar que está dentro de las fechas para incluir las Calificaciones
                            if (Fecha.Date >= periodo.FecRige.Date && Fecha.Date <= Convert.ToDateTime(periodo.FecVence).Date)
                            {
                                var cedulaJefatura = "";
                                if (cedula != null)
                                {
                                    cedulaJefatura = cedula;
                                }
                                else
                                {
                                    cedulaJefatura = ((CFuncionarioDTO)usuario[1][0]).Cedula;
                                }

                                var listadoFuncionarios = servicioCalificacion.ListarFuncionariosJefatura(Periodo, cedulaJefatura)
                                              .Select(Q => new SelectListItem
                                              {
                                                  Value = ((CFuncionarioDTO)Q).Cedula,
                                                  Text = ((CFuncionarioDTO)Q).Nombre
                                              });

                                modelF.Funcionarios = new SelectList(listadoFuncionarios, "Value", "Text");
                                modelF.FuncionarioSeleccionado = "";

                                return View(modelF);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "No se pueden registrar Evaluaciones porque está cerrado el periodo correspondiente");
                                return View("_ErrorCalificacion");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)dato).MensajeError);
                            return View("_ErrorCalificacion");
                        }
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        [HttpPost]
        public ActionResult Calificar(string cedula, int Fpregunta, string SubmitButton)
        {
            DateTime Fecha = DateTime.Today;
            int periodo = Fecha.AddYears(-1).Year;

            FormularioCalificacionVM modelF = new FormularioCalificacionVM();

            var dato = servicioCalificacion.DescargarCalificacionCedula(cedula, periodo).FirstOrDefault();
            if (dato.GetType() != typeof(CErrorDTO))
            {
                var calificacion = (CCalificacionNombramientoDTO)dato;
                if (calificacion.IndEstadoDTO == 1)
                {
                    modelF = ObtenerCalificacion(calificacion.IdEntidad);
                    if (calificacion.IndEntregadoDTO)
                        modelF.EsAnulable = false;
                    else
                        modelF.EsAnulable = true;

                    return PartialView("_CalificacionExistente", modelF);

                    //ModelState.AddModelError("Busqueda", "Ya existe una calificación para el funcionario para el periodo correspondiente");
                    //return PartialView("_ErrorCalificacion", modelF);
                }
            }

            //var datosDetallePuestoF = servicioFuncionario.BuscarFuncionarioDetallePuesto(cedula);
            var datosDetallePuestoF = servicioCalificacion.DescargarFuncionarioCalificarDetallePuesto(cedula);
            var catalogoPreguntas = servicioCalificacion.ListarPreguntas(Fpregunta);
            var datosDetalleNombramiento = servicioCalificacion.ListarCalificaciones(cedula);

            modelF.NombreFormulario = ObtenerNombreFormulario(Fpregunta);
            modelF.CatalogoPregunta = new List<CCatalogoPreguntaDTO>();
            modelF.CNombramientoB = new List<CCalificacionNombramientoDTO>();
            if (datosDetallePuestoF != null && datosDetallePuestoF.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                modelF.Funcionario = (CFuncionarioDTO)datosDetallePuestoF.ElementAt(0);
                modelF.Periodos = periodo.ToString();
                modelF.Puesto = (CPuestoDTO)datosDetallePuestoF.ElementAt(1);
                modelF.DetallePuesto = (CDetallePuestoDTO)datosDetallePuestoF.ElementAt(2);
                modelF.DetalleContratacion = (CDetalleContratacionDTO)datosDetallePuestoF.ElementAt(3);
                modelF.Expediente = (CExpedienteFuncionarioDTO)(datosDetalleNombramiento.ElementAt(4).ElementAt(0));
                //modelF.Detalle = new List<CDetalleCalificacionNombramientoDTO>();
                modelF.CalificacionNombramiento = new CCalificacionNombramientoDTO();
                modelF.CalificacionNombramiento.JefeInmediato = (CFuncionarioDTO)(datosDetalleNombramiento.ElementAt(5).ElementAt(0));
                modelF.CalificacionNombramiento.JefeSuperior = (CFuncionarioDTO)(datosDetalleNombramiento.ElementAt(6).ElementAt(0));
                modelF.CalificacionNombramiento.IndFormularioDTO = Fpregunta;

                int cod = Convert.ToInt32(modelF.Puesto.CodPuesto);
                modelF.CalificacionNombramiento.DetalleCalificacion = new List<CDetalleCalificacionNombramientoDTO>();

                //  var calificacionNombramiento = servicioCalificacion.ObtenerCalificacion(cod);
                //  modelF.CalificacionNombramiento = (CCalificacionNombramientoDTO)calificacionNombramiento.ElementAt(0);
                if (datosDetalleNombramiento.ElementAt(1).Count() > 0)
                {
                    foreach (var item in datosDetalleNombramiento.ElementAt(1))
                    {
                        if (((CCalificacionNombramientoDTO)item).NombramientoDTO.IdEntidad > 0)
                            modelF.CNombramientoB.Add((CCalificacionNombramientoDTO)item);
                    }
                }
                else
                {
                    modelF.CNombramientoB.Add(new CCalificacionNombramientoDTO());
                }

                modelF.Fecha = Fecha;
                modelF.Usuario = WindowsIdentity.GetCurrent().Name.ToString();
                if (catalogoPreguntas != null)
                {
                    foreach (var item in catalogoPreguntas)
                    {
                        modelF.CatalogoPregunta.Add((CCatalogoPreguntaDTO)item);
                        modelF.CalificacionNombramiento.DetalleCalificacion.Add(new CDetalleCalificacionNombramientoDTO
                        {
                            NumNotasPorPreguntaDTO = "0",
                            CatalogoPreguntaDTO = (CCatalogoPreguntaDTO)item
                        });
                    }
                }
            }
            else
            {
                modelF.Error = (CErrorDTO)datosDetallePuestoF.ElementAt(0);
                ModelState.AddModelError("Busqueda", modelF.Error.MensajeError);
                return PartialView("_ErrorCalificacion", modelF);
            }
            return PartialView("_Calificar", modelF);
        }

        [HttpPost]
        public ActionResult DetailsCalificacion(FormularioCalificacionVM model)
        {
            string usuarioEvaluador = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            model.CalificacionNombramiento.Periodo.IdEntidad = Convert.ToInt32(model.Periodos);
            model.CalificacionNombramiento.NombramientoDTO = model.CNombramientoB.LastOrDefault().NombramientoDTO;
            model.CalificacionNombramiento.CalificacionDTO = new CCalificacionDTO();
            model.CalificacionNombramiento.CalificacionDTO.IdEntidad = model.CalificacionFinal;
            model.CalificacionNombramiento.UsrEvaluadorDTO = usuarioEvaluador;
            model.CalificacionNombramiento.IndEstadoDTO = 1;
            model.CalificacionNombramiento.FecCreacionDTO = DateTime.Today;
            model.CalificacionNombramiento.JefeInmediato.Sexo = GeneroEnum.Indefinido;
            model.CalificacionNombramiento.JefeSuperior.Sexo = GeneroEnum.Indefinido;
            model.CalificacionNombramiento.IndFormularioDTO = model.CatalogoPregunta[0].IndTipoFormularioDTO;

            model.NombreFormulario = ObtenerNombreFormulario(model.CatalogoPregunta[0].IndTipoFormularioDTO);

            if (model.CalificacionNombramiento.DetalleCalificacion == null)
                model.CalificacionNombramiento.DetalleCalificacion = new List<CDetalleCalificacionNombramientoDTO>();

            for (int i = 0; i < model.Detalle.Count; i++)
            {
                model.Detalle[i].CatalogoPreguntaDTO = model.CatalogoPregunta[i];
                model.CalificacionNombramiento.DetalleCalificacion.Add(model.Detalle[i]);
            }


            // Validar
            if ((model.CalificacionNombramiento.ObsCapacitacionDTO != null ? model.CalificacionNombramiento.ObsCapacitacionDTO : "").Length <= 0)
            {
                return JavaScript("MostrarMensaje('Debe incluir la información de CAPACITACIÓN Y OTRAS MEDIDAS DE MEJORAMIENTO');");
            }
            if ((model.CalificacionNombramiento.ObsJustificacionCapacitacionDTO != null ? model.CalificacionNombramiento.ObsJustificacionCapacitacionDTO : "").Length <= 0)
            {
                return JavaScript("MostrarMensaje('Debe incluir la información de CAPACITACIÓN Y OTRAS MEDIDAS DE MEJORAMIENTO');");
            }
            if ((model.CalificacionNombramiento.ObsGeneralDTO != null ? model.CalificacionNombramiento.ObsGeneralDTO : "").Length <= 0 && model.CalificacionNombramiento.CalificacionDTO.IdEntidad == 1)
            {
                return JavaScript("MostrarMensaje('Debe incluir la información de CAPACITACIÓN Y OTRAS MEDIDAS DE MEJORAMIENTO');");
            }

            var respuesta = servicioCalificacion.GuardarCalificacionFuncionario(model.CalificacionNombramiento, model.Detalle.ToArray());
            if (respuesta.GetType() != typeof(CErrorDTO))
            {
                model.CalificacionNombramiento = (CCalificacionNombramientoDTO)respuesta[0];
                //model.Detalle = (CDetalleCalificacionNombramientoDTO)respuesta[1];

                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion),
                                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(model.CalificacionNombramiento.IdEntidad),
                                                        CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));

                var datos = servicioCalificacion.ObtenerCalificacion(Convert.ToInt32(model.CalificacionNombramiento.IdEntidad));
                if (datos.Count() > 1)
                {
                    model.CalificacionNombramiento = (CCalificacionNombramientoDTO)datos.ElementAt(0);
                    model.CalificacionNombramiento.Periodo.IdEntidad = Convert.ToInt32(model.Periodos);
                    model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                    model.Puesto = (CPuestoDTO)datos.ElementAt(2);
                    model.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(3);
                    model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
                    model.PuntuacionFinal = model.CalificacionNombramiento.DetalleCalificacion.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
                }
                else
                {
                    model.Error = (CErrorDTO)datos.ElementAt(0);
                }
            }
            else
            {
                model.Error = new CErrorDTO { MensajeError = respuesta[0].Mensaje };
            }

            return PartialView("_DetailsCalificacion", model);
        }


        public ActionResult Details(int codigo)
        {
            return View(ObtenerCalificacion(codigo));

            //FormularioCalificacionVM modelB = new FormularioCalificacionVM();

            //var datos = servicioCalificacion.ObtenerCalificacion(codigo);

            //if (datos.Count() > 1)
            //{
            //    modelB.CalificacionNombramiento = (CCalificacionNombramientoDTO)datos.ElementAt(0);
            //    if (modelB.CalificacionNombramiento.DetalleCalificacionModificado == null)
            //        modelB.CalificacionNombramiento.DetalleCalificacionModificado = new List<CDetalleCalificacionNombramientoDTO>();

            //    modelB.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
            //    modelB.Puesto = (CPuestoDTO)datos.ElementAt(2);
            //    modelB.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(3);
            //    modelB.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
            //    if (modelB.CalificacionNombramiento.DetalleCalificacionModificado.Count() == 0)
            //    {
            //        modelB.PuntuacionFinal = modelB.CalificacionNombramiento.DetalleCalificacion.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
            //        modelB.CalificacionFinalLetra = modelB.CalificacionNombramiento.CalificacionDTO.DesCalificacion;
            //    }
            //    else
            //    {
            //        int idCalificacion = 0;
            //        string DesCalificacion = "";
            //        modelB.PuntuacionFinal = modelB.CalificacionNombramiento.DetalleCalificacionModificado.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
            //        ObtenerDetalleCalificacion(modelB.PuntuacionFinal, out idCalificacion, out DesCalificacion);
            //        modelB.CalificacionFinalLetra = DesCalificacion;
            //    }

            //    // Verificar que está dentro de las fechas para anular las Calificaciones
            //    DateTime Fecha = DateTime.Today;
            //    int periodo = Fecha.AddYears(-1).Year;
            //    if (Fecha.Date >= modelB.CalificacionNombramiento.Periodo.FecRige.Date && Fecha.Date <= Convert.ToDateTime(modelB.CalificacionNombramiento.Periodo.FecVence).Date)
            //    {
            //        modelB.EsAnulable = true;

            //        // Verificar que no está entregado el formulario
            //        if (modelB.CalificacionNombramiento.IndEntregadoDTO)
            //            modelB.EsAnulable = false;
            //    }
            //    else
            //    {
            //        modelB.EsAnulable = false;
            //    }
            //}
            //else
            //{
            //    modelB.Error = (CErrorDTO)datos.ElementAt(0);
            //}

            //return View(modelB);
        }

        public ActionResult DetailsHistorial(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                BusquedaHistorialCalificacionVM modelB = new BusquedaHistorialCalificacionVM();

                var datos = servicioCalificacion.ObtenerCalificacion(codigo);

                if (datos.Count() > 1)
                {
                    modelB.DetalleCNombramiento = (CCalificacionNombramientoDTO)datos.ElementAt(0);
                    modelB.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                    modelB.Puesto = (CPuestoDTO)datos.ElementAt(2);
                    modelB.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(3);
                    if (datos.ElementAt(4).IdEntidad > 0)
                    {
                        modelB.DetalleCalificacionNombramiento = ((CDetalleCalificacionNombramientoDTO)datos.ElementAt(4));
                        modelB.MensajeTSE = "";
                    }
                    else
                    {
                        modelB.MensajeTSE = "Dato Antiguo, no posee Detalle Calificación";
                    }
                    modelB.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(5);
                }
                else
                {
                    modelB.Error = (CErrorDTO)datos.ElementAt(0);
                }

                return View(modelB);
            }
        }


        public ActionResult Ratificar(string cedula)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                   Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                   Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Operativo))] != null)
                {
                    var usuario = servicioUsuario.ObtenerUsuarioPorNombre(principal.Identity.Name);
                    if (usuario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        FormularioCalificacionVM modelF = new FormularioCalificacionVM();

                        // Verificar que exista para el Periodo para el año actual menos 1
                        DateTime Fecha = DateTime.Today;
                        int Periodo = Fecha.AddYears(-1).Year;
                        var dato = servicioCalificacion.ObtenerPeriodoCalificacion(Periodo);

                        if (dato.GetType() != typeof(CErrorDTO))
                        {
                            var periodo = (CPeriodoCalificacionDTO)dato;

                            // Verificar que está dentro de las fechas para incluir las Calificaciones
                            if (Fecha.Date >= periodo.FecRige.Date && Fecha.Date <= Convert.ToDateTime(periodo.FecVence).Date)
                            {
                                // Listar los funcionarios de Jefe Superior
                                if (cedula == null)
                                    cedula = ((CFuncionarioDTO)usuario[1][0]).Cedula;

                                //var cedulaJefatura = "0110070883";
                                var listadoFuncionarios = servicioCalificacion.ListarFuncionarios(Periodo, "", "", cedula, 0, 0, 0)
                                              .Select(Q => new SelectListItem
                                              {
                                                  Value = ((CCalificacionNombramientoFuncionarioDTO)Q).Funcionario.Cedula,
                                                  Text = ((CCalificacionNombramientoFuncionarioDTO)Q).Funcionario.Nombre
                                              }).OrderBy(Q => Q.Text);
                                //listadoFuncionarios = listadoFuncionarios
                                modelF.Funcionarios = new SelectList(listadoFuncionarios, "Value", "Text");
                                modelF.FuncionarioSeleccionado = "";

                                return View(modelF);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "No se pueden registrar Evaluaciones porque está cerrado el periodo correspondiente");
                                return View("_ErrorCalificacion");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)dato).MensajeError);
                            return View("_ErrorCalificacion");
                        }
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorAcceso(Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        [HttpPost]
        public ActionResult Ratificar(FormularioCalificacionVM model, string SubmitButton)
        {
            if (SubmitButton == null)
            {
                string cedula = model.CedulaBuscar;
                DateTime Fecha = DateTime.Today;
                int periodo = Fecha.AddYears(-1).Year;

                FormularioCalificacionVM modelF = new FormularioCalificacionVM();

                // Buscar si el funcionario tiene una Calificación para el periodo correspondiente
                var dato = servicioCalificacion.DescargarCalificacionCedula(cedula, periodo).FirstOrDefault();
                if (dato.GetType() != typeof(CErrorDTO))
                {
                    var resultado = servicioCalificacion.ObtenerCalificacion(((CCalificacionNombramientoDTO)dato).IdEntidad);
                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        // Mostrar datos
                        modelF.Fecha = Fecha;
                        modelF.Periodos = periodo.ToString();
                        modelF.Usuario = WindowsIdentity.GetCurrent().Name.ToString();

                        //var datosDetalleNombramiento = servicioCalificacion.ListarCalificaciones(cedula);
                        //var datosDetallePuestoF = servicioFuncionario.BuscarFuncionarioDetallePuesto(cedula);
                        //var catalogoPreguntas = servicioCalificacion.ListarPreguntas(Fpregunta);

                        modelF.CatalogoPregunta = new List<CCatalogoPreguntaDTO>();
                        modelF.CNombramientoB = new List<CCalificacionNombramientoDTO>();
                        modelF.CalificacionNombramiento = (CCalificacionNombramientoDTO)resultado.ElementAt(0);

                        //if (modelF.CalificacionNombramiento.IndRatificacionDTO == 1)
                        //{
                        //    // Ya fue ratificado
                        //    ModelState.AddModelError("Busqueda", "Dicha Evaluación ya fue ratificada");
                        //    return PartialView("_ErrorCalificacion", modelF);
                        //}
                        if (modelF.CalificacionNombramiento.DetalleCalificacionModificado.Count > 0)
                        {
                            // Ya fue modificado
                            ModelState.AddModelError("Busqueda", "Dicha Evaluación ya fue modificada");
                            return PartialView("_ErrorCalificacion", modelF);
                        }
                        modelF.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1);
                        modelF.Puesto = (CPuestoDTO)resultado.ElementAt(2);
                        modelF.DetallePuesto = (CDetallePuestoDTO)resultado.ElementAt(3);
                        modelF.Expediente = (CExpedienteFuncionarioDTO)(resultado.ElementAt(4));
                        //modelF.Detalle = new List<CDetalleCalificacionNombramientoDTO>();

                        modelF.Detalle = modelF.CalificacionNombramiento.DetalleCalificacion;
                        modelF.CalificacionNombramiento.JefeInmediato.Nombre = modelF.CalificacionNombramiento.JefeInmediato.Nombre.TrimEnd() + " " +
                                                                        (modelF.CalificacionNombramiento.JefeInmediato.PrimerApellido != null ? modelF.CalificacionNombramiento.JefeInmediato.PrimerApellido.TrimEnd() : "") + " " +
                                                                        (modelF.CalificacionNombramiento.JefeInmediato.SegundoApellido != null ? modelF.CalificacionNombramiento.JefeInmediato.SegundoApellido.TrimEnd() : "");

                        modelF.CalificacionNombramiento.JefeSuperior.Nombre = modelF.CalificacionNombramiento.JefeSuperior.Nombre.TrimEnd() + " " +
                                                                        (modelF.CalificacionNombramiento.JefeSuperior.PrimerApellido != null ? modelF.CalificacionNombramiento.JefeSuperior.PrimerApellido.TrimEnd() : "") + " " +
                                                                        (modelF.CalificacionNombramiento.JefeSuperior.SegundoApellido != null ? modelF.CalificacionNombramiento.JefeSuperior.SegundoApellido.TrimEnd() : "");

                        int Fpregunta = modelF.CalificacionNombramiento.IndFormularioDTO;

                        modelF.CalificacionFinalLetra = modelF.CalificacionNombramiento.CalificacionDTO.DesCalificacion;
                        modelF.NombreFormulario = ObtenerNombreFormulario(Fpregunta);
                        modelF.CalificacionNombramiento.IndFormularioDTO = Fpregunta;

                        modelF.CalificacionNombramiento.DetalleCalificacion = new List<CDetalleCalificacionNombramientoDTO>();

                        modelF.PuntuacionFinal = 0;
                        foreach (var item in modelF.Detalle)
                        {
                            modelF.CatalogoPregunta.Add(item.CatalogoPreguntaDTO);
                            modelF.CalificacionNombramiento.DetalleCalificacion.Add(new CDetalleCalificacionNombramientoDTO
                            {
                                NumNotasPorPreguntaDTO = item.NumNotasPorPreguntaDTO,
                                CatalogoPreguntaDTO = item.CatalogoPreguntaDTO
                            });
                            modelF.PuntuacionFinal += Convert.ToDecimal(item.NumNotasPorPreguntaDTO);
                        }

                        return PartialView("_Ratificar", modelF);
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.FirstOrDefault()).MensajeError);
                        return PartialView("_ErrorCalificacion", modelF);
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)dato).MensajeError);
                    return PartialView("_ErrorCalificacion", modelF);
                }
            }

            if (SubmitButton == "Ratificar Evaluación")
            {
                var respuesta = servicioCalificacion.RatificarCalificacionNombramiento(model.CalificacionNombramiento.IdEntidad);
                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion),
                                                            Convert.ToInt32(EAccionesBitacora.Editar), Convert.ToInt32(model.CalificacionNombramiento.IdEntidad),
                                                            CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));

                    var datos = servicioCalificacion.ObtenerCalificacion(Convert.ToInt32(model.CalificacionNombramiento.IdEntidad));
                    if (datos.Count() > 1)
                    {
                        model.CalificacionNombramiento = (CCalificacionNombramientoDTO)datos.ElementAt(0);
                        model.CalificacionNombramiento.Periodo.IdEntidad = Convert.ToInt32(model.Periodos);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                        model.Puesto = (CPuestoDTO)datos.ElementAt(2);
                        model.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(3);
                        model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
                        model.PuntuacionFinal = model.CalificacionNombramiento.DetalleCalificacion.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }
                }
                else
                {
                    model.Error = new CErrorDTO { MensajeError = respuesta.Mensaje };
                }

                return PartialView("_DetailsCalificacion", model);
            }

            if (SubmitButton == "Guardar")
            {
                model.CalificacionNombramiento.JefeInmediato.Sexo = GeneroEnum.Indefinido;
                model.CalificacionNombramiento.JefeSuperior.Sexo = GeneroEnum.Indefinido;
                if (model.CalificacionNombramiento.DetalleCalificacion == null)
                    model.CalificacionNombramiento.DetalleCalificacion = new List<CDetalleCalificacionNombramientoDTO>();

                for (int i = 0; i < model.Detalle.Count; i++)
                {
                    model.Detalle[i].CatalogoPreguntaDTO = model.CatalogoPregunta[i];
                    model.CalificacionNombramiento.DetalleCalificacion.Add(model.Detalle[i]);
                }

                var respuesta = servicioCalificacion.GuardarCalificacionModificada(model.CalificacionNombramiento, model.Detalle.ToArray());
                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    model.CalificacionNombramiento = (CCalificacionNombramientoDTO)respuesta[0];
                    //model.Detalle = (CDetalleCalificacionNombramientoDTO)respuesta[1];

                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion),
                                                            Convert.ToInt32(EAccionesBitacora.Editar), Convert.ToInt32(model.CalificacionNombramiento.IdEntidad),
                                                            CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));

                    var datos = servicioCalificacion.ObtenerCalificacion(Convert.ToInt32(model.CalificacionNombramiento.IdEntidad));
                    if (datos.Count() > 1)
                    {
                        model.CalificacionNombramiento = (CCalificacionNombramientoDTO)datos.ElementAt(0);
                        model.CalificacionNombramiento.Periodo.IdEntidad = Convert.ToInt32(model.Periodos);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                        model.Puesto = (CPuestoDTO)datos.ElementAt(2);
                        model.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(3);
                        model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
                        model.PuntuacionFinal = model.CalificacionNombramiento.DetalleCalificacion.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }
                }
                else
                {
                    model.Error = new CErrorDTO { MensajeError = respuesta[0].Mensaje };
                }

                return PartialView("_DetailsCalificacion", model);
            }

            return PartialView("_ErrorCalificacion", model);
        }

        public ActionResult Historial()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Historial(string ced)
        {
            BusquedaHistorialCalificacionVM model = new BusquedaHistorialCalificacionVM();
            try
            {
                context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                  Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                  Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
                {
                    model.EsAdministrador = true;
                }
                else
                {
                    model.EsAdministrador = false;
                }
                    
                if (ced != "")
                {
                    model.CedulaSearch = ced;
                    model.MensajeFuncionario = "La persona consultada no está registrada";
                    if (ModelState.IsValid)
                    {
                        var resultado = servicioCalificacion.ListarCalificaciones(ced);
                        if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = ((CFuncionarioDTO)resultado.FirstOrDefault().FirstOrDefault());
                            model.Calificaciones = new List<CCalificacionNombramientoDTO>();
                            model.Puesto = ((CPuestoDTO)resultado[2][0]);
                            model.DetallePuesto = ((CDetallePuestoDTO)resultado[3][0]);
                            model.Expediente = ((CExpedienteFuncionarioDTO)resultado[4][0]);
                            if (resultado != null)
                            {
                                foreach (var item in resultado.ElementAt(1))
                                {
                                    model.Calificaciones.Add((CCalificacionNombramientoDTO)item);
                                }
                            }

                            return PartialView("_Historial", model); 
                        }
                        else
                        {
                            if (((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).Codigo == -1)
                            {
                                ModelState.AddModelError("Busqueda", "La cédula digitada no se encontró, por favor revise que la información suministrada sea correcta");
                                throw new Exception("Busqueda");
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).MensajeError);
                                throw new Exception("Busqueda");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", "La cédula digitada no se encontró, por favor revise que la información suministrada sea correcta");
                }
            }
            catch (Exception error)
            {
                PartialView("_Historial", model);
                if (error.Message == "Busqueda")
                {
                    PartialView("_Historial", model);
                }
            }

            return PartialView("_Historial", model);
        }



        public PartialViewResult CalificarHistorico(int periodo, string cedFuncionario, decimal nota, string justificacion, bool EsPolicia)
        {
            Session["errorU"] = null;
            try
            {
                if (String.IsNullOrEmpty(cedFuncionario))
                {
                    return PartialView();
                }
                else
                {
                    CPeriodoCalificacionDTO datoPeriodo = new CPeriodoCalificacionDTO
                    {
                        IdEntidad = periodo
                    };

                    CFuncionarioDTO datoFuncionario = new CFuncionarioDTO
                    {
                        Cedula = cedFuncionario,
                        Sexo = GeneroEnum.Indefinido
                    };

                    var respuesta = servicioCalificacion.AgregarCalificacionHistorico(datoPeriodo, datoFuncionario, nota, justificacion, EsPolicia);
                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return PartialView("_Mensaje", new FormularioCalificacionVM());
                    }
                    else
                    {
                        ModelState.AddModelError("Agregar", ((CErrorDTO)respuesta).MensajeError);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
            }
            catch (Exception error)
            {
                return PartialView("_ErrorJefatura");
            }
        }
        public ActionResult List()
        {

            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                   Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                   Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
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
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        public ActionResult EditHistorialDetalle(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                BusquedaHistorialCalificacionVM model = new BusquedaHistorialCalificacionVM();

                var respuesta = servicioCalificacion.EditarCalificacionNombramiento(codigo);

                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion),
                                    Convert.ToInt32(EAccionesBitacora.Editar), Convert.ToInt32(codigo),
                                    CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));
                }
                else
                {
                    model.Error = new CErrorDTO { MensajeError = respuesta.Mensaje };
                }

                return PartialView("_EditHistorialDetalle", model);
            }
        }

        public ActionResult AnularCalificacion(int codigo, int codigoDCN)
        {

            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                BusquedaHistorialCalificacionVM model = new BusquedaHistorialCalificacionVM();
                try
                {
                    if (codigo == codigoDCN)
                    {
                        var respuesta = servicioCalificacion.EditarCalificacionNombramiento(codigoDCN);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion),
                                    Convert.ToInt32(EAccionesBitacora.Editar), Convert.ToInt32(codigoDCN),
                                    CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));

                            model.MensajeFuncionario = "Calificación ha sido anulada exitosamente";
                            return PartialView("_EditHistorialDetalle", model);

                        }
                        else
                        {
                            ModelState.AddModelError("modificar", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Calificación imposible de anular ya que es muy antigua.");
                        throw new Exception();
                    }
                }
                catch (Exception io)
                {
                    model.MensajeFuncionario = io.ToString();
                    return PartialView("AnularCalificacionNueva", model);
                }
            }
        }

        public ActionResult Periodo()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                  Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                  Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
                {
                    FormularioCalificacionVM modelF = new FormularioCalificacionVM();

                    // Verificar que exista para el Periodo para el año actual menos 1
                    var anio = DateTime.Today.AddYears(-1).Year;
                    modelF.PeriodoCalificacion = new CPeriodoCalificacionDTO
                    {
                        IdEntidad = anio,
                        FecRige = DateTime.Today.Date,
                        FecRigeReglaTecnica = DateTime.Today.Date
                    };

                    var dato = servicioCalificacion.ObtenerPeriodoCalificacion(anio);
                    if (dato.GetType() != typeof(CErrorDTO))
                    {
                        modelF.PeriodoCalificacion = (CPeriodoCalificacionDTO)dato;
                    }

                    return View(modelF);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        [HttpPost]
        public ActionResult Periodo(FormularioCalificacionVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Guardar Periodo")
                {
                    var respuesta = servicioCalificacion.AgregarPeriodoCalificacion(model.PeriodoCalificacion);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return PartialView("_Periodo", model);
                    }
                    else
                    {
                        ModelState.AddModelError("Agregar", respuesta.Mensaje);
                        model.Error.MensajeError = respuesta.Mensaje;
                        throw new Exception(respuesta.Mensaje);
                    }
                }

                if (SubmitButton == "Generar Listado Funcionarios")
                {
                    var respuesta = servicioCalificacion.GenerarListadoFuncionariosPeriodo(model.PeriodoCalificacion.IdEntidad);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        model.Periodos = ((CRespuestaDTO)respuesta).Contenido.ToString();
                        return PartialView("List", model);
                    }
                    else
                    {
                        ModelState.AddModelError("Agregar", respuesta.Mensaje);
                        model.Error = ((CErrorDTO)respuesta);

                        throw new Exception(respuesta.Mensaje);
                    }
                }
                throw new Exception("Busqueda");

            }
            catch (Exception error)
            {
                return PartialView("_Periodo", model);
            }
        }


        public ActionResult ReglaTecnica()
        {

            //context.IniciarSesionModulo(Session, "MOPT\\fmolinas", Convert.ToInt32(EModulosHelper.Calificacion), 0);
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                FormularioCalificacionVM modelF = new FormularioCalificacionVM();
                DateTime Fecha = DateTime.Today;
                var anio = DateTime.Today.AddYears(-1).Year;

                // Verificar que exista para el Periodo para el año actual menos 1
                var dato = servicioCalificacion.ObtenerPeriodoCalificacion(anio);

                if (dato.GetType() != typeof(CErrorDTO))
                {
                    var periodo = (CPeriodoCalificacionDTO)dato;

                    modelF.PeriodoCalificacion = periodo;
                    modelF.ReglasTecnicas = new List<CCalificacionReglaTecnicaDTO>();

                    // Consultar la lista
                    var regla = servicioCalificacion.ListarReglaTecnica(anio);

                    if (regla.GetType() == typeof(CErrorDTO))
                    {
                        modelF.Error = (CErrorDTO)regla[0];
                        ModelState.AddModelError("Busqueda", modelF.Error.MensajeError);
                        return PartialView("_ErrorCalificacion", modelF);
                    }


                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                       Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                       Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
                    {
                        modelF.EsAdministrador = true;
                       
                        foreach (CCalificacionReglaTecnicaDTO item in regla)
                        {
                            if (item.PorcExcelentesDTO >= 80)
                            {
                                modelF.ReglasTecnicas.Add(item);
                            }
                        }
                        return View(modelF);
                    }
                    else
                    {
                        // Solo debe permitir cargar el archivo
                        if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Operativo))] != null)
                        {
                            modelF.EsAdministrador = false;

                            if (Fecha.Date >= periodo.FecRigeReglaTecnica.Date && Fecha.Date <= Convert.ToDateTime(periodo.FecVenceReglaTecnica).Date)
                            {
                                //var usuario = servicioUsuario.ObtenerUsuarioPorNombre("MOPT\\fmolinas");
                                var usuario = servicioUsuario.ObtenerUsuarioPorNombre(principal.Identity.Name);
                                if (usuario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    // Buscar
                                    foreach (CCalificacionReglaTecnicaDTO item in regla)
                                    {
                                        if (item.DirectorDTO.IdEntidad == ((CFuncionarioDTO)usuario[1][0]).IdEntidad)
                                        {
                                            modelF.ReglasTecnicas.Add(item);
                                        }
                                    }
                                    return View(modelF);
                                }
                                else
                                {
                                    modelF.Error = (CErrorDTO)usuario[0][0];
                                    ModelState.AddModelError("Busqueda", modelF.Error.MensajeError);
                                    return PartialView("_ErrorCalificacion", modelF);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "No se pueden registrar la Regla Técnica porque está cerrado el periodo correspondiente");
                                return View("_ErrorCalificacion");
                            }
                        }
                        else
                        {
                            CAccesoWeb.CargarErrorAcceso(Session);
                            return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                        }
                    }
                }
                else
                {
                    modelF.Error = (CErrorDTO)dato;
                    ModelState.AddModelError("Busqueda", modelF.Error.MensajeError);
                    return PartialView("_ErrorCalificacion", modelF);
                }
            }
        }


        [HttpPost]
        public ActionResult ReglaTecnica(FormularioCalificacionVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Subir Archivo")
                {
                    if (model.File != null)
                    {
                        Stream str = model.File.InputStream;
                        BinaryReader Br = new BinaryReader(str);
                        model.ReglasTecnicas[0].ImagenDocumento = Br.ReadBytes((Int32)str.Length);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Agregar", "Debe ingresar una imagen del error");
                        throw new Exception("Agregar");
                    }

                    var respuesta = servicioCalificacion.CargarArchivoReglaTecnica(model.ReglasTecnicas[0]);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return PartialView("_Mensaje", model);
                    }
                    else
                    {
                        ModelState.AddModelError("Agregar", respuesta.Mensaje);
                        model.Error.MensajeError = respuesta.Mensaje;
                        throw new Exception(respuesta.Mensaje);
                    }
                }

                if (SubmitButton == "Enviar Correo")
                {
                    EmailWebHelper correo = new EmailWebHelper();

                    string correoUsuario = "";
                    string salida = "";

                    // Buscar correo de usuario
                    var usuario = servicioUsuario.ObtenerUsuarioPorNombre(principal.Identity.Name);
                    if (usuario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        correoUsuario = ((CUsuarioDTO)usuario[0][0]).EmailOficial;
                    }

                    foreach (var item in model.ReglasTecnicas.Where(Q => Q.IndEstadoDTO == 0))
                    {
                        // Enviar Correo    
                        correo.Asunto = "Evaluación de Desempeño. Informe de Regla Técnica";
                        correo.Destinos = item.EmailDTO + ";" + correoUsuario;
                        correo.EmailBody = ConstruirCorreo(salida, item);
                        correo.EnviarCorreo();

                        //NotificacionesEmailHelper.SendEmail(correo.Destinos, correo.Asunto, correo.EmailBody);
                    }

                    return PartialView("_Mensaje", model);
                }

                throw new Exception("Busqueda");
            }
            catch (Exception error)
            {
                return PartialView("_Mensaje", model);
            }
        }


        private static string ConstruirCorreo(string salida, CCalificacionReglaTecnicaDTO item)
        {
            salida = "Buenas, <br><br>";
            salida += "<table style='  border: solid 1px #e8eef4;border-collapse: collapse;'>";
            salida += "<tr>";
            salida += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Incidencia</th>";
            salida += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Funcionario Emisor</th>";
            salida += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Descripción del Error</th>";
            salida += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Fecha de Inicio</th>";
            salida += "</tr>";
            salida += "<tr>";
            salida += "<td style='padding: 5px;width:40%;'>" + item.DirectorDTO.Nombre + "</td>";
            salida += "<td style='padding: 5px;width:20%;'>La dirección que usted tiene a cargo tiene un resultado igual o mayor al 80 % de funcionarios Evaluados con nota Excelente para el periodo" + item.Periodo.IdEntidad + ", </td>";
            salida += "<td style='padding: 5px;width:20%;'>por lo que es necesario que realice el documento de la Regla Técnica</td>";
            salida += "</tr>";
            salida += "</table><br><br>";
            salida += "Para subir el archivo de la Regla Técnica debe ingresar al Módulo de Evaluación de Desempeño";
            salida += "<br><br>Atentamente,";
            salida += "<br>Unidad de Servicios de Personal.";
            salida += "<br>Dirección de Gestión Institucional de Recursos Humanos.";

            return salida;
        }

        public ActionResult ListFuncionario()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                  Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                  Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), Convert.ToInt32(EAccionesBitacora.Login), 0,
                       CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        [HttpPost]
        public ActionResult ListFuncionario(FormularioCalificacionVM modelo, string cedula, string cedulaJefe,
                                 string coddivision, string coddireccion, string coddepartamento, string codseccion, int? page)
        {
            Session["errorF"] = null;
            try
            {
                //FuncionarioModel modelo = new FuncionarioModel();
                int paginaActual = page.HasValue ? page.Value : 1;


                if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(cedulaJefe) && String.IsNullOrEmpty(coddivision) &&
                    String.IsNullOrEmpty(coddireccion) && String.IsNullOrEmpty(coddepartamento) && String.IsNullOrEmpty(codseccion))
                {
                    page = 1;
                    return View();
                }
                else
                {
                    if (cedula == null)
                    {
                        cedula = "";
                    }

                    string[] division = coddivision != "" ? coddivision.Split('-') : new string[] { "0" };
                    string[] direccion = coddireccion != "" ? coddireccion.Split('-') : new string[] { "0" };
                    string[] departamento = coddepartamento != "" ? coddepartamento.Split('-') : new string[] { "0" };
                    string[] seccion = codseccion != "" ? codseccion.Split('-') : new string[] { "0" };

                    int periodo = DateTime.Now.AddYears(-1).Year;
                    modelo.Periodos = periodo.ToString();
                    var datos = servicioCalificacion.ListarFuncionarios(periodo, cedula, cedulaJefe, "", Convert.ToInt32(seccion[0]), Convert.ToInt32(direccion[0]), Convert.ToInt32(division[0]));

                    modelo.FuncionariosCalificar = new List<CCalificacionNombramientoFuncionarioDTO>();
                    foreach (var item in datos)
                    {
                        modelo.FuncionariosCalificar.Add((CCalificacionNombramientoFuncionarioDTO)item);
                    }

                    //modelo.TotalFuncionarios = funcionarios.Count();
                    //modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                    //modelo.PaginaActual = paginaActual;
                    //Session["funcionarios"] = funcionarios.ToList();
                    //if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                    //{
                    //    modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                    //}
                    //else
                    //{
                    //    modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    //}


                    return PartialView("_ListFuncionario", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n" + error.ToString());
                return PartialView("_ErrorFuncionario");
            }
        }

        public ActionResult ListCalificacion()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                  Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                  Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), Convert.ToInt32(EAccionesBitacora.Login), 0,
                       CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        [HttpPost]
        public ActionResult ListCalificacion(FormularioCalificacionVM modelo, string cedula, string cedulaJefe, int? page, string SubmitButton)
        {
            Session["errorF"] = null;
            try
            {

                if (SubmitButton == "Buscar")
                {
                    //FuncionarioModel modelo = new FuncionarioModel();
                    int paginaActual = page.HasValue ? page.Value : 1;


                    if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(cedulaJefe))
                    {
                        page = 1;
                        return View();
                    }
                    else
                    {
                        if (cedula == null)
                        {
                            cedula = "";
                        }

                        int periodo = DateTime.Now.AddYears(-1).Year;
                        modelo.Periodos = periodo.ToString();
                        //var datos = servicioCalificacion.ListarFuncionarios(periodo, cedula, cedulaJefe, "",0, 0,0);

                        var datos = servicioCalificacion.ListarCalificacionesJefatura(periodo, cedulaJefe, cedula);

                        modelo.CNombramientoB = new List<CCalificacionNombramientoDTO>();
                        foreach (var item in datos)
                        {
                            modelo.CNombramientoB.Add((CCalificacionNombramientoDTO)item);
                        }

                        return PartialView("_ListCalificacion", modelo);
                    }
                }
                else
                {
                    var dato = "";

                    // guardar datos
                    foreach (var item in modelo.CNombramientoB)
                    {
                        dato += item.IdEntidad.ToString() + " " + item.IndEntregadoDTO.ToString() + " " + item.IndConformidadDTO.ToString() + "/";
                        var respuesta = servicioCalificacion.RecibirCalificacionNombramiento(item.IdEntidad, item.IndEntregadoDTO, item.IndConformidadDTO);

                        if (respuesta.GetType() == typeof(CErrorDTO))                        
                        {
                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                            modelo.Error.MensajeError = respuesta.Mensaje;
                            throw new Exception(respuesta.Mensaje);
                        }
                    }

                    return PartialView("_Mensaje", modelo);
                }

            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n" + error.ToString());
                return PartialView("_Mensaje", modelo);
            }
        }


        public ActionResult DatosEvaluacion()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                  Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
                  Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.Administrador))] != null)
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), Convert.ToInt32(EAccionesBitacora.Login), 0,
                       CAccesoWeb.ListarEntidades(typeof(CCalificacionDTO).Name));
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
                }
            }
        }

        public PartialViewResult Jefatura_Index(string cedula, string nombre, string apellido1, string apellido2, int? page)
        {
            Session["errorU"] = null;
            try
            {
                FuncionarioModel modelo = new FuncionarioModel();
                int periodo = DateTime.Now.AddYears(-1).Year;

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(apellido1) && String.IsNullOrEmpty(apellido2))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CedulaSearch = cedula;
                    modelo.NombreSearch = nombre;
                    modelo.PrimerApellidoSearch = apellido1;
                    modelo.SegundoApellidoSearch = apellido2;

                    List<CFuncionarioDTO> lista = new List<CFuncionarioDTO>();

                    //modelo.Grupo = periodo.ToString();
                    //modelo.PrimerApellidoSearch = idFuncionario;

                    //var datos = servicioCalificacion.ListarJefaturas(periodo);

                    //foreach (var item in datos.ToList())
                    //{
                    //    var func = ((CCalificacionNombramientoFuncionarioDTO)item).Funcionario;

                    //    if (cedulaJefe != "" && func.Cedula.Equals(cedulaJefe))
                    //    {
                    //        lista.Add(func);
                    //    }
                    //    else
                    //    {
                    //        if (nomJefe != null && func.Nombre.ToUpper().Contains(nomJefe.ToUpper()))
                    //        {
                    //            lista.Add(func);
                    //        }
                    //    }
                    //}

                    var datos = servicioFuncionario.BuscarFuncionarioParam(cedula, nombre, apellido1, apellido2);
                    foreach (var item in datos.ToList())
                    {
                        item.Nombre = item.Nombre.TrimEnd() + " " + item.PrimerApellido.TrimEnd() + " " + item.SegundoApellido.TrimEnd();
                        lista.Add(item);
                    }

                    modelo.TotalFuncionarios = lista.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                    {
                        modelo.Funcionario = lista.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Funcionario = lista.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Jefatura_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorJefatura");
            }
        }


        public PartialViewResult Director_Index(string cedula, string nombre, string apellido1, string apellido2, string correo, int? page)
        {
            Session["errorU"] = null;
            try
            {
                FuncionarioModel modelo = new FuncionarioModel();
                int periodo = DateTime.Now.AddYears(-1).Year;

                int paginaActual = page.HasValue ? page.Value : 1;
                
                if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(apellido1) && String.IsNullOrEmpty(apellido2))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CedulaSearch = cedula;
                    modelo.NombreSearch = nombre;
                    modelo.PrimerApellidoSearch = apellido1;
                    modelo.SegundoApellidoSearch = apellido2;
                    modelo.Grupo = correo;

                    List<CFuncionarioDTO> lista = new List<CFuncionarioDTO>();

                    var datos = servicioFuncionario.BuscarFuncionarioParam(cedula, nombre, apellido1, apellido2);
                    foreach (var item in datos.ToList())
                    {
                        item.Nombre = item.Nombre.TrimEnd() + " " + item.PrimerApellido.TrimEnd() + " " + item.SegundoApellido.TrimEnd();
                        lista.Add(item);
                    }

                    modelo.TotalFuncionarios = lista.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                    {
                        modelo.Funcionario = lista.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Funcionario = lista.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Director_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorJefatura");
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDatos()
        {
            FormularioDatosEvaluacionVM model = new FormularioDatosEvaluacionVM();

            int periodo = DateTime.Now.AddYears(-1).Year;

            var resultado = servicioCalificacion.ObtenerDatosEvaluacion(periodo);

            //var resultadoDE = servicioCalificacion.DescargarDatosGEvaluacion();
            model.DatosGeneralesEvaluacion = new List<CProcAlmacenadoDatosGeneralesDTO>();
            model.DatosEvaluacionCC = new List<CProcAlmacenadoDTO>();

            if (resultado != null)
            {
                foreach (var itemDE in resultado[0])
                {
                    model.DatosGeneralesEvaluacion.Add((CProcAlmacenadoDatosGeneralesDTO)itemDE);
                }
                foreach (var item in resultado[1])
                {
                    model.DatosEvaluacionCC.Add((CProcAlmacenadoDTO)item);
                }
            }
            List<DatosMEDRptDat> modelo = new List<DatosMEDRptDat>();

            modelo.Add(DatosMEDRptDat.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Calificacion"), "DatosCuantitativosCualitativosFEDRSCRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleCalificacion(FormularioCalificacionVM model)
        {
            model.PuntuacionFinal = 0;
            model.Periodos = model.CalificacionNombramiento.Periodo.IdEntidad.ToString();
            model.NombreFormulario = ObtenerNombreFormulario(model.CalificacionNombramiento.DetalleCalificacion[0].CatalogoPreguntaDTO.IndTipoFormularioDTO);
            model.CalificacionFinalLetra = "";
            if (model.CalificacionNombramiento.FecCreacionDTO.Year == 1)
                model.CalificacionNombramiento.FecCreacionDTO = DateTime.Today;

            if (model.CalificacionNombramiento.JefeInmediato.Nombre != null)
            {
                model.CalificacionNombramiento.JefeInmediato.Nombre = model.CalificacionNombramiento.JefeInmediato.Cedula.TrimEnd() + " - " + 
                                                                     model.CalificacionNombramiento.JefeInmediato.Nombre.TrimEnd() + " " +
                                                                    (model.CalificacionNombramiento.JefeInmediato.PrimerApellido != null ? model.CalificacionNombramiento.JefeInmediato.PrimerApellido.TrimEnd() : "") + " " +
                                                                    (model.CalificacionNombramiento.JefeInmediato.SegundoApellido != null ? model.CalificacionNombramiento.JefeInmediato.SegundoApellido.TrimEnd() : "");
            }
            else
            {
                model.CalificacionNombramiento.JefeInmediato.Nombre = "";
            }


            if (model.CalificacionNombramiento.JefeSuperior.Nombre != null)
            {
                model.CalificacionNombramiento.JefeSuperior.Nombre = model.CalificacionNombramiento.JefeSuperior.Cedula.TrimEnd() + " - " +
                                                                     model.CalificacionNombramiento.JefeSuperior.Nombre.TrimEnd() + " " +
                                                                    (model.CalificacionNombramiento.JefeSuperior.PrimerApellido != null ? model.CalificacionNombramiento.JefeSuperior.PrimerApellido.TrimEnd() : "") + " " +
                                                                    (model.CalificacionNombramiento.JefeSuperior.SegundoApellido != null ? model.CalificacionNombramiento.JefeSuperior.SegundoApellido.TrimEnd() : "");
            }
            else
            {
                model.CalificacionNombramiento.JefeSuperior.Nombre = "";
            }


            for (int i = 0; i < model.Detalle.Count; i++)
            {
                model.Detalle[i].CatalogoPreguntaDTO = model.CalificacionNombramiento.DetalleCalificacion[i].CatalogoPreguntaDTO;
                if (model.CalificacionNombramiento.DetalleCalificacion[i].NumNotasPorPreguntaDTO != "0")
                    model.Detalle[i].NumNotasPorPreguntaDTO = model.CalificacionNombramiento.DetalleCalificacion[i].NumNotasPorPreguntaDTO;
                else
                    model.Detalle[i].NumNotasPorPreguntaDTO = model.Detalle[i].NumNotasPorPreguntaDTO != null ? model.Detalle[i].NumNotasPorPreguntaDTO : model.CalificacionNombramiento.DetalleCalificacion[i].NumNotasPorPreguntaDTO;

                model.PuntuacionFinal += decimal.Parse(model.Detalle[i].NumNotasPorPreguntaDTO.Replace(",", "."));
            }

            model.CalificacionFinal = model.CalificacionNombramiento.CalificacionDTO.IdEntidad;
            model.CalificacionFinalLetra = model.CalificacionNombramiento.CalificacionDTO.DesCalificacion;

            List<CalificacionRptData> modelo = new List<CalificacionRptData>();

            modelo.Add(CalificacionRptData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Calificacion"), "CalificacionRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleHistorial(BusquedaHistorialCalificacionVM model)
        {
            List<HistorialRptDat> modelo = new List<HistorialRptDat>();

            modelo.Add(HistorialRptDat.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Calificacion"), "HistorialDetalleRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public ActionResult AsignarJefatura(int idPeriodo, int idFuncionario, int idJefatura)
        {
            try
            {
                if (idPeriodo != null)
                {
                    var respuesta = servicioCalificacion.AsignarJefatura(idPeriodo, idFuncionario, idJefatura);
                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, idEntidad = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AsignarDirector(int idRegla, int idJefatura, string correo)
        {
            try
            {
                CCalificacionReglaTecnicaDTO regla = new CCalificacionReglaTecnicaDTO {
                    IdEntidad = idRegla,
                    DirectorDTO = new CFuncionarioDTO { IdEntidad = idJefatura },
                    EmailDTO = correo
                };

                var respuesta = servicioCalificacion.AsignarDirectorReglaTecnica(regla);
                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, idEntidad = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private string ObtenerNombreFormulario(int codigo)
        {
            var nombre = "";
            switch (codigo)
            {
                case 1:
                    nombre = "FED-1 GRUPO LABORAL A: JEFATURAS";
                    break;
                case 2:
                    nombre = "FED-2 GRUPO LABORAL B: PROFESIONAL";
                    break;
                case 3:
                    nombre = "FED-3 GRUPO LABORAL C: TÉCNICO";
                    break;
                case 4:
                    nombre = "FED-4 GRUPO LABORAL D: CALIFICADO";
                    break;
                case 5:
                    nombre = "FED-5 GRUPO LABORAL E: OPERATIVO";
                    break;
            }
            return nombre;
        }

        public ActionResult EvaluacionFueraDominio()
        {
            return View();
            //context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Calificacion), 0);


            //if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Calificacion)].ToString().StartsWith("Error"))
            //{
            //    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            //}
            //else
            //{
            //    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
            //        Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Calificacion)]) ||
            //        Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Calificacion, Convert.ToInt32(ENivelesCalificacion.FueraDominio))] != null)
            //    {
            //        return View();
            //    }
            //    else
            //    {
            //        CAccesoWeb.CargarErrorAcceso(Session);
            //        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Calificacion) });
            //    }
            //}
        }

        [HttpPost]
        public ActionResult EvaluacionFueraDominio(string ced, string contra)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Helpers", "fueraDominio.xml");
                XmlTextReader reader = new XmlTextReader(path);
                string elemento = "";
                string datoCedula = "";
                string datoContra = "";
                bool encontrado = false;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            elemento = reader.Name;
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (elemento == "cedula")
                            {
                                datoCedula = reader.Value;
                            }
                            else
                            {
                                datoContra = reader.Value;
                            }
                            break;
                        default:
                            break;
                    }
                    if (elemento == "contra")
                    {
                        if (datoContra != "")
                        {
                            if (datoCedula == ced && datoContra == contra)
                            {
                                encontrado = true;
                                break;
                            }
                            else
                            {
                                datoContra = "";
                                datoCedula = "";
                            }
                        }
                    }
                }
                if (encontrado)
                {
                    return JavaScript("window.location = '/Calificacion/Calificar?cedula=" + ced + "'");
                }
                else
                {
                    ModelState.AddModelError("", "No se encontró el usuario y contraseña indicados, por favor revise sus datos de inicio de sesión");
                    return PartialView("_ErrorCalificacion");
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("", error.Message + " " + error.InnerException);
                return PartialView("_ErrorCalificacion");
            }
        }


        private FormularioCalificacionVM ObtenerCalificacion(int codigo)
        {
            FormularioCalificacionVM modelB = new FormularioCalificacionVM();

            var datos = servicioCalificacion.ObtenerCalificacion(codigo);

            if (datos.Count() > 1)
            {
                modelB.CalificacionNombramiento = (CCalificacionNombramientoDTO)datos.ElementAt(0);
                if (modelB.CalificacionNombramiento.DetalleCalificacionModificado == null)
                    modelB.CalificacionNombramiento.DetalleCalificacionModificado = new List<CDetalleCalificacionNombramientoDTO>();

                modelB.Funcionario = (CFuncionarioDTO)datos.ElementAt(1);
                modelB.Puesto = (CPuestoDTO)datos.ElementAt(2);
                modelB.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(3);
                modelB.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
                if (modelB.CalificacionNombramiento.DetalleCalificacionModificado.Count() == 0)
                {
                    modelB.PuntuacionFinal = modelB.CalificacionNombramiento.DetalleCalificacion.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
                    modelB.CalificacionFinalLetra = modelB.CalificacionNombramiento.CalificacionDTO.DesCalificacion;
                }
                else
                {
                    int idCalificacion = 0;
                    string DesCalificacion = "";
                    modelB.PuntuacionFinal = modelB.CalificacionNombramiento.DetalleCalificacionModificado.Sum(Q => decimal.Parse(Q.NumNotasPorPreguntaDTO.Replace(",", ".")));
                    ObtenerDetalleCalificacion(modelB.PuntuacionFinal, out idCalificacion, out DesCalificacion);
                    modelB.CalificacionFinalLetra = DesCalificacion;
                }

                // Verificar que está dentro de las fechas para anular las Calificaciones
                DateTime Fecha = DateTime.Today;
                int periodo = Fecha.AddYears(-1).Year;
                if (Fecha.Date >= modelB.CalificacionNombramiento.Periodo.FecRige.Date && Fecha.Date <= Convert.ToDateTime(modelB.CalificacionNombramiento.Periodo.FecVence).Date)
                {
                    modelB.EsAnulable = true;

                    // Verificar que no está entregado el formulario
                    if (modelB.CalificacionNombramiento.IndEntregadoDTO)
                        modelB.EsAnulable = false;
                }
                else
                {
                    modelB.EsAnulable = false;
                }
            }
            else
            {
                modelB.Error = (CErrorDTO)datos.ElementAt(0);
            }

            return modelB;
        }


        [HttpGet]
        public ActionResult DetallesReglaTecnica(int id)
        {
            FormularioCalificacionVM model = new FormularioCalificacionVM();
            model.ReglasTecnicas = new List<CCalificacionReglaTecnicaDTO>();
            var datos = servicioCalificacion.ObtenerReglaTecnica(id);
            model.ReglasTecnicas.Add((CCalificacionReglaTecnicaDTO)datos.ElementAt(0));
            return PartialView("DetailsReglaTecnica", model);
        }

        public ActionResult GetImage(int id)
        {
            var datos = servicioCalificacion.ObtenerReglaTecnica(id);
            var image = ((CCalificacionReglaTecnicaDTO)datos.ElementAt(0)).ImagenDocumento;
            return File(image, "application/pdf");
        }

        private void ObtenerDetalleCalificacion(decimal suma, out int idCalificacion, out string DesCalificacion)
        {
            idCalificacion = 0;
            DesCalificacion = "";

            if (suma >= 95 && suma <= 100)
            {
                DesCalificacion = "Excelente";
                idCalificacion = 1;
            }
            else if (suma >= 85 && suma < 95)
            {
                DesCalificacion = "Muy Bueno";
                idCalificacion = 2;
            }
            else if (suma >= 75 && suma < 85)
            {
                DesCalificacion = "Bueno";
                idCalificacion = 3;
            }
            else if (suma < 75)
            {
                DesCalificacion = "Regular";
                idCalificacion = 4;
            }
            else if (suma == 0)
            {
                DesCalificacion = "Deficiente";
                idCalificacion = 5;
            }
            #endregion
        }
    }
}