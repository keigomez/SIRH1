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
        public CRelPresupuestoExtraDTO CargarRelPresupuestoExtraPorID(string IdPresupuesto)
        {
            CRelPresupuestoExtraL logica = new CRelPresupuestoExtraL();
            return logica.CargarRelPresupuestoExtraPorID(IdPresupuesto);
        }

        public CRelPresupuestoExtraDTO RetornarRelPresupuestoExtra(string Cedula)
        {
            CRelPresupuestoExtraL logica = new CRelPresupuestoExtraL();
            return logica.RetornarRelPresupuestoExtra(Cedula);
        }

        public List<CRelPresupuestoExtraDTO> RetornarRelPresupuestoExtras()
        {
            CRelPresupuestoExtraL logica = new CRelPresupuestoExtraL();
            return logica.RetornarRelPresupuestoExtras();
        }

        public CRespuestaDTO RegistrarTiempoExtra(CFuncionarioDTO funcionario, CRegistroTiempoExtraDTO registro,
                                                List<CDetalleTiempoExtraDTO> extras)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.RegistrarTiempoExtra(funcionario,registro,extras);
        }

        public CRespuestaDTO GuardarRegistroTiempoExtra(CFuncionarioDTO funcionario, CRegistroTiempoExtraDTO registro,
                                                    List<CDetalleTiempoExtraDTO> detalle)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.RegistrarTiempoExtra(funcionario, registro, detalle);
        }

        public List<CBaseDTO> ObtenerRegistroExtrasEncabezado(string cedula, string periodo) 
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.ObtenerRegistroExtrasEncabezado(cedula, periodo);
        }

        public List<List<CBaseDTO>> ObtenerRegistroExtrasDetalle(string cedula, string periodo)
        {
            CRegistroTEL respuesta = new CRegistroTEL();
            return respuesta.ObtenerRegistroExtrasDetalle(cedula, periodo);
        }
    }
}
