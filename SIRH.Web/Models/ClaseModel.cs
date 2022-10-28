using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Collections.Generic;

namespace SIRH.Web.Models
{
    public class ClaseModel
    {
        public List<CClaseDTO> Clase { get; set; }
        public int TotalClases { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public string CodigoSearch { get; set; }
        public string NombreSearch { get; set; }
    }
}
