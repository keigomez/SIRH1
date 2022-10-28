using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioPolicialVM
    {

        public List<BusquedaFuncionarioPolicialVM> Funcionarios { get; set; }

        public BusquedaFuncionarioPolicialVM paramBusqueda { get; set; }

        public int TotalFuncionarios { get; set; }

        public int TotalPaginas { get; set; }

        public int PaginaActual { get; set; }
    }
}