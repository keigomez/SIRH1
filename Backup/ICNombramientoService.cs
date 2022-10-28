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
        List<CBaseDTO> DescargarCalificacionesCedula(string cedula);

        [OperationContract]
        CBaseDTO GuardarNombramiento(CNombramientoDTO nombramiento);

        [OperationContract]
        CBaseDTO CrearNombramientoInicial(CNombramientoDTO entidad);
    }
}
