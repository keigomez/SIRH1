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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CViaticoCorridoGastoTransporteService" in both code and config file together.
    public class CViaticoCorridoGastoTransporteService : ICViaticoCorridoGastoTransporteService
    {
        //-----------------------ViaticoCorrido----------------------------//
        public CBaseDTO ObtenerEliminacion(string codigo, string tipo)
        {
            var respuesta = new CDetalleEliminacionViaticoCorridoGastoTransporteL();
            return respuesta.ObtenerEliminacion(codigo, tipo);
        }
        public List<CBaseDTO> ObtenerDeduccionViaticoCorrido(string codigo)
        {
            var respuesta = new CDetalleDeduccionViaticoCorridoL();
            return respuesta.ObtenerDeduccionViaticoCorrido(codigo);
        }
        public List<List<CBaseDTO>> BuscarMovimientoViaticoCorrido(CFuncionarioDTO funcionario, string codigo)
        {
            var respuesta = new CMovimientoViaticoCorridoL();
            return respuesta.BuscarMovimientoViaticoCorrido(funcionario, codigo);
        }
        public CBaseDTO ObtenerMovimientoViaticoCorrido(string codigo)
        {
            var respuesta = new CMovimientoViaticoCorridoL();
            return respuesta.ObtenerMovimientoViaticoCorrido(codigo);
        }
        public CBaseDTO AnularMovimientoViaticoCorrido(CMovimientoViaticoCorridoDTO mViaticoC)
        {
            var respuesta = new CMovimientoViaticoCorridoL();
            return respuesta.AnularMovimientoViaticoCorrido(mViaticoC);
        }
        //public List<CBaseDTO> DescargarDireccion(string cedula) {
        //    var respuesta = new CDireccionL();
        //    return respuesta.DescargarDireccion(cedula);
        //}
        public List<CBaseDTO> AgregarDetalleDedcuccionViaticoCorrido(List<CDetalleDeduccionViaticoCorridoDTO> detalleDVC,
                                                      CMovimientoViaticoCorridoDTO movimientoVC)
        {
            var respuesta = new CDetalleDeduccionViaticoCorridoL();
            return respuesta.AgregarDetalleDedcuccionViaticoCorrido(detalleDVC, movimientoVC);
        }
        //public List<CBaseDTO> ObtenerRegistroIncapacidad(int codigo) {
        //    var respuesta = new CRegistroIncapacidadL();
        //    return respuesta.ObtenerRegistroIncapacidad(codigo);
        //}
        public List<CBaseDTO> ListarRegistroIncapacidad(string cedula) {
            var respuesta = new CRegistroIncapacidadL();
            return respuesta.ListarRegistroIncapacidad(cedula);
        }
        public List<CBaseDTO> AgregarDetalleEliminacionViaticoCorrido(CDetalleEliminacionViaticoCorridoGastoTransporteDTO detalleEVC,
                                                              CMovimientoViaticoCorridoDTO movimientoVC)
        {
            var respuesta = new CDetalleEliminacionViaticoCorridoGastoTransporteL();
            return respuesta.AgregarDetalleEliminacionViaticoCorrido(detalleEVC, movimientoVC);
        }
        public CBaseDTO BuscarCartaPresentacionCedula(CFuncionarioDTO funcionario)
        {
            var respuesta = new CCartaPresentacionL();
            return respuesta.BuscarCartaPresentacionCedula(funcionario);

        }
         public CBaseDTO ObtenerViaticoCorridoActual(string cedula)
         {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ObtenerViaticoCorridoActual(cedula);

        }
        public CBaseDTO AgregarMovimientoViaticoCorridoEliminacion(CMovimientoViaticoCorridoDTO movimientoVC)
        {
            var respuesta = new CMovimientoViaticoCorridoL();
            return respuesta.AgregarMovimientoViaticoCorridoEliminacion(movimientoVC);

        }
        public CBaseDTO CargarDistritoId(int idDistrito) {
            var respuesta = new CDistritoL();
            return respuesta.CargarDistritoId(idDistrito);
        }
        public List<List<CBaseDTO>> ActualizarVencimientoViaticoCorrido(DateTime fecha)
        {
            return (new CViaticoCorridoL()).ActualizarVencimientoViaticoCorrido(fecha);
        }

        public CBaseDTO AgregarContratoArrendamientoViaticoCorrido(CViaticoCorridoDTO viaticoC, CContratoArrendamientoDTO contrato)
        {
            var respuesta = new CContratoArrendamientoL();
            return respuesta.AgregarContratoArrendamientoViaticoCorrido(viaticoC, contrato);
        }

        public CBaseDTO AgregarViaticoCorrido(CCartaPresentacionDTO carta, CFuncionarioDTO funcionario, CViaticoCorridoDTO viaticoC
                                  ,List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contrato)
       {
            var respuesta = new CViaticoCorridoL();
            return respuesta.AgregarViaticoCorrido(carta, funcionario, viaticoC, facturas, contrato);
        }

          public CBaseDTO AgregarFacturaViaticoCorrido(CViaticoCorridoDTO viaticoC, CFacturaDesarraigoDTO factura)
          {
              var respuesta = new CFacturaDesarraigoL();
              return respuesta.AgregarFacturaViaticoCorrido(viaticoC, factura);
          }

        public CBaseDTO AnularViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.AnularViaticoCorrido(viaticoC);
        }

        public CBaseDTO FinalizarContratoVC(CViaticoCorridoDTO viaticoC)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.FinalizarViaticoCorrido(viaticoC);
        }

        public List<List<CBaseDTO>> BuscarViaticoCorrido(CFuncionarioDTO funcionario, CViaticoCorridoDTO viaticoC, List<DateTime> fechasEmision,
                                              List<DateTime> fechasVencimiento, List<string> lugarContrato)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.BuscarViaticoCorrido(funcionario, viaticoC, fechasEmision, fechasVencimiento, lugarContrato);
       }
        public List<List<CBaseDTO>> ListarPagoMesesAnteriores(string cedula) { 

            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarPagoMesesAnteriores(cedula);
        }

        public List<List<CBaseDTO>> ListarPagosViaticoCorrido(int mes, int anio)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarPagosViaticoCorrido(mes, anio);
        }

        public List<CBaseDTO> BuscarFuncionarioCedula(string cedula)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.BuscarFuncionarioCedula(cedula);
        }

        //[Este ya no se usa]Se hizo GetLocalizacion para disminuir la carga al servidor
        public List<CBaseDTO> CargarCantones()
        {
            return (new CCantonL()).ListarCantones();
        }

        //[Este ya no se usa]Se hizo GetLocalizacion para disminuir la carga al servidor
        public List<CBaseDTO> CargarDistritos()
        {
            return (new CDistritoL()).ListarDistritos();
        }

        //[Este ya no se usa]Se hizo GetLocalizacion para disminuir la carga al servidor
        public List<CBaseDTO> CargarProvincias()
        {
            return (new CProvinciaL()).ListarProvincias();
        }

        /*public List<List<CBaseDTO>> DesarraigosPorVencer(DateTime fecha)
        {
            return (new CDesarraigoL()).DesarraigosPorVencer(fecha);
        }*/

        public List<List<CBaseDTO>> GetLocalizacion(bool cantones, bool distritos, bool provincias)
        {
            return (new CUbicacionPuestoL()).GetLocalizacion(cantones, 0, distritos, provincias, 0);
        }

        public List<List<CBaseDTO>> ListarViaticoCorrido()
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarViaticoCorrido();
        }

        public List<List<CBaseDTO>> ListarViaticos(CViaticoCorridoDTO datoBusqueda)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarViaticos(datoBusqueda);
        }

        public List<List<CBaseDTO>> ListarViaticoCorridoPago(int mes, int anio)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarViaticoCorridoPago(mes, anio);
        }

        public List<List<CBaseDTO>> ListarViaticoPagosPendientes(int anio)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarViaticoPagosPendientes(anio);
        }
        public List<CBaseDTO> ListarEstadosViaticoCorrido()
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ListarEstadosViaticoCorrido();
        }

        public CBaseDTO ModificarEstadoViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            return new CViaticoCorridoL().ModificarEstadoViaticoCorrido(viaticoC);
        }

        public CBaseDTO ModificarViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            return new CViaticoCorridoL().ModificarViaticoCorrido(viaticoC);
        }

        public List<CBaseDTO> ObtenerContratosArrendamientosViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            var respuesta = new CContratoArrendamientoL();
            return respuesta.ObtenerContratosArrendamientosViaticoCorrido(viaticoC);
        }

        public List<List<CBaseDTO>> ObtenerViaticoCorrido(string codigo)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ObtenerViaticoCorrido(codigo);
        }

        public List<CBaseDTO> ObtenerDiasRebajar(CFuncionarioDTO funcionario, CViaticoCorridoDTO viatico, int mes, int anio, decimal montoDia)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ObtenerDiasRebajar(funcionario, viatico, mes, anio, montoDia);
        }
        public List<CBaseDTO> ObtenerDiasPagar(CViaticoCorridoDTO viatico, int mes, int anio)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ObtenerDiasPagar(viatico, mes, anio);
        }

        public List<CBaseDTO> ObtenerFacturasViaticoCorrido(CViaticoCorridoDTO vaiticoC)
        {
            var respuesta = new CFacturaDesarraigoL();
            return respuesta.ObtenerFacturasViaticoCorrido(vaiticoC);
        }

        public List<CBaseDTO> ObtenerCorreosElectronicos(string codigo)
        {
            return (new CInformacionContactoL()).DescargarInformacionContacto(codigo);
        }

        public CBaseDTO ObtenerMontoRetroactivo(CCartaPresentacionDTO carta, List<DateTime> fecha)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ObtenerMontoRetroactivo(carta, fecha);
        }

        public CBaseDTO AgregarPagoViaticoCorrido(CPagoViaticoCorridoDTO pago, List<CDetallePagoViaticoCorridoDTO> detalles, CFuncionarioDTO funcionario)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.AgregarPagoViaticoCorrido(pago, detalles, funcionario);
        }
        public CBaseDTO AsignarReservaRecurso(CPagoViaticoCorridoDTO pago)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.AsignarReservaRecurso(pago);
        }
        public CBaseDTO AnularPagoViaticoCorrido(CPagoViaticoCorridoDTO pago)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.AnularPagoViaticoCorrido(pago);
        }

        public List<List<CBaseDTO>> ObtenerPagoViaticoCorrido(int codigo)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.ObtenerPagoViaticoCorrido(codigo);
        }

        public CBaseDTO AgregarReintegro(List<CViaticoCorridoReintegroDTO> lista)
        {
            var respuesta = new CViaticoCorridoL();
            return respuesta.AgregarReintegro(lista);
        }

        public CBaseDTO ModificarReintegroVC(int id, int estado)
        {
            var respuesta = new CMovimientoViaticoCorridoL();
            return respuesta.EditarReintegro(id, estado);
        }


        //-----------------------------------------------------------------//
        //-------------------------GastoTransporte-------------------------//
        public List<CBaseDTO> ListarAsignacion(int codigo)
        {
            var respuesta = new CDetalleAsignacionGastoTransporteL();
            return respuesta.ListarAsignacion(codigo);
        }
        public List<CBaseDTO> ListarAsignacionModificada(int codigo)
        {
            var respuesta = new CDetalleAsignacionGastoTransporteModificadaL();
            return respuesta.ListarAsignacionModificada(codigo);
        }
        public List<CBaseDTO> ObtenerDeduccionGastoTransporte(string codigo)
        {
            var respuesta = new CDetalleDeduccionGastoTransporteL();
            return respuesta.ObtenerDeduccionGastoTransporte(codigo);
        }
        public List<CBaseDTO> ObtenerDiasRebajarGT(CFuncionarioDTO funcionario, CGastoTransporteDTO gasto, int mes, int anio, decimal montoDia)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ObtenerDiasRebajarGT(funcionario, gasto, mes, anio, montoDia);
        }

        public List<CBaseDTO> ObtenerDiasPagarGT(CGastoTransporteDTO gasto, int mes, int anio)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ObtenerDiasPagar(gasto, mes, anio);
        }
        public List<List<CBaseDTO>> BuscarMovimientoGastoTransporte(CFuncionarioDTO funcionario, string codigo) {
            var respuesta = new CMovimientoGastoTransporteL();
            return respuesta.BuscarMovimientoGastoTransporte(funcionario, codigo);
        }
        public CBaseDTO ObtenerMovimientoGastoTransporte(string codigo)
        {
            var respuesta = new CMovimientoGastoTransporteL();
            return respuesta.ObtenerMovimientoGastoTransporte(codigo);
        }
        public CBaseDTO AnularMovimientoGastoTransporte(CMovimientoGastoTransporteDTO mGastoT)
        {
            var respuesta = new CMovimientoGastoTransporteL();
            return respuesta.AnularMovimientoGastoTransporte(mGastoT);
        }
        public List<List<CBaseDTO>> ListarPagoMesesAnterioresGastoTransporte(string cedula) {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarPagoMesesAnterioresGastoTransporte(cedula);
        }
        public List<List<CBaseDTO>> ListarPagosGastoTransporte(int mes, int anio)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarPagosGastoTransporte(mes, anio);
        }
        public CBaseDTO AsignarReservaRecursoGT(CPagoGastoTransporteDTO pago)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.AsignarReservaRecurso(pago);
        }
        public CBaseDTO ObtenerGastoTransporteActual(string cedula) {
            var respuesta = new CGastoTransporteL();
            return respuesta.ObtenerGastoTransporteActual(cedula);
        }
        public List<List<CBaseDTO>> ObtenerGastosTransporte(string codigo) {
            var respuesta = new CGastoTransporteL();
            return respuesta.ObtenerGastosTransporte(codigo);
        }
        public List<CBaseDTO> AgregarDetalleDedcuccionGastoTransporte(List<CDetalleDeduccionGastoTransporteDTO> detalleDGT,
                                                      CMovimientoGastoTransporteDTO movimientoGT)
        {
            var respuesta = new CDetalleDeduccionGastoTransporteL();
            return respuesta.AgregarDetalleDedcuccionGastoTransporte(detalleDGT, movimientoGT);
        }
        public List<CBaseDTO> AgregarDetalleEliminacionGastoTransporte(CDetalleEliminacionViaticoCorridoGastoTransporteDTO detalleEVC,
                                                      CMovimientoGastoTransporteDTO movimientoGT)
        {
            var respuesta = new CDetalleEliminacionViaticoCorridoGastoTransporteL();
            return respuesta.AgregarDetalleEliminacionGastoTransporte(detalleEVC, movimientoGT);
        }
        public List<List<CBaseDTO>> ActualizarVencimientoGastoTransporte(DateTime fecha)
        {
            return (new CGastoTransporteL()).ActualizarVencimientoGastoTransporte(fecha);
        }
        
        public List<CBaseDTO> AgregarGastoTransporte(CCartaPresentacionDTO carta, CFuncionarioDTO funcionario, CGastoTransporteDTO gastoT,List<CDetalleAsignacionGastoTransporteDTO>detalleAGT
                                   /*,List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contratos*/)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.AgregarGastoTransporte(carta, funcionario, gastoT, detalleAGT);
        }
        public List<CBaseDTO> AgregarDetalleAsignModificada(List<CDetalleAsignacionGastoTransporteModificadaDTO> detalleAsigModif, int idgasto)
        {
            var respuesta = new CDetalleAsignacionGastoTransporteModificadaL();
            return respuesta.AgregarDetalleAsignacionGTModificada(detalleAsigModif, idgasto);
        }

        public CBaseDTO AnularGastoTransporte(CGastoTransporteDTO gastoT)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.AnularGastoTransporte(gastoT);
        }

        public CBaseDTO FinalizarContratoGT(CGastoTransporteDTO gastoT)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.FinalizarGastoTransporte(gastoT);
        }

        public List<List<CBaseDTO>> BuscarGastoTransporte(CFuncionarioDTO funcionario, CGastoTransporteDTO gastoT, List<DateTime> fechasEmision,
                                              List<DateTime> fechasVencimiento, List<string> lugarContrato)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.BuscarGastoTransporte(funcionario, gastoT, fechasEmision, fechasVencimiento, lugarContrato);
        }

        public List<List<CBaseDTO>> ListarGastoTransporte()
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarGastoTransporte();
        }

        public List<List<CBaseDTO>> ListarGastos(CGastoTransporteDTO datoBusqueda)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarGastos(datoBusqueda);
        }

        public List<List<CBaseDTO>> ListarGastoTransportePago(int mes, int anio)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarGastoTransportePago(mes,anio);
        }
        public List<List<CBaseDTO>> ListarGastoPagosPendientes(int anio)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarGastoPagosPendientes(anio);
        }

        public List<CBaseDTO> ListarEstadosGastoTransporte()
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ListarEstadosGastoTransporte();
        }

        public CBaseDTO ModificarEstadoGastoTransporte(CGastoTransporteDTO gastoT)
        {
            return new CGastoTransporteL().ModificarEstadoGastoTransporte(gastoT);
        }

        public CBaseDTO ModificarGastoTransporte(CGastoTransporteDTO gastoT)
        {
            return new CGastoTransporteL().ModificarGastoTransporte(gastoT);
        }

        public CBaseDTO ModificarMontoGastoTrans(int idgasto, decimal newMonto)
        {
            return new CGastoTransporteL().ModificarMontoGastoTrans(idgasto, newMonto);
        }
        public CBaseDTO AgregarPagoGastoTransporte(CPagoGastoTransporteDTO pago)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.AgregarPagoGastoTransporte(pago);
        }      
        public CBaseDTO AnularPagoGastoTransporte(CPagoGastoTransporteDTO pago)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.AnularPagoGastoTransporte(pago);
        }

        public List<List<CBaseDTO>> ObtenerPagoGastoTransporte(int codigo)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.ObtenerPagoGastoTransporte(codigo);
        }

        public CBaseDTO AgregarReintegroGT(List<CGastoTransporteReintegroDTO> lista)
        {
            var respuesta = new CGastoTransporteL();
            return respuesta.AgregarReintegro(lista);
        }

        public CBaseDTO ModificarReintegroGT(int id, int estado)
        {
            var respuesta = new CMovimientoGastoTransporteL();
            return respuesta.EditarReintegro(id, estado);
        }
        public CBaseDTO ModificarFecContratoNumDocGT(int idgasto, DateTime feccontrato, string numdocumento)
        {
            return new CGastoTransporteL().ModificarFecContratoNumDocGT(idgasto, feccontrato, numdocumento);
        }

        public CBaseDTO ModificarPagoReservaRecursoGT(int idPago, string reserva)
        {
            return new CGastoTransporteL().ModificarPagoReservaRecursoGT(idPago, reserva);
        }

        
        public CBaseDTO ModificarFecContratoNumDocVC(int idviatico, DateTime feccontrato, string numdocumento)
        {
            return new CViaticoCorridoL().ModificarFecContratoNumDocVC(idviatico, feccontrato, numdocumento);
        }

        public CBaseDTO ModificarPagoReservaRecursoVC(int idPago, string reserva)
        {
            return new CViaticoCorridoL().ModificarPagoReservaRecursoVC(idPago, reserva);
        }

        //public List<List<CBaseDTO>> ObtenerGastoTransporte(string codigo)
        //{
        //    var respuesta = new CGastoTransporteL();
        //    return respuesta.ObtenerGastoTransporte(codigo);
        //}
        //public CBaseDTO ObtenerMontoRetroactivoGastoTransporte(CCartaPresentacionDTO carta, List<DateTime> fecha)
        //{
        //    var respuesta = new CGastoTransporteL();
        //    return respuesta.ObtenerMontoRetroactivoGastoTransporte(carta, fecha);
        //}

        //public CBaseDTO RetomarDesarraigo(CDesarraigoDTO desarraigo)
        //{
        //    var respuesta = new CDesarraigoL();
        //    return respuesta.RetomarDesarraigo(desarraigo);
        //}
        //-----------------------------------------------------------------//


        public List<List<CBaseDTO>> ListarTipoDia()
        {
            return new CTipoDiaL().RetornarTiposDia();
        }

        public CBaseDTO AgregarCatalogoDia(CCatDiaViaticoGastoDTO registro)
        {
            return new CViaticoCorridoL().AgregarCatalogoDia(registro);
        }

        public List<CBaseDTO> ListarCatalogoDia()
        {
            return new CViaticoCorridoL().ListarCatalogoDia();
        }

        public CBaseDTO ActualizarRutaTarifa(List<CGastoTransporteRutasDTO> rutas)
        {
            return new CGastoTransporteL().ActualizarRutaTarifa(rutas);
        }
    }
}
