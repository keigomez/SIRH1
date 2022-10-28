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
    // NOTE: If you change the class name "CTipoIncapacidadService" here, you must also update the reference to "CTipoIncapacidadService" in App.config.
    public class CPlanificacionService : ICPlanificacionService
    {
        CPlanificacionL respuesta = new CPlanificacionL();

        public List<List<CBaseDTO>> ListarNombramientosActivos(CListaNombramientosActivosDTO datoBusqueda)
        {
            return respuesta.ListarNombramientosActivos(datoBusqueda);
        }

        public List<CBaseDTO> ListarNombramientosFuncionario(string cedula)
        {
            return respuesta.ListarNombramientosFuncionario(cedula);
        }

        public List<CBaseDTO> ListarPagosFuncionario(string cedula)
        {
            return respuesta.ListarPagosFuncionario(cedula);
        }

        public List<CBaseDTO> CrearSolicitudVacia()
        {
            return respuesta.CrearSolicitudVacia();
        }


        public List<CBaseDTO> ListarDivisiones()
        {
            return new CDivisionL().ListarDivisiones();
        }
        public List<CBaseDTO> ListarDireccionGeneral()
        {
            return new CDireccionGeneralL().ListarDireccionGeneral();
        }
        public List<CBaseDTO> ListarDepartamentos()
        {
            return new CDepartamentoL().ListarDepartamentos();
        }
        public List<CBaseDTO> ListarSecciones()
        {
            return new CSeccionL().ListarSecciones();
        }

        public List<CBaseDTO> ListarClases()
        {
            return respuesta.ListarClases();
        }
        public List<CBaseDTO> ListarEstadoCivil()
        {
            return respuesta.ListarEstadoCivil();
        }
        public List<CBaseDTO> ListarEstadoFuncionario()
        {
            return respuesta.ListarEstadoFuncionario();
        }

        public List<string[]> GenerarDatosReportes(List<CParametrosDTO> parametros)
        {
            return respuesta.GenerarDatosReportes(parametros);
        }
    }
}