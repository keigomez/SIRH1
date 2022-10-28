using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEntidadMedicaD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CEntidadMedicaD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }

        #endregion
        
        #region Métodos

        /// <summary>
        /// Obtiene las Entidades Médicas de la BD
        /// </summary>
        /// <returns>Retorna Entidades Médicas</returns>
        public CRespuestaDTO RetornarEntidadMedica()
        {
            CRespuestaDTO respuesta;
            try
            {
                var entidad = contexto.EntidadMedica.ToList();

                if (entidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de las Entidades Médicas"
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

        public CRespuestaDTO CargarEntidadMedicaPorID(int idEntidadMedica)
        {
            CRespuestaDTO respuesta;
            try
            {
                EntidadMedica entidad = new EntidadMedica();

                entidad = contexto.EntidadMedica.Where(E => E.PK_EntidadMedica == idEntidadMedica).FirstOrDefault();

                if (entidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Entidad Médica"
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

        public CRespuestaDTO GuardarEntidadMedica(EntidadMedica entidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.EntidadMedica.Add(entidad);
                contexto.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = entidad.PK_EntidadMedica
                }; 
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

        public CRespuestaDTO EditarEntidadMedica(EntidadMedica entidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = contexto.EntidadMedica
                                .Where(E => E.PK_EntidadMedica == entidad.PK_EntidadMedica)
                                .FirstOrDefault();

                if (datosEntidad != null)
                {
                    datosEntidad.DesEntidad = entidad.DesEntidad;

                    entidad = datosEntidad;
                    contexto.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad.PK_EntidadMedica
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontraron datos de la Entidad Médica asociados a la clave especificada"
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