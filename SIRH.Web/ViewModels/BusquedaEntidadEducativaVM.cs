using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class BusquedaEntidadEducativaVM
    {
        public CEntidadEducativaDTO EntidadEducativa { get; set; }

        public SelectList TiposEntidad { get; set; }

        [DisplayName("Tipo")]
        public string TipoEntidadSeleccionado { get; set; }

        public CErrorDTO Error { get; set; }
    }
}