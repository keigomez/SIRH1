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
    [ServiceContract]
    public interface ICTipoAccionPersonalService
    {
        [OperationContract]
        List<CBaseDTO> RetornarTipos();

        [OperationContract]
        List<CBaseDTO> ObtenerTipo(int codigo);

        [OperationContract]
        CBaseDTO AgregarTipo(CTipoAccionPersonalDTO tipo);

        [OperationContract]
        CBaseDTO EditarTipo(CTipoAccionPersonalDTO tipo);
    }
}
