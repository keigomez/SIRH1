using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
using SIRH.Web.TipoJornadaLocal;
using SIRH.Web.FuncionarioLocal;
using SIRH.DTO;

namespace SIRH.Web.Controllers
{
    public class JornadasController : Controller
    {
        CFuncionarioServiceClient funcionarioService = new CFuncionarioServiceClient();
        CTipoJornadaServiceClient jornadaService = new CTipoJornadaServiceClient();
        //
        // GET: /Jornadas/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Jornadas/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Jornadas/Create

        public ActionResult Create()
        {
            JornadaFuncionarioVM model = new JornadaFuncionarioVM();



            return View(model);
        }

        //
        // POST: /Jornadas/Create

        [HttpPost]
        public ActionResult Create(JornadaFuncionarioVM model, string submit)
        {
            try
            {
                if (submit == "Buscar")
                {
                    var dato = funcionarioService.BuscarFuncionarioJornada(model.Funcionario.Cedula);

                    if (dato.GetType() != typeof(CErrorDTO))
                    {
                        model.Funcionario = ((CFuncionarioDTO)dato[0]);
                        model.Nombramiento = ((CNombramientoDTO)dato[1]);
                        model.Puesto = ((CPuestoDTO)dato[2]);
                        model.Jornada = ((CTipoJornadaDTO)dato[3]);
                        if (model.Jornada.IdEntidad > 0)
                        {
                            model.Accion = "Editar";
                        }
                        else
                        {
                            model.Accion = "Agregar";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)dato[0]).Mensaje);
                        throw new Exception();
                    }
                }
                else
                {
                    if (model.Accion == "Editar")
                    {
                        var resultado = jornadaService
                                            .EditarJornadaFuncionario(model.Jornada);
                        if (resultado.GetType() != typeof(CErrorDTO))
                        {

                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado[0]).Mensaje);
                            throw new Exception();
                        }
                    }
                    else
                    {
                        var resultado = jornadaService
                                           .EditarJornadaFuncionario(model.Jornada);
                        if (resultado.GetType() != typeof(CErrorDTO))
                        {

                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado[0]).Mensaje);
                            throw new Exception();
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorJornadas");
                }
                else
                {
                    return PartialView("_FormularioJornada", model);
                }
            }
        }

        //
        // GET: /Jornadas/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Jornadas/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Jornadas/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Jornadas/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
