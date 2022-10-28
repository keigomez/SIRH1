using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;

namespace SIRH.Servicios
{
    // NOTE: If you change the interface name "ICRegistroTEService" here, you must also update the reference to "ICRegistroTEService" in App.config.
    [ServiceContract]
    public interface ICRegistroTEService
    {
        [OperationContract]
        List<CBaseDTO> ListarClasesConFormato(string formato);

        [OperationContract]
        List<CBaseDTO> ListarPresupuestos();

        [OperationContract]
        CRespuestaDTO RegistrarTiempoExtra(string cedula, CRegistroTiempoExtraDTO registro, List<CDetalleTiempoExtraDTO> extras);

        [OperationContract]
        CRespuestaDTO RegistrarTiempoExtraDoble(string id, List<CDetalleTiempoExtraDTO> extras);

        [OperationContract]
        CRespuestaDTO EstaEnVacaciones(string cedula, DateTime fechaInicio, DateTime fechaFin);

        [OperationContract]
        CRespuestaDTO EstaIncapacitado(string cedula, DateTime fechaInicio, DateTime fechaFin);

        [OperationContract]
        CRespuestaDTO BuscarArchivo(int id);

        [OperationContract]
        CRespuestaDTO AnularRegistroTiempoExtra(int id, DateTime fechaCarga, bool doble, string estado);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroExtrasSaved(string cedula, string periodo, bool doble);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroExtrasDetalleDoble(string cedula, string periodo);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroExtrasDetalle(DateTime fechaRegistro, int id, bool doble);

        [OperationContract]
        CRespuestaDTO BuscarRegistroTiempoExtra(string cedula, string periodo);

        [OperationContract]
        List<CRegistroTiempoExtraDTO> BuscarTiempoExtraFiltros(string cedula, DateTime? fechaDesde, DateTime? fechaHasta, string coddivision, string coddireccion,
                    string coddepartamento, string codseccion, int estado, bool doble);

        [OperationContract]
        CBaseDTO ActualizarObservacionEstado(int registro, string observacion, bool doble);
    }
}
