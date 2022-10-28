using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICNombramientoService" here, you must also update the reference to "ICNombramientoService" in App.config.
    [ServiceContract]
    public interface ICNombramientoService
    {
        [OperationContract]
        List<List<CBaseDTO>> BuscarDatosParaNombramiento(string cedula, string codpuesto);
        [OperationContract]
        List<CBaseDTO> DescargarCalificacionesCedula(string cedula);

        [OperationContract]
        CBaseDTO GuardarNombramiento(CNombramientoDTO nombramiento, CPuestoDTO puesto);

        [OperationContract]
        CBaseDTO CrearNombramientoInicial(CNombramientoDTO entidad);

        [OperationContract]
        List<List<CBaseDTO>> BuscarDatosRegistroNombramiento(string codpuesto, string cedula);

        [OperationContract]
        List<CBaseDTO> ListarEstadosNombramiento();

        [OperationContract]
        List<CBaseDTO> DescargarNombramientoActualCedula(string cedula);

        [OperationContract]
        List<List<CBaseDTO>> BuscarHistorialNombramiento(CFuncionarioDTO funcionario,
                                                List<DateTime> fechasEmision, CPuestoDTO puesto);

        [OperationContract]
        List<CBaseDTO> NombramientoPorCodigo(int codigoNombramiento);

        [OperationContract]
        List<List<CBaseDTO>> ListarNombramientosVence(DateTime fecha);
    }
}
