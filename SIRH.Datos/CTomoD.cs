using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
    public class CTomoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor
        public CTomoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion

        #region Métodos

        public CRespuestaDTO AgregarTomo(Tomo tomo) {

            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Tomo.Add(tomo);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO {
                    Codigo = 1,
                    Contenido = tomo.PK_IdTomo
                };
                return respuesta;
            }
            catch (Exception error) {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public int ObtenerTomosPorExpediente(Funcionario func, ExpedienteFuncionario expediente) {
            int resultado;
            try
            {

                resultado = entidadBase.Tomo.Where(t => t.FK_ExpedienteFuncionario == expediente.PK_IdExpedienteFuncionario).Count();
            }
            catch (Exception error) {
                resultado = -1;
            }
            return resultado;
        }


        public List<CRespuestaDTO> ObtenerListaTomosPorExpediente(ExpedienteFuncionario expediente) {
            List<CRespuestaDTO> respuesta = new List<CRespuestaDTO>();

            try
            {
                var tomos = entidadBase.Tomo.Include("Folio").Where(t => t.FK_ExpedienteFuncionario == expediente.PK_IdExpedienteFuncionario).ToList();
                if (tomos.Count() > 0)
                {
                    foreach (Tomo item in tomos)
                    {
                        respuesta.Add(new CRespuestaDTO { Codigo = 1, Contenido = item });
                    }
                }
                else {
                    respuesta.Add(new CRespuestaDTO { Codigo = -1, Contenido = new CErrorDTO { MensajeError = "No existen tomos asociados a este funcionario de momento."} });
                }
            }
            catch (Exception error) {
                respuesta.Add(new CRespuestaDTO { Codigo = -1, Contenido = new CErrorDTO { MensajeError = error.Message } });
            }

            return respuesta;
        }
        #endregion
    }
}
