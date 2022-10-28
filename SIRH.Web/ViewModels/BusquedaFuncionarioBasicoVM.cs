using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class BusquedaFuncionarioBasicoVM
    {
        public string TituloPantalla { get; set; }

        [DisplayName("Cédula")]
        [MaxLength(10, ErrorMessage = "La cédula debe tener 10 dígitos")]
        public string Cedula { get; set; }

        public string TextoBoton { get; set; }
    }
}