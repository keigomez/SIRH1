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
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioExperienciaVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        public CExperienciaProfesionalDTO Experiencia { get; set; }

        public SelectList TiposExperiencia { get; set; }
        [DisplayName("Tipo de Experiencia")]
        public int TipoExperienciaSeleccionada { get; set; }

        public CErrorDTO Error { get; set; }
        public string Mensaje { get; set; }
    }
}
