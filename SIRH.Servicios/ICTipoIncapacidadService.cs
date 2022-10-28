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
    // NOTE: If you change the interface name "ICTipoIncapacidadService" here, you must also update the reference to "ICTipoIncapacidadService" in App.config.
    [ServiceContract]
    public interface ICTipoIncapacidadService
    {
        [OperationContract]
        List<CBaseDTO> RetornarTiposIncapacidad();

        [OperationContract]
        List<CBaseDTO> ObtenerTipoIncapacidad(int codigo);

        [OperationContract]
        CBaseDTO AgregarTipoIncapacidad(CTipoIncapacidadDTO tipo);

        [OperationContract]
        CBaseDTO EditarTipoIncapacidad(CTipoIncapacidadDTO tipo);
    }
}
