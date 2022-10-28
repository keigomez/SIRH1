using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CTipoDiaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTipoDiaL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Convierte una entidad de tipo de día a DTO
        /// </summary>
        /// <returns>Retorna el tipo de día como objeto DTO</returns>
        internal static CTipoDiaDTO ConvertirTipoDiaADto(TipoDia item)
        {
            return new CTipoDiaDTO
            {
                IdEntidad = item.PK_TipoDia,
                DescripcionTipoDia = item.DesTipoDia
            };
        }

        /// <summary>
        /// Busca todos los tipo de día registrados en la BD
        /// </summary>
        /// <returns>Retorna los registros</returns>
        public List<List<CBaseDTO>> RetornarTiposDia()
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CTipoDiaD intermedio = new CTipoDiaD(contexto);

                var tipoDia = intermedio.ListarTipoDia();

                if (tipoDia.Codigo > 0)
                {
                    foreach (var pago in (List<TipoDia>)tipoDia.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoTipoDia = ConvertirTipoDiaADto(pago);
                        temporal.Add(datoTipoDia);

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)tipoDia.Contenido);
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
        /// Obtiene un tipo de día registrado en la BD
        /// </summary>
        /// <returns>Retorna el tipo de día</returns>
        public List<CBaseDTO> ObtenerTipoDia(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CTipoDiaD intermedio = new CTipoDiaD(contexto);

                var tipoDia = intermedio.BuscarTipoDia(codigo);

                if (tipoDia.Codigo > 0)
                {

                    var datoDeduccion = ConvertirTipoDiaADto((TipoDia)tipoDia.Contenido);
                    respuesta.Add(datoDeduccion);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipoDia.Contenido);
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

