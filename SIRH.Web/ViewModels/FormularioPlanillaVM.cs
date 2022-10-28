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
    public class FormularioPlanillaVM
    {
       
        public CErrorDTO Error { get; set; }

        public CComponentePresupuestarioDTO ComponentePresupuestario { get; set; }

        //public List<CComponentePresupuestarioDTO> Componentes { get; set; }

        public SelectList Programas { get; set; }

        [DisplayName("Programa")]
        public int ProgramaSeleccionado { get; set; }

        public SelectList ObjetoGasto { get; set; }

        [DisplayName("Objeto de Gasto")]
        public int ObjetoGastoSeleccionado { get; set; }

       
        public SelectList TiposMovimiento { get; set; }

        [DisplayName("Tipo de Movimiento")]
        public int TipoSeleccionado { get; set; }
    }
}