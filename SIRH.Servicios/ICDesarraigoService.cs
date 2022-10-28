using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICDesarraigoService" here, you must also update the reference to "ICDesarraigoService" in App.config.
    [ServiceContract]
    public interface ICDesarraigoService
    {

        [OperationContract]
        List<List<CBaseDTO>> ActualizarVencimientoDesarraigo(DateTime fecha);

        [OperationContract]
        CBaseDTO AgregarContratoArrendamiento(CDesarraigoDTO desarraigo, CContratoArrendamientoDTO contrato);

        [OperationContract]
        CBaseDTO AgregarDesarraigo(CFuncionarioDTO funcionario, CDesarraigoDTO desarraigo,
                                   List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contratos);
        [OperationContract]
        CBaseDTO AgregarFactura(CDesarraigoDTO desarraigo, CFacturaDesarraigoDTO factura);

        [OperationContract]
        CBaseDTO AnularDesarraigo(CDesarraigoDTO desarraigo);

        [OperationContract]
        List<List<CBaseDTO>> BuscarDesarraigo(CFuncionarioDTO funcionario, CDesarraigoDTO desarraigo, List<DateTime> fechas, CDistritoDTO distrito);

        [OperationContract]
        List<CBaseDTO> CargarCantones();

        [OperationContract]
        List<CBaseDTO> CargarDistritos();

        [OperationContract]
        List<CBaseDTO> CargarProvincias();

        [OperationContract]
        List<List<CBaseDTO>> DesarraigosPorVencer(DateTime fecha);

        [OperationContract]
        List<List<CBaseDTO>> GetLocalizacion(bool cantones, bool distritos, bool provincias);

        [OperationContract]
        List<List<CBaseDTO>> ListarDesarraigo();

        [OperationContract]
        List<CBaseDTO> ListarEstadosDesarraigo();

        [OperationContract]
        CBaseDTO ModificarDesarraigo(CDesarraigoDTO desarraigo, List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contrato);

        [OperationContract]
        CBaseDTO ModificarEstadoDesarraigo(CDesarraigoDTO desarraigo);

        [OperationContract]
        List<CBaseDTO> ObtenerContratosArrendamientos(CDesarraigoDTO desarraigo);

        [OperationContract]
        List<List<CBaseDTO>> ObtenerDesarraigo(int codigo);

        [OperationContract]
        List<CBaseDTO> ObtenerFacturasDesarraigo(CDesarraigoDTO desarraigo);

        [OperationContract]
        List<CBaseDTO> ObtenerCorreosElectronicos(string codigo);

        [OperationContract]
        CBaseDTO ObtenerMontoRetroactivo(string cedula, List<DateTime> fecha);

        [OperationContract]
        CBaseDTO ObtenerSalarioBase(int idCategoriaEscala);

        [OperationContract]
        CBaseDTO RetomarDesarraigo(CDesarraigoDTO desarraigo);


        ////  Detalle Eliminación Desarraigo
        [OperationContract]
        CBaseDTO AgregarDetalleDesarraigoEliminacion(CDetalleDesarraigoEliminacionDTO detalle, CDesarraigoDTO desarraigo);
        
        [OperationContract]
        CBaseDTO ModificarDetalleDesarraigoEliminacion(CDetalleDesarraigoEliminacionDTO detalle);

        [OperationContract]
        List<List<CBaseDTO>> CerrarDesarraigosVencidos();

        [OperationContract]
        List<List<CBaseDTO>> ListarDesarraigosIncapacidades(DateTime fechaInicio, DateTime fechaFinal);
    }
}
