using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using System.Security.Principal;
using System.Threading;
using System.DirectoryServices.AccountManagement;

namespace SIRH.Datos
{
    public class CUsuarioD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CUsuarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Guarda un Usuario Nuevo en la BD
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Retorna el ID del Nuevo Usuario</returns>
        public int GuardarUsuario(Usuario usuario)
        {
            entidadBase.Usuario.Add(usuario);

            return usuario.PK_Usuario;
        }

        /// <summary>
        /// Obtiene un Usuario de la BD
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns>Retorna un Usuario.</returns>
        public CRespuestaDTO CargarPerfilUsuario(string nombreUsuario)
        {
            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.Usuario.Include("DetalleAcceso").
                                            Include("DetalleAcceso.Funcionario").
                                            Include("DetalleAcceso.Usuario").
                                            Include("DetalleAcceso.PerfilAcceso").
                                            Include("DetalleAcceso.PerfilAcceso.CatPermiso").
                                            Include("DetalleAcceso.PerfilAcceso.CatPermiso.Perfil").
                                            Where(Q => Q.NomUsuario.ToLower().Equals(nombreUsuario.ToLower())).OrderBy(Q=> Q.NomUsuario).FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el usuario con el parámetro que se estableció.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message } 
                };
                return respuesta;
            }
        }

        public CRespuestaDTO CargarPerfilEspecificoUsuario(string nombreUsuario, int catPermiso, int perfil)
        {
            CRespuestaDTO respuesta;

            try
            {
                DetalleAcceso resultado;
                if (catPermiso != 0)
                {
                    resultado = entidadBase.DetalleAcceso.
                            Include("Funcionario").
                            Include("PerfilAcceso").
                            Include("PerfilAcceso.CatPermiso").
                            Include("PerfilAcceso.CatPermiso.Perfil").
                            Where(Q => Q.Usuario.NomUsuario.ToLower().Contains(nombreUsuario.ToLower())
                                    && (Q.PerfilAcceso.Where(P => P.CatPermiso.PK_CatPermiso == catPermiso
                                        || (P.CatPermiso.Perfil.PK_Perfil == perfil
                                            || P.CatPermiso.Perfil.PK_Perfil == 1)
                                        || (P.CatPermiso.NomPermiso.StartsWith("Administrador")
                                            && P.CatPermiso.Perfil.PK_Perfil == perfil))
                                            .Count() > 0)).FirstOrDefault();
                }
                else
                {
                    // Si unicamente se envía el perfil, sin un permiso específico
                    // Para las páginas de inicio de los módulos.
                    resultado = entidadBase.DetalleAcceso.
                               Include("Funcionario").
                               Include("PerfilAcceso").
                               Include("PerfilAcceso.CatPermiso").
                               Include("PerfilAcceso.CatPermiso.Perfil").
                               Where(Q => Q.Usuario.NomUsuario.ToLower().Contains(nombreUsuario.ToLower())
                                       && (Q.PerfilAcceso.Where(P => (P.CatPermiso.Perfil.PK_Perfil == perfil
                                               || P.CatPermiso.Perfil.PK_Perfil == 1)
                                           || (P.CatPermiso.NomPermiso.StartsWith("Administrador")
                                               && P.CatPermiso.Perfil.PK_Perfil == perfil))
                                               .Count() > 0)).FirstOrDefault();
                }

                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("El usuario no tiene permisos para acceder a esta sección del sistema.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO RegistrarUsuario(string nombreUsuario, string cedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                Usuario usuario = entidadBase.Usuario.Where(Q => Q.NomUsuario.Contains(nombreUsuario)).FirstOrDefault();

                if (usuario == null)
                {

                    PrincipalContext pc = new PrincipalContext(ContextType.Domain);

                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, "MOPT\\" + nombreUsuario);

                    if (up != null)
                    {
                        usuario = new Usuario 
                        {
                            NomUsuario = "MOPT\\" + nombreUsuario,
                            EmlOficial = up.EmailAddress,
                            TelOficial = up.VoiceTelephoneNumber,
                        };
                        entidadBase.Usuario.Add(usuario);
                    }
                    else
                    {
                        throw new Exception("El usuario no existe en el directorio oficial del Ministerio.");
                    }
                }
                else
                {
                    throw new Exception("El usuario ya se encuentra registrado en el SIRH.");
                }

                Funcionario funcionario = entidadBase.Funcionario
                                                .Where(Q => Q.IdCedulaFuncionario == cedula)
                                                .FirstOrDefault();

                if (funcionario != null)
                {
                    DetalleAcceso acceso = new DetalleAcceso
                    {
                        Usuario = usuario,
                        Funcionario = funcionario,
                        FecCreacion = DateTime.Now
                    };
                    entidadBase.DetalleAcceso.Add(acceso);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = usuario
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("El funcionario cuya cedula fue suministrada, no se encontró en el SIRH.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO AsignarAccesosUsuario(string nombreUsuario, List<CatPermiso> permisos)
        {
            CRespuestaDTO respuesta;

            try
            {
                string user = "MOPT\\" + nombreUsuario;  
                var acceso = entidadBase.DetalleAcceso.Include("Usuario")
                                                        .Include("Funcionario")
                                                        .Where(Q => Q.Usuario.NomUsuario ==
                                                                user).FirstOrDefault();

                if (acceso != null)
                {
                    foreach (var item in permisos)
                    {
                        PerfilAcceso perfil = new PerfilAcceso 
                        {
                            DetalleAcceso = acceso,
                            FecAsignacion = DateTime.Now,
                            CatPermiso = item
                        };
                        entidadBase.PerfilAcceso.Add(perfil);
                    }
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = acceso
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("El usuario suministrado no se encuentra registrado en el SIRH.");
                }

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        /// <summary>
        /// Carga un Usuario correspondiente a un Nombre y una Cédula
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <param name="cedulaFunc"></param>
        /// <returns>Retorna un Usuario</returns>
        public List<Usuario> CargarUsuariosParam(string nombreUsuario, string cedulaFunc)
        {
            List<Usuario> resultado = entidadBase.Usuario.Include("DetalleAcceso").
                                                            Include("DetalleAcceso.Funcionario").
                                                            Include("DetalleAcceso.PerfilAcceso").
                                                            Include("DetalleAcceso.PerfilAcceso.CatPermiso").
                                                            Include("DetalleAcceso.PerfilAcceso.CatPermiso.Perfil").ToList();
            bool nombreDigitado = false;
            bool cedulaDigitada = false;

            if (nombreUsuario != "" && nombreUsuario != null)
            {
                resultado = resultado.Where(Q => Q.NomUsuario.ToLower().Contains(nombreUsuario.ToLower())).ToList();
            }
            else
            {
                nombreDigitado = true;
            }
            if (cedulaFunc != "" && cedulaFunc != null)
            {
                //resultado = resultado.Where(Q => Q.DetalleAcceso.FirstOrDefault().Funcionario.IdCedulaFuncionario.Contains(cedulaFunc)).ToList();
                resultado = resultado.Where(Q => Q.DetalleAcceso.Count() > 0 &&
                                                Q.DetalleAcceso.FirstOrDefault().Funcionario != null &&
                                                Q.DetalleAcceso.FirstOrDefault().Funcionario.IdCedulaFuncionario.Contains(cedulaFunc))
                                     .ToList();
            }
            else
            {
                cedulaDigitada = true;
            }

            if (resultado.Count < 1)
            {
                resultado = new List<Usuario>();
            }
            if (nombreDigitado && cedulaDigitada)
            {
                resultado = new List<Usuario>();
            }

            return resultado;
        }

        public CRespuestaDTO CargarUsuariosPerfil(int idPerfil, int catPermiso)
        {
            CRespuestaDTO respuesta;
            List<DetalleAcceso> resultado;

            if (catPermiso != 0)
            {
                resultado = entidadBase.DetalleAcceso
                                      .Include("Usuario")
                                      .Include("Funcionario")
                                      .Include("Funcionario.EstadoFuncionario")
                                      .Include("PerfilAcceso")
                                      .Include("PerfilAcceso.CatPermiso")
                                      .Include("PerfilAcceso.CatPermiso.Perfil")
                                      .Where(Q => Q.PerfilAcceso.Where(P => (P.CatPermiso.Perfil.PK_Perfil == idPerfil) &&
                                                    P.CatPermiso.PK_CatPermiso == catPermiso).Count() > 0)
                                      .OrderBy(P => P.Funcionario.NomFuncionario)
                                      .ToList();
            }
            else
            {
                resultado = entidadBase.DetalleAcceso
                                      .Include("Usuario")
                                      .Include("Funcionario")
                                      .Include("Funcionario.EstadoFuncionario")
                                      .Include("PerfilAcceso")
                                      .Include("PerfilAcceso.CatPermiso")
                                      .Include("PerfilAcceso.CatPermiso.Perfil")
                                      .Where(Q => Q.PerfilAcceso.Where(P => (P.CatPermiso.Perfil.PK_Perfil == idPerfil)).Count() > 0)
                                      .OrderBy(P => P.Funcionario.NomFuncionario)
                                      .ToList();
            }


            if (resultado != null)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = resultado
                };

                return respuesta;
            }
            else
            {
                throw new Exception("No se encontraron funcionarios que tengan el Perfil." + idPerfil.ToString());
            }

            return respuesta;
        }

        public CRespuestaDTO DeshabilitarUsuario(string nombreUsuario,string observacion)
        {
            try
            {
                string username = "MOPT\\" + nombreUsuario;
                Usuario resultado = entidadBase.Usuario.Where(u => u.NomUsuario == username).FirstOrDefault();
                if (resultado != null)
                {
                    resultado.IndEstadoUsuario = 2;
                    resultado.Observacion = DateTime.Now + "-" +observacion;
                    entidadBase.SaveChanges();
                }
                else
                {
                    throw new Exception("No se encontró el usuario especificado");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = resultado
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        MensajeError = error.Message.ToString()
                    }
                };
            }
        }

        #endregion
    }
}
