using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPrestacionLegalL
    {

        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPrestacionLegalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos 
        
        internal static CPrestacionLegalDTO ConvertirDatosPrestacionADto(PrestacionLegal dato)
        {
            List<CDetallePrestacionCuadroDTO> listaSalario = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaAguinaldo = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaSalarioEscolar = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaVacacionesAcumuladas = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaVacacionesProporcionales = new List<CDetallePrestacionCuadroDTO>();

            List<CDetallePrestacionAfiliacionDTO> listaAfiliacion = new List<CDetallePrestacionAfiliacionDTO>();
            List<CDetallePrestacionDTO> detalle = new List<CDetallePrestacionDTO> ();

            if (dato.DetallePrestacion.FirstOrDefault() != null)
            {
                var datoDetalle = dato.DetallePrestacion.FirstOrDefault();
                detalle.Add(new CDetallePrestacionDTO
                {
                    IdEntidad = datoDetalle.PK_DetallePrestacion,
                    NomJefe = datoDetalle.NomJefe,
                    DesPuesto = datoDetalle.DesPuestoJefatura
                });
            }

            CDetallePrestacionCuadroDTO detalleCuadro = new CDetallePrestacionCuadroDTO();
            foreach (var item in dato.DetallePrestacionCuadro)
            {
                detalleCuadro = new CDetallePrestacionCuadroDTO
                {
                    FecPeriodo = item.FecPeriodo,
                    MtoSalario = item.MtoSalario,
                    MtoExtra = item.MtoExtras,
                    MtoFeriado = item.MtoFeriados,
                    MtoTotal = item.MtoTotal,
                    MtoSalarioEscolar = item.MtoEscolar
                };

                switch (item.FK_Tipo)
                {
                    case 1:
                        listaSalario.Add(detalleCuadro);
                        break;
                    case 2:
                        listaAguinaldo.Add(detalleCuadro);
                        break;
                    case 3:
                        listaSalarioEscolar.Add(detalleCuadro);
                        break;
                    case 4:
                        listaVacacionesAcumuladas.Add(detalleCuadro);
                        break;
                    case 5:
                        listaVacacionesProporcionales.Add(detalleCuadro);
                        break;
                }    
            }
           
            foreach (var item in dato.DetallePrestacionAfiliacion)
            {
                listaAfiliacion.Add(new CDetallePrestacionAfiliacionDTO
                {
                    IdEntidad = item.PK_Detalle,
                    DesAfiliacion = item.DesAfiliacion,
                    MtoAfiliacion = item.MtoAfiliacion
                });
            }

            return new CPrestacionLegalDTO
            {
                IdEntidad = dato.PK_PrestacionLegal,
                FecCreacion = Convert.ToDateTime(dato.FecCreacion),
                IndEstado = Convert.ToInt32(dato.IndEstado),
                NumPrestacion = dato.NumPrestacion,
                TipoPrestacion = new CTipoPrestacionDTO
                {
                    IdEntidad = dato.TipoPrestacion.PK_TipoPrestacion,
                    DesPrestacion = dato.TipoPrestacion.DesPrestacion
                },
                Expediente = CExpedienteL.ConvertirExpedienteADTO(dato.Nombramiento.Funcionario.ExpedienteFuncionario.FirstOrDefault()),
                DetalleContratacion = CDetalleContratacionL.ConvertirDetalleContratacionADTO(dato.Nombramiento.Funcionario.DetalleContratacion.FirstOrDefault()),
                Detalle = detalle,
                ListaSalario = listaSalario,
                ListaAguinaldo = listaAguinaldo,
                ListaSalarioEscolar = listaSalarioEscolar,
                ListaVacacionesAcumuladas = listaVacacionesAcumuladas,
                ListaVacacionesProporcionales = listaVacacionesProporcionales,
                ListaAfiliacion = listaAfiliacion
            };
        }

        internal static string DefinirEstadoPrestacion(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Activa";
                    break;
                case 2:
                    respuesta = "Anulada";
                    break;
                default:
                    respuesta = "Indefinido";
                    break;
            }
            return respuesta;
        }

        public CBaseDTO AgregarPrestacionLegal(CNombramientoDTO nombramiento, CTipoPrestacionDTO tipo,CPrestacionLegalDTO prestacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CPrestacionLegalD intermedio = new CPrestacionLegalD(contexto);
                CTipoPrestacionD intermedioTipo= new CTipoPrestacionD(contexto);
                CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);

                PrestacionLegal datosPrestacion = new PrestacionLegal
                {
                   FecCreacion=DateTime.Now,
                   IndEstado=prestacion.IndEstado,
                   NumPrestacion=prestacion.NumPrestacion
                };

                List<DetallePrestacion> detalle = new List<DetallePrestacion>();
                List<DetallePrestacionAfiliacion> detalleAfiliacion = new List<DetallePrestacionAfiliacion>();
                List<DetallePrestacionCuadro> detalleCuadro = new List<DetallePrestacionCuadro>();


                foreach(var item in prestacion.Detalle)
                {
                    var entidadPresupuesto = intermedioPresupuesto.CargarPresupuestoPorID(item.Presupuesto.IdEntidad);
                    if (entidadPresupuesto == null)
                    {
                        throw new Exception("No existe el código presupuestario");
                    }

                    detalle.Add(new DetallePrestacion
                    {
                        FK_Presupuesto = entidadPresupuesto.PK_Presupuesto,
                        FK_Division = item.Division.IdEntidad,
                        FK_DireccionGeneral = item.DireccionGeneral.IdEntidad,
                        FK_Departamento = item.Departamento.IdEntidad,
                        FK_Seccion = item.Seccion.IdEntidad,
                        FK_AccionIngreso = item.AccionIngreso.IdEntidad,
                        FK_AccionCese = item.AccionCese.IdEntidad,
                        NomJefe = item.NomJefe,
                        DesPuestoJefatura = item.DesPuesto,
                        FK_Usuario = item.Usuario.IdEntidad,
                    });
                }

                if (prestacion.ListaAfiliacion != null)
                    foreach (var item in prestacion.ListaAfiliacion)
                    {
                        detalleAfiliacion.Add(new DetallePrestacionAfiliacion
                        {
                            DesAfiliacion = item.DesAfiliacion,
                            MtoAfiliacion = item.MtoAfiliacion
                        });
                    }

                if (prestacion.ListaSalario != null)
                    foreach (var item in prestacion.ListaSalario)
                    {
                        detalleCuadro.Add(new DetallePrestacionCuadro 
                        {
                            FK_Tipo = 1,
                            FecPeriodo = item.FecPeriodo,
                            MtoSalario = item.MtoSalario,
                            MtoExtras = item.MtoExtra,
                            MtoFeriados = item.MtoFeriado,
                            MtoEscolar = item.MtoSalarioEscolar,
                            MtoTotal = item.MtoTotal,
                        });
                    }

                if (prestacion.ListaAguinaldo != null)
                    foreach (var item in prestacion.ListaAguinaldo)
                    {
                        detalleCuadro.Add(new DetallePrestacionCuadro
                        {
                            FK_Tipo = 2,
                            FecPeriodo = item.FecPeriodo,
                            MtoSalario = item.MtoSalario,
                            MtoExtras = item.MtoExtra,
                            MtoFeriados = item.MtoFeriado,
                            MtoEscolar = item.MtoSalarioEscolar,
                            MtoTotal = item.MtoTotal,
                        });
                    }

                if (prestacion.ListaSalarioEscolar != null)
                    foreach (var item in prestacion.ListaSalarioEscolar)
                    {
                        detalleCuadro.Add(new DetallePrestacionCuadro
                        {
                            FK_Tipo = 3,
                            FecPeriodo = item.FecPeriodo,
                            MtoSalario = item.MtoSalario,
                            MtoExtras = item.MtoExtra,
                            MtoFeriados = item.MtoFeriado,
                            MtoEscolar = item.MtoSalarioEscolar,
                            MtoTotal = item.MtoTotal,
                        });
                    }

                if (prestacion.ListaVacacionesAcumuladas != null)
                    foreach (var item in prestacion.ListaVacacionesAcumuladas)
                    {
                        detalleCuadro.Add(new DetallePrestacionCuadro
                        {
                            FK_Tipo = 4,
                            FecPeriodo = item.FecPeriodo,
                            MtoSalario = item.MtoSalario,
                            MtoExtras = item.MtoExtra,
                            MtoFeriados = item.MtoFeriado,
                            MtoEscolar = item.MtoSalarioEscolar,
                            MtoTotal = item.MtoTotal,
                        });
                    }

                if (prestacion.ListaVacacionesProporcionales != null)
                    foreach (var item in prestacion.ListaVacacionesProporcionales)
                    {
                        detalleCuadro.Add(new DetallePrestacionCuadro
                        {
                            FK_Tipo = 5,
                            FecPeriodo = item.FecPeriodo,
                            MtoSalario = item.MtoSalario,
                            MtoExtras = item.MtoExtra,
                            MtoFeriados = item.MtoFeriado,
                            MtoEscolar = item.MtoSalarioEscolar,
                            MtoTotal = item.MtoTotal,
                        });
                    }

                var entidadTipos = intermedioTipo.ObtenerTipoPrestacion(tipo.IdEntidad);

                if (entidadTipos.Codigo != -1)
                {
                    datosPrestacion.TipoPrestacion = (TipoPrestacion)entidadTipos.Contenido;
                }
                else
                {
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)respuesta).Contenido).MensajeError);
                }

                var entidadNombramiento = intermedioNombramiento.CargarNombramiento(nombramiento.IdEntidad);

                if (entidadNombramiento.PK_Nombramiento != -1)
                {
                    datosPrestacion.Nombramiento = entidadNombramiento;
                }
                else
                {
                    //respuesta = (CErrorDTO)((CRespuestaDTO)entidadNombramiento).Contenido;
                    respuesta = (CErrorDTO)new CBaseDTO { IdEntidad = Convert.ToInt32(entidadNombramiento.PK_Nombramiento) };
                    throw new Exception();
                }

                datosPrestacion.DetallePrestacion = detalle;
                datosPrestacion.DetallePrestacionAfiliacion = detalleAfiliacion;
                datosPrestacion.DetallePrestacionCuadro = detalleCuadro;
                respuesta = intermedio.AgregarPrestacionLegal(datosPrestacion);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)respuesta).Contenido).MensajeError);
                }
                else
                {
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = (error.InnerException != null ? error.InnerException.Message : "")
                };

                return respuesta;
            }
        }
                
        public CBaseDTO AnularPrestacion(CPrestacionLegalDTO prestacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CPrestacionLegalD intermedio = new CPrestacionLegalD(contexto);

                PrestacionLegal prestacionBD = new PrestacionLegal
                {
                    PK_PrestacionLegal = prestacion.IdEntidad,
                  
                };

                var datosPrestacion = intermedio.AnularPrestacion(prestacionBD);

                if (datosPrestacion.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = prestacion.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosPrestacion.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public CBaseDTO ObtenerPrestacionLegal(CPrestacionLegalDTO prestacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CPrestacionLegalD intermedio = new CPrestacionLegalD(contexto);
                CPresupuestoD intermedioPresupuesto = new CPresupuestoD(contexto);
                CDivisionD intermedioDivision = new CDivisionD(contexto);
                CDireccionGeneralD intermedioDireccion = new CDireccionGeneralD(contexto);
                CDepartamentoD intermedioDepartamento = new CDepartamentoD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);
                CAccionPersonalD intermedioAccion = new CAccionPersonalD(contexto);
                CUsuarioD intermedioUsuario= new CUsuarioD(contexto);
                CDetalleContratacionD intermedioDetalle = new CDetalleContratacionD(contexto);

                var resultado = intermedio.ObtenerPrestacion(prestacion);

                if (resultado.Codigo > 0)
                {
                    var datoPrestacion = (PrestacionLegal)resultado.Contenido;
                    var dato = ConvertirDatosPrestacionADto(datoPrestacion);
                    
                    dato.Nombramiento = CNombramientoL.ConvertirDatosNombramientoADTO(datoPrestacion.Nombramiento);
                    dato.Nombramiento.Funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(datoPrestacion.Nombramiento.Funcionario);
                    dato.Nombramiento.Puesto = CPuestoL.ConvertirCPuestoGeneralDatosaDTO(datoPrestacion.Nombramiento.Puesto);
                    var dp = datoPrestacion.Nombramiento.Puesto.DetallePuesto.Where(Q => Q.FK_Nombramiento == dato.Nombramiento.IdEntidad).FirstOrDefault();
                    if(dp != null) {
                        dato.Nombramiento.Puesto.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(dp);
                    }
                    else
                    {
                        dp = datoPrestacion.Nombramiento.Puesto.DetallePuesto.LastOrDefault();
                        if (dp != null)
                            dato.Nombramiento.Puesto.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(dp);
                        else
                            dp = new DetallePuesto();
                    }

                    dato.Nombramiento.Puesto.DetallePuesto = CDetallePuestoL.ConstruirDetallePuesto(dp);

                    if (datoPrestacion.DetallePrestacion.FirstOrDefault() != null)
                    {
                        var detalle = datoPrestacion.DetallePrestacion.FirstOrDefault();
                        dato.Detalle.FirstOrDefault().Presupuesto = CPresupuestoL.ConvertirPresupuestoDatosaDTO(intermedioPresupuesto.CargarPresupuestoPorID(detalle.FK_Presupuesto));
                        dato.Detalle.FirstOrDefault().Division = CDivisionL.ConvertirDivisionADTO(intermedioDivision.CargarDivisionPorID(Convert.ToInt32(detalle.FK_Division)));
                        dato.Detalle.FirstOrDefault().DireccionGeneral = CDireccionGeneralL.ConvertirDireccionGeneralADTO(intermedioDireccion.CargarDireccionGeneralPorID(Convert.ToInt32(detalle.FK_DireccionGeneral)));
                        if (detalle.FK_Departamento != null)
                            dato.Detalle.FirstOrDefault().Departamento = CDepartamentoL.ConvertirDepartamentoADTO(intermedioDepartamento.CargarDepartamentoPorID(Convert.ToInt32(detalle.FK_Departamento)));
                        else
                            dato.Detalle.FirstOrDefault().Departamento = new CDepartamentoDTO();

                        if (detalle.FK_Seccion != null)
                            dato.Detalle.FirstOrDefault().Seccion = CSeccionL.ConvertirSeccionADTO(intermedioSeccion.CargarSeccionPorID(Convert.ToInt32(detalle.FK_Seccion)));
                        else
                            dato.Detalle.FirstOrDefault().Seccion = new CSeccionDTO();

                        if (detalle.FK_Usuario > 0)
                        {
                            var usuario = intermedio.CargarUsuarioPorID(detalle.FK_Usuario);
                            dato.Detalle.FirstOrDefault().Usuario = new CUsuarioDTO
                            {
                                IdEntidad = usuario.PK_Usuario,
                                NombreUsuario = usuario.DetalleAcceso.FirstOrDefault().Funcionario.NomFuncionario.TrimEnd() + " " +
                                                usuario.DetalleAcceso.FirstOrDefault().Funcionario.NomPrimerApellido.TrimEnd() + " " +
                                                usuario.DetalleAcceso.FirstOrDefault().Funcionario.NomSegundoApellido.TrimEnd()
                            };
                        }
                        else
                        {
                            dato.Detalle.FirstOrDefault().Usuario = new CUsuarioDTO { IdEntidad = 0, NombreUsuario = "" };
                        }


                        if (detalle.FK_AccionIngreso > 0)
                            dato.Detalle.FirstOrDefault().AccionIngreso = CAccionPersonalL.ConvertirDatosADto(intermedio.CargarAccionPorID(Convert.ToInt32(detalle.FK_AccionIngreso)));
                        else
                            dato.Detalle.FirstOrDefault().AccionIngreso = new CAccionPersonalDTO() { IdEntidad = 0, NumAccion = "sin número" };

                        if (detalle.FK_AccionCese > 0)
                            dato.Detalle.FirstOrDefault().AccionCese = CAccionPersonalL.ConvertirDatosADto(intermedio.CargarAccionPorID(Convert.ToInt32(detalle.FK_AccionCese)));
                        else
                            dato.Detalle.FirstOrDefault().AccionCese = new CAccionPersonalDTO() { IdEntidad = 0, NumAccion = "sin número" };
                    }
                    return dato;
                }
                else
                {                    
                    respuesta = (CErrorDTO)resultado.Contenido;
                    throw new Exception();
                }
            }
            catch
            {
                return respuesta;
            }
        }

        public List<CBaseDTO> CalcularPrestacion(string cedula)
        {
            DateTime fechaCese;
            DateTime fechaVacaciones;
            DateTime fechaVacacionesAcum = new DateTime();
            DateTime fechaInicio;
            DateTime fechaFinal;
            int numAnualidades = 0;

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<CDetallePrestacionCuadroDTO> listaSalario = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaAguinaldo = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaSalarioEscolar = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaVacacionesAcumuladas = new List<CDetallePrestacionCuadroDTO>();
            List<CDetallePrestacionCuadroDTO> listaVacacionesProporcionales = new List<CDetallePrestacionCuadroDTO>();

            CPrestacionLegalD intermedio = new CPrestacionLegalD(contexto);

            CPuestoL intermedioPuesto = new CPuestoL();
            CAccionPersonalL intermedioAcciones = new CAccionPersonalL();
            CDesgloseSalarialL intermedioDesglose = new CDesgloseSalarialL();
            CHistoricoPlanillaL intermediohistoricoPlanilla = new CHistoricoPlanillaL();
            CPeriodoVacacionesL intermedioVacaciones = new CPeriodoVacacionesL();
            CExpedienteL intermedioExpediente = new CExpedienteL();

            CPrestacionLegalDTO Prestacion = new CPrestacionLegalDTO {
                Detalle = new List<CDetallePrestacionDTO>(),
                //Acciones = new List<CAccionPersonalDTO>(),
                ListaSalario = new List<CDetallePrestacionCuadroDTO>(),
                ListaAguinaldo = new List<CDetallePrestacionCuadroDTO>(),
                ListaSalarioEscolar = new List<CDetallePrestacionCuadroDTO>(),
                ListaVacacionesAcumuladas = new List<CDetallePrestacionCuadroDTO>(),
                ListaVacacionesProporcionales = new List<CDetallePrestacionCuadroDTO>(),
                ListaAfiliacion = new List<CDetallePrestacionAfiliacionDTO>(),
                Vacaciones = new List<CPeriodoVacacionesDTO>()
            };

            CDetallePrestacionDTO detalle = new CDetallePrestacionDTO();

            try
            {
                var datos = intermedioPuesto.DescargarPerfilPuestoAccionesFuncionario(cedula);
                if (datos.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                {
                    var datoNombramiento = new CNombramientoDTO();

                    datoNombramiento = (CNombramientoDTO)datos.ElementAt(1).ElementAt(0);
                    datoNombramiento.Funcionario = (CFuncionarioDTO)datos.ElementAt(0).ElementAt(0);
                    // En datoNombramientoFuncionario.Mensaje, se encuentra el número de expediente.
                    datoNombramiento.Puesto = (CPuestoDTO)datos.ElementAt(3).ElementAt(0);
                    datoNombramiento.Puesto.DetallePuesto = (CDetallePuestoDTO)datos.ElementAt(4).ElementAt(0);

                    // [0]  CNombramientoDTO
                    respuesta.Add(datoNombramiento);

                    // [1] CDetalleContratacionDTO 
                    var datoContratacion = (CDetalleContratacionDTO)datos.ElementAt(2).ElementAt(0);
                    respuesta.Add(datoContratacion);

                    detalle.Division = datoNombramiento.Puesto.UbicacionAdministrativa.Division;
                    detalle.DireccionGeneral = datoNombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral;
                    if(datoNombramiento.Puesto.UbicacionAdministrativa.Seccion != null)
                        detalle.Seccion = datoNombramiento.Puesto.UbicacionAdministrativa.Seccion;
                    if (datoNombramiento.Puesto.UbicacionAdministrativa.Departamento != null)
                        detalle.Departamento = datoNombramiento.Puesto.UbicacionAdministrativa.Departamento;


                    numAnualidades = datoContratacion.NumeroAnualidades;

                    // Fecha Cese
                    if (datoContratacion.FechaCese != null)
                        if (datoContratacion.FechaCese.Year != 1)
                            fechaCese = datoContratacion.FechaCese;
                        else
                            fechaCese = DateTime.Today;
                    else
                        fechaCese = DateTime.Today;
                  

                    // Fecha Vacaciones
                    if (datoContratacion.FechaVacaciones != null)
                        fechaVacaciones = datoContratacion.FechaVacaciones;
                    else
                    {
                        if (datoContratacion.FechaIngreso != null)
                            fechaVacaciones = datoContratacion.FechaIngreso;
                        else
                            fechaVacaciones = DateTime.Today;
                    }

                    //////////////////////////////////////////////////////////////////////////////
                    //////////////////    DATOS PRUEBA   /////////////////////////////////////////
                    //fechaCese = fechaCese.AddMonths(-2);
                    //fechaVacaciones = fechaVacaciones.AddMonths(2);
                    //////////////////////////////////////////////////////////////////////////////


                    // [2] CPrestacionLegalDTO 
                  

                    // 2.1  Accion Ingreso, Accion Cese
                    CAccionPersonalDTO accionIngreso = new CAccionPersonalDTO() { IdEntidad = 0, NumAccion = "sin número"};
                    CAccionPersonalDTO accionCese = new CAccionPersonalDTO() { IdEntidad = 0, NumAccion = "sin número" }; ;

                    //CAccionPersonalHistoricoDTO accionH = new CAccionPersonalHistoricoDTO { Cedula = cedula };
                    //var datoAccionH = intermedioAcciones.BuscarHistorial(accionH, new List<DateTime>());

                    //if(datoAccionH.ElementAt(0).GetType() != typeof(CErrorDTO))
                    //{
                    //    // Buscar la acción de Ingreso más antigua
                    //}
                    
                    var datoAccion = intermedioAcciones.BuscarAccion(datoNombramiento.Funcionario, null, null, new List<DateTime>());
                    if (datoAccion.ElementAt(0).ElementAt(0).GetType() != typeof(CErrorDTO))
                    {
                        //// Buscar la acción de Ingreso más antigua
                        //if(accionIngreso.NumAccion == "")
                        //{
                        //    var listaAccionesIngreso = new List<int> { 57 };
                        //    var accionI = datoAccion.Where(Q => listaAccionesIngreso.Contains(((CAccionPersonalDTO)Q.ElementAt(0)).TipoAccion.IdEntidad))
                        //                            .FirstOrDefault();
                        //}

                        // Buscar la acción de Cese
                        var listaAccionesCese = new List<int> {2, 4, 12};
                        var accion = datoAccion.Where(Q => listaAccionesCese.Contains(((CAccionPersonalDTO)Q.ElementAt(0)).TipoAccion.IdEntidad))
                                                .FirstOrDefault();
                        if (accion != null)
                        {
                            accionCese.IdEntidad = ((CAccionPersonalDTO)accion.ElementAt(0)).IdEntidad;
                            accionCese.NumAccion = ((CAccionPersonalDTO)accion.ElementAt(0)).NumAccion;
                        }
                            
                    }

                    //Prestacion.Acciones.Add(accionIngreso);
                    //Prestacion.Acciones.Add(accionCese);

                    detalle.AccionIngreso = accionIngreso;
                    detalle.AccionCese = accionCese;
                    
                    // 2.2  Expediente
                    var datoExpediente = intermedioExpediente.ObtenerExpedientePorCedula(cedula);
                    if(datoExpediente.GetType() != typeof(CErrorDTO))
                        Prestacion.Expediente = (CExpedienteFuncionarioDTO)datoExpediente[0];
                    else
                        throw new Exception(((CErrorDTO)datoExpediente[0]).MensajeError);



                    // 2.3 Llenar Cuadros 


                    // Periodo Vacaciones
                    // Obtener dias derecho
                    decimal numDiasDerecho = 0;
                    decimal numDiasReconocido = 0;
                    bool vacacionesAcumuladas = false;
                    string nomPeriodoVaca = "";
                    decimal numDiasPeriodo = 0;

                    if (numAnualidades > 0 && numAnualidades <= 5)
                        numDiasDerecho = 15;
                    if (numAnualidades > 5 && numAnualidades <= 10)
                        numDiasDerecho = 20;
                    if (numAnualidades >= 11)
                        numDiasDerecho = 26;

                    numDiasReconocido = Math.Truncate(100 * numDiasDerecho / 12) / 100;

                    var datoVaca = intermedioVacaciones.ObtenerDetalleVacaciones(cedula);

                    if (datoVaca[0].GetType() != typeof(CErrorDTO))
                    {
                        vacacionesAcumuladas = true;
                        nomPeriodoVaca = ((CPeriodoVacacionesDTO)datoVaca[0][0]).Periodo;
                        numDiasPeriodo = Convert.ToDecimal(((CPeriodoVacacionesDTO)datoVaca[0][0]).Saldo);

                        //foreach (var itemV in datoVaca[0])
                        //{
                        //    if (itemV.GetType() != typeof(CErrorDTO))
                        //    {

                        //        // ((CPeriodoVacacionesDTO)itemV).Periodo;
                        //        //((CPeriodoVacacionesDTO)itemV).Saldo;

                        //        // Vacaciones Proporcionales
                        //        // Sacar los salarios posteriores a la Fecha de Vacaciones hasta la fecha Cese
                        //    }
                        //}
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datoVaca[0][0]).MensajeError);
                    }


                    // Buscar salarios
                    var datosDesglose = intermedio.BuscarFuncionarioDesgloceSalarial(cedula);
                    if (datosDesglose.Codigo > 0)
                    {
                        var detallePrestacion = new CDetallePrestacionCuadroDTO();
                        var listaDesglose = (List<DesgloseSalarial>)datosDesglose.Contenido;
                        foreach (var item in listaDesglose.OrderByDescending(Q => Q.IndPeriodo).Take(36))
                        {
                            if(item.IndPeriodo.Day == 1)  // Primera Quincena
                                item.IndPeriodo = item.IndPeriodo.AddDays(14);
                            else
                                item.IndPeriodo = new DateTime(item.IndPeriodo.Year, item.IndPeriodo.Month, 1).AddMonths(1).AddDays(-1);  // Último día del mes

                            detallePrestacion = new CDetallePrestacionCuadroDTO
                            {
                                FecPeriodo = item.IndPeriodo,
                                MtoSalario = Convert.ToDecimal(item.DetalleDesgloseSalarial.Sum(Q => Q.MtoPagocomponenteSalarial)),
                                MtoExtra = 0,
                                MtoFeriado = 0,
                                PeriodoVacaciones ="",
                                NumSaldoVacaciones = 0,
                            };

                            detallePrestacion.MtoTotal = detallePrestacion.MtoSalario + detallePrestacion.MtoExtra + detallePrestacion.MtoFeriado;
                            detallePrestacion.MtoSalarioEscolar = Math.Round(detallePrestacion.MtoTotal * Convert.ToDecimal(0.0833), 2);
                           
                            // Cuadro Salario
                            if (listaSalario.Count() < 12)
                            {
                                listaSalario.Add(detallePrestacion);
                            }


                            // Cuadro Aguinaldo
                            if(fechaCese.Month == 12) // Si es Noviembre o Diciembre, solo se incluyen solo los pagos de esos meses
                            {
                                if (item.IndPeriodo.Year == fechaCese.Year && item.IndPeriodo.Month == fechaCese.Month)
                                    listaAguinaldo.Add(detallePrestacion);
                            }
                            else
                            {
                                if (item.IndPeriodo.Year == fechaCese.Year)
                                {
                                    listaAguinaldo.Add(detallePrestacion);
                                }
                                else if (item.IndPeriodo.Year == fechaCese.Year - 1)
                                {
                                    if (item.IndPeriodo.Month >= 11)
                                        listaAguinaldo.Add(detallePrestacion);
                                }
                            }
                            
                            // Cuadro Salario Escolar
                            if (item.IndPeriodo.Year == fechaCese.Year)
                            {
                                listaSalarioEscolar.Add(detallePrestacion);
                            }


                            // Cuadro Vacaciones 
                            if (item.IndPeriodo.Year == fechaCese.Year &&
                                item.IndPeriodo >= new DateTime(fechaCese.Year, fechaVacaciones.Month, fechaVacaciones.Day) )
                            {
                                // Vacaciones Proporcionales
                                listaVacacionesProporcionales.Add(detallePrestacion);
                            }
                            else
                            {
                                // Vacaciones Acumuladas
                                if(vacacionesAcumuladas)
                                {
                                    listaVacacionesAcumuladas.Add(detallePrestacion);
                                    fechaVacacionesAcum = item.IndPeriodo;
                                }
                            }                         
                        }


                        if (fechaVacacionesAcum.Year == 1)
                            fechaVacacionesAcum = new DateTime(fechaCese.Year, fechaVacaciones.Month, fechaVacaciones.Day);
                        ////////////   AGUINALDO
                        // Si la Fecha de Cese es menor al 01 Diciembre del 2020, se debe consultar el histórico de Planillas
                        if (fechaCese.Year == 2020 && fechaCese.Month < 12)
                        {
                            fechaInicio = new DateTime(2019, 11, 11);
                            fechaFinal = new DateTime(2020, 1, 30);

                            var datohistoricoPlanilla = intermediohistoricoPlanilla.BuscarDatosPlanilla(cedula, fechaInicio, fechaFinal);
                            if (datohistoricoPlanilla.ElementAt(0).GetType() != typeof(CErrorDTO))
                            {
                                foreach (var item in datohistoricoPlanilla)
                                {
                                    CHistoricoPlanillaDTO temp = (CHistoricoPlanillaDTO)item;
                                    if (temp.FechaPeriodo != null)
                                    {
                                        decimal quincena = 0;
                                        try
                                        {
                                            quincena = Convert.ToDecimal(temp.SalarioMensual);
                                            quincena = quincena / 2;
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                quincena = Convert.ToDecimal(temp.SalarioMensual.Replace(',','.'));
                                            }
                                            catch
                                            {
                                                quincena = Convert.ToDecimal(temp.SalarioMensual.Replace('.', ','));
                                            }
                                        }
                                            
                                        quincena = temp.SalarioMensual != null ? Convert.ToDecimal(temp.SalarioMensual.Replace('.', ',')) / 2 : 0;
                                        var a = temp.FechaPeriodo.Substring(0, 4);
                                        var m = temp.FechaPeriodo.Substring(5, 2);
                                        var d = temp.FechaPeriodo.Substring(8, 2);
                                        fechaInicio = new DateTime(Convert.ToInt32(temp.FechaPeriodo.Substring(0,4)),
                                                                   Convert.ToInt32(temp.FechaPeriodo.Substring(5,2)),
                                                                   Convert.ToInt32(temp.FechaPeriodo.Substring(8,2)));
                                        detallePrestacion = new CDetallePrestacionCuadroDTO
                                        {
                                            FecPeriodo = fechaInicio,
                                            MtoSalario = quincena,
                                            MtoExtra = 0,
                                            MtoFeriado = 0,
                                            MtoTotal = quincena
                                        };
                                        listaAguinaldo.Add(detallePrestacion);
                                    }
                                }
                            }
                        }


                        ////////////   VACACIONES PROPORCIONALES
                        // Cálculos
                        // Días a reconocer
                        numDiasReconocido = numDiasReconocido * (listaVacacionesProporcionales.Count() / 2);
                        // MontoTotal
                        decimal MtoTotal = listaVacacionesProporcionales.Sum(Q => Q.MtoTotal) + listaVacacionesProporcionales.Sum(Q => Q.MtoSalarioEscolar);
                        // MONTO PROMEDIO CON QUINCEN:
                        decimal MtoQuinc = 0;
                        if(listaVacacionesProporcionales.Count > 0)
                            MtoQuinc = MtoTotal / listaVacacionesProporcionales.Count();

                        // MONTO PROMEDIO DIARIO.....:  
                        decimal MtoDiario = MtoQuinc / 15;
                        // MONTO DIAS VACAC.A PAGAR.:
                        foreach (var vac in listaVacacionesProporcionales)
                        {
                            vac.NumSaldoVacaciones = numDiasReconocido;
                            vac.MtoSaldoVacaciones = MtoDiario * numDiasReconocido;
                            vac.PeriodoVacaciones = DateTime.Now.Year.ToString() + DateTime.Now.AddYears(1).Year.ToString();
                        }



                        ////////////   VACACIONES ACUMULADAS
                        // Si la Fecha de Cese es menor al 01 Diciembre del 2020, se debe consultar el histórico de Planillas
                        if (vacacionesAcumuladas)
                        {
                            var cantidad = listaVacacionesAcumuladas.Count() / 2;
                            if (cantidad < 12)
                            {
                                fechaFinal = new DateTime(fechaVacacionesAcum.Year, fechaVacacionesAcum.Month, fechaVacacionesAcum.Day);

                                fechaVacacionesAcum = fechaVacacionesAcum.AddMonths(cantidad - 12);
                                fechaInicio = new DateTime(fechaVacacionesAcum.Year, fechaVacacionesAcum.Month, fechaVacacionesAcum.Day);

                                var datohistoricoPlanilla = intermediohistoricoPlanilla.BuscarDatosPlanilla(cedula, fechaInicio, fechaFinal);
                                if (datohistoricoPlanilla.ElementAt(0).GetType() != typeof(CErrorDTO))
                                {
                                    foreach (var item in datohistoricoPlanilla)
                                    {
                                        CHistoricoPlanillaDTO temp = (CHistoricoPlanillaDTO)item;
                                        if (temp.FechaPeriodo != null)
                                        {
                                            decimal quincena = 0;
                                            try
                                            {
                                                quincena = Convert.ToDecimal(temp.SalarioMensual);
                                                quincena = quincena / 2;
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    quincena = Convert.ToDecimal(temp.SalarioMensual.Replace(',', '.'));
                                                }
                                                catch
                                                {
                                                    quincena = Convert.ToDecimal(temp.SalarioMensual.Replace('.', ','));
                                                }
                                            }

                                            quincena = temp.SalarioMensual != null ? Convert.ToDecimal(temp.SalarioMensual.Replace('.', ',')) / 2 : 0;
                                            var a = temp.FechaPeriodo.Substring(0, 4);
                                            var m = temp.FechaPeriodo.Substring(5, 2);
                                            var d = temp.FechaPeriodo.Substring(8, 2);
                                            if (m == "02" && Convert.ToInt32(d) > 28)
                                                d = "28";
                                            //fechaInicio = new DateTime(Convert.ToInt32(temp.FechaPeriodo.Substring(0, 4)),
                                            //                           Convert.ToInt32(temp.FechaPeriodo.Substring(5, 2)),
                                            //                           Convert.ToInt32(temp.FechaPeriodo.Substring(8, 2)));
                                            fechaInicio = new DateTime(Convert.ToInt32(a),
                                                                       Convert.ToInt32(m),
                                                                       Convert.ToInt32(d));

                                            detallePrestacion = new CDetallePrestacionCuadroDTO
                                            {
                                                FecPeriodo = fechaInicio,
                                                MtoSalario = quincena,
                                                MtoExtra = 0,
                                                MtoFeriado = 0,
                                                MtoTotal = quincena,
                                                MtoSalarioEscolar = Math.Round(quincena * Convert.ToDecimal(0.0833), 2)
                                        };
                                            listaVacacionesAcumuladas.Add(detallePrestacion);
                                        }
                                    }
                                }
                            }

                            // Cálculos
                            // MontoTotal 
                            MtoTotal = listaVacacionesAcumuladas.Sum(Q => Q.MtoTotal) + listaVacacionesAcumuladas.Sum(Q => Q.MtoSalarioEscolar);
                            // MONTO PROMEDIO CON QUINCEN:
                            MtoQuinc = MtoTotal / listaVacacionesAcumuladas.Count();
                            // MONTO PROMEDIO DIARIO.....:  
                            MtoDiario = MtoQuinc / 15;
                            // MONTO DIAS VACAC.A PAGAR.:
                            foreach(var vac in listaVacacionesAcumuladas)
                            {
                                vac.NumSaldoVacaciones = numDiasPeriodo;
                                vac.MtoSaldoVacaciones = MtoDiario * numDiasPeriodo;
                                vac.PeriodoVacaciones = nomPeriodoVaca;
                            }
                        }
                        
                        Prestacion.ListaSalario = listaSalario.OrderBy(Q => Q.FecPeriodo).ToList();
                        Prestacion.ListaAguinaldo = listaAguinaldo.OrderBy(Q => Q.FecPeriodo).ToList();
                        Prestacion.ListaSalarioEscolar = listaSalarioEscolar.OrderBy(Q => Q.FecPeriodo).ToList();
                        Prestacion.ListaVacacionesProporcionales = listaVacacionesProporcionales.OrderBy(Q => Q.FecPeriodo).ToList();
                        Prestacion.ListaVacacionesAcumuladas = listaVacacionesAcumuladas.OrderBy(Q => Q.FecPeriodo).ToList();
                        
                        decimal montoSalario = 0;
                        decimal montoEscolar = 0;
                        decimal montoTotal = 0;

                        // Salario
                        montoSalario = listaSalario.Sum(Q => Q.MtoTotal);
                        montoEscolar = listaSalario.Sum(Q => Q.MtoSalarioEscolar);
                        montoTotal = montoSalario + montoEscolar;

                        Prestacion.MtoTotal = montoTotal / 6;

                        // Aguinaldo
                        montoSalario = listaAguinaldo.Sum(Q => Q.MtoTotal);
                        montoEscolar = listaAguinaldo.Sum(Q => Q.MtoSalarioEscolar);
                        montoTotal = montoSalario + montoEscolar;
                        Prestacion.MtoAguinaldo = montoTotal / 12;

                        // Salario Escolar
                        montoSalario = listaSalarioEscolar.Sum(Q => Q.MtoTotal);
                        montoEscolar = listaSalarioEscolar.Sum(Q => Q.MtoSalarioEscolar);
                        decimal montoDeduccion = (montoEscolar * Convert.ToDecimal(0.105));
                        montoTotal = montoEscolar - montoDeduccion;

                        Prestacion.MtoEscolarSinRebajo = montoEscolar;
                        Prestacion.MtoEscolarConRebajo = montoTotal;

                        // [2] CPrestacionLegalDTO 
                        Prestacion.Detalle.Add(detalle);
                        respuesta.Add(Prestacion);
                    }
                    else
                    {
                        var mensaje = ((CErrorDTO)datosDesglose.Contenido).MensajeError;
                        throw new Exception(mensaje);
                    }
                }
                else
                {
                    var mensaje = ((CErrorDTO)datos.ElementAt(0).ElementAt(0)).MensajeError;
                    throw new Exception(mensaje); // throw new Exception("Busqueda");
                }

                // [0]  Nombramiento
                // Funcionario Céd, Nomb, Sexo, Expediente
                // Puesto (Núm Puesto, Ubicac, Clase, Cod Presup)

                // [1] DetalleContratacion   Fec Ingreso, Fec Cese
                // [2] Accion Ingreso, Accion Cese

                // [3] Quincenas 
                // Últimas 36 quincenas

                // [4] Vacaciones

                
                // Buscar  Incapacidades, Permiso sin goce
                // Buscar Pago de Extras, Feriados
            }
            catch (Exception error)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = (error.InnerException != null ? error.InnerException.Message : error.Message)
                });
            }

            return respuesta;
        }

        #endregion
    }
}
