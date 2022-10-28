using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEstadoGastoTransporteD
    {
        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoGastoTransporteD(SIRHEntities entidadGlobal)
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
        public CRespuestaDTO BuscarEstadoGastoTransporteNombre(string nombre)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoGastoTransporte
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
                    throw new Exception("No se encontró ningún estado de Gasto Transporte");
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
        /// metodo encargado de Listar los estados de Gasto Transporte
        /// </summary>
        /// <returns></returns>
        /// 
        public CRespuestaDTO BuscarEstadoGastoTransporteId(int id)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoGastoTransporte
                                                .FirstOrDefault(E => E.PK_EstadoGastosTransporte == id);
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
                    throw new Exception("No se encontró ningún estado de Gasto Transporte");
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

        public CRespuestaDTO ListarEstadoGastoTransporte()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadesBase.EstadoGastoTransporte.ToList();
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
                    throw new Exception("No se encontró estados de Gasto Transporte");
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
