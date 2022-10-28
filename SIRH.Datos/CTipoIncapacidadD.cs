using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public  class CTipoIncapacidadD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoIncapacidadD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene los tipos de Incapacidad de la BD
        /// </summary>
        /// <returns>Retorna Tipos de Incapacidad</returns>
        public CRespuestaDTO RetornarTipoIncapacidad()
        {
            CRespuestaDTO respuesta;
            try
            {
                var tipo = contexto.TipoIncapacidad.ToList();

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
                    //throw new Exception("Ocurrió un error al leer los datos del Tipo de Incapacidad");
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Incapacidad"
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


        public CRespuestaDTO CargarTipoIncapacidadPorID(int idTipoIncapacidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                TipoIncapacidad tipo = new TipoIncapacidad();

                tipo = contexto.TipoIncapacidad.Where(T => T.PK_TipoIncapacidad == idTipoIncapacidad).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Incapacidad"
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


        public CRespuestaDTO GuardarTipoIncapacidad(TipoIncapacidad tipo)
        {
            CRespuestaDTO respuesta;

            try
            {
                contexto.TipoIncapacidad.Add(tipo);
                contexto.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = tipo.PK_TipoIncapacidad
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


        public CRespuestaDTO EditarTipoIncapacidad(TipoIncapacidad tipo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosTipo = contexto.TipoIncapacidad
                                .Where(T => T.PK_TipoIncapacidad == tipo.PK_TipoIncapacidad)
                                .FirstOrDefault();

                if (datosTipo != null)
                {
                    datosTipo.DesIncapacidad = tipo.DesIncapacidad;

                    tipo = datosTipo;
                    contexto.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo.PK_TipoIncapacidad
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontraron datos del Tipo de Incapacidad asociados a la clave especificada"
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