using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEstadoTramiteD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoTramiteD (SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca un estado de trámite especifico
        /// </summary>
        /// <returns>Retorna el estado de trámite específico</returns>
        public CRespuestaDTO BuscarEstadoTramite(int codTipo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.EstadoTramite.Where(T => T.PK_EstadoTramite == codTipo).FirstOrDefault();

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
                    throw new Exception("No se encontró el tipo de estado indicado");
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
        /// Carga todos los estados de trámite almacenados en la BD
        /// </summary>
        /// <returns>Retorna los estados encontrados</returns>
        public CRespuestaDTO ListarEstado()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.EstadoTramite.ToList();

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
                    throw new Exception("No se encontró ningún tipo de estado");
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
