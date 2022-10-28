using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.DeamonServicios.DesarraigoService;
//using SIRH.DeamonServicios.CaucionService;

namespace SIRH.DeamonServicios
{
    class CDesarraigoWinService
    {
        internal void EjecutarServicioVencimientoDesarraigo(CDesarraigoServiceClient desarraigo, ServiceSIRH winservice)
        {
            CEmailHelper mensajero = new CEmailHelper();
            winservice.EscribirEntrada("Construyó el email");
            var desarraigoActulizado = desarraigo.ActualizarVencimientoDesarraigo(DateTime.Now);
            winservice.EscribirEntrada("Ejecutó el servicio");
            if (desarraigoActulizado.Count() > 0)
            {
                winservice.EscribirEntrada("Hay desarraigos vencidos");
                if (desarraigoActulizado.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    winservice.EscribirEntrada("No hay errores en la búsqueda");
                    mensajero = this.EmailVencimientoDesarraigosEncabezado(mensajero);
                    winservice.EscribirEntrada("Construye el email");

                    foreach (var item in desarraigoActulizado)
                    {
                        this.ConstruirTablaDesarraigo(mensajero, item.ToList());
                        winservice.EscribirEntrada("Se construyó tabla");
                    }

                    this.EmailPie(mensajero);

                    mensajero.EnviarCorreo();

                    winservice.EscribirEntrada("Servicio [Vencimiento de Desarraigos] ejecutado: Se ha enviado un correo electrónico a los encargados del proceso.");
                }
                else
                {
                    winservice.EscribirEntrada("No entró al if");
                    if (((CErrorDTO)desarraigoActulizado.FirstOrDefault().FirstOrDefault()).MensajeError.StartsWith("ATENCIÓN"))
                    {
                        winservice.EscribirEntrada("Servicio [Vencimiento de Desarraigo] ejecutado: " + ((CErrorDTO)desarraigoActulizado.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)desarraigoActulizado.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                }
            }
            else
            {
                throw new Exception("No se pudo contactar al servicio");
            }
        }

        internal void EjecutarServicioDesarraigosPorVencer(CDesarraigoServiceClient desarraigo, ServiceSIRH winservice)
        {
            CEmailHelper mensajero = new CEmailHelper();

            var desarraigoActulizado = desarraigo.DesarraigosPorVencer(DateTime.Now);

            if (desarraigoActulizado.Count() > 0)
            {
                if (desarraigoActulizado.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {

                    mensajero = this.EmailDesarraigosPorVencer(mensajero);

                    foreach (var item in desarraigoActulizado)
                    {
                        this.ConstruirTablaDesarraigo(mensajero, item.ToList());
                    }

                    this.EmailPie(mensajero);

                    mensajero.EnviarCorreo();

                    winservice.EscribirEntrada("Servicio [Pólizas por Vencer] ejecutado: Se ha enviado un correo electrónico a los encargados del proceso.");
                }
                else
                {
                    if (((CErrorDTO)desarraigoActulizado.FirstOrDefault().FirstOrDefault()).MensajeError.StartsWith("ATENCIÓN"))
                    {
                        winservice.EscribirEntrada("Servicio ejecutado: " + ((CErrorDTO)desarraigoActulizado.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)desarraigoActulizado.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                }
            }
            else
            {
                throw new Exception("No se pudo contactar al servicio");
            }
        }

        private CEmailHelper EmailDesarraigosPorVencer(CEmailHelper salida)
        {
            salida.Asunto = "Desarraigos cercanas a vencimiento";
            salida.EmailBody = "Buenos días, <br><br>";
            salida.EmailBody += "Los siguientes tramites de desarraigo están cercanas a alcanzar su fecha de vencimiento: <br><br>";
            return  this.EncabezadoTablaDesarraigosPorVencer(salida);

        }

        private CEmailHelper EmailVencimientoDesarraigosEncabezado(CEmailHelper salida)
        {
            salida.Asunto = "Vencimiento de Desarraigos";
            salida.EmailBody = "Buenos días, <br><br>";
            salida.EmailBody += "Los siguientes tramites de desarraigo han vencido y deben renovarse: <br><br>";

            salida = this.EncabezadoTablaDesarraigosVencidos(salida);

            return salida;
        }

        private CEmailHelper EncabezadoTablaDesarraigosPorVencer(CEmailHelper salida)
        {
            salida.EmailBody += "<table style='  border: solid 1px #e8eef4;border-collapse: collapse;'>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Cédula</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'># Desarraigo</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Nombre</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Fecha de Vencimiento</th>";
            salida.EmailBody += "</tr>";

            return salida;
        }
        private CEmailHelper EncabezadoTablaDesarraigosVencidos(CEmailHelper salida)
        {
            salida.EmailBody += "<table style='  border: solid 2px #e8eef4;border-collapse: collapse;'>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Cédula</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'># Desarraigo</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Nombre</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Fecha de Vencimiento</th>";
            salida.EmailBody += "<th style='padding: 6px 5px;text-align: left;background-color: #e8eef4; border: solid 1px #e8eef4;'>Motivo de vencimiento</th>";
            salida.EmailBody += "</tr>";

            return salida;
        }

        private CEmailHelper ConstruirTablaDesarraigo(CEmailHelper salida, List<CBaseDTO> item)
        {           
            var estadoDEsarraigo = ((CDesarraigoDTO)item[0]).EstadoDesarraigo.IdEntidad;
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CFuncionarioDTO)item[1]).Cedula + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CDesarraigoDTO)item[0]).CodigoDesarraigo + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:40%;'>" + ((CFuncionarioDTO)item[1]).Nombre + " "
                                + ((CFuncionarioDTO)item[1]).PrimerApellido + " "
                                + ((CFuncionarioDTO)item[1]).SegundoApellido + "</td>";
            if (estadoDEsarraigo == 5 || estadoDEsarraigo == 6 || estadoDEsarraigo == 7 || estadoDEsarraigo==1 || estadoDEsarraigo==2)
            {
                salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CDesarraigoDTO)item[0]).FechaFin.ToShortDateString() + " (En " + Math.Ceiling((((CDesarraigoDTO)item[0]).FechaFin - DateTime.Now).TotalDays) + " días)";
            }
            else {
                salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CDesarraigoDTO)item[0]).FechaFin.ToShortDateString();
            }
            salida.EmailBody += "</td>";
            if (estadoDEsarraigo == 4)
            {
                salida.EmailBody += "<td style='padding: 5px;width:20%;'> Ha llegado a su fecha de vencimiento </td>";
            }
            if (estadoDEsarraigo == 5)
            {
                salida.EmailBody += "<td style='padding: 5px;width:20%;'> Ha excedido los 30 dias de permiso sin salario</td>";
            }
            if (estadoDEsarraigo == 6)
            {
                salida.EmailBody += "<td style='padding: 5px;width:20%;'> Ha excedido los 30 dias de vacaciones </td>";
            }
            if (estadoDEsarraigo == 7)
            {
                salida.EmailBody += "<td style='padding: 5px;width:20%;'> Ha excedido los 30 dias de incapacidad </td>";
            }
            salida.EmailBody += "</tr>";

            return salida;
        }


        private CEmailHelper EmailPie(CEmailHelper salida)
        {
            salida.EmailBody += "</table><br><br>";
            salida.EmailBody += "Para más información puede ingresar al Módulo de Administración de Desarraigos en la ubicación: http://sisrh.mopt.go.cr:82/Desarraigo/";
            salida.EmailBody += "<br><br>Por favor no responder a este correo, ya que fue generado automáticamente";
            salida.EmailBody += "<br><br>Atentamente,";
            salida.EmailBody += "<br>Unidad de Informática. DGIRH.";
            salida.EmailBody += "<br>Dirección de Gestión Institucional de Recursos Humanos.";
            //salida.Destinos = "dguiltrc@mopt.go.cr, dgrangeg@mopt.go.cr, vchavesc@mopt.go.cr";
            salida.Destinos = "maryalvarezhernandez@hotmail.es";

            return salida;
        }

    }
}
