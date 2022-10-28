using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCuentaBancariaD
   {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CCuentaBancariaD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        public CuentaBancaria RetornarCuentaBancariaEspecifico(string Cedula)
        {
            return contexto.CuentaBancaria.Where(Q => Q.DetalleContratacion.Funcionario.IdCedulaFuncionario == Cedula).FirstOrDefault();
        }
        
        public int GuardarCuentaBancariaFuncionario(CuentaBancaria CuentaBancaria)
        {
            contexto.CuentaBancaria.Add(CuentaBancaria);
            return CuentaBancaria.PK_CuentaBancaria;
        }

        public CRespuestaDTO GuardarCuentaBancaria(CuentaBancaria cuentaBancaria)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.CuentaBancaria.Add(cuentaBancaria);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = cuentaBancaria
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

         //<summary>
         //Obtiene la carga de cuentas bancarias de la BD
         //</summary>
         //<returns>Retorna las cuentas bancarias</returns>    
         public CuentaBancaria CargarCuentaBancariaPorID(String Cedula)
        {
            CuentaBancaria resultado = new CuentaBancaria();

            resultado = contexto.CuentaBancaria.Where(R => R.DetalleContratacion.Funcionario.IdCedulaFuncionario == Cedula).FirstOrDefault();

            return resultado;
        }
        
        #endregion
    }
}
