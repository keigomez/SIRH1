using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
//using SIRH.Web.FuncionarioService;
using SIRH.Web.FuncionarioLocal;
//using SIRH.Web.PuestoService;
using SIRH.Web.Models;
using SIRH.Web.PuestoLocal;
//using SIRH.Web.AccionPersonalService;
using SIRH.Web.AccionPersonalLocal;
using SIRH.DTO;
using SIRH.Web.Helpers;
using System.IO;
using SIRH.Web.Reports.Funcionarios;
using SIRH.Web.Reports.Puesto;
using SIRH.Web.Reports.PDF;
using SIRH.Web.ServicioTSE;
//using SIRH.Web.DesarraigoService;
using SIRH.Web.DesarraigoLocal;
using SIRH.Web.UserValidation;
using System.Security.Principal;
//using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.PerfilUsuarioLocal;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace SIRH.Web.Controllers
{
    public class PuestoController : Controller
    {
        //Prueba de TFS
        CFuncionarioServiceClient funcionarioReference = new CFuncionarioServiceClient();
        CPuestoServiceClient puestoReference = new CPuestoServiceClient();
        CDesarraigoServiceClient ServicioDesarraigo = new CDesarraigoServiceClient();
        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();

        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        #region R_PAGINA_PRINCIPAL

        public ActionResult Vacio()
        {
            return View();
        }

        public ActionResult Index(string codpuesto, string codclase, string codespecialidad, string codocupacion, string estadoPuestoSeleccionado, string confianzaSeleccionada, string codnivel, int? page)
        {
            Session["errorP"] = null;
            try
            {

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codpuesto) && String.IsNullOrEmpty(codclase) && String.IsNullOrEmpty(codespecialidad) &&
                    String.IsNullOrEmpty(codocupacion) && 
                    (String.IsNullOrEmpty(estadoPuestoSeleccionado) || estadoPuestoSeleccionado.Equals("Seleccione una opción")) &&
                    (String.IsNullOrEmpty(confianzaSeleccionada) || confianzaSeleccionada.Equals("Seleccione una opción")) &&
                    (String.IsNullOrEmpty(codnivel) || codnivel.Equals("Seleccione una opción")))
                {
                    page = 1;
                    return View();
                }
                else
                {
                    FuncionarioPuestoVM modelo = new FuncionarioPuestoVM();

                    modelo.CodPuestoSearch = codpuesto;
                    modelo.CodClaseSearch = codclase;
                    modelo.CodNivelSearch = codnivel;
                    modelo.CodEspecialidadSearch = codespecialidad;
                    modelo.CodOcupacionRealSearch = codocupacion;
                    modelo.CodEstadoPuestoSearch = estadoPuestoSeleccionado;
                    modelo.CodConfianzaSearch = confianzaSeleccionada;
                    int codigoClase = String.IsNullOrEmpty(modelo.CodClaseSearch) ? 0 : Convert.ToInt32(modelo.CodClaseSearch.Split('-')[0]);
                    int codigoEspecialidad = String.IsNullOrEmpty(modelo.CodEspecialidadSearch) ? 0 : Convert.ToInt32(modelo.CodEspecialidadSearch.Split('-')[0]);
                    int codigoOcupacion = String.IsNullOrEmpty(modelo.CodOcupacionRealSearch) ? 0 : Convert.ToInt32(modelo.CodOcupacionRealSearch.Split('-')[0]);

                    var puestos =
                        puestoReference.BuscarFuncionarioPuestoPuesto(modelo.CodPuestoSearch,
                                                                        codigoClase,
                                                                        codigoEspecialidad,
                                                                        codigoOcupacion, estadoPuestoSeleccionado, confianzaSeleccionada, NivelOcupacionalHelper.ObtenerId(codnivel));
                    modelo.TotalPuestos = puestos.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalPuestos / 10);
                    modelo.PaginaActual = paginaActual;
                    Session["puestos"] = puestos.ToList();
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalPuestos)
                    {
                        modelo.Puestos = puestos.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalPuestos) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Puestos = puestos.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorPuesto");
            }
        }

        //[HttpPost]
        //public ActionResult Index(FuncionarioPuestoVM modelo, string codpuesto, string codclase, string codespecialidad, string codocupacion, string estadoPuestoSeleccionado, string confianzaSeleccionada, string codnivel, int? page)
        //{
        //    Session["errorP"] = null;
        //    try
        //    {

        //        int paginaActual = page.HasValue ? page.Value : 1;

        //        modelo.CodPuestoSearch = codpuesto;
        //        modelo.CodClaseSearch = codclase;
        //        modelo.CodEspecialidadSearch = codespecialidad;
        //        modelo.CodOcupacionRealSearch = codocupacion;
        //        modelo.CodNivelSearch = codnivel;
        //        modelo.CodEstadoPuestoSearch = estadoPuestoSeleccionado;
        //        modelo.CodConfianzaSearch = confianzaSeleccionada;
        //        int codigoClase = String.IsNullOrEmpty(modelo.CodClaseSearch) ? 0 : Convert.ToInt32(modelo.CodClaseSearch.Split('-')[0]);
        //        int codigoEspecialidad = String.IsNullOrEmpty(modelo.CodEspecialidadSearch) ? 0 : Convert.ToInt32(modelo.CodEspecialidadSearch.Split('-')[0]);
        //        int codigoOcupacion = String.IsNullOrEmpty(modelo.CodOcupacionRealSearch) ? 0 : Convert.ToInt32(modelo.CodOcupacionRealSearch.Split('-')[0]);

        //        var puestos =
        //               puestoReference.BuscarFuncionarioPuestoPuesto(modelo.CodPuestoSearch,
        //                                                               codigoClase,
        //                                                               codigoEspecialidad,
        //                                                               codigoOcupacion, estadoPuestoSeleccionado, confianzaSeleccionada, NivelOcupacionalHelper.ObtenerId(codnivel));
        //        modelo.TotalPuestos = puestos.Count();
        //        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalPuestos / 10);
        //        modelo.PaginaActual = paginaActual;
        //        Session["puestos"] = puestos.ToList();
        //        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalPuestos)
        //        {
        //            modelo.Puestos = puestos.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalPuestos) - (((paginaActual - 1) * 10))).ToList(); ;
        //        }
        //        else
        //        {
        //            modelo.Puestos = puestos.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
        //        }

        //        return PartialView("Index_Result", modelo);
        //    }
        //    catch (Exception error)
        //    {
        //        ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
        //        return PartialView("_ErrorPuesto");
        //    }
        //}
        public PartialViewResult Clase_Index(string codigoclase, string nomclase, int? page)
        {
            Session["errorF"] = null;
            try {
                ClaseModel modelo = new ClaseModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoclase) && String.IsNullOrEmpty(nomclase))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoclase;
                    modelo.NombreSearch = nomclase;
                    int codigoClase = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var clases =
                        puestoReference.BuscarClaseParams(codigoClase, modelo.NombreSearch);
                    modelo.TotalClases = clases.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalClases / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalClases)
                    {
                        modelo.Clase = clases.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalClases) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Clase = clases.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Clase_Index_Result", modelo);
                }
            }
            catch(Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorPuesto");
            }
        }

        public PartialViewResult Especialidad_Index(string codigoEspecialidad, string nomEspecialidad, int? page)
        {
            Session["errorP"] = null;
            try
            {
                EspecialidadModel modelo = new EspecialidadModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoEspecialidad) && String.IsNullOrEmpty(nomEspecialidad))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoEspecialidad;
                    modelo.NombreSearch = nomEspecialidad;
                    int codigoEsp = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var especialidades =
                        puestoReference.BuscarEspecialidadParams(codigoEsp, modelo.NombreSearch);
                    modelo.TotalEspecialidades = especialidades.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalEspecialidades / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalEspecialidades)
                    {
                        modelo.Especialidad = especialidades.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalEspecialidades) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Especialidad = especialidades.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Especialidad_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorPuesto");
            }
        }

        public PartialViewResult Ocupacion_Index(string codigoOcupacion, string nomOcupacion, int? page)
        {
            Session["errorP"] = null;
            try
            {
                OcupacionModel modelo = new OcupacionModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoOcupacion) && String.IsNullOrEmpty(nomOcupacion))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoOcupacion;
                    modelo.NombreSearch = nomOcupacion;
                    int codigoOcup = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var ocupaciones =
                        puestoReference.BuscarOcupacionParams(codigoOcup, modelo.NombreSearch);
                    modelo.TotalOcupaciones = ocupaciones.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalOcupaciones / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalOcupaciones)
                    {
                        modelo.Ocupacion = ocupaciones.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalOcupaciones) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Ocupacion = ocupaciones.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Ocupacion_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorPuesto");
            }
        }

        public PartialViewResult Subespecialidad_Index(string codigoSubespecialidad, string nomSubespecialidad, int? page)
        {
            Session["errorP"] = null;
            try
            {
                SubespecialidadModel modelo = new SubespecialidadModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoSubespecialidad) && String.IsNullOrEmpty(nomSubespecialidad))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoSubespecialidad;
                    modelo.NombreSearch = nomSubespecialidad;
                    int codigoSubes = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    List<CSubEspecialidadDTO> subespecialidades = new List<CSubEspecialidadDTO>();
                    foreach (var item in puestoReference.BuscarSubespecialidadParam(codigoSubes, modelo.NombreSearch))
                    {
                        subespecialidades.Add((CSubEspecialidadDTO)item);
                    }
                    modelo.TotalSubespecialidades = subespecialidades.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalSubespecialidades / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalSubespecialidades)
                    {
                        modelo.Subespecialidad = subespecialidades.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalSubespecialidades) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Subespecialidad = subespecialidades.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Subespecialidad_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorPuesto");
            }
        }

        public ActionResult Details(string codPuesto)
        {
            Session["errorF"] = null;
            try
            {
                PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();

                var perfil = puestoReference.DescargarPerfilPuestoAcciones(codPuesto);
                if (perfil.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    modelo = ConstruirModeloPuesto(modelo, perfil);
                    if (modelo.Nombramiento != null && modelo.Nombramiento.Mensaje == null )
                    {
                        var datos = funcionarioReference.BuscarFuncionarioSalario(modelo.Funcionario.Cedula);
                        modelo.Salario = (CSalarioDTO)datos.ElementAt(1);

                        // No tiene un nombramiento activo
                        if(modelo.Funcionario.Mensaje == null)
                        {
                            modelo.DetalleContrato.NumeroAnualidades = 1;
                            modelo.Salario.MtoAumentosAnuales = modelo.Salario.DetallePuesto.EscalaSalarial.MontoAumentoAnual;
                            modelo.Salario.NumPuntos = 1;
                            modelo.Salario.MtoGradoGrupo = modelo.Salario.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera;
                            modelo.Salario.MtoOtros = 0;
                            modelo.Salario.DetallePuesto.DetalleRubros = new List<CDetallePuestoRubroDTO>();
                             
                            modelo.Salario.MtoTotal = modelo.Salario.DetallePuesto.EscalaSalarial.SalarioBase +
                                                      modelo.Salario.DetallePuesto.EscalaSalarial.MontoAumentoAnual +
                                                      modelo.Salario.MtoProhibicion +
                                                      modelo.Salario.MtoGradoGrupo;
                        }
                    }
                }
                else
                {
                    var mensaje = ((CErrorDTO)perfil.ElementAt(0).ElementAt(0)).MensajeError;
                    throw new Exception(mensaje); // throw new Exception("Busqueda");
                }
                if (modelo == null) throw new Exception("modelo");
                Session["funcionario"] = modelo;

                return View(modelo);
            }
            catch (Exception error)
            {
                Session["errorF"] = "error";

                if (error.Message != "modelo")
                {
                    ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de ver el perfil del funcionario, ponerse en contacto con el personal autorizado. \n" + error.ToString());

                }
                else
                {
                    ModelState.AddModelError("modelo", "No existen datos del lugar de contrato ni el lugar de trabajo.\n\n Pongase en contacto con el personal autorizado.");

                }
                return View();

            }
        }
        public ActionResult Browse(string cedula, string codpuesto, string SubmitButton)
        {
            try
            {
                PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                var motivosMovimiento = puestoReference.ListarMotivosMovimiento()
                    .Where(Q => Q.IdEntidad == 7 || Q.IdEntidad == 9 || Q.IdEntidad == 10
                            || Q.IdEntidad == 11 || Q.IdEntidad == 13 || Q.IdEntidad == 14 || Q.IdEntidad == 15 || Q.IdEntidad == 16
                            || Q.IdEntidad == 19 || Q.IdEntidad == 20 || Q.IdEntidad == 12 || Q.IdEntidad == 22
                            || Q.IdEntidad == 17 || Q.IdEntidad == 21 || Q.IdEntidad == 35 || (Q.IdEntidad >= 45 && Q.IdEntidad <= 52) || Q.IdEntidad == 65 || Q.IdEntidad == 66)
                    .Select(Q => new SelectListItem
                    {
                        Value = ((CMotivoMovimientoDTO)Q).IdEntidad.ToString(),
                        Text = ((CMotivoMovimientoDTO)Q).DesMotivo.ToString()
                    });

                if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(codpuesto))
                {
                    if (SubmitButton == null)
                    {
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", "Debe digitar al menos un parámetro de búsqueda.");
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(cedula))
                    {
                        var datos = puestoReference.DescargarPerfilPuestoAcciones(codpuesto);

                        if (datos.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                        {
                            modelo = ConstruirModeloPuesto(modelo, datos);
                            modelo.MotivosMovimiento = new SelectList(motivosMovimiento, "Value", "Text");
                            modelo.MovimientoPuesto = new SIRH.DTO.CMovimientoPuestoDTO
                            {
                                Puesto = modelo.Puesto
                            };
                            return PartialView("Browse_Result", modelo);
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        var datos = puestoReference.DescargarPerfilPuestoAccionesFuncionario(cedula);

                        if (datos.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                        {
                            if (((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 1
                                || ((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 2
                                || ((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 3
                                || ((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 6
                                || ((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 23
                                || ((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 24
                                || ((CFuncionarioDTO)datos.ElementAt(0).ElementAt(0)).EstadoFuncionario.IdEntidad == 25)
                            {
                                modelo = ConstruirModeloPuesto(modelo, datos);
                                modelo.MotivosMovimiento = new SelectList(motivosMovimiento, "Value", "Text");
                                modelo.MovimientoPuesto = new SIRH.DTO.CMovimientoPuestoDTO
                                {
                                    Puesto = modelo.Puesto
                                };
                                return PartialView("Browse_Result", modelo);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "El funcionario no se encuentra activo en la planilla del Ministerio");
                                throw new Exception("Busqueda");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", (error.Message));
                return PartialView("_ErrorPuesto");
            }
        }

        private PerfilFuncionarioVM ConstruirModeloPuesto(PerfilFuncionarioVM modelo, CBaseDTO[][] datos)
        {
            modelo.Funcionario = (CFuncionarioDTO)datos.ElementAt(0).ElementAt(0);
            modelo.Nombramiento = (CNombramientoDTO)datos.ElementAt(1).ElementAt(0);
            modelo.DetalleContrato = (CDetalleContratacionDTO)datos.ElementAt(2).ElementAt(0);
            modelo.Puesto = (CPuestoDTO)datos.ElementAt(3).ElementAt(0);
            modelo.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(4).ElementAt(0);

            if (datos.ElementAt(5).Count() > 0)
            {
                if (datos.ElementAt(5).ElementAt(0).IdEntidad.Equals(1))
                {
                    modelo.UbicacionContrato = (CUbicacionPuestoDTO)datos.ElementAt(5).ElementAt(0);
                    modelo.UbicacionTrabajo = (CUbicacionPuestoDTO)datos.ElementAt(5).ElementAt(1);
                }
                else
                {
                    modelo.UbicacionContrato = (CUbicacionPuestoDTO)datos.ElementAt(5).ElementAt(1);
                    modelo.UbicacionTrabajo = (CUbicacionPuestoDTO)datos.ElementAt(5).ElementAt(0);
                }
            }
            else
            {
                modelo.UbicacionContrato = new CUbicacionPuestoDTO();
                modelo.UbicacionTrabajo = new CUbicacionPuestoDTO();
            }

            if (datos.ElementAt(6).ElementAt(0).IdEntidad != -1)
            {
                modelo.MensajeEstudio = "El puesto actualmente está en estudio por el Departamento de Gestión de la Organización del Trabajo, bajo el número de oficio: "
                                        + ((CEstudioPuestoDTO)datos.ElementAt(6).ElementAt(0)).NumeroOficio;
            }

            if (datos.ElementAt(7).ElementAt(0).IdEntidad != -1)
            {
                modelo.MensajePedimento = "El puesto ya cuenta con un pedimento de personal activo: "
                                        + ((CPedimentoPuestoDTO)datos.ElementAt(7).ElementAt(0)).NumeroPedimento;
            }

            if (datos.ElementAt(8).ElementAt(0).IdEntidad != -1)
            {
                modelo.MensajePrestamo = "El puesto se encuentra actualmente en calidad de préstamo, bajo la resolución número: "
                                        + ((CPrestamoPuestoDTO)datos.ElementAt(8).ElementAt(0)).NumDeResolucion;
            }

            if (datos.ElementAt(9).ElementAt(0).IdEntidad == -1)
            {
                modelo.MensajePresupuesto = "Este puesto no cuenta con contenido presupuestario registrado para su utilización.";
            }
            else
            {
                modelo.MensajePresupuesto = "Este puesto cuenta con contenido presupuestario registrado para su utilización, bajo la resolución número: "
                                        + ((CContenidoPresupuestarioDTO)datos.ElementAt(9).ElementAt(0)).NumeroResolucion;

            }
            if (datos.Count() > 14)
            {
                if (datos.ElementAt(14).ElementAt(0).IdEntidad != -1)
                {
                    modelo.FuncionarioPropietario = (CNombramientoDTO)datos.ElementAt(14).ElementAt(0);
                }
                if (datos.ElementAt(15).ElementAt(0).IdEntidad != -1)
                {
                    modelo.FuncionarioOcupante = (CNombramientoDTO)datos.ElementAt(15).ElementAt(0);
                }
            }

            return modelo;
        }

        #endregion

        #region R_ESTUDIO_PUESTO

        //
        // GET: /Puesto/CreateEstudio
        public ActionResult CreateEstudio(string codigo)
        {
            EstudioPuestoVM model = new EstudioPuestoVM();
            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            var resultado = puestoReference.DescargarPuestoCompleto(codigo);

            if (resultado.Count() > 1)
            {
                model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.UbicacionPuesto = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
            }
            else
            {
                model.Error = ((CErrorDTO)resultado.FirstOrDefault());
            }

            return View(model);
        }

        //
        // POST: /Puesto/CreateEstudio
        [HttpPost]
        public ActionResult CreateEstudio(EstudioPuestoVM model)
        {
            return JavaScript("window.location = '/Puesto/EstudioDetails?codigo=" +
                model.Puesto.CodPuesto + "&accion=guardar" + "'");
        }

        //
        // GET: /Puesto/EditEstudio
        public ActionResult EditEstudio(string codigo)
        {
            EstudioPuestoVM model = new EstudioPuestoVM();
            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            return View(model);
        }

        //
        // POST: /Puesto/EditEstudio
        [HttpPost]
        public ActionResult EditEstudio(EstudioPuestoVM model)
        {
            //PROCESAR LOS DATOS DEL ESTUDIO DEL PUESTO
            return View(model);
        }

        #endregion

        #region R_PRESTAMO_PUESTO

        //
        // GET: /Puesto/CreatePrestamo
        public ActionResult CreatePrestamo(string codigo)
        {
            PrestamoPuestoVM model = new PrestamoPuestoVM();
            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            return View(model);
        }

        //
        // POST: /Puesto/CreatePrestamo
        [HttpPost]
        public ActionResult CreatePrestamo(PrestamoPuestoVM model)
        {
            //PROCESAR LOS DATOS DEL ESTUDIO DEL PUESTO
            return View(model);
        }

        //
        // GET: /Puesto/EditPrestamo
        public ActionResult EditPrestamo(string codigo)
        {
            PrestamoPuestoVM model = new PrestamoPuestoVM();
            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            return View(model);
        }

        //
        // POST: /Puesto/EditPrestamo
        [HttpPost]
        public ActionResult EditPrestamo(PrestamoPuestoVM model)
        {
            //PROCESAR LOS DATOS DEL ESTUDIO DEL PUESTO
            return View(model);
        }

        #endregion

        #region R_PEDIMENTO_PUESTO

        //
        // GET: /Puesto/SearchPedimento
        public ActionResult SearchPedimento()
        {
            return View();
        }

        //
        // POST: /Puesto/SearchPedimento
        [HttpPost]
        public ActionResult SearchPedimento(PedimentoPuestoVM model)
        {
            var resultado = puestoReference.DescargarPuestoPedimento(model.PedimentoPuesto.NumeroPedimento);

            if (resultado.Count() > 1)
            {
                model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.UbicacionPuesto = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
                model.PedimentoPuesto = ((CPedimentoPuestoDTO)resultado.ElementAt(3));
            }
            else
            {
                model.Error = ((CErrorDTO)resultado.FirstOrDefault());
            }

            return PartialView("_ResultadoPedimento", model);
        }

        //
        // GET: /Puesto/ListPedimentos
        public ActionResult ListPedimentos(string codigo)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();

            var resultado = puestoReference.BuscarPedimentosPorPuesto(new CPuestoDTO { CodPuesto = codigo });

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.UbicacionPuesto = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
                model.ListaPedimentos = new List<CPedimentoPuestoDTO>();
                for (int i = 3; i < resultado.Count(); i++)
                {
                    model.ListaPedimentos.Add(((CPedimentoPuestoDTO)resultado.ElementAt(i)));
                }
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
            }

            return View(model);
        }

        //
        // GET: /Puesto/ModificarUbicacionTrabajo
        public ActionResult ModificarUbicacionTrabajo(string codigo)
        {
            PuestoUbicacionGeograficaVM model = new PuestoUbicacionGeograficaVM();

            var resultado = puestoReference.DescargarUbicacionTrabajoPedimento(codigo);
            var datosGeograficos = puestoReference.GetLocalizacion(false, 0, false, true, 0);

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.DetallePuesto = new DetallePuestoVM();
                model.DetallePuesto.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.DetallePuesto.UbicacionContrato = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
                model.DetallePuesto.UbicacionTrabajo = ((CUbicacionPuestoDTO)resultado.ElementAt(3));
                model.DetallePuesto.PedimentoPuesto = ((CPedimentoPuestoDTO)resultado.ElementAt(4));

                model.Cantones = new SelectList(new List<SelectListItem>());
                model.Distritos = new SelectList(new List<SelectListItem>());
                model.Provincias = new SelectList(datosGeograficos.ElementAt(0).Select(Q => new SelectListItem
                {
                    Value = ((CProvinciaDTO)Q).IdEntidad.ToString(),
                    Text = ((CProvinciaDTO)Q).NomProvincia
                }), "Value", "Text");
                TempData["DetallePuesto"] = model.DetallePuesto;
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GetCantones(int provincia)
        {
            var datosGeograficos = puestoReference.GetLocalizacion(true, 0, false, false, provincia);
            var cantones = new SelectList(datosGeograficos.ElementAt(0)
                .Select(Q => new SelectListItem
                {
                    Value = ((CCantonDTO)Q).IdEntidad.ToString(),
                    Text = ((CCantonDTO)Q).NomCanton
                }), "Value", "Text");

            return Json(cantones, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDistritos(int canton)
        {
            var datosGeograficos = puestoReference.GetLocalizacion(false, canton, true, false, 0);
            var distritos = new SelectList(datosGeograficos.ElementAt(0)
                .Select(Q => new SelectListItem
                {
                    Value = ((CDistritoDTO)Q).IdEntidad.ToString(),
                    Text = ((CDistritoDTO)Q).NomDistrito
                }), "Value", "Text");

            return Json(distritos, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Puesto/ModificarUbicacionTrabajo
        [HttpPost]
        public ActionResult ModificarUbicacionTrabajo(PuestoUbicacionGeograficaVM model)
        {
            try
            {
                model.DetallePuesto = TempData["DetallePuesto"] as DetallePuestoVM;
                string[] provincia = model.NombreProvincia.Split('-');
                string[] canton = model.CantonSeleccionado.Split('-');
                string[] distrito = model.DistritoSeleccionado.Split('-');
                model.DetallePuesto.UbicacionTrabajo.Distrito = new CDistritoDTO { NomDistrito = distrito.ElementAt(1) };
                model.DetallePuesto.UbicacionTrabajo.Distrito.Canton = new CCantonDTO { NomCanton = canton.ElementAt(1) };
                model.DetallePuesto.UbicacionTrabajo.Distrito.Canton.Provincia = new CProvinciaDTO { NomProvincia = provincia.ElementAt(1) };
                model.UbicacionTrabajoNueva.Distrito = new CDistritoDTO { IdEntidad = Convert.ToInt32(distrito.ElementAt(0)) };
                if (ModelState.IsValid)
                {
                    var resultado = puestoReference.ModificarUbicacionPuesto(model.DetallePuesto.Puesto, model.UbicacionTrabajoNueva);
                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        TempData["DataUbicacionPuesto"] = model;
                        model.DetallePuesto.Accion = SIRH.Web.Helpers.EAccionesPuestoHelper.GUARDAR_UBICACION;
                        return JavaScript("window.location = '/Puesto/UbicacionPuestoDetails?codPuesto=" +
                                    ((CRespuestaDTO)resultado).Contenido + "'");
                    }
                    else
                    {
                        ModelState.AddModelError("Guardar", ((CErrorDTO)resultado).MensajeError);
                        throw new Exception("Guardado");
                    }
                }
                else
                {
                    throw new Exception("Validacion");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Validacion" && error.Message != "Guardado")
                {
                    ModelState.AddModelError("Sistema", error.Message);
                }
                //Aquí debe retornar el partial view con los errores :)
                var datosGeograficos = puestoReference.GetLocalizacion(false, 0, false, true, 0);
                model.Provincias = new SelectList(datosGeograficos.ElementAt(0).Select(Q => new SelectListItem
                {
                    Value = ((CProvinciaDTO)Q).IdEntidad.ToString(),
                    Text = ((CProvinciaDTO)Q).NomProvincia
                }), "Value", "Text");
                model.Cantones = new SelectList(new List<SelectListItem>());
                model.Distritos = new SelectList(new List<SelectListItem>());
                return PartialView("_ErrorPuesto");
            }
        }

        //
        // GET: /Puesto/UbicacionPuestoDetails
        public ActionResult UbicacionPuestoDetails(string codPuesto, string codigo)
        {
            var model = new PuestoUbicacionGeograficaVM();
            try
            {
                if (TempData["DataUbicacionPuesto"] != null)
                {
                    model = TempData["DataUbicacionPuesto"] as PuestoUbicacionGeograficaVM;
                    return View(model);
                }
                else
                {
                    //Método que trae los detalles de la ubicación del puesto específica, por código de la ubicación
                    return View(model);
                }
            }
            catch (Exception error)
            {
                return View(model);
            }
        }

        //
        // GET: /Puesto/UbicacionPuestoList
        public ActionResult UbicacionPuestoList(string codPuesto)
        {
            var model = new PuestoUbicacionGeograficaVM();
            try
            {
                var resultado = puestoReference.BuscarHistorialUbicacionTrabajo(codPuesto);

                if (resultado.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.DetallePuesto = new DetallePuestoVM();
                    model.DetallePuesto.Puesto = ((CPuestoDTO)resultado.ElementAt(0).ElementAt(0));
                    model.DetallePuesto.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1).ElementAt(0));
                    model.DetallePuesto.UbicacionContrato = ((CUbicacionPuestoDTO)resultado.ElementAt(2).ElementAt(0));
                    model.HistorialTrabajo = resultado.ElementAt(3).Select(Q => new CUbicacionPuestoDTO
                    {
                        IdEntidad = ((CUbicacionPuestoDTO)Q).IdEntidad,
                        Distrito = ((CUbicacionPuestoDTO)Q).Distrito,
                        EstadoUbicacionPuesto = ((CUbicacionPuestoDTO)Q).EstadoUbicacionPuesto,
                        FechaActualizacion = ((CUbicacionPuestoDTO)Q).FechaActualizacion,
                        ObsUbicacionPuesto = ((CUbicacionPuestoDTO)Q).ObsUbicacionPuesto,
                        TipoUbicacion = ((CUbicacionPuestoDTO)Q).TipoUbicacion
                    }).ToList();
                    return View(model);
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception error)
            {
                return View(model);
            }
        }

        //
        // GET: /Puesto/PedimentoDetails
        public ActionResult PedimentoRevisar(string codigo, string accion)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();

            var resultado = puestoReference.DescargarPuestoPedimento(codigo);

            if (accion == "aprobar")
            {
                EmailWebHelper correo = new EmailWebHelper
                {
                    Asunto = "APROBADO: Revisión del pedimento de puesto #" + codigo,
                    EmailBody = "Buenas tardes! </br> El usuario Deivert Guiltrichs, ha revisado el pedimento de puesto #"
                                + codigo + ".</br> El mismo fue APROBADO. Puede encontrar el detalle del mismo en el siguiente enlace:</br>" +
                                "<a href=\"http://localhost:32910/Puesto/PedimentoDetails?codigo=" + codigo + "\">Ver Pedimento</a>",
                    Destinos = "dguiltrc@mopt.go.cr"
                };
                correo.EnviarCorreo();
            }

            if (accion == "rechazar")
            {
                EmailWebHelper correo = new EmailWebHelper
                {
                    Asunto = "RECHAZADO: Revisión del pedimento de puesto #" + codigo,
                    EmailBody = "Buenas tardes! </br> El usuario Deivert Guiltrichs, ha revisado el pedimento de puesto #"
                                + codigo + ".</br> El mismo fue RECHAZADO. Puede encontrar el detalle del mismo en el siguiente enlace:</br>" +
                                "<a href=\"http://localhost:32910/Puesto/PedimentoDetails?codigo=" + codigo + "\">Ver Pedimento</a>",
                    Destinos = "dguiltrc@mopt.go.cr"
                };
                correo.EnviarCorreo();
            }

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.UbicacionContrato = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
                model.UbicacionPuesto = ((CUbicacionPuestoDTO)resultado.ElementAt(3));
                model.PedimentoPuesto = ((CPedimentoPuestoDTO)resultado.ElementAt(4));
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
            }

            return View(model);
        }

        //
        // GET: /Puesto/PedimentoDetails
        public ActionResult PedimentoDetails(string codigo, string accion)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();

            var resultado = puestoReference.DescargarPuestoPedimento(codigo);

            if (accion == "vistobueno")
            {
                EmailWebHelper correo = new EmailWebHelper
                {
                    Asunto = "Revisión del pedimento de puesto #" + codigo,
                    EmailBody = "Buenas tardes! </br> El usuario Deivert Guiltrichs, ha solicitado su revisión del pedimento de puesto #"
                                + codigo + ".</br> Puede encontrar el detalle del mismo en el siguiente enlace:</br>" +
                                "<a href=\"http://localhost:32910/Puesto/PedimentoRevisar?codigo=" + codigo + "\">Solicitar visto bueno</a>",
                    Destinos = "dguiltrc@mopt.go.cr"
                };
                correo.EnviarCorreo();
            }

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.UbicacionContrato = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
                model.UbicacionPuesto = ((CUbicacionPuestoDTO)resultado.ElementAt(3));
                model.PedimentoPuesto = ((CPedimentoPuestoDTO)resultado.ElementAt(4));
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
            }

            return View(model);
        }

        //
        // GET: /Puesto/CreatePedimento
        public ActionResult CreatePedimento(string codigo)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();
            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            var resultado = puestoReference.DescargarPuestoVacante(codigo);

            if (resultado.Count() > 1)
            {
                model.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.UbicacionPuesto = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
            }
            else
            {
                model.Error = ((CErrorDTO)resultado.FirstOrDefault());
            }

            return View(model);
        }

        //
        // POST: /Puesto/CreatePedimento
        [HttpPost]
        public ActionResult CreatePedimento(PedimentoPuestoVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = puestoReference.GuardarPedimentoPuesto(model.Puesto, model.PedimentoPuesto);

                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        int tipo = 0;
                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                        {
                            IdEntidad = 1 // Registrado
                        };

                        tipo = Convert.ToInt32(ETipoAccionesHelper.NombPropiedad);

                        CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = tipo
                        };

                        CAccionPersonalDTO accion = new CAccionPersonalDTO
                        {
                            AnioRige = DateTime.Now.Year,
                            CodigoModulo = Convert.ToInt32(EModulosHelper.Vacantes),
                            CodigoObjetoEntidad = Convert.ToInt32(((CRespuestaDTO)resultado).Contenido),
                            FecRige = model.PedimentoPuesto.FechaPedimento,
                            FecVence = null,
                            FecRigeIntegra = model.PedimentoPuesto.FechaPedimento,
                            FecVenceIntegra = null,
                            Observaciones = model.PedimentoPuesto.ObservacionesPedimento,
                            IndDato = model.PedimentoPuesto.IdEntidad
                        };

                        var respuestaAP = servicioAccion.AgregarAccion(model.Funcionario,
                                                                        estado,
                                                                        tipoAP,
                                                                        accion,
                                                                        null);

                        return JavaScript("window.location = '/Puesto/PedimentoDetails?codigo=" +
                                Convert.ToInt32(((CRespuestaDTO)resultado).Contenido) + "&accion=guardar" + "'");
                    }
                    else
                    {
                        ModelState.AddModelError("Guardar", ((CErrorDTO)resultado).MensajeError);
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return PartialView("_ErrorPuesto");
            }
        }

        //
        // GET: /Puesto/EditPedimento
        public ActionResult EditPedimento(string codigo)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();
            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            return View(model);
        }

        //
        // POST: /Puesto/EditPedimento
        [HttpPost]
        public ActionResult EditPedimento(PedimentoPuestoVM model)
        {
            //PROCESAR LOS DATOS DEL ESTUDIO DEL PUESTO
            return View(model);
        }

        //
        // GET: /Puesto/TareasPuestoDetails
        public ActionResult TareasPuestoDetails(string codigo)
        {
            TareasPuestoVM model = new TareasPuestoVM();

            var resultado = puestoReference.DescargarUbicacionTrabajoPedimento(codigo);

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.DetallePuesto = new DetallePuestoVM();
                model.DetallePuesto.Puesto = ((CPuestoDTO)resultado.ElementAt(0));
                model.DetallePuesto.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(1));
                model.DetallePuesto.UbicacionContrato = ((CUbicacionPuestoDTO)resultado.ElementAt(2));
                model.DetallePuesto.UbicacionTrabajo = ((CUbicacionPuestoDTO)resultado.ElementAt(3));
                model.DetallePuesto.PedimentoPuesto = ((CPedimentoPuestoDTO)resultado.ElementAt(4));
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
            }

            //DESCARGAR LOS DETALLES DEL PUESTO DESDE EL WS
            return View(model);
        }

        //
        // POST: /Puesto/TareasPuestoDetails
        [HttpPost]
        public ActionResult TareasPuestoDetails(TareasPuestoVM model)
        {
            //PROCESAR LOS DATOS DEL ESTUDIO DEL PUESTO
            return View(model);
        }

        #endregion

        #region R_MOVIMIENTO_PUESTO

        //
        // GET: /Puesto/DetalleMovimientoPuesto

        public ActionResult DetalleMovimientoPuesto(int codMovimiento)
        {
            try
            {
                MovimientoPuestoVM model = new MovimientoPuestoVM();

                var resultado = puestoReference.DetalleMovimiento(codMovimiento);

                if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.MovimientoPuesto = ((CMovimientoPuestoDTO)resultado.ElementAt(0));
                    model.Puesto = ((CPuestoDTO)resultado.ElementAt(1));
                    model.DetallePuesto = ((CDetallePuestoDTO)resultado.ElementAt(2));
                    model.Funcionario = ((CFuncionarioDTO)resultado.ElementAt(3));

                    NotificacionesEmailHelper.SendEmail("dguiltrc@mopt.go.cr",
                                    "Puesto # " + model.Puesto.CodPuesto + " - Vacante",
                                    "El puesto # " + model.Puesto.CodPuesto + " se ha declarado como vacante");

                    return View(model);
                }
                else
                {
                    throw new Exception("error");
                }
            }
            catch (Exception)
            {
                return View();
            }

        }

        #endregion

        public bool EntidadCompleta(CBaseDTO objeto)
        {
            bool respuesta = true;
            if (!String.IsNullOrEmpty(objeto.Mensaje))
            {
                respuesta = false;
            }
            return respuesta;
        }

        private PerfilFuncionarioVM ConstruirPerfil(CBaseDTO[][] perfil, PerfilFuncionarioVM modelo)
        {
            try
            {

                if (perfil.Length < 10)
                {
                    throw new Exception("modelo");
                }
                modelo.Funcionario = EntidadCompleta(perfil[0][0]) ? (CFuncionarioDTO)perfil[0][0] : null;
                //modelo.EstadoFuncionario = EntidadCompleta(perfil[0][1]) ? (CEstadoFuncionarioDTO)perfil[0][1] : null;
                modelo.EstadoCivil = new List<CHistorialEstadoCivilDTO>();
                if (perfil[1].Count() > 0)
                {
                    foreach (var item in perfil[1])
                    {
                        modelo.EstadoCivil.Add((CHistorialEstadoCivilDTO)item);
                    }
                }
                modelo.InformacionContacto = new List<CInformacionContactoDTO>();
                if (perfil[2].Count() > 0)
                {
                    foreach (var item in perfil[2])
                    {
                        modelo.InformacionContacto.Add((CInformacionContactoDTO)item);
                    }
                }
                modelo.Direccion = EntidadCompleta(perfil[3][0]) ? (CDireccionDTO)perfil[3][0] : null;
                modelo.ProvinciaDireccion = EntidadCompleta(perfil[3][1]) ? (CProvinciaDTO)perfil[3][1] : null;
                modelo.CantonDireccion = EntidadCompleta(perfil[3][2]) ? (CCantonDTO)perfil[3][2] : null;
                modelo.DistritoDireccion = EntidadCompleta(perfil[3][3]) ? (CDistritoDTO)perfil[3][3] : null;
                modelo.DetalleContrato = EntidadCompleta(perfil[4][0]) ? (CDetalleContratacionDTO)perfil[4][0] : null;
                modelo.Nombramiento = EntidadCompleta(perfil[5][0]) ? (CNombramientoDTO)perfil[5][0] : null;
                modelo.Calificaciones = new List<CCalificacionNombramientoDTO>();
                if (perfil[6].Count() > 0)
                {
                    foreach (var item in perfil[6])
                    {
                        modelo.Calificaciones.Add((CCalificacionNombramientoDTO)item);
                    }
                }
                modelo.Puesto = EntidadCompleta(perfil[7][0]) ? (CPuestoDTO)perfil[7][0] : null;
                modelo.EstadoPuesto = EntidadCompleta(perfil[7][1]) ? (CEstadoPuestoDTO)perfil[7][1] : null;
                modelo.ClasePuesto = EntidadCompleta(perfil[7][2]) ? (CClaseDTO)perfil[7][2] : null;
                modelo.EspecialidadPuesto = EntidadCompleta(perfil[7][3]) ? (CEspecialidadDTO)perfil[7][3] : null;
                modelo.SubEspecialidadPuesto = EntidadCompleta(perfil[7][4]) ? (CSubEspecialidadDTO)perfil[7][4] : null;
                modelo.OcupacionRealPuesto = EntidadCompleta(perfil[7][5]) ? (COcupacionRealDTO)perfil[7][5] : null;
                modelo.DetallePuesto = EntidadCompleta(perfil[7][6]) ? (CDetallePuestoDTO)perfil[7][6] : null;
                modelo.PresupuestoPuesto = EntidadCompleta(perfil[8][0]) ? (CPresupuestoDTO)perfil[8][0] : null;
                modelo.ProgramaPuesto = EntidadCompleta(perfil[8][1]) ? (CProgramaDTO)perfil[8][1] : null;
                //modelo.AreaPuesto = EntidadCompleta(perfil[8][2]) ? (CAreaDTO)perfil[8][2] : null;
                //modelo.ActividadPuesto = EntidadCompleta(perfil[8][3]) ? (CActividadDTO)perfil[8][3] : null;
                modelo.DivisionPuesto = EntidadCompleta(perfil[8][2]) ? (CDivisionDTO)perfil[8][2] : null;
                modelo.DireccionGeneralPuesto = EntidadCompleta(perfil[8][3]) ? (CDireccionGeneralDTO)perfil[8][3] : null;
                modelo.DepartamentoPuesto = EntidadCompleta(perfil[8][4]) ? (CDepartamentoDTO)perfil[8][4] : null;
                modelo.SeccionPuesto = EntidadCompleta(perfil[8][5]) ? (CSeccionDTO)perfil[8][5] : null;
                if (((CTipoUbicacionDTO)perfil[9][0]).IdEntidad.Equals(1))
                {
                    modelo.TipoUbicacionContrato = EntidadCompleta(perfil[9][0]) ? (CTipoUbicacionDTO)perfil[9][0] : null;
                    modelo.ProvinciaContrato = EntidadCompleta(perfil[9][1]) ? (CProvinciaDTO)perfil[9][1] : null;
                    modelo.CantonContrato = EntidadCompleta(perfil[9][2]) ? (CCantonDTO)perfil[9][2] : null;
                    modelo.DistritoContrato = EntidadCompleta(perfil[9][3]) ? (CDistritoDTO)perfil[9][3] : null;
                    modelo.UbicacionContrato = EntidadCompleta(perfil[9][4]) ? (CUbicacionPuestoDTO)perfil[9][4] : null;
                    modelo.TipoUbicacionTrabajo = EntidadCompleta(perfil[10][0]) ? (CTipoUbicacionDTO)perfil[10][0] : null;
                    modelo.ProvinciaTrabajo = EntidadCompleta(perfil[10][1]) ? (CProvinciaDTO)perfil[10][1] : null;
                    modelo.CantonTrabajo = EntidadCompleta(perfil[10][2]) ? (CCantonDTO)perfil[10][2] : null;
                    modelo.DistritoTrabajo = EntidadCompleta(perfil[10][3]) ? (CDistritoDTO)perfil[10][3] : null;
                    modelo.UbicacionTrabajo = EntidadCompleta(perfil[10][4]) ? (CUbicacionPuestoDTO)perfil[10][4] : null;
                }
                else
                {
                    modelo.TipoUbicacionContrato = EntidadCompleta(perfil[10][0]) ? (CTipoUbicacionDTO)perfil[10][0] : null;
                    modelo.ProvinciaContrato = EntidadCompleta(perfil[10][1]) ? (CProvinciaDTO)perfil[10][1] : null;
                    modelo.CantonContrato = EntidadCompleta(perfil[10][2]) ? (CCantonDTO)perfil[10][2] : null;
                    modelo.DistritoContrato = EntidadCompleta(perfil[10][3]) ? (CDistritoDTO)perfil[10][3] : null;
                    modelo.UbicacionContrato = EntidadCompleta(perfil[10][4]) ? (CUbicacionPuestoDTO)perfil[10][4] : null;
                    modelo.TipoUbicacionTrabajo = EntidadCompleta(perfil[9][0]) ? (CTipoUbicacionDTO)perfil[9][0] : null;
                    modelo.ProvinciaTrabajo = EntidadCompleta(perfil[9][1]) ? (CProvinciaDTO)perfil[9][1] : null;
                    modelo.CantonTrabajo = EntidadCompleta(perfil[9][2]) ? (CCantonDTO)perfil[9][2] : null;
                    modelo.DistritoTrabajo = EntidadCompleta(perfil[9][3]) ? (CDistritoDTO)perfil[9][3] : null;
                    modelo.UbicacionTrabajo = EntidadCompleta(perfil[9][4]) ? (CUbicacionPuestoDTO)perfil[9][4] : null;
                }

                return modelo;
            }
            catch (Exception error)
            {
                if (error.Message == "modelo")
                    return null;
                else
                    throw new Exception(error.ToString());
            }

        }

        public ActionResult Generar(FuncionarioModel funcionarios)
        {
            Session["errorP"] = null;
            try
            {
                PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                List<FuncionariosRptData> modelo1 = new List<FuncionariosRptData>();
                List<CFuncionarioDTO> fun = (List<CFuncionarioDTO>)Session["funcionarios"];
                foreach (var item in fun)
                {
                    var perfil = funcionarioReference.DescargarPerfilFuncionarioCompleto(item.Cedula);

                    modelo = ConstruirPerfil(perfil, modelo);

                    if (modelo == null) throw new Exception("modelo");

                    modelo1.Add(FuncionariosRptData.GenerarDatosReportePorFuncionarios(modelo));
                }

                return ReporteFuncionarios(modelo1);
            }
            catch (Exception error)
            {
                Session["errorP"] = "error";
                if (error.Message != "modelo")
                {
                    ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte de funcionarios, ponerse en contacto con el personal autorizado. \n");
                }
                else
                {
                    ModelState.AddModelError("modelo", "No existen datos del lugar de contrato ni el lugar de trabajo.\n\n Pongase en contacto con el personal autorizado.");

                }

                return View("Index");
            }

        }

        public ActionResult GenerarReportePuestos(FuncionarioPuestoVM funcionarios)
        {
            string style = @"<style> TD { mso-number-format:\@; } </style>";
            //var headerTable = @"<Table><tr><td><img src=""https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNqlnu1iMrzA69ctdlBJvavSQ8d8bKAUtilw&usqp=CAU"" \></td></tr></Table>";
            var products = new System.Data.DataTable("teste");

            //products.Rows.Add("SIRH", "Fecha:", DateTime.Now.ToShortDateString(), "Usuario:", principal.Identity.Name.Split('\\')[1], "", "", "", "", "");

            products.Columns.Add("NOMBRE OCUPANTE", typeof(string));
            products.Columns.Add("CEDULA OCUPANTE", typeof(string));
            products.Columns.Add("NOMBRE PROPIETARIO", typeof(string));
            products.Columns.Add("CEDULA PROPIETARIO", typeof(string));
            products.Columns.Add("PUESTO", typeof(string));
            products.Columns.Add("CLASE", typeof(string));
            products.Columns.Add("ESPECIALIDAD", typeof(string));
            products.Columns.Add("SUBESPECIALIDAD", typeof(string));
            products.Columns.Add("ESTADO", typeof(string));
            products.Columns.Add("NIVEL OCUPACIONAL", typeof(string));
            products.Columns.Add("RIGE NOMBRAMIENTO", typeof(string));
            products.Columns.Add("VENCE NOMBRAMIENTO", typeof(string));
            products.Columns.Add("DIVISION", typeof(string));
            products.Columns.Add("DIRECCION", typeof(string));
            products.Columns.Add("DEPARTAMENTO", typeof(string));
            products.Columns.Add("SECCION", typeof(string));

            List<CPuestoDTO> listaPuestos = (List<CPuestoDTO>)Session["puestos"];

            foreach (var item in listaPuestos)
            {
                products.Rows.Add(item.Nombramiento != null ? (item.Nombramiento.FecVence >= DateTime.Now || item.Nombramiento.FecVence.Year == 1) ? item.Nombramiento.Funcionario != null ? item.Nombramiento.Funcionario.Nombre + " " + item.Nombramiento.Funcionario.PrimerApellido + " " + item.Nombramiento.Funcionario.SegundoApellido : "No tiene" : "No tiene" : "No tiene",
                                  item.Nombramiento != null ? (item.Nombramiento.FecVence >= DateTime.Now || item.Nombramiento.FecVence.Year == 1) ? item.Nombramiento.Funcionario != null ? item.Nombramiento.Funcionario.Cedula : "No tiene" : "No tiene": "No tiene",
                                  item.Nombramiento != null ? item.Nombramiento.Mensaje != null ? item.Nombramiento.Mensaje.Split('-')[1] : "No tiene" : "No tiene",
                                  item.Nombramiento != null ? item.Nombramiento.Mensaje != null ? item.Nombramiento.Mensaje.Split('-')[0] : "No tiene" : "No tiene",
                                  item.CodPuesto,
                                  item.DetallePuesto.Clase.DesClase,
                                  item.DetallePuesto.Especialidad.DesEspecialidad,
                                  item.DetallePuesto.SubEspecialidad != null ? item.DetallePuesto.SubEspecialidad.DesSubEspecialidad : "",
                                  item.EstadoPuesto.DesEstadoPuesto,
                                  item.NivelOcupacional,
                                  item.Nombramiento != null && !item.EstadoPuesto.DesEstadoPuesto.StartsWith("VAC") ? item.Nombramiento.FecRige.ToShortDateString() : "",
                                  item.Nombramiento != null && !item.EstadoPuesto.DesEstadoPuesto.StartsWith("VAC") ? item.Nombramiento.FecVence != null ? item.Nombramiento.FecVence.Year != 1 ? item.Nombramiento.FecVence.ToShortDateString() : "" : "" : "",
                                  item.UbicacionAdministrativa.Division.NomDivision,
                                  item.UbicacionAdministrativa.DireccionGeneral != null ? item.UbicacionAdministrativa.DireccionGeneral.NomDireccion : "",
                                  item.UbicacionAdministrativa.Departamento != null ? item.UbicacionAdministrativa.Departamento.NomDepartamento : "",
                                  item.UbicacionAdministrativa.Seccion.NomSeccion
                                  );
            }

            products.Rows.Add("", "", "", "", "", "", "", "", "", "");

            products.Rows.Add("SIRH", "Fecha:", DateTime.Now.ToShortDateString(), "Usuario:", principal.Identity.Name.Split('\\')[1], "", "", "", "", "");

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.AddHeader("Transfer-Encoding", "identity");
            Response.ContentType = "application/ms-excel";

            //Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);
            Response.Write(style);
            //Response.Write(headerTable);
            Response.Write(sw.ToString());
            Response.Flush();
            Response.Close();
            Response.End();
            return View();
        }

        private CrystalReportPdfResult ReporteFuncionarios(List<FuncionariosRptData> modelo)
        {
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Funcionarios"), "ReporteFuncionarios.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "EXCEL");
        }


        public ActionResult OrdenMovimiento()
        {
            OrdenMovimientoVM model = new OrdenMovimientoVM();
            model.Funcionario = new CFuncionarioDTO();
            return View(model);
        }

        [HttpPost]
        public ActionResult OrdenMovimiento(OrdenMovimientoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (!String.IsNullOrEmpty(model.Funcionario.Cedula) || !String.IsNullOrEmpty(model.Puesto.CodPuesto))
                    {
                        var cedula = model.Funcionario.Cedula;
                        var datos = puestoReference.DatosOrdenMovimiento(cedula, model.Puesto.CodPuesto);

                        if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                            throw new Exception("error");
                        }
                        else
                        {
                            model.Orden = new COrdenMovimientoDTO();
                            model.Declaracion = new COrdenMovimientoDeclaracionDTO();

                            var datosOrden = OrdenMovimientoHelper.datosOrden();
                            // Buscar Pedimento Puesto
                            var datoPedimento = puestoReference.BuscarPedimentosPorPuesto(model.Puesto);
                            if (datoPedimento.FirstOrDefault().GetType().Equals(typeof(CErrorDTO)))
                            {
                                //ModelState.Clear();
                                //ModelState.AddModelError("Sistema", ((CErrorDTO)datoPedimento.ElementAt(0)).MensajeError);
                                //throw new Exception("Se debe registrar un Pedimento antes de realizar una Orden de Movimiento");
                                model.Orden.Pedimento = new CPedimentoPuestoDTO();
                            }
                            else
                            {
                                model.Orden.Pedimento = (CPedimentoPuestoDTO)datoPedimento[3];
                            }

                            model.Orden.DesObservaciones = datosOrden[0].ToString();
                            model.Orden.DesPartidaPresupuestaria = datosOrden[1].ToString();
                            model.Declaracion.Academica = datosOrden[2].ToString();
                            model.Declaracion.Experiencia = datosOrden[3].ToString();
                            model.Declaracion.Capacitacion = datosOrden[4].ToString();
                            model.Declaracion.Licencias = datosOrden[5].ToString();
                            model.Declaracion.Colegiaturas = datosOrden[6].ToString();

                            model.Funcionario = (CFuncionarioDTO)datos[0];

                            if (model.Funcionario.Cedula == "0")
                            {
                                wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
                                try
                                {
                                    string cedulaTSE = FuncionarioHelper.CedulaEmulacionATSE(cedula);
                                    var persona = servicioTSE.wsConsultaDatosPersona(0, cedulaTSE, true);
                                    model.Funcionario = FuncionarioHelper.ConvertirPersonaTSEAFuncionario(persona);
                                }
                                catch
                                {
                                    model.Funcionario = new CFuncionarioDTO { IdEntidad = 0, Cedula = "", Nombre = "", PrimerApellido = "", SegundoApellido = "" };
                                }
                            }
                            else
                            {
                                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                            }

                            //model.Funcionario.Nombre = model.Funcionario.PrimerApellido.TrimEnd() + " " +
                            //                            model.Funcionario.SegundoApellido.TrimEnd() + " " +
                            //                            model.Funcionario.Nombre.TrimEnd();

                            //model.Orden.FuncionarioSustituido.Nombre = model.Orden.FuncionarioSustituido.PrimerApellido.TrimEnd() + " " +
                            //                          model.Orden.FuncionarioSustituido.SegundoApellido.TrimEnd() + " " +
                            //                          model.Orden.FuncionarioSustituido.Nombre.TrimEnd();

                            var resultado = servicioUsuario.RetornarPerfilUsuario(principal.Identity.Name);
                            var usuario = (CUsuarioDTO)resultado.ElementAt(0).ElementAt(0);
                            model.Orden.FuncionarioResponsable = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(0);

                            model.Orden.FuncionarioResponsable.Nombre = model.Orden.FuncionarioResponsable.Nombre.TrimEnd() + " " +
                                                       model.Orden.FuncionarioResponsable.PrimerApellido.TrimEnd() + " " +
                                                       model.Orden.FuncionarioResponsable.SegundoApellido.TrimEnd();

                            var listaJefes = OrdenMovimientoHelper.ListaJefes();

                            //model.Orden.FuncionarioRevision = new CFuncionarioDTO
                            //{
                            //    IdEntidad = listaJefes[0].Id,
                            //    Cedula = listaJefes[0].Cedula,
                            //    Nombre = listaJefes[0].Nombre,
                            //    Mensaje = listaJefes[0].Correo,
                            //    Sexo = GeneroEnum.Indefinido
                            //};

                            //model.Orden.FuncionarioJefatura = new CFuncionarioDTO
                            //{
                            //    IdEntidad = listaJefes[1].Id,
                            //    Cedula = listaJefes[1].Cedula,
                            //    Nombre = listaJefes[1].Nombre,
                            //    Mensaje = listaJefes[1].Correo,
                            //    Sexo = GeneroEnum.Indefinido
                            //};


                            //model.Orden.FuncionarioRevision.Nombre = model.Orden.FuncionarioRevision.Nombre.TrimEnd() + " " +
                            //                            model.Orden.FuncionarioRevision.PrimerApellido.TrimEnd() + " " +
                            //                            model.Orden.FuncionarioRevision.SegundoApellido.TrimEnd();

                            //model.Orden.FuncionarioJefatura.Nombre = model.Orden.FuncionarioJefatura.Nombre.TrimEnd() + " " +
                            //                            model.Orden.FuncionarioJefatura.PrimerApellido.TrimEnd() + " " +
                            //                            model.Orden.FuncionarioJefatura.SegundoApellido.TrimEnd();

                            model.Puesto = (CPuestoDTO)datos[1];
                            model.ObservacionesNombramiento = ((CNombramientoDTO)datos[2]).Mensaje;


                            // model.ObservacionesNombramiento = datos[11].ToString().TrimEnd();
                            //List<SelectListItem> listadoTipos = new List<SelectListItem>();
                            //SelectListItem selListItem = new SelectListItem() { Value = "Cese de Interinidad", Text = "Cese de Interinidad" };
                            //listadoTipos.Add(selListItem);

                            //model.Tipos = new SelectList(listadoTipos, "Value", "Text");

                            var listadoMotivos = puestoReference.ListarMotivosMovimiento()
                              .Select(Q => new SelectListItem
                              {
                                  Value = ((CMotivoMovimientoDTO)Q).IdEntidad.ToString(),
                                  Text = ((CMotivoMovimientoDTO)Q).DesMotivo
                              });

                            model.Tipos = new SelectList(listadoMotivos, "Value", "Text");
                            model.Motivos = new SelectList(listadoMotivos, "Value", "Text");

                            return PartialView("OrdenMovimientoResult", model);
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Busqueda", "Los parámetros ingresados no son suficientes para realizar la búsqueda.");
                        throw new Exception("Busqueda");
                    }
                }


                if (SubmitButton == "Guardar")
                {
                    var listadoMotivos = puestoReference.ListarMotivosMovimiento()
                              .Select(Q => new SelectListItem
                              {
                                  Value = ((CMotivoMovimientoDTO)Q).IdEntidad.ToString(),
                                  Text = ((CMotivoMovimientoDTO)Q).DesMotivo
                              });

                    model.Tipos = new SelectList(listadoMotivos, "Value", "Text");
                    model.Motivos = new SelectList(listadoMotivos, "Value", "Text");

                    // Validar Campos
                    ModelState.Clear();
                    bool validado = true;
                    if (!(model.Orden.FecRige.Year > 0001))
                    {
                        ModelState.AddModelError("formulario", "Debe indicar la Fecha Rige");
                        validado = false;
                    }
                    if (!(model.Declaracion.FechaCertificacion.Year > 0001))
                    {
                        ModelState.AddModelError("formulario", "Debe indicar la Fecha de la Certificación");
                        validado = false;
                    }
                    if (model.TipoSeleccionado == null || model.TipoSeleccionado == "")
                    {
                        ModelState.AddModelError("formulario", "Debe indicar Tipo de Movimiento");
                        validado = false;
                    }
                    if (model.MotivoSeleccionado == null || model.MotivoSeleccionado == "")
                    {
                        ModelState.AddModelError("formulario", "Debe indicar el Motivo del Movimiento");
                        validado = false;
                    }

                    if (!validado)
                        return PartialView("OrdenMovimientoResult", model);

                    //Guardar datos
                    model.Orden.FuncionarioOrden = model.Funcionario;
                    model.Orden.DetallePuesto = model.Puesto.DetallePuesto;
                    model.Orden.TipoMovimiento = new CMotivoMovimientoDTO { IdEntidad = Convert.ToInt32(model.TipoSeleccionado) };
                    model.Orden.MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = Convert.ToInt32(model.MotivoSeleccionado) };

                    model.Orden.FuncionarioRevision = new CFuncionarioDTO();
                    model.Orden.FuncionarioJefatura = new CFuncionarioDTO();

                    model.Orden.FuncionarioOrden.Sexo = GeneroEnum.Indefinido;


                    if (model.Orden.FuncionarioSustituido.Cedula != null && model.Orden.FuncionarioSustituido.Cedula != "")
                    {
                        var datosFuncionario = funcionarioReference.BuscarFuncionarioBase(model.Orden.FuncionarioSustituido.Cedula);
                        model.Orden.FuncionarioSustituido = (CFuncionarioDTO)datosFuncionario;
                    }
                    else
                    {
                        model.Orden.FuncionarioSustituido.Sexo = GeneroEnum.Indefinido;
                    }

                    model.Orden.FuncionarioResponsable.Sexo = GeneroEnum.Indefinido;
                    model.Orden.FuncionarioRevision.Sexo = GeneroEnum.Indefinido;
                    model.Orden.FuncionarioJefatura.Sexo = GeneroEnum.Indefinido;

                    var respuesta = puestoReference.AgregarOrden(model.Orden, model.Declaracion);
                    //var respuesta = new CErrorDTO();
                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        if (((CRespuestaDTO)respuesta).Codigo > 0)
                        {
                            List<string> entidades = new List<string>();
                            entidades.Add(typeof(COrdenMovimientoDTO).Name);

                            //context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal),
                            //        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                            //        CAccesoWeb.ListarEntidades(entidades.ToArray()));

                            EmailWebHelper correo = new EmailWebHelper
                            {
                                Asunto = "REGISTRO: Se registró una Orden de Movimiento a nombre de " + model.Funcionario.PrimerApellido.TrimEnd() + " "
                                                           + model.Funcionario.SegundoApellido.TrimEnd() + " "
                                                           + model.Funcionario.Nombre.TrimEnd() + " " +
                                                           model.Funcionario.Cedula,
                                EmailBody = "El funcionario " + model.Orden.FuncionarioResponsable.Nombre.TrimEnd() + ", ha registrado la orden de movimiento"
                               + "para su respectiva revisión:</br>",
                                Destinos = model.Orden.FuncionarioRevision.Mensaje

                                //Asunto = "APROBADO: Revisión del pedimento de puesto #" + codigo,
                                //EmailBody = "Buenas tardes! </br> El usuario Deivert Guiltrichs, ha revisado el pedimento de puesto #"
                                //        + codigo + ".</br> El mismo fue APROBADO. Puede encontrar el detalle del mismo en el siguiente enlace:</br>" +
                                //        "<a href=\"http://localhost:32910/Puesto/PedimentoDetails?codigo=" + codigo + "\">Ver Pedimento</a>",
                                //Destinos = "dguiltrc@mopt.go.cr"
                            };
                            correo.EnviarCorreo();

                            return JavaScript("window.location = '/Puesto/OrdenDetails?codigo=" + ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "';");
                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", respuesta.Mensaje + " Cont 1");
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Agregar", respuesta.Mensaje + " Cont 2");
                        throw new Exception(respuesta.Mensaje);
                    }
                }

                return null;
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    ModelState.AddModelError("Busqueda", error.Message);
                    return PartialView("_ErrorPuesto");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", error.Message);
                    return PartialView("_ErrorPuesto");
                }
            }
        }

        public ActionResult OrdenDetails(string codigo)
        {
            try
            {
                OrdenMovimientoVM model = new OrdenMovimientoVM();
                var datos = puestoReference.ObtenerOrden(Convert.ToInt32(codigo));

                if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    throw new Exception("error");
                }
                else
                {
                    var usr = principal.Identity.Name;
                    var resultado = servicioUsuario.RetornarPerfilUsuario(usr);
                    var usuario = (CUsuarioDTO)resultado.ElementAt(0).ElementAt(0);
                    var funcionario = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(0);

                    context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.ViaticoCorrido), 0);

                    if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.ViaticoCorrido)].ToString().StartsWith("Error"))
                    {
                        model.permisoAprobar = false;
                        model.permisoRevisar = false;
                    }
                    else
                    {
                        if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                            Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.ViaticoCorrido)]))
                        {
                            model.permisoRevisar = true;
                            model.permisoAprobar = true;
                        }

                        if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.ViaticoCorrido, Convert.ToInt32(ENivelesViaticoCorrido.Operativo))] != null)
                            model.permisoRevisar = true;

                        if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.ViaticoCorrido, Convert.ToInt32(ENivelesViaticoCorrido.Aprobador))] != null)
                            model.permisoAprobar = true;
                    }

                    model.Orden = (COrdenMovimientoDTO)datos[0];
                    model.Declaracion = (COrdenMovimientoDeclaracionDTO)datos[1];
                    model.Puesto = (CPuestoDTO)datos[2];
                    model.Puesto.DetallePuesto = (CDetallePuestoDTO)datos[3];

                    if (model.permisoRevisar)  // Usuario Revision
                    {
                        model.Orden.FuncionarioRevision = funcionario;
                        //model.Orden.FuncionarioRevision.Nombre = model.Orden.FuncionarioRevision.Nombre.TrimEnd() + " " +
                        //                                   model.Orden.FuncionarioRevision.PrimerApellido.TrimEnd() + " " +
                        //                                   model.Orden.FuncionarioRevision.SegundoApellido.TrimEnd();
                    }

                    if (model.permisoAprobar)  // Usuario Jefatura
                    {
                        model.Orden.FuncionarioRevision = funcionario;
                        //model.Orden.FuncionarioJefatura.Nombre = model.Orden.FuncionarioJefatura.Nombre.TrimEnd() + " " +
                        //                       model.Orden.FuncionarioJefatura.PrimerApellido.TrimEnd() + " " +
                        //                       model.Orden.FuncionarioJefatura.SegundoApellido.TrimEnd();
                    }

                    return View(model);
                }

            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return View("_ErrorPuesto");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", error.Message);
                    return View("_ErrorPuesto");
                }
            }
        }

        [HttpPost]
        public ActionResult OrdenDetails(OrdenMovimientoVM model, string SubmitButton)
        {
            try
            {
                ModelState.Clear();
                model.Orden.FuncionarioOrden.Sexo = GeneroEnum.Indefinido;
                model.Orden.FuncionarioSustituido.Sexo = GeneroEnum.Indefinido;
                model.Orden.FuncionarioResponsable.Sexo = GeneroEnum.Indefinido;
                model.Orden.FuncionarioRevision.Sexo = GeneroEnum.Indefinido;
                model.Orden.FuncionarioJefatura.Sexo = GeneroEnum.Indefinido;

                if (SubmitButton == "Revisar")
                {
                    model.Orden.Estado.IdEntidad = 2; //Revisado
                    var respuesta = puestoReference.ActualizarOrden(model.Orden);

                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Sistema", ((CErrorDTO)respuesta).MensajeError);
                        throw new Exception("error");
                    }
                    else
                    {

                    }
                }

                if (SubmitButton == "Aprobar")
                {
                    model.Orden.Estado.IdEntidad = 3; //Aprobar
                    var respuesta = puestoReference.ActualizarOrden(model.Orden);

                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Sistema", ((CErrorDTO)respuesta).MensajeError);
                        throw new Exception("error");
                    }
                    else
                    {

                    }
                }

                if (SubmitButton == "PDForden" || SubmitButton == "PDFdeclaracion")
                {
                    return ReporteOrdenMovimiento(model, SubmitButton);
                }

                return null;
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorPuesto");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", error.Message);
                    return PartialView("_ErrorPuesto");
                }
            }
        }

        public ActionResult OrdenSearch()
        {
            try
            {
                BusquedaOrdenVM model = new BusquedaOrdenVM();
                var listadoMotivos = puestoReference.ListarMotivosMovimiento()
                              .Select(Q => new SelectListItem
                              {
                                  Value = ((CMotivoMovimientoDTO)Q).IdEntidad.ToString(),
                                  Text = ((CMotivoMovimientoDTO)Q).DesMotivo
                              });


                var listadoEstados = puestoReference.ListarOrdenEstados()
                              .Select(Q => new SelectListItem
                              {
                                  Value = ((CEstadoOrdenDTO)Q).IdEntidad.ToString(),
                                  Text = ((CEstadoOrdenDTO)Q).DesEstado
                              });

                model.Tipos = new SelectList(listadoMotivos, "Value", "Text");
                model.Estados = new SelectList(listadoEstados, "Value", "Text");

                return View(model);
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return View("_ErrorPuesto");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", error.Message);
                    return View("_ErrorPuesto");
                }
            }
        }

        [HttpPost]
        public ActionResult OrdenSearch(BusquedaOrdenVM model)
        {
            try
            {
                if (!String.IsNullOrEmpty(model.Cedula)
                    || !String.IsNullOrEmpty(model.Puesto.CodPuesto)
                    || !String.IsNullOrEmpty(model.Orden.NumOrden)
                    || (model.FechaRige.Year > 1 && model.FechaVence.Year > 1)
                    || !String.IsNullOrEmpty(model.EstadoSeleccionado)
                    || !String.IsNullOrEmpty(model.TipoSeleccionado))
                {
                    List<DateTime> fechas = new List<DateTime>();

                    if (model.FechaRige.Year > 1 && model.FechaVence.Year > 1)
                    {
                        fechas.Add(model.FechaRige);
                        fechas.Add(model.FechaVence);
                    }
                    else
                    {
                        fechas.Add(new DateTime(1, 1, 1));
                        fechas.Add(new DateTime(1, 1, 1));
                    }

                    //  Tipo de Movimiento
                    if (Convert.ToInt32(model.TipoSeleccionado) > 0)
                    {
                        CMotivoMovimientoDTO tipoMov = new CMotivoMovimientoDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };
                        model.Orden.TipoMovimiento = tipoMov;
                    }

                    //  Estado
                    if (Convert.ToInt32(model.EstadoSeleccionado) > 0)
                    {
                        CEstadoOrdenDTO estado = new CEstadoOrdenDTO
                        {
                            IdEntidad = Convert.ToInt32(model.EstadoSeleccionado)
                        };
                        model.Orden.Estado = estado;
                    }

                    model.Orden.FuncionarioOrden = new CFuncionarioDTO { Cedula = model.Cedula, Sexo = GeneroEnum.Indefinido };
                    var datos = puestoReference.BuscarOrdenes(model.Orden, fechas.ToArray());

                    if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.Ordenes = new List<OrdenMovimientoVM>();

                        foreach (var item in datos)
                        {
                            OrdenMovimientoVM temp = new OrdenMovimientoVM();
                            temp.Orden = (COrdenMovimientoDTO)item;
                            temp.Puesto = temp.Orden.DetallePuesto.Puesto;
                            temp.Funcionario = temp.Orden.FuncionarioOrden;

                            model.Ordenes.Add(temp);
                        }

                        return PartialView("_SearchOrden", model.Ordenes);
                    }
                }

                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Sistema", "Los parámetros ingresados no son suficientes para realizar la búsqueda.");
                    throw new Exception("formulario");
                }

            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorPuesto");
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", error.Message);
                    return PartialView("_ErrorPuesto");
                }
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteOrdenMovimiento(OrdenMovimientoVM model, string SubmitButton)
        {
            string reportPath = "0";
            
            try
            {
                List<OrdenMovimientoRPTData> modelo = new List<OrdenMovimientoRPTData>();
                modelo.Add(OrdenMovimientoRPTData.GenerarDatosReporte(model, 0, String.Empty));
                if (SubmitButton == "PDForden")
                    reportPath = Path.Combine(Server.MapPath("~/Reports/Puesto"), "OrdenMovimientoRPT.rpt");
                else
                    reportPath = Path.Combine(Server.MapPath("~/Reports/Puesto"), "DeclaracionJuradaRPT.rpt");

                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(reportPath + " " + ex.InnerException.Message);
            }
        }

        public ActionResult EditPuesto(string codpuesto = null)
        {
            DetallePuestoVM model = new DetallePuestoVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPuesto(DetallePuestoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    var resultado = puestoReference.DatosUbicacionDetallePuesto(model.Puesto.CodPuesto);

                    var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
                    model.CantonesTrabajo = model.CantonesContrato = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
                    model.DistritosTrabajo = model.DistritosContrato = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
                    model.ProvinciasTrabajo = model.ProvinciasContrato = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));

                    if (resultado.ElementAt(0).GetType() != typeof(CErrorDTO))
                    {
                        model.Puesto = (CPuestoDTO)resultado[0];
                        model.DetallePuesto = (CDetallePuestoDTO)resultado[1];
                        model.UbicacionContrato = (CUbicacionPuestoDTO)resultado[2];
                        model.UbicacionContrato.Mensaje = model.UbicacionContrato.Distrito.Canton.Provincia.NomProvincia + "-" + model.UbicacionContrato.Distrito.Canton.NomCanton + "-" + model.UbicacionContrato.Distrito.NomDistrito;
                        model.UbicacionTrabajo = (CUbicacionPuestoDTO)resultado[3];
                        model.UbicacionTrabajo.Mensaje = model.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia + "-" + model.UbicacionTrabajo.Distrito.Canton.NomCanton + "-" + model.UbicacionTrabajo.Distrito.NomDistrito;


                        if (resultado.Count() > 5)
                        {
                            model.Funcionario = (CFuncionarioDTO)resultado[5];
                            model.Funcionario.Mensaje = "Este puesto tiene un nombramiento activo de tipo: " + ((CNombramientoDTO)resultado[4]).EstadoNombramiento.DesEstado;
                            model.Funcionario.Mensaje += ". Para el funcionario: " + model.Funcionario.Cedula + " - " + model.Funcionario.Nombre + " " + model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido;
                            model.Funcionario.Mensaje += ". Por lo que no puede modificar desde esta pantalla la información de Clase y Especialidad. Para esto debe realizar una acción de personal de Reasignación";
                        }
                        else
                        {
                            model.Puesto.Mensaje = ((CBaseDTO)resultado[4]).Mensaje;
                        }

                        model.ListaNivelOcupacional = new SelectList(Enum.GetValues(typeof(Helpers.ENivelOcupacional)).Cast<ENivelOcupacional>().Select(v => new SelectListItem
                        {
                            Text = v.ToString(),
                            Value = ((int)v).ToString()
                        }).ToList(), "Value", "Text");

                        model.NivelSeleccionado = Convert.ToInt32(model.Puesto.NivelOcupacional.ToString());

                        Session["modeloPuesto"] = model;

                        //return View();

                        return PartialView("_ResultsEditPuesto", model);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado[0]).MensajeError);
                    }
                }
                else
                {
                    CDetallePuestoDTO detallePuesto = null;
                    CUbicacionAdministrativaDTO ubicacionAdmin = null;
                    CUbicacionPuestoDTO ubicacionContrato = null;
                    CUbicacionPuestoDTO ubicacionTrabajo = null;
                    CPuestoDTO puestoCambio = model.Puesto;

                    if (PuestoHasChanges(model, "detallePuesto"))
                    {
                        detallePuesto = model.DetallePuesto;
                    }
                    if (PuestoHasChanges(model, "ubicacionAdmin"))
                    {
                        ubicacionAdmin = model.Puesto.UbicacionAdministrativa;
                    }
                    if (PuestoHasChanges(model, "ubicacionContrato"))
                    {
                        ubicacionContrato = model.UbicacionContrato;
                    }
                    if (PuestoHasChanges(model, "ubicacionTrabajo"))
                    {
                        ubicacionTrabajo = model.UbicacionTrabajo;
                    }
                    if (PuestoHasChanges(model, "nivel"))
                    {
                        puestoCambio.NivelOcupacional = model.NivelSeleccionado;
                        puestoCambio.Mensaje = "Cambio";
                    }

                    var resultado = puestoReference.ActualizarDatosPuesto(detallePuesto, ubicacionAdmin, ubicacionContrato, ubicacionTrabajo, puestoCambio);

                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/Puesto/Details?codPuesto=" +
                                            ((CPuestoDTO)resultado).CodPuesto + "'");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado).MensajeError);
                    }
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Error", error.Message);
                return PartialView("_ErrorPuesto");
            }
        }

        private bool PuestoHasChanges(DetallePuestoVM model, string elemento)
        {
            var modeloOriginal = (DetallePuestoVM)Session["modeloPuesto"];
            switch (elemento)
            {
                case "detallePuesto":
                    if ((model.DetallePuesto.Clase.DesClase != modeloOriginal.DetallePuesto.Clase.DesClase)
                        || (model.DetallePuesto.Especialidad.DesEspecialidad != modeloOriginal.DetallePuesto.Especialidad.DesEspecialidad)
                        || (model.DetallePuesto.OcupacionReal.DesOcupacionReal != modeloOriginal.DetallePuesto.OcupacionReal.DesOcupacionReal)
                        || (model.DetallePuesto.SubEspecialidad.DesSubEspecialidad != modeloOriginal.DetallePuesto.SubEspecialidad.DesSubEspecialidad))
                    {
                        return true;
                    }
                    break;
                case "ubicacionAdmin":
                    if ((model.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento != modeloOriginal.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento)
                        || (model.Puesto.UbicacionAdministrativa.Division.NomDivision != modeloOriginal.Puesto.UbicacionAdministrativa.Division.NomDivision)
                        || (model.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion != modeloOriginal.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion)
                        || (model.Puesto.UbicacionAdministrativa.Seccion.NomSeccion != modeloOriginal.Puesto.UbicacionAdministrativa.Seccion.NomSeccion)
                        || (model.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto != modeloOriginal.Puesto.UbicacionAdministrativa.Presupuesto.CodigoPresupuesto))
                    {
                        return true;
                    }
                    break;
                case "ubicacionContrato":
                    if (model.UbicacionContrato.Mensaje != modeloOriginal.UbicacionContrato.Mensaje)
                    {
                        return true;
                    }
                    break;
                case "ubicacionTrabajo":
                    if (model.UbicacionTrabajo.Mensaje != modeloOriginal.UbicacionTrabajo.Mensaje)
                    {
                        return true;
                    }
                    break;
                case "nivel":
                    if (model.NivelSeleccionado != modeloOriginal.NivelSeleccionado && model.NivelSeleccionado != 0)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        [HttpPost]
        public ActionResult GetNombre(string cedula)
        {
            try
            {
                var datosFuncionario = funcionarioReference.BuscarFuncionarioBase(cedula);
                var funcionario = (CFuncionarioDTO)datosFuncionario;
                return Json(new
                {
                    success = true,
                    id = funcionario.IdEntidad,
                    nombre = funcionario.Cedula + " - " +
                                funcionario.PrimerApellido.TrimEnd() + " " +
                                funcionario.SegundoApellido.TrimEnd() + " " +
                                funcionario.Nombre.TrimEnd()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private CrystalReportPdfResult ReportePuestos(List<PuestosRptData> modelo)
        {
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Puesto"), "ListaPuestos.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "EXCEL");
        }

        //public ActionResult Generar(FuncionarioModel funcionarios)
        //{
        //    Session["errorP"] = null;
        //    try
        //    {
        //        PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
        //        List<FuncionariosRptData> modelo1 = new List<FuncionariosRptData>();
        //        List<CFuncionarioDTO> fun = (List<CFuncionarioDTO>)Session["funcionarios"];
        //        foreach (var item in fun)
        //        {
        //            var perfil = funcionarioReference.DescargarPerfilFuncionarioCompleto(item.Cedula);

        //            modelo = ConstruirPerfil(perfil, modelo);

        //            if (modelo == null) throw new Exception("modelo");

        //            modelo1.Add(FuncionariosRptData.GenerarDatosReportePorFuncionarios(modelo));
        //        }

        //        return ReporteFuncionarios(modelo1);
        //    }
        //    catch (Exception error)
        //    {
        //        Session["errorP"] = "error";
        //        if (error.Message != "modelo")
        //        {
        //            ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte de funcionarios, ponerse en contacto con el personal autorizado. \n");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("modelo", "No existen datos del lugar de contrato ni el lugar de trabajo.\n\n Pongase en contacto con el personal autorizado.");

        //        }

        //        return View("Index");
        //    }

        //}

        //private CrystalReportPdfResult ReporteFuncionarios(List<FuncionariosRptData> modelo)
        //{
        //    string reportPath = Path.Combine(Server.MapPath("~/Reports/Funcionarios"), "ReporteFuncionarios.rpt");
        //    return new CrystalReportPdfResult(reportPath, modelo, "EXCEL");
        //}
    }
}
