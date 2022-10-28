using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPeriodoVacacionesD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPeriodoVacacionesD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        public CRespuestaDTO RegistrarPeriodoVacaciones(string cedFuncionario, PeriodoVacaciones periodo)
        {
            try
            {
                var nombramiento = entidadBase.Nombramiento.Include("Funcionario").Include("PeriodoVacaciones")
                                              .Where(N => N.Funcionario.IdCedulaFuncionario == cedFuncionario
                                                                   && (N.FecVence >= DateTime.Now || N.FecVence == null))
                                                                   .OrderByDescending(O => O.FecRige).FirstOrDefault();

                if (nombramiento != null)
                {
                    var idPeriodo = nombramiento.PeriodoVacaciones.Where(Q => Q.IndPeriodo == periodo.IndPeriodo && Q.IndEstado == 1).FirstOrDefault();

                    if (idPeriodo == null)
                    {
                        periodo.IndEstado = 1;
                        periodo.Nombramiento = nombramiento;
                        entidadBase.PeriodoVacaciones.Add(periodo);
                        entidadBase.SaveChanges();

                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = periodo,
                            Mensaje = "Se insertó un nuevo periodo de vacaciones para el funcionario"
                        };
                    }
                    else
                    {
                        idPeriodo.IndSaldo = periodo.IndSaldo;
                        entidadBase.SaveChanges();

                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = periodo,
                            Mensaje = "Se actualizó el periodo de vacaciones para el funcionario"
                        };
                    }
                }
                else
                {
                    throw new Exception("No se encontró el funcionario indicado o bien no se encuentra registrado como activo en planilla.");
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

        public CRespuestaDTO AnularPeriodoVacaciones(int codigoPeriodo)
        {
            try
            {
                var periodo = entidadBase.PeriodoVacaciones.Include("Nombramiento")
                                              .Include("Nombramiento.Funcionario").Include("PeriodoVacaciones")
                                              .FirstOrDefault(P => P.PK_PeriodoVacaciones == codigoPeriodo);
                if (periodo != null)
                {
                    periodo.IndEstado = 4;
                    entidadBase.SaveChanges();

                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodo
                    };
                }
                else
                {
                    throw new Exception("No se encontró el periodo indicado para anular.");
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

        public CRespuestaDTO BuscarDetallePeriodoEspecifico(string periodo, string cedula)
        {
            try
            {
                var nombramiento = entidadBase.Nombramiento.Include("Funcionario").Include("PeriodoVacaciones")
                                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                                              .FirstOrDefault(N => N.FecVence == null &&
                                              N.PeriodoVacaciones.FirstOrDefault(P => P.IndPeriodo == periodo) != null &&
                                              N.Funcionario.IdCedulaFuncionario == cedula);

                if (nombramiento != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = nombramiento
                    };
                }
                else
                {
                    throw new Exception("No se encontraron datos para la búsqueda especificada.");
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

        public CRespuestaDTO DesplegarDetallePeriodoEspecifico(int periodo)
        {
            try
            {
                var periodoRetorno = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                                              .FirstOrDefault(P => P.PK_PeriodoVacaciones == periodo);

                if (periodoRetorno != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodoRetorno
                    };
                }
                else
                {
                    throw new Exception("No se encontraró el periodo especificado.");
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

        public CRespuestaDTO ListarPeriodosActivos(string cedula)
        {
            try
            {
                var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                              .Where(P => P.Nombramiento.PeriodoVacaciones.Count() > 0 &&
                              P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                              (P.IndEstado == 1 || P.IndEstado == 2)).ToList();
                if (periodos.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron periodos activos asociados al funcionario indicado.");
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

        //public CRespuestaDTO HistorialPeriodos(string cedula)
        //{
        //    try
        //    {
        //        var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
        //                      .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
        //                      .Where(P => P.Nombramiento.FecVence == null &&
        //                      P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
        //                      P.IndEstado != 4).ToList();
        //        if (periodos.Count > 0)
        //        {
        //            return new CRespuestaDTO
        //            {
        //                Codigo = 1,
        //                Contenido = periodos
        //            };
        //        }
        //        else
        //        {
        //            throw new Exception("No se encontraron periodos asociados al funcionario indicado.");
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        return new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { MensajeError = error.Message }
        //        };
        //    }
        //}

        public CRespuestaDTO ConsultaSaldoHistoricoVacaciones(string cedula)
        {
            try
            {
                var datos = entidadBase.C_EMU_Vacaciones_Saldo.Where(Q => Q.CEDULA == cedula).ToList();

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
                    throw new Exception("No se encontraron periodos asociados al funcionario indicado.");
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

        public CRespuestaDTO ConsultaEstadoVacaciones(List<PeriodoVacaciones> datosPrevios,
                                                        string parametro, string contenido)
        {
            try
            {
                var datos = FiltrarEstadoVacaciones(parametro, contenido, datosPrevios);
                if (datos.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron resultados para los parámetros establecidos.");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO { Codigo = -1, Contenido = new CErrorDTO { MensajeError = error.Message } };
            }
        }

        private List<PeriodoVacaciones> FiltrarEstadoVacaciones(string parametro, string contenido, List<PeriodoVacaciones> datosPrevios)
        {
            int contNumero = 0;
            int numEstado = 0;
            if (parametro == "seccion")
            {
                contNumero = Convert.ToInt32(contenido);
            }

            if (parametro == "estado")
            {
                numEstado = Convert.ToInt32(contenido);
            }

            if (datosPrevios.Count > 0)
            {
                switch (parametro)
                {
                    case "cedula":
                        return datosPrevios
                               .Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario == contenido
                                      && Q.IndSaldo > 0).ToList();
                    case "nombre":
                        return datosPrevios
                               .Where(Q => Q.Nombramiento.Funcionario.NomFuncionario.Contains(contenido)
                                      && Q.IndSaldo > 0).ToList();
                    case "apellido1":
                        return datosPrevios
                               .Where(Q => Q.Nombramiento.Funcionario.NomPrimerApellido.Contains(contenido)
                                      && Q.IndSaldo > 0).ToList();
                    case "apellido2":
                        return datosPrevios
                               .Where(Q => Q.Nombramiento.Funcionario.NomSegundoApellido.Contains(contenido)
                                      && Q.IndSaldo > 0).ToList();
                    case "seccion":
                        return datosPrevios
                               .Where(Q => Q.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion == contNumero
                                      && Q.IndSaldo > 0).ToList();
                    case "estado":
                        if (numEstado == 1)
                        {
                            var resultado = datosPrevios
                               .Where(Q => Q.IndSaldo > 0).ToList();
                            if (resultado.Count == 2)
                            {
                                return resultado;
                            }
                            else
                            {
                                return datosPrevios;
                            }
                        }
                        else
                        {
                            if (numEstado == 2)
                            {
                                var resultado = datosPrevios
                                .Where(Q => Q.IndSaldo > 0).ToList();
                                if (resultado.Count > 2)
                                {
                                    return resultado;
                                }
                                else
                                {
                                    return datosPrevios;
                                }
                            }
                            else
                            {
                                if (numEstado < 0)
                                {
                                    return datosPrevios
                                       .Where(Q => Q.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion == contNumero
                                              && Q.IndSaldo < 0).ToList();
                                }
                                else
                                {
                                    return datosPrevios;
                                }
                            }
                        }

                    default:
                        return datosPrevios;
                }
            }
            else
            {
                switch (parametro)
                {
                    case "cedula":
                        return entidadBase.PeriodoVacaciones
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Include("Nombramiento.Puesto")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                               .Where(Q => Q.Nombramiento.Funcionario.IdCedulaFuncionario == contenido
                                      && Q.IndSaldo > 0).ToList();
                    case "nombre":
                        return entidadBase.PeriodoVacaciones
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Include("Nombramiento.Puesto")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                               .Where(Q => Q.Nombramiento.Funcionario.NomFuncionario.Contains(contenido)
                                      && Q.IndSaldo > 0).ToList();
                    case "apellido1":
                        return entidadBase.PeriodoVacaciones
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Include("Nombramiento.Puesto")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                               .Where(Q => Q.Nombramiento.Funcionario.NomPrimerApellido.Contains(contenido)
                                      && Q.IndSaldo > 0).ToList();
                    case "apellido2":
                        return entidadBase.PeriodoVacaciones
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Include("Nombramiento.Puesto")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                               .Where(Q => Q.Nombramiento.Funcionario.NomSegundoApellido.Contains(contenido)
                                      && Q.IndSaldo > 0).ToList();
                    case "seccion":
                        return entidadBase.PeriodoVacaciones
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Include("Nombramiento.Puesto")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                               .Where(Q => Q.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion == contNumero
                                      && Q.IndSaldo > 0).ToList();
                    case "estado":
                        if (numEstado == 1)
                        {
                            var resultado = entidadBase.PeriodoVacaciones
                               .Include("Nombramiento")
                               .Include("Nombramiento.Funcionario")
                               .Include("Nombramiento.Puesto")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                               .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                               .Where(Q => Q.IndSaldo > 0).ToList();
                            if (resultado.Count == 2)
                            {
                                return resultado;
                            }
                            else
                            {
                                return datosPrevios;
                            }
                        }
                        else
                        {
                            if (numEstado == 2)
                            {
                                var resultado = entidadBase.PeriodoVacaciones
                                .Include("Nombramiento")
                                .Include("Nombramiento.Funcionario")
                                .Include("Nombramiento.Puesto")
                                .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                .Where(Q => Q.IndSaldo > 0).ToList();
                                if (resultado.Count > 2)
                                {
                                    return resultado;
                                }
                                else
                                {
                                    return datosPrevios;
                                }
                            }
                            else
                            {
                                if (numEstado < 0)
                                {
                                    return entidadBase.PeriodoVacaciones
                                       .Include("Nombramiento")
                                       .Include("Nombramiento.Funcionario")
                                       .Include("Nombramiento.Puesto")
                                       .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                       .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                       .Where(Q => Q.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion == contNumero
                                              && Q.IndSaldo < 0).ToList();
                                }
                                else
                                {
                                    return datosPrevios;
                                }
                            }
                        }

                    default:
                        return datosPrevios;
                }
            }
        }

        public CRespuestaDTO ObtenerPeriodo(string cedFuncionario, string periodo)
        {
            try
            {
                var periodoVacaciones = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Puesto").Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.RelPuestoUbicacion")
                    .Where(P => P.Nombramiento.Funcionario.IdCedulaFuncionario == cedFuncionario && P.IndPeriodo == periodo).FirstOrDefault();

                if (periodoVacaciones != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodoVacaciones
                    };
                }
                else
                {
                    return new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun periodo." }
                    };
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

        public CRespuestaDTO ObtenerPeriodoCodigo(string cedFuncionario, string periodo, int codigo)
        {
            try
            {
                var periodoVacaciones = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Puesto").Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.RelPuestoUbicacion")
                    .Where(P => P.Nombramiento.Funcionario.IdCedulaFuncionario == cedFuncionario && P.IndPeriodo == periodo && P.PK_PeriodoVacaciones == codigo).FirstOrDefault();

                if (periodoVacaciones != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodoVacaciones
                    };
                }
                else
                {
                    return new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun periodo." }
                    };
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


        public CRespuestaDTO ListarPeriodosNoActivos(string cedula)
        {
            try
            {
                var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                              .Where(P => P.Nombramiento.PeriodoVacaciones.Count() > 0 &&
                              P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                              P.IndEstado == 3).ToList();
                if (periodos.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron periodos desactivos asociados al funcionario indicado.");
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

        public CRespuestaDTO ListaPeriodos(string cedula)
        {
            try
            {
                var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                              .Where(P => P.Nombramiento.PeriodoVacaciones.Count() > 0 &&
                              P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).ToList();
                if (periodos.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron periodos desactivos asociados al funcionario indicado.");
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

        public CRespuestaDTO HistorialPeriodos(string cedula)
        {
            try
            {
                var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                              .Where(P =>
                              P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula
                              ).ToList();
                //var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                //.Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                //.Where(P => P.Nombramiento.FecVence == null &&
                //P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                //P.IndEstado != 4).ToList();
                if (periodos.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron periodos asociados al funcionario indicado.");
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

        public CRespuestaDTO TrasladarPeriodosVacacionesNombramiento(string cedula, int idNombramientoActual)
        {
            try
            {
                var periodos = entidadBase.PeriodoVacaciones.Where(P => P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && P.IndEstado == 1).ToList();
                if (periodos != null)
                {
                    var nombramientoActual = entidadBase.Nombramiento.Where(N => N.PK_Nombramiento == idNombramientoActual).FirstOrDefault();
                    foreach (var item in periodos)
                    {
                        //nombramientoActual.PeriodoVacaciones.Add(item);
                        item.Nombramiento = nombramientoActual;
                    }
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = nombramientoActual.PK_Nombramiento
                    };
                }
                else
                {
                    throw new Exception("El funcionario no tiene periodos asociados");
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

        public CRespuestaDTO ActualizarDatosVacaciones(int idregistro, string cambio, decimal saldo)
        {
            try
            {
                if (cambio != "")
                {
                    var periodo = entidadBase.PeriodoVacaciones.FirstOrDefault(P => P.PK_PeriodoVacaciones == idregistro);
                    if (periodo != null)
                    {
                        if (cambio == "saldo")
                        {
                            periodo.IndSaldo = Convert.ToDouble(saldo);
                        }
                        else
                        {
                            if (cambio == "derecho")
                            {
                                periodo.IndDiasDerecho = Convert.ToDecimal(saldo);
                            }
                            else
                            {
                                if (cambio == "anular")
                                {
                                    periodo.IndEstado = 3;
                                }
                                else
                                {
                                    throw new Exception("Proceso no identificado.");
                                }
                            }
                        }
                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = periodo
                        };
                    }
                    else
                    {
                        throw new Exception("No se encontró el periodo indicado");
                    }
                }
                else
                {
                    throw new Exception("No se pudo completar la solicitud");
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

        public CRespuestaDTO BuscarHistorialMovimientoVacaciones(string cedula, string periodo)
        {
            try
            {
                var vacaciones = entidadBase.C_EMU_Vacaciones_Movimiento.Where(P => P.CEDULA == cedula && P.PERI_TRAN == periodo).ToList();

                if (vacaciones.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = vacaciones
                    };
                }
                else
                {
                    throw new Exception("No se encontraron Registros de Vacaciones asociados a la cédula indicada.");
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

        public CRespuestaDTO BuscarHistorialPeriodoVacacionesSinActuales(string cedula, List<string> actuales)
        {
            try
            {
                var vacaciones = entidadBase.C_EMU_Vacaciones_Movimiento.Where(P => P.CEDULA == cedula && !actuales.Contains(P.PERI_TRAN)).ToList();

                if (vacaciones.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = vacaciones
                    };
                }
                else
                {
                    throw new Exception("No se encontraron Registros de Vacaciones asociados a la cédula indicada.");
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

        #endregion
    }
}
