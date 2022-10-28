using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
//using CrystalDecisions.Shared;

namespace SIRH.DeamonServicios
{
    public class CEmailHelper
    {
        public string EmailBody { get; set; }
        public string Asunto { get; set; }
        public string Destinos { get; set; }
        public bool AttachmentFlag = false;

        internal string EnviarCorreo()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                MailMessage mensaje = new MailMessage("notificaciones@mopt.go.cr", Destinos, Asunto, EmailBody);
                mensaje.IsBodyHtml = true;
                if(AttachmentFlag)
                    mensaje.Attachments.Add(new Attachment("ReporteConsolidadoDepartamentoRPT.rpt"));
                SmtpClient cliente = new SmtpClient("smtp.office365.com", 587);
                cliente.Credentials = new System.Net.NetworkCredential("notificaciones@mopt.go.cr", "Mopt2020**");
                cliente.EnableSsl = true;
                cliente.Send(mensaje);
                return "correcto";
            }
            catch(Exception error)
            {
                return error.InnerException != null ? error.InnerException.Message : error.Message;
            }
        }
    }
}
