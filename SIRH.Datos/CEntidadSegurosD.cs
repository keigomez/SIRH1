using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEntidadSegurosD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEntidadSegurosD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Métodos

        public CRespuestaDTO ObtenerEntidadSeguros(int codEntidadSeguros)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosEntidad = entidadBase.EntidadSeguros.Where(M => M.PK_EntidadSeguros == codEntidadSeguros).FirstOrDefault();

                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ninguna Entidad de Seguros asociada a la clave especificada.");
                }
            }
            catch(Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                
                return respuesta;
            }
        }

        public CRespuestaDTO ListarEntidadSeguros()
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosEntidad = entidadBase.EntidadSeguros.ToList();

                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Entidades de Seguros asociadas.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }
        
        #endregion
    }
}
