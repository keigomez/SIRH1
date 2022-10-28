using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Text.RegularExpressions;
using SIRH.DTO;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.Reports.PDF;
using SIRH.Web.ViewModels;

//using SIRH.Web.ViaticoGastosLocal;
//using SIRH.Web.FuncionarioLocal;

//using SIRH.Web.ViaticoGastosDesa;

using SIRH.Web.ViaticoGastosService;
using SIRH.Web.FuncionarioService;
using SIRH.Web.PuestoService;
using SIRH.Web.PerfilUsuarioService;
//using SIRH.Web.AresepService;

using SIRH.Web.Reports.ViaticoCorrido;
using SIRH.Web.Reports.GastoTransporte;
using SIRH.Web.UserValidation;
using System.Globalization;
using SIRH.Web.Helpers;
using Newtonsoft.Json;

namespace SIRH.Web.Controllers
{
    public class JavaScriptResult : ContentResult
    {
        public JavaScriptResult(string script)
        {
            this.Content = script;
            this.ContentType = "application/javascript";
        }
    }

    public class ViaticoCorridoController : Controller
    {
        #region Variables

        //CDesarraigoServiceClient ServicioDesarraigo = new CDesarraigoServiceClient();
        CViaticoCorridoGastoTransporteServiceClient ServicioViaticoCorridoGastoTransporte = new CViaticoCorridoGastoTransporteServiceClient();
        CPerfilUsuarioServiceClient ServicioUsuario = new CPerfilUsuarioServiceClient();
        CFuncionarioServiceClient ServicioFuncionario = new CFuncionarioServiceClient();
        CPuestoServiceClient ServicioPuesto = new CPuestoServiceClient();
        
        //PliegoTarifarioClient ServicioTarifa = new PliegoTarifarioClient();

        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        #endregion
        #region Metodos Privados
        /// <summary>
        /// Llena una lista con los estados de un Viatico Corrido para visualizarlos en el combobox de la vista, 
        /// incluye los estados extra en caso de que se requieran.
        /// </summary>
        /// <param name="model">View Model que se está manejando en el método.</param>
        /// <param name="extras">Estados extra del ViaticoCorrido (Anulado o Vencido). True: Se quiere incluir los extra. False: No se incluyen los extra.</param>
        private void InsertarEstado(FormularioViaticoCorridoVM model, bool extras)
        {
            List<string> estados = new List<string>();
            estados.Add("Valido");
            estados.Add("Espera");
            if (extras)
            {
                estados.Add("Anulado");
                estados.Add("Vencido");
            }
            model.Estado = new SelectList(estados);
        }
        
        /// <summary>
        /// Llena una lista con los estados de un Gasto de Transporte para visualizarlos en el combobox de la vista, 
        /// incluye los estados extra en caso de que se requieran.
        /// </summary>
        /// <param name="model">View Model que se está manejando en el método.</param>
        /// <param name="extras">Estados extra del GT (Anulado o Vencido). True: Se quiere incluir los extra. False: No se incluyen los extra </param>
        //Se llamaba InsertarEstadoGastoTransporte, pero era confuso. Solo se cambiaron nombres de métodos relacionados al GastoTransporte
        private void LlenarComboEstadoGT(FormularioGastoTransporteVM model, bool extras)
        {
            List<string> estados = new List<string>();
            estados.Add("Valido");
            estados.Add("Espera");
            if (extras)
            {
                estados.Add("Anulado");
                estados.Add("Vencido");
            }
            model.Estado = new SelectList(estados);
        }
        
        /// <summary>
        /// Validaciones extra para el formulario de VC. Verifica que se llenaron los campos: Facturas, Selección del Estado del VC
        /// </summary>
        /// <param name="model">El ViewModel que se está manejando en el método.</param>
        private void ValidarExtraFormularioViaticoCorrido(FormularioViaticoCorridoVM model)
        {
            //if (model.NumCartaPresentacion == null)
            //{
            //    ModelState.AddModelError("Formulario", "El campo N° de Carta de Presentación es obligatorio.");
            //    throw new Exception("Formulario");
            //}
            //if (model.ViaticoCorrido.HojaIndividualizadaDTO == null)
            //{
            //    ModelState.AddModelError("Formulario", "El campo Hoja Individualizada es obligatorio.");
            //    throw new Exception("Formulario");
            //}
            if (model.EstadoSeleccion == null)
            {
                ModelState.AddModelError("Formulario", "El campo Estado del Viático es obligatorio.");
                throw new Exception("Formulario");
            }
            if(model.Facturas == null)
            {
                ModelState.AddModelError("Formulario", "Debe registrar la información de las facturas.");
                throw new Exception("Formulario");
            }               
        }
        
        /// <summary>
        /// Validaciones extra para el formulario de GT. Verifica que se llenaran los campos: Numero Carta de Presentación, Selección del Estado del GT
        /// </summary>
        /// <param name="model">El ViewModel que se está manejando en el método.</param>
        private void ValidarExtraFormularioGastoTransporte(FormularioGastoTransporteVM model)
        {
            bool error = false; //si ocurre alguno de los errores se activa
            if (model.Carta.NumeroCarta == null)
            {
                ModelState.AddModelError("Formulario", "El campo n° de carta de presentación es obligatorio.");
                error = true;
            }
            if (model.EstadoSeleccion == null)
            {
                ModelState.AddModelError("Formulario", "El campo de estado del gasto de transporte es obligatorio.");
                error = true;
            }
            if (model.PresupuestoSelected == null)
            {
                ModelState.AddModelError("Formulario", "El campo de presupuesto es obligatorio.");
                error = true;
            }
            string rgx = "^[1-9]\\d*(\\.\\d+)?$";
            var match = Regex.Match(model.GastoTransporte.MontGastoTransporteDTO, rgx);
            if (!match.Success) { 
            
                ModelState.AddModelError("Formulario", "El campo de monto es obligatorio y solo admite números");
                error = true;
            }
            if (model.GastoTransporte.FecInicioDTO > model.GastoTransporte.FecFinDTO)
            {
                ModelState.AddModelError("Formulario", "La fecha de inicio no puede ser después de la fecha final");
                error = true;
            }
            if (model.GastoTransporte.FecInicioDTO < model.Nombramiento.FecRige)
            {
                ModelState.AddModelError("Formulario", "La fecha de inicio del gasto no puede ser anterior al inicio de nombramiento del funcionario");
                error = true;
            }
            if (model.GastoTransporte.FecFinDTO.Year > model.GastoTransporte.FecInicioDTO.Year)
            {
                ModelState.AddModelError("Formulario", "La fecha final máxima es el 31 de diciembre del mismo año que la fecha de inicio");
                error = true;
            }
            if (model.Nombramiento.FecVence.ToShortDateString() != "01/01/0001")//!= null
            {
                if (model.GastoTransporte.FecFinDTO > Convert.ToDateTime(model.Nombramiento.FecVence))
                {
                    ModelState.AddModelError("Formulario","La fecha final del gasto no puede exceder el periodo de nombramieto del funcionario.");
                    error = true;
                }
            }            
            if (model.Rutas == null)
            {
                ModelState.AddModelError("Formulario", "Debe ingresar la(s) ruta(s) del gasto de transporte");
                error = true;
            }            
            if(error)
            {
                throw new Exception("Formulario");
            }                
        }
        
        /// <summary>
        /// Lista los pagos de meses anteriores del funcionario a quien se hace el GT, además calcula la sumatoria de los meses anteriores. 
        /// </summary>
        /// <param name="model">El ViewModel que se está manejando en método</param>
        private void ListarMesesAnterioresGT(FormularioGastoTransporteVM model)
        {
            var datosMesesA = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnterioresGastoTransporte(model.Funcionario.Cedula);
            model.GastoTransporteList = new List<CGastoTransporteDTO>();
            if (datosMesesA != null)
            {
                double totalMA = 0.0;
                foreach (var item in datosMesesA.ElementAt(1))
                {
                    model.GastoTransporteList.Add((CGastoTransporteDTO)item);
                    totalMA += Convert.ToDouble(((CGastoTransporteDTO)item).MontGastoTransporteDTO);
                }
                model.TotalMA = totalMA;
            }
        }
        /// <summary>
        /// Cargar el combo de codigos presupuestarios para crear un gasto
        /// </summary>
        private void ListarCodigosPresupuesto(FormularioGastoTransporteVM model)
        {
            var codpresupuestos = ServicioPuesto.BuscarPresupuestoParams("");
            //model.CodigosPresupuestoList = new List<string>();
            List<string> tempPresupuestos = new List<string>();
            if (codpresupuestos != null)
            {
                foreach (var i in codpresupuestos.ElementAt(0).OrderBy(Q => Q.CodigoPresupuesto).ToList())
                {
                    tempPresupuestos.Add(i.CodigoPresupuesto);
                }
                model.CodigosPresupuestoList = new SelectList(tempPresupuestos);
            }
        }
        /// <summary>
        /// Llena datos faltantes del modelo DTO de Viatico Corrido para luego enviar a la BD
        /// </summary>
        /// <param name="model">ViewModel que se está manejando en el método.</param>        
        private void LlenarModeloParaEnviar(FormularioViaticoCorridoVM model)
        {
            model.Funcionario.Sexo = GeneroEnum.Indefinido;
            model.ViaticoCorrido.EstadoViaticoCorridoDTO = new CEstadoViaticoCorridoDTO { NomEstadoDTO = model.EstadoSeleccion };
            model.ViaticoCorrido.PresupuestoDTO = new CPresupuestoDTO { CodigoPresupuesto = model.PresupuestoSelected };
            if (model.ContratosArrendamiento == null)
                model.ContratosArrendamiento = new List<CContratoArrendamientoDTO>();
        }

        private void LlenarModeloListadoPagosVC(ListaViaticoCorridoVM model, CBaseDTO[][] datos)
        {
            model.PagoRealizado = false;
            //Cargar combobox de codigos presupuestarios
            var codpresupuestos = ServicioPuesto.BuscarPresupuestoParams("");
            List<string> tempPresupuestos = new List<string>();
            if (codpresupuestos != null)
            {
                foreach (var i in codpresupuestos.ElementAt(0).OrderBy(Q => Q.CodigoPresupuesto).ToList())
                {
                    tempPresupuestos.Add(i.CodigoPresupuesto);
                }
                model.CodigosPresupuestoList = new SelectList(tempPresupuestos);
            }

            model.Viaticos = new List<CViaticoCorridoDTO>();
            foreach (var item in datos)
            {
                CViaticoCorridoDTO datoViatico = new CViaticoCorridoDTO();

                datoViatico = (CViaticoCorridoDTO)item[0];
                datoViatico.Pagos = new List<CPagoViaticoCorridoDTO>();
                datoViatico.Pagos.Add((CPagoViaticoCorridoDTO)item[1]);
                datoViatico.NombramientoDTO = new CNombramientoDTO();
                datoViatico.NombramientoDTO.Funcionario = (CFuncionarioDTO)item[2];
                model.Viaticos.Add(datoViatico);

                if (datoViatico.Pagos[0].NumBoleta != null && datoViatico.Pagos[0].NumBoleta != "")
                    model.PagoRealizado = true;
            }
        }

        /// <summary>
        /// Llena datos faltantes del modelo DTO de Gasto Transporte para luego enviarlos a la BD
        /// </summary>
        /// <param name="model">ViewModel que se está manejando en el método.</param>        
        private void LlenarModeloParaEnviarGastoTransporte(FormularioGastoTransporteVM model)
        {
            model.Funcionario.Sexo = GeneroEnum.Indefinido;
            model.GastoTransporte.EstadoGastoTransporteDTO = new CEstadoGastoTransporteDTO { NomEstadoDTO = model.EstadoSeleccion };
            model.GastoTransporte.PresupuestoDTO = new CPresupuestoDTO { CodigoPresupuesto = model.PresupuestoSelected };
            if (model.ContratosArrendamiento == null)
                model.ContratosArrendamiento = new List<CContratoArrendamientoDTO>();
        }

        private void LlenarModeloListadoPagosGT(ListaGastoTransporteVM model, CBaseDTO[][] datos)
        {
            model.PagoRealizado = false;
            
            //Cargar combobox de codigos presupuestarios
            var codpresupuestos = ServicioPuesto.BuscarPresupuestoParams("");
            //model.CodigosPresupuestoList = new List<string>();
            List<string> tempPresupuestos = new List<string>();
            if (codpresupuestos != null)
            {
                foreach (var i in codpresupuestos.ElementAt(0).OrderBy(Q => Q.CodigoPresupuesto).ToList())
                {
                    tempPresupuestos.Add(i.CodigoPresupuesto);
                }
                model.CodigosPresupuestoList = new SelectList(tempPresupuestos);
            }

            model.Gastos = new List<CGastoTransporteDTO>();
            foreach (var item in datos)
            {
                CGastoTransporteDTO datoGasto = new CGastoTransporteDTO();

                datoGasto = (CGastoTransporteDTO)item[0];
                datoGasto.Pagos = new List<CPagoGastoTransporteDTO>();
                datoGasto.Pagos.Add((CPagoGastoTransporteDTO)item[1]);
                datoGasto.NombramientoDTO = new CNombramientoDTO();
                datoGasto.NombramientoDTO.Funcionario = (CFuncionarioDTO)item[2];
                model.Gastos.Add(datoGasto);

                if (datoGasto.Pagos[0].NumBoleta != null && datoGasto.Pagos[0].NumBoleta != "")
                    model.PagoRealizado = true;
            }
        }
        #endregion

        #region Control

        //---------------------------------------------------------------INDEX------------------------------------------------------//
        /// <summary>
        /// Pagina principal de Módulo de Viatico Corrido y Gasto Transporte tipo GET
        /// </summary>
        /// <example>GET: /ViaticoCorrido/</example>
        /// <returns>Retorna un error o la vista principal del modulo de Viatico Corrido y Gasto de Transporte</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  Pagina principal del segmento de módulo de Viatico Corrido tipo GET
        /// </summary>
        /// <returns>Retorna un error o la vista principal del segmento de Viatico Corrido</returns>
        public ActionResult IndexViaticoCorrido()
        {
            return View();
        }

        /// <summary>
        ///  Pagina principal del segmento de módulo de Gasto de Transporte tipo GET
        /// </summary>
        /// <returns>Retorna un error o la vista principal del segmento de Gasto de Transporte</returns>
        public ActionResult IndexGastoTransporte()
        {         
            //var a = ServicioTarifa.ObtenerPliegoTarifarioAutobus("0");
            return View();
        }

        //--------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------- CREATE -----------------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------//

        //-------------------------------------GET: Create VC----------------------------------//
        /// <summary>
        /// Pagina principal de la creacion de un viatico Corriod
        /// </summary>
        /// <remarks>Solo es el campo para buscar la informacion del funcionario</remarks>
        /// <example>GET: /ViaticoCorrido/CreateRegistroViaticoCorrido</example>
        /// <returns>Retorna un error o la pagina principal para crear un ViaticoCorrido</returns>
        public ActionResult CreateRegistroViaticoCorrido()
        {
            return View();
        }
        
        //-------------------------------------POST: Create VC----------------------------------//
        /// <summary>
        /// Muestra el formulario de creacion
        /// </summary>
        /// <example>POST:/ViaticoCorrido/CreateRegistroViaticoCorrido</example>
        /// <param name="model">El formulario de creacion de un viatico Corrido</param>
        /// <param name="SubmitButton">La accion del metodo(si solo se esta buscando, o si se esta insertando un viatico Corrido)</param>
        /// <returns>Retorna un error o la vista de crecion de un viatico Corrido</returns>
        [HttpPost]
        public ActionResult CreateRegistroViaticoCorrido(FormularioViaticoCorridoVM model, string SubmitButton, string Fformulario)
        {
            string Pview = "";
            string temporal = "";
            try
            {
                if (SubmitButton == "Buscar" && Fformulario != "")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(model.Funcionario.Cedula);
                        var datosDireccion = ServicioFuncionario.DescargarDireccion(model.Funcionario.Cedula);
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                            {
                                if (datosDireccion[0].GetType() != typeof(CErrorDTO))
                                {
                                    model.Direccion = (CDireccionDTO)datosDireccion[0];
                                    model.Direccion.Distrito = (CDistritoDTO)datosDireccion[3];
                                    model.Direccion.Distrito.Canton = (CCantonDTO)datosDireccion[2];
                                    model.Direccion.Distrito.Canton.Provincia = (CProvinciaDTO)datosDireccion[1];
                                }
                                model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                                var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                                {
                                    model.UbicacionContrato = ubicacion;
                                    model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                                }
                                else {
                                    model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                                    model.UbicacionTrabajo = ubicacion;
                                }
                                model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                                temporal = model.Nombramiento.FecVence.Date.ToString("yyyy-MM-dd");
                                model.Fechalimite = temporal;


                                var listaEstados = new List<int> { 1, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                                if (model.Nombramiento.EstadoNombramiento.IdEntidad == 2)
                                {
                                    model.Nombramiento.EstadoNombramiento.DesEstado = "Interino";
                                }
                                else
                                {
                                    if (listaEstados.Contains(model.Nombramiento.EstadoNombramiento.IdEntidad))
                                    {
                                        model.Nombramiento.EstadoNombramiento.DesEstado = "Propiedad";
                                    }
                                    else {
                                        ModelState.AddModelError("Busqueda", "No tiene un nombramiento válido" );
                                        throw new Exception("Busqueda");
                                    }
                                }
                                
                                var datosMesesA = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnteriores(model.Funcionario.Cedula);
                                model.ViaticoCorridoList = new List<CViaticoCorridoDTO>();
                                if (datosMesesA != null)
                                {
                                    double totalMA = 0.0;
                                    foreach (var item in datosMesesA.ElementAt(1))
                                    {
                                        model.ViaticoCorridoList.Add((CViaticoCorridoDTO)item);
                                        totalMA = Convert.ToDouble(((CViaticoCorridoDTO)item).MontViaticoCorridoDTO) + totalMA;
                                    }
                                    model.TotalMA = totalMA;
                                }
                                InsertarEstado(model, false);
                                //var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
                                var localizacion = ServicioPuesto.GetLocalizacion(true, model.Direccion.Distrito.Canton.IdEntidad, true, true, model.Direccion.Distrito.Canton.Provincia.IdEntidad);
                                model.Provincias = new SelectList(localizacion.ElementAt(2).Select(Q => new SelectListItem
                                {
                                    Value = ((CProvinciaDTO)Q).IdEntidad.ToString(),
                                    Text = ((CProvinciaDTO)Q).NomProvincia
                                }), "Value", "Text");
                                model.Cantones = new SelectList(localizacion.ElementAt(0).Select(Q => new SelectListItem
                                {
                                    Value = ((CCantonDTO)Q).IdEntidad.ToString(),
                                    Text = ((CCantonDTO)Q).NomCanton
                                }), "Value", "Text");
                                model.Distritos = new SelectList(localizacion.ElementAt(1).Select(Q => new SelectListItem
                                {
                                    Value = ((CDistritoDTO)Q).IdEntidad.ToString(),
                                    Text = ((CDistritoDTO)Q).NomDistrito
                                }), "Value", "Text");

                                //Cargar combobox de codigos presupuestarios
                                var codpresupuestos = ServicioPuesto.BuscarPresupuestoParams("");
                                //model.CodigosPresupuestoList = new List<string>();
                                List<string> tempPresupuestos = new List<string>();
                                if (codpresupuestos != null)
                                {
                                    foreach (var i in codpresupuestos.ElementAt(0).OrderBy(Q => Q.CodigoPresupuesto).ToList())
                                    {
                                        tempPresupuestos.Add(i.CodigoPresupuesto);
                                    }
                                    model.CodigosPresupuestoList = new SelectList(tempPresupuestos);
                                }

                                if (Fformulario == "1")
                                {
                                    model.Formulario = "PI-568 Asignación";
                                    Pview = "_FormularioAsignacionViaticoCorrido";
                                }
                                else if (Fformulario == "2")
                                {
                                    var datosViatico = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorridoActual(model.Funcionario.Cedula);
                                    if (datosViatico.GetType() != typeof(CErrorDTO))
                                    {
                                        model.ViaticoCorrido = (CViaticoCorridoDTO)datosViatico;

                                        // FormularioMotivoDeduccionVM model = new FormularioMotivoDeduccionVM();
                                        var datosincapacidad = ServicioViaticoCorridoGastoTransporte.ListarRegistroIncapacidad(model.Funcionario.Cedula);
                                        model.Incapacidades = new SelectList(datosincapacidad.Select(Q => new SelectListItem
                                        {
                                            Value = Convert.ToString((((CRegistroIncapacidadDTO)Q).FecVence - ((CRegistroIncapacidadDTO)Q).FecRige).TotalDays),
                                            Text = "Fecha: " + ((CRegistroIncapacidadDTO)Q).FecRige.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).FecVence.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).CodIncapacidad
                                        }), "Value", "Text");
                                        var datosVC = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnteriores(model.Funcionario.Cedula);
                                        model.ViaticoCorridoList = new List<CViaticoCorridoDTO>();
                                        if (datosMesesA != null)
                                        {
                                            double totalMA = 0.0;
                                            foreach (var item in datosMesesA.ElementAt(1))
                                            {
                                                model.ViaticoCorridoList.Add((CViaticoCorridoDTO)item);
                                                totalMA = Convert.ToDouble(((CViaticoCorridoDTO)item).MontViaticoCorridoDTO) + totalMA;
                                            }
                                            model.TotalMA = totalMA;
                                        }

                                        if (model.ViaticoCorridoList.Count == 0)
                                        {
                                            ModelState.AddModelError("Formulario", "No tiene Pagos Anteriores");
                                            throw new Exception("Pagos Anteriores");
                                        }
                                        else
                                        {
                                            var fecha = model.ViaticoCorridoList[0].FecInicioDTO;
                                            var difMeses = ((model.ViaticoCorridoList[0].FecFinDTO.Year - model.ViaticoCorridoList[0].FecInicioDTO.Year) * 12) + model.ViaticoCorridoList[0].FecFinDTO.Month - model.ViaticoCorridoList[0].FecInicioDTO.Month;
                                            //var i = model.ViaticoCorridoList[0].FecInicioDTO.Month;
                                            List<SelectListItem> listaMeses = new List<SelectListItem>();

                                            for (int i = 0; i <= difMeses; i++)
                                            {
                                                var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                                                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                                                fecha = fecha.AddMonths(1);
                                            }
                                            model.MesesViatico = listaMeses;
                                        }
                               
                                        model.Formulario = "PI-1104 Deducción";
                                        Pview = "_FormularioDeduccionViaticoCorrido";
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Formulario", ((CErrorDTO)datosViatico).MensajeError);
                                        throw new Exception("Formulario");
                                    }
                                }
                                else if (Fformulario == "3")
                                {
                                    var datosViatico = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorridoActual(model.Funcionario.Cedula);

                                    if (datosViatico.GetType() != typeof(CErrorDTO))
                                    {
                                        //var datoCarta = ServicioViaticoCorridoGastoTransporte.BuscarCartaPresentacionCedula(model.Funcionario);
                                        model.ViaticoCorrido = (CViaticoCorridoDTO)datosViatico;
                                        //model.Carta = (CCartaPresentacionDTO)datoCarta;
                                        model.Formulario = "PI-1103 Eliminación";
                                        Pview = "_FormularioEliminacionViaticoCorrido";              
                                    }
                                    else {
                                        ModelState.AddModelError("Formulario", ((CErrorDTO)datosViatico).MensajeError);
                                        throw new Exception("Formulario");
                                    }
                                }
                                return PartialView(Pview, model);
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda",
                                    ((CErrorDTO)datosFuncionario[4]).MensajeError
                                );
                                throw new Exception("Busqueda");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda",
                                ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.Clear();
                    int idcod = Convert.ToInt32(model.DistritoSeleccion);

                    model.ViaticoCorrido.NombramientoDTO = model.Nombramiento;
                    model.ViaticoCorrido.NombramientoDTO.EstadoNombramiento = model.Nombramiento.EstadoNombramiento;
                    if (idcod != 0)
                    {
                        var datoslocalizacion = ServicioViaticoCorridoGastoTransporte.CargarDistritoId(idcod);

                        model.Dist = (CDistritoDTO)datoslocalizacion;
                        model.ViaticoCorrido.NomDistritoDTO = model.Dist;
                    }
                    else
                    {
                        var datoslocalizacion = ServicioViaticoCorridoGastoTransporte.CargarDistritoId(model.Direccion.Distrito.IdEntidad);
                        model.Dist = (CDistritoDTO)datoslocalizacion;
                        model.ViaticoCorrido.NomDistritoDTO = model.Dist;
                    }

                    var errors = ModelState
                     .Where(x => x.Value.Errors.Count > 0)
                     .Select(x => new { x.Key, x.Value.Errors })
                     .ToArray();
                    if (model.ViaticoCorrido != null)
                    {
                        ValidarExtraFormularioViaticoCorrido(model);
                        var carta = new CCartaPresentacionDTO { NumeroCarta = model.NumCartaPresentacion };
                        LlenarModeloParaEnviar(model);
                        var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarViaticoCorrido(carta, model.Funcionario, model.ViaticoCorrido, model.Facturas.ToArray(),
                                                                             model.ContratosArrendamiento.ToArray());
                        if (respuesta.GetType() == typeof(CErrorDTO))
                        {
                            //Response.Write("Respuesta es CErrorDTO: " + respuesta.Mensaje +"\n");
                            ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta).MensajeError);
                            throw new Exception("Formulario");
                        }

                        //return Content("Intentó ir a página de detalles: "+ respuesta.Mensaje + " y " + respuesta.IdEntidad);   ((CRespuestaDTO)respuesta)
                        return JavaScript("window.location = '/ViaticoCorrido/DetailsViaticoCorrido?codigo=" + respuesta.IdEntidad + "&accion=Guardar';");
                    }
                    else
                    {
                        //Response.Write("model.ViaticoCorrido venía null");
                        //return Content("model.ViaticoCorrido venía null");
                        ValidarExtraFormularioViaticoCorrido(model);
                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Cayó al catch con error: " + ex.Message);
                //return Content("Cayó al catch con error: " + ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------GET: Create Pago VC----------------------------------//
        public ActionResult CreatePagoViaticoCorrido(string numCedula, int MesPago, int AnioPago)
        {
            try
            {
                FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();

                var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(numCedula);
                if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                    {
                        var datosDireccion = ServicioFuncionario.DescargarDireccion(numCedula);
                        if (datosDireccion[0].GetType() != typeof(CErrorDTO))
                        {
                            model.Direccion = (CDireccionDTO)datosDireccion[0];
                            model.Direccion.Distrito = (CDistritoDTO)datosDireccion[3];
                            model.Direccion.Distrito.Canton = (CCantonDTO)datosDireccion[2];
                            model.Direccion.Distrito.Canton.Provincia = (CProvinciaDTO)datosDireccion[1];
                        }
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                        if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                        {
                            model.UbicacionContrato = ubicacion;
                            model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                        }
                        else
                        {
                            model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                            model.UbicacionTrabajo = ubicacion;
                        }
                        model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                        model.Fechalimite = model.Nombramiento.FecVence.Date.ToString("yyyy-MM-dd");

                        InsertarEstado(model, false);

                        // Buscar Información del Viático.
                        var datosViatico = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorridoActual(numCedula);
                        if (datosViatico.GetType() != typeof(CErrorDTO))
                        {
                            model.ViaticoCorrido = (CViaticoCorridoDTO)datosViatico;
                            model.EstadoSeleccion = model.ViaticoCorrido.EstadoViaticoCorridoDTO.IdEntidad.ToString();
                            model.ViaticoCorrido.FecPagoDTO = DateTime.Today;

                            var fecha = model.ViaticoCorrido.FecInicioDTO;
                            var difMeses = ((model.ViaticoCorrido.FecFinDTO.Year - model.ViaticoCorrido.FecInicioDTO.Year) * 12) + model.ViaticoCorrido.FecFinDTO.Month - model.ViaticoCorrido.FecInicioDTO.Month;
                            //var i = model.ViaticoCorridoList[0].FecInicioDTO.Month;
                            List<SelectListItem> listaMeses = new List<SelectListItem>();

                            for (int i = 0; i <= difMeses; i++)
                            {
                                var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                                fecha = fecha.AddMonths(1);
                            }

                            model.MesesViatico = listaMeses;
                            //if(fechaPago != null)
                            //{
                            //    model.MesSeleccion = fechaPago.ToShortDateString();
                            //}
                            model.MesSeleccion = new DateTime(AnioPago, MesPago, 1).ToShortDateString();
                            model.MesPago = MesPago;
                            model.AnioPago = AnioPago;
                            model.ReservaRecurso = "S/R";


                            /////////////////////////////
                            ///////////////////////////
                            decimal monPago = Convert.ToDecimal(model.ViaticoCorrido.MontViaticoCorridoDTO);
                            decimal monDia = Decimal.Round((monPago / 26), 2);
                            model.ViaticoCorrido.FecPagoDTO = Convert.ToDateTime(model.MesSeleccion);

                            var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagar(model.ViaticoCorrido, model.ViaticoCorrido.FecPagoDTO.Month, model.ViaticoCorrido.FecPagoDTO.Year);
                            if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                var maxDias = ((CDetallePagoViaticoCorridoDTO)diasPagar.FirstOrDefault()).IdEntidad;
                                if (maxDias < 26)
                                    monPago = Decimal.Round((monDia * maxDias), 2);
                            }

                            CPagoViaticoCorridoDTO pago = new CPagoViaticoCorridoDTO
                            {
                                FecPago = model.ViaticoCorrido.FecPagoDTO,
                                HojaIndividualizada = "",  // model.ViaticoCorrido.Pagos[0].HojaIndividualizada,
                                NumBoleta = "",
                                ReservaRecurso = model.ReservaRecurso,
                                ViaticoCorridoDTO = new CViaticoCorridoDTO { IdEntidad = model.ViaticoCorrido.IdEntidad },
                                IndEstado = 1
                            };

                            List<CDetallePagoViaticoCorridoDTO> detalles = new List<CDetallePagoViaticoCorridoDTO>();
                            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajar(model.Funcionario, model.ViaticoCorrido, model.ViaticoCorrido.FecPagoDTO.Month, model.ViaticoCorrido.FecPagoDTO.Year, monDia);

                            foreach (CDetallePagoViaticoCorridoDTO item in datos)
                            {
                                detalles.Add(new CDetallePagoViaticoCorridoDTO
                                {
                                    FecDiaPago = item.FecDiaPago,
                                    MonPago = item.MonPago,
                                    CodEntidad = item.CodEntidad,
                                    TipoDetalleDTO = item.TipoDetalleDTO
                                });

                                if (item.TipoDetalleDTO.IdEntidad == 5)
                                    monPago += item.MonPago;
                                else
                                    monPago -= item.MonPago;
                            }

                            pago.MonPago = monPago;
                            pago.Detalles = detalles;
                            model.ViaticoCorrido.Pagos.Clear();
                            model.ViaticoCorrido.Pagos.Add(pago);
                            //////////////////////////////////

                            return View(model);
                            //return View("_FormularioPagoViaticoCorrido", model);
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)datosViatico).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario[4]).MensajeError);
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------POST: Create Pago VC----------------------------------//
        [HttpPost]
        public ActionResult CreatePagoViaticoCorrido(FormularioViaticoCorridoVM model, string SubmitButton)
        {            
            try
            {
                if (model.MesSeleccion == null)
                {
                    ModelState.AddModelError("Error", "Debe seleccionar el mes a Pagar");
                    throw new Exception("Error");
                }
                if (model.ReservaRecurso == null || model.ReservaRecurso == "" )
                {
                    ModelState.AddModelError("Error", "Debe digitar la Reserva Recurso");
                    throw new Exception("Error");
                }

                //if (model.ViaticoCorrido.Pagos[0].HojaIndividualizada == null)
                //{
                //    ModelState.AddModelError("Formulario", "Debe digitar la información de Hoja Individualizada");
                //    throw new Exception("Formulario");
                //}

                model.ViaticoCorrido.NombramientoDTO = model.Nombramiento;
                
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                         .Select(x => new { x.Key, x.Value.Errors })
                                         .ToArray();

                if (model.ViaticoCorrido != null)
                {
                    LlenarModeloParaEnviar(model);
                    decimal monPago = Convert.ToDecimal(model.ViaticoCorrido.MontViaticoCorridoDTO);
                    decimal monDia = Decimal.Round((monPago / 26), 2);
                    model.ViaticoCorrido.FecPagoDTO = Convert.ToDateTime(model.MesSeleccion);

                    var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagar(model.ViaticoCorrido, model.ViaticoCorrido.FecPagoDTO.Month, model.ViaticoCorrido.FecPagoDTO.Year);
                    if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        var maxDias = ((CDetallePagoViaticoCorridoDTO)diasPagar.FirstOrDefault()).IdEntidad;
                        if (maxDias  < 26)
                            monPago = Decimal.Round((monDia * maxDias), 2);
                    }

                    CPagoViaticoCorridoDTO pago = new CPagoViaticoCorridoDTO
                    {
                        FecPago = model.ViaticoCorrido.FecPagoDTO,
                        HojaIndividualizada = "",  // model.ViaticoCorrido.Pagos[0].HojaIndividualizada,
                        NumBoleta = "",
                        ReservaRecurso = model.ReservaRecurso,
                        ViaticoCorridoDTO = new CViaticoCorridoDTO { IdEntidad = model.ViaticoCorrido.IdEntidad },
                        IndEstado = 1
                    };

                    List<CDetallePagoViaticoCorridoDTO> detalles = new List<CDetallePagoViaticoCorridoDTO>();
                    var datos = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajar(model.Funcionario, model.ViaticoCorrido, model.ViaticoCorrido.FecPagoDTO.Month, model.ViaticoCorrido.FecPagoDTO.Year, monDia);

                    foreach (CDetallePagoViaticoCorridoDTO item in datos)
                    {
                        detalles.Add(new CDetallePagoViaticoCorridoDTO
                        {
                            FecDiaPago = item.FecDiaPago,
                            MonPago = item.MonPago,
                            CodEntidad = item.CodEntidad,
                            TipoDetalleDTO = item.TipoDetalleDTO
                        });

                        if (item.TipoDetalleDTO.IdEntidad == 5)
                            monPago += item.MonPago;
                        else
                            monPago -= item.MonPago;
                    }

                    pago.MonPago = monPago;
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarPagoViaticoCorrido(pago, detalles.ToArray(), model.Funcionario);
                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta).Mensaje);
                        throw new Exception("Formulario");
                    }
                    //return JavaScript("window.location = '/ViaticoCorrido/DetailsPagoViaticoCorrido?codigo=" + ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar';");
                    return new JavaScriptResult("ObtenerDetalleRegistroPago(" + ((CRespuestaDTO)respuesta).Contenido + ");");
                }
                else
                {
                    ValidarExtraFormularioViaticoCorrido(model);
                    throw new Exception("Formulario");
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }



        //-------------------------------------POST: Create Eliminación VC----------------------------------//
        /// <summary>
        /// Agrega un registro de DetalleEliminación de viatico corrido a la BD
        /// </summary>
        /// <example>POST:/ViaticoCorrido/CreateRegistroViaticoCorrido</example>
        /// <param name="model">El formulario de creacion de un viatico Corrido</param>
        /// <returns>Retorna la vista de error o la vista de detalles de la eliminación del viatico, el registro se agrega correctamente</returns>
        [HttpPost]
        public ActionResult CreateDeleteViaticoCorrido(FormularioViaticoCorridoVM model, string SubmitButton, string Fformulario)
        {
            try
            {
                model.MovimientoVC.Nomtipo = 3;
                model.MovimientoVC.EstadoDTO = 1;
                model.MovimientoVC.ViaticoCorridoDTO = model.ViaticoCorrido;
                var errors = ModelState
                 .Where(x => x.Value.Errors.Count > 0)
                 .Select(x => new { x.Key, x.Value.Errors })
                 .ToArray();
                if (model.ViaticoCorrido != null)
                {
                    //model.DetalleEliminacion.FecFinDTO = model.DetalleEliminacion.FecInicioDTO;
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarDetalleEliminacionViaticoCorrido(model.DetalleEliminacion, model.MovimientoVC);
                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta[0]).MensajeError);
                        throw new Exception("Formulario");
                    }
                    model.MovimientoViaticoCorrido = (CMovimientoViaticoCorridoDTO)respuesta[0];
                    model.Eliminacion = (CDetalleEliminacionViaticoCorridoGastoTransporteDTO)respuesta[1];
                    model.ViaticoCorrido = (CViaticoCorridoDTO)model.MovimientoVC.ViaticoCorridoDTO;
                    model.NumCartaPresentacion = model.Carta.NumeroCarta;
                    return PartialView("_DetailsDeleteViaticoCorrido", model);
                }
                else
                {
                    ValidarExtraFormularioViaticoCorrido(model);
                    throw new Exception("Formulario");
                }
            }
            catch
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------POST: Create Deducción VC----------------------------------//
        /// <summary>
        /// Agrega un registro de deducción en la BD
        /// </summary>
        /// <example>POST:/ViaticoCorrido/CreateRegistroViaticoCorrido</example>
        /// <param name="model">El formulario de creacion de un viatico Corrido</param>
        /// <returns>Retorna un error o la vista de crecion de un viatico Corrido</returns>
        /// 

        private FormularioViaticoCorridoVM CargarModeloDeduccionVC(FormularioViaticoCorridoVM model, CBaseDTO[][] datosMesesA)
        {
            var datosincapacidad = ServicioViaticoCorridoGastoTransporte.ListarRegistroIncapacidad(model.Funcionario.Cedula);
            model.Incapacidades = new SelectList(datosincapacidad.Select(Q => new SelectListItem
            {
                Value = Convert.ToString((((CRegistroIncapacidadDTO)Q).FecVence - ((CRegistroIncapacidadDTO)Q).FecRige).TotalDays),
                Text = "Fecha: " + ((CRegistroIncapacidadDTO)Q).FecRige.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).FecVence.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).CodIncapacidad
            }), "Value", "Text");
            var datosVC = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnteriores(model.Funcionario.Cedula);
            model.ViaticoCorridoList = new List<CViaticoCorridoDTO>();
            if (datosMesesA != null)
            {
                double totalMA = 0.0;
                foreach (var item in datosMesesA.ElementAt(1))
                {
                    model.ViaticoCorridoList.Add((CViaticoCorridoDTO)item);
                    totalMA = Convert.ToDouble(((CViaticoCorridoDTO)item).MontViaticoCorridoDTO) + totalMA;
                }
                model.TotalMA = totalMA;
            }

            if (model.ViaticoCorridoList.Count == 0)
            {
                ModelState.AddModelError("Formulario", "No tiene Pagos Anteriores");
                throw new Exception("Pagos Anteriores");
            }
            else
            {
                var fecha = model.ViaticoCorridoList[0].FecInicioDTO;
                var difMeses = ((model.ViaticoCorridoList[0].FecFinDTO.Year - model.ViaticoCorridoList[0].FecInicioDTO.Year) * 12) + model.ViaticoCorridoList[0].FecFinDTO.Month - model.ViaticoCorridoList[0].FecInicioDTO.Month;
                //var i = model.ViaticoCorridoList[0].FecInicioDTO.Month;
                List<SelectListItem> listaMeses = new List<SelectListItem>();

                for (int i = 0; i <= difMeses; i++)
                {
                    var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                    listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                    fecha = fecha.AddMonths(1);
                }
                model.MesesViatico = listaMeses;
            }

            return model;
        }

        public ActionResult CreateDeduccionViaticoCorrido(string numCedula, string fechaPago)
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();
            try
            {
                var datosViatico = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorridoActual(numCedula);
                if (datosViatico.GetType() != typeof(CErrorDTO))
                {
                    model.ViaticoCorrido = (CViaticoCorridoDTO)datosViatico;
                    var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(numCedula);
                    if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                        if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                        {
                            model.UbicacionContrato = ubicacion;
                            model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                        }
                        else
                        {
                            model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                            model.UbicacionTrabajo = ubicacion;
                        }
                        model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                        throw new Exception("Busqueda");
                    }

                    var datosMesesA = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnteriores(numCedula);
                    model.ViaticoCorridoList = new List<CViaticoCorridoDTO>();
                    if (datosMesesA != null)
                    {
                        double totalMA = 0.0;
                        foreach (var item in datosMesesA.ElementAt(1))
                        {
                            model.ViaticoCorridoList.Add((CViaticoCorridoDTO)item);
                            totalMA = Convert.ToDouble(((CViaticoCorridoDTO)item).MontViaticoCorridoDTO) + totalMA;
                        }
                        model.TotalMA = totalMA;
                    }

                    model = CargarModeloDeduccionVC(model, datosMesesA);
                    model.MesSeleccion = fechaPago;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("Formulario", ex.Message);
                //return PartialView("_ErrorViaticoCorrido");
                model.Error = new CErrorDTO { IdEntidad = -1, MensajeError = ex.Message };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateDeduccionViaticoCorrido(FormularioViaticoCorridoVM model, string SubmitButton, string Fformulario)
        {
            try
            {
                if (model.DetalleD == null)
                {
                    ModelState.AddModelError("Formulario", "Debe indicar al menos un motivo para la deducción");
                    throw new Exception("Formulario");
                }

                if (model.MesSeleccion == null)
                {
                    ModelState.AddModelError("Formulario", "Debe seleccionar un mes para el registro de deducción");
                    throw new Exception("Formulario");
                }

                model.MovimientoVC.FecMovimientoDTO = Convert.ToDateTime(model.MesSeleccion);
                model.MovimientoVC.Nomtipo = 2;
                model.MovimientoVC.EstadoDTO = 1;
                model.MovimientoVC.ViaticoCorridoDTO = model.ViaticoCorridoList[0];

                var errors = ModelState
                 .Where(x => x.Value.Errors.Count > 0)
                 .Select(x => new { x.Key, x.Value.Errors })
                 .ToArray();
                if (model.ViaticoCorridoList[0] != null)
                {
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarDetalleDedcuccionViaticoCorrido(model.DetalleD.ToArray(), model.MovimientoVC);
                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta[0]).MensajeError);
                        throw new Exception("Formulario");
                    }
                    model.MovimientoViaticoCorrido = (CMovimientoViaticoCorridoDTO)respuesta[0];
                    model.ViaticoCorrido = (CViaticoCorridoDTO)model.ViaticoCorridoList[0];
                    model.Deduccion = new List<CDetalleDeduccionViaticoCorridoDTO>();
                    if (respuesta != null)
                    {
                        foreach (var item in model.DetalleD)
                        {
                            model.Deduccion.Add((CDetalleDeduccionViaticoCorridoDTO)item);
                        }
                    }

                    return PartialView("_DetailsDeduccionViaticoCorrido", model);
                }
                else
                {
                    ValidarExtraFormularioViaticoCorrido(model);
                    throw new Exception("Formulario");
                }
            }
            catch
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }


        public ActionResult CreateReintegroDiasVC(string numCedula, string fechaPago)
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();
            try
            {
                var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(numCedula);
                if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                        if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                        {
                            model.UbicacionContrato = ubicacion;
                            model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                        }
                        else
                        {
                            model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                            model.UbicacionTrabajo = ubicacion;
                        }
                        model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                        model.Fechalimite = model.Nombramiento.FecVence.Date.ToString("yyyy-MM-dd");

                        var listaEstados = new List<int> { 1, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                        if (model.Nombramiento.EstadoNombramiento.IdEntidad == 2)
                        {
                            model.Nombramiento.EstadoNombramiento.DesEstado = "Interino";
                        }
                        else
                        {
                            if (listaEstados.Contains(model.Nombramiento.EstadoNombramiento.IdEntidad))
                            {
                                model.Nombramiento.EstadoNombramiento.DesEstado = "Propiedad";
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "No tiene un nombramiento válido");
                                throw new Exception("Busqueda");
                            }
                        }

                        InsertarEstado(model, false);

                        var datosViatico = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorridoActual(model.Funcionario.Cedula);
                        if (datosViatico.GetType() != typeof(CErrorDTO))
                        {
                            model.ViaticoCorrido = (CViaticoCorridoDTO)datosViatico;

                            List<DateTime> diasReintegro = new List<DateTime>();
                            List<CDetalleDeduccionViaticoCorridoDTO> listaDetalle = new List<CDetalleDeduccionViaticoCorridoDTO>();
                                                        
                            var listadoPagos = model.ViaticoCorrido.Pagos.Where(Q => Q.IndEstado == 1).ToList();                          

                            foreach (var pagos in listadoPagos)
                            {
                                foreach (var detalle in pagos.Detalles)
                                {
                                    // Listar los días que ya se han reintegrado
                                    if (detalle.TipoDetalleDTO.IdEntidad == 5)  // 5 - Reintegro
                                        diasReintegro.Add(Convert.ToDateTime(detalle.FecDiaPago));
                                }
                            }

                            //Pago retroactivo. Se debe verificar que no se hayan hecho los pagos de meses anteriores
                            bool agregar = false;
                            decimal monto = 0;
                            DateTime fechaInicio = model.ViaticoCorrido.FecInicioDTO;
                            fechaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
                            DateTime fechaFinal = Convert.ToDateTime(fechaPago);

                            while (fechaInicio < fechaFinal)
                            {
                                if (listadoPagos.Count > 0)
                                {
                                    if (listadoPagos.Where(Q => Q.FecPago.Month == fechaInicio.Month && Q.FecPago.Year == fechaInicio.Year && Q.NumBoleta == "").FirstOrDefault() != null
                                        && diasReintegro.Contains(fechaInicio) == false)
                                    {
                                        monto = listadoPagos.Where(Q => Q.FecPago.Month == fechaInicio.Month && Q.FecPago.Year == fechaInicio.Year && Q.NumBoleta == "").FirstOrDefault().MonPago;
                                        agregar = true;
                                    }    
                                }
                                else
                                {
                                    agregar = true;
                                }

                                if (agregar)
                                {
                                    listaDetalle.Add(new CDetalleDeduccionViaticoCorridoDTO
                                    {
                                        DesMotivoDTO = "PAGO RETROACTIVO " + CultureInfo.CurrentUICulture.TextInfo.ToUpper(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fechaInicio.Month - 1].ToString()),
                                        FecRigeDTO = fechaInicio.ToShortDateString(),
                                        FecVenceDTO = fechaInicio.ToShortDateString(),
                                        MontMontoRebajarDTO = monto.ToString(), // model.ViaticoCorrido.MontViaticoCorridoDTO.ToString(),
                                        MontMontoBajarDTO = monto.ToString()  //model.ViaticoCorrido.MontViaticoCorridoDTO.ToString()
                                    });
                                }

                                fechaInicio = fechaInicio.AddMonths(1);
                                agregar = false;
                            }

                            foreach (var pagos in listadoPagos)
                            {
                                foreach (var detalle in pagos.Detalles)
                                {
                                    // Listar las deducciones que se le han hecho y no se le han devuelto
                                    if (detalle.TipoDetalleDTO.IdEntidad != 5 && diasReintegro.Contains(Convert.ToDateTime(detalle.FecDiaPago)) == false)  // 5 - Reintegro
                                        listaDetalle.Add(new CDetalleDeduccionViaticoCorridoDTO
                                        {
                                            DesMotivoDTO = detalle.TipoDetalleDTO.DescripcionTipo,
                                            FecRigeDTO = detalle.FecDiaPago,
                                            FecVenceDTO = detalle.FecDiaPago,
                                            MontMontoRebajarDTO = detalle.MonPago.ToString(),
                                            MontMontoBajarDTO = detalle.MonPago.ToString()
                                        });
                                }
                            }
                            if (listaDetalle.Count == 0)
                                throw new Exception("No tiene deducciones para su reintegro");

                            model.DetalleD = listaDetalle;
                            model.Formulario = "Reintegro de Días";
                            //Pview = "_FormularioReintegroViaticoCorrido";
                        }

                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario[4]).MensajeError);
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("Formulario", ex.Message);
                //return PartialView("_ErrorViaticoCorrido");
                model.Error = new CErrorDTO{ IdEntidad = -1,  MensajeError = ex.Message };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateReintegroDiasVC(FormularioViaticoCorridoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Agregar Reintegro")
                {
                    ModelState.Clear();

                    if (model.MovimientoVC.ObsObservacionesDTO == null)
                        throw new Exception("Debe digitar el Motivo del Reintegro");
                    else if (model.MovimientoVC.ObsObservacionesDTO.TrimEnd().Length == 0)
                        throw new Exception("Debe digitar el Motivo del Reintegro");

                    if (model.ViaticoCorrido != null)
                    {
                        List<CViaticoCorridoReintegroDTO> lista = new List<CViaticoCorridoReintegroDTO>();
                        foreach (var item in model.DetalleD.Where(Q => Q.IndReintegroDTO).ToList())
                        {
                            lista.Add(new CViaticoCorridoReintegroDTO
                            {
                                FecDiaDTO = Convert.ToDateTime(item.FecRigeDTO),
                                MonReintegroDTO = Convert.ToDecimal(item.MontMontoBajarDTO),
                                ObsMotivoDTO = model.MovimientoVC.ObsObservacionesDTO,
                                EstadoDTO = 1,
                                ViaticoCorridoDTO = model.ViaticoCorrido
                            });
                        }
                        LlenarModeloParaEnviar(model);
                        var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarReintegro(lista.ToArray());
                        if (respuesta.GetType() == typeof(CErrorDTO))
                        {
                            ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta).MensajeError);
                            throw new Exception("Formulario");
                        }

                        //return JavaScript("window.location = '/ViaticoCorrido/DetailsViaticoCorrido?codigo=" + respuesta.IdEntidad + "&accion=Guardar';");
                        return new JavaScriptResult("ObtenerDetalleReintegroPago('" + model.Funcionario.Cedula + "','" + model.ViaticoCorrido.IdEntidad + "','" + model.ViaticoCorrido.MontViaticoCorridoDTO + "','Viatico');");
                    }
                    else
                    {
                        ValidarExtraFormularioViaticoCorrido(model);
                        throw new Exception("Formulario");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }


        public ActionResult EditPagoReservaRecursoVC()
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();
            List<SelectListItem> listaMeses = new List<SelectListItem>();
            var fecha = new DateTime(DateTime.Today.Year, 1, 1);

            for (int i = 0; i <= 11; i++)
            {
                var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i].ToString()) + " del " + DateTime.Today.Year.ToString();
                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                fecha = fecha.AddMonths(1);
            }
            model.MesesViatico = listaMeses;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPagoReservaRecursoVC(FormularioViaticoCorridoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (model.MesSeleccion != null)
                    {
                        var fecha = Convert.ToDateTime(model.MesSeleccion);
                        var datos = ServicioViaticoCorridoGastoTransporte.ListarPagosViaticoCorrido(fecha.Month, fecha.Year);
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            //Cargar combobox de codigos presupuestarios
                            var codpresupuestos = ServicioPuesto.BuscarPresupuestoParams("");
                            //model.CodigosPresupuestoList = new List<string>();
                            List<string> tempPresupuestos = new List<string>();
                            if (codpresupuestos != null)
                            {
                                foreach (var i in codpresupuestos.ElementAt(0))
                                {
                                    tempPresupuestos.Add(i.CodigoPresupuesto);
                                }
                                model.CodigosPresupuestoList = new SelectList(tempPresupuestos);
                            }

                            CPagoViaticoCorridoDTO pago = new CPagoViaticoCorridoDTO();
                            model.ViaticoCorrido = new CViaticoCorridoDTO { IdEntidad = 1 }; // Un registro de Viático Vacío
                            model.ViaticoCorrido.Pagos = new List<CPagoViaticoCorridoDTO>();

                            foreach (var item in datos)
                            {
                                pago = (CPagoViaticoCorridoDTO)item[0];
                                if(pago.IndEstado == 1)
                                {
                                    pago.ViaticoCorridoDTO = (CViaticoCorridoDTO)item[1];
                                    pago.ViaticoCorridoDTO.NombramientoDTO.Funcionario = (CFuncionarioDTO)item[2];
                                    model.ViaticoCorrido.Pagos.Add(pago);
                                }
                            }
                            
                            return View("_EditPagoReservaRecursoVC", model);
                        }
                        else
                        {
                            //ModelState.AddModelError("Busqueda", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        throw new Exception("Debe escoger el Mes");
                    }
                }
                else
                {
                    if(model.PresupuestoSelected != null)
                    {
                        if (model.ReservaRecurso != null && model.ReservaRecurso !="")
                        {
                            var listado = model.ViaticoCorrido.Pagos.Where(Q => Q.ViaticoCorridoDTO.PresupuestoDTO.CodigoPresupuesto.Contains(model.PresupuestoSelected)).ToList();

                            foreach (var pago in listado)
                            {
                                pago.ReservaRecurso = model.ReservaRecurso;
                                var datos = ServicioViaticoCorridoGastoTransporte.AsignarReservaRecurso(pago);

                                if(datos.GetType() == typeof(CErrorDTO))
                                {
                                    ModelState.Clear();
                                    throw new Exception(((CErrorDTO)datos).MensajeError);
                                }
                            }
 
                            return View("EditPagoReservaRecursoVC", model);
                        }
                        else
                        {
                            ModelState.Clear();
                            throw new Exception("Debe digitar la Reserva Recurso");
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        throw new Exception("Debe escoger Código Presupuestario");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------GET: Create GT ----------------------------------//
        /// <summary>
        /// Pagina principal de la creacion de un Gasto de Transporte
        /// </summary>
        /// <remarks>Solo es el campo para buscar la informacion del funcionario</remarks>
        /// <example>GET: /ViaticoCorrido/CreateRegistroGastoTransporte</example>
        /// <returns>Retorna un error o la pagina principal para crear un GastoTransporte</returns>
        /// 
        public ActionResult CreateRegistroGastoTransporte()
        {         
            return View();
        }
        
        //-------------------------------------POST: Create GT ----------------------------------//
        /// <summary>
        /// Muestra el formulario de creacion y crea un Gasto de Transporte
        /// </summary>
        /// <example>POST:/ViaticoCorrido/CreateRegistroGastoTransporte</example>
        /// <param name="model">El formulario de creacion de un gasto transporte</param>
        /// <param name="SubmitButton">La accion del metodo(si solo se esta buscando, o si se esta insertando un gasto transporte)</param>
        /// <returns>Retorna un error o la vista de crecion de un gasto transporte</returns>
        [HttpPost]
        public ActionResult CreateRegistroGastoTransporte(FormularioGastoTransporteVM model, string SubmitButton, string Fformulario)
        {
            string Pview = ""; //El tipo de formulario (asignación/deducción/eliminación) que se mostrará de acuerdo a la acción del usuario.
            try
            {
                if (SubmitButton == "Buscar" && Fformulario != "")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(model.Funcionario.Cedula);
                        var datosDireccion = ServicioFuncionario.DescargarDireccion(model.Funcionario.Cedula);
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            //Si sí trae la ubicación del puesto 
                            if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                            {
                                //Si sí trae el funcionario 
                                if (datosDireccion[0].GetType() != typeof(CErrorDTO))
                                {
                                    model.Direccion = (CDireccionDTO)datosDireccion[0];
                                    model.Direccion.Distrito = (CDistritoDTO)datosDireccion[3];
                                    model.Direccion.Distrito.Canton = (CCantonDTO)datosDireccion[2];
                                    model.Direccion.Distrito.Canton.Provincia = (CProvinciaDTO)datosDireccion[1];
                                }
                                model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                                var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                                {
                                    model.UbicacionContrato = ubicacion;
                                    model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                                }
                                else
                                {
                                    model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                                    model.UbicacionTrabajo = ubicacion;
                                }
                                model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                                model.Fechalimite = model.Nombramiento.FecVence.Date.ToString("yyyy-MM-dd");                                

                                //Clasificar en interino/propiedad los tipos de nombramiento que apliquen
                                if (model.Nombramiento.EstadoNombramiento.IdEntidad == 2)
                                {
                                    model.Nombramiento.EstadoNombramiento.DesEstado = "Interino";
                                }
                                else
                                {
                                    var lsTipoNombram = new List<int> { 1, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 }; //1:Propiedad, 9:AscensoInterino, 20:RegresoAlTrabajo, 21:AscensoEnPropiedad , 22:DescensoEnPropiedad
                                    //Si el nombramiento que viene está en la lista (aplica), se clasifica como 'propiedad'
                                    if (lsTipoNombram.Contains(model.Nombramiento.EstadoNombramiento.IdEntidad))
                                    {
                                        model.Nombramiento.EstadoNombramiento.DesEstado = "Propiedad";
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Busqueda", "El funcionario no tiene un nombramiento válido");
                                        throw new Exception("Busqueda");
                                    }
                                }

                                //Cargar combobox de codigos presupuestarios
                                ListarCodigosPresupuesto(model);
                                //Calcular el total de gastos de transporte hechos por el funcionario
                                ListarMesesAnterioresGT(model);                                
                                //Llenar combobox del estado en la vista
                                LlenarComboEstadoGT(model, false);
                                
                                var localizacion = ServicioPuesto.GetLocalizacion(true, model.Direccion.Distrito.Canton.IdEntidad, true, true, model.Direccion.Distrito.Canton.Provincia.IdEntidad);
                                model.Distritos = new SelectList(localizacion.ElementAt(1).Select(Q => new SelectListItem
                                {
                                    Value = ((CDistritoDTO)Q).IdEntidad.ToString(),
                                    Text = ((CDistritoDTO)Q).NomDistrito
                                }), "Value", "Text");

                                //___________________________________ Verificar Formulario Seleccionado ____________________________________
                                if (Fformulario == "1") // Asignación
                                {
                                    //model.RutasARESEP = JsonConvert.DeserializeObject<List<Models.RutasARESEPModel>>(ReadJson());
                                    model.Formulario = "PI-555 Asignación";
                                    Pview = "_FormularioAsignacionGastoTransporte";
                                }
                                else if (Fformulario == "2") //Deducción
                                {
                                    var datosGasto = ServicioViaticoCorridoGastoTransporte.ObtenerGastoTransporteActual(model.Funcionario.Cedula);
                                    if(datosGasto.GetType() != typeof(CErrorDTO))
                                    {
                                        model.GastoTransporte = (CGastoTransporteDTO)datosGasto;

                                        var datosincapacidad = ServicioViaticoCorridoGastoTransporte.ListarRegistroIncapacidad(model.Funcionario.Cedula);
                                        model.Incapacidades = new SelectList(datosincapacidad.Select(Q => new SelectListItem
                                        {
                                            Value = Convert.ToString((((CRegistroIncapacidadDTO)Q).FecVence - ((CRegistroIncapacidadDTO)Q).FecRige).TotalDays),
                                            Text = "Fecha: " + ((CRegistroIncapacidadDTO)Q).FecRige.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).FecVence.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).CodIncapacidad
                                        }), "Value", "Text");

                                        //Calcular el total de GT de los meses anteriores para el funcionario 
                                        ListarMesesAnterioresGT(model);
                                        
                                        if (model.GastoTransporteList.Count == 0)
                                        {
                                            ModelState.AddModelError("Formulario", "No tiene Pagos Anteriores");
                                            throw new Exception("Pagos Anteriores");
                                        }
                                        else
                                        {
                                            var fecha = model.GastoTransporteList[0].FecInicioDTO;
                                            var difMeses = ((model.GastoTransporteList[0].FecFinDTO.Year - model.GastoTransporteList[0].FecInicioDTO.Year) * 12) + model.GastoTransporteList[0].FecFinDTO.Month - model.GastoTransporteList[0].FecInicioDTO.Month;
                                            //var i = model.ViaticoCorridoList[0].FecInicioDTO.Month;

                                            List<SelectListItem> listaMeses = new List<SelectListItem>();
                                            for (int i = 0; i <= difMeses; i++)
                                            {
                                                var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                                                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                                                fecha = fecha.AddMonths(1);
                                            }
                                            model.MesesGasto = listaMeses;
                                        }

                                        model.Formulario = "PI-1105 Deducción";
                                        Pview = "_FormularioDeduccionGastoTransporte";
                                    }
                                }
                                else if (Fformulario == "3") //Eliminación
                                {
                                    var datosGasto = ServicioViaticoCorridoGastoTransporte.ObtenerGastoTransporteActual(model.Funcionario.Cedula);

                                    if (datosGasto.GetType() != typeof(CErrorDTO))
                                    {
                                        model.GastoTransporte = (CGastoTransporteDTO)datosGasto;
                                        model.Carta = new CCartaPresentacionDTO{ NumeroCarta = "No encontrado" };
                                        
                                        //Carta es null por default, pero si encuentra un registro de carta para el funcionario lo usa
                                        CCartaPresentacionDTO carta = new CCartaPresentacionDTO();
                                        var datoCarta = ServicioViaticoCorridoGastoTransporte.BuscarCartaPresentacionCedula(model.Funcionario);
                                        if (datoCarta.GetType() != typeof(CErrorDTO))
                                        {
                                            model.Carta = (CCartaPresentacionDTO)datoCarta;
                                        }
                                        
                                        model.Formulario = "PI-1106 Eliminación";
                                        Pview = "_FormularioEliminacionGastoTransporte";
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Formulario", ((CErrorDTO)datosGasto).MensajeError);
                                        throw new Exception("Formulario");
                                    }
                                }

                                return PartialView(Pview, model);
                            }
                            else
                            {
                                //La ubicación del puesto viene null
                                ModelState.AddModelError("Busqueda", 
                                    ((CErrorDTO)datosFuncionario[4]).MensajeError);
                                throw new Exception("Busqueda");
                            }
                        }
                        else
                        {
                            //Error al obtener el funcionario señalado
                            ModelState.AddModelError("Busqueda",
                                ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        //ModelState is FALSE
                        throw new Exception("Busqueda");
                    }
                }
                else //Hace clic al botón GUARDAR REGISTRO.. _______________________________________________________________
                {
                    //ModelState.Clear();
                    model.GastoTransporte.NombramientoDTO = model.Nombramiento;
                    
                    int idcod = Convert.ToInt32(model.DistritoSeleccion);
                    if (idcod != 0)
                    {
                        var datoslocalizacion = ServicioViaticoCorridoGastoTransporte.CargarDistritoId(idcod);
                        model.Dist = (CDistritoDTO)datoslocalizacion;
                    }
                    else
                    {
                        var datoslocalizacion = ServicioViaticoCorridoGastoTransporte.CargarDistritoId(model.Direccion.Distrito.IdEntidad);
                        model.Dist = (CDistritoDTO)datoslocalizacion;
                    }
                    
                    //Obtener la lista de errores si ocurrieron
                    var errors = ModelState
                     .Where(x => x.Value.Errors.Count > 0)
                     .Select(x => new { x.Key, x.Value.Errors })
                     .ToArray();
                    
                    if (model.GastoTransporte != null)
                    {
                        LlenarModeloParaEnviarGastoTransporte(model);
                        ValidarExtraFormularioGastoTransporte(model);
                        
                        var carta = new CCartaPresentacionDTO { NumeroCarta = model.Carta.NumeroCarta };
                        var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarGastoTransporte(carta, model.Funcionario, model.GastoTransporte, model.Rutas.ToArray());
                            
                        //Response.Write(respuesta[0].GetType());
                        if (respuesta[0].GetType() == typeof(CErrorDTO))
                        //if (respuesta.GetType() == typeof(CErrorDTO))
                        {
                            ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta[0]).Mensaje);
                            throw new Exception("Formulario");
                        }
                        else
                        {
                            return JavaScript("window.location = '/ViaticoCorrido/DetailsGastoTransporte?codigo=" + respuesta[0].IdEntidad + "&accion=Guardar';");
                        }                        
                    }
                    else
                    {
                        //GT es null
                        ValidarExtraFormularioGastoTransporte(model);
                        throw new Exception("Formulario");
                    }
                    
                }

            }
            catch (Exception ex)
            {
                //return Content("\n Controller Line 1309. Catch: " + ex.Message);
                return PartialView("_ErrorGastoTransporte");
                throw new Exception(ex.Message);
            }

        }

        //-------------------------------------GET: Create Pago GT ----------------------------------//
        /// <summary>
        /// Obtiene los datos o información del funcionario y del gasto de transporte al que el usuario desea generar el pago, realiza
        /// además el cálculo del periodo de meses por los que se asignó ese gasto.
        /// </summary>
        /// <param name="numCedula">Número de cédula del funcionario al que se realizó el gasto de transporte</param>
        /// <returns>Llamada al método POST CreatePagoGT para mostrar la vista que permite realizar el pago</returns>
        public ActionResult CreatePagoGastoTransporte(string numCedula, int MesPago, int AnioPago)
        {
            try
            {
                FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();

                var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(numCedula);
                if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                    {
                        var datosDireccion = ServicioFuncionario.DescargarDireccion(numCedula);
                        if (datosDireccion[0].GetType() != typeof(CErrorDTO))
                        {
                            model.Direccion = (CDireccionDTO)datosDireccion[0];
                            model.Direccion.Distrito = (CDistritoDTO)datosDireccion[3];
                            model.Direccion.Distrito.Canton = (CCantonDTO)datosDireccion[2];
                            model.Direccion.Distrito.Canton.Provincia = (CProvinciaDTO)datosDireccion[1];
                        }
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                        if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                        {
                            model.UbicacionContrato = ubicacion;
                            model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                        }
                        else
                        {
                            model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                            model.UbicacionTrabajo = ubicacion;
                        }
                        model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                        model.Fechalimite = model.Nombramiento.FecVence.Date.ToString("yyyy-MM-dd");

                        LlenarComboEstadoGT(model, false);

                        // Buscar Información del gasto.
                        var datosGasto = ServicioViaticoCorridoGastoTransporte.ObtenerGastoTransporteActual(numCedula);
                        if (datosGasto.GetType() != typeof(CErrorDTO))
                        {
                            model.GastoTransporte = (CGastoTransporteDTO)datosGasto;
                            model.EstadoSeleccion = model.GastoTransporte.EstadoGastoTransporteDTO.IdEntidad.ToString();
                            model.GastoTransporte.FecPagoDTO = DateTime.Today;

                            var fecha = model.GastoTransporte.FecInicioDTO;
                            var difMeses = ((model.GastoTransporte.FecFinDTO.Year - model.GastoTransporte.FecInicioDTO.Year) * 12) + model.GastoTransporte.FecFinDTO.Month - model.GastoTransporte.FecInicioDTO.Month;
                            //var i = model.ViaticoCorridoList[0].FecInicioDTO.Month;
                            List<SelectListItem> listaMeses = new List<SelectListItem>();

                            for (int i = 0; i <= difMeses; i++)
                            {
                                var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                                fecha = fecha.AddMonths(1);
                            }
                            model.MesesGasto = listaMeses;
                            model.MesSeleccion = new DateTime(AnioPago, MesPago, 1).ToShortDateString();
                            model.MesPago = MesPago;
                            model.AnioPago = AnioPago;
                            model.ReservaRecurso = "S/R";


                            /////////////////////////////
                            ///////////////////////////
                            decimal monPago = Convert.ToDecimal(model.GastoTransporte.MontGastoTransporteDTO);
                            decimal monDia = Decimal.Round((monPago / 22), 2);
                            model.GastoTransporte.FecPagoDTO = Convert.ToDateTime(model.MesSeleccion);

                            var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagarGT(model.GastoTransporte, model.GastoTransporte.FecPagoDTO.Month, model.GastoTransporte.FecPagoDTO.Year);
                            if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                var maxDias = ((CDetallePagoGastoTrasporteDTO)diasPagar.FirstOrDefault()).IdEntidad;
                                if (maxDias < 22)
                                    monPago = Decimal.Round((monDia * maxDias), 2);
                            }

                            CPagoGastoTransporteDTO pago = new CPagoGastoTransporteDTO
                            {
                                FecPago = model.GastoTransporte.FecPagoDTO,
                                HojaIndividualizada = "",  // model.ViaticoCorrido.Pagos[0].HojaIndividualizada,
                                NumBoleta = "",
                                ReservaRecurso = model.ReservaRecurso,
                                GastoTransporteDTO = new CGastoTransporteDTO { IdEntidad = model.GastoTransporte.IdEntidad },
                                IndEstado = 1
                            };

                            List<CDetallePagoGastoTrasporteDTO> detalles = new List<CDetallePagoGastoTrasporteDTO>();
                            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajarGT(model.Funcionario, model.GastoTransporte, model.GastoTransporte.FecPagoDTO.Month, model.GastoTransporte.FecPagoDTO.Year, monDia);

                            foreach (CDetallePagoGastoTrasporteDTO item in datos)
                            {
                                detalles.Add(new CDetallePagoGastoTrasporteDTO
                                {
                                    FecDiaPago = item.FecDiaPago,
                                    MonPago = item.MonPago,
                                    CodEntidad = item.CodEntidad,
                                    TipoDetalleDTO = item.TipoDetalleDTO
                                });

                                if (item.TipoDetalleDTO.IdEntidad == 5)
                                    monPago += item.MonPago;
                                else
                                    monPago -= item.MonPago;
                            }

                            pago.MonPago = monPago;
                            pago.Detalles = detalles;
                            model.GastoTransporte.Pagos.Clear();
                            model.GastoTransporte.Pagos.Add(pago);
                            //////////////////////////////////

                            return View(model);
                            //return View("_FormularioPagoViaticoCorrido", model);
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)datosGasto).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario[4]).MensajeError);
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                return PartialView("_ErrorGastoTransporte");
            }
        }

        //-------------------------------------POST: Create Pago GT ----------------------------------//
        /// <summary>
        /// Encargado de realizar validaciones y mostrar el formulario para crear un pago del gasto de transporte 
        /// (los pagos solo pueden realizarse al mes anterior al actual)
        /// </summary>
        /// <param name="model">ViewModel gasto de transporte</param>
        /// <returns>Vista de detalles del pago de gasto de transporte</returns>
        [HttpPost]
        public ActionResult CreatePagoGastoTransporte(FormularioGastoTransporteVM model)
        {
            try
            {
                //Verificar que seleccione a cuál mes quiere generar el pago
                if (model.MesSeleccion == null)
                {
                    ModelState.AddModelError("Error", "Debe seleccionar el mes a pagar");
                    throw new Exception("Error");
                }

                if (model.ReservaRecurso == null || model.ReservaRecurso == "")
                {
                    ModelState.AddModelError("Error", "Debe digitar la Reserva Recurso");
                    throw new Exception("Error");
                }

                model.GastoTransporte.NombramientoDTO = model.Nombramiento;

                var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                                         .Select(x => new { x.Key, x.Value.Errors })
                                         .ToArray();

                //Si el gasto no viene vacío
                if (model.GastoTransporte != null)
                {
                    LlenarModeloParaEnviarGastoTransporte(model);
                    decimal monPago = Convert.ToDecimal(model.GastoTransporte.MontGastoTransporteDTO);
                    decimal monDia = decimal.Round((monPago / 22), 2);
                    model.GastoTransporte.FecPagoDTO = Convert.ToDateTime(model.MesSeleccion);

                    var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagarGT(model.GastoTransporte, model.GastoTransporte.FecPagoDTO.Month, model.GastoTransporte.FecPagoDTO.Year);
                    if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        var maxDias = ((CDetallePagoGastoTrasporteDTO)diasPagar.FirstOrDefault()).IdEntidad;
                        if (maxDias < 22)
                            monPago = Decimal.Round((monDia * maxDias), 2);
                    }

                    //Poner los datos del pago en un objeto
                    CPagoGastoTransporteDTO pago = new CPagoGastoTransporteDTO
                    {
                        FecPago = model.GastoTransporte.FecPagoDTO,
                        HojaIndividualizada = "",  // model.ViaticoCorrido.Pagos[0].HojaIndividualizada,
                        NumBoleta = "",
                        ReservaRecurso = model.ReservaRecurso,
                        GastoTransporteDTO = new CGastoTransporteDTO { IdEntidad = model.GastoTransporte.IdEntidad },
                        IndEstado = 1
                    };
                    //Obtener los detalles del pago (dias)
                    List<CDetallePagoGastoTrasporteDTO> detalles = new List<CDetallePagoGastoTrasporteDTO>();
                    var datos = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajarGT(model.Funcionario, model.GastoTransporte, model.GastoTransporte.FecPagoDTO.Month, model.GastoTransporte.FecPagoDTO.Year, monDia);
                    foreach (CDetallePagoGastoTrasporteDTO item in datos)
                    {
                        detalles.Add(new CDetallePagoGastoTrasporteDTO
                        {
                            FecDiaPago = item.FecDiaPago,
                            MonPago = item.MonPago,
                            CodEntidad = item.CodEntidad,
                            TipoDetalleDTO = item.TipoDetalleDTO
                        });
                        monPago -= item.MonPago;
                    }

                    pago.MonPago = monPago;
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarPagoGastoTransporte(pago, detalles.ToArray(), model.Funcionario);
                    if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO) || respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Error", respuesta.Mensaje);
                        throw new Exception("Error");
                    }

                    return new JavaScriptResult("ObtenerDetalleRegistroPago(" + ((CRespuestaDTO)respuesta).Contenido + ");");
                    //return JavaScript("window.location = '/ViaticoCorrido/DetailsPagoGastoTransporte?codigo=" + ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar';");
                }
                else
                {
                    ValidarExtraFormularioGastoTransporte(model);
                    throw new Exception("Formulario");
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------POST: Create Eliminación GT----------------------------------//
        /// <summary>
        /// Agrega un registro de DetalleEliminación de gasto de transporte a la BD
        /// </summary>
        /// <example>POST:/ViaticoCorrido/CreateRegistroGastoTransporte</example>
        /// <param name="model">El formulario de creacion de un gasto de transporte</param>
        /// <returns>Retorna un error o la vista de crecion de un viatico Corrido</returns>
        [HttpPost]
        public ActionResult CreateDeleteGastoTransporte(FormularioGastoTransporteVM model, string SubmitButton, string Fformulario)
        {
            try
            {
                model.MovimientoGT.FecMovimientoDTO = DateTime.Now;//.Now.ToString("MM/dd/yyyy");
                model.MovimientoGT.Nomtipo = 3;
                model.MovimientoGT.EstadoDTO = 1;                
                model.MovimientoGT.GastoTransporteDTO = model.GastoTransporte;
                
                var errors = ModelState
                 .Where(x => x.Value.Errors.Count > 0)
                 .Select(x => new { x.Key, x.Value.Errors })
                 .ToArray();
                if (model.DetalleEliminacion != null)
                {
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarDetalleEliminacionGastoTransporte(model.DetalleEliminacion, model.MovimientoGT);
                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta[0]).MensajeError);
                        throw new Exception("Formulario");
                    }
                    model.MovimientoGastoTransporte = (CMovimientoGastoTransporteDTO)respuesta[0];
                    model.Eliminacion = (CDetalleEliminacionViaticoCorridoGastoTransporteDTO)respuesta[1];
                    model.GastoTransporte = model.MovimientoGT.GastoTransporteDTO;
                    model.NumCartaPresentacion = model.NumCartaPresentacion;
                    return PartialView("_DetailsDeleteGastoTransporte", model);
                }
                else
                {
                    ValidarExtraFormularioGastoTransporte(model);
                    throw new Exception("Formulario");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------POST: Create Deducción GT----------------------------------//
        /// <summary>
        /// Agrega un registro de deducción en la BD
        /// </summary>
        /// <example>POST:/ViaticoCorrido/CreateRegistroGastoTransporte</example>
        /// <param name="model">El ViewModel con atributos necesarios para crear una deducción de un gasto transporte</param>        
        /// <returns>Retorna un error o la vista de deducción de un gasto de transporte</returns>

        private FormularioGastoTransporteVM CargarModeloDeduccionGT(FormularioGastoTransporteVM model, CBaseDTO[][] datosMesesA)
        {
            var datosincapacidad = ServicioViaticoCorridoGastoTransporte.ListarRegistroIncapacidad(model.Funcionario.Cedula);
            model.Incapacidades = new SelectList(datosincapacidad.Select(Q => new SelectListItem
            {
                Value = Convert.ToString((((CRegistroIncapacidadDTO)Q).FecVence - ((CRegistroIncapacidadDTO)Q).FecRige).TotalDays),
                Text = "Fecha: " + ((CRegistroIncapacidadDTO)Q).FecRige.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).FecVence.ToShortDateString() + " - " + ((CRegistroIncapacidadDTO)Q).CodIncapacidad
            }), "Value", "Text");
            var datosVC = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnterioresGastoTransporte(model.Funcionario.Cedula);
            model.GastoTransporteList = new List<CGastoTransporteDTO>();
            if (datosMesesA != null)
            {
                double totalMA = 0.0;
                foreach (var item in datosMesesA.ElementAt(1))
                {
                    model.GastoTransporteList.Add((CGastoTransporteDTO)item);
                    totalMA = Convert.ToDouble(((CGastoTransporteDTO)item).MontGastoTransporteDTO) + totalMA;
                }
                model.TotalMA = totalMA;
            }

            if (model.GastoTransporteList.Count == 0)
            {
                ModelState.AddModelError("Formulario", "No tiene Pagos Anteriores");
                throw new Exception("Pagos Anteriores");
            }
            else
            {
                var fecha = model.GastoTransporteList[0].FecInicioDTO;
                var difMeses = ((model.GastoTransporteList[0].FecFinDTO.Year - model.GastoTransporteList[0].FecInicioDTO.Year) * 12) + model.GastoTransporteList[0].FecFinDTO.Month - model.GastoTransporteList[0].FecInicioDTO.Month;
                //var i = model.ViaticoCorridoList[0].FecInicioDTO.Month;
                List<SelectListItem> listaMeses = new List<SelectListItem>();

                for (int i = 0; i <= difMeses; i++)
                {
                    var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fecha.Month - 1].ToString()) + " del " + fecha.Year.ToString();
                    listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                    fecha = fecha.AddMonths(1);
                }
                model.MesesGasto = listaMeses;
            }

            return model;
        }

        public ActionResult CreateDeduccionGastoTransporte(string numCedula, string fechaPago)
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();
            try
            {
                var datosGasto = ServicioViaticoCorridoGastoTransporte.ObtenerGastoTransporteActual(numCedula);
                if (datosGasto.GetType() != typeof(CErrorDTO))
                {
                    model.GastoTransporte = (CGastoTransporteDTO)datosGasto;
                    var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(numCedula);
                    if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                        if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                        {
                            model.UbicacionContrato = ubicacion;
                            model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                        }
                        else
                        {
                            model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                            model.UbicacionTrabajo = ubicacion;
                        }
                        model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                        throw new Exception("Busqueda");
                    }

                    var datosMesesA = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnterioresGastoTransporte(numCedula);
                    model.GastoTransporteList = new List<CGastoTransporteDTO>();
                    if (datosMesesA != null)
                    {
                        double totalMA = 0.0;
                        foreach (var item in datosMesesA.ElementAt(1))
                        {
                            model.GastoTransporteList.Add((CGastoTransporteDTO)item);
                            totalMA = Convert.ToDouble(((CGastoTransporteDTO)item).MontGastoTransporteDTO) + totalMA;
                        }
                        model.TotalMA = totalMA;
                    }

                    model = CargarModeloDeduccionGT(model, datosMesesA);
                    model.MesSeleccion = fechaPago;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        [HttpPost]
        public ActionResult CreateDeduccionGastoTransporte(FormularioGastoTransporteVM model, string SubmitButton, string Fformulario)
        {
            try
            {
                if (model.DetalleD == null)
                {
                    ModelState.AddModelError("Formulario", "Debe indicar al menos un motivo para la deducción");
                    throw new Exception("Formulario");
                }

                if (model.MesSeleccion == null)
                {
                    ModelState.AddModelError("Formulario", "Debe seleccionar un mes para el registro de deducción");
                    throw new Exception("Formulario");
                }

                model.MovimientoGT.FecMovimientoDTO = Convert.ToDateTime(model.MesSeleccion);
                model.MovimientoGT.Nomtipo = 2;
                model.MovimientoGT.EstadoDTO = 1;
                model.MovimientoGT.GastoTransporteDTO = model.GastoTransporteList[0];

                var errors = ModelState
                 .Where(x => x.Value.Errors.Count > 0)
                 .Select(x => new { x.Key, x.Value.Errors })
                 .ToArray();
                if (model.GastoTransporteList[0] != null)
                {
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarDetalleDedcuccionGastoTransporte(model.DetalleD.ToArray(), model.MovimientoGT);
                    if (respuesta.GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta[0]).MensajeError);
                        throw new Exception("Formulario");
                    }
                    model.MovimientoGastoTransporte = (CMovimientoGastoTransporteDTO)respuesta[0];
                    model.GastoTransporte = model.GastoTransporteList[0];
                    model.Deduccion = new List<CDetalleDeduccionGastoTransporteDTO>();
                    if (respuesta != null)
                    {
                        foreach (var item in model.DetalleD)
                        {
                            model.Deduccion.Add(item);
                        }
                    }

                    return PartialView("_DetailsDeduccionGastoTransporte", model);
                }
                else
                {
                    ValidarExtraFormularioGastoTransporte(model);
                    throw new Exception("Formulario");
                }
            }
            catch
            {
                return PartialView("_ErrorViaticoCorrido");
            }

        }


        //--------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------- Edit (Anular) ----------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------//

        //-------------------------------------GET: Edit Pago VC----------------------------------//
        /// <summary>
        /// Obtiene el registro de pago de VC que corresponda al código indicado y lo agrega al 
        /// ViewModel de viatico para luego mostrar la vista.
        /// </summary>
        /// <param name="codigo">Codigo(PK) del pago de viatico</param>
        /// <returns></returns>
        public ActionResult EditPagoViaticoCorrido (int codigo)
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();

            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerPagoViaticoCorrido(codigo);

            if (datos.Count() > 1)
            {
                model.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][1];
                model.ViaticoCorrido.Pagos[0] = (CPagoViaticoCorridoDTO)datos[0][0];
                model.Funcionario = (CFuncionarioDTO)datos[0][2];
                model.Puesto = ((CPuestoDTO)datos[0][4]);
                model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][5]).NumeroCarta;
                model.DetallePuesto = ((CDetallePuestoDTO)datos[0][6]);
                var ubicacion = (CUbicacionPuestoDTO)datos[0][7];
                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                {
                    model.UbicacionContrato = ubicacion;
                    model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[0][8];
                }
                else
                {
                    model.UbicacionContrato = (CUbicacionPuestoDTO)datos[0][8];
                    model.UbicacionTrabajo = ubicacion;
                }
                model.Facturas = datos.ElementAt(1).Select(F => (CFacturaDesarraigoDTO)F).ToList();
                model.ContratosArrendamiento = datos.ElementAt(2).Select(C => (CContratoArrendamientoDTO)C).ToList();
                model.EstadoSeleccion = model.ViaticoCorrido.EstadoViaticoCorridoDTO.NomEstadoDTO;
            }
            else
            {
                model.Error = (CErrorDTO)datos[0][0];
            }

            return View(model);
        }
        //-------------------------------------POST: Edit Pago VC----------------------------------//
        /// <summary>
        /// Muestra la vista de detalles cuando se anula un pago de viatico 
        /// </summary>
        /// <param name="codigo">Código(PK) del pago de viatico</param>
        /// <param name="model">ViewModel de viatico</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPagoViaticoCorrido(int codigo, FormularioViaticoCorridoVM model)
        {
            try
            {
                if (model.ViaticoCorrido.ObsViaticoCorridoDTO != null)
                {
                    model.ViaticoCorrido.Pagos[0].IdEntidad = codigo;
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AnularPagoViaticoCorrido(model.ViaticoCorrido.Pagos[0]);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.ViaticoCorrido), Convert.ToInt32(EAccionesBitacora.Editar), 0,
                                                CAccesoWeb.ListarEntidades(typeof(CPagoViaticoCorridoDTO).Name));

                        //return JavaScript("window.location = '/ViaticoCorrido/DetailsPagoViaticoCorrido?codigo=" + codigo + "&accion=modificar" + "'");
                        return new JavaScriptResult("ObtenerDetalleAnularPago(" + codigo + ");");
                    }
                    else
                    {
                        ModelState.AddModelError("modificar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para anular este pago de viatico corrido");
                    throw new Exception();
                }
            }
            catch
            {
                return View(model);
            }
        }


        //-------------------------------------GET: Edit Pago GT----------------------------------//
        /// <summary>
        /// Obtiene el registro de pago de GT que corresponda al código indicado y lo agrega al 
        /// ViewModel de gasto para luego mostrar la vista.
        /// </summary>
        /// <param name="codigo">Codigo(PK) del pago de gasto</param>
        /// <returns></returns>
        public ActionResult EditPagoGastoTransporte(int codigo)
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();

            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerPagoGastoTransporte(codigo);

            if (datos.Count() >= 1)
            {
                model.GastoTransporte = (CGastoTransporteDTO)datos[0][1];
                model.GastoTransporte.Pagos[0] = (CPagoGastoTransporteDTO)datos[0][0];
                model.Funcionario = (CFuncionarioDTO)datos[0][2];
                model.Nombramiento = (CNombramientoDTO)datos[0][3];
                //model.Puesto = ((CPuestoDTO)datos[0][4]);
                model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][4]).NumeroCarta;
                //model.DetallePuesto = ((CDetallePuestoDTO)datos[0][6]);
                //model.UbicacionContrato = ((CUbicacionPuestoDTO)datos[0][7]);
                //model.UbicacionTrabajo = ((CUbicacionPuestoDTO)datos[0][8]);
                //model.Facturas = datos.ElementAt(1).Select(F => (CFacturaDesarraigoDTO)F).ToList();
                //model.ContratosArrendamiento = datos.ElementAt(2).Select(C => (CContratoArrendamientoDTO)C).ToList();
                model.EstadoSeleccion = model.GastoTransporte.EstadoGastoTransporteDTO.NomEstadoDTO;
            }
            else
            {
                model.Error = (CErrorDTO)datos[0][0];
            }

            return View(model);
        }
        
        //-------------------------------------POST: Edit Pago GT----------------------------------//
        /// <summary>
        /// Muestra la vista de detalles cuando se anula un pago de gasto 
        /// </summary>
        /// <param name="codigo">Código(PK) del pago de gasto</param>
        /// <param name="model">ViewModel de gasto</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPagoGastoTransporte(int codigo, FormularioGastoTransporteVM model)
        {
            try
            {
                if (model.GastoTransporte.ObsGastoTransporteDTO != null)
                {
                    model.GastoTransporte.Pagos[0].IdEntidad = codigo;
                    var respuesta = ServicioViaticoCorridoGastoTransporte.AnularPagoGastoTransporte(model.GastoTransporte.Pagos[0]);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.ViaticoCorrido), Convert.ToInt32(EAccionesBitacora.Editar), 0,
                                                CAccesoWeb.ListarEntidades(typeof(CPagoGastoTransporteDTO).Name));

                        return new JavaScriptResult("ObtenerDetalleAnularPago(" + codigo + ");");
                        //return JavaScript("window.location = '/ViaticoCorrido/DetailsPagoGastoTransporte?codigo=" + codigo + "&accion=modificar" + "'");
                        //return RedirectToAction("Details", new { codigo = codigo, accion = "modificar" });
                    }
                    else
                    {
                        ModelState.AddModelError("modificar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para anular este pago de gasto de transporte");
                    throw new Exception();
                }
            }
            catch
            {
                return View(model);
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------//
        //---------------------------------------------------------------Search-----------------------------------------------------//
        //--------------------------------------------------------------------------------------------------------------------------//

        //-------------------------------------GET: Search VC----------------------------------//    
        /// <summary>
        /// Llena variables y muestra la vista de buscar viático corrido (formulario de búsqueda)
        /// </summary>
        /// <example>GET: /ViaticoCorrido/SearchViaticoCorrido/</example>
        /// <returns>Retorna la vista de buscar viatico corrido o la vista parcial de error</returns>
        public ActionResult SearchViaticoCorrido()
        {
            BusquedaViaticoCorridoVM model = new BusquedaViaticoCorridoVM();

            List<string> estados = new List<string>();
            estados.Add("Valido");
            estados.Add("Espera");
            estados.Add("Anulado");
            estados.Add("Vencido");
            estados.Add("Vencido_PSS");
            estados.Add("Vencido_Vac");
            estados.Add("Vencido_Incap");
            estados.Add("Todas las Notificaciones");
            model.Estados = new SelectList(estados);
            //var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
            //model.Cantones = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
            //model.Distritos = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
            //model.Provincias = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));

            return View(model);
        }

        //-------------------------------------POST: Search VC----------------------------------//
        /// <summary>
        /// Busca un viatico corrido para el funcionario indicado o muestra los errores cometidos
        /// </summary>
        /// <example>POST: /ViaticoCorrido/SearchViaticoCorrido</example>
        /// <param name="model">View Model de la búsqueda del viatico corrido</param>
        /// <returns>Los datos obtenidos o los errores obtenidos</returns>
        [HttpPost]
        public ActionResult SearchViaticoCorrido(BusquedaViaticoCorridoVM model, string Fformulario)
        {
            try
            {
                if (model.Funcionario.Cedula != null && Fformulario == "1")
                {
                    var filtro = new List<string>();
                    List<DateTime> fechasEmision = new List<DateTime>();
                    List<DateTime> fechasVencimiento = new List<DateTime>();

                    model.Funcionario.Sexo = GeneroEnum.Indefinido;

                    if (model.FechaInicioViaticoCorridoI.Year > 1 && model.FechaInicioViaticoCorridoF.Year > 1)
                    {
                        fechasEmision.Add(model.FechaInicioViaticoCorridoI);
                        fechasEmision.Add(model.FechaInicioViaticoCorridoF);
                        filtro.Add(" Fecha de Inicio");
                    }

                    if (model.FechaInicioViaticoCorridoI.Year > 1 && model.FechaInicioViaticoCorridoF.Year > 1)
                    {
                        fechasVencimiento.Add(model.FechaInicioViaticoCorridoI);
                        fechasVencimiento.Add(model.FechaInicioViaticoCorridoF);
                        filtro.Add(" Fecha de Vencimiento");
                    }

                    if (model.EstadoSeleccion != null)
                    {
                        model.ViaticoCorrido.EstadoViaticoCorridoDTO = new CEstadoViaticoCorridoDTO { NomEstadoDTO = model.EstadoSeleccion };
                        filtro.Add(" Estado ViaticoCorrido");
                    }

                    List<string> lugarContrato = new List<string>();
                    //if (model.DistritoSeleccion != null)
                    //{
                    //    lugarContrato.Add(model.DistritoSeleccion);
                    //    filtro.Add(" Distrito");
                    //}
                    //else lugarContrato.Add("null");
                    //if (model.CantonSeleccion != null)
                    //{
                    //    lugarContrato.Add(model.CantonSeleccion);
                    //    filtro.Add(" Cantón");
                    //}
                    //else lugarContrato.Add("null");
                    //if (model.ProvinciaSeleccion != null)
                    //{
                    //    lugarContrato.Add(model.ProvinciaSeleccion);
                    //    filtro.Add(" Provincia");
                    //}
                    //else lugarContrato.Add("null");

                    var datos = ServicioViaticoCorridoGastoTransporte.BuscarViaticoCorrido(model.Funcionario, model.ViaticoCorrido,
                                                                    fechasEmision.ToArray(), fechasVencimiento.ToArray(),
                                                                    lugarContrato.ToArray());

                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.ViaticoCorridoRes = new List<FormularioViaticoCorridoVM>();
                        foreach (var item in datos)
                        {
                            FormularioViaticoCorridoVM temp = new FormularioViaticoCorridoVM();
                            temp.ViaticoCorrido = (CViaticoCorridoDTO)item.ElementAt(0);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(1);
                            temp.Puesto = (CPuestoDTO)item.ElementAt(2);
                            temp.DetallePuesto = ((CDetallePuestoDTO)item.ElementAt(3));
                            var ubicacion = (CUbicacionPuestoDTO)item.ElementAt(4);
                            if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                            {
                                temp.UbicacionContrato = ubicacion;
                                temp.UbicacionTrabajo = (CUbicacionPuestoDTO)item.ElementAt(5);
                            }
                            else
                            {
                                temp.UbicacionContrato = (CUbicacionPuestoDTO)item.ElementAt(5);
                                temp.UbicacionTrabajo = ubicacion;
                            }
                            model.ViaticoCorridoRes.Add(temp);
                        }

                        ViewData["filtro"] = string.Join(",", filtro.ToArray()) + ".";
                        return PartialView("_SearchResultsViaticoCorrido", model.ViaticoCorridoRes);
                    }

                }
                else if (model.Funcionario.Cedula != null && Fformulario == "2")
                {
                    model.Funcionario.Sexo = GeneroEnum.Indefinido;

                    var datos = ServicioViaticoCorridoGastoTransporte.BuscarMovimientoViaticoCorrido(model.Funcionario, Fformulario);

                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        string cod = Convert.ToString(((CMovimientoViaticoCorridoDTO)datos[1][0]).ViaticoCorridoDTO.IdEntidad);
                        model.ViaticoCorridoRes = new List<FormularioViaticoCorridoVM>();
                        var datosvc = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(cod);
                        foreach (var item in datos[0])
                        {
                            FormularioViaticoCorridoVM temp = new FormularioViaticoCorridoVM();
                            temp.ViaticoCorrido = (CViaticoCorridoDTO)datosvc[0][0];
                            temp.Funcionario = (CFuncionarioDTO)item;
                            temp.Puesto = (CPuestoDTO)datosvc[0][3];
                            temp.DetallePuesto = ((CDetallePuestoDTO)datosvc[1][1]);
                            //temp.UbicacionContrato = ((CUbicacionPuestoDTO)item.ElementAt(4));
                            //temp.UbicacionTrabajo = ((CUbicacionPuestoDTO)item.ElementAt(5));
                            model.ViaticoCorridoRes.Add(temp);
                        }
                        FormularioViaticoCorridoVM temp2 = new FormularioViaticoCorridoVM();
                        temp2.MovimientoList = new List<CMovimientoViaticoCorridoDTO>();
                        foreach (var item in datos[1])
                        {
                            temp2.MovimientoList.Add((CMovimientoViaticoCorridoDTO)item);
                            model.ViaticoCorridoRes.Add(temp2);
                        }

                        return PartialView("_SearchResultsMovimientoViaticoCorrido", model.ViaticoCorridoRes);
                    }

                }
                else if (model.Funcionario.Cedula != null && Fformulario == "3")
                {
                    model.Funcionario.Sexo = GeneroEnum.Indefinido;

                    var datos = ServicioViaticoCorridoGastoTransporte.BuscarMovimientoViaticoCorrido(model.Funcionario, Fformulario);


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        string cod = Convert.ToString(((CMovimientoViaticoCorridoDTO)datos[1][1]).ViaticoCorridoDTO.IdEntidad);
                        model.ViaticoCorridoRes = new List<FormularioViaticoCorridoVM>();
                        var datosvc = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(cod);
                        foreach (var item in datos[0])
                        {
                            FormularioViaticoCorridoVM temp = new FormularioViaticoCorridoVM();
                            temp.ViaticoCorrido = (CViaticoCorridoDTO)datosvc[0][0];
                            temp.Funcionario = (CFuncionarioDTO)item;
                            temp.Puesto = (CPuestoDTO)datosvc[0][3];
                            temp.DetallePuesto = ((CDetallePuestoDTO)datosvc[1][1]);
                            //temp.UbicacionContrato = ((CUbicacionPuestoDTO)item.ElementAt(4));
                            //temp.UbicacionTrabajo = ((CUbicacionPuestoDTO)item.ElementAt(5));
                            model.ViaticoCorridoRes.Add(temp);
                        }
                        FormularioViaticoCorridoVM temp2 = new FormularioViaticoCorridoVM();
                        temp2.MovimientoList = new List<CMovimientoViaticoCorridoDTO>();
                        foreach (var item in datos[1])
                        {
                            temp2.MovimientoList.Add((CMovimientoViaticoCorridoDTO)item);
                            model.ViaticoCorridoRes.Add(temp2);
                        }

                        return PartialView("_SearchResultsMovimientoViaticoCorrido", model.ViaticoCorridoRes);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaInicioViaticoCorridoI < model.FechaInicioViaticoCorridoF && model.FechaInicioViaticoCorridoI.Year > 1)
                    {
                        ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
                    }

                    if (model.FechaFinalViaticoCorridoI < model.FechaFinalViaticoCorridoF && model.FechaFinalViaticoCorridoI.Year > 1)
                    {
                        ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                    }

                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            { //El error se matiene porque no se para que lo usan
                //Response.Write(error);
                return PartialView("_ErrorViaticoCorrido");
                //if (error.Message == "Busqueda")
                //{
                //    return PartialView("_ErrorViaticoCorrido");
                //}
                //else
                //{
                //    return PartialView("_SearchResultsViaticoCorrido", model.ViaticoCorridoRes);
                //}
            }

        }

        public ActionResult ListarViaticoCorridoPagos()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.ViaticoCorrido), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.ViaticoCorrido)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.ViaticoCorrido) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.ViaticoCorrido)]))
                {
                    ListaViaticoCorridoVM model = new ListaViaticoCorridoVM();
                    List<SelectListItem> listaMeses = new List<SelectListItem>();
                    var mes = "";

                    var fecha = new DateTime(DateTime.Today.Year, 1, 1);

                    for (int i = 0; i <= 11; i++)
                    {
                        mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i].ToString()) + " del " + DateTime.Today.Year.ToString();
                        listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                        fecha = fecha.AddMonths(1);
                    }
                    model.MesesViatico = listaMeses;
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
                }
            }
        }

        [HttpPost]
        public ActionResult ListarViaticoCorridoPagos(ListaViaticoCorridoVM model, string SubmitButton)
        {
            try
            {
                DateTime fecha;
                CBaseDTO[][] datos;

                switch (SubmitButton)
                {
                    case "Buscar":
                    case "Inicio":
                        if (model.MesSeleccion == null)
                        {
                            ModelState.Clear();
                            throw new Exception("Debe escoger el Mes");
                        }
                        break;

                    case "Asignar Reserva":
                        if (model.PresupuestoSelected != null)
                        {
                            if (model.ReservaRecurso != null && model.ReservaRecurso != "")
                            {
                                var listado = model.Viaticos.Where(Q => Q.PresupuestoDTO.CodigoPresupuesto.Contains(model.PresupuestoSelected) && Q.Pagos[0].IndSeleccionar == true).ToList();
                                foreach (var item in listado)
                                {
                                    item.Pagos[0].ReservaRecurso = model.ReservaRecurso;
                                    var datosAsignar = ServicioViaticoCorridoGastoTransporte.AsignarReservaRecurso(item.Pagos[0]);

                                    if (datosAsignar.GetType() == typeof(CErrorDTO))
                                    {
                                        ModelState.Clear();
                                        throw new Exception(((CErrorDTO)datosAsignar).MensajeError);
                                    }
                                }
                            }
                            else
                            {
                                ModelState.Clear();
                                throw new Exception("Debe digitar la Reserva Recurso");
                            }
                            break;
                        }
                        else
                        {
                            ModelState.Clear();
                            throw new Exception("Debe escoger Código Presupuestario");
                        }

                    case "Generar Pago":
                        decimal monPago = 0;
                        decimal monDia = 0;

                        // Generar los Contratos que tienen Monto Pago en cero
                        foreach (var itemViatico in model.Viaticos.Where(Q => Q.Pagos[0].MonPago == 0 && Q.Pagos[0].IndSeleccionar == true).ToList())//.Where(Q => Q.IndReintegroDTO)
                        {
                            monPago = Convert.ToDecimal(itemViatico.MontViaticoCorridoDTO);
                            monDia = Decimal.Round((monPago / 26), 2);
                            itemViatico.FecPagoDTO = Convert.ToDateTime(model.MesSeleccion);

                            var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagar(itemViatico, itemViatico.FecPagoDTO.Month, itemViatico.FecPagoDTO.Year);
                            if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                var maxDias = ((CDetallePagoViaticoCorridoDTO)diasPagar.FirstOrDefault()).IdEntidad;
                                if (maxDias < 26)
                                    monPago = Decimal.Round((monDia * maxDias), 2);
                            }

                            CPagoViaticoCorridoDTO pago = new CPagoViaticoCorridoDTO
                            {
                                FecPago = itemViatico.FecPagoDTO,
                                HojaIndividualizada = "",  // model.ViaticoCorrido.Pagos[0].HojaIndividualizada,
                                NumBoleta = "",
                                ReservaRecurso = "S/R",
                                ViaticoCorridoDTO = new CViaticoCorridoDTO { IdEntidad = itemViatico.IdEntidad },
                                IndEstado = 1
                            };

                            List<CDetallePagoViaticoCorridoDTO> detalles = new List<CDetallePagoViaticoCorridoDTO>();
                            var datosRebajar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajar(itemViatico.NombramientoDTO.Funcionario, itemViatico, itemViatico.FecPagoDTO.Month, itemViatico.FecPagoDTO.Year, monDia);

                            foreach (CDetallePagoViaticoCorridoDTO item in datosRebajar)
                            {
                                detalles.Add(new CDetallePagoViaticoCorridoDTO
                                {
                                    FecDiaPago = item.FecDiaPago,
                                    MonPago = item.MonPago,
                                    CodEntidad = item.CodEntidad,
                                    TipoDetalleDTO = item.TipoDetalleDTO
                                });

                                if (item.TipoDetalleDTO.IdEntidad == 5)
                                    monPago += item.MonPago;
                                else
                                    monPago -= item.MonPago;
                            }

                            pago.MonPago = monPago;
                            var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarPagoViaticoCorrido(pago, detalles.ToArray(), itemViatico.NombramientoDTO.Funcionario);
                        }
                        break;                       
                    
                    default:
                        break;
                }

                fecha = Convert.ToDateTime(model.MesSeleccion);
                datos = ServicioViaticoCorridoGastoTransporte.ListarViaticoCorridoPago(fecha.Month, fecha.Year);
                if (datos.FirstOrDefault().FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    LlenarModeloListadoPagosVC(model, datos);
                    return View("_ListarViaticoCorridoPagos", model);
                }
                else
                {
                    //ModelState.AddModelError("Busqueda", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
                    throw new Exception(((CErrorDTO)datos.FirstOrDefault().FirstOrDefault()).MensajeError);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        //-------------------------------------GET: Search GT----------------------------------//
        /// <summary>
        /// Llena variablas y muestra la vista de buscar gasto de transporte (formulario de búsqueda)
        /// </summary>
        /// <example>GET: /ViaticoCorrido/SearchGastoTransporte/</example>
        /// <returns>Retorna la vista de buscar gasto de transporte o la vista parcial de error</returns>
        public ActionResult SearchGastoTransporte()
        {
            BusquedaGastoTransporteVM model = new BusquedaGastoTransporteVM();

            List<string> estados = new List<string>();
            estados.Add("Valido");
            estados.Add("Espera");
            estados.Add("Anulado");
            estados.Add("Vencido");
            estados.Add("Vencido_PSS");
            estados.Add("Vencido_Vac");
            estados.Add("Vencido_Incap");
            estados.Add("Todas las Notificaciones");
            model.Estados = new SelectList(estados);            
            //var localizacion = ServicioDesarraigo.GetLocalizacion(true, true, true);
            //model.Cantones = new SelectList(localizacion[0].Select(Q => ((CCantonDTO)Q).NomCanton));
            //model.Distritos = new SelectList(localizacion[1].Select(Q => ((CDistritoDTO)Q).NomDistrito));
            //model.Provincias = new SelectList(localizacion[2].Select(Q => ((CProvinciaDTO)Q).NomProvincia));

            return View(model);
        }

        //-------------------------------------POST: Search GT----------------------------------//
        /// <summary>
        /// Busca un gasto de transporte para el funcionario indicado o muestra los errores cometidos
        /// </summary>
        /// <example>POST: /ViaticoCorrido/SearchGastoTransporte</example>
        /// <param name="model">View Model de la búsqueda del gasto transporte</param>
        /// <returns>Los datos obtenidos o los errores obtenidos</returns>
        [HttpPost]
        public ActionResult SearchGastoTransporte(BusquedaGastoTransporteVM model, string Fformulario)
        {
            try
            {
                if (model.Funcionario.Cedula != null && Fformulario == "1") //ASIGNACIÓN
                {
                    var filtro = new List<string>();
                    List<DateTime> fechasEmision = new List<DateTime>();
                    List<DateTime> fechasVencimiento = new List<DateTime>();

                    model.Funcionario.Sexo = GeneroEnum.Indefinido;

                    if (model.FechaInicioGastoTransporteI.Year > 1 && model.FechaInicioGastoTransporteF.Year > 1)
                    {
                        fechasEmision.Add(model.FechaInicioGastoTransporteI);
                        fechasEmision.Add(model.FechaInicioGastoTransporteF);
                        filtro.Add(" Fecha de Inicio");
                    }

                    if (model.FechaInicioGastoTransporteI.Year > 1 && model.FechaInicioGastoTransporteF.Year > 1)
                    {
                        fechasVencimiento.Add(model.FechaInicioGastoTransporteI);
                        fechasVencimiento.Add(model.FechaInicioGastoTransporteF);
                        filtro.Add(" Fecha de Vencimiento");
                    }

                    if (model.EstadoSeleccion != null)
                    {
                        model.GastoTransporte.EstadoGastoTransporteDTO = new CEstadoGastoTransporteDTO { NomEstadoDTO = model.EstadoSeleccion };
                        filtro.Add(" Estado GastoTransporte");
                    }

                    List<string> lugarContrato = new List<string>();
                    //if (model.DistritoSeleccion != null)
                    //{
                    //    lugarContrato.Add(model.DistritoSeleccion);
                    //    filtro.Add(" Distrito");
                    //}
                    //else lugarContrato.Add("null");
                    //if (model.CantonSeleccion != null)
                    //{
                    //    lugarContrato.Add(model.CantonSeleccion);
                    //    filtro.Add(" Cantón");
                    //}
                    //else lugarContrato.Add("null");
                    //if (model.ProvinciaSeleccion != null)
                    //{
                    //    lugarContrato.Add(model.ProvinciaSeleccion);
                    //    filtro.Add(" Provincia");
                    //}
                    //else lugarContrato.Add("null");

                    var datos = ServicioViaticoCorridoGastoTransporte.BuscarGastoTransporte(model.Funcionario, model.GastoTransporte,
                                                                    fechasEmision.ToArray(), fechasVencimiento.ToArray(),
                                                                    lugarContrato.ToArray());


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.GastoTransporteRes = new List<FormularioGastoTransporteVM>();

                        foreach (var item in datos)
                        {
                            FormularioGastoTransporteVM temp = new FormularioGastoTransporteVM();
                            temp.GastoTransporte = (CGastoTransporteDTO)item.ElementAt(0);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(1);
                            temp.Puesto = (CPuestoDTO)item.ElementAt(2);
                            temp.DetallePuesto = ((CDetallePuestoDTO)item.ElementAt(3));
                            //temp.UbicacionContrato = ((CUbicacionPuestoDTO)item.ElementAt(4));
                            //temp.UbicacionTrabajo = ((CUbicacionPuestoDTO)item.ElementAt(5));
                            model.GastoTransporteRes.Add(temp);
                        }

                        ViewData["filtro"] = string.Join(",", filtro.ToArray()) + ".";
                        return PartialView("_SearchResultsGastoTransporte", model.GastoTransporteRes);
                    }
                }
                else if (model.Funcionario.Cedula != null && Fformulario == "2") //DEDUCCIÓN
                {
                    model.Funcionario.Sexo = GeneroEnum.Indefinido;

                    var datos = ServicioViaticoCorridoGastoTransporte.BuscarMovimientoGastoTransporte(model.Funcionario, Fformulario);


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        if(datos[1].Length > 0)
                        {
                            string cod = Convert.ToString(((CMovimientoGastoTransporteDTO)datos[1][0]).GastoTransporteDTO.IdEntidad);
                            model.GastoTransporteRes = new List<FormularioGastoTransporteVM>();

                            var datosgt = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(cod);
                            foreach (var item in datos[0])
                            {
                                FormularioGastoTransporteVM temp = new FormularioGastoTransporteVM();
                                temp.GastoTransporte = (CGastoTransporteDTO)datosgt[0][0];
                                temp.Funcionario = (CFuncionarioDTO)item;
                                temp.Carta = (CCartaPresentacionDTO)datosgt[0][3];
                                temp.Puesto = (CPuestoDTO)datosgt[1][0];
                                temp.DetallePuesto = (CDetallePuestoDTO)datosgt[1][1];
                                var ubicacion = (CUbicacionPuestoDTO)datosgt[1][2];
                                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                                {
                                    temp.UbicacionContrato = ubicacion;
                                    temp.UbicacionTrabajo = (CUbicacionPuestoDTO)datosgt[1][3];
                                }
                                else
                                {
                                    temp.UbicacionContrato = (CUbicacionPuestoDTO)datosgt[1][3];
                                    temp.UbicacionTrabajo = ubicacion;
                                }
                                temp.MovimeintoList = new List<CMovimientoGastoTransporteDTO>();
                                foreach (var i in datos[1])
                                {
                                    temp.MovimeintoList.Add((CMovimientoGastoTransporteDTO)i);
                                }
                                model.GastoTransporteRes.Add(temp);
                            }
                        } 
                        else
                        {
                            ModelState.AddModelError("Busqueda", "El funcionario indicado no tiene movimientos de deducción");
                            throw new Exception("Busqueda");
                        }                        

                        return PartialView("_SearchResultsMovimientoGastoTransporte", model.GastoTransporteRes);
                    }
                }
                else if (model.Funcionario.Cedula != null && Fformulario == "3") //ELIMINACIÓN
                {
                    model.Funcionario.Sexo = GeneroEnum.Indefinido;

                    var datos = ServicioViaticoCorridoGastoTransporte.BuscarMovimientoGastoTransporte(model.Funcionario, Fformulario);


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        if(datos[1].Length > 0) // Si sí tienen movimientos
                        {
                            string cod = Convert.ToString(((CMovimientoGastoTransporteDTO)datos[1][0]).GastoTransporteDTO.IdEntidad);
                            model.GastoTransporteRes = new List<FormularioGastoTransporteVM>();
                            
                            var datosgt = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(cod);
                            foreach (var item in datos[0])
                            {
                                FormularioGastoTransporteVM temp = new FormularioGastoTransporteVM();
                                temp.GastoTransporte = (CGastoTransporteDTO)datosgt[0][0];
                                temp.Funcionario = (CFuncionarioDTO)item;
                                temp.Carta = (CCartaPresentacionDTO)datosgt[0][3];
                                temp.Puesto = (CPuestoDTO)datosgt[1][0];
                                temp.DetallePuesto = (CDetallePuestoDTO)datosgt[1][1];
                                var ubicacion = (CUbicacionPuestoDTO)datosgt[1][2];
                                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                                {
                                    temp.UbicacionContrato = ubicacion;
                                    temp.UbicacionTrabajo = (CUbicacionPuestoDTO)datosgt[1][3];
                                }
                                else
                                {
                                    temp.UbicacionContrato = (CUbicacionPuestoDTO)datosgt[1][3];
                                    temp.UbicacionTrabajo = ubicacion;
                                }
                                temp.MovimeintoList = new List<CMovimientoGastoTransporteDTO>();
                                foreach (var i in datos[1])
                                {
                                    temp.MovimeintoList.Add((CMovimientoGastoTransporteDTO)i);
                                }
                                model.GastoTransporteRes.Add(temp);
                            }
                            //FormularioGastoTransporteVM temp2 = new FormularioGastoTransporteVM();
                            //temp2.MovimeintoList = new List<CMovimientoGastoTransporteDTO>();
                            //foreach (var item in datos[1])
                            //{
                            //    temp2.MovimeintoList.Add((CMovimientoGastoTransporteDTO)item);
                            //    model.GastoTransporteRes.Add(temp2);
                            //}
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", "El funcionario indicado no tiene movimientos de eliminación");
                            throw new Exception("Busqueda");
                        }

                        return PartialView("_SearchResultsMovimientoGastoTransporte", model.GastoTransporteRes);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaInicioGastoTransporteI < model.FechaInicioGastoTransporteF && model.FechaInicioGastoTransporteI.Year > 1)
                    {
                        ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
                    }

                    if (model.FechaFinalGastoTransporteI < model.FechaFinalGastoTransporteF && model.FechaFinalGastoTransporteI.Year > 1)
                    {
                        ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                    }

                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch(Exception error)
            {
                //Response.Write(error);
                //El error se matiene porque no se para que lo usan
                return PartialView("_ErrorGastoTransporte");
            }
        }
        
        //------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------Details-------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Detalle de un viatico corrido
        /// </summary>
        /// <example>GET: /ViaticoCorrido/Details/5</example>
        /// <param name="codigo">Codigo del desarraigo</param>
        /// <param name="accion">La pantalla de la cual de se llama el detalle</param>
        /// <returns>Retorna un error o la vista del detalle</returns>
        public ActionResult DetailsViaticoCorrido(string codigo, string accion)
        {
            //var acceso = AccesoEsPermitido(15, 5); // permiso: Consulta, perfil: desarraigo 
            //var acceso = (Object)true;
            //Session["Permiso_Desarraigo_Consulta"] = true;

            //   if (acceso.GetType() != typeof(bool))
            //     return (RedirectToRouteResult)acceso;

            // if (Convert.ToBoolean(Session["Administrador_Global"]) ||
            //      Convert.ToBoolean(Session["Administrador_Desarraigo"]) ||
            //     Session["Permiso_Desarraigo_Consulta"] != null ||
            //       Session["Permiso_Desarraigo_Operativo"] != null)
            //  {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();

            ViewData["viewMode"] = accion;
            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(codigo);

            if (datos.Count() > 1)
            {
                model.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][0];
                model.Funcionario = (CFuncionarioDTO)datos[0][1];
                model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][4]).NumeroCarta;
                model.Puesto = ((CPuestoDTO)datos[1][0]);
                model.DetallePuesto = ((CDetallePuestoDTO)datos[1][1]);
                var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                {
                    model.UbicacionContrato = ubicacion;
                    model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                }
                else
                {
                    model.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                    model.UbicacionTrabajo = ubicacion;
                }
                model.Facturas = datos.ElementAt(2).Select(F => (CFacturaDesarraigoDTO)F).ToList();
                model.ContratosArrendamiento = datos.ElementAt(3).Select(C => (CContratoArrendamientoDTO)C).ToList();
                model.EstadoSeleccion = model.ViaticoCorrido.EstadoViaticoCorridoDTO.NomEstadoDTO;
                InsertarEstado(model, true);
            }
            else
            {
                return RedirectToAction("ViaticoCorrido", "Error", new { errorType = "404", modulo = "ViaticoCorrido" });
            }
            return View(model);
        }

        /// <summary>
        /// Detalle de un movimiento de viatico corrido
        /// </summary>
        /// <example>GET: /ViaticoCorrido/Details/5</example>
        /// <param name="codigo">Codigo del desarraigo</param>
        /// <param name="accion">La pantalla de la cual de se llama el detalle</param>
        /// <returns>Retorna un error o la vista del detalle</returns>
        public ActionResult DetailsMovimientosViaticoCorrido(string codigo, int accion)
        {
            //var acceso = AccesoEsPermitido(15, 5); // permiso: Consulta, perfil: desarraigo 
            //var acceso = (Object)true;
            //Session["Permiso_Desarraigo_Consulta"] = true;

            //   if (acceso.GetType() != typeof(bool))
            //     return (RedirectToRouteResult)acceso;

            // if (Convert.ToBoolean(Session["Administrador_Global"]) ||
            //      Convert.ToBoolean(Session["Administrador_Desarraigo"]) ||
            //     Session["Permiso_Desarraigo_Consulta"] != null ||
            //       Session["Permiso_Desarraigo_Operativo"] != null)
            //  {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();

            ViewData["viewMode"] = accion;

            var datosMovimiento = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoViaticoCorrido(codigo);
            if (datosMovimiento != null)
            {
                string codigoVC = ((CMovimientoViaticoCorridoDTO)datosMovimiento).ViaticoCorridoDTO.IdEntidad.ToString();
                if (accion == 2)
                {
                    var datosDetalleMovimiento = ServicioViaticoCorridoGastoTransporte.ObtenerDeduccionViaticoCorrido(codigo);
                    model.DetalleD = new List<CDetalleDeduccionViaticoCorridoDTO>();
                    foreach (var item in datosDetalleMovimiento)
                    {
                        model.DetalleD.Add((CDetalleDeduccionViaticoCorridoDTO)item);
                    }
                }
                else if (accion == 3)
                {

                    var datosDetalleMovimiento = ServicioViaticoCorridoGastoTransporte.ObtenerEliminacion(codigo, "VC");
                    model.DetalleEliminacion = (CDetalleEliminacionViaticoCorridoGastoTransporteDTO)datosDetalleMovimiento;
                }


                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(codigoVC);
                if (datos.Count() > 1)
                {
                    model.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][0];
                    model.Funcionario = (CFuncionarioDTO)datos[0][1];
                    model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][4]).NumeroCarta;
                    model.Puesto = ((CPuestoDTO)datos[1][0]);
                    model.DetallePuesto = ((CDetallePuestoDTO)datos[1][1]);
                    var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                    if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                    {
                        model.UbicacionContrato = ubicacion;
                        model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                    }
                    else
                    {
                        model.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                        model.UbicacionTrabajo = ubicacion;
                    }
                    model.Facturas = datos.ElementAt(2).Select(F => (CFacturaDesarraigoDTO)F).ToList();
                    model.ContratosArrendamiento = datos.ElementAt(3).Select(C => (CContratoArrendamientoDTO)C).ToList();
                    model.EstadoSeleccion = model.ViaticoCorrido.EstadoViaticoCorridoDTO.NomEstadoDTO;
                    InsertarEstado(model, true);
                }
                else
                {
                    return RedirectToAction("ViaticoCorrido", "Error", new { errorType = "404", modulo = "ViaticoCorrido" });
                }
            }
            return View(model);
        }

        public ActionResult DetailsPagoViaticoCorrido(int codigo, string accion)
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();

            ViewData["viewMode"] = accion;

            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerPagoViaticoCorrido(codigo);

            if (datos.Count() > 1)
            {
                model.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][1];
                model.ViaticoCorrido.Pagos[0] = (CPagoViaticoCorridoDTO)datos[0][0];
                model.Funcionario = (CFuncionarioDTO)datos[0][2];
                model.Puesto = ((CPuestoDTO)datos[0][4]);
                model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][5]).NumeroCarta;
                model.DetallePuesto = ((CDetallePuestoDTO)datos[0][6]);
                var ubicacion = ((CUbicacionPuestoDTO)datos[0][7]);
                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                {
                    model.UbicacionContrato = ubicacion;
                    model.UbicacionTrabajo = ((CUbicacionPuestoDTO)datos[0][8]);
                }
                else
                {
                    model.UbicacionContrato = ((CUbicacionPuestoDTO)datos[0][8]);
                    model.UbicacionTrabajo = ubicacion;
                }
                model.Facturas = datos.ElementAt(1).Select(F => (CFacturaDesarraigoDTO)F).ToList();
                model.ContratosArrendamiento = datos.ElementAt(2).Select(C => (CContratoArrendamientoDTO)C).ToList();
                model.EstadoSeleccion = model.ViaticoCorrido.EstadoViaticoCorridoDTO.NomEstadoDTO;
                InsertarEstado(model, true);
            }
            else
            {
                return RedirectToAction("ViaticoCorrido", "Error", new { errorType = "404", modulo = "ViaticoCorrido" });
            }

            return View(model);
        }

        public ActionResult DetailsDeleteViaticoCorrido(string codigo, string codigoVC)
        {
            FormularioViaticoCorridoVM modelo = new FormularioViaticoCorridoVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(codigoVC);
                if (datos != null)
                {
                    modelo.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][0];
                    modelo.Funcionario = (CFuncionarioDTO)datos[0][1];

                    var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                    if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                    {
                        modelo.UbicacionContrato = ubicacion;
                        modelo.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                    }
                    else
                    {
                        modelo.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                        modelo.UbicacionTrabajo = ubicacion;
                    }

                    modelo.DetallePuesto = (CDetallePuestoDTO)datos[1][1];
                    modelo.Puesto = (CPuestoDTO)datos[0][3];
                    modelo.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][4]).NumeroCarta;
                    var datosM = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoViaticoCorrido(codigo);
                    if (datosM != null)
                    {
                        modelo.MovimientoViaticoCorrido = (CMovimientoViaticoCorridoDTO)datosM;
                        var datosME = ServicioViaticoCorridoGastoTransporte.ObtenerEliminacion(codigo, "VC");
                        if (datosME.GetType() != typeof(CErrorDTO))
                        {
                            modelo.Eliminacion = (CDetalleEliminacionViaticoCorridoGastoTransporteDTO)datosME;
                            modelo.Mensaje = "DetalleE";
                            return PartialView("_DetailsDeleteViaticoCorrido", modelo);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }


            }
            else
            {
                return PartialView("_ErrorViaticoCorrido");
            }

            return PartialView("_ErrorViaticoCorrido");
        }

        /// <summary>
        /// Mostrar la vista de detalles tras elminar un ViaticoCorrido 
        /// </summary>
        /// <param name="codigo">PrimaryKey del Movimiento del VC</param>
        /// <param name="codigoGT">PrimaryKey del ViaticoCorrido en sí</param>
        /// <returns>Vista de detalles tras la deducción del VC</returns>
        public ActionResult DetailsDeduccionViaticoCorrido(string codigo, string codigoCV)
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(codigoCV);
                if (datos != null)
                {
                    model.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][0];
                    model.Funcionario = (CFuncionarioDTO)datos[0][1];
                    var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                    if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                    {
                        model.UbicacionContrato = ubicacion;
                        model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                    }
                    else
                    {
                        model.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                        model.UbicacionTrabajo = ubicacion;
                    }
                    model.DetallePuesto = (CDetallePuestoDTO)datos[1][1];
                    model.Puesto = (CPuestoDTO)datos[0][3];
                    model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][4]).NumeroCarta;
                    var datosM = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoViaticoCorrido(codigo);
                    if (datosM != null)
                    {
                        model.MovimientoViaticoCorrido = (CMovimientoViaticoCorridoDTO)datosM;
                        var respuesta = ServicioViaticoCorridoGastoTransporte.ObtenerDeduccionViaticoCorrido(codigo);
                        model.Deduccion = new List<CDetalleDeduccionViaticoCorridoDTO>();
                        if (respuesta != null)
                        {
                            foreach (var item in respuesta)
                            {
                                model.Deduccion.Add((CDetalleDeduccionViaticoCorridoDTO)item);
                            }
                            model.Mensaje = "DetalleD";
                            return PartialView("_DetailsDeduccionViaticoCorrido", model);
                        }
                    }
                    return PartialView("_DetailsDeduccionViaticoCorrido", model);
                }
                else
                {
                    return PartialView("_ErrorViaticoCorrido");
                }
            }
            else
            {
                return PartialView("_ErrorViaticoCorrido");
            }

        }




        /// <summary>
        /// Encargado de llenar los datos del ViewModel (del GT) para mostrar la vista 
        /// con los detalles del mismo usuario de pendiendo de la acción que se haya 
        /// realizado
        /// </summary>
        /// <param name="codigo">Codigo del GT recién creado en la BD (la PK)</param>
        /// <param name="accion">Acción realizada (Guardar, Editar, Detalle)</param>
        /// <returns>Mostrar la vista de detalle</returns>
        public ActionResult DetailsGastoTransporte(string codigo, string accion)
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();  

            ViewData["viewMode"] = accion;            
            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(codigo);
            int cod = Convert.ToInt32(codigo);

            if (datos.Count() > 1)
            {
                var datosDA = ServicioViaticoCorridoGastoTransporte.ListarAsignacion(cod);
                var datosModif = ServicioViaticoCorridoGastoTransporte.ListarAsignacionModificada(cod);
                model.GastoTransporte = (CGastoTransporteDTO)datos[0][0];
                model.Funcionario = (CFuncionarioDTO)datos[0][1];
                model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][3]).NumeroCarta;
                model.Puesto = (CPuestoDTO)datos[1][0];
                model.DetallePuesto = (CDetallePuestoDTO)datos[1][1];
                var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                {
                    model.UbicacionContrato = ubicacion;
                    model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                }
                else
                {
                    model.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                    model.UbicacionTrabajo = ubicacion;
                }
               
                model.EstadoSeleccion = model.GastoTransporte.EstadoGastoTransporteDTO.NomEstadoDTO;

                //Mostrar detalles de asignación(rutas) verificando en la otra tabla, si alguna se ha modificado para este gasto.
                model.detalleAGT = new List<CDetalleAsignacionGastoTransporteModificadaDTO>();
                if (datosDA != null)
                {
                    double totalMA = 0.0;
                    foreach (var item in datosDA)
                    {
                        CDetalleAsignacionGastoTransporteDTO rutaOriginal = (CDetalleAsignacionGastoTransporteDTO)item;
                        bool Modified = false;
                        for (int k = 0; k < datosModif.Count(); k++)
                        {
                            CDetalleAsignacionGastoTransporteModificadaDTO rutaNueva = (CDetalleAsignacionGastoTransporteModificadaDTO)datosModif[k];

                            //Verificar si la ruta original coincide con alguna en la lista de rutas nuevas, se ha modificado y debe mostrarse
                            if (rutaNueva.NomRutaDTO.Equals(rutaOriginal.NomRutaDTO) &&
                                rutaNueva.NomFraccionamientoDTO.Equals(rutaOriginal.NomFraccionamientoDTO))
                            {

                                Modified = true;
                                model.detalleAGT.Add(rutaNueva);
                                totalMA = Convert.ToDouble(rutaNueva.MontTarifa) + totalMA;
                            }
                        }
                        //Si no ha sido modificada, se agrega para mostrarse
                        if (!Modified)
                        {
                            //Se convierte el tipo de dato de la original a las modificadas para mostrarla sin numero de gaceta
                            var original = new CDetalleAsignacionGastoTransporteModificadaDTO
                            {
                                NomRutaDTO = rutaOriginal.NomRutaDTO,
                                NomFraccionamientoDTO = rutaOriginal.NomFraccionamientoDTO,
                                //MontTarifa = rutaOriginal.MontTarifa,
                                NumGaceta = "--"
                            };
                            model.detalleAGT.Add(original);
                            totalMA = Convert.ToDouble(((CDetalleAsignacionGastoTransporteDTO)item).MontTarifa) + totalMA;
                        }
                    }
                    model.TotalMA = totalMA;
                }
                LlenarComboEstadoGT(model, true);
            }
            else
            {
                return RedirectToAction("ViaticoCorrido", "Error", new { errorType = "404", modulo = "ViaticoCorrido" });
            }
            return View(model);
        }

        //Details movimientos

        /// <summary>
        /// Carga el view model de gasto de transporte para mostrar la vista de DetailsPagoGT.
        /// (Llamado por el método CreatePagoGT)
        /// </summary>
        /// <param name="codigo">Código(PK) del pago del gasto de transporte</param>
        /// <param name="accion">Tipo de detalles a mostrar</param>
        /// <returns></returns>
        public ActionResult DetailsPagoGastoTransporte(int codigo, string accion)
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();

            ViewData["viewMode"] = accion;

            var datos = ServicioViaticoCorridoGastoTransporte.ObtenerPagoGastoTransporte(codigo);

            if (datos.Count() >= 1)
            {
                //DATOS: [0]pago. [1]GT. [2]Funcionario. [3]Nombramiento. [4]Carta.                
                model.GastoTransporte = (CGastoTransporteDTO)datos[0][1];
                model.GastoTransporte.Pagos[0] = (CPagoGastoTransporteDTO)datos[0][0];
                model.Funcionario = (CFuncionarioDTO)datos[0][2];
                model.Nombramiento = (CNombramientoDTO)datos[0][3];
                model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][4]).NumeroCarta;
                model.DetallePuesto = (CDetallePuestoDTO)datos[0][5];
                var ubicacion = (CUbicacionPuestoDTO)datos[0][6];
                if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                {
                    model.UbicacionContrato = ubicacion;
                    model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[0][7];
                }
                else
                {
                    model.UbicacionContrato = (CUbicacionPuestoDTO)datos[0][7];
                    model.UbicacionTrabajo = ubicacion;
                }
                model.EstadoSeleccion = model.GastoTransporte.EstadoGastoTransporteDTO.NomEstadoDTO;
                LlenarComboEstadoGT(model, true);
            }
            else
            {
                return RedirectToAction("ViaticoCorrido", "Error", new { errorType = "404", modulo = "ViaticoCorrido" });
            }

            return View(model);
        }

        /// <summary>
        /// Mostrar la vista de detalles tras elminar un GastoTransporte (_DetailsDeleteGastoTransporte)
        /// </summary>
        /// <param name="codigo">PrimaryKey del Movimiento del GT</param>
        /// <param name="codigoGT">PrimaryKey del GastoTransporte en sí</param>
        /// <returns>Vista de detalles tras eliminar el GT</returns>
        public ActionResult DetailsDeleteGastoTransporte(string codigo, string codigoGT)
        {
            FormularioGastoTransporteVM modelo = new FormularioGastoTransporteVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(codigoGT);
                if (datos != null)
                {
                    modelo.GastoTransporte = (CGastoTransporteDTO)datos[0][0];
                    modelo.Funcionario = (CFuncionarioDTO)datos[0][1];
       
                    var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                    if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                    {
                        modelo.UbicacionContrato = ubicacion;
                        modelo.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                    }
                    else
                    {
                        modelo.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                        modelo.UbicacionTrabajo = ubicacion;
                    }
                    modelo.DetallePuesto = (CDetallePuestoDTO)datos[1][1];
                    modelo.Puesto = (CPuestoDTO)datos[1][0];
                    modelo.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][3]).NumeroCarta; 
                    var datosM = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoGastoTransporte(codigo);
                    if (datosM != null)
                    {
                        modelo.MovimientoGastoTransporte = (CMovimientoGastoTransporteDTO)datosM;
                        var datosME = ServicioViaticoCorridoGastoTransporte.ObtenerEliminacion(codigo, "GT");
                        if (datosME.GetType() != typeof(CErrorDTO))
                        {
                            modelo.Eliminacion = (CDetalleEliminacionViaticoCorridoGastoTransporteDTO)datosME;
                            modelo.Mensaje = "DetalleE";
                            return PartialView("_DetailsDeleteGastoTransporte", modelo);
                        }
                        else
                        {
                            modelo.Mensaje = "Error, eliminación del GT está nula";
                        }
                    }
                    else
                    {
                        modelo.Mensaje = "Error, el movimiento de GT está nulo";
                    }
                }
                else
                {
                    modelo.Mensaje = "Error, el GastoTransporte está nulo";
                }
            }
            else
            {
                return PartialView("_ErrorViaticoCorrido");
            }

            return PartialView("_ErrorViaticoCorrido");
        }
        
        /// <summary>
        /// Mostrar la vista de detalles tras la deducción de un GastoTransporte
        /// </summary>
        /// <param name="codigo">PrimaryKey del Movimiento del GT</param>
        /// <param name="codigoGT">PrimaryKey del GastoTransporte en sí</param>
        /// <returns>Vista de detalles tras la deducción del GT</returns>
        public ActionResult DetailsDeduccionGastoTransporte(string codigo, string codigoGT)
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(codigoGT);
                if (datos != null)
                {
                    model.GastoTransporte = (CGastoTransporteDTO)datos[0][0];
                    model.Funcionario = (CFuncionarioDTO)datos[0][1];
                    var ubicacion = (CUbicacionPuestoDTO)datos[1][2];
                    if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                    {
                        model.UbicacionContrato = ubicacion;
                        model.UbicacionTrabajo = (CUbicacionPuestoDTO)datos[1][3];
                    }
                    else
                    {
                        model.UbicacionContrato = (CUbicacionPuestoDTO)datos[1][3];
                        model.UbicacionTrabajo = ubicacion;
                    }
                    model.DetallePuesto = (CDetallePuestoDTO)datos[1][1];
                    model.Puesto = (CPuestoDTO)datos[1][0];
                    //model.NumCartaPresentacion = ((CCartaPresentacionDTO)datos[0][3]).NumeroCarta; 
                    var datosM = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoGastoTransporte(codigo);
                    if (datosM != null)
                    {
                        model.MovimientoGastoTransporte = (CMovimientoGastoTransporteDTO)datosM;
                        var respuesta = ServicioViaticoCorridoGastoTransporte.ObtenerDeduccionGastoTransporte(codigo);
                        model.Deduccion = new List<CDetalleDeduccionGastoTransporteDTO>();
                        if (respuesta != null || respuesta.GetType() != typeof(CErrorDTO))
                        {
                            foreach (var item in respuesta)
                            {
                                if(item.GetType() != typeof(CErrorDTO))
                                {
                                    model.Deduccion.Add((CDetalleDeduccionGastoTransporteDTO)item);
                                }
                                else
                                {
                                    Response.Write(model.Mensaje);
                                    return PartialView("_ErrorViaticoCorrido");
                                }
                            }
                            model.Mensaje = "DetalleD";
                            return PartialView("_DetailsDeduccionGastoTransporte", model);
                        } 
                        else
                        {
                            return PartialView("_ErrorViaticoCorrido");
                        }
                    }
                    return PartialView("_DetailsDeduccionGastoTransporte", model);
                }
                else
                {
                    return PartialView("_ErrorViaticoCorrido");
                }
            }
            else
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }



        //-----------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------Anular-------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Obtiene un ViaticoCorrido específico de acuerdo a su código y lo anula
        /// </summary>
        /// <param name="codigo">PrimaryKey de la BD</param>
        /// <returns>Vista parcial de Detalles Anular VC</returns>
        public ActionResult AnularViaticoCorrido(string codigo)
        {
            FormularioViaticoCorridoVM modelo = new FormularioViaticoCorridoVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(codigo);
                modelo.ViaticoCorrido = (CViaticoCorridoDTO)datos[0][0];
                var datosAnulacion = ServicioViaticoCorridoGastoTransporte.AnularViaticoCorrido(modelo.ViaticoCorrido);
                if (datosAnulacion.GetType() != typeof(CErrorDTO))
                {
                    modelo.Mensaje = "Exito";
                }
                else
                {
                    modelo.Mensaje = "Error";
                }
            }
            return PartialView("_DetailsAnularViaticoCorrido", modelo);
        }

        /// <summary>
        /// Obtiene un movimiento de ViaticoCorrido específico de acuerdo a su código y lo anula
        /// </summary>
        /// <param name="codigo">PrimaryKey de la BD</param>
        /// <returns>Vista parcial de Detalles Anular VC</returns>
        public ActionResult AnularMovimientoViaticoCorrido(string codigo)
        {
            FormularioViaticoCorridoVM modelo = new FormularioViaticoCorridoVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoViaticoCorrido(codigo);
                modelo.MovimientoViaticoCorrido = (CMovimientoViaticoCorridoDTO)datos;
                var datosAnulacion = ServicioViaticoCorridoGastoTransporte.AnularMovimientoViaticoCorrido(modelo.MovimientoViaticoCorrido);
                if (datosAnulacion.GetType() != typeof(CErrorDTO))
                {
                    modelo.Mensaje = "Exito";
                }
                else
                {
                    modelo.Mensaje = "Error";
                }
            }
            return PartialView("_DetailsAnularViaticoCorrido", modelo);
        }
        
        /// <summary>
        /// Obtiene un GastoTransporte específico de acuerdo a su código y lo anula
        /// </summary>
        /// <param name="codigo">PrimaryKey de la BD</param>
        /// <returns>Vista parcial de Detalles Anular GT</returns>
        
        public ActionResult AnularGastoTransporte(string codigo)
        {
            FormularioGastoTransporteVM modelo = new FormularioGastoTransporteVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(codigo);
                modelo.GastoTransporte = (CGastoTransporteDTO)datos[0][0];
                var datosAnulacion = ServicioViaticoCorridoGastoTransporte.AnularGastoTransporte(modelo.GastoTransporte);
                if (datosAnulacion.GetType() != typeof(CErrorDTO))
                {
                    modelo.Mensaje = "Exito";
                }
                else
                {
                    modelo.Mensaje = "Error";
                }
            }
            return PartialView("_DetailsAnularGastoTransporte", modelo);
        }
        /// <summary>
        /// Obtiene un movimiento de Gasto Transporte específico de acuerdo a su código y le pone
        /// estado anulado(2)
        /// </summary>
        /// <param name="codigo">PrimaryKey del movimiento en la BD</param>
        /// <returns>Vista parcial de Detalles Anular GT</returns>
        public ActionResult AnularMovimientoGastoTransporte(string codigo)
        {
            FormularioGastoTransporteVM modelo = new FormularioGastoTransporteVM();
            if (ModelState.IsValid == true)
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerMovimientoGastoTransporte(codigo);
                modelo.MovimientoGastoTransporte = (CMovimientoGastoTransporteDTO)datos;
                var datosAnulacion = ServicioViaticoCorridoGastoTransporte.AnularMovimientoGastoTransporte(modelo.MovimientoGastoTransporte);
                if (datosAnulacion.GetType() != typeof(CErrorDTO))
                {
                    modelo.Mensaje = "Exito";
                }
                else
                {
                    modelo.Mensaje = "Error";
                }
            }
            //Vista que muestra si hubo éxito o no
            return PartialView("_DetailsAnularGastoTransporte", modelo);
        }


        //-----------------------------------------------------------------------------------------------------------------------//
        //---------------------------------------------------Reportes------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Metodo encargado de enviar la peticion de Crystal de una asignacion de Viatico Corrido
        /// </summary>
        /// <param name="modelV"></param>
        /// <returns></returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteAsignacionVC(FormularioViaticoCorridoVM modelV)
        {
            double suma = 0;
            int j = 0;
            var dato = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnteriores(modelV.Funcionario.Cedula);
            modelV.ViaticoCorridoList = new List<CViaticoCorridoDTO>();
            if (dato != null)
            {
                foreach (var item in dato[1])
                {
                    modelV.ViaticoCorridoList.Add((CViaticoCorridoDTO)item);
                    suma = Convert.ToDouble(modelV.ViaticoCorridoList[j].MontViaticoCorridoDTO) + suma;
                    j++;
                }
                modelV.TotalMA = suma;
            }
            if (modelV.ViaticoCorrido.PernocteDTO != null ||
            modelV.ViaticoCorrido.HospedajeDTO != null ||
            modelV.ViaticoCorrido.PernocteDTO != "" ||
            modelV.ViaticoCorrido.HospedajeDTO != "")
            {
                modelV.Cabinas = "Si";
            }
            else
            {
                modelV.Cabinas = "No";
            }
            var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(modelV.Funcionario.Cedula);
            var datosDireccion = ServicioFuncionario.DescargarDireccion(modelV.Funcionario.Cedula);
            if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                {
                    if (datosDireccion[0].GetType() != typeof(CErrorDTO))
                    {
                        modelV.Direccion = (CDireccionDTO)datosDireccion[0];
                        modelV.Direccion.Distrito = (CDistritoDTO)datosDireccion[3];
                        modelV.Direccion.Distrito.Canton = (CCantonDTO)datosDireccion[2];
                        modelV.Direccion.Distrito.Canton.Provincia = (CProvinciaDTO)datosDireccion[1];
                    }
                }
                //modelV.Nombramiento = modelV.ViaticoCorrido.NombramientoDTO;
            }

            List<AsignacionPI568RptData> modelo = new List<AsignacionPI568RptData>();
            for (int i = 0; i < modelV.ViaticoCorridoList.Count; i++)
            {
                modelo.Add(AsignacionPI568RptData.GenerarDatosReporte(modelV, String.Empty, i));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/ViaticoCorrido"), "AsignacionPI-568.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        /// <summary>
        /// Metodo encargado de enviar la peticion de Crystal de una deduccion de Viatico Corrido
        /// </summary>
        /// <param name="modelV"></param>
        /// <returns></returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleDeduccionVC(FormularioViaticoCorridoVM modelV)
        {
            string cod = modelV.MovimientoViaticoCorrido.IdEntidad.ToString();
            var dato = ServicioViaticoCorridoGastoTransporte.ObtenerDeduccionViaticoCorrido(cod);
            
            modelV.Deduccion = new List<CDetalleDeduccionViaticoCorridoDTO>();
            if (dato != null)
            {
                foreach (var item in dato)
                {
                    modelV.Deduccion.Add((CDetalleDeduccionViaticoCorridoDTO)item);
                }
            }
            List<DeduccionPI1104RptData> modelo = new List<DeduccionPI1104RptData>();
            for (int i = 0; i < modelV.Deduccion.Count; i++)
            {
                modelo.Add(DeduccionPI1104RptData.GenerarDatosReporte(modelV, String.Empty, i));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/ViaticoCorrido"), "DeduccionPI-1104.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }
        
        /// <summary>
        /// Metodo encargado de enviar la peticion de Crystal de una eliminacion de Viatico Corrido
        /// </summary>
        /// <param name="modelV"></param>
        /// <returns></returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleEliminacionVC(FormularioViaticoCorridoVM modelV)
        {
            List<EliminacionPI1103RptData> modelo = new List<EliminacionPI1103RptData>();
            modelo.Add(EliminacionPI1103RptData.GenerarDatosReporte(modelV, String.Empty));
            string reportPath = Path.Combine(Server.MapPath("~/Reports/ViaticoCorrido"), "EliminacionPI-1103.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetallePagoVC(FormularioViaticoCorridoVM modelV)
        {
            List<DetallePagoRptData> modelo = new List<DetallePagoRptData>();
            
            if (modelV.ViaticoCorrido.Pagos[0].Detalles != null)
            {
                modelV.TotalMA = Convert.ToDouble(modelV.ViaticoCorrido.Pagos[0].Detalles.Sum(Q => Q.MonPago));
                modelV.diasRebajo = modelV.ViaticoCorrido.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad != 5).Count();
                modelV.TotalRebajo = Convert.ToDouble(modelV.ViaticoCorrido.Pagos[0].Detalles.Where(Q=> Q.TipoDetalleDTO.IdEntidad != 5).Sum(Q => Q.MonPago));
                modelV.diasReintegro = modelV.ViaticoCorrido.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad == 5).Count();
                modelV.TotalReintegro = Convert.ToDouble(modelV.ViaticoCorrido.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad == 5).Sum(Q => Q.MonPago));

                for (int i = 0; i < modelV.ViaticoCorrido.Pagos[0].Detalles.Count; i++)
                {
                    modelo.Add(DetallePagoRptData.GenerarDatosReporteVC(modelV, String.Empty, i));
                }
            }
            else
            {
                modelV.TotalMA = 0;
                modelV.diasRebajo = 0;
                modelV.TotalRebajo = 0;
                modelV.diasReintegro = 0;
                modelV.TotalReintegro = 0;
                modelo.Add(DetallePagoRptData.GenerarDatosReporteVC(modelV, String.Empty, 0));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/ViaticoCorrido"), "DetallePagoVC.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReportePagoMesVC(ListaViaticoCorridoVM modelV)
        {
            List<PagoMesRptData> modelo = new List<PagoMesRptData>();

            if (modelV.Viaticos != null && modelV.Viaticos.Count > 0)
            {
                var lista = modelV.Viaticos
                                    .OrderBy(Q => Q.Pagos[0].ReservaRecurso)
                                    .ThenBy(Q => Q.PresupuestoDTO.CodigoPresupuesto)
                                    .ThenBy(Q => Q.NombramientoDTO.Funcionario.PrimerApellido)
                                    .ThenBy(Q => Q.NombramientoDTO.Funcionario.SegundoApellido)
                                    .ThenBy(Q => Q.NombramientoDTO.Funcionario.Nombre)
                                    .ToList();

                for (int i = 0; i < lista.Count; i++)
                {
                    FormularioViaticoCorridoVM formulario = new FormularioViaticoCorridoVM();
                    formulario.ViaticoCorrido = lista[i]; //modelV.Viaticos[i];
                    formulario.MesSeleccion = modelV.MesSeleccion;
                    modelo.Add(PagoMesRptData.GenerarDatosReporteVC(formulario, String.Empty, i));
                }
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/ViaticoCorrido"), "PagoMesVC.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }
        /// <summary>
        /// Metodo encargado de enviar la peticion de Crystal de una asignacion de Gasto Transporte
        /// </summary>
        /// <param name="modelG">ViewModel con los datos del gasto</param>
        /// <returns></returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteAsigancionGT(FormularioGastoTransporteVM modelG)
        {
            double suma = 0;
            int j = 0;
            var dato = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnterioresGastoTransporte(modelG.Funcionario.Cedula);
            modelG.GastoTransporteList = new List<CGastoTransporteDTO>();
            if (dato != null)
            {
                foreach (var item in dato[1])
                {
                    modelG.GastoTransporteList.Add((CGastoTransporteDTO)item);
                    suma = Convert.ToDouble(modelG.GastoTransporteList[j].MontGastoTransporteDTO) + suma;
                    j++;
                }
                modelG.TotalMA = suma;
            }

            var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(modelG.Funcionario.Cedula);
            var datosDireccion = ServicioFuncionario.DescargarDireccion(modelG.Funcionario.Cedula);
            if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                {
                    if (datosDireccion[0].GetType() != typeof(CErrorDTO))
                    {
                        modelG.Direccion = (CDireccionDTO)datosDireccion[0];
                        modelG.Direccion.Distrito = (CDistritoDTO)datosDireccion[3];
                        modelG.Direccion.Distrito.Canton = (CCantonDTO)datosDireccion[2];
                        modelG.Direccion.Distrito.Canton.Provincia = (CProvinciaDTO)datosDireccion[1];
                    }
                }
                modelG.Nombramiento = (CNombramientoDTO)datosFuncionario[6];
            }
            //var datos = ServicioViaticoCorridoGastoTransporte.ListarPagoMesesAnterioresGastoTransporte(modelG.Funcionario.Cedula);
            modelG.GastoTransporteList = new List<CGastoTransporteDTO>();
            if (dato != null)
            {
                foreach (var item in dato[1])
                {
                    modelG.GastoTransporteList.Add((CGastoTransporteDTO)item);
                }

            }
            //Obtener las rutas (originales o modificadas) a mostrar
            var datoA = ServicioViaticoCorridoGastoTransporte.ListarAsignacion(modelG.GastoTransporte.IdEntidad);
            var datosModif = ServicioViaticoCorridoGastoTransporte.ListarAsignacionModificada(modelG.GastoTransporte.IdEntidad);

            modelG.Rutas = new List<CDetalleAsignacionGastoTransporteDTO>();
            if (dato != null)
            {
                //foreach (var item in datoA)
                //{
                //    modelG.Rutas.Add((CDetalleAsignacionGastoTransporteDTO)item);
                //}

                //Para sobreponer las rutas modificadas del gasto en el reporte (si las hay)
                foreach (var item in datoA)
                {
                    CDetalleAsignacionGastoTransporteDTO rutaOriginal = (CDetalleAsignacionGastoTransporteDTO)item;
                    bool Modified = false;

                    foreach (var modified in datosModif)
                    {
                        CDetalleAsignacionGastoTransporteModificadaDTO rutaNueva = (CDetalleAsignacionGastoTransporteModificadaDTO)modified;

                        //Verificar si la ruta original coincide con alguna en la lista de rutas nuevas, se ha modificado y debe mostrarse
                        if (rutaNueva.NomRutaDTO.Equals(rutaOriginal.NomRutaDTO) &&
                            rutaNueva.NomFraccionamientoDTO.Equals(rutaOriginal.NomFraccionamientoDTO))
                        {
                            //Se convierte el tipo de dato de la original a las modificadas para mostrarla sin numero de gaceta
                            var tempRModified = new CDetalleAsignacionGastoTransporteDTO
                            {
                                IdEntidad = rutaNueva.IdEntidad,
                                NomRutaDTO = rutaNueva.NomRutaDTO,
                                NomFraccionamientoDTO = rutaNueva.NomFraccionamientoDTO,
                                //MontTarifa = rutaNueva.MontTarifa
                            };

                            Modified = true;
                            modelG.Rutas.Add(tempRModified);
                        }
                    }
                    //Si no ha sido modificada, se agrega para mostrarse
                    if (!Modified)
                    {
                        modelG.Rutas.Add((CDetalleAsignacionGastoTransporteDTO)item);
                    }
                }
            }

            //Recorrer gastos para sacar la fecha de registro y el monto de los meses a pagar
            string allMonths = "";
            string allMonthsMoney = "";
            for (int g = 0; g < modelG.GastoTransporteList.Count; g++)
            {
                var FecRegistroGT = modelG.GastoTransporteList[g].FecRegistroDTO;
                var FecInicioGT = modelG.GastoTransporteList[g].FecInicioDTO;
                var difMeses = ((FecRegistroGT.Year - FecInicioGT.Year) * 12) + FecRegistroGT.Month - FecInicioGT.Month;

                List<SelectListItem> listaMeses = new List<SelectListItem>();
                
                modelG.TotalMA = 0;
                for (int m = 0; m < difMeses; m++)
                {
                    var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[FecInicioGT.Month - 1].ToString()) + " del " + FecRegistroGT.Year.ToString();
                    listaMeses.Add(new SelectListItem() { Text = mes, Value = FecInicioGT.ToShortDateString() });
                    FecInicioGT = FecInicioGT.AddMonths(1);
                    allMonths += mes + "\n";
                    allMonthsMoney += Convert.ToDouble(modelG.GastoTransporteList[g].MontGastoTransporteDTO).ToString("#,#.00#;(#,#.00#)") + " \n";
                    modelG.TotalMA += Convert.ToDouble(modelG.GastoTransporteList[g].MontGastoTransporteDTO);
                }
            }

            int i = 0;
            int h = 0;
            int k = 0;
            List<AsignacionPI555RptData> modelo = new List<AsignacionPI555RptData>();
            List<CPagoGastoTransporteDTO> tempi = new List<CPagoGastoTransporteDTO>();
            for (i = 0; i < modelG.GastoTransporteList.Count; i++)
            {
                while (k < modelG.Rutas.Count || k < modelG.GastoTransporteList[i].Pagos.Count)
                {
                    modelo.Add(AsignacionPI555RptData.GenerarDatosReporte(modelG, allMonths, allMonthsMoney, i, k, h));
                    h++;
                    k++;
                }

                //for (int k = 0; k < modelG.Rutas.Count; k++)
                //{
                //    //modelo.Add(AsignacionPI555RptData.GenerarDatosReporte(modelG, String.Empty, i, k));
                //}
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/GastoTransporte"), "AsignacionPI-555.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        /// <summary>
        /// Metodo encargado de enviar la peticion de Crystal de una deduccion de Gasto Transporte
        /// </summary>
        /// <param name="modelV"></param>
        /// <returns></returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleDeduccionGT(FormularioGastoTransporteVM modelG)
        {
            string cod = modelG.MovimientoGastoTransporte.IdEntidad.ToString();
            var dato = ServicioViaticoCorridoGastoTransporte.ObtenerDeduccionGastoTransporte(cod);
            modelG.Deduccion = new List<CDetalleDeduccionGastoTransporteDTO>();
            if (dato != null)
            {
                foreach (var item in dato)
                {
                    modelG.Deduccion.Add((CDetalleDeduccionGastoTransporteDTO)item);
                }
            }
            List<DeduccionPI1105RptData> modelo = new List<DeduccionPI1105RptData>();
            for (int i = 0; i < modelG.Deduccion.Count; i++)
            {
                modelo.Add(DeduccionPI1105RptData.GenerarDatosReporte(modelG, String.Empty, i));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/GastoTransporte"), "DeduccionPI-1105.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }
        
        /// <summary>
        /// Metodo encargado de enviar la peticion de Crystal de una eliminacion de Gasto Transporte
        /// </summary>
        /// <param name="modelV"></param>
        /// <returns></returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleEliminacionGT(FormularioGastoTransporteVM modelG)
        {
            List<EliminacionPI1106RptData> modelo = new List<EliminacionPI1106RptData>();
            modelo.Add(EliminacionPI1106RptData.GenerarDatosReporte(modelG, String.Empty));
            string reportPath = Path.Combine(Server.MapPath("~/Reports/GastoTransporte"), "EliminacionPI-1106.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        /// <summary>
        /// Genera un reporte con los datos del funcionario, el gasto y los pagos que se han 
        /// hecho a este y lo exporta como un archivo PDF.
        /// </summary>
        /// <param name="model">ViewModel del gasto de transporte</param>
        /// <returns>Archivo PDF con los datos de pagos</returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetallePagoGT(FormularioGastoTransporteVM modelG)
        {
            List<DetallePagoGTRptData> model = new List<DetallePagoGTRptData>();

            if (modelG.GastoTransporte.Pagos[0].Detalles != null)
            {
                modelG.TotalMA = Convert.ToDouble(modelG.GastoTransporte.Pagos[0].Detalles.Sum(Q => Q.MonPago));
                modelG.diasRebajo = modelG.GastoTransporte.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad != 5).Count();
                modelG.TotalRebajo = Convert.ToDouble(modelG.GastoTransporte.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad != 5).Sum(Q => Q.MonPago));
                modelG.diasReintegro = modelG.GastoTransporte.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad == 5).Count();
                modelG.TotalReintegro = Convert.ToDouble(modelG.GastoTransporte.Pagos[0].Detalles.Where(Q => Q.TipoDetalleDTO.IdEntidad == 5).Sum(Q => Q.MonPago));

                for (int i = 0; i < modelG.GastoTransporte.Pagos[0].Detalles.Count; i++)
                {
                    model.Add(DetallePagoGTRptData.GenerarDatosReporteGT(modelG, String.Empty, i));
                }
            }
            else
            {
                modelG.TotalMA = 0;
                modelG.diasRebajo = 0;
                modelG.TotalRebajo = 0;
                modelG.diasReintegro = 0;
                modelG.TotalReintegro = 0;
                model.Add(DetallePagoGTRptData.GenerarDatosReporteGT(modelG, String.Empty, 0));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/GastoTransporte"), "DetallePagoGT.rpt");
            return new CrystalReportPdfResult(reportPath, model, "PDF");
        }
        
        public CrystalReportPdfResult ReportePagoMesGT(ListaGastoTransporteVM modelG)
        {
            List<PagoMesRptData> modelo = new List<PagoMesRptData>();

            if (modelG.Gastos != null && modelG.Gastos.Count > 0)
            {
                var lista = modelG.Gastos
                                    .OrderBy(Q => Q.Pagos[0].ReservaRecurso)
                                    .ThenBy(Q => Q.PresupuestoDTO.CodigoPresupuesto)
                                    .ThenBy(Q => Q.NombramientoDTO.Funcionario.PrimerApellido)
                                    .ThenBy(Q => Q.NombramientoDTO.Funcionario.SegundoApellido)
                                    .ThenBy(Q => Q.NombramientoDTO.Funcionario.Nombre)
                                    .ToList();
                for (int i = 0; i < lista.Count; i++)
                {
                    FormularioGastoTransporteVM formulario = new FormularioGastoTransporteVM();
                    formulario.GastoTransporte = lista[i]; // modelG.Gastos[i];
                    formulario.MesSeleccion = modelG.MesSeleccion;
                    modelo.Add(PagoMesRptData.GenerarDatosReporteGT(formulario, String.Empty, i));
                }
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/GastoTransporte"), "PagoMesGT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        //-------------------------------------------------------- OTROS ----------------------------------------------------
        [HttpPost]
        public ActionResult GetCantones(int idProvincia)
        {
            List<CCantonDTO> listado = new List<CCantonDTO>();
            try
            {
                var localizacion = ServicioPuesto.GetLocalizacion(true, 0, true, true, idProvincia);

                foreach (CCantonDTO canton in localizacion.ElementAt(0))
                {
                    listado.Add(new CCantonDTO
                    {
                        IdEntidad = canton.IdEntidad,
                        NomCanton = canton.NomCanton
                    });
                }

                return Json(new
                {
                    success = true,
                    listado = listado
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception error)
            {
                return Json(new { success = false, mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetDistritos(int idCanton)
        {
            List<CDistritoDTO> listado = new List<CDistritoDTO>();
            try
            {
                var localizacion = ServicioPuesto.GetLocalizacion(true, idCanton, true, true, 0);

                foreach (CDistritoDTO distrito in localizacion.ElementAt(1))
                {
                    listado.Add(new CDistritoDTO
                    {
                        IdEntidad = distrito.IdEntidad,
                        NomDistrito = distrito.NomDistrito
                    });
                }

                return Json(new
                {
                    success = true,
                    listado = listado
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método encargado de calcular cuántos días de rebajo le corresponden al funcionario y el 
        /// total a disminuir del gasto o viatico.
        /// (Metodo llamado en el FormularioGastoTransporte.js y en FormularioViaticoCorrido.js)
        /// </summary>
        /// <param name="cedula">Identificador del funcionario</param>
        /// <param name="fecha">Fecha en que se hacen los rebajos</param>
        /// <param name="monViatico">Monto total del viatico o gasto</param>
        /// <param name="tipo">Verifica si los calculos se hacen para un gasto o viatico</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDiasRebajar(string cedula, int idCodigo, DateTime fecha, decimal monViatico, string tipo)
        {
            decimal monDia = 0;
                        
            //List<CDetallePagoViaticoCorridoDTO> listadoDias = new List<CDetallePagoViaticoCorridoDTO>();
            try
            {
                CFuncionarioDTO funcionario = new CFuncionarioDTO  {
                    Cedula = cedula,
                    Sexo = GeneroEnum.Indefinido
                };

                CViaticoCorridoDTO viatico = new CViaticoCorridoDTO
                {
                    IdEntidad = idCodigo
                };
                monDia = Decimal.Round((monViatico / 26), 2);

                //var datos = new CBaseDTO[0];
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajar(funcionario, viatico, fecha.Month, fecha.Year, monDia);

                if (tipo != "Viatico")
                {
                    monDia = Decimal.Round((monViatico / 22), 2);
                    CGastoTransporteDTO gasto = new CGastoTransporteDTO
                    {
                        IdEntidad = idCodigo
                    };
                    datos = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajarGT(funcionario, gasto, fecha.Month, fecha.Year, monDia);
                }
                    
                return Json(new
                {
                    success = true,
                    lista = datos, 
                    montoMes = 0
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, porc = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetDiasPagarVC(int idCodigo, DateTime fecha, decimal monViatico)
        {
            decimal monDia = 0;
            try
            {
                CViaticoCorridoDTO viatico = new CViaticoCorridoDTO
                {
                    IdEntidad = idCodigo
                };
                monDia = Decimal.Round((monViatico / 26), 2);
                var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagar(viatico, fecha.Month, fecha.Year);
                if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    var maxDias = ((CDetallePagoViaticoCorridoDTO)diasPagar.FirstOrDefault()).IdEntidad;
                    if (maxDias < 26)
                        monViatico = Decimal.Round((monDia * maxDias), 2);
                }
                
                return Json(new
                {
                    success = true,
                    monto = monViatico
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, porc = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetDiasPagarGT(int idCodigo, DateTime fecha, decimal monGasto)
        {
            decimal monDia = 0;
            try
            {
                CGastoTransporteDTO gasto = new CGastoTransporteDTO
                {
                    IdEntidad = idCodigo
                };
                monDia = Decimal.Round((monGasto / 22), 2);
                var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagarGT(gasto, fecha.Month, fecha.Year);
                if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    var maxDias = ((CDetallePagoGastoTrasporteDTO)diasPagar.FirstOrDefault()).IdEntidad;
                    if (maxDias < 22)
                        monGasto = Decimal.Round((monDia * maxDias), 2);
                }

                return Json(new
                {
                    success = true,
                    monto = monGasto
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, porc = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Encargado de verificar validaciones y obtener los detalles(motivos) de una deduccion 
        /// de Viatico Corrido en espefícico.
        /// (Este método se llama desde el método JS "ObtenerDetallesDeduccion" cuando el usuario hace cambios
        /// en el dropdown "mesDeduccion" del formulario de deduccion)
        /// </summary>
        /// <param name="idViatico">Código identificador del VC</param>
        /// <param name="fecha">Fecha de pago seleccionada en el dropdown mesDeduccion</param>
        /// <returns>Json con el listado de detalles de deduccion y otras variables que se utilizan en el método js</returns>
        [HttpPost]
        public ActionResult GetDetalleDeduccion(int idViatico, DateTime fecha)
        {
            List<CDetalleDeduccionViaticoCorridoDTO> listadoDias = new List<CDetalleDeduccionViaticoCorridoDTO>();
            string Observaciones = "";

            try
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerViaticoCorrido(idViatico.ToString());
                bool bolAgregarDeducciones = true;

                if (datos != null)
                {
                    FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();
                    var viatico = (CViaticoCorridoDTO)datos[0][0];

                    // Verificar si el viático es Válido. Si no, no se pueden agregar deducciones
                    if (viatico.EstadoViaticoCorridoDTO.IdEntidad == 3)
                        bolAgregarDeducciones = false;

                    // Verificar si ya se procesó el pago para esa fecha. Si se procesó, no se pueden agregar deducciones
                    var pago = viatico.Pagos.Where(Q => Q.FecPago == fecha && Q.IndEstado == 1).ToList();
                    if (pago.Count > 0)
                        bolAgregarDeducciones = false;

                    var movimiento = viatico.Movimientos.Where(Q => Q.FecMovimientoDTO == fecha && Q.EstadoDTO == 1).ToList();

                    if(movimiento.Count > 0)
                    {
                        //if (movimiento.FirstOrDefault().EstadoDTO == 1)
                        //    bolAgregarDeducciones = false;

                        Observaciones = movimiento.FirstOrDefault().ObsObservacionesDTO;

                        foreach (var item in movimiento.FirstOrDefault().Deducciones)
                        {
                            listadoDias.Add(new CDetalleDeduccionViaticoCorridoDTO
                            {
                                IdEntidad = item.IdEntidad,
                                DesMotivoDTO = item.DesMotivoDTO,
                                FecRigeDTO = item.FecRigeDTO,
                                FecVenceDTO = item.FecVenceDTO,
                                NumNoDiaDTO = item.NumNoDiaDTO,
                                MontMontoBajarDTO = item.MontMontoBajarDTO,
                                MontMontoRebajarDTO = item.MontMontoRebajarDTO,
                                TotRebajarDTO = item.TotRebajarDTO,
                                NumSolicitudAccionPDTO = item.NumSolicitudAccionPDTO
                            });
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    lista = listadoDias,
                    agregar = bolAgregarDeducciones,
                    observaciones = Observaciones
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Encargado de verificar validaciones y obtener los detalles(motivos) de deducción 
        /// de un Gasto de Transporte en específico.
        /// (Este método se llama desde el método JS "ObtenerDetallesDeduccion" cuando el usuario hace cambios
        /// en el dropdown "mesDeduccion" del formulario de deduccion)
        /// </summary>
        /// <param name="idGastoTrans">Identificador del gasto de transporte (la PK)</param>
        /// <param name="fecha">Fecha de pago seleccionada en el dropdown mesDeduccion</param>
        /// <returns>Json con el listado de detalles de deduccion y otras variables que se utilizan en el método js</returns>
        [HttpPost]
        public ActionResult GetDetalleDeduccionGT(int idGastoTrans, DateTime fecha)
        {
            List<CDetalleDeduccionGastoTransporteDTO> listadoDias = new List<CDetalleDeduccionGastoTransporteDTO>();
            string Observaciones = "";

            try
            {
                var datos = ServicioViaticoCorridoGastoTransporte.ObtenerGastosTransporte(idGastoTrans.ToString());
                bool agregarDeducciones = true;

                if (datos != null)
                {
                    FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();
                    var gasto = (CGastoTransporteDTO)datos[0][0];
                    
                    // Verificar si el gasto es Válido. Si no, no se pueden agregar deducciones
                    if (gasto.EstadoGastoTransporteDTO.IdEntidad == 3)
                        agregarDeducciones = false;
                    
                    // Verificar si ya se procesó el pago del gasto para esa fecha. Si se procesó, no se pueden aplicar deducciones
                    var pago = gasto.Pagos.Where(Q => Q.FecPago == fecha && Q.IndEstado == 1).ToList();
                    if (pago.Count > 0)
                        agregarDeducciones = false;
                    
                    var movimiento = gasto.Movimientos.Where(Q => Q.FecMovimientoDTO == fecha && Q.EstadoDTO == 1).ToList();//ACM: para gastos la table no tiene fecha movimiento

                    if (movimiento.Count > 0)
                    {
                        Observaciones = movimiento.FirstOrDefault().ObsObservacionesDTO;

                        foreach (var item in movimiento.FirstOrDefault().Deducciones)
                        {
                            listadoDias.Add(new CDetalleDeduccionGastoTransporteDTO
                            {
                                IdEntidad = item.IdEntidad,
                                DesMotivoDTO = item.DesMotivoDTO,
                                FecRigeDTO = item.FecRigeDTO,
                                FecVenceDTO = item.FecVenceDTO,
                                NumNoDiaDTO = item.NumNoDiaDTO,
                                MontMontoBajarDTO = item.MontMontoBajarDTO,
                                MontMontoRebajarDTO = item.MontMontoRebajarDTO,
                                TotRebajarDTO = item.TotRebajarDTO,
                                NumSolicitudAccionPDTO = item.NumSolicitudAccionPDTO
                            });
                        }
                    }
                }
                return Json(new
                {
                    success = true,
                    lista = listadoDias,
                    agregar = agregarDeducciones,
                    observaciones = Observaciones
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Método para leer el archivo json que tiene las rutas de ARESEP
        /// </summary>
        /// <returns></returns>
        public string ReadJson()
        {
            string file = Server.MapPath("~/Helpers/Datos_Abiertos_ARESEP_.json");

            string jsonResult;

            using (StreamReader streamReader = new StreamReader(file))
            {
                jsonResult = streamReader.ReadToEnd();
            }
            return jsonResult;
        }

        /// <summary>
        /// Método encargado de buscar y mostrar los resultados de las rutas, gestionando las 
        /// páginas en la vista cuando los resultados son extensos.
        /// </summary>
        /// <param name="model">Viewmodel del gasto de transporte</param>
        /// <param name="numeroRuta">Codigo de ruta</param>
        /// <param name="nomFraccionamiento">Nombre de fraccionamiento </param>
        /// <param name="page">Numero de página que se está mostrando</param>
        /// <returns>La vista parcial con los resultados de la búsqueda.</returns>

        public PartialViewResult Search_Rutas_Aresep(FormularioGastoTransporteVM model, string numeroRuta, string nomFraccionamiento, int? page)
        {
            try
            {
                int paginaActual = page.HasValue ? page.Value : 1;

                if (string.IsNullOrEmpty(numeroRuta) && string.IsNullOrEmpty(nomFraccionamiento))
                {
                    return PartialView();
                }
                else
                {
                    //Llenar datos para las páginas de búsqueda

                    model.CodigoSearch = numeroRuta;
                    model.NombreSearch = nomFraccionamiento;
                    //Búsqueda de la/s rutas.                    
                    var foundRutas = BusquedaRuta(model.CodigoSearch, model.NombreSearch);

                    model.TotalRutas = foundRutas.Count();
                    model.TotalPaginas = (int)Math.Ceiling((double)model.TotalRutas / 10);
                    model.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > model.TotalRutas)
                    {
                        model.RutasARESEP = foundRutas.ToList().GetRange((paginaActual - 1) * 10, (model.TotalRutas - (paginaActual - 1) * 10)).ToList();
                    }
                    else
                    {
                        model.RutasARESEP = foundRutas.ToList().GetRange((paginaActual - 1) * 10, 10).ToList();
                    }
                    return PartialView("_RutasAresepResult", model);
                }
            }
            catch (Exception error)
            {
                Response.Write(error);
                ModelState.AddModelError("Busqueda Rutas", "Ha ocurrido un error en la búsqueda de rutas, póngase en contacto con el personal autorizado.");
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        /// <summary>
        /// Método encargado de buscar en los datos de ARESEP (archivo json) 
        /// según los parámetros(codigoRuta o nombreFraccionamiento) ingresados por el usuario.
        /// </summary>
        /// <param name="numRuta"></param>
        /// <param name="nomFrac"></param>
        /// <returns></returns>
        List<Models.RutasARESEPModel> BusquedaRuta(string numRuta, string nomFrac)
        {
            List<Models.RutasARESEPModel> rutasList = new List<Models.RutasARESEPModel>();
            rutasList = JsonConvert.DeserializeObject<List<Models.RutasARESEPModel>>(ReadJson());

            var foundRutas = new List<Models.RutasARESEPModel>();
            if (!string.IsNullOrEmpty(numRuta) && !string.IsNullOrEmpty(nomFrac))
            {
                nomFrac = nomFrac.ToUpper();
                foundRutas = rutasList.Where(r => r.NomFraccionamiento.Contains(nomFrac.ToUpper()) && r.CodRuta == numRuta).ToList();
            }
            else if (!string.IsNullOrEmpty(nomFrac))
            {
                nomFrac = nomFrac.ToUpper();
                foundRutas = rutasList.Where(r => r.NomFraccionamiento.Contains(nomFrac.ToUpper())).ToList();
                return foundRutas.ToList();
            }
            else if (!string.IsNullOrEmpty(numRuta))
            {
                foundRutas = rutasList.Where(r => r.CodRuta == numRuta).ToList();

            }
            return foundRutas;
        }

        /// <summary>
        /// Método encargado de enviar los datos correspondientes para hacer registros de rutas modificadas.
        /// </summary>
        /// <param name="IDgasto">PK del gasto</param>
        /// <param name="rutasModif">Lista de rutas a modificar</param>
        /// <param name="newMonto">Monto del gasto recalculado según las nuevas rutas.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarRutasModificadas(string IDgasto, List<List<string>> rutasModif, string newMonto)
        {
            bool inserta = true;
            string msjResultado = "";
            //Convertir las rutas que ingresa el usuario en una lista tipo "RutaModificada"
            List<CDetalleAsignacionGastoTransporteModificadaDTO> ModificadasList = new List<CDetalleAsignacionGastoTransporteModificadaDTO>();
            foreach (var item in rutasModif)
            {
                var modif = new CDetalleAsignacionGastoTransporteModificadaDTO
                {
                    NomRutaDTO = item[0],
                    NomFraccionamientoDTO = item[1],
                    MontTarifa = item[2],
                    NumGaceta = item[3]
                };
                ModificadasList.Add(modif);
            }

            //_____________________________________________________________________________________________________________________________________

            //Obtener las rutas que ya existen en la tabla de rutas modificadas
            List<CDetalleAsignacionGastoTransporteModificadaDTO> Existentes = new List<CDetalleAsignacionGastoTransporteModificadaDTO>();
            var modifExistentes = ServicioViaticoCorridoGastoTransporte.ListarAsignacionModificada(Convert.ToInt32(IDgasto));
            foreach (var item in modifExistentes)
            {
                CDetalleAsignacionGastoTransporteModificadaDTO rutaExistente =
                                (CDetalleAsignacionGastoTransporteModificadaDTO)item;
                var modif = new CDetalleAsignacionGastoTransporteModificadaDTO
                {
                    NomRutaDTO = rutaExistente.NomRutaDTO,
                    NomFraccionamientoDTO = rutaExistente.NomFraccionamientoDTO,
                    MontTarifa = rutaExistente.MontTarifa,
                    NumGaceta = rutaExistente.NumGaceta
                };
                Existentes.Add(modif);
            }

            //Y comparar las rutas que quiere guardar el usuario con las que ya existen, y sacar las rutas que vengan nuevas (que no estén en la lista de Existentes)
            RutasModificadasEqualityComparer comparer = new RutasModificadasEqualityComparer();
            var nuevasRutas = ModificadasList.Except(Existentes, comparer);//Las cosas de la lista de insertadas que no este en en las existentes


            //Ahora sacar las rutas de la tabla de rutas originales (Y ENGAÑAR CON EL TIPO DE DATO)
            List<CDetalleAsignacionGastoTransporteModificadaDTO> originales = new List<CDetalleAsignacionGastoTransporteModificadaDTO>();
            var asignOriginales = ServicioViaticoCorridoGastoTransporte.ListarAsignacion(Convert.ToInt32(IDgasto));
            foreach (var i in asignOriginales)
            {
                CDetalleAsignacionGastoTransporteModificadaDTO temp = new CDetalleAsignacionGastoTransporteModificadaDTO
                {
                    IdEntidad = ((CDetalleAsignacionGastoTransporteDTO)i).IdEntidad,
                    NomRutaDTO = ((CDetalleAsignacionGastoTransporteDTO)i).NomRutaDTO,
                    NomFraccionamientoDTO = ((CDetalleAsignacionGastoTransporteDTO)i).NomFraccionamientoDTO,
                    //MontTarifa = ((CDetalleAsignacionGastoTransporteDTO)i).MontTarifa,
                    NumGaceta = "-"
                };
                originales.Add(temp);
            }
            //PARA poder hacer la comparación y sacar de ahí las rutas que ya estén en la lista de originales
            var withoutOriginales = nuevasRutas.Except(originales, comparer);


            List<CDetalleAsignacionGastoTransporteModificadaDTO> definitivasEnviar = withoutOriginales.ToList();
            if (definitivasEnviar.Count > 0)
            {
                var insert = ServicioViaticoCorridoGastoTransporte.AgregarDetalleAsignModificada(definitivasEnviar.ToArray(), Convert.ToInt32(IDgasto));
                var mod = ServicioViaticoCorridoGastoTransporte.ModificarMontoGastoTrans(Convert.ToInt32(IDgasto), newMonto);
                //MODIFICAR MONTO

                if (insert[0].GetType() == typeof(CErrorDTO))
                {
                    msjResultado = ((CErrorDTO)insert[0]).MensajeError;
                    inserta = false;
                }
                else if (mod.GetType() == typeof(CErrorDTO))
                {
                    msjResultado = ((CErrorDTO)mod).MensajeError;
                    inserta = false;
                }
            }
            else
            {
                inserta = false;
            }

            //_____________________________________________________________________________________________________________________________________

            return Json(new
            {
                insertando = inserta,
                textoError = msjResultado,
                success = true
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CreateReintegroDiasGT(string numCedula, string fechaPago)
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();
            try
            {
                var datosFuncionario = ServicioViaticoCorridoGastoTransporte.BuscarFuncionarioCedula(numCedula);
                if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    if (datosFuncionario[4].GetType() != typeof(CErrorDTO))
                    {
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        var ubicacion = (CUbicacionPuestoDTO)datosFuncionario[5];
                        if (ubicacion.TipoUbicacion.IdEntidad == 1) // Contrato
                        {
                            model.UbicacionContrato = ubicacion;
                            model.UbicacionTrabajo = (CUbicacionPuestoDTO)datosFuncionario[6];
                        }
                        else
                        {
                            model.UbicacionContrato = (CUbicacionPuestoDTO)datosFuncionario[6];
                            model.UbicacionTrabajo = ubicacion;
                        }
                        model.Nombramiento = (CNombramientoDTO)datosFuncionario[7];
                        model.Fechalimite = model.Nombramiento.FecVence.Date.ToString("yyyy-MM-dd");

                        var listaEstados = new List<int> { 1, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
                        if (model.Nombramiento.EstadoNombramiento.IdEntidad == 2)
                        {
                            model.Nombramiento.EstadoNombramiento.DesEstado = "Interino";
                        }
                        else
                        {
                            if (listaEstados.Contains(model.Nombramiento.EstadoNombramiento.IdEntidad))
                            {
                                model.Nombramiento.EstadoNombramiento.DesEstado = "Propiedad";
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "No tiene un nombramiento válido");
                                throw new Exception("Busqueda");
                            }
                        }

                        var datosViatico = ServicioViaticoCorridoGastoTransporte.ObtenerGastoTransporteActual(model.Funcionario.Cedula);
                        if (datosViatico.GetType() != typeof(CErrorDTO))
                        {
                            model.GastoTransporte = (CGastoTransporteDTO)datosViatico;

                            List<DateTime> diasReintegro = new List<DateTime>();
                            List<CDetalleDeduccionGastoTransporteDTO> listaDetalle = new List<CDetalleDeduccionGastoTransporteDTO>();

                            var listadoPagos = model.GastoTransporte.Pagos.Where(Q => Q.IndEstado == 1).ToList();

                            foreach (var pagos in listadoPagos)
                            {
                                foreach (var detalle in pagos.Detalles)
                                {
                                    // Listar los días que ya se han reintegrado
                                    if (detalle.TipoDetalleDTO.IdEntidad == 5)  // 5 - Reintegro
                                        diasReintegro.Add(Convert.ToDateTime(detalle.FecDiaPago));
                                }
                            }

                            //Pago retroactivo. Se debe verificar que no se hayan hecho los pagos de meses anteriores
                            bool agregar = false;
                            decimal monto = 0;
                            DateTime fechaInicio = model.GastoTransporte.FecInicioDTO;
                            fechaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
                            DateTime fechaFinal = Convert.ToDateTime(fechaPago);

                            while (fechaInicio < fechaFinal)
                            {
                                if (listadoPagos.Count > 0)
                                {
                                    if (listadoPagos.Where(Q => Q.FecPago.Month == fechaInicio.Month && Q.FecPago.Year == fechaInicio.Year && Q.NumBoleta == "").FirstOrDefault() != null
                                        && diasReintegro.Contains(fechaInicio) == false)
                                    {
                                        monto = listadoPagos.Where(Q => Q.FecPago.Month == fechaInicio.Month && Q.FecPago.Year == fechaInicio.Year && Q.NumBoleta == "").FirstOrDefault().MonPago;
                                        agregar = true;
                                    }
                                }
                                else
                                {
                                    agregar = true;
                                }

                                if (agregar)
                                {
                                    listaDetalle.Add(new CDetalleDeduccionGastoTransporteDTO
                                    {
                                        DesMotivoDTO = "PAGO RETROACTIVO " + CultureInfo.CurrentUICulture.TextInfo.ToUpper(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[fechaInicio.Month - 1].ToString()),
                                        FecRigeDTO = fechaInicio.ToShortDateString(),
                                        FecVenceDTO = fechaInicio.ToShortDateString(),
                                        MontMontoRebajarDTO = monto.ToString(), // model.GastoTransporte.MontGastoTransporteDTO.ToString(),
                                        MontMontoBajarDTO = monto.ToString() //model.GastoTransporte.MontGastoTransporteDTO.ToString()
                                    });
                                }

                                fechaInicio = fechaInicio.AddMonths(1);
                                agregar = false;
                            }

                            foreach (var pagos in listadoPagos)
                            {
                                foreach (var detalle in pagos.Detalles)
                                {
                                    // Listar las deducciones que se le han hecho y no se le han devuelto
                                    if (detalle.TipoDetalleDTO.IdEntidad != 5 && diasReintegro.Contains(Convert.ToDateTime(detalle.FecDiaPago)) == false)  // 5 - Reintegro
                                        listaDetalle.Add(new CDetalleDeduccionGastoTransporteDTO
                                        {
                                            DesMotivoDTO = detalle.TipoDetalleDTO.DescripcionTipo,
                                            FecRigeDTO = detalle.FecDiaPago,
                                            FecVenceDTO = detalle.FecDiaPago,
                                            MontMontoRebajarDTO = detalle.MonPago.ToString(),
                                            MontMontoBajarDTO = detalle.MonPago.ToString()
                                        });
                                }
                            }
                            if (listaDetalle.Count == 0)
                                throw new Exception("No tiene deducciones para su reintegro");

                            model.DetalleD = listaDetalle;
                            model.Formulario = "Reintegro de Días";
                        }
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario[4]).MensajeError);
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        [HttpPost]
        public ActionResult CreateReintegroDiasGT(FormularioGastoTransporteVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Agregar Reintegro")
                {
                    ModelState.Clear();

                    if (model.MovimientoGT.ObsObservacionesDTO == null)
                        throw new Exception("Debe digitar el Motivo del Reintegro");
                    else if (model.MovimientoGT.ObsObservacionesDTO.TrimEnd().Length == 0)
                        throw new Exception("Debe digitar el Motivo del Reintegro");

                    if (model.GastoTransporte != null)
                    {
                        List<CGastoTransporteReintegroDTO> lista = new List<CGastoTransporteReintegroDTO>();
                        foreach (var item in model.DetalleD.Where(Q => Q.IndReintegroDTO).ToList())
                        {
                            lista.Add(new CGastoTransporteReintegroDTO
                            {
                                FecDiaDTO = Convert.ToDateTime(item.FecRigeDTO),
                                MonReintegroDTO = Convert.ToDecimal(item.MontMontoBajarDTO),
                                ObsMotivoDTO = model.MovimientoGT.ObsObservacionesDTO,
                                EstadoDTO = 1,
                                GastoTransporteDTO = model.GastoTransporte
                            });
                        }
                        LlenarModeloParaEnviarGastoTransporte(model);
                        var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarReintegroGT(lista.ToArray());
                        if (respuesta.GetType() == typeof(CErrorDTO))
                        {
                            ModelState.AddModelError("Formulario", ((CErrorDTO)respuesta).MensajeError);
                            throw new Exception("Formulario");
                        }
                        //return JavaScript("window.location = '/ViaticoCorrido/DetailsGastoTransporte?codigo=" + respuesta.IdEntidad + "&accion=Guardar';");
                        return new JavaScriptResult("ObtenerDetalleReintegroPago('" + model.Funcionario.Cedula + "','" + model.GastoTransporte.IdEntidad + "','" + model.GastoTransporte.MontGastoTransporteDTO + "','Gasto');");
                    }
                    else
                    {
                        ValidarExtraFormularioGastoTransporte(model);
                        throw new Exception("Formulario");
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        public ActionResult EditPagoReservaRecursoGT()
        {
            FormularioGastoTransporteVM model = new FormularioGastoTransporteVM();
            List<SelectListItem> listaMeses = new List<SelectListItem>();
            var fecha = new DateTime(DateTime.Today.Year, 1, 1);

            for (int i = 0; i <= 11; i++)
            {
                var mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i].ToString()) + " del " + DateTime.Today.Year.ToString();
                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                fecha = fecha.AddMonths(1);
            }
            model.MesesGasto = listaMeses;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPagoReservaRecursoGT(FormularioGastoTransporteVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (model.MesSeleccion != null)
                    {
                        var fecha = Convert.ToDateTime(model.MesSeleccion);
                        var datos = ServicioViaticoCorridoGastoTransporte.ListarPagosGastoTransporte(fecha.Month, fecha.Year);
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            //Cargar combobox de codigos presupuestarios
                            var codpresupuestos = ServicioPuesto.BuscarPresupuestoParams("");
                            //model.CodigosPresupuestoList = new List<string>();
                            List<string> tempPresupuestos = new List<string>();
                            if (codpresupuestos != null)
                            {
                                foreach (var i in codpresupuestos.ElementAt(0))
                                {
                                    tempPresupuestos.Add(i.CodigoPresupuesto);
                                }
                                model.CodigosPresupuestoList = new SelectList(tempPresupuestos);
                            }

                            CPagoGastoTransporteDTO pago = new CPagoGastoTransporteDTO();
                            model.GastoTransporte = new CGastoTransporteDTO { IdEntidad = 1 }; // Un registro de Gasto Vacío
                            model.GastoTransporte.Pagos = new List<CPagoGastoTransporteDTO>();

                            foreach (var item in datos)
                            {
                                pago = (CPagoGastoTransporteDTO)item[0];
                                if (pago.IndEstado == 1)
                                {
                                    pago.GastoTransporteDTO = (CGastoTransporteDTO)item[1];
                                    pago.GastoTransporteDTO.NombramientoDTO.Funcionario = (CFuncionarioDTO)item[2];
                                    model.GastoTransporte.Pagos.Add(pago);
                                }
                            }

                            return View("_EditPagoReservaRecursoGT", model);
                        }
                        else
                        {
                            //ModelState.AddModelError("Busqueda", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
                            throw new Exception("Busqueda");
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        throw new Exception("Debe escoger el Mes");
                    }
                }
                else
                {
                    if (model.PresupuestoSelected != null)
                    {
                        if (model.ReservaRecurso != null && model.ReservaRecurso != "")
                        {
                            var listado = model.GastoTransporte.Pagos.Where(Q => Q.GastoTransporteDTO.PresupuestoDTO.CodigoPresupuesto.Contains(model.PresupuestoSelected)).ToList();

                            foreach (var pago in listado)
                            {
                                pago.ReservaRecurso = model.ReservaRecurso;
                                var datos = ServicioViaticoCorridoGastoTransporte.AsignarReservaRecursoGT(pago);

                                if (datos.GetType() == typeof(CErrorDTO))
                                {
                                    ModelState.Clear();
                                    throw new Exception(((CErrorDTO)datos).MensajeError);
                                }
                            }

                            return View("EditPagoReservaRecursoGT", model);
                        }
                        else
                        {
                            ModelState.Clear();
                            throw new Exception("Debe digitar la Reserva Recurso");
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        throw new Exception("Debe escoger Código Presupuestario");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }

        public ActionResult ListarGastoTransportePagos()
        {
            ListaGastoTransporteVM model = new ListaGastoTransporteVM();
            List<SelectListItem> listaMeses = new List<SelectListItem>();
            var mes = "";

            var fecha = new DateTime(DateTime.Today.Year, 1, 1);

            for (int i = 0; i <= 11; i++)
            {
                mes = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i].ToString()) + " del " + DateTime.Today.Year.ToString();
                listaMeses.Add(new SelectListItem() { Text = mes, Value = fecha.ToShortDateString() });
                fecha = fecha.AddMonths(1);
            }
            model.MesesViatico = listaMeses;
            return View(model);
        }

        [HttpPost]
        public ActionResult ListarGastoTransportePagos(ListaGastoTransporteVM model, string SubmitButton)
        {
            try
            {
                DateTime fecha;
                CBaseDTO[][] datos;

                switch (SubmitButton)
                {
                    case "Buscar":
                    case "Inicio":
                        if (model.MesSeleccion == null)
                        {
                            ModelState.Clear();
                            throw new Exception("Debe escoger el Mes");
                        }
                        break;

                    case "Asignar Reserva":
                        if (model.PresupuestoSelected != null)
                        {
                            if (model.ReservaRecurso != null && model.ReservaRecurso != "")
                            {
                                var listado = model.Gastos.Where(Q => Q.PresupuestoDTO.CodigoPresupuesto.Contains(model.PresupuestoSelected) && Q.Pagos[0].IndSeleccionar == true).ToList();
                                foreach (var item in listado)
                                {
                                    item.Pagos[0].ReservaRecurso = model.ReservaRecurso;
                                    var datosAsignar = ServicioViaticoCorridoGastoTransporte.AsignarReservaRecursoGT(item.Pagos[0]);

                                    if (datosAsignar.GetType() == typeof(CErrorDTO))
                                    {
                                        ModelState.Clear();
                                        throw new Exception(((CErrorDTO)datosAsignar).MensajeError);
                                    }
                                }
                            }
                            else
                            {
                                ModelState.Clear();
                                throw new Exception("Debe digitar la Reserva Recurso");
                            }
                            break;
                        }
                        else
                        {
                            ModelState.Clear();
                            throw new Exception("Debe escoger Código Presupuestario");
                        }

                    case "Generar Pago":
                        decimal monPago = 0;
                        decimal monDia = 0;

                        // Generar los Contratos que tienen Monto Pago en cero
                        foreach (var itemGasto in model.Gastos.Where(Q => Q.Pagos[0].MonPago == 0 && Q.Pagos[0].IndSeleccionar == true).ToList())//.Where(Q => Q.IndReintegroDTO)
                        {
                            monPago = Convert.ToDecimal(itemGasto.MontGastoTransporteDTO);
                            monDia = Decimal.Round((monPago / 22), 2);
                            itemGasto.FecPagoDTO = Convert.ToDateTime(model.MesSeleccion);

                            var diasPagar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasPagarGT(itemGasto, itemGasto.FecPagoDTO.Month, itemGasto.FecPagoDTO.Year);
                            if (diasPagar.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                var maxDias = ((CDetallePagoGastoTrasporteDTO)diasPagar.FirstOrDefault()).IdEntidad;
                                if (maxDias < 22)
                                    monPago = Decimal.Round((monDia * maxDias), 2);
                            }

                            //Poner los datos del pago en un objeto
                            CPagoGastoTransporteDTO pago = new CPagoGastoTransporteDTO
                            {
                                FecPago = itemGasto.FecPagoDTO,
                                HojaIndividualizada = "",  // model.ViaticoCorrido.Pagos[0].HojaIndividualizada,
                                NumBoleta = "",
                                ReservaRecurso = "S/R",
                                GastoTransporteDTO = new CGastoTransporteDTO { IdEntidad = itemGasto.IdEntidad },
                                IndEstado = 1
                            };
                            //Obtener los detalles del pago (dias)
                            List<CDetallePagoGastoTrasporteDTO> detalles = new List<CDetallePagoGastoTrasporteDTO>();
                            var datosRebajar = ServicioViaticoCorridoGastoTransporte.ObtenerDiasRebajarGT(itemGasto.NombramientoDTO.Funcionario, itemGasto, itemGasto.FecPagoDTO.Month, itemGasto.FecPagoDTO.Year, monDia);
                            foreach (CDetallePagoGastoTrasporteDTO item in datosRebajar)
                            {
                                detalles.Add(new CDetallePagoGastoTrasporteDTO
                                {
                                    FecDiaPago = item.FecDiaPago,
                                    MonPago = item.MonPago,
                                    CodEntidad = item.CodEntidad,
                                    TipoDetalleDTO = item.TipoDetalleDTO
                                });
                                if (item.TipoDetalleDTO.IdEntidad == 5)
                                    monPago += item.MonPago;
                                else
                                    monPago -= item.MonPago;
                            }

                            pago.MonPago = monPago;
                            var respuesta = ServicioViaticoCorridoGastoTransporte.AgregarPagoGastoTransporte(pago, detalles.ToArray(), itemGasto.NombramientoDTO.Funcionario);
                        }
                        break;

                    default:
                        break;
                }

                fecha = Convert.ToDateTime(model.MesSeleccion);
                datos = ServicioViaticoCorridoGastoTransporte.ListarGastoTransportePago(fecha.Month, fecha.Year);
                if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    LlenarModeloListadoPagosGT(model, datos);
                    return View("_ListarGastoTransportePagos", model);
                }
                else
                {
                    //ModelState.AddModelError("Busqueda", ((CErrorDTO)datos.FirstOrDefault()).MensajeError);
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Formulario", ex.Message);
                return PartialView("_ErrorViaticoCorrido");
            }
        }



        public ActionResult CreateRegistroDias()
        {
            FormularioViaticoCorridoVM model = new FormularioViaticoCorridoVM();

            var listadoTipos = ServicioViaticoCorridoGastoTransporte.ListarTipoDia()
                         .Select(Q => new SelectListItem
                         {
                             Value = ((CTipoDiaDTO)Q.FirstOrDefault()).IdEntidad.ToString(),
                             Text = ((CTipoDiaDTO)Q.FirstOrDefault()).IdEntidad.ToString() + " - " + ((CTipoDiaDTO)Q.FirstOrDefault()).DescripcionTipoDia
                         });

            model.Estado = new SelectList(listadoTipos, "Value", "Text");

            var dato = ServicioViaticoCorridoGastoTransporte.ListarCatalogoDia();
            if(dato.FirstOrDefault().GetType() != typeof(CErrorDTO))
            {
                //model.CatalogoDia = new CCatDiaViaticoGastoDTO();
                //model.CatalogoDia.IndGasto = true;
                model.ListaCatalogoDia = new List<CCatDiaViaticoGastoDTO>();
                foreach (var item in dato)
                {
                    model.ListaCatalogoDia.Add((CCatDiaViaticoGastoDTO)item);
                }
            }
            else
            {
                model.Error = (CErrorDTO)dato.FirstOrDefault();
            }
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateRegistroDias(FormularioViaticoCorridoVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Registrar")
                {
                    if (ModelState.IsValid == true)
                    {
                        throw new Exception("Busqueda");
                    }
                    else
                    {
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorViaticoCorrido");
            }
        }


    }

    //------------------------------------------------------------------------------------------------------------------------//

    #endregion
}