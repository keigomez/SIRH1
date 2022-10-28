using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICCaucionesService" here, you must also update the reference to "ICCaucionesService" in App.config.
    [ServiceContract]
    public interface ICCaucionesService
    {
        [OperationContract]
        CBaseDTO AgregarCaucion(CFuncionarioDTO funcionario, CCaucionDTO caucion,
                                            CEntidadSegurosDTO aseguradora, CMontoCaucionDTO montoCaucion);

        [OperationContract]
        List<CBaseDTO> ListarMontosCaucion();

        [OperationContract]
        List<CBaseDTO> ListarEntidadSeguros();

        [OperationContract]
        List<CBaseDTO> ObtenerCaucion(int codigo);

        [OperationContract]
        List<List<CBaseDTO>> BuscarCauciones(CFuncionarioDTO funcionario, CCaucionDTO caucion,
                                                        List<DateTime> fechasEmision,
                                                        List<DateTime> fechasVencimiento,
                                                        CPuestoDTO puesto,
                                                        CMontoCaucionDTO nivel);

        [OperationContract]
        CBaseDTO AnularCaucion(CCaucionDTO caucion);

        [OperationContract]
        List<CBaseDTO> BuscarMontosCaucion(CMontoCaucionDTO caucion, List<DateTime> fechas,
                                                    List<decimal> montos);

        [OperationContract]
        CBaseDTO ObtenerMontoCaucion(int codigo);

        [OperationContract]
        CBaseDTO EditarMontoCaucion(CMontoCaucionDTO monto);

        [OperationContract]
        CBaseDTO AgregarMontoCaucion(CMontoCaucionDTO montoCaucion);

        [OperationContract]
        List<List<CBaseDTO>> ActualizarVencimientoPolizas(DateTime fechaVencimiento);

        [OperationContract]
        List<List<CBaseDTO>> PolizasPorVencer(DateTime fechaVencimiento);

        [OperationContract]
        CBaseDTO ActualizarObservacionesPuestoCaucion(string codpuesto, string observaciones);
    }
}
