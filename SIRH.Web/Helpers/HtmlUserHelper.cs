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
using System.Web.Mvc;
//using System.DirectoryServices.AccountManagement;
using System.Threading;
using System.Security.Principal;

namespace SIRH.Web.Helpers
{
    public static class HtmlUserHelper
    {
        public static MvcHtmlString UserId(this HtmlHelper htmlHelper, string username)
        {
            return MvcHtmlString.Create(HttpContext.Current.User.Identity.Name.ToString());
        }
    }
}
