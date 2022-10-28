using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.FuncionarioLocal;
using SIRH.Web.PerfilUsuarioLocal;
using SIRH.Web.ViewModels;
using SIRH.DTO;
using System.DirectoryServices.AccountManagement;


namespace SIRH.Web.Controllers
{
    public class GestionUsuarioController : Controller
    {
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        //
        // GET: /GestionUsuario/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /GestionUsuario/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /GestionUsuario/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GestionUsuario/Create

        [HttpPost]
        public ActionResult Create(PerfilUsuarioVM model, string submit)
        {
            try
            {
                if (submit.Equals("Buscar"))
                {
                    if (ModelState.IsValid)
                    {
                        var resultado = servicioFuncionario.BuscarFuncionarioBase(model.Funcionario.Cedula);
                        if (resultado.GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)resultado;

                            PrincipalContext pc = new PrincipalContext(ContextType.Domain);
                            UserPrincipal up = UserPrincipal.FindByIdentity(pc, "MOPT\\" + model.Usuario.NombreUsuario);
                            model.Usuario.EmailOficial = up.EmailAddress;
                            model.Usuario.TelefonoOficial = up.VoiceTelephoneNumber;


                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError("SIRHError", ((CErrorDTO)resultado).MensajeError);
                            throw new Exception(((CErrorDTO)resultado).MensajeError);
                        }
                    }
                    else
                    {
                        return View(model);
                    }
                }
                else
                {
                    if (submit.Equals("Registrar"))
                    {

                    }
                    else
                    {

                    }
                }
                return View(model);
            }
            catch (Exception error)
            {
                return View(model);
            }
        }

        //
        // GET: /GestionUsuario/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /GestionUsuario/Edit/5

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
        // GET: /GestionUsuario/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /GestionUsuario/Delete/5

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
