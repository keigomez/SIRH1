using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatalogoDiaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatalogoDiaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Busca el catalogo del día en la BD con el código que recibe por parámetros
        /// </summary>
        /// <returns>Retorna el registro del catálogo especificado</returns>
        public CRespuestaDTO BuscarCatalogoDia(int codDia)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoDia.Include("TipoDia").Where(C => C.PK_CatalogoDia == codDia).FirstOrDefault();

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
                    throw new Exception("No se encontró el día indicado");
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
        /// Busca el catálogo completo de días en la BD
        /// </summary>
        /// <returns>Retorna todos los registros del catálogo</returns>
        public CRespuestaDTO ListarCatalogoDia()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoDia.ToList();

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
                    throw new Exception("No se encontró ningún día registrado");
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
        /// Almacena un día de asueto en la tabla respectiva
        /// </summary>
        /// <returns>Retorna el día registrado</returns>
        public CRespuestaDTO AgregarDiaAsueto(CatalogoDia diaAsueto, TipoDia tipoDia )
        {
            CRespuestaDTO respuesta;
            try
            {

                entidadBase.CatalogoDia.Add(diaAsueto);

               entidadBase.TipoDia.Where(Q => Q.PK_TipoDia == tipoDia.PK_TipoDia).FirstOrDefault()
                                               .CatalogoDia.Add(diaAsueto);

                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = diaAsueto.PK_CatalogoDia
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

        /// <summary>
        /// Busca el catalogo del día en la BD según el tipo enviado por parámetros
        /// </summary>
        /// <returns>Retorna el cátalogo de días según el tipo especificado</returns>
        public CRespuestaDTO ListarCatalogoDiaPorTipo(int tipo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatalogoDia.Where(CA => CA.TipoDia.PK_TipoDia == tipo).ToList();

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
                    throw new Exception("No se encontró ningún día registrado correspondiente al tipo indicado");
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
