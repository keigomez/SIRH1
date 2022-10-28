using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CEstadoBorradorL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEstadoBorradorL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CEstadoBorradorDTO ConvertirEstadoBorradorADto(EstadoBorrador item)
        {
            return new CEstadoBorradorDTO
            {
                IdEntidad = item.PK_EstadoBorrador,
                DesEstadoBorrador = item.DesEstadoBorrador
            };
        }

        public List<CBaseDTO> RetornarEstadosBorrador()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CEstadoBorradorD intermedio = new CEstadoBorradorD(contexto);

                var estadoBorrador = intermedio.RetornarEstadosBorrador();

                if (estadoBorrador.Codigo > 0)
                {
                    foreach (var estado in (List<EstadoBorrador>)estadoBorrador.Contenido)
                    {
                        var datoEstado = ConvertirEstadoBorradorADto(estado);
                        resultado.Add(datoEstado);
                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)estadoBorrador.Contenido);
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
        /// Obtiene un Estado de Borrador registrado en la BD
        /// </summary>
        /// <returns>Retorna el Estado de Borrador</returns>
        public List<CBaseDTO> ObtenerEstadoBorrador(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CEstadoBorradorD intermedio = new CEstadoBorradorD(contexto);

                var tipoInc = intermedio.CargarEstadoBorradorPorID(codigo);

                if (tipoInc.Codigo > 0)
                {
                    var dato = ConvertirEstadoBorradorADto((EstadoBorrador)tipoInc.Contenido);
                    respuesta.Add(dato);
                }
                else
                {
                    respuesta.Add((CErrorDTO)tipoInc.Contenido);
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