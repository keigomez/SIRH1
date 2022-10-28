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
    // NOTE: If you change the interface name "AccionPersonalService" here, you must also update the reference to "ICAccionPersonalService" in App.config.
    [ServiceContract]
    public interface ICAccionPersonalService
    {
        [OperationContract]
        CBaseDTO AgregarAccion(CFuncionarioDTO funcionario, CEstadoBorradorDTO estado,
                               CTipoAccionPersonalDTO tipo, CAccionPersonalDTO accion, CDetalleAccionPersonalDTO detalle);

        [OperationContract]
        List<CBaseDTO> ObtenerAccion(string numAccion);

        [OperationContract]
        List<CBaseDTO> ObtenerAccionProrroga(int idTipo, string numCedula);

        [OperationContract]
        CBaseDTO AnularAccion(CAccionPersonalDTO accion);

        [OperationContract]
        CBaseDTO AnularAccionModulo(CAccionPersonalDTO accion);

        [OperationContract]
        CBaseDTO ModificarObservaciones(CAccionPersonalDTO accion);

        [OperationContract]
        CBaseDTO AprobarAccion(CAccionPersonalDTO accion);

        [OperationContract]
        List<List<CBaseDTO>> BuscarAccion(CFuncionarioDTO funcionario, CPuestoDTO puesto, CAccionPersonalDTO accion, List<DateTime> fechas);

        [OperationContract]
        List<List<CBaseDTO>> BuscarAccionProrroga(string tipoAccion, List<DateTime> fechas);

        [OperationContract]
        List<List<CBaseDTO>> BuscarAccionDesarraigo(bool registrarAprobacion);

        [OperationContract]
        List<CBaseDTO> BuscarHistorial(CAccionPersonalHistoricoDTO accion, List<DateTime> fechas);

        [OperationContract]
        List<CBaseDTO> ObtenerAccionHistorico(int codigo);

        [OperationContract]
        CBaseDTO ObtenerPorcentajeComponenteSalarial(CFuncionarioDTO funcionario, int codigo);

        [OperationContract]
        CBaseDTO ObtenerPorcentajeComponenteSalarialDetallePuesto(CDetallePuestoDTO detalle, int codigo);

        [OperationContract]
        CBaseDTO ObtenerCategoriaClase(int codClase);

        [OperationContract]
        CBaseDTO ObtenerSalario(string numCedula);

        [OperationContract]
        List<CBaseDTO> ObtenerEscalaCategoriaPeriodo(int indCategoria, int indPeriodo);

        [OperationContract]
        CBaseDTO CargarAccionHistorico();

        [OperationContract]
        CBaseDTO AgregarRubro(CDetallePuestoDTO detallePuesto, CDetalleAccionPersonalDTO detalle);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuestoAnterior(string cedula, int codPuestoActual, int codNombramiento = 0);

        [OperationContract]
        List<CBaseDTO> BuscarAccionDetalleAnterior(int codAccion);
    }
}