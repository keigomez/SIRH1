using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CTipoIndicadorMetaL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoIndicadorMetaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CTipoIndicadorMetaDTO ConvertirTipoADto(TipoIndicadorMeta item)
        {
            return new CTipoIndicadorMetaDTO
            {
                IdEntidad = item.PK_TipoIndicador,
                DesTipoIndicadorMeta = item.DesTipoIndicador
            };
        }


        /// <summary>
        /// Busca todos los Tipos Indicador de Meta registrados en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<CBaseDTO> RetornarTipos()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CTipoIndicadorMetaD intermedio = new CTipoIndicadorMetaD(contexto);

                var tipoInc = intermedio.RetornarTipoIndicadorMeta();

                if (tipoInc.Codigo > 0)
                {
                    foreach (var tipo in (List<TipoIndicadorMeta>)tipoInc.Contenido)
                    {
                        var datoTipoInc = ConvertirTipoADto((TipoIndicadorMeta)tipo);
                        resultado.Add(datoTipoInc);
                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)tipoInc.Contenido);
                }
            }
            catch (Exception error)
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return resultado;
        }


        /// <summary>
        /// Obtiene un Tipo de Acción de Personal registrado en la BD
        /// </summary>
        /// <returns>Retorna el Tipo de Incapacidad</returns>
        public List<CBaseDTO> ObtenerTipo(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CTipoIndicadorMetaD intermedio = new CTipoIndicadorMetaD(contexto);

                var tipo = intermedio.CargarTipoIndicadorMetaPorID(codigo);

                if (tipo.Codigo > 0)
                {
                    var dato = ConvertirTipoADto((TipoIndicadorMeta)tipo.Contenido);
                    respuesta.Add(dato);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipo.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }


        //public CBaseDTO AgregarTipo(CTipoAccionPersonalDTO tipo)
        //{
        //    CBaseDTO respuesta;

        //    try
        //    {
        //        CTipoAccionPersonalD intermedio = new CTipoAccionPersonalD(contexto);

        //        var dato = new TipoAccionPersonal
        //        {
        //            PK_TipoAccionPersonal = tipo.IdEntidad,
        //            DesTipoAccion = tipo.DesTipoAccion
        //        };

        //        var tipoInc = intermedio.GuardarTipoAccionPersonal(dato);

        //        if (tipoInc.Codigo > 0)
        //        {
        //            respuesta = new CEntidadMedicaDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
        //        }
        //        else
        //        {
        //            respuesta = ((CErrorDTO)tipoInc.Contenido);
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CErrorDTO
        //        {
        //            Codigo = -1,
        //            MensajeError = error.Message
        //        };
        //    }

        //    return respuesta;
        //}



        //public CBaseDTO EditarTipo(CTipoAccionPersonalDTO tipo)
        //{
        //    CBaseDTO respuesta;

        //    try
        //    {
        //        CTipoAccionPersonalD intermedio = new CTipoAccionPersonalD(contexto);

        //        var dato = new TipoAccionPersonal
        //        {
        //            PK_TipoAccionPersonal = tipo.IdEntidad,
        //            DesTipoAccion = tipo.DesTipoAccion
        //        };

        //        var tipoInc = intermedio.EditarTipoAccionPersonal(dato);

        //        if (tipoInc.Codigo > 0)
        //        {
        //            respuesta = new CTipoAccionPersonalDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
        //        }
        //        else
        //        {
        //            respuesta = ((CErrorDTO)tipoInc.Contenido);
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CErrorDTO
        //        {
        //            Codigo = -1,
        //            MensajeError = error.Message
        //        };
        //    }

        //    return respuesta;
        //}

        #endregion

    }
}