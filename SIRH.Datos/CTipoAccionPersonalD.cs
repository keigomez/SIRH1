using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTipoAccionPersonalD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoAccionPersonalD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene los tipos de Acción de Personal de la BD
        /// </summary>
        /// <returns>Retorna Tipos de Incapacidad</returns>
        public CRespuestaDTO RetornarTipoAccionPersonal()
        {
            CRespuestaDTO respuesta;
            try
            {
                var tipo = contexto.TipoAccionPersonal.ToList();

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
                    //throw new Exception("Ocurrió un error al leer los datos del Tipo de Acción de Personal");
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Acción de Personal"
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


        public CRespuestaDTO CargarTipoAccionPersonalPorID(int idTipoAccionPersonal)
        {
            CRespuestaDTO respuesta;
            try
            {
                TipoAccionPersonal tipo = new TipoAccionPersonal();

                tipo = contexto.TipoAccionPersonal.Where(T => T.PK_TipoAccionPersonal == idTipoAccionPersonal).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Acción de Personal"
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


        public CRespuestaDTO GuardarTipoAccionPersonal(TipoAccionPersonal tipo)
        {
            CRespuestaDTO respuesta;

            try
            {
                contexto.TipoAccionPersonal.Add(tipo);
                contexto.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = tipo.PK_TipoAccionPersonal
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


        public CRespuestaDTO EditarTipoAccionPersonal(TipoAccionPersonal tipo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosTipo = contexto.TipoAccionPersonal
                                .Where(T => T.PK_TipoAccionPersonal == tipo.PK_TipoAccionPersonal)
                                .FirstOrDefault();

                if (datosTipo != null)
                {
                    datosTipo.DesTipoAccion = tipo.DesTipoAccion;

                    tipo = datosTipo;
                    contexto.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo.PK_TipoAccionPersonal
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontraron datos del Tipo de Acción de Personal asociados a la clave especificada"
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