using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.PerfilUsuarioLocal;
using SIRH.DTO;

namespace SIRH.Web.Helpers
{
    public static class GestionUsuariosHelper
    {
        public static string UsuarioPermitido(CPerfilUsuarioServiceClient servicioUsuario, string nombreUsuario, int permiso, int perfil)
        {
            string respuesta;

            var dato = servicioUsuario.CargarPerfilUsuarioEspecifico(nombreUsuario, permiso,
                                                                        perfil);

            if (dato.GetType() == typeof(CErrorDTO))
            {
                if (((CErrorDTO)dato).MensajeError.StartsWith("El usuario no tiene permisos"))
                {
                    respuesta = "Denegado";
                }
                else
                {
                    respuesta = "Error " + ((CErrorDTO)dato).MensajeError;
                }
            }
            else
            {
                respuesta = "Autorizado";
            }

            return respuesta;
        }
    }
}