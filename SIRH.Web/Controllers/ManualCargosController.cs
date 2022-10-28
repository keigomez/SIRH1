using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.Models;
//using SIRH.Web.ManualCargosLocal;
using SIRH.Web.ManualCargosService;
using SIRH.DTO;

namespace SIRH.Web.Controllers
{
    public class ManualCargosController : Controller
    {
        CManualCargosServiceClient servicioManual = new CManualCargosServiceClient();
        // GET: ManualCargos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DescripcionCargo(int codigo = 0)
        {
            if (codigo < 1)
            {
                return View();
            }
            else
            {
                ManualCargosVM model = new ManualCargosVM();

                var resultado = servicioManual.ObtenerCargo(codigo);

                if (resultado.GetType() != typeof(CErrorDTO))
                {
                    model.Cargo = (CCargoDTO)resultado;
                }
                else
                {
                    model.Error = (CErrorDTO)resultado;
                }

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult DescripcionCargo(ManualCargosVM model, string SubmitButton)
        {
            try
            {
                if (model.Cargo.ClaseCargo != null && model.Cargo.ClaseCargo.DesClase != "")
                {
                    if (model.Cargo.ClaseCargo.DesClase != null && model.Cargo.ClaseCargo.DesClase.Contains("-"))
                    {
                        model.Cargo.ClaseCargo.IdEntidad = Convert.ToInt32(model.Cargo.ClaseCargo.DesClase.Split('-')[0]);
                    }
                }
                else
                {
                    ModelState.AddModelError("Clase", "Debe digitar la clase del cargo");
                    throw new Exception("Validacion");
                }
                if (model.Cargo.EspecialidadCargo != null && model.Cargo.EspecialidadCargo.DesEspecialidad != "")
                {
                    if (model.Cargo.EspecialidadCargo.DesEspecialidad != null && model.Cargo.EspecialidadCargo.DesEspecialidad.Contains("-"))
                    {
                        model.Cargo.EspecialidadCargo.IdEntidad = Convert.ToInt32(model.Cargo.EspecialidadCargo.DesEspecialidad.Split('-')[0]);
                    }
                }
                else
                {
                    ModelState.AddModelError("Especialidad", "Debe digitar la especialidad del cargo");
                    throw new Exception("Validacion");
                }
                if (model.Cargo.SubespecialidadCargo != null && model.Cargo.SubespecialidadCargo.DesSubEspecialidad != "")
                {
                    if (model.Cargo.SubespecialidadCargo.DesSubEspecialidad != null && model.Cargo.SubespecialidadCargo.DesSubEspecialidad.Contains("-"))
                    {
                        model.Cargo.SubespecialidadCargo.IdEntidad = Convert.ToInt32(model.Cargo.SubespecialidadCargo.DesSubEspecialidad.Split('-')[0]);
                    }
                }

                if (model.Cargo.SeccionCargo != null && model.Cargo.SeccionCargo.NomSeccion != "")
                {
                    if (model.Cargo.SeccionCargo.NomSeccion != null && model.Cargo.SeccionCargo.NomSeccion.Contains("-"))
                    {
                        model.Cargo.SeccionCargo.IdEntidad = Convert.ToInt32(model.Cargo.SeccionCargo.NomSeccion.Split('-')[0]);
                    }
                }
                else
                {
                    ModelState.AddModelError("Seccion", "Debe digitar la sección del cargo");
                    throw new Exception("Validacion");
                }

                bool entidadLlave = ModelState.Keys.Any(P => P.Contains("IdEntidad"));
                if(entidadLlave)
                {
                    var errores = ModelState.Keys.Where(P => P.Contains("IdEntidad")).ToList();

                    foreach (var error in errores)
                    {
                        ModelState[error].Errors.Clear();
                    }
                }

                if (ModelState.IsValid)
                {
                    var resultado = servicioManual.RegistrarCargo(model.Cargo);

                    if (resultado.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/ManualCargos/Actividades?codigo=" +
                                                ((CBaseDTO)resultado).IdEntidad + "'");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("El formulario contiene errores, por favor revíselo antes de continuar");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Validacion")
                {
                    ModelState.AddModelError("Form", error.Message);
                }
                return PartialView("_ErrorManual");
            }
        }

        public ActionResult Actividades(int codigo)
        {
            ManualCargosVM model = new ManualCargosVM();

            var resultado = servicioManual.ObtenerResultadoCargo(codigo);

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.Resultados = new List<CResultadoCargoDTO>();

                foreach (var item in resultado)
                {
                    model.Resultados.Add((CResultadoCargoDTO)item);
                }

                model.Cargo = new CCargoDTO
                {
                    IdEntidad = ((CResultadoCargoDTO)resultado.FirstOrDefault()).Cargo.IdEntidad,
                    NombreCargo = ((CResultadoCargoDTO)resultado.FirstOrDefault()).Cargo.NombreCargo,
                    Mensaje = "Datos"                    
                };
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
                if (model.Error.MensajeError.Contains("No se encontró ningún resultado relacionado"))
                {
                    model.Error = null;
                    var cargo = servicioManual.ObtenerCargo(codigo);
                    if (cargo.GetType() != typeof(CErrorDTO))
                    {
                        model.Cargo = (CCargoDTO)cargo;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)cargo;
                    }
                }
            }

            return View(model);
        }

        public ActionResult EliminarResultado(int id, int cargo)
        {
            var resultado = servicioManual.EliminarResultado(id);

            if (resultado.GetType() == typeof(CErrorDTO))
            {
                Session["ErrorResultado"] = ((CErrorDTO)resultado).MensajeError;
            }

            return RedirectToAction("Actividades", new { codigo  =  cargo });
        }

        public ActionResult EliminarActividad(int id, int cargo)
        {
            var resultado = servicioManual.EliminarActividad(id);

            if (resultado.GetType() == typeof(CErrorDTO))
            {
                Session["ErrorResultado"] = ((CErrorDTO)resultado).MensajeError;
            }

            return RedirectToAction("Actividades", new { codigo = cargo });
        }

        [HttpPost]
        public ActionResult Actividades(FormCollection form)
        {
            try
            {
                ManualCargosVM model = new ManualCargosVM();
                var resultados = form.AllKeys.Where(P => !P.Contains("actividad") && P.Contains("resultado"));
                if (resultados.Count() > 0)
                {
                    model.Resultados = new List<CResultadoCargoDTO>();
                    foreach (var resultado in resultados)
                    {
                        var tempResultado = new CResultadoCargoDTO
                        {
                            Cargo = new CCargoDTO { IdEntidad = Convert.ToInt32(form["Cargo.IdEntidad"]) },
                            ResultadoCargo = form[resultado]
                        };

                        var actividades = form.AllKeys.Where(P => P.Contains("actividad") && P.Contains(resultado));
                        if (actividades.Count() > 0)
                        {
                            tempResultado.ActividadesResultado = new List<CActividadResultadoCargoDTO>();
                            foreach (var actividad in actividades)
                            {
                                tempResultado.ActividadesResultado.Add(new CActividadResultadoCargoDTO
                                {
                                    ActividadResultadoCargo = form[actividad]
                                });
                            }
                        }

                        model.Resultados.Add(tempResultado);
                    }
                }

                if (form["Cargo.Mensaje"] != null && resultados.Count() < 1)
                {
                    return JavaScript("window.location = '/ManualCargos/Factores?codigo=" +
                        form["Cargo.IdEntidad"] + "'");
                }
                else
                {
                    var respuesta = servicioManual.RegistrarResultadosCargo(model.Resultados.ToArray());

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/ManualCargos/Factores?codigo=" +
                                ((CBaseDTO)respuesta).IdEntidad + "'");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)respuesta).MensajeError);
                    }
                }

            }
            catch (Exception error)
            {
                if (error.Message != "Validacion")
                {
                    ModelState.AddModelError("Form", error.Message);
                }
                return PartialView("_ErrorManual");
            }
        }

        public ActionResult Factores(int codigo = 0)
        {
            ManualCargosVM model = new ManualCargosVM();

            var resultado = servicioManual.ObtenerFactoresCargo(codigo);

            if (resultado.GetType() != typeof(CErrorDTO))
            {
                model.Factor = (CFactorClasificacionCargoDTO)resultado;
            }
            else
            {
                model.Error = (CErrorDTO)resultado;
                if (model.Error.MensajeError.StartsWith("No se encontró ningún"))
                {
                    model.Error = null;
                    var cargo = servicioManual.ObtenerCargo(codigo);
                    if (cargo.GetType() != typeof(CErrorDTO))
                    {
                        model.Factor = new CFactorClasificacionCargoDTO();
                        model.Factor.Cargo = (CCargoDTO)cargo;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)cargo;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Factores(ManualCargosVM model)
        {
            try
            {
                model.Factor.Cargo = new CCargoDTO { IdEntidad = model.Factor.Cargo.IdEntidad };

                var respuesta = servicioManual.ResgistrarFactorCargo(model.Factor);

                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    return JavaScript("window.location = '/ManualCargos/Requerimientos?codigo=" +
                            ((CBaseDTO)respuesta).IdEntidad + "'");
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Validacion")
                {
                    ModelState.AddModelError("Form", error.Message);
                }
                return PartialView("_ErrorManual");
            }
        }

        public ActionResult Requerimientos(int codigo = 0)
        {
            ManualCargosVM model = new ManualCargosVM();

            var resultado = servicioManual.ObtenerRequerimientosEspecificos(codigo);

            if (resultado.GetType() != typeof(CErrorDTO))
            {
                model.RequerimientoEspecifico = (CRequerimientoEspecificoCargoDTO)resultado;
            }
            else
            {
                model.Error = (CErrorDTO)resultado;
                if (model.Error.MensajeError.StartsWith("No se encontró ningún"))
                {
                    model.Error = null;
                    var cargo = servicioManual.ObtenerCargo(codigo);
                    if (cargo.GetType() != typeof(CErrorDTO))
                    {
                        model.RequerimientoEspecifico = new CRequerimientoEspecificoCargoDTO();
                        model.RequerimientoEspecifico.Cargo = (CCargoDTO)cargo;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)cargo;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Requerimientos(ManualCargosVM model)
        {
            try
            {
                model.RequerimientoEspecifico.Cargo = new CCargoDTO { IdEntidad = model.RequerimientoEspecifico.Cargo.IdEntidad };

                var respuesta = servicioManual.RegistrarRequerimientosEspecificos(model.RequerimientoEspecifico);

                if (respuesta.GetType() != typeof(CErrorDTO))
                {
                    return JavaScript("window.location = '/ManualCargos/Competencias?codigo=" +
                            ((CBaseDTO)respuesta).IdEntidad + "'");
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta).MensajeError);
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Validacion")
                {
                    ModelState.AddModelError("Form", error.Message);
                }
                return PartialView("_ErrorManual");
            }
        }

        public ActionResult Competencias(int codigo = 0)
        {
            ManualCargosVM model = new ManualCargosVM();

            var resultado = servicioManual.ObtenerCompetenciaTransversal(codigo);

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.CompetenciasTransversales = new List<CCompetenciaTransversalCargoDTO>();

                foreach (var item in resultado)
                {
                    model.CompetenciasTransversales.Add((CCompetenciaTransversalCargoDTO)item);
                }

                model.Cargo = new CCargoDTO
                {
                    IdEntidad = ((CCompetenciaTransversalCargoDTO)resultado.FirstOrDefault()).Cargo.IdEntidad,
                    NombreCargo = ((CCompetenciaTransversalCargoDTO)resultado.FirstOrDefault()).Cargo.NombreCargo,
                    Mensaje = "Datos"
                };
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
                if (model.Error.MensajeError.Contains("No se encontró ninguna competencia relacionada"))
                {
                    model.Error = null;
                    var cargo = servicioManual.ObtenerCargo(codigo);
                    if (cargo.GetType() != typeof(CErrorDTO))
                    {
                        model.Cargo = (CCargoDTO)cargo;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)cargo;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Competencias(FormCollection form)
        {
            try
            {
                ManualCargosVM model = new ManualCargosVM();
                //var resultados = form.AllKeys.Where(P => P.Contains("resultado"));
                var niveles = form.AllKeys.Where(P => P.Contains("nivel"));

                model.CompetenciasTransversales = new List<CCompetenciaTransversalCargoDTO>();

                foreach (var nivel in niveles)
                {
                    var resultados = form.AllKeys.Where(P => P.Contains("resultado"+nivel.Last()));

                    List<CComportamientoTransversalDTO> comportamientoTemp = new List<CComportamientoTransversalDTO>();

                    foreach (var resultado in resultados)
                    {
                        List<CEvidenciaComportamientoTransversalDTO> evidenciasTemp = new List<CEvidenciaComportamientoTransversalDTO>();
                        var evidencias = form.AllKeys.Where(P => P.Contains("comportamiento" + resultado.Substring(("resultado").Count(), 3)));
                        if (evidencias.Count() > 0)
                        {
                            foreach (var evidencia in evidencias)
                            {
                                evidenciasTemp.Add(new CEvidenciaComportamientoTransversalDTO
                                {
                                    Evidencia = form[evidencia]
                                });
                            }
                            comportamientoTemp.Add(new CComportamientoTransversalDTO
                            {
                                ComportamientoTransversal = form[resultado],
                                EvidenciasComportamientoTransversal = evidenciasTemp
                            });
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    model.CompetenciasTransversales.Add(new CCompetenciaTransversalCargoDTO
                    {
                        NivelDominio = DeterminarNivelDominio(form[nivel]),
                        Cargo = new CCargoDTO { IdEntidad = Convert.ToInt32(form["Cargo.IdEntidad"]) },
                        ComportamientosTransversales = comportamientoTemp,
                        TipoCompetencia = Convert.ToInt32(nivel.Substring(("nivel").Count(), 1))
                    });
                }

                if (niveles.Count() > 0)
                {
                    var respuesta = servicioManual.RegistrarCompetenciasTransversales(model.CompetenciasTransversales.ToArray());

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/ManualCargos/Competencias2?codigo=" +
                                ((CBaseDTO)respuesta).IdEntidad + "'");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)respuesta).MensajeError);
                    }
                }
                else
                {
                    return JavaScript("window.location = '/ManualCargos/Competencias2?codigo=" +
                        form["Cargo.IdEntidad"] + "'");
                }
            }
            catch (Exception error)
            {
                return View();
            }
        }

        public ActionResult EliminarCompetenciasTransversales(int cargo)
        {
            var resultado = servicioManual.EliminarCompetenciasTransversales(cargo);

            if (resultado.GetType() == typeof(CErrorDTO))
            {
                Session["ErrorResultado"] = ((CErrorDTO)resultado).MensajeError;
            }

            return RedirectToAction("Competencias", new { codigo = cargo });
        }

        private int? DeterminarNivelDominio(string nivel)
        {
            switch (nivel)
            {
                case "basico":
                    return 1;
                case "intermedio":
                    return 2;
                case "avanzado":
                    return 3;
                case "destacado":
                    return 4;
                default:
                    return 0;
            }
        }

        public ActionResult Competencias2(int codigo = 0)
        {
            ManualCargosVM model = new ManualCargosVM();

            var resultado = servicioManual.ObtenerCompetenciaGrupo(codigo);

            if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                model.CompetenciasGrupo = new List<CCompetenciaGrupoOcupacionalDTO>();

                foreach (var item in resultado)
                {
                    model.CompetenciasGrupo.Add((CCompetenciaGrupoOcupacionalDTO)item);
                }

                model.Cargo = new CCargoDTO
                {
                    IdEntidad = ((CCompetenciaGrupoOcupacionalDTO)resultado.FirstOrDefault()).Cargo.IdEntidad,
                    NombreCargo = ((CCompetenciaGrupoOcupacionalDTO)resultado.FirstOrDefault()).Cargo.NombreCargo,
                    Mensaje = "Datos"
                };
            }
            else
            {
                model.Error = (CErrorDTO)resultado.FirstOrDefault();
                if (model.Error.MensajeError.Contains("No se encontró ninguna competencia relacionada"))
                {
                    model.Error = null;
                    var cargo = servicioManual.ObtenerCargo(codigo);
                    if (cargo.GetType() != typeof(CErrorDTO))
                    {
                        model.Cargo = (CCargoDTO)cargo;
                    }
                    else
                    {
                        model.Error = (CErrorDTO)cargo;
                    }
                }
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult Competencias2(FormCollection form)
        {
            try
            {
                ManualCargosVM model = new ManualCargosVM();

                var competencias = form.AllKeys.Where(P => P.Contains("competencia"));

                model.CompetenciasGrupo = new List<CCompetenciaGrupoOcupacionalDTO>();

                foreach (var competencia in competencias)
                {
                    var comportamientos = form.AllKeys.Where(P => P.Contains("comportamiento" + competencia.Last()));

                    List<CComportamientoGrupoOcupacionalDTO> comportamientoTemp = new List<CComportamientoGrupoOcupacionalDTO>();

                    foreach (var comportamiento in comportamientos)
                    {
                        List<CEvidenciaGrupoOcupacionalDTO> evidenciasTemp = new List<CEvidenciaGrupoOcupacionalDTO>();

                        var evidencias = form.AllKeys.Where(P => P.Contains("evidencia" + comportamiento.Substring(("comportamiento").Count(), 3)));

                        if (evidencias.Count() > 0)
                        {
                            foreach (var evidencia in evidencias)
                            {
                                evidenciasTemp.Add(new CEvidenciaGrupoOcupacionalDTO
                                {
                                    Evidencia = form[evidencia]
                                });
                            }
                            comportamientoTemp.Add(new CComportamientoGrupoOcupacionalDTO
                            {
                                Comportamiento = form[comportamiento],
                                EvidenciasGrupo = evidenciasTemp
                            });
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    model.CompetenciasGrupo.Add(new CCompetenciaGrupoOcupacionalDTO
                    {
                        Nivel = DeterminarNivelDominio(form["nivelCompetencia"+competencia.Last()]),
                        Cargo = new CCargoDTO { IdEntidad = Convert.ToInt32(form["Cargo.IdEntidad"]) },
                        ComportamientosGrupo = comportamientoTemp,
                        TipoGrupoOcupacional = form[competencia]
                    });
                }

                if (competencias.Count() > 0)
                {
                    var respuesta = servicioManual.RegistrarCompetenciasGrupo(model.CompetenciasGrupo.ToArray());

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        return JavaScript("window.location = '/ManualCargos/Competencias2?codigo=" +
                                ((CBaseDTO)respuesta).IdEntidad + "'");
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)respuesta).MensajeError);
                    }
                }
                else
                {
                    return JavaScript("window.location = '/ManualCargos/DetalleCargo?codigo=" +
                            form["Cargo.IdEntidad"] + "'");
                }
            }
            catch (Exception error)
            {
                return View();
            }
        }

        public ActionResult EliminarCompetenciaGrupo(int id, int cargo)
        {
            var resultado = servicioManual.EliminarCompetenciaGrupo(id);

            if (resultado.GetType() == typeof(CErrorDTO))
            {
                Session["ErrorResultado"] = ((CErrorDTO)resultado).MensajeError;
            }

            return RedirectToAction("Competencias2", new { codigo = cargo });
        }

        public ActionResult EliminarComportamientoGrupo(int id, int cargo)
        {
            var resultado = servicioManual.EliminarComportamientoGrupo(id);

            if (resultado.GetType() == typeof(CErrorDTO))
            {
                Session["ErrorResultado"] = ((CErrorDTO)resultado).MensajeError;
            }

            return RedirectToAction("Competencias2", new { codigo = cargo });
        }

        public ActionResult EliminarEvidenciaGrupo(int id, int cargo)
        {
            var resultado = servicioManual.EliminarEvidenciaGrupo(id);

            if (resultado.GetType() == typeof(CErrorDTO))
            {
                Session["ErrorResultado"] = ((CErrorDTO)resultado).MensajeError;
            }

            return RedirectToAction("Competencias2", new { codigo = cargo });
        }

        public ActionResult DetalleCargo(int codigo)
        {
            ManualCargosVM model = new ManualCargosVM();

            try
            {
                var descripcion = servicioManual.ObtenerCargo(codigo);

                if (descripcion.GetType() != typeof(CErrorDTO))
                {
                    model.Cargo = (CCargoDTO)descripcion;
                }
                else
                {
                    throw new Exception(((CErrorDTO)descripcion).MensajeError);
                }

                var resultado = servicioManual.ObtenerResultadoCargo(codigo);

                if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.Resultados = new List<CResultadoCargoDTO>();

                    foreach (var item in resultado)
                    {
                        model.Resultados.Add((CResultadoCargoDTO)item);
                    }

                    ViewBag.Resultados = "Datos";
                }
                else
                {
                    ViewBag.Resultados = ((CErrorDTO)resultado.FirstOrDefault()).MensajeError;
                }

                var factor = servicioManual.ObtenerFactoresCargo(codigo);

                if (factor.GetType() != typeof(CErrorDTO))
                {
                    model.Factor = (CFactorClasificacionCargoDTO)factor;
                }
                else
                {
                    ViewBag.Factor = ((CErrorDTO)factor).MensajeError;
                }

                var requerimientos = servicioManual.ObtenerRequerimientosEspecificos(codigo);

                if (requerimientos.GetType() != typeof(CErrorDTO))
                {
                    model.RequerimientoEspecifico = (CRequerimientoEspecificoCargoDTO)requerimientos;
                }
                else
                {
                    ViewBag.Requerimiento = ((CErrorDTO)requerimientos).MensajeError;
                }

                var competencias = servicioManual.ObtenerCompetenciaTransversal(codigo);

                if (competencias.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.CompetenciasTransversales = new List<CCompetenciaTransversalCargoDTO>();

                    foreach (var item in competencias)
                    {
                        model.CompetenciasTransversales.Add((CCompetenciaTransversalCargoDTO)item);
                    }
                }
                else
                {
                    ViewBag.Competencias = ((CErrorDTO)competencias.FirstOrDefault()).MensajeError;
                }

                var competenciasGrupo = servicioManual.ObtenerCompetenciaGrupo(codigo);

                if (competenciasGrupo.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.CompetenciasGrupo = new List<CCompetenciaGrupoOcupacionalDTO>();

                    foreach (var item in competenciasGrupo)
                    {
                        model.CompetenciasGrupo.Add((CCompetenciaGrupoOcupacionalDTO)item);
                    }
                }
                else
                {
                    ViewBag.CompetenciasGrupo = ((CErrorDTO)competenciasGrupo.FirstOrDefault()).MensajeError;
                }

                return View(model);
            }
            catch (Exception error)
            {
                model.Error = new CErrorDTO { MensajeError = error.Message };
                return View(model);
            }
        }
    }
}