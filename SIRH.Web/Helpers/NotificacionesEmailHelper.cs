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
using System.Net.Mail;

namespace SIRH.Web.Helpers
{
    public static class NotificacionesEmailHelper
    {
        public static void SendEmail(string to, string subject, string body)
        {
            MailMessage message = new MailMessage("SIRH@mopt.go.cr", to, subject, body);
            SmtpClient smtp = new SmtpClient("MOPTS101M1");
            message.IsBodyHtml = true;
            smtp.Send(message);
        }

        public static string DeterminarModulo(int modulo)
        {
            switch (modulo)
            {
                case 4:
                    return "Módulo de cauciones";
                default:
                    return "SIRH";
            }
        }
    }
}
