using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class FormularioNombramientoVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CFuncionarioDTO FuncionarioPuesto { get; set; }
        public CPuestoDTO PuestoFuncionario { get; set; }
        public CDetallePuestoDTO DetallePuestoFuncionario { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CNombramientoDTO NombramientoNuevo { get; set; }
        public CDetalleAccionPersonalDTO DetalleAccion { get; set; }
        public CAccionPersonalHistoricoDTO AccionHistorica { get; set; }
        public CErrorDTO Error { get; set; }
        public List<CErrorDTO> ListaWarnings { get; set; }
        public SelectList MotivosMovimiento { get; set; }

        [DisplayName("Tipo de nombramiento")]
        public int MotivoSeleccionado { get; set; }

        [DisplayName("Tipo de nombramiento")]
        public string CodOficio { get; set; }

        [DisplayName("Explicación")]
        public string Explicacion { get; set; }
    
        public string TextoMotivo { get; set; }

        [DisplayName("Fecha nombramiento desde")]
        public DateTime FechaEmisionDesde { get; set; }

        [DisplayName("Fecha nombramiento hasta")]
        public DateTime FechaEmisionHasta { get; set; }
    }

    public class ListaFormularioNombramientoVM
    {
        public List<FormularioNombramientoVM> Lista { get; set; }
    }
}