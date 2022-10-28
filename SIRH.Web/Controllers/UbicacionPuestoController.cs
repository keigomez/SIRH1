using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
using SIRH.Web.FuncionarioService;
//using SIRH.Web.PuestoService;
using SIRH.Web.PuestoLocal;
using SIRH.Web.Models;
using System.IO;
using SIRH.DTO;
using SIRH.Web.Reports.Funcionarios;
using SIRH.Web.Reports.PDF;

namespace SIRH.Web.Controllers
{
    
    public class UbicacionPuestoController : Controller
    {
        static SelectList UniqueIdPresupuestosTemp = null;
        CFuncionarioServiceClient funcionarioReference = new CFuncionarioServiceClient();
        CPuestoServiceClient puestoReference = new CPuestoServiceClient();
        //
        // GET: /UbicacionPuesto/

        public ActionResult Index(string coddivision, string coddireccion, string coddepartamento, string codseccion, string codpresupuesto, int? page)
        {
            try
            {
                Session["errorU"] = null;
                FuncionarioUbicacionVM modelo = new FuncionarioUbicacionVM();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(coddivision) && String.IsNullOrEmpty(coddireccion) && String.IsNullOrEmpty(coddepartamento) &&
                    String.IsNullOrEmpty(codseccion) && String.IsNullOrEmpty(codseccion) && String.IsNullOrEmpty(codpresupuesto))
                {
                    page = 1;
                    PresupuestoModel PresupuestoModel = new PresupuestoModel();

                    var auxiliar = puestoReference.ObtenerCodigosProgramas().Select(
                                    P => new SelectListItem
                                    {
                                        Value = P.IdEntidad.ToString(),
                                        Text = P.IdEntidad.ToString()
                                    }
                                    );

                    PresupuestoModel.CodigosPresupuestarios = new SelectList(auxiliar, "Value", "Text");
                    ViewBag.PresupuestoModel = PresupuestoModel;
                    return View();
                }
                else
                {
                    modelo.DivisionSearch = coddivision;
                    modelo.DireccionSearch = coddireccion;
                    modelo.DepartamentoSearch = coddepartamento;
                    modelo.SeccionSearch = codseccion;
                    modelo.PresupuestoSearch = codpresupuesto;
                    int codigodivision = String.IsNullOrEmpty(modelo.DivisionSearch) ? 0 : Convert.ToInt32(modelo.DivisionSearch.Split('-')[0]);
                    int codigodireccion = String.IsNullOrEmpty(modelo.DireccionSearch) ? 0 : Convert.ToInt32(modelo.DireccionSearch.Split('-')[0]);
                    int codigodepartamento = String.IsNullOrEmpty(modelo.DepartamentoSearch) ? 0 : Convert.ToInt32(modelo.DepartamentoSearch.Split('-')[0]);
                    int codigoseccion = String.IsNullOrEmpty(modelo.SeccionSearch) ? 0 : Convert.ToInt32(modelo.SeccionSearch.Split('-')[0]);
                    string codigopresupuesto = String.IsNullOrEmpty(codpresupuesto) ? "0" : codpresupuesto;

                    var funcionarios =
                        funcionarioReference.BuscarFuncionarioUbicacion(codigodivision,
                                                                            codigodireccion,
                                                                            codigodepartamento,
                                                                            codigoseccion, codigopresupuesto);
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
                        modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
            }

        }

        public PartialViewResult Division_Index(string codigodivision, string nomdivision, int? page)
        {
            Session["errorU"] = null;
            try
            {
                DivisionModel modelo = new DivisionModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigodivision) && String.IsNullOrEmpty(nomdivision))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigodivision;
                    modelo.NombreSearch = nomdivision;
                    int codigoDivision = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var divisiones =
                        puestoReference.BuscarDivisionParams(codigoDivision, modelo.NombreSearch);
                    modelo.TotalDivisiones = divisiones.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalDivisiones / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalDivisiones)
                    {
                        modelo.Division = divisiones.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalDivisiones) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Division = divisiones.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Division_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
            }
        }

        public PartialViewResult Direccion_Index(string codigodireccion, string nomdireccion, int? page)
        {
            Session["errorU"] = null;
            try
            {
                DireccionGeneralModel modelo = new DireccionGeneralModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigodireccion) && String.IsNullOrEmpty(nomdireccion))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigodireccion;
                    modelo.NombreSearch = nomdireccion;
                    int codigoDireccion = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var direcciones =
                        puestoReference.BuscarDireccionGeneralParams(codigoDireccion, modelo.NombreSearch);
                    modelo.TotalDirecciones = direcciones.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalDirecciones / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalDirecciones)
                    {
                        modelo.Direccion = direcciones.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalDirecciones) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Direccion = direcciones.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Direccion_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
            }
        }

        public PartialViewResult Departamento_Index(string codigodepartamento, string nomdepartamento, int? page)
        {
            Session["errorU"] = null;
            try
            {
                DepartamentoModel modelo = new DepartamentoModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigodepartamento) && String.IsNullOrEmpty(nomdepartamento))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigodepartamento;
                    modelo.NombreSearch = nomdepartamento;
                    int codigoDepartamento = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var departamentos = puestoReference.BuscarDepartamentoParams(codigoDepartamento, modelo.NombreSearch);
                    modelo.TotalDepartamentos = departamentos.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalDepartamentos / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalDepartamentos)
                    {
                        modelo.Departamento = departamentos.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalDepartamentos) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Departamento = departamentos.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Departamento_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
            }

        }

        public PartialViewResult Seccion_Index(string codigoseccion, string nomseccion, int? page)
        {
            Session["errorU"] = null;
            try
            {
                SeccionModel modelo = new SeccionModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoseccion) && String.IsNullOrEmpty(nomseccion))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoseccion;
                    modelo.NombreSearch = nomseccion;
                    int codigoSeccion = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var secciones =
                        puestoReference.BuscarSeccionParams(codigoSeccion, modelo.NombreSearch);
                    modelo.TotalSecciones = secciones.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalSecciones / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalSecciones)
                    {
                        modelo.Seccion = secciones.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalSecciones) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Seccion = secciones.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Seccion_Index_Result", modelo);
                }
            }
            catch (Exception error) {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
            }
        }

        public PartialViewResult Seccion_Index_UbicacionAdministrativa(string codigoseccion, int? page)
        {
            Session["errorU"] = null;
            try
            {
                UbicacionAdministrativaModel modelo = new UbicacionAdministrativaModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoseccion))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoseccion;
                    int codigoSeccion = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    List<CUbicacionAdministrativaDTO> ubicaciones = new List<CUbicacionAdministrativaDTO>();
                    foreach (var item in puestoReference.UbicacionAdministrativaSeccion(codigoSeccion))
                    {
                        ubicaciones.Add((CUbicacionAdministrativaDTO)item);
                    }
                    modelo.TotalUbicaciones = ubicaciones.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalUbicaciones / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalUbicaciones)
                    {
                        modelo.UbicacionesAdministrativas = ubicaciones.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalUbicaciones) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.UbicacionesAdministrativas = ubicaciones.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Seccion_Index_UbicacionAdministrativa_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
            }
        }

        public PartialViewResult Presupuesto_Index(PresupuestoModel presupuestoModel, bool? reset, int? page)
        {
            Session["errorU"] = null;
            try
            {
                if (reset != null && reset == true) {
                    UniqueIdPresupuestosTemp = null;
                }
                PresupuestoModel modelo = new PresupuestoModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(presupuestoModel.CodigoPresupuesto))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = presupuestoModel.CodigoPresupuesto;
                    var presupuestos =
                        puestoReference.BuscarPresupuestoParams(presupuestoModel.CodigoPresupuesto);
                    modelo.TotalPresupuestos = presupuestos[0].Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalPresupuestos / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalPresupuestos)
                    {
                        modelo.Presupuesto = presupuestos[0].ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalPresupuestos) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Presupuesto = presupuestos[0].ToList().GetRange(((paginaActual - 1) * 10), 10).ToList();
                    }

                    var filter = presupuestos[1].ToList().Select(
                                    P => new SelectListItem
                                    {
                                        Value = P.CodigoPresupuesto.ToString(),
                                        Text = P.text
                                    }
                                    );
                    if (UniqueIdPresupuestosTemp == null)
                    {
                        modelo.UniqueIdPresupuestos = new SelectList(filter, "Value", "Text");
                        UniqueIdPresupuestosTemp = new SelectList(filter, "Value", "Text");
                    }
                    else {
                        modelo.UniqueIdPresupuestos = UniqueIdPresupuestosTemp;
                    }
                    

                    return PartialView("Presupuesto_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorUbicacionPuesto");
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
            Session["errorU"] = null;
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
                Session["errorU"] = "error";
                if (error.Message != "modelo")
                {
                    ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte de funcionarios, ponerse en contacto con el personal autorizado. \n");
                }
                else
                {
                    ModelState.AddModelError("modelo", "No existen datos del lugar de contrato ni el lugar de trabajo.\n\n Pongase en contacto con el personal autorizado.");

                }
                PresupuestoModel PresupuestoModel = new PresupuestoModel();

                var auxiliar = puestoReference.ObtenerCodigosProgramas().Select(
                                P => new SelectListItem
                                {
                                    Value = P.IdEntidad.ToString(),
                                    Text = P.IdEntidad.ToString()
                                }
                                );

                PresupuestoModel.CodigosPresupuestarios = new SelectList(auxiliar, "Value", "Text");
                ViewBag.PresupuestoModel = PresupuestoModel;

                return View("Index");
            }

        }

        private CrystalReportPdfResult ReporteFuncionarios(List<FuncionariosRptData> modelo)
        {
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Funcionarios"), "ReporteFuncionarios.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "EXCEL");
        }

    }
}
