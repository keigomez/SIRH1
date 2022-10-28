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
    public class FormularioCatalogoDiasVM
    {
        public List<CCatalogoDiaDTO> CatalogoDias { get; set; }
        public CErrorDTO Error { get; set; }
        public Boolean esFeriado { get; set; }
    }
}