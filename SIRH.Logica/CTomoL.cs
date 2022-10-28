using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CTomoL
    {
        #region Variables
        SIRHEntities contexto;
        #endregion

        #region Construtor
        public CTomoL() {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Métodos

        public CBaseDTO AgregarTomo(Tomo tomo) {

            CBaseDTO respuesta = new CBaseDTO();

            CTomoD intermedio = new CTomoD(contexto);
                var temp = intermedio.AgregarTomo(tomo);
            if (temp.Codigo > 0)
            {
                respuesta.IdEntidad = Convert.ToInt32(temp.Contenido);
            }
            else {
                CErrorDTO error = (CErrorDTO)temp.Contenido;
                respuesta.Mensaje = error.MensajeError;
            }
            return respuesta;
        }

        public int ObtenerTomosPorExpediente(Funcionario func, ExpedienteFuncionario expediente) {
            CTomoD intermedio = new CTomoD(contexto);
            int resultado;
            try
            {
                resultado = intermedio.ObtenerTomosPorExpediente(func, expediente);
            }
            catch (Exception error) {
                resultado = -1;
            }
            return resultado;
        }
        #endregion
    }
}
