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
    public class BusquedaHistoricoCarreraVM
    {

        public CCarreraProfesionalDTO Carrera { get; set; }

        [DisplayName("Fecha desde")]
        public DateTime FechaDesde { get; set; }

        [DisplayName("Fecha hasta")]
        public DateTime FechaHasta { get; set; }

        [DisplayName("Curso")]
        public string Curso { get; set; }

        public CErrorDTO Error { get; set; }
    }
}