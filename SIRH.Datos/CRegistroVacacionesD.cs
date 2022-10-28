using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CRegistroVacacionesD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CRegistroVacacionesD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO ConsultaVacacionesHistorial(string cedula)
        {
            try
            {
                //Se eliminó validación para que no devuleva reintegros de vacaciones  && Q.TRANSACCION != "99")
                var datos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula /*&& ((!Q.DOCUMENTO.Contains("1299") || !Q.DOCUMENTO.Contains("0499") || !Q.DOCUMENTO.Contains("0399")))*/).OrderBy(Q => Q.FECHA_RIGE).ToList();
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
                    throw new Exception("No se encontraron datos de vacaciones para la cédula indicada");
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

        public CRespuestaDTO ConsultaVacacionesHistorialModulo(string cedula)
        {
            try
            {
                //Se eliminó validación para que no devuleva reintegros de vacaciones  && Q.TRANSACCION != "99")
                var datos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula && Q.TRANSACCION != "99" /*&& ((!Q.DOCUMENTO.Contains("1299") || !Q.DOCUMENTO.Contains("0499") || !Q.DOCUMENTO.Contains("0399")))*/).OrderBy(Q => Q.FECHA_RIGE).ToList();
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
                    throw new Exception("No se encontraron datos de vacaciones para la cédula indicada");
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

        public CRespuestaDTO ConsultaVacaciones(string cedula)
        {
            try
            {
                //Se modificó para que traiga los registros de vacaciones anulados y pueda mostrar los reintegros && Q.IndEstado == 1
                var datos = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Include("ReintegroVacaciones").Where(Q => Q.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula
                                                                      /*&& Q.IndEstado == 1*/).OrderBy(Q => Q.FecInicio).ToList();
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
                    throw new Exception("No se encontraron datos de vacaciones para la cédula indicada");
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

        public CRespuestaDTO GuardarRegistroVacaciones(RegistroVacaciones datos, string cedula, string periodoParam, double cantDias)
        {
            try
            {

                //Se le agregó que se descargue al primero que encuentre activo para que no descuente días de los periodos anulados
                var periodo = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                              .Include("RegistroVacaciones").Include("RegistroVacaciones.ReintegroVacaciones")
                              .Where(P => P.Nombramiento.PeriodoVacaciones.Count() > 0 &&
                              P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                              P.IndPeriodo == periodoParam && P.IndSaldo >= cantDias && P.IndEstado == 1).FirstOrDefault();
                if (periodo != null)
                {
                    periodo.IndSaldo = periodo.IndSaldo - cantDias;
                    periodo.RegistroVacaciones.Add(datos);
                    var resultado = entidadBase.SaveChanges();
                    if (resultado > 0)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                        };
                    }
                    else
                    {
                        throw new Exception("No se pudo guardar el registro indicado debido a un error interno del servicio");
                    }
                }
                else
                {
                    throw new Exception("No se encontraron saldos de vacaciones para la cédula y periodo indicados");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO GuardarRegistroVacacionesModulo(RegistroVacaciones datos, string cedula, string periodoVacaciones)
        {
            try
            {
                var dias = Convert.ToDouble(datos.CntDias);
                var periodo = entidadBase.PeriodoVacaciones.Where(P =>
                 P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && P.IndPeriodo == periodoVacaciones && P.IndEstado == 1).FirstOrDefault();
                var periodos = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                .Where(P => P.Nombramiento.PeriodoVacaciones.Count() > 0 &&
                 P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula &&
                 P.IndEstado == 1).OrderBy(P => P.PK_PeriodoVacaciones).ToList();
                var diasRebajados = 0.0;
                var resultado = 0;
                if (periodo != null)
                {
                    if (periodo.IndSaldo >= dias)
                    {
                        periodo.IndSaldo = periodo.IndSaldo - dias;
                        periodo.RegistroVacaciones.Add(datos);
                        diasRebajados = dias;
                        if (periodo.IndSaldo == 0)
                        {
                            periodo.IndEstado = 2;
                        }
                    }
                    else
                    {
                        foreach (var item in periodos)
                        {
                            if (diasRebajados != dias)
                            {
                                if (item.IndSaldo >= (dias - diasRebajados))
                                {
                                    item.IndSaldo -= (dias - diasRebajados);
                                    diasRebajados += (dias - diasRebajados);

                                }
                                else
                                {
                                    diasRebajados += Convert.ToDouble(item.IndSaldo);
                                    item.IndSaldo = 0;

                                }
                                if (item.IndSaldo == 0)
                                {
                                    periodo.IndEstado = 2;
                                }
                            }
                        }

                    }
                    if (diasRebajados == dias)
                    {
                        resultado = entidadBase.SaveChanges();
                    }
                    else
                    {
                        if (datos.IndTipoTransaccion == 5)
                        {
                            periodo.IndSaldo = periodo.IndSaldo - dias;
                            resultado = entidadBase.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encontraron saldos suficientes de vacaciones para la cédula indicada.");
                        }
                    }
                    if (resultado > 0)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = periodo.PK_PeriodoVacaciones
                        };
                    }
                    else
                    {
                        throw new Exception("No se pudo guardar el registro indicado debido a un error interno del servicio.");
                    }
                }
                else
                {
                    throw new Exception("No se encontraron saldos suficientes de vacaciones para la cédula.");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO ConsultaVacacionesPeriodo(string cedula, string periodo, int codigo)
        {
            try
            {
                var datoPeriodo = entidadBase.PeriodoVacaciones.FirstOrDefault(P => P.IndPeriodo == periodo && P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula && P.PK_PeriodoVacaciones == codigo);
                var datos = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones")
                                                          .Include("PeriodoVacaciones.Nombramiento")
                                                          .Include("PeriodoVacaciones.Nombramiento.Funcionario")
                                                          .Where(R => R.PeriodoVacaciones.PK_PeriodoVacaciones == datoPeriodo.PK_PeriodoVacaciones).ToList();
                //var datos = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Where(Q => Q.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula
                //                                                      && Q.FK_PeriodoVacaciones == periodo).OrderBy(Q => Q.FecInicio).ToList();
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
                    throw new Exception("No se encontraron datos de vacaciones para la cédula indicada.");
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

        public CRespuestaDTO TrasladarRegistroVacaciones(int idRegistro, decimal dias, int periodoDestino)
        {
            try
            {
                var registro = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Include("ReintegroVacaciones").FirstOrDefault(R => R.PK_RegistroVacaciones == idRegistro);
                var registroPeriodo = entidadBase.PeriodoVacaciones.FirstOrDefault(P => P.PK_PeriodoVacaciones == periodoDestino);
                var repetido = registroPeriodo.RegistroVacaciones.FirstOrDefault(R => R.NumTransaccion == registro.NumTransaccion);

                if (registro != null)
                {
                    var periodoOriginal = registro.PeriodoVacaciones;
                    decimal diasOriginales = Convert.ToDecimal(registro.CntDias);
                    DateTime fecFinOriginal = Convert.ToDateTime(registro.FecFin);
                    if (registro.CntDias > dias)
                    {
                        periodoOriginal.IndSaldo += Convert.ToDouble(dias);
                        registro.CntDias -= Convert.ToDecimal(dias);
                        registro.FecFin = Convert.ToDateTime(registro.FecFin).AddDays(Convert.ToDouble(-Math.Floor(dias)));
                        
                        if (repetido == null)
                        {
                            RegistroVacaciones nuevoRegistro = new RegistroVacaciones
                            {
                                CntDias = dias,
                                FecActualizacion = DateTime.Now,
                                FecInicio = Convert.ToDateTime(registro.FecFin).AddDays(1),
                                FecFin = fecFinOriginal,
                                IndEstado = registro.IndEstado,
                                IndTipoTransaccion = registro.IndTipoTransaccion,
                                NumTransaccion = registro.NumTransaccion
                            };
                            registroPeriodo.IndSaldo -= Convert.ToDouble(dias);
                            registroPeriodo.RegistroVacaciones.Add(nuevoRegistro);
                            entidadBase.SaveChanges();
                        }
                        else
                        {
                            if (repetido.FecInicio >= registro.FecInicio)
                            {
                                repetido.FecInicio = registro.FecInicio;
                            }
                            if (repetido.FecFin <= registro.FecFin)
                            {
                                repetido.FecFin = registro.FecFin;
                            }
                            repetido.CntDias += dias;
                            registroPeriodo.IndSaldo -= Convert.ToDouble(dias);
                            if (registro.CntDias == 0)
                            {
                                entidadBase.RegistroVacaciones.Remove(registro);
                            }
                            entidadBase.SaveChanges();
                        }
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = periodoOriginal
                        };
                    }
                    else
                    {
                        if (registro.CntDias == dias)
                        {
                            if (repetido == null)
                            {
                                registro.FK_PeriodoVacaciones = periodoDestino;
                                entidadBase.PeriodoVacaciones.FirstOrDefault(P => P.PK_PeriodoVacaciones == periodoDestino).IndSaldo -= Convert.ToDouble(dias);
                                periodoOriginal.IndSaldo += Convert.ToDouble(dias);
                                entidadBase.SaveChanges();
                            }
                            else
                            {
                                if (repetido.FecInicio >= registro.FecInicio)
                                {
                                    repetido.FecInicio = registro.FecInicio;
                                }
                                if (repetido.FecFin <= registro.FecFin)
                                {
                                    repetido.FecFin = registro.FecFin;
                                }
                                repetido.CntDias += dias;
                                entidadBase.PeriodoVacaciones.FirstOrDefault(P => P.PK_PeriodoVacaciones == periodoDestino).IndSaldo -= Convert.ToDouble(dias);
                                periodoOriginal.IndSaldo += Convert.ToDouble(dias);
                                entidadBase.RegistroVacaciones.Remove(registro);
                                entidadBase.SaveChanges();
                            }
                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = periodoOriginal
                            };
                        }
                        else
                        {
                            throw new Exception("No hay suficientes días en este periodo para generar el traslado del registro");
                        }
                    }
                }
                else
                {
                    throw new Exception("No se encontró el registro de vacaciones que se intenta trasladar");
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

        public CRespuestaDTO ConsultaVacacionesPorDocumento(string cedula, string numeroDocumento)
        {
            try
            {
                RegistroVacaciones registro = null;
                var datos = entidadBase.RegistroVacaciones
                                       .Include("PeriodoVacaciones")
                                       .Where(R => R.NumTransaccion == numeroDocumento
                                       && R.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula)
                                       .OrderByDescending(O => O.FK_PeriodoVacaciones).ToList();
                if (datos.Where(R => R.ReintegroVacaciones.Count > 0).Count() > 0)
                {
                    datos = datos.Where(R => R.ReintegroVacaciones.Count < 1 || R.ReintegroVacaciones.Sum(C => C.CntDias) < R.CntDias).ToList();
                    if (datos.Count > 0)
                    {
                        registro = datos.ElementAt(0);
                    }
                }
                else
                {
                    registro = datos.ElementAt(0);
                }
                //var datos = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Where(Q => Q.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula
                //                                                      && Q.FK_PeriodoVacaciones == periodo).OrderBy(Q => Q.FecInicio).ToList();
                if (registro != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    var datoHistorico = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains(numeroDocumento)
                                                                                      && Q.CEDULA == cedula)
                                                                                      .OrderByDescending(O => O.PERI_TRAN).FirstOrDefault();

                    if (datoHistorico != null)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = 2,
                            Contenido = datoHistorico
                        };
                    }
                    else
                    {
                        throw new Exception("No se encontró el registro de vacaciones con el documento indicado.");
                    }
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

        public CRespuestaDTO Filtrar(object[] parametros, string estadoSeleccion)
        {
            var datos = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Include("PeriodoVacaciones.Nombramiento.Funcionario").AsQueryable();
            if (estadoSeleccion == "Rebajo colectivo")
            {
                datos = datos.Where(D => D.IndTipoTransaccion == 88);
            }
            string elemento;
            try
            {
                if (datos.Count() != 0)
                {
                    for (int i = 0; i < parametros.Length; i = i + 2)
                    {
                        elemento = parametros[i].ToString();
                        switch (elemento)
                        {
                            case "PeriodoVacaciones":
                                var periodo = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.PeriodoVacaciones.IndPeriodo == periodo);
                                break;
                            case "Cedula":
                                var cedula = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula);
                                break;
                            case "NumeroTransaccion":
                                var numeroTransaccion = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.NumTransaccion == numeroTransaccion);
                                break;
                            case "Direccion":
                                var direccion = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion == direccion);
                                break;
                            case "Departamento":
                                var departamento = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento == departamento);
                                break;
                            case "Division":
                                var division = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.Division.NomDivision == division);
                                break;
                            case "Seccion":
                                var seccion = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.NomSeccion == seccion);
                                break;
                            case "Fecha":
                                var fechaInicio = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                                var fechaFin = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                                datos = datos.Where(D => D.FecInicio >= fechaInicio && D.FecInicio <= fechaFin);
                                break;
                            case "TipoVacaciones":
                                var tipoVacaciones = parametros[i + 1].ToString();
                                var numTipoVacaciones = 5;
                                if (tipoVacaciones == "Vacaciones")
                                {
                                    numTipoVacaciones = 1;
                                }
                                else
                                    if (tipoVacaciones == "Permiso sin goce de salario")
                                {
                                    numTipoVacaciones = 6;
                                }
                                datos = datos.Where(D => D.IndTipoTransaccion == numTipoVacaciones);
                                break;


                            default: throw new Exception("Busqueda no definida");
                        }
                    }
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = datos.ToList()
                };
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO GuardarRebajoColectivo(CFuncionarioDTO funcionario, RegistroVacaciones rebajoColectivo)
        {
            try
            {
                var periodoNuevo = Convert.ToString(rebajoColectivo.FecInicio?.Year) + Convert.ToString(rebajoColectivo.FecInicio?.Year + 1);
                var dias = Convert.ToDouble(rebajoColectivo.CntDias);
                var ultimafecha = rebajoColectivo.FecFin;
                var diasRebajados = 0.0;
                var periodo = entidadBase.PeriodoVacaciones.Where(P => P.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.Cedula && P.IndSaldo > 0 && P.IndEstado == 1).OrderBy(P => P.PK_PeriodoVacaciones).ToList();
                var nombramiento = entidadBase.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == funcionario.Cedula && (N.FecVence == null || N.FecVence >= DateTime.Now) && N.FK_EstadoNombramiento != 15).OrderByDescending(P => P.FecRige).FirstOrDefault();
                //var nombramiento = entidadBase.Funcionario.Include("Nombramiento").Where(q => q.IdCedulaFuncionario == funcionario.Cedula).FirstOrDefault().Nombramiento.LastOrDefault();
                if (nombramiento != null)
                {
                    var fechainicioActual = rebajoColectivo.FecInicio;
                    var fechafinActual = rebajoColectivo.FecFin;
                    if (periodo.Count() != 0)
                    {
                        foreach (var periodoV in periodo)
                        {
                            if (diasRebajados != dias)
                            {
                                //Rebajo temporal para bajar los días correctos
                                var tempRebajo = rebajoColectivo;
                                if (dias <= Convert.ToDouble(periodoV.IndSaldo))
                                {
                                    double cantRebajo = Convert.ToDouble(dias - diasRebajados);
                                    diasRebajados += cantRebajo;
                                    periodoV.IndSaldo -= cantRebajo;
                                    tempRebajo.CntDias = Convert.ToDecimal(cantRebajo);
                                    tempRebajo.FecInicio = fechainicioActual;
                                    tempRebajo.FecFin = fechafinActual;
                                    //periodoV.IndSaldo -= dias;
                                }
                                else
                                {
                                    bool primera = diasRebajados == 0 ? true : false;
                                    //rebajoColectivo.CntDias = Convert.ToDecimal(periodoV.IndSaldo);
                                    tempRebajo.CntDias = Convert.ToDecimal(periodoV.IndSaldo);
                                    diasRebajados += Convert.ToDouble(periodoV.IndSaldo);
                                    if (primera)
                                    {
                                        tempRebajo.FecFin = AddBusinessDays(Convert.ToDateTime(fechainicioActual).AddDays(-1), Convert.ToInt32(Math.Ceiling(diasRebajados)));
                                        if (diasRebajados % 1 == 0)
                                        {
                                            //fechainicioActual = tempRebajo.FecFin;
                                            fechainicioActual = AddBusinessDays(Convert.ToDateTime(tempRebajo.FecFin),1);
                                        }
                                        else
                                        {
                                            fechainicioActual = tempRebajo.FecFin;
                                        }
                                    }
                                    else
                                    {
                                        tempRebajo.FecInicio = fechainicioActual;
                                        if (tempRebajo.CntDias % 1 == 0)
                                        {
                                            tempRebajo.FecFin = AddBusinessDays(Convert.ToDateTime(fechainicioActual), Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(tempRebajo.CntDias))));
                                        }
                                        else
                                        {
                                            tempRebajo.FecFin = AddBusinessDays(Convert.ToDateTime(fechainicioActual).AddDays(-1), Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(tempRebajo.CntDias))));
                                        }
                                        fechainicioActual = tempRebajo.FecFin;
                                    }
                                    periodoV.IndSaldo = 0;
                                    //periodoV.IndEstado = 2;
                                }
                                //rebajoColectivo.PeriodoVacaciones = periodoV;
                                //rebajoColectivo.FK_PeriodoVacaciones = periodoV.PK_PeriodoVacaciones;
                                tempRebajo.PeriodoVacaciones = periodoV;
                                //tempRebajo.FK_PeriodoVacaciones = periodoV.PK_PeriodoVacaciones;
                                entidadBase.RegistroVacaciones.Add(new RegistroVacaciones
                                {
                                    CntDias = tempRebajo.CntDias,
                                    FecActualizacion = DateTime.Now,
                                    FecFin = tempRebajo.FecFin,
                                    FecInicio = tempRebajo.FecInicio,
                                    FK_PeriodoVacaciones = periodoV.PK_PeriodoVacaciones,
                                    IndEstado = tempRebajo.IndEstado,
                                    IndTipoTransaccion = tempRebajo.IndTipoTransaccion,
                                    NumTransaccion = tempRebajo.NumTransaccion
                                });
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (periodo.Count == 0 || diasRebajados != dias)
                    {
                        entidadBase.PeriodoVacaciones.Add(new PeriodoVacaciones
                        {
                            FecCarga = DateTime.Now,
                            IndEstado = 1,
                            Nombramiento = nombramiento,
                            IndPeriodo = periodoNuevo,
                            FK_Nombramiento = nombramiento.PK_Nombramiento,
                            IndSaldo = Convert.ToDouble(-(dias - diasRebajados))
                        }
                        );
                        entidadBase.SaveChanges();
                        var nuevoPeriodo = entidadBase.PeriodoVacaciones.Where(P => P.FK_Nombramiento == nombramiento.PK_Nombramiento && P.IndPeriodo == periodoNuevo).FirstOrDefault();
                        //rebajoColectivo.PeriodoVacaciones = nuevoPeriodo;
                        var tempRebajo = rebajoColectivo;
                        tempRebajo.FK_PeriodoVacaciones = nuevoPeriodo.PK_PeriodoVacaciones;
                        tempRebajo.CntDias = Convert.ToDecimal(dias - diasRebajados);
                        entidadBase.RegistroVacaciones.Add(new RegistroVacaciones
                        {
                            CntDias = tempRebajo.CntDias,
                            FecActualizacion = DateTime.Now,
                            FecFin = ultimafecha,
                            FecInicio = fechainicioActual,
                            FK_PeriodoVacaciones = tempRebajo.FK_PeriodoVacaciones,
                            IndEstado = tempRebajo.IndEstado,
                            IndTipoTransaccion = tempRebajo.IndTipoTransaccion,
                            NumTransaccion = tempRebajo.NumTransaccion
                        });
                    }
                    var resultado = entidadBase.SaveChanges();
                    if (resultado > 0)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = 1
                        };
                    }
                    else
                    {
                        throw new Exception("No se pudo realizar el rebajo colectivo.");
                    }

                }
                else
                {
                    throw new Exception("No se pudo realizar el rebajo colectivo.");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO AnularRebajoColectivo(CFuncionarioDTO funcionario, string numTransaccion)
        {
            try
            {
                var registroVacaciones = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Where(V => V.IndTipoTransaccion == 88 && V.PeriodoVacaciones.Nombramiento.Funcionario
                 .IdCedulaFuncionario == funcionario.Cedula && V.NumTransaccion == numTransaccion).ToList();

                foreach (var rebajo in registroVacaciones)
                {
                    if (rebajo.PeriodoVacaciones != null)
                    {
                        if (rebajo.PeriodoVacaciones.IndSaldo < 0)
                        {
                            entidadBase.PeriodoVacaciones.Remove(rebajo.PeriodoVacaciones);
                        }
                        else
                        {
                            rebajo.PeriodoVacaciones.IndEstado = 1;
                            rebajo.PeriodoVacaciones.IndSaldo = Convert.ToDouble(rebajo.CntDias);
                        }
                        entidadBase.RegistroVacaciones.Remove(rebajo);
                    }

                }
                var resultado = entidadBase.SaveChanges();
                if (resultado > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                    };
                }
                else
                {
                    throw new Exception("Ocurrio un error al anular el rebajo colectivo.");
                }

            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO ValidarNumeroDocumento(string numTransaccion)
        {
            try
            {
                var numeroDocumento = entidadBase.RegistroVacaciones.Where(V => V.IndTipoTransaccion == 88 && V.NumTransaccion == numTransaccion).ToList();

                if (numeroDocumento.Count() != 0)
                {
                    throw new Exception("Ya existe el numero de documento.");
                }
                else
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                    };
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO ConsultaInconsistencias(DateTime fechaInicio, DateTime fechaFin, string cedula)
        {
            try
            {

                var datos = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones").Where(V => V.FecInicio >= fechaInicio && V.FecInicio < fechaFin && V.FecFin <= fechaFin && V.FecFin > fechaInicio && V.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula
                ).ToList();

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
                    throw new Exception("No se encontraron inconsistencias.");
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

        public static DateTime AddBusinessDays(DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);

        }

        #endregion
    }
}
