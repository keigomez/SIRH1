using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class EntidadEducativaVM
    {
        public List<CEntidadEducativaDTO> Entidades { get; set; }

        public BusquedaEntidadEducativaVM paramBusqueda { get; set; }

        public int TotalEntidades { get; set; }

        public int TotalPaginas { get; set; }

        public int PaginaActual { get; set; }

    }
}