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
    // NOTE: If you change the class name "CDesarraigoService" here, you must also update the reference to "CDesarraigoService" in App.config.
    public class CDesarraigoService : ICDesarraigoService
    {

        public List<List<CBaseDTO>> ActualizarVencimientoDesarraigo(DateTime fecha) { 
            return (new CDesarraigoL()).ActualizarVencimientoDesarraigo(fecha);
        }

        public CBaseDTO AgregarContratoArrendamiento(CDesarraigoDTO desarraigo, CContratoArrendamientoDTO contrato)
        {
            var respuesta = new CContratoArrendamientoL();
            return respuesta.AgregarContratoArrendamiento(desarraigo, contrato);
        }

        public CBaseDTO AgregarDesarraigo(CFuncionarioDTO funcionario, CDesarraigoDTO desarraigo,
                                   List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contratos)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.AgregarDesarraigo( funcionario, desarraigo, facturas, contratos);
        }

        public CBaseDTO AgregarFactura(CDesarraigoDTO desarraigo, CFacturaDesarraigoDTO factura)
        {
            var respuesta = new CFacturaDesarraigoL();
            return respuesta.AgregarFactura(desarraigo, factura);
        }

        public CBaseDTO AnularDesarraigo(CDesarraigoDTO desarraigo)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.AnularDesarraigo(desarraigo);
        }

        public List<List<CBaseDTO>> BuscarDesarraigo(CFuncionarioDTO funcionario, CDesarraigoDTO desarraigo, List<DateTime> fechas, CDistritoDTO distrito)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.BuscarDesarraigo(funcionario, desarraigo, fechas, distrito);
        }

        //public List<CBaseDTO> BuscarFuncionarioCedula(string cedula)
        //{
        //    var respuesta = new CDesarraigoL();
        //    return respuesta.BuscarFuncionarioCedula(cedula);
        //}

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

        public List<List<CBaseDTO>> DesarraigosPorVencer(DateTime fecha)
        {
            return (new CDesarraigoL()).DesarraigosPorVencer(fecha);
        }

        public List<List<CBaseDTO>> GetLocalizacion(bool cantones, bool distritos, bool provincias) { 
            return (new CUbicacionPuestoL()).GetLocalizacion(cantones,0,distritos,provincias,0);
        }

        public List<List<CBaseDTO>> ListarDesarraigo()
        {
            var respuesta = new CDesarraigoL();
            return respuesta.ListarDesarraigo();
        }

        public List<CBaseDTO> ListarEstadosDesarraigo() {
            var respuesta = new CDesarraigoL();
            return respuesta.ListarEstadosDesarraigo();
        }

        public CBaseDTO ModificarDesarraigo(CDesarraigoDTO desarraigo, List<CFacturaDesarraigoDTO> facturas, List<CContratoArrendamientoDTO> contrato){
            return new CDesarraigoL().ModificarDesarraigo(desarraigo,facturas,contrato);
        }
        
        public CBaseDTO ModificarEstadoDesarraigo(CDesarraigoDTO desarraigo)
        {
            return new CDesarraigoL().ModificarEstadoDesarraigo(desarraigo);
        }

        public List<CBaseDTO> ObtenerContratosArrendamientos(CDesarraigoDTO desarraigo)
        {
            var respuesta = new CContratoArrendamientoL();
            return respuesta.ObtenerContratosArrendamientos(desarraigo);
        }

        public List<List<CBaseDTO>> ObtenerDesarraigo(int codigo)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.ObtenerDesarraigo(codigo);
        }

        public List<CBaseDTO> ObtenerFacturasDesarraigo(CDesarraigoDTO desarraigo)
        {
            var respuesta = new CFacturaDesarraigoL();
            return respuesta.ObtenerFacturasDesarraigo(desarraigo);
        }

        public List<CBaseDTO> ObtenerCorreosElectronicos(string codigo)
        {
            return (new CInformacionContactoL()).DescargarInformacionContacto(codigo);
        }

        public CBaseDTO ObtenerMontoRetroactivo(string cedula, List<DateTime> fecha)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.ObtenerMontoRetroactivo(cedula, fecha);
        }

        public CBaseDTO ObtenerSalarioBase(int idCategoriaEscala)
        {
            return (new CDesarraigoL()).ObtenerSalarioBase(idCategoriaEscala);
        }
        public CBaseDTO RetomarDesarraigo(CDesarraigoDTO desarraigo)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.RetomarDesarraigo(desarraigo);
        }


        ////  Detalle Eliminación Desarraigo
        public CBaseDTO AgregarDetalleDesarraigoEliminacion(CDetalleDesarraigoEliminacionDTO detalle, CDesarraigoDTO desarraigo)    
        {
            var respuesta = new CDesarraigoL();
            return respuesta.AgregarDetalleDesarraigoEliminacion(detalle, desarraigo);
        }

        public CBaseDTO ModificarDetalleDesarraigoEliminacion(CDetalleDesarraigoEliminacionDTO detalle)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.ModificarDetalleDesarraigoEliminacion(detalle);
        }

        public List<List<CBaseDTO>> CerrarDesarraigosVencidos()
        {
            var respuesta = new CDesarraigoL();
            return respuesta.CerrarDesarraigosVencidos();
        }

        public List<List<CBaseDTO>> ListarDesarraigosIncapacidades(DateTime fechaInicio, DateTime fechaFinal)
        {
            var respuesta = new CDesarraigoL();
            return respuesta.ListarDesarraigosIncapacidades(fechaInicio, fechaFinal);
        }
    }
}
