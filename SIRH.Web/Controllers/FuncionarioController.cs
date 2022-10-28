using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using SIRH.Web.CalificacionService;
using SIRH.Web.DesarraigoLocal;
//using SIRH.Web.DesarraigoService;
using SIRH.Web.FuncionarioLocal;
using SIRH.Web.PuestoLocal;
//using SIRH.Web.FuncionarioService;
//using SIRH.Web.PuestoService;
using SIRH.Web.Models;
using SIRH.Web.ViewModels;
using SIRH.DTO;
using SIRH.Web.ServicioTSE;
using SIRH.Web.Helpers;
using SIRH.Web.Reports.Funcionarios;
using SIRH.Web.Reports.PDF;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using SIRH.Web.AntecedentesPenalesService;


namespace SIRH.Web.Controllers
{
    public class FuncionarioController : Controller
    {
        CFuncionarioServiceClient funcionarioReference = new CFuncionarioServiceClient();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CPuestoServiceClient puestoLocal = new CPuestoServiceClient();
        CDesarraigoServiceClient ServicioDesarraigo = new CDesarraigoServiceClient();

        //
        // GET: /Funcionario/
        //Dato para check in.
        //public ActionResult Index(/*string cedula, string nombre, string apellido1, string apellido2,
        //                          string codpuesto, string codclase, string codespecialidad,string codocupacion,
        //                          string coddivision, string coddireccion, string coddepartamento, string codseccion,
        //                          string codpresupuesto, string codscostos,*/ int? page)
        //{
        //    Session["errorF"] = null;
        //    FuncionarioModel modelo = new FuncionarioModel();
        //    modelo.Parametros = new List<bool> { true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false };
        //    return View();
        //    //try
        //    //{
        //    //    FuncionarioModel modelo = new FuncionarioModel();
        //    //    int paginaActual = page.HasValue ? page.Value : 1;


        //    //    if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(apellido1) &&
        //    //        String.IsNullOrEmpty(apellido2) && String.IsNullOrEmpty(codpuesto) && String.IsNullOrEmpty(codclase) &&
        //    //        String.IsNullOrEmpty(codespecialidad) && String.IsNullOrEmpty(codocupacion) && String.IsNullOrEmpty(coddivision) &&
        //    //        String.IsNullOrEmpty(coddireccion) && String.IsNullOrEmpty(coddepartamento) && String.IsNullOrEmpty(codseccion) && String.IsNullOrEmpty(codpresupuesto))
        //    //    {
        //    //        page = 1;
        //    //        return View();
        //    //    }
        //    //    else
        //    //    {
        //    //        if (cedula == null)
        //    //        {
        //    //            cedula = "";
        //    //        }
        //    //        modelo.CedulaSearch = cedula;
        //    //        modelo.NombreSearch = nombre;
        //    //        modelo.PrimerApellidoSearch = apellido1;
        //    //        modelo.SegundoApellidoSearch = apellido2;
        //    //        var funcionarios = funcionarioReference.BuscarFuncionarioCompleto(cedula, nombre, apellido1, apellido2, codpuesto, codclase, codespecialidad, codocupacion, coddivision,
        //    //                                                                          coddireccion, coddepartamento, codseccion, codpresupuesto);
        //    //        modelo.TotalFuncionarios = funcionarios.Count();
        //    //        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
        //    //        modelo.PaginaActual = paginaActual;
        //    //        Session["funcionarios"] = funcionarios.ToList();
        //    //        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
        //    //        {
        //    //            modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
        //    //        }
        //    //        else
        //    //        {
        //    //            modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
        //    //        }


        //    //        return PartialView("Index_Result", modelo);
        //    //    }
        //    //}
        //    //catch (Exception error)
        //    //{
        //    //    ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n" + error.ToString());
        //    //    return PartialView("_ErrorFuncionario");
        //    //}
        //}

        public PartialViewResult CentroCostos_Index(string codigocostos)
        {
            Session["errorF"] = null;
            try
            {
                CentroCostosModel modelo = new CentroCostosModel();

                if (String.IsNullOrEmpty(codigocostos))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigocostos;
                    var codigosResult = puestoLocal.BuscarCentroCostosParams(codigocostos.Trim());
                    List<string> listcodigos = new List<string>();
                    foreach (var item in codigosResult)
                    {
                        listcodigos.Add(item.Mensaje);
                    }
                    modelo.CentrosCostosList = new SelectList(listcodigos);
                    return PartialView("CentroCostos_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorFuncionario");
            }
        }

        public ActionResult Index(FuncionarioModel modelo, string cedula, string nombre, string apellido1, string apellido2,
                                  string codpuesto, string codclase, string codespecialidad, string codnivel, string codestado,
                                  string coddivision, string coddireccion, string coddepartamento, string codseccion,
                                  string codpresupuesto, string codcostos, int? page)
        {
            
            Session["errorF"] = null;
            try
            {
                int paginaActual = page ?? 1;

                if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(apellido1) &&
                    String.IsNullOrEmpty(apellido2) && String.IsNullOrEmpty(codpuesto) && String.IsNullOrEmpty(codclase) &&
                    String.IsNullOrEmpty(codespecialidad) && String.IsNullOrEmpty(codnivel) && String.IsNullOrEmpty(coddivision) &&
                    String.IsNullOrEmpty(coddireccion) && String.IsNullOrEmpty(coddepartamento) && String.IsNullOrEmpty(codseccion) && String.IsNullOrEmpty(codpresupuesto)
                    && String.IsNullOrEmpty(codcostos) && String.IsNullOrEmpty(codestado))
                {
                    page = 1;
                    return View();
                }
                else
                {
                    codpuesto = codpuesto == null ? "" : codpuesto;
                    codpresupuesto = codpresupuesto == null ? "" : codpresupuesto;
                    codcostos = codcostos == null ? "" : codcostos;

                    if (cedula == null)
                    {
                        cedula = "";
                    }
                    modelo.CedulaSearch = cedula;
                    modelo.NombreSearch = nombre;
                    modelo.PrimerApellidoSearch = apellido1;
                    modelo.SegundoApellidoSearch = apellido2;
                    modelo.EstadoSearch = codestado;
                    modelo.CodPuestoSearch = codpuesto;
                    modelo.CodClaseSearch = codclase;
                    modelo.CodEspecialidadSearch = codespecialidad;
                    modelo.CodNivelSearch = codnivel;
                    modelo.CodDivisionSearch = coddivision;
                    modelo.CodDireccionSearch = coddireccion;
                    modelo.CodDepartamentoSearch = coddepartamento;
                    modelo.CodSeccionSearch = codseccion;
                    modelo.CodPresupuestoSearch = codpresupuesto;
                    modelo.CodCostosSearch = codcostos;


                    string[] clase = codclase != "" && codclase != null ? codclase.Split('-') : new string[] { "" };
                    string[] especialidad = codespecialidad != "" && codespecialidad != null ? codespecialidad.Split('-') : new string[] { "" };
                    string[] division = coddivision != "" && coddivision != null ? coddivision.Split('-') : new string[] { "" };
                    string[] direccion = coddireccion != "" && coddireccion != null ? coddireccion.Split('-') : new string[] { "" };
                    string[] departamento = coddepartamento != "" && coddepartamento != null ? coddepartamento.Split('-') : new string[] { "" };
                    string[] seccion = codseccion != "" && codseccion != null ? codseccion.Split('-') : new string[] { "" };

                    var funcionarios = funcionarioReference.BuscarFuncionarioCompletoPP(cedula, nombre, apellido1, apellido2, codpuesto, clase[0], especialidad[0], NivelOcupacionalHelper.ObtenerId(codnivel), division[0],
                                                                                      direccion[0], departamento[0], seccion[0], codpresupuesto, codestado, codcostos);
                    modelo.TotalFuncionarios = funcionarios.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                    modelo.PaginaActual = paginaActual;
                    Session["funcionarios"] = funcionarios.ToList();
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                    {
                        modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList();
                    }


                    return PartialView("Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n" + error.Message);
                return PartialView("_ErrorFuncionario");
            }
        }

        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string cedula)
        {
            Session["errorF"] = null;
            try
            {
                PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();

                var perfil = puestoLocal.DescargarPerfilPuestoAccionesFuncionarioPP(cedula);
                if (perfil.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    modelo = ConstruirModeloPuesto(modelo, perfil);
                    
                    var datos = funcionarioReference.BuscarFuncionarioSalario(cedula);
                    modelo.Salario = (CSalarioDTO)datos.ElementAt(1);
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
                    ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de ver el perfil del funcionario, ponerse en contacto con el personal autorizado. \n" + error.Message);

                }
                else
                {
                    ModelState.AddModelError("modelo", "No existen datos del lugar de contrato ni el lugar de trabajo.\n\n Pongase en contacto con el personal autorizado.");

                }
                return View();

            }
        }

        private PerfilFuncionarioVM ConstruirModeloPuesto(PerfilFuncionarioVM modelo, CBaseDTO[][] datos)
        {
            modelo.Funcionario = (CFuncionarioDTO)datos.ElementAt(0).ElementAt(0);
            modelo.Nombramiento = (CNombramientoDTO)datos.ElementAt(1).ElementAt(0);
            modelo.DetalleContrato = (CDetalleContratacionDTO)datos.ElementAt(2).ElementAt(0);
            modelo.Puesto = (CPuestoDTO)datos.ElementAt(3).ElementAt(0);
            modelo.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(4).ElementAt(0);
            modelo.Direccion = (CDireccionDTO)datos.ElementAt(12).ElementAt(0);

            if (datos.ElementAt(5).Count() > 0)
            {
                if (((CUbicacionPuestoDTO)datos.ElementAt(5).ElementAt(0)).TipoUbicacion.IdEntidad.Equals(1))
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

            if (datos.ElementAt(9).Count() > 0)
            {
                if (datos.ElementAt(9).ElementAt(0).IdEntidad != -1)
                {
                    modelo.EstadoCivil = new List<CHistorialEstadoCivilDTO>();
                    foreach (var item in datos.ElementAt(9))
                    {
                        modelo.EstadoCivil.Add((CHistorialEstadoCivilDTO)item);
                    }
                }
            }

            modelo.InformacionContacto = new List<CInformacionContactoDTO>();
            if (datos.ElementAt(10) != null)
            {
                if (datos.ElementAt(10).Count() > 0) //(datos.ElementAt(10).ElementAt(0).IdEntidad != -1)
                {
                    foreach (var item in datos.ElementAt(10))
                    {
                        modelo.InformacionContacto.Add((CInformacionContactoDTO)item);
                    }
                }
            }

            modelo.Calificaciones = new List<CCalificacionNombramientoDTO>();
            if (datos.ElementAt(11).Count() > 0)
            {
                if (datos.ElementAt(11).ElementAt(0).IdEntidad != -1)
                {
                    foreach (var item in datos.ElementAt(11))
                    {
                        modelo.Calificaciones.Add((CCalificacionNombramientoDTO)item);
                    }
                }
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

        private PerfilFuncionarioVM ConstruirPerfil(CBaseDTO[][] perfil, PerfilFuncionarioVM modelo)
        {
            try
            {


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
                modelo.Direccion = perfil[3].Count() > 0 ? (EntidadCompleta(perfil[3][0]) ? (CDireccionDTO)perfil[3][0] : null) : null;
                modelo.ProvinciaDireccion = perfil[3].Count() > 0 ? (EntidadCompleta(perfil[3][1]) ? (CProvinciaDTO)perfil[3][1] : null) : null;
                modelo.CantonDireccion = perfil[3].Count() > 0 ? (EntidadCompleta(perfil[3][2]) ? (CCantonDTO)perfil[3][2] : null) : null;
                modelo.DistritoDireccion = perfil[3].Count() > 0 ? (EntidadCompleta(perfil[3][3]) ? (CDistritoDTO)perfil[3][3] : null) : null;
                modelo.DetalleContrato = perfil[4].Count() > 0 ? (EntidadCompleta(perfil[4][0]) ? (CDetalleContratacionDTO)perfil[4][0] : null) : null;
                modelo.Nombramiento = perfil[5].Count() > 0 ? (EntidadCompleta(perfil[5][0]) ? (CNombramientoDTO)perfil[5][0] : null) : null;
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

                if (perfil.Length > 10)
                {// change > to <
                    //throw new Exception("modelo");
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


        public bool EntidadCompleta(CBaseDTO objeto)
        {
            bool respuesta = true;
            if (!String.IsNullOrEmpty(objeto.Mensaje))
            {
                respuesta = false;
            }
            return respuesta;
        }

        public PartialViewResult Presupuesto_Index(string codigopresupuesto)
        {
            Session["errorF"] = null;
            try
            {
                PresupuestoModel modelo = new PresupuestoModel();

                if (String.IsNullOrEmpty(codigopresupuesto))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigopresupuesto;
                    var codigosResult = puestoLocal.BuscarCodigoPresupuestarioParams(codigopresupuesto.Trim());
                    List<string> listcodigos = new List<string>();
                    foreach (var item in codigosResult)
                    {
                        listcodigos.Add(item.Mensaje);
                    }
                    modelo.CodigosPresupuestarios = new SelectList(listcodigos);
                    return PartialView("Presupuesto_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorFuncionario");
            }
        }

        //
        // GET: /Funcionario/RegistrarDomicilio
        public ActionResult RegistrarDomicilio(string cedula, string accion)
        {
            DireccionFuncionarioVM model = new DireccionFuncionarioVM();

            var resultado = servicioFuncionario.BuscarFuncionarioOferente(cedula);

            if (resultado.Count() > 1)
            {
                model.Funcionario = ((CFuncionarioDTO)resultado.FirstOrDefault().FirstOrDefault());
                model.Edad = FuncionarioHelper.CalcularEdadFuncionario(model.Funcionario.FechaNacimiento);
                model.EstadoCivil = ((CHistorialEstadoCivilDTO)resultado.ElementAt(1).FirstOrDefault());

                Dictionary<int, string> adscrita = GenerarProvincias();
                model.Provincias = new SelectList(adscrita, "Key", "Value");
                Dictionary<int, string> cantones = GenerarCantones();
                model.Cantones = new SelectList(cantones, "Key", "Value");
                Dictionary<int, string> distritos = GenerarDistritos();
                model.Distritos = new SelectList(distritos, "Key", "Value");
            }
            else
            {
                model.Error = ((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault());
            }

            return View(model);
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
        // POST: /Funcionario/RegistrarDomicilio
        [HttpPost]
        public ActionResult RegistrarDomicilio(DireccionFuncionarioVM model)
        {
            try
            {
                if (model.ProvinciaSeleccionada > 0 && model.CantonSeleccionado > 0 && model.DistritoSeleccionado > 0
                    && model.Direccion.DirExacta != null)
                {
                    var resultado = servicioFuncionario.GuardarDireccionFuncionario(model.Funcionario, model.Direccion);

                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/Funcionario/OferenteDetails?cedula=" +
                                            model.Funcionario.Cedula + "&accion=guardar" + "'");
                    }
                    else
                    {
                        ModelState.AddModelError("Guardar", ((CErrorDTO)((CRespuestaDTO)resultado).Contenido).MensajeError);
                        throw new Exception();
                    }
                }
                else
                {
                    if (model.ProvinciaSeleccionada < 1)
                    {
                        ModelState.AddModelError("ProvinciaForm", "Debe seleccionar una provincia.");
                    }
                    if (model.CantonSeleccionado < 1)
                    {
                        ModelState.AddModelError("CantonForm", "Debe seleccionar un cantón.");
                    }
                    if (model.DistritoSeleccionado < 1)
                    {
                        ModelState.AddModelError("DistritoForm", "Debe seleccionar un distrito.");
                    }
                    if (model.Direccion.DirExacta == null)
                    {
                        ModelState.AddModelError("SenasForm", "Debe ingresar los detalles de la dirección exacta.");
                    }
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }

        //
        // GET: /Funcionario/AsignarPuestoOferente
        public ActionResult AsignarPuestoOferente(string pedimento)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();
            var resultado = puestoLocal.DescargarPuestoPedimento(pedimento);

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

            return View(model);
        }

        //
        // POST: /Funcionario/AsignarPuestoOferente
        [HttpPost]
        public ActionResult NombrarOferente(PedimentoPuestoVM model)
        {
            return JavaScript("window.location = '/Funcionario/NombramientoDetails?cedula=" +
                       model.Funcionario.Cedula + "&pedimento=" + model.PedimentoPuesto.NumeroPedimento + "'");
        }

        //
        // GET: /Funcionario/AsignarPuestoOferente
        public ActionResult NombramientoDetails(string cedula, string pedimento)
        {
            PedimentoPuestoVM model = new PedimentoPuestoVM();

            var resultado = puestoLocal.DescargarPuestoPedimento(pedimento);

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

            var resultadofunc = servicioFuncionario.BuscarFuncionarioOferente(cedula);

            if (resultado.Count() > 1)
            {
                model.Funcionario = ((CFuncionarioDTO)resultadofunc.FirstOrDefault().FirstOrDefault());
                model.Edad = FuncionarioHelper.CalcularEdadFuncionario(model.Funcionario.FechaNacimiento);
                model.EstadoCivil = ((CHistorialEstadoCivilDTO)resultadofunc.ElementAt(1).FirstOrDefault());
            }
            else
            {
                model.Error = new CErrorDTO { MensajeError = ((CErrorDTO)resultadofunc.FirstOrDefault().FirstOrDefault()).MensajeError };
            }

            return View(model);
        }

        //
        // POST: /Funcionario/AsignarPuestoOferente
        [HttpPost]
        public ActionResult AsignarPuestoOferente(PedimentoPuestoVM model)
        {
            var resultado = servicioFuncionario.BuscarFuncionarioOferente(model.Funcionario.Cedula);

            if (resultado.Count() > 1)
            {
                model.Funcionario = ((CFuncionarioDTO)resultado.FirstOrDefault().FirstOrDefault());
                model.Edad = FuncionarioHelper.CalcularEdadFuncionario(model.Funcionario.FechaNacimiento);
                model.EstadoCivil = ((CHistorialEstadoCivilDTO)resultado.ElementAt(1).FirstOrDefault());
            }
            else
            {
                model.Error = new CErrorDTO { MensajeError = ((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).MensajeError };
            }

            return PartialView("_FormularioOferente", model);
        }

        //
        // GET: /Funcionario/OferenteDetails
        public ActionResult OferenteDetails(string cedula, string accion)
        {
            PerfilBasicoFuncionarioVM model = new PerfilBasicoFuncionarioVM();

            var resultado = servicioFuncionario.BuscarFuncionarioNuevo(cedula);

            if (resultado.GetType() != typeof(CErrorDTO))
            {

                model.Funcionario = ((CFuncionarioDTO)resultado);
                model.Edad = FuncionarioHelper.CalcularEdadFuncionario(model.Funcionario.FechaNacimiento);
                    }
                else
                {
                model.Error = new CErrorDTO { MensajeError = ((CErrorDTO)resultado).MensajeError };
                }

            return View(model);
        }

        //
        // GET: /Funcionario/RegistrarCandidato
        public ActionResult RegistrarCandidato()
        {
            return View();
        }

        //
        // POST: /Funcionario/RegistrarCandidato
        [HttpPost]
        public ActionResult RegistrarCandidato(PerfilBasicoFuncionarioVM model, string SubmitButton)
        {
            //Lista los servicios necesarios para ejecutar el método
            wsConsultaPersonaSoapClient servicioTSE = new wsConsultaPersonaSoapClient();
            AntecedentesPenalesService.CertificarClient consulta = new CertificarClient();

            SolicitudEn solicitud = new SolicitudEn();

            try
            {
                if (SubmitButton == "Buscar")
                {
                    var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
                    model.Cantones = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
                    model.Distritos = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
                    model.Provincias = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));
                    model.Sexo = new SelectList(new List<string> { "Masculino", "Femenino" });
                    
                    if (ModelState.IsValid)
                    {
                        //Llama al servicio con el número de cédula
                        var resultado = servicioFuncionario.BuscarFuncionarioOferente(model.Funcionario.Cedula);
                        if (resultado.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            //Llena el VM con los datos que tiene el servicio
                            //Se debe validar que el funcionario no tenga un puesto asignado, el puesto debe estar libre antes de registrar al oferente
                            model.Funcionario = ((CFuncionarioDTO)resultado.FirstOrDefault().FirstOrDefault());
                            solicitud = consulta.ConsultarAntecedentesPersona(TipoIdentificacion.Cedula, model.Funcionario.Cedula, model.Funcionario.Nombre, model.Funcionario.PrimerApellido, model.Funcionario.SegundoApellido, "pj.umopt-rh");
                            model.MensajePoderJudicial = DefinirMensajePoderJudicial(solicitud.CodigoSolicitud);
                            model.MensajeTSE = "La persona ya ha trabajado anteriormente para el Ministerio";
                            model.Edad = FuncionarioHelper.CalcularEdadFuncionario(model.Funcionario.FechaNacimiento);
                            model.EstadoCivil = ((CHistorialEstadoCivilDTO)resultado.ElementAt(1).FirstOrDefault());
                            model.DatosContacto = new List<CInformacionContactoDTO>();
                            model.TiposContacto = new List<CTipoContactoDTO>();
                            model.DatosContacto = new List<CInformacionContactoDTO>
                            {
                                            new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 2, DesTipoContacto = "Teléfono Domicilio" } } ,
                                            new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 3, DesTipoContacto = "Teléfono Celular" } } ,
                                            new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 1, DesTipoContacto = "Correo Electrónico" } }
                                    };
                            if (model.Funcionario.EstadoFuncionario.IdEntidad != 20)
                            {
                                model.Nombramiento = (CNombramientoDTO)resultado.ElementAt(3).FirstOrDefault();
                                model.Puesto = (CPuestoDTO)resultado.ElementAt(4).FirstOrDefault();
                                model.Direccion = (CDireccionDTO)resultado.ElementAt(5).FirstOrDefault();
                                model.ProvinciaSeleccionada = model.Direccion.Distrito.Canton.Provincia.IdEntidad.ToString();
                                model.CantonSeleccionado = model.Direccion.Distrito.Canton.IdEntidad.ToString();
                                model.DistritoSeleccionado = model.Direccion.Distrito.IdEntidad.ToString();
                            }

                            return PartialView("_FormularioCandidato", model);
                        }
                        else
                        {
                            if (((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).Codigo == -1)
                            {
                                //Si no encuentra al funcionario busca en el servicio del TSE
                                //Preguntar a Don Miguel como se identifican los extranjeros en emulación
                                string cedulaTSE = FuncionarioHelper.CedulaEmulacionATSE(model.Funcionario.Cedula);
                                var persona = servicioTSE.wsConsultaDatosPersona(0, cedulaTSE, true);
                                //Si existe la persona en el TSE la devuelve
                                if (persona != null)
                                {
                                    model.Funcionario = FuncionarioHelper.ConvertirPersonaTSEAFuncionario(persona);
                                    solicitud = consulta.ConsultarAntecedentesPersona(TipoIdentificacion.Cedula, cedulaTSE, model.Funcionario.Nombre, model.Funcionario.PrimerApellido, model.Funcionario.SegundoApellido, "pj.umopt-rh");
                                    model.MensajePoderJudicial = DefinirMensajePoderJudicial(solicitud.CodigoSolicitud);
                                    model.EstadoCivil = new CHistorialEstadoCivilDTO { CatEstadoCivil = new CCatEstadoCivilDTO { DesEstadoCivil = persona.Indice_Estado_Civil } };
                                    model.Edad = Convert.ToInt32(persona.Edad.Substring(0, 2));
                                    model.MensajeTSE = "La persona consultada no ha trabajado en el MOPT anteriormente, los datos mostrados provienen de los registros del Tribunal Supremo de Elecciones";
                                    model.DatosContacto = new List<CInformacionContactoDTO>
                                    {
                                            new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 2, DesTipoContacto = "Teléfono Domicilio" } } ,
                                         new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 3, DesTipoContacto = "Teléfono Celular" } } ,
                                            new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 1, DesTipoContacto = "Correo Electrónico" } }
                                    };
                                    return PartialView("_FormularioCandidato", model);
                                }
                                //Si no existe la persona entonces muestra un error en pantalla
                                else
                                {
                                    ModelState.AddModelError("Busqueda", "La cédula digitada no se encontró en los registros del TSE, por favor revise que la información suministrada sea correcta");
                                    throw new Exception("Busqueda");
                                }
                            }
                            else
                            {
                                if (((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).Codigo == 5)
                                {
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).MensajeError);
                                throw new Exception("Busqueda");
                            }
                                else
                                {
                                    ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.FirstOrDefault().FirstOrDefault()).MensajeError);
                                    throw new Exception("Busqueda");
                        }
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
                        if (model.DatosContacto.FirstOrDefault(Q => Q.DesContenido != null) != null)
                        {
                        if ((model.ProvinciaSeleccionada != null || model.Direccion.Distrito != null)
                            && ((model.CantonSeleccionado != null && model.CantonSeleccionado != "0") || model.Direccion.Distrito != null) 
                            && ((model.DistritoSeleccionado != null && model.CantonSeleccionado != "0") || model.Direccion.Distrito != null) 
                            && (model.Direccion.DirExacta != null && model.Direccion.DirExacta != String.Empty) && (model.SexoSeleccionado != null))
                        {
                            //Si el mensajeTSE es diferente a nulo quiere decir que se está registrando un funcionario nuevo
                            //Registra al nuevo funcionario y coloca su estado como oferente
                            //Continúa en el registro de los nuevos datos del funcionario
                            if (model.Direccion.Distrito == null)
                            {
                                model.Direccion.Distrito = new CDistritoDTO
                                {
                                    NomDistrito = model.DistritoSeleccionado,
                                    Mensaje = model.ProvinciaSeleccionada + "-" + model.CantonSeleccionado
                                };
                            }
                            model.Funcionario.Sexo = model.SexoSeleccionado == "Masculino" ? GeneroEnum.Masculino : model.SexoSeleccionado == "Femenino" ? GeneroEnum.Femenino : GeneroEnum.Indefinido;
                            var respuesta = servicioFuncionario.GuardarDatosPersonalesFuncionario(model.Funcionario, model.EstadoCivil, model.DatosContacto.ToArray(), model.Direccion);
                            if (respuesta.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    return JavaScript("window.location = '/Funcionario/OferenteDetails?cedula=" +
                                         model.Funcionario.Cedula + "&accion=guardar" + "'");
                                }
                            else
                            {
                                ModelState.AddModelError("Guardado", ((CErrorDTO)respuesta.FirstOrDefault()).MensajeError);
                                throw new Exception();
                            }
                        }
                        else
                        {
                            if (model.SexoSeleccionado == null)
                            {
                                ModelState.AddModelError("Guardado", "Debe seleccionar el sexo del funcionario");
                                throw new Exception();
                            }
                            else
                            {
                                ModelState.AddModelError("Guardado", "Los datos de la dirección están incompletos, debe completarlos para continuar");
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Guardado", "Debe digitar al menos un medio de contacto para el oferente");
                        throw new Exception();
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorCandidato");
                }
                else
                {
                    var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
                    model.Cantones = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
                    model.Distritos = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
                    model.Provincias = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));
                    model.DatosContacto = new List<CInformacionContactoDTO>
                    {
                         new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 2, DesTipoContacto = "Teléfono Domicilio" } } ,
                         new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 3, DesTipoContacto = "Teléfono Celular" } } ,
                         new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = 1, DesTipoContacto = "Correo Electrónico" } }
                    };
                    return PartialView("_ErrorCandidato", model);
                }
            }
        }

        private string DefinirMensajePoderJudicial(int codigoSolicitud)
        {
            switch (codigoSolicitud)
            {
                case -2:
                    return "Según los registros del Poder Judicial, la persona no presenta antecedentes penales.";
                case -1:
                    return "Ya se ha solicitado previamente la información de antecedentes penales al Poder Judicial";
                case -3:
                    return "No se encontró información de la persona en los registros del Poder Judicial";
                case -4:
                    return "No se pudo contactar al Poder Judicial para verificar los antecedentes penales de la persona, debe solicitar la hoja de delincuencia";
                default:
                    if (codigoSolicitud > 0)
                    {
                        return "Se generó la solicitud # " + codigoSolicitud.ToString() + ", para verificar los antecedentes de la persona, ya que cuenta con antecedentes penales";
                    }
                    else
                    {
                        return "No se pudo contactar al Poder Judicial para verificar los antecedentes penales de la persona, debe solicitar la hoja de delincuencia";
                    }
            }
        }

        public ActionResult DetalleContratacionSearch()
        {
            DetalleContratacionVM model = new DetalleContratacionVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult DetalleContratacionSearch(DetalleContratacionVM model)
        {
            try
            {
                var resultado = servicioFuncionario.CargarDetalleContratacionFuncionario(model.Funcionario.Cedula);

                if (resultado.ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    model.Funcionario = (CFuncionarioDTO)resultado.ElementAt(0);
                    model.DetalleContratacion = (CDetalleContratacionDTO)resultado.ElementAt(1);
                    model.Expediente = (CExpedienteFuncionarioDTO)resultado.ElementAt(2);
                    model.DetalleContratacion.Mensaje = "Actual";
                    if (model.DetalleContratacion.EstadoMarcacion == null)
                    {
                        model.EstadoMarca = false;
                    }
                    else
                    {
                        model.EstadoMarca = Convert.ToBoolean(model.DetalleContratacion.EstadoMarcacion);
                    }
                    return PartialView("_FormularioDetalleContratacion", model);
                }
                else
                {
                    var soloFuncionario = servicioFuncionario.BuscarInformacionBasicaFuncionario(model.Funcionario.Cedula);

                    if(soloFuncionario.ElementAt(0).GetType() == typeof(CErrorDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)soloFuncionario.ElementAt(0);
                        model.DetalleContratacion = new CDetalleContratacionDTO();
                        model.DetalleContratacion.Mensaje = "Nuevo";
                        return PartialView("_FormularioDetalleContratacion", model);
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)resultado.ElementAt(0)).MensajeError);
                    }
                }
            }
            catch (Exception error)
            {
                throw;
            }
        }

        public ActionResult EditarInformacionFuncionario()
        {
            PerfilBasicoFuncionarioVM model = new PerfilBasicoFuncionarioVM();

            return View(model);
        }

        [HttpPost]
        public ActionResult EditarInformacionFuncionario(PerfilBasicoFuncionarioVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
                    model.Cantones = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
                    model.Distritos = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
                    model.Provincias = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));
                    model.EstadosCiviles = new SelectList(new List<string> { "Soltero/a", "Casado/a", "Divorciado/a", "Unión Libre",
                                                                             "Viudo/a" });

                    if (ModelState.IsValid)
                    {
                        //Llama al servicio con el número de cédula
                        var resultado = servicioFuncionario.BuscarInformacionBasicaFuncionario(model.Funcionario.Cedula);
                        if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            //Llena el VM con los datos que tiene el servicio
                            //Se debe validar que el funcionario no tenga un puesto asignado, el puesto debe estar libre antes de registrar al oferente
                            model.Funcionario = ((CFuncionarioDTO)resultado.FirstOrDefault());
                            model.Edad = FuncionarioHelper.CalcularEdadFuncionario(model.Funcionario.FechaNacimiento);
                            model.Direccion = ((CDireccionDTO)resultado.ElementAt(1));
                            model.EstadoCivil = ((CHistorialEstadoCivilDTO)resultado.ElementAt(2));
                            model.DatosContacto = new List<CInformacionContactoDTO>();
                            model.TiposContacto = new List<CTipoContactoDTO>();
                            var listaContactos = resultado.ToList().GetRange(3, resultado.Count() - 3);
                            for (int i = 1; i < 4; i++)
                            {
                                var item = listaContactos.FirstOrDefault(Q => ((CInformacionContactoDTO)Q).TipoContacto.IdEntidad == i);
                                if (item != null)
                                {
                                    model.DatosContacto.Add((CInformacionContactoDTO)item);
                                }
                                else
                                {
                                    model.DatosContacto.Add(new CInformacionContactoDTO { TipoContacto = new CTipoContactoDTO { IdEntidad = i, DesTipoContacto = i == 1 ? "Correo Electrónico" : i == 2 ? "Teléfono Domicilio" : "Teléfono Celular" } });
                                }
                            }

                            Session["ModeloViejo"] = model;

                            return PartialView("_FormularioEdicionFuncionario", model);
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.FirstOrDefault()).MensajeError);
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
                    if (((model.DistritoSeleccionado != null && model.DistritoSeleccionado != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).Direccion.Distrito.NomDistrito)
                        && (model.Direccion.DirExacta != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).Direccion.DirExacta))
                        || model.DatosContacto.ElementAt(0).DesContenido != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).DatosContacto.ElementAt(0).DesContenido
                        || model.DatosContacto.ElementAt(1).DesContenido != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).DatosContacto.ElementAt(1).DesContenido
                        || model.DatosContacto.ElementAt(2).DesContenido != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).DatosContacto.ElementAt(2).DesContenido
                        || model.EstadoCivil.CatEstadoCivil.DesEstadoCivil != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).EstadoCivil.CatEstadoCivil.DesEstadoCivil)
                    {
                        if (model.DatosContacto.FirstOrDefault(Q => Q.DesContenido != null) != null)
                        {
                            if ((model.ProvinciaSeleccionada != null || model.Direccion.Distrito != null)
                                && ((model.CantonSeleccionado != null && model.CantonSeleccionado != "0") || model.Direccion.Distrito != null)
                                && ((model.DistritoSeleccionado != null && model.CantonSeleccionado != "0") || model.Direccion.Distrito != null)
                                && (model.Direccion.DirExacta != null && model.Direccion.DirExacta != String.Empty))
                            {
                                //Si el mensajeTSE es diferente a nulo quiere decir que se está registrando un funcionario nuevo
                                //Registra al nuevo funcionario y coloca su estado como oferente
                                //Continúa en el registro de los nuevos datos del funcionario
                                if (model.Direccion.Distrito.NomDistrito == null)
                                {
                                    model.Direccion.Distrito = new CDistritoDTO
                                    {
                                        NomDistrito = model.DistritoSeleccionado,
                                        Mensaje = model.ProvinciaSeleccionada + "-" + model.CantonSeleccionado
                    };
                                }

                                if (!((model.Direccion.Distrito.NomDistrito != null && model.Direccion.Distrito != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).Direccion.Distrito)
                                        && (model.Direccion.DirExacta != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).Direccion.DirExacta)))
                                {
                                    model.Direccion.Mensaje = "No";
                                }

                                if (!(model.DatosContacto.ElementAt(0).DesContenido != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).DatosContacto.ElementAt(0).DesContenido))
                                {
                                    model.DatosContacto.ElementAt(0).Mensaje = "No";
                                }

                                if (!(model.DatosContacto.ElementAt(1).DesContenido != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).DatosContacto.ElementAt(1).DesContenido))
                                {
                                    model.DatosContacto.ElementAt(1).Mensaje = "No";
                                }

                                if (!(model.DatosContacto.ElementAt(2).DesContenido != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).DatosContacto.ElementAt(2).DesContenido))
                                {
                                    model.DatosContacto.ElementAt(2).Mensaje = "No";
                                }

                                if (!(model.EstadoCivil.CatEstadoCivil.DesEstadoCivil != ((PerfilBasicoFuncionarioVM)Session["ModeloViejo"]).EstadoCivil.CatEstadoCivil.DesEstadoCivil))
                                {
                                    model.EstadoCivil.Mensaje = "No";
                                }

                                var respuesta = servicioFuncionario.ActualizarInformacionBasicaFuncionario(model.Funcionario, model.Direccion, model.DatosContacto.ToArray(), model.EstadoCivil);
                                if (respuesta.GetType() != typeof(CErrorDTO))
                                {
                                    return JavaScript("window.location = '/Funcionario/OferenteDetails?cedula=" +
                                            model.Funcionario.Cedula + "&accion=guardar" + "'");
                                }
                                else
                                {
                                    ModelState.AddModelError("Guardado", ((CErrorDTO)respuesta).MensajeError);
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Guardado", "Los datos de la dirección están incompletos, debe completarlos para continuar");
                                throw new Exception();
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Guardado", "Debe digitar al menos un medio de contacto");
                            throw new Exception();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Guardado", "No se registran cambios en el formulario");
                        throw new Exception();
                    }
                }
                }
            catch (Exception error)
            {
                //if (error.Message == "Busqueda")
                //{
                //    return PartialView("_ErrorCandidato");
                //}
                //else
                //{
                    //var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
                    //model.Cantones = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
                    //model.Distritos = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
                    //model.Provincias = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));
                    return PartialView("_ErrorCandidato", model);
                //}
            }
        }

        public ActionResult GenerarReporteFuncionario()
        {
            try
            {
                PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                List<FuncionariosRptData> modelo1 = new List<FuncionariosRptData>();
                PerfilFuncionarioVM fun = (PerfilFuncionarioVM)Session["funcionario"];
                modelo1.Add(FuncionariosRptData.GenerarDatosReporteFuncionario(fun));
                return ReporteFuncionarios(modelo1, "ReporteFuncionario.rpt", "PDF");
            }
            catch (Exception error)
            {
                Session["errorF"] = "error";
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte del funcionario, ponerse en contacto con el personal autorizado. \n");
                return View("Details");
            }

        }

        public ActionResult GenerarReportePorFuncionarios()
        {
            Session["errorF"] = null;
            PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
            List<FuncionariosRptData> modelo1 = new List<FuncionariosRptData>();
            try
            {
                List<CFuncionarioDTO> fun = (List<CFuncionarioDTO>)Session["funcionarios"];

                foreach (var item in fun)
                {
                    var perfil = puestoLocal.DescargarPerfilPuestoAccionesFuncionarioPP(item.Cedula);

                    if (perfil.ElementAt(5).FirstOrDefault().IdEntidad > 0)
                    {
                        modelo = ConstruirModeloPuesto(modelo, perfil);
                    }

                    if (modelo == null) throw new Exception("modelo");

                    modelo1.Add(FuncionariosRptData.GenerarDatosReportePorFuncionarios(modelo));
                }


                //return ExportToExcel(modelo1);
                return ReporteFuncionarios(modelo1, "ReporteFuncionarios.rpt", "EXCEL");
            }
            catch (Exception error)
            {
                Session["errorF"] = "error";
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

        public ActionResult ExportToExcel(List<FuncionariosRptData> modelo1)
        {
            var products = new System.Data.DataTable("teste");

            products.Columns.Add("CEDULA", typeof(string));
            products.Columns.Add("NOMBRE", typeof(string));
            products.Columns.Add("FECHA NACIMIENTO", typeof(string));
            products.Columns.Add("EDAD", typeof(string));
            products.Columns.Add("PUESTO", typeof(string));
            products.Columns.Add("CLASE", typeof(string));
            products.Columns.Add("ESPECIALIDAD", typeof(string));
            products.Columns.Add("DIVISION", typeof(string));
            products.Columns.Add("DIRECCION", typeof(string));
            products.Columns.Add("DEPARTAMENTO", typeof(string));
            products.Columns.Add("SECCION", typeof(string));

            foreach (var item in modelo1)
            {
                products.Rows.Add(item.Cedula, item.Nombre, item.FechaNacimiento, 2020-Convert.ToDateTime(item.FechaNacimiento).Year, item.Puesto, item.Clase, item.Especialidad, item.Division, item.Direccion, item.Departamento, item.Seccion);
            }

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            ////Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            ////Response.ContentType = "application/ms-excel";

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Reporte_Funcionarios.xlsx"));

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.Close();
            Response.End();

            return View("Index");
        }

        public ActionResult GenerarReportePorFuncionariosParam(FuncionarioModel modelF)
        {
            Session["errorF"] = null;
            try
            {
                List<PerfilFuncionarioVM> groupList = new List<PerfilFuncionarioVM>();
                List<FuncionariosRptData> modeloFinal = new List<FuncionariosRptData>();
                List<CFuncionarioDTO> fun = (List<CFuncionarioDTO>)Session["funcionarios"];

                List<string> filtros = new List<string>
                {
                    modelF.CedulaSearch != null ? modelF.CedulaSearch.ToString().Trim() : "",
                    modelF.NombreSearch != null ? modelF.NombreSearch.ToString().Trim() : "",
                    modelF.PrimerApellidoSearch != null ? modelF.PrimerApellidoSearch.ToString().Trim() : "",
                    modelF.SegundoApellidoSearch != null ? modelF.SegundoApellidoSearch.ToString().Trim() : "",
                    modelF.CodPuestoSearch != null ? modelF.CodPuestoSearch.ToString().Trim() : "",
                    modelF.CodClaseSearch != null ? modelF.CodClaseSearch.ToString().Trim() : "",
                    modelF.CodEspecialidadSearch != null ? modelF.CodEspecialidadSearch.ToString().Trim() : "",
                    modelF.CodNivelSearch != null ?  modelF.CodNivelSearch.ToString().Trim() : "",
                    modelF.CodDivisionSearch != null ? modelF.CodDivisionSearch.ToString().Trim() : "",
                    modelF.CodDireccionSearch != null ? modelF.CodDireccionSearch.ToString().Trim() : "",
                    modelF.CodDepartamentoSearch != null ? modelF.CodDepartamentoSearch.ToString().Trim() : "",
                    modelF.CodSeccionSearch != null ? modelF.CodSeccionSearch.ToString().Trim() : "",
                    modelF.CodPresupuestoSearch != null ? modelF.CodPresupuestoSearch.ToString().Trim() : "",
                    modelF.CodCostosSearch != null ? modelF.CodCostosSearch.ToString().Trim() : ""
                };

                foreach (var item in fun)
                {
                    var perfil = funcionarioReference.DescargarPerfilFuncionarioCompleto(item.Cedula);
                    PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                    modelo = ConstruirPerfil(perfil, modelo);
                    if (modelo == null) throw new Exception("modelo");
                    groupList.Add(modelo);
                }

                List<PerfilFuncionarioVM> modeloGroup = new List<PerfilFuncionarioVM>();
                if (!modelF.Grupo.Equals("Ninguno"))
                {
                    switch (modelF.Grupo)
                    {
                        case "Clase":
                            var agrupar = groupList.GroupBy(C => C.DetallePuesto?.Clase?.DesClase).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "División":
                            agrupar = groupList.GroupBy(C => C.DivisionPuesto?.NomDivision).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "Dirección":
                            agrupar = groupList.GroupBy(C => C.DireccionGeneralPuesto?.NomDireccion).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "Departamento":
                            agrupar = groupList.GroupBy(C => C.DepartamentoPuesto?.NomDepartamento).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "Sección":
                            agrupar = groupList.GroupBy(C => C.SeccionPuesto?.NomSeccion).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "Especialidad":
                            agrupar = groupList.GroupBy(C => C.EspecialidadPuesto?.DesEspecialidad).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "Nivel Ocupacional":
                            agrupar = groupList.GroupBy(C => C.Puesto?.NivelOcupacional.ToString()).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                        case "Código presupuestario":
                            agrupar = groupList.GroupBy(C => C.Puesto?.UbicacionAdministrativa?.Presupuesto?.CodigoPresupuesto).ToList();
                            foreach (var item in agrupar) foreach (var func in item) modeloGroup.Add(func);
                            break;
                    }
                }
                else
                {
                    modeloGroup = groupList;
                }
                foreach (var item in modeloGroup)
                {
                    modeloFinal.Add(FuncionariosRptData.GenerarDatosReporteFuncionarioParam(item, modelF.Parametros, filtros, modelF.Grupo));
                }
                return ReporteFuncionarios(modeloFinal, "ReporteFuncionarioParam.rpt", "EXCEL");
            }
            catch (Exception error)
            {
                Session["errorF"] = "error";
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

        private CrystalReportPdfResult ReporteFuncionarios(List<FuncionariosRptData> modelo, string reporte, string tipo)
        {
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Funcionarios"), reporte);
            return new CrystalReportPdfResult(reportPath, modelo, tipo);
        }

        [HttpPost]
        public ActionResult Search_Funcionario()
        {
            return View();
        }

        
        public ActionResult Search_Funcionario(FuncionarioModel modelo, string nombre, string apellido1, string apellido2,
                                  string codestado, int? page)
        {
            Session["errorF"] = null;
            try
            {
                int paginaActual = page ?? 1;

                if (String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(apellido1) &&
                    String.IsNullOrEmpty(apellido2) &&  String.IsNullOrEmpty(codestado))
                {
                    page = 1;
                    return View();
                }
                else
                {
                    modelo.NombreSearch = nombre;
                    modelo.PrimerApellidoSearch = apellido1;
                    modelo.SegundoApellidoSearch = apellido2;
                    modelo.EstadoSearch = codestado;
               
                    var funcionarios = funcionarioReference.BuscarFuncionarioCompletoPP("", nombre, apellido1, apellido2, "","","",0,"",
                                                                                     "", "", "", "", codestado, "");
                    modelo.TotalFuncionarios = funcionarios.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                    modelo.PaginaActual = paginaActual;
                    Session["funcionarios"] = funcionarios.ToList();
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                    {
                        modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList();
                    }


                    return PartialView("Search_Funcionario_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n" + error.Message);
                return PartialView("_ErrorFuncionario");
            }
        }


        public ActionResult IndexExfuncionario(FuncionarioModel modelo, string cedula, string nombre, string apellido1, string apellido2,
                                 string codpuesto, int? page)
        {
            Session["errorF"] = null;
            try
            {
                int paginaActual = page ?? 1;

                if (String.IsNullOrEmpty(cedula) && String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(apellido1) &&
                    String.IsNullOrEmpty(apellido2) && String.IsNullOrEmpty(codpuesto))
                {
                    page = 1;
                    return PartialView("IndexExfuncionario");
                }
                else
                {
                    if (cedula == null)
                    {
                        cedula = "";
                    }
                    modelo.CedulaSearch = cedula;
                    modelo.NombreSearch = nombre;
                    modelo.PrimerApellidoSearch = apellido1;
                    modelo.SegundoApellidoSearch = apellido2;
                    modelo.CodPuestoSearch = codpuesto;

                    var funcionarios = funcionarioReference.BuscarExfuncionarioFiltros(cedula, nombre, apellido1, apellido2, codpuesto);
                    modelo.TotalFuncionarios = funcionarios.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                    modelo.PaginaActual = paginaActual;
                    Session["exfuncionarios"] = funcionarios.ToList();
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                    {
                        modelo.Exfuncionarios = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Exfuncionarios = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList();
                    }


                    return PartialView("IndexExfuncionario_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorFuncionario");
            }
        }


        public ActionResult DetailsExfuncionario(string cedula)
        {
            Session["errorF"] = null;
            try
            {
                FuncionarioModel modelo = new FuncionarioModel();
                var funcionarios = funcionarioReference.BuscarExfuncionarioFiltros(cedula, "", "", "", "");
                modelo.Exfuncionarios = funcionarios.ToList();
                modelo.TotalFuncionarios = funcionarios.Count();
                Session["exfuncionarios"] = funcionarios.ToList();
                return View(modelo);
            }
            catch (Exception error)
            {
                Session["errorF"] = "error";
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de ver el perfil, ponerse en contacto con el personal autorizado. \n\n");
                return View();
            }
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleExFuncionario(FuncionarioModel model)
        {
            string reportPath = "0";
            try
            {
                List<ExFuncionariosRptData> modelo = new List<ExFuncionariosRptData>();

                modelo.Add(ExFuncionariosRptData.GenerarDatosReporteExFuncionario(model.Exfuncionarios[0]));

                reportPath = Path.Combine(Server.MapPath("~/Reports/Funcionarios"), "ReporteExFuncionario.rpt");

                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(reportPath + " " + ex.InnerException.Message);
            }
        }
    }
}
