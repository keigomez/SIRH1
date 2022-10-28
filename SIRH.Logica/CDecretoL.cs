using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Logica
{
    class CDecretoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDecretoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        internal static CDecretoDTO ConvertirDecretoADTO(Decreto item)
        {
            return new CDecretoDTO
            {
                IdEntidad = item.PK_Decreto,
                NumeroDecreto = item.NumDecreto,
                TituloDecreto = item.TituloDecreto,
                FechaDecreto = Convert.ToDateTime(item.FecDecreto),
                ObservacionDecreto = item.ObsDecreto

            };

        }

        internal static Decreto ConvertirDTODecretoADatos(CDecretoDTO item)
        {
            return new Decreto
            {
                NumDecreto = item.NumeroDecreto,
                TituloDecreto = item.TituloDecreto,
                FecDecreto = item.FechaDecreto,
                ObsDecreto = item.ObservacionDecreto
    
            };
        }


        public CBaseDTO AgregarDecreto(CDecretoDTO decreto)
        {
            CBaseDTO respuesta;

            try
            {
                CDecretoD intermedio = new CDecretoD(contexto);

                Decreto decretoBD = ConvertirDTODecretoADatos(decreto);

                
                var resultado = intermedio.AgregarDecreto(decretoBD);

                if (resultado.Codigo > 0)
                {
                    respuesta = new CDecretoDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }


        #endregion
    }
}
