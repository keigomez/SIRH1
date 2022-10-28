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
    public class BusquedaMontosVM
    {
        public CMontoCaucionDTO MontoCaucion { get; set; }

        public CErrorDTO Error { get; set; }

        [DisplayName("Fecha rige desde")]
        public DateTime FechaInicio { get; set; }
        [DisplayName("Fecha rige hasta")]
        public DateTime FechaFin { get; set; }

        [DisplayName("Montos a partir de")]
        public decimal MontoInicio { get; set; }
        [DisplayName("Montos hasta")]
        public decimal MontoFinal { get; set; }

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }
    }
}
