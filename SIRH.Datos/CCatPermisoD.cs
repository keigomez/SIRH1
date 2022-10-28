using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatPermisoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatPermisoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO DescargarPermisosPerfil(int idPerfil)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.CatPermiso.Where(Q => Q.Perfil.PK_Perfil == idPerfil).ToList();

                if (datos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron permisos registrados para el perfil indicado.");
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

        public CRespuestaDTO AgregarPermiso(CatPermiso permiso, int idPerfil)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.CatPermiso.Add(permiso);
                entidadBase.Perfil.Where(Q => Q.PK_Perfil == idPerfil).FirstOrDefault()
                                                .CatPermiso.Add(permiso);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = permiso
                };
                return respuesta;

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

        public CRespuestaDTO ListarPermisos()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.CatPermiso.
                    OrderBy(Q => Q.Perfil.PK_Perfil).ToList();

                if (datos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron permisos registrados.");
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

        #endregion
    }
}
