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
        List<List<CBaseDTO>> BuscarDatosCursos(CFuncionarioDTO funcionario, CDetalleContratacionDTO contratacion,
                                                      CBaseDTO curso, List<DateTime> fechas);

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

        [OperationContract]
        CBaseDTO AnularCurso(CBaseDTO curso);

        [OperationContract]
        CBaseDTO EditarCurso(CBaseDTO curso);

        [OperationContract]
        List<CBaseDTO> CargarDatosCarreraProfesional();

        [OperationContract]
        List<CBaseDTO> BuscarDatosCarreraProfesional(CCarreraProfesionalDTO carrera, List<DateTime> fechas);

        [OperationContract]
        CBaseDTO CargarDatosCarreraProfesionalPorID(int id);


        [OperationContract]
        List<CBaseDTO> CargarCursos();

        [OperationContract]
        List<CBaseDTO> BuscarCursos(CCursoDTO curso, List<DateTime> fechas);

        [OperationContract]
        CBaseDTO CargarCursoPorID(int id);

        [OperationContract]
        List<CBaseDTO> CargarDatosPuntos();

        [OperationContract]
        List<CBaseDTO> BuscarDatosPuntos(CPuntosDTO punto);

        [OperationContract]
        CBaseDTO CargarDatosPuntosPorID(int id);

        [OperationContract]
        List<CBaseDTO> BuscarDatosEntidadEducativa(string nombre, int tipo);

        [OperationContract]
        CBaseDTO GuardarEntidadEducativa(string nombre, int tipo);

        [OperationContract]
        CBaseDTO AnularEntidadEducativa(int id);

        [OperationContract]
        CBaseDTO AgregarPuntosAdicionales(string cedula, int puntos, string observaciones, string numDoc);

        //Retorna el PeriodoEscalaSalarial con el valor del punto
        [OperationContract]
        CBaseDTO ObtenerValorPunto();


        [OperationContract]
        CBaseDTO CalcularPuntosFuncionario(string cedula);

        [OperationContract]
        List<CBaseDTO> AsignarResolucion(List<CCursoCapacitacionDTO> cursos, string numResolucion);

    }
}
