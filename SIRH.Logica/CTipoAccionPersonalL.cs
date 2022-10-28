using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CTipoAccionPersonalL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoAccionPersonalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CTipoAccionPersonalDTO ConvertirTipoAccionPersonalADto(TipoAccionPersonal item)
        {
            return new CTipoAccionPersonalDTO
            {
                IdEntidad = item.PK_TipoAccionPersonal,
                DesTipoAccion = item.DesTipoAccion,
                IndCategoria = item.IndCategoria
            };
        }


        /// <summary>
        /// Busca todos los Tipos de Acciones de Personal registrados en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<CBaseDTO> RetornarTipos()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CTipoAccionPersonalD intermedio = new CTipoAccionPersonalD(contexto);

                var tipoInc = intermedio.RetornarTipoAccionPersonal();

                if (tipoInc.Codigo > 0)
                {
                    foreach (var tipo in (List<TipoAccionPersonal>)tipoInc.Contenido)
                    {
                        var datoTipoInc = ConvertirTipoAccionPersonalADto((TipoAccionPersonal)tipo);
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
                CTipoAccionPersonalD intermedio = new CTipoAccionPersonalD(contexto);

                var tipo = intermedio.CargarTipoAccionPersonalPorID(codigo);

                if (tipo.Codigo > 0)
                {
                    var dato = ConvertirTipoAccionPersonalADto((TipoAccionPersonal)tipo.Contenido);
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


        public CBaseDTO AgregarTipo(CTipoAccionPersonalDTO tipo)
        {
            CBaseDTO respuesta;

            try
            {
                CTipoAccionPersonalD intermedio = new CTipoAccionPersonalD(contexto);

                var dato = new TipoAccionPersonal
                {
                    PK_TipoAccionPersonal = tipo.IdEntidad,
                    DesTipoAccion = tipo.DesTipoAccion
                };

                var tipoInc = intermedio.GuardarTipoAccionPersonal(dato);

                if (tipoInc.Codigo > 0)
                {
                    respuesta = new CEntidadMedicaDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)tipoInc.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }



        public CBaseDTO EditarTipo(CTipoAccionPersonalDTO tipo)
        {
            CBaseDTO respuesta;

            try
            {
                CTipoAccionPersonalD intermedio = new CTipoAccionPersonalD(contexto);

                var dato = new TipoAccionPersonal
                {
                    PK_TipoAccionPersonal = tipo.IdEntidad,
                    DesTipoAccion = tipo.DesTipoAccion
                };

                var tipoInc = intermedio.EditarTipoAccionPersonal(dato);

                if (tipoInc.Codigo > 0)
                {
                    respuesta = new CTipoAccionPersonalDTO { IdEntidad = Convert.ToInt32(tipoInc.Contenido) };
                }
                else
                {
                    respuesta = ((CErrorDTO)tipoInc.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        #endregion

    }
}