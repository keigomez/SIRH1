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
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class FormularioCorreoVM
    {
        public List<ArchivoExcel> Registros { get; set; }
        public SelectList Motivos { get; set; }
        public string MotivoSeleccionado { get; set; }

        public int CantidadCorreos { get; set; }

        public CErrorDTO Error { get; set; }
        public HttpPostedFileBase File { get; set; }

        public string Imagen { get; set; }
    }

    public class ArchivoExcel {

        public string Numero { get; set; }
        public string Cedula { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Estado { get; set; }
        public string FechaEnvio { get; set; }
    }
}