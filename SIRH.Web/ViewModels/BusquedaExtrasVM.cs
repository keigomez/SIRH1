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
using System.ComponentModel;
using System.Web.Mvc;
using SIRH.DTO;
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class BusquedaExtrasVM
    {
        [DisplayName("Número de cédula")]
        public string Cedula { get; set; }
        
        [DisplayName("Dirección")]
        public string CodDireccion { get; set; }
        
        [DisplayName("Departamento")]
        public string CodDepartamento { get; set; }
        
        [DisplayName("División")]
        public string CodDivision { get; set; }
        
        [DisplayName("Sección")]
        public string CodSeccion { get; set; }

        [DisplayName("Estado")]
        public string Estado { get; set; }

        [DisplayName("Tipo de extra")]
        public string PagoDoble { get; set; }

        [DisplayName("Desde (fecha emisión)")]
        public DateTime? FechaDesde { get; set; }

        [DisplayName("Hasta (fecha emisión)")]
        public DateTime? FechaHasta { get; set; }

        public List<CRegistroTiempoExtraDTO> Registros { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }

    }
}