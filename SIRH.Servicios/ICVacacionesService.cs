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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICVacacionesService" in both code and config file together.
    [ServiceContract]
    public interface ICVacacionesService
    {
        [OperationContract]
        CBaseDTO RegistraReintegroVacaciones(string numeroDocumento, CReintegroVacacionesDTO reintegroVacaciones, string cedFuncionario);

        [OperationContract]
        CBaseDTO AnularPeriodoVacaciones(int codigo);

        [OperationContract]
        CBaseDTO ObtenerPeriodo(string cedFuncionario, string codigoPeriodo, int codigo);

        [OperationContract]
        CBaseDTO GuardarRegistroVacaciones(CRegistroVacacionesDTO registroVacaciones, string cedulaFuncionario, string periodo, int documento);
        [OperationContract]
        List<List<CBaseDTO>> ObtenerDetalleVacaciones(string cedFuncionario);
        [OperationContract]
        CBaseDTO GuardaRegistroPeriodo(CPeriodoVacacionesDTO registroPeriodo, string cedulaFuncionario);
        [OperationContract]
        List<CBaseDTO> ListarRegistroVacaciones(string cedula, string periodo, int codigo);
        [OperationContract]
        List<CBaseDTO> ListarReintegrosPeriodos(string cedula, string periodo, int codigo);
        [OperationContract]
        List<CBaseDTO> ListarPeriodosActivos(string cedula);
        [OperationContract]
        List<CBaseDTO> ListarPeriodosNoActivos(string cedula);
        [OperationContract]
        List<CBaseDTO> ListarDivisiones();
        [OperationContract]
        List<CBaseDTO> ListarDireccionGeneral();
        [OperationContract]
        List<CBaseDTO> ListarDepartamentos();
        [OperationContract]
        List<CBaseDTO> ListarSecciones();
        [OperationContract]
        List<List<CBaseDTO>> BuscarVacaciones(CFuncionarioDTO funcionario, CPeriodoVacacionesDTO periodoVacaciones, CRegistroVacacionesDTO registroVacaciones,
                                  List<DateTime> fechas, string direccion, string seccion, string division, string departamento, string estadoSeleccion, string tipoVacaciones);
        [OperationContract]
        CBaseDTO DetalleContratacion(string cedula);
        [OperationContract]
        List<CBaseDTO> ObtenerRegistro(string cedFuncionario, string numeroDocumento);
        [OperationContract]
        List<List<CBaseDTO>> BuscarReintegros(CFuncionarioDTO funcionario, CPeriodoVacacionesDTO periodoVacaciones, CRegistroVacacionesDTO registroVacaciones,
                             List<DateTime> fechas, string direccion, string seccion, string division, string departamento);
        [OperationContract]
        CBaseDTO GuardarRebajoColectivo(CFuncionarioDTO funcionario, CRegistroVacacionesDTO rebajoColectivo);
        [OperationContract]
        List<CBaseDTO> ListarFuncionariosActivos(bool guardasSeguridad, bool oficialesTransito);
        [OperationContract]
        CBaseDTO ValidarNumeroDocumento(string numTransaccion);
        [OperationContract]
        CBaseDTO AnularRebajoColectivo(CFuncionarioDTO funcionario, string rebajoColectivo);
        [OperationContract]
        List<List<CBaseDTO>> ObtenerInconsistencias(DateTime fechaInicio, DateTime fechaFin, string cedula);
        [OperationContract]
        List<CBaseDTO> ListaPeriodos(string cedula);

        [OperationContract]
        CBaseDTO ActualizarDatosVacaciones(int idregistro, string cambio, decimal saldo);

        [OperationContract]
        CBaseDTO TrasladarRegistroVacaciones(int idRegistro, decimal dias, int periodoDestino);

        [OperationContract]
        List<CBaseDTO> BuscarHistorialMovimientoVacaciones(string cedula, string periodo);

        [OperationContract]
        List<CBaseDTO> BuscarHistorialPeriodoVacacionesSinActuales(string cedula, List<string> actuales);

        [OperationContract]
        CBaseDTO RegistrarReintegroRegistro(CReintegroVacacionesDTO reintegro);
    }
}
