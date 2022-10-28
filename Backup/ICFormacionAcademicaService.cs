using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICFormacionAcademicaService" here, you must also update the reference to "ICFormacionAcademicaService" in App.config.
    [ServiceContract]
    public interface ICFormacionAcademicaService
    {
        [OperationContract]
        List<CBaseDTO> BuscarCursoCapacitacionPorCodigo(CCursoCapacitacionDTO curso);

        [OperationContract]
        List<CBaseDTO> BuscarCursoGradoPorCodigo(CCursoGradoDTO curso);

        [OperationContract]
        List<CBaseDTO> BuscarDatosCarreraCedula(string cedula);

        [OperationContract]
        List<List<CBaseDTO>> BuscarDatosCursos(List<string> elementos, List<object> parametros);

        [OperationContract]
        List<CBaseDTO> GuardarFormacionAcademica(CBaseDTO curso, CFuncionarioDTO funcionario);

        [OperationContract]
        List<CBaseDTO> BuscarExperienciaProfesionalCedula(string cedula);

        [OperationContract]
        List<CBaseDTO> GuardarExperienciaProfesional(CExperienciaProfesionalDTO experiencia, CFuncionarioDTO funcionario);

        [OperationContract]
        CBaseDTO BuscarEntidadEducativa(int codigo);

        [OperationContract]
        List<CBaseDTO> ListarEntidadEducativa();

        [OperationContract]
        CBaseDTO BuscarModalidad(int codigo);

        [OperationContract]
        List<CBaseDTO> ListarModalidad();
    
    }
}
