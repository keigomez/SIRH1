using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class HistoricoPuntosVM
    {
        public List<CPuntosDTO> Puntos { get; set; }

        public BusquedaHistoricoPuntosVM paramBusqueda { get; set; }

        public int TotalPuntos { get; set; }

        public int TotalPaginas { get; set; }

        public int PaginaActual { get; set; }
    }
}