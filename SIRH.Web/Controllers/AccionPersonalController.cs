using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//using SIRH.Web.AccionPersonalLocal;
//using SIRH.Web.TipoAccionPersonalLocal;
//using SIRH.Web.FuncionarioLocal;
//using SIRH.Web.PuestoLocal;
//using SIRH.Web.PerfilUsuarioLocal;
//using SIRH.Web.EstadoBorradorLocal;

//using SIRH.Web.AccionPersonalDesa;
//using SIRH.Web.FuncionarioDesa;
//using SIRH.Web.PuestoDesa;
//using SIRH.Web.PerfilUsuarioDesa;
//using SIRH.Web.TipoAccionPersonalDesa;
//using SIRH.Web.EstadoBorradorDesa;

using SIRH.Web.AccionPersonalService;
using SIRH.Web.TipoAccionPersonalService;
using SIRH.Web.FuncionarioService;
using SIRH.Web.PuestoService;
using SIRH.Web.PerfilUsuarioService;
using SIRH.Web.EstadoBorradorService;

using SIRH.DTO;
using SIRH.Web.ViewModels;
using SIRH.Web.Reports.PDF;
using System.IO;
using SIRH.Web.Reports.AccionPersonal;
using SIRH.Web.Helpers;
using System.Security.Principal;
using System.Threading;
using System.Web.UI;
using SIRH.Web.UserValidation;
using System.Globalization;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;
using SIRH.Web.Models;

namespace SIRH.Web.Controllers
{
    public class AccionPersonalController : Controller
    {
        // GET: /AccionPersonal/

        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CPerfilUsuarioServiceClient servicioUsuario = new CPerfilUsuarioServiceClient();
        CTipoAccionPersonalServiceClient servicioTipo = new CTipoAccionPersonalServiceClient();
        CPuestoServiceClient servicioPuesto = new CPuestoServiceClient();
        CEstadoBorradorServiceClient servicioEstado = new CEstadoBorradorServiceClient();
        CAccesoWeb context = new CAccesoWeb();

        EmailWebHelper em = new EmailWebHelper();
        
        //WindowsIdentity principal = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        const string strAsunto = "SIRH. Módulo de Acción de Personal";
        
        public enum EstadosBorrador
        {
            Indefinido = 0,
            ElaboracionBorrador = 1,
            EnAnalisis = 2,
            EnRegistro = 3,
            Finalizado = 4,
            Anulado = 5
        }

        public ActionResult Index()
        {
            //principal.Name
            //string Name = System.Environment.UserName;
            //UserPrincipal user = UserPrincipal.Current;

            //context.IniciarSesionModulo(Session, principal.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CAccionPersonalDTO).Name));
                return View();
            }
        }

        // GET: /AccionPersonal/Details/1
        public ActionResult Details(string numAccion)
        {
             context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

             if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
             {
                 return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
             }
             else
             {
                 if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                     Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                     Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Consulta))] != null ||
                     Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null)
                 {
                     FormularioAccionPersonalVM model = new FormularioAccionPersonalVM();

                     var datos = servicioAccion.ObtenerAccion(numAccion);

                    decimal monSalarioBaseCalculo = 0;
                    decimal monSalarioBaseAnteriorCalculo = 0;

                    if (datos.Count() > 1)
                     {
                        model.Accion = (CAccionPersonalDTO)datos.ElementAt(0);
                        model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                        model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(2);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                        model.Expediente = (CExpedienteFuncionarioDTO)datos.ElementAt(4);
                        model.Puesto = (CPuestoDTO)datos.ElementAt(5);
                        model.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(6);
                        model.Contrato = (CDetalleContratacionDTO)datos.ElementAt(7);

                        ////var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        //var datosFuncionario = servicioAccion.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);

                        //if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        //{
                        //    model.Puesto = (CPuestoDTO)datosFuncionario.ElementAt(1);
                        //    model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario.ElementAt(2);
                        //    model.Contrato = (CDetalleContratacionDTO)datosFuncionario.ElementAt(3);

                        //    model.MesSeleccionado = model.Contrato.FechaMesAumento;
                        //}
                        //else //Si no tiene un nombramiento activo, buscar el de la Accion de Personal
                        //{
                        //    //var xx = ser.
                        //}

                        if(model.Contrato.CodigoPolicial <= 0)
                        {
                            var puntos = (CRespuestaDTO)servicioFuncionario.BuscarFuncionarioPuntosCarreraProfesional(model.Funcionario.Cedula);
                            model.PuntosCarrera = Convert.ToInt16(puntos.Contenido);
                        }
                        else
                        {
                            model.PuntosCarrera = 0;
                        }

                        if (datos.Count() > 8)
                        {
                            model.Detalle = (CDetalleAccionPersonalDTO)datos.ElementAt(8);

                            //model.Detalle.NumAnualidadAnterior = (model.TipoAccion.IdEntidad == 32) ? Convert.ToInt16((model.Contrato.NumeroAnualidades - model.Accion.IndDato)) : model.Contrato.NumeroAnualidades;
                            model.Detalle.NumAnualidadAnterior = (model.TipoAccion.IdEntidad == 32) ? Convert.ToInt16((model.Detalle.NumAnualidad - model.Accion.IndDato)) : model.Detalle.NumAnualidad;
                            model.Detalle.MtoAumentosAnualesAnterior = model.Detalle.NumAnualidadAnterior * model.Detalle.DetallePuestoAnterior.EscalaSalarial.MontoAumentoAnual;


                            model.Detalle.NumGradoGrupo = model.PuntosCarrera;
                            model.Detalle.MtoGradoGrupo = model.PuntosCarrera * model.Detalle.MtoPunto;
                            
                            model.Detalle.MtoTotal = Convert.ToDecimal(model.Detalle.MtoSalarioBase + model.Detalle.MtoAumentosAnuales + model.Detalle.MtoProhibicion +
                                                    model.Detalle.MtoGradoGrupo + model.Detalle.MtoOtros + model.Detalle.MtoRecargo);

                            model.Detalle.MtoTotalAnterior = Convert.ToDecimal(model.Detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase + model.Detalle.MtoAumentosAnualesAnterior + model.Detalle.MtoProhibicionAnterior +
                                                    model.Detalle.MtoGradoGrupo + model.Detalle.MtoOtrosAnterior + model.Detalle.MtoRecargo);


                            model.Detalle.Accion.NumAccion = model.Accion.NumAccion;

                            model.Clase = model.DetallePuesto.Clase; // model.Detalle.DetallePuesto.Clase;
                        }
                        else
                        {
                            model.Detalle = new CDetalleAccionPersonalDTO();
                            switch (model.TipoAccion.IdEntidad)
                            {
                                case 30:  // Ascenso en propiedad.
                                    var datosAnterior30 = servicioAccion.BuscarFuncionarioDetallePuestoAnterior(model.Funcionario.Cedula, model.Puesto.IdEntidad, 0);
                                    if (datosAnterior30.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {
                                        model.Detalle.DetallePuestoAnterior = (CDetallePuestoDTO)datosAnterior30[1];
                                        model.Detalle.DetallePuestoAnterior.Puesto = (CPuestoDTO)datosAnterior30[0];

                                    }
                                    // Si no tiene cargar la información del Detalle del Puesto que ocupa actualmente
                                    else
                                    {
                                        model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                                        model.DetallePuesto.Puesto = model.Puesto;
                                        model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                                    }
                                    break;

                                case 31:  // Ascenso interino.
                                    var datosAnterior31 = servicioAccion.BuscarFuncionarioDetallePuestoAnterior(model.Funcionario.Cedula, model.Puesto.IdEntidad, model.Accion.CodigoObjetoEntidad);
                                    if (datosAnterior31.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {

                                        // Asignar como Puesto Anterior, los datos del Puesto del FK_Nombramiento de la Acción
                                        model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                                        model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                                        // 
                                        model.Puesto = (CPuestoDTO)datosAnterior31[0];
                                        model.DetallePuesto = (CDetallePuestoDTO)datosAnterior31[1];
                                        model.Accion.Nombramiento.Puesto = model.Puesto;
                                    }
                                    // Si no tiene cargar la información del Detalle del Puesto que ocupa actualmente
                                    else
                                    {
                                        model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                                        model.DetallePuesto.Puesto = model.Puesto;
                                        model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                                    }
                                    break;

                                case 48:  // Prórroga de Ascenso Interino
                                    var datosAnterior = servicioAccion.BuscarFuncionarioDetallePuestoAnterior(model.Funcionario.Cedula, model.Puesto.IdEntidad,  0);
                                    if (datosAnterior.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {
                                        model.Detalle.DetallePuestoAnterior = (CDetallePuestoDTO)datosAnterior[1];
                                        model.Detalle.DetallePuestoAnterior.Puesto = (CPuestoDTO)datosAnterior[0];

                                    }
                                    // Si no tiene cargar la información del Detalle del Puesto que ocupa actualmente
                                    else
                                    {
                                        model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                                        model.DetallePuesto.Puesto = model.Puesto;
                                        model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                                    }
                                    break;

                                default:
                                    model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                                    model.DetallePuesto.Puesto = model.Puesto;
                                    model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                                    break;
                            }
                            //// Tipo 48. Prórroga de Ascenso Interino
                            //if (model.TipoAccion.IdEntidad == 48)
                            //{
                            //    var datosAnterior = servicioAccion.BuscarFuncionarioDetallePuestoAnterior(model.Funcionario.Cedula, model.Puesto.IdEntidad);
                            //    if (datosAnterior.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            //    {
                            //        model.Detalle.DetallePuestoAnterior = (CDetallePuestoDTO)datosAnterior[1];
                            //        model.Detalle.DetallePuestoAnterior.Puesto = (CPuestoDTO)datosAnterior[0];

                            //    }
                            //    // Si no tiene cargar la información del Detalle del Puesto que ocupa actualmente
                            //    else
                            //    {
                            //        model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                            //        model.DetallePuesto.Puesto = model.Puesto;
                            //        model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                            //    }
                            //}
                            //else {
                            //    // Buscar el detalle del Puesto en Propiedad
                            //    var datosFuncionarioPro = servicioFuncionario.BuscarFuncionarioDetallePuestoPropiedad(model.Funcionario.Cedula);

                            //    // Si tiene, cargarlo en el modelo.Detalle.DetallePuestoAnterior
                            //    if (datosFuncionarioPro.FirstOrDefault().GetType() != typeof(CErrorDTO))
                            //    {
                            //        model.Detalle.DetallePuestoAnterior = (CDetallePuestoDTO)datosFuncionarioPro[2];
                            //        model.Detalle.DetallePuestoAnterior.Puesto = (CPuestoDTO)datosFuncionarioPro[1];

                            //    }
                            //    // Si no tiene puesto en Propiedad, cargar la información del Detalle del Puesto que ocupa actualmente
                            //    else
                            //    {
                            //        model.Detalle.DetallePuestoAnterior = model.DetallePuesto;
                            //        model.DetallePuesto.Puesto = model.Puesto;
                            //        model.Detalle.DetallePuestoAnterior.Puesto = model.Puesto;
                            //    }
                            //}
                            
                            // Salario Base
                            model.Detalle.MtoSalarioBase = model.DetallePuesto.EscalaSalarial.SalarioBase;
                            
                            // Aumentos Anuales
                            model.Detalle.MtoAnual = model.DetallePuesto.EscalaSalarial.MontoAumentoAnual;
                            model.Detalle.NumAnualidad = Convert.ToInt32(model.Contrato.NumeroAnualidades);
                            model.Detalle.NumAnualidadAnterior = model.Detalle.NumAnualidad;

                            model.Detalle.MtoAumentosAnuales = model.Detalle.MtoAnual *  model.Detalle.NumAnualidad;
                            model.Detalle.MtoAumentosAnualesAnterior = model.Detalle.DetallePuestoAnterior.EscalaSalarial.MontoAumentoAnual * model.Detalle.NumAnualidadAnterior;


                            // Grado o Grupo
                            model.Detalle.NumGradoGrupo = model.PuntosCarrera;
                            model.Detalle.MtoGradoGrupo = model.PuntosCarrera * model.Detalle.DetallePuestoAnterior.EscalaSalarial.Periodo.MontoPuntoCarrera;
                            //model.Detalle.MtoGradoGrupo = model.PuntosCarrera * model.Detalle.MtoPunto;


                            // Prohibición / Dedicación
                            if (model.Detalle.DetallePuestoAnterior.PorDedicacion > model.Detalle.DetallePuestoAnterior.PorProhibicion)
                                model.Detalle.DetallePuestoAnterior.PorProhibicion = model.Detalle.DetallePuestoAnterior.PorDedicacion;

                            model.Detalle.PorProhibicion = model.Detalle.DetallePuestoAnterior.PorProhibicion;
                            model.Detalle.PorProhOriginal = model.Detalle.PorProhibicion;
                            model.Detalle.MtoProhibicion = (model.Detalle.MtoSalarioBase * model.Detalle.PorProhibicion) / 100;
                            model.Detalle.MtoProhibicionAnterior = (model.Detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase * model.Detalle.DetallePuestoAnterior.PorProhibicion) / 100;


                            // Otros
                            foreach(var rubro in model.DetallePuesto.DetalleRubros)
                            {
                                switch (rubro.Componente.IdEntidad)
                                {
                                    case 3:
                                        model.Detalle.PorBonificacion = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 4:
                                        model.Detalle.PorCarreraPolicial = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 9:
                                        model.Detalle.PorConsulta = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 10:
                                        model.Detalle.PorCurso = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 13:
                                        model.Detalle.PorDisponibilidad = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 16:
                                        model.Detalle.PorGradoPolicial = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 23:
                                        model.Detalle.PorQuinquenio = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 30:
                                        model.Detalle.PorRiesgo = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 35:
                                        model.Detalle.PorPeligrosidad = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    default:
                                        break;
                                }
                            }

                            // Otros Puesto Anterior
                            foreach (var rubro in model.Detalle.DetallePuestoAnterior.DetalleRubros)
                            {
                                switch (rubro.Componente.IdEntidad)
                                {
                                    case 3:
                                        model.Detalle.PorBonificacionAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 4:
                                        model.Detalle.PorCarreraPolicialAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 9:
                                        model.Detalle.PorConsultaAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 10:
                                        model.Detalle.PorCursoAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 13:
                                        model.Detalle.PorDisponibilidadAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 16:
                                        model.Detalle.PorGradoPolicialAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 23:
                                        model.Detalle.PorQuinquenioAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 30:
                                        model.Detalle.PorRiesgoAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 35:
                                        model.Detalle.PorPeligrosidadAnterior = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            //var porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 3);
                            //model.Detalle.PorBonificacion = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 3);
                            //model.Detalle.PorBonificacionAnterior = Convert.ToDecimal(porc.Contenido);


                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 9);
                            //model.Detalle.PorConsulta = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 9);
                            //model.Detalle.PorConsultaAnterior = Convert.ToDecimal(porc.Contenido);


                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 10);
                            //model.Detalle.PorCurso = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 10);
                            //model.Detalle.PorCursoAnterior = Convert.ToDecimal(porc.Contenido);


                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 13);
                            //model.Detalle.PorDisponibilidad = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 13);
                            //model.Detalle.PorDisponibilidadAnterior = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 16);
                            //model.Detalle.PorGradoPolicial = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 16);
                            //model.Detalle.PorGradoPolicialAnterior = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 23);
                            //model.Detalle.PorQuinquenio = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 23);
                            //model.Detalle.PorQuinquenioAnterior = Convert.ToDecimal(porc.Contenido);


                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 30);
                            //model.Detalle.PorRiesgo = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 30);
                            //model.Detalle.PorRiesgoAnterior = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 35);
                            //model.Detalle.PorPeligrosidad = Convert.ToDecimal(porc.Contenido);
                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.Detalle.DetallePuestoAnterior, 35);
                            //model.Detalle.PorPeligrosidadAnterior = Convert.ToDecimal(porc.Contenido);

                            monSalarioBaseCalculo = model.Detalle.MtoSalarioBase;
                            monSalarioBaseAnteriorCalculo = model.Detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase;

                            // Los incentivos policiales se calculan con la Escala Salarial de Julio 2018
                            if (model.Contrato.CodigoPolicial > 0)
                            {
                                var salario = servicioAccion.ObtenerEscalaCategoriaPeriodo(model.DetallePuesto.EscalaSalarial.CategoriaEscala, 1);
                                if (salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    monSalarioBaseCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                                }

                                salario = servicioAccion.ObtenerEscalaCategoriaPeriodo(model.Detalle.DetallePuestoAnterior.EscalaSalarial.CategoriaEscala, 1);
                                if(salario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    monSalarioBaseAnteriorCalculo = ((CEscalaSalarialDTO)salario[0]).SalarioBase;
                                }                              
                            }

                            model.Detalle.MtoOtros = (monSalarioBaseCalculo * 
                                                        (model.Detalle.PorQuinquenio +
                                                        model.Detalle.PorCarreraPolicial +
                                                        model.Detalle.PorCurso +
                                                        model.Detalle.PorDisponibilidad +
                                                        model.Detalle.PorRiesgo +
                                                        model.Detalle.PorPeligrosidad +
                                                        model.Detalle.PorGradoPolicial +
                                                        model.Detalle.PorConsulta +
                                                        model.Detalle.PorBonificacion)
                                                    ) / 100;

                            model.Detalle.MtoOtrosAnterior = (monSalarioBaseAnteriorCalculo * 
                                                                        (model.Detalle.PorQuinquenioAnterior +
                                                                        model.Detalle.PorCarreraPolicialAnterior +
                                                                        model.Detalle.PorCursoAnterior + 
                                                                        model.Detalle.PorDisponibilidadAnterior + 
                                                                        model.Detalle.PorRiesgoAnterior + 
                                                                        model.Detalle.PorPeligrosidadAnterior +
                                                                        model.Detalle.PorGradoPolicialAnterior +
                                                                        model.Detalle.PorConsultaAnterior + 
                                                                        model.Detalle.PorBonificacionAnterior) 
                                                              ) / 100;


                            //  TOTALES
                            model.Detalle.MtoTotal = Convert.ToDecimal(model.Detalle.MtoSalarioBase + model.Detalle.MtoAumentosAnuales + model.Detalle.MtoProhibicion +
                                                    model.Detalle.MtoGradoGrupo + model.Detalle.MtoOtros + model.Detalle.MtoRecargo);

                            model.Detalle.MtoTotalAnterior = Convert.ToDecimal(model.Detalle.DetallePuestoAnterior.EscalaSalarial.SalarioBase + model.Detalle.MtoAumentosAnualesAnterior + model.Detalle.MtoProhibicionAnterior +
                                                    model.Detalle.MtoGradoGrupo + model.Detalle.MtoOtrosAnterior + model.Detalle.MtoRecargo);


                            model.MesSeleccionado = CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[Convert.ToInt16(model.Contrato.FechaMesAumento) - 1].ToString().ToUpper();
                            model.Clase = model.DetallePuesto.Clase;

                            model.Detalle.Accion = new CAccionPersonalDTO { NumAccion = model.Accion.NumAccion };
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
                     CAccesoWeb.CargarErrorAcceso(Session);
                     return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
                 }
             }
        }

        [HttpGet]
        public ActionResult DetailsH(int codigo)
        {
            FormularioAccionPersonalVM model = new FormularioAccionPersonalVM();

            var datos = servicioAccion.ObtenerAccionHistorico(codigo);
            if (datos.Count() > 0)
            {
                model.Historico = (CAccionPersonalHistoricoDTO)datos.ElementAt(0);
                //model.Historico.MtoTotal = Convert.ToDecimal(model.Historico.MtoSalarioBase) + Convert.ToDecimal(model.Historico.MtoAumentosAnuales) + Convert.ToDecimal(model.Historico.MtoProhibicion) +
                //                          Convert.ToDecimal(model.Historico.MtoRecargo) + Convert.ToDecimal(model.Historico.MtoGradoGrupo) + Convert.ToDecimal(model.Historico.MtoOtros);
                //model.Historico.MtoTotal = Convert.ToDecimal(model.Historico.MtoSalarioBase != "" ? model.Historico.MtoSalarioBase : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoAumentosAnuales != "" ? model.Historico.MtoAumentosAnuales : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoProhibicion != "" ? model.Historico.MtoProhibicion : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoRecargo != "" ? model.Historico.MtoRecargo : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoGradoGrupo != "" ? model.Historico.MtoGradoGrupo : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoOtros != "" ? model.Historico.MtoOtros : "0");
                //model.Historico.MtoTotal2 = Convert.ToDecimal(model.Historico.MtoSalarioBase2 != "" ? model.Historico.MtoSalarioBase2 : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoAumentosAnuales2 != "" ? model.Historico.MtoAumentosAnuales2 : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoProhibicion2 != "" ? model.Historico.MtoProhibicion2 : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoRecargo2 != "" ? model.Historico.MtoRecargo2 : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoGradoGrupo2 != "" ? model.Historico.MtoGradoGrupo2 : "0") +
                //                            Convert.ToDecimal(model.Historico.MtoOtros2 != "" ? model.Historico.MtoOtros2 : "0");

                //model.Historico.MtoSalarioBase = model.Historico.MtoSalarioBase; // Convert.ToDecimal(model.Historico.MtoSalarioBase != "" ? model.Historico.MtoSalarioBase : "0").ToString("#,##0.00");
                //model.Historico.MtoAumentosAnuales = Convert.ToDecimal(model.Historico.MtoAumentosAnuales != "" ? model.Historico.MtoAumentosAnuales : "0").ToString("#,##0.00");
                //model.Historico.MtoProhibicion = Convert.ToDecimal(model.Historico.MtoProhibicion != "" ? model.Historico.MtoProhibicion : "0").ToString("#,##0.00");
                //model.Historico.MtoRecargo = Convert.ToDecimal(model.Historico.MtoRecargo != "" ? model.Historico.MtoRecargo : "0").ToString("#,##0.00");
                //model.Historico.MtoGradoGrupo = Convert.ToDecimal(model.Historico.MtoGradoGrupo != "" ? model.Historico.MtoGradoGrupo : "0").ToString("#,##0.00");
                //model.Historico.MtoOtros = Convert.ToDecimal(model.Historico.MtoOtros != "" ? model.Historico.MtoOtros : "0").ToString("#,##0.00");

                //model.Historico.MtoSalarioBase2 = String.Format(model.Historico.MtoSalarioBase2, "{0:n}"); //Convert.ToDecimal(model.Historico.MtoSalarioBase2 != "" ? model.Historico.MtoSalarioBase2 : "0").ToString("#,##0.00");
                //model.Historico.MtoAumentosAnuales2 = Convert.ToDecimal(model.Historico.MtoAumentosAnuales2 != "" ? model.Historico.MtoAumentosAnuales2 : "0").ToString("#,##0.00");
                //model.Historico.MtoProhibicion2 = Convert.ToDecimal(model.Historico.MtoProhibicion2 != "" ? model.Historico.MtoProhibicion2 : "0").ToString("#,##0.00");
                //model.Historico.MtoRecargo2 = Convert.ToDecimal(model.Historico.MtoRecargo2 != "" ? model.Historico.MtoRecargo2 : "0").ToString("#,##0.00");
                //model.Historico.MtoGradoGrupo2 = Convert.ToDecimal(model.Historico.MtoGradoGrupo2 != "" ? model.Historico.MtoGradoGrupo2 : "0").ToString("#,##0.00");
                //model.Historico.MtoOtros2 = Convert.ToDecimal(model.Historico.MtoOtros2 != "" ? model.Historico.MtoOtros2 : "0").ToString("#,##0.00");


                //var datosFuncionario = servicioFuncionario.BuscarFuncionarioBase(model.Historico.Cedula);

                //if (datosFuncionario.GetType() != typeof(CErrorDTO))
                //{
                //    model.Funcionario = (CFuncionarioDTO)datosFuncionario;
                //}

                model.Funcionario = new CFuncionarioDTO
                {
                    Cedula = model.Historico.Cedula,
                    Nombre = model.Historico.Nombre,
                    PrimerApellido = model.Historico.Apellido1,
                    SegundoApellido = model.Historico.Apellido2
                };

                var datosTipo = servicioTipo.ObtenerTipo(Convert.ToInt16(model.Historico.CodAccion));

                if (datosTipo.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.TipoAccion = (CTipoAccionPersonalDTO)datosTipo.ElementAt(0);
                }
            }
            else
            {
                model.Error = (CErrorDTO)datos.ElementAt(0);
            }

            return PartialView("DetailsH", model);
        }


        // GET: /AccionPersonal/Create
        public ActionResult Create()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
                }
            }
        }

        // POST: /AccionPersonal/Create
        [HttpPost]
        public ActionResult Create(FormularioAccionPersonalVM model, string SubmitButton)
        {
            model.Error = null;
            IEnumerable<SelectListItem> listadoTipos = null;
            IEnumerable<SelectListItem> listadoDirecciones = null;
            IEnumerable<SelectListItem> listadoSecciones = null;
            IEnumerable<SelectListItem> listadoClases = null;
            IEnumerable<SelectListItem> listadoEspecialidades = null;
            IEnumerable<SelectListItem> listadoSubespecialidades = null;
            IEnumerable<SelectListItem> listadoCategorias = null;

            List<SelectListItem> listadoMeses = new List<SelectListItem>();
            List<SelectListItem> listadoAnios = new List<SelectListItem>();
            List<SelectListItem> listadoPorcentajes = new List<SelectListItem>();

            try
            {
                if (string.IsNullOrEmpty(model.Funcionario.Cedula))
                {
                    throw new Exception("Busqueda");
                }

                listadoTipos = servicioTipo.RetornarTipos()
                  .Where(item => ((CTipoAccionPersonalDTO)item).IndCategoria != 0)
                  .Select(Q => new SelectListItem
                  {
                      Value = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString(),
                      Text = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString() + " - " + ((CTipoAccionPersonalDTO)Q).DesTipoAccion
                  });

                listadoDirecciones = servicioPuesto.DescargarDireccionGenerals(0, "")
                 .Select(Q => new SelectListItem
                 {
                     Value = ((CDireccionGeneralDTO)Q).IdEntidad.ToString(),
                     Text = ((CDireccionGeneralDTO)Q).IdEntidad.ToString() + " - " + ((CDireccionGeneralDTO)Q).NomDireccion
                 });

                listadoSecciones = servicioPuesto.DescargarSeccions(0, "")
                    .Select(Q => new SelectListItem
                    {
                      Value = ((CSeccionDTO)Q).IdEntidad.ToString(),
                      Text = ((CSeccionDTO)Q).IdEntidad.ToString() + " - " + ((CSeccionDTO)Q).NomSeccion
                    });

                listadoEspecialidades = servicioPuesto.DescargarEspecialidades(0,"")
                   .Select(Q => new SelectListItem
                   {
                       Value = Q.IdEntidad.ToString(),
                       Text = Q.DesEspecialidad
                   });

                listadoSubespecialidades = servicioPuesto.DescargarSubEspecialidades("")
                   .Select(Q => new SelectListItem
                   {
                       Value = Q.IdEntidad.ToString(),
                       Text = Q.DesSubEspecialidad
                   });

                listadoClases = servicioPuesto.DescargarClases(0, "")
                    .Select(Q => new SelectListItem
                    {
                        Value = ((CClaseDTO)Q).IdEntidad.ToString(),
                        Text = ((CClaseDTO)Q).IdEntidad.ToString() + " - " + ((CClaseDTO)Q).DesClase
                    });

                listadoCategorias = servicioPuesto.ListarCategoriasEscalaSalarial()
                   .Select(Q => new SelectListItem
                   {
                       Value = Q.IdEntidad.ToString(),
                       Text = Q.IdEntidad.ToString()
                   });
                                               
                SelectListItem selListItem = new SelectListItem() { Value = "10", Text = "10" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "15", Text = "15" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "20", Text = "20" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "30", Text = "30" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "25", Text = "25" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "35", Text = "35" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "55", Text = "55" };
                listadoPorcentajes.Add(selListItem);
                selListItem = new SelectListItem() { Value = "65", Text = "65" };
                listadoPorcentajes.Add(selListItem);
                                
                listadoAnios.Add(new SelectListItem() { Value = DateTime.Today.Year.ToString(), Text = DateTime.Today.Year.ToString() });
                listadoAnios.Add(new SelectListItem() { Value = DateTime.Today.AddYears(-1).Year.ToString(), Text = DateTime.Today.AddYears(-1).Year.ToString() });
                listadoAnios.Add(new SelectListItem() { Value = DateTime.Today.AddYears(-2).Year.ToString(), Text = DateTime.Today.AddYears(-2).Year.ToString() });
                listadoAnios.Add(new SelectListItem() { Value = DateTime.Today.AddYears(-3).Year.ToString(), Text = DateTime.Today.AddYears(-3).Year.ToString() });

                
                for (int i = 0; i < 12; i ++ )
                {
                    listadoMeses.Add(new SelectListItem() { Value = (i + 1).ToString(), Text = CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i].ToString().ToUpper() });
                }

                if (SubmitButton == "Buscar")
                {
                    if (ModelState.IsValid == true)
                    {
                        //var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        var datosFuncionario = servicioAccion.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                        ViewBag.Message = model.Funcionario.Cedula;
                        if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                            model.Puesto = (CPuestoDTO)datosFuncionario[1];
                            model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                            model.Contrato = (CDetalleContratacionDTO)datosFuncionario[3];
                            model.Nombramiento = (CNombramientoDTO)datosFuncionario[4];
                            model.MtoSalarioCalculo = ((CDetalleAccionPersonalDTO)datosFuncionario[5]).MtoSalarioBase;

                            model.Tipos = new SelectList(listadoTipos, "Value", "Text");

                            var datosUbicacion = servicioPuesto.DescargarUbicacionAdministrativa(model.Funcionario.Cedula);
                            model.Programa = (CProgramaDTO)datosUbicacion[1];

                            model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                            model.DireccionSeleccionada = model.Puesto.UbicacionAdministrativa.DireccionGeneral.IdEntidad.ToString();

                            model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                            model.SeccionSeleccionada = model.Puesto.UbicacionAdministrativa.Seccion.IdEntidad.ToString();

                            model.Clases = new SelectList(listadoClases, "Value", "Text");
                            model.ClaseSeleccionada = model.DetallePuesto.Clase.IdEntidad.ToString();

                            model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                            model.SeccionSeleccionada = model.Puesto.UbicacionAdministrativa.Seccion.IdEntidad.ToString();

                            model.Especialidades = new SelectList(listadoEspecialidades, "Value", "Text");
                            model.EspecialidadSeleccionada = model.DetallePuesto.Especialidad.IdEntidad.ToString();

                            model.Subespecialidades = new SelectList(listadoSubespecialidades, "Value", "Text");
                            model.SubespecialidadSeleccionada = (model.DetallePuesto.SubEspecialidad != null) ? model.DetallePuesto.SubEspecialidad.IdEntidad.ToString() : "";

                            model.Categorias = new SelectList(listadoCategorias, "Value", "Text");
                            model.CategoriaSeleccionada = model.DetallePuesto.EscalaSalarial.CategoriaEscala.ToString();

                            model.Meses = new SelectList(listadoMeses, "Value", "Text");
                            model.MesSeleccionado = model.Contrato.FechaMesAumento;

                            model.Annios = new SelectList(listadoAnios, "Value", "Text");

                            // Si es Policía de Tránsito, no tiene puntos de carrera
                            if (model.Contrato.CodigoPolicial > 0)
                            {
                                model.PuntosCarrera = 0;
                            }
                            else
                            {
                                var puntos = (CRespuestaDTO)servicioFuncionario.BuscarFuncionarioPuntosCarreraProfesional(model.Funcionario.Cedula);
                                model.PuntosCarrera = Convert.ToInt16(puntos.Contenido);
                            }
                            
                            decimal porProhDedicExc = 0;
                            decimal porDisponibilidad = 0;
                            decimal porCursoPolicial = 0;
                            decimal porRiesgo = 0;
                            decimal porQuinquenio = 0;
                            decimal porPeligrosidad = 0;
                            decimal porConsultaExterna = 0;
                            decimal porBonificacion = 0;
                            decimal porGradoPolicial = 0;
                            decimal porCarreraPolicial = 0;

                            decimal mtoRecargo = 0;

                            if (model.DetallePuesto.PorProhibicion != null)
                                porProhDedicExc = model.DetallePuesto.PorProhibicion;

                            if (model.DetallePuesto.PorDedicacion != null && porProhDedicExc == 0)
                                porProhDedicExc = model.DetallePuesto.PorDedicacion;

                            if (porProhDedicExc > 0 && listadoPorcentajes.FindIndex(L => L.Value == porProhDedicExc.ToString()) < 0)
                            {
                                selListItem = new SelectListItem() { Value = porProhDedicExc.ToString(), Text = porProhDedicExc.ToString() };
                                listadoPorcentajes.Add(selListItem);
                            }

                            model.Porcentajes = new SelectList(listadoPorcentajes, "Value", "Text");

                            //var porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 3);
                            //porBonificacion = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 9);
                            //porConsultaExterna = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 10);
                            //porCursoPolicial = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 13);
                            //porDisponibilidad = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 16);
                            //porGradoPolicial = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 23);
                            //porQuinquenio = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 30);
                            //porRiesgo = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 32);
                            //mtoRecargo = Convert.ToDecimal(porc.Contenido);

                            //porc = (CRespuestaDTO)servicioAccion.ObtenerPorcentajeComponenteSalarialDetallePuesto(model.DetallePuesto, 33);
                            //porPeligrosidad = Convert.ToDecimal(porc.Contenido);

                            // Otros
                            foreach (var rubro in model.DetallePuesto.DetalleRubros)
                            {
                                switch (rubro.Componente.IdEntidad)
                                {
                                    case 3:
                                        porBonificacion = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 4:
                                        porCarreraPolicial = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 9:
                                        porConsultaExterna = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 10:
                                        porCursoPolicial = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 13:
                                        porDisponibilidad = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 16:
                                        porGradoPolicial = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 23:
                                        porQuinquenio = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 30:
                                        porRiesgo = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    case 35:
                                        porPeligrosidad = Convert.ToDecimal(rubro.PorValor);
                                        break;

                                    default:
                                        break;
                                }
                            }

                            decimal mtoSalario = model.DetallePuesto.EscalaSalarial.SalarioBase;
                            decimal mtoAnuales = Convert.ToDecimal(model.DetallePuesto.EscalaSalarial.MontoAumentoAnual * model.Contrato.NumeroAnualidades);
                            decimal mtoGradoGrupo = Convert.ToDecimal(model.PuntosCarrera * model.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera);
                            decimal mtoProhibicion = Convert.ToDecimal(model.DetallePuesto.EscalaSalarial.SalarioBase * porProhDedicExc / 100);
                            //decimal mtoOtros = Convert.ToDecimal(model.DetallePuesto.EscalaSalarial.SalarioBase * (porQuinquenio + porDisponibilidad + porCursoPolicial + porRiesgo + porPeligrosidad + porGradoPolicial + porConsultaExterna + porBonificacion)) / 100;
                            decimal mtoOtros = Convert.ToDecimal(model.DetallePuesto.DetalleRubros.Sum(Q => Q.MtoValor));
                            model.Detalle = new CDetalleAccionPersonalDTO
                            {
                                CodPrograma = model.Programa.IdEntidad,
                                CodSeccion = Convert.ToInt16(model.SeccionSeleccionada),
                                CodPuesto = model.Puesto.CodPuesto,
                                CodEspecialidad = Convert.ToInt16(model.DetallePuesto.Especialidad.IdEntidad),
                                CodSubespecialidad = (model.DetallePuesto.SubEspecialidad != null) ? Convert.ToInt16(model.DetallePuesto.SubEspecialidad.IdEntidad) : 0,
                                IndCategoria = model.DetallePuesto.EscalaSalarial.CategoriaEscala,
                                NumAnualidad = model.Contrato.NumeroAnualidades,
                                MesAumento = Convert.ToInt16(model.Contrato.FechaMesAumento),
                                MtoSalarioBase = mtoSalario,
                                MtoAnual = model.DetallePuesto.EscalaSalarial.MontoAumentoAnual,
                                MtoAumentosAnuales = mtoAnuales,
                                PorCarreraPolicial = porCarreraPolicial,
                                PorCurso = porCursoPolicial,
                                PorDisponibilidad = porDisponibilidad,
                                PorRiesgo = porRiesgo,
                                PorBonificacion = porBonificacion,
                                PorConsulta = porConsultaExterna,
                                PorPeligrosidad = porPeligrosidad,
                                PorGradoPolicial = porGradoPolicial,
                                MtoRecargo = mtoRecargo,
                                MtoPunto = model.DetallePuesto.EscalaSalarial.Periodo.MontoPuntoCarrera,
                                NumGradoGrupo = model.PuntosCarrera,
                                MtoGradoGrupo = mtoGradoGrupo,
                                PorProhOriginal = porProhDedicExc,
                                PorProhibicion = porProhDedicExc,
                                MtoProhibicion = mtoProhibicion,
                                PorQuinquenio = porQuinquenio,
                                MtoOtros = mtoOtros,
                                MtoTotal = mtoSalario + mtoAnuales + mtoRecargo + mtoGradoGrupo + mtoProhibicion + mtoOtros,
                                MtoTotalNuevo = mtoSalario + mtoAnuales + mtoRecargo + mtoGradoGrupo + mtoProhibicion + mtoOtros
                            };
                            //model.Accion = new CAccionPersonalDTO();
                            
                            //model.Accion.CodigoModulo = 0;
                            //model.Accion.CodigoObjetoEntidad = 0;

                            return PartialView("_FormularioAccionPersonal", model);
                        }
                        else
                        {
                            ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
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
                    if (model.AnioSeleccionado == null)
                        model.AnioSeleccionado = DateTime.Today.Year.ToString();

                    if (!(model.Accion.FecRige.Year > 0001))
                    {
                        ModelState.AddModelError("formulario", "Debe completar todos los campos");
                        throw new Exception("Busqueda");
                    }

                    if (ModelState.IsValid == true)
                    {
                        model.Funcionario.Sexo = GeneroEnum.Indefinido;

                        //CProgramaDTO programa = new CProgramaDTO
                        //{
                        //    IdEntidad = model.Programa.IdEntidad
                        //};

                        //CSeccionDTO seccion = new CSeccionDTO
                        //{
                        //    IdEntidad = Convert.ToInt32(model.SeccionSeleccionada)
                        //};

                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                        {
                            IdEntidad = 3 // En Registro
                        };

                        CTipoAccionPersonalDTO tipo = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };

                        //model.Detalle.IndCategoria = Convert.ToInt32(model.CategoriaSeleccionada);
                        model.Detalle.CodClase = Convert.ToInt32(model.ClaseSeleccionada);
                        model.Detalle.CodPrograma = model.Programa.IdEntidad;
                        model.Detalle.CodSeccion = Convert.ToInt32(model.SeccionSeleccionada);
                        model.Detalle.CodEspecialidad = Convert.ToInt32(model.EspecialidadSeleccionada);
                        model.Detalle.CodSubespecialidad = Convert.ToInt32(model.SubespecialidadSeleccionada);

                        switch(model.TipoSeleccionado)
                        {
                            case 9:  // Prórroga de permiso con salario
                            case 10: // Prórroga de permiso sin salario
                            case 48: // Prórroga de asc. interino
                                // Asignar el Nombramiento del último Permiso
                                if (model.Accion.CodigoObjetoEntidad > 0)
                                    model.Nombramiento = new CNombramientoDTO { IdEntidad = model.Accion.CodigoObjetoEntidad };
                                break;

                            case 32: // Aumento Anual
                                model.Accion.IndDato = -(model.Contrato.NumeroAnualidades - model.Detalle.NumAnualidad);
                                break;

                            case 41: //  Reaj. Aprobación Peligrosidad
                                model.Detalle.MtoOtros = 5;
                                model.Accion.IndDato = 5;
                                break;

                            case 42: //  Reaj. Eliminación Peligrosidad
                                model.Detalle.MtoOtros = 0;
                                model.Accion.IndDato = -5;
                                break;

                            case 53: //  Reajuste de sobresueldos.
                                model.Accion.IndDato = model.Detalle.MtoRecargo;
                                break;

                            case 55:  // Recargo de funciones.
                                model.Detalle.MtoRecargo = Math.Abs(model.Detalle.MtoTotalNuevo - model.Detalle.MtoTotal);
                                break;

                            case 60: //  Reajuste aprobación prohibición.
                                model.Accion.IndDato = -(model.DetallePuesto.PorProhibicion - model.Detalle.PorProhibicion);
                                break;

                            case 62: //  Reajuste aprob. Dedic. Exclusiva.
                                model.Accion.IndDato = -(model.DetallePuesto.PorDedicacion - model.Detalle.PorProhibicion);
                                break;

                            case 61: //  Reajuste eliminación prohibición.
                                model.Detalle.PorProhibicion = 0;
                                model.Accion.IndDato = -(model.DetallePuesto.PorProhibicion);
                                break;

                            case 63: //  Reaj. Eliminación dedic. Exclusiva.
                                model.Detalle.PorProhibicion = 0;
                                model.Accion.IndDato = -(model.DetallePuesto.PorDedicacion);
                                break;

                            case 66: //  Reaj. Aprob. Riesgo policial
                                model.Detalle.MtoOtros = 18;
                                model.Accion.IndDato = 18;
                                break;

                            case 67: //  Reaj. Eliminación. Riesgo policial
                                model.Detalle.MtoOtros = 0;
                                model.Accion.IndDato = -18;
                                break;

                            case 68: //  Reaj. Aprob. Disponibilidad
                                model.Detalle.MtoOtros = 25; // model.Detalle.PorDisponibilidad;
                                model.Accion.IndDato = 25;  //model.Detalle.PorDisponibilidad;
                                break;

                            case 69: //  Reaj. Eliminac. Disponibilidad
                                model.Detalle.MtoOtros = 0;
                                model.Accion.IndDato = -25;
                                break;

                            case 74: //  Reaj. Por quinquenio
                                model.Detalle.MtoOtros = model.Detalle.PorQuinquenio + 5;
                                model.Accion.IndDato = 5;
                                break;

                            case 81: //  Reaj. Aprobación Consulta Externa
                                model.Detalle.MtoOtros = 22;
                                model.Accion.IndDato = 22;
                                break;

                            case 82: //  Reaj. Eliminación Consulta Externa
                                model.Detalle.MtoOtros = 0;
                                model.Accion.IndDato = -22;
                                break;

                            case 83: //  Reaj. Aprobación Bonificación Adicional
                                model.Detalle.MtoOtros = 17;
                                model.Accion.IndDato = 17;
                                break;

                            case 84: //  Reaj. Eliminación Bonificación Adicional
                                model.Detalle.MtoOtros = 0;
                                model.Accion.IndDato = -17;
                                break;

                            case 88: // Modificación de fecha de A.A  (Aumento Anual)
                                model.Accion.IndDato = Convert.ToInt16(model.MesSeleccionado) - Convert.ToInt16(model.Contrato.FechaMesAumento);
                                break;

                            default:
                                model.Accion.IndDato = 0;
                                break; 
                        }
                        
                        model.Accion.AnioRige = Convert.ToInt16(model.AnioSeleccionado);
                        model.Accion.Nombramiento = model.Nombramiento;
                        var respuesta = servicioAccion.AgregarAccion(model.Funcionario,
                                                                    estado,
                                                                    tipo,
                                                                    model.Accion,
                                                                    model.Detalle);
                        //var respuesta = new CErrorDTO();
                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (((CRespuestaDTO)respuesta).Codigo > 0)
                            {
                                List<string> entidades = new List<string>();
                                entidades.Add(typeof(CAccionPersonalDTO).Name);

                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal),
                                        Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                                        CAccesoWeb.ListarEntidades(entidades.ToArray()));

                                return JavaScript("window.location = '/AccionPersonal/Details?numAccion=" +
                                                    ((CRespuestaDTO)respuesta).Contenido + "&accion=guardar" + "';");
                            }
                            else
                            {
                                ModelState.AddModelError("Agregar", respuesta.Mensaje + " Cont 1");
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", respuesta.Mensaje + " Cont 2");
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        throw new Exception("Formulario" + " Cont 3");
                    }
                }
            }
            catch (Exception error)
            {
                model.Tipos = new SelectList(listadoTipos, "Value", "Text");
                model.Direcciones = new SelectList(listadoDirecciones, "Value", "Text");
                model.Secciones = new SelectList(listadoSecciones, "Value", "Text");
                model.Clases = new SelectList(listadoClases, "Value", "Text");
                model.Especialidades = new SelectList(listadoEspecialidades, "Value", "Text");
                model.Subespecialidades = new SelectList(listadoSubespecialidades, "Value", "Text");
                model.Categorias = new SelectList(listadoCategorias, "Value", "Text");
                model.Porcentajes = new SelectList(listadoPorcentajes, "Value", "Text");
                model.Meses = new SelectList(listadoMeses, "Value", "Text");
                model.Annios = new SelectList(listadoAnios, "Value", "Text");

                if (error.Message == "Busqueda")
                {
                    var mensaje = "";
                    foreach(var modelError in ModelState.SelectMany(keyValuePair => keyValuePair.Value.Errors))
                    {
                        mensaje += modelError.ErrorMessage + "\n";
                    }

                    model.Error = new CErrorDTO { MensajeError = mensaje };
                    return PartialView("_FormularioAccionPersonal", model);
                    //return JavaScript("alert(" + 1 + ");");
                    //return new JavaScriptResult("MostrarMensajeError('" + mensaje + "');");
                    //return PartialView("_ErrorAccion");
                }
                else
                {
                    ModelState.AddModelError("Busqueda", error.Message + " Cont 4");
                    return PartialView("_FormularioAccionPersonal", model);
                    //return new JavaScriptResult("MostrarMensajeError('" + error.Message + " Cont 4" + "');");
                    //return PartialView("_ErrorAccion");
                }
            }
        }


        public ActionResult Edit(string numAccion)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null)
                {
                    FormularioAccionPersonalVM model = new FormularioAccionPersonalVM();

                    var datos = servicioAccion.ObtenerAccion(numAccion);

                    if (datos.Count() > 1)
                    {
                        model.Accion = (CAccionPersonalDTO)datos.ElementAt(0);
                        model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                        model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(2);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

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
        public ActionResult Edit(int codigo, FormularioAccionPersonalVM model, string SubmitButton)
        {
            try
            {
                if (model.Accion.Observaciones != null)
                {

                    var respuesta = servicioAccion.AnularAccion(model.Accion);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal),
                                Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                CAccesoWeb.ListarEntidades(typeof(CAccionPersonalDTO).Name));

                        //return JavaScript("window.location = '/AccionPersonal/Details?numAccion=" + model.Accion.NumAccion + "&accion=modificar';");
                        return new JavaScriptResult("ObtenerAnularDetalle('" + model.Accion.NumAccion + "');");
                    }
                    else
                    {
                        var msg = ((CErrorDTO)respuesta).MensajeError;
                        ModelState.AddModelError("modificar", msg);
                        throw new Exception(msg);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar las Observaciones para realizar esta acción");
                    throw new Exception();
                }
            }
            catch
            {
                return PartialView("_ErrorAccion");
                //return View(model);
            }
        }


        public ActionResult Aprobar(string numAccion)
        {

            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Administrador))] != null ||
                                         Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Consulta))] != null ||
                     Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null)
                {

                    FormularioAccionPersonalVM model = new FormularioAccionPersonalVM();

                    var datos = servicioAccion.ObtenerAccion(numAccion);

                    if (datos.Count() > 1)
                    {
                        model.Accion = (CAccionPersonalDTO)datos.ElementAt(0);
                        model.Estado = (CEstadoBorradorDTO)datos.ElementAt(1);
                        model.TipoAccion = (CTipoAccionPersonalDTO)datos.ElementAt(2);
                        model.Funcionario = (CFuncionarioDTO)datos.ElementAt(3);
                    }
                    else
                    {
                        model.Error = (CErrorDTO)datos.ElementAt(0);
                    }

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
        public ActionResult Aprobar(int codigo, FormularioAccionPersonalVM model)
        {
            try
            {
                if (model.Accion.Observaciones != null)
                {

                    var respuesta = servicioAccion.AprobarAccion(model.Accion);

                    if (respuesta.GetType() != typeof(CErrorDTO))
                    {

                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal),
                                Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                CAccesoWeb.ListarEntidades(typeof(CAccionPersonalDTO).Name));

                        //return RedirectToAction("Details", new { codigo = codigo, accion = "aprobar" });
                        return JavaScript("window.location = '/AccionPersonal/Details?numAccion=" + model.Accion.NumAccion + "&accion=aprobar" + "'");
                    }
                    else
                    {
                        ModelState.AddModelError("aprobar", respuesta.Mensaje);
                        throw new Exception(respuesta.Mensaje);
                    }
                }
                else
                {
                    ModelState.AddModelError("contenido", "Debe digitar una justificación para realizar esta acción");
                    throw new Exception();
                }
            }
            catch
            {
                return View(model);
            }
        }


        // GET: /AccionPersonal/Search
        public ActionResult Search()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Consulta))] != null)
                {

                    BusquedaAccionPersonalVM model = new BusquedaAccionPersonalVM();

                    var listadoTipos = servicioTipo.RetornarTipos()
                          .Select(Q => new SelectListItem
                          {
                              Value = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString(),
                              Text = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString() + " - " + ((CTipoAccionPersonalDTO)Q).DesTipoAccion
                          });

                    model.Tipos = new SelectList(listadoTipos, "Value", "Text");
                  
                    var listadoEstados = servicioEstado.RetornarEstados()
                         .Where(Q => ((CEstadoBorradorDTO)Q).IdEntidad == 3 || 
                                    ((CEstadoBorradorDTO)Q).IdEntidad == 7 || 
                                    ((CEstadoBorradorDTO)Q).IdEntidad == 8)
                         .Select(Q => new SelectListItem
                         {
                             Value = ((CEstadoBorradorDTO)Q).IdEntidad.ToString(),
                             Text = ((CEstadoBorradorDTO)Q).DesEstadoBorrador
                         });
                    
                    model.Estados = new SelectList(listadoEstados, "Value", "Text");
                  
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
        public ActionResult Search(BusquedaAccionPersonalVM model)
        {
            try
            {              
                if (!String.IsNullOrEmpty(model.Cedula) 
                    || !String.IsNullOrEmpty(model.Puesto.CodPuesto)
                    || !String.IsNullOrEmpty(model.Accion.NumAccion)
                    || (model.FechaRigeDesde.Year > 1 && model.FechaVenceHasta.Year > 1)
                    || (model.FechaRigeIntegraDesde.Year > 1 && model.FechaVenceIntegraHasta.Year > 1)
                    || !String.IsNullOrEmpty(model.EstadoSeleccionado) || model.TipoSeleccionado != 0)
                {
                    List<DateTime> fechas = new List<DateTime>();
                    List<DateTime> fechasIntegra = new List<DateTime>();

                    model.Funcionario = new CFuncionarioDTO
                    {
                        Cedula = model.Cedula,
                        Sexo = GeneroEnum.Indefinido
                    };

                    if (model.FechaRigeDesde.Year > 1 && model.FechaVenceHasta.Year > 1)
                    {
                        fechas.Add(model.FechaRigeDesde);
                        fechas.Add(model.FechaVenceHasta);
                    }

                    if (model.FechaRigeIntegraDesde.Year > 1 && model.FechaVenceIntegraHasta.Year > 1)
                    {
                        fechasIntegra.Add(model.FechaRigeIntegraDesde);
                        fechasIntegra.Add(model.FechaVenceIntegraHasta);
                    }


                    //  Tipo de Acción
                    if (Convert.ToInt32(model.TipoSeleccionado) > 0)
                    {
                        CTipoAccionPersonalDTO tipoAcc = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };
                        model.Accion.TipoAccion = tipoAcc;  
                    }


                    //  Estado
                    if (Convert.ToInt32(model.EstadoSeleccionado) > 0)
                    {
                        CEstadoBorradorDTO tipoEst = new CEstadoBorradorDTO
                        {
                            IdEntidad = Convert.ToInt32(model.EstadoSeleccionado)
                        };
                        model.Accion.Estado = tipoEst;
                    }

                    var datos = servicioAccion.BuscarAccion(model.Funcionario, model.Puesto, model.Accion, fechas.ToArray());


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.Acciones = new List<FormularioAccionPersonalVM>();

                        foreach (var item in datos)
                        {
                            FormularioAccionPersonalVM temp = new FormularioAccionPersonalVM();
                            temp.Accion = (CAccionPersonalDTO)item.ElementAt(0);
                            temp.Estado = (CEstadoBorradorDTO)item.ElementAt(1);
                            temp.TipoAccion = (CTipoAccionPersonalDTO)item.ElementAt(2);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(3);

                            model.Acciones.Add(temp);
                        }

                        return PartialView("_SearchResults", model.Acciones);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaRigeDesde.Year > 1 || model.FechaRigeHasta.Year > 1)
                    {
                        if (!(model.FechaRigeDesde.Year > 1 && model.FechaRigeHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Rige, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    if (model.FechaVenceDesde.Year > 1 || model.FechaVenceHasta.Year > 1)
                    {
                        if (!(model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados no son suficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorAccion");
                }
                else
                {
                    return PartialView("_ErrorAccion");
                }
            }
        }


        public ActionResult SearchProrroga()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Consulta))] != null)
                {

                    BusquedaAccionPersonalVM model = new BusquedaAccionPersonalVM();
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
        public ActionResult SearchProrroga(BusquedaAccionPersonalVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    if (model.FechaVenceHasta.Year > 1)
                    {
                        var datos = servicioAccion.BuscarAccionProrroga(model.FechaVenceHasta);

                        if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                            throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        }
                        else
                        {
                            model.Acciones = new List<FormularioAccionPersonalVM>();

                            foreach (var item in datos)
                            {
                                FormularioAccionPersonalVM temp = new FormularioAccionPersonalVM();
                                temp.Seleccionado = true;
                                temp.Accion = (CAccionPersonalDTO)item.ElementAt(0);
                                temp.Accion.FecUltRige = Convert.ToDateTime(temp.Accion.FecVence).AddDays(1);
                                temp.Accion.FecUltVence = Convert.ToDateTime(temp.Accion.FecUltRige).AddMonths(6);
                                temp.Estado = (CEstadoBorradorDTO)item.ElementAt(1);
                                temp.TipoAccion = (CTipoAccionPersonalDTO)item.ElementAt(2);
                                temp.Funcionario = (CFuncionarioDTO)item.ElementAt(3);

                                model.Acciones.Add(temp);
                            }

                            /*  Buscar los Nombramientos que vencen en esa fecha.                              
                             */

                            return PartialView("_SearchResultsProrroga", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda", "Para realizar la búsqueda debe ingresar la fecha");
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    foreach (var item in model.Acciones.Where(Q => Q.Seleccionado).ToList())
                    {
                        model.Funcionario = new CFuncionarioDTO
                        {
                            IdEntidad = item.Funcionario.IdEntidad,
                            Cedula = item.Funcionario.Cedula,
                            Sexo = GeneroEnum.Indefinido
                        };

                        model.Accion = new CAccionPersonalDTO
                        {
                            NumAccion = item.Accion.NumAccion,
                            AnioRige = Convert.ToDateTime(item.Accion.FecUltRige).Year,
                            FecRige = Convert.ToDateTime(item.Accion.FecUltRige),
                            FecVence = Convert.ToDateTime(item.Accion.FecUltVence),
                            FecRigeIntegra = Convert.ToDateTime(item.Accion.FecUltRige),
                            FecVenceIntegra = Convert.ToDateTime(item.Accion.FecUltVence),
                            Observaciones = item.Accion.Observaciones,
                            CodigoModulo = item.Accion.CodigoModulo,
                            CodigoObjetoEntidad = item.Accion.CodigoObjetoEntidad,
                            IndDato = item.Accion.IndDato
                        };
                   
                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                        {
                            IdEntidad = 3 // En Registro
                        };

                        CTipoAccionPersonalDTO tipo = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(model.TipoSeleccionado)
                        };

                        

                       // var respuesta = servicioAccion.AgregarAccion(model.Funcionario,
                       //                                             estado,
                       //                                             tipo,
                       //                                             model.Accion,
                       //                                             null);

                       // if (respuesta.GetType() != typeof(CErrorDTO))
                       // {
                       //     if (((CRespuestaDTO)respuesta).Codigo > 0)
                       //     {
                       //         List<string> entidades = new List<string>();
                       //         entidades.Add(typeof(CAccionPersonalDTO).Name);

                       //         context.GuardarBitacora(principal.Name, Convert.ToInt32(EModulosHelper.AccionPersonal),
                       //                 Convert.ToInt32(EAccionesBitacora.Guardar), Convert.ToInt32(((CRespuestaDTO)respuesta).Contenido),
                       //                 CAccesoWeb.ListarEntidades(entidades.ToArray()));
                       //     }
                       //}

                    }
                    return null;
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorAccion");
                }
                else
                {
                    return PartialView("_ErrorAccion");
                }
            }
        }

        // GET: /AccionPersonal/BuscarH
        public ActionResult BuscarH()
        {
            // var a = "MOPT\\";
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.AccionPersonal), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.AccionPersonal)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.AccionPersonal) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.AccionPersonal)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Operativo))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.AccionPersonal, Convert.ToInt32(ENivelesAccionPersonal.Consulta))] != null)
                {

                    BusquedaAccionPersonalVM model = new BusquedaAccionPersonalVM();

                    var listadoTipos = servicioTipo.RetornarTipos()
                          .Select(Q => new SelectListItem
                          {
                              Value = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString(),
                              Text = ((CTipoAccionPersonalDTO)Q).IdEntidad.ToString() + " - " + ((CTipoAccionPersonalDTO)Q).DesTipoAccion
                          });

                    model.Tipos = new SelectList(listadoTipos, "Value", "Text");

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
        public ActionResult BuscarH(BusquedaAccionPersonalVM model, string codclase)
        {
            try
            {
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                if (!String.IsNullOrEmpty(model.Funcionario.Cedula) || !String.IsNullOrEmpty(model.Accion.NumAccion)
                    || (model.FechaRigeDesde.Year > 1 && model.FechaRigeHasta.Year > 1)
                    //|| (model.FechaRigeIntegraDesde.Year > 1 && model.FechaVenceIntegraHasta.Year > 1)
                    || !String.IsNullOrEmpty(model.Accion.Nombramiento.Puesto.CodPuesto) || model.TipoSeleccionado != 0 || !String.IsNullOrEmpty(codclase))
                {
                    List<DateTime> fechas = new List<DateTime>();

                    if (model.FechaRigeDesde.Year > 1 && model.FechaRigeHasta.Year > 1)
                    {
                        fechas.Add(model.FechaRigeDesde);
                        fechas.Add(model.FechaRigeHasta);
                    }

                    CAccionPersonalHistoricoDTO hist = new CAccionPersonalHistoricoDTO
                    {
                        NumAccion = model.Accion.NumAccion,
                        Cedula = model.Funcionario.Cedula,
                        CodAccion = model.TipoSeleccionado,
                        CodPuesto = model.Accion.Nombramiento.Puesto.CodPuesto,
                        CodClase = String.IsNullOrEmpty(codclase) ? "" : codclase.Split('-')[0]
                    };

                    var datos = servicioAccion.BuscarHistorial(hist, fechas.ToArray());


                    if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.Acciones = new List<FormularioAccionPersonalVM>();

                        foreach (var item in datos)
                        {
                            FormularioAccionPersonalVM temp = new FormularioAccionPersonalVM();
                            temp.Historico = (CAccionPersonalHistoricoDTO)item;

                            var tipo = servicioTipo.ObtenerTipo(Convert.ToInt32(temp.Historico.CodAccion));
                            temp.TipoAccion = (CTipoAccionPersonalDTO)tipo[0];
                            model.Acciones.Add(temp);
                        }

                        return PartialView("_BuscarResults", model.Acciones);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaRigeDesde.Year > 1 || model.FechaRigeHasta.Year > 1)
                    {
                        if (!(model.FechaRigeDesde.Year > 1 && model.FechaRigeHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Rige, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    //if (model.FechaVenceDesde.Year > 1 || model.FechaVenceHasta.Year > 1)
                    //{
                    //    if (!(model.FechaVenceDesde.Year > 1 && model.FechaVenceHasta.Year > 1))
                    //    {
                    //        ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Vencimiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
                    //    }
                    //}
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorAccion");
                }
                else
                {
                    return PartialView("_ErrorAccion");
                }
            }
        }

        public PartialViewResult Clase_Index(string codigoclase, string nomclase, int? page)
        {
            Session["errorF"] = null;
            try
            {
                ClaseModel modelo = new ClaseModel();

                int paginaActual = page.HasValue ? page.Value : 1;

                if (String.IsNullOrEmpty(codigoclase) && String.IsNullOrEmpty(nomclase))
                {
                    return PartialView();
                }
                else
                {
                    modelo.CodigoSearch = codigoclase;
                    modelo.NombreSearch = nomclase;
                    int codigoClase = String.IsNullOrEmpty(modelo.CodigoSearch) ? 0 : Convert.ToInt32(modelo.CodigoSearch);
                    var clases =
                        servicioPuesto.BuscarClaseParams(codigoClase, modelo.NombreSearch);
                    modelo.TotalClases = clases.Count();
                    modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalClases / 10);
                    modelo.PaginaActual = paginaActual;
                    if ((((paginaActual - 1) * 10) + 10) > modelo.TotalClases)
                    {
                        modelo.Clase = clases.ToList().GetRange(((paginaActual - 1) * 10), (modelo.TotalClases) - (((paginaActual - 1) * 10))).ToList(); ;
                    }
                    else
                    {
                        modelo.Clase = clases.ToList().GetRange(((paginaActual - 1) * 10), 10).ToList(); ;
                    }

                    return PartialView("Clase_Index_Result", modelo);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Busqueda", "Ha ocurrido un error a la hora de realizar la búsqueda, ponerse en contacto con el personal autorizado. \n\n");
                return PartialView("_ErrorPuesto");
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleAccion(FormularioAccionPersonalVM model)
        {
            string reportPath = "0";
            try
            {
                List<AccionRptData> modelo = new List<AccionRptData>();

                modelo.Add(AccionRptData.GenerarDatosReporteDetalle(model, String.Empty));

                reportPath = Path.Combine(Server.MapPath("~/Reports/AccionPersonal"), "AccionRpt.rpt");

                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(reportPath + " " + ex.InnerException.Message);
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleHistorico(FormularioAccionPersonalVM model)
        {
            string reportPath = "0";
            try
            {
                List<AccionRptData> modelo = new List<AccionRptData>();

                modelo.Add(AccionRptData.GenerarDatosHistoricoDetalle(model, String.Empty));

                reportPath = Path.Combine(Server.MapPath("~/Reports/AccionPersonal"), "DetalleHistoricoRPT.rpt");

                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            catch (Exception ex)
            {
                throw new Exception(reportPath + " " + ex.InnerException.Message);
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteBorradores(List<FormularioAccionPersonalVM> model)
        {
            string reportPath = "";
            List<AccionRptData> modelo = new List<AccionRptData>();

            foreach (var item in model)
            {
                modelo.Add(AccionRptData.GenerarDatosReporte(item, String.Empty));
            }

            reportPath = Path.Combine(Server.MapPath("~/Reports/AccionPersonal"), "ReporteAccionesRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteHistorico(List<FormularioAccionPersonalVM> model)
        {
            List<AccionRptData> modelo = new List<AccionRptData>();

            foreach (var item in model)
            {
                modelo.Add(AccionRptData.GenerarDatosReporteHistorico(item, String.Empty));
            }
            string reportPath = Path.Combine(Server.MapPath("~/Reports/AccionPersonal"), "ReporteHistoricoRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "EXCEL");
        }

        [HttpPost]
        public ActionResult GetCategoria(int? idCategoria)
        {
            try
            {
                if (idCategoria != null)
                {
                    var datos = servicioPuesto.BuscarEscalaCategoria(Convert.ToInt16(idCategoria));
                    var categoria = (CEscalaSalarialDTO)datos.ElementAt(0);
                    return Json(new { success = true, salario = categoria.SalarioBase, aumento = categoria.MontoAumentoAnual }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, salario = "0", aumento = "0 ", mensaje = "No existe la Categoría" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, salario = "0", aumento = "0 ", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetClaseCategoria(int? idClase)
        {
            try
            {
                if (idClase != null)
                {
                    var datos = (CRespuestaDTO)servicioAccion.ObtenerCategoriaClase(Convert.ToInt16(idClase));
                    var categoria = Convert.ToDecimal(datos.Contenido);
                    return Json(new { success = true, categoria = categoria}, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, categoria = "0", mensaje = "No existe la Categoría" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                return Json(new { success = false, categoria = "0", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetFechas(int? idTipoAccion, string numCedula)
        {
            try
            {
                var datos = servicioAccion.ObtenerAccionProrroga(Convert.ToInt16(idTipoAccion), numCedula.ToString());

                if (!datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                {
                    var accion = (CAccionPersonalDTO)datos.ElementAt(0);
                    return Json(new
                    {
                        success = true,
                        id = accion.IdEntidad,
                        numAcc = accion.NumAccion,
                        fecR = accion.FecRige.ToShortDateString(),
                        fecV = ( ! accion.FecVence.ToString().Contains("01/01/0001"))  ? DateTime.Parse(accion.FecVence.ToString()).ToShortDateString() : "",
                        fecRI = (! accion.FecVence.ToString().Contains("01/01/0001")) ? DateTime.Parse(accion.FecVence.ToString()).AddDays(1).ToShortDateString() : "",
                        fecVI = ( ! accion.FecVence.ToString().Contains("01/01/0001")) ? DateTime.Parse(accion.FecVence.ToString()).AddDays(2).ToShortDateString() : "",
                        codObj = accion.CodigoObjetoEntidad
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, id = "", numAcc = "", fecR = "", fecV = "", fecRI = "", fecVI = "", mensaje = "No existe Acción de Personal relacionada" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception error)
            {
                return Json(new { success = false, id = "", numAcc = "", fecR = "", fecV = "", fecRI = "", fecVI = "", mensaje = error.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}