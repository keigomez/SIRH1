using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.DTO
{
    public class CTipoDeduccionL
    {
         #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoDeduccionL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Convierte una entidad de tipo deduccion a DTO
        /// </summary>
        /// <returns>Retorna el tipo de deducción como objeto DTO</returns>
        internal static CTipoDeduccionDTO ConvertirTipoDiaADto(TipoDeduccion item)
        {
            return new CTipoDeduccionDTO
            {
                IdEntidad = item.PK_TipoDeduccion,
                DescripcionTipoDeduccion = item.DesTipDeduccion
            };
        }

        /// <summary>
        /// Busca todos los tipo de deducción registrados en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<List<CBaseDTO>> RetornarTiposDeduccion()
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();
             
            try
            {
            CTipoDeduccionD intermedio = new CTipoDeduccionD(contexto);
           
                var tipoDeduccion = intermedio.ListarTipoDeduccion();

                if (tipoDeduccion.Codigo > 0)
                {
                    foreach (var pago in (List<TipoDeduccion>)tipoDeduccion.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoTipoDeduccion = ConvertirTipoDiaADto(pago);
                        temporal.Add(datoTipoDeduccion);

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)tipoDeduccion.Contenido);
                    resultado.Add(temporal);
                }
                
            }
            catch (Exception error)
            {
                temporal.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                resultado.Add(temporal);
            }
            return resultado;
        }

        /// <summary>
        /// Obtiene un tipo de deducción registrado en la BD
        /// </summary>
        /// <returns>Retorna el tipo de deducción</returns>
        public List<CBaseDTO> ObtenerTipoDeduccion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CTipoDeduccionD intermedio = new CTipoDeduccionD(contexto);

                var tipoDeduccion = intermedio.BuscarTipoDeduccion(codigo);

                if (tipoDeduccion.Codigo > 0)
                {

                    var datoDeduccion = ConvertirTipoDiaADto((TipoDeduccion)tipoDeduccion.Contenido);
                    respuesta.Add(datoDeduccion);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipoDeduccion.Contenido);
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

        #endregion

    }
}
