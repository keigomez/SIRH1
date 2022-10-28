using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICPagoFeriadoService" here, you must also update the reference to "ICPagoFeriadoService" in App.config.
    [ServiceContract]
    public interface ICPagoFeriadoService
    {
        [OperationContract]
         CBaseDTO AgregarPagoExtraordinario(CFuncionarioDTO funcionario, CPagoExtraordinarioDTO pagoExtraordinario);

        [OperationContract]
         List<CBaseDTO> ObtenerPagoExtraordinario(int codigo);


        [OperationContract]
         CBaseDTO AgregarPagoFeriado(CPagoExtraordinarioDTO pagoExtraordinario, CPagoFeriadoDTO pagoFeriado,
                                       CEstadoTramiteDTO estadoTramite, CFuncionarioDTO funcionario);

        [OperationContract]
         List<CBaseDTO> ObtenerPagoFeriado(int codigo);

        [OperationContract]
        List<List<CBaseDTO>> BuscarPagosFeriado(CFuncionarioDTO funcionario, CPagoFeriadoDTO tramite,
                                                     List<DateTime> fechasTramite, CEstadoTramiteDTO estadoTramite,
                                                     List<string> diasFeriados);

        [OperationContract]
         CBaseDTO AnularPagoFeriado(CPagoFeriadoDTO tramite);


        [OperationContract]
        CBaseDTO AgregarDeduccion(CFuncionarioDTO funcionario, List<CDeduccionEfectuadaDTO> deducciones, List<CCatalogoDeduccionDTO> catalogoDeduccion,
                                          CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario);

        [OperationContract]
         List<CBaseDTO> ObtenerDeduccionEfectuada(int codigo);

        [OperationContract]
         List<List<CBaseDTO>> RetornarDeduccionesPorPagoFeriado(int codigo);


        [OperationContract]
         List<CBaseDTO> ObtenerCatalogoDeduccion(int codigo);

        [OperationContract]
         List<List<CBaseDTO>> ListarDeduccionesTipo(int tipo);


        [OperationContract]
        CBaseDTO AgregarDiaPagado(CFuncionarioDTO funcionario, List<CDiaPagadoDTO> dias, List<CCatalogoDiaDTO> catalogodias,
                                         CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario);

        [OperationContract]
         List<CBaseDTO> ObtenerDiaPagado(int codigo);

        [OperationContract]
         List<List<CBaseDTO>> RetornarDiasPorTramitePagado(int codigo);


        [OperationContract]
         CBaseDTO AgregarAsueto(CCatalogoDiaDTO asueto, CTipoDiaDTO tipoDia);

        [OperationContract]
         List<CBaseDTO> ObtenerCatalogoDia(int codigo);

        [OperationContract]
         List<List<CBaseDTO>> ListarDiasPorTipo(int tipo);

        [OperationContract]
         CBaseDTO AgregarUbicacionAsueto(CCantonDTO canton, CCatalogoDiaDTO asueto, CUbicacionAsuetoDTO ubicacionAsueto);

        [OperationContract]
        List<List<CBaseDTO>> ListarAsuetosPorUbicacion(string provincia, string canton);

        [OperationContract]
        List<List<CBaseDTO>> ListarCantones();

        [OperationContract]
        List<CBaseDTO> ObtenerCanton(int codigo);

        [OperationContract]
        CBaseDTO BuscarDesgloceSalarialPF(string cedula);

        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula);
       
        [OperationContract]
        List<CBaseDTO> BuscarPuestoPF(string cedula);

        [OperationContract]
        CBaseDTO EliminarPagoExtraordinario(CPagoExtraordinarioDTO pagoExtraordinario);

        [OperationContract]
        CBaseDTO EliminarPagoFeriado(CPagoFeriadoDTO pagoFeriado);

        [OperationContract]
        CBaseDTO EliminarDeduccionEfectuada(CPagoFeriadoDTO pagoFeriado);

        [OperationContract]
        CBaseDTO EliminarDiaPagado(CPagoFeriadoDTO pagoFeriado);

        [OperationContract]
        List<CBaseDTO> RetornarDiaPorTramitePagado(int codigo);

        [OperationContract]
        List<CBaseDTO>  ObtenerSalarioEscolar();

        [OperationContract]
        CBaseDTO AgregarBeneficio(CFuncionarioDTO funcionario, CRemuneracionEfectuadaPFDTO beneficio,
                                          CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario);

        [OperationContract]
        CBaseDTO ActualizarSalarioEscolar(CCatalogoRemuneracionDTO remuneracionEfectuada);

        [OperationContract]
        List<List<CBaseDTO>> ListarDeduccionesPagoTipo(int tipo, int codigo);

        [OperationContract]
        CBaseDTO ObtenerSalarioEscolarEfectuado(int codigoTramite);

    }
}
