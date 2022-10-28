using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using SIRH.DTO;
using SIRH.Web.FuncionarioLocal;
using SIRH.Web.Helpers;
using SIRH.Web.MarcacionesLocal;
//using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.PerfilUsuarioLocal;
using SIRH.Web.Reports.PDF;
using SIRH.Web.ViewModels;
using System.IO;
using SIRH.Web.Reports.RelojMarcador;


namespace SIRH.Web.Controllers
{
    public class MarcasAsistenciaController : Controller
    {

        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CMarcacionServiceClient servicioMarcasAsistencia = new CMarcacionServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CPerfilUsuarioServiceClient perfilUsuario = new CPerfilUsuarioServiceClient();

        /// <summary>
        /// Metodo variable de FuncionReport
        /// </summary>
        /// <returns>Un reporte o un error</returns>
        private delegate CrystalReportPdfResult FunctionReportDel(FormularioReporteMarcas mo, CTipoJornadaDTO t, CFuncionarioDTO f,
        CDetalleContratacionDTO d, CUbicacionAdministrativaDTO u, List<List<CMarcacionesDTO>> m);

        /// <summary>
        /// Identifica los permisos del usuario logeado en el sistema
        /// </summary>
        /// <returns>Retorna true si el usuario puede acceder, de lo contrario retorna false</returns>
        private object AccesoEsPermitido(int permiso, int perfil)
        {
            if (Session["Iniciada"] == null)
            {
                Session["Iniciada"] = true;
            }

            if (Session["Marcas"] == null)
            {
                Session["Marcas"] = true;
                var principal = WindowsIdentity.GetCurrent();
                //WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;

                string resultado = GestionUsuariosHelper.UsuarioPermitido(servicioUsuario,
                                                principal.Name, permiso, perfil);

                if (resultado == "Denegado")
                {
                    Session["Perfil_Marcas"] = "Denegado";
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                }
                else
                {
                    if (resultado.StartsWith("Error"))
                    {
                        Session["Perfil_Marcas"] = "Error";
                        return RedirectToAction("Index", "Error", new { errorType = resultado, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                        }
                    else
                    {
                        var permisos = servicioUsuario.CargarPerfilUsuarioCompleto(principal.Name, permiso, perfil);
                        if (permisos == null)
                            throw new Exception("Error al cargar el perfil de usuario completo, favor contactar al personal encargado.");
                        
                        if(permisos.FirstOrDefault() == null || permisos.FirstOrDefault().FirstOrDefault() == null)
                            throw new Exception("Error al cargar el perfil de usuario completo, favor contactar al personal encargado.");

                        if (permisos.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            Session["Perfil_Marcas"] = "Autorizado";

                            Session["Nombre_Usuario"] = principal.Name;

                            Session["Nombre_Completo"] = ((CFuncionarioDTO)permisos.FirstOrDefault().FirstOrDefault()).Nombre + " "
                                + ((CFuncionarioDTO)permisos.FirstOrDefault().FirstOrDefault()).PrimerApellido + " " +
                                ((CFuncionarioDTO)permisos.FirstOrDefault().FirstOrDefault()).SegundoApellido;

                            var perfiles = permisos[1];

                            Session["Administrador_Global"] = perfiles.Where(Q => Q.GetType() == typeof(CPerfilDTO))
                                                        .ToList().Where(Q => Q.IdEntidad == 1)
                                                        .ToList().Count > 0 ? true : false;

                            if (Convert.ToBoolean(Session["Administrador_Global"]) == false)
                            {
                                var uno = perfiles.Where(Q => Q.GetType() == typeof(CCatPermisoDTO))
                                                                        .ToList();
                                var dos = uno.Where(Q => ((CCatPermisoDTO)Q).NomPermiso.StartsWith("Administrador"))
                                                                       .ToList();
                                Session["Administrador_Marcas"] = perfiles.Where(Q => Q.GetType() == typeof(CCatPermisoDTO))
                                                                        .ToList().Where(Q => ((CCatPermisoDTO)Q).NomPermiso.StartsWith("Administrador"))
                                                                       .ToList().Count > 0 ? true : false;

                                if (Convert.ToBoolean(Session["Administrador_Marcas"]) == false)
                                {
                                    var restantes = perfiles.Where(Q => Q.GetType() == typeof(CCatPermisoDTO))
                                                                .ToList();

                                    foreach (var item in restantes)
                                    {
                                        if (item.GetType() != typeof(CPerfilDTO))
                                        {
                                            Session["Permiso_Marcas_" + ((CCatPermisoDTO)item).NomPermiso.Trim()] = ((CCatPermisoDTO)item).NomPermiso;
                                            Session["Descripcion_Permiso_Marcas_" + ((CCatPermisoDTO)item).NomPermiso.Trim()] = ((CCatPermisoDTO)item).DesPermiso;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Session["Perfil_Marcas"] = "Error";
                            return RedirectToAction("Index", "Error", new { errorType = ((CErrorDTO)permisos.FirstOrDefault().FirstOrDefault()).MensajeError, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                        }

                        return true;
                    }
                }
            }
            else
            {
                if (Session["Perfil_Marcas"].ToString() == "Autorizado")
                {
                    return true;
                }
                else
                {
                    if (Session["Perfil_Marcas"].ToString() == "Denegado")
                    {
                        return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { errorType = "sistema", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                    }
                }
            }
        }

        /// <summary>
        /// Carga la página principal del módulo
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Index
        /// </example>
        /// <returns>Retorna la vista principal</returns>
        public ActionResult Index()
        {
            try
            {
                var acceso = AccesoEsPermitido(0, 7);
                if (acceso != null)
                {
                    if (acceso.GetType() != typeof(bool))
                    {
                        return (RedirectToRouteResult)acceso;
                    }
                    else
                    {
                        return View();
                    }
                }else
                {
                    throw new Exception("Ha ocurrido un error a la hora de obtener el acceso, contacte el personal autorizado.");
                }
                
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "Error", new { errorType= error.Message , modulo = Convert.ToInt32(EModulosHelper.Marcas) });
            }
            
        }

        //-------Registrar funcionario
        /// <summary>
        /// Carga la interfaz para registrar un funcionario
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Create
        /// </example>
        /// <returns>Retorna la vista de Create</returns>
        public ActionResult Create()
        {
            try
            {
                var acceso = AccesoEsPermitido(0, 7);

                if(acceso != null)
                {
                    if (acceso.GetType() != typeof(bool))
                    {
                        return (RedirectToRouteResult)acceso;
                    }
                    else
                    {
                        if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                            Convert.ToBoolean(Session["Administrador_Marcas"]) ||
                            Session["Permiso_Marcas_Operativo"] != null)
                        {
                            FormularioMarcasAsistenciaVM model = new FormularioMarcasAsistenciaVM();

                            return View();
                        }
                        else
                        {
                            return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                        }
                    }
                }else
                {
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
            }
        }

        /// <summary>
        /// Realiza la búsqueda del funcionario y lo registra
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Create
        /// </example>
        ///  <param name="model">View Model asociado</param>
        ///  <param name="SubmitButton">Nombre del botón de submit</param>
        /// <returns>Retorna la vista de detalles</returns>
        [HttpPost]
        public ActionResult Create(FormularioMarcasAsistenciaVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        CEmpleadoDTO empleadoEntidad = new CEmpleadoDTO
                        {
                            CodigoEmpleado = ConvertirCedula(model.Funcionario.Cedula)
                        };

                        var empleado = servicioMarcasAsistencia.BuscarEmpleadoActivo(empleadoEntidad);

                        if (empleado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            ModelState.AddModelError("Busqueda", "El empleado ya está registrado en la base de datos del reloj marcador");
                            throw new Exception("Busqueda");
                        }
                        else
                        {
                            var datosFuncionario =
                                        servicioMarcasAsistencia.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                            if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];

                                var detallePuesto = servicioMarcasAsistencia.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                                if (detallePuesto.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    model.DetallePuesto = (CDetallePuestoDTO)detallePuesto.ElementAt(2);

                                    var jornadaLaboral = servicioMarcasAsistencia.ObtenerJornadaFuncionario(model.Funcionario);
                                    if (jornadaLaboral.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {
                                        model.JornadaLaboral = (CTipoJornadaDTO)jornadaLaboral.ElementAt(0);

                                        var dispositivos = servicioMarcasAsistencia.ListarDispositivos();

                                        if (dispositivos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                        {
                                            model.ListaDispositivos = new List<CDispositivoDTO>();

                                            foreach (var dispositivo in dispositivos)
                                            {
                                                model.ListaDispositivos.Add((CDispositivoDTO)dispositivo);
                                            }
                                            Session["dispositivos"] = model.ListaDispositivos;
                                            return PartialView("_FormularioMarcasAsistencia", model);
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("Busqueda", "No se pudo obtener el catalogo de dispositivos");
                                            throw new Exception("Busqueda");
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Busqueda", "El funcionario no tiene una jornada laboral asignada");
                                        throw new Exception("Busqueda");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("Busqueda", "El funcionario no tiene un puesto asignado");
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
                        List<CDispositivoDTO> list = (List < CDispositivoDTO >) Session["dispositivos"];

                        if (model.ListaDispositivos != null)
                        {
                            if (model.FuncionarioAux.Count > 0)
                            {
                                model.FuncionarioAux.ElementAt(0).Sexo = SIRH.DTO.GeneroEnum.Masculino;
                                model.Funcionario = model.FuncionarioAux.ElementAt(0);
                                CEmpleadoDTO empleado = new CEmpleadoDTO
                                {
                                    PrimerNombre = model.Funcionario.Nombre,
                                    SegundoNombre = "",
                                    ApellidoMaterno = model.Funcionario.SegundoApellido,
                                    ApellidoPaterno = model.Funcionario.PrimerApellido,
                                    CodigoEmpleado = model.Funcionario.Cedula
                                };

                                CFuncionarioDTO funcionario = model.Funcionario;

                                var respuesta1 = servicioMarcasAsistencia.AgregarEmpleado(empleado, funcionario);

                                if (respuesta1.GetType() != typeof(CErrorDTO))
                                {
                                    var empleadoEntidad = (CEmpleadoDTO)respuesta1;


                                    foreach (var disp in model.ListaDispositivos)
                                    {

                                        CDispositivoDTO dispositivo = new CDispositivoDTO
                                        {
                                            IdEntidad = disp.IdEntidad
                                        };

                                        CEmpleadoDispositivoDTO empleadoDispositivo = new CEmpleadoDispositivoDTO
                                        {
                                            Digitos = empleado.Digitos
                                        };

                                        var respuesta2 = servicioMarcasAsistencia.AsignarRelojMarcador(empleadoDispositivo, empleadoEntidad, dispositivo);
                                        if (respuesta2.GetType() == typeof(CErrorDTO))
                                        {
                                            ModelState.AddModelError("Agregar", ((CErrorDTO)respuesta2).MensajeError + "");
                                            throw new Exception();
                                        }
                                    }
                                    
                                    return JavaScript("window.location = '/MarcasAsistencia/Details?codigo=" + model.Funcionario.Cedula + "&accion=guardar" + "'");
                                }
                                else
                                {
                                    ModelState.AddModelError("Agregar", ((CErrorDTO)respuesta1).MensajeError + "");
                                    throw new Exception();
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("Agregar", "Hubo un error al obtener el funcionario registrado en S.I.R.H.");
                                throw new Exception();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", "Debe asignar al menos un reloj marcador al funcionario");
                            throw new Exception();
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception error)
            {

                return PartialView("_ErrorMarcasAsistencia");

            }
        }

        /// <summary>
        /// arregla datos inconsistentes del formulario de reportes
        /// </summary>
        /// <param name="model">El formulariode reportes</param>
        private void ArreglarFormulario(FormularioReporteMarcas model)
        {
            model.Funcionario.Sexo = GeneroEnum.Indefinido;
            model.FechaF = model.FechaF.Year > 1 ? model.FechaF : model.FechaI;
        }

        /// <summary>
        /// Genera el resumen de un reporte de marcas consolidadas
        /// </summary>
        /// <param name="ausT">ausencias totales</param>
        /// <param name="ausM">ausencias media jornada</param>
        /// <param name="tar05">tardias de 5 min</param>
        /// <param name="tar20">tardias de 20 min</param>
        /// <param name="laborado">total de horas laboradas</param>
        /// <param name="fechaF">fecha final del reporte</param>
        /// <param name="model">La lista de marcaciones(Reportes)</param>
        private void SummaryConsolidadoFuncionario(out int ausT, out int ausM, out int tar05, out int tar20, out TimeSpan laborado, DateTime fechaF, List<ConsolidadoFuncionarioRptData> model)
        {
            laborado = new TimeSpan(0, 0, 0);
            ausT = 0; ausM = 0; tar05 = 0; tar20 = 0;
            foreach (var m in model)
            {
                if (m.Laborado != "xx:xx:xx*" && m.Laborado != "xx:xx:xx")
                    laborado = laborado + Convert.ToDateTime(m.Laborado).TimeOfDay;
                if (m.Falta == "AU-T")
                    ausT++;
                if (m.Falta == "AU-M")
                    ausM++;
                if (m.Falta == "T-05")
                    tar05++;
                if (m.Falta == "T-20")
                    tar20++;
            }
        }

        private CrystalReportPdfResult FunctionReport(FormularioReporteMarcas model, string mode, FunctionReportDel fun)
        {
            try
            {
                ArreglarFormulario(model);
                CBaseDTO[][] datos;
                if (mode == "MC")
                    datos = this.servicioMarcasAsistencia.ReporteMarcacionesPorDia(new[] { model.FechaI, model.FechaF }, model.Funcionario);
                else
                    datos = this.servicioMarcasAsistencia.ReporteConsolidadoPorFuncionario(model.Funcionario, new[] { model.FechaI, model.FechaF });
                //datos[0] = [CFuncionarioDTO?,CDetalleContratacionDTO,CTipoJornadaDTO?,CUbicacionAdministrativaDTO]
                // el signo ? es que puede ser un CErrorDTO
                if (datos != null) {
                    if (datos[0][0].GetType() != typeof(CErrorDTO))
                    {
                        var marcas = datos[1][0].GetType() != typeof(CErrorDTO) ? datos.Skip(1).Select(L => L.Select(M => (CMarcacionesDTO)M).ToList()).ToList() : null;
                        var jornada = datos[0][2].GetType() == typeof(CErrorDTO) ? null : (CTipoJornadaDTO)datos[0][2];
                        return fun(model, jornada, (CFuncionarioDTO)datos[0][0], (CDetalleContratacionDTO)datos[0][1], datos[0][3] == null ? null : (CUbicacionAdministrativaDTO)datos[0][3], marcas);
                    }
                    else throw new Exception(((CErrorDTO)datos[0][0]).MensajeError); //No se encontro el funcionario (Se muestra un error, no hay reporte)
                }else
                {
                    throw new Exception("Ha ocurrido un error a la hora de obtener el reporte, favor contactar el personal autorizado.");
                }
            }
            catch (Exception error)
            {
                Session["ErrorMessage"] = error.Message;
                return null;
            }
        }

        /// <summary>
        /// Muestra la vista de generar reportes
        /// </summary>
        /// <example>GET: /ReportesMarcasAsistencia</example>
        /// <returns>La vista de generar reportes o un error en caso de que nose tenga permisos</returns>
        public ActionResult ReportesMarcasAsistencia()
        {
            try
            {
                var acceso = AccesoEsPermitido(0, 7);
                if (acceso.GetType() != typeof(bool))
                {
                    return (RedirectToRouteResult)acceso;
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_Marcas"]) ||
                        Session["Permiso_Marcas_Operativo"] != null ||
                         Session["Permiso_Marcas_Consulta"] != null)
                    {
                        FormularioReporteMarcas model = new FormularioReporteMarcas();
                        var departamentos = servicioMarcasAsistencia.ListarDepartamentos().ToList();
                        model.Departamentos = new SelectList(departamentos.Select(D => new SelectListItem
                        {
                            Text = ((CDepartamentoDTO)D).NomDepartamento,
                            Value = ((CDepartamentoDTO)D).IdEntidad.ToString()
                        }), "Value", "Text");
                        model.FechaI = DateTime.Today;
                        model.FechaF = DateTime.Today.AddDays(1);
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "marcasAsistencia" });
                    }
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
            }
        }

        /// <summary>
        /// Devuelve el reporte correspondiente
        /// </summary>
        /// <example>POST: /ReportesMarcasAsistencia</example>
        /// <param name="model">El formulario de reportes</param>
        /// <returns>Un reporte o un error</returns>
        [HttpPost]
        public ActionResult ReportesMarcasAsistencia(FormularioReporteMarcas model)
        {
            try
            {

                ModelState.Clear();
                switch (model.TipoReporte)
                {
                    case "MC":
                        var resultado = FunctionReport(model, "MC", this.CrearReporteMarcasCompletas);
                        if (resultado != null)
                            return resultado;
                        else
                            throw new Exception((string)Session["ErrorMessage"]);
                    case "CF":
                        resultado =  FunctionReport(model, "CF", this.CrearReporteConsolidadoPorFuncionario);
                        if (resultado != null)
                            return resultado;
                        else
                            throw new Exception((string)Session["ErrorMessage"]);
                    case "CD":
                        resultado = ReporteConsolidadoPorDepartamento(model);
                        if (resultado != null)
                            return resultado;
                        else
                            throw new Exception((string)Session["ErrorMessage"]);
                    case "CDP":
                        resultado = ReporteFuncionariosPorDepartamentos(model);
                        if (resultado != null)
                            return resultado;
                        else
                            throw new Exception((string)Session["ErrorMessage"]);

                    default: throw new Exception("El tipo del reporte es obligatorio");
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("ErrorGeneral", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Marcas) });

            }
        }

        /// <summary>
        /// Crea un reporte de todas las marcas de un funcionario
        /// </summary>
        /// <param name="jornada">La jornada del funcionario</param>
        /// <param name="marcas">Las marcas del funcionario (Lista por dia)</param>
        /// <returns>Retorna un reporte</returns>
        private CrystalReportPdfResult CrearReporteMarcasCompletas(FormularioReporteMarcas model, CTipoJornadaDTO jornada,
                                                                   CFuncionarioDTO funcionario, CDetalleContratacionDTO detalle,
                                                                   CUbicacionAdministrativaDTO ubicacion, List<List<CMarcacionesDTO>> marcas)
        {
            try
            {
                List<ReporteEncabezadoRptData> modelo1 = new List<ReporteEncabezadoRptData>();
                List<MarcasCompletasRptData> modelo2 = new List<MarcasCompletasRptData>();
                modelo1.Add(ReporteEncabezadoRptData.GenerarDatosReporteAsistencia(model, funcionario, jornada, detalle, ubicacion, "", 0, 0, 0, 0, ""));
                if (marcas != null)
                {
                    marcas.Add(new List<CMarcacionesDTO> { new CMarcacionesDTO { FechaHoraMarca = model.FechaF.AddDays(2) } });
                    foreach (var elem in marcas)
                    {
                        for (; model.FechaI.Date != elem[0].FechaHoraMarca.Date && model.FechaI.Date <= model.FechaF.Date; model.FechaI = model.FechaI.AddDays(1))
                            if (model.MostarSinMarcas)
                                modelo2.Add(MarcasCompletasRptData.GenerarDatosReporteAsistenciaSinMarca(model.FechaI, true));
                        if (elem[0].FechaHoraMarca.Date == model.FechaI.Date)
                        {
                            modelo2.Add(MarcasCompletasRptData.GenerarDatosReporteAsistencia(elem.Take(13).ToList()));
                            if (elem.Count > 13)
                            {//Las marcas sobrepasan una tabla del reporte (Muy poco probable)
                                var aux = elem.Skip(13).ToList();
                                while (aux.Count == 0)
                                { //Se dividen en tablas separadas
                                    modelo2.Add(MarcasCompletasRptData.GenerarDatosReporteAsistencia(aux.Take(13).ToList()));
                                    aux = aux.Skip(13).ToList();
                                }
                            }
                            model.FechaI = model.FechaI.AddDays(1);
                        }
                    }
                }
                else
                {
                    for (; model.FechaI.Date <= model.FechaF.Date && model.MostarSinMarcas; model.FechaI = model.FechaI.AddDays(1))
                        modelo2.Add(MarcasCompletasRptData.GenerarDatosReporteAsistenciaSinMarca(model.FechaI, true));
                    if (!model.MostarSinMarcas)
                        modelo2.Add(MarcasCompletasRptData.GenerarDatosReporteAsistenciaSinMarca(model.FechaI, false));
                }
                string reportPath = Path.Combine(Server.MapPath("~/Reports/MarcasAsistencia"), "ReporteMarcasCompletasRPT.rpt");
                return new CrystalReportPdfResult(modelo1, modelo2, reportPath);
            }
            catch (Exception error)
            {
                Session["ErrorMessage"] = error.Message;
                return null;
            }
        }


        private List<object> GenerarModelosConsolidados(FormularioReporteMarcas model, CTipoJornadaDTO jornada,
                                                                             CFuncionarioDTO funcionario, CDetalleContratacionDTO detalle,
                                                                             CUbicacionAdministrativaDTO ubicacion, List<List<CMarcacionesDTO>> marcas)
        {
            try
            {
                List<ReporteEncabezadoRptData> modelo1 = new List<ReporteEncabezadoRptData>();
                List<ConsolidadoFuncionarioRptData> modelo2 = new List<ConsolidadoFuncionarioRptData>();
                var FechaIJornada = jornada != null ? Convert.ToDateTime(jornada.InicioJornada) : default(DateTime);
                var FechaFJornada = jornada != null ? Convert.ToDateTime(jornada.FinJornada) : default(DateTime);
                var FechaI = model.FechaI.AddDays(0);
                if (marcas != null)
                {
                    marcas.Add(new List<CMarcacionesDTO> { new CMarcacionesDTO { FechaHoraMarca = model.FechaF.AddDays(2) } });
                    foreach (var elem in marcas)
                    {
                        for (; FechaI != elem[0].FechaHoraMarca.Date && FechaI <= model.FechaF.Date; FechaI = FechaI.AddDays(1))
                            if (model.MostarSinMarcas)
                                modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistenciaSinMarca(FechaI, true));
                        if (elem[0].FechaHoraMarca.Date == FechaI)
                        {
                            modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistencia(elem, FechaIJornada, FechaFJornada));
                            FechaI = FechaI.AddDays(1);
                        }
                    }
                }
                else
                {
                    for (; FechaI <= model.FechaF.Date && model.MostarSinMarcas; FechaI = FechaI.AddDays(1))
                        modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistenciaSinMarca(FechaI, true));
                    if (!model.MostarSinMarcas)
                        modelo2.Add(ConsolidadoFuncionarioRptData.GenerarDatosReporteAsistenciaSinMarca(FechaI, false));
                }
                TimeSpan laborado = new TimeSpan(0, 0, 0);
                int ausT = -1, ausM = -1, tar05 = -1, tar20 = -1;
                if (jornada != null)
                    SummaryConsolidadoFuncionario(out ausT, out ausM, out tar05, out tar20, out laborado, model.FechaF, modelo2);

                modelo1.Add(ReporteEncabezadoRptData.GenerarDatosReporteAsistencia(model, funcionario, jornada, detalle,
                                                                                   ubicacion, "", ausT, ausM, tar05, tar20,
                                                                                   String.Format("{0}:{1}", (int)laborado.TotalHours, laborado.Minutes)));
                return new List<object> { modelo1, modelo2 };
            }
            catch (Exception error) {
                Session["ErrorMessage"] = error.Message;
                return null;
            }

        }

        private CrystalReportPdfResult CrearReporteConsolidadoPorFuncionario(FormularioReporteMarcas model, CTipoJornadaDTO jornada,
                                                                             CFuncionarioDTO funcionario, CDetalleContratacionDTO detalle,
                                                                             CUbicacionAdministrativaDTO ubicacion, List<List<CMarcacionesDTO>> marcas)
        {
            try
            {
                var modelos = GenerarModelosConsolidados(model, jornada, funcionario, detalle, ubicacion, marcas);
                if (modelos != null)
                {
                    string reportPath = Path.Combine(Server.MapPath("~/Reports/MarcasAsistencia"), "ConsolidadoFuncionarioRPT.rpt");
                    return new CrystalReportPdfResult(modelos[0], modelos[1], reportPath);
                }
                else
                {
                    if (Session["ErrorMessage"] != null)
                        throw new Exception((string)Session["ErrorMessage"]);
                    else
                        throw new Exception("Ha ocurrido un error a la hora de generar un reporte consolidado por funcionario, favor contactar al personal autorizado.");
                }
            }
            catch (Exception error)
            {
                Session["ErrorMessage"] = error.Message;
                return null;
            }
            
            
        }

        private CrystalReportPdfResult ReporteConsolidadoPorDepartamento(FormularioReporteMarcas model)
        {
            try
            {
                ArreglarFormulario(model);
                var departemento = new CDepartamentoDTO { IdEntidad = int.Parse(model.DepartamentosSeleccion) };
                var datos = this.servicioMarcasAsistencia.ReporteConsolidadoPorDepartamento(departemento, new[] { model.FechaI, model.FechaF });
                if (datos != null) {
                    if(datos.Count() > 0)
                    {
                        List<ReporteEncabezadoSimpleRptData> modelo1 = new List<ReporteEncabezadoSimpleRptData>();
                        List<ReporteEncabezadoRptData> modelo2 = new List<ReporteEncabezadoRptData>();
                        List<ConsolidadoFuncionarioRptData> modelo3 = new List<ConsolidadoFuncionarioRptData>();
                        List<object> modelos;
                        //datos[0] = [CFuncionarioDTO?,CDetalleContratacionDTO,CTipoJornadaDTO?,CUbicacionAdministrativaDTO]
                        // el signo ? es que puede ser un CErrorDTO

                        for (int i = 0; i < datos.Length;)
                        {
                            if (datos[i][0].GetType() != typeof(CErrorDTO))
                            {
                                if (modelo1.Count == 0)//Se genera el encabezado del reporte
                                    modelo1.Add(ReporteEncabezadoSimpleRptData.GenerarDatosReporteAsistencia(model, (CUbicacionAdministrativaDTO)datos[i][3]));
                                if (((CDetalleContratacionDTO)datos[i][1]).EstadoMarcacion != false)
                                {
                                    List<List<CMarcacionesDTO>> marcas = new List<List<CMarcacionesDTO>>();
                                    if (datos[i + 1][0].GetType() == typeof(CErrorDTO))
                                    { //datos[i + 1][0] = Una marca o un CErrroDTO
                                        modelos = GenerarModelosConsolidados(model, datos[i][2].GetType() != typeof(CErrorDTO) ? (CTipoJornadaDTO)datos[i][2] : null, (CFuncionarioDTO)datos[i][0], (CDetalleContratacionDTO)datos[i][1], (CUbicacionAdministrativaDTO)datos[i][3], null);
                                        if (modelos == null)
                                            throw new Exception((Session["ErrorMessage"] != null) ? (string)Session["ErrorMessage"] : "Ha ocurrido un error a la hora de generar los modelos consolidados, favor contactar al personal encargado");

                                        modelo2.Add(((List<ReporteEncabezadoRptData>)(modelos)[0])[0]);
                                        modelo3.AddRange(((List<ConsolidadoFuncionarioRptData>)(modelos)[1]).Select(M => { M.id = modelo2.Last().Funcionario; return M; }));
                                        i += 2;
                                    }
                                    else
                                    {
                                        for (int j = i + 1; j < datos.Length; j++)
                                        {
                                            if (datos[j][0].GetType() == typeof(CMarcacionesDTO))
                                                marcas.Add(datos[j].Select(E => (CMarcacionesDTO)E).ToList());
                                            else
                                            {
                                                modelos = GenerarModelosConsolidados(model, datos[i][2].GetType() != typeof(CErrorDTO) ? (CTipoJornadaDTO)datos[i][2] : null, (CFuncionarioDTO)datos[i][0], (CDetalleContratacionDTO)datos[i][1], (CUbicacionAdministrativaDTO)datos[i][3], marcas);
                                                if (modelos == null)
                                                    throw new Exception((Session["ErrorMessage"] != null) ? (string)Session["ErrorMessage"] : "Ha ocurrido un error a la hora de generar los modelos consolidados, favor contactar al personal encargado");
                                            
                                                modelo2.Add(((List<ReporteEncabezadoRptData>)(modelos)[0])[0]);
                                                modelo3.AddRange(((List<ConsolidadoFuncionarioRptData>)(modelos)[1]).Select(M => { M.id = modelo2.Last().Funcionario; return M; }));
                                                i = j;
                                                break;
                                            }
                                            if (j + 1 == datos.Length)
                                            {
                                                i = datos.Length;
                                            }
                                        }
                                    }
                                }
                            }
                            else i++; //No se encontro el funcionario se salta el funcionario   
                        }
                        string reportPath = Path.Combine(Server.MapPath("~/Reports/MarcasAsistencia"), "ReporteConsolidadoDepartamentoRPT.rpt");
                        return new CrystalReportPdfResult(modelo1, reportPath, modelo2, modelo3);
                    }else
                    {
                        throw new Exception("No se encontraron los datos solicitados.");
                    }
                }else
                {
                    throw new Exception("Ha ocurrido un error a la hora de general el reporte consolidado por departamento, favor contactar al personal encargado.");
                }
                
            }
            catch (Exception error)
            {
                Session["ErrorMessage"] = error.Message;
                return null;

            }
        }


        private CrystalReportPdfResult ReporteFuncionariosPorDepartamentos(FormularioReporteMarcas model)
        {
            try
            {
                var datos = this.servicioMarcasAsistencia.ReporteFuncionariosPorDepartamento(new CDepartamentoDTO { IdEntidad = int.Parse(model.DepartamentosSeleccion) });
                if (datos != null)
                {
                    var modelo = new List<FuncionarioPorDepartamentoRptData>();
                    if (datos[0][0].GetType() != typeof(CErrorDTO))
                    {
                        foreach (var elem in datos)
                        {
                            modelo.Add(FuncionarioPorDepartamentoRptData.GenerarDatosReporteAsistencia((CFuncionarioDTO)elem[0], (CDetalleContratacionDTO)elem[1], (CUbicacionAdministrativaDTO)elem[3]));
                        }
                    }
                    else throw new Exception(((CErrorDTO)datos[0][0]).MensajeError); //No se encontro el funcionario (Se muestra un error, no hay reporte) 
                    string reportPath = Path.Combine(Server.MapPath("~/Reports/MarcasAsistencia"), "FuncionariosPorDepartamentoRPT.rpt");
                    return new CrystalReportPdfResult(reportPath, modelo);
                }else
                {
                    throw new Exception("Ha ocurrido un error al obtener el reporte de funcionarios por departamento, favor contactar al personal encargado.");
                } 
                
            }
            catch (Exception error)
            {
                Session["ErrorMessage"] = error.Message;
                return null;
            }
        }

        /// <summary>
        /// Carga la vista de detalles para cuando se registre un empleado en la BD del reloj marcador
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Details?codigo=0041400942&accion=search
        /// </example>
        /// <param name="codigo">Cédula del funcionario en formato de SIRH</param>
        /// <param name="accion">Acción realizada previamente desde donde se redirigio el sistema a la interfaz de detalles</param>
        /// <returns>Retorna la vista de detalles para un empleado ingresado</returns>
        public ActionResult Details(string codigo, string accion)
        {

            try
            {
                var acceso = AccesoEsPermitido(0, 7);

                if (acceso.GetType() != typeof(bool))
                {
                    return (RedirectToRouteResult)acceso;
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_Marcas"]) ||
                        Session["Permiso_Marcas_Operativo"] != null ||
                         Session["Permiso_Marcas_Consulta"] != null)
                    {
                        FormularioMarcasAsistenciaVM model = new FormularioMarcasAsistenciaVM();

                        CFuncionarioDTO funcionario = new CFuncionarioDTO
                        {
                            Cedula = codigo
                        };

                        funcionario.Sexo = GeneroEnum.Indefinido;

                        if (codigo.Length > 5 && codigo.Length == 10)
                        {
                            codigo = ConvertirCedula(codigo);
                        }

                        CEmpleadoDTO empleado = new CEmpleadoDTO
                        {
                            CodigoEmpleado = codigo
                        };

                        var datos = servicioMarcasAsistencia.BuscarEmpleado(empleado);
                        if (datos == null)
                            throw new Exception("Ha ocurrudo un error a la hora de buscar el empleado, favor comunicarse con el encargado.");

                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {

                            var datos2 = servicioMarcasAsistencia.ObtenerDetalleNombramientoFuncionario(funcionario);

                            if (datos2.GetType() != typeof(CErrorDTO))
                            {
                                var dispositivos = servicioMarcasAsistencia.BuscarDispositivosAsignados(empleado);

                                model.ListaDispositivos = new List<CDispositivoDTO>();
                                if (dispositivos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    foreach (CEmpleadoDispositivoDTO dispositivo in dispositivos)
                                    {
                                        CDispositivoDTO dispositivoAux = new CDispositivoDTO
                                        {
                                            IdEntidad = Convert.ToInt32(dispositivo.Dispositivo)
                                        };

                                        var dispositivoBaseDatos = servicioMarcasAsistencia.BuscarDispositivo(dispositivoAux);
                                        if (dispositivos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                        {
                                            model.ListaDispositivos.Add((CDispositivoDTO)dispositivoBaseDatos);
                                        }

                                    }

                                    model.Empleado = (CEmpleadoDTO)datos.FirstOrDefault();
                                    model.DetalleNombramiento = (CDetalleNombramientoDTO)datos2;
                                }
                                else
                                {
                                    model.Error = ((CErrorDTO)dispositivos.FirstOrDefault());
                                }
                            }
                            else
                            {
                                model.Error = ((CErrorDTO)datos2);
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
                        return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                    }
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
            }
        }

        //
        // GET: /MarcasAsistencia/Search
        /// <summary>
        /// Prepara la interfaz de búsquedas
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Search/
        /// </example>
        /// <returns>Retorna la vista de search</returns>
        public ActionResult Search()
        {
            try
            {
                var acceso = AccesoEsPermitido(0, 7);

                if (acceso.GetType() != typeof(bool))
                {
                    return (RedirectToRouteResult)acceso;
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_Marcas"]) ||
                        Session["Permiso_Marcas_Operativo"] != null ||
                        Session["Permiso_Marcas_Consulta"] != null)
                    {
                        BusquedaEmpleadosMarcasAsistenciaVM model = new BusquedaEmpleadosMarcasAsistenciaVM();

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
                        model.Empleado = new CEmpleadoDTO();

                        model.Estados = new SelectList(datoEstado, "Value", "Text");

                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                    }
                }
            }
            catch (Exception error) {
                return RedirectToAction("Index", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
            }
        }

        //
        // POST: /MarcasAsistencia/Search
        /// <summary>
        /// Realiza la búsqueda de funcionarios
        /// </summary
        /// <example>
        /// /MarcasAsistencia/Search/
        /// </example>
        /// <param name="model">View model asociado a la busqueda de empleados</param>
        /// <returns>Retorna la vista parcial de resultados</returns>
        /// 
        [HttpPost]
        public ActionResult Search(BusquedaEmpleadosMarcasAsistenciaVM model,int ? page)
        {
            try
            {
                if(page != null)
                     model = TempData["model"] as BusquedaEmpleadosMarcasAsistenciaVM;

                if (!string.IsNullOrEmpty(model.Cedula) || model.EstadoSeleccionado != null || !string.IsNullOrEmpty(model.Codigo) || !string.IsNullOrEmpty(model.Nombre) ||
                    !string.IsNullOrEmpty(model.NombreApellido1) || !string.IsNullOrEmpty(model.NombreApellido2))
                {

                    if (model.Cedula != null)
                    {
                        CEmpleadoDTO aux = new CEmpleadoDTO
                        {
                            CodigoEmpleado = model.Cedula,
                            Estado = -1
                        };
                        model.Empleado = aux;
                    }

                    if (!string.IsNullOrEmpty(model.Nombre))
                    {

                        if (model.Empleado == null)
                        {
                            model.Empleado = new CEmpleadoDTO();
                        }
                        model.Nombre = model.Nombre.ToUpper();
                        model.Empleado.PrimerNombre = model.Nombre;
                        model.Empleado.Estado = -1;
                    }

                    if (!string.IsNullOrEmpty(model.NombreApellido1))
                    {
                        if (model.Empleado == null)
                        {
                            model.Empleado = new CEmpleadoDTO();
                        }
                        model.NombreApellido1 = model.NombreApellido1.ToUpper();
                        model.Empleado.ApellidoPaterno = model.NombreApellido1;
                        model.Empleado.Estado = -1;
                    }

                    if (!string.IsNullOrEmpty(model.NombreApellido2))
                    {
                        if (model.Empleado == null)
                        {
                            model.Empleado = new CEmpleadoDTO();
                        }
                        model.NombreApellido2 = model.NombreApellido2.ToUpper();
                        model.Empleado.ApellidoMaterno = model.NombreApellido2;
                        model.Empleado.Estado = -1;
                    }

                    if (!string.IsNullOrEmpty(model.Codigo))
                    {
                        if (model.Empleado == null)
                        {
                            model.Empleado = new CEmpleadoDTO();
                        }
                        model.Empleado.Digitos = model.Codigo;
                        model.Empleado.Estado = -1;
                    }

                    if (model.EstadoSeleccionado != null)
                    {
                        if (model.Empleado == null)
                        {
                            model.Empleado = new CEmpleadoDTO();
                        }

                        if (model.EstadoSeleccionado.Equals("Activo"))
                        {
                            model.Empleado.Estado = 1;
                        }
                        else
                        {

                            model.Empleado.Estado = 0;
                        }
                    }

                    var datos = servicioMarcasAsistencia.SearchEmpleado(model.Empleado, null);

                    if (datos.Count() != 0)
                    {
                        if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                            throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        }
                        else
                        {
                            
                            model.ListaResultados = new List<CEmpleadoDTO>();
                            foreach (var empl in datos)
                            {
                                if (string.IsNullOrEmpty(((CEmpleadoDTO)empl).SegundoNombre))
                                {
                                    ((CEmpleadoDTO)empl).SegundoNombre = "";
                                }
                                model.ListaResultados.Add((CEmpleadoDTO)empl);
                            }
                            TempData["model"] = model;

                            #region Pagination
                            int paginaActual = 0;
                            ViewBag.page = page;

                            if (ViewBag.page != null)
                            {
                                paginaActual = ViewBag.page;

                            }
                            else
                            {
                                paginaActual = 1;
                            }


                            ViewBag.TotalFuncionarios = model.ListaResultados.Count();
                            ViewBag.TotalPaginas = (int)Math.Ceiling((double)ViewBag.TotalFuncionarios / 5);
                            ViewBag.PaginaActual = paginaActual;

                            if ((((paginaActual - 1) * 5) + 5) > ViewBag.TotalFuncionarios)
                            {

                                model.ListaResultados = (List<CEmpleadoDTO>)model.ListaResultados.GetRange(((paginaActual - 1) * 5), (ViewBag.TotalFuncionarios) - (((paginaActual - 1) * 5)));
                            }
                            else
                            {

                                model.ListaResultados = (List<CEmpleadoDTO>)model.ListaResultados.GetRange(((paginaActual - 1) * 5), 5);
                            }
                            #endregion

                            return PartialView("_SearchResults", model);
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                }
                else
                {
                    ModelState.Clear();


                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorMarcasAsistencia");
                }
                else
                {
                    return PartialView("_ErrorMarcasAsistencia");
                }
            }
        }


        //
        // GET: /MarcasAsistencia/Edit/
        /// <summary>
        /// Carga la interfaz de editar un funcionario
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Edit?codigo=0015150829&accion=edit1
        /// </example>
        /// <param name="codigo">Cédula del funcionario en formato de SIRH</param>
        /// <param name="accion">"edit1": Si dio de alta a un funcionario
        ///                      "edit2": Si dio de baja a un funcionario</param>
        /// <returns>Retorna la vista de edit</returns>
        public ActionResult Edit(string codigo, string accion)
        {
            try
            {
                var acceso = AccesoEsPermitido(0, 7);

                if (acceso.GetType() != typeof(bool))
                {
                    return (RedirectToRouteResult)acceso;
                }
                else
                {
                    if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                        Convert.ToBoolean(Session["Administrador_Marcas"]) ||
                        Session["Permiso_Marcas_Operativo"] != null)
                    {
                        FormularioMarcasAsistenciaVM model = new FormularioMarcasAsistenciaVM();

                        CEmpleadoDTO empleadoAuxiliar = new CEmpleadoDTO
                        {
                            CodigoEmpleado = codigo
                        };

                        if (codigo.Length > 5)
                        {
                            empleadoAuxiliar.CodigoEmpleado = ConvertirCedula(codigo);
                        }

                        var datos = servicioMarcasAsistencia.BuscarEmpleado(empleadoAuxiliar);
                        if (datos == null)
                            throw new Exception("Ha ocurrudo un error a la hora de buscar el empleado, favor comunicarse con el encargado.");

                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Empleado = (CEmpleadoDTO)datos.FirstOrDefault();
                            if (codigo.Length > 5)
                            {
                                model.Empleado.CodigoEmpleado = ConvertirCedulaSIRH(model.Empleado.CodigoEmpleado);
                            }
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.ElementAt(0);
                        }

                        var motivos = servicioMarcasAsistencia.ListarMotivoBaja().Select(
                        P => new SelectListItem
                        {
                            Value = ((CMotivoBajaDTO)P).IdEntidad.ToString(),
                            Text = ((CMotivoBajaDTO)P).Descripcion.ToString()
                        }
                        );
                        if (motivos == null)
                            throw new Exception("Ha ocurrido un error a la hora de listar los motivos de baja, favor contactar al personal encargado. ");

                        model.CatalogoMotivoBaja = new SelectList(motivos, "Value", "Text");


                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.Marcas) });
                    }
                }
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "Error", new { errorType = error.Message, modulo = Convert.ToInt32(EModulosHelper.Marcas) });
            }
        }

        //
        // POST: /MarcasAsistencia/Edit/
        /// <summary>
        /// Edita un funcionario
        /// </summary>
        /// <example>
        /// /MarcasAsistencia/Edit?codigo=0015150829&accion=edit1
        /// </example>
        /// <param name="model">View model asociado al formulario de edición de funcionarios</param>
        /// <param name="codigo">Cédula del funcionario en formato de SIRH</param>
        /// <param name="accion">"edit1": Si dio de alta a un funcionario
        ///                      "edit2": Si dio de baja a un funcionario</param>
        /// <returns>Retorna la vista de detalles</returns>
        [HttpPost]
        public ActionResult Edit(string codigo, string accion, FormularioMarcasAsistenciaVM model)
        {
            try
            {
                if (accion == "edit1")
                {
                    if (model.MotivoBajaSeleccionado != null)
                    {
                        if (model.DetalleNombramiento.ObservacionesTipoJornada != null)
                        {

                            CEmpleadoDTO empleadoAux = new CEmpleadoDTO
                            {
                                CodigoEmpleado = codigo,
                                ApellidoPaterno = model.Empleado.ApellidoPaterno,
                                ApellidoMaterno = model.Empleado.ApellidoMaterno,
                                PrimerNombre = model.Empleado.PrimerNombre,
                                Digitos = model.Empleado.Digitos
                            };

                            if (codigo.Length > 5)
                            {
                                empleadoAux.CodigoEmpleado = ConvertirCedula(codigo);
                            }

                            model.Empleado = new CEmpleadoDTO();
                            model.Empleado = empleadoAux;

                            model.MotivoBaja = new CMotivoBajaDTO
                            {
                                IdEntidad = Convert.ToInt32(model.MotivoBajaSeleccionado)
                            };

                            CFuncionarioDTO funcionarioAux = new CFuncionarioDTO
                            {
                                Cedula = codigo
                            };

                            funcionarioAux.Sexo = GeneroEnum.Indefinido;

                            var respuesta = servicioMarcasAsistencia.DesactivarEmpleado(model.Empleado, funcionarioAux, model.DetalleNombramiento, model.MotivoBaja);

                            if (respuesta.GetType() != typeof(CErrorDTO))
                            {
                                return RedirectToAction("Details", new { codigo = codigo, accion = "edit1" });
                            }
                            else
                            {
                                return RedirectToAction("Details", new { codigo = model.Empleado.CodigoEmpleado, accion = "search" });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("contenido", "Debe indicar una justificación para anular al funcionario");
                            throw new Exception();
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("contenido", "Debe seleccionar el motivo de la baja");
                        throw new Exception();
                    }
                }
                else
                {

                    if (model.DetalleNombramiento.ObservacionesTipoJornada != null)
                    {
                        CEmpleadoDTO empleadoAux = new CEmpleadoDTO
                        {
                            CodigoEmpleado = codigo,
                            ApellidoPaterno = model.Empleado.ApellidoPaterno,
                            ApellidoMaterno = model.Empleado.ApellidoMaterno,
                            PrimerNombre = model.Empleado.PrimerNombre,
                            Digitos = model.Empleado.Digitos
                        };

                        if (codigo.Length > 5)
                        {
                            empleadoAux.CodigoEmpleado = ConvertirCedula(codigo);
                        }

                        model.Empleado = new CEmpleadoDTO();
                        model.Empleado = empleadoAux;

                        model.MotivoBaja = new CMotivoBajaDTO
                        {
                            IdEntidad = Convert.ToInt32(model.MotivoBajaSeleccionado)
                        };

                        CFuncionarioDTO funcionarioAux = new CFuncionarioDTO
                        {
                            Cedula = codigo
                        };

                        funcionarioAux.Sexo = GeneroEnum.Indefinido;

                        var respuesta = servicioMarcasAsistencia.ActivarEmpleado(model.Empleado, funcionarioAux, model.DetalleNombramiento);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            return RedirectToAction("Details", new { codigo = codigo, accion = "edit2" });
                        }
                        else
                        {
                            ModelState.AddModelError("modificar", ((CErrorDTO)respuesta).MensajeError);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Debe indicar una justificación para dar de alta al funcionario");
                        throw new Exception();
                    }
                }
            }

            catch (Exception e)
            {
                var motivos = servicioMarcasAsistencia.ListarMotivoBaja().Select(
                 P => new SelectListItem
                 {
                     Value = ((CMotivoBajaDTO)P).IdEntidad.ToString(),
                     Text = ((CMotivoBajaDTO)P).Descripcion.ToString()
                 }
                 );

                model.CatalogoMotivoBaja = new SelectList(motivos, "Value", "Text");
                if (model.Empleado.CodigoEmpleado.Length > 5)
                {
                    model.Empleado.CodigoEmpleado = ConvertirCedulaSIRH(model.Empleado.CodigoEmpleado);
                }
                model.Error = new CErrorDTO { MensajeError = "ErrorEdit" };
                return View(model);
            }
        }


        /// <summary>
        /// Convierte un número de cédula del formato SIRH al formato del reloj marcador
        /// </summary>
        /// <param name="cedula">Número de cedula del funcionario</param>
        /// <returns>Retorna la cédula en formato del reloj marcador</returns>
        private string ConvertirCedula(string cedula)
        {
            if (cedula[1] == '0')
            {
                string codigoEmpleadoAux = cedula[2] + "0";
                string ultimosDigitos = cedula.Substring(3);
                cedula = codigoEmpleadoAux + ultimosDigitos;
                return cedula;
            }
            else
            {
                cedula = cedula.Substring(1);
                return cedula;
            }
        }

        /// <summary>
        /// Convierte un número de cédula del formato SIRH al formato del reloj marcador
        /// </summary>
        /// <param name="cedula">Número de cedula del funcionario</param>
        /// <returns>Retorna la cédula en formato del reloj marcador</returns>
        private string ConvertirCedulaSIRH(string cedula)
        {
            if (cedula[1] == '0')
            {
                string codigoEmpleadoAux = cedula[0] + "";
                string ultimosDigitos = cedula.Substring(2);
                cedula = codigoEmpleadoAux + ultimosDigitos;
                return "00" + cedula;
            }
            else
            {
                cedula = "0" + cedula;
                return cedula;
            }
        }

    }
}
