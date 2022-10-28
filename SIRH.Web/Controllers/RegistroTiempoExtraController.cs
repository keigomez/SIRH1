using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
//using SIRH.Web.FuncionarioLocal;
using SIRH.Web.FuncionarioService;
//using SIRH.Web.RegistroTELocal;
using SIRH.Web.RegistroTEService;
using SIRH.DTO;
using SIRH.Web.Helpers;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Reports.RegistroTiempoExtra;
using System.Security.Cryptography;
using System.Security.Principal;
using SIRH.Web.UserValidation;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;

namespace SIRH.Web.Controllers
{


    public class RegistroTiempoExtraController : Controller
    {
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CRegistroTEServiceClient servicioTiempoExtra = new CRegistroTEServiceClient();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        private readonly string DES_CLASE = "OFIC.SEGUR.SERV.CIVIL";
        private readonly int MAX_SIZE = 20 * 1024 * 1024;
        CAccesoWeb context = new CAccesoWeb();
        //
        // GET: /RegistroTiempoExtra/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /RegistroTiempoExtra/Create

        public ActionResult Create()
        {
            FuncionarioRegistroExtrasVM reg = new FuncionarioRegistroExtrasVM();
            int meses = DateTime.Now.Month;
            if (meses == 1)
            {
                reg.ListaMeses = new SelectList(RegistroTiempoExtraHelper.DeterminarMeses(meses+3));
            }
            else
            {
                reg.ListaMeses = new SelectList(RegistroTiempoExtraHelper.DeterminarMeses(DateTime.Now.Month-1));
            }
            reg.MesActual = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            reg.RegistroTiempoExtra = new SIRH.DTO.CRegistroTiempoExtraDTO();
            reg.RegistroTiempoExtra.FechaEmision = DateTime.Now;
            return View(reg);
        }

        //
        // POST: /RegistroTiempoExtra/Create
        [HttpPost]
        public ActionResult Create(FuncionarioRegistroExtrasVM model, string submit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }
                if (model.MesActual == null)
                {
                    ModelState.AddModelError("Error", "El periodo es requerido");
                    throw new Exception();
                }
                //ModelState es valido y mesActual es distinto de null
                if (model.DetalleExtras == null || submit == "Descartar Cambios")
                {
                    return MostrarInfoFuncionario(model, ObtenerClases(), ObtenerPresupuestos());
                }
                else
                {
                    model.ListaClases = new SelectList(ObtenerClases(), "Value", "Text", model.ClaseActual ?? "");
                    model.ListaPresupuesto = new SelectList(ObtenerPresupuestos(), "Value", "Text", model.PresupuestoSeleccionado ?? "");
                    if (submit == "Calcular")
                    {
                        model.MesActualPago = RegistroTiempoExtraHelper.CalcularPeriodoPago();
                        model.RegistroTiempoExtra.FechaEmision = DateTime.Now;
                        model.H0 = new List<decimal>();
                        model.H1 = new List<decimal>();
                        model.H2 = new List<decimal>();

                        if (RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) && !model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE) && model.ClaseActual == null)
                        {
                            CalcularAdministrativo(model); //CALCULO DE TIEMPO EXTRA PARA ADMINISTRATIVOS
                        }
                        else
                        {
                            CalcularGuardaVista(model,"");//CALCULO DE TIEMPO PARA OFICIALES DE SEGURIDAD
                        }
                        ErroresCalculo("1", "La hora de inicio no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("2", "La hora de fin no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("3", "La hora de inicio debe ir de las 0 a las 23 horas y el minuto de inicio de 0 a 59 minutos, para las siguientes fechas:");
                        ErroresCalculo("4", "La hora de fin debe ir de las 0 a las 23 horas y el minuto de fin de 0 a 59 minutos, para las siguientes fechas:");
                        ErroresCalculo("J", "La jornada laborada no ha sido escogida, para las siguientes fechas:");
                        ErroresCalculo("H", "Debe digitar la jornada completa junto con las extras, para las siguientes fechas:");
                        ErroresCalculo("F", "La fecha de inicio debe ser menor a la fecha de vencimineto del nombramiento, para las siguientes fechas:");
                        ErroresCalculo("C", "Debe registrar al menos una hora completa antes de registrar fracciones de hora, para las siguientes fechas:");
                        ErroresCalculo("V", "No se puede registrar extras, ya que el funcionario se encontraba en vacaciones, para las siguientes fechas:");
                        ErroresCalculo("I", "No se puede registrar extras, ya que el funcionario se encontraba incapacitado, para las siguientes fechas:");
                        return PartialView("_DetalleExtras", model);
                    }
                    else
                    {
                        //REGISTRO DE TIEMPO EXTRAORDINARIO//
                        if (model.RegistroTiempoExtra.Area == null || model.RegistroTiempoExtra.Area == "")
                        {
                            ModelState.AddModelError("Error", "Debe digitar el área");
                            throw new Exception();
                        }
                        if (model.RegistroTiempoExtra.Actividad == null || model.RegistroTiempoExtra.Actividad == "")
                        {
                            ModelState.AddModelError("Error", "Debe digitar la actividad");
                            throw new Exception();
                        }
                        if (model.RegistroTiempoExtra.Justificacion == null || model.RegistroTiempoExtra.Justificacion == "")
                        {
                            ModelState.AddModelError("Error", "Debe digitar una justificación del registro de tiempo extraordinario");
                            throw new Exception();
                        }
                        model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
                        List<CDetalleTiempoExtraDTO> detalles = QuitarDetallesVacios(model.DetalleExtras);
                        if (detalles.Count() == 0)
                        {
                            throw new Exception("Debe registrar tiempo extra al menos para un día");
                        }
                        if (model.File != null)
                        {
                            if (model.File.ContentLength > MAX_SIZE)
                            {
                                throw new Exception("El archivo supera el peso máximo de 20 MB");
                            }
                            Stream str = model.File.InputStream;
                            BinaryReader Br = new BinaryReader(str);
                            byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                            model.RegistroTiempoExtra.Archivo = FileDet;
                        }
                        //model.RegistroTiempoExtra.Archivo = null;
                        model.RegistroTiempoExtra.Periodo = model.MesActual;
                        model.RegistroTiempoExtra.QuincenaA = new CDesgloseSalarialDTO { IdEntidad = model.Desglose1.IdEntidad };
                        model.RegistroTiempoExtra.QuincenaB = new CDesgloseSalarialDTO { IdEntidad = model.Desglose2.IdEntidad };
                        model.RegistroTiempoExtra.FecPago = RegistroTiempoExtraHelper.ConvertirFechaPago(model.MesActualPago);
                        model.RegistroTiempoExtra.Estado = EstadoExtraEnum.Activo;
                        if (model.ClaseActual != null)
                        {
                            model.RegistroTiempoExtra.Clase = new CClaseDTO { IdEntidad = Convert.ToInt32(model.ClaseActual) };
                        } else if (model.DetallePuesto.Clase != null)
                        {
                            model.RegistroTiempoExtra.Clase = new CClaseDTO { IdEntidad = model.DetallePuesto.Clase.IdEntidad };
                        }
                        model.RegistroTiempoExtra.Presupuesto = model.PresupuestoSeleccionado != null ? new CPresupuestoDTO { IdEntidad = Convert.ToInt32(model.PresupuestoSeleccionado) } : null;
                        model.RegistroTiempoExtra.Detalles = null;
                        //model.RegistroTiempoExtra.Justificacion = model.RegistroTiempoExtra.Justificacion != null ? model.RegistroTiempoExtra.Justificacion + "<" + model.Area + "<" + model.Actividad : " " + "<" + model.Area + "<" + model.Actividad;
                        model.Funcionario.Sexo = GeneroEnum.Indefinido;
                        foreach (var detalle in detalles) {
                            detalle.TipoExtra = new CTipoExtraDTO { IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtra(detalle, !RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) || model.ClaseActual != null || model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE)) };
                            detalle.Estado = EstadoDetalleExtraEnum.Activo;
                        }
                        CRespuestaDTO respuesta = servicioTiempoExtra.RegistrarTiempoExtra(model.Funcionario.Cedula, model.RegistroTiempoExtra, detalles.ToArray());

                        if (respuesta.Codigo > 0)
                        {
                            string envia = principal.Identity.Name.Split('\\')[1];
                            var correo = new EmailWebHelper
                            {
                                Asunto = "SIRH - Tiempo Extraordinario",
                                Destinos = "elisa.robles@mopt.go.cr, grettel.urena@mopt.go.cr" + "," + envia + "@mopt.go.cr",
                                EmailBody = "Estimados(as) usuarios, <br/><br/> El encargado administrativo: <b>" + principal.Identity.Name + "</b> ha generado un nuevo registro de tiempo extra para el funcionario <b>" + model.Funcionario.Cedula + " - " + model.Funcionario.Nombre + " " + model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido + "</b> para el periodo <b>" + model.RegistroTiempoExtra.Periodo + "</b><br/> Diríjase al módulo de tiempo extraordinario en el SIRH para verificar el dato. <br/><br/> Puede encontrar el detalle de lo ingresado <a href='http://sisrh.mopt.go.cr:84/RegistroTiempoExtra/Details/" + respuesta.IdEntidad + "?fechaRegistro=01%2F01%2F0001%2000%3A00%3A00&doble=Tiempo Extraordinario'>AQUÍ</a> <br/><br/> Saludos."
                            };

                            var enviado = correo.EnviarCorreo();

                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.TiempoExtra),
                                Convert.ToInt32(EAccionesBitacora.Guardar), respuesta.IdEntidad,
                                CAccesoWeb.ListarEntidades(typeof(CRegistroTiempoExtraDTO).Name));

                            return RedirectToAction("Saved", new { cedula = model.Funcionario.Cedula, periodo = model.RegistroTiempoExtra.Periodo, mensaje = respuesta.Mensaje, codigo = respuesta.Codigo });
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)respuesta.Contenido).Mensaje);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                if (model.DetalleExtras == null)
                {
                    return PartialView("_ErrorBusqueda");
                }
                if (ex.Message != null)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
                return PartialView("_DetalleExtras", model);
            }
        }

        //
        // GET: /RegistroTiempoExtra/CreateDoble
        public ActionResult CreateDoble()
        {
            FuncionarioRegistroExtrasVM model = new FuncionarioRegistroExtrasVM();
            model.ListaMeses = new SelectList(RegistroTiempoExtraHelper.DeterminarMeses(6));
            //reg.MesActual = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            model.RegistroTiempoExtra = new SIRH.DTO.CRegistroTiempoExtraDTO();
            return View(model);
        }

        //
        // POST: /RegistroTiempoExtra/CreateDoble
        [HttpPost]
        public ActionResult CreateDoble(FuncionarioRegistroExtrasVM model, string submit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }
                if (model.MesActual == null)
                {
                    ModelState.AddModelError("Error", "El periodo es requerido");
                    throw new Exception();
                }
                //ModelState es valido y mesActual es distinto de null
                if (model.DetalleExtras == null)
                {
                    return MostrarInfoJornadaDoble(model);
                }
                else
                {
                    if (submit == "+")
                    {
                        model.DetalleExtras.Add(new CDetalleTiempoExtraDTO { FechaInicioDoble = Convert.ToDateTime(model.FechaMin)});
                        return PartialView("_DetalleExtrasDoble", model);
                    }
                    else if (submit == "-")
                    {
                        if (model.DetalleExtras.Count > 1)
                        {
                            model.DetalleExtras.RemoveAt(model.DetalleExtras.Count - 1);
                        }
                        return PartialView("_DetalleExtrasDoble", model);
                    }
                    else if (submit == "Calcular")
                    {
                        model.MesActualPago = RegistroTiempoExtraHelper.CalcularPeriodoPago();
                        model.RegistroTiempoExtra.FechaEmision = DateTime.Now;
                        model.H0 = new List<decimal>();
                        model.H1 = new List<decimal>();
                        model.H2 = new List<decimal>();
                        foreach(var detalle in model.DetalleExtras)
                        {
                            detalle.FechaInicio = detalle.FechaInicioDoble != null? Convert.ToDateTime(detalle.FechaInicioDoble) : DateTime.MinValue;
                        }
                        CalcularGuardaDobleVista(model);//CALCULO DE TIEMPO PARA OFICIALES DE SEGURIDAD

                        ErroresCalculo("N", "La fila se encuentra vacia, para las siguientes fechas:");
                        ErroresCalculo("X", "Las jornadas dobles ingresadas deben coincidir con una fecha del registro de tiempo extraordinario, para las siguientes fechas:");
                        ErroresCalculo("1", "La hora de inicio no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("2", "La hora de fin no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("3", "La hora de inicio debe ir de las 0 a las 23 horas y el minuto de inicio de 0 a 59 minutos, para las siguientes fechas:");
                        ErroresCalculo("4", "La hora de fin debe ir de las 0 a las 23 horas y el minuto de fin de 0 a 59 minutos, para las siguientes fechas:");
                        ErroresCalculo("5", "La hora H0 no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("6", "La hora H1 no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("7", "La hora H2 no ha sido digitada o carece de datos, para las siguientes fechas:");
                        ErroresCalculo("8", "El total de horas extras debe ser mayor a 0, para las siguientes fechas:");
                        ErroresCalculo("9", "No se puede registrar extras en las 3 casillas en una misma jornada, para las siguientes fechas:");
                        ErroresCalculo("T", "No se puede registrar extras para las casilla H1 y H2 en una misma jornada, para las siguientes fechas:");
                        ErroresCalculo("J", "La jornada laborada no ha sido escogida, para las siguientes fechas:");
                        ErroresCalculo("F", "La fecha de inicio debe ser menor a la fecha de vencimineto del nombramiento, para las siguientes fechas:");
                        ErroresCalculo("C", "Debe registrar al menos una hora completa antes de registrar fracciones de hora, para las siguientes fechas:");
                        ErroresCalculo("V", "No se puede registrar extras, ya que el funcionario se encontraba en vacaciones, para las siguientes fechas:");
                        ErroresCalculo("I", "No se puede registrar extras, ya que el funcionario se encontraba incapacitado, para las siguientes fechas:");
                        return PartialView("_DetalleExtrasDoble", model);
                    }
                    else
                    {
                        //REGISTRO DE TIEMPO EXTRAORDINARIO//
                        List<CDetalleTiempoExtraDTO> detalles = QuitarDetallesVacios(model.DetalleExtras, true);
                        if (detalles.Count() == 0)
                        {
                            throw new Exception("Debe registrar tiempo extra al menos para un día");
                        }
                        foreach(var detalle in model.DetalleExtras)
                        {
                            detalle.Estado = EstadoDetalleExtraEnum.Activo;
                            detalle.FechaInicio = Convert.ToDateTime(detalle.FechaInicioDoble);
                            detalle.TipoExtra = new CTipoExtraDTO { IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtraDoble(detalle) };
                        }
                        CRespuestaDTO respuesta = servicioTiempoExtra.RegistrarTiempoExtraDoble(model.RegistroTiempoExtra.IdEntidad.ToString(), detalles.ToArray());
                        if (respuesta.Codigo > 0)
                        {
                            var correo = new EmailWebHelper
                            {
                                Asunto = "SIRH - Tiempo Extraordinario",
                                Destinos = "elisa.robles@mopt.go.cr, grettel.urena@mopt.go.cr" + "," + principal.Identity.Name + "@mopt.go.cr",
                                EmailBody = "Estimados(as) usuarios, <br/> El encargado administrativo: <b>" + principal.Identity.Name + "</b> ha generado un nuevo registro de jornadas dobles de tiempo extra para el funcionario <b>" + model.Funcionario.Cedula + " - " + model.Funcionario.Nombre + " " + model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido + "</b> para el periodo <b>" + model.RegistroTiempoExtra.Periodo + "</b><br/> Diríjase al módulo de tiempo extraordinario en el SIRH para verificar el dato. <br/> Saludos."
                            };

                            var enviado = correo.EnviarCorreo();

                            return RedirectToAction("Saved", "RegistroTiempoExtra", new { cedula = model.Funcionario.Cedula, periodo = model.RegistroTiempoExtra.Periodo, mensaje = respuesta.Mensaje, codigo = respuesta.Codigo, doble = true });
                        }
                        else
                        {
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (model.DetalleExtras == null)
                {
                    return PartialView("_ErrorBusqueda");
                }
                if (ex.Message != null)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
                return PartialView("_DetalleExtrasDoble", model);
            }
        }

        private ActionResult MostrarInfoFuncionario(FuncionarioRegistroExtrasVM model, System.Collections.IEnumerable clases, System.Collections.IEnumerable presupuestos)
        {
            try
            {
                string[] mesAnio = model.MesActual.Split(' ');
                int mes = DateTime.ParseExact(mesAnio[0].ToLower(), "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
                int annio = mesAnio.Count() > 1 ? Convert.ToInt32(mesAnio[1]) : DateTime.Now.Year;
                DateTime fecEmision = model.RegistroTiempoExtra.FechaEmision;
                var lista = servicioFuncionario.BuscarFuncionarioDesgloceSalarial(model.Funcionario.Cedula, model.MesActual);
                var registro = servicioTiempoExtra.BuscarRegistroTiempoExtra(model.Funcionario.Cedula, model.MesActual);
                if (lista[0].GetType() == typeof(CErrorDTO))
                {
                    throw new Exception(((CErrorDTO)lista[0]).MensajeError);
                }
                model.Funcionario = (CFuncionarioDTO)lista[0];
                model.Puesto = (CPuestoDTO)lista[1];
                model.DetallePuesto = (CDetallePuestoDTO)lista[2];
                model.Desglose1 = (CDesgloseSalarialDTO)lista[3];
                model.Desglose2 = (CDesgloseSalarialDTO)lista[4];
                model.Nombramineto = (CNombramientoDTO)lista[5];
                if(registro.Codigo == -2)
                {
                    throw new Exception(((CErrorDTO)registro.Contenido).MensajeError);
                }
                if (registro.Codigo > 0)
                {
                    model.YaExiste = true;
                    model.RegistroTiempoExtra = (CRegistroTiempoExtraDTO)registro.Contenido;
                    model.DetalleExtras = model.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
                    model.DetalleExtras = CompletarDetalles(model.DetalleExtras, annio, mes);
                    model.MesActual = mesAnio[0] + " " + mesAnio[1];
                    model.MesActualPago = RegistroTiempoExtraHelper.CalcularPeriodoPagoRegistrado(model.RegistroTiempoExtra.FecPago);
                    //if (model.RegistroTiempoExtra.Clase.Mensaje != null && model.RegistroTiempoExtra.Clase.Mensaje != "")
                    //    model.ClaseActual = null;
                    //if (model.RegistroTiempoExtra.Presupuesto.Mensaje != null && model.RegistroTiempoExtra.Presupuesto.Mensaje != "")
                    //    model.RegistroTiempoExtra.Presupuesto = null;
                    for(int i = 0; i < model.DetalleExtras.Count; i++)
                    {
                        if (model.DetalleExtras[i].TipoExtra != null)
                        {
                            model.DetalleExtras[i].Jornada = RegistroTiempoExtraHelper.DefinirJornada(model.DetalleExtras[i].TipoExtra.IdEntidad);
                            List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(model.DetalleExtras[i], model.DetalleExtras[i].TipoExtra.DesTipExtra.Contains("doble"));
                            model.DetalleExtras[i].FechaInicioEspecial = libres[0];
                            model.DetalleExtras[i].FechaFinalEspecial = libres[1];
                        }
                    }
                    model.H0 = new List<decimal>();
                    model.H1 = new List<decimal>();
                    model.H2 = new List<decimal>();
                    if (!RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) ||
                        model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE) ||
                        (model.RegistroTiempoExtra.Clase?.DesClase != null && model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
                    {
                        CalcularGuardaVista(model,"");
                    }
                    else
                    {
                        CalcularAdministrativo(model);
                    }
                    model.HayArchivo = model.RegistroTiempoExtra.Archivo != null;
                }
                else
                {
                    model.YaExiste = false;
                    model.HayArchivo = false;
                    model.RegistroTiempoExtra = new CRegistroTiempoExtraDTO();
                    model.DetalleExtras = new List<SIRH.DTO.CDetalleTiempoExtraDTO>();
                    for (int i = 1; i <= DateTime.DaysInMonth(annio, mes); i++)
                    {
                        model.DetalleExtras.Add(new SIRH.DTO.CDetalleTiempoExtraDTO()
                        {
                            FechaInicio = new DateTime(annio, mes, i),
                        });
                    }
                    model.MesActualPago = RegistroTiempoExtraHelper.CalcularPeriodoPago();
                    model.RegistroTiempoExtra.FechaEmision = fecEmision;
                    
                }
                model.RegistroTiempoExtra.MontoDiurna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240;
                model.RegistroTiempoExtra.MontoMixta = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 210;
                model.RegistroTiempoExtra.MontoNocturna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 180;
                if (model.RegistroTiempoExtra.Clase != null && model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE))
                {
                    model.ListaClases = new SelectList(clases, "Value", "Text", model.RegistroTiempoExtra.Clase.IdEntidad.ToString());
                }
                else
                {
                    model.ListaClases = new SelectList(clases, "Value", "Text");
                }

                if (model.RegistroTiempoExtra.Presupuesto != null)
                {
                    model.ListaPresupuesto = new SelectList(presupuestos, "Value", "Text", model.RegistroTiempoExtra.Presupuesto.IdEntidad.ToString());
                }
                else if (model.Puesto.UbicacionAdministrativa?.Presupuesto != null && model.Puesto.UbicacionAdministrativa.Presupuesto?.Mensaje == null)
                {
                    model.ListaPresupuesto = new SelectList(presupuestos, "Value", "Text", model.Puesto.UbicacionAdministrativa.Presupuesto.IdEntidad.ToString());
                }
                else
                {
                    model.ListaPresupuesto = new SelectList(presupuestos, "Value", "Text");
                }
                return PartialView("_DetalleExtras", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return PartialView("_ErrorBusqueda");
            }
        }
        private ActionResult MostrarInfoJornadaDoble(FuncionarioRegistroExtrasVM model)
        {
            try
            {
                string[] mesAnio = model.MesActual.Split(' ');
                int mes = DateTime.ParseExact(mesAnio[0].ToLower(), "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
                int annio = mesAnio.Count() > 1 ? Convert.ToInt32(mesAnio[1]) : DateTime.Now.Year;
                model.FechaMin = "1/" + mes.ToString() + "/" + annio.ToString();
                model.FechaMax = DateTime.DaysInMonth(annio, mes).ToString() + "/" + mes.ToString() + "/" + annio.ToString();
                var lista = servicioTiempoExtra.ObtenerRegistroExtrasDetalleDoble(model.Funcionario.Cedula, model.MesActual);
                if (lista.Count() == 0)
                {
                    throw new Exception("Error no identificado. Contacte al administrador del sistema");
                }
                if (lista[0].GetType() == typeof(CErrorDTO))
                {
                    throw new Exception(((CErrorDTO)lista[0]).MensajeError);
                }
                model.Funcionario = (CFuncionarioDTO)lista[0];
                model.Puesto = (CPuestoDTO)lista[1];
                model.DetallePuesto = (CDetallePuestoDTO)lista[2];
                model.Desglose1 = (CDesgloseSalarialDTO)lista[3];
                model.Desglose2 = (CDesgloseSalarialDTO)lista[4];
                model.Nombramineto = (CNombramientoDTO)lista[5];
                model.RegistroTiempoExtra = (CRegistroTiempoExtraDTO)lista[6];
                model.HayArchivo = model.RegistroTiempoExtra.Archivo != null;
                model.MesActual = model.RegistroTiempoExtra.Periodo;
                model.MesActualPago = RegistroTiempoExtraHelper.CalcularPeriodoPagoRegistrado(model.RegistroTiempoExtra.FecPago);
                if (model.RegistroTiempoExtra.Clase?.Mensaje != null && model.RegistroTiempoExtra.Clase.Mensaje != "")
                    model.RegistroTiempoExtra.Clase = new CClaseDTO { DesClase = "NO TIENE"};
                else
                    model.ClaseActual = model.RegistroTiempoExtra.Clase.DesClase;
                if (model.RegistroTiempoExtra.Presupuesto?.Mensaje != null && model.RegistroTiempoExtra.Presupuesto.Mensaje != "")
                    model.RegistroTiempoExtra.Presupuesto = null;
                model.RegistroTiempoExtra.MontoDiurna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240;
                model.RegistroTiempoExtra.MontoMixta = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 210;
                model.RegistroTiempoExtra.MontoNocturna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 180;
                model.HayArchivo = model.RegistroTiempoExtra.Archivo != null;
                model.DetalleExtras = model.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
                model.DetalleExtrasGuardados = model.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
                model.DetalleExtras = CompletarDetalles(model.DetalleExtras, Convert.ToInt32(model.RegistroTiempoExtra.Periodo.Split(' ')[1]),
                    DateTime.ParseExact(model.RegistroTiempoExtra.Periodo.Split(' ')[0], "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month);
                if (RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) && !model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE) && (model.RegistroTiempoExtra.Clase == null || !model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
                {
                    throw new Exception("Solo se puede registrar jornadas dobles a funcionarios que laboraron el tiempo extra como guarda");
                }
                for (int i = 0; i < model.DetalleExtras.Count; i++)
                {
                    if (model.DetalleExtras[i].TipoExtra != null)
                    {
                        model.DetalleExtras[i].Jornada = RegistroTiempoExtraHelper.DefinirJornada(model.DetalleExtras[i].TipoExtra.IdEntidad);
                        List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(model.DetalleExtras[i], model.DetalleExtras[i].TipoExtra.DesTipExtra.Contains("doble"));
                        model.DetalleExtras[i].FechaInicioEspecial = libres[0];
                        model.DetalleExtras[i].FechaFinalEspecial = libres[1];
                    }
                }
                model.H0 = new List<decimal>();
                model.H1 = new List<decimal>();
                model.H2 = new List<decimal>();
                CalcularGuardaVista(model,"");//CALCULO DE TIEMPO PARA OFICIALES DE SEGURIDAD
                model.TotalHorasH0Ver = model.TotalHorasH0;
                model.TotalHorasH1Ver = model.TotalHorasH1;
                model.TotalHorasH2Ver = model.TotalHorasH2;
                model.TotalH0Ver = model.TotalH0;
                model.TotalH1Ver = model.TotalH1;
                model.TotalH2Ver = model.TotalH2;
                model.TotalPagarVer = model.TotalPagar;
                model.TotalHorasH0 = model.TotalHorasH1 = model.TotalHorasH2 = model.TotalH0 = model.TotalH1 = model.TotalH2 = model.TotalPagar = 0;
                model.DetalleExtras = new List<CDetalleTiempoExtraDTO> 
                {
                    new CDetalleTiempoExtraDTO{FechaInicioDoble = Convert.ToDateTime(model.FechaMin)}
                };
                return PartialView("_DetalleExtrasDoble", model);
            } catch(Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return PartialView("_ErrorBusqueda");
            }
        }
        //
        // POST: /RegistroTiempoExtra/GetFile
        public ActionResult ViewFile(string cedula, string nombre, string apellido, string mesActual, int id)
        {
            var datos = servicioTiempoExtra.BuscarArchivo(id);

            if (((CRespuestaDTO)datos).Codigo > 0)
            {
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = $"{cedula}_{nombre.Trim()}_{apellido.Trim().Substring(0, 1).ToUpper()}_{mesActual.Replace(" ", "_")}.pdf",
                    Inline = true
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                var image = (byte[])((CRespuestaDTO)datos).Contenido;
                return File(image, "application/pdf");
            }
            else
            {
                return File(new byte[0], "application/pdf");
            }
           
        }
        public ActionResult GetFile(string cedula, string nombre, string apellido, string mesActual, int id)
        {
            var datos = servicioTiempoExtra.BuscarArchivo(id);
            if (((CRespuestaDTO)datos).Codigo > 0)
            {
                var image = (byte[])((CRespuestaDTO)datos).Contenido;
                return File(image, "application/pdf", $"{cedula}_{nombre.Trim()}_{apellido.Trim().Substring(0, 1).ToUpper()}_{mesActual.Replace(" ", "_")}.pdf");
            }
            else
            {
                return File(new byte[0], "application/pdf");
            }
           
        }
        //private void CalcularAdministrativo2(FuncionarioRegistroExtrasVM model) {
        //    string validacion;
        //    bool hayHoraCompleta = false; //Identifica si hay una hora o más ya registrada para que pueda ingresar fracciones de hora en fechas posteriores
        //    for (int i = 0; i < model.DetalleExtras.Count; i++)
        //    {
        //        model.H0.Add(0);
        //        model.H1.Add(0);
        //        model.H2.Add(0); 
        //        if (!RegistroTiempoExtraHelper.EsVacio(model.DetalleExtras[i]))
        //        {
        //            validacion = RegistroTiempoExtraHelper.ValidarRegistroAdministrativo2(model.DetalleExtras[i], servicioTiempoExtra, model.Funcionario.Cedula, ref hayHoraCompleta);
        //            if (validacion.StartsWith("Error:"))//Valida hora inicio, hora fin y jornada completa mayor a 8 horas o día libre, vacaciones, incapacidades
        //            {
        //                ModelState.AddModelError("Error" + i.ToString() + validacion.Substring(6, 1), model.DetalleExtras[i].FechaInicio.ToShortDateString());
        //                continue;
        //            }

        //            //Inicio de calculos
        //            List<decimal> calculo = RegistroTiempoExtraHelper.CalcularHorasFilaAdministrativo2(model.DetalleExtras[i]);
        //            model.H0[i] = calculo[0];
        //            model.H1[i] = calculo[1];
        //            model.H2[i] = calculo[2];
        //            model.DetalleExtras[i].Jornada = JornadaEnum.D;
        //            model.DetalleExtras[i].TipoExtra = new CTipoExtraDTO
        //            {
        //                IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtra(model.DetalleExtras[i])
        //            };
        //            if (model.DetalleExtras[i].TipoExtra.IdEntidad == 1)
        //            {
        //                model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHoras((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240, calculo[1], "H1");
        //                model.TotalH1 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
        //            }
        //            else
        //            {
        //                model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHoras((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240, calculo[0], "H0", DiasFestivosHelper.EsFeriado(model.DetalleExtras[i].FechaInicio));
        //                model.TotalH0 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
        //                decimal h2 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHoras((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240, calculo[2], "H2");
        //                model.TotalH2 += h2;
        //                model.DetalleExtras[i].TotalLinea += h2;
        //            }

        //        }
        //    }

        //    ModelState.Remove("TotalPagar");
        //    model.TotalHorasH0 = model.H0.Sum();
        //    model.TotalHorasH1 = model.H1.Sum();
        //    model.TotalHorasH2 = model.H2.Sum();
        //    model.TotalPagar = model.TotalH0 + model.TotalH1 + model.TotalH2;
        //    model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
        //}
        private void CalcularAdministrativo(FuncionarioRegistroExtrasVM model, bool validar = true)
        {
            string validacion;
            bool hayHoraCompleta = false; //Identifica si hay una hora o más ya registrada para que pueda ingresar fracciones de hora en fechas posteriores
            model.TotalH0 = model.TotalH1 = model.TotalH2 = model.TotalHorasH0 = model.TotalHorasH1 = model.TotalHorasH2 = model.TotalPagar = 0;
            for (int i = 0; i < model.DetalleExtras.Count; i++)
            {
                model.DetalleExtras[i].TotalLinea = 0;
                model.H0.Add(0);
                model.H1.Add(0);
                model.H2.Add(0);
                if (!RegistroTiempoExtraHelper.EsVacio(model.DetalleExtras[i]))
                {
                    if (validar)
                    {
                        validacion = RegistroTiempoExtraHelper.ValidarRegistroAdministrativo(model.DetalleExtras[i], model.Nombramineto.FecVence, servicioTiempoExtra, model.Funcionario.Cedula, ref hayHoraCompleta);
                        if (validacion.StartsWith("Error:"))//Valida hora inicio, hora fin y hora completa, día libre, vacaciones, incapacidades
                        {
                            ModelState.AddModelError("Error" + i.ToString() + validacion.Substring(6, 1), model.DetalleExtras[i].FechaInicio.ToShortDateString());
                            continue;
                        }
                    }
                    else
                    {
                        RegistroTiempoExtraHelper.FormatearFechas(model.DetalleExtras[i]);
                    }
                    //Inicio de calculos
                    List<decimal> calculo = RegistroTiempoExtraHelper.CalcularHorasFilaAdministrativo(model.DetalleExtras[i]);
                    model.H0[i] = calculo[0];
                    if (calculo[1] > 4)
                    {
                        model.H1[i] = 4;
                        calculo[1] = 4;
                        model.Funcionario.Mensaje = "Algunas de las entradas sobrepasan el máximo de horas extra a pagar en la jornada, por lo que se actualizaron a ese límite (4 horas)";
                    }
                    else
                    {
                        model.H1[i] = calculo[1];
                    }
                    if (calculo[2] > 4)
                    {
                        model.H2[i] = 4;
                        calculo[2] = 4;
                        model.Funcionario.Mensaje = "Algunas de las entradas sobrepasan el máximo de horas extra a pagar en jornda noctura, por lo que se actualizaron a ese límite (4 horas)";
                    }
                    else
                    {
                        model.H2[i] = calculo[2];
                    }
                    //model.H0[i] = calculo[0];
                    //model.H1[i] = calculo[1];
                    //model.H2[i] = calculo[2];
                    model.DetalleExtras[i].Jornada = JornadaEnum.D;
                    if (model.DetalleExtras[i].TipoExtra == null)
                    {
                        model.DetalleExtras[i].TipoExtra = new CTipoExtraDTO
                        {
                            IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtra(model.DetalleExtras[i], false)
                        };
                    }
                    if (model.DetalleExtras[i].TipoExtra.IdEntidad == 1)
                    {
                        model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240, calculo[1], "H1");
                        model.TotalH1 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                    }
                    else
                    {
                        model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240, calculo[0], "H0");
                        model.TotalH0 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                        decimal h2 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240, calculo[2], "H2");
                        model.TotalH2 += h2;
                        model.DetalleExtras[i].TotalLinea += h2;
                        //model.TotalHorasH0 += model.H0[i];//Solo suma al total si era feriado
                    }
                }
            }
            ModelState.Remove("TotalPagar");
            model.TotalHorasH0 = model.H0.Sum();
            model.TotalHorasH1 = model.H1.Sum();
            model.TotalHorasH2 = model.H2.Sum();
            model.TotalPagar = model.TotalH0 + model.TotalH1 + model.TotalH2;
            model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
        }
        private void CalcularGuardaVista(FuncionarioRegistroExtrasVM model, string reporte, bool validar = true)
        {
            string validacion;
            bool hayHoraCompleta = false; //Identifica si hay una hora o más ya registrada para que pueda ingresar fracciones de hora en fechas posteriores
            model.TotalH0 = model.TotalH1 = model.TotalH2 = model.TotalHorasH0 = model.TotalHorasH1 = model.TotalHorasH2 = model.TotalPagar = 0;
            for (int i = 0; i < model.DetalleExtras.Count; i++)
            {
                model.DetalleExtras[i].TotalLinea = 0;
                model.H0.Add(0);
                model.H1.Add(0);
                model.H2.Add(0);
                if (!RegistroTiempoExtraHelper.EsVacio(model.DetalleExtras[i]))
                {
                    if (validar)
                    {
                        validacion = RegistroTiempoExtraHelper.ValidarRegistroGuarda(model.DetalleExtras[i], model.Nombramineto.FecVence, servicioTiempoExtra, model.Funcionario.Cedula, ref hayHoraCompleta);
                        if (validacion.StartsWith("Error:"))//Valida hora inicio, hora fin y jornada completa mayor a 8 horas o día libre, vacaciones, incapacidades
                        {
                            ModelState.AddModelError("Error" + i.ToString() + validacion.Substring(6, 1), model.DetalleExtras[i].FechaInicio.ToShortDateString());
                            continue;
                        }
                    }
                    else
                    {
                        RegistroTiempoExtraHelper.FormatearFechas(model.DetalleExtras[i]);
                    }
                    //Inicio de calculos
                    int horas;
                    horas = RegistroTiempoExtraHelper.DefinirHorasPorJornada((JornadaEnum)model.DetalleExtras[i].Jornada);
                    decimal montoHora = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / horas;
                    List<decimal> calculo = RegistroTiempoExtraHelper.CalcularHorasFilaGuardaVista(model.DetalleExtras[i], reporte);
                    if ((JornadaEnum)model.DetalleExtras[i].Jornada == JornadaEnum.N)
                    {
                        model.H0[i] = calculo[0];
                        if (calculo[1] > 4)
                        {
                            model.H1[i] = 4;
                            calculo[1] = 4;
                            model.Funcionario.Mensaje = "Algunas de las entradas sobrepasan el máximo de horas extra a pagar en jornda noctura, por lo que se actualizaron a ese límite (4 horas)";
                        }
                        else
                        {
                            model.H1[i] = calculo[1];
                        }
                        if (calculo[2] > 4)
                        {
                            model.H2[i] = 4;
                            calculo[2] = 4;
                            model.Funcionario.Mensaje = "Algunas de las entradas sobrepasan el máximo de horas extra a pagar en jornda noctura, por lo que se actualizaron a ese límite (4 horas)";
                        }
                        else
                        {
                            model.H2[i] = calculo[2];
                        }
                    }
                    else
                    {
                        model.H0[i] = calculo[0];
                        model.H1[i] = calculo[1];
                        model.H2[i] = calculo[2];
                    }

                    if (model.DetalleExtras[i].TipoExtra == null)
                    {
                        model.DetalleExtras[i].TipoExtra = new CTipoExtraDTO
                        {
                            IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtra(model.DetalleExtras[i], true)
                        };
                    }
                    if (model.DetalleExtras[i].TipoExtra.IdEntidad == 1 || model.DetalleExtras[i].TipoExtra.IdEntidad == 3 || model.DetalleExtras[i].TipoExtra.IdEntidad == 5)
                    {//Aqui solo deberian llegar normales, por lo que no se revisan las dobles
                        if (reporte == "reporte" && model.DetalleExtras[i].TipoExtra.IdEntidad == 5)
                        {
                            model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, calculo[2], "H1");
                            model.TotalH2 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                        }
                        else
                        {
                            model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, calculo[1], "H1");
                            model.TotalH1 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                        }
                    }
                    else
                    {
                        model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, calculo[0], "H0");
                        model.TotalH0 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                        decimal h2 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, calculo[2], "H2");
                        model.TotalH2 += h2;
                        model.DetalleExtras[i].TotalLinea += h2;
                        //model.TotalHorasH0 += model.H0[i];//Solo suma al total si era feriado
                    }
                }
            }
            ModelState.Remove("TotalPagar");
            model.TotalHorasH0 = model.H0.Sum();
            model.TotalHorasH1 = model.H1.Sum();
            model.TotalHorasH2 = model.H2.Sum();
            model.TotalPagar = model.TotalH0 + model.TotalH1 + model.TotalH2;
            model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
        }
        private void CalcularGuardaDobleVista(FuncionarioRegistroExtrasVM model, bool validar = true, List<CDetalleTiempoExtraDTO> detalles = null)
        {
            string validacion;
            bool hayHoraCompleta = true; //En doble no se valida que haya una Hora completa antes de meter fracciones
            model.TotalH0 = model.TotalH1 = model.TotalH2 = model.TotalHorasH0 = model.TotalHorasH1 = model.TotalHorasH2 = model.TotalPagar = 0;
            var grupos = model.DetalleExtras.GroupBy(D => D.FechaInicio)
                                .Select(G => new { Value = G.Key, Count = G.Count() })
                                .OrderByDescending(X => X.Count);
            List<DateTime> fechasRepetidas = new List<DateTime>();
            foreach (var grupo in grupos)
            {
                if (grupo.Count > 1)
                {
                    fechasRepetidas.Add(grupo.Value);
                    ModelState.AddModelError("Error" + grupo.Value, "La fecha de inicio " + grupo.Value.ToShortDateString() + " se repite " + grupo.Count + " veces.");
                }
            }
            for (int i = 0; i < model.DetalleExtras.Count; i++)
            {
                model.DetalleExtras[i].TotalLinea = 0;
                model.H0.Add(0);
                model.H1.Add(0);
                model.H2.Add(0);
                if (validar)
                {
                    if (fechasRepetidas.Contains(model.DetalleExtras[i].FechaInicio))
                    {
                        continue;
                    }
                    validacion = RegistroTiempoExtraHelper.ValidarRegistroGuarda(model.DetalleExtras[i], model.Nombramineto.FecVence, servicioTiempoExtra, model.Funcionario.Cedula, ref hayHoraCompleta, true, model.DetalleExtras.Select(D => D.FechaInicio).ToList());
                    if (validacion.StartsWith("Error:"))//Valida hora inicio, hora fin y jornada completa mayor a 8 horas o día libre, vacaciones, incapacidades
                    {
                        ModelState.AddModelError("Error" + i.ToString() + validacion.Substring(6, 1), model.DetalleExtras[i].FechaInicio.ToShortDateString());
                        continue;
                    }
                }
                else
                {
                    RegistroTiempoExtraHelper.FormatearFechas(model.DetalleExtras[i]);
                }
                //Inicio de calculos
                int horas; //Con el nuevo formato, las jornadas dobles siempre se calculan con 240, suponiendo que meteran horas solo de jornada 240 a como lo manejaban siempre a mano.
                horas = RegistroTiempoExtraHelper.DefinirHorasPorJornada((JornadaEnum)model.DetalleExtras[i].Jornada);
                decimal montoHora = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / horas;
                model.H0[i] = Convert.ToInt32(model.DetalleExtras[i].HoraTotalH0) + (decimal)Convert.ToInt32(model.DetalleExtras[i].MinutoTotalH0) / 100;
                model.H1[i] = Convert.ToInt32(model.DetalleExtras[i].HoraTotalH1) + (decimal)Convert.ToInt32(model.DetalleExtras[i].MinutoTotalH1) / 100;
                model.H2[i] = Convert.ToInt32(model.DetalleExtras[i].HoraTotalH2) + (decimal)Convert.ToInt32(model.DetalleExtras[i].MinutoTotalH2) / 100;
                if (model.DetalleExtras[i].TipoExtra == null)
                {
                    model.DetalleExtras[i].TipoExtra = new CTipoExtraDTO
                    {
                        IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtraDoble(model.DetalleExtras[i])
                    };
                }
                if (model.DetalleExtras[i].TipoExtra.IdEntidad == 9 || model.DetalleExtras[i].TipoExtra.IdEntidad == 11 || model.DetalleExtras[i].TipoExtra.IdEntidad == 13)
                {
                    if (model.H0[i] > 0)
                    {
                        model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, model.H0[i], "H0");
                        model.TotalH0 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                    }
                    if (model.H1[i] > 0)
                    {
                        model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, model.H1[i], "H1");
                        model.TotalH1 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                    }
                    if (model.H2[i] > 0)
                    {
                        model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, model.H2[i], "H2");
                        model.TotalH2 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                    }
                }
                else
                {
                    model.DetalleExtras[i].TotalLinea = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, model.H0[i], "H0");
                    model.TotalH0 += Convert.ToDecimal(model.DetalleExtras[i].TotalLinea);
                    decimal h2 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasVista(montoHora, model.H2[i], "H2");
                    model.TotalH2 += h2;
                    model.DetalleExtras[i].TotalLinea += h2;
                    model.TotalHorasH0 += model.H0[i];//Solo suma al total si era feriado
                }
            }
            ModelState.Remove("TotalPagar");
            model.TotalHorasH0 = model.H0.Sum();
            model.TotalHorasH1 = model.H1.Sum();
            model.TotalHorasH2 = model.H2.Sum();
            model.TotalPagar = model.TotalH0 + model.TotalH1 + model.TotalH2;
            model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
        }
        private void CalcularGuardaReporte(FuncionarioRegistroExtrasVM model, bool validar)
        {
            string validacion;
            bool hayHoraCompleta = false; //Identifica si hay una hora o más ya registrada para que pueda ingresar fracciones de hora en fechas posteriores
            model.TotalH0 = model.TotalH1 = model.TotalH2 = model.TotalHorasH0 = model.TotalHorasH1 = model.TotalHorasH2 = model.TotalPagar =  0;
            for (int i = 0; i < model.DetalleExtras.Count; i++)
            {
                model.DetalleExtras[i].TotalLinea = 0;
                model.H0.Add(0);
                model.H1.Add(0);
                model.H2.Add(0);
                if (!RegistroTiempoExtraHelper.EsVacio(model.DetalleExtras[i]))
                {
                    if (validar)
                    {
                        validacion = RegistroTiempoExtraHelper.ValidarRegistroGuarda(model.DetalleExtras[i], model.Nombramineto.FecVence, servicioTiempoExtra, model.Funcionario.Cedula, ref hayHoraCompleta);
                        if (validacion.StartsWith("Error:"))//Valida hora inicio, hora fin y jornada completa mayor a 8 horas o día libre, vacaciones, incapacidades
                        {
                            ModelState.AddModelError("Error" + i.ToString() + validacion.Substring(6, 1), model.DetalleExtras[i].FechaInicio.ToShortDateString());
                            continue;
                        }
                    }
                    else
                    {
                        RegistroTiempoExtraHelper.FormatearFechas(model.DetalleExtras[i]);
                    }

                    //Inicio de calculos
                    int horas;
                    horas = RegistroTiempoExtraHelper.DefinirHorasPorJornada((JornadaEnum)model.DetalleExtras[i].Jornada);
                    decimal montoHora = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / horas;
                    List<decimal> calculo = RegistroTiempoExtraHelper.CalcularHorasFilaGuardaReporte(model.DetalleExtras[i]);
                    model.H0[i] = calculo[0];
                    model.H1[i] = calculo[1];
                    model.H2[i] = calculo[2];
                    if (model.DetalleExtras[i].TipoExtra == null)
                    {
                        model.DetalleExtras[i].TipoExtra = new CTipoExtraDTO
                        {
                            IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtra(model.DetalleExtras[i], true)
                        };
                    }
                    decimal h0 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasGuardaReporte(montoHora, calculo[0], "240", model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial);
                    model.TotalH0 += h0;
                    model.DetalleExtras[i].TotalLinea += h0;
                    decimal h1 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasGuardaReporte(montoHora, calculo[1], "210", model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial);
                    model.TotalH1 += h1;
                    model.DetalleExtras[i].TotalLinea += h1;
                    decimal h2 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasGuardaReporte(montoHora, calculo[2], "180", model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial);
                    model.TotalH2 += h2;
                    model.DetalleExtras[i].TotalLinea += h2;
                    if (model.DetalleExtras[i].Jornada == JornadaEnum.D)
                    {
                        model.TotalHorasH0 += calculo[0];
                    }
                    else if (model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial)
                    {
                        if (model.DetalleExtras[i].Jornada == JornadaEnum.M)
                        {
                            if (calculo[0] == 7)
                                model.TotalHorasH0 += calculo[0] + 1;
                        }
                        else if(model.DetalleExtras[i].Jornada == JornadaEnum.N)
                        {
                            if (calculo[0] == 6)
                                model.TotalHorasH0 += calculo[0] + 2;
                        }
                    }
                }

            }
            ModelState.Remove("TotalPagar");
            //model.TotalHorasH0 = model.H0.Sum();
            model.TotalHorasH1 = model.H1.Sum();
            model.TotalHorasH2 = model.H2.Sum();
            model.TotalPagar = model.TotalH0 + model.TotalH1 + model.TotalH2;
            model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
        }
        private void CalcularGuardaDobleReporte(FuncionarioRegistroExtrasVM model, bool validar)
        {
            string validacion;
            bool hayHoraCompleta = true; //En doble no se valida que haya una Hora completa antes de meter fracciones
            model.TotalH0 = model.TotalH1 = model.TotalH2 = model.TotalHorasH0 = model.TotalHorasH1 = model.TotalHorasH2 = model.TotalPagar = 0;
            var grupos = model.DetalleExtras.GroupBy(D => D.FechaInicio)
                                .Select(G => new { Value = G.Key, Count = G.Count() })
                                .OrderByDescending(X => X.Count);
            List<DateTime> fechasRepetidas = new List<DateTime>();
            foreach(var grupo in grupos)
            {
                if (grupo.Count > 1)
                {
                    fechasRepetidas.Add(grupo.Value);
                    ModelState.AddModelError("Error" + grupo.Value, "La fecha de inicio " + grupo.Value.ToShortDateString() + " se repite " + grupo.Count + " veces.");
                }
            }
            for (int i = 0; i < model.DetalleExtras.Count; i++)
            {
                model.DetalleExtras[i].TotalLinea = 0;
                model.H0.Add(0);
                model.H1.Add(0);
                model.H2.Add(0);
                if (validar)
                {
                    if (fechasRepetidas.Contains(model.DetalleExtras[i].FechaInicio))
                    {
                        continue;
                    }
                    validacion = RegistroTiempoExtraHelper.ValidarRegistroGuarda(model.DetalleExtras[i], model.Nombramineto.FecVence, servicioTiempoExtra, model.Funcionario.Cedula, ref hayHoraCompleta, true);
                    if (validacion.StartsWith("Error:"))//Valida hora inicio, hora fin y jornada completa mayor a 8 horas o día libre, vacaciones, incapacidades
                    {
                        ModelState.AddModelError("Error" + i.ToString() + validacion.Substring(6, 1), model.DetalleExtras[i].FechaInicio.ToShortDateString());
                        continue;
                    }
                }
                else
                {
                    RegistroTiempoExtraHelper.FormatearFechas(model.DetalleExtras[i]);
                }
                //Inicio de calculos
                int horas; //Con el nuevo formato, las jornadas dobles siempre se calculan con 240, suponiendo que meteran horas solo de jornada 240 a como lo manejaban siempre a mano.
                horas = RegistroTiempoExtraHelper.DefinirHorasPorJornada((JornadaEnum)model.DetalleExtras[i].Jornada);
                decimal montoHora = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / horas;
                model.H0[i] = Convert.ToInt32(model.DetalleExtras[i].HoraTotalH0) + (decimal)Convert.ToInt32(model.DetalleExtras[i].MinutoTotalH0) / 100;
                model.H1[i] = Convert.ToInt32(model.DetalleExtras[i].HoraTotalH1) + (decimal)Convert.ToInt32(model.DetalleExtras[i].MinutoTotalH1) / 100;
                model.H2[i] = Convert.ToInt32(model.DetalleExtras[i].HoraTotalH2) + (decimal)Convert.ToInt32(model.DetalleExtras[i].MinutoTotalH2) / 100;
                if (model.DetalleExtras[i].TipoExtra == null)
                {
                    model.DetalleExtras[i].TipoExtra = new CTipoExtraDTO
                    {
                        IdEntidad = RegistroTiempoExtraHelper.DefinirTipoExtraDoble(model.DetalleExtras[i])
                    };
                }
                decimal h0 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasGuardaReporte((model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240,
                    model.H0[i], "240", model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial);
                model.TotalH0 += h0;
                model.DetalleExtras[i].TotalLinea += h0;
                decimal h1 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasGuardaReporte(montoHora, model.H1[i], "210", model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial);
                model.TotalH1 += h1;
                model.DetalleExtras[i].TotalLinea += h1;
                decimal h2 = RegistroTiempoExtraHelper.CalcularMontoPagoPorHorasGuardaReporte(montoHora, model.H2[i], "180", model.DetalleExtras[i].FechaInicioEspecial || model.DetalleExtras[i].FechaFinalEspecial);
                model.TotalH2 += h2;
                model.DetalleExtras[i].TotalLinea += h2;
            }
            ModelState.Remove("TotalPagar");
            model.TotalHorasH0 = model.H0.Sum();
            model.TotalHorasH1 = model.H1.Sum();
            model.TotalHorasH2 = model.H2.Sum();
            model.TotalPagar = model.TotalH0 + model.TotalH1 + model.TotalH2;
            model.RegistroTiempoExtra.MontoTotal = model.TotalPagar;
        }
        private System.Collections.IEnumerable ObtenerClases()
        {
            System.Collections.IEnumerable clases = servicioTiempoExtra.ListarClasesConFormato(DES_CLASE)
                           .Select(Q => new SelectListItem
                           {
                               Value = ((CClaseDTO)Q).IdEntidad.ToString(),
                               Text = ((CClaseDTO)Q).DesClase
                           });
            return clases;
        }
        private System.Collections.IEnumerable ObtenerPresupuestos()
        {
            System.Collections.IEnumerable presupuestos = servicioTiempoExtra.ListarPresupuestos()
                           .Select(Q => new SelectListItem
                           {
                               Value = ((CPresupuestoDTO)Q).IdEntidad.ToString(),
                               Text = ((CPresupuestoDTO)Q).CodigoPresupuesto
                           });
            return presupuestos;
        }
        private List<CDetalleTiempoExtraDTO> QuitarDetallesVacios(List<CDetalleTiempoExtraDTO> detallesModel, bool quitarDobles = false)
        {
            List<CDetalleTiempoExtraDTO> detalles = new List<CDetalleTiempoExtraDTO>();
            if (quitarDobles)
            {
                detalles = detallesModel.Where(D => !RegistroTiempoExtraHelper.EsVacioDoble(D)).ToList();
            }
            else
            {
                detalles = detallesModel.Where(D => !RegistroTiempoExtraHelper.EsVacio(D)).ToList();
            }
            
            return detalles;
        }
        private List<CDetalleTiempoExtraDTO> CompletarDetalles(List<CDetalleTiempoExtraDTO> detalleExtras, int annio, int mes)
        {
            for (int i = 0; i < DateTime.DaysInMonth(annio, mes); i++)
            {
                var fechaInicio = new DateTime(annio, mes, i + 1);
                if (i < detalleExtras.Count)
                {
                    if (detalleExtras[i].FechaInicio != fechaInicio)
                    {
                        detalleExtras.Insert(i, new SIRH.DTO.CDetalleTiempoExtraDTO() { FechaInicio = fechaInicio });
                    }
                }
                else
                {
                    detalleExtras.Add(new SIRH.DTO.CDetalleTiempoExtraDTO() { FechaInicio = fechaInicio });
                }
            }
            return detalleExtras;
        }
        public void ErroresCalculo(string filtro, string asociado)
        {
            string respuesta = asociado;
            List<string> listaErrores = new List<string>();

            var lista = ModelState.Where(m => m.Key.EndsWith(filtro)).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Value.Errors.Count > 0)
                {
                    listaErrores.Add(lista[i].Value.Errors[0].ErrorMessage);
                    ModelState.Remove(lista[i]);
                }
            }

            if (listaErrores.Count > 0)
            {
                for (int i = 0; i < listaErrores.Count; i++)
                {
                    if (i == listaErrores.Count - 1)
                    {
                        respuesta += " " + listaErrores[i];
                    }
                    else
                    {
                        respuesta += " " + listaErrores[i] + ", ";
                    }
                }
                ModelState.AddModelError("Error" + filtro, respuesta);
            }
        }
        public void ErroresBusqueda(string filtro, string asociado)
        {
            string respuesta = asociado;
            List<string> listaErrores = new List<string>();

            var lista = ModelState.Where(m => m.Key.EndsWith(filtro)).ToList();
            var fechas = ModelState.Where(m => m.Key.EndsWith("FechaInicio")).ToList();

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Value.Errors.Count > 0)
                {
                    listaErrores.Add(Convert.ToDateTime(fechas[i].Value.Value.AttemptedValue).ToShortDateString());
                    ModelState.Remove(lista[i]);
                }
            }

            if (listaErrores.Count > 0)
            {
                for (int i = 0; i < listaErrores.Count; i++)
                {
                    if (i == listaErrores.Count - 1)
                    {
                        respuesta += " " + listaErrores[i];
                    }
                    else
                    {
                        respuesta += " " + listaErrores[i] + ", ";
                    }
                }
                ModelState.AddModelError("Error" + filtro, respuesta);
            }
        }
        //
        // GET: /RegistroTiempoExtra/Saved
        public ActionResult Saved(string cedula, string periodo, string mensaje, int codigo, bool doble = false)
        {
            FuncionarioRegistroExtrasVM model = new FuncionarioRegistroExtrasVM();
            try
            {
                var lista = servicioTiempoExtra.ObtenerRegistroExtrasSaved(cedula, periodo, doble);
                if(lista.Count() == 0)
                {
                    throw new Exception("Error no identificado. Contacte al administrador del sistema");
                }
                if (lista[0].GetType() == typeof(CErrorDTO))
                {
                    throw new Exception(((CErrorDTO)lista[0]).MensajeError);
                }
                if(codigo < 0)
                {
                    throw new Exception(mensaje);
                }
                
                model.tituloSaved = mensaje;
                model.Doble = doble;
                model.Funcionario = (CFuncionarioDTO)lista[0];
                model.Puesto = (CPuestoDTO)lista[1];
                model.DetallePuesto = (CDetallePuestoDTO)lista[2];
                model.Desglose1 = (CDesgloseSalarialDTO)lista[3];
                model.Desglose2 = (CDesgloseSalarialDTO)lista[4];
                model.Nombramineto = (CNombramientoDTO)lista[5];
                model.RegistroTiempoExtra = (CRegistroTiempoExtraDTO)lista[6];
                model.RegistroTiempoExtra.MontoDiurna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240;
                model.RegistroTiempoExtra.MontoMixta = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 210;
                model.RegistroTiempoExtra.MontoNocturna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 180;
                //model.DetalleExtras = model.RegistroTiempoExtra.Detalles;
                //model.EstadoDetalles = model.DetalleExtras[0].Estado;
                //foreach (var detalle in model.DetalleExtras)
                //{
                //    if(detalle.TipoExtra != null)
                //    {
                //        detalle.Jornada = RegistroTiempoExtraHelper.DefinirJornada(detalle.TipoExtra.IdEntidad);
                //        List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(detalle, detalle.TipoExtra.DesTipExtra.Contains("doble"));
                //        detalle.FechaInicioEspecial = libres[0];
                //        detalle.FechaFinalEspecial = libres[1];
                //    }
                //}
                //if (model.RegistroTiempoExtra.Clase != null && model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE))
                //{
                //    model.ClaseActual = model.RegistroTiempoExtra.Clase.IdEntidad.ToString();
                //}
                //model.MesActual = model.RegistroTiempoExtra.Periodo;
                //model.H0 = new List<decimal>();
                //model.H1 = new List<decimal>();
                //model.H2 = new List<decimal>();
                //if (doble)
                //{
                //    CalcularGuardaDobleVista(model);
                //}
                //else if (!RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) ||
                //        model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE) ||
                //        (model.RegistroTiempoExtra.Clase?.DesClase != null && model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
                //{
                //    CalcularGuardaVista(model);
                //}
                //else
                //{
                //    CalcularAdministrativo(model);
                //}
                return View(model);
            } 
            catch(Exception ex)
            {
                model.Funcionario = new CFuncionarioDTO
                {
                    Cedula = cedula,
                    Sexo = GeneroEnum.Indefinido,
                    Mensaje = "Error"
                };
                model.RegistroTiempoExtra = new CRegistroTiempoExtraDTO
                {
                    Periodo = periodo,
                    Mensaje = ex.Message
                };
                return View(model);
            }
        }
        //
        // GET: /RegistroTiempoExtra/Details
        public ActionResult Details(DateTime fechaRegistro, int id,  string doble)
        {
            FuncionarioRegistroExtrasVM model = new FuncionarioRegistroExtrasVM();
            model.Doble = doble == "Jornada Doble" ? true : false;
            var lista = servicioTiempoExtra.ObtenerRegistroExtrasDetalle(fechaRegistro, id, model.Doble);
            if (lista.Count() == 0)
            {
                throw new Exception("Error no identificado. Contacte al administrador del sistema");
            }
            if (lista[0].GetType() == typeof(CErrorDTO))
            {
                throw new Exception(((CErrorDTO)lista[0]).MensajeError);
            }
            model.Funcionario = (CFuncionarioDTO)lista[0];
            model.Puesto = (CPuestoDTO)lista[1];
            model.DetallePuesto = (CDetallePuestoDTO)lista[2];
            model.Desglose1 = (CDesgloseSalarialDTO)lista[3];
            model.Desglose2 = (CDesgloseSalarialDTO)lista[4];
            model.Nombramineto = (CNombramientoDTO)lista[5];
            model.RegistroTiempoExtra = (CRegistroTiempoExtraDTO)lista[6];
            model.UsuarioEnvia = model.RegistroTiempoExtra.Mensaje;
            model.RegistroTiempoExtra.Mensaje = doble;
            model.HayArchivo = model.RegistroTiempoExtra.Archivo != null;
            model.MesActual = model.RegistroTiempoExtra.Periodo;
            model.MesActualPago = RegistroTiempoExtraHelper.CalcularPeriodoPagoRegistrado(model.RegistroTiempoExtra.FecPago);
            if (model.RegistroTiempoExtra.Clase?.Mensaje != null && model.RegistroTiempoExtra.Clase.Mensaje != "")
                model.RegistroTiempoExtra.Clase = new CClaseDTO { DesClase = "NO TIENE" };
            else
                model.ClaseActual = model.RegistroTiempoExtra.Clase.DesClase;
            if (model.RegistroTiempoExtra.Presupuesto?.Mensaje != null && model.RegistroTiempoExtra.Presupuesto.Mensaje != "")
                model.RegistroTiempoExtra.Presupuesto = null;
            model.RegistroTiempoExtra.MontoDiurna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 240;
            model.RegistroTiempoExtra.MontoMixta = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 210;
            model.RegistroTiempoExtra.MontoNocturna = (model.Desglose1.MontoSalarioBruto + model.Desglose2.MontoSalarioBruto) / 180;
            model.RegistroTiempoExtra.FecRegistroDetalles = fechaRegistro;
            var datosJustificacion = ((CRegistroTiempoExtraDTO)lista[6]).Justificacion != null ? ((CRegistroTiempoExtraDTO)lista[6]).Justificacion.Split('<') : null;
            if (datosJustificacion != null && datosJustificacion.Count() > 1)
            {
                model.RegistroTiempoExtra.Justificacion = datosJustificacion[0];
                model.RegistroTiempoExtra.Area = datosJustificacion[1];
                model.RegistroTiempoExtra.Actividad = datosJustificacion[2];
            }
            model.HayArchivo = model.RegistroTiempoExtra.Archivo != null;
            model.DetalleExtras = model.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
            model.EstadoDetalles = model.DetalleExtras.ElementAt(0).Estado;
            model.DetalleExtras = CompletarDetalles(model.DetalleExtras, Convert.ToInt32(model.RegistroTiempoExtra.Periodo.Split(' ')[1]),
                DateTime.ParseExact(model.RegistroTiempoExtra.Periodo.Split(' ')[0], "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month);
            for (int i = 0; i < model.DetalleExtras.Count; i++)
            {
                if (model.DetalleExtras[i].TipoExtra != null)
                {
                    model.DetalleExtras[i].Jornada = RegistroTiempoExtraHelper.DefinirJornada(model.DetalleExtras[i].TipoExtra.IdEntidad);
                    List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(model.DetalleExtras[i], model.DetalleExtras[i].TipoExtra.DesTipExtra.Contains("doble"));
                    model.DetalleExtras[i].FechaInicioEspecial = libres[0];
                    model.DetalleExtras[i].FechaFinalEspecial = libres[1];
                }
            }
            model.H0 = new List<decimal>();
            model.H1 = new List<decimal>();
            model.H2 = new List<decimal>();
            if (model.Doble)
            {
                CalcularGuardaDobleVista(model);
            }
            else if (!RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) ||
                    model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE) ||
                    (model.RegistroTiempoExtra.Clase?.DesClase != null && model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
            {
                CalcularGuardaVista(model,"");
            }
            else
            {
                CalcularAdministrativo(model);
            }

            return View(model);
        }

        //
        // POST: /RegistroTiempoExtra/Details
        [HttpPost]
        public ActionResult Details(FuncionarioRegistroExtrasVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Request.IsAjaxRequest())
                    {
                       // var lista = servicioTiempoExtra.ObtenerRegistroExtrasDetalle(model.Funcionario.Cedula, model.MesActual + "/" + model.AnnioSeleccionado.ToString());
                        
                        //model.Funcionario = (CFuncionarioDTO)lista[0].ElementAt(0);
                        //model.Puesto = (CPuestoDTO)lista[1].ElementAt(0);
                       // model.DetallePuesto = (CDetallePuestoDTO)lista[1].ElementAt(1);
                        //model.RegistroTiempoExtra = (CRegistroTiempoExtraDTO)lista[2].ElementAt(0);
                        //foreach (var item in lista[3])
                        //{
                        //    model.DetalleExtras.Add((CDetalleTiempoExtraDTO)item);
                        //}
                        return View();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorBusqueda");
            }
        }

        //
        // GET: /RegistroTiempoExtra/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RegistroTiempoExtra/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RegistroTiempoExtra/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RegistroTiempoExtra/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Search(string cedula, string fechaDesde, string fechaHasta,
                                  string coddivision, string coddireccion, string coddepartamento, string codseccion, string pagoDoble, string estado, int page = 1)
        {
            BusquedaExtrasVM model = new BusquedaExtrasVM();
            try
            {
                if (Request.IsAjaxRequest())
                {
                    if (string.IsNullOrEmpty(cedula) && string.IsNullOrEmpty(fechaDesde) && string.IsNullOrEmpty(fechaHasta) && string.IsNullOrEmpty(coddivision) &&
                   string.IsNullOrEmpty(coddireccion) && string.IsNullOrEmpty(coddepartamento) && string.IsNullOrEmpty(codseccion) &&
                   string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(pagoDoble))
                    {
                        throw new Exception("Debe llenar al menos un parametro de búsqueda");
                    }
                    model.Cedula = cedula;
                    model.FechaDesde = string.IsNullOrEmpty(fechaDesde) ? (DateTime?)null : Convert.ToDateTime(fechaDesde);
                    model.FechaHasta = string.IsNullOrEmpty(fechaHasta) ? (DateTime?)null : Convert.ToDateTime(fechaHasta).AddDays(1).AddSeconds(-1);
                    model.CodDivision = coddivision;
                    model.CodDireccion = coddireccion;
                    model.CodDepartamento = coddepartamento;
                    model.CodSeccion = codseccion;
                    model.Estado = estado;
                    model.PagoDoble = pagoDoble;

                    int estadoReal = 0;
                    bool doble = false;
                    if (pagoDoble == "Tiempo Extraordinario")
                    {
                        switch (estado)
                        {
                            case "Activo":
                                estadoReal = (int)EstadoExtraEnum.Activo;
                                break;
                            case "Rechazado":
                                estadoReal = (int)EstadoExtraEnum.Rechazado;
                                break;
                            case "Aprobado":
                                estadoReal = (int)EstadoExtraEnum.Aprobado;
                                break;
                            case "Cerrado":
                                estadoReal = (int)EstadoExtraEnum.Cerrado;
                                break;
                            case "Anulado":
                                estadoReal = (int)EstadoExtraEnum.Anulado;
                                break;
                            default:
                                estadoReal = 0;
                                break;
                        }
                    }
                    else if (pagoDoble == "Jornada Doble")
                    {
                        switch (estado)
                        {
                            case "Activo":
                                estadoReal = (int)EstadoExtraEnum.Activo;
                                break;
                            case "Rechazado":
                                estadoReal = (int)EstadoExtraEnum.Rechazado;
                                break;
                            case "Aprobado":
                                estadoReal = (int)EstadoExtraEnum.Aprobado;
                                break;
                            case "Cerrado":
                                estadoReal = (int)EstadoExtraEnum.Cerrado;
                                break;
                            case "Anulado":
                                estadoReal = (int)EstadoExtraEnum.Anulado;
                                break;
                            default:
                                estadoReal = 0;
                                break;
                        }
                        doble = true;
                    }

                    var registros = servicioTiempoExtra.BuscarTiempoExtraFiltros(cedula != null && cedula.Trim() != "" ? cedula : null, model.FechaDesde, model.FechaHasta,
                                                                string.IsNullOrEmpty(coddivision) ? null : coddivision.Split('-')[0],
                                                                string.IsNullOrEmpty(coddireccion) ? null : coddireccion.Split('-')[0],
                                                                string.IsNullOrEmpty(coddepartamento) ? null : coddepartamento.Split('-')[0],
                                                                string.IsNullOrEmpty(codseccion) ? null : codseccion.Split('-')[0],
                                                                estadoReal, doble
                                                                );
                    foreach (var registro in registros)
                    {
                        if (doble)
                        {
                            if (registro.Detalles.FirstOrDefault().Estado == (EstadoDetalleExtraEnum)estadoReal || estadoReal == 0)
                            {
                                FuncionarioRegistroExtrasVM modelo = new FuncionarioRegistroExtrasVM();
                                modelo.Funcionario = registro.Funcionario;
                                modelo.Desglose1 = registro.QuincenaA;
                                modelo.Desglose2 = registro.QuincenaB;
                                modelo.Nombramineto = new CNombramientoDTO
                                {
                                    FecVence = registro.FechVenceNombramiento
                                };
                                modelo.RegistroTiempoExtra = registro;

                                modelo.RegistroTiempoExtra.MontoDiurna = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 240;
                                modelo.RegistroTiempoExtra.MontoMixta = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 210;
                                modelo.RegistroTiempoExtra.MontoNocturna = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 180;

                                modelo.DetalleExtras = modelo.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
                                for (int i = 0; i < modelo.DetalleExtras.Count; i++)
                                {
                                    if (modelo.DetalleExtras[i].TipoExtra != null)
                                    {
                                        modelo.DetalleExtras[i].Jornada = RegistroTiempoExtraHelper.DefinirJornada(modelo.DetalleExtras[i].TipoExtra.IdEntidad);
                                        List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(modelo.DetalleExtras[i], modelo.DetalleExtras[i].TipoExtra.DesTipExtra.Contains("doble"));
                                        modelo.DetalleExtras[i].FechaInicioEspecial = libres[0];
                                        modelo.DetalleExtras[i].FechaFinalEspecial = libres[1];
                                    }
                                }
                                modelo.H0 = new List<decimal>();
                                modelo.H1 = new List<decimal>();
                                modelo.H2 = new List<decimal>();
                                if (doble)
                                {
                                    CalcularGuardaDobleVista(modelo, false);
                                }
                                else if (!RegistroTiempoExtraHelper.EsAdministrativo(modelo.RegistroTiempoExtra.Ocupacion)
                                 || (modelo.RegistroTiempoExtra.Clase?.DesClase != null && modelo.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
                                {
                                    CalcularGuardaVista(modelo, "", false);
                                }
                                else
                                {
                                    CalcularAdministrativo(modelo);
                                }
                                registro.MontoTotal = modelo.RegistroTiempoExtra.MontoTotal;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            FuncionarioRegistroExtrasVM modelo = new FuncionarioRegistroExtrasVM();
                            modelo.Funcionario = registro.Funcionario;
                            modelo.Desglose1 = registro.QuincenaA;
                            modelo.Desglose2 = registro.QuincenaB;
                            modelo.Nombramineto = new CNombramientoDTO
                            {
                                FecVence = registro.FechVenceNombramiento
                            };
                            modelo.RegistroTiempoExtra = registro;

                            modelo.RegistroTiempoExtra.MontoDiurna = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 240;
                            modelo.RegistroTiempoExtra.MontoMixta = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 210;
                            modelo.RegistroTiempoExtra.MontoNocturna = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 180;

                            modelo.DetalleExtras = modelo.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
                            for (int i = 0; i < modelo.DetalleExtras.Count; i++)
                            {
                                if (modelo.DetalleExtras[i].TipoExtra != null)
                                {
                                    modelo.DetalleExtras[i].Jornada = RegistroTiempoExtraHelper.DefinirJornada(modelo.DetalleExtras[i].TipoExtra.IdEntidad);
                                    List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(modelo.DetalleExtras[i], modelo.DetalleExtras[i].TipoExtra.DesTipExtra.Contains("doble"));
                                    modelo.DetalleExtras[i].FechaInicioEspecial = libres[0];
                                    modelo.DetalleExtras[i].FechaFinalEspecial = libres[1];
                                }
                            }
                            modelo.H0 = new List<decimal>();
                            modelo.H1 = new List<decimal>();
                            modelo.H2 = new List<decimal>();
                            if (doble)
                            {
                                CalcularGuardaDobleVista(modelo, false);
                            }
                            else if (!RegistroTiempoExtraHelper.EsAdministrativo(modelo.RegistroTiempoExtra.Ocupacion)
                             || (modelo.RegistroTiempoExtra.Clase?.DesClase != null && modelo.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
                            {
                                CalcularGuardaVista(modelo, "", false);
                            }
                            else
                            {
                                CalcularAdministrativo(modelo);
                            }
                            registro.MontoTotal = modelo.RegistroTiempoExtra.MontoTotal;
                        }
                    }
                    //model.TotalRegistros = registros.Count();
                    //model.TotalPaginas = (int)Math.Ceiling((double)model.TotalRegistros / 10);
                    //model.PaginaActual = page;
                    //if ((((page - 1) * 10) + 10) > model.TotalRegistros)
                    //{
                    //    model.Registros = registros.ToList().GetRange(((page - 1) * 10), (model.TotalRegistros) - (((page - 1) * 10))).ToList();
                    //}
                    //else
                    //{
                    //    model.Registros = registros.ToList().GetRange(((page - 1) * 10), 10).ToList(); ;
                    //}
                    model.Registros = registros.ToList();
                    return PartialView("Search_Result", model);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == null)
                {
                    ModelState.AddModelError("Busqueda", "Ha ocurrido un error al realizar la búsqueda, pongase en contacto con el personal autorizado. \n\n");
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ex.Message);
                }
                return PartialView("_ErrorBusqueda");
            }

        }


        [HttpPost]
        public ActionResult Search(FuncionarioRegistroExtrasVM model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Cancel(DateTime fechaDetalles, int idRegistro, bool doble, string SubmitButton, string user, FormCollection form)
        {
            FuncionarioRegistroExtrasVM model = new FuncionarioRegistroExtrasVM();
            try
            {
                var respuesta = servicioTiempoExtra.AnularRegistroTiempoExtra(idRegistro, fechaDetalles, doble, SubmitButton);
                if (respuesta.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
                string mensaje = "";
                if (respuesta.Codigo == 1)
                {
                    mensaje = doble ? "La jornadas dobles fueron modificadas correctamente" : "El registro de tiempo extraordinario fue modificado correctamente";
                    string envia = principal.Identity.Name.Split('\\')[1];
                    string recibe = "";
                    string destino = "";
                    string nota = "";
                    if (user != "vacio" && user != null)
                    {
                        recibe = user.Split('\\')[1];
                        destino = recibe + "@mopt.go.cr" + "," + envia + "@mopt.go.cr" + ",grettel.urena@mopt.go.cr, elisa.robles@mopt.go.cr";
                    }
                    else
                    {
                        destino = envia + "@mopt.go.cr" + "," + envia + "@mopt.go.cr" + ",grettel.urena@mopt.go.cr, elisa.robles@mopt.go.cr";
                        nota = "<br/><br/> <b>NOTA: No se pudo cargar el correo del encargado administrativo para enviar esta notificación, por favor reenvíela a quien corresponda.</b> <br/><br/>";
                    }
                    if (SubmitButton == "Aprobar")
                    {
                        var correo = new EmailWebHelper
                        {
                            Asunto = "SIRH - Tiempo Extraordinario",
                            Destinos = destino,
                            EmailBody = "Estimados(as) usuarios, <br/><br/> Se ha <b>APROBADO</b> el registro de tiempo extra # " + idRegistro + "<br/></br/>Diríjase al módulo de tiempo extraordinario en el SIRH para verificar el dato. <br/><br/> Puede encontrar el detalle de lo ingresado <a href='http://sisrh.mopt.go.cr:84/RegistroTiempoExtra/Details/" + idRegistro + "?fechaRegistro=01%2F01%2F0001%2000%3A00%3A00&doble=Tiempo Extraordinario'> AQUÍ </a> <br/><br/> Con esta confirmación ya puede imprimir el formulario final para las firmas correspondientes. <br/><br/> Saludos." + nota
                        };

                        var enviado = correo.EnviarCorreo();
                    }
                    if (SubmitButton == "Rechazar")
                    {
                        var actualizar = servicioTiempoExtra.ActualizarObservacionEstado(idRegistro, form["Justificacion"], doble);
                        var correo = new EmailWebHelper
                        {
                            Asunto = "SIRH - Tiempo Extraordinario",
                            Destinos = destino,
                            EmailBody = "Estimados(as) usuarios, <br/><br/> Se ha <b>RECHAZADO</b> el registro de tiempo extra # " + idRegistro + "<br/></br/> El motivo del rechazo es:<b> " + form["Justificacion"] +" </b><br/><br/>Diríjase al módulo de tiempo extraordinario en el SIRH para verificar el dato. <br/><br/> Puede encontrar el detalle de lo ingresado <a href='http://sisrh.mopt.go.cr:84/RegistroTiempoExtra/Details/" + idRegistro + "?fechaRegistro=01%2F01%2F0001%2000%3A00%3A00&doble=Tiempo Extraordinario'> AQUÍ </a> <br/><br/> Saludos." + nota
                        };

                        var enviado = correo.EnviarCorreo();
                    }
                }
                else if (respuesta.Codigo == 2)
                {
                    if (SubmitButton == "Rechazar")
                    {
                        var actualizar = servicioTiempoExtra.ActualizarObservacionEstado(idRegistro, form["Justificacion"], doble);
                        string envia = principal.Identity.Name.Split('\\')[1];
                        string recibe = "";
                        string destino = "";
                        string nota = "";
                        if (user != null)
                        {
                            recibe = user.Split('\\')[1];
                            destino = recibe + "@mopt.go.cr" + "," + envia + "@mopt.go.cr" + ",grettel.urena@mopt.go.cr, elisa.robles@mopt.go.cr";
                        }
                        else
                        {
                            destino = envia + "@mopt.go.cr";
                            nota = "<br/><br/> <b>NOTA: No se pudo cargar el correo del encargado administrativo para enviar esta notificación, por favor reenvíela a quien corresponda.</b> <br/><br/>";
                        }
                        var correo = new EmailWebHelper
                        {
                            Asunto = "SIRH - Tiempo Extraordinario",
                            Destinos = destino,
                            EmailBody = "Estimados(as) usuarios, <br/><br/> Se ha <b>RECHAZADO</b> el registro de tiempo extra # " + idRegistro + "<br/></br/> El motivo del rechazo es:<b> " + form["Justificacion"] + " </b><br/><br/>Diríjase al módulo de tiempo extraordinario en el SIRH para verificar el dato. <br/><br/> Puede encontrar el detalle de lo ingresado <a href='http://sisrh.mopt.go.cr:84/RegistroTiempoExtra/Details/" + idRegistro + "?fechaRegistro=01%2F01%2F0001%2000%3A00%3A00&doble=Tiempo Extraordinario'> AQUÍ </a> <br/><br/> Saludos." + nota
                        };

                        var enviado = correo.EnviarCorreo();
                    }
                    mensaje = "Se ha enviado la solicitud correctamente, pero no se dieron cambios en los datos";
                }
                else
                {
                    mensaje = "Se modificaron los datos";

                }
                model.RegistroTiempoExtra = new CRegistroTiempoExtraDTO
                {
                    Estado = EstadoExtraEnum.Activo,
                    Mensaje = "EXITO/" + mensaje
                };
                return PartialView("_ResultadoAnular", model);
            } catch(Exception ex)
            {
                model.RegistroTiempoExtra = new CRegistroTiempoExtraDTO
                {
                    Estado = EstadoExtraEnum.Activo,
                    Mensaje = "ERROR/" + ex.Message
                };
                return PartialView("_ResultadoAnular", model);
            }
        }
        private void FormatearPagoDoble(FuncionarioRegistroExtrasVM model)
        {
            //foreach(var detalle in model.DetalleExtras)
            //{
            //    if(detalle.Jornada == JornadaEnum.ND && detalle.FechaInicioEspecial == false && detalle.FechaFinalEspecial == false)
            //    {
            //        detalle.HoraTotalH2 = detalle.HoraTotalH1;
            //        detalle.MinutoTotalH2 = detalle.MinutoTotalH1;
            //        detalle.HoraTotalH1 = detalle.MinutoTotalH1 = null;
            //    } else if (detalle.Jornada == JornadaEnum.MD && (detalle.FechaInicioEspecial == true || detalle.FechaFinalEspecial == true))
            //    {
            //        detalle.HoraTotalH1 = detalle.HoraTotalH2;
            //        detalle.MinutoTotalH1 = detalle.MinutoTotalH2;
            //        detalle.HoraTotalH2 = detalle.MinutoTotalH2 = null;
            //    }
            //}
        }
        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseHorasExtras(FuncionarioRegistroExtrasVM model)
        {
            string[] meses = model.MesActual.Split(' ');
            model.RegistroTiempoExtra.Periodo = model.MesActual;
            if (model.Area == null && model.Actividad == null)
            {
                var datosJustificacion = model.RegistroTiempoExtra.Justificacion != null ? model.RegistroTiempoExtra.Justificacion.Split('<') : null;
                if (datosJustificacion != null && datosJustificacion.Count() > 1)
                {
                    model.RegistroTiempoExtra.Justificacion = datosJustificacion[0];
                    model.RegistroTiempoExtra.Area = datosJustificacion[1];
                    model.RegistroTiempoExtra.Actividad = datosJustificacion[2];
                }
            }
            string mesBuscar = meses[0].ToLower();
            int mes = DateTime.ParseExact(mesBuscar, "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
            int annio = Convert.ToInt32(meses[1]);
            model.DetalleExtras = CompletarDetalles(model.DetalleExtras, annio, mes);
            model.H0 = new List<decimal>();
            model.H1 = new List<decimal>();
            model.H2 = new List<decimal>();
            model.Doble = model.RegistroTiempoExtra.Mensaje.Contains("Doble") ? true : false;
            if (!model.Doble)
            {
                if (RegistroTiempoExtraHelper.EsAdministrativo(model.DetallePuesto.OcupacionReal.DesOcupacionReal) && !model.DetallePuesto.Clase.DesClase.Contains(DES_CLASE) && (model.RegistroTiempoExtra.Clase == null || !model.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
                {
                    CalcularAdministrativo(model); //CALCULO DE TIEMPO EXTRA PARA ADMINISTRATIVOS
                    for (int i = 0; i < model.DetalleExtras.Count(); i++)
                    {
                        model.DetalleExtras[i].H0 = model.H0[i];
                        model.DetalleExtras[i].H1 = model.H1[i];
                        model.DetalleExtras[i].H2 = model.H2[i];
                    }
                }
                else
                {
                    CalcularGuardaVista(model, "reporte", false);//CALCULO DE TIEMPO PARA OFICIALES DE SEGURIDAD
                    for (int i = 0; i < model.DetalleExtras.Count(); i++)
                    {
                        model.DetalleExtras[i].H0 = model.H0[i];
                        model.DetalleExtras[i].H1 = model.H1[i];
                        model.DetalleExtras[i].H2 = model.H2[i];
                    }
                }
            }
            else
            {
                model.DetalleExtras = QuitarDetallesVacios(model.DetalleExtras, true);
                FormatearPagoDoble(model);
                CalcularGuardaDobleVista(model, false);

                for (int i = 0; i < model.DetalleExtras.Count(); i++)
                {
                    model.DetalleExtras[i].H0 = model.H0[i];
                    model.DetalleExtras[i].H1 = model.H1[i];
                    model.DetalleExtras[i].H2 = model.H2[i];
                }
                model.DetalleExtras = CompletarDetalles(model.DetalleExtras, annio, mes);
            }
            List<RegistroTiempoExtraRptData> datosReporte = new List<RegistroTiempoExtraRptData>();

            for (int i = 0; i < model.DetalleExtras.Count; i++) {
                datosReporte.Add(RegistroTiempoExtraRptData.GenerarDatosReporte(model, string.Empty, i));
            }
            
            string reportPath = Path.Combine(Server.MapPath("~/Reports/RegistroTiempoExtra"), "RegistroTiempoExtraRPT.rpt");
            return new CrystalReportPdfResult(reportPath, datosReporte, "PDF");
        }

        [HttpPost]
        public ActionResult ReporteGeneralHorasExtras(BusquedaExtrasVM model)
        {
            var products = new System.Data.DataTable("teste");

            products.Columns.Add("CEDULA", typeof(string));
            products.Columns.Add("NOMBRE", typeof(string));
            products.Columns.Add("ESTADO", typeof(string));
            products.Columns.Add("AÑO", typeof(string));
            products.Columns.Add("MES", typeof(string));
            products.Columns.Add("QUINCENA", typeof(string));
            products.Columns.Add("RUBROS", typeof(string));
            products.Columns.Add("MONTO", typeof(string));
            products.Columns.Add("PUESTO", typeof(string));
            products.Columns.Add("CENTRO DE COSTO", typeof(string));
            products.Columns.Add("OBSERVACIONES", typeof(string));
            products.Columns.Add("DEPENDENCIA", typeof(string));
            

            foreach (var item in model.Registros)
            {
                products.Rows.Add(ConvertirCedulaHacienda(item.Funcionario.Cedula),
                                  item.Funcionario.Nombre + " " + item.Funcionario.PrimerApellido + " " + item.Funcionario.SegundoApellido,
                                  item.EstadoDetalles,
                                  "2021",
                                  item.Periodo.Substring(0,2),
                                  "02",
                                  "709000064-HORAS EXTRAS - OTROS INGRESOS-P.Adeudados",
                                  Math.Round(Convert.ToDouble(item.MontoTotal),2),
                                  item.Mensaje,
                                  item.Presupuesto.IdUnidadPresupuestaria,
                                  "Pago de Tiempo Extraordinario correspondiente al Mes de " + (item.Periodo.StartsWith("01") == true ? " ENERO" : item.Periodo.StartsWith("02") == true ? " FEBRERO" : item.Periodo.StartsWith("03") == true ? " MARZO" : item.Periodo.StartsWith("04") == true ? "ABRIL" : item.Periodo.StartsWith("05") == true ? "MAYO" : item.Periodo.StartsWith("06") == true ? "JUNIO" : item.Periodo.StartsWith("07") == true ? "JULIO" : item.Periodo.StartsWith("08") == true ? "AGOSTO" : item.Periodo.StartsWith("09") == true ? "SETIEMBRE" : item.Periodo.StartsWith("10") ? "OCTUBRE" : item.Periodo.StartsWith("11") ? "NOVIEMBRE" : "DICIEMBRE") + " 2021, de conformidad con Informe # " + item.IdEntidad + " -SIRH.",
                                  item.Seccion.NomSeccion);
            }


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.Close();
            Response.End();
            return View();
            //    int estadoReal = 0;
            //    bool doble = false;
            //    if (model.PagoDoble == "Tiempo Extraordinario")
            //    {
            //        estadoReal = model.Estado == "Activo" ? (int)EstadoExtraEnum.Activo : model.Estado == "Anulado" ? (int)EstadoExtraEnum.Anulado : 0;
            //    }
            //    else if (model.PagoDoble == "Jornada Doble")
            //    {
            //        estadoReal = model.Estado == "Activo" ? (int)EstadoDetalleExtraEnum.Activo : model.Estado == "Anulado" ? (int)EstadoDetalleExtraEnum.Anulado : 0;
            //        doble = true;
            //    }
            //    var registros = servicioTiempoExtra.BuscarTiempoExtraFiltros(model.Cedula != null && model.Cedula.Trim() != "" ? model.Cedula : null, model.FechaDesde, model.FechaHasta,
            //                                                        string.IsNullOrEmpty(model.CodDivision) ? null : model.CodDivision.Split('-')[0],
            //                                                        string.IsNullOrEmpty(model.CodDireccion) ? null : model.CodDireccion.Split('-')[0],
            //                                                        string.IsNullOrEmpty(model.CodDepartamento) ? null : model.CodDepartamento.Split('-')[0],
            //                                                        string.IsNullOrEmpty(model.CodSeccion) ? null : model.CodSeccion.Split('-')[0],
            //                                                        estadoReal, doble);
            //    foreach (var registro in registros)
            //    {
            //        FuncionarioRegistroExtrasVM modelo = new FuncionarioRegistroExtrasVM();
            //        modelo.Funcionario = registro.Funcionario;
            //        modelo.Desglose1 = registro.QuincenaA;
            //        modelo.Desglose2 = registro.QuincenaB;
            //        modelo.Nombramineto = new CNombramientoDTO
            //        {
            //            FecVence = registro.FechVenceNombramiento
            //        };
            //        modelo.RegistroTiempoExtra = registro;

            //        modelo.RegistroTiempoExtra.MontoDiurna = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 240;
            //        modelo.RegistroTiempoExtra.MontoMixta = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 210;
            //        modelo.RegistroTiempoExtra.MontoNocturna = (modelo.Desglose1.MontoSalarioBruto + modelo.Desglose2.MontoSalarioBruto) / 180;

            //        modelo.DetalleExtras = modelo.RegistroTiempoExtra.Detalles.OrderBy(D => D.FechaInicio).ToList();
            //        for (int i = 0; i < modelo.DetalleExtras.Count; i++)
            //        {
            //            if (modelo.DetalleExtras[i].TipoExtra != null)
            //            {
            //                modelo.DetalleExtras[i].Jornada = RegistroTiempoExtraHelper.DefinirJornada(modelo.DetalleExtras[i].TipoExtra.IdEntidad);
            //                List<bool> libres = RegistroTiempoExtraHelper.DefinirDiaLibre(modelo.DetalleExtras[i], modelo.DetalleExtras[i].TipoExtra.DesTipExtra.Contains("doble"));
            //                modelo.DetalleExtras[i].FechaInicioEspecial = libres[0];
            //                modelo.DetalleExtras[i].FechaFinalEspecial = libres[1];
            //            }
            //        }
            //        modelo.H0 = new List<decimal>();
            //        modelo.H1 = new List<decimal>();
            //        modelo.H2 = new List<decimal>();
            //        if (doble)
            //        {
            //            CalcularGuardaDobleVista(modelo, false);
            //        }
            //        else if (!RegistroTiempoExtraHelper.EsAdministrativo(modelo.RegistroTiempoExtra.Ocupacion)
            //                 || (modelo.RegistroTiempoExtra.Clase?.DesClase != null && modelo.RegistroTiempoExtra.Clase.DesClase.Contains(DES_CLASE)))
            //        {
            //            CalcularGuardaVista(modelo, false);
            //        }
            //        else
            //        {
            //            CalcularAdministrativo(modelo);
            //        }
            //        registro.MontoTotal = modelo.RegistroTiempoExtra.MontoTotal;
            //    }
            //    List<string> filtros = new List<string>
            //    {
            //        model.Cedula ?? "",
            //        model.CodDepartamento ?? "",
            //        model.CodDireccion ?? "",
            //        model.CodDivision ?? "",
            //        model.FechaDesde == null ? "" : model.FechaDesde.ToString(),
            //        model.FechaHasta == null ? "" : model.FechaHasta.ToString(),
            //        model.CodSeccion ?? "",
            //        model.Estado == null ? "" : model.Estado,
            //        model.PagoDoble ?? ""
            //    };

            //    List<RegistroTiempoExtraRptData> datosReporte = new List<RegistroTiempoExtraRptData>();

            //    foreach (var item in registros)
            //    {
            //        datosReporte.Add(RegistroTiempoExtraRptData.GenerarDatosReporteGeneral(item, filtros));
            //    }

            //    string reportPath = Path.Combine(Server.MapPath("~/Reports/RegistroTiempoExtra"), "RegistroTiempoExtraGeneral.rpt");
            //    return new CrystalReportPdfResult(reportPath, datosReporte, "PDF");
            //}
        }

        private string ConvertirCedulaHacienda(string cedula)
        {
            if (cedula.StartsWith("00"))
            {
                return "0" + cedula.ElementAt(2) + "0" + cedula.Substring(3);
            }
            else
            {
                return cedula;
            }
        }
    }
}
