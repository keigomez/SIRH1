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
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class BusquedaCaucionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CCaucionDTO Caucion { get; set; }
        public CMontoCaucionDTO NivelCaucion { get; set; }
        public List<FormularioCaucionVM> Cauciones { get; set; }

        [DisplayName("Fecha de emisión desde")]
        public DateTime FechaEmisionDesde { get; set; }

        [DisplayName("Fecha de emisión hasta")]
        public DateTime FechaEmisionHasta { get; set; }

        [DisplayName("Fecha de vencimiento desde")]
        public DateTime FechaVenceDesde { get; set; }

        [DisplayName("Fecha de vencimiento hasta")]
        public DateTime FechaVenceHasta { get; set; }

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }
    }
}
