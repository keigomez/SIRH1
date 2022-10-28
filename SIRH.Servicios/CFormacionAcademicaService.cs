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
    // NOTE: If you change the class name "CFormacionAcademicaService" here, you must also update the reference to "CFormacionAcademicaService" in App.config.
    public class CFormacionAcademicaService : ICFormacionAcademicaService
    {

        public List<CBaseDTO> BuscarCursoCapacitacionPorCodigo(CCursoCapacitacionDTO curso)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.BuscarCursoCapacitacionPorCodigo(curso);
        }

        public List<CBaseDTO> BuscarCursoGradoPorCodigo(CCursoGradoDTO curso)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.BuscarCursoGradoPorCodigo(curso);
        }

        public List<CBaseDTO> BuscarDatosCarreraCedula(string cedula)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.BuscarDatosCarreraCedula(cedula);
        }

        public List<List<CBaseDTO>> BuscarDatosCursos(CFuncionarioDTO funcionario, CDetalleContratacionDTO contratacion,
                                                      CBaseDTO curso, List<DateTime> fechas)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.BuscarDatosCursos(funcionario, contratacion, curso, fechas);
        }

        public List<CBaseDTO> GuardarFormacionAcademica(CBaseDTO curso, CFuncionarioDTO funcionario) 
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.GuardarFormacionAcademica(curso, funcionario);
        }

        public List<CBaseDTO> BuscarExperienciaProfesionalCedula(string cedula)
        {
            CExperienciaProfesionalL respuesta = new CExperienciaProfesionalL();
            return respuesta.BuscarExperienciaProfesionalCedula(cedula);        
        }

        public List<CBaseDTO> GuardarExperienciaProfesional(CExperienciaProfesionalDTO experiencia, CFuncionarioDTO funcionario)
        {
            CExperienciaProfesionalL respuesta = new CExperienciaProfesionalL();
            return respuesta.GuardarExperienciaProfesional(experiencia, funcionario);
        }

        public CBaseDTO BuscarEntidadEducativa(int codigo)
        {
            CEntidadEducativaL respuesta = new CEntidadEducativaL();
            return respuesta.BuscarEntidadEducativa(codigo);
        }

        public List<CBaseDTO> ListarEntidadEducativa()
        {
            CEntidadEducativaL respuesta = new CEntidadEducativaL();
            return respuesta.ListarEntidadEducativa();
        }

        public CBaseDTO BuscarModalidad(int codigo)
        {
            CModalidadL respuesta = new CModalidadL();
            return respuesta.BuscarModalidad(codigo);
        }

        public List<CBaseDTO> ListarModalidad()
        {
            CModalidadL respuesta = new CModalidadL();
            return respuesta.ListarModalidad();
        }

        public CBaseDTO AnularCurso(CBaseDTO curso)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.AnularCurso(curso);
        }

        public CBaseDTO EditarCurso(CBaseDTO curso)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.EditarCurso(curso);
        }

        public List<CBaseDTO> CargarDatosCarreraProfesional()
        {
            CCarreraProfesionalL respuesta = new CCarreraProfesionalL();
            return respuesta.CargarCarreraProfesional();
        }

        public List<CBaseDTO> BuscarDatosCarreraProfesional(CCarreraProfesionalDTO carrera, List<DateTime> fechas)
        {
            CCarreraProfesionalL respuesta = new CCarreraProfesionalL();
            return respuesta.BuscarCarreraProfesional(carrera, fechas);
        }

        public CBaseDTO CargarDatosCarreraProfesionalPorID(int id)
        {
            CCarreraProfesionalL respuesta = new CCarreraProfesionalL();
            return respuesta.CargarCarreraProfesionalPorID(id);
        }


        public List<CBaseDTO> CargarCursos()
        {
            CCursoL respuesta = new CCursoL();
            return respuesta.CargarCursos();
        }

        public List<CBaseDTO> BuscarCursos(CCursoDTO curso, List<DateTime> fechas)
        {
            CCursoL respuesta = new CCursoL();
            return respuesta.BuscarCursos(curso, fechas);
        }

        public CBaseDTO CargarCursoPorID(int id)
        {
            CCursoL respuesta = new CCursoL();
            return respuesta.CargarCursoPorID(id);
        }

        public List<CBaseDTO> CargarDatosPuntos()
        {
            CPuntosL respuesta = new CPuntosL();
            return respuesta.CargarDatosPuntos();
        }

        public List<CBaseDTO> BuscarDatosPuntos(CPuntosDTO punto)
        {
            CPuntosL respuesta = new CPuntosL();
            return respuesta.BuscarDatosPuntos(punto);
        }

        public CBaseDTO CargarDatosPuntosPorID(int id)
        {
            CPuntosL respuesta = new CPuntosL();
            return respuesta.CargarDatosPuntosPorId(id);
        }

        public List<CBaseDTO> BuscarDatosEntidadEducativa(string nombre, int tipo)
        {
            CEntidadEducativaL respuesta = new CEntidadEducativaL();
            return respuesta.BuscarEntidadEducativa(nombre, tipo);
        }

        public CBaseDTO GuardarEntidadEducativa(string nombre, int tipo)
        {
            CEntidadEducativaL respuesta = new CEntidadEducativaL();
            return respuesta.GuardarEntidadEducativa(nombre, tipo);
        }

        public CBaseDTO AnularEntidadEducativa(int id)
        {
            CEntidadEducativaL respuesta = new CEntidadEducativaL();
            return respuesta.AnularEntidadEducativa(id);
        }

        public CBaseDTO AgregarPuntosAdicionales(string cedula, int puntos, string observaciones, string numDoc)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.AgregarPuntosAdicionales(cedula, puntos, observaciones, numDoc);
        }


        //Retorna el PeriodoEscalaSalarial con el valor del punto
        public CBaseDTO ObtenerValorPunto()
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.ObtenerValorPunto();
        }


        public CBaseDTO CalcularPuntosFuncionario(string cedula)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.CalcularPuntosFuncionario(cedula);
        }

        public List<CBaseDTO> AsignarResolucion(List<CCursoCapacitacionDTO> cursos, string numResolucion)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.AsignarResolucion(cursos, numResolucion);
        }
    }
}
