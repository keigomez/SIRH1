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
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class BusquedaEmpleadosMarcasAsistenciaVM
    {
        public CEmpleadoDTO Empleado;

        public SelectList Estados { get; set; }
        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }

        public string Cedula { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string NombreApellido1 { get; set; }

        public string NombreApellido2 { get; set; }

        public List<CEmpleadoDTO> ListaResultados { get; set; }


    }
}
