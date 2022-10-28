using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEstadoViaticoCorridoD
    {
        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoViaticoCorridoD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// metodo encargado de buscar un estado por el nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public CRespuestaDTO BuscarEstadoViaticoCorridoNombre(string nombre)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoViaticoCorrido
                                                .FirstOrDefault(E => E.NomEstado == nombre);
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
                    throw new Exception("No se encontró ningún estado de viatico corrido");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        /// <summary>
        /// metodo encargado de Listar los estado de viatico corrido
        /// </summary>
        /// <returns></returns>
        public CRespuestaDTO ListarEstadoViaticoCorrido()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoViaticoCorrido.ToList();
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
                    throw new Exception("No se encontró estados de viatico Corrido");
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
