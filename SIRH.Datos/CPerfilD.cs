using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPerfilD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPerfilD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Carga los Perfiles
        /// </summary>
        /// <returns>Retorna los perfiles</returns>
        public CRespuestaDTO DescargarPerfiles()
        {
            CRespuestaDTO respuesta;
            try
            {
                var perfiles = entidadBase.Perfil.Include("CatPermiso").ToList();
                if (perfiles != null)
                {
                    respuesta = new CRespuestaDTO 
                    {
                        Codigo = 1,
                        Contenido = perfiles
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron perfiles registrados en el sistema");
                }
            }
            catch(Exception error)
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
        /// Cargar el perfil de un usuario específico
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns>Retorna un perfil</returns>
        public List<Perfil> DescargarPerfilesUsuario(string nombreUsuario)
        {
            List<Perfil> resultado = new List<Perfil>();
            var perfiles = entidadBase.Perfil.Include("CatPermiso")
                                                //.Include("CatPermiso.PerfilAcceso")
                                                //.Include("CatPermiso.PerfilAcceso.DetalleAcceso")
                                                //.Include("CatPermiso.PerfilAcceso.DetalleAcceso.Funcionario")
                                                //.Include("CatPermiso.PerfilAcceso.DetalleAcceso.Usuario")
                                                .Where(Q => Q.CatPermiso
                                                    .Where(R => R.PerfilAcceso
                                                        .Where(K => K.DetalleAcceso.Usuario.NomUsuario.Equals(nombreUsuario) 
                                                            && K.CatPermiso.PK_CatPermiso.Equals(R.PK_CatPermiso)).Count() > 0).Count() > 0).ToList();

            if (perfiles != null)
            {
                resultado = perfiles;
            }
            return resultado;
        }

        public CRespuestaDTO AgregarPerfil(Perfil perfil)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Perfil.Add(perfil);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO 
                {
                    Codigo  = 1,
                    Contenido = perfil
                };
                return respuesta;
            }
            catch(Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        #endregion
    }
}
