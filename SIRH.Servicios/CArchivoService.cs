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
    public class CArchivoService : ICArchivoService
    {
        public CBaseDTO AgregarBoletaPrestamo(CFuncionarioDTO funcionario, CUsuarioDTO usuario, CBoletaPrestamoDTO boleta) {
            CBoletaPrestamoL respuesta = new Logica.CBoletaPrestamoL();
            return respuesta.AgregarBoletaPrestamo(funcionario, usuario, boleta);
        }

        public List<CBaseDTO> ListarDepartamentos() {
            CDepartamentoL respuesta = new CDepartamentoL();
            return respuesta.ListarDepartamentos();
        }

        public List<CBaseDTO> ObtenerExpedientePorCedula(string cedula) {

            CExpedienteL respuesta = new CExpedienteL();
            return respuesta.ObtenerExpedientePorCedula(cedula);
        }

        public List<CBaseDTO> ObtenerBoleta(int codigo)
        {
            CBoletaPrestamoL respuesta = new CBoletaPrestamoL();
            return respuesta.ObtenerBoleta(codigo);
        }

        public CBaseDTO VerificarExistenciaBoleta(string cedula) {

            CBoletaPrestamoL respuesta = new CBoletaPrestamoL();
            return respuesta.VerificarExistenciaBoleta(cedula);
        }

        public List<CBaseDTO> BusquedaBoletaSegunParametros(CExpedienteFuncionarioDTO expediente)
        {
            CBoletaPrestamoL respuesta = new CBoletaPrestamoL();
            return respuesta.BusquedaBoletaSegunParametros(expediente);
        }

        public List<List<CBaseDTO>> RealizarFoleo(string cedula) {
            CBoletaPrestamoL respuesta = new CBoletaPrestamoL();
            return respuesta.RealizarFoleo(cedula);
        }

        public List<CBaseDTO> VerificarFechaVencimientoPrestamo(DateTime fecha) {
                CBoletaPrestamoL respuesta = new CBoletaPrestamoL();
                return respuesta.VerificarFechaVencimientoPrestamo(fecha);
        }

        public List<CBaseDTO> ObtenerExpedientePorNumeroExpediente(int numero_expediente)
        {
            CExpedienteL respuesta = new CExpedienteL();
            return respuesta.ObtObtenerExpedientePorNumeroExpediente(numero_expediente);
        }

        public CBaseDTO ActualizarFechaTrasladoArchivoCentralExpediente(CExpedienteFuncionarioDTO expediente)
        {
            CExpedienteL respuesta = new CExpedienteL();
            return respuesta.ActualizarFechaTrasladoArchivoCentralExpediente(expediente);
        }

        public List<CBaseDTO> ObtenerExpedientePorCedulaFuncionario(string cedula)
        {
            CExpedienteL respuesta = new CExpedienteL();
            return respuesta.ObtenerExpedientePorCedulaFuncionario(cedula);
        }

        public CBaseDTO ActualizarEstadoExpedienteEnPrestamo(CExpedienteFuncionarioDTO expediente)
        {
            CExpedienteL respuesta = new CExpedienteL();
            return respuesta.ActualizarEstadoExpedienteEnPrestamo(expediente);
        }
    }
}
