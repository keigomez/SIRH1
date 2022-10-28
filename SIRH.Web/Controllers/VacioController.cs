using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ServicioGeneral;
using SIRH.Web.Helpers;
using SIRH.Web.CaucionesLocal;
using SIRH.DTO;

namespace SIRH.Web.Controllers
{
    public class VacioController : Controller
    {
        CCaucionesServiceClient caucion = new CCaucionesServiceClient();
        // GET: Vacio
        public ActionResult Index()
        {
            //EmailWebHelper mensajero = new EmailWebHelper();

            //var caucionesActualizadas = caucion.PolizasPorVencer(new DateTime(2021,10,1));

            //if (caucionesActualizadas.Count() > 0)
            //{
            //    if (caucionesActualizadas.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
            //    {

            //        mensajero = this.EmailPolizasPorVencer(mensajero);

            //        foreach (var item in caucionesActualizadas)
            //        {
            //            this.ConstruirTablaPolizas(mensajero, item.ToList());
            //        }

            //        this.EmailPie(mensajero);

            //        mensajero.EnviarCorreo();
            //    }
            //    else
            //    {
            //        if (((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError.StartsWith("ATENCIÓN"))
            //        {
                        
            //        }
            //        else
            //        {
            //            throw new Exception(((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError);
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception("Servicio [Pólizas por Vencer] con errores: No se pudo contactar al servicio");
            //}
            return View();
        }

        private EmailWebHelper EmailPolizasPorVencer(EmailWebHelper salida)
        {
            salida.Asunto = "Pólizas con vencimiento - Octubre";
            salida.EmailBody = "Buenos días, <br><br>";
            salida.EmailBody += "Las siguientes pólizas de caución están cercanas a alcanzar su fecha de vencimiento: <br><br>";

            salida = this.EncabezadoTablaPolizas(salida);

            return salida;
        }

        private EmailWebHelper EmailVencimientoPolizasEncabezado(EmailWebHelper salida)
        {
            salida.Asunto = "Vencimiento de pólizas de caución";
            salida.EmailBody = "Buenos días, <br><br>";
            salida.EmailBody += "Las siguientes pólizas de caución han llegado a su fecha de vencimiento y deben renovarse: <br><br>";

            salida = this.EncabezadoTablaPolizas(salida);

            return salida;
        }

        private EmailWebHelper EncabezadoTablaPolizas(EmailWebHelper salida)
        {
            salida.EmailBody += "<table style='  border: solid 1px #e8eef4;border-collapse: collapse;'>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Cédula</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'># Póliza</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Nombre</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Fecha de Vencimiento</th>";
            salida.EmailBody += "</tr>";

            return salida;
        }

        private EmailWebHelper ConstruirTablaPolizas(EmailWebHelper salida, List<CBaseDTO> item)
        {
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CFuncionarioDTO)item[1]).Cedula + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CCaucionDTO)item[0]).NumeroPoliza + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:40%;'>" + ((CFuncionarioDTO)item[1]).Nombre + " "
                                + ((CFuncionarioDTO)item[1]).PrimerApellido + " "
                                + ((CFuncionarioDTO)item[1]).SegundoApellido + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CCaucionDTO)item[0]).FechaVence.ToShortDateString();
            //salida.EmailBody += ((CCaucionDTO)item[0]).FechaVence.ToShortDateString() /*== DateTime.Now.ToShortDateString() ? " <b>(HOY)</b>" : " En " + Math.Ceiling((((CCaucionDTO)item[0]).FechaVence - DateTime.Now).TotalDays) + " días"*/;
            salida.EmailBody += "</td>";
            salida.EmailBody += "</tr>";

            return salida;
        }

        private EmailWebHelper EmailPie(EmailWebHelper salida)
        {
            salida.EmailBody += "</table><br><br>";
            salida.EmailBody += "Para más información puede ingresar al Módulo de Administración de Cauciones en la ubicación: http://sisrh.mopt.go.cr:84/Caucion/";
            salida.EmailBody += "<br><br>Por favor no responder a este correo, ya que fue generado automáticamente";
            salida.EmailBody += "<br><br>Atentamente,";
            salida.EmailBody += "<br>Unidad de Informática. DGIRH.";
            salida.EmailBody += "<br>Dirección de Gestión Institucional de Recursos Humanos.";
            salida.Destinos = "deivert.guiltrichs@mopt.go.cr, noelia.sanchez@mopt.go.cr";//

            return salida;
        }
    }
}