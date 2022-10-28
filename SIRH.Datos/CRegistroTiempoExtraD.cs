 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos.Helpers;
using SIRH.DTO;
using System.Data.Entity;

namespace SIRH.Datos
{
    public class CRegistroTiempoExtraD
    {
        #region Variables

        SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CRegistroTiempoExtraD(SIRHEntities contextoGlobal)
        {
            entidadBase = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        public List<RegistroTiempoExtra> RetornarTiempoExtra()
        {
            return entidadBase.RegistroTiempoExtra.ToList();
        }
        //-----------------------------------------------------------------------------------------------------------------------------
        public CRespuestaDTO RegistrarTiempoExtra(string cedula, RegistroTiempoExtra registro, CDesgloseSalarialDTO desgloseA, CDesgloseSalarialDTO desgloseB)
        {
            try
            {
                var desgloseDatos1 = entidadBase.DesgloseSalarial.Include("RegistroTiempoExtra")
                                                            .Include("RegistroTiempoExtra.DetalleTiempoExtra")
                                                            .Include("RegistroTiempoExtra.DetalleTiempoExtra.TipoExtra")
                                                            .Include("Nombramiento")
                                                            .Include("Nombramiento.Funcionario")
                                                            .FirstOrDefault(D => D.PK_DesgloseSalarial == desgloseA.IdEntidad);
                var desgloseDatos2 = entidadBase.DesgloseSalarial.Include("RegistroTiempoExtra")
                                                            .Include("RegistroTiempoExtra.DetalleTiempoExtra")
                                                            .Include("RegistroTiempoExtra.DetalleTiempoExtra.TipoExtra")
                                                            .Include("Nombramiento")
                                                            .Include("Nombramiento.Funcionario")
                                                            .FirstOrDefault(D => D.PK_DesgloseSalarial == desgloseB.IdEntidad);

                if (desgloseDatos1 == null || desgloseDatos2 == null)
                {
                    throw new Exception("No se encontró un desglose salarial para el mes indicado");
                }
                if(desgloseDatos1.Nombramiento.PK_Nombramiento != desgloseDatos2.Nombramiento.PK_Nombramiento)
                {
                    throw new Exception("Los desgloses de ambas quincenas deben pertenecer al mismo nombramiento, contacte al encargado.");
                }

                Nombramiento nombramiento = desgloseDatos1.Nombramiento;
                if(nombramiento.Funcionario.IdCedulaFuncionario != cedula)
                {
                    throw new Exception("La cédula y los datos del nombramineto no coinciden, contacte al encargado.");
                }
                RegistroTiempoExtra actual = desgloseDatos1.RegistroTiempoExtra.FirstOrDefault(R => R.PK_RegistroTiempoExtra == registro.PK_RegistroTiempoExtra);
                if(actual != null) //Actualiza
                {
                    actual.FecEmision = registro.FecEmision;
                    actual.FecPago = registro.FecPago;
                    actual.FK_Clase = registro.Clase.PK_Clase;
                    actual.FK_Presupuesto = registro.Presupuesto.PK_Presupuesto;
                    actual.NumOficioJustificacion = registro.NumOficioJustificacion;
                    actual.Justificacion = registro.Justificacion;
                    actual.ArchivoJustificacion = registro.ArchivoJustificacion;
                    //NO SE PUEDEN QUITAR DIAS YA AGREGADOS, si se marcaran como eliminados, no se deberia poder generar reportes de eso, ya que sería un conglomerado de fechas que podrian ser repetidas.
                    foreach(var detalle in actual.DetalleTiempoExtra)
                    {
                        if(!detalle.TipoExtra.DesTipExtra.Contains("Addendum"))
                        {
                            DetalleTiempoExtra nuevoDetalle = registro.DetalleTiempoExtra.FirstOrDefault(D => D.FecInicio == detalle.FecInicio && !D.TipoExtra.DesTipExtra.Contains("Addendum"));
                            if(nuevoDetalle != null)
                            {
                                detalle.FecFin = nuevoDetalle.FecFin;
                                detalle.IndHoraInicio = nuevoDetalle.IndHoraInicio;
                                detalle.IndHoraFin = nuevoDetalle.IndHoraFin;
                                detalle.FK_TipoExtra = nuevoDetalle.TipoExtra.PK_TipoExtra;
                                registro.DetalleTiempoExtra.Remove(nuevoDetalle);
                            }
                        }
                    }
                    foreach(var detalle in registro.DetalleTiempoExtra)
                    {
                        actual.DetalleTiempoExtra.Add(detalle);
                    }
                }
                else //Registra
                {
                    desgloseDatos1.RegistroTiempoExtra.Add(registro);
                    desgloseDatos2.RegistroTiempoExtra1.Add(registro);
                }
                
                int eval = entidadBase.SaveChanges();
                if (eval > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        IdEntidad = registro.PK_RegistroTiempoExtra,
                        Mensaje = actual == null ? "Los datos se registraron correctamente" : "Los datos se actualizaron correctamente"
                    };
                }
                else
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 2,
                        Mensaje = "Los datos se enviaron, pero no se dieron cambios en los mismos"
                    };
                }
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
        //Obtener datos registro recien guardado
        public CRespuestaDTO RegistrarTiempoExtraDoble(int idRegistroTiempoExtra, List<DetalleTiempoExtra> detalles)
        {
            try
            {
                var registro = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                                               .Include("DetalleTiempoExtra.TipoExtra")
                                               .FirstOrDefault(R => R.PK_RegistroTiempoExtra == idRegistroTiempoExtra);
                if(registro == null)
                {
                    throw new Exception("No se encontro el registro de tiempo extra.");
                }
                if(registro.IndEstadoExtra != (int)EstadoExtraEnum.Activo 
                   && registro.IndEstadoExtra != (int)EstadoExtraEnum.Aprobado
                   && registro.IndEstadoExtra != (int)EstadoExtraEnum.Rechazado)
                {
                    throw new Exception("El registro de tiempo extra ya no se encuentra activo");
                }
                foreach (DetalleTiempoExtra detalle in detalles)
                {
                    if(detalle.TipoExtra != null)
                    {
                        detalle.FK_TipoExtra = detalle.TipoExtra.PK_TipoExtra;
                    }
                    registro.DetalleTiempoExtra.Add(detalle);
                }
                int eval = entidadBase.SaveChanges();
                if (eval > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Mensaje = "Los datos de jornadas dobles se registraron correctamente"
                    };
                }
                else
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 2,
                        Mensaje = "Los datos se enviaron, pero no se dieron cambios en los mismos"
                    };
                }

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
        public CRespuestaDTO ObtenerRegistroExtrasSaved(string cedula, string periodo)
        {
            try
            {
                var respuesta = entidadBase.RegistroTiempoExtra.Include("DesgloseSalarial")
                                                                .Include("DesgloseSalarial.DetalleDesgloseSalarial")
                                                                .Include("DesgloseSalarial.Nombramiento")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                                .Include("DesgloseSalarial.Nombramiento.Funcionario")
                                                                .Include("Clase")
                                                                .Include("Presupuesto")
                                                                .Include("DetalleTiempoExtra")
                                                                .Include("DetalleTiempoExtra.TipoExtra")
                                                                .FirstOrDefault(Q => Q.IndEstadoExtra != (int)EstadoExtraEnum.Anulado && Q.IndEstadoExtra != (int)EstadoExtraEnum.Cerrado && Q.IndPeriodo == periodo && Q.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario == cedula);
                if (respuesta == null)
                {
                    throw new Exception("No se encontraron resultados con los criterios de búsqueda establecidos");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = respuesta
                };
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

        public CRespuestaDTO ObtenerRegistroExtrasDetalle(string cedula, string periodo)
        {
            try
            {
                var respuesta = entidadBase.RegistroTiempoExtra.Include("DesgloseSalarial")
                                                                .Include("DesgloseSalarial.DetalleDesgloseSalarial")
                                                                .Include("DesgloseSalarial.Nombramiento")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                                .Include("DesgloseSalarial.Nombramiento.Funcionario")
                                                                .Include("Clase")
                                                                .Include("Presupuesto")
                                                                .Include("DetalleTiempoExtra")
                                                                .Include("DetalleTiempoExtra.TipoExtra")
                                                                .FirstOrDefault(R => R.IndPeriodo == periodo 
                                                                                && R.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario == cedula 
                                                                                && (R.IndEstadoExtra == (int)EstadoExtraEnum.Activo
                                                                                    || R.IndEstadoExtra == (int)EstadoExtraEnum.Aprobado
                                                                                    || R.IndEstadoExtra == (int)EstadoExtraEnum.Rechazado));
                if (respuesta == null)
                {
                    throw new Exception("No se encontraron resultados con los criterios de búsqueda establecidos");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = respuesta
                };
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

        public CRespuestaDTO ObtenerRegistroExtrasDetalle(int id)
        {
            try
            {
                var respuesta = entidadBase.RegistroTiempoExtra.Include("DesgloseSalarial")
                                                                .Include("DesgloseSalarial.DetalleDesgloseSalarial")
                                                                .Include("DesgloseSalarial.Nombramiento")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                                .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                                .Include("DesgloseSalarial.Nombramiento.Funcionario")
                                                                .Include("Clase")
                                                                .Include("Presupuesto")
                                                                .Include("DetalleTiempoExtra")
                                                                .Include("DetalleTiempoExtra.TipoExtra")
                                                                .FirstOrDefault(E => E.PK_RegistroTiempoExtra == id);
                if (respuesta == null)
                {
                    throw new Exception("No se encontraron resultados con los criterios de búsqueda establecidos");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = respuesta
                };
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

        public CRespuestaDTO BuscarRegistroTiempoExtra(string cedula, string periodo) {
            try
            {
                var resultado = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                                                               .Include("DetalleTiempoExtra.TipoExtra")
                                                               .Include("Clase")
                                                               .Include("Presupuesto")
                                                               .FirstOrDefault(Q => (Q.IndEstadoExtra != (int)EstadoExtraEnum.Anulado && Q.IndEstadoExtra != (int)EstadoExtraEnum.Cerrado) && Q.IndPeriodo == periodo
                                                               && Q.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario == cedula);
                if(resultado == null)
                {
                    throw new Exception("No se encontraron resultados con los criterios de búsqueda establecidos");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = resultado
                };
            } catch(Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        public CRespuestaDTO BuscarArchivo(int id)
        {
            try
            {
                var resultado = entidadBase.RegistroTiempoExtra.FirstOrDefault(R => R.PK_RegistroTiempoExtra == id);
                if(resultado == null)
                {
                    throw new Exception("Registro de tiempo extra no encontrado");
                }
                if(resultado.ArchivoJustificacion == null)
                {
                    throw new Exception("El registro no posee un archivo de justificación");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = resultado.ArchivoJustificacion
                };
            } catch(Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = ex.Message
                };
            }
        }

        public CRespuestaDTO ActualizarObservacionEstado(int registro, string observacion, bool doble)
        {
            try
            {
                var resultado = entidadBase.RegistroTiempoExtra.FirstOrDefault(R => R.PK_RegistroTiempoExtra == registro);

                if (resultado != null)
                {
                    if (doble)
                    {
                        resultado.ObsEstadoDoble = observacion;
                        entidadBase.SaveChanges();
                    }
                    else
                    {
                        resultado.ObsEstado = observacion;
                        entidadBase.SaveChanges();
                    }

                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se encontró el registro de tiempo extra a modificar.");
                }
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

        public CRespuestaDTO AnularRegistroTiempoExtra(int idRegistroTiempoExtra, DateTime fechaCarga, bool doble, int estado)
        {
            try
            {
                var registro = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                                               .FirstOrDefault(R => R.PK_RegistroTiempoExtra == idRegistroTiempoExtra);
                if (registro == null)
                {
                    throw new Exception("No se encontro el registro de tiempo extra.");
                }
                if ((estado == 2) && ((registro.IndEstadoExtra == (int)EstadoExtraEnum.Anulado) || (registro.IndEstadoExtra == (int)EstadoExtraEnum.Cerrado)))
                {
                    throw new Exception("El registro de tiempo extra ya se había anulado con anterioridad o bien es un registro cerrado (que ya se procesó para pago)");
                }
                if ((estado == 4) && ((registro.IndEstadoExtra != (int)EstadoExtraEnum.Activo) && (registro.IndEstadoExtra != (int)EstadoExtraEnum.Rechazado)))
                {
                    throw new Exception("Sólo se pueden aprobar registros activos");
                }
                if ((estado == 5) && ((registro.IndEstadoExtra != (int)EstadoExtraEnum.Activo) && (registro.IndEstadoExtra != (int)EstadoExtraEnum.Aprobado) && (registro.IndEstadoExtra != (int)EstadoExtraEnum.Rechazado)))
                {
                    throw new Exception("Sólo se pueden rechazar registros activos o aprobados");
                }
                if (doble)
                {
                    foreach (DetalleTiempoExtra detalle in registro.DetalleTiempoExtra)
                    {
                        DateTime buscar = Convert.ToDateTime(fechaCarga);
                        DateTime actual = Convert.ToDateTime(detalle.FecRegistro);
                        if (actual != DateTime.MinValue && actual.Year == buscar.Year && actual.Month == buscar.Month && 
                            actual.Day == buscar.Day &&actual.Hour == buscar.Hour &&
                            actual.Minute == buscar.Minute && actual.Second == buscar.Second)
                        {
                            detalle.IndEstado = estado;
                        }
                    }
                }
                else
                {
                    registro.IndEstadoExtra = estado;
                    foreach (DetalleTiempoExtra detalle in registro.DetalleTiempoExtra)
                    {
                        detalle.IndEstado = estado;
                    }
                }
                int eval = entidadBase.SaveChanges();
                if (eval > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Mensaje = "Los datos modificaron correctamente."
                    };
                }
                else
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 2,
                        Mensaje = "Los datos se enviaron, pero no se dieron cambios en los mismos."
                    };
                }

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
        public List<RegistroTiempoExtra> BuscarTiempoExtraFiltros(string cedula, DateTime? fechaDesde, DateTime? fechaHasta, int? coddivision, int? coddireccion, int? coddepartamento, int? codseccion, int estado, bool doble)
        {
            List<RegistroTiempoExtra> registros = new List<RegistroTiempoExtra>();
            //List<RegistroTiempoExtra> registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
            //                                                                    .Include("DetalleTiempoExtra.TipoExtra")
            //                                                                    .Include("DesgloseSalarial")
            //                                                                    .Include("DesgloseSalarial.Nombramiento")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Funcionario")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
            //                                                                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
            //                                                                    .ToList();
            if (cedula != null)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.DesgloseSalarial?.Nombramiento?.Funcionario?.IdCedulaFuncionario == cedula).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                                            .Include("DetalleTiempoExtra.TipoExtra")
                                            .Include("DesgloseSalarial")
                                            .Include("DesgloseSalarial.Nombramiento")
                                            .Include("DesgloseSalarial.Nombramiento.Funcionario")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                            .Where(R => R.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();
                }
            }
            if (!doble)//Tiempo Extra
            {
                if (estado != 0)
                {
                    if (registros.Count() > 0)
                    {
                        registros = registros.Where(R => R.IndEstadoExtra == 0).ToList();
                        registros = registros.Where(R => R.DetalleTiempoExtra?.Where(D => D.TipoExtra != null && !D.TipoExtra.DesTipExtra.Contains("Addendum")).Count() > 0).ToList();
                    }
                    else
                    {
                        registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                                            .Include("DetalleTiempoExtra.TipoExtra")
                                            .Include("DesgloseSalarial")
                                            .Include("DesgloseSalarial.Nombramiento")
                                            .Include("DesgloseSalarial.Nombramiento.Funcionario")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                            .Where(R => R.IndEstadoExtra == estado).ToList();
                        registros = registros.Where(R => R.DetalleTiempoExtra?.Where(D => D.TipoExtra != null && !D.TipoExtra.DesTipExtra.Contains("Addendum")).Count() > 0).ToList();

                    }
                }

            }
            else if (doble)//Jornada doble
            {
                if (estado != 0)
                {
                    if (registros.Count() > 0)
                    {
                        registros = registros.Where(R => R.DetalleTiempoExtra?.Where(D => D.IndEstado == estado).Count() > 0).ToList();
                        registros = registros.Where(R => R.DetalleTiempoExtra?.Where(D => D.TipoExtra != null && D.TipoExtra.DesTipExtra.Contains("Addendum")).Count() > 0).ToList();
                    }
                    else
                    {
                        registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                            .Include("DetalleTiempoExtra.TipoExtra")
                            .Include("DesgloseSalarial")
                            .Include("DesgloseSalarial.Nombramiento")
                            .Include("DesgloseSalarial.Nombramiento.Funcionario")
                            .Include("DesgloseSalarial.Nombramiento.Puesto")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                            .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                            .Where(R => R.DetalleTiempoExtra.Where(D => D.IndEstado == estado).Count() > 0).ToList();
                        registros = registros.Where(R => R.DetalleTiempoExtra?.Where(D => D.TipoExtra != null && D.TipoExtra.DesTipExtra.Contains("Addendum")).Count() > 0).ToList();
                    }
                }
            }
            if (coddivision != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.Division?.PK_Division == coddivision).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                    .Include("DetalleTiempoExtra.TipoExtra")
                    .Include("DesgloseSalarial")
                    .Include("DesgloseSalarial.Nombramiento")
                    .Include("DesgloseSalarial.Nombramiento.Funcionario")
                    .Include("DesgloseSalarial.Nombramiento.Puesto")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                    .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                    .Where(R => R.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division.PK_Division == coddivision).ToList();
                }
            }
            if (coddireccion != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.DireccionGeneral?.PK_DireccionGeneral == coddireccion).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                        .Include("DetalleTiempoExtra.TipoExtra")
                        .Include("DesgloseSalarial")
                        .Include("DesgloseSalarial.Nombramiento")
                        .Include("DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("DesgloseSalarial.Nombramiento.Puesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral == coddireccion).ToList();
                }
            }
            if (coddepartamento != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.Departamento?.PK_Departamento == coddepartamento).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                        .Include("DetalleTiempoExtra.TipoExtra")
                        .Include("DesgloseSalarial")
                        .Include("DesgloseSalarial.Nombramiento")
                        .Include("DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("DesgloseSalarial.Nombramiento.Puesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento.PK_Departamento == coddepartamento).ToList();
                }
            }
            if (codseccion != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.Seccion?.PK_Seccion == codseccion).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                        .Include("DetalleTiempoExtra.TipoExtra")
                        .Include("DesgloseSalarial")
                        .Include("DesgloseSalarial.Nombramiento")
                        .Include("DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("DesgloseSalarial.Nombramiento.Puesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion == codseccion).ToList();
                }
            }
            if (fechaDesde != null)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.FecEmision >= fechaDesde).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                        .Include("DetalleTiempoExtra.TipoExtra")
                        .Include("DesgloseSalarial")
                        .Include("DesgloseSalarial.Nombramiento")
                        .Include("DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("DesgloseSalarial.Nombramiento.Puesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.FecEmision >= fechaDesde).ToList();
                }
            }
            if (fechaHasta != null)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.FecEmision <= fechaHasta).ToList();
                }
                else
                {
                    registros = entidadBase.RegistroTiempoExtra.Include("DetalleTiempoExtra")
                        .Include("DetalleTiempoExtra.TipoExtra")
                        .Include("DesgloseSalarial")
                        .Include("DesgloseSalarial.Nombramiento")
                        .Include("DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("DesgloseSalarial.Nombramiento.Puesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.FecEmision <= fechaHasta).ToList();
                }
            }

            return registros;
        }

        public List<DetalleTiempoExtra> BuscarTiempoExtraDobleFiltros(string cedula, DateTime? fechaDesde, DateTime? fechaHasta, int? coddivision, int? coddireccion, int? coddepartamento, int? codseccion, int estado)
        {
            List<DetalleTiempoExtra> registros = new List<DetalleTiempoExtra>();
            if (cedula != null)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.DesgloseSalarial?.Nombramiento?.Funcionario?.IdCedulaFuncionario == cedula).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                                            .Include("TipoExtra")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                            .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                            .Where(R => R.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();
                    registros = registros.Where(R => R.TipoExtra != null && R.TipoExtra.DesTipExtra.Contains("Addendum")).ToList();
                }
            }
            if (estado != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.IndEstado == estado).ToList();
                    registros = registros.Where(R => R.TipoExtra != null && R.TipoExtra.DesTipExtra.Contains("Addendum")).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra
                        //.Include("RegistroTiempoExtra")
                        //.Include("TipoExtra")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        //.Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.IndEstado == estado).ToList();
                    registros = registros.Where(R => R.TipoExtra != null && R.TipoExtra.DesTipExtra.Contains("Addendum")).ToList();
                }
            }
            if (coddivision != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.Division?.PK_Division == coddivision).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                    .Include("TipoExtra")
                    .Include("RegistroTiempoExtra.DesgloseSalarial")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                    .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                    .Where(R => R.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division.PK_Division == coddivision).ToList();
                }
            }
            if (coddireccion != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.DireccionGeneral?.PK_DireccionGeneral == coddireccion).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                        .Include("TipoExtra")
                        .Include("RegistroTiempoExtra.DesgloseSalarial")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral == coddireccion).ToList();
                }
            }
            if (coddepartamento != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.Departamento?.PK_Departamento == coddepartamento).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                        .Include("TipoExtra")
                        .Include("RegistroTiempoExtra.DesgloseSalarial")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento.PK_Departamento == coddepartamento).ToList();
                }
            }

            if (codseccion != 0)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.DesgloseSalarial?.Nombramiento?.Puesto?.UbicacionAdministrativa?.Seccion?.PK_Seccion == codseccion).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                        .Include("TipoExtra")
                        .Include("RegistroTiempoExtra.DesgloseSalarial")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion == codseccion).ToList();
                }
            }
            if (fechaDesde != null)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.FecEmision >= fechaDesde).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                        .Include("TipoExtra")
                        .Include("RegistroTiempoExtra.DesgloseSalarial")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.RegistroTiempoExtra.FecEmision >= fechaDesde).ToList();
                }
            }
            if (fechaHasta != null)
            {
                if (registros.Count() > 0)
                {
                    registros = registros.Where(R => R.RegistroTiempoExtra.FecEmision <= fechaHasta).ToList();
                }
                else
                {
                    registros = entidadBase.DetalleTiempoExtra.Include("RegistroTiempoExtra")
                        .Include("TipoExtra")
                        .Include("RegistroTiempoExtra.DesgloseSalarial")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Funcionario")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Division")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                        .Include("RegistroTiempoExtra.DesgloseSalarial.Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                        .Where(R => R.RegistroTiempoExtra.FecEmision <= fechaHasta).ToList();
                }
            }
            return registros;
        }

        //-----------------------------------------------------------------------------------------------------------------------------

        private string DefinirPeriodoPago(DateTime fechapago)
        {
            string respuesta = "";

            if (fechapago.Day > 15)
            {

                respuesta += "02/";
            }
            else
            {
                respuesta += "01/";
            }

            if (fechapago.Month < 10)
            {
                respuesta += "0" + fechapago.Month.ToString() + "/" + fechapago.Year.ToString();
            }
            else
            {
                respuesta += fechapago.Month.ToString() + "/" + fechapago.Year.ToString();
            }

            return respuesta;
        }

        #endregion
    }
}