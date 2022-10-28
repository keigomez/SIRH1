using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class BusquedaHistoricoPlanillaVM
    {
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public List<CHistoricoPlanillaDTO> Historico { get; set; }
    }
}