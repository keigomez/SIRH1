using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIRH.Web.Models
{
    public class PresupuestoModel
    {
        public List<CPresupuestoDTO> Presupuesto { get; set; }
        public int TotalPresupuestos { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public string CodigoSearch { get; set; }
        public SelectList CodigosPresupuestarios { get; set; }
        public SelectList UniqueIdPresupuestos { get; set; }
        public string CodigoPresupuesto { get; set; }      
    }
}