using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SIRH.Web.ViewModels
{
    public class BusquedaHistoricoCursosVM
    {

        public CCursoDTO Curso { get; set; }

        [DisplayName("Fecha rige desde")]
        public DateTime FechaDesde { get; set; }

        [DisplayName("Fecha rige hasta")]
        public DateTime FechaHasta { get; set; }

        public CErrorDTO Error { get; set; }
    }
}