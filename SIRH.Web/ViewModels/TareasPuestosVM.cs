using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class TareasPuestoVM
    {
        public DetallePuestoVM DetallePuesto { get; set; }
        public CErrorDTO Error { get; set; }
        public CTareasPuestoDTO Tarea { get; set; }
    }
}
