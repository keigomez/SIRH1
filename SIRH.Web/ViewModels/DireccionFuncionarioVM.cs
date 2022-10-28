using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.Web.Mvc;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class DireccionFuncionarioVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CDireccionDTO Direccion { get; set; }
        public CHistorialEstadoCivilDTO EstadoCivil { get; set; }
        public int Edad { get; set; }
        public CErrorDTO Error { get; set; }

        public SelectList Distritos { get; set; }
        [DisplayName("Distrito")]
        public int DistritoSeleccionado { get; set; }

        public SelectList Cantones { get; set; }
        [DisplayName("Cantón")]
        public int CantonSeleccionado { get; set; }

        public SelectList Provincias { get; set; }
        [DisplayName("Provincia")]
        public int ProvinciaSeleccionada { get; set; }
    }
}
