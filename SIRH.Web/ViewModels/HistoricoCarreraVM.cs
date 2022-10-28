using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.ViewModels
{
    public class HistoricoCarreraVM
    {
        public List<CCarreraProfesionalDTO> Carreras { get; set; }

        public BusquedaHistoricoCarreraVM paramBusqueda { get; set; }
        public int TotalCarreras { get; set; }

        public int TotalPaginas { get; set; }

        public int PaginaActual { get; set; }
    }
}