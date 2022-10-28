using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    [ServiceContract]
    public interface ICArchivoService
    {
        [OperationContract]
        CBaseDTO AgregarBoletaPrestamo(CFuncionarioDTO funcionario, CUsuarioDTO usuario, CBoletaPrestamoDTO boleta);

        [OperationContract]
        CBaseDTO ActualizarFechaTrasladoArchivoCentralExpediente(CExpedienteFuncionarioDTO expediente);

        [OperationContract]
        CBaseDTO ActualizarEstadoExpedienteEnPrestamo(CExpedienteFuncionarioDTO expediente);

        [OperationContract]
        List<CBaseDTO> ListarDepartamentos();

        [OperationContract]
        List<CBaseDTO> ObtenerExpedientePorCedula(string cedula);

        [OperationContract]
        List<CBaseDTO> ObtenerBoleta(int codigo);

        [OperationContract]
        CBaseDTO VerificarExistenciaBoleta(string cedula);

        [OperationContract]
        List<CBaseDTO> ObtenerExpedientePorNumeroExpediente(int numero_expediente);

        [OperationContract]
        List<CBaseDTO> BusquedaBoletaSegunParametros(CExpedienteFuncionarioDTO expediente);

        [OperationContract]
        List<CBaseDTO> ObtenerExpedientePorCedulaFuncionario(string cedula);

        [OperationContract]
        List<List<CBaseDTO>> RealizarFoleo(string cedula);

        [OperationContract]
        List<CBaseDTO> VerificarFechaVencimientoPrestamo(DateTime fecha);
    }
}
