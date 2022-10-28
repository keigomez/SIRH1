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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CPlanillaService" in both code and config file together.
    public class CPlanillaService : ICPlanillaService
    {
        public List<CBaseDTO> DescargarMeses()
        {
            return null;
        }

        public List<CBaseDTO> BuscarDatosPlanilla(string cedula, DateTime fechaInicio, DateTime fechaFinal)
        {
            return new CHistoricoPlanillaL().BuscarDatosPlanilla(cedula, fechaInicio, fechaFinal);
        }

        public CBaseDTO ObtenerPagoID(int idPago)
        {
            return new CHistoricoPlanillaL().ObtenerPagoID(idPago);
        }

        #region DeduccionTemporal

        public CBaseDTO ObtenerTipoDeduccion(int codigo)
        {
            return new CDeduccionTemporalL().ObtenerTipoDeduccion(codigo);
        }

        public List<CBaseDTO> ListarTiposDeduccion()
        {
            return new CDeduccionTemporalL().ListarTiposDeduccion();
        }

        public CBaseDTO AgregarDeduccionTemporal(CFuncionarioDTO funcionario, CDeduccionTemporalDTO deduccion)
        {
            return new CDeduccionTemporalL().AgregarDeduccionTemporal(funcionario, deduccion);
        }

        public CBaseDTO AnularDeduccionTemporal(CDeduccionTemporalDTO deduccion)
        {
            return new CDeduccionTemporalL().AnularDeduccionTemporal(deduccion);
        }

        public List<CBaseDTO> DescargarDetalleDeduccion(CDeduccionTemporalDTO deduccion)
        {
            return new CDeduccionTemporalL().DescargarDetalleDeduccion(deduccion);
        }

        public List<List<CBaseDTO>> BuscarDeducciones(CFuncionarioDTO funcionario, CDeduccionTemporalDTO deduccion,
                                                CBitacoraUsuarioDTO bitacora, List<DateTime> fechas, List<DateTime> fechasBitacora)
        {
            return new CDeduccionTemporalL().BuscarDeducciones(funcionario, deduccion, bitacora, fechas, fechasBitacora);
        }

        public CBaseDTO AprobarDeduccionTemporal(CDeduccionTemporalDTO deduccion)
        {
            return new CDeduccionTemporalL().ModificarEstadoDeduccion(deduccion, 1); // 1 - Aprobada
        }

        public CBaseDTO ModificarDeduccionExplicacion(CDeduccionTemporalDTO deduccion)
        {
            return new CDeduccionTemporalL().ModificarExplicacion(deduccion);
        }

        #endregion
    }
}