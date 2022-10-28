using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.DeamonServicios.CaucionProduccion;

namespace SIRH.DeamonServicios
{
    public class CCaucionWinService
    {
        internal void EjecutarServicioVencimientoPolizas(CCaucionesServiceClient caucion, ServiceSIRH winservice)
        {
            CEmailHelper mensajero = new CEmailHelper();
            winservice.EscribirEntrada("Construyó el email");

            var caucionesActualizadas = caucion.ActualizarVencimientoPolizas(DateTime.Now);
            winservice.EscribirEntrada("Ejecutó el servicio");

            if (caucionesActualizadas.Count() > 0)
            {
                winservice.EscribirEntrada("Hay cauciones vencidas");
                if (caucionesActualizadas.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    winservice.EscribirEntrada("No hay errores en la búsqueda");
                    mensajero = this.EmailVencimientoPolizasEncabezado(mensajero);
                    winservice.EscribirEntrada("Construye el email");

                    foreach (var item in caucionesActualizadas)
                    {
                        this.ConstruirTablaPolizas(mensajero, item.ToList());
                    }
                    winservice.EscribirEntrada("Construye la tabla del email");

                    this.EmailPie(mensajero);
                    winservice.EscribirEntrada("Escribe el pie de pagina");
                    winservice.EscribirEntrada(mensajero.EnviarCorreo());

                    winservice.EscribirEntrada("Servicio [Vencimiento de Pólizas] ejecutado: Se ha enviado un correo electrónico a los encargados del proceso.");
                }
                else
                {
                    winservice.EscribirEntrada("No hay cauciones");
                    if (((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError.StartsWith("ATENCIÓN"))
                    {
                        winservice.EscribirEntrada("Servicio [Vencimiento de Pólizas] ejecutado: " + ((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                    else
                    {
                        winservice.EscribirEntrada("Error del método");
                        throw new Exception(((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                }
            }
            else
            {
                throw new Exception("Servicio [Vencimiento de Pólizas] con errores: No se pudo contactar al servicio");
            }
        }

        internal void EjecutarServicioPolizasPorVencer(CCaucionesServiceClient caucion, ServiceSIRH winservice)
        {
            CEmailHelper mensajero = new CEmailHelper();

            var caucionesActualizadas = caucion.PolizasPorVencer(DateTime.Now);

            if (caucionesActualizadas.Count() > 0)
            {
                if (caucionesActualizadas.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {

                    mensajero = this.EmailPolizasPorVencer(mensajero);

                    foreach (var item in caucionesActualizadas)
                    {
                        this.ConstruirTablaPolizas(mensajero, item.ToList());
                    }

                    this.EmailPie(mensajero);

                    winservice.EscribirEntrada(mensajero.EnviarCorreo());

                    winservice.EscribirEntrada("Servicio [Pólizas por Vencer] ejecutado: Se ha enviado un correo electrónico a los encargados del proceso.");
                }
                else
                {
                    if (((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError.StartsWith("ATENCIÓN"))
                    {
                        winservice.EscribirEntrada("Servicio [Pólizas por Vencer] ejecutado: " + ((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)caucionesActualizadas.FirstOrDefault().FirstOrDefault()).MensajeError);
                    }
                }
            }
            else
            {
                throw new Exception("Servicio [Pólizas por Vencer] con errores: No se pudo contactar al servicio");
            }
        }

        private CEmailHelper EmailPolizasPorVencer(CEmailHelper salida)
        {
            salida.Asunto = "Pólizas cercanas a vencimiento";
            salida.EmailBody = "Buenos días, <br><br>";
            salida.EmailBody += "Las siguientes pólizas de caución están cercanas a alcanzar su fecha de vencimiento: <br><br>";

            salida = this.EncabezadoTablaPolizas(salida);

            return salida;
        }

        private CEmailHelper EmailVencimientoPolizasEncabezado(CEmailHelper salida)
        {
            salida.Asunto = "Vencimiento de pólizas de caución";
            salida.EmailBody = "Buenos días, <br><br>";
            salida.EmailBody += "Las siguientes pólizas de caución han llegado a su fecha de vencimiento y deben renovarse: <br><br>";

            salida = this.EncabezadoTablaPolizas(salida);

            return salida;
        }

        private CEmailHelper EncabezadoTablaPolizas(CEmailHelper salida)
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

        private CEmailHelper ConstruirTablaPolizas(CEmailHelper salida, List<CBaseDTO> item)
        {
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CFuncionarioDTO)item[1]).Cedula + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CCaucionDTO)item[0]).NumeroPoliza + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:40%;'>" + ((CFuncionarioDTO)item[1]).Nombre + " "
                                + ((CFuncionarioDTO)item[1]).PrimerApellido + " "
                                + ((CFuncionarioDTO)item[1]).SegundoApellido + "</td>";
            salida.EmailBody += "<td style='padding: 5px;width:20%;'>" + ((CCaucionDTO)item[0]).FechaVence.ToShortDateString();
            salida.EmailBody += ((CCaucionDTO)item[0]).FechaVence.ToShortDateString() == DateTime.Now.ToShortDateString() ? " <b>(HOY)</b>" : " En " + Math.Ceiling((((CCaucionDTO)item[0]).FechaVence - DateTime.Now).TotalDays) + " días";
            salida.EmailBody += "</td>";
            salida.EmailBody += "</tr>";

            return salida;
        }

        private CEmailHelper EmailPie(CEmailHelper salida)
        {
            salida.EmailBody += "</table><br><br>";
            salida.EmailBody += "Para más información puede ingresar al Módulo de Administración de Cauciones <a href='http://sisrh.mopt.go.cr:84/Caucion/'>Aquí</a>";
            salida.EmailBody += "<br><br>Por favor no responder a este correo, ya que fue generado automáticamente";
            salida.EmailBody += "<br><br>Atentamente,";
            salida.EmailBody += "<br>Unidad de Informática. DGIRH.";
            salida.EmailBody += "<br>Dirección de Gestión Institucional de Recursos Humanos.";
            salida.Destinos = "deivert.guiltrichs@mopt.go.cr, noelia.sanchez@mopt.go.cr";

            return salida;
        }
    }
}
