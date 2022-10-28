using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIRH.Web.ViewModels;
using SIRH.Web.FuncionarioLocal;
using SIRH.Web.FormacionAcademicaLocal;
using SIRH.Web.AccionPersonalLocal;
using SIRH.DTO;
using SIRH.Web.UserValidation;
using System.Security.Principal;
using System.Threading;
using SIRH.Web.Helpers;
using SIRH.Web.Reports.PDF;
using SIRH.Web.Reports.Carrera;
using System.IO;

namespace SIRH.Web.Controllers
{
    public class CarreraController : Controller
    {
        static bool ejecutado = false;
        static int grado = 0;
        static int capacitacion = 0;
        CFuncionarioServiceClient servicioFuncionario = new CFuncionarioServiceClient();
        CFormacionAcademicaServiceClient servicioCarrera = new CFormacionAcademicaServiceClient();
        CAccionPersonalServiceClient servicioAccion = new CAccionPersonalServiceClient();
        CAccesoWeb context = new CAccesoWeb();
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        //
        // GET: /Carrera/

        public ActionResult Index()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), Convert.ToInt32(EAccionesBitacora.Login), 0,
                    CAccesoWeb.ListarEntidades(typeof(CExperienciaProfesionalDTO).Name));
                return View();
            }
        }

        public ActionResult GetImage(int id, int tipo)
        {
            if (tipo == 2)
            {
                var datos = servicioCarrera.BuscarCursoCapacitacionPorCodigo(new CCursoCapacitacionDTO { IdEntidad = id }).ToList();
                var image = ((CCursoCapacitacionDTO)datos.ElementAt(1)).ImagenTitulo;
                return File(image, "application/pdf");
            }
            else
            {
                var datos = servicioCarrera.BuscarCursoGradoPorCodigo(new CCursoGradoDTO { IdEntidad = id }).ToList();
                var image = ((CCursoGradoDTO)datos.ElementAt(1)).ImagenTitulo;
                return File(image, "application/pdf");
            }
        }

        //
        // GET: /Carrera/Details/5

        public ActionResult Details(int codigo, string accion, int tipo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                    if (tipo == 2)
                    {
                        var datos = servicioCarrera.BuscarCursoCapacitacionPorCodigo(new CCursoCapacitacionDTO { IdEntidad = codigo }).ToList();
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                            model.CursoCapacitacion = (CCursoCapacitacionDTO)datos.ElementAt(1);
                            model.CursoCapacitacion.EntidadEducativa = (CEntidadEducativaDTO)datos.ElementAt(2);
                            model.CursoCapacitacion.Modalidad = (CModalidadDTO)datos.ElementAt(3);
                            model.DetalleContratacion = (CDetalleContratacionDTO)datos.ElementAt(4);
                            if (model.DetalleContratacion.CodigoPolicial != 0)
                            {
                                model.TituloFieldSet = "puntos porcentuales (%)";
                            }
                            else
                            {
                                model.TituloFieldSet = "puntos";
                            }
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.FirstOrDefault();
                        }
                    }
                    else
                    {
                        var datos = servicioCarrera.BuscarCursoGradoPorCodigo(new CCursoGradoDTO { IdEntidad = codigo }).ToList();
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                            model.CursoGrado = (CCursoGradoDTO)datos.ElementAt(1);
                            model.CursoGrado.EntidadEducativa = (CEntidadEducativaDTO)datos.ElementAt(2);
                            model.DetalleContratacion = (CDetalleContratacionDTO)datos.ElementAt(3);
                            if (model.DetalleContratacion.CodigoPolicial != 0)
                            {
                                var tipos = this.GenerarDiccionario(true);
                                model.CursoSeleccionado = tipos.FirstOrDefault(Q => Q.Key == model.CursoGrado.TipoGrado).Value;
                                model.TituloFieldSet = "puntos porcentuales (%)";
                            }
                            else
                            {
                                var tipos = this.GenerarDiccionario(false);
                                model.CursoSeleccionado = tipos.FirstOrDefault(Q => Q.Key == model.CursoGrado.TipoGrado).Value;
                                model.TituloFieldSet = "puntos";
                            }
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.FirstOrDefault();
                        }
                    }

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        //
        // GET: /Carrera/Create
        public ActionResult Create()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    List<string> tipos = new List<string>();
                    tipos.Add("Curso de capacitación");
                    tipos.Add("Curso de grado");
                    FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                    model.TipoCurso = new SelectList(tipos);
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        //
        // POST: /Carrera/Create

        [HttpPost]
        public ActionResult Create(FuncionarioCarreraVM model, string SubmitButton)
        {
            List<string> tipos = new List<string>();
            tipos.Add("Curso de capacitación");
            tipos.Add("Curso de grado");

            model.TipoCurso = new SelectList(tipos);

            try
            {
                var entidadesEducativas = servicioCarrera.ListarEntidadEducativa()
                            .Select(Q => new SelectListItem
                            {
                                Value = ((CEntidadEducativaDTO)Q).IdEntidad.ToString(),
                                Text = ((CEntidadEducativaDTO)Q).DescripcionEntidad
                            }).OrderBy(Q => Q.Text);

                model.EntidadesEducativas = new SelectList(entidadesEducativas, "Value", "Text");

                if (SubmitButton == "Buscar")
                {
                    var datosFuncionario = new List<CBaseDTO>();

                    if (model.CursoSeleccionado == null)
                    {
                        ModelState.AddModelError("Search", "Debe seleccionar el tipo de curso a registrar");
                        throw new Exception("Busqueda");
                    }

                    if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null)
                    {
                        datosFuncionario =
                            servicioFuncionario.BuscarFuncionarioPolicial(model.Funcionario.Cedula).ToList();
                    }
                    else
                    {
                        if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null)
                        {
                            datosFuncionario =
                                servicioFuncionario.BuscarFuncionarioProfesional(model.Funcionario.Cedula).ToList();
                        }
                        else
                        {
                            datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula).ToList();
                        }
                    }


                    if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        var datosCarrera = servicioCarrera.BuscarDatosCarreraCedula(model.Funcionario.Cedula);
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.Funcionario.Mensaje = datosCarrera[0].Mensaje;
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];

                        var calificacion = servicioFuncionario.CargarCalificacionActual(model.Funcionario.Cedula);
                        if (calificacion.GetType() == typeof(CErrorDTO))
                            model.Calficacion = "NO INDICA";
                        else
                            model.Calficacion = ((CCalificacionNombramientoDTO)calificacion).CalificacionDTO.DesCalificacion;

                        if (model.CursoSeleccionado.Contains("grado"))
                        {
                            model.CursoGrado = new CCursoGradoDTO();
                        }
                        else
                        {
                            model.CursoCapacitacion = new CCursoCapacitacionDTO();
                        }

                        if (model.DetalleContratacion.CodigoPolicial != 0)
                        {
                            model.TituloFieldSet = "Carrera Policial";
                            if (model.CursoCapacitacion != null)
                            {
                                var modalidades = servicioCarrera.ListarModalidad()
                                    .Where(Q => Q.IdEntidad < 4 || Q.IdEntidad > 5 || Q.IdEntidad == 10 || Q.IdEntidad == 11)
                                    .Select(Q => new SelectListItem
                                    {
                                        Value = ((CModalidadDTO)Q).IdEntidad.ToString(),
                                        Text = ((CModalidadDTO)Q).Descripcion
                                    });
                                model.Modalidades = new SelectList(modalidades, "Value", "Text");

                                if (datosCarrera.ElementAt(1) != null)
                                {
                                    var capacitacion = datosCarrera
                                            .Where(Q => Q.GetType() == typeof(CCursoCapacitacionDTO) &&
                                                    ((CCursoCapacitacionDTO)Q).Estado != 3)
                                            .ToList();
                                    //capacitacion = capacitacion.Where(Q => ((CCursoCapacitacionDTO)Q).Estado != 3).ToList();

                                    model.PuntosEspecializada = SumarPuntosCapacitacionEspecializada(capacitacion, true);
                                    model.PorcentajeCurso = capacitacion
                                                                    .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == 7)
                                                                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalPuntos);
                                    model.PorcentajeRiesgo = capacitacion
                                                                    .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == 6)
                                                                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalPuntos);

                                    model.PorcentajeInstruccionOficial = capacitacion
                                                                    .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == 3)
                                                                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalPuntos);

                                    model.HorasAprovechamiento = CalcularHoras(capacitacion, 1, true);
                                    model.HorasPartipacion = CalcularHoras(capacitacion, 2, true);
                                }
                                else
                                {
                                    ActualizarModeloInicial(model, true);
                                }
                            }
                            else
                            {
                                Dictionary<int, string> grados = GenerarDiccionario(true);

                                model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                                if (datosCarrera.ElementAt(1) != null)
                                {
                                    var cursosGrado = datosCarrera.Where(Q => Q.GetType() == typeof(CCursoGradoDTO)).ToList();
                                    if (cursosGrado.Count() > 0)
                                    {
                                        model.CursoGradoActual = ((CCursoGradoDTO)cursosGrado.
                                                                    OrderByDescending(Q => ((CCursoGradoDTO)Q).FechaEmision)
                                                                    .FirstOrDefault()).TipoGrado;
                                    }
                                    else
                                    {
                                        model.CursoGradoActual = 0;
                                    }
                                }
                                else
                                {
                                    model.CursoGradoActual = 0;
                                }
                            }
                        }
                        else
                        {
                            model.TituloFieldSet = "Carrera Profesional";
                            if (model.CursoCapacitacion != null)
                            {
                                var modalidades = servicioCarrera.ListarModalidad()
                                    .Where(Q => Q.IdEntidad < 6 || Q.IdEntidad == 10 || Q.IdEntidad == 11)
                                    .Select(Q => new SelectListItem
                                    {
                                        Value = ((CModalidadDTO)Q).IdEntidad.ToString(),
                                        Text = ((CModalidadDTO)Q).Descripcion
                                    });
                                model.Modalidades = new SelectList(modalidades, "Value", "Text");

                                if (datosCarrera.ElementAt(1) != null)
                                {
                                    var capacitacion = datosCarrera
                                                        .Where(Q => Q.GetType() == typeof(CCursoCapacitacionDTO) &&
                                                        ((CCursoCapacitacionDTO)Q).Estado != 3)
                                                        .ToList();

                                    model.PuntosEspecializada = SumarPuntosCapacitacionEspecializada(capacitacion, false);
                                    model.PuntosPublicaciones = capacitacion
                                                                    .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == 4)
                                                                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalPuntos);
                                    model.PuntosLibros = capacitacion
                                                                    .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == 5)
                                                                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalPuntos);

                                    model.HorasAprovechamiento = CalcularHoras(capacitacion, 1, false);
                                    model.HorasPartipacion = CalcularHoras(capacitacion, 2, false);
                                    model.HorasInstruccion = CalcularHoras(capacitacion, 3, false);
                                }
                                else
                                {
                                    ActualizarModeloInicial(model, true);
                                }
                            }
                            else
                            {
                                Dictionary<int, string> grados = GenerarDiccionario(false);
                                model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                                if (datosCarrera.ElementAt(1) != null)
                                {
                                    var cursosGrado = datosCarrera.Where(Q => Q.GetType() == typeof(CCursoGradoDTO)).ToList();
                                    if (cursosGrado.Count() > 0)
                                    {
                                        model.CursoGradoActual = ((CCursoGradoDTO)cursosGrado.
                                                                    OrderByDescending(Q => ((CCursoGradoDTO)Q).FechaEmision)
                                                                    .FirstOrDefault()).TipoGrado;
                                    }
                                    else
                                    {
                                        model.CursoGradoActual = 0;
                                    }
                                }
                                else
                                {
                                    model.CursoGradoActual = 0;
                                }
                            }
                        }

                        return PartialView("_FormularioCarrera", model);
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
                    if (ModelState.IsValid)
                    {
                        model.Funcionario.Sexo = GeneroEnum.Indefinido;
                        if (model.CursoCapacitacion != null)
                        {
                            if (model.CursoCapacitacion.TotalHoras < 840)
                            {
                                model.CursoCapacitacion.EntidadEducativa = new CEntidadEducativaDTO { IdEntidad = model.EntidadEducativaSeleccionada };
                                model.CursoCapacitacion.Modalidad = new CModalidadDTO { IdEntidad = model.ModalidadSeleccionada };

                                if (model.ModalidadSeleccionada == 10 || model.ModalidadSeleccionada == 11)
                                {
                                    DateTime fecha = DateTime.Now;
                                    model.CursoCapacitacion.FecRegistro = fecha;
                                    model.CursoCapacitacion.FecVence = new DateTime(fecha.Year + 5, fecha.Month, fecha.Day);
                                }

                                if (model.File != null)
                                {
                                    Stream str = model.File.InputStream;
                                    BinaryReader Br = new BinaryReader(str);
                                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                                    model.CursoCapacitacion.ImagenTitulo = FileDet;
                                }
                                if (ejecutado == false)
                                {
                                    var resultado = servicioCarrera.GuardarFormacionAcademica(model.CursoCapacitacion, model.Funcionario);
                                    if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                    {

                                        capacitacion = ((CCursoCapacitacionDTO)resultado.Last()).IdEntidad;

                                        // Riesgo Policial - Curso Básico
                                        if (model.CursoCapacitacion.Modalidad.IdEntidad == 6 || model.CursoCapacitacion.Modalidad.IdEntidad == 7)
                                        {
                                            int tipo = 0;
                                            CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                                            {
                                                IdEntidad = 1 // Registrado
                                            };

                                            if (model.CursoCapacitacion.Modalidad.IdEntidad == 6)
                                                tipo = Convert.ToInt32(ETipoAccionesHelper.ReajAprobRiesgoPolicial);
                                            else
                                                tipo = Convert.ToInt32(ETipoAccionesHelper.ReajCursoBas);

                                            CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                                            {
                                                IdEntidad = tipo
                                            };

                                            CAccionPersonalDTO accion = new CAccionPersonalDTO
                                            {
                                                AnioRige = DateTime.Now.Year,
                                                CodigoModulo = Convert.ToInt32(EModulosHelper.Carrera),
                                                CodigoObjetoEntidad = capacitacion,
                                                FecRige = DateTime.Now,
                                                FecVence = null,
                                                FecRigeIntegra = DateTime.Now,
                                                FecVenceIntegra = null,
                                                Observaciones = model.CursoCapacitacion.Observaciones,
                                                IndDato = model.CursoCapacitacion.TotalPuntos
                                            };

                                            CDetalleAccionPersonalDTO detalle = new CDetalleAccionPersonalDTO
                                            {
                                                CodNombramiento = 0,
                                                CodPrograma = 0,
                                                CodSeccion = 0,

                                                IndCategoria = model.DetallePuesto.EscalaSalarial.CategoriaEscala,
                                                CodClase = model.DetallePuesto.Clase.IdEntidad,
                                                PorProhibicion = model.CursoCapacitacion.TotalPuntos
                                            };

                                            var respuestaAP = servicioAccion.AgregarAccion(model.Funcionario,
                                                                                            estado,
                                                                                            tipoAP,
                                                                                            accion,
                                                                                            detalle);

                                        }

                                        List<string> entidades = new List<string>();
                                        entidades.Add(typeof(CAccionPersonalDTO).Name);

                                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                                                Convert.ToInt32(EAccionesBitacora.Guardar), capacitacion,
                                                CAccesoWeb.ListarEntidades(entidades.ToArray()));


                                        ejecutado = true;

                                        return JavaScript("");
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Agregar", ((CErrorDTO)resultado.FirstOrDefault()).MensajeError);
                                        throw new Exception(((CErrorDTO)resultado.FirstOrDefault()).MensajeError);
                                    }
                                }
                                else
                                {
                                    ejecutado = false;
                                    return RedirectToAction("Details", new { codigo = capacitacion, accion = "guardar", tipo = 2 });
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("Agregar", "Se excedió el Total de Horas");
                                throw new Exception("Agregar");
                            }
                        }
                        else
                        {
                            model.CursoGrado.EntidadEducativa = new CEntidadEducativaDTO { IdEntidad = model.EntidadEducativaSeleccionada };
                            model.CursoGrado.TipoGrado = model.GradoAcademicoSeleccionado;
                            if (model.File != null)
                            {
                                using (var reader = new System.IO.BinaryReader(model.File.InputStream))
                                {
                                    model.CursoGrado.ImagenTitulo = reader.ReadBytes(model.File.ContentLength);
                                }
                            }
                            if (ejecutado == false)
                            {
                                var resultado = servicioCarrera.GuardarFormacionAcademica(model.CursoGrado, model.Funcionario);

                                if (resultado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                                {
                                    grado = ((CCursoGradoDTO)resultado.Last()).IdEntidad;

                                    CEstadoBorradorDTO estado = new CEstadoBorradorDTO
                                    {
                                        IdEntidad = 1 // Registrado
                                    };

                                    CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                                    {
                                        IdEntidad = Convert.ToInt32(ETipoAccionesHelper.ReajCarreraProf)
                                    };


                                    CAccionPersonalDTO accion;
                                    if (model.CursoCapacitacion == null)
                                    {
                                        accion = new CAccionPersonalDTO
                                        {
                                            AnioRige = DateTime.Now.Year,
                                            CodigoModulo = Convert.ToInt32(EModulosHelper.Carrera),
                                            CodigoObjetoEntidad = grado,
                                            FecRige = DateTime.Now,
                                            FecVence = null,
                                            FecRigeIntegra = DateTime.Now,
                                            FecVenceIntegra = null,
                                            Observaciones = model.CursoGrado.Observaciones,
                                            //IndDato = model.CursoCapacitacion.TotalPuntos   //fix null pointer
                                        };
                                    }
                                    else
                                    {
                                        accion = new CAccionPersonalDTO
                                        {
                                            AnioRige = DateTime.Now.Year,
                                            CodigoModulo = Convert.ToInt32(EModulosHelper.Carrera),
                                            CodigoObjetoEntidad = grado,
                                            FecRige = DateTime.Now,
                                            FecVence = null,
                                            FecRigeIntegra = DateTime.Now,
                                            FecVenceIntegra = null,
                                            Observaciones = model.CursoGrado.Observaciones,
                                            IndDato = model.CursoCapacitacion.TotalPuntos
                                        };
                                    }

                                    var respuestaAP = servicioAccion.AgregarAccion(model.Funcionario,
                                                                                   estado,
                                                                                   tipoAP,
                                                                                   accion,
                                                                                   null);

                                    ejecutado = true;

                                    return JavaScript("");
                                }
                                else
                                {
                                    ModelState.AddModelError("Agregar", ((CErrorDTO)resultado.FirstOrDefault()).MensajeError);
                                    throw new Exception(((CErrorDTO)resultado.FirstOrDefault()).MensajeError);
                                }
                            }
                            else
                            {
                                ejecutado = false;
                                return RedirectToAction("Details", new { codigo = grado, accion = "guardar", tipo = 1 });
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Agregar");
                    }
                }
            }
            catch (Exception error)
            {
                Dictionary<int, string> grados = GenerarDiccionario(false);
                model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    if (model.CursoCapacitacion != null && model.DetalleContratacion.CodigoPolicial == 0)
                    {
                        var modalidades = servicioCarrera.ListarModalidad()
                            .Where(Q => Q.IdEntidad < 6)
                            .Select(Q => new SelectListItem
                            {
                                Value = ((CModalidadDTO)Q).IdEntidad.ToString(),
                                Text = ((CModalidadDTO)Q).Descripcion
                            });
                        model.Modalidades = new SelectList(modalidades, "Value", "Text");
                    }
                    else
                    {
                        var modalidades = servicioCarrera.ListarModalidad()
                                                           .Where(Q => Q.IdEntidad < 4 || Q.IdEntidad > 5 || Q.IdEntidad == 10 || Q.IdEntidad == 11
                                                           )
                                                           .Select(Q => new SelectListItem
                                                           {
                                                               Value = ((CModalidadDTO)Q).IdEntidad.ToString(),
                                                               Text = ((CModalidadDTO)Q).Descripcion
                                                           });
                        model.Modalidades = new SelectList(modalidades, "Value", "Text");
                    }
                    return PartialView("_FormularioCarrera", model);
                }
            }
        }

        private Dictionary<int, string> GenerarDiccionario(bool oficial)
        {
            if (oficial)
            {
                return new Dictionary<int, string>
                {
                    {1, "Diplomado"},
                    {2, "Bachiller"},
                    {3, "Licenciatura"}
                };
            }
            else
            {
                return new Dictionary<int, string>
                {
                    {1, "Bachiller"},
                    {2, "Licenciatura"},
                    {3, "Licenciatura adicional"},
                    {4, "Maestría"},
                    {5, "Maestría Adicional"},
                    {6, "Doctorado"},
                    {7, "Doctorado Adicional"},
                    {8, "Especialidad con base a Bachillerato"},
                    {9, "Especialidad con base a Licenciatura"},
                    {10, "Especialidad adicional"},
                };
            }
        }

        private Dictionary<int, string> TipoExperiencia()
        {
            return new Dictionary<int, string>
            {
                {1, "Reconocimiento"},
                {2, "Ajuste" }
            };
        }

        private void ActualizarModeloInicial(FuncionarioCarreraVM model, bool oficial)
        {
            if (oficial)
            {
                model.PuntosEspecializada = 0;
                model.PorcentajeCurso = 0;
                model.PorcentajeRiesgo = 0;
                model.PorcentajeInstruccionOficial = 0;
                model.HorasAprovechamiento = 0;
                model.HorasPartipacion = 0;
            }
            else
            {
                model.PuntosEspecializada = 0;
                model.PuntosPublicaciones = 0;
                model.PuntosLibros = 0;
                model.HorasAprovechamiento = 0;
                model.HorasPartipacion = 0;
                model.HorasInstruccion = 0;
            }
        }

        private int CalcularHoras(List<CBaseDTO> capacitacion, int modalidad, bool oficial)
        {
            //int respuesta = 0;
            int totalHoras = 0;
            //decimal division = 0;
            //int precision = 0;

            if (oficial)
            {
                if (modalidad == 1)
                {
                    totalHoras = capacitacion
                        .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == modalidad && ((CCursoCapacitacionDTO)Q).TotalHoras >= 12)
                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalHoras);
                }

                if (modalidad == 2)
                {
                    totalHoras = capacitacion
                        .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == modalidad && ((CCursoCapacitacionDTO)Q).TotalHoras >= 30)
                        .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalHoras);
                }

            }
            else
            {
                totalHoras = capacitacion
                                    .Where(Q => ((CCursoCapacitacionDTO)Q).Modalidad.IdEntidad == modalidad)
                                    .Sum(Q => ((CCursoCapacitacionDTO)Q).TotalHoras);

            }

            return totalHoras;
        }

        private int SumarPuntosCapacitacionEspecializada(List<CBaseDTO> capacitacion, bool oficial)
        {
            int respuesta = 0;
            if (!oficial)
            {
                foreach (var item in capacitacion)
                {
                    if (((CCursoCapacitacionDTO)item).Modalidad.IdEntidad == 1
                        || ((CCursoCapacitacionDTO)item).Modalidad.IdEntidad == 2
                        || ((CCursoCapacitacionDTO)item).Modalidad.IdEntidad == 3)
                    {
                        respuesta += ((CCursoCapacitacionDTO)item).TotalPuntos;
                    }
                }
            }
            else
            {
                foreach (var item in capacitacion)
                {
                    if (((CCursoCapacitacionDTO)item).Modalidad.IdEntidad == 1
                        || ((CCursoCapacitacionDTO)item).Modalidad.IdEntidad == 2)
                    {
                        respuesta += ((CCursoCapacitacionDTO)item).TotalPuntos;
                    }
                }
            }
            return respuesta;
        }

        //
        // GET: /Carrera/Search/

        public ActionResult Search(int param)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    BusquedaCarreraVM model = new BusquedaCarreraVM();

                    List<string> tipos = new List<string>();
                    tipos.Add("Curso de capacitación");
                    tipos.Add("Curso de grado");

                    model.TipoCurso = new SelectList(tipos);

                    var entidadesEducativas = servicioCarrera.ListarEntidadEducativa()
                                                .Select(Q => new SelectListItem
                                                {
                                                    Value = ((CEntidadEducativaDTO)Q).IdEntidad.ToString(),
                                                    Text = ((CEntidadEducativaDTO)Q).DescripcionEntidad
                                                }).OrderBy(Q => Q.Text);

                    model.EntidadesEducativas = new SelectList(entidadesEducativas, "Value", "Text");

                    if (param == 1)
                    {
                        Dictionary<int, string> grados = grados = GenerarDiccionario(true);
                        model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                        var modalidades = servicioCarrera.ListarModalidad()
                                            .Where(Q => Q.IdEntidad < 4 || Q.IdEntidad > 5)
                                            .Select(Q => new SelectListItem
                                            {
                                                Value = ((CModalidadDTO)Q).IdEntidad.ToString(),
                                                Text = ((CModalidadDTO)Q).Descripcion
                                            });
                        model.Modalidades = new SelectList(modalidades, "Value", "Text");
                    }
                    else
                    {
                        Dictionary<int, string> grados = grados = GenerarDiccionario(false);
                        model.GradosAcademicos = new SelectList(grados, "Key", "Value");
                        var modalidades = servicioCarrera.ListarModalidad()
                        .Where(Q => Q.IdEntidad < 6)
                        .Select(Q => new SelectListItem
                        {
                            Value = ((CModalidadDTO)Q).IdEntidad.ToString(),
                            Text = ((CModalidadDTO)Q).Descripcion
                        });
                        model.Modalidades = new SelectList(modalidades, "Value", "Text");
                    }

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        //
        // POST: /Carrera/Search/
        [HttpPost]
        public ActionResult Search(BusquedaCarreraVM model)
        {
            try
            {
                var param = Request.Form["parametro"];
                model.Funcionario.Sexo = GeneroEnum.Indefinido;
                model.Funcionario.Mensaje = param;
                if ((model.Funcionario.Cedula != null ||
                   (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1) || model.TituloCurso != null
                    || model.NumeroResolucion != null || model.EntidadEducativaSeleccionada != 0
                    || model.Contratacion.CodigoPolicial > 0) && model.CursoSeleccionado != null)
                {
                    var datos = new CBaseDTO[0][];
                    List<DateTime> fechasEmision = new List<DateTime>();
                    if (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1)
                    {
                        fechasEmision.Add(model.FechaDesde);
                        fechasEmision.Add(model.FechaHasta);
                    }

                    if (model.CursoSeleccionado.Contains("grado"))
                    {
                        model.CursoGrado = new CCursoGradoDTO();
                        model.CursoGrado.EntidadEducativa = new CEntidadEducativaDTO();
                        model.CursoGrado.EntidadEducativa.IdEntidad = model.EntidadEducativaSeleccionada;
                        model.CursoGrado.CursoGrado = model.TituloCurso;
                        model.CursoGrado.Resolucion = model.NumeroResolucion;
                        model.CursoGrado.TipoGrado = model.GradoAcademicoSeleccionado;
                        datos = servicioCarrera.BuscarDatosCursos(model.Funcionario, model.Contratacion,
                                                                  model.CursoGrado, fechasEmision.ToArray());
                    }
                    else
                    {
                        model.CursoCapacitacion = new CCursoCapacitacionDTO();
                        model.CursoCapacitacion.EntidadEducativa = new CEntidadEducativaDTO();
                        model.CursoCapacitacion.EntidadEducativa.IdEntidad = model.EntidadEducativaSeleccionada;
                        model.CursoCapacitacion.DescripcionCapacitacion = model.TituloCurso;
                        model.CursoCapacitacion.TotalHoras = model.TotalHoras;
                        model.CursoCapacitacion.Resolucion = model.NumeroResolucion;
                        model.CursoCapacitacion.Modalidad = new CModalidadDTO();
                        model.CursoCapacitacion.Modalidad.IdEntidad = model.ModalidadSeleccionada;
                        datos = servicioCarrera.BuscarDatosCursos(model.Funcionario, model.Contratacion,
                                                                  model.CursoCapacitacion, fechasEmision.ToArray());
                    }

                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        model.Cursos = new List<BusquedaCarreraVM>();

                        foreach (var item in datos.ElementAt(1).ToList())
                        {
                            if (model.CursoSeleccionado.Contains("grado"))
                            {
                                BusquedaCarreraVM temp = new BusquedaCarreraVM();
                                model.Tipo = "grado";
                                temp.Funcionario = (CFuncionarioDTO)datos.ElementAt(0).ElementAt(1);
                                temp.CursoGrado = (CCursoGradoDTO)item;
                                if (param == "1")
                                {
                                    var tipos = this.GenerarDiccionario(true);
                                    temp.NombreGrado = tipos.FirstOrDefault(Q => Q.Key == temp.CursoGrado.TipoGrado).Value;
                                }
                                else
                                {
                                    var tipos = this.GenerarDiccionario(false);
                                    temp.NombreGrado = tipos.FirstOrDefault(Q => Q.Key == temp.CursoGrado.TipoGrado).Value;
                                }
                                model.Cursos.Add(temp);
                            }
                            else
                            {
                                BusquedaCarreraVM temp = new BusquedaCarreraVM();
                                model.Tipo = "capacitacion";
                                temp.Funcionario = (CFuncionarioDTO)datos.ElementAt(0).ElementAt(1);
                                temp.CursoCapacitacion = (CCursoCapacitacionDTO)item;
                                model.Cursos.Add(temp);
                            }
                        }

                        if (model.Funcionario.Cedula != null && (model.FechaDesde.Year == 1 && model.FechaHasta.Year == 1) && model.TituloCurso == null
                            && model.NumeroResolucion == null && model.EntidadEducativaSeleccionada == 0
                            && (model.Contratacion == null || model.Contratacion.CodigoPolicial == 0) && model.Tipo == "capacitacion")
                        {
                            model = CalcularHorasResumen(model, Convert.ToInt32(param));
                        }
                        else
                        {
                            model.TotalPorcentaje = model.TotalPuntos = model.HorasAcumuladas = model.TotalHoras = 0;
                        }

                        return PartialView("_SearchCarreraResults", model);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaDesde.Year > 1 || model.FechaHasta.Year > 1)
                    {
                        if (!(model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
                        }
                    }
                    if (model.CursoSeleccionado == null)
                    {
                        ModelState.AddModelError("Busqueda", "Debe seleccionar el tipo de curso para realizar la búsqueda.");
                    }
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Sistema", error.Message);
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }
        }


        private BusquedaCarreraVM CalcularHorasResumen(BusquedaCarreraVM model, int param)
        {
            if (param == 1) //Policial
            {
                switch (model.ModalidadSeleccionada)
                {
                    case 1: //Aprovechamiento
                        model.TotalHoras = model.Cursos.Where(C => C.CursoCapacitacion.TotalHoras >= 12).Select(C => C.CursoCapacitacion.TotalHoras).Sum();
                        model.TotalPorcentaje = (int)Math.Floor((double)model.TotalHoras / 40);
                        model.HorasAcumuladas = model.TotalHoras - (model.TotalPorcentaje * 40);
                        model.PorcentajeAdicional = Convert.ToInt32(model.Cursos[0].Funcionario.Mensaje);
                        model.TotalPorcentaje = model.TotalPorcentaje + model.PorcentajeAdicional;
                        break;
                    case 2: //Participación
                        model.TotalHoras = model.Cursos.Where(C => C.CursoCapacitacion.TotalHoras >= 30).Select(C => C.CursoCapacitacion.TotalHoras).Sum();
                        model.TotalPorcentaje = (int)Math.Floor((double)model.TotalHoras / 80);
                        model.HorasAcumuladas = model.TotalHoras - (model.TotalPorcentaje * 80);
                        model.PorcentajeAdicional = Convert.ToInt32(model.Cursos[0].Funcionario.Mensaje);
                        model.TotalPorcentaje = model.TotalPorcentaje + model.PorcentajeAdicional;
                        break;
                    default:
                        model.TotalHoras = model.HorasAcumuladas = model.TotalPorcentaje = 0;
                        break;
                }
                model.TotalPuntos = 0;
                model.PuntosAdicionales = 0;
            }
            else //Administrativo
            {
                switch (model.ModalidadSeleccionada)
                {
                    case 1: //Aprovechamiento
                        model.TotalHoras = model.Cursos.Where(C => C.CursoCapacitacion.TotalHoras >= 1).Select(C => C.CursoCapacitacion.TotalHoras).Sum();
                        model.TotalPuntos = (int)Math.Floor((double)model.TotalHoras / 40);
                        model.HorasAcumuladas = model.TotalHoras - (model.TotalPuntos * 40);
                        model.PuntosAdicionales = Convert.ToInt32(model.Cursos[0].Funcionario.Mensaje);
                        model.TotalPuntos = model.TotalPuntos + model.PuntosAdicionales;
                        break;
                    case 2: //Participación
                        model.TotalHoras = model.Cursos.Where(C => C.CursoCapacitacion.TotalHoras >= 1).Select(C => C.CursoCapacitacion.TotalHoras).Sum();
                        model.TotalPuntos = (int)Math.Floor((double)model.TotalHoras / 80);
                        model.HorasAcumuladas = model.TotalHoras - (model.TotalPuntos * 80);
                        model.PuntosAdicionales = Convert.ToInt32(model.Cursos[0].Funcionario.Mensaje);
                        model.TotalPuntos = model.TotalPuntos + model.PuntosAdicionales;
                        break;
                    case 3: //Instrucción
                        model.TotalHoras = model.Cursos.Where(C => C.CursoCapacitacion.TotalHoras >= 1).Select(C => C.CursoCapacitacion.TotalHoras).Sum();
                        model.TotalPuntos = (int)Math.Floor((double)model.TotalHoras / 24);
                        model.HorasAcumuladas = model.TotalHoras - (model.TotalPuntos * 24);
                        model.PuntosAdicionales = Convert.ToInt32(model.Cursos[0].Funcionario.Mensaje);
                        model.TotalPuntos = model.TotalPuntos + model.PuntosAdicionales;
                        break;
                    default:
                        model.TotalHoras = model.HorasAcumuladas = model.TotalPuntos = 0;
                        break;
                }
                model.TotalPorcentaje = 0;
                model.PorcentajeAdicional = 0;
            }
            return model;
        }

        //
        // GET: /Carrera/Edit/5

        public ActionResult Edit(int id, int tipo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                    if (tipo == 2)
                    {
                        var datos = servicioCarrera.BuscarCursoCapacitacionPorCodigo(new CCursoCapacitacionDTO { IdEntidad = id }).ToList();
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                            model.CursoCapacitacion = (CCursoCapacitacionDTO)datos.ElementAt(1);
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.FirstOrDefault();
                        }
                    }
                    else
                    {
                        var datos = servicioCarrera.BuscarCursoGradoPorCodigo(new CCursoGradoDTO { IdEntidad = id }).ToList();
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                            model.CursoGrado = (CCursoGradoDTO)datos.ElementAt(1);
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.FirstOrDefault();
                        }
                    }

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        //
        // POST: /Carrera/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FuncionarioCarreraVM model)
        {
            try
            {
                int tipo = 0;

                if (model.CursoCapacitacion != null)
                {
                    if (model.CursoCapacitacion.Observaciones != null)
                    {
                        model.CursoCapacitacion.IdEntidad = id;
                        var respuesta = servicioCarrera.AnularCurso(model.CursoCapacitacion);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            if (model.CursoCapacitacion.Modalidad.IdEntidad == 6)
                                tipo = Convert.ToInt32(ETipoAccionesHelper.ReajAprobRiesgoPolicial);
                            else
                                tipo = Convert.ToInt32(ETipoAccionesHelper.ReajCursoBas);

                            CTipoAccionPersonalDTO tipoAP = new CTipoAccionPersonalDTO
                            {
                                IdEntidad = tipo
                            };

                            CAccionPersonalDTO accion = new CAccionPersonalDTO
                            {
                                CodigoModulo = Convert.ToInt32(EModulosHelper.Carrera),
                                CodigoObjetoEntidad = id,
                                Observaciones = model.CursoCapacitacion.Observaciones,
                                TipoAccion = tipoAP
                            };

                            servicioAccion.AnularAccionModulo(accion);

                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                                    Convert.ToInt32(EAccionesBitacora.Editar), id,
                                    CAccesoWeb.ListarEntidades(typeof(CCursoCapacitacionDTO).Name));
                            return JavaScript($"window.location = '/Carrera/Details?codigo={id}&accion=anular&tipo=2'");
                        }
                        else
                        {
                            ModelState.AddModelError("anular", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Debe digitar una justificación para anular este curso");
                        throw new Exception();
                    }
                }
                else
                {
                    if (model.CursoGrado.Observaciones != null)
                    {
                        model.CursoGrado.IdEntidad = id;
                        var respuesta = servicioCarrera.AnularCurso(model.CursoGrado);

                        if (respuesta.GetType() != typeof(CErrorDTO))
                        {
                            CAccionPersonalDTO accion = new CAccionPersonalDTO
                            {
                                CodigoModulo = Convert.ToInt32(EModulosHelper.Carrera),
                                CodigoObjetoEntidad = id,
                                Observaciones = model.CursoGrado.Observaciones
                            };

                            servicioAccion.AnularAccionModulo(accion);

                            context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                                    Convert.ToInt32(EAccionesBitacora.Editar), id,
                                    CAccesoWeb.ListarEntidades(typeof(CCursoGradoDTO).Name));
                            return JavaScript($"window.location = '/Carrera/Details?codigo={id}&accion=anular&tipo=1'");
                        }
                        else
                        {
                            ModelState.AddModelError("anular", respuesta.Mensaje);
                            throw new Exception(respuesta.Mensaje);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Debe digitar una justificación para anular este curso");
                        throw new Exception();
                    }
                }


            }
            catch
            {
                return PartialView("_ErrorCarrera");
            }
        }

        //
        // GET: /Carrera/EditCurso/5

        public ActionResult EditCurso(int id, int tipo)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    FuncionarioCarreraVM model = new FuncionarioCarreraVM();
                    if (tipo == 2)
                    {
                        var datos = servicioCarrera.BuscarCursoCapacitacionPorCodigo(new CCursoCapacitacionDTO { IdEntidad = id }).ToList();
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                            model.CursoCapacitacion = (CCursoCapacitacionDTO)datos.ElementAt(1);
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.FirstOrDefault();
                        }
                    }
                    else
                    {
                        var datos = servicioCarrera.BuscarCursoGradoPorCodigo(new CCursoGradoDTO { IdEntidad = id }).ToList();
                        if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                            model.CursoGrado = (CCursoGradoDTO)datos.ElementAt(1);
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos.FirstOrDefault();
                        }
                    }

                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        //
        // POST: /Carrera/EditCurso/5
        [HttpPost]
        public ActionResult EditCurso(int id, FuncionarioCarreraVM model)
        {
            try
            {
                if (model.CursoCapacitacion != null)
                {
                    if (model.CursoCapacitacion.Resolucion != null || model.File != null)
                    {
                        model.CursoCapacitacion.IdEntidad = id;

                        if (model.File != null)
                        {
                            Stream str = model.File.InputStream;
                            BinaryReader Br = new BinaryReader(str);
                            Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                            model.CursoCapacitacion.ImagenTitulo = FileDet;
                        }
                        
                        if (ejecutado == false)
                        {
                            var respuesta = servicioCarrera.EditarCurso(model.CursoCapacitacion);

                            if (respuesta.GetType() != typeof(CErrorDTO))
                            {
                                
                                capacitacion = respuesta.IdEntidad;
                                ejecutado = true;

                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                                        Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                        CAccesoWeb.ListarEntidades(typeof(CCursoCapacitacionDTO).Name));
                                return JavaScript("");                               
                            }
                            else
                            {
                                ModelState.AddModelError("modificar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            ejecutado = false;
                            return RedirectToAction("Details", new { codigo = capacitacion, accion = "modificar", tipo = 2 });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Debe editar al menos un campo para guardar los cambios");
                        throw new Exception();
                    }
                }
                else
                {
                    if (model.CursoGrado.Resolucion != null || model.File != null)
                    {
                        model.CursoGrado.IdEntidad = id;

                        if (model.File != null)
                        {
                            Stream str = model.File.InputStream;
                            BinaryReader Br = new BinaryReader(str);
                            byte[] FileDet = Br.ReadBytes((int)str.Length);
                            model.CursoGrado.ImagenTitulo = FileDet;
                        }


                        if (ejecutado == false)
                        {
                            var respuesta = servicioCarrera.EditarCurso(model.CursoGrado);

                            if (respuesta.GetType() != typeof(CErrorDTO))
                            {

                                grado = respuesta.IdEntidad;
                                ejecutado = true;

                                context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                                        Convert.ToInt32(EAccionesBitacora.Editar), respuesta.IdEntidad,
                                        CAccesoWeb.ListarEntidades(typeof(CCursoGradoDTO).Name));
                                return JavaScript("");
                            }
                            else
                            {
                                ModelState.AddModelError("modificar", respuesta.Mensaje);
                                throw new Exception(respuesta.Mensaje);
                            }
                        }
                        else
                        {
                            ejecutado = false;
                            return RedirectToAction("Details", new { codigo = grado, accion = "modificar", tipo = 1 });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("contenido", "Debe editar al menos un campo para guardar los cambios");
                        throw new Exception();
                    }
                }
            }
            catch
            {
                return PartialView("_ErrorCarrera");
            }
        }

        public ActionResult CreateExperiencia()
        {
            return View();
        }

        //
        // POST: /Carrera/Edit/5
        [HttpPost]
        public ActionResult CreateExperiencia(FuncionarioExperienciaVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    var datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula);
                    if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        var datosCarrera = servicioCarrera.BuscarExperienciaProfesionalCedula(model.Funcionario.Cedula);
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];

                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];
                        if (datosCarrera.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            model.Experiencia = (CExperienciaProfesionalDTO)datosCarrera[1];

                            model.Mensaje = "Ya se tiene registrada la Carrera Profesional para el funcionario consultado. La misma fue registrada el: " + model.Experiencia.FechaReg.ToShortDateString();
                            return PartialView("_FormularioExperiencia", model);
                        }
                        else
                        {
                            if (((CErrorDTO)datosCarrera[0]).MensajeError.Contains("Experiencia Profesional"))
                            {
                                model.Mensaje = "Registro Primera Vez";
                                Dictionary<int, string> tipos = TipoExperiencia();
                                model.TiposExperiencia = new SelectList(tipos, "Key", "Value");
                                return PartialView("_FormularioExperiencia", model);
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError("Busqueda", ((CErrorDTO)datosCarrera[0]).MensajeError);
                                throw new Exception("Busqueda");
                            }
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Busqueda", ((CErrorDTO)datosFuncionario[0]).MensajeError);
                        throw new Exception("Busqueda");
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        model.Experiencia.TipoExp = model.TipoExperienciaSeleccionada;
                        var guardado = servicioCarrera.GuardarExperienciaProfesional(model.Experiencia, model.Funcionario);
                        if (guardado.FirstOrDefault().GetType() != typeof(CErrorDTO))
                        {
                            return JavaScript("window.location = '/Carrera/DetailsExperiencia?ced=" +
                                                       ((CFuncionarioDTO)guardado.FirstOrDefault()).Cedula + "&accion=guardar" + "'");
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("Guardado", ((CErrorDTO)guardado[0]).MensajeError);
                            throw new Exception("Guardado");
                        }
                    }
                    else
                    {
                        Dictionary<int, string> tipos = TipoExperiencia();
                        model.TiposExperiencia = new SelectList(tipos, "Key", "Value");
                        throw new Exception("Formulario");
                    }
                }
            }
            catch (Exception error)
            {
                if (error.Message == "Busqueda")
                {
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    if (error.Message == "Guardado")
                    {
                        model.Error = new CErrorDTO { MensajeError = error.Message };
                        return PartialView("_FormularioExperiencia", model);
                    }
                    else
                    {
                        return PartialView("_FormularioExperiencia", model);
                    }
                }
            }
        }

        //
        // GET: /Carrera/Details/5

        public ActionResult DetailsExperiencia(string ced, string accion)
        {
            FuncionarioExperienciaVM model = new FuncionarioExperienciaVM();
            try
            {
                var datos = servicioCarrera.BuscarExperienciaProfesionalCedula(ced).ToList();
                if (datos.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    model.Funcionario = (CFuncionarioDTO)datos.ElementAt(0);
                    model.Experiencia = (CExperienciaProfesionalDTO)datos.ElementAt(1);
                }
                else
                {
                    model.Error = (CErrorDTO)datos.FirstOrDefault();
                }

                return View(model);
            }
            catch (Exception error)
            {
                model.Error = new CErrorDTO { MensajeError = error.Message };
                return View(model);
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleCarrera(FuncionarioCarreraVM model)
        {
            if (model.CursoGrado != null)
            {
                List<CursoGradoRptData> modelo = new List<CursoGradoRptData>();

                modelo.Add(CursoGradoRptData.GenerarDatosReporteIndividual(model, String.Empty));

                string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "CursoGradoRPT.rpt");
                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            else
            {
                List<CursoCapacitacionRptData> modelo = new List<CursoCapacitacionRptData>();

                modelo.Add(CursoCapacitacionRptData.GenerarDatosReporteIndividual(model, String.Empty));

                string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "CursoCapacitacionRPT.rpt");
                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseCarrera(List<BusquedaCarreraVM> model)
        {
            if (model.FirstOrDefault().CursoGrado != null)
            {
                List<CursoGradoRptData> modelo = new List<CursoGradoRptData>();

                foreach (var item in model)
                {
                    modelo.Add(CursoGradoRptData.GenerarDatosReporte(item, String.Empty));
                }

                string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "DesgloseCursoGradoRPT.rpt");
                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
            else
            {
                List<CursoCapacitacionRptData> modelo = new List<CursoCapacitacionRptData>();

                foreach (var item in model)
                {
                    modelo.Add(CursoCapacitacionRptData.GenerarDatosReporte(item, String.Empty));
                }

                string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "DesgloseCursoCapacitacionRPT.rpt");
                return new CrystalReportPdfResult(reportPath, modelo, "PDF");
            }
        }

        #region EntidadEducativa
        public ActionResult SearchEntidadEducativa()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    List<string> tipos = new List<string>();
                    tipos.Add("Entidad Gubernamental / ONG");
                    tipos.Add("Entidad universitaria");
                    tipos.Add("Entidad técnica");
                    tipos.Add("Entidad básica (educación básica, primer y segundo ciclo)");
                    tipos.Add("Otro");
                    BusquedaEntidadEducativaVM model = new BusquedaEntidadEducativaVM();
                    model.TiposEntidad = new SelectList(tipos);
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchEntidadEducativa(BusquedaEntidadEducativaVM model, string nombre, string tipo, int? page)
        {
            try
            {
                ModelState.Clear();
                if (model == null)
                {
                    model = new BusquedaEntidadEducativaVM();
                    model.EntidadEducativa = new CEntidadEducativaDTO();
                    model.EntidadEducativa.DescripcionEntidad = nombre;
                    model.EntidadEducativa.TipoEntidad = Convert.ToInt32(tipo);
                }

                switch (model.TipoEntidadSeleccionado)
                {
                    case "Entidad Gubernamental / ONG": model.EntidadEducativa.TipoEntidad = 1; break;
                    case "Entidad universitaria": model.EntidadEducativa.TipoEntidad = 2; break;
                    case "Entidad técnica": model.EntidadEducativa.TipoEntidad = 3; break;
                    case "Entidad básica (educación básica, primer y segundo ciclo)": model.EntidadEducativa.TipoEntidad = 4; break;
                    case "Otro": model.EntidadEducativa.TipoEntidad = 5; break;
                    default: model.EntidadEducativa.TipoEntidad = 0; break;
                }

                if (model.EntidadEducativa.DescripcionEntidad != null || model.EntidadEducativa.TipoEntidad > 0)
                {

                    var datos = servicioCarrera.BuscarDatosEntidadEducativa(model.EntidadEducativa.DescripcionEntidad, model.EntidadEducativa.TipoEntidad);

                    if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                    else
                    {

                        EntidadEducativaVM modelo = new EntidadEducativaVM();
                        modelo.Entidades = new List<CEntidadEducativaDTO>();

                        foreach (CEntidadEducativaDTO item in datos)
                        {
                            modelo.Entidades.Add(item);
                        }

                        modelo.TotalEntidades = modelo.Entidades.Count();
                        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalEntidades / 10);
                        int paginaActual = page ?? 1;
                        modelo.PaginaActual = paginaActual;
                        Session["entidadesEducativas"] = datos.ToList();
                        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalEntidades)
                        {
                            modelo.Entidades = modelo.Entidades.GetRange(((paginaActual - 1) * 10), (modelo.TotalEntidades) - (((paginaActual - 1) * 10))).ToList(); ;
                        }
                        else
                        {
                            modelo.Entidades = modelo.Entidades.GetRange(((paginaActual - 1) * 10), 10).ToList();
                        }

                        modelo.paramBusqueda = model;

                        return PartialView("_SearchEntidadEducativaResults", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }
        }

        public ActionResult CreateEntidadEducativa()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    List<string> tipos = new List<string>();
                    tipos.Add("Entidad Gubernamental / ONG");
                    tipos.Add("Entidad universitaria");
                    tipos.Add("Entidad técnica");
                    tipos.Add("Entidad básica (educación básica, primer y segundo ciclo)");
                    tipos.Add("Otro");
                    BusquedaEntidadEducativaVM model = new BusquedaEntidadEducativaVM();
                    model.TiposEntidad = new SelectList(tipos);
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult CreateEntidadEducativa(BusquedaEntidadEducativaVM model)
        {
            try
            {

                ModelState.Clear();
                if (model.EntidadEducativa.DescripcionEntidad == null)
                {
                    throw new Exception("Nombre de entidad inválido");
                }
                switch (model.TipoEntidadSeleccionado)
                {
                    case "Entidad Gubernamental / ONG": model.EntidadEducativa.TipoEntidad = 1; break;
                    case "Entidad universitaria": model.EntidadEducativa.TipoEntidad = 2; break;
                    case "Entidad técnica": model.EntidadEducativa.TipoEntidad = 3; break;
                    case "Entidad básica (educación básica, primer y segundo ciclo)": model.EntidadEducativa.TipoEntidad = 4; break;
                    case "Otro": model.EntidadEducativa.TipoEntidad = 5; break;
                    default: model.EntidadEducativa.TipoEntidad = 0; break;
                }

                var resultado = servicioCarrera.GuardarEntidadEducativa(model.EntidadEducativa.DescripcionEntidad, model.EntidadEducativa.TipoEntidad);

                if (resultado.GetType() != typeof(CErrorDTO))
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                        Convert.ToInt32(EAccionesBitacora.Guardar), resultado.IdEntidad,
                        CAccesoWeb.ListarEntidades(typeof(CEntidadEducativaDTO).Name));
                    return RedirectToAction("DetailsEntidadEducativa", new { id = resultado.IdEntidad, accion = "guardar" });
                    //return JavaScript("window.location = '/MontoCaucion/Details?id=" + resultado.IdEntidad + "&accion=guardar" + "'");
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado).MensajeError);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("BDError", error.Message);
                List<string> tipos = new List<string>();
                tipos.Add("Entidad Gubernamental / ONG");
                tipos.Add("Entidad universitaria");
                tipos.Add("Entidad técnica");
                tipos.Add("Entidad básica (educación básica, primer y segundo ciclo)");
                tipos.Add("Otro");
                model.TiposEntidad = new SelectList(tipos);
                return View(model);
            }
        }

        public ActionResult DetailsEntidadEducativa(int id)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    CEntidadEducativaDTO entidad = (CEntidadEducativaDTO)servicioCarrera.BuscarEntidadEducativa(id);
                    BusquedaEntidadEducativaVM model = new BusquedaEntidadEducativaVM();
                    model.EntidadEducativa = entidad;
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult DetailsEntidadEducativa(BusquedaEntidadEducativaVM model)
        {
            try
            {

                var resultado = servicioCarrera.AnularEntidadEducativa(model.EntidadEducativa.IdEntidad);

                if (resultado.GetType() != typeof(CErrorDTO))
                {
                    context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                        Convert.ToInt32(EAccionesBitacora.Guardar), resultado.IdEntidad,
                        CAccesoWeb.ListarEntidades(typeof(CEntidadEducativaDTO).Name));
                    return RedirectToAction("DetailsEntidadEducativa", new { id = resultado.IdEntidad, accion = "modificar" });
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado).MensajeError);
                }
            }
            catch (Exception error)
            {
                ModelState.AddModelError("BDError", error.Message);
                return View(model);
            }
        }
        #endregion

        #region Históricos
        public ActionResult SearchHistoricoCarrera()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchHistoricoCarrera(BusquedaHistoricoCarreraVM model,
                                                   string cedula,
                                                   string nombre,
                                                   string curso,
                                                   string puesto,
                                                   string diaDesde, string mesDesde, string annoDesde,
                                                   string diaHasta, string mesHasta, string annoHasta,
                                                   int? page)
        {
            try
            {

                List<DateTime> fechas = new List<DateTime>();

                if (model == null)
                {
                    model = new BusquedaHistoricoCarreraVM();
                    model.Carrera = new CCarreraProfesionalDTO();
                    model.Carrera.Nombre = nombre;
                    model.Carrera.Cedula = cedula;
                    model.Carrera.Curso1 = curso;
                    model.Carrera.Puesto = Convert.ToInt32(puesto);

                    if (Convert.ToInt32(annoDesde) > 1 && Convert.ToInt32(annoHasta) > 1)
                    {
                        model.FechaDesde = new DateTime(Convert.ToInt32(annoDesde), Convert.ToInt32(mesDesde), Convert.ToInt32(diaDesde));
                        model.FechaHasta = new DateTime(Convert.ToInt32(annoHasta), Convert.ToInt32(mesHasta), Convert.ToInt32(diaHasta));
                        fechas.Add(model.FechaDesde);
                        fechas.Add(model.FechaHasta);
                    }
                }

                if (model.Carrera.Cedula != null || model.Carrera.Nombre != null || (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1) ||
                    model.Carrera.Puesto > 0 || model.Curso != null)
                {
                    model.Carrera.Curso1 = model.Curso;
                    if (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1)
                    {
                        fechas.Add(model.FechaDesde);
                        fechas.Add(model.FechaHasta);
                    }


                    var datos = servicioCarrera.BuscarDatosCarreraProfesional(model.Carrera, fechas.ToArray());


                    if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        HistoricoCarreraVM modelo = new HistoricoCarreraVM();
                        modelo.Carreras = new List<CCarreraProfesionalDTO>();
                        foreach (CCarreraProfesionalDTO item in datos)
                        {
                            modelo.Carreras.Add(item);
                        }

                        modelo.TotalCarreras = modelo.Carreras.Count();
                        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalCarreras / 10);
                        int paginaActual = page ?? 1;
                        modelo.PaginaActual = paginaActual;
                        Session["carreras"] = datos.ToList();
                        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalCarreras)
                        {
                            modelo.Carreras = modelo.Carreras.GetRange(((paginaActual - 1) * 10), (modelo.TotalCarreras) - (((paginaActual - 1) * 10))).ToList(); ;
                        }
                        else
                        {
                            modelo.Carreras = modelo.Carreras.GetRange(((paginaActual - 1) * 10), 10).ToList();
                        }

                        modelo.paramBusqueda = model;

                        return PartialView("_SearchHistoricoCarreraResults", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaDesde.Year > 1 || model.FechaHasta.Year > 1)
                    {
                        if (!(model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
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
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }

        }

        public ActionResult DetailsHistoricoCarrera(int id)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    BusquedaHistoricoCarreraVM model = new BusquedaHistoricoCarreraVM();
                    try
                    {
                        var datos = servicioCarrera.CargarDatosCarreraProfesionalPorID(id);
                        if (datos.GetType() != typeof(CErrorDTO))
                        {
                            model.Carrera = (CCarreraProfesionalDTO)datos;
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos;
                        }

                        return View(model);
                    }
                    catch (Exception error)
                    {
                        model.Error = new CErrorDTO { MensajeError = error.Message };
                        return View(model);
                    }
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        public ActionResult SearchHistoricoPuntos()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchHistoricoPuntos(BusquedaHistoricoPuntosVM model, string cedula, string nombre, int? page)
        {
            try
            {
                if (model == null)
                {
                    model = new BusquedaHistoricoPuntosVM();
                    model.Puntos = new CPuntosDTO();
                    model.Puntos.Cedula = cedula;
                    model.Puntos.Nombre = nombre;
                }

                if (model.Puntos.Cedula != null || model.Puntos.Nombre != null)
                {

                    var datos = servicioCarrera.BuscarDatosPuntos(model.Puntos);

                    if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                    else
                    {

                        HistoricoPuntosVM modelo = new HistoricoPuntosVM();
                        modelo.Puntos = new List<CPuntosDTO>();

                        foreach (CPuntosDTO item in datos)
                        {
                            modelo.Puntos.Add(item);
                        }

                        modelo.TotalPuntos = modelo.Puntos.Count();
                        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalPuntos / 10);
                        int paginaActual = page ?? 1;
                        modelo.PaginaActual = paginaActual;
                        Session["puntos"] = datos.ToList();
                        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalPuntos)
                        {
                            modelo.Puntos = modelo.Puntos.GetRange(((paginaActual - 1) * 10), (modelo.TotalPuntos) - (((paginaActual - 1) * 10))).ToList(); ;
                        }
                        else
                        {
                            modelo.Puntos = modelo.Puntos.GetRange(((paginaActual - 1) * 10), 10).ToList();
                        }

                        modelo.paramBusqueda = model;

                        return PartialView("_SearchHistoricoPuntosResults", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError("Busqueda", "Los parámetros ingresados son insuficientes para realizar la búsqueda.");
                    throw new Exception("Busqueda");
                }
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }
        }

        public ActionResult DetailsHistoricoPuntos(int id)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    BusquedaHistoricoPuntosVM model = new BusquedaHistoricoPuntosVM();
                    try
                    {
                        var datos = servicioCarrera.CargarDatosPuntosPorID(id);
                        if (datos.GetType() != typeof(CErrorDTO))
                        {
                            model.Puntos = (CPuntosDTO)datos;
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos;
                        }

                        return View(model);
                    }
                    catch (Exception error)
                    {
                        model.Error = new CErrorDTO { MensajeError = error.Message };
                        return View(model);
                    }
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        public ActionResult SearchHistoricoCursos()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }


        [HttpPost]
        public ActionResult SearchHistoricoCursos(BusquedaHistoricoCursosVM model,
                                                  string cedula, string nombre,
                                                  string resolucion, string nomCurso,
                                                  string tipoCurso,
                                                  string diaDesde, string mesDesde, string annoDesde,
                                                  string diaHasta, string mesHasta, string annoHasta,
                                                  int? page)
        {
            try
            {
                List<DateTime> fechas = new List<DateTime>();

                if (model == null)
                {
                    model = new BusquedaHistoricoCursosVM();
                    model.Curso = new CCursoDTO();
                    model.Curso.Cedula = cedula;
                    model.Curso.Nombre = nombre;
                    model.Curso.Resolucion = resolucion;
                    model.Curso.NombreCurso = nomCurso;
                    model.Curso.TipoCurso = tipoCurso;
                    if (Convert.ToInt32(annoDesde) > 1 && Convert.ToInt32(annoHasta) > 1)
                    {
                        model.FechaDesde = new DateTime(Convert.ToInt32(annoDesde), Convert.ToInt32(mesDesde), Convert.ToInt32(diaDesde));
                        model.FechaHasta = new DateTime(Convert.ToInt32(annoHasta), Convert.ToInt32(mesHasta), Convert.ToInt32(diaHasta));
                        fechas.Add(model.FechaDesde);
                        fechas.Add(model.FechaHasta);
                    }


                }

                if (model.Curso.Cedula != null || model.Curso.Nombre != null || (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1) ||
                    model.Curso.Resolucion != null || model.Curso.NombreCurso != null || model.Curso.TipoCurso != null)
                {

                    if (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1)
                    {
                        fechas.Add(model.FechaDesde);
                        fechas.Add(model.FechaHasta);
                    }

                    var datos = servicioCarrera.BuscarCursos(model.Curso, fechas.ToArray());

                    if (datos.ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        HistoricoCursosVM modelo = new HistoricoCursosVM();
                        modelo.Cursos = new List<CCursoDTO>();
                        foreach (CCursoDTO item in datos)
                        {
                            modelo.Cursos.Add(item);
                        }

                        modelo.TotalCursos = modelo.Cursos.Count();
                        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalCursos / 10);
                        int paginaActual = page ?? 1;
                        modelo.PaginaActual = paginaActual;
                        Session["cursos"] = datos.ToList();
                        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalCursos)
                        {
                            modelo.Cursos = modelo.Cursos.GetRange(((paginaActual - 1) * 10), (modelo.TotalCursos) - (((paginaActual - 1) * 10))).ToList(); ;
                        }
                        else
                        {
                            modelo.Cursos = modelo.Cursos.GetRange(((paginaActual - 1) * 10), 10).ToList();
                        }

                        modelo.paramBusqueda = model;

                        return PartialView("_SearchHistoricoCursosResults", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaDesde.Year > 1 || model.FechaHasta.Year > 1)
                    {
                        if (!(model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de Emisión, debe ingresar la fecha -desde- y la fecha -hasta-.");
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
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }

        }

        public ActionResult DetailsHistoricoCursos(int id)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    BusquedaHistoricoCursosVM model = new BusquedaHistoricoCursosVM();
                    try
                    {
                        var datos = servicioCarrera.CargarCursoPorID(id);
                        if (datos.GetType() != typeof(CErrorDTO))
                        {
                            model.Curso = (CCursoDTO)datos;
                        }
                        else
                        {
                            model.Error = (CErrorDTO)datos;
                        }

                        return View(model);
                    }
                    catch (Exception error)
                    {
                        model.Error = new CErrorDTO { MensajeError = error.Message };
                        return View(model);
                    }
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleHistoricoCursos(BusquedaHistoricoCursosVM model)
        {
            List<HistorialCursoRptData> modelo = new List<HistorialCursoRptData>();

            modelo.Add(HistorialCursoRptData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "HistorialCursoRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseHistoricoCurso()
        {
            List<HistorialCursoRptData> modelo = new List<HistorialCursoRptData>();

            List<CBaseDTO> temp = (List<CBaseDTO>)Session["cursos"];

            if (temp != null)
                foreach (CCursoDTO item in temp)
                {
                    BusquedaHistoricoCursosVM aux = new BusquedaHistoricoCursosVM();
                    aux.Curso = item;
                    modelo.Add(HistorialCursoRptData.GenerarDatosReporte(aux, String.Empty));
                }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "DesgloseHistorialCursoRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleHistoricoPuntos(BusquedaHistoricoPuntosVM model)
        {
            List<HistorialPuntosRPTData> modelo = new List<HistorialPuntosRPTData>();

            modelo.Add(HistorialPuntosRPTData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "HistorialPuntosRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseHistoricoPuntos()
        {
            List<HistorialPuntosRPTData> modelo = new List<HistorialPuntosRPTData>();

            List<CBaseDTO> temp = (List<CBaseDTO>)Session["puntos"];

            if (temp != null)
                foreach (CPuntosDTO item in temp)
                {
                    BusquedaHistoricoPuntosVM aux = new BusquedaHistoricoPuntosVM();
                    aux.Puntos = item;
                    modelo.Add(HistorialPuntosRPTData.GenerarDatosReporte(aux, String.Empty));
                }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "DesgloseHistorialPuntosRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }


        [HttpPost]
        public CrystalReportPdfResult ReporteDetalleHistoricoCarrera(BusquedaHistoricoCarreraVM model)
        {
            List<HistorialCarreraRPTData> modelo = new List<HistorialCarreraRPTData>();

            modelo.Add(HistorialCarreraRPTData.GenerarDatosReporte(model, String.Empty));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "HistorialCarreraRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseHistoricoCarrera()
        {
            List<HistorialCarreraRPTData> modelo = new List<HistorialCarreraRPTData>();

            List<CBaseDTO> temp = (List<CBaseDTO>)Session["carreras"];

            if (temp != null)
                foreach (CCarreraProfesionalDTO item in temp)
                {
                    BusquedaHistoricoCarreraVM aux = new BusquedaHistoricoCarreraVM();
                    aux.Carrera = item;
                    modelo.Add(HistorialCarreraRPTData.GenerarDatosReporte(aux, String.Empty));
                }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "DesgloseHistorialCarreraRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }
        #endregion

        #region FuncionarioPolicial
        public ActionResult SearchFuncionarioPolicial()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    return View();
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchFuncionarioPolicial(BusquedaFuncionarioPolicialVM model, string cedula, string codigo,
                                                      string diaDesde, string mesDesde, string annoDesde,
                                                      string diaHasta, string mesHasta, string annoHasta, int? page)
        {
            try
            {

                List<DateTime> fechas = new List<DateTime>();

                if (model == null)
                {
                    model = new BusquedaFuncionarioPolicialVM();
                    model.Funcionario = new CFuncionarioDTO();
                    model.Funcionario.Cedula = cedula;
                    model.DetalleContratacion = new CDetalleContratacionDTO();
                    model.DetalleContratacion.CodigoPolicial = Convert.ToInt32(codigo);

                    if (Convert.ToInt32(annoDesde) > 1 && Convert.ToInt32(annoHasta) > 1)
                    {
                        model.FechaDesde = new DateTime(Convert.ToInt32(annoDesde), Convert.ToInt32(mesDesde), Convert.ToInt32(diaDesde));
                        model.FechaHasta = new DateTime(Convert.ToInt32(annoHasta), Convert.ToInt32(mesHasta), Convert.ToInt32(diaHasta));
                        fechas.Add(model.FechaDesde);
                        fechas.Add(model.FechaHasta);
                    }
                }

                if (model.Funcionario.Cedula != null || (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1) ||
                    model.DetalleContratacion.CodigoPolicial > 0)
                {
                    if (model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1)
                    {
                        fechas.Add(model.FechaDesde);
                        fechas.Add(model.FechaHasta);
                    }

                    CDetalleContratacionDTO temp = new CDetalleContratacionDTO();
                    temp.CodigoPolicial = model.DetalleContratacion.CodigoPolicial;
                    temp.FechaRegimenPolicial = model.DetalleContratacion.FechaRegimenPolicial;
                    temp.Funcionario = new CFuncionarioDTO();
                    temp.Funcionario.Cedula = model.Funcionario.Cedula;
                    temp.Funcionario.Sexo = GeneroEnum.Indefinido;
                    var datos = servicioFuncionario.BuscarDetalleFuncionarioPolicial(temp, fechas.ToArray());


                    if (datos.ElementAt(0).ElementAt(0).GetType().Equals(typeof(CErrorDTO)))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("Sistema", ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                        throw new Exception(((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError);
                    }
                    else
                    {
                        FuncionarioPolicialVM modelo = new FuncionarioPolicialVM();
                        modelo.Funcionarios = new List<BusquedaFuncionarioPolicialVM>();
                        foreach (var item in datos)
                        {
                            BusquedaFuncionarioPolicialVM tempDato = new BusquedaFuncionarioPolicialVM();
                            tempDato.Funcionario = (CFuncionarioDTO)item[0];
                            tempDato.Puesto = (CPuestoDTO)item[1];
                            tempDato.DetallePuesto = (CDetallePuestoDTO)item[2];
                            tempDato.DetalleContratacion = (CDetalleContratacionDTO)item[3];
                            modelo.Funcionarios.Add(tempDato);
                        }

                        modelo.TotalFuncionarios = modelo.Funcionarios.Count();
                        modelo.TotalPaginas = (int)Math.Ceiling((double)modelo.TotalFuncionarios / 10);
                        int paginaActual = page ?? 1;
                        modelo.PaginaActual = paginaActual;
                        Session["funcionariosPoliciales"] = datos;
                        if ((((paginaActual - 1) * 10) + 10) > modelo.TotalFuncionarios)
                        {
                            modelo.Funcionarios = modelo.Funcionarios.GetRange(((paginaActual - 1) * 10), (modelo.TotalFuncionarios) - (((paginaActual - 1) * 10))).ToList(); ;
                        }
                        else
                        {
                            modelo.Funcionarios = modelo.Funcionarios.GetRange(((paginaActual - 1) * 10), 10).ToList();
                        }

                        modelo.paramBusqueda = model;

                        return PartialView("_SearchFuncionarioPolicialResults", modelo);
                    }
                }
                else
                {
                    ModelState.Clear();
                    if (model.FechaDesde.Year > 1 || model.FechaHasta.Year > 1)
                    {
                        if (!(model.FechaDesde.Year > 1 && model.FechaHasta.Year > 1))
                        {
                            ModelState.AddModelError("Busqueda", "Para realizar una búsqueda con rango de fechas de ingreso, debe ingresar la fecha -desde- y la fecha -hasta-.");
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
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }
        }

        [HttpPost]
        public CrystalReportPdfResult ReporteDesgloseFuncionarioPolicial()
        {
            List<FuncionarioPolicialRPTData> modelo = new List<FuncionarioPolicialRPTData>();

            CBaseDTO[][] temp = (CBaseDTO[][])Session["funcionariosPoliciales"];

            if (temp != null)
                foreach (var item in temp)
                {
                    BusquedaFuncionarioPolicialVM tempDato = new BusquedaFuncionarioPolicialVM();
                    tempDato.Funcionario = (CFuncionarioDTO)item[0];
                    tempDato.Puesto = (CPuestoDTO)item[1];
                    tempDato.DetallePuesto = (CDetallePuestoDTO)item[2];
                    tempDato.DetalleContratacion = (CDetalleContratacionDTO)item[3];
                    modelo.Add(FuncionarioPolicialRPTData.GenerarDatosReporte(tempDato, String.Empty));
                }

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "FuncionarioPolicialRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }
        #endregion

        #region DetallePuntosAdicionales
        public ActionResult CreateDetallePuntos(string error)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    ModelState.Clear();
                    DetallePuntosAdicionalesVM model = new DetallePuntosAdicionalesVM();
                    if (error != null)
                        model.Error = new CErrorDTO { MensajeError = error };
                    model.Funcionario = new CFuncionarioDTO();
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult CreateDetallePuntos(DetallePuntosAdicionalesVM model, string SubmitButton)
        {
            try
            {
                if (SubmitButton == "Buscar")
                {
                    var datosFuncionario = new List<CBaseDTO>();

                    if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null)
                    {
                        datosFuncionario = servicioFuncionario.BuscarFuncionarioPolicial(model.Funcionario.Cedula).ToList();
                    }
                    else
                    {
                        if (Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null)
                        {
                            datosFuncionario = servicioFuncionario.BuscarFuncionarioProfesional(model.Funcionario.Cedula).ToList();
                        }
                        else
                        {
                            datosFuncionario = servicioFuncionario.BuscarFuncionarioDetallePuesto(model.Funcionario.Cedula).ToList();
                        }
                    }


                    if (datosFuncionario.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        var datosCarrera = servicioCarrera.BuscarDatosCarreraCedula(model.Funcionario.Cedula);
                        model.Funcionario = (CFuncionarioDTO)datosFuncionario[0];
                        model.PuntosAdicionales = Convert.ToInt32(datosCarrera[0].Mensaje);
                        model.Puesto = (CPuestoDTO)datosFuncionario[1];
                        model.DetallePuesto = (CDetallePuestoDTO)datosFuncionario[2];
                        model.DetalleContratacion = (CDetalleContratacionDTO)datosFuncionario[3];


                    }
                    else
                    {
                        ModelState.AddModelError("Busqueda",
                                ((CErrorDTO)datosFuncionario.FirstOrDefault()).MensajeError);
                        throw new Exception("No se encontró ningún resultado");
                    }
                }
                else
                {
                    if (model.Puntos != 0 && model.numDoc != null && model.Observaciones != null)
                    {
                        var resultado = servicioCarrera.AgregarPuntosAdicionales(model.Funcionario.Cedula, model.Puntos, model.Observaciones, model.numDoc);
                        if (resultado.GetType() != typeof(CErrorDTO))
                        {
                            return RedirectToAction("DetailsPuntosAdicionales", new { cedula = model.Funcionario.Cedula, puntos = model.Puntos, accion = "guardar" });
                        }
                        else
                        {
                            ModelState.AddModelError("Agregar", ((CErrorDTO)resultado).MensajeError);
                            throw new Exception(((CErrorDTO)resultado).MensajeError);
                        }
                    }
                    else
                    {
                        throw new Exception("Debe ingresar los datos solicitados");
                    }
                }
                model.Error = null;
                return PartialView("_FormularioDetallePuntos", model);
            }
            catch (Exception error)
            {
                ModelState.Clear();
                ModelState.AddModelError("Sistema", error.Message);
                model.Error = new CErrorDTO { MensajeError = error.Message };
                return RedirectToAction("CreateDetallePuntos", new { error = error.Message }); //Se podría manejar mejor
            }
        }

        public ActionResult DetailsPuntosAdicionales(string cedula, int puntos)
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    DetallePuntosAdicionalesVM model = new DetallePuntosAdicionalesVM();
                    model.Funcionario = new CFuncionarioDTO();
                    model.Funcionario.Cedula = cedula;
                    model.Puntos = puntos;
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }



        #endregion

        #region Resoluciones
        public ActionResult CreateResolucion()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    GenerarResolucionVM model = new GenerarResolucionVM();
                    model.FuncionarioBusqueda = new CFuncionarioDTO();
                    model.ListaCursos = new List<CursoResolucionVM>();
                    model.CursosSeleccionados = new List<CursoResolucionVM>();
                    Session.Remove("cursosSeleccionados");
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult CreateResolucion(GenerarResolucionVM model, string SubmitButton)
        {
            try
            {

                model.Error = null;

                List<CursoResolucionVM> sesion = (List<CursoResolucionVM>)Session["cursosSeleccionados"];
                if (sesion != null && sesion.Count() > 0)
                    model.CursosSeleccionados = sesion;

                switch (SubmitButton)
                {
                    case "Buscar":
                        if (model.NumResolucion != null && model.FechaRige.Year > 1 && model.FechaVence.Year > 1)
                        {
                            if (model.FuncionarioBusqueda.Cedula != null)
                            {

                                List<CBaseDTO> datos = servicioFuncionario.CargarDetalleContratacionFuncionario(model.FuncionarioBusqueda.Cedula).ToList();

                                CFuncionarioDTO funcionario = (CFuncionarioDTO)datos[0];
                                CDetalleContratacionDTO detalle = (CDetalleContratacionDTO)datos[1];

                                var datosCarrera = servicioCarrera.BuscarDatosCarreraCedula(funcionario.Cedula);

                                var puntosAdicionales = Convert.ToInt32(datosCarrera[0].Mensaje);
                                funcionario.Mensaje = puntosAdicionales.ToString();

                                var capacitacion = datosCarrera.Where(Q => Q.GetType() == typeof(CCursoCapacitacionDTO)
                                                                        && ((CCursoCapacitacionDTO)Q).Estado != 3
                                                                        && ((CCursoCapacitacionDTO)Q).Estado != 1).ToList();

                                model.ListaCursos = new List<CursoResolucionVM>();
                                foreach (CCursoCapacitacionDTO item in capacitacion)
                                {
                                    CursoResolucionVM curso = new CursoResolucionVM();
                                    curso.Funcionario = funcionario;
                                    curso.Detalle = detalle;
                                    curso.Curso = item;
                                    model.ListaCursos.Add(curso);
                                }
                            }
                        }
                        else
                        {
                            model.Error = new CErrorDTO { MensajeError = "Debe ingresar los datos de la resolución" };
                        }
                        break;
                    case "Agregar":
                        if (model.CursoSeleccionado > model.ListaCursos.Count() || model.CursoSeleccionado <= 0)
                            model.Error = new CErrorDTO { MensajeError = "El número indicado no es válido" };
                        else
                        {
                            CursoResolucionVM c = model.ListaCursos[model.CursoSeleccionado - 1];
                            if (model.CursosSeleccionados.Any(X => X.Curso.IdEntidad == c.Curso.IdEntidad))
                                model.Error = new CErrorDTO { MensajeError = "El curso ya fue seleccionado" };
                            else if (model.CursosSeleccionados.Count > 0)
                            {
                                if (c.Detalle.CodigoPolicial > 0 && model.CursosSeleccionados.All(X => X.Detalle.CodigoPolicial > 0))
                                    model.CursosSeleccionados.Add(c);
                                else if (c.Detalle.CodigoPolicial == 0 && model.CursosSeleccionados.All(X => X.Detalle.CodigoPolicial == 0))
                                    model.CursosSeleccionados.Add(c);
                                else
                                    model.Error = new CErrorDTO { MensajeError = "Solo puede incluir cursos de un tipo de funcionario (policial o profesional)" };
                            }
                            else
                                model.CursosSeleccionados.Add(c);

                        }
                        break;
                    case "Remover":
                        if (model.CursoSeleccionado > model.CursosSeleccionados.Count() || model.CursoSeleccionado <= 0)
                            model.Error = new CErrorDTO { MensajeError = "El número indicado no es válido" };
                        else
                        {
                            model.CursosSeleccionados.RemoveAt(model.CursoSeleccionado - 1);
                        }
                        break;
                    default:
                        model.Error = new CErrorDTO { MensajeError = "Ha ocurrido un error inesperado" };
                        break;
                }
                Session["cursosSeleccionados"] = model.CursosSeleccionados;
                return PartialView("_FormularioGenerarResolucion", model);
            }
            catch (Exception error)
            {

                model.Error = new CErrorDTO { MensajeError = error.Message };
                return PartialView("_FormularioGenerarResolucion", model);
            }
        }

        [HttpPost]
        public ActionResult GenerarResolucion(GenerarResolucionVM model)
        {
            try
            {
                if (model.CursosSeleccionados.Count() > 0)
                {
                    bool oficial;
                    if (model.CursosSeleccionados.All(X => X.Detalle.CodigoPolicial > 0))
                        oficial = true;
                    else
                        oficial = false;


                    List<GenerarResolucionRptData> modelo = new List<GenerarResolucionRptData>();

                    CPeriodoEscalaSalarialDTO valorPunto = (CPeriodoEscalaSalarialDTO)servicioCarrera.ObtenerValorPunto();

                    List<CursoResolucionVM> cursos = model.CursosSeleccionados.GroupBy(X => X.Funcionario.Cedula).Select(F => F.First()).ToList();

                    List<string> cedulas = model.CursosSeleccionados.Select(X => X.Funcionario.Cedula).Distinct().ToList();
                    List<decimal> puntosAdicionales = CalcularPuntosadicionales(model.CursosSeleccionados, cedulas, oficial);

                    int i = 0;
                    foreach (CursoResolucionVM c in cursos)
                    {
                        CDetallePuntosDTO puntos = (CDetallePuntosDTO)servicioCarrera.CalcularPuntosFuncionario(c.Funcionario.Cedula);
                        CSalarioDTO salario = new CSalarioDTO();
                        if (oficial)
                            salario = (CSalarioDTO)servicioFuncionario.BuscarFuncionarioSalario(cedulas[i])[1];

                        FuncionarioResolucionVM fun = new FuncionarioResolucionVM();
                        fun.Funcionario = c.Funcionario;
                        fun.ValorPunto = oficial ? 0 : valorPunto.MontoPuntoCarrera;
                        fun.PuntosActuales = puntos.TotalPuntos;
                        fun.PuntosAdicionales = puntosAdicionales[i++];
                        fun.PuntosTotales = fun.PuntosActuales + fun.PuntosAdicionales;
                        fun.FecRige = model.FechaRige;
                        fun.FecVence = model.FechaVence;
                        fun.SalarioBase = oficial ? salario.MtoTotal : 0;
                        fun.MontoPagar = oficial ? fun.PuntosAdicionales / 100 * fun.SalarioBase : fun.PuntosTotales * fun.ValorPunto;
                        fun.MontoTotal = oficial ? fun.PuntosTotales / 100 * fun.SalarioBase : 0;
                        GenerarResolucionRptData dato = GenerarResolucionRptData.GenerarDatosReporte(fun);
                        modelo.Add(dato);
                    }

                    var actualizacion = servicioCarrera.AsignarResolucion(model.CursosSeleccionados.Select(C => C.Curso).ToArray(), model.NumResolucion);

                    if (actualizacion.FirstOrDefault().GetType() != typeof(CErrorDTO))
                    {
                        //Bitácora de generación de resolución, estado de cursos pasa a 1 y se les pone numero de resolución
                        context.GuardarBitacora(principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera),
                        Convert.ToInt32(EAccionesBitacora.GeneracionResolucion), 0,
                        CAccesoWeb.ListarEntidades(typeof(CFormacionAcademicaDTO).Name));

                        string reportPath;
                        if (oficial)
                            reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "GenerarResolucionRPT.rpt");
                        else
                            reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "GenerarResolucionProfesionalRPT.rpt");

                        return new CrystalReportPdfResult(reportPath, modelo, "WORD");
                    }
                    else
                    {
                        model.Error = new CErrorDTO { MensajeError = "Ha ocurrido un error al generar la resolución" };
                        return PartialView("_FormularioGenerarResolucion", model);
                    }
                }
                else
                {
                    model.Error = new CErrorDTO { MensajeError = "No ha seleccionado ningún curso" };
                    return PartialView("_FormularioGenerarResolucion", model);
                }
            }
            catch (Exception error)
            {
                model.Error = new CErrorDTO { MensajeError = error.Message };
                return PartialView("_FormularioGenerarResolucion", model);
            }

        }

        private List<decimal> CalcularPuntosadicionales(List<CursoResolucionVM> cursos, List<string> cedulas, bool oficial)
        {
            List<decimal> resultados = new List<decimal>();
            foreach (string cedula in cedulas)
            {
                List<CCursoCapacitacionDTO> temp = cursos.Where(F => F.Funcionario.Cedula == cedula).Select(X => X.Curso).ToList();
                decimal resultado = 0;
                if (oficial)
                {
                    //Aprovechamiento
                    decimal HorasAprovechamiento = temp.Where(X => (X.Modalidad.IdEntidad == 1 || X.Modalidad.IdEntidad == 10) && X.TotalHoras >= 12).Sum(S => S.TotalHoras);
                    decimal PuntosAprovechamiento = Math.Floor(HorasAprovechamiento / 40);
                    //decimal HorasExcAprovechamiento = HorasAprovechamiento - (PuntosAprovechamiento * 40);

                    //Participacion
                    decimal HorasParticipacion = temp.Where(X => (X.Modalidad.IdEntidad == 2 || X.Modalidad.IdEntidad == 11) && X.TotalHoras >= 30).Sum(S => S.TotalHoras);
                    decimal PuntosParticipacion = Math.Floor(HorasParticipacion / 80);
                    //decimal HorasExcParticipacion = HorasParticipacion - (PuntosParticipacion * 80);

                    resultado = PuntosAprovechamiento + PuntosParticipacion;

                }
                else
                {
                    //Aprovechamiento
                    decimal HorasAprovechamiento = temp.Where(X => X.Modalidad.IdEntidad == 1 || X.Modalidad.IdEntidad == 10).Sum(S => S.TotalHoras);
                    decimal PuntosAprovechamiento = Math.Floor(HorasAprovechamiento / 40);
                    //decimal HorasExcAprovechamiento = HorasAprovechamiento - (PuntosAprovechamiento * 40);

                    //Participacion
                    decimal HorasParticipacion = temp.Where(X => X.Modalidad.IdEntidad == 2 || X.Modalidad.IdEntidad == 11).Sum(S => S.TotalHoras);
                    decimal PuntosParticipacion = Math.Floor(HorasParticipacion / 80);
                    //decimal HorasExcParticipacion = HorasParticipacion - (PuntosParticipacion * 80);

                    //Instruccion
                    decimal HorasInstruccion = temp.Where(X => X.Modalidad.IdEntidad == 3).Sum(S => S.TotalHoras);
                    decimal PuntosInstruccion = Math.Floor(HorasInstruccion / 24);

                    resultado = PuntosAprovechamiento + PuntosParticipacion + PuntosInstruccion;
                }
                resultados.Add(resultado);
            }
            return resultados;
        }


        #endregion

        #region Buscar experiencia
        public ActionResult SearchExperiencia()
        {
            context.IniciarSesionModulo(Session, principal.Identity.Name, Convert.ToInt32(EModulosHelper.Carrera), 0);

            if (Session["Perfil_" + Convert.ToInt32(EModulosHelper.Carrera)].ToString().StartsWith("Error"))
            {
                return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
            }
            else
            {
                if (Convert.ToBoolean(Session["Administrador_Global"]) ||
                    Convert.ToBoolean(Session["Administrador_" + Convert.ToInt32(EModulosHelper.Carrera)]) ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Consulta))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Policial))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Profesional))] != null ||
                    Session[CAccesoWeb.GenerarCadenaPermiso(EModulosHelper.Carrera, Convert.ToInt32(ENivelesCarrera.Experiencia))] != null)
                {
                    BusquedaExperienciaVM model = new BusquedaExperienciaVM();
                    return View(model);
                }
                else
                {
                    CAccesoWeb.CargarErrorAcceso(Session);
                    return RedirectToAction("Index", "Error", new { modulo = Convert.ToInt32(EModulosHelper.Carrera) });
                }
            }
        }

        [HttpPost]
        public ActionResult SearchExperiencia(BusquedaExperienciaVM model)
        {
            try
            {
                var datos = servicioCarrera.BuscarDatosCarreraCedula(model.Cedula);
                var puntos = servicioCarrera.CalcularPuntosFuncionario(model.Cedula);
                var montoPunto = servicioCarrera.ObtenerValorPunto();
                var calificacion = servicioFuncionario.CargarCalificacionActual(model.Cedula);

                model.Funcionario = (CFuncionarioDTO)datos[0];
                model.Puesto = (CPuestoDTO)datos[1];
                model.Detalle = (CDetalleContratacionDTO)datos[2];

                model.Periodo = (CPeriodoEscalaSalarialDTO)montoPunto;

                if (calificacion.GetType() == typeof(CErrorDTO))
                    model.Calificacion = new CCalificacionDTO { DesCalificacion = "NO INDICA" };
                else
                    model.Calificacion = (CCalificacionDTO)calificacion;

                model.Puntos = (CDetallePuntosDTO)puntos;



                return PartialView("_SearchExperiencia", model);
            }
            catch (Exception error)
            {
                if (error.Message != "Busqueda")
                {
                    return PartialView("_ErrorCarrera");
                }
                else
                {
                    return PartialView("_ErrorCarrera");
                }
            }

        }

        [HttpPost]
        public CrystalReportPdfResult ReporteExperienciaFuncionario(BusquedaExperienciaVM model)
        {
            List<ExperienciaRPTData> modelo = new List<ExperienciaRPTData>();
            modelo.Add(ExperienciaRPTData.GenerarDatosReporte(model));

            string reportPath = Path.Combine(Server.MapPath("~/Reports/Carrera"), "ExperienciaRPT.rpt");
            return new CrystalReportPdfResult(reportPath, modelo, "PDF");
        }

        #endregion
    }
}