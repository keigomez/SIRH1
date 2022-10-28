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
    // NOTE: If you change the interface name "ICEstadoBorrador Service" here, you must also update the reference to "ICEstadoBorrador Service" in App.config.
    [ServiceContract]
    public interface ICEstadoBorradorService
    {
        [OperationContract]
        List<CBaseDTO> RetornarEstados();

        [OperationContract]
        List<CBaseDTO> ObtenerEstado(int codigo);
    }
}