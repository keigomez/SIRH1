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

        public List<List<CBaseDTO>> BuscarDatosCursos(List<string> elementos, List<object> parametros)
        {
            CFormacionAcademicaL respuesta = new CFormacionAcademicaL();
            return respuesta.BuscarDatosCursos(elementos, parametros);
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

    }
}
