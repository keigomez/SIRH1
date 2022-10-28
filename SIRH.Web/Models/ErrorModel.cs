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

namespace SIRH.Web.Models
{
    public class ErrorModel
    {
        public string Mensaje { get; set; }
        public string Modulo { get; set; }
    }
}
