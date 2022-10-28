using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICPlanillaService" in both code and config file together.
    [ServiceContract]
    public interface ICPlanillaService
    {
        [OperationContract]
        List<CBaseDTO> DescargarMeses();

        [OperationContract]
        List<CBaseDTO> BuscarDatosPlanilla(string cedula, DateTime fechaInicio, DateTime fechaFinal);

        [OperationContract]
        CBaseDTO ObtenerPagoID(int idPago);

        #region DeduccionTemporal
        [OperationContract]
        CBaseDTO ObtenerTipoDeduccion(int codigo);

        [OperationContract]
        List<CBaseDTO> ListarTiposDeduccion();

        [OperationContract]
        CBaseDTO AgregarDeduccionTemporal(CFuncionarioDTO funcionario, CDeduccionTemporalDTO deduccion);

        [OperationContract]
        CBaseDTO AnularDeduccionTemporal(CDeduccionTemporalDTO deduccion);

        [OperationContract]
        List<CBaseDTO> DescargarDetalleDeduccion(CDeduccionTemporalDTO deduccion);

        [OperationContract]
        List<List<CBaseDTO>> BuscarDeducciones(CFuncionarioDTO funcionario, CDeduccionTemporalDTO deduccion,
                                                CBitacoraUsuarioDTO bitacora, List<DateTime> fechas, List<DateTime> fechasBitacora);

        [OperationContract]
        CBaseDTO AprobarDeduccionTemporal(CDeduccionTemporalDTO deduccion);

        [OperationContract]
        CBaseDTO ModificarDeduccionExplicacion(CDeduccionTemporalDTO deduccion);

        #endregion
    }
}