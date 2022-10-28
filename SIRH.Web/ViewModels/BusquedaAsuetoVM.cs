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
    public class BusquedaAsuetoVM
    {
        public CCatalogoDiaDTO dia { get; set; }

        public SelectList Provincia { get; set; }
        [DisplayName("Provincia")]
        public string ProvinciaSeleccionado { get; set; }

        public SelectList Canton { get; set; }
        [DisplayName("Cantones")]
        public string CantonSeleccionado { get; set; }

    }
}