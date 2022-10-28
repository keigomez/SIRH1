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
    // NOTE: If you change the interface name "ICPlanificacionService" here, you must also update the reference to "ICTipoIncapacidadService" in App.config.
    [ServiceContract]
    public interface ICPlanificacionService
    {
        [OperationContract]
        List<List<CBaseDTO>> ListarNombramientosActivos(CListaNombramientosActivosDTO datoBusqueda);

        [OperationContract]
        List<CBaseDTO> ListarNombramientosFuncionario(string cedula);

        [OperationContract]
        List<CBaseDTO> ListarPagosFuncionario(string cedula);

        [OperationContract]
        List<CBaseDTO> CrearSolicitudVacia();

        [OperationContract]
        List<CBaseDTO> ListarDivisiones();

        [OperationContract]
        List<CBaseDTO> ListarDireccionGeneral();

        [OperationContract]
        List<CBaseDTO> ListarDepartamentos();

        [OperationContract]
        List<CBaseDTO> ListarSecciones();

        [OperationContract]
        List<CBaseDTO> ListarClases();

        [OperationContract]
        List<CBaseDTO> ListarEstadoCivil();

        [OperationContract]
        List<CBaseDTO> ListarEstadoFuncionario();

        [OperationContract]
        List<string[]> GenerarDatosReportes(List<CParametrosDTO> parametros);
    }
}