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
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class FormularioAsuetoVM
    {
        public CCatalogoDiaDTO Dia { get; set; }
        public CTipoDiaDTO TipoDia { get; set; }
        public CErrorDTO Error { get; set; }
        public CUbicacionAsuetoDTO UbicacionAsueto { get; set; }

        public SelectList Canton { get; set; }
        [DisplayName("Cantón")]
        public string CantonSeleccionado { get; set; }

        public SelectList Mes { get; set; }
        [DisplayName("Tipo nombramiento")]
        public string MesSeleccionado { get; set; }

        public SelectList Dias1 { get; set; }
        [DisplayName("Día")]
        public string Dias1Seleccionado { get; set; }

        public SelectList Dias2 { get; set; }
        [DisplayName("Día")]
        public string Dias2Seleccionado { get; set; }

        public SelectList Dias3 { get; set; }
        [DisplayName("Día")]
        public string Dias3Seleccionado { get; set; }

        [DisplayName("Detalle")]
        public string Detalle { get; set; }
    }
}