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
    // NOTE: If you change the interface name "ICEntidadMedicaService" here, you must also update the reference to "ICEntidadMedicaService" in App.config.
    [ServiceContract]
    public interface ICEntidadMedicaService
    {
        [OperationContract]
        List<CBaseDTO> RetornarEntidadesMedicas();

        [OperationContract]
        List<CBaseDTO> ObtenerEntidadMedica(int codigo);

        [OperationContract]
        CBaseDTO GuardarEntidadMedica(CEntidadMedicaDTO item);

        [OperationContract]
        CBaseDTO EditarEntidadMedica(CEntidadMedicaDTO item);
    }
}
