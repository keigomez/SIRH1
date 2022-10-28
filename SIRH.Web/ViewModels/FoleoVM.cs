using System;
using System.Collections.Generic;
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

namespace SIRH.Web.ViewModels
{
    public class FoleoVM
    {
        public CExpedienteFuncionarioDTO expediente { get; set; }

        [DisplayName("Cédula Usuario")]
        public string CedulaABuscar { get; set; }

        public CErrorDTO Error { get; set; }
    }
}