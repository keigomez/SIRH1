using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos.Helpers;
namespace SIRH.Datos
{
    /// <summary>
    /// Clase que administrara los datos de la tabla CCatalogoPreguntas de la base de datos.
    /// </summary>
    public class CCatalogoPreguntaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatalogoPreguntaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Añade preguntas al formulario tabla respectiva
        /// </summary>
        /// <param name="pregunta">Tipo CatalogoPreguntaDTO</param>
        /// <returns></returns>
        public CRespuestaDTO AgregarPregunta(CatalogoPregunta pregunta)
        {
            
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.CatalogoPregunta.Add(pregunta);

                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = pregunta.PK_CatalogoPregunta
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
        /// Modifica el estado de las preguntas del formulario tabla respectiva
        /// </summary>
        /// <param name="pregunta">Tipo CatalogoPreguntaDTO</param>
        /// <returns></returns>

        public CRespuestaDTO EditarPregunta(CatalogoPregunta pregunta)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datospregunta = entidadBase.CatalogoPregunta.Where(C => C.PK_CatalogoPregunta == pregunta.PK_CatalogoPregunta).FirstOrDefault();

                if (datospregunta != null)
                {
                    datospregunta.IndEstado = pregunta.IndEstado;

                    pregunta = datospregunta;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = pregunta.PK_CatalogoPregunta
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron preguntas asociadas a la clave especificada.");
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

        /// <summary>
        /// Obtiene preguntas del catalogo para el formulario
        /// </summary>
        /// <param name="codPregunta">Parametro tipo int</param>
        /// <returns></returns>
        public CRespuestaDTO ObtenerPreguntas(int codPregunta)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datospregunta = entidadBase.CatalogoPregunta.Where(C => C.PK_CatalogoPregunta == codPregunta).FirstOrDefault();

                if (datospregunta != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datospregunta
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Preguntas asociadas a la clave especificada.");
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
        /// <summary>
        /// Metodo encargado de traer las preguntas para el tipo de formulario.
        /// </summary>
        /// <param name="tipoFormulario">Tipo int</param>
        /// <returns></returns>
        public List<CatalogoPregunta> ListarPreguntas(int tipoFormulario)
        {
            List<CatalogoPregunta> resultado = new List<CatalogoPregunta>();

            try
            {
                var datospregunta = entidadBase.CatalogoPregunta.Where(C => C.IndTipoFormulario == tipoFormulario && C.IndEstado != 2).ToList();
                if (datospregunta != null)
                {
                    resultado = datospregunta;
                    return resultado.OrderByDescending(Q => Q.IndEstado).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron Preguntas asociadas al tipoFormulario.");
                }
            }
            catch (Exception error) {
            }
            
     
            return resultado;
        }



        #endregion
    }
}
