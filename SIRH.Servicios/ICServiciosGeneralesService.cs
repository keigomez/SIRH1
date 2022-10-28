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
    // NOTE: If you change the interface name "ServiciosGeneralesService" here, you must also update the reference to "ICAccionPersonalService" in App.config.
    [ServiceContract(Namespace = "http://sisrh.mopt.go.cr:85/")]
    public interface ICServiciosGeneralesService
    {
        [OperationContract]
        string[] ConsultaFuncionario(string cedula);

        [OperationContract]
        string[] ConsultaFuncionarioClase(string cedula, string clase1, string clase2, string clase3);

        [OperationContract]
        List<string[]> ConsultaFuncionarioPolicial(decimal codPolicial);

        [OperationContract]
        bool ConsultaFuncionarioPolicialCedula(decimal codPolicial, string cedula);

        [OperationContract]
        List<string[]> ConsultaFuncionarioNombre(string apellido1, string apellido2, int titP);

        [OperationContract]
        List<string[]> ConsultaFuncionariosDireccion(int codDireccion, int codSeccion);
        
        [OperationContract]
        List<string[]> ConsultaFuncionariosOcupacionReal(int codOcupacionReal, int codDivision, int codDireccion);

        [OperationContract]
        List<string[]> ConsultaJefaturaDependencia(int codSeccion);

        [OperationContract]
        List<string[]> ConsultaExFuncionarios(int mes, int anio);

        [OperationContract]
        List<string[]> ConsultaFuncionarioCalificaciones(string cedula);

        [OperationContract]
        List<string[]> ConsultaFuncionarioAccionPersonal(string cedula);

        [OperationContract]
        List<string[]> ConsultaFuncionarioPermisoSinSalario(DateTime FechaInicial, DateTime FechaFinal, bool MenosMes);

        [OperationContract]
        List<string[]> ConsultaFuncionarioPermisoVacaciones(DateTime FechaInicial, DateTime FechaFinal);

        [OperationContract]
        List<string[]> ConsultaViaticoCorrido(int anio, int mes, string cedula);

        [OperationContract]
        List<string[]> ActualizarViaticoCorrido(int anio, int mes, string cedula, string reserva, string numBoleta);

        [OperationContract]
        List<string[]> ConsultaVacaciones(string cedula);

        [OperationContract]
        List<string[]> ConsultaSaldoVacaciones(string cedula);

        [OperationContract]
        List<string[]> ListarFuncionariosActivos();

        [OperationContract]
        List<string[]> ListarFuncionariosTotal();

        [OperationContract]
        List<string[]> ConsultaFuncionarioPago(string Cedula, DateTime FechaInicial, DateTime FechaFinal);

        [OperationContract]
        string[] ConsultaFuncionarioPuntosCarrera(string cedula);

        [OperationContract]
        List<string[]> SaldosVacacionesFraccionamiento(string cedula);

        [OperationContract]
        string[] GuardarRegistroVacaciones(string numeroSolicitud, int tipo, DateTime fechaInicio, DateTime fechaFinal, int estado, string periodoVacaciones, string cedula, double cantDias);

        [OperationContract]
        string[] RegistrarPeriodoVacaciones(string cedFuncionario, string periodo, double cntDias, DateTime fecha, int estado);

        [OperationContract]
        string[] RegistrarReintegroVacaciones(string cedula, string documento, string documentoReintegro, DateTime inicio, DateTime final, string periodo, int motivo, string observaciones, decimal cntDias);

        [OperationContract]
        List<string[]> ConsultaEstadoVacaciones(string cedula, string nombre, string apellido1,
                                                            string apellido2, string seccion, string estado);

        [OperationContract]
        CBaseDTO GuardarEnvioCorreos(List<CTemp_EnviarCorreoDTO> correos);

        [OperationContract]
        CBaseDTO VerificarCorreosEnviados(DateTime fecha);

        [OperationContract]
        string[] ConsultaFuncionarioFechaNombramiento(string cedula);
    }
}