using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTipoIndicadorMetaD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoIndicadorMetaD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO RetornarTipoIndicadorMeta()
        {
            CRespuestaDTO respuesta;
            try
            {
                var tipo = contexto.TipoIndicadorMeta.ToList();

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
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Indicador de Meta"
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


        public CRespuestaDTO CargarTipoIndicadorMetaPorID(int idTipoIndicadorMeta)
        {
            CRespuestaDTO respuesta;
            try
            {
                TipoIndicadorMeta tipo = new TipoIndicadorMeta();

                tipo = contexto.TipoIndicadorMeta.Where(T => T.PK_TipoIndicador == idTipoIndicadorMeta).FirstOrDefault();

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
                        Contenido = "Ocurrió un error al leer los datos del Tipo de Indicador de Meta"
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


        //public CRespuestaDTO GuardarTipoAccionPersonal(TipoAccionPersonal tipo)
        //{
        //    CRespuestaDTO respuesta;

        //    try
        //    {
        //        contexto.TipoAccionPersonal.Add(tipo);
        //        contexto.SaveChanges();

        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = 1,
        //            Contenido = tipo.PK_TipoAccionPersonal
        //        };
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { MensajeError = error.Message }
        //        };
        //    }

        //    return respuesta;
        //}


        //public CRespuestaDTO EditarTipoAccionPersonal(TipoAccionPersonal tipo)
        //{
        //    CRespuestaDTO respuesta;
        //    try
        //    {
        //        var datosTipo = contexto.TipoAccionPersonal
        //                        .Where(T => T.PK_TipoAccionPersonal == tipo.PK_TipoAccionPersonal)
        //                        .FirstOrDefault();

        //        if (datosTipo != null)
        //        {
        //            datosTipo.DesTipoAccion = tipo.DesTipoAccion;

        //            tipo = datosTipo;
        //            contexto.SaveChanges();

        //            respuesta = new CRespuestaDTO
        //            {
        //                Codigo = 1,
        //                Contenido = tipo.PK_TipoAccionPersonal
        //            };
        //        }
        //        else
        //        {
        //            respuesta = new CRespuestaDTO
        //            {
        //                Codigo = -1,
        //                Contenido = "No se encontraron datos del Tipo de Acción de Personal asociados a la clave especificada"
        //            };
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { MensajeError = error.Message }
        //        };
        //    }
        //    return respuesta;
        //}

        #endregion
    }
}