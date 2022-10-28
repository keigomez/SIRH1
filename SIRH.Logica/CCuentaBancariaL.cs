using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCuentaBancariaL
    {
        #region Varialbes

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCuentaBancariaL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Metodos

        internal static CCuentaBancariaDTO ConvertirDatosCuentaBancariaADTO(CuentaBancaria ctaBancaria)
        {
            return new CCuentaBancariaDTO
            {
                CtaCliente = ctaBancaria.CtaCliente,
                IndEstCuentaBancaria = Convert.ToInt32(ctaBancaria.IndEstadoCtaBancaria)
            };
        }     
        
        //....REVISAR CON DEIVERT.....
        //INSERTE ESTE METODO EN ICFuncionarioService y CFuncionarioService el 25/01/2017....
        public CBaseDTO GuardarCuentaBancariaFuncionario(CCuentaBancariaDTO cuenta)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CCuentaBancariaD intermedio = new CCuentaBancariaD(contexto);
                CEntidadFinancieraD intermedioEntidad = new CEntidadFinancieraD(contexto);
                CDetalleContratacionD intermedioMotivo = new CDetalleContratacionD(contexto);               
                
                CuentaBancaria datos = new CuentaBancaria
                {
                    CtaCliente = cuenta.CtaCliente,                   
                    EntidadFinanciera = intermedioEntidad.CargarEntidadFinancieraPorID(cuenta.EntidadFinanciera.CodEntidad),
                    DetalleContratacion = intermedioMotivo.CargarDetalleContratacionPorID(cuenta.DetalleContratacion.IdEntidad),                                       
                };

                intermedio.GuardarCuentaBancaria(datos);                

                return respuesta;
            }
            catch (Exception error)
            {
                //en respuesta del catch, se digita el error 
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        #endregion
    }
}
