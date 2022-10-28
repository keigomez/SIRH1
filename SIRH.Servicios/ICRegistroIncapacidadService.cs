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
    // NOTE: If you change the interface name "ICRegistroIncapacidadService" here, you must also update the reference to "ICRegistroIncapacidadService" in App.config.
    [ServiceContract]
    public interface ICRegistroIncapacidadService
    {
        [OperationContract]
        CBaseDTO GuardarRegistroIncapacidad(CFuncionarioDTO funcionario, CTipoIncapacidadDTO tipo, CRegistroIncapacidadDTO registro);

        [OperationContract]
        CBaseDTO VerificarFechasIncapacidad(CFuncionarioDTO funcionario, CRegistroIncapacidadDTO registro);

        [OperationContract]
        CBaseDTO EditarRegistroIncapacidad(CRegistroIncapacidadDTO registro);

        [OperationContract]
        CBaseDTO AprobarRegistroIncapacidad(CRegistroIncapacidadDTO registro);

        [OperationContract]
        CBaseDTO AnularRegistroIncapacidad(CRegistroIncapacidadDTO registro);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroIncapacidad(int codigo);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroIncapacidadCodigo(string numIncapacidad);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroIncapacidadProrroga(string numCedula, int idEntidad, int idTipo, DateTime fecha);

        [OperationContract]
        List<List<CBaseDTO>> BuscarRegistroIncapacidades(CFuncionarioDTO funcionario,
                                                        CRegistroIncapacidadDTO registro,
                                                        CEntidadMedicaDTO entidadMedica,
                                                        CBitacoraUsuarioDTO bitacora,
                                                        List<DateTime> fechasRige,
                                                        List<DateTime> fechasVence,
                                                        List<DateTime> fechasBitacora);
        [OperationContract]
        List<CBaseDTO> ListarTipoIncapacidad();

        [OperationContract]
        List<CBaseDTO> ObtenerTipoIncapacidad(int codigo);
        
        [OperationContract]
        List<CBaseDTO> ListarEntidadMedica();

        [OperationContract]
        List<List<CBaseDTO>> FuncionariosConIncapacidades();


        [OperationContract]
        CBaseDTO ObtenerSalarioBruto(string cedula);
    }
}
