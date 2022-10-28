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
    // NOTE: If you change the class name "CNombramientoService" here, you must also update the reference to "CNombramientoService" in App.config.
    public class CNombramientoService : ICNombramientoService
    {
        public List<List<CBaseDTO>> BuscarDatosParaNombramiento(string cedula, string codpuesto)
        {
            return new CNombramientoL().BuscarDatosParaNombramiento(cedula, codpuesto);
        }
        public List<CBaseDTO> DescargarCalificacionesCedula(string cedula)
        {
            CCalificacionNombramientoL logica = new CCalificacionNombramientoL();
            return logica.DescargarCalificacionesCedula(cedula);
        }

        public CNombramientoDTO DescargarNombramiento(string cedula)
        {
            CNombramientoL logica = new CNombramientoL();
            return logica.DescargarNombramiento(cedula);
        }

        public List<CBaseDTO> DescargarNombramientoActualCedula(string cedula)
        {
            CNombramientoL logica = new CNombramientoL();
            return logica.DescargarNombramientoActualCedula(cedula);
        }

        public CNombramientoDTO DescargarNombramientoActualPuesto(string codPuesto)
        {
            CNombramientoL logica = new CNombramientoL();
            return logica.DescargarNombramientoActualPuesto(codPuesto);
        }

        public CBaseDTO GuardarNombramiento(CNombramientoDTO nombramiento, CPuestoDTO puesto)
        {
            CNombramientoL logica = new CNombramientoL();
            return logica.GuardarNombramiento(nombramiento, puesto);
        }

        public CBaseDTO CrearNombramientoInicial(CNombramientoDTO entidad)
        {
            CBaseDTO respuesta = new CBaseDTO();

            CNombramientoL nombramiento = new CNombramientoL();

            respuesta = nombramiento.CrearNombramientoInicial(entidad);

            return respuesta;
        }

        public List<List<CBaseDTO>> BuscarDatosRegistroNombramiento(string codpuesto, string cedula)
        {
            return new CNombramientoL().BuscarDatosRegistroNombramiento(codpuesto, cedula);
        }

        public List<CBaseDTO> ListarEstadosNombramiento()
        {
            return new CNombramientoL().ListarEstadosNombramiento();
        }

        public List<List<CBaseDTO>> BuscarHistorialNombramiento(CFuncionarioDTO funcionario,
                                                List<DateTime> fechasEmision, CPuestoDTO puesto)
        {
            return new CNombramientoL().BuscarHistorialNombramiento(funcionario, fechasEmision, puesto);
        }

        public List<CBaseDTO> NombramientoPorCodigo(int codigoNombramiento)
        {
            return new CNombramientoL().NombramientoPorCodigo(codigoNombramiento);
        }

        public List<List<CBaseDTO>> ListarNombramientosVence(DateTime fecha) => new CNombramientoL().ListarNombramientosVence(fecha);
    }
}
