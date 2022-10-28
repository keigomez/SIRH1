using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICRegistroTEService" here, you must also update the reference to "ICRegistroTEService" in App.config.
    [ServiceContract]
    public interface ICRegistroTEService
    {
        [OperationContract]
        CRelPresupuestoExtraDTO CargarRelPresupuestoExtraPorID(string IdPresupuesto);

        [OperationContract]
        CRelPresupuestoExtraDTO RetornarRelPresupuestoExtra(string Cedula);

        [OperationContract]
        List<CRelPresupuestoExtraDTO> RetornarRelPresupuestoExtras();

        [OperationContract]
        CRespuestaDTO RegistrarTiempoExtra(CFuncionarioDTO funcionario, CRegistroTiempoExtraDTO registro,
                                                List<CDetalleTiempoExtraDTO> extras);

        [OperationContract]
        CRespuestaDTO GuardarRegistroTiempoExtra(CFuncionarioDTO funcionario, CRegistroTiempoExtraDTO registro,
                                            List<CDetalleTiempoExtraDTO> detalle);
        [OperationContract]
        List<CBaseDTO> ObtenerRegistroExtrasEncabezado(string cedula, string periodo);

        [OperationContract]
        List<List<CBaseDTO>> ObtenerRegistroExtrasDetalle(string cedula, string periodo);
    }
}
