using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICFuncionarioService" here, you must also update the reference to "ICFuncionarioService" in App.config.
    [ServiceContract]
    public interface ICFuncionarioService
    {
        //OperationContract indica que un método define una operación, es un servicio de contrato 
        //Comentario para subir otra vez

        [OperationContract]
        CBaseDTO GuardarDireccionFuncionario(CFuncionarioDTO funcionario, CDireccionDTO direccion);

        [OperationContract]
        List<List<CBaseDTO>> BuscarFuncionarioOferente(string cedula);

        [OperationContract]
        List<CBaseDTO> ListarEntidadesFinancieras();

        [OperationContract]
        List<CBaseDTO> ListarEntidadesGubernamentales();

        [OperationContract]
        List<CBaseDTO> ListarEntidadesAdscritas();

        [OperationContract]
        CBaseDTO BuscarEntidadFinanciera(int codigo);

        [OperationContract]
        CBaseDTO GuardarInformacionContacto(CFuncionarioDTO funcionario, CInformacionContactoDTO informacionContacto);

        [OperationContract]
        CBaseDTO GuardarCuentaBancariaFuncionario(CCuentaBancariaDTO cuenta);

        [OperationContract]
        List<CBaseDTO> DescargarDireccion(string cedula);

        [OperationContract]
        List<CBaseDTO> DescargarCatEstadoCivil(string cedula);

        [OperationContract]
        List<CBaseDTO> EditarJornadaFuncionario(CTipoJornadaDTO jornada);

        [OperationContract]
        List<CBaseDTO> RegistrarJornadaFuncionario(CFuncionarioDTO funcionario,
                                                            CNombramientoDTO nombramiento,
                                                            CTipoJornadaDTO jornada);
        [OperationContract]
        List<CBaseDTO> DescargarInformacionContacto(string cedula);

        [OperationContract]
        CBaseDTO GuardarHistEstadoCivil(CFuncionarioDTO funcionario, CHistorialEstadoCivilDTO historialEstadoCivil);

        [OperationContract]
        List<CBaseDTO> GuardarDetalleContratacion(CDetalleContratacionDTO detalle, CCuentaBancariaDTO cuenta);

        [OperationContract]
        CBaseDTO GuardarDetalleNombramiento(CDetalleNombramientoDTO detalleNombramiento);

        [OperationContract]
        List<CBaseDTO> GuardarDatosPersonalesFuncionario(CFuncionarioDTO funcionario, 
                                                         CHistorialEstadoCivilDTO estadoCivil, 
                                                         List<CInformacionContactoDTO> informacion,
                                                         CDireccionDTO direccion);

        [OperationContract]
        List<List<CBaseDTO>> FuncionariosConCauciones();

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioUsuario(string nombreUsuario);

        [OperationContract]
        CBaseDTO BuscarFuncionarioBase(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioJornada(string cedula);

        [OperationContract]
        CFuncionarioDTO DescargarPerfilFuncionarioBásico(string cedula);

        [OperationContract]
        List<CFuncionarioDTO> BuscarFuncionarioGeneral(List<string> query);

        [OperationContract]
        List<CFuncionarioDTO> BuscarFuncionarioParam(string cedula, string nombre, string apellido1, string apellido2);

        [OperationContract]
        List<CFuncionarioDTO> BuscarFuncionarioPuesto(string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal);

        [OperationContract]
        List<CFuncionarioDTO> BuscarFuncionarioUbicacion(int division, int direccion, int departamento, int seccion, string presupuesto);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDesgloceSalarial(string cedula, string periodo);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuestoVacaciones(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuestoPV(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuestoPropiedad(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioSalario(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioProfesional(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioPolicial(string cedula);

        [OperationContract]
        List<List<CBaseDTO>> DescargarPerfilFuncionarioCompleto(string cedula);

        [OperationContract]
        CBaseDTO BuscarFuncionarioPuntosCarreraProfesional(string cedula);

        [OperationContract]
        List<CBaseDTO> ListarTiposContacto();

        [OperationContract]
        CBaseDTO BuscarFuncionarioNuevo(string cedula);

        [OperationContract]
        List<CFuncionarioDTO> BuscarFuncionarioCompleto(string cedula, string nombre, string apellido1, string apellido2,
                                                     string codpuesto, string codclase, string codespecialidad,
                                                     string codocupacion, string coddivision, string coddireccion, string coddepartamento,
                                                     string codseccion, string codpresupuesto);

        [OperationContract]
        List<CFuncionarioDTO> BuscarFuncionarioCompletoPP(string cedula, string nombre, string apellido1, string apellido2,
                                                         string codpuesto, string codclase, string codespecialidad,
                                                         int codnivel, string coddivision, string coddireccion, string coddepartamento,
                                                         string codseccion, string codpresupuesto, string codestado, string codcostos);

        [OperationContract]
        CBaseDTO ActualizarInformacionBasicaFuncionario(CFuncionarioDTO funcionario, CDireccionDTO direccion,
                                                               List<CInformacionContactoDTO> contacto, CHistorialEstadoCivilDTO historial);

        [OperationContract]
        List<CBaseDTO> BuscarInformacionBasicaFuncionario(string cedula);

        [OperationContract]
        CBaseDTO GuardarDetalleContratacionFuncionario(CDetalleContratacionDTO datos);

        [OperationContract]
        List<CBaseDTO> CargarDetalleContratacionFuncionario(string cedula);

        [OperationContract]
        List<CEMUExfuncionarioDTO> BuscarExfuncionarioFiltros(string cedula, string nombre, string primerApellido, string segundoApellido, string codPuesto);

        [OperationContract]
        List<List<CBaseDTO>> BuscarDetalleFuncionarioPolicial(CDetalleContratacionDTO detalle, List<DateTime> fechas);

        [OperationContract]
        CBaseDTO CargarCalificacionActual(string cedula);
    }
}
