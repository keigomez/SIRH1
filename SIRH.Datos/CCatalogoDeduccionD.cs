using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatalogoDeduccionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatalogoDeduccionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca una deduccion especifica según el párametro recibido
        /// </summary>
        /// <returns>Retorna la deducción</returns>
        public CRespuestaDTO BuscarCatalogoDeduccion(int codCatalogoDeduccion)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoDeduccion.Include("TipoDeduccion").Where(C => C.PK_CatalogoDeduccion == codCatalogoDeduccion).FirstOrDefault();

                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };

                }
                else
                {
                    throw new Exception("No se encontró la deducción indicada");
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
            return respuesta;
        }

        /// <summary>
        /// Retorna todas las deduciones registradas en la BD
        /// </summary>
        /// <returns>Retorna las deducciones en la BD</returns>
        public CRespuestaDTO ListarCatalogoDeduccion()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoDeduccion.ToList();

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
                    throw new Exception("No se encontró ninguna deducción registrada");
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
        /// Busca las deducciones según el tipo 1. Obrero
        ///                                     2. Patronales
        /// </summary>
        /// <returns>Retorna las deducciones</returns>
        public CRespuestaDTO ListarCatalogoDeduccionPorTipo(int tipo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoDeduccion.Where(CD => CD.TipoDeduccion.PK_TipoDeduccion == tipo).ToList();

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
                    throw new Exception("No se encontró ninguna deducción correspondiente al tipo indicado");
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
        /// Retorna las deducciones efectuadas a un pago de feriado según el tipo 1. Obrero
        ///                                                                       2. Patronal
        /// </summary>
        /// <returns>Retorna una lista de deducciones</returns>
        public CRespuestaDTO ListarDeduccionPagoPorTipo(int codigo,int tipo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DeduccionEfectuada.Include("CatalogoDeduccion").Include("CatalogoDeduccion.TipoDeduccion")
                    .Where(CD => CD.PagoFeriado.PK_PagoFeriado==codigo && CD.CatalogoDeduccion.TipoDeduccion.PK_TipoDeduccion==tipo).ToList();

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
                    throw new Exception("No se encontró ninguna deducción correspondiente al tipo indicado o trámite de pago feriado");
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
