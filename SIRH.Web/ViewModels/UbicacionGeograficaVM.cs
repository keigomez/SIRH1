using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class SeleccionProvinciaVM
    {
        public List<CProvinciaDTO> EntidadesProvincia { get; set; }
        public SelectList Provincias { get; set; }
        [DisplayName("Provincia")]
        public int ProvinciaSeleccionada { get; set; }
    }

    public class SeleccionCantonVM
    {
        public List<CCantonDTO> EntidadesCanton { get; set; }
        public SelectList Cantones { get; set; }
        [DisplayName("Cantón")]
        public int CantonSeleccionado { get; set; }
    }

    public class SeleccionDistritoVM
    {
        public List<CDistritoDTO> EntidadesDistrito { get; set; }
        public SelectList Distritos { get; set; }
        [DisplayName("Distrito")]
        public int DistritoSeleccionado { get; set; }
    }
}
