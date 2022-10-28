using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CReintegroVacacionesD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor
    
        public CReintegroVacacionesD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO RegistrarReintegroVacaciones(string cedula, string periodo, string documento, ReintegroVacaciones reintegro, decimal cntDias)
        {
            try
            {
                //var nombramiento = entidadBase.Nombramiento.Include("Funcionario").Include("PeriodoVacaciones")
                //              .Where(N => N.Funcionario.IdCedulaFuncionario == cedula && 
                //              N.PeriodoVacaciones.Where(P => P.RegistroVacaciones.Where(
                //                  R => R.NumTransaccion == documento).Count() > 0).Count() > 0).FirstOrDefault();

                var periodoBusca = entidadBase.PeriodoVacaciones.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                                .Include("RegistroVacaciones").Where(P => P.IndPeriodo == periodo && P.IndEstado == 1
                                                                && P.Nombramiento.Funcionario.IdCedulaFuncionario == cedula).FirstOrDefault();

                var nombramiento = periodoBusca.Nombramiento;

                if (nombramiento != null)
                {
                    PeriodoVacaciones idPeriodo = null;
                    if (nombramiento.PeriodoVacaciones.Count > 0)
                    {
                        idPeriodo = nombramiento.PeriodoVacaciones.Where(Q => Q.IndPeriodo == periodo).FirstOrDefault();
                    }
                    var periodoHistorico = entidadBase.C_EMU_Vacaciones_Saldo.Where(Q => Q.PERI_VACA == periodo && Q.CEDULA == cedula).FirstOrDefault();

                    if (idPeriodo != null)
                    {
                        var registro = idPeriodo.RegistroVacaciones.Where(Q => Q.NumTransaccion == documento).FirstOrDefault();

                        if (registro != null)
                        {
                            registro.ReintegroVacaciones.Add(reintegro);
                            idPeriodo.IndSaldo = idPeriodo.IndSaldo + Convert.ToDouble(cntDias);
                            entidadBase.SaveChanges();

                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = nombramiento,
                                Mensaje = "Se insertó el reintegro de vacaciones"
                            };
                        }
                        else
                        {
                            var registroVacaciones = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula && Q.PERI_TRAN == periodo).ToList();

                            if (registroVacaciones != null)
                            {
                                var datoHistorico = registroVacaciones.Where(Q => Q.DOCUMENTO.Trim() == documento).FirstOrDefault();
                                var nuevoRegistro = new RegistroVacaciones
                                {
                                    CntDias = Convert.ToDecimal(datoHistorico.DIAS.Replace('.',',')),
                                    FecActualizacion = Convert.ToDateTime(datoHistorico.FEC_ACT),
                                    FecInicio = Convert.ToDateTime(datoHistorico.FECHA_RIGE),
                                    FecFin = Convert.ToDateTime(datoHistorico.FECHA_VENCE),
                                    IndEstado = 1,
                                    IndTipoTransaccion = Convert.ToInt32(datoHistorico.TIPO_REG),
                                    NumTransaccion = documento,
                                };
                                idPeriodo.IndSaldo = idPeriodo.IndSaldo + Convert.ToDouble(cntDias);
                                idPeriodo.RegistroVacaciones.Add(nuevoRegistro);
                                idPeriodo.RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                datoHistorico.ESTADO = "Reintegro";
                                entidadBase.SaveChanges();

                                return new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = nombramiento,
                                    Mensaje = "Se insertó el reintegro de vacaciones"
                                };
                            }
                            else
                            {
                                throw new Exception("No se econtró ningún registro asociado al número de documento indicado.");
                            }
                        }
                    }
                    else
                    {
                        if (periodoHistorico != null)
                        {
                            var datosHistoricos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains(documento)).FirstOrDefault();
                            //var lista = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains("0001031")).ToList();
                            //var registroHistorico = idPeriodo.RegistroVacaciones.Where(Q => Q.NumTransaccion == documento).FirstOrDefault();

                            if (datosHistoricos != null)
                            {
                                nombramiento.PeriodoVacaciones.Add(
                                    new PeriodoVacaciones
                                    {
                                        FecCarga = DateTime.Now,
                                        IndDiasDerecho = cntDias,
                                        IndEstado = 1,
                                        IndPeriodo = periodo,
                                        IndSaldo = Convert.ToDouble(cntDias)
                                    }
                                );

                                nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Add(new RegistroVacaciones
                                {
                                    CntDias = Convert.ToDecimal(datosHistoricos.DIAS.Replace('.',',')),
                                    FecInicio = Convert.ToDateTime(datosHistoricos.FECHA_RIGE),
                                    FecFin = Convert.ToDateTime(datosHistoricos.FECHA_VENCE),
                                    IndEstado = datosHistoricos.ESTADO.Trim() != "" ? Convert.ToInt32(datosHistoricos.ESTADO) : 1,
                                    NumTransaccion = datosHistoricos.DOCUMENTO,
                                });

                                nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                datosHistoricos.ESTADO = "Reintegro";
                                entidadBase.SaveChanges();

                                return new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = nombramiento,
                                    Mensaje = "Se insertó el reintegro de vacaciones"
                                };
                            }
                            else
                            {
                                throw new Exception("No se econtró ningún registro asociado al número de documento indicado.");
                            }
                        }
                        else
                        {
                            //EN ESTE ELSE TENGO QUE BUSCAR EL HISTÓRICO DE VACACIONES
                            var registroVacaciones = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula && Q.PERI_TRAN == periodo).ToList();

                            if (registroVacaciones != null)
                            {
                                var datoHistorico = registroVacaciones.Where(Q => Q.DOCUMENTO.Trim() == documento).FirstOrDefault();
                                if (datoHistorico != null)
                                {
                                    nombramiento.PeriodoVacaciones.Add(
                                        new PeriodoVacaciones
                                        {
                                            FecCarga = DateTime.Now,
                                            IndDiasDerecho = cntDias,
                                            IndEstado = 1,
                                            IndPeriodo = periodo,
                                            IndSaldo = Convert.ToDouble(cntDias)
                                        }
                                    );

                                    nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Add(new RegistroVacaciones
                                    {
                                        CntDias = Convert.ToDecimal(datoHistorico.DIAS.Replace('.', ',')),
                                        FecInicio = Convert.ToDateTime(datoHistorico.FECHA_RIGE),
                                        FecFin = Convert.ToDateTime(datoHistorico.FECHA_VENCE),
                                        IndEstado = datoHistorico.ESTADO.Trim() != "" ? Convert.ToInt32(datoHistorico.ESTADO) : 1,
                                        NumTransaccion = datoHistorico.DOCUMENTO,
                                    });

                                    nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                    datoHistorico.ESTADO = "Reintegro";
                                    entidadBase.SaveChanges();

                                    return new CRespuestaDTO
                                    {
                                        Codigo = 1,
                                        Contenido = nombramiento,
                                        Mensaje = "Se insertó el reintegro de vacaciones"
                                    };
                                }
                                else
                                {
                                    throw new Exception("No se econtró el número de documento suministrado.");
                                }
                            }
                            else
                            {
                                throw new Exception("No se econtró ningún periodo con los datos suministrados.");
                            }
                        }
                    }
                }
                else
                {
                    PeriodoVacaciones idPeriodo = null;
                    nombramiento = entidadBase.Nombramiento.Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula &&
                                                                       (Q.FecVence == null || Q.FecVence > DateTime.Now))
                                                                 .OrderByDescending(O => O.FecRige).FirstOrDefault();
                    if (nombramiento.PeriodoVacaciones.Count > 0)
                    {
                        idPeriodo = nombramiento.PeriodoVacaciones.Where(Q => Q.IndPeriodo == periodo).FirstOrDefault();
                    }

                    var periodoHistorico = entidadBase.C_EMU_Vacaciones_Saldo.Where(Q => Q.PERI_VACA == periodo && Q.CEDULA == cedula).FirstOrDefault();

                    if (idPeriodo != null)
                    {
                        var registro = idPeriodo.RegistroVacaciones.Where(Q => Q.NumTransaccion == documento).FirstOrDefault();

                        if (registro != null)
                        {
                            registro.ReintegroVacaciones.Add(reintegro);
                            idPeriodo.IndSaldo = idPeriodo.IndSaldo + Convert.ToDouble(cntDias);
                            entidadBase.SaveChanges();

                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = nombramiento,
                                Mensaje = "Se insertó el reintegro de vacaciones"
                            };
                        }
                        else
                        {
                            var registroVacaciones = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula && Q.PERI_TRAN == periodo).ToList();

                            if (registroVacaciones != null)
                            {
                                var datoHistorico = registroVacaciones.Where(Q => Q.DOCUMENTO.Trim() == documento).FirstOrDefault();
                                var nuevoRegistro = new RegistroVacaciones
                                {
                                    CntDias = Convert.ToDecimal(datoHistorico.DIAS.Replace('.', ',')),
                                    FecActualizacion = Convert.ToDateTime(datoHistorico.FEC_ACT),
                                    FecInicio = Convert.ToDateTime(datoHistorico.FECHA_RIGE),
                                    FecFin = Convert.ToDateTime(datoHistorico.FECHA_VENCE),
                                    IndEstado = 1,
                                    IndTipoTransaccion = Convert.ToInt32(datoHistorico.TIPO_REG),
                                    NumTransaccion = documento,
                                };
                                idPeriodo.IndSaldo = idPeriodo.IndSaldo + Convert.ToDouble(cntDias);
                                idPeriodo.RegistroVacaciones.Add(nuevoRegistro);
                                idPeriodo.RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                datoHistorico.ESTADO = "Reintegro";
                                entidadBase.SaveChanges();

                                return new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = nombramiento,
                                    Mensaje = "Se insertó el reintegro de vacaciones"
                                };
                            }
                            else
                            {
                                throw new Exception("No se econtró ningún registro asociado al número de documento indicado.");
                            }
                        }
                    }
                    else
                    {
                        if (periodoHistorico != null)
                        {
                            var datosHistoricos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains(documento)).FirstOrDefault();
                            //var lista = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains("0001031")).ToList();
                            //var registroHistorico = idPeriodo.RegistroVacaciones.Where(Q => Q.NumTransaccion == documento).FirstOrDefault();

                            if (datosHistoricos != null)
                            {
                                nombramiento.PeriodoVacaciones.Add(
                                    new PeriodoVacaciones
                                    {
                                        FecCarga = DateTime.Now,
                                        IndDiasDerecho = cntDias,
                                        IndEstado = 1,
                                        IndPeriodo = periodo,
                                        IndSaldo = Convert.ToDouble(cntDias)
                                    }
                                );

                                nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Add(new RegistroVacaciones
                                {
                                    CntDias = Convert.ToDecimal(datosHistoricos.DIAS.Replace('.', ',')),
                                    FecInicio = Convert.ToDateTime(datosHistoricos.FECHA_RIGE),
                                    FecFin = Convert.ToDateTime(datosHistoricos.FECHA_VENCE),
                                    IndEstado = datosHistoricos.ESTADO.Trim() != "" ? Convert.ToInt32(datosHistoricos.ESTADO) : 1,
                                    NumTransaccion = datosHistoricos.DOCUMENTO,
                                });

                                nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                datosHistoricos.ESTADO = "Reintegro";
                                entidadBase.SaveChanges();

                                return new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = nombramiento,
                                    Mensaje = "Se insertó el reintegro de vacaciones"
                                };
                            }
                            else
                            {
                                throw new Exception("No se econtró ningún registro asociado al número de documento indicado.");
                            }
                        }
                        else
                        {
                            //EN ESTE ELSE TENGO QUE BUSCAR EL HISTÓRICO DE VACACIONES
                            var registroVacaciones = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula && Q.PERI_TRAN == periodo).ToList();

                            if (registroVacaciones != null)
                            {
                                var datoHistorico = registroVacaciones.Where(Q => Q.DOCUMENTO.Trim() == documento).FirstOrDefault();
                                if (datoHistorico != null)
                                {
                                    nombramiento.PeriodoVacaciones.Add(
                                        new PeriodoVacaciones
                                        {
                                            FecCarga = DateTime.Now,
                                            IndDiasDerecho = cntDias,
                                            IndEstado = 1,
                                            IndPeriodo = periodo,
                                            IndSaldo = Convert.ToDouble(cntDias)
                                        }
                                    );

                                    nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Add(new RegistroVacaciones
                                    {
                                        CntDias = Convert.ToDecimal(datoHistorico.DIAS.Replace('.', ',')),
                                        FecInicio = Convert.ToDateTime(datoHistorico.FECHA_RIGE),
                                        FecFin = Convert.ToDateTime(datoHistorico.FECHA_VENCE),
                                        IndEstado = datoHistorico.ESTADO.Trim() != "" ? Convert.ToInt32(datoHistorico.ESTADO) : 1,
                                        NumTransaccion = datoHistorico.DOCUMENTO,
                                    });

                                    nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                    datoHistorico.ESTADO = "Reintegro";
                                    entidadBase.SaveChanges();

                                    return new CRespuestaDTO
                                    {
                                        Codigo = 1,
                                        Contenido = nombramiento,
                                        Mensaje = "Se insertó el reintegro de vacaciones"
                                    };
                                }
                                else
                                {
                                    throw new Exception("No se econtró el número de documento suministrado.");
                                }
                            }
                            else
                            {
                                throw new Exception("No se econtró ningún periodo con los datos suministrados.");
                            }
                        }
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


        //public CRespuestaDTO RegistrarReintegroVacaciones(string cedula, string periodo, string documento, ReintegroVacaciones reintegro, decimal cntDias)
        //{
        //    try
        //    {
        //        var nombramiento = entidadBase.Nombramiento.Include("Funcionario").Include("PeriodoVacaciones")
        //                      .Where(N => N.Funcionario.IdCedulaFuncionario == cedula && N.PeriodoVacaciones.Count() > 0).OrderBy(Q => Q.FecRige).FirstOrDefault();

        //        if (nombramiento != null)
        //        {
        //            var idPeriodo = nombramiento.PeriodoVacaciones.Where(Q => Q.IndPeriodo == periodo).FirstOrDefault();
        //            var periodoHistorico = entidadBase.C_EMU_Vacaciones_Saldo.Where(Q => Q.PERI_VACA == periodo && Q.CEDULA == cedula).FirstOrDefault();

        //            if (idPeriodo != null)
        //            {
        //                var registro = idPeriodo.RegistroVacaciones.Where(Q => Q.NumTransaccion == documento).FirstOrDefault();

        //                if (registro != null)
        //                {
        //                    registro.ReintegroVacaciones.Add(reintegro);
        //                    idPeriodo.IndSaldo = idPeriodo.IndSaldo + Convert.ToDouble(cntDias);
        //                    entidadBase.SaveChanges();

        //                    return new CRespuestaDTO
        //                    {
        //                        Codigo = 1,
        //                        Contenido = nombramiento,
        //                        Mensaje = "Se insertó el reintegro de vacaciones"
        //                    };
        //                }
        //                else
        //                {
        //                    throw new Exception("No se econtró ningún registro asociado al número de documento indicado.");
        //                }
        //            }
        //            else
        //            {
        //                if (periodoHistorico != null)
        //                {
        //                    var datosHistoricos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO == documento).FirstOrDefault();
        //                    //var lista = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains("0001031")).ToList();
        //                    //var registroHistorico = idPeriodo.RegistroVacaciones.Where(Q => Q.NumTransaccion == documento).FirstOrDefault();

        //                    if (datosHistoricos != null)
        //                    {
        //                        nombramiento.PeriodoVacaciones.Add(
        //                            new PeriodoVacaciones
        //                            {
        //                                FecCarga = DateTime.Now,
        //                                IndDiasDerecho = cntDias,
        //                                IndEstado = 1,
        //                                IndPeriodo = periodo,
        //                                IndSaldo = Convert.ToDouble(cntDias)
        //                            }
        //                        );

        //                        nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Add(new RegistroVacaciones
        //                        {
        //                            CntDias = Convert.ToDecimal(datosHistoricos.DIAS.Replace('.', ',')),
        //                            FecInicio = Convert.ToDateTime(datosHistoricos.FECHA_RIGE),
        //                            FecFin = Convert.ToDateTime(datosHistoricos.FECHA_VENCE),
        //                            IndEstado = datosHistoricos.ESTADO.Trim() != "" ? Convert.ToInt32(datosHistoricos.ESTADO) : 1,
        //                            NumTransaccion = datosHistoricos.DOCUMENTO,
        //                        });

        //                        nombramiento.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
        //                        entidadBase.SaveChanges();

        //                        return new CRespuestaDTO
        //                        {
        //                            Codigo = 1,
        //                            Contenido = nombramiento,
        //                            Mensaje = "Se insertó el reintegro de vacaciones"
        //                        };
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("No se econtró ningún registro asociado al número de documento indicado.");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("No se econtró ningún periodo con los datos suministrados.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("No se econtró ningún funcionario con el número de cédula indicado.");
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

        public CRespuestaDTO RegistrarReintegroVacacionesModulo(string numeroDocumento, ReintegroVacaciones reintegro, string cedula, string periodo)
        {
            try
            {
                var nombramiento = entidadBase.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == cedula);
                if (nombramiento != null)
                {
                    var registrosCoinciden = entidadBase.RegistroVacaciones.Include("PeriodoVacaciones")
                                              .Where(Q => Q.NumTransaccion == numeroDocumento 
                                              && Q.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula
                                              && Q.PeriodoVacaciones.IndPeriodo == periodo).ToList();
                    RegistroVacaciones registro = null;
                    int encontrado = 0;

                    if (registrosCoinciden.Count > 1)
                    {
                        foreach (var item in registrosCoinciden)
                        {
                            if (reintegro.FecInicio >= item.FecInicio && reintegro.FecFin <= item.FecFin)
                            {
                                encontrado++;
                                registro = item;
                                break;
                            }
                        }
                    }
                    else
                    {
                        encontrado++;
                        registro = registrosCoinciden.FirstOrDefault();
                    }

                    if (registro != null && encontrado == 1)
                    {
                        if ((Convert.ToDateTime(registro.FecInicio).Date <= Convert.ToDateTime(reintegro.FecInicio).Date) && Convert.ToDateTime(registro.FecFin).Date >= Convert.ToDateTime(reintegro.FecFin).Date && reintegro.CntDias <= registro.CntDias)
                        {
                            registro.ReintegroVacaciones.Add(reintegro);
                            if (registro.PeriodoVacaciones.IndSaldo == null)
                            {
                                registro.PeriodoVacaciones.IndEstado = 1;
                                registro.PeriodoVacaciones.IndSaldo = Convert.ToDouble(reintegro.CntDias);
                                registro.IndEstado = 2;
                            }
                            else
                            {
                                registro.PeriodoVacaciones.IndEstado = 1;
                                registro.PeriodoVacaciones.IndSaldo = registro.PeriodoVacaciones.IndSaldo + Convert.ToDouble(reintegro.CntDias);
                                registro.IndEstado = 2;
                            }

                            entidadBase.SaveChanges();

                            return new CRespuestaDTO
                            {
                                Codigo = registro.PeriodoVacaciones.PK_PeriodoVacaciones,
                                Contenido = reintegro.PK_ReintegroVacaciones,
                                Mensaje = "Se insertó el reintegro de vacaciones"
                            };
                        }
                        else
                        {
                            if (reintegro.CntDias >= registro.CntDias)
                            {
                                throw new Exception("La cantidad de dias del reintegro es mayor al del registro de vacaciones.");
                            }
                            else
                            {
                                throw new Exception("La fecha del reintegro no está entre el rango de fechas del registro de vacaciones.");
                            }
                        }
                    }
                    else
                    {
                        //AQUI VA EL REINTEGRO DE PERIODOS ANTERIORES
                        var datosHistoricos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.DOCUMENTO.Contains(numeroDocumento) 
                                                                                            && Q.CEDULA.Contains(cedula)).FirstOrDefault();

                        var nombramientoActual = entidadBase.Nombramiento.Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula
                                                                                && (Q.FecVence >= DateTime.Now || Q.FecVence == null))
                                                                                .OrderByDescending(O => O.FecRige).FirstOrDefault();
                        if (datosHistoricos != null)
                        {
                            var periodoBusca = nombramientoActual.PeriodoVacaciones.Where(Q => Q.IndPeriodo == datosHistoricos.PERI_TRAN).FirstOrDefault();

                            if (periodoBusca != null)
                            {
                                var nuevoRegistro = new RegistroVacaciones
                                {
                                    CntDias = Convert.ToDecimal(datosHistoricos.DIAS.Replace('.', ',')),
                                    FecActualizacion = Convert.ToDateTime(datosHistoricos.FEC_ACT),
                                    FecInicio = Convert.ToDateTime(datosHistoricos.FECHA_RIGE),
                                    FecFin = Convert.ToDateTime(datosHistoricos.FECHA_VENCE),
                                    IndEstado = 1,
                                    IndTipoTransaccion = Convert.ToInt32(datosHistoricos.TIPO_REG),
                                    NumTransaccion = numeroDocumento,
                                };
                                periodoBusca.IndSaldo = periodoBusca.IndSaldo + Convert.ToDouble(datosHistoricos.DIAS.Replace('.', ','));
                                periodoBusca.RegistroVacaciones.Add(nuevoRegistro);
                                periodoBusca.RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                datosHistoricos.ESTADO = "Reintegro";
                                entidadBase.SaveChanges();

                                return new CRespuestaDTO
                                {
                                    Codigo = periodoBusca.PK_PeriodoVacaciones,
                                    Contenido = periodoBusca.RegistroVacaciones.Last().ReintegroVacaciones.Last().PK_ReintegroVacaciones,
                                    Mensaje = "Se insertó el reintegro de vacaciones"
                                };
                            }
                            else
                            {
                                nombramientoActual.PeriodoVacaciones.Add(
                                        new PeriodoVacaciones
                                        {
                                            FecCarga = DateTime.Now,
                                            IndDiasDerecho = Convert.ToDecimal(datosHistoricos.DIAS.Replace('.', ',')),
                                            IndEstado = 1,
                                            IndPeriodo = datosHistoricos.PERI_TRAN,
                                            IndSaldo = Convert.ToDouble(datosHistoricos.DIAS.Replace('.', ','))
                                        }
                                    );

                                nombramientoActual.PeriodoVacaciones.Last().RegistroVacaciones.Add(new RegistroVacaciones
                                {
                                    CntDias = Convert.ToDecimal(datosHistoricos.DIAS.Replace('.', ',')),
                                    FecInicio = Convert.ToDateTime(datosHistoricos.FECHA_RIGE),
                                    FecFin = Convert.ToDateTime(datosHistoricos.FECHA_VENCE),
                                    IndEstado = datosHistoricos.ESTADO.Trim() != "" ? Convert.ToInt32(datosHistoricos.ESTADO) : 1,
                                    NumTransaccion = datosHistoricos.DOCUMENTO,
                                });

                                nombramientoActual.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Add(reintegro);
                                datosHistoricos.ESTADO = "Reintegro";
                                entidadBase.SaveChanges();

                                return new CRespuestaDTO
                                {
                                    Codigo = nombramientoActual.PeriodoVacaciones.Last().PK_PeriodoVacaciones,
                                    Contenido = nombramientoActual.PeriodoVacaciones.Last().RegistroVacaciones.Last().ReintegroVacaciones.Last().PK_ReintegroVacaciones,
                                    Mensaje = "Se insertó el reintegro de vacaciones"
                                };
                            }
                        }
                        else
                        {
                            throw new Exception("No se encontró ningun registro.");
                        }
                    }

                }
                else
                {
                    throw new Exception("No se encontró el ningún funcionario activo asociado a la cédula suministrada. No es posible reintegrar vacaciones a funcionarios inactivos.");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message },
                    Mensaje = error.Message
                };
            }
        }

        public CRespuestaDTO ConsultaReintegroPeriodo(string IdCedulaFuncionario, string periodo, int codigo)
        {
            try
            {
                var datos = entidadBase.ReintegroVacaciones.Where(R => R.RegistroVacaciones.PeriodoVacaciones.IndPeriodo == periodo && R.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == IdCedulaFuncionario && R.RegistroVacaciones.PeriodoVacaciones.PK_PeriodoVacaciones == codigo).ToList();
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
                    throw new Exception("No se encontraron reintegros.");
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

        public CRespuestaDTO ListarReintegrosHistorial(string cedula, string periodo)
        {
            try
            {
                var datos = entidadBase.C_EMU_Vacaciones_Movimiento.Where(Q => Q.CEDULA == cedula && Q.TRANSACCION == "99" && Q.PERI_TRAN.Contains(periodo) /*&& ((!Q.DOCUMENTO.Contains("1299") || !Q.DOCUMENTO.Contains("0499") || !Q.DOCUMENTO.Contains("0399")))*/).OrderBy(Q => Q.FECHA_RIGE).ToList();
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

        public CRespuestaDTO Filtrar(object[] parametros)
        {
            var datos = entidadBase.ReintegroVacaciones.Include("RegistroVacaciones.PeriodoVacaciones").Include("RegistroVacaciones.PeriodoVacaciones.Nombramiento.Funcionario").AsQueryable();
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
                                datos = datos.Where(D => D.RegistroVacaciones.PeriodoVacaciones.IndPeriodo == periodo);
                                break;
                            case "Cedula":
                                var cedula = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario == cedula);
                                break;
                            case "NumeroTransaccion":
                                var numeroTransaccion = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.RegistroVacaciones.NumTransaccion == numeroTransaccion);
                                break;
                            case "Direccion":
                                var direccion = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion == direccion);
                                break;
                            case "Departamento":
                                var departamento = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento == departamento);
                                break;
                            case "Division":
                                var division = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.Division.NomDivision == division);
                                break;
                            case "Seccion":
                                var seccion = parametros[i + 1].ToString();
                                datos = datos.Where(D => D.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.NomSeccion == seccion);
                                break;
                            case "Fecha":
                                var fechaInicio = ((List<DateTime>)parametros[i + 1]).ElementAt(0);
                                var fechaFin = ((List<DateTime>)parametros[i + 1]).ElementAt(1);
                                datos = datos.Where(D => D.FecInicio == fechaInicio && D.FecFin == fechaFin);
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

        public CRespuestaDTO RegistrarReintegroRegistro(int registro, string fuente, ReintegroVacaciones reintegro)
        {
            try
            {
                if (fuente == "SIRH")
                {
                    var documento = entidadBase.RegistroVacaciones.FirstOrDefault(R => R.PK_RegistroVacaciones == registro);
                    var periodo = documento.PeriodoVacaciones;

                    if (documento != null)
                    {
                        if (documento.ReintegroVacaciones.Count() > 0)
                        {
                            if (documento.CntDias > (documento.ReintegroVacaciones.Sum(R => R.CntDias) + reintegro.CntDias))
                            {
                                documento.ReintegroVacaciones.Add(reintegro);
                                periodo.IndSaldo = periodo.IndSaldo + Convert.ToDouble(reintegro.CntDias);
                                documento.IndEstado = 2;
                                entidadBase.SaveChanges();
                                return new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = periodo.PK_PeriodoVacaciones
                                };
                            }
                            else
                            {
                                throw new Exception("El reintegro no pudo realizarse. La cantidad de días a reintegrar es superior a la cantidad de días del registro. Puede que hayan otros reintegros asociados al registro.");
                            }
                        }
                        else
                        {
                            documento.ReintegroVacaciones.Add(reintegro);
                            periodo.IndSaldo = periodo.IndSaldo + Convert.ToDouble(reintegro.CntDias);
                            documento.IndEstado = 2;
                            entidadBase.SaveChanges();
                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = periodo.PK_PeriodoVacaciones
                            };
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el registro a reintegrar");
                    }
                }
                else
                {
                    var documento = entidadBase.C_EMU_Vacaciones_Movimiento.FirstOrDefault(R => R.ID == registro);
                    var existePeriodo = (entidadBase.PeriodoVacaciones.Where(P => P.IndPeriodo == documento.PERI_TRAN && P.Nombramiento.Funcionario.IdCedulaFuncionario == documento.CEDULA && P.IndEstado == 1).Count() > 0);
                    if (documento != null)
                    {
                        RegistroVacaciones dato = new RegistroVacaciones
                        {
                            CntDias = Convert.ToDecimal(documento.DIAS),
                            FecActualizacion = Convert.ToDateTime(documento.FEC_ACT),
                            FecFin = Convert.ToDateTime(documento.FECHA_VENCE),
                            FecInicio = Convert.ToDateTime(documento.FECHA_RIGE),
                            IndEstado = 1,
                            IndTipoTransaccion = Convert.ToInt32(documento.TRANSACCION),
                            NumTransaccion = documento.DOCUMENTO
                        };

                        dato.ReintegroVacaciones.Add(reintegro);

                        if (existePeriodo)
                        {
                            var periodoEmu = entidadBase.PeriodoVacaciones.Where(P => P.IndPeriodo == documento.PERI_TRAN && P.Nombramiento.Funcionario.IdCedulaFuncionario == documento.CEDULA && P.IndEstado == 1).FirstOrDefault();
                            documento.ESTADO = "Reintegro";
                            periodoEmu.IndSaldo = periodoEmu.IndSaldo + Convert.ToDouble(reintegro.CntDias);
                            entidadBase.SaveChanges();
                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = periodoEmu.PK_PeriodoVacaciones
                            };
                        }
                        else
                        {
                            PeriodoVacaciones nuevoPeriodo = new PeriodoVacaciones
                            {
                                FecCarga = DateTime.Now,
                                IndDiasDerecho = 0,
                                IndEstado = 1,
                                IndPeriodo = documento.PERI_TRAN,
                                FK_Nombramiento = entidadBase.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == documento.CEDULA && (N.FecVence == null || N.FecVence >= DateTime.Now)).OrderByDescending(F => F.FecRige).FirstOrDefault().PK_Nombramiento,
                                IndSaldo = Convert.ToDouble(reintegro.CntDias),
                            };
                            nuevoPeriodo.RegistroVacaciones.Add(dato);
                            documento.ESTADO = "Reintegro";
                            entidadBase.SaveChanges();
                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = nuevoPeriodo.PK_PeriodoVacaciones
                            };
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró el registro a reintegrar");
                    }
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.InnerException != null ? error.InnerException.Message : error.Message }
                };
            }
        }

        #endregion

    }
}
