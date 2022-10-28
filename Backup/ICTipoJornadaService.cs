using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICTipoJornadaService" here, you must also update the reference to "ICTipoJornadaService" in App.config.
    [ServiceContract]
    public interface ICTipoJornadaService
    {
        [OperationContract]
        List<CBaseDTO> RegistrarJornadaFuncionario(CFuncionarioDTO funcionario,
                                                            CNombramientoDTO nombramiento,
                                                            CTipoJornadaDTO jornada);
        
        [OperationContract]
        List<CBaseDTO> EditarJornadaFuncionario(CTipoJornadaDTO jornada);
    }
}
