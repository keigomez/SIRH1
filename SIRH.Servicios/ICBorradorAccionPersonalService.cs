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
    // NOTE: If you change the interface name "ICBorradorAccionPersonalService" here, you must also update the reference to "ICBorradorAccionPersonalService" in App.config.
    [ServiceContract]
    public interface ICBorradorAccionPersonalService
    {
        [OperationContract]
        CBaseDTO GuardarSolicitud(CBorradorAccionPersonalDTO registro, string codUsuarioEnvia, string codUsuarioRecibe);


        [OperationContract]
        CBaseDTO GuardarBorrador(CFuncionarioDTO funcionario, CEstadoBorradorDTO estado,
                                        CTipoAccionPersonalDTO tipo, CBorradorAccionPersonalDTO registro,
                                        CDetalleBorradorAccionPersonalDTO detalle, string codUsuarioEnvia, string codUsuarioRecibe);

        [OperationContract]
        CBaseDTO EditarBorrador(CFuncionarioDTO funcionario, CEstadoBorradorDTO estado,
                                CTipoAccionPersonalDTO tipo, CBorradorAccionPersonalDTO registro,
                                CDetalleBorradorAccionPersonalDTO detalle, CMovimientoBorradorAccionPersonalDTO movimiento);


        [OperationContract]
        List<CBaseDTO> ObtenerBorrador(int codigo);


        [OperationContract]
        List<List<CBaseDTO>> BuscarSolicitud(CBorradorAccionPersonalDTO registro);


        [OperationContract]
        List<List<CBaseDTO>> BuscarBorrador(CFuncionarioDTO funcionario, CBorradorAccionPersonalDTO registro,
                                            CDetalleBorradorAccionPersonalDTO detalle,
                                            List<DateTime> fechasRige, List<DateTime> fechasVence);

        [OperationContract]
        CBaseDTO ActualizarEstado(CBorradorAccionPersonalDTO registro, CMovimientoBorradorAccionPersonalDTO movimiento);

        
        [OperationContract]
        List<List<CBaseDTO>> FuncionariosConBorradores();
    }
}