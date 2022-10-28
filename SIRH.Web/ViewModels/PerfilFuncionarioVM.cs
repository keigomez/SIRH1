using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class PerfilFuncionarioVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CNombramientoDTO FuncionarioPropietario { get; set; }
        public CNombramientoDTO FuncionarioOcupante { get; set; }
        public CEstadoFuncionarioDTO EstadoFuncionario { get; set; }
        public List<CHistorialEstadoCivilDTO> EstadoCivil { get; set; }
        public List<CInformacionContactoDTO> InformacionContacto { get; set; }
        public CDireccionDTO Direccion { get; set; }
        public CProvinciaDTO ProvinciaDireccion { get; set; }
        public CCantonDTO CantonDireccion { get; set; }
        public CDistritoDTO DistritoDireccion { get; set; }
        public CDetalleContratacionDTO DetalleContrato { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CEstadoNombramientoDTO EstadoNombramiento { get; set; }
        public List<CCalificacionNombramientoDTO> Calificaciones { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CEstadoPuestoDTO EstadoPuesto { get; set; }
        public CClaseDTO ClasePuesto { get; set; }
        public CEspecialidadDTO EspecialidadPuesto { get; set; }
        public CSubEspecialidadDTO SubEspecialidadPuesto { get; set; }
        public COcupacionRealDTO OcupacionRealPuesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CPresupuestoDTO PresupuestoPuesto { get; set; }
        public CProgramaDTO ProgramaPuesto { get; set; }
        public CAreaDTO AreaPuesto { get; set; }
        public CActividadDTO ActividadPuesto { get; set; }
        public CDivisionDTO DivisionPuesto { get; set; }
        public CDireccionGeneralDTO DireccionGeneralPuesto { get; set; }
        public CDepartamentoDTO DepartamentoPuesto { get; set; }
        public CSeccionDTO SeccionPuesto { get; set; }
        public CTipoUbicacionDTO TipoUbicacionTrabajo { get; set; }
        public CProvinciaDTO ProvinciaTrabajo { get; set; }
        public CCantonDTO CantonTrabajo { get; set; }
        public CDistritoDTO DistritoTrabajo { get; set; }
        public CUbicacionPuestoDTO UbicacionTrabajo { get; set; }
        public CTipoUbicacionDTO TipoUbicacionContrato { get; set; }
        public CProvinciaDTO ProvinciaContrato { get; set; }
        public CCantonDTO CantonContrato { get; set; }
        public CDistritoDTO DistritoContrato { get; set; }
        public CUbicacionPuestoDTO UbicacionContrato { get; set; }
        public CMovimientoPuestoDTO MovimientoPuesto { get; set; }
        public CSalarioDTO Salario  { get; set; }

        public SelectList MotivosMovimiento { get; set; }

        public string MensajeEstudio { get; set; }
        public string MensajePresupuesto { get; set; }
        public string MensajePedimento { get; set; }
        public string MensajePrestamo { get; set; }

        public DateTime Fecha { get; set; }
    }
}
