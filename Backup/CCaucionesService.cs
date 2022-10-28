using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;
using SIRH.Logica;

namespace SIRH.Servicios
{
    // NOTE: If you change the class name "CCaucionesService" here, you must also update the reference to "CCaucionesService" in App.config.
    public class CCaucionesService : ICCaucionesService
    {
        public CBaseDTO AgregarCaucion(CFuncionarioDTO funcionario, CCaucionDTO caucion, CEntidadSegurosDTO aseguradora,
                                     CMontoCaucionDTO montoCaucion)
        {
            CCaucionL respuesta = new CCaucionL();

            return respuesta.AgregarCaucion(funcionario, caucion, aseguradora, montoCaucion);
        }

        public List<CBaseDTO> ListarMontosCaucion()
        {
            CMontoCaucionL respuesta = new CMontoCaucionL();

            return respuesta.ListarMontosCaucion();
        }

        public List<CBaseDTO> ListarEntidadSeguros()
        {
            CEntidadSegurosL respuesta = new CEntidadSegurosL();

            return respuesta.ListarEntidadesSeguros();
        }


        public List<CBaseDTO> ObtenerCaucion(int codigo)
        {
            CCaucionL respuesta = new CCaucionL();

            return respuesta.ObtenerCaucion(codigo);
        }

        public List<List<CBaseDTO>> BuscarCauciones(CFuncionarioDTO funcionario, CCaucionDTO caucion,
                                                        List<DateTime> fechasEmision, 
                                                        List<DateTime> fechasVencimiento)
        {
            CCaucionL respuesta = new CCaucionL();

            return respuesta.BuscarCauciones(funcionario, caucion, fechasEmision, fechasVencimiento);
        }

        public CBaseDTO AnularCaucion(CCaucionDTO caucion)
        {
            CCaucionL respuesta = new CCaucionL();

            return respuesta.AnularCaucion(caucion);
        }

        public List<CBaseDTO> BuscarMontosCaucion(CMontoCaucionDTO caucion, List<DateTime> fechas,
                                                    List<decimal> montos)
        {
            CMontoCaucionL respuesta = new CMontoCaucionL();

            return respuesta.BuscarMontosCaucion(caucion, fechas, montos);
        }

        public CBaseDTO ObtenerMontoCaucion(int codigo)
        {
            CMontoCaucionL respuesta = new CMontoCaucionL();

            return respuesta.ObtenerMontoCaucion(codigo);
        }

        public CBaseDTO EditarMontoCaucion(CMontoCaucionDTO monto)
        {
            CMontoCaucionL respuesta = new CMontoCaucionL();

            return respuesta.EditarMontoCaucion(monto);
        }

        public CBaseDTO AgregarMontoCaucion(CMontoCaucionDTO montoCaucion)
        {
            CMontoCaucionL respuesta = new CMontoCaucionL();

            return respuesta.AgregarMontoCaucion(montoCaucion);
        }

        public List<List<CBaseDTO>> ActualizarVencimientoPolizas(DateTime fechaVencimiento)
        {
            CCaucionL respuesta = new CCaucionL();

            return respuesta.ActualizarVencimientoPolizas(fechaVencimiento);
        }

        public List<List<CBaseDTO>> PolizasPorVencer(DateTime fechaVencimiento)
        {
            CCaucionL respuesta = new CCaucionL();

            return respuesta.PolizasPorVencer(fechaVencimiento);
        }
    }
}
