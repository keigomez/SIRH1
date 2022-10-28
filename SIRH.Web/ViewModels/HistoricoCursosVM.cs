using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.ViewModels
{
    public class HistoricoCursosVM
    {
        public List<CCursoDTO> Cursos { get; set; }

        public BusquedaHistoricoCursosVM paramBusqueda { get; set; }

        public int TotalCursos { get; set; }

        public int TotalPaginas { get; set; }

        public int PaginaActual { get; set; }
    }
}