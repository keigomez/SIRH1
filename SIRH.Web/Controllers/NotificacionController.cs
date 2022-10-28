using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.DTO;
using SIRH.Web.Helpers;

namespace SIRH.Web.Controllers
{
    public class NotificacionController : Controller
    {
        // GET: Notificacion
        public ActionResult Index(int modulo)
        {
            //Ponerle aquí el search

            return View();
        }

        [HttpPost]
        public ActionResult Index(CNotificacionUsuarioDTO model)
        {
            //Hacer el search y mostrar los details
            return View(model);
        }

        public ActionResult Details(int id)
        {
            //Busca la notificación en la BD

            // Setea el modelo o envía el error
            CNotificacionUsuarioDTO model = new CNotificacionUsuarioDTO();
           
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CNotificacionUsuarioDTO model)
        {
            if (ModelState.IsValid == true)
            {
                //Envía a guardar la notificacion

                //Valida que los datos se insertaron

                if (true)
                {
                    //Si los datos se insertaron correctamente redirecciona a pantalla de éxito
                }
                else
                {
                    //Si los datos no se insertaron envía una excepción
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
            return View(model);
        }
    }
}