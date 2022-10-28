using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.PagoFeriadoLocal;
using SIRH.Web.FuncionarioLocal;
using SIRH.DTO;
using SIRH.Web.Models;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Helpers;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.PerfilUsuarioService;
using System.Globalization;
using SIRH.Web.Reports.PagoFeriado;
using SIRH.Web.UserValidation;

namespace SIRH.Web.Controllers
{
    public class PagoFeriadoController : Controller
    {

        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CPagoFeriadoServiceClient servicioPagoFeriado = new CPagoFeriadoServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        /// <summary>
        /// Identifica los permisos del usuario logeado en el sistema
        /// </summary>
        /// <returns>Retorna true si el usuario puede acceder, de lo contrario retorna false</returns>


        //
        // GET: /PagoFeriado/Index/
        /// <summary>
        /// Carga la página principal del módulo
        /// </summary>
        /// <example>
        /// /PagoFeriado/Index
        /// </example>
        /// <returns>Retorna la vista principal</returns>
        public ActionResult Index()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CPagoFeriadoDTO).Name));
                return View();
            }
        }

        //
        // GET: /PagoFeriado/Details/
        /// <summary>
        /// Muestra los detalles al guardar, buscar o editar un trámite de pago
        /// </summary>
        /// <example>
        /// /PagoFeriado/Details
        /// </example>
        /// <param name="accion">Determina el tipo de acción realizada antes de ingresar a la interfaz de detalles</param>
        /// <param name="codigo">Número de trámite de pago</param>
        /// <returns>Retorna la vista de detalles</returns>
        public ActionResult Details(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Consulta"] != null ||
                    Session["Permiso_Feriado_Operativo"] != null)
                {
                    FormularioPagoFeriadoVM model = new FormularioPagoFeriadoVM();

                    var datos = servicioPagoFeriado.ObtenerPagoFeriado(codigo);


                    if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        var datos2 = servicioPagoFeriado.RetornarDiasPorTramitePagado(codigo);

                        if (datos2.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            var datos3 = servicioPagoFeriado.ListarDeduccionesPagoTipo(1, codigo);
                            if (datos3.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            {
                                var datos4 = servicioPagoFeriado.ListarDeduccionesPagoTipo(2, codigo);

                                if (datos4.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    model.PagoFeriados = (CPagoFeriadoDTO)datos.ElementAt(0);
                                    model.PagoExtraordinario = (CPagoExtraordinarioDTO)datos.ElementAt(1);
                                    model.Funcionario = (CFuncionarioDTO)datos.ElementAt(2);
                                    model.EstadoTramite = (CEstadoTramiteDTO)datos.ElementAt(3);
                                    model.CatalogoDiaAsueto = new List<CCatalogoDiaDTO>();
                                    model.DiasPagados = new List<CDiaPagadoDTO>();

                                    model.SubtotalDiferencias = Math.Round(model.PagoFeriados.MontoSubtotalDia + model.PagoFeriados.MontoSalarioEscolar, 2);
                                    foreach (var dia in datos2)
                                    {
                                        model.DiasPagados.Add((CDiaPagadoDTO)dia.ElementAt(0));
                                        model.CatalogoDiaAsueto.Add((CCatalogoDiaDTO)dia.ElementAt(3));

                                    }
                                    model.DeduccionesObreroEfectuada = new List<CDeduccionEfectuadaDTO>();
                                    model.DeduccionesPatronalEfectuada = new List<CDeduccionEfectuadaDTO>();
                                    model.DeduccionesObrero = new List<CCatalogoDeduccionDTO>();
                                    model.DeduccionesPatronal = new List<CCatalogoDeduccionDTO>();
                                    foreach (var deduccion in datos3)
                                    {
                                        var aux = deduccion.ElementAt(0);
                                        model.DeduccionesObreroEfectuada.Add((CDeduccionEfectuadaDTO)deduccion.ElementAt(0));
                                        model.DeduccionesObrero.Add((CCatalogoDeduccionDTO)deduccion.ElementAt(1));

                                    }
                                    foreach (var deduccionP in datos4)
                                    {
                                        model.DeduccionesPatronalEfectuada.Add((CDeduccionEfectuadaDTO)deduccionP.ElementAt(0));
                                        model.DeduccionesPatronal.Add((CCatalogoDeduccionDTO)deduccionP.ElementAt(1));

                                    }
                                    var datosFuncionario =
                                                servicioPagoFeriado.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                                    if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {
                                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];


                                        model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];
                                    }

                                    else
                                    {
                                        model.Error = (CErrorDTO)datosFuncionario.ElementAt(0);
                                    }
                                }
                                else
                                {
                                    model.Error = (CErrorDTO)datos4.ElementAt(0).ElementAt(0);
                                }
                            }
                            else
                            {
                                model.Error = (CErrorDTO)datos3.ElementAt(0).FirstOrDefault();
                            }
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos2.ElementAt(0).ElementAt(0);
                        }
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }

        }

        //-------Registrar trámite de pago------------------------------------------

        //
        // GET: /PagoFeriado/Create
        /// <summary>
        /// Carga la interfaz para almacenar un trámite de pago
        /// </summary>
        /// <example>
        /// /PagoFeriado/Create
        /// </example>
        /// <returns>Retorna la vista de Create</returns>
        public ActionResult Create()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null)
                {
                    FormularioPagoFeriadoVM model = new FormularioPagoFeriadoVM();
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
                }
            }
        }

        //
        // POST: /PagoFeriado/Create
        /// <summary>
        /// Realiza la búsqueda del funcionario y almacena el trámite de pago
        /// </summary>
        /// <example>
        /// /PagoFeriado/Create
        /// </example>
        /// <returns>Retorna la vista de detalles</returns>
        [HttpPost]
        public ActionResult Create(FormularioPagoFeriadoVM model, int[] horaTabla, string SubmitButton, string[] anioTabla, string[] salarioTabla, int[] id_dia, string[] deduccion_obreroTabla, string Total_deduccion_obreroTabla, string[] deduccion_patronalTabla, string total_deduccion_patronalTabla)
        {
            try
            {
                var codigoCanton = servicioPagoFeriado.ListarCantones().Select(
                 P => new SelectListItem
                 {
                     Value = ((CCantonDTO)P.ElementAt(0)).IdEntidad.ToString(),
                     Text = ((CCantonDTO)P.ElementAt(0)).NomCanton.ToString()
                 }
               );


                model.Canton = new SelectList(codigoCanton, "Value", "Text");

                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        var datosFuncionario =
                                    servicioPagoFeriado.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];


                            model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];
                            if (model.DetalleContratacion.CodigoPolicial > 0)
                            {

                                var desgloseSalarial = servicioPagoFeriado.BuscarDesgloceSalarialPF(model.Funcionario.Cedula);
                                if (desgloseSalarial.GetType() != typeof(CErrorDTO))
                                {
                                    model.DesgloseSalarial = (CDesgloseSalarialDTO)desgloseSalarial;
                                    var detallePuesto = servicioPagoFeriado.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                                    if (detallePuesto.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {
                                        model.DetallePuesto = (CDetallePuestoDTO)detallePuesto.ElementAt(2);

                                        List<CCatalogoDiaDTO> dias = new List<CCatalogoDiaDTO>();
                                        var diasCatalogo = servicioPagoFeriado.ListarDiasPorTipo(1);
                                        if (diasCatalogo != null)
                                        {
                                            foreach (var catalogo in diasCatalogo)
                                            {
                                                dias.Add((CCatalogoDiaDTO)catalogo.ElementAt(0));
                                            }
                                            model.CatalogoDiaFeriado = dias;
                                        }
                                        else
                                        {
                                            model.Error = (CErrorDTO)diasCatalogo.ElementAt(0).FirstOrDefault();
                                        }


                                        var CatalogoDeduccionesObrero = servicioPagoFeriado.ListarDeduccionesTipo(1);
                                        List<CCatalogoDeduccionDTO> deduccionesObrero = new List<CCatalogoDeduccionDTO>();
                                        if (CatalogoDeduccionesObrero.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                        {
                                            var aux2 = CatalogoDeduccionesObrero.ToArray();
                                            foreach (var deduccion in CatalogoDeduccionesObrero)
                                            {
                                                deduccionesObrero.Add((CCatalogoDeduccionDTO)deduccion.ElementAt(0));
                                            }
                                            model.DeduccionesObrero = deduccionesObrero;
                                            var CatalogoDeduccionesPatronal = servicioPagoFeriado.ListarDeduccionesTipo(2);
                                            List<CCatalogoDeduccionDTO> deduccionesPatronales = new List<CCatalogoDeduccionDTO>();
                                            if (CatalogoDeduccionesPatronal.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                            {
                                                var aux3 = CatalogoDeduccionesPatronal.ToArray();
                                                foreach (var deduccionP in CatalogoDeduccionesPatronal)
                                                {
                                                    deduccionesPatronales.Add((CCatalogoDeduccionDTO)deduccionP.ElementAt(0));

                                                }
                                                model.DeduccionesPatronal = deduccionesPatronales;



                                                var codigoNom = servicioPagoFeriado.BuscarPuestoPF(model.Funcionario.Cedula).Select(
                                                P => new SelectListItem
                                                {
                                                    Value = ((CPuestoDTO)P).IdEntidad.ToString(),
                                                    Text = ((CPuestoDTO)P).CodPuesto.ToString()
                                                }
                                                );
                                                var salEsc = servicioPagoFeriado.ObtenerSalarioEscolar();
                                                if (salEsc.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                                {
                                                    model.SalEscolar = (CCatalogoRemuneracionDTO)salEsc.ElementAt(0);

                                                    model.TipoNombramiento = new SelectList(codigoNom, "Value", "Text");


                                                    return PartialView("_FormularioPagoFeriado", model);
                                                }
                                                else
                                                {
                                                    ModelState.AddModelError("Busqueda", ((CErrorDTO)salEsc.FirstOrDefault()).MensajeError);
                                                    throw new Exception("Busqueda");
                                                }
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("Busqueda",
                                                    ((CErrorDTO)CatalogoDeduccionesPatronal.ElementAt(0).FirstOrDefault()).MensajeError);
                                                throw new Exception("Busqueda");
                                            }
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("Busqueda",
                                                ((CErrorDTO)CatalogoDeduccionesObrero.ElementAt(0).FirstOrDefault()).MensajeError);
                                            throw new Exception("Busqueda");
                                        }

                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Busqueda",
                                            ((CErrorDTO)detallePuesto.FirstOrDefault()).MensajeError);
                                        throw new Exception("Busqueda");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("Busqueda",
                                        ((CErrorDTO)desgloseSalarial).MensajeError);
                                    throw new Exception("Busqueda");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Busqueda", "Código policial inválido");
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
                } //Registrar trámite de pago
                else
                {
                    int A = ModelState.Count();
                    ModelState.Clear();
                    if (salarioTabla != null)
                    {
                        model.Funcionario.Sexo = GeneroEnum.Indefinido;
                        var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                        //Agregar Pago Extraordinario
                        DateTime thisDay = DateTime.Now;

                        CPagoExtraordinarioDTO pagoExtraordinario = new CPagoExtraordinarioDTO
                        {
                            funcionario = model.Funcionario,
                            FechaTramite = thisDay
                        };

                        model.PagoExtraordinario = pagoExtraordinario;

                        var resp1Aux = servicioPagoFeriado.AgregarPagoExtraordinario(model.Funcionario, model.PagoExtraordinario);
                        if (resp1Aux.GetType() != typeof(CErrorDTO))
                        {
                            CRespuestaDTO resp1 = (CRespuestaDTO)resp1Aux;
                            decimal subtotalPeriodo = Math.Round(decimal.Parse(model.SubtotalDias.Substring(1), CultureInfo.InvariantCulture) + decimal.Parse(model.porcentajeSalarioEscolar.Substring(1), CultureInfo.InvariantCulture), 2);
                            decimal diferenciaLiquida = Math.Round(subtotalPeriodo - decimal.Parse(Total_deduccion_obreroTabla.Substring(1), CultureInfo.InvariantCulture), 2);
                            if (model.Observaciones == null)
                            {
                                model.Observaciones = "";
                            }
                            if (model.NombramientoSeleccionado != 0)
                            {
                                model.Observaciones = "Nombramiento: " + model.NombramientoSeleccionado + " \n" + model.Observaciones;
                            }
                            //Agregar Pago feriado
                            model.PagoExtraordinario.IdEntidad = Convert.ToInt32(resp1.Contenido);

                            CPagoFeriadoDTO pagoFeriado = new CPagoFeriadoDTO
                            {
                                MontoSubtotalDia = decimal.Parse(model.SubtotalDias.Substring(1), CultureInfo.InvariantCulture),
                                MontoSalarioEscolar = decimal.Parse(model.porcentajeSalarioEscolar.Substring(1), CultureInfo.InvariantCulture),
                                MontoDeduccionObrero = decimal.Parse(Total_deduccion_obreroTabla.Substring(1), CultureInfo.InvariantCulture),
                                MontoDeduccionPatronal = decimal.Parse(total_deduccion_patronalTabla.Substring(1), CultureInfo.InvariantCulture),
                                MontoDiferenciaLiquida = diferenciaLiquida,
                                MontoAguinaldoProporcional = Math.Round(subtotalPeriodo / 12, 2),
                                MontoDeTotal = Math.Round((subtotalPeriodo / 12) + diferenciaLiquida, 2),
                                ObsevacionTramite = model.Observaciones,
                                MontoSalaroBruto = model.DesgloseSalarial.MontoSalarioBruto
                            };

                            CEstadoTramiteDTO estadoTramite = new CEstadoTramiteDTO
                            {
                                IdEntidad = 1
                            };

                            model.EstadoTramite = estadoTramite;
                            model.PagoFeriados = pagoFeriado;
                            var resp2Aux = servicioPagoFeriado.AgregarPagoFeriado(model.PagoExtraordinario, model.PagoFeriados,
                                                                             model.EstadoTramite, model.Funcionario);

                            if (resp2Aux.GetType() != typeof(CErrorDTO))
                            {
                                CRespuestaDTO resp2 = (CRespuestaDTO)resp2Aux;
                                model.PagoFeriados.IdEntidad = Convert.ToInt32(resp2.Contenido);

                                //Agregar deducciones
                                var CatalogoDeduccionesObrero = servicioPagoFeriado.ListarDeduccionesTipo(1);
                                List<CCatalogoDeduccionDTO> deduccionesObrero = new List<CCatalogoDeduccionDTO>();

                                var aux2 = CatalogoDeduccionesObrero.ToArray();
                                foreach (var deduccion in CatalogoDeduccionesObrero)
                                {
                                    deduccionesObrero.Add((CCatalogoDeduccionDTO)deduccion.ElementAt(0));
                                }

                                model.DeduccionesObrero = deduccionesObrero;
                                var CatalogoDeduccionesPatronal = servicioPagoFeriado.ListarDeduccionesTipo(2);
                                List<CCatalogoDeduccionDTO> deduccionesPatronales = new List<CCatalogoDeduccionDTO>();

                                var aux3 = CatalogoDeduccionesPatronal.ToArray();
                                foreach (var deduccionP in CatalogoDeduccionesPatronal)
                                {
                                    deduccionesPatronales.Add((CCatalogoDeduccionDTO)deduccionP.ElementAt(0));

                                }
                                model.DeduccionesPatronal = deduccionesPatronales;

                                List<CDeduccionEfectuadaDTO> deduccionObreroEfectuar = new List<CDeduccionEfectuadaDTO>();
                                for (int i = 0; i < deduccion_obreroTabla.Count(); i++)
                                {
                                    CDeduccionEfectuadaDTO auxDeduccionObrero = new CDeduccionEfectuadaDTO
                                    {
                                        PagoFeriado = model.PagoFeriados,
                                        MontoDeduccion = decimal.Parse(deduccion_obreroTabla[i].Substring(1), CultureInfo.InvariantCulture),
                                        PorcentajeEfectuado = model.DeduccionesObrero.ElementAt(i).PorcentajeDeduccion
                                    };
                                    deduccionObreroEfectuar.Add(auxDeduccionObrero);
                                }

                                List<CDeduccionEfectuadaDTO> deduccionPatronalEfectuar = new List<CDeduccionEfectuadaDTO>();
                                for (int i = 0; i < deduccion_patronalTabla.Count(); i++)
                                {
                                    CDeduccionEfectuadaDTO auxDeduccionPatronal = new CDeduccionEfectuadaDTO
                                    {
                                        PagoFeriado = model.PagoFeriados,
                                        MontoDeduccion = decimal.Parse(deduccion_patronalTabla[i].Substring(1), CultureInfo.InvariantCulture),
                                        PorcentajeEfectuado = model.DeduccionesPatronal.ElementAt(i).PorcentajeDeduccion
                                    };
                                    deduccionPatronalEfectuar.Add(auxDeduccionPatronal);
                                }
                                var resp3Aux = servicioPagoFeriado.AgregarDeduccion(model.Funcionario, deduccionObreroEfectuar.ToArray(),
                                     model.DeduccionesObrero.ToArray(), model.PagoFeriados, model.PagoExtraordinario);
                                var resp4Aux = servicioPagoFeriado.AgregarDeduccion(model.Funcionario, deduccionPatronalEfectuar.ToArray(),
                                                 model.DeduccionesPatronal.ToArray(), model.PagoFeriados, model.PagoExtraordinario);
                                if (resp3Aux.GetType() != typeof(CErrorDTO) && resp4Aux.GetType() != typeof(CErrorDTO))
                                {
                                    //Agregar días pagados
                                    CRespuestaDTO resp3 = (CRespuestaDTO)resp3Aux;
                                    CRespuestaDTO resp4 = (CRespuestaDTO)resp4Aux;
                                    List<CCatalogoDiaDTO> catalogoDiasSeleccionados = new List<CCatalogoDiaDTO>();
                                    List<CDiaPagadoDTO> diasPorPagar = new List<CDiaPagadoDTO>();
                                    for (int i = 0; i < id_dia.Count(); i++)
                                    {
                                        CCatalogoDiaDTO auxCatalogoDia = new CCatalogoDiaDTO
                                        {
                                            IdEntidad = id_dia[i]
                                        };
                                        catalogoDiasSeleccionados.Add(auxCatalogoDia);
                                        decimal salarioHora = Math.Round(model.DesgloseSalarial.MontoSalarioBruto / 240, 2);

                                        CDiaPagadoDTO auxDiaPagado = new CDiaPagadoDTO
                                        {

                                            Anio = anioTabla[i],
                                            MontoSalarioHora = salarioHora,
                                            CantidadHoras = horaTabla[i],
                                            MontoTotal = decimal.Parse(salarioTabla[i].Substring(1), CultureInfo.InvariantCulture)
                                        };
                                        diasPorPagar.Add(auxDiaPagado);
                                    }
                                    var resp5Aux = servicioPagoFeriado.AgregarDiaPagado(model.Funcionario, diasPorPagar.ToArray(), catalogoDiasSeleccionados.ToArray(),
                                        model.PagoFeriados, model.PagoExtraordinario);
                                    if (resp5Aux.GetType() == typeof(CErrorDTO))
                                    {

                                        CPagoFeriadoDTO aux = model.PagoFeriados;

                                        servicioPagoFeriado.EliminarDeduccionEfectuada(aux);
                                        servicioPagoFeriado.EliminarDiaPagado(aux);
                                        servicioPagoFeriado.EliminarPagoFeriado(aux);
                                        servicioPagoFeriado.EliminarPagoExtraordinario(model.PagoExtraordinario);
                                        ModelState.AddModelError("Agregar", ((CErrorDTO)resp5Aux).MensajeError + "");
                                        throw new Exception();

                                    }
                                    else
                                    {
                                        CRemuneracionEfectuadaPFDTO auxRem = new CRemuneracionEfectuadaPFDTO
                                        {
                                            PorcentajeEfectuado = model.SalEscolar.PorcentajeRemuneracion
                                        };

                                        model.SalEscolarEfectuado = auxRem;
                                        var resp6Aux = servicioPagoFeriado.AgregarBeneficio(model.Funcionario, model.SalEscolarEfectuado, model.PagoFeriados, model.PagoExtraordinario);
                                        if (resp6Aux.GetType() == typeof(CErrorDTO))
                                        {
                                            CPagoFeriadoDTO aux = model.PagoFeriados;

                                            servicioPagoFeriado.EliminarDeduccionEfectuada(aux);
                                            servicioPagoFeriado.EliminarDiaPagado(aux);
                                            servicioPagoFeriado.EliminarPagoFeriado(aux);
                                            servicioPagoFeriado.EliminarPagoExtraordinario(model.PagoExtraordinario);

                                            ModelState.AddModelError("Agregar", ((CErrorDTO)resp6Aux).MensajeError + "");
                                            throw new Exception();
                                        }
                                        else
                                        {
                                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT),
                                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)resp2Aux).Contenido),
                                                        CAccesoWeb.ListarEntidades(typeof(CPagoFeriadoDTO).Name));
                                            return JavaScript("window.location = '/PagoFeriado/Details?codigo=" + model.PagoFeriados.IdEntidad + "&accion=guardar" + "'");
                                        }
                                    }
                                }
                                else
                                {
                                    CPagoFeriadoDTO aux = model.PagoFeriados;


                                    servicioPagoFeriado.EliminarDeduccionEfectuada(aux);
                                    servicioPagoFeriado.EliminarPagoFeriado(aux);
                                    servicioPagoFeriado.EliminarPagoExtraordinario(model.PagoExtraordinario);
                                    ModelState.AddModelError("Agregar", ((CErrorDTO)resp3Aux).MensajeError + "");
                                    ModelState.AddModelError("Agregar", ((CErrorDTO)resp4Aux).MensajeError + "");
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                servicioPagoFeriado.EliminarPagoFeriado(model.PagoFeriados);
                                servicioPagoFeriado.EliminarPagoExtraordinario(model.PagoExtraordinario);

                                ModelState.AddModelError("Agregar", ((CErrorDTO)resp2Aux).MensajeError + "");
                                throw new Exception();
                            }
                        }
                        else
                        {
                            servicioPagoFeriado.EliminarPagoExtraordinario(model.PagoExtraordinario);
                            ModelState.AddModelError("Agregar", ((CErrorDTO)resp1Aux).MensajeError + "");
                            throw new Exception();
                        }

                    }
                    else
                    {

                        ModelState.AddModelError("Agregar", "No agregó días feriados o de asueto");
                        throw new Exception("No agregó días feriados o de asueto");
                    }

                }

            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorPagoFeriado");
                }
                else
                {
                    var codigoNom = servicioPagoFeriado.BuscarPuestoPF(model.Funcionario.Cedula).Select(
                                     P => new SelectListItem
                                     {
                                         Value = ((CPuestoDTO)P).IdEntidad.ToString(),
                                         Text = ((CPuestoDTO)P).CodPuesto.ToString()
                                     }
                                     );

                    model.TipoNombramiento = new SelectList(codigoNom, "Value", "Text");

                    return PartialView("_ErrorPagoFeriado");
                }
            }
        }


        //-------Registrar asueto------------------------------------------------------

        //
        // GET: /PagoFeriado/DetailsAsueto/
        /// <summary>
        /// Carga la vista de detalles para cuando se ingrese un nuevo día de asueto
        /// </summary>
        /// <example>
        /// /PagoFeriado/DetailsAsueto/
        /// </example>
        /// <returns>Retorna la vista de detalles para un asueto</returns>
        public ActionResult DetailsAsueto(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_PagoFeriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null)
                {
                    FormularioAsuetoVM model = new FormularioAsuetoVM();

                    var datos = servicioPagoFeriado.ObtenerCatalogoDia(codigo);

                    if (datos.Count() > 0)
                    {
                        model.Dia = (CCatalogoDiaDTO)datos.ElementAt(0);

                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }
        }

        //
        // GET: /PagoFeriado/CreateAsueto
        /// <summary>
        /// Prepara la interfaz para almacenar un día de asueto
        /// </summary>
        /// <example>
        /// /PagoFeriado/CreateAsueto/
        /// </example>
        /// <returns>Retorna la vista de create</returns>
        public ActionResult CreateAsueto()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null)
                {
                    FormularioAsuetoVM model = new FormularioAsuetoVM();

                    var datos = servicioPagoFeriado.ListarCantones();
                    var codigoNom = servicioPagoFeriado.ListarCantones().Select(
                   P => new SelectListItem
                   {
                       Value = ((CCantonDTO)P.ElementAt(0)).IdEntidad.ToString(),
                       Text = ((CCantonDTO)P.ElementAt(0)).NomCanton.ToString()
                   }
                 );

                    model.Canton = new SelectList(codigoNom, "Value", "Text");
                    var meses = new List<string>
                                     {
                                         "Enero", "Febrero","Marzo", "Abril", "Mayo","Junio","Julio","Agosto","Setiembre","Octubre","Noviembre","Diciembre"
                                     };

                    var datoMes = meses.Select(P => new SelectListItem
                    {
                        Value = P.ToString(),
                        Text = P.ToString()
                    });
                    model.Mes = new SelectList(datoMes, "Value", "Text");


                    var dias1 = new List<string>
                                     {
                                         "01","02", "03","04","05","06", "07","08", "09", "10","11","12","13","14","15","16","17",
                                         "18","19","20","21","22","23","24","25","26","27","28","29","30","31"
                                     };
                    var datoDias1 = dias1.Select(P => new SelectListItem
                    {
                        Value = P.ToString(),
                        Text = P.ToString()
                    });
                    model.Dias1 = new SelectList(datoDias1, "Value", "Text");

                    var dias2 = new List<string>
                                     {
                                         "01","02", "03","04","05","06", "07","08", "09", "10", "11",
                                         "12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28"
                                     };

                    var datoDias2 = dias2.Select(P => new SelectListItem
                    {
                        Value = P.ToString(),
                        Text = P.ToString()
                    });
                    model.Dias2 = new SelectList(datoDias2, "Value", "Text");

                    var dias3 = new List<string>
                                     {
                                         "01", "02", "03",  "04", "05", "06","07", "08","09", "10","11",
                                         "12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30"
                                     };
                    var datoDias3 = dias3.Select(P => new SelectListItem
                    {
                        Value = P.ToString(),
                        Text = P.ToString()
                    });
                    model.Dias3 = new SelectList(datoDias3, "Value", "Text");

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }
        }


        //
        // POST: /PagoFeriado/CreateAsueto
        /// <summary>
        /// Realiza la búsqueda del funcionario y almacena el trámite de pago
        /// </summary>
        /// <example>
        /// /PagoFeriado/CreateAsueto/
        /// </example>
        /// <param name="model">Modelo para el formulario de asueto</param>
        /// <returns>Retorna la vista de detalles</returns>
        [HttpPost]
        public ActionResult CreateAsueto(FormularioAsuetoVM model)
        {
            try
            {

                if (ModelState.IsValid == true)
                {
                    if ((model.Dias1Seleccionado != null || model.Dias2Seleccionado != null || model.Dias3Seleccionado != null) && model.MesSeleccionado != null)
                    {
                        if (model.CantonSeleccionado != null)
                        {
                            if (model.Detalle != null)
                            {
                                if (model.Detalle.ToString().Length > 0)
                                {
                                    //Agregar Asueto
                                    string auxDia = "";
                                    if (model.Dias1Seleccionado != null)
                                    {

                                        auxDia = model.Dias1Seleccionado;
                                    }
                                    else if (model.Dias2Seleccionado != null)
                                    {
                                        auxDia = model.Dias2Seleccionado;
                                    }
                                    if (model.Dias3Seleccionado != null)
                                    {
                                        auxDia = model.Dias3Seleccionado;
                                    }

                                    string auxMes = this.numeroMes(model.MesSeleccionado);
                                    CTipoDiaDTO tipoDia = new CTipoDiaDTO
                                    {
                                        IdEntidad = 2
                                    };

                                    CCatalogoDiaDTO dia = new CCatalogoDiaDTO
                                    {
                                        DescripcionDia = model.Detalle,
                                        Dia = auxDia,
                                        Mes = auxMes,
                                        TipoDia = tipoDia
                                    };

                                    model.Dia = dia;
                                    model.TipoDia = tipoDia;

                                    var respuesta = servicioPagoFeriado.AgregarAsueto(model.Dia, model.TipoDia);

                                    if (respuesta.GetType() != typeof(CErrorDTO))
                                    {

                                        if (((CRespuestaDTO)respuesta).Codigo > 0)
                                        {
                                            CRespuestaDTO repAux = (CRespuestaDTO)respuesta;
                                            //Agregar ubicacion 
                                            model.Dia.IdEntidad = Convert.ToInt32(repAux.Contenido);
                                            var canton = servicioPagoFeriado.ObtenerCanton(Convert.ToInt32(model.CantonSeleccionado));
                                            CCantonDTO cantonAux = (CCantonDTO)canton.ElementAt(0);

                                            CUbicacionAsuetoDTO ubicacionAux = new CUbicacionAsuetoDTO
                                            {
                                                Dia = dia,
                                                Canton = cantonAux
                                            };
                                            model.UbicacionAsueto = ubicacionAux;

                                            respuesta = servicioPagoFeriado.AgregarUbicacionAsueto(model.UbicacionAsueto.Canton, model.Dia, model.UbicacionAsueto);

                                            if (respuesta.GetType() != typeof(CErrorDTO))
                                            {
                                                if (((CRespuestaDTO)respuesta).Codigo > 0)
                                                {
                                                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT),
                                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                                        CAccesoWeb.ListarEntidades(typeof(CPagoFeriadoDTO).Name));
                                                    return JavaScript("window.location = '/PagoFeriado/DetailsAsueto?codigo=" + model.Dia.IdEntidad + "&accion=guardar" + "'");

                                                }
                                                else
                                                {

                                                    ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                                    throw new Exception(respuesta.Mensaje);
                                                }
                                            }
                                            else
                                            {

                                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                                throw new Exception(respuesta.Mensaje);
                                            }
                                        }
                                        else
                                        {

                                            ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                            throw new Exception(respuesta.Mensaje);
                                        }
                                    }
                                    else
                                    {

                                        ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                        throw new Exception(respuesta.Mensaje);
                                    }
                                }

                                else
                                {
                                    ModelState.AddModelError("Agregar", "Debe ingresar una descripción para el día de asueto");
                                    throw new Exception("Debe ingresar una descripción para el día de asueto");
                                }
                            }

                            else
                            {
                                ModelState.AddModelError("Agregar", "Debe ingresar una descripción para el día de asueto");
                                throw new Exception("Debe ingresar una descripción para el día de asueto");
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", "Debe seleccionar el cantón del asueto");
                            throw new Exception("Debe seleccionar el cantón del asueto");
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("Agregar", "Debe seleccionar el mes y el día del asueto");
                        throw new Exception("Debe seleccionar el mes y el día del asueto");
                    }
                }
                else
                {


                    throw new Exception("Formulario");
                }

            }
            catch (Exception error)
            {
                var codigoNom = servicioPagoFeriado.ListarCantones().Select(
                  P => new SelectListItem
                  {
                      Value = ((CCantonDTO)P.ElementAt(0)).IdEntidad.ToString(),
                      Text = ((CCantonDTO)P.ElementAt(0)).NomCanton.ToString()
                  }
                );

                model.Canton = new SelectList(codigoNom, "Value", "Text");
                var meses = new List<string>
                                     {
                                         "Enero", "Febrero", "Marzo", "Abril","Mayo", "Junio", "Julio",  "Agosto", "Setiembre",  "Octubre", "Noviembre",  "Diciembre"
                                     };

                var datoMes = meses.Select(P => new SelectListItem
                {
                    Value = P.ToString(),
                    Text = P.ToString()
                });
                model.Mes = new SelectList(datoMes, "Value", "Text");


                var dias1 = new List<string>
                                     {
                                         "01", "02","03", "04","05","06","07","08", "09", "10","11",
                                         "12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31"
                                     };
                var datoDias1 = dias1.Select(P => new SelectListItem
                {
                    Value = P.ToString(),
                    Text = P.ToString()
                });
                model.Dias1 = new SelectList(datoDias1, "Value", "Text");

                var dias2 = new List<string>
                                     {
                                         "01", "02", "03",  "04", "05", "06", "07",  "08","09",  "10", "11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28"
                                     };

                var datoDias2 = dias2.Select(P => new SelectListItem
                {
                    Value = P.ToString(),
                    Text = P.ToString()
                });
                model.Dias2 = new SelectList(datoDias2, "Value", "Text");

                var dias3 = new List<string>
                                     {
                                         "01",  "02", "03", "04", "05", "06", "07","08", "09", "10",  "11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30"
                                     };
                var datoDias3 = dias3.Select(P => new SelectListItem
                {
                    Value = P.ToString(),
                    Text = P.ToString()
                });
                model.Dias3 = new SelectList(datoDias3, "Value", "Text");
                return PartialView("_ErrorPagoFeriado");


            }

        }

        //------Realizar búsquedas------------------------------------

        //
        // GET: /PagoFeriado/Search
        /// <summary>
        /// Prepara la interfaz de búsquedas
        /// </summary>
        /// <example>
        /// /PagoFeriado/Search/
        /// </example>
        /// <returns>Retorna la vista de search</returns>
        public ActionResult Search()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null ||
                    Session["Permiso_Feriado_Consulta"] != null)
                {
                    BusquedaPagoFeriadoVM model = new BusquedaPagoFeriadoVM();

                    List<string> diasFeriados = new List<string>();
                    var auxiliar = servicioPagoFeriado.ListarDiasPorTipo(1).Select(
                                    P => new SelectListItem
                                    {
                                        Value = ((CCatalogoDiaDTO)P.ElementAt(0)).IdEntidad.ToString(),
                                        Text = ((CCatalogoDiaDTO)P.ElementAt(0)).DescripcionDia.ToString()
                                    }
                                    );

                    model.DiaFeriado = new SelectList(auxiliar, "Value", "Text");


                    var auxiliarA = servicioPagoFeriado.ListarDiasPorTipo(2).Select(
                                  P => new SelectListItem
                                  {
                                      Value = ((CCatalogoDiaDTO)P.ElementAt(0)).IdEntidad.ToString(),
                                      Text = ((CCatalogoDiaDTO)P.ElementAt(0)).DescripcionDia.ToString()
                                  }
                                  );

                    model.DiaAsueto = new SelectList(auxiliarA, "Value", "Text");
                    var estados = new List<string>
                                     {
                                         "Activo",
                                         "Anulado"
                                     };

                    var datoEstado = estados.Select(P => new SelectListItem
                    {
                        Value = P.ToString(),
                        Text = P.ToString()
                    });

                    model.Estados = new SelectList(datoEstado, "Value", "Text");

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }
        }

        //
        // POST: /PagoFeriado/Search
        /// <summary>
        /// Realiza la búsqueda de trámites de pago
        /// </summary
        /// <example>
        /// /PagoFeriado/Search/
        /// </example>
        /// <param name="model">Modelo para el formulario de búsquedas</param>
        /// <returns>Retorna la sub vista de resultados</returns>
        [HttpPost]
        public ActionResult Search(BusquedaPagoFeriadoVM model)
        {
            try
            {
                if (model.Funcionario.Cedula != null || model.Consecutivo > 0 ||
                    (model.FechaTramiteDesde.Year > 1 && model.FechaTramitenHasta.Year > 1) ||
                    (model.EstadoSeleccionado != null) || (model.DiaSeleccionado != null) || (model.DiaAsuetoSeleccionado != null))
                {
                    List<DateTime> fechas = new List<DateTime>();

                    if (model.FechaTramiteDesde.Year > 1 && model.FechaTramitenHasta.Year > 1)
                    {
                        fechas.Add(model.FechaTramiteDesde);
                        fechas.Add(model.FechaTramitenHasta);
                    }
                    CPagoFeriadoDTO aux = new CPagoFeriadoDTO
                    {
                        IdEntidad = model.Consecutivo
                    };

                    model.PagoFeriado = aux;
                    List<string> diasBusqueda = new List<string>();

                    if (model.DiaSeleccionado != null)
                    {
                        diasBusqueda.Add(model.DiaSeleccionado);
                    }
                    if (model.EstadoSeleccionado != null)
                    {
                        diasBusqueda.Add(model.DiaAsuetoSeleccionado);
                    }
                    model.Funcionario.Sexo = GeneroEnum.Indefinido;
                    CEstadoTramiteDTO estadoDTO = new CEstadoTramiteDTO
                    {
                        IdEntidad = -1,
                        DescripcionEstado = ""
                    };
                    if (model.EstadoSeleccionado != null)
                    {
                        if (model.EstadoSeleccionado.Equals("Activo"))
                        {
                            estadoDTO.IdEntidad = 1;
                        }
                        else
                        {

                            estadoDTO.IdEntidad = 2;
                        }
                    }
                    List<string> ai = new List<string>();
                    if (model.DiaAsuetoSeleccionado != null)
                    {
                        ai.Add(model.DiaAsuetoSeleccionado);
                    }
                    if (model.DiaSeleccionado != null)
                    {
                        ai.Add(model.DiaSeleccionado);
                    }

                    var datos = servicioPagoFeriado.BuscarPagosFeriado(model.Funcionario, model.PagoFeriado,
                                                                     fechas.ToArray(), estadoDTO, ai.ToArray());

                    if (datos.Count() != 0)
                    {
                        if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                            throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        }
                        else
                        {
                            model.EstadosTramite = new List<CEstadoTramiteDTO>();
                            model.PagosFeriado = new List<CPagoFeriadoDTO>();
                            model.PagosExtraordinarios = new List<CPagoExtraordinarioDTO>();
                            model.Funcionarios = new List<CFuncionarioDTO>();

                            foreach (var dato in datos)
                            {
                                model.PagosFeriado.Add((CPagoFeriadoDTO)dato.ElementAt(0));
                                model.PagosExtraordinarios.Add((CPagoExtraordinarioDTO)dato.ElementAt(1));
                                model.Funcionarios.Add((CFuncionarioDTO)dato.ElementAt(2));
                                model.EstadosTramite.Add((CEstadoTramiteDTO)dato.ElementAt(3));
                            }

                            return PartialView("_SearchResults", model);
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaTramiteDesde.Year > 1 || model.FechaTramitenHasta.Year > 1)
                    {
                        if (!(model.FechaTramiteDesde.Year > 1 && model.FechaTramitenHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de trámite, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }

                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorPagoFeriado");
                }
                else
                {
                    return PartialView("_ErrorPagoFeriado");
                }
            }
        }


        //-------Actualizar Salario escolar

        //
        // GET: /PagoFeriado/CreateSalarioEscolar
        /// <summary>
        /// Carga la vista para modificar el salario escolar
        /// </summary>
        /// <example>
        /// /PagoFeriado/CreateSalarioEscolar/
        /// </example>
        /// <returns>Retorna la vista de create</returns>
        public ActionResult CreateSalarioEscolar()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null)
                {
                    FormularioSalarioEscolarVM model = new FormularioSalarioEscolarVM();
                    var salEscolar = servicioPagoFeriado.ObtenerSalarioEscolar();
                    model.CatalogoRemuneracion = (CCatalogoRemuneracionDTO)salEscolar.ElementAt(0);

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }
        }


        //
        // POST: /PagoFeriado/CreateSalarioEscolar
        /// <summary>
        /// Actualiza el porcentaje de salario escolar
        /// </summary>
        /// <example>
        /// /PagoFeriado/CreateSalarioEscolar/
        /// </example>
        /// <param name="model">Modelo para el formulario para cambiar el salario escolar</param>
        /// <returns>Retorna la vista de detalles</returns>
        [HttpPost]
        public ActionResult CreateSalarioEscolar(FormularioSalarioEscolarVM model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    if (model.CatalogoRemuneracion.PorcentajeRemuneracion > 0)
                    {

                        if (model.Descripcion != null)
                        {

                            var respuesta = servicioPagoFeriado.ActualizarSalarioEscolar(model.CatalogoRemuneracion);

                            if (respuesta.GetType() != typeof(CErrorDTO))
                            {
                                if (((CCatalogoRemuneracionDTO)respuesta).IdEntidad > 0)
                                {
                                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT),
                                                        Convert.ToInt32(EAccionesBitacora.Editar), Convert.ToInt32(((CCatalogoRemuneracionDTO)respuesta).IdEntidad),
                                                        CAccesoWeb.ListarEntidades(typeof(CPagoFeriadoDTO).Name));
                                    return JavaScript("window.location = '/PagoFeriado/DetailsSalarioEscolar?codigo=1&accion=guardar" + "'");

                                }
                                else
                                {
                                    ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                    throw new Exception(respuesta.Mensaje);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Agregar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", "Debe ingresar el número de resolución");
                            throw new Exception("Debe ingresar el número de resolución");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Agregar", "Debe ingresar el nuevo porcentaje");
                        throw new Exception("Debe ingresar el nuevo porcentaje");
                    }
                }
                else
                {
                    throw new Exception("");
                }

            }

            catch (Exception error)
            {
                return PartialView("_ErrorPagoFeriado");
            }
        }


        //
        // GET: /PagoFeriado/DetailsSalarioEscolar/
        /// <summary>
        /// Detalla el nuevo porcentaje de salario escolar
        /// </summary>
        /// <example>
        /// /PagoFeriado/DetailsSalarioEscolar/
        /// </example>
        /// <param name="codigo">Codigo de la remuneración efectuada</param>
        /// <param name="accion">Acción del details</param>
        /// <returns>Retorna la vista de detalles</returns>
        public ActionResult DetailsSalarioEscolar(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_Feriado"]) ||
                    Session["Permiso_Feriado_Operativo"] != null)
                {
                    FormularioSalarioEscolarVM model = new FormularioSalarioEscolarVM();

                    var datos = servicioPagoFeriado.ObtenerSalarioEscolar();

                    if (datos.Count() > 0)
                    {
                        model.CatalogoRemuneracion = (CCatalogoRemuneracionDTO)datos.ElementAt(0);
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
                }
            }
        }


        //-------Buscar días de asueto 

        //
        // GET: /PagoFeriado/Create/Asueto_Index/
        /// <summary>
        /// Realiza una búsqueda de asuetos
        /// </summary>
        /// <example>
        /// /PagoFeriado/Create/Asueto_Index/
        /// </example>
        /// <param name="model">Modelo para el formulario de pago de feriado</param>
        /// <param name="NombreSearch">Filtro de busqueda</param>
        /// <param name="CantonSeleccionado">Filtro de busqueda</param>
        /// <param name="page">Página de resultados</param>
        /// <returns>Retorna la vista de resultados</returns>
        public PartialViewResult Asueto_Index(FormularioPagoFeriadoVM modelo, string NombreSearch, string CantonSeleccionado, int? page)
        {
            int paginaActual = page.HasValue ? page.Value : 1;

            if (String.IsNullOrEmpty(CantonSeleccionado) && String.IsNullOrEmpty(NombreSearch))
            {
                return PartialView();
            }
            else
            {
                if (modelo == null)
                {
                    modelo = new FormularioPagoFeriadoVM();
                    if (CantonSeleccionado != null)
                    {
                        modelo.CantonSeleccionado = CantonSeleccionado;
                    }
                    else
                    {
                        modelo.CantonSeleccionado = "0";
                    }
                    if (NombreSearch != null)
                    {
                        modelo.NombreSearch = NombreSearch;
                    }
                    else
                    {
                        modelo.NombreSearch = "";
                    }
                }

                int codigoClase = String.IsNullOrEmpty(modelo.CantonSeleccionado) ? 0 : Convert.ToInt32(modelo.CantonSeleccionado);
                var dias = new List<CCatalogoDiaDTO>();
                var diasCatalogoAs = servicioPagoFeriado.ListarAsuetosPorUbicacion(modelo.NombreSearch, modelo.CantonSeleccionado);
                if (diasCatalogoAs != null)
                {
                    foreach (var catalogo in diasCatalogoAs)
                    {
                        dias.Add((CCatalogoDiaDTO)catalogo.ElementAt(0));
                    }
                    modelo.CatalogoDiaAsueto = dias;
                }
                else
                {
                    modelo.Error = (CErrorDTO)diasCatalogoAs.ElementAt(0).FirstOrDefault();
                }
                modelo.TotalAsuetos = diasCatalogoAs.Count();
                modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalAsuetos / 10);
                modelo.PaginaActual = paginaActual;
                if ((((paginaActual - 1) * 10) + 10) > modelo.TotalAsuetos)
                {
                    modelo.CatalogoDiaAsueto = dias.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalAsuetos) - (((paginaActual - 1) * 10))).ToList(); ;
                }
                else
                {
                    modelo.CatalogoDiaAsueto = dias.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                }
                return PartialView("DiaAsueto_Dialog", modelo);
            }
        }


        //-------Editar trámite de pago--------------------------
        //
        // GET: /PagoFeriado/Edit/
        /// <summary>
        /// Carga la interfaz de anular un trámite de pago
        /// </summary>
        /// <example>
        /// /PagoFeriado/Edit/
        /// </example>
        /// <returns>Retorna la vista de edit</returns>
        public ActionResult Edit(int codigo, string accion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
            }
            else
            {
                FormularioPagoFeriadoVM model = new FormularioPagoFeriadoVM();

                var datos = servicioPagoFeriado.ObtenerPagoFeriado(codigo);
                var datos2 = servicioPagoFeriado.RetornarDiasPorTramitePagado(codigo);
                if (datos.Count() > 0 && datos2.Count() > 0)
                {

                    model.PagoFeriados = (CPagoFeriadoDTO)datos.ElementAt(0);
                    model.PagoExtraordinario = (CPagoExtraordinarioDTO)datos.ElementAt(1);
                    model.Funcionario = (CFuncionarioDTO)datos.ElementAt(2);
                    model.EstadoTramite = (CEstadoTramiteDTO)datos.ElementAt(3);
                    model.CatalogoDiaAsueto = new List<CCatalogoDiaDTO>();
                    model.DiasPagados = new List<CDiaPagadoDTO>();
                    foreach (var dia in datos2)
                    {
                        model.DiasPagados.Add((CDiaPagadoDTO)dia.ElementAt(0));
                        model.CatalogoDiaAsueto.Add((CCatalogoDiaDTO)dia.ElementAt(3));

                    }
                }
                else
                {
                    model.Error = (CErrorDTO)datos.ElementAt(0);
                }

                return View(model);
            }
        }


        //
        // POST: /PagoFeriado/Edit/
        /// <summary>
        /// Anula un trámite de pago
        /// </summary>
        /// <example>
        /// /PagoFeriado/Edit/
        /// </example>
        /// <returns>Retorna la vista de detalles</returns>
        [HttpPost]
        public ActionResult Edit(int codigo, FormularioPagoFeriadoVM model)
        {
            try
            {
                if (model.Observaciones != null)
                {
                    model.PagoFeriados = new CPagoFeriadoDTO();
                    model.PagoFeriados.IdEntidad = codigo;
                    model.PagoFeriados.ObsevacionTramite = "Anulado por: " + model.Observaciones;
                    var respuesta = servicioPagoFeriado.AnularPagoFeriado(model.PagoFeriados);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        this.SendEmail(codigo);
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT),
                                                        Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                                        CAccesoWeb.ListarEntidades(typeof(CPagoFeriadoDTO).Name));
                        return RedirectToAction("Details", new { codigo = codigo, accion = "edit" });

                    }
                    else
                    {
                        ModelState.AddModelError("modificar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe indicar una justificación para anular este trámite de pago");
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                var datos2 = servicioPagoFeriado.RetornarDiasPorTramitePagado(codigo);
                if (datos2.Count() > 0)
                {

                    model.CatalogoDiaAsueto = new List<CCatalogoDiaDTO>();
                    model.DiasPagados = new List<CDiaPagadoDTO>();

                    foreach (var dia in datos2)
                    {
                        model.DiasPagados.Add((CDiaPagadoDTO)dia.ElementAt(0));
                        model.CatalogoDiaAsueto.Add((CCatalogoDiaDTO)dia.ElementAt(3));

                    }
                }
                else
                {
                    model.Error = (CErrorDTO)datos2.ElementAt(0).ElementAt(0);
                }

                return View(model);
            }
        }


        /// <summary>
        /// Define el número de mes según el nombre del mismo
        /// </summary>
        /// <returns>Retorna el número de mes</returns>
        private string numeroMes(string mes)
        {
            switch (mes)
            {
                case "Enero":
                    return "01";
                case "Febrero":
                    return "02";
                case "Marzo":
                    return "03";
                case "Abril":
                    return "04";
                case "Mayo":
                    return "05";
                case "Junio":
                    return "06";
                case "Julio":
                    return "07";
                case "Agosto":
                    return "08";
                case "Setiembre":
                    return "09";
                case "Octubre":
                    return "10";
                case "Noviembre":
                    return "11";
                case "Diciembre":
                    return "12";
                default:
                    return " ";
            }

        }


        //-------Reportes

        /// <summary>
        /// Carga un reporte para un trámite de pago específico
        /// </summary>
        /// <returns>Retorna el reporte</returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetallePagoFeriado(FormularioPagoFeriadoVM model)
        {
            List<PagoFeriadoRptData> modelo = new List<PagoFeriadoRptData>();
            List<PagoFeriadoRptData> modelo2 = new List<PagoFeriadoRptData>();

            model.SalEscolarEfectuado = new CRemuneracionEfectuadaPFDTO();
            model.DiaPagadoAuxiliar = new CDiaPagadoDTO();
            model.CatalogoDiaAuxiliar = new CCatalogoDiaDTO();

            model.SalEscolarEfectuado = new CRemuneracionEfectuadaPFDTO();

            var salEscolarEfectuado = servicioPagoFeriado.ObtenerSalarioEscolarEfectuado(model.PagoFeriados.IdEntidad);
            model.SalEscolarEfectuado = (CRemuneracionEfectuadaPFDTO)salEscolarEfectuado;

            modelo.Add(PagoFeriadoRptData.GenerarDatosReporte(model, String.Empty, "DetalleSearch"));

            var datos = servicioPagoFeriado.RetornarDiasPorTramitePagado(model.PagoFeriados.IdEntidad);

            foreach (var dia in datos)
            {
                model.DiaPagadoAuxiliar = (CDiaPagadoDTO)dia.ElementAt(0);
                model.CatalogoDiaAuxiliar = (CCatalogoDiaDTO)dia.ElementAt(3);
                modelo2.Add(PagoFeriadoRptData.GenerarDatosReporte(model, String.Empty, "DetalleSearch"));
            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/PagoFeriado"), "DetallesPagoFeriadoRPT.rpt");

            return new CrystalReportPdfResult(reportPath, modelo, modelo2, null, null);
        }

        /// <summary>
        /// Carga un reporte para una lista de trámites de pago
        /// </summary>
        /// <returns>Retorna el reporte</returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleBusqueda(BusquedaPagoFeriadoVM model)
        {
            String filtro = "Filtrado por: ";
            List<string> filtros = new List<string>();
            List<PagoFeriadoRptData> modelo = new List<PagoFeriadoRptData>();
            List<PagoFeriadoRptData> modelo2 = new List<PagoFeriadoRptData>();

            if (model.Funcionario != null && !String.IsNullOrEmpty(model.Funcionario.Cedula))
            {
                filtros.Add("cédula");
            }
            if (!String.IsNullOrEmpty(model.Consecutivo + "") && model.Consecutivo > 0)
            {
                filtros.Add("consecutivo");
            }
            if (!String.IsNullOrEmpty(model.DiaSeleccionado))
            {
                filtros.Add("día feriado");
            }

            if (!String.IsNullOrEmpty(model.DiaAsuetoSeleccionado))
            {
                filtros.Add("día de asueto");
            }

            if (!String.IsNullOrEmpty(model.EstadoSeleccionado))
            {
                filtros.Add("estado del trámite");
            }

            if (model.FechaTramiteDesde.Year > 1)
            {
                filtros.Add("fecha del trámite");
            }
            for (int j = 0; j < filtros.Count(); j++)
            {
                if (j == 0)
                {
                    filtro = filtro + " " + filtros[j];
                }
                else if (j == (filtros.Count() - 1))
                {
                    filtro = filtro + " y " + filtros[j] + ".";
                }
                else if (j > 0)
                {
                    filtro = filtro + ", " + filtros[j];
                }

            }

            for (int i = 0; i < model.PagosFeriado.Count(); i++)
            {
                CPagoFeriadoDTO pagoFeriadoAux = (CPagoFeriadoDTO)servicioPagoFeriado.ObtenerPagoFeriado(model.PagosFeriado[i].IdEntidad).ElementAt(0);
                model.PagoFeriado = pagoFeriadoAux;
                var datos = servicioPagoFeriado.RetornarDiasPorTramitePagado(model.PagosFeriado[i].IdEntidad);
                model.Funcionario = model.Funcionarios[i];
                model.PagoExtraordinario = model.PagosExtraordinarios[i];
                model.EstadoTramite = model.EstadosTramite[i];
                model.SalEscolarEfectuado = new CRemuneracionEfectuadaPFDTO();
                var salEscolarEfectuado = servicioPagoFeriado.ObtenerSalarioEscolarEfectuado(model.PagoFeriado.IdEntidad);
                if (salEscolarEfectuado.GetType() != typeof(CErrorDTO))
                {
                    model.SalEscolarEfectuado = (CRemuneracionEfectuadaPFDTO)salEscolarEfectuado;
                }
                foreach (var dia in datos)
                {
                    model.DiaPagadoAuxiliar = (CDiaPagadoDTO)dia.ElementAt(0);
                    model.CatalogoDiaAuxiliar = (CCatalogoDiaDTO)dia.ElementAt(3);
                    modelo2.Add(PagoFeriadoRptData.GenerarDatosReporteBusqueda(model, String.Empty));
                }
                modelo.Add(PagoFeriadoRptData.GenerarDatosReporteBusqueda(model, filtro));
            }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/PagoFeriado"), "VerDetallesPagoFeriadoRPT.rpt");

            return new CrystalReportPdfResult(reportPath, modelo, modelo2, null, null);
        }

        /// <summary>
        /// Carga un reporte para un trámite de pago específico cuando este es almacenado en la BD
        /// </summary>
        /// <returns>Retorna el reporte</returns>
        [HttpPost]
        public CrystalReportPdfResult ReporteGuardarPagoFeriado(FormularioPagoFeriadoVM model)
        {
            List<PagoFeriadoRptData> modelo = new List<PagoFeriadoRptData>();
            List<PagoFeriadoRptData> modelo2 = new List<PagoFeriadoRptData>();
            List<PagoFeriadoRptData> modelo3 = new List<PagoFeriadoRptData>();
            List<PagoFeriadoRptData> modelo4 = new List<PagoFeriadoRptData>();
            List<PagoFeriadoRptData> modelo5 = new List<PagoFeriadoRptData>();
            model.DeduccionesEfectuadasObreraAuxiliar = new CDeduccionEfectuadaDTO();
            model.DeduccionesEfectuadasPatronalAuxiliar = new CDeduccionEfectuadaDTO();
            model.DeduccionesObreroAuxiliar = new CCatalogoDeduccionDTO();
            model.DeduccionesPatronalAuxiliar = new CCatalogoDeduccionDTO();
            model.DiaPagadoAuxiliar = new CDiaPagadoDTO();
            model.CatalogoDiaAuxiliar = new CCatalogoDiaDTO();

            model.SalEscolarEfectuado = new CRemuneracionEfectuadaPFDTO();
            var datos5 = servicioPagoFeriado.ObtenerSalarioEscolarEfectuado(model.PagoFeriados.IdEntidad);

            model.SalEscolarEfectuado = (CRemuneracionEfectuadaPFDTO)datos5;

            modelo.Add(PagoFeriadoRptData.GenerarDatosReporte(model, String.Empty, "DetalleGuardar"));

            var datos = servicioPagoFeriado.RetornarDiasPorTramitePagado(model.PagoFeriados.IdEntidad);

            foreach (var dia in datos)
            {
                model.DiaPagadoAuxiliar = (CDiaPagadoDTO)dia.ElementAt(0);
                model.CatalogoDiaAuxiliar = (CCatalogoDiaDTO)dia.ElementAt(3);
                modelo2.Add(PagoFeriadoRptData.GenerarDatosReporte(model, String.Empty, "DetalleGuardar"));
            }
            model.DiaPagadoAuxiliar = new CDiaPagadoDTO();
            model.CatalogoDiaAuxiliar = new CCatalogoDiaDTO();
            var datos3 = servicioPagoFeriado.ListarDeduccionesPagoTipo(1, model.PagoFeriados.IdEntidad);
            var datos4 = servicioPagoFeriado.ListarDeduccionesPagoTipo(2, model.PagoFeriados.IdEntidad);

            foreach (var deduccion in datos3)
            {
                model.DeduccionesEfectuadasObreraAuxiliar = ((CDeduccionEfectuadaDTO)deduccion.ElementAt(0));
                model.DeduccionesObreroAuxiliar = ((CCatalogoDeduccionDTO)deduccion.ElementAt(1));
                modelo3.Add(PagoFeriadoRptData.GenerarDatosReporte(model, String.Empty, "DetalleGuardar"));

            }
            foreach (var deduccionP in datos4)
            {
                model.DeduccionesEfectuadasObreraAuxiliar = ((CDeduccionEfectuadaDTO)deduccionP.ElementAt(0));
                model.DeduccionesObreroAuxiliar = ((CCatalogoDeduccionDTO)deduccionP.ElementAt(1));
                modelo4.Add(PagoFeriadoRptData.GenerarDatosReporte(model, String.Empty, "DetalleGuardar"));

            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/PagoFeriado"), "PagoFeriadoRPT.rpt");

            return new CrystalReportPdfResult(reportPath, modelo, modelo2, modelo3, modelo4);
        }

        public FormularioPagoFeriadoVM CargarDatosEmail(int codigo)
        {
            FormularioPagoFeriadoVM model = new FormularioPagoFeriadoVM();

            var datos = servicioPagoFeriado.ObtenerPagoFeriado(codigo);
            var datos2 = servicioPagoFeriado.RetornarDiasPorTramitePagado(codigo);
            if (datos.Count() > 0 && datos2.Count() > 0)
            {

                model.PagoFeriados = (CPagoFeriadoDTO)datos.ElementAt(0);
                model.PagoExtraordinario = (CPagoExtraordinarioDTO)datos.ElementAt(1);
                model.Funcionario = (CFuncionarioDTO)datos.ElementAt(2);
                model.EstadoTramite = (CEstadoTramiteDTO)datos.ElementAt(3);
                model.CatalogoDiaAsueto = new List<CCatalogoDiaDTO>();
                model.DiasPagados = new List<CDiaPagadoDTO>();
                model.SubtotalDiferencias = Math.Round(model.PagoFeriados.MontoSubtotalDia + model.PagoFeriados.MontoSalarioEscolar, 2);

                foreach (var dia in datos2)
                {
                    model.DiasPagados.Add((CDiaPagadoDTO)dia.ElementAt(0));
                    model.CatalogoDiaAsueto.Add((CCatalogoDiaDTO)dia.ElementAt(3));

                }
            }
            return model;
        }

        /// <summary>
        /// Envía un correo al encargado (a) de Integra si el usuario anuló un trámite de pago
        /// </summary>
        /// <returns>Nulo</returns>
        public void SendEmail(int codigo)
        {
            try
            {
                EmailWebHelper mensajero = new EmailWebHelper();
                FormularioPagoFeriadoVM model = new FormularioPagoFeriadoVM();
                model = CargarDatosEmail(codigo);
                EmailAnulacion(mensajero, model);

                mensajero.EnviarCorreo();

            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Dibuja el encabezado del correo electrónico y después llama los métodos para completar el correo electrónico
        /// </summary>
        /// <returns>El helper del email lleno</returns>
        private EmailWebHelper EmailAnulacion(EmailWebHelper salida, FormularioPagoFeriadoVM model)
        {
            salida.Asunto = "Anulación del trámite de pago";

            string hora = string.Format("{0:hh:mm:ss tt}", DateTime.Now);

            string[] tipoHora = hora.Split(' ');

            if (tipoHora[1] == "a.m.")
            {
                salida.EmailBody = "Buenos días, <br><br>";
            }
            else
            {
                salida.EmailBody = "Buenas tardes, <br><br>";
            }

            salida.EmailBody += "Se ha anulado el siguiente trámite de pago: <br><br>";

            salida = this.DatosPrincipalesEmail(salida, model);
            this.DibujarDiasPagadosEmail(salida, model);
            this.DibujarDeduccionesEmail(salida, model);
            this.DibujarTotalesEmail(salida, model);
            this.EmailPie(salida);
            return salida;
        }

        /// <summary>
        /// Dibuja los datos principales del funcionario y el trámite anulado
        /// </summary>
        /// <returns>El helper del email lleno</returns>
        private EmailWebHelper DatosPrincipalesEmail(EmailWebHelper salida, FormularioPagoFeriadoVM model)
        {
            salida.EmailBody += "<table>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td><b>Consecutivo</b></td>";
            salida.EmailBody += "<td>" + model.PagoFeriados.IdEntidad + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td><b>Fecha del trámite</b></td>";
            salida.EmailBody += "<td>" + model.PagoExtraordinario.FechaTramite + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td><b>Estado del trámite</b></td>";
            salida.EmailBody += "<td>" + model.EstadoTramite.DescripcionEstado + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td><b>Funcionario</b></td>";
            salida.EmailBody += "<td>" + model.Funcionario.Cedula + "-" + model.Funcionario.Nombre + " " + model.Funcionario.PrimerApellido + " " + model.Funcionario.SegundoApellido + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td><b>Salario bruto</b></td>";
            salida.EmailBody += "<td>" + "₡ " + model.PagoFeriados.MontoSalaroBruto.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td><b>Observaciones</b></td>";
            salida.EmailBody += "<td>" + model.PagoFeriados.ObsevacionTramite + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "</table>";

            return salida;
        }

        /// <summary>
        /// Dibuja los días pagados y subtotales del trámite anulado
        /// </summary>
        /// <returns>El helper del email lleno</returns>
        private EmailWebHelper DibujarDiasPagadosEmail(EmailWebHelper salida, FormularioPagoFeriadoVM model)
        {

            salida.EmailBody += "<br/>";
            salida.EmailBody += "<table style=' border: solid 1px #e8eef4;font-weight: bold;border-collapse: collapse;width:70%;'>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Descripción</th>";
            salida.EmailBody += "<th style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Cantidad de horas</th>";
            salida.EmailBody += "<th style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Año de día</th>";
            salida.EmailBody += "<th style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Salario por horas</th>";
            salida.EmailBody += "<th style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Monto</th>";
            salida.EmailBody += "</tr>";

            for (int i = 0; i < model.DiasPagados.Count(); i++)
            {
                salida.EmailBody += "<tr>";
                salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + model.CatalogoDiaAsueto[i].DescripcionDia + "</td>";
                salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + model.DiasPagados[i].CantidadHoras + "</td>";
                salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + model.DiasPagados[i].Anio + "</td>";
                salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡" + model.DiasPagados[i].MontoSalarioHora.ToString("#,#.00#;(#,#.00#)") + "</td>";
                salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡" + model.DiasPagados[i].MontoTotal.ToString("#,#.00#;(#,#.00#)") + "</td>";
                salida.EmailBody += "</tr>";
            }
            salida.EmailBody += "</table>";
            salida.EmailBody += "<br/>";

            salida.EmailBody += "<table style=' border: solid 1px #e8eef4;font-weight: bold;border-collapse: collapse;width:70%;'>";

            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;' colspan='2'>Cálculo de diferencias de varios periodos</th>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Salario Escolar</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡ " + model.PagoFeriados.MontoSalarioEscolar.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Subtotal días</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡ " + model.PagoFeriados.MontoSubtotalDia.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Subtotal</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡ " + (Math.Round(model.PagoFeriados.MontoSalarioEscolar + model.PagoFeriados.MontoSubtotalDia, 2)).ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "</table>";
            salida.EmailBody += "<br/>";
            return salida;
        }

        /// <summary>
        /// Dibuja las deducciones efectuadas para el trámite de pago
        /// </summary>
        /// <returns>El helper del email lleno</returns>
        private EmailWebHelper DibujarDeduccionesEmail(EmailWebHelper salida, FormularioPagoFeriadoVM model)
        {

            salida.EmailBody += "<table style=' border: solid 1px #e8eef4;font-weight: bold;border-collapse: collapse;width:70%;'>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th colspan ='2' style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Deducciones</th>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Total de deducciones obrero</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡" + model.PagoFeriados.MontoDeduccionObrero.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Total de deducciones patronales</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡" + model.PagoFeriados.MontoDeduccionPatronal.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "</table>";

            return salida;
        }

        /// <summary>
        /// Dibuja las tablas de totales del trámite de pago anulado
        /// </summary>
        /// <returns>El helper del email lleno</returns>
        private EmailWebHelper DibujarTotalesEmail(EmailWebHelper salida, FormularioPagoFeriadoVM model)
        {
            salida.EmailBody += "<br/>";
            salida.EmailBody += "<table style=' border: solid 1px #e8eef4;font-weight: bold;border-collapse: collapse;width:70%;'>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<th  colspan ='2' style='background-color: #808080;color: white;padding: 6px 5px;text-align: left; border: solid 1px #e8eef4;'>Totales</th>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Diferencia líquida</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡" + model.PagoFeriados.MontoDiferenciaLiquida.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>Aguinaldo proporcional</td>";
            salida.EmailBody += "<td style ='padding: 5px;width:25%;'>" + "₡" + model.PagoFeriados.MontoAguinaldoProporcional.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "<tr>";
            salida.EmailBody += "<td style='background-color: #DDDDDD; padding: 5px;width:25%;'>Monto total</td>";
            salida.EmailBody += "<td style='background-color: #DDDDDD; padding: 5px;width:25%;'>" + "₡" + model.PagoFeriados.MontoDeTotal.ToString("#,#.00#;(#,#.00#)") + "</td>";
            salida.EmailBody += "</tr>";
            salida.EmailBody += "</table>";

            return salida;
        }

        /// <summary>
        /// Dibuja el pie del correo elecyrónico y asigna los destinatarios
        /// </summary>
        /// <returns>El helper del email lleno</returns>
        private EmailWebHelper EmailPie(EmailWebHelper salida)
        {
            salida.EmailBody += "<br><br>";
            salida.EmailBody += "Para más información, puede ingresar al Módulo de Control de Pago de Feriados en la ubicación: http://sisrh.mopt.go.cr:82/PagoFeriado/";
            salida.EmailBody += "<br><br>Por favor no responder a este correo, ya que fue generado automáticamente.";
            salida.EmailBody += "<br><br>Atentamente,";
            salida.EmailBody += "<br>Unidad de Informática. DGIRH.";
            salida.EmailBody += "<br>Dirección de Gestión Institucional de Recursos Humanos.";
            salida.Destinos = "keffek98@live.com";

            return salida;
        }


        //-------------------Aguinaldo-------------------------------------------------

        //public ActionResult SearchAguinaldo()
        //{
        //    context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.FeriadosPT), 0);
        //    if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.FeriadosPT)].ToString().StartsWith("Error"))
        //    {
        //        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.FeriadosPT) });
        //    }
        //    else
        //    {
        //        if (Convert.ToBoolean(Session["Administrador_Global"]) ||
        //            Convert.ToBoolean(Session["Administrador_Feriado"]) ||
        //            Session["Permiso_Feriado_Operativo"] != null ||
        //            Session["Permiso_Feriado_Consulta"] != null)
        //        {
        //            BusquedaPagoFeriadoVM model = new BusquedaPagoFeriadoVM();

        //            List<string> diasFeriados = new List<string>();
        //            var auxiliar = servicioPagoFeriado.ListarDiasPorTipo(1).Select(
        //                            P => new SelectListItem
        //                            {
        //                                Value = ((CCatalogoDiaDTO)P.ElementAt(0)).IdEntidad.ToString(),
        //                                Text = ((CCatalogoDiaDTO)P.ElementAt(0)).DescripcionDia.ToString()
        //                            }
        //                            );

        //            model.DiaFeriado = new SelectList(auxiliar, "Value", "Text");


        //            var auxiliarA = servicioPagoFeriado.ListarDiasPorTipo(2).Select(
        //                          P => new SelectListItem
        //                          {
        //                              Value = ((CCatalogoDiaDTO)P.ElementAt(0)).IdEntidad.ToString(),
        //                              Text = ((CCatalogoDiaDTO)P.ElementAt(0)).DescripcionDia.ToString()
        //                          }
        //                          );

        //            model.DiaAsueto = new SelectList(auxiliarA, "Value", "Text");
        //            var estados = new List<string>
        //                             {
        //                                 "Activo",
        //                                 "Anulado"
        //                             };

        //            var datoEstado = estados.Select(P => new SelectListItem
        //            {
        //                Value = P.ToString(),
        //                Text = P.ToString()
        //            });

        //            model.Estados = new SelectList(datoEstado, "Value", "Text");

        //            return View(model);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Error", new { errorType = "acceso", modulo = "pagoFeriado" });
        //        }
        //    }
        //}



    }



}
