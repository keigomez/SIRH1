using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.Models;
using System.Runtime.Remoting.Contexts;
using SIRH.Web.Helpers;
using System.Security.Principal;

namespace SIRH.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index(int modulo)
        {
            ErrorModel model = new ErrorModel();

            model.Modulo = DeterminarModulo(modulo);

            if ((EErrorAcceso)Session["Tipo_Error"] == EErrorAcceso.Acceso)
            {
                model.Mensaje = "Error de acceso: " + Session["Detalle_Error"].ToString();
            }
            else
            {
                WindowsIdentity principal = WindowsIdentity.GetCurrent();
                model.Mensaje = "Error de sistema: " + Session["Detalle_Error"].ToString() + principal.Name;
            }

            return View(model);
        }

        private string DeterminarModulo(int modulo)
        {
            switch (modulo)
            {
                case 3:
                    return "Módulo de Administración de Usuarios.";
                case 4:
                    return "Módulo de Administración de Cauciones.";
                case 5:
                    return "Módulo de Desarraigo.";
                case 9:
                    return "Módulo de Carrera Profesional y Policial.";
                case 10:
                    return "Módulo de Gestión de Puestos Vacantes.";
                case 11:
                    return "Módulo de Evaluación de Desempeño.";
                case 13:
                    return "Módulo de Incapacidades.";
                case 14:
                    return "Módulo de Planilla.";
                case 15:
                    return "Módulo de Viático Corrido.";
                case 16:
                    return "Módulo de Tiempo Extraordinario.";
                case 17:
                    return "Módulo de Vacaciones.";
                case 18:
                    return "Módulo de Archivo.";
                default:
                    Session["Tipo_Error"] = EErrorAcceso.Sistema;
                    Session["Detalle_Error"] = "El sistema enfrenta un error inesperado, por favor contacte al Administrador del sistema para solicitar la ayuda correspondiente";
                    return "Error general de Sistema.";
            }
        }

        public ActionResult ErrorGeneral(string errorType, int modulo)
        {
            ErrorModel model = new ErrorModel();

            model.Modulo = DeterminarModulo(modulo);

            if(Session["Tipo_Error"] != null)
            {
                if ((EErrorAcceso)Session["Tipo_Error"] == EErrorAcceso.Acceso)
                {
                    model.Mensaje = "Error de acceso: " + Session["Detalle_Error"].ToString();
                }
                else
                {
                    model.Mensaje = "Error de sistema: " + errorType.ToString();
                }
            }
            else
            {
                model.Mensaje = "Error de sistema: " + errorType.ToString();
            }

            return View(model);
        }

    }
}
