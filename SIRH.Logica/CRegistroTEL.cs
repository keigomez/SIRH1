using SIRH.Datos;
using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SIRH.Logica
{
    public class CRegistroTEL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CRegistroTEL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        /*******************************************************************************************************/
        public CRespuestaDTO RegistrarTiempoExtra(string cedula, CRegistroTiempoExtraDTO registro, List<CDetalleTiempoExtraDTO> extras)
        {
            try
            {
                CRespuestaDTO respuesta = new CRespuestaDTO();
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);

                string periodo = FormatearPeriodoBase(registro.Periodo);

                RegistroTiempoExtra registroDatos = new RegistroTiempoExtra
                {
                    FecEmision = registro.FechaEmision,
                    FecPago = registro.FecPago,
                    IndPeriodo = periodo,
                    NumOficioJustificacion = registro.OficJustificacion,
                    Justificacion = registro.Justificacion,
                    ArchivoJustificacion = registro.Archivo,
                    Clase = registro.Clase != null ? contexto.Clase.FirstOrDefault(C => C.PK_Clase == registro.Clase.IdEntidad) : null,
                    Presupuesto = registro.Presupuesto != null ? contexto.Presupuesto.FirstOrDefault(P => P.PK_Presupuesto == registro.Presupuesto.IdEntidad) : null,
                    IndEstadoExtra = (int)EstadoExtraEnum.Activo,
                    IndActividad = registro.Actividad,
                    IndArea = registro.Area
                };
                if(registro.IdEntidad != 0)
                {
                    registroDatos.PK_RegistroTiempoExtra = registro.IdEntidad;
                }
                foreach (var item in extras)
                {
                    if (item.HoraInicio != null)
                    {
                        if (item.FechaFinal < item.FechaInicio)
                        {
                            item.FechaFinal = item.FechaInicio;
                        }
                        registroDatos.DetalleTiempoExtra.Add(new DetalleTiempoExtra
                        {
                            FecInicio = item.FechaInicio,
                            FecFin = item.FechaFinal,
                            IndHoraInicio = $"{item.HoraInicio}:{item.MinutoInicio}",
                            IndHoraFin = $"{item.HoraFinal}:{item.MinutoFinal}",
                            TipoExtra = contexto.TipoExtra.FirstOrDefault(Q => Q.PK_TipoExtra == item.TipoExtra.IdEntidad),
                            IndEstado = (int)EstadoDetalleExtraEnum.Activo
                        });
                    }
                }

                respuesta = intermedio.RegistrarTiempoExtra(cedula, registroDatos, registro.QuincenaA, registro.QuincenaB);
                if(respuesta.Codigo == -1)
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
                return respuesta;
            }
            catch(Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }
        public CRespuestaDTO RegistrarTiempoExtraDoble(string id, List<CDetalleTiempoExtraDTO> extras)
        {
            try
            {
                CRespuestaDTO respuesta = new CRespuestaDTO();
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);

                List<DetalleTiempoExtra> detalles = new List<DetalleTiempoExtra>();
                DateTime ahora = DateTime.Now;
                foreach (var item in extras)
                {
                    if (item.HoraInicio != null)
                    {
                        if (item.FechaFinal < item.FechaInicio)
                        {
                            item.FechaFinal = item.FechaInicio;
                        }
                        detalles.Add(new DetalleTiempoExtra
                        {
                            FecInicio = item.FechaInicio,
                            FecFin = item.FechaFinal,
                            IndHoraInicio = $"{item.HoraInicio}:{item.MinutoInicio}",
                            IndHoraFin = $"{item.HoraFinal}:{item.MinutoFinal}",
                            MtoH0 = item.HoraTotalH0 != null && item.MinutoTotalH0 != null ? $"{item.HoraTotalH0}:{item.MinutoTotalH0}" : null,
                            MtoH1 = item.HoraTotalH1 != null && item.MinutoTotalH1 != null ? $"{item.HoraTotalH1}:{item.MinutoTotalH1}" : null,
                            MtoH2 = item.HoraTotalH2 != null && item.MinutoTotalH2 != null ? $"{item.HoraTotalH2}:{item.MinutoTotalH2}" : null,
                            FecRegistro = Convert.ToDateTime(ahora),
                            TipoExtra = contexto.TipoExtra.FirstOrDefault(Q => Q.PK_TipoExtra == item.TipoExtra.IdEntidad),
                            IndEstado = (int)EstadoDetalleExtraEnum.Activo
                        });
                    }
                }

                respuesta = intermedio.RegistrarTiempoExtraDoble(Convert.ToInt32(id), detalles);
                if (respuesta.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }
        public CRespuestaDTO EstaEnVacaciones(string cedula, DateTime fechaInicio, DateTime fechaFin)
        {
            CRespuestaDTO respuesta;
            CRegistroVacacionesD intermedio = new CRegistroVacacionesD(contexto);
            try
            {
                var datos = intermedio.ConsultaVacaciones(cedula);
                if (datos.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
                List<RegistroVacaciones> lista = (List<RegistroVacaciones>)datos.Contenido;
                RegistroVacaciones vacacion = lista.FirstOrDefault(V => (V.FecInicio <= fechaInicio && V.FecFin >= fechaInicio) || (V.FecInicio <= fechaFin && V.FecFin >= fechaInicio));
                ReintegroVacaciones reintegro = contexto.ReintegroVacaciones.FirstOrDefault(R => Convert.ToDateTime(R.FecInicio).Date <= fechaInicio.Date && Convert.ToDateTime(R.FecFin).Date >= fechaFin.Date && R.FK_RegistroVacaciones == vacacion.PK_RegistroVacaciones);
                if (vacacion == null)
                {
                    throw new Exception("El funcionario no estaba en vacaciones");
                }
                else
                {
                    if (reintegro == null)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = new CRegistroVacacionesDTO
                            {
                                IdEntidad = vacacion.PK_RegistroVacaciones,
                                FechaRige = Convert.ToDateTime(vacacion.FecInicio),
                                FechaVence = Convert.ToDateTime(vacacion.FecFin)
                            }
                        };
                    }
                    else
                    {
                        throw new Exception("El funcionario no estaba en vacaciones");
                    }
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
            return respuesta;
        }
        public CRespuestaDTO EstaIncapacitado(string cedula, DateTime fechaInicio, DateTime fechaFin)
        {
            CRespuestaDTO respuesta;
            CRegistroIncapacidadD intermedio = new CRegistroIncapacidadD(contexto);
            try
            {
                var datos = intermedio.ConsultaIncapacidades(cedula);
                if (datos.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
                List<RegistroIncapacidad> lista = (List<RegistroIncapacidad>)datos.Contenido;
                RegistroIncapacidad vacacion = lista.FirstOrDefault(I => (I.FecRige <= fechaInicio && I.FecVence >= fechaInicio) || (I.FecRige <= fechaFin && I.FecVence >= fechaInicio));
                if (vacacion == null)
                {
                    throw new Exception("El funcionario no estaba en incapacitado");
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = new CRegistroIncapacidadDTO
                        {
                            IdEntidad = vacacion.PK_RegistroIncapacidad,
                            FecRige = Convert.ToDateTime(vacacion.FecRige),
                            FecVence = Convert.ToDateTime(vacacion.FecVence)
                        }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
            return respuesta;
        }
        internal static CRegistroTiempoExtraDTO ConvertirDatosRegistroTiempoExtraADto(RegistroTiempoExtra registro)
        {
            return new CRegistroTiempoExtraDTO {
                IdEntidad = registro.PK_RegistroTiempoExtra,
                Periodo = registro.IndPeriodo,
                FechaEmision = Convert.ToDateTime(registro.FecEmision),
                FecPago = Convert.ToDateTime(registro.FecPago),
                Justificacion = registro.Justificacion,
                Archivo = registro.ArchivoJustificacion,
                OficJustificacion = registro.NumOficioJustificacion,
                Estado = (EstadoExtraEnum)registro.IndEstadoExtra,
                ObservacionesEstado = registro.ObsEstado,
                ObservacionesEstadoDoble = registro.ObsEstadoDoble,
                Area = registro.IndArea,
                Actividad = registro.IndActividad,
                Clase = (registro.Clase != null) ? new CClaseDTO
                {
                    IdEntidad = registro.Clase.PK_Clase,
                    DesClase = registro.Clase.DesClase
                } : new CClaseDTO { Mensaje = "No se encontro la clase"},
                Presupuesto = (registro.Presupuesto != null) ? new CPresupuestoDTO
                {
                    IdEntidad = registro.Presupuesto.PK_Presupuesto,
                    CodigoPresupuesto = registro.Presupuesto.IdPresupuesto,
                    Actividad = registro.Presupuesto.CodActividad,
                    Area = registro.Presupuesto.CodArea
                } : new CPresupuestoDTO { Mensaje = "No se encontro el presupuesto"}
            };
        }
        internal static CDetalleTiempoExtraDTO ConvertirDatosDetalleTiempoExtraADto(DetalleTiempoExtra detalle)
        {
            return new CDetalleTiempoExtraDTO
            {
                FechaCarga = detalle.FecRegistro,                
                FechaInicio = Convert.ToDateTime(detalle.FecInicio),
                FechaFinal = Convert.ToDateTime(detalle.FecFin),
                HoraInicio = detalle.IndHoraInicio.Split(':')[0],
                HoraFinal = detalle.IndHoraFin.Split(':')[0],
                MinutoInicio = detalle.IndHoraInicio.Split(':')[1],
                MinutoFinal = detalle.IndHoraFin.Split(':')[1],
                HoraTotalH0 = detalle.MtoH0?.Split(':')[0],
                MinutoTotalH0 = detalle.MtoH0?.Split(':')[1],
                HoraTotalH1 = detalle.MtoH1?.Split(':')[0],
                MinutoTotalH1 = detalle.MtoH1?.Split(':')[1],
                HoraTotalH2 = detalle.MtoH2?.Split(':')[0],
                MinutoTotalH2 = detalle.MtoH2?.Split(':')[1],
                Estado = (EstadoDetalleExtraEnum)detalle.IndEstado,
                TipoExtra = detalle.TipoExtra != null ? new CTipoExtraDTO { IdEntidad = detalle.TipoExtra.PK_TipoExtra, DesTipExtra = detalle.TipoExtra.DesTipExtra }
                                                           : new CTipoExtraDTO { Mensaje = "No se encontro el registro de tipo extra" }
            };
        }
        public List<CBaseDTO> ObtenerRegistroExtrasSaved(string cedula, string periodo, bool doble)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);
                var datos = intermedio.ObtenerRegistroExtrasSaved(cedula, FormatearPeriodoBase(periodo));
                if (datos.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
                RegistroTiempoExtra registro = (RegistroTiempoExtra)datos.Contenido;
                //Funcionario [0]
                Funcionario funcionario = registro.DesgloseSalarial.Nombramiento.Funcionario;
                CFuncionarioDTO funcionarioDTO = CFuncionarioL.FuncionarioGeneral(funcionario);
                respuesta.Add(funcionarioDTO);
                //Puesto [1]
                Nombramiento nombramiento = registro.DesgloseSalarial.Nombramiento;
                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(nombramiento.Puesto, puesto);
                respuesta.Add(puesto);
                //Detalle puesto [2]
                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();
                detallePuesto.Especialidad = new CEspecialidadDTO
                {
                    IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                    DesEspecialidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                };
                detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                {
                    IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial.PK_Escala,
                    SalarioBase = nombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                    MontoAumentoAnual = Convert.ToDecimal(nombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial.MtoAumento)
                };
                if (nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase != null)
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                        DesClase = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase,
                    };
                }
                else
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        DesClase = "NO TIENE"
                    };
                }
                if (nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }
                respuesta.Add(detallePuesto);
                //Desgloce salarial Q1 [3]
                respuesta.Add(CDesgloseSalarialL.DesgloseSalarialGeneral(registro.DesgloseSalarial));
                //Desgloce salarial Q2 [4]
                respuesta.Add(CDesgloseSalarialL.DesgloseSalarialGeneral(registro.DesgloseSalarial1));
                //CNombramientoDTO [5]
                respuesta.Add(new CNombramientoDTO { IdEntidad = nombramiento.PK_Nombramiento, FecRige = Convert.ToDateTime(nombramiento.FecRige), FecVence = Convert.ToDateTime(nombramiento.FecVence) });
                //Detalle tiempo extra
                List<DetalleTiempoExtra> detallesDatos;
                if (doble)
                    detallesDatos = registro.DetalleTiempoExtra.Where(D => D.TipoExtra.DesTipExtra.Contains("Addendum") && D.IndEstado == (int)EstadoDetalleExtraEnum.Activo).ToList();
                else
                    detallesDatos = registro.DetalleTiempoExtra.Where(D => !D.TipoExtra.DesTipExtra.Contains("Addendum") && D.IndEstado == (int)EstadoDetalleExtraEnum.Activo).ToList();
                List<CDetalleTiempoExtraDTO> detalles = new List<CDetalleTiempoExtraDTO>();
                foreach (var detalle in detallesDatos)
                {
                    detalles.Add(ConvertirDatosDetalleTiempoExtraADto(detalle));
                }
                //Registro tiempo extra [6]
                CRegistroTiempoExtraDTO registroDTO = ConvertirDatosRegistroTiempoExtraADto(registro);
                if (doble)
                {
                    registroDTO.FecRegistroDetalles = Convert.ToDateTime(detalles[0].FechaCarga);
                }
                registroDTO.Detalles = detalles;
                registroDTO.Periodo = periodo;
                if (doble && registroDTO.Detalles.Count > 0)
                    registroDTO.FechaEmision = Convert.ToDateTime(registroDTO.Detalles[0].FechaCarga);
                respuesta.Add(registroDTO);
            } catch(Exception ex)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO 
                { 
                    Codigo = -1,
                    MensajeError = ex.Message
                });
            }       
            return respuesta;
        }
        public List<CBaseDTO> ObtenerRegistroExtrasDetalleDoble(string cedula, string periodo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);
                var datos = intermedio.ObtenerRegistroExtrasDetalle(cedula, FormatearPeriodoBase(periodo));
                if (datos.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
                RegistroTiempoExtra registro = (RegistroTiempoExtra)datos.Contenido;
                if(registro.DesgloseSalarial.Nombramiento.PK_Nombramiento != registro.DesgloseSalarial1.Nombramiento.PK_Nombramiento)
                {
                    throw new Exception("Ha ocurrido un error con el desglose salarial, contacte al encargado");
                }
                Nombramiento nombramiento = registro.DesgloseSalarial.Nombramiento;
                //Funcionario [0]
                Funcionario funcionario = nombramiento.Funcionario;
                CFuncionarioDTO funcionarioDTO = CFuncionarioL.FuncionarioGeneral(funcionario);
                respuesta.Add(funcionarioDTO);
                //Puesto [1]
                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(nombramiento.Puesto, puesto);
                respuesta.Add(puesto);
                //DetallePuesto[2]
                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();
                detallePuesto.Especialidad = new CEspecialidadDTO
                {
                    IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.PK_Especialidad,
                    DesEspecialidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Especialidad.DesEspecialidad
                };

                detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                {
                    IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial.PK_Escala,
                    SalarioBase = nombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                    MontoAumentoAnual = Convert.ToDecimal(nombramiento.Puesto.DetallePuesto.FirstOrDefault().EscalaSalarial.MtoAumento)
                };

                if(nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase != null)
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.PK_Clase,
                        DesClase = nombramiento.Puesto.DetallePuesto.FirstOrDefault().Clase.DesClase,
                    };
                }
                else
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        DesClase = "NO TIENE"
                    };
                }

                if (nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }
                respuesta.Add(detallePuesto);
                //Desgloce salarial Q1 [3]
                respuesta.Add(CDesgloseSalarialL.DesgloseSalarialGeneral(registro.DesgloseSalarial));
                //Desgloce salarial Q2 [4]
                respuesta.Add(CDesgloseSalarialL.DesgloseSalarialGeneral(registro.DesgloseSalarial1));
                //Nombramineto [5]
                respuesta.Add(new CNombramientoDTO { IdEntidad = nombramiento.PK_Nombramiento, FecRige = Convert.ToDateTime(nombramiento.FecRige), FecVence = Convert.ToDateTime(nombramiento.FecVence) });
                if(registro.DetalleTiempoExtra.Where(D => D.TipoExtra.DesTipExtra.Contains("Addendum") && D.IndEstado == (int)EstadoDetalleExtraEnum.Activo).Count() > 0)
                {
                    throw new Exception("El registro de tiempo extra ya tiene jornadas dobles asignadas");
                }
                List<DetalleTiempoExtra> detallesDatos;
                detallesDatos = registro.DetalleTiempoExtra.Where(D => !D.TipoExtra.DesTipExtra.Contains("Addendum")).ToList();
                List<CDetalleTiempoExtraDTO> detalles = new List<CDetalleTiempoExtraDTO>();
                foreach (var detalle in detallesDatos)
                {
                    detalles.Add(ConvertirDatosDetalleTiempoExtraADto(detalle));
                }
                //Registro tiempo extra [6]
                CRegistroTiempoExtraDTO registroDTO = ConvertirDatosRegistroTiempoExtraADto(registro);
                if (!detallePuesto.OcupacionReal.DesOcupacionReal.StartsWith("GUARDA") && (registroDTO.Clase == null || !registroDTO.Clase.DesClase.Contains("OFIC.SEGUR.SERV.CIVIL")))
                {
                    throw new Exception("Solo se puede registrar jornadas dobles a funcionarios que laboraron el tiempo extra como guarda");
                }
                registroDTO.Detalles = detalles;
                registroDTO.Periodo = FormatearPeriodoVista(registro.IndPeriodo);
                if (registro.Presupuesto != null)
                {
                    registroDTO.Presupuesto = new CPresupuestoDTO
                    {
                        IdEntidad = registro.Presupuesto.PK_Presupuesto,
                        IdDireccionPresupuestaria = registro.Presupuesto.IdDireccionPresupuestaria,
                        Area = registro.Presupuesto.CodArea,
                        Actividad = registro.Presupuesto.CodActividad,
                        CodigoPresupuesto = registro.Presupuesto.IdPresupuesto
                    };
                }
                else
                {
                    registroDTO.Presupuesto = new CPresupuestoDTO
                    {
                        Mensaje = "No se encontró el presupuesto"
                    };
                }
                if(registro.Clase != null)
                {
                    registroDTO.Clase = new CClaseDTO
                    {
                        IdEntidad = registro.Clase.PK_Clase,
                        DesClase = registro.Clase.DesClase
                    };
                }
                else
                {
                    registroDTO.Clase = new CClaseDTO
                    {
                        Mensaje = "No se encontró la clase"
                    };
                }
                respuesta.Add(registroDTO);
            }
            catch (Exception ex)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = ex.Message
                });
            }
            return respuesta;
        }
        public List<CBaseDTO> ObtenerRegistroExtrasDetalle(DateTime fechaRegistro, int id, bool doble)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);
                var datos = intermedio.ObtenerRegistroExtrasDetalle(id);
                if (datos.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
                RegistroTiempoExtra registro = (RegistroTiempoExtra)datos.Contenido;
                if (registro.DesgloseSalarial.Nombramiento.PK_Nombramiento != registro.DesgloseSalarial1.Nombramiento.PK_Nombramiento)
                {
                    throw new Exception("Ha ocurrido un error con el desglose salarial, contacte al encargado");
                }
                Nombramiento nombramiento = registro.DesgloseSalarial.Nombramiento;
                //Funcionario [0]
                Funcionario funcionario = nombramiento.Funcionario;
                CFuncionarioDTO funcionarioDTO = CFuncionarioL.FuncionarioGeneral(funcionario);
                respuesta.Add(funcionarioDTO);
                //Puesto [1]
                CPuestoDTO puesto = new CPuestoDTO();
                puesto = CPuestoL.ConstruirPuesto(nombramiento.Puesto, puesto);
                respuesta.Add(puesto);
                //DetallePuesto[2]
                CDetallePuestoDTO detallePuesto = new CDetallePuestoDTO();
                if (nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Especialidad != null)
                {
                    detallePuesto.Especialidad = new CEspecialidadDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Especialidad.PK_Especialidad,
                        DesEspecialidad = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Especialidad.DesEspecialidad
                    };
                }
                else
                {
                    detallePuesto.Especialidad = new CEspecialidadDTO
                    {
                        DesEspecialidad = "NO TIENE"
                    };
                }

                detallePuesto.EscalaSalarial = new CEscalaSalarialDTO
                {
                    IdEntidad = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().EscalaSalarial.PK_Escala,
                    SalarioBase = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().EscalaSalarial.MtoSalarioBase.Value,
                    MontoAumentoAnual = Convert.ToDecimal(nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().EscalaSalarial.MtoAumento)
                };

                if (nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Clase != null)
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Clase.PK_Clase,
                        DesClase = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().Clase.DesClase,
                    };
                }
                else
                {
                    detallePuesto.Clase = new CClaseDTO
                    {
                        DesClase = "NO TIENE"
                    };
                }

                if (nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().OcupacionReal != null)
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        IdEntidad = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().OcupacionReal.PK_OcupacionReal,
                        DesOcupacionReal = nombramiento.Puesto.DetallePuesto.Where(Q => Q.IndEstadoDetallePuesto == 1 || Q.IndEstadoDetallePuesto == null).OrderByDescending(O => O.FecRige).FirstOrDefault().OcupacionReal.DesOcupacionReal
                    };
                }
                else
                {
                    detallePuesto.OcupacionReal = new COcupacionRealDTO
                    {
                        DesOcupacionReal = "NO TIENE"
                    };
                }
                respuesta.Add(detallePuesto);
                //Desgloce salarial Q1 [3]
                respuesta.Add(CDesgloseSalarialL.DesgloseSalarialGeneral(registro.DesgloseSalarial));
                //Desgloce salarial Q2 [4]
                respuesta.Add(CDesgloseSalarialL.DesgloseSalarialGeneral(registro.DesgloseSalarial1));
                //CNombramientoDTO [5]
                respuesta.Add(new CNombramientoDTO { IdEntidad = nombramiento.PK_Nombramiento, FecRige = Convert.ToDateTime(nombramiento.FecRige), FecVence = Convert.ToDateTime(nombramiento.FecVence) });
                List<DetalleTiempoExtra> detallesDatos;
                if (!doble)
                    detallesDatos = registro.DetalleTiempoExtra.Where(D => !D.TipoExtra.DesTipExtra.Contains("Addendum")).ToList();
                else if (doble)
                {
                    detallesDatos = registro.DetalleTiempoExtra.Where(D => D.TipoExtra.DesTipExtra.Contains("Addendum") && D.FecRegistro != null &&
                    Convert.ToDateTime(D.FecRegistro).Year == Convert.ToDateTime(fechaRegistro).Year &&
                    Convert.ToDateTime(D.FecRegistro).Month == Convert.ToDateTime(fechaRegistro).Month &&
                    Convert.ToDateTime(D.FecRegistro).Day == Convert.ToDateTime(fechaRegistro).Day &&
                    Convert.ToDateTime(D.FecRegistro).Hour == Convert.ToDateTime(fechaRegistro).Hour &&
                    Convert.ToDateTime(D.FecRegistro).Minute == Convert.ToDateTime(fechaRegistro).Minute &&
                    Convert.ToDateTime(D.FecRegistro).Second == Convert.ToDateTime(fechaRegistro).Second
                    ).ToList();
                }
                    
                else
                    detallesDatos = registro.DetalleTiempoExtra.ToList();
                List<CDetalleTiempoExtraDTO> detalles = new List<CDetalleTiempoExtraDTO>();
                foreach (var detalle in detallesDatos)
                {
                    detalles.Add(ConvertirDatosDetalleTiempoExtraADto(detalle));
                }
                //Registro tiempo extra [6]
                CRegistroTiempoExtraDTO registroDTO = ConvertirDatosRegistroTiempoExtraADto(registro);
                registroDTO.Detalles = detalles;
                registroDTO.Periodo = FormatearPeriodoVista(registro.IndPeriodo);
                registroDTO.FecRegistroDetalles = fechaRegistro;
                var datoUsuario = contexto.BitacoraUsuario.FirstOrDefault(Q => Q.CodModulo == 16 && Q.CodObjetoEntidad == registroDTO.IdEntidad);
                if (datoUsuario != null)
                {
                    if (datoUsuario.Usuario != null)
                    {
                        registroDTO.Mensaje = contexto.Usuario.FirstOrDefault(Q => Q.PK_Usuario == datoUsuario.Usuario.PK_Usuario).NomUsuario;
                    }
                    else
                    {
                        registroDTO.Mensaje = "vacio";
                    }
                }
                if (registro.Presupuesto != null)
                {
                    registroDTO.Presupuesto = new CPresupuestoDTO
                    {
                        IdEntidad = registro.Presupuesto.PK_Presupuesto,
                        IdDireccionPresupuestaria = registro.Presupuesto.IdDireccionPresupuestaria,
                        Area = registro.Presupuesto.CodArea,
                        Actividad = registro.Presupuesto.CodActividad,
                        CodigoPresupuesto = registro.Presupuesto.IdPresupuesto
                    };
                }
                else
                {
                    registroDTO.Presupuesto = new CPresupuestoDTO
                    {
                        Mensaje = "No se encontró el presupuesto"
                    };
                }
                if (registro.Clase != null)
                {
                    registroDTO.Clase = new CClaseDTO
                    {
                        IdEntidad = registro.Clase.PK_Clase,
                        DesClase = registro.Clase.DesClase
                    };
                }
                else
                {
                    registroDTO.Clase = new CClaseDTO
                    {
                        Mensaje = "No se encontró la clase"
                    };
                }
                respuesta.Add(registroDTO);
            }
            catch (Exception ex)
            {
                respuesta.Clear();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = ex.Message
                });
            }
            return respuesta;
        }
        public CRespuestaDTO BuscarRegistroTiempoExtra(string cedula, string periodo)
        {
            int codigo = -1;
            try
            {
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);
                var datos = intermedio.BuscarRegistroTiempoExtra(cedula, FormatearPeriodoBase(periodo));
                if (datos.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
                RegistroTiempoExtra registro = (RegistroTiempoExtra)datos.Contenido;

                var fechaVariable = (new DateTime(Convert.ToInt32(registro.IndPeriodo.Substring(2, 4)), Convert.ToInt32(registro.IndPeriodo.Substring(0, 2)), 1)).AddDays(90);
                //AQUI, SUSTITUIR CUALQUIERA DE ESTAS CÉDULAS POR LA QUE OCUPA.
                if (fechaVariable <= DateTime.Now && (cedula != "0061820503" && cedula != "0023690987" && cedula != "0023640073" && cedula != "0024240474" && 
                    cedula != "0041400858" && cedula != "0032670456" && cedula != "0033450413" && cedula != "0033550939" && cedula != "0017340559" &&
                    cedula != "0110840773" && cedula != "0032960558" && cedula != "0033910088" && cedula != "0033610276" && cedula != "0033380143" &&
                    cedula != "0024810173" && cedula != "0061820503" && cedula != "0052700239" && cedula != "0024240674" && cedula != "0023810875" &&
                    cedula != "0023980154" && cedula != "0024260772" && cedula != "0023640073" && cedula != "0062940267" &&
                    cedula != "0110910103" && cedula != "0111630434"))
                {
                    codigo = -2;
                    throw new Exception("La fecha de pago para el registro de tiempo extra que intenta modificar ya se cumplió");
                }
                //Detalle tiempo extra
                List<DetalleTiempoExtra> detallesDatos;
                detallesDatos = registro.DetalleTiempoExtra.Where(D => !D.TipoExtra.DesTipExtra.Contains("Addendum")).ToList();

                List<CDetalleTiempoExtraDTO> detalles = new List<CDetalleTiempoExtraDTO>();
                foreach (var detalle in detallesDatos)
                {
                    detalles.Add(ConvertirDatosDetalleTiempoExtraADto(detalle));
                }
                //Registro tiempo extra
                CRegistroTiempoExtraDTO registroDTO = ConvertirDatosRegistroTiempoExtraADto(registro);
                registroDTO.Detalles = detalles;
                registroDTO.Periodo = periodo;
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = registroDTO
                };

            }
            catch(Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = codigo,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        public List<CRegistroTiempoExtraDTO> BuscarTiempoExtraFiltros(string cedula, DateTime? fechaDesde, DateTime? fechaHasta,
            string coddivision, string coddireccion, string coddepartamento, string codseccion, int estado, bool doble)
        {
            List<CRegistroTiempoExtraDTO> resultado = new List<CRegistroTiempoExtraDTO>();

            CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);

            if (doble)
            {
                var registrosDobles = intermedio.BuscarTiempoExtraDobleFiltros(cedula, fechaDesde, fechaHasta, Convert.ToInt32(coddivision), Convert.ToInt32(coddireccion),
                                                                               Convert.ToInt32(coddepartamento), Convert.ToInt32(codseccion), estado);

                var datosRegistro = new List<DetalleTiempoExtra>();

                if (registrosDobles.Count > 0)
                {
                    datosRegistro = registrosDobles.GroupBy(R => R.FK_RegistroTiempoExtra).Select(F => F.First()).ToList();
                    foreach (var item in datosRegistro)
                    {
                        Nombramiento nombramiento = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento;
                        CSeccionDTO seccion;
                        if (nombramiento.Puesto?.UbicacionAdministrativa?.Seccion != null)
                        {
                            seccion = new CSeccionDTO
                            {
                                IdEntidad = nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion,
                                NomSeccion = nombramiento.Puesto.UbicacionAdministrativa.Seccion.NomSeccion
                            };
                        }
                        else
                        {
                            seccion = new CSeccionDTO
                            {
                                NomSeccion = "NO TIENE"
                            };
                        }
                        CDesgloseSalarialDTO desgloceQ1 = CDesgloseSalarialL.DesgloseSalarialGeneral(item.RegistroTiempoExtra.DesgloseSalarial);
                        CDesgloseSalarialDTO desgloceQ2 = CDesgloseSalarialL.DesgloseSalarialGeneral(item.RegistroTiempoExtra.DesgloseSalarial1);
                        CFuncionarioDTO funcionario = new CFuncionarioDTO
                        {
                            IdEntidad = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario.PK_Funcionario,
                            Cedula = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario,
                            Nombre = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario.NomFuncionario,
                            PrimerApellido = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario.NomPrimerApellido,
                            SegundoApellido = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        };
                        List<CDetalleTiempoExtraDTO> detallesGrupo = new List<CDetalleTiempoExtraDTO>();
                        foreach (var detalle in registrosDobles.Where(P => P.FK_RegistroTiempoExtra == item.RegistroTiempoExtra.PK_RegistroTiempoExtra).ToList())
                        {
                            detallesGrupo.Add(ConvertirDatosDetalleTiempoExtraADto(detalle));
                        }
                        CRegistroTiempoExtraDTO registroTE = new CRegistroTiempoExtraDTO
                        {
                            IdEntidad = item.RegistroTiempoExtra.PK_RegistroTiempoExtra,
                            FecPago = Convert.ToDateTime(item.RegistroTiempoExtra.FecPago),
                            Estado = (EstadoExtraEnum)item.RegistroTiempoExtra.IndEstadoExtra,
                            Clase = (item.RegistroTiempoExtra.Clase != null) ? new CClaseDTO
                            {
                                IdEntidad = item.RegistroTiempoExtra.Clase.PK_Clase,
                                DesClase = item.RegistroTiempoExtra.Clase.DesClase
                            } : new CClaseDTO { Mensaje = "No se encontro la clase" },
                            Seccion = seccion,
                            QuincenaA = desgloceQ1,
                            QuincenaB = desgloceQ2,
                            Funcionario = funcionario,
                            Detalles = detallesGrupo,
                            EstadoDetalles = (EstadoDetalleExtraEnum)item.IndEstado,
                            FecRegistroDetalles = Convert.ToDateTime(item.FecRegistro),
                            FechVenceNombramiento = Convert.ToDateTime(nombramiento.FecVence),
                            Ocupacion = nombramiento.Puesto?.DetallePuesto?.FirstOrDefault().OcupacionReal?.DesOcupacionReal != null ?
                            nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal : "NO TIENE",
                            Mensaje = item.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.CodPuesto,
                            Periodo = item.RegistroTiempoExtra.IndPeriodo
                        };
                        resultado.Add(registroTE);
                    }
                }
                //else
                //{
                //    throw new Exception("No se encontraron resultados asociados a la búsqueda establecida");
                //}
            }
            else
            {
                var registrosTE = intermedio.BuscarTiempoExtraFiltros(cedula, fechaDesde, fechaHasta, coddivision != null ? Convert.ToInt32(coddivision) : 0,
                                                                coddireccion != null ? Convert.ToInt32(coddireccion) : 0,
                                                                coddepartamento != null ? Convert.ToInt32(coddepartamento) : 0,
                                                                codseccion != null ? Convert.ToInt32(codseccion) : 0,
                                                                estado,
                                                                doble);
                foreach (var item in registrosTE)
                {

                    Nombramiento nombramiento = item.DesgloseSalarial.Nombramiento;
                    CSeccionDTO seccion;
                    if (nombramiento.Puesto?.UbicacionAdministrativa?.Seccion != null)
                    {
                        seccion = new CSeccionDTO
                        {
                            IdEntidad = nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion,
                            NomSeccion = nombramiento.Puesto.UbicacionAdministrativa.Seccion.NomSeccion
                        };
                    }
                    else
                    {
                        seccion = new CSeccionDTO
                        {
                            NomSeccion = "NO TIENE"
                        };
                    }
                    CDesgloseSalarialDTO desgloceQ1 = CDesgloseSalarialL.DesgloseSalarialGeneral(item.DesgloseSalarial);
                    CDesgloseSalarialDTO desgloceQ2 = CDesgloseSalarialL.DesgloseSalarialGeneral(item.DesgloseSalarial1);
                    CFuncionarioDTO funcionario = new CFuncionarioDTO
                    {
                        IdEntidad = item.DesgloseSalarial.Nombramiento.Funcionario.PK_Funcionario,
                        Cedula = item.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario,
                        Nombre = item.DesgloseSalarial.Nombramiento.Funcionario.NomFuncionario,
                        PrimerApellido = item.DesgloseSalarial.Nombramiento.Funcionario.NomPrimerApellido,
                        SegundoApellido = item.DesgloseSalarial.Nombramiento.Funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    };
                    List<CDetalleTiempoExtraDTO> detallesGrupo = new List<CDetalleTiempoExtraDTO>();
                    foreach (var detalle in item.DetalleTiempoExtra)
                    {
                        if (!detalle.TipoExtra.DesTipExtra.Contains("Addendum"))
                        {
                            detallesGrupo.Add(ConvertirDatosDetalleTiempoExtraADto(detalle));
                        }
                    }
                    CRegistroTiempoExtraDTO registroTE = new CRegistroTiempoExtraDTO
                    {
                        IdEntidad = item.PK_RegistroTiempoExtra,
                        FecPago = Convert.ToDateTime(item.FecPago),
                        Estado = (EstadoExtraEnum)item.IndEstadoExtra,
                        Clase = (item.Clase != null) ? new CClaseDTO
                        {
                            IdEntidad = item.Clase.PK_Clase,
                            DesClase = item.Clase.DesClase
                        } : new CClaseDTO { Mensaje = "No se encontro la clase" },
                        Seccion = seccion,
                        QuincenaA = desgloceQ1,
                        QuincenaB = desgloceQ2,
                        Funcionario = funcionario,
                        Detalles = detallesGrupo,
                        EstadoDetalles = (EstadoDetalleExtraEnum)item.DetalleTiempoExtra.ElementAt(0).IndEstado,
                        FecRegistroDetalles = Convert.ToDateTime(item.DetalleTiempoExtra.ElementAt(0).FecRegistro),
                        FechVenceNombramiento = Convert.ToDateTime(nombramiento.FecVence),
                        Ocupacion = nombramiento.Puesto?.DetallePuesto?.FirstOrDefault().OcupacionReal?.DesOcupacionReal != null ?
                            nombramiento.Puesto.DetallePuesto.FirstOrDefault().OcupacionReal.DesOcupacionReal : "NO TIENE",
                        Presupuesto = new CPresupuestoDTO { IdUnidadPresupuestaria = item.Presupuesto.IdPresupuesto },
                        Mensaje = item.DesgloseSalarial.Nombramiento.Puesto.CodPuesto,
                        Periodo = item.IndPeriodo
                    };
                    resultado.Add(registroTE);
                }
            }
            return resultado;
        }

        public CRespuestaDTO BuscarArchivo(int id)
        {
            try
            {
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);
                var datos = intermedio.BuscarArchivo(id);
                if (((CRespuestaDTO)datos).Codigo < 0)
                {
                    throw new Exception(((CRespuestaDTO)datos).Mensaje);
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = ((CRespuestaDTO)datos).Contenido
                };
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = ex.Message
                };
            }
        }

        public CRespuestaDTO AnularRegistroTiempoExtra(int id, DateTime fechaCarga, bool doble, string estado)
        {
            try
            {
                CRespuestaDTO respuesta = new CRespuestaDTO();
                CRegistroTiempoExtraD intermedio = new CRegistroTiempoExtraD(contexto);
                int datoEstado = DeterminarEstadoRegistro(estado);

                respuesta = intermedio.AnularRegistroTiempoExtra(id, fechaCarga, doble, datoEstado);
                if (respuesta.Codigo < 0)
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        private int DeterminarEstadoRegistro(string estado)
        {
            switch (estado)
            {
                case "Anular":
                    return 2;
                case "Aprobar":
                    return 4;
                case "Rechazar":
                    return 5;
                default:
                    return 0;
            }
        }

        /*******************************************************************************************************/

        public static string FormatearPeriodoBase(string periodo)
        {
            string[] div = periodo.Split(' ');
            int mes = DateTime.ParseExact(div[0], "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
            return (mes < 10 ? "0" : "") + mes.ToString() + div[1];
        }
        
        public static string FormatearPeriodoVista(string periodo)
        {
            int mes = Convert.ToInt32(periodo.Substring(0, 2));
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[mes - 1][0].ToString().ToUpper() +
               (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[mes - 1]).Substring(1) + " " + periodo.Substring(2);
        }
        #endregion

        public static string DefinirDesglocePago(DateTime periodoExtras)
        {
            string respuesta = "";

            if (periodoExtras.Day > 15)
            {

                respuesta += "02/";
            }
            else
            {
                respuesta += "01/";
            }

            if (periodoExtras.Month < 10)
            {
                respuesta += "0" + periodoExtras.Month.ToString() + "/" + periodoExtras.Year.ToString();
            }
            else
            {
                respuesta += periodoExtras.Month.ToString() + "/" + periodoExtras.Year.ToString();
            }

            return respuesta;
        }

        public CBaseDTO ActualizarObservacionEstado(int registro, string observacion, bool doble)
        {
            try
            {
                var intermedio = new CRegistroTiempoExtraD(contexto);

                var resultado = intermedio.ActualizarObservacionEstado(registro, observacion, doble);

                if (resultado.Codigo != -1)
                {
                    return ConvertirDatosRegistroTiempoExtraADto((RegistroTiempoExtra)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }
    }
}
