using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.FuncionarioService;
using SIRH.Web.Models;
using SIRH.Web.AntecedentesPenalesService;
using SIRH.Web.ServiciosGeneralesService;
using System.Net.Sockets;
using System.IO;
using SIRH.Web.ViewModels;
using SIRH.DTO;
using SIRH.Web.Reports.Funcionarios;
using SIRH.Web.Reports.PDF;
using SIRH.Web.Helpers;
using System.DirectoryServices.AccountManagement;

using ExcelDataReader;
using System.Data;

namespace SIRH.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        CFuncionarioServiceClient funcionarioReference = new CFuncionarioServiceClient();
        CServiciosGeneralesServiceClient servicioReference = new CServiciosGeneralesServiceClient();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email_envio, string cedula, string usuario)
        {
            string name = "";
            try
            {
                EmailWebHelper email = new EmailWebHelper();
                email.Asunto = "Acceso al SIRH";
                email.Destinos = email_envio + ", " + "dguiltrc@mopt.go.cr, aarguelv@mopt.go.cr";
                email.EmailBody = "<div style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px;\">"+
                                  "<b> Solicitud de acceso al SIRH </b></div><br />" +
                                  "<div style=\"padding: 20px 0 30px 0; color: #153643; font-family: Arial, sans-serif; font-size: 14px; line-height: 20px;\">" +
                                  "El funcionario con cedula: <b>" + cedula +
                                  "</b> y usuario de dominio: <b>" + usuario + "</b> requiere acceso al SIRH, por favor verifique los permisos que tenía en el sistema Emulación, para grantizarle los accesos correspondientes.<br/>"
                                  + "<br/>Por favor, no responda a este correo electrónico ya que se ha generado de forma automática."
                                  + "</div>";
                var correo = email.EnviarMensajeCorreo();
                if (correo == "Enviado")
                {
                    return PartialView("_CorreoEnviado", new CBaseDTO { IdEntidad = 1 });
                }
                else
                {
                    throw new Exception("El servidor de correo no respondió a nuestra solicitud de envío. Por favor inténtelo de nuevo en unos minutos. " + correo + name);
                }
            }
            catch (Exception error)
            {
                return PartialView("_CorreoEnviado", new CBaseDTO { IdEntidad = -1, Mensaje = error.Message + name });
            }
        }

        public ActionResult Result(string query, int? page, string primera)
        {
            Session["error"] = null;
            FuncionarioModel modelo = new FuncionarioModel();
            try {
                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(query))
                {
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                    
                }

                var palabras = query.Split(' ').ToArray();

                modelo.NombreSearch = query;
                var funcionarios = funcionarioReference.BuscarFuncionarioGeneral(palabras);
                modelo.TotalFuncionarios = funcionarios.Count();
                modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                modelo.PaginaActual = paginaActual;
                Session["funcionarios"] = funcionarios.ToList();
                if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                {
                    modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                }
                else
                {
                    modelo.Funcionario = funcionarios.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                }
                Session["model"] = modelo;
                if (primera.Equals("Si"))
                {
                    return View(modelo);
                }
                else
                {
                    return PartialView("Result_Details", modelo);
                }
            }
            catch (Exception error) {
                Session["error"] = "error";
                return View("Result");
            }
            
        }

        public ActionResult About()
        {
            return View();
        }

        public bool EntidadCompleta(CBaseDTO objeto)
        {
            bool respuesta = true;
            if (!String.IsNullOrEmpty(objeto.Mensaje))
            {
                respuesta = false;
            }
            return respuesta;
        }

        private PerfilFuncionarioVM ConstruirPerfil(CBaseDTO[][] perfil, PerfilFuncionarioVM modelo)
        {
            try
            {

                if (perfil.Length < 10)
                {
                    throw new Exception("modelo");
                }
                modelo.Funcionario = EntidadCompleta(perfil[0][0]) ? (CFuncionarioDTO)perfil[0][0] : null;
                //modelo.EstadoFuncionario = EntidadCompleta(perfil[0][1]) ? (CEstadoFuncionarioDTO)perfil[0][1] : null;
                modelo.EstadoCivil = new List<CHistorialEstadoCivilDTO>();
                if (perfil[1].Count() > 0)
                {
                    foreach (var item in perfil[1])
                    {
                        modelo.EstadoCivil.Add((CHistorialEstadoCivilDTO)item);
                    }
                }
                modelo.InformacionContacto = new List<CInformacionContactoDTO>();
                if (perfil[2].Count() > 0)
                {
                    foreach (var item in perfil[2])
                    {
                        modelo.InformacionContacto.Add((CInformacionContactoDTO)item);
                    }
                }
                modelo.Direccion = EntidadCompleta(perfil[3][0]) ? (CDireccionDTO)perfil[3][0] : null;
                modelo.ProvinciaDireccion = EntidadCompleta(perfil[3][1]) ? (CProvinciaDTO)perfil[3][1] : null;
                modelo.CantonDireccion = EntidadCompleta(perfil[3][2]) ? (CCantonDTO)perfil[3][2] : null;
                modelo.DistritoDireccion = EntidadCompleta(perfil[3][3]) ? (CDistritoDTO)perfil[3][3] : null;
                modelo.DetalleContrato = EntidadCompleta(perfil[4][0]) ? (CDetalleContratacionDTO)perfil[4][0] : null;
                modelo.Nombramiento = EntidadCompleta(perfil[5][0]) ? (CNombramientoDTO)perfil[5][0] : null;
                modelo.Calificaciones = new List<CCalificacionNombramientoDTO>();
                if (perfil[6].Count() > 0)
                {
                    foreach (var item in perfil[6])
                    {
                        modelo.Calificaciones.Add((CCalificacionNombramientoDTO)item);
                    }
                }
                modelo.Puesto = EntidadCompleta(perfil[7][0]) ? (CPuestoDTO)perfil[7][0] : null;
                modelo.EstadoPuesto = EntidadCompleta(perfil[7][1]) ? (CEstadoPuestoDTO)perfil[7][1] : null;
                modelo.ClasePuesto = EntidadCompleta(perfil[7][2]) ? (CClaseDTO)perfil[7][2] : null;
                modelo.EspecialidadPuesto = EntidadCompleta(perfil[7][3]) ? (CEspecialidadDTO)perfil[7][3] : null;
                modelo.SubEspecialidadPuesto = EntidadCompleta(perfil[7][4]) ? (CSubEspecialidadDTO)perfil[7][4] : null;
                modelo.OcupacionRealPuesto = EntidadCompleta(perfil[7][5]) ? (COcupacionRealDTO)perfil[7][5] : null;
                modelo.DetallePuesto = EntidadCompleta(perfil[7][6]) ? (CDetallePuestoDTO)perfil[7][6] : null;
                modelo.PresupuestoPuesto = EntidadCompleta(perfil[8][0]) ? (CPresupuestoDTO)perfil[8][0] : null;
                modelo.ProgramaPuesto = EntidadCompleta(perfil[8][1]) ? (CProgramaDTO)perfil[8][1] : null;
                //modelo.AreaPuesto = EntidadCompleta(perfil[8][2]) ? (CAreaDTO)perfil[8][2] : null;
                //modelo.ActividadPuesto = EntidadCompleta(perfil[8][3]) ? (CActividadDTO)perfil[8][3] : null;
                modelo.DivisionPuesto = EntidadCompleta(perfil[8][2]) ? (CDivisionDTO)perfil[8][2] : null;
                modelo.DireccionGeneralPuesto = EntidadCompleta(perfil[8][3]) ? (CDireccionGeneralDTO)perfil[8][3] : null;
                modelo.DepartamentoPuesto = EntidadCompleta(perfil[8][4]) ? (CDepartamentoDTO)perfil[8][4] : null;
                modelo.SeccionPuesto = EntidadCompleta(perfil[8][5]) ? (CSeccionDTO)perfil[8][5] : null;
                if (((CTipoUbicacionDTO)perfil[9][0]).IdEntidad.Equals(1))
                {
                    modelo.TipoUbicacionContrato = EntidadCompleta(perfil[9][0]) ? (CTipoUbicacionDTO)perfil[9][0] : null;
                    modelo.ProvinciaContrato = EntidadCompleta(perfil[9][1]) ? (CProvinciaDTO)perfil[9][1] : null;
                    modelo.CantonContrato = EntidadCompleta(perfil[9][2]) ? (CCantonDTO)perfil[9][2] : null;
                    modelo.DistritoContrato = EntidadCompleta(perfil[9][3]) ? (CDistritoDTO)perfil[9][3] : null;
                    modelo.UbicacionContrato = EntidadCompleta(perfil[9][4]) ? (CUbicacionPuestoDTO)perfil[9][4] : null;
                    modelo.TipoUbicacionTrabajo = EntidadCompleta(perfil[10][0]) ? (CTipoUbicacionDTO)perfil[10][0] : null;
                    modelo.ProvinciaTrabajo = EntidadCompleta(perfil[10][1]) ? (CProvinciaDTO)perfil[10][1] : null;
                    modelo.CantonTrabajo = EntidadCompleta(perfil[10][2]) ? (CCantonDTO)perfil[10][2] : null;
                    modelo.DistritoTrabajo = EntidadCompleta(perfil[10][3]) ? (CDistritoDTO)perfil[10][3] : null;
                    modelo.UbicacionTrabajo = EntidadCompleta(perfil[10][4]) ? (CUbicacionPuestoDTO)perfil[10][4] : null;
                }
                else
                {
                    modelo.TipoUbicacionContrato = EntidadCompleta(perfil[10][0]) ? (CTipoUbicacionDTO)perfil[10][0] : null;
                    modelo.ProvinciaContrato = EntidadCompleta(perfil[10][1]) ? (CProvinciaDTO)perfil[10][1] : null;
                    modelo.CantonContrato = EntidadCompleta(perfil[10][2]) ? (CCantonDTO)perfil[10][2] : null;
                    modelo.DistritoContrato = EntidadCompleta(perfil[10][3]) ? (CDistritoDTO)perfil[10][3] : null;
                    modelo.UbicacionContrato = EntidadCompleta(perfil[10][4]) ? (CUbicacionPuestoDTO)perfil[10][4] : null;
                    modelo.TipoUbicacionTrabajo = EntidadCompleta(perfil[9][0]) ? (CTipoUbicacionDTO)perfil[9][0] : null;
                    modelo.ProvinciaTrabajo = EntidadCompleta(perfil[9][1]) ? (CProvinciaDTO)perfil[9][1] : null;
                    modelo.CantonTrabajo = EntidadCompleta(perfil[9][2]) ? (CCantonDTO)perfil[9][2] : null;
                    modelo.DistritoTrabajo = EntidadCompleta(perfil[9][3]) ? (CDistritoDTO)perfil[9][3] : null;
                    modelo.UbicacionTrabajo = EntidadCompleta(perfil[9][4]) ? (CUbicacionPuestoDTO)perfil[9][4] : null;
                }

                return modelo;
            }
            catch (Exception error)
            {
                if (error.Message == "modelo")
                    return null;
                else
                    throw new Exception(error.ToString());
            }

        }

        public ActionResult Generar()
        {
            Session["error"] = null;
            try
            {
                PerfilFuncionarioVM modelo = new PerfilFuncionarioVM();
                List<FuncionariosRptData> modelo1 = new List<FuncionariosRptData>();
                List<CFuncionarioDTO> fun = (List<CFuncionarioDTO>)Session["funcionarios"];
                foreach (var item in fun)
                {
                    var perfil = funcionarioReference.DescargarPerfilFuncionarioCompleto(item.Cedula);

                    modelo = ConstruirPerfil(perfil, modelo);
                    if (modelo == null) throw new Exception("modelo");
                    modelo1.Add(FuncionariosRptData.GenerarDatosReportePorFuncionarios(modelo));
                }

                return ReporteFuncionarios(modelo1);
            }
            catch (Exception error)
            {
                Session["error"] = "error";
                if(error.Message == "modelo")
                    ModelState.AddModelError("modelo", "No existen datos del lugar de contrato ni el lugar de trabajo.\n\n Pongase en contacto con el personal autorizado.");
                else
                    ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de generar el reporte, ponerse en contacto con el personal autorizado. \n\n");


                return View("Result");

            }
            
           
        }

        private CrystalReportPdfResult ReporteFuncionarios(List<FuncionariosRptData> modelo)
        {
            string reportPath = Path.Combine(Server.MapPath("~/Reports/Funcionarios"), "ReporteFuncionarios.rpt");
            return new CrystalReportPdfResult(reportPath, modelo,"EXCEL");
        }


        private void CargarComboMotivos(FormularioCorreoVM model)
        {
            List<SelectListItem> listadoMotivos = new List<SelectListItem>();
            SelectListItem selListItem = new SelectListItem() { Value = "1", Text = "Convocatorias a las pruebas Físicas" };
            listadoMotivos.Add(selListItem);
            selListItem = new SelectListItem() { Value = "2", Text = "Participantes no aptos (reprobaron las pruebas físicas)" };
            listadoMotivos.Add(selListItem);
            selListItem = new SelectListItem() { Value = "3", Text = "Rechazadas - Por no completar satisfactoriamente la oferta" };
            listadoMotivos.Add(selListItem);
            selListItem = new SelectListItem() { Value = "4", Text = "Rechazadas - Participantes que no cuentan con la licencia de conducir de requisito o la tienen vencida" };
            listadoMotivos.Add(selListItem);
            selListItem = new SelectListItem() { Value = "5", Text = "Rechazadas - Por no contar con registro del todo o por error del participante al ingresar datos personales)" };
            listadoMotivos.Add(selListItem);
            selListItem = new SelectListItem() { Value = "6", Text = "Convocatorias a las pruebas Psicológicas" };
            listadoMotivos.Add(selListItem);

            model.Motivos = new SelectList(listadoMotivos, "Value", "Text");
        }

        public ActionResult EnviarCorreo()
        {
            // Instalar via nugget 
            // ExcelDataReader
            // ExcelDataReader.DataSet

            FormularioCorreoVM model = new FormularioCorreoVM();
            CargarComboMotivos(model);

            model.Registros = new List<ArchivoExcel>();
            return View(model);
        }

        [HttpPost]
        public ActionResult EnviarCorreo(FormularioCorreoVM model, string SubmitButton)
        {
            try
            {
                switch (SubmitButton)
                {
                    case "Cargar Excel":

                        if (model.MotivoSeleccionado == null || model.MotivoSeleccionado == "")
                            throw new Exception("Debe seleccionar un Motivo");

                        if (model.File == null)
                            throw new Exception("Debe seleccionar el archivo");

                        // Ejemplos de acceso a datos
                        DataTable table;
                        DataRow row;

                        using (var stream = model.File.InputStream)
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                var result = reader.AsDataSet();

                                // Ejemplos de acceso a datos
                                table = result.Tables[0];
                                row = table.Rows[0];
                                string cell = row[0].ToString();
                            }
                        }

                        /////// Validar cuántos correos se enviaron el día de hoy
                        var cantidad = servicioReference.VerificarCorreosEnviados(DateTime.Now);

                        model.CantidadCorreos = Int16.Parse(cantidad.Mensaje);
                        var CantidadMax = 100 - model.CantidadCorreos;
                        if (table.Rows.Count > CantidadMax)
                            throw new Exception("Ya se enviaron hoy " + model.CantidadCorreos.ToString() + " correos. El archivo contiene más de "+ CantidadMax + " registros permitidos para enviar el día de hoy.");
                        
                        switch (model.MotivoSeleccionado)
                        {
                            case "1": // Pruebas Físicas
                                model.Registros = new List<ArchivoExcel>();
                                model.Registros = (from DataRow dr in table.Rows
                                                   select new ArchivoExcel()
                                                   {
                                                       Numero = dr[0].ToString(),
                                                       Cedula = dr[1].ToString(),
                                                       PrimerApellido = dr[2].ToString(),
                                                       SegundoApellido = dr[3].ToString(),
                                                       Nombre = dr[4].ToString(),
                                                       Correo = dr[5].ToString(),
                                                       Fecha = dr[6].ToString(),
                                                       Hora = dr[7].ToString(),
                                                       Estado = "Por enviar",
                                                       FechaEnvio = ""
                                                   }).ToList();

                                model.Registros = model.Registros.Where(Q => Q.Correo.Contains("@")).ToList();
                                break;

                            case "2":  // Participantes no aptos  
                            case "3":  // Rechazadas - Por no completar
                            case "4":  // Rechazadas - Sin Licencia
                            case "5":  // Rechazadas - Por Error de Registro
                                model.Registros = new List<ArchivoExcel>();
                                model.Registros = (from DataRow dr in table.Rows
                                                   select new ArchivoExcel()
                                                   {
                                                       Numero = dr[0].ToString(),
                                                       Cedula = dr[1].ToString(),
                                                       PrimerApellido = dr[2].ToString(),
                                                       SegundoApellido = dr[3].ToString(),
                                                       Nombre = dr[4].ToString(),
                                                       Correo = dr[5].ToString(),
                                                       Estado = "Por enviar",
                                                       FechaEnvio = ""
                                                   }).ToList();

                                model.Registros = model.Registros.Where(Q => Q.Correo.Contains("@")).ToList();
                                break;

                            case "6":  // Pruebas Psicológicas
                                model.Registros = new List<ArchivoExcel>();
                                model.Registros = (from DataRow dr in table.Rows
                                                   select new ArchivoExcel()
                                                   {
                                                       Numero = dr[0].ToString(),
                                                       Cedula = dr[1].ToString(),
                                                       PrimerApellido = dr[2].ToString(),
                                                       SegundoApellido = dr[3].ToString(),
                                                       Nombre = dr[4].ToString(),
                                                       Correo = dr[5].ToString(),
                                                       Fecha = dr[6].ToString(),
                                                       Hora = dr[7].ToString(),
                                                       Estado = "Por enviar",
                                                       FechaEnvio = ""
                                                   }).ToList();

                                model.Registros = model.Registros.Where(Q => Q.Correo.Contains("@")).ToList();


                                break;


                        }
                        model.File = null;
                        model.Registros = RetornarRespuestas(model.Registros, model.MotivoSeleccionado);
                        CargarComboMotivos(model);
                        //return PartialView("_ListCorreo", model);
                        return View(model);

                    case "Enviar Correos":
                        return null;

                    default:
                        return View(model);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Error", error.Message);
                model.Error = new CErrorDTO { MensajeError = error.Message };
                CargarComboMotivos(model);
                return View(model);
            }
        }

        public List<ArchivoExcel> RetornarRespuestas(List<ArchivoExcel> input, string tipo)
        {
            List<CTemp_EnviarCorreoDTO> correos = new List<CTemp_EnviarCorreoDTO>();
            foreach (var item in input)
            {
                EmailWebHelper correo = new EmailWebHelper();
                var resultado = correo.EnviarCorreoSendGrid(item.Correo, item.Nombre, item.Cedula, item.Fecha, item.Hora, item.PrimerApellido, item.SegundoApellido, tipo);
                item.Estado = resultado == true ? "Enviado" : "Error en el envío";
                correos.Add(new CTemp_EnviarCorreoDTO
                {
                    Cedula = item.Cedula,
                    Correo = item.Correo,
                    FechaEnvio = Convert.ToDateTime(item.FechaEnvio),
                    IdMotivo = item.Estado == "Enviado" ? Convert.ToInt32(tipo) : 7,
                    Nombre = item.Nombre
                });
            }

            servicioReference.GuardarEnvioCorreos(correos.ToArray());
            return input;
        }
    }
}
