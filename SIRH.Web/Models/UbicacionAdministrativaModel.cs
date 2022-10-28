using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.Models
{
    public class UbicacionAdministrativaModel
    {
        public List<CUbicacionAdministrativaDTO> UbicacionesAdministrativas { get; set; }
        public int TotalUbicaciones { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public string CodigoSearch { get; set; }
    }
}