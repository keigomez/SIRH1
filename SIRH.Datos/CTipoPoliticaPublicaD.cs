using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTipoPoliticaPublicaD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoPoliticaPublicaD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene los tipos de Acción de Personal de la BD
        /// </summary>
        /// <returns>Retorna Tipos de Incapacidad</returns>
        public CRespuestaDTO RetornarTipoPolitica()
        {
            CRespuestaDTO respuesta;
            try
            {
                var tipo = contexto.TipoPoliticaPublica.ToList();

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
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Política Pública"
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


        public CRespuestaDTO CargarTipoPorID(int idTipoPolitica)
        {
            CRespuestaDTO respuesta;
            try
            {
                TipoPoliticaPublica tipo = new TipoPoliticaPublica();

                tipo = contexto.TipoPoliticaPublica.Where(T => T.PK_TipoPP == idTipoPolitica).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Política Pública"
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


        public CRespuestaDTO GuardarTipo (TipoPoliticaPublica tipo)
        {
            CRespuestaDTO respuesta;

            try
            {
                contexto.TipoPoliticaPublica.Add(tipo);
                contexto.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = tipo.PK_TipoPP
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


        public CRespuestaDTO EditarTipo(TipoPoliticaPublica tipo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosTipo = contexto.TipoPoliticaPublica
                                .Where(T => T.PK_TipoPP == tipo.PK_TipoPP)
                                .FirstOrDefault();

                if (datosTipo != null)
                {
                    datosTipo.DesTipoPP = tipo.DesTipoPP;

                    tipo = datosTipo;
                    contexto.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tipo.PK_TipoPP
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontraron datos del Tipo de Política Pública asociados a la clave especificada"
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