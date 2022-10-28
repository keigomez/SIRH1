using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;
using System.Data.SqlClient;
using System.Data;

namespace SIRH.Datos
{
    public class CCalificacionNombramientoD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CCalificacionNombramientoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Metodos
        /// <summary>
        /// Guarda la CalificacionDTO del nombramiento
        /// </summary>
        /// <returns>Retorna la calificacion del nombramiento</returns>
       /* public int GuardarCalificacionNombramiento(CalificacionNombramientoDTO CalificacionNombramientoDTO)
        {
            entidadBase.CalificacionNombramientoDTO.Add(CalificacionNombramientoDTO);
            return CalificacionNombramientoDTO.PK_CalificacionNombramiento;
        }*/

        public CRespuestaDTO AgregarCalificacionNombramiento(CalificacionNombramiento calificacionN)
        {
            CRespuestaDTO respuesta;
            try
            {
                calificacionN.Nombramiento = entidadBase.Nombramiento.Where(Q => Q.PK_Nombramiento == calificacionN.Nombramiento.PK_Nombramiento).FirstOrDefault();
                entidadBase.CalificacionNombramiento.Add(calificacionN);

                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = calificacionN
                };

                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };

                return respuesta;
            }
        }

        public CRespuestaDTO AgregarCalificacionNombramientoFuncionario(CalificacionNombramientoFuncionarios calificacionN)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.CalificacionNombramientoFuncionarios
                                        .Where(Q => Q.PeriodoCalificacion.PK_PeriodoCalificacion == calificacionN.PeriodoCalificacion.PK_PeriodoCalificacion
                                                && Q.FK_Funcionario == calificacionN.FK_Funcionario).FirstOrDefault();
                if (dato == null)
                {
                    entidadBase.CalificacionNombramientoFuncionarios.Add(calificacionN);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacionN
                    };
                }
                else
                {
                    throw new Exception("El Funcionario ya está registrado para la Calificación del periodo seleccionado.");
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

        public CRespuestaDTO ActualizarCalificacionNombramientoFuncionario(CalificacionNombramientoFuncionarios calificacionN)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoOld = entidadBase.CalificacionNombramientoFuncionarios
                                        .Where(Q => Q.PK_CalificacionNombramiento == calificacionN.PK_CalificacionNombramiento)
                                        .FirstOrDefault();
                if (datoOld != null)
                {
                    datoOld.FK_Nombramiento = calificacionN.FK_Nombramiento;
                    datoOld.FK_Puesto = calificacionN.FK_Puesto;
                    datoOld.FK_DetallePuesto = calificacionN.FK_DetallePuesto;
                    datoOld.FK_Seccion = calificacionN.FK_Seccion;
                    datoOld.FK_Departamento = calificacionN.FK_Departamento;
                    datoOld.FK_DireccionGeneral = calificacionN.FK_DireccionGeneral;
                    datoOld.FK_Division = calificacionN.FK_Division;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacionN
                    };
                }
                else
                {
                    throw new Exception("No se encuentra el registro de la Calificación del periodo seleccionado.");
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

        public CRespuestaDTO AgregarCalificacionHistorico(C_EMU_Calificacion calificacionN)
        {
            CRespuestaDTO respuesta;
            try
            {
                var historico = entidadBase.C_EMU_Calificacion.Where(Q => Q.Periodo.Contains(calificacionN.Periodo) && Q.Cedula.Contains(calificacionN.Cedula)).FirstOrDefault();
                if (historico == null)
                {
                    int periodo = Convert.ToInt32(calificacionN.Periodo);
                    var calif = entidadBase.CalificacionNombramiento.Where(Q => Q.PeriodoCalificacion.PK_PeriodoCalificacion == periodo && Q.IndEstado == 1  && Q.Nombramiento.Funcionario.IdCedulaFuncionario.Contains(calificacionN.Cedula)).FirstOrDefault();
                    if (calif == null)
                    {
                        entidadBase.C_EMU_Calificacion.Add(calificacionN);
                        entidadBase.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Ya existe una Calificación para ese año.");
                    }
                }
                else
                {
                    throw new Exception("Ya existe una Calificación para ese año.");
                }

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = calificacionN
                };

                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };

                return respuesta;
            }
        }
        //public CRespuestaDTO GuardarCalificacionNombramiento(CalificacionNombramiento calificacion)
        //{
        //    CRespuestaDTO respuesta;
        //    try
        //    {
        //        entidadBase.CalificacionNombramiento.Add(calificacion);
        //        entidadBase.SaveChanges();
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = 1,
        //            Contenido = calificacion
        //        };
        //        return respuesta;
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { MensajeError = error.Message }
        //        };
        //        return respuesta;
        //    }
        //}


        public CRespuestaDTO EditarCalificacionNombramiento(int codCalificacion)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosCalificacion = entidadBase.CalificacionNombramiento.Where(DE => DE.PK_CalificacionNombramiento == codCalificacion).FirstOrDefault();

                if (datosCalificacion != null)
                {
                    datosCalificacion.IndEstado = 2;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosCalificacion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos de la Calificación.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }

        public CRespuestaDTO EditarCalificacionNombramientoFuncionario(CalificacionNombramientoFuncionarios calificacionN)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosCalificacion = entidadBase.CalificacionNombramientoFuncionarios
                                        .Where(Q => Q.PeriodoCalificacion.PK_PeriodoCalificacion == calificacionN.PeriodoCalificacion.PK_PeriodoCalificacion
                                                && Q.FK_Funcionario == calificacionN.FK_Funcionario)
                                        .FirstOrDefault();

                if (datosCalificacion != null)
                {
                    datosCalificacion.IdJefeInmediato = calificacionN.IdJefeInmediato;
                    datosCalificacion.IdJefeSuperior = calificacionN.IdJefeSuperior;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosCalificacion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos de la Calificación.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }

        public CRespuestaDTO RatificarCalificacionNombramiento(int codCalificacion, int codRatificado)
        {
            // codRatificado (0-Default, 1-Ratificado, 2-Modificado)
            CRespuestaDTO respuesta;

            try
            {
                var datosCalificacion = entidadBase.CalificacionNombramiento.Where(DE => DE.PK_CalificacionNombramiento == codCalificacion).FirstOrDefault();

                if (datosCalificacion != null)
                {
                    datosCalificacion.IndRatificado = codRatificado;
                    datosCalificacion.FecRatificacion = DateTime.Today;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosCalificacion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos de la Calificación.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }


        public CRespuestaDTO RecibirCalificacionNombramiento(int codCalificacion, int codEntregado, int codConformidad)
        {
            // codRatificado (0-Default, 1-Ratificado, 2-Modificado)
            CRespuestaDTO respuesta;

            try
            {
                var datosCalificacion = entidadBase.CalificacionNombramiento.Where(DE => DE.PK_CalificacionNombramiento == codCalificacion).FirstOrDefault();

                if (datosCalificacion != null)
                {
                    datosCalificacion.IndEntregado = codEntregado;
                    datosCalificacion.IndConformidad = codConformidad;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosCalificacion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos de la Calificación.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }

        public CRespuestaDTO AsignarJefatura(int periodo, int codFuncionario, int codJefatura)
        {
            CRespuestaDTO respuesta;
            int idJefeSuperior = 0;

            try
            {
                var datosCalificacion = entidadBase.CalificacionNombramientoFuncionarios.Where(Q => Q.FK_PeriodoCalificacion == periodo && Q.FK_Funcionario == codFuncionario).FirstOrDefault();

                if (datosCalificacion != null)
                {
                    if (codJefatura > 0)
                    {
                        // Obtener el jefe inmediato del jefe inmediato
                        var datoJefeSuperior = entidadBase.CalificacionNombramientoFuncionarios.Where(Q => Q.FK_PeriodoCalificacion == periodo && Q.FK_Funcionario == codJefatura).FirstOrDefault();
                        if (datoJefeSuperior != null)
                            idJefeSuperior = Convert.ToInt32(datoJefeSuperior.IdJefeInmediato);
                    }

                    datosCalificacion.IdJefeInmediato = codJefatura;
                    datosCalificacion.IdJefeSuperior = idJefeSuperior;
                    entidadBase.SaveChanges();

                    // Cambiar los datos 
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosCalificacion
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos del Funcionario para el periodo de Calificación.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }


        /// <summary>
        /// Obtiene lista de las Calificaciones de NombramientoDTO
        /// </summary>
        /// <returns>Retorna la lista de calificaciones de nombramientos</returns>
        public List<CalificacionNombramiento> CargarCalificacionesDeNombramientos()
        {
            List<CalificacionNombramiento> resultados = new List<CalificacionNombramiento>();

            resultados = entidadBase.CalificacionNombramiento.ToList();

            return resultados;
        }

        /// <summary>
        /// Carga la calificacion de NombramientoDTO de la BD
        /// </summary>
        /// <returns>Retorna calificacion de nombramiento</returns>
        public CalificacionNombramiento CargarCalificacionNombramientoPorID(int idCalificacionNombramiento)
        {
            CalificacionNombramiento resultado = new CalificacionNombramiento();

            resultado = entidadBase.CalificacionNombramiento.Where(R => R.PK_CalificacionNombramiento == idCalificacionNombramiento).FirstOrDefault();

            return resultado;
        }
        
        /// <summary>
        /// POR CEDULA    
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public CalificacionNombramiento CargarCalificacionNombramientoCedula(string cedula)
        {
            CalificacionNombramiento resultado = new CalificacionNombramiento();

            var items = entidadBase.CalificacionNombramiento.Include("CalificacionDTO").Where(R => R.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();

            resultado = items.OrderByDescending(Q => Q.FK_PeriodoCalificacion).FirstOrDefault();

            return resultado;
        }

        public CRespuestaDTO ListarFuncionariosCalificacion(int idPeriodo)
        {
            try
            {
                var datos = entidadBase.USP_LISTAR_FUNCIONARIOS_CALIFICACION(idPeriodo).ToList();

                if (datos != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("Ocurrió un error al descargar los datos de los funcionarios");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        public CRespuestaDTO ListarFuncionariosJefatura(int idPeriodo, string cedulaJefatura)
        {
            try
            {
                //var datos = entidadBase.USP_LISTAR_FUNCIONARIOS_CALIFICACION_JEFATURA(idPeriodo, cedulaJefatura).ToList();
                
                var datos = (from FC in entidadBase.CalificacionNombramientoFuncionarios
                            join F in entidadBase.Funcionario on FC.FK_Funcionario equals F.PK_Funcionario
                            join J in entidadBase.Funcionario on FC.IdJefeInmediato equals J.PK_Funcionario
                            where J.IdCedulaFuncionario.Contains(cedulaJefatura)
                            && FC.PeriodoCalificacion.PK_PeriodoCalificacion == idPeriodo
                             select new CFuncionarioDTO
                            {
                                IdEntidad = FC.FK_Funcionario,
                                Cedula = F.IdCedulaFuncionario,
                                Nombre = F.NomFuncionario.TrimEnd() + " " + F.NomPrimerApellido.TrimEnd() + " " + F.NomSegundoApellido.TrimEnd()
                            }).OrderBy(Q => Q.Nombre).ToList();

                if (datos != null)
                {
                    //datos = datos.Where(Q => Q.IdCedulaJefeInmediato == cedulaJefatura).ToList();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("Ocurrió un error al descargar los datos de los funcionarios");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        public List<CalificacionNombramiento> ListarCalificacionNombramientoCedula(string cedula)
        {
            List<CalificacionNombramiento> resultado = new List<CalificacionNombramiento>();

            resultado = entidadBase.CalificacionNombramiento.Include("Calificacion").Where(R => R.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();

            return resultado.OrderBy(Q => Q.FK_PeriodoCalificacion).ToList();
        }

        public List<C_EMU_Calificacion> ListarCalificacionHistoricoCedula(string cedula)
        {
            List<C_EMU_Calificacion> resultado = new List<C_EMU_Calificacion>();

            resultado = entidadBase.C_EMU_Calificacion.Where(R => R.Cedula == cedula).ToList();

            return resultado.OrderBy(Q => Q.Periodo).ToList();
        }

        /// <summary>
        /// POR PARÁMETROS
        /// </summary>
        /// <param name="senas"></param>
        /// <returns></returns>
        public CalificacionNombramiento CargarCalificacionNombramientoParam(int Periodo)
        {
            CalificacionNombramiento resultado = new CalificacionNombramiento();

            resultado = entidadBase.CalificacionNombramiento.Where(R => R.FK_PeriodoCalificacion == Periodo).FirstOrDefault();

            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public List<CalificacionNombramiento> ListarCalificaciones(string cedula)
        {
            List<CalificacionNombramiento> resultado = new List<CalificacionNombramiento>();

            try
            {
                var datospregunta = entidadBase.CalificacionNombramiento.Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();
                if (datospregunta != null)
                {
                    resultado = datospregunta;
                    return resultado.OrderByDescending(Q => Q.PK_CalificacionNombramiento).ToList();
             }
                else
                {
                    throw new Exception("No se encontraron calificaciones.");
               }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
              return resultado;
        }
        
        public List<CalificacionNombramiento> ListarCalificacionesJefatura(int periodo, int idJefatura, int idFuncionario)
        {
            List<CalificacionNombramiento> resultado = new List<CalificacionNombramiento>();

            try
            {
                var datos = entidadBase.CalificacionNombramiento.Where(C => C.PeriodoCalificacion.PK_PeriodoCalificacion == periodo).ToList();

                if (idJefatura > 0)
                {
                    datos = datos.Where(C => C.IdJefeInmediato == idJefatura).ToList();
                }

                if (idFuncionario > 0)
                {
                    datos = datos.Where(C => C.Nombramiento.Funcionario.PK_Funcionario == idFuncionario).ToList();
                }           

                if (datos != null && datos.Count() > 0)
                {
                    resultado = datos;
                    return resultado.OrderBy(Q => Q.Nombramiento.Funcionario.NomFuncionario).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron calificaciones.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        public List<CalificacionNombramiento> ListarCalificacionesPeriodo(int periodo)
        {
            List<CalificacionNombramiento> resultado = new List<CalificacionNombramiento>();

            try
            {
                var datospregunta = entidadBase.CalificacionNombramiento.Where(C => C.PeriodoCalificacion.PK_PeriodoCalificacion == periodo).ToList();
                if (datospregunta != null)
                {
                    return datospregunta;
                }
                else
                {
                    throw new Exception("No se encontraron calificaciones.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }
        public CRespuestaDTO ObtenerCalificacion(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var calificacion = entidadBase.CalificacionNombramiento
                                            .Include("PeriodoCalificacion")
                                            .Include("DetalleCalificacion")
                                            .Include("DetalleCalificacion.CatalogoPregunta")
                                            .Include("DetalleCalificacionModificada")
                                            .Include("DetalleCalificacionModificada.CatalogoPregunta")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                            .Where(C => C.PK_CalificacionNombramiento == codigo).FirstOrDefault();

                if (calificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna calificación." }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerCalificacionHistorico(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var calificacion = entidadBase.C_EMU_Calificacion.Where(R => R.ID == codigo).FirstOrDefault();

                if (calificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna calificación." }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerCalificacionFuncionario(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var calificacion = entidadBase.CalificacionNombramientoFuncionarios
                                           .Where(C => C.PK_CalificacionNombramiento == codigo).FirstOrDefault();

                if (calificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna calificación." }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerCalificacionNombramientoFuncionario(int idFuncionario, int idPeriodo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var calificacion = entidadBase.CalificacionNombramientoFuncionarios
                                            .Include("PeriodoCalificacion")
                                            .Include("DetalleCalificacion")
                                            .Include("DetalleCalificacion.CatalogoPregunta")
                                            .Include("DetalleCalificacionModificada")
                                            .Include("DetalleCalificacionModificada.CatalogoPregunta")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                            .Include("Nombramiento.Puesto")
                                            .Include("Nombramiento.Puesto.DetallePuesto")
                                            .Where(C => C.FK_Funcionario == idFuncionario && C.PeriodoCalificacion.PK_PeriodoCalificacion == idPeriodo)
                                            .FirstOrDefault();

                if (calificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna calificación." }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        public CRespuestaDTO DescargarFuncionarioCalificar(string cedula, int periodo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var funcionario = entidadBase.Funcionario.Where(F => F.IdCedulaFuncionario == cedula).FirstOrDefault();

                if (funcionario != null)
                {
                    var calificacion = entidadBase.CalificacionNombramientoFuncionarios.Where(C => C.FK_Funcionario == funcionario.PK_Funcionario && C.PeriodoCalificacion.PK_PeriodoCalificacion == periodo).FirstOrDefault();

                    if (calificacion != null)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = calificacion
                        };

                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("El funcionario no está registrado para la evaluación del periodo " + periodo);
                    }
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario.");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };

                return respuesta;
            }
        }

        public List<CProcAlmacenadoDTO> ListarDatosCCFRSC()
        {
            object T = new object();
            List<CProcAlmacenadoDTO> resultado = new List<CProcAlmacenadoDTO>();
            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            try
            {
                DatosConexion str = new DatosConexion();
                //int resultado = 0;
                //SqlConnection conn = new SqlConnection(str.ConSIRH);
                //  int res = 0;
                SqlConnection conn = new SqlConnection(str.ConSIRH);
                conn.Open();
                SqlCommand commandSP = new SqlCommand("dbo.OptenerDatosCCFE", conn);
                commandSP.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(commandSP);
                // var reader = commandSP.ExecuteReader();

                adp.Fill(ds);
                table = ds.Tables[0];
                foreach (DataRow dr in table.Rows)
                {
                    //foreach (DataColumn dc in table.Columns)
                    //{
                    CProcAlmacenadoDTO temp = new CProcAlmacenadoDTO();
                    temp.Estratos = dr[0].ToString();
                    temp.AbsExcelente = dr[1].ToString();
                    temp.PorcExcelente = dr[2].ToString();
                    temp.AbsMuyBueno = dr[3].ToString();
                    temp.PorcMuyBueno = dr[4].ToString();
                    temp.AbsBueno = dr[5].ToString();
                    temp.PorcBueno = dr[6].ToString();
                    temp.AbsRegular = dr[7].ToString();
                    temp.PorcRegular = dr[8].ToString();
                    temp.AbsDeficiente = dr[9].ToString();
                    temp.PorcDeficiente = dr[10].ToString();
                    temp.TotalEvaluacion = dr[11].ToString();
                    temp.PorcTotalEvalacion = dr[12].ToString();
                    temp.TotalDatosNoEvaluados = dr[13].ToString();
                    temp.PorcTotalDatosNoEvaluados = dr[14].ToString();
                    temp.TotalInstitucional = dr[15].ToString();
                    temp.PorcTotalInstitucional = dr[16].ToString();
                    //resultado.Add(dr[dc.ColumnName].ToString());
                    //}
                    resultado.Add(temp);
                }
                conn.Close();
                return resultado;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return resultado;
        }

        public List<CProcAlmacenadoDatosGeneralesDTO> ListarDatosGFRSC()
        {
            object T = new object();
            List<CProcAlmacenadoDatosGeneralesDTO> resultado = new List<CProcAlmacenadoDatosGeneralesDTO>();
            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            try
            {
                DatosConexion str = new DatosConexion();
                //int resultado = 0;
                //SqlConnection conn = new SqlConnection(str.ConSIRH);
                //  int res = 0;
                SqlConnection conn = new SqlConnection(str.ConSIRH);
                conn.Open();
                SqlCommand commandSP = new SqlCommand("dbo.DatosGeneralesEvaluacion", conn);
                commandSP.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter(commandSP);
                // var reader = commandSP.ExecuteReader();

                adp.Fill(ds);
                table = ds.Tables[0];
                foreach (DataRow dr in table.Rows)
                {
                    //foreach (DataColumn dc in table.Columns)
                    //{
                    CProcAlmacenadoDatosGeneralesDTO temp = new CProcAlmacenadoDatosGeneralesDTO();
                    temp.Periodos = dr[0].ToString();
                    temp.NombreInstitucion = dr[1].ToString();
                    temp.CantPuestosInstitucionales = dr[2].ToString();
                    temp.Propiedad = dr[3].ToString();
                    temp.Interinos = dr[4].ToString();
                    temp.SinInterinos = dr[5].ToString();
                    temp.CantidadPFRSC = dr[6].ToString();
                    temp.CantidadPuestosFueraRSC = dr[7].ToString();
                    temp.Excluidos = dr[8].ToString();
                    temp.PuestoConfianza = dr[9].ToString();
                    temp.Exceptuados = dr[10].ToString();
                    temp.Oposicion = dr[11].ToString();
                    temp.Otros = dr[12].ToString();
                    temp.FuncionariosDentroRSC = dr[13].ToString();
                    temp.Evaluados = dr[14].ToString();
                    temp.NoEvaluados = dr[15].ToString();

                    //resultado.Add(dr[dc.ColumnName].ToString());
                    //}
                    resultado.Add(temp);
                }
                conn.Close();
                return resultado;
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            return resultado;
        }

        public CRespuestaDTO AgregarPeriodoCalificacion(PeriodoCalificacion periodo)
        {

            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.PeriodoCalificacion.Where(Q => Q.PK_PeriodoCalificacion == periodo.PK_PeriodoCalificacion).FirstOrDefault();
                if(dato == null)
                {
                    entidadBase.PeriodoCalificacion.Add(periodo);
                    entidadBase.SaveChanges();
                }
                else
                {
                    dato.FecRige = periodo.FecRige;
                    dato.FecVence = periodo.FecVence;
                    dato.FecRigeReglaTecnica = periodo.FecRigeReglaTecnica;
                    dato.FecVenceReglaTecnica = periodo.FecVenceReglaTecnica;

                    entidadBase.Entry(dato).State = EntityState.Modified;
                    var resultado = entidadBase.SaveChanges();
                    if (resultado <= 0)
                        throw new Exception("No se pudieron registrar los cambios");
                }
               

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = periodo
                };

                return respuesta;

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerPeriodoCalificacion(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var calificacion = entidadBase.PeriodoCalificacion.Where(C => C.PK_PeriodoCalificacion == codigo).FirstOrDefault();

                if (calificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = calificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró el Periodo para Evaluación." }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO GenerarListadoFuncionariosPeriodo(int codigo)
        {
            CRespuestaDTO respuesta = null;

            try
            {
                var numTotalFuncionarios = entidadBase.USP_GENERAR_LISTA_CALIFICACION_FUNCIONARIOS(codigo).ToList();

                if (numTotalFuncionarios[0] != -1)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = numTotalFuncionarios[0].ToString()
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO
                        {
                            Mensaje = "No se puede generar el listado de Funcionarios, porque ya fue generado previamente"
                        }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.InnerException.Message
                    }
                };
            }
            return respuesta;
        }
        
        public CRespuestaDTO ObtenerDatosGenerales(int periodo)
        {
            CRespuestaDTO respuesta = null;

            try
            {
                var datos = entidadBase.USP_OBTENER_DATOS_CALIFICACION(periodo).ToList();

                if (datos.Count() > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO
                        {
                            Mensaje = "No hay datos"
                        }
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.InnerException.Message
                    }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ObtenerDatosCalificacion(int periodo)
        {
            CRespuestaDTO respuesta = null;
            try
            {
                var datos = entidadBase.USP_OBTENER_RESULTADOS_CALIFICACION(periodo).ToList();

                if (datos.Count() > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("No hay datos");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.InnerException.Message
                    }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO GenerarListaReglaTecnica(int periodo)
        {
            CRespuestaDTO respuesta = null;
            try
            {
                var datoPeriodo = entidadBase.PeriodoCalificacion.Where(Q => Q.PK_PeriodoCalificacion == periodo).ToList();

                if (datoPeriodo != null)
                {
                    var datos = entidadBase.USP_GENERAR_LISTA_CALIFICACION_REGLA_TECNICA(periodo);
                    var datosRegla = entidadBase.CalificacionReglaTecnica.Where(Q => Q.FK_PeriodoCalificacion == periodo).ToList();

                    if (datosRegla.Count() > 0)
                    {
                        //foreach(var item in datosRegla)
                        //{
                        //    // ENVIAR CORREOS
                        //    CEmailHelper salida;
                        //    salida.Asunto = "Evaluación de Desempeño";
                        //    salida.EmailBody = "Estimado (a) se le recuerda que se deben concordar los resultados de la evaluación del desempeño cuando supera más del 20% de funcionarios/as con la categoría excelente, hecho que debe razonarse mediante resolución al efecto ante la Oficina de Gestión Institucional de Recursos Humanos (OGEREH) respectiva, conforme la normativa vigente.";
                        //    salida.EmailBody += "Para más información puede ingresar al Módulo de Evaluación de Desempeño en la ubicación: http://sisrh.mopt.go.cr:84/Calificacion/";
                        //    salida.EmailBody += "<br><br>Por favor no responder a este correo, ya que fue generado automáticamente";
                        //    salida.EmailBody += "<br><br>Atentamente,";
                        //    salida.EmailBody += "<br>Dirección de Gestión Institucional de Recursos Humanos.";
                        //    salida.Destinos = "dguiltrc@mopt.go.cr, dgrangeg@mopt.go.cr, vchavesc@mopt.go.cr";
                        //}
                        
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = datosRegla
                        };
                    }
                    else
                    {
                        throw new Exception("No hay datos");
                    }
                }
                else
                {
                    throw new Exception("No existen datos del periodo " + periodo.ToString());
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Mensaje = error.Message
                    }
                };
            }
            return respuesta;
        }


        public CRespuestaDTO CargarArchivoReglaTecnica(CalificacionReglaTecnica regla)
        {

            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.CalificacionReglaTecnica.Where(Q => Q.PK_CalificacionReglaTecnica == regla.PK_CalificacionReglaTecnica).FirstOrDefault();
                if (dato != null)
                {
                    dato.IndEstado = 1;
                    dato.ImgDocumento = regla.ImgDocumento;

                    entidadBase.Entry(dato).State = EntityState.Modified;
                    var resultado = entidadBase.SaveChanges();
                    if (resultado <= 0)
                        throw new Exception("No se pudieron registrar los cambios");
                }


                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = regla
                };

                return respuesta;

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }


        public CRespuestaDTO AsignarDirectorReglaTecnica(CalificacionReglaTecnica regla)
        {

            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.CalificacionReglaTecnica.Where(Q => Q.PK_CalificacionReglaTecnica == regla.PK_CalificacionReglaTecnica).FirstOrDefault();
                if (dato != null)
                {
                    dato.IndDirector = regla.IndDirector;
                    
                    entidadBase.Entry(dato).State = EntityState.Modified;
                    var resultado = entidadBase.SaveChanges();
                    if (resultado <= 0)
                        throw new Exception("No se pudieron registrar los cambios");
                }
                
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = regla
                };

                return respuesta;

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }


        public CRespuestaDTO ListarReglaTecnica(int periodo)
        {

            CRespuestaDTO respuesta;
            try
            {
                var datosRegla = entidadBase.CalificacionReglaTecnica.Where(Q => Q.FK_PeriodoCalificacion == periodo).ToList();

                if (datosRegla.Count() > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosRegla
                    };
                }
                else
                {
                    throw new Exception("No hay datos");
                }

                return respuesta;

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }


        public CRespuestaDTO ObtenerReglaTecnica(int codRegla)
        {

            CRespuestaDTO respuesta;
            try
            {
                var datosRegla = entidadBase.CalificacionReglaTecnica.Where(Q => Q.PK_CalificacionReglaTecnica == codRegla).ToList();

                if (datosRegla.Count() > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosRegla
                    };
                }
                else
                {
                    throw new Exception("No hay datos");
                }

               return respuesta;

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO ObtenerPeriodosCalificacion()
        {
            try
            {
                var datos = entidadBase.PeriodoCalificacion.ToList();
                if (datos == null || datos.Count() == 0)
                {
                    throw new Exception("No se encontraron periodos de calificación");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = datos
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
        public CRespuestaDTO ObtenerFuncionariosNoEvaluadosPorPeriodo(int periodo)
        {
            try
            {

                var listaEvaluados = (from C in entidadBase.CalificacionNombramiento
                                      join N in entidadBase.Nombramiento on C.FK_Nombramiento equals N.PK_Nombramiento
                                      join F in entidadBase.Funcionario on N.FK_Funcionario equals F.PK_Funcionario
                                      where C.FK_PeriodoCalificacion == periodo && C.IndEstado == 1
                                      select new
                                      {
                                        F.PK_Funcionario
                                      }).ToList();


                var strAnio = periodo.ToString();
                var listaEvaluadosHistorico = (from C in entidadBase.C_EMU_Calificacion
                                               join F in entidadBase.Funcionario on C.Cedula equals F.IdCedulaFuncionario
                                               where C.Periodo == strAnio
                                               select new
                                               {
                                                   F.PK_Funcionario
                                               }).ToList();

                var listaNoEvaluados = entidadBase.CalificacionNombramientoFuncionarios.Where(Q => Q.FK_PeriodoCalificacion == periodo).ToList();
                listaNoEvaluados = listaNoEvaluados.Where(Q => !listaEvaluados.Select(R => R.PK_Funcionario).Contains(Q.FK_Funcionario)).ToList();
                listaNoEvaluados = listaNoEvaluados.Where(Q => !listaEvaluadosHistorico.Select(R => R.PK_Funcionario).Contains(Q.FK_Funcionario)).ToList();

                var noEvaluados = (from FC in listaNoEvaluados
                                   join F in entidadBase.Funcionario on FC.FK_Funcionario equals F.PK_Funcionario
                                   join P in entidadBase.Puesto on FC.FK_Puesto equals P.PK_Puesto
                                   where P.IndNivelOcupacional != 7
                                   //join JI in entidadBase.Funcionario on FC.IdJefeInmediato equals JI.PK_Funcionario
                                   //join JS in entidadBase.Funcionario on FC.IdJefeSuperior equals JS.PK_Funcionario

                                   select new CCalificacionNombramientoFuncionarioDTO
                                   {
                                       IdEntidad = FC.PK_CalificacionNombramiento,
                                       Funcionario = new CFuncionarioDTO
                                       {
                                           Cedula = F.IdCedulaFuncionario,
                                           Nombre = F.NomFuncionario.TrimEnd() + " " + F.NomPrimerApellido.TrimEnd() + " " + F.NomSegundoApellido.TrimEnd(),
                                           PrimerApellido = F.NomPrimerApellido.TrimEnd(),
                                           SegundoApellido = F.NomSegundoApellido.TrimEnd(),
                                           Sexo = (GeneroEnum)Convert.ToInt32(F.IndSexo)
                                         },
                                         Puesto = new CPuestoDTO
                                         {
                                             IdEntidad = P.PK_Puesto,
                                             CodPuesto= P.CodPuesto,
                                             PuestoConfianza = P.IndPuestoConfianza.HasValue ? Convert.ToBoolean(P.IndPuestoConfianza) : false,
                                             NivelOcupacional = P.IndNivelOcupacional.HasValue ? P.IndNivelOcupacional.Value : 0
                                         },
                                         JefeInmediato = new CFuncionarioDTO
                                         {
                                             IdEntidad = Convert.ToInt32(FC.IdJefeInmediato),
                                             Sexo = GeneroEnum.Indefinido
                                         },
                                        JefeSuperior = new CFuncionarioDTO
                                         {
                                             IdEntidad = Convert.ToInt32(FC.IdJefeSuperior),
                                             Sexo = GeneroEnum.Indefinido
                                        },                                         
                                     }).OrderBy(Q =>Q.JefeInmediato.IdEntidad).ToList();

               // var noEvaluados = entidadBase.USP_LISTAR_FUNCIONARIOS_NO_EVALUADOS(periodo).ToList();

                if (noEvaluados == null)
                {
                    throw new Exception("No se han encontrado datos");
                }
                if (noEvaluados.Count() == 0)
                {
                    throw new Exception("No existen datos del periodo " + periodo.ToString());
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = noEvaluados
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
        public CRespuestaDTO ObtenerFuncionariosEvaluadosPorPeriodo(int periodo)
        {
            try
            {
                var listado = entidadBase.CalificacionNombramiento.Include("Calificacion").Where(Q => Q.FK_PeriodoCalificacion == periodo && Q.IndEstado == 1).ToList();

                var calificaciones = (from C in listado
                                      join N in entidadBase.Nombramiento on C.FK_Nombramiento equals N.PK_Nombramiento
                                      join F in entidadBase.Funcionario on N.FK_Funcionario equals F.PK_Funcionario
                                      join P in entidadBase.Puesto on N.FK_Puesto equals P.PK_Puesto
                                      where C.FK_PeriodoCalificacion == periodo && C.IndEstado == 1
                                      select new CCalificacionNombramientoDTO
                                      {
                                          IdEntidad = C.PK_CalificacionNombramiento,
                                          IndFormularioDTO = C.IndFormulario,
                                          FecCreacionDTO = C.FecCreacion,
                                          IndRatificacionDTO = C.IndRatificado,
                                          FecRatificacionDTO = C.FecRatificacion,
                                          IndEntregadoDTO = C.IndEntregado == 1 ? true : false,
                                          IndConformidadDTO = C.IndConformidad == 1 ? true : false,
                                          Funcionario = new CFuncionarioDTO
                                          {
                                              Cedula = F.IdCedulaFuncionario,
                                              Nombre = F.NomFuncionario.TrimEnd() + " " + F.NomPrimerApellido.TrimEnd() + " " + F.NomSegundoApellido.TrimEnd(),
                                              PrimerApellido = F.NomPrimerApellido.TrimEnd(),
                                              SegundoApellido = F.NomSegundoApellido.TrimEnd(),
                                              Sexo = F.IndSexo == "1" ? GeneroEnum.Masculino : GeneroEnum.Femenino
                                          },
                                          CalificacionDTO = new CCalificacionDTO
                                          {
                                              IdEntidad =  C.Calificacion.PK_Calificacion,
                                              DesCalificacion = C.Calificacion.DesCalificacion
                                          },
                                          Puesto = new CPuestoDTO
                                          {
                                              IdEntidad = P.PK_Puesto,
                                              CodPuesto = P.CodPuesto,
                                              PuestoConfianza = P.IndPuestoConfianza.HasValue ? P.IndPuestoConfianza.Value ? P.IndPuestoConfianza.Value : false : false,
                                              NivelOcupacional = P.IndNivelOcupacional.HasValue ? P.IndNivelOcupacional.Value : 0
                                          },
                                          JefeInmediato = new CFuncionarioDTO
                                          {
                                              IdEntidad = C.IdJefeInmediato,
                                              Sexo = GeneroEnum.Indefinido
                                          },
                                          JefeSuperior = new CFuncionarioDTO
                                          {
                                              IdEntidad = C.IdJefeSuperior,
                                              Sexo = GeneroEnum.Indefinido
                                          },
                                      }).OrderBy(Q => Q.JefeInmediato.IdEntidad).ToList();

                //var calificaciones = entidadBase.USP_LISTAR_FUNCIONARIOS_EVALUADOS(periodo).ToList();

                if (calificaciones == null)
                {
                    throw new Exception("No se han encontrado datos");
                }
                if (calificaciones.Count() == 0)
                {
                    throw new Exception("No existen datos del periodo " + periodo.ToString());
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = calificaciones
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

        public CRespuestaDTO AgregarAutoEvaluacion(int periodo, int codFuncionario, decimal nota)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosCalificacion = entidadBase.CalificacionNombramientoFuncionarios.Where(Q => Q.FK_PeriodoCalificacion == periodo && Q.FK_Funcionario == codFuncionario).FirstOrDefault();

                if (datosCalificacion != null)
                {

                    datosCalificacion.PorAutoEvaluacion = nota;
                    entidadBase.SaveChanges();

                    // Cambiar los datos 
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosCalificacion
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron datos del Funcionario para el periodo de Calificación " + periodo.ToString());
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }

        #endregion
    }
}