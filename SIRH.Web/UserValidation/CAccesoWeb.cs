using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.Web.Helpers;
//using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.PerfilUsuarioLocal;
using SIRH.DTO;

namespace SIRH.Web.UserValidation
{
    public class CAccesoWeb
    {
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();

        internal static string GenerarCadenaPermiso(EModulosHelper modulo, int nivel)
        {
            return "Permiso_" + Convert.ToInt32(modulo).ToString() + "_" + nivel.ToString();
        }

        internal static HttpSessionStateBase CargarErrorAcceso(HttpSessionStateBase sesionActiva)
        {
            sesionActiva["Tipo_Error"] = EErrorAcceso.Acceso;
            sesionActiva["Detalle_Error"] = "Usted no tiene permiso para acceder a esta sección del sistema. Si requiere acceso contacte a su jefatura directa para que le solicite los permisos correspondientes.";
            return sesionActiva;
        }

        internal static HttpSessionStateBase CargarErrorSistema(string error, HttpSessionStateBase sesionActiva)
        {
            sesionActiva["Tipo_Error"] = EErrorAcceso.Sistema;
            sesionActiva["Detalle_Error"] = error;
            return sesionActiva;
        }

        internal HttpSessionStateBase IniciarSesionModulo(HttpSessionStateBase sesionActiva, string nombreUsuario, int modulo, int permiso)
        {
            try
            {
                if (sesionActiva["Iniciada"] == null)
                {
                    sesionActiva["Iniciada"] = true;
                    sesionActiva["NombreUsuario"] = nombreUsuario;
                }

                if (sesionActiva[modulo.ToString()] == null)
                {
                    sesionActiva[modulo.ToString()] = true;

                    // nota
                    string resultado = GestionUsuariosHelper.UsuarioPermitido(servicioUsuario,
                        nombreUsuario, permiso, modulo);

                    if (resultado == "Denegado")
                    {
                        sesionActiva["Perfil_" + modulo] = "Error";
                        sesionActiva = CargarErrorAcceso(sesionActiva);
                    }
                    else
                    {
                        if (resultado.StartsWith("Error"))
                        {
                            sesionActiva["Perfil_" + modulo] = "Error";
                            sesionActiva["Tipo_Error"] = EErrorAcceso.Sistema;
                            sesionActiva["Detalle_Error"] = resultado;
                        }
                        else
                        {
                            // nota
                            var permisos = servicioUsuario.CargarPerfilUsuarioCompleto(nombreUsuario, permiso, modulo);
                            if (permisos.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {

                                sesionActiva["Perfil_" + modulo] = "Autorizado";

                                sesionActiva["Nombre_Completo"] = ((CFuncionarioDTO)permisos.FirstOrDefault().FirstOrDefault()).Nombre + " "
                                        + ((CFuncionarioDTO)permisos.FirstOrDefault().FirstOrDefault()).PrimerApellido + " " +
                                          ((CFuncionarioDTO)permisos.FirstOrDefault().FirstOrDefault()).SegundoApellido;

                                var perfiles = permisos[1];

                                sesionActiva["Administrador_Global"] = perfiles.Where(Q => Q.GetType() == typeof(CPerfilDTO))
                                    .ToList().Where(Q => Q.IdEntidad == 1)
                                    .ToList().Count > 0 ? true : false;

                                if (Convert.ToBoolean(sesionActiva["Administrador_Global"]) == false)
                                {
                                    sesionActiva["Administrador_" + modulo] = perfiles.Where(Q => Q.GetType() == typeof(CCatPermisoDTO))
                                                                            .ToList().Where(Q => ((CCatPermisoDTO)Q).NomPermiso.Equals("Administrador"))
                                                                           .ToList().Count > 0 ? true : false;

                                    if (Convert.ToBoolean(sesionActiva["Administrador_" + modulo]) == false)
                                    {
                                        var restantes = perfiles.Where(Q => Q.GetType() == typeof(CCatPermisoDTO))
                                                                    .ToList();

                                        foreach (var item in restantes)
                                        {
                                            if (item.GetType() != typeof(CPerfilDTO))
                                            {
                                                sesionActiva["Permiso_" + modulo + "_" + ((CCatPermisoDTO)item).IdEntidad] = ((CCatPermisoDTO)item).NomPermiso;
                                                sesionActiva["Descripcion_Permiso_" + modulo + "_" + ((CCatPermisoDTO)item).NomPermiso.Trim()] = ((CCatPermisoDTO)item).DesPermiso;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                sesionActiva["Perfil_" + modulo] = "Error";
                                sesionActiva["Tipo_Error"] = EErrorAcceso.Sistema;
                                sesionActiva["Detalle_Error"] = ((CErrorDTO)permisos.FirstOrDefault().FirstOrDefault()).MensajeError;
                            }

                        }
                    }
                }
                else
                {
                    return sesionActiva;
                }

                return sesionActiva;
            }
            catch (Exception error)
            {
                sesionActiva["Perfil_" + modulo] = "Error";
                sesionActiva["Tipo_Error"] = EErrorAcceso.Sistema;
                sesionActiva["Detalle_Error"] = error.Message;
                return sesionActiva;
            }
        }

        internal void GuardarBitacora(string username, int modulo, int accion, int objeto, string[] entidades)
        {
            CBitacoraUsuarioDTO bitacora = new CBitacoraUsuarioDTO
            {
                CodigoAccion = accion,
                CodigoModulo = modulo,
                CodigoObjetoEntidad = objeto,
                EntidadesAfectadas = entidades,
                Usuario = new CUsuarioDTO { NombreUsuario = username }
            };

            servicioUsuario.GuardarBitacora(bitacora);
        }

        internal static string[] ListarEntidades(params string[] args)
        {
            List<string> respuesta = new List<string>();
            foreach (var item in args)
            {
                string temp = item.Substring(1, item.Length - 1);
                temp = temp.Substring(0, temp.Length - 3);
                respuesta.Add(temp);
            }
            return respuesta.ToArray();
        }
    }
}
