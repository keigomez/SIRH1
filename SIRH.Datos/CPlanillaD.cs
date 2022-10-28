using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Datos
{
    public class CPlanillaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase Planilla
        /// </summary>
        /// <param name = "entidadGlobal" ></ param >
        public CPlanillaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        public CRespuestaDTO GenerarPlanilla(DateTime quincena)
        {
            CRespuestaDTO respuesta = null;
            try
            {
                entidadBase.USP_GENERAR_PLANILLA(quincena);

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = quincena
                };

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message
                    }
                };
            }
            return respuesta;

           

            


        }
        public List<DesgloseSalarial> ConsultarPagoFuncionario(string Cedula, DateTime FechaI, DateTime FechaF)
        //public List<DesgloseSalarial> ConsultarPagoFuncionario(string Cedula, string FechaI, string FechaF)
        {
            List<DesgloseSalarial> resultado = new List<DesgloseSalarial>();


            resultado = entidadBase.DesgloseSalarial.Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario == Cedula
                                                   && Q.IndPeriodo >= FechaI
                                                   && Q.IndPeriodo <= FechaF).ToList();
            return resultado;
        }

        public List<C_EMU_HistoricoPlanilla> ConsultarPagoFuncionarioHistorico(string Cedula, DateTime FechaI, DateTime FechaF)
        //public List<C_EMU_HistoricoPlanilla> ConsultarPagoFuncionarioHistorico(string Cedula, string FechaI, string FechaF) 
        {
            List<C_EMU_HistoricoPlanilla> resultado = new List<C_EMU_HistoricoPlanilla>();

            List<DateTime> fechas = new List<DateTime>();
            //List<string> fechas = new List<string>();
            bool condicionFecha = false;
            string fmt = "00";

            fechas.Add(FechaI);
            fechas.Add(FechaF);

            //string FInicio = Convert.ToString(FechaI).Substring(6, 4) + Convert.ToString(FechaI).Substring(3, 2) + Convert.ToString(FechaI).Substring(0, 2);
            //string FFin = Convert.ToString(FechaF).Substring(6, 4) + Convert.ToString(FechaF).Substring(3, 2) + Convert.ToString(FechaF).Substring(0, 2);

            string FInicio = FechaI.Year.ToString() + FechaI.Month.ToString(fmt) + FechaI.Day.ToString(fmt);
            string FFin = FechaF.Year.ToString() + FechaF.Month.ToString(fmt) + FechaF.Day.ToString(fmt);

            //string FInicio = FechaI.Substring(6, 4) + FechaI.Substring(3, 2) + FechaI.Substring(0, 2);
            //string FFin = FechaF.Substring(6, 4) + FechaF.Substring(3, 2) + FechaF.Substring(0, 2);


            if (fechas.Count() > 0)
            {
                condicionFecha = true;
            }
            long x;
            resultado = entidadBase.C_EMU_HistoricoPlanilla.Where(Q =>
                                                   Q.cedula == Cedula).ToList();

            if (condicionFecha)

                resultado = resultado.Where(Q => Convert.ToInt64(Q.fecha_periodo) >= Convert.ToInt64(FInicio) && Convert.ToInt64(Q.fecha_periodo) <= Convert.ToInt64(FFin)).ToList();

            
            return resultado;
        }

        #endregion
    }
}
