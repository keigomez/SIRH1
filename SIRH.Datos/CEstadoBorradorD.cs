using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEstadoBorradorD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoBorradorD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene y Enlista los Estados de Borrador de la BD
        /// </summary>
        /// <returns>Retorna los estados de los Borrador</returns>
        public CRespuestaDTO RetornarEstadosBorrador()
        {
            CRespuestaDTO respuesta;
            try
            {
                var tipo = entidadBase.EstadoBorrador.ToList();

                if (tipo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo
                    };
                }
                else
                {
                    //throw new Exception("Ocurrió un error al leer los datos de los Estados de Borrador");
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de los Estados de Borrador"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }

        /// <summary>
        /// Obtiene la lista de las Estados de Borradors de la BD
        /// </summary>
        /// <returns>Retorna una lista de Especialidades</returns>
        public List<EstadoBorrador> CargarEstadoBorrador()
        {
            List<EstadoBorrador> resultados = new List<EstadoBorrador>();

            resultados = entidadBase.EstadoBorrador.ToList();

            return resultados;
        }


        /// <summary>
        /// Obtiene la carga de los Estados de los Borrador de la BD
        /// </summary>
        /// <returns>Retorna los Estados de los Borrador</returns>
        public CRespuestaDTO CargarEstadoBorradorPorID(int idEstadoBorrador)
        {
            CRespuestaDTO respuesta;
            try
            {
                EstadoBorrador tipo = new EstadoBorrador();

                tipo = entidadBase.EstadoBorrador.Where(T => T.PK_EstadoBorrador == idEstadoBorrador).FirstOrDefault();

                if (tipo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de los Estados de Borrador"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }

        #endregion
    }
}