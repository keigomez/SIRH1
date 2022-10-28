using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Logica
{
    public class CSalarioL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CSalarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        public List<CBaseDTO> ObtenerSalario(string numCedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CFuncionarioL intermedioFuncionario = new CFuncionarioL();

                //var registro = intermedioFuncionario.BuscarFuncionarioDetallePuesto(numCedula);
                //if (registro.FirstOrDefault().GetType() == typeof(CErrorDTO))
                //    registro = new CAccionPersonalL().BuscarFuncionarioDetallePuesto(numCedula);

                var registro = new CAccionPersonalL().BuscarFuncionarioDetallePuesto(numCedula);
                if (registro.FirstOrDefault().GetType() == typeof(CErrorDTO))
                    registro = intermedioFuncionario.BuscarFuncionarioDetallePuesto(numCedula);

                if (registro.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    CSalarioDTO datoSalario = new CSalarioDTO();
                    decimal mtoSalarioBase = 0;
                    decimal mtoAnual = 0;
                    decimal mtoPunto = 0;

                    datoSalario.Puesto = (CPuestoDTO)registro[1];
                    datoSalario.DetallePuesto = (CDetallePuestoDTO)registro[2];

                    mtoSalarioBase = datoSalario.DetallePuesto.EscalaSalarial.SalarioBase;
                    mtoAnual = datoSalario.DetallePuesto.EscalaSalarial.MontoAumentoAnual;
                    mtoPunto = datoSalario.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera;

                    if (datoSalario.DetallePuesto.PorProhibicion > 0)
                        datoSalario.PorProhibicion = datoSalario.DetallePuesto.PorProhibicion;
                    else
                        datoSalario.PorProhibicion = datoSalario.DetallePuesto.PorDedicacion;

                    datoSalario.MtoAumentosAnuales = ((CDetalleContratacionDTO)registro[3]).NumeroAnualidades * mtoAnual;
                    datoSalario.MtoProhibicion = (datoSalario.PorProhibicion * mtoSalarioBase) / 100;
                    var puntos = intermedioFuncionario.BuscarFuncionarioPuntosCarreraProfesional(numCedula);
                    datoSalario.NumPuntos = Convert.ToDecimal(puntos.Contenido);
                    datoSalario.MtoGradoGrupo = datoSalario.NumPuntos * mtoPunto;
                    datoSalario.MtoOtros = datoSalario.DetallePuesto.DetalleRubros.Sum(D => D.MtoValor);

                    datoSalario.MtoTotal = mtoSalarioBase + datoSalario.MtoAumentosAnuales + datoSalario.MtoProhibicion + datoSalario.MtoGradoGrupo + datoSalario.MtoOtros;

                    datoSalario.MtoDia = datoSalario.MtoTotal / 30;
                    datoSalario.MtoHora = datoSalario.MtoDia / 8;

                    // Funcionario 00
                    var funcionario = ((CFuncionarioDTO)registro[0]);
                    respuesta.Add(funcionario);

                    // Salario 01
                    respuesta.Add(datoSalario);
                }
                else
                {
                    throw new Exception(((CErrorDTO)registro.FirstOrDefault()).MensajeError);
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