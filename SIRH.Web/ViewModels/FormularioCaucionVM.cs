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
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FormularioCaucionVM
    {
        public CCaucionDTO Caucion { get; set; }
        public CEntidadSegurosDTO EntidadSeguros { get; set; }
        public CMontoCaucionDTO MontoCaucion { get; set; }

        public CNotificacionUsuarioDTO Notificacion { get; set; }

        public CDetalleContratacionDTO DetalleContratacion { get; set; }
        public CExpedienteFuncionarioDTO Expediente { get; set; }
        public CErrorDTO Error { get; set; }

        public SelectList Aseguradoras { get; set; }

        [DisplayName("Aseguradora que emite")]
        public int AseguradoraSeleccionada { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }

        public SelectList Montos { get; set; }

        [DisplayName("Nivel")]
        public int MontoSeleccionado { get; set; }
    }
}
