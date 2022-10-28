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
    // NOTE: If you change the class name "CAccionPersonalService" here, you must also update the reference to "CAccionPersonalService" in App.config.
    public class CAccionPersonalService : ICAccionPersonalService
    {
        public CBaseDTO AgregarAccion(CFuncionarioDTO funcionario, CEstadoBorradorDTO estado,
                                      CTipoAccionPersonalDTO tipo, CAccionPersonalDTO accion, CDetalleAccionPersonalDTO detalle)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.AgregarAccion(funcionario, estado, tipo, accion, detalle);
        }

        public List<CBaseDTO> ObtenerAccion(string numAccion)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerAccion(numAccion);
        }

        public List<CBaseDTO> ObtenerAccionProrroga(int idTipo, string numCedula)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerAccionProrroga(idTipo, numCedula);
        }

        public CBaseDTO AnularAccion(CAccionPersonalDTO accion)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.AnularAccion(accion);
        }

        public CBaseDTO AnularAccionModulo(CAccionPersonalDTO accion)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.AnularAccionModulo(accion);
        }

        public CBaseDTO ModificarObservaciones(CAccionPersonalDTO accion)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ModificarObservaciones(accion);
        }

        public CBaseDTO AprobarAccion(CAccionPersonalDTO accion)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.AprobarAccion(accion);
        }

        public List<List<CBaseDTO>> BuscarAccion(CFuncionarioDTO funcionario, CPuestoDTO puesto, CAccionPersonalDTO accion, List<DateTime> fechas)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.BuscarAccion(funcionario, puesto, accion, fechas);
        }
        public List<List<CBaseDTO>> BuscarAccionProrroga(string tipoAccion, List<DateTime> fechas)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.BuscarAccionProrroga(tipoAccion, fechas);
        }
        public List<List<CBaseDTO>> BuscarAccionDesarraigo(bool registrarAprobacion)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.BuscarAccionDesarraigo(registrarAprobacion);
        }
        public List<CBaseDTO> BuscarHistorial(CAccionPersonalHistoricoDTO accion, List<DateTime> fechas)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.BuscarHistorial(accion, fechas);
        }

        public List<CBaseDTO> ObtenerAccionHistorico(int codigo)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerAccionHistorico(codigo);
        }

        public CBaseDTO ObtenerPorcentajeComponenteSalarial(CFuncionarioDTO funcionario, int codigo)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerPorcentajeComponenteSalarial(funcionario, codigo);
        }

        public CBaseDTO ObtenerPorcentajeComponenteSalarialDetallePuesto(CDetallePuestoDTO detalle, int codigo)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerPorcentajeComponenteSalarial(detalle, codigo);
        }

        public CBaseDTO ObtenerCategoriaClase(int codClase)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerCategoriaClase(codClase);
        }

        public CBaseDTO ObtenerSalario(string numCedula)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.ObtenerSalario(numCedula);
        }

        public List<CBaseDTO> ObtenerEscalaCategoriaPeriodo(int indCategoria, int indPeriodo)
        {
            CEscalaSalarialL respuesta = new CEscalaSalarialL();
            return respuesta.BuscarEscalaCategoriaPeriodo(indCategoria, indPeriodo);
        }

        public CBaseDTO CargarAccionHistorico()
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.CargarAccionHistorico();
        }

        public CBaseDTO AgregarRubro(CDetallePuestoDTO detallePuesto, CDetalleAccionPersonalDTO detalle)
        {
            CAccionPersonalL respuesta = new CAccionPersonalL();
            return respuesta.AgregarRubro(detallePuesto, detalle);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            CAccionPersonalL logica = new CAccionPersonalL();
            return logica.BuscarFuncionarioDetallePuesto(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuestoAnterior(string cedula, int codPuestoActual, int codNombramiento = 0)
        {
            CAccionPersonalL logica = new CAccionPersonalL();
            return logica.BuscarFuncionarioDetallePuestoAnterior(cedula, codPuestoActual, codNombramiento);
        }

        public List<CBaseDTO> BuscarAccionDetalleAnterior(int codAccion)
        {
            CAccionPersonalL logica = new CAccionPersonalL();
            return logica.BuscarAccionDetalleAnterior(codAccion);
        }
    }
}