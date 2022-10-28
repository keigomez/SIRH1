using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using SIRH.DTO;

namespace SIRH.Servicios
{
    [ServiceContract]
    public interface ICMarcacionService
    {

        [OperationContract]
        List<List<CBaseDTO>> ReporteConsolidadoPorFuncionario(CFuncionarioDTO funcionario, List<DateTime> fechas);

        [OperationContract]
        List<List<CBaseDTO>> ReporteConsolidadoPorDepartamento(CDepartamentoDTO departamento, List<DateTime> fechas);

        [OperationContract]
        List<List<CBaseDTO>> ReporteMarcacionesPorDia(List<DateTime> fechas, CFuncionarioDTO funcionario);

        [OperationContract]
        List<List<CBaseDTO>> ReporteFuncionariosPorDepartamento(CDepartamentoDTO departamento);

        [OperationContract]
        List<CBaseDTO> ListarDepartamentos();

        [OperationContract]
        List<CBaseDTO> ObtenerJornadaFuncionario(CFuncionarioDTO funcionario);

        //Dispositivos-----------------------------------------------------
        [OperationContract]
        CBaseDTO BuscarDispositivo(CDispositivoDTO dispositivo);

        [OperationContract]
        List<CBaseDTO> ListarDispositivos();

        //Empleado----------------------------------------------------------
        [OperationContract]
        CBaseDTO AgregarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario);

        [OperationContract]
        CBaseDTO DesactivarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario,CDetalleNombramientoDTO detalleNombramiento, CMotivoBajaDTO motivoBaja);

        [OperationContract]
        List<CBaseDTO> BuscarEmpleadoActivo(CEmpleadoDTO empleado);

        [OperationContract]
        List<CBaseDTO> BuscarEmpleado(CEmpleadoDTO empleado);

        [OperationContract]
        CBaseDTO ActivarEmpleado(CEmpleadoDTO empleado, CFuncionarioDTO funcionario, CDetalleNombramientoDTO detalleNombramiento);

        [OperationContract]
        List<CBaseDTO> SearchEmpleado(CEmpleadoDTO empleado, List<string> diasFeriados);

        [OperationContract]
        CBaseDTO ObtenerDetalleNombramientoFuncionario(CFuncionarioDTO funcionario);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula);

        //EmpleadoDispositivo----------------------------------------------------------

        [OperationContract]
        CBaseDTO AsignarRelojMarcador(CEmpleadoDispositivoDTO empleadoDispositivo, CEmpleadoDTO empleado, CDispositivoDTO dispositivo);

        [OperationContract]
        CBaseDTO DesAsignarRelojMarcador(CEmpleadoDispositivoDTO empleadoDispositivo, CEmpleadoDTO empleado, CDispositivoDTO dispositivo);

        [OperationContract]
        List<CBaseDTO> BuscarDispositivosAsignados(CEmpleadoDTO empleado);

        
        //Motivo Baja-------------------------------------------------------------------

        [OperationContract]
        CBaseDTO BuscarMotivoBaja(CMotivoBajaDTO motivoBaja);

        [OperationContract]
        List<CBaseDTO> ListarMotivoBaja();

    }
}
