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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICViaticoCorridoGastoTransporteService" in both code and config file together.
    [ServiceContract]
    public interface ICViaticoCorridoGastoTransporteService
    {
        //----------------------ViaticoCorrido-------------------------//
        [OperationContract]
        CBaseDTO ObtenerEliminacion(string codigo, string tipo);
        [OperationContract]
        List<CBaseDTO> ObtenerDeduccionViaticoCorrido(string codigo);
        [OperationContract]
        List<List<CBaseDTO>> BuscarMovimientoViaticoCorrido(CFuncionarioDTO funcionario, string codigo);
        [OperationContract]
        CBaseDTO ObtenerMovimientoViaticoCorrido(string codigo);
        [OperationContract]
        CBaseDTO AnularMovimientoViaticoCorrido(CMovimientoViaticoCorridoDTO mViaticoC);
        //[OperationContract]
        //List<CBaseDTO> DescargarDireccion(string cedula);
        [OperationContract]
        List<CBaseDTO> AgregarDetalleDedcuccionViaticoCorrido(List<CDetalleDeduccionViaticoCorridoDTO> detalleDVC,
                                                      CMovimientoViaticoCorridoDTO movimientoVC);
        //[OperationContract]
        //List<CBaseDTO> ObtenerRegistroIncapacidad(int codigo);
        [OperationContract]
        List<CBaseDTO> ListarRegistroIncapacidad(string cedula);
        [OperationContract]
        List<CBaseDTO> AgregarDetalleEliminacionViaticoCorrido(CDetalleEliminacionViaticoCorridoGastoTransporteDTO detalleEVC,
                                                              CMovimientoViaticoCorridoDTO movimientoVC);
        [OperationContract]
        CBaseDTO BuscarCartaPresentacionCedula(CFuncionarioDTO funcionario);
        [OperationContract]
        CBaseDTO ObtenerViaticoCorridoActual(string cedula);
        [OperationContract]
        CBaseDTO AgregarMovimientoViaticoCorridoEliminacion(CMovimientoViaticoCorridoDTO movimientoVC);
        [OperationContract]
        CBaseDTO CargarDistritoId(int idDistrito);
        
        [OperationContract]
        List<List<CBaseDTO>> ActualizarVencimientoViaticoCorrido(DateTime fecha);

        [OperationContract]
        CBaseDTO AgregarContratoArrendamientoViaticoCorrido(CViaticoCorridoDTO viaticoC, CContratoArrendamientoDTO contrato);

        [OperationContract]
        CBaseDTO AgregarViaticoCorrido(CCartaPresentacionDTO carta, CFuncionarioDTO funcionario, CViaticoCorridoDTO viaticoC
                                   ,List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contrato);
        [OperationContract]
        CBaseDTO AgregarFacturaViaticoCorrido(CViaticoCorridoDTO viaticoC, CFacturaDesarraigoDTO factura);

        [OperationContract]
        CBaseDTO AnularViaticoCorrido(CViaticoCorridoDTO viaticoC);

        [OperationContract]
        CBaseDTO FinalizarContratoVC(CViaticoCorridoDTO viaticoC);

        [OperationContract]
        List<List<CBaseDTO>> BuscarViaticoCorrido(CFuncionarioDTO funcionario, CViaticoCorridoDTO viaticoC, List<DateTime> fechasEmision,
                                              List<DateTime> fechasVencimiento, List<string> lugarContrato);
        [OperationContract]
        List<CBaseDTO> BuscarFuncionarioCedula(string cedula);

        [OperationContract]
        List<CBaseDTO> CargarCantones();

        [OperationContract]
        List<CBaseDTO> CargarDistritos();

        [OperationContract]
        List<CBaseDTO> CargarProvincias();

        /*[OperationContract]
        List<List<CBaseDTO>> ViaticoCorridoPorVencer(DateTime fecha);*/

        [OperationContract]
        List<List<CBaseDTO>> GetLocalizacion(bool cantones, bool distritos, bool provincias);
        [OperationContract]
        List<List<CBaseDTO>> ListarPagoMesesAnteriores(string cedula);

        [OperationContract]
        List<List<CBaseDTO>> ListarPagosViaticoCorrido(int mes, int anio);

        [OperationContract]
        List<List<CBaseDTO>> ListarViaticoCorrido();

        [OperationContract]
        List<List<CBaseDTO>> ListarViaticos(CViaticoCorridoDTO datoBusqueda);

        [OperationContract]
        List<List<CBaseDTO>> ListarViaticoCorridoPago(int mes, int anio);

        [OperationContract]
        List<List<CBaseDTO>> ListarViaticoPagosPendientes(int anio);

        [OperationContract]
        List<CBaseDTO> ListarEstadosViaticoCorrido();

        [OperationContract]
        CBaseDTO ModificarEstadoViaticoCorrido(CViaticoCorridoDTO viaticoC);

        [OperationContract]
        CBaseDTO ModificarViaticoCorrido(CViaticoCorridoDTO viaticoC);

        [OperationContract]
        List<CBaseDTO> ObtenerContratosArrendamientosViaticoCorrido(CViaticoCorridoDTO viaticoC);

        [OperationContract]
        List<List<CBaseDTO>> ObtenerViaticoCorrido(string codigo);

        //[OperationContract]
        //List<CBaseDTO> ObtenerDiasRebajar(CFuncionarioDTO funcionario, int mes, int anio, decimal montoDia);

        [OperationContract]
        List<CBaseDTO> ObtenerDiasRebajar(CFuncionarioDTO funcionario, CViaticoCorridoDTO viatico, int mes, int anio, decimal montoDia);

        [OperationContract]
        List<CBaseDTO> ObtenerDiasPagar(CViaticoCorridoDTO viatico, int mes, int anio);

        [OperationContract]
        List<CBaseDTO> ObtenerFacturasViaticoCorrido(CViaticoCorridoDTO ViaticoC);

        [OperationContract]
        List<CBaseDTO> ObtenerCorreosElectronicos(string codigo);

        [OperationContract]
        CBaseDTO ObtenerMontoRetroactivo(CCartaPresentacionDTO carta, List<DateTime> fecha);

        [OperationContract]
        CBaseDTO AgregarPagoViaticoCorrido(CPagoViaticoCorridoDTO pago, List<CDetallePagoViaticoCorridoDTO> detalles, CFuncionarioDTO funcionario);

        [OperationContract]
        CBaseDTO AsignarReservaRecurso(CPagoViaticoCorridoDTO pago);

        [OperationContract]
        CBaseDTO AnularPagoViaticoCorrido(CPagoViaticoCorridoDTO pago);

        [OperationContract]
        List<List<CBaseDTO>> ObtenerPagoViaticoCorrido(int codigo);

        [OperationContract]
        CBaseDTO AgregarReintegro(List<CViaticoCorridoReintegroDTO> lista);

        [OperationContract]
        CBaseDTO ModificarReintegroVC(int id, int estado);

        /*[OperationContract]
        CBaseDTO RetomarViaticoCorrido(CViaticoCorridoDTO viaticoC);*/
        //-------------------------------------------------------------//
        //----------------------GastosTransporte-----------------------//
        [OperationContract]
        List<CBaseDTO> ListarAsignacion(int codigo);

        [OperationContract]
        List<CBaseDTO> ListarAsignacionModificada(int codigo);
        [OperationContract]
        List<CBaseDTO> ObtenerDeduccionGastoTransporte(string codigo);
        //[OperationContract]
        //List<CBaseDTO> ObtenerDiasRebajarGT(CFuncionarioDTO funcionario, int mes, int anio, decimal montoDia);

        [OperationContract]
        List<CBaseDTO> ObtenerDiasRebajarGT(CFuncionarioDTO funcionario, CGastoTransporteDTO gasto, int mes, int anio, decimal montoDia);

        [OperationContract]
        List<CBaseDTO> ObtenerDiasPagarGT(CGastoTransporteDTO gasto, int mes, int anio);

        [OperationContract]
        List<List<CBaseDTO>> BuscarMovimientoGastoTransporte(CFuncionarioDTO funcionario, string codigo);
        [OperationContract]
        CBaseDTO ObtenerMovimientoGastoTransporte(string codigo);
        [OperationContract]
        CBaseDTO AnularMovimientoGastoTransporte(CMovimientoGastoTransporteDTO mGastoT);
        [OperationContract]
        List<List<CBaseDTO>> ListarPagoMesesAnterioresGastoTransporte(string cedula);

        [OperationContract]
        List<List<CBaseDTO>> ListarPagosGastoTransporte(int mes, int anio);
    
        [OperationContract]
        CBaseDTO AsignarReservaRecursoGT(CPagoGastoTransporteDTO pago);

        [OperationContract]
        CBaseDTO ObtenerGastoTransporteActual(string cedula);
        [OperationContract]
        List<List<CBaseDTO>> ObtenerGastosTransporte(string codigo);
        [OperationContract]
        List<CBaseDTO> AgregarDetalleDedcuccionGastoTransporte(List<CDetalleDeduccionGastoTransporteDTO> detalleDGT,
                                                      CMovimientoGastoTransporteDTO movimientoGT);
        [OperationContract]
        List<CBaseDTO> AgregarDetalleEliminacionGastoTransporte(CDetalleEliminacionViaticoCorridoGastoTransporteDTO detalleEVC,
                                                      CMovimientoGastoTransporteDTO movimientoGT);
        [OperationContract]
        List<List<CBaseDTO>> ActualizarVencimientoGastoTransporte(DateTime fecha);
        
        [OperationContract]
        List<CBaseDTO> AgregarGastoTransporte(CCartaPresentacionDTO carta, CFuncionarioDTO funcionario, CGastoTransporteDTO gastoT, List<CDetalleAsignacionGastoTransporteDTO> detalleAGT
                                   /*,List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contratos*/);
        [OperationContract]
        List<CBaseDTO> AgregarDetalleAsignModificada(List<CDetalleAsignacionGastoTransporteModificadaDTO> detalleAsigModif, int idgasto);


        [OperationContract]
        CBaseDTO AnularGastoTransporte(CGastoTransporteDTO gastoT);

        [OperationContract]
        CBaseDTO FinalizarContratoGT(CGastoTransporteDTO gastoT);

        [OperationContract]
        List<List<CBaseDTO>> BuscarGastoTransporte(CFuncionarioDTO funcionario, CGastoTransporteDTO gastoT, List<DateTime> fechasEmision,
                                              List<DateTime> fechasVencimiento, List<string> lugarContrato);
        /*[OperationContract]
        List<List<CBaseDTO>> GastoTransportePorVencer(DateTime fecha);*/

        [OperationContract]
        List<List<CBaseDTO>> ListarGastoTransporte();

        [OperationContract]
        List<List<CBaseDTO>> ListarGastos(CGastoTransporteDTO datoBusqueda);

        [OperationContract]
        List<List<CBaseDTO>> ListarGastoTransportePago(int mes, int anio);

        [OperationContract]
        List<List<CBaseDTO>> ListarGastoPagosPendientes(int anio);

        [OperationContract]
        List<CBaseDTO> ListarEstadosGastoTransporte();

        [OperationContract]
        CBaseDTO ModificarEstadoGastoTransporte(CGastoTransporteDTO gastoT);

        [OperationContract]
        CBaseDTO ModificarGastoTransporte(CGastoTransporteDTO gastoT);

        [OperationContract]
        CBaseDTO ModificarMontoGastoTrans(int idgasto, decimal newMonto);

        [OperationContract]
        CBaseDTO AgregarPagoGastoTransporte(CPagoGastoTransporteDTO pago);

        [OperationContract]
        CBaseDTO AnularPagoGastoTransporte(CPagoGastoTransporteDTO pago);

        [OperationContract]
        List<List<CBaseDTO>> ObtenerPagoGastoTransporte(int codigo);

        [OperationContract]
        CBaseDTO AgregarReintegroGT(List<CGastoTransporteReintegroDTO> lista);

        [OperationContract]
        CBaseDTO ModificarReintegroGT(int id, int estado);

        [OperationContract]
        CBaseDTO ModificarFecContratoNumDocGT(int idgasto, DateTime feccontrato, string numdocumento);

        [OperationContract]
        CBaseDTO ModificarPagoReservaRecursoGT(int idPago, string reserva);

        [OperationContract]
        CBaseDTO ModificarFecContratoNumDocVC(int idviatico, DateTime feccontrato, string numdocumento);
        
        [OperationContract]
        CBaseDTO ModificarPagoReservaRecursoVC(int idPago, string reserva);
        //[OperationContract]
        //List<List<CBaseDTO>> ObtenerGastoTransporte(string codigo);



        //[OperationContract]
        //CBaseDTO ObtenerMontoRetroactivoGastoTransporte(CCartaPresentacionDTO carta, List<DateTime> fecha);

        /*[OperationContract]
        CBaseDTO RetomarViaticoCorrido(CViaticoCorridoDTO viaticoC);*/
        //-------------------------------------------------------------//


        [OperationContract]
        List<List<CBaseDTO>> ListarTipoDia();

        [OperationContract]
        CBaseDTO AgregarCatalogoDia(CCatDiaViaticoGastoDTO registro);

        [OperationContract]
        List<CBaseDTO> ListarCatalogoDia();

        [OperationContract]
        CBaseDTO ActualizarRutaTarifa(List<CGastoTransporteRutasDTO> rutas);
    }
}
