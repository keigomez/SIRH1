using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.NombramientoLocal;
//using SIRH.Web.NombramientoService;
//using SIRH.Web.PuestoService;
using SIRH.Web.PuestoLocal;
using SIRH.DTO;
using SIRH.Web.ViewModels;
using SIRH.Web.Helpers;
using SIRH.Web.AccionPersonalLocal;
//using SIRH.Web.AccionPersonalService;
using SIRH.Web.UserValidation;
using System.Security.Principal;

namespace SIRH.Web.Controllers
{
    public class NombramientoController : Controller
    {
        //Comentario para subirlo.
        CPuestoServiceClient puestoService = new CPuestoServiceClient();
        CNombramientoServiceClient nombramientoService = new CNombramientoServiceClient();
        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
        //
        // GET: /Nombramiento/

        public ActionResult Index()
        {
            string a = principal.Identity.Name;
            return View();
        }

        //
        // GET: /Nombramiento/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Nombramiento/Create

        public ActionResult Create()
        {
            FormularioNombramientoVM model = new FormularioNombramientoVM();

            var motivosMovimiento = puestoService.ListarMotivosMovimiento()
                .Where(Q => (Q.IdEntidad >= 2 && Q.IdEntidad <= 6) || Q.IdEntidad == 8 || Q.IdEntidad == 9 || Q.IdEntidad == 10
                        || Q.IdEntidad == 11 || Q.IdEntidad == 13 || Q.IdEntidad == 14 || Q.IdEntidad == 15 || Q.IdEntidad == 16
                        || Q.IdEntidad == 19 || Q.IdEntidad == 20 || Q.IdEntidad == 12 || Q.IdEntidad == 22)
                .Select(Q => new SelectListItem
                {
                    Value = ((CMotivoMovimientoDTO)Q).IdEntidad.ToString(),
                    Text = ((CMotivoMovimientoDTO)Q).DesMotivo.ToString()
                });

            model.MotivosMovimiento = new SelectList(motivosMovimiento, "Value", "Text");

            return View(model);
        }

        //
        // POST: /Nombramiento/Create

        [HttpPost]
        public ActionResult Create(CNombramientoDTO model)
        {
            string submitButton = "a";
            try
            {
                switch (submitButton)
                {
                    case "a":
                        CFuncionarioDTO funcionario = new CFuncionarioDTO
                        {
                            Nombre = "Deivert",
                            PrimerApellido = "Guiltrichs"
                        };
                        model.Funcionario = funcionario;
                        return View(model);
                    default:
                        break;
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Descarga la información del funcionario
        // POST: /Nombramiento/Create <<Funcionario>>

        [HttpPost]
        public PartialViewResult CargarInformacionFuncionario(CNombramientoDTO model)
        {
            CFuncionarioDTO modelFuncionario = new CFuncionarioDTO();
            modelFuncionario = model.Funcionario;
            return PartialView(modelFuncionario);
        }

        //
        // GET: /Nombramiento/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Nombramiento/Edit/5

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
        // GET: /Nombramiento/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Nombramiento/Delete/5

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

        //
        // GET: /Nombramiento/CreateNombramiento
        public ActionResult CreateNombramiento()
        {
            FormularioNombramientoVM modelo = new FormularioNombramientoVM();
            modelo.Funcionario = new CFuncionarioDTO();
            modelo.Puesto = new CPuestoDTO();
            return View(modelo);
        }

        //
        // GET: /Nombramiento/CreateNombramiento
        [HttpPost]
        public ActionResult CreateNombramiento(FormularioNombramientoVM modelo)
        {
            try
            {
                ListaFormularioNombramientoVM modeloResultado = new ListaFormularioNombramientoVM();
                modeloResultado.Lista = new List<FormularioNombramientoVM>();
                if(modelo.Funcionario.Cedula == null)
                {
                    modelo.Funcionario.Cedula = "0019960876";
                }
                if (modelo.Puesto.CodPuesto != null && modelo.Funcionario.Cedula != null)
                {
                    var resultado = nombramientoService.BuscarDatosRegistroNombramiento(modelo.Puesto.CodPuesto, modelo.Funcionario.Cedula);
                    if (resultado.ElementAt(0).ElementAt(0).GetType() == typeof(CErrorDTO))
                    {
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)resultado.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception("Busqueda");
                    }
                    else
                    {
                        var motivosMovimiento = nombramientoService.ListarEstadosNombramiento()
                                                .Where(Q => Q.IdEntidad == 1 || Q.IdEntidad == 2 || Q.IdEntidad == 5 || Q.IdEntidad == 9 || Q.IdEntidad == 11
                                                    || Q.IdEntidad == 12 || Q.IdEntidad == 13 || Q.IdEntidad == 16 || Q.IdEntidad == 18 || Q.IdEntidad == 19
                                                    || Q.IdEntidad == 20 || Q.IdEntidad == 21 || Q.IdEntidad == 22 || Q.IdEntidad == 23 || Q.IdEntidad == 24
                                                     || Q.IdEntidad == 26 || Q.IdEntidad == 27 || (Q.IdEntidad >= 28 && Q.IdEntidad <= 39))
                                                .Select(Q => new SelectListItem
                                                {
                                                Value = ((CEstadoNombramientoDTO)Q).IdEntidad.ToString(),
                                                Text = ((CEstadoNombramientoDTO)Q).DesEstado.ToString()
                                                });

                        FormularioNombramientoVM datoPuesto = new FormularioNombramientoVM();
                        datoPuesto.DetalleAccion = new CDetalleAccionPersonalDTO();
                        if (resultado.ElementAt(0).Count() > 2)
                        {
                            datoPuesto.Nombramiento = (CNombramientoDTO)resultado.ElementAt(0).ElementAt(0);
                            datoPuesto.Puesto = (CPuestoDTO)resultado.ElementAt(0).ElementAt(1);
                            datoPuesto.DetallePuesto = (CDetallePuestoDTO)resultado.ElementAt(0).ElementAt(2);
                            datoPuesto.Funcionario = (CFuncionarioDTO)resultado.ElementAt(0).ElementAt(3);
                            datoPuesto.DetalleContratacion = (CDetalleContratacionDTO)resultado.ElementAt(0).ElementAt(4);
                            if (!datoPuesto.Puesto.EstadoPuesto.DesEstadoPuesto.Contains("Traslado interino"))
                            {
                                datoPuesto.ListaWarnings = new List<CErrorDTO>
                                {
                                    new CErrorDTO { MensajeError = "El puesto indicado actualmente tiene un funcionario asignado" },
                                    new CErrorDTO { MensajeError = "Para poder realizar el nombramiento con lo datos ingresados, primero se debe liberar el puesto" }
                                };
                            }
                            datoPuesto.DetalleAccion.MtoTotal = (datoPuesto.DetallePuesto.EscalaSalarial.SalarioBase
                                     + (datoPuesto.DetallePuesto.EscalaSalarial.MontoAumentoAnual
                                     * ((CDetalleContratacionDTO)resultado.ElementAt(0).ElementAt(4)).NumeroAnualidades));
                        }
                        else
                        {
                            datoPuesto.Puesto = (CPuestoDTO)resultado.ElementAt(0).ElementAt(0);
                            datoPuesto.DetallePuesto = (CDetallePuestoDTO)resultado.ElementAt(0).ElementAt(1);
                            datoPuesto.ListaWarnings = new List<CErrorDTO>
                            {
                                new CErrorDTO { MensajeError = "El puesto indicado no presenta ningún nombramiento activo. Por favor verifique su estado antes de continuar para asegurarse de que el puesto puede ser asignado a un funcionario" }
                            };
                            datoPuesto.DetalleAccion.MtoTotal = (datoPuesto.DetallePuesto.EscalaSalarial.SalarioBase
                                     + (datoPuesto.DetallePuesto.EscalaSalarial.MontoAumentoAnual
                                     * 0));
                        }

                        datoPuesto.MotivosMovimiento = new SelectList(motivosMovimiento, "Value", "Text");


                        datoPuesto.NombramientoNuevo = new CNombramientoDTO();
                        modeloResultado.Lista.Add(datoPuesto);

                        FormularioNombramientoVM datoFuncionario = new FormularioNombramientoVM();
                        if (resultado.ElementAt(1).Count() > 2)
                        {
                            datoFuncionario.Nombramiento = (CNombramientoDTO)resultado.ElementAt(1).ElementAt(0);
                            datoFuncionario.Puesto = (CPuestoDTO)resultado.ElementAt(1).ElementAt(1);
                            datoFuncionario.DetallePuesto = (CDetallePuestoDTO)resultado.ElementAt(1).ElementAt(2);
                            datoFuncionario.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(3);
                            datoFuncionario.DetalleContratacion = (CDetalleContratacionDTO)resultado.ElementAt(1).ElementAt(4);
                            datoFuncionario.ListaWarnings = new List<CErrorDTO>
                            {
                                new CErrorDTO { MensajeError = "El funcionario indicado actualmente se encuentra ocupando otro puesto" },
                                new CErrorDTO { MensajeError = "Si continúa con el nombramiento, se liberará el puesto que el funcionario ocupa actualmente" }
                            };
                            if (datoFuncionario.Nombramiento.EstadoNombramiento.IdEntidad == 9)
                            {
                                datoFuncionario.ListaWarnings.Add(new CErrorDTO { MensajeError = "El funcionario actualmente se encuentra con un ascenso interino, por lo que inicialmente se le debe aplicar un regreso a su puesto en propiedad" });
                            }
                        }
                        else
                        {
                            datoFuncionario.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1).ElementAt(0);
                            datoFuncionario.DetalleContratacion = (CDetalleContratacionDTO)resultado.ElementAt(1).ElementAt(1);
                            datoFuncionario.ListaWarnings = new List<CErrorDTO>
                            {
                                new CErrorDTO { MensajeError = "El funcionario indicado no presenta ningún nombramiento activo. Por favor verifique su estado antes de continuar para asegurarse de que puede asignarse al puesto indicado" }
                            };
                        }

                        if (datoPuesto.Nombramiento != null && datoPuesto.Nombramiento.EstadoNombramiento.DesEstado == "Ad Honorem")
                        {
                            datoPuesto.ListaWarnings = new List<CErrorDTO>
                            {
                                new CErrorDTO { MensajeError = "El puesto indicado tiene un nombramiento Ad Honorem, por favor verifique adecuadamente sus datos antes de continuar" }
                            };
                        }

                        if (datoPuesto.Puesto != null && datoFuncionario.Puesto != null && datoPuesto.Puesto.CodPuesto == datoFuncionario.Puesto.CodPuesto
                            && datoFuncionario.Funcionario.EstadoFuncionario.DesEstadoFuncionario.Contains("interino a otra institución"))
                        {
                            datoPuesto.ListaWarnings = new List<CErrorDTO>
                            {
                                new CErrorDTO { MensajeError = "Está a punto de nombrar a un funcionario en el mismo puesto que ya ocupa actualmente, por favor verifique adecuadamente los datos antes de continuar" }
                            };
                        }

                        modeloResultado.Lista.Add(datoFuncionario);

                        return PartialView("_CreateNombramientoResult", modeloResultado);
                    }
                }
                else
                {
                    ModelState.AddModelError("Busqueda", "Debe digitar tanto la cédula como el número de puesto, para registrar el nombramiento");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    ModelState.AddModelError("Busqueda", error.Message);
                }
                else
                {
                    ModelState.AddModelError("Otro", error.Message);
                }
                return PartialView("_ErrorNombramiento");
            }
        }

        [HttpPost]
        public ActionResult GuardarNombramiento(ListaFormularioNombramientoVM model)
        {
            try
            {
                bool error = false;
                ModelState.Clear();
                if (model.Lista.ElementAt(0).MotivoSeleccionado == 0)
                {
                    error = true;
                    ModelState.AddModelError("Guardar", "Debe seleccionar el tipo de nombramiento a registrar");
                }
                if (model.Lista.ElementAt(0).CodOficio == "" || model.Lista.ElementAt(0).CodOficio == null)
                {
                    error = true;
                    ModelState.AddModelError("Guardar", "Debe digitar el número de oficio de respaldo para el movimiento");
                }
                if (model.Lista.ElementAt(0).Explicacion == "" || model.Lista.ElementAt(0).Explicacion == null)
                {
                    error = true;
                    ModelState.AddModelError("Guardar", "Debe ingresar la explicación de respaldo del nombramiento");
                }
                if (model.Lista.ElementAt(0).NombramientoNuevo.FecRige.Year == 1)
                {
                    error = true;
                    ModelState.AddModelError("Guardar", "Debe digitar la fecha de rige del Nombramiento");
                }
                if (model.Lista.ElementAt(0).NombramientoNuevo.FecVence.Year == 1
                    && (model.Lista.ElementAt(0).MotivoSeleccionado == 2
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 28
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 9
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 18
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 23
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 26
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 35
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 36
                        || model.Lista.ElementAt(0).MotivoSeleccionado == 39))
                {
                    error = true;
                    ModelState.AddModelError("Guardar", "Debe digitar la fecha de vence del Nombramiento");
                }
                else
                {
                    if (model.Lista.ElementAt(0).NombramientoNuevo.FecRige > model.Lista.ElementAt(0).NombramientoNuevo.FecVence
                        && model.Lista.ElementAt(0).NombramientoNuevo.FecVence.Year > 1)
                    {
                        error = true;
                        ModelState.AddModelError("Guardar", "La fecha de vencimiento del nombramiento no puede ser inferior a la fecha de rige");
                    }
                }

                if (!error)
                {
                    model.Lista.ElementAt(1).Funcionario.Sexo = GeneroEnum.Indefinido;
                    model.Lista.ElementAt(1).Funcionario.EstadoFuncionario.IdEntidad = 1;
                    if (model.Lista.ElementAt(1).Nombramiento != null)
                    {
                        model.Lista.ElementAt(1).Funcionario.Mensaje = model.Lista.ElementAt(1).Nombramiento.IdEntidad.ToString();
                    }
                    model.Lista.ElementAt(0).Puesto.EstadoPuesto.IdEntidad = DefinirMotivoParaPuestoNombrado(model.Lista.ElementAt(0).MotivoSeleccionado);

                    CNombramientoDTO NombramientoProceso = new CNombramientoDTO
                    {
                        EstadoNombramiento = new CEstadoNombramientoDTO { IdEntidad = model.Lista.ElementAt(0).MotivoSeleccionado },
                        FecNombramiento = DateTime.Now,
                        FecRige = model.Lista.ElementAt(0).NombramientoNuevo.FecRige,
                        FecVence = model.Lista.ElementAt(0).NombramientoNuevo.FecVence,
                        Funcionario = model.Lista.ElementAt(1).Funcionario,
                        Puesto = model.Lista.ElementAt(0).Puesto
                    };

                    CBaseDTO resultadoNombramiento = null;
                    CBaseDTO resultadoMovimiento = null;
                    CBaseDTO resultadoMovimientoSegundoPuesto = null;

                    CMovimientoPuestoDTO movimiento = new CMovimientoPuestoDTO
                    {
                        CodOficio = model.Lista.ElementAt(0).CodOficio,
                        Explicacion = model.Lista.ElementAt(0).Explicacion,
                        FecMovimiento = NombramientoProceso.FecRige,
                        FechaVencimiento = NombramientoProceso.FecVence,
                        MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = DeterminarMotivoPorNombramiento(model.Lista.ElementAt(0).MotivoSeleccionado) }, //AQUÍ HAY QUE CONVERTIR AL MOTIVO CORRECTO SEGÚN EL NOMB.
                        Puesto = new CPuestoDTO { CodPuesto = model.Lista.ElementAt(0).Puesto.CodPuesto }
                    };

                    if (model.Lista.ElementAt(1).Puesto != null && model.Lista.ElementAt(1).Puesto.CodPuesto != null)
                    {
                        model.Lista.ElementAt(1).Puesto.EstadoPuesto.IdEntidad = DefinirEstadoPuestoLiberado(model.Lista.ElementAt(0).MotivoSeleccionado);
                        //resultadoNombramiento = new CBaseDTO { IdEntidad = 1 };
                        resultadoNombramiento = nombramientoService.GuardarNombramiento(NombramientoProceso, model.Lista.ElementAt(1).Puesto);
                        if (resultadoNombramiento.IdEntidad < 0)
                        {
                            throw new Exception(((CErrorDTO)resultadoNombramiento).MensajeError);
                        }
                        else
                        {
                            resultadoMovimiento = puestoService.InsertarMovimientoPuesto(movimiento);
                            if (model.Lista.ElementAt(0).Puesto.CodPuesto != model.Lista.ElementAt(1).Puesto.CodPuesto)
                            {
                                CMovimientoPuestoDTO movimientoPuestoLiberado = new CMovimientoPuestoDTO
                                {
                                    CodOficio = model.Lista.ElementAt(0).CodOficio,
                                    Explicacion = "Vacante por ",
                                    FecMovimiento = NombramientoProceso.FecRige,
                                    FechaVencimiento = NombramientoProceso.FecVence,
                                    MotivoMovimiento = new CMotivoMovimientoDTO { IdEntidad = DeterminarMotivoPorNombramiento(model.Lista.ElementAt(0).MotivoSeleccionado) }, //AQUÍ HAY QUE CONVERTIR AL MOTIVO CORRECTO SEGÚN EL NOMB.
                                    Puesto = new CPuestoDTO { CodPuesto = model.Lista.ElementAt(1).Puesto.CodPuesto }
                                };
                                resultadoMovimientoSegundoPuesto = puestoService.InsertarMovimientoPuesto(movimientoPuestoLiberado);
                            }
                        }
                    }
                    else
                    {
                        //resultadoNombramiento = new CBaseDTO { IdEntidad = 1 };
                        resultadoNombramiento = nombramientoService.GuardarNombramiento(NombramientoProceso, null);
                        if (resultadoNombramiento.IdEntidad < 0)
                        {
                            throw new Exception(((CErrorDTO)resultadoNombramiento).MensajeError);
                        }
                        else
                        {
                            puestoService.InsertarMovimientoPuesto(movimiento);
                        }
                    }

                    //Enviar estos estados para actualizar la información del funcionario y los puestos
                    //Validar en caso de que el funcionario no tenga puesto que liberar

                    //guardar nombramiento debería incluir el puesto a liberar


                    if (resultadoNombramiento.IdEntidad < 0)
                    {
                        throw new Exception(((CErrorDTO)resultadoNombramiento).MensajeError);
                    }
                    else
                    {
                        CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                        {
                            IdEntidad = 1 // Registrado
                        };

                        CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                        {
                            IdEntidad = Convert.ToInt32(DefinirMotivoParaAccion(model.Lista.ElementAt(0).MotivoSeleccionado))
                        };

                        CAccionPersonalDTO accion = new CAccionPersonalDTO
                        {
                            AnioRige = DateTime.Now.Year,
                            CodigoModulo = Convert.ToInt32(EModulosHelper.Vacantes),
                            CodigoObjetoEntidad = resultadoNombramiento.IdEntidad,
                            FecRige = NombramientoProceso.FecRige,
                            FecVence = NombramientoProceso.FecVence.Year == 1 ? (DateTime?)null : NombramientoProceso.FecVence,
                            FecRigeIntegra = NombramientoProceso.FecRige,
                            FecVenceIntegra = NombramientoProceso.FecVence.Year == 1 ? (DateTime?)null : NombramientoProceso.FecVence,
                            Observaciones = model.Lista.ElementAt(0).TextoMotivo + ". " + model.Lista.ElementAt(0).Explicacion
                            
                        };

                        if (model.Lista.ElementAt(1).Nombramiento != null)
                        {
                            accion.Nombramiento = model.Lista.ElementAt(1).Nombramiento;
                        }

                        //var respuestaAP = servicioAccion.AgregarAccion(model.Lista.ElementAt(1).Funcionario, //aquí tengo que enviar el funcionario para que haga la acción de personal
                        //                                 estado,
                        //                                 tipoAP,
                        //                                 accion,
                        //                                 null);

                        model.Lista.ElementAt(0).DetallePuesto.Puesto = model.Lista.ElementAt(0).Puesto;
                        model.Lista.ElementAt(0).DetallePuesto.FecRige = NombramientoProceso.FecRige;
                        model.Lista.ElementAt(0).DetallePuesto.FK_Nombramiento = resultadoNombramiento.IdEntidad;
                        var respuestaDetalleAP = servicioAccion.AgregarRubro(model.Lista.ElementAt(0).DetallePuesto,
                                                                             model.Lista.ElementAt(0).DetalleAccion);

                        if (respuestaDetalleAP.GetType() != typeof(CErrorDTO))
                        {
                            //model.Lista.ElementAt(0).DetallePuesto.Puesto = model.Lista.ElementAt(0).Puesto;
                            //model.Lista.ElementAt(0).DetallePuesto.FecRige = NombramientoProceso.FecRige;
                            //model.Lista.ElementAt(0).DetallePuesto.FK_Nombramiento = resultadoNombramiento.IdEntidad;
                            //var respuestaDetalleAP = servicioAccion.AgregarRubro(model.Lista.ElementAt(0).DetallePuesto,
                            //                                                     model.Lista.ElementAt(0).DetalleAccion);

                            var respuestaAP = servicioAccion.AgregarAccion(model.Lista.ElementAt(1).Funcionario, //aquí tengo que enviar el funcionario para que haga la acción de personal
                                                             estado,
                                                             tipoAP,
                                                             accion,
                                                             null);

                            if (respuestaAP.GetType() != typeof(CErrorDTO))
                            {
                                if (model.Lista.ElementAt(1).Puesto != null && model.Lista.ElementAt(1).Puesto.CodPuesto != null)
                                {
                                    return JavaScript("window.location = '/Vacantes/Vacante_Prepare?codPuesto=" + model.Lista.ElementAt(0).Puesto.CodPuesto + "_" + model.Lista.ElementAt(1).Puesto.CodPuesto + "'");
                                }
                                else
                                {
                                    return JavaScript("window.location = '/Vacantes/Vacante_Prepare?codPuesto=" + model.Lista.ElementAt(0).Puesto.CodPuesto + "'");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Guardar", "La acción de personal asociada a este nombramiento no pudo guardarse. Contacte al administrador del sistema para reversar los datos y revisar los detalles del error");
                                // ModelState.AddModelError("Guardar", "No se pudieron guardar los rubros de la acción de personal, revise los detalles de la misma para verificar la información guardada");
                                throw new Exception("Guardar");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Guardar", "No se pudieron guardar los rubros de la acción de personal, revise los detalles de la misma para verificar la información guardada");
                            //ModelState.AddModelError("Guardar", "La acción de personal asociada a este nombramiento no pudo guardarse. Contacte al administrador del sistema para reversar los datos y revisar los detalles del error");
                            throw new Exception("Guardar");
                        }
                    }
                    //return JavaScript("window.location = '/Vacantes/Vacante_Prepare?codPuesto=" + model.Lista.ElementAt(0).Puesto.CodPuesto + "'");
                }
                else
                {
                    throw new Exception("Guardar");
                }
                //return View();
            }
            catch (Exception error)
            {
                ModelState.AddModelError("Guardar", error.Message + "web");
                return PartialView("_ErrorNombramiento");
            }
        }

        private int DeterminarMotivoPorNombramiento(int motivoSeleccionado)
        {
            switch (motivoSeleccionado)
            {
                case 1:
                    return 2;
                case 2:
                    return 36;
                case 11:
                    return 60;
                case 12:
                    return 61;
                case 13:
                    return 62;
                case 16:
                    return 59;
                case 9:
                    return 3;
                case 18:
                    return 57;
                case 19:
                    return 58;
                case 20:
                    return 23;
                case 21:
                    return 4;
                case 22:
                    return 8;
                case 23:
                    return 63;
                case 24:
                    return 37;
                case 26:
                    return 44;
                case 27:
                    return 64;
                case 28:
                    return 38;
                case 30:
                    return 41;
                case 33:
                    return 42;
                case 35:
                    return 43;
                case 36:
                    return 53;
                case 37:
                    return 54;
                case 38:
                    return 55;
                case 39:
                    return 56;
                case 5:
                default:
                    return 1;
            }
        }

        private ETipoAccionesHelper DefinirMotivoParaAccion(int motivo)
        {
            switch (motivo)
            {
                case 1:
                    return ETipoAccionesHelper.NombPropiedad;
                case 2:
                    return ETipoAccionesHelper.NombInterino;
                case 9:
                    return ETipoAccionesHelper.AscensoInterino;
                case 18:
                    return ETipoAccionesHelper.NombPlazoFijo;
                case 19:
                    return ETipoAccionesHelper.NombEspecial;
                case 20:
                    return ETipoAccionesHelper.RegresoTrabajo;
                case 21:
                    return ETipoAccionesHelper.AscensoPropiedad;
                case 22:
                    return ETipoAccionesHelper.DescensoPropiedad;
                case 23:
                    return ETipoAccionesHelper.DescensoInterino;
                case 24:
                case 30:
                case 33:
                case 37:
                    return ETipoAccionesHelper.Traslado;
                case 26:
                case 28:
                case 35:
                case 36:
                    return ETipoAccionesHelper.TrasladoInterino;
                case 27:
                    return ETipoAccionesHelper.RegresoPuestoPropiedad;
                case 38:
                    return ETipoAccionesHelper.ReinstalacionPropiedad;
                case 39:
                    return ETipoAccionesHelper.ReinstalacionInterino;
                default:
                    return ETipoAccionesHelper.RestructPuesto;
            }
        }

        private int DefinirEstadoPuestoLiberado(int motivo)
        {
            switch (motivo)
            {
                case 1:
                    return 24;
                case 2:
                    return 24;
                case 9:
                    return 7;
                case 18:
                    return 24;
                case 19:
                    return 24;
                case 20:
                    return 24;
                case 21:
                    return 24;
                case 22:
                    return 24;
                case 23:
                    return 8;
                case 24:
                    return 24;
                case 28:
                case 30:
                case 33:
                case 35:
                case 36:
                case 37:
                case 26:
                case 27:
                    return 24;
                case 38:
                case 39:
                    return 0;
                default:
                    return 0;
            }
        }

        private int DefinirMotivoParaPuestoNombrado(int motivo)
        {
            switch (motivo)
            {
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 9:
                    return 9;
                case 18:
                    return 3;
                case 19:
                    return 3;
                case 20:
                    return 2;
                case 21:
                    return 2;
                case 22:
                    return 2;
                case 23:
                    return 3;
                case 24:
                    return 2;
                case 26:
                case 28:
                case 35:
                case 36:
                case 39:
                    return 3;
                case 27:
                case 30:
                case 33:
                case 37:
                case 38:
                    return 2;
                default:
                    return 2;
            }
        }

        public ActionResult SearchHistorialNombramiento()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacantes)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacantes, Convert.ToInt32(ENivelesVacantes.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacantes, Convert.ToInt32(ENivelesVacantes.AdministradorVacantesGeneral))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchHistorialNombramiento(FormularioNombramientoVM model)
        {
            try
            {
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                if (model.Funcionario.Cedula != null || 
                    (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1) ||
                    model.Puesto.CodPuesto != null)
                {
                    List<DateTime> fechasEmision = new List<DateTime>();
                    List<DateTime> fechasVencimiento = new List<DateTime>();
                    if (model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1)
                    {
                        fechasEmision.Add(model.FechaEmisionDesde);
                        fechasEmision.Add(model.FechaEmisionHasta);
                    }

                    var datos = nombramientoService.BuscarHistorialNombramiento(model.Funcionario,
                                                                    fechasEmision.ToArray(), model.Puesto);

                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        var modelo = new List<FormularioNombramientoVM>();

                        foreach (var item in datos)
                        {
                            FormularioNombramientoVM temp = new FormularioNombramientoVM();
                            temp.Nombramiento = (CNombramientoDTO)item.ElementAt(0);
                            temp.Funcionario = (CFuncionarioDTO)item.ElementAt(1);
                            temp.Puesto = (CPuestoDTO)item.ElementAt(2);
                            modelo.Add(temp);
                        }

                        modelo = modelo.OrderByDescending(O => O.Nombramiento.FecRige).ToList();

                        return PartialView("_ResultadoHistorialNombramiento", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaEmisionDesde.Year > 1 || model.FechaEmisionHasta.Year > 1)
                    {
                        if (!(model.FechaEmisionDesde.Year > 1 && model.FechaEmisionHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de nombramiento, debe ingresar la fecha -desde- y la fecha -hasta-.");
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
                    return PartialView("_ErrorCaucion");
                }
                else
                {
                    return PartialView("_ErrorCaucion");
                }
            }
        }

        public ActionResult DetalleNombramiento(int codigo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Vacantes), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Vacantes)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Vacantes)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacantes, Convert.ToInt32(ENivelesVacantes.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Vacantes, Convert.ToInt32(ENivelesVacantes.AdministradorVacantesGeneral))] != null)
                {
                    FormularioNombramientoVM model = new FormularioNombramientoVM();

                    var resultado = nombramientoService.NombramientoPorCodigo(codigo);

                    if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        model.Nombramiento = (CNombramientoDTO)resultado.ElementAt(0);
                        model.Funcionario = (CFuncionarioDTO)resultado.ElementAt(1);
                        model.Puesto = (CPuestoDTO)resultado.ElementAt(2);
                        model.DetallePuesto = (CDetallePuestoDTO)resultado.ElementAt(3);
                        if (resultado.ElementAt(4).Mensaje == "SIRH")
                        {
                            model.DetalleAccion = new CDetalleAccionPersonalDTO { Accion = (CAccionPersonalDTO)resultado.ElementAt(4) };
                        }
                        else
                        {
                            model.AccionHistorica = (CAccionPersonalHistoricoDTO)resultado.ElementAt(4);
                        }
                        return View(model);
                    }
                    else
                    {
                        CAccesoWeb.CargarErrorSistema(((CErrorDTO)resultado.FirstOrDefault()).MensajeError, Session);
                        return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                    }
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Vacantes) });
                }
            }
        }

    }
}
