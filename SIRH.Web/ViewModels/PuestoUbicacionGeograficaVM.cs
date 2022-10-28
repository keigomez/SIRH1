using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class PuestoUbicacionGeograficaVM
    {
        public DetallePuestoVM DetallePuesto { get; set; }

        public CUbicacionPuestoDTO UbicacionTrabajoNueva { get; set; }

        public List<CUbicacionPuestoDTO> HistorialTrabajo { get; set; }

        public string NombreProvincia { get; set; }

        public SelectList Provincias { get; set; }
        [DisplayName("Provincia")]
        [Required(ErrorMessage = "Debe indicar la provincia donde se ubicará el puesto")]
        public string ProvinciaSeleccionada { get; set; }

        public SelectList Cantones { get; set; }
        [DisplayName("Cantón")]
        [Required(ErrorMessage = "Debe indicar el cantón donde se ubicará el puesto")]
        public string CantonSeleccionado { get; set; }

        public SelectList Distritos { get; set; }
        [DisplayName("Distrito")]
        [Required(ErrorMessage = "Debe indicar el distrito donde se ubicará el puesto")]
        public string DistritoSeleccionado { get; set; }

        public CErrorDTO Error { get; set; }
    }
}
