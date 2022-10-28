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
    // NOTE: If you change the class name "CRegistroTEService" here, you must also update the reference to "CRegistroTEService" in App.config.
    public class CRegistroTEService : ICRegistroTEService
    {
        public List<CBaseDTO> ListarClasesConFormato(string formato)
        {
            CClaseL respuesta = new CClaseL();
            List<CBaseDTO> datos = respuesta.ListarClasesConFormato(formato);
            return datos;
        }

        public List<CBaseDTO> ListarPresupuestos()
        {
            CPresupuestoL respuesta = new CPresupuestoL();
            return respuesta.ListarPresupuestos();
        }

        public CRespuestaDTO RegistrarTiempoExtra(string cedula, CRegistroTiempoExtraDTO registro,
                                                List<CDetalleTiempoExtraDTO> extras)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.RegistrarTiempoExtra(cedula,registro,extras);
        }
        public CRespuestaDTO RegistrarTiempoExtraDoble(string id, List<CDetalleTiempoExtraDTO> extras)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.RegistrarTiempoExtraDoble(id, extras);
        }
        public CRespuestaDTO EstaEnVacaciones(string cedula, DateTime fechaInicio, DateTime fechaFin)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.EstaEnVacaciones(cedula, fechaInicio, fechaFin);
        }

        public CRespuestaDTO EstaIncapacitado(string cedula, DateTime fechaInicio, DateTime fechaFin)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.EstaIncapacitado(cedula, fechaInicio, fechaFin);
        }
        public CRespuestaDTO BuscarArchivo(int id)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.BuscarArchivo(id);
        }
        public CRespuestaDTO AnularRegistroTiempoExtra(int id, DateTime fechaCarga, bool doble, string estado)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.AnularRegistroTiempoExtra(id, fechaCarga, doble, estado);
        }
        public List<CBaseDTO> ObtenerRegistroExtrasSaved(string cedula, string periodo, bool doble) 
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.ObtenerRegistroExtrasSaved(cedula, periodo, doble);
        }
        public List<CBaseDTO> ObtenerRegistroExtrasDetalleDoble(string cedula, string periodo)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.ObtenerRegistroExtrasDetalleDoble(cedula, periodo);
        }
        public List<CBaseDTO> ObtenerRegistroExtrasDetalle(DateTime fechaRegistro, int id, bool doble)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.ObtenerRegistroExtrasDetalle(fechaRegistro, id, doble);
        }

        public CRespuestaDTO BuscarRegistroTiempoExtra(string cedula, string periodo)
       {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.BuscarRegistroTiempoExtra(cedula, periodo);
        }

        public List<CRegistroTiempoExtraDTO> BuscarTiempoExtraFiltros(string cedula, DateTime? fechaDesde, DateTime? fechaHasta, string coddivision, string coddireccion,
                    string coddepartamento, string codseccion, int estado, bool doble)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.BuscarTiempoExtraFiltros(cedula, fechaDesde, fechaHasta, coddivision, coddireccion, coddepartamento, codseccion, estado, doble);
        }

        public CBaseDTO ActualizarObservacionEstado(int registro, string observacion, bool doble)
        {
            return new CRegistroTEL().ActualizarObservacionEstado(registro, observacion, doble);
        }
    }
}
