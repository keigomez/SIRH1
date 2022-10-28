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
    // NOTE: If you change the class name "CServiciosGeneralesService" here, you must also update the reference to "CServiciosGeneralesService" in App.config.
    [ServiceBehavior(Namespace = "http://sisrh.mopt.go.cr:85/")]
    public class CServiciosGeneralesService : ICServiciosGeneralesService
    {
        public string[] ConsultaFuncionario(string cedula)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionario(cedula);
        }

        public List<string[]> ConsultaFuncionarioPolicial(decimal codPolicial)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioPolicial(codPolicial);
        }
        
        public bool ConsultaFuncionarioPolicialCedula(decimal codPolicial, string cedula)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioPolicial(codPolicial, cedula);
        }

        public string[] ConsultaFuncionarioClase(string cedula, string clase1, string clase2, string clase3)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioClase(cedula, clase1, clase2, clase3);
        }

        public List<string[]> ConsultaFuncionarioNombre(string apellido1, string apellido2, int titP)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioNombre(apellido1, apellido2, titP);
        }

        public List<string[]> ConsultaFuncionariosOcupacionReal(int codOcupacionReal, int codDivision, int codDireccion)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionariosOcupacionReal(codOcupacionReal, codDivision, codDireccion);
        }

        public List<string[]> ConsultaFuncionariosDireccion(int codDireccion, int codSeccion)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionariosDireccion(codDireccion, codSeccion);
        }

        public List<string[]> ConsultaJefaturaDependencia(int codSeccion)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaJefaturaDependencia(codSeccion);
        }

        public List<string[]> ConsultaExFuncionarios(int mes, int anio)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaExFuncionarios(mes, anio);
        }

        public List<string[]> ConsultaFuncionarioCalificaciones(string cedula)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioCalificaciones(cedula);
        }

        public List<string[]> ConsultaFuncionarioAccionPersonal(string cedula)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioAccionPersonal(cedula);
        }

        public List<string[]> ConsultaFuncionarioPermisoSinSalario(DateTime FechaInicial, DateTime FechaFinal, bool MenosMes)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioPermisoSinSalario(FechaInicial, FechaFinal, MenosMes);
        }
        
        public List<string[]> ConsultaFuncionarioPermisoVacaciones(DateTime FechaInicial, DateTime FechaFinal)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioPermisoVacaciones(FechaInicial, FechaFinal);
        }

        public List<string[]> ConsultaViaticoCorrido(int anio, int mes, string cedula)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaViaticoCorrido(anio, mes, cedula);
        }

        public List<string[]> ActualizarViaticoCorrido(int anio, int mes, string cedula, string reserva, string numBoleta)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ActualizarViaticoCorrido(anio, mes, cedula, reserva, numBoleta);
        }

        public List<string[]> ConsultaVacaciones(string cedula)
        {
            return new CServiciosGeneralesL().ConsultaVacaciones(cedula);
        }

        public List<string[]> ConsultaSaldoVacaciones(string cedula)
        {
            return new CServiciosGeneralesL().ConsultaSaldoVacaciones(cedula);
        }

        public List<string[]> ListarFuncionariosActivos()
        {
            return new CServiciosGeneralesL().ListarFuncionariosActivos();
        }

        public List<string[]> ListarFuncionariosTotal()
        {
            return new CServiciosGeneralesL().ListarFuncionariosTotal();
        }

        public List<string[]> ConsultaFuncionarioPago(string Cedula, DateTime FechaInicial, DateTime FechaFinal)
        {
            CServiciosGeneralesL respuesta = new CServiciosGeneralesL();
            return respuesta.ConsultaFuncionarioPago(Cedula, FechaInicial, FechaFinal);
        }

        public string[] ConsultaFuncionarioPuntosCarrera(string cedula)
        {
            return new CServiciosGeneralesL().ConsultaFuncionarioPuntosCarrera(cedula);
        }

        public List<string[]> SaldosVacacionesFraccionamiento(string cedula)
        {
            return new CPeriodoVacacionesL().ListarPeriodosSG(cedula);
        }

        public string[] GuardarRegistroVacaciones(string numeroSolicitud, int tipo, DateTime fechaInicio, DateTime fechaFinal, int estado, string periodoVacaciones, string cedula, double cantDias)
        {
            return new CServiciosGeneralesL().GuardarRegistroVacaciones(numeroSolicitud, tipo, fechaInicio, fechaFinal, estado, periodoVacaciones, cedula, cantDias);
        }

        public string[] RegistrarPeriodoVacaciones(string cedFuncionario, string periodo, double cntDias, DateTime fecha, int estado)
        {
            return new CServiciosGeneralesL().RegistrarPeriodoVacaciones(cedFuncionario, periodo, cntDias, fecha, estado);
        }

        public string[] RegistrarReintegroVacaciones(string cedula, string documento, string documentoReintegro, DateTime inicio, DateTime final, string periodo, int motivo, string observaciones, decimal cntDias)
        {
            return new CServiciosGeneralesL().RegistrarReintegroVacaciones(cedula, documento, documentoReintegro, inicio, final, periodo, motivo, observaciones, cntDias);
        }

        public List<string[]> ConsultaEstadoVacaciones(string cedula, string nombre, string apellido1,
                                                    string apellido2, string seccion, string estado)
        {
            return new CPeriodoVacacionesL().ConsultaEstadoVacaciones(cedula, nombre, apellido1, apellido2, seccion, estado);
        }

        public CBaseDTO GuardarEnvioCorreos(List<CTemp_EnviarCorreoDTO> correos) => new CNotificacionUsuarioL().GuardarEnvioCorreos(correos);

        public CBaseDTO VerificarCorreosEnviados(DateTime fecha) => new CNotificacionUsuarioL().VerificarCorreosEnviados(fecha);

        public string[] ConsultaFuncionarioFechaNombramiento(string cedula)
        {
            return new CServiciosGeneralesL().ConsultaFuncionarioFechaNombramiento(cedula);
        }
    }
}