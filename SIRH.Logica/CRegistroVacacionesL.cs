using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Logica
{
    public class CRegistroVacacionesL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CRegistroVacacionesL()
        {
            contexto = new SIRHEntities();
        }


        internal static CRegistroVacacionesDTO ConvertirDatosRegistroVacacionesADTO(RegistroVacaciones item)
        {
            return new CRegistroVacacionesDTO
            {
                IdEntidad = item.PK_RegistroVacaciones,
                FechaRige = Convert.ToDateTime(item.FecInicio),
                FechaVence = Convert.ToDateTime(item.FecFin),
                Estado = Convert.ToInt16(item.IndEstado),
                NumeroTransaccion = item.NumTransaccion,
                Dias = Convert.ToDecimal(item.CntDias),
                TipoTransaccion = Convert.ToInt32(item.IndTipoTransaccion)

            };
        }
        public List<CBaseDTO> ListarRegistroVacaciones(string cedula, string periodo, int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CRegistroVacacionesD intermedio = new CRegistroVacacionesD(contexto);
                
                var resultado = intermedio.ConsultaVacacionesPeriodo(cedula, periodo, codigo);
                var resultadoHistorial = intermedio.ConsultaVacacionesHistorialModulo(cedula);
                if (resultado.Codigo != -1)
                {
                    var listaPeriodos = ((List<RegistroVacaciones>)resultado.Contenido);

                    foreach (var item in listaPeriodos)
                    {
                        var dato = ConvertirDatosRegistroVacacionesADTO(item);
                        dato.Fuente = "SIRH";
                        respuesta.Add(dato);
                    }
                }

                if (resultadoHistorial.Codigo != -1)
                {
                    var listaPeriodosHistorial = ((List<C_EMU_Vacaciones_Movimiento>)resultadoHistorial.Contenido).Where(Q => Q.PERI_TRAN == periodo).ToList();

                    foreach (var item in listaPeriodosHistorial)
                    {
                        var dato = ConvertirDatosHistorialVacacionesADTO(item);
                        dato.Fuente = "Emulación";
                        if (item.ESTADO == "Reintegro")
                        {
                            dato.Estado = -1;
                        }
                        respuesta.Add(dato);
                    }
                }

                if (respuesta.Count > 0)
                {
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO>();
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message,
                    Mensaje = error.Message
                });
                return respuesta;
            }
        }

        internal static CRegistroVacacionesDTO ConvertirDatosHistorialVacacionesADTO(C_EMU_Vacaciones_Movimiento item)
        {
            return new CRegistroVacacionesDTO
            {
                Dias = Convert.ToDecimal(item.DIAS.Replace('.', ',')),
                Estado = item.ESTADO != null && item.ESTADO.TrimEnd() != "" && item.ESTADO != "Reintegro" ? Convert.ToInt32(item.ESTADO) : 0,
                FechaActualizacion = Convert.ToDateTime(item.FEC_ACT),
                FechaRige = Convert.ToDateTime(item.FECHA_RIGE),
                FechaVence = Convert.ToDateTime(item.FECHA_VENCE),
                IdEntidad = item.ID,
                Mensaje = "Historico",
                NumeroTransaccion = item.DOCUMENTO,
                Periodo = new CPeriodoVacacionesDTO { Periodo = item.PERI_TRAN },
                TipoTransaccion = item.TIPO_REG != null ? Convert.ToInt32(item.TIPO_REG) : 0
            };
        }

        public List<CBaseDTO> ObtenerRegistro(string cedFuncionario, string numeroDocumento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CRegistroVacacionesD intermedioVacaciones = new CRegistroVacacionesD(contexto);
                CFuncionarioD intermediofuncionario = new CFuncionarioD(contexto);
                CPeriodoVacacionesL intermedioPeriodo = new CPeriodoVacacionesL();

                var funcionario = intermediofuncionario.BuscarFuncionarioCedula(cedFuncionario);
                if (funcionario != null)
                {
                    var registro = intermedioVacaciones.ConsultaVacacionesPorDocumento(cedFuncionario, numeroDocumento);
                    if (registro.Codigo > 0)
                    {
                        if (registro.Codigo == 1)
                        {
                            var datoRegistro = ConvertirDatosRegistroVacacionesADTO((RegistroVacaciones)registro.Contenido);
                            respuesta.Add(datoRegistro);
                            var periodo = intermedioPeriodo.ObtenerPeriodo(cedFuncionario, ((RegistroVacaciones)registro.Contenido).PeriodoVacaciones.IndPeriodo, ((RegistroVacaciones)registro.Contenido).PeriodoVacaciones.PK_PeriodoVacaciones);
                            respuesta.Add(periodo);
                        }
                        else
                        {
                            var registroHistorico = new CRegistroVacacionesDTO
                            {
                                Dias = Convert.ToDecimal(((C_EMU_Vacaciones_Movimiento)registro.Contenido).DIAS.Replace('.',',')),
                                FechaRige = Convert.ToDateTime(((C_EMU_Vacaciones_Movimiento)registro.Contenido).FECHA_RIGE),
                                FechaVence = Convert.ToDateTime(((C_EMU_Vacaciones_Movimiento)registro.Contenido).FECHA_VENCE),
                                NumeroTransaccion = ((C_EMU_Vacaciones_Movimiento)registro.Contenido).DOCUMENTO
                            };
                            respuesta.Add(registroHistorico);
                            var periodoHistorico = new CPeriodoVacacionesDTO
                            {
                                DiasDerecho = 0,
                                FechaCarga = Convert.ToDateTime(((C_EMU_Vacaciones_Movimiento)registro.Contenido).FEC_SOLICITUD),
                                Periodo = ((C_EMU_Vacaciones_Movimiento)registro.Contenido).PERI_TRAN,
                                Saldo = 0
                            };
                            respuesta.Add(periodoHistorico);
                        }
                    }
                    else
                    {
                        respuesta.Add(new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No se encontró el registro de vacaciones con el documento indicado."
                        });
                    }
                }
                else
                {
                    respuesta.Add(new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "No se encontró el funcionario digitado."
                    });
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
            }
            return respuesta;
        }
        public CBaseDTO GuardaRegistroVacaciones(CRegistroVacacionesDTO registroVacaciones, string cedulaFuncionario, string periodo, int documento)
        {
            CBaseDTO respuesta = new CBaseDTO();
            CFuncionarioD intermediofuncionario = new CFuncionarioD(contexto);
            try
            {

                CRegistroVacacionesD intermedioRegistroVacaciones = new CRegistroVacacionesD(contexto);
                RegistroVacaciones datosRegistroVacaciones = new RegistroVacaciones
                {
                    CntDias = registroVacaciones.Dias,
                    FecFin = registroVacaciones.FechaVence,
                    FecInicio = registroVacaciones.FechaRige,
                    IndTipoTransaccion = documento,
                    NumTransaccion = registroVacaciones.NumeroTransaccion,
                    IndEstado = 1,
                    FecActualizacion = DateTime.Now
                };
                respuesta = intermedioRegistroVacaciones.GuardarRegistroVacacionesModulo(datosRegistroVacaciones, cedulaFuncionario, periodo);
                return respuesta;
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
        public List<List<CBaseDTO>> BuscarVacaciones(CFuncionarioDTO funcionario, CPeriodoVacacionesDTO periodoVacaciones, CRegistroVacacionesDTO registroVacaciones,
                                    List<DateTime> fechas, string direccion, string seccion, string division, string departamento, string estadoSeleccion, string tipoVacaciones)
        {
            List<List<CBaseDTO>> res = new List<List<CBaseDTO>>();
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<CBaseDTO> respuestaFuncionario = new List<CBaseDTO>();
            List<CBaseDTO> respuestaPeriodo = new List<CBaseDTO>();

            CRegistroVacacionesD intermedioRegistroVacaciones = new CRegistroVacacionesD(contexto);
            CFuncionarioL intermedioFuncionario = new CFuncionarioL();
            CPeriodoVacacionesL intermedioPeriodo = new CPeriodoVacacionesL();
            try
            {
                List<object> parametros = new List<object>();

                if (funcionario.Cedula != null)
                {
                    parametros.Add("Cedula");
                    parametros.Add(funcionario.Cedula);
                }
                if (periodoVacaciones.Periodo != null)
                {
                    parametros.Add("PeriodoVacaciones");
                    parametros.Add(periodoVacaciones.Periodo);
                }
                if (registroVacaciones.NumeroTransaccion != null)
                {
                    parametros.Add("NumeroTransaccion");
                    parametros.Add(registroVacaciones.NumeroTransaccion);
                }
                if (seccion != "null")
                {
                    parametros.Add("Seccion");
                    parametros.Add(seccion);
                }
                if (division != "null")
                {
                    parametros.Add("Division");
                    parametros.Add(division);
                }
                if (departamento != "null")
                {
                    parametros.Add("Departamento");
                    parametros.Add(departamento);
                }
                if (direccion != "null")
                {
                    parametros.Add("Direccion");
                    parametros.Add(direccion);
                }
                if (fechas.Count == 2)
                {
                    parametros.Add("Fecha");
                    parametros.Add(fechas);
                }
                if (tipoVacaciones != "null")
                {
                    parametros.Add("TipoVacaciones");
                    parametros.Add(tipoVacaciones);
                }
                var listaVacaciones = intermedioRegistroVacaciones.Filtrar(parametros.ToArray(), estadoSeleccion);
                var lista = ((List<RegistroVacaciones>)listaVacaciones.Contenido);
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        var dato = ConvertirDatosRegistroVacacionesADTO(item);
                        var datoCed = new Funcionario { IdCedulaFuncionario = item.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario };
                        respuesta.Add(dato);
                        respuestaPeriodo.Add(intermedioPeriodo.DescargarPeriodo(item.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario, item.PeriodoVacaciones.IndPeriodo));

                        respuestaFuncionario.Add(intermedioFuncionario.DescargarFuncionario(datoCed.IdCedulaFuncionario));
                    }
                    if (respuesta.Count() == 0 || respuestaFuncionario.Count() == 0 || respuestaPeriodo.Count() == 0)
                    {
                        respuesta.Add(new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No se encontraron registros para los parametros ingresados!"
                        });
                        res.Add(respuesta);
                        return res;
                    }
                    else
                    {
                        res.Add(respuesta);
                        res.Add(respuestaFuncionario);
                        res.Add(respuestaPeriodo);
                        return res;
                    }
                }
                else
                {
                    respuesta.Add(new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "No se encontraron registros para los parametros ingresados!"
                    });
                    res.Add(respuesta);
                    return res;
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message,
                    Mensaje = error.Message
                });
                res.Add(respuesta);
                return res;
            }
        }
        public CBaseDTO GuardarRebajoColectivo(CFuncionarioDTO funcionario, CRegistroVacacionesDTO rebajoColectivo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {

                CRegistroVacacionesD intermedioRegistroVacaciones = new CRegistroVacacionesD(contexto);
                RegistroVacaciones datosReintegro = new RegistroVacaciones
                {
                    CntDias = rebajoColectivo.Dias,
                    FecFin = rebajoColectivo.FechaVence,
                    FecInicio = rebajoColectivo.FechaRige,
                    IndTipoTransaccion = 88,
                    NumTransaccion = rebajoColectivo.NumeroTransaccion,
                    IndEstado = 1
                };
                respuesta = intermedioRegistroVacaciones.GuardarRebajoColectivo(funcionario, datosReintegro);
                return respuesta;
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
        public CBaseDTO AnularRebajoColectivo(CFuncionarioDTO funcionario, string numTransaccion)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {

                CRegistroVacacionesD intermedioRegistroVacaciones = new CRegistroVacacionesD(contexto);
                RegistroVacaciones datosReintegro = new RegistroVacaciones
                {
                    NumTransaccion = numTransaccion

                };
                respuesta = intermedioRegistroVacaciones.AnularRebajoColectivo(funcionario, numTransaccion);
                return respuesta;
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
        public CBaseDTO ValidarNumeroDocumento(string numTransaccion)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {

                CRegistroVacacionesD intermedioRegistroVacaciones = new CRegistroVacacionesD(contexto);
                respuesta = intermedioRegistroVacaciones.ValidarNumeroDocumento(numTransaccion);
                return respuesta;
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
        public List<List<CBaseDTO>> ObtenerInconsistencias(DateTime fechaInicio, DateTime fechaFin, string cedula)
        {
            List<CBaseDTO> respuestaFuncionario = new List<CBaseDTO>();
            CFuncionarioL intermedioFuncionario = new CFuncionarioL();
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<List<CBaseDTO>> res = new List<List<CBaseDTO>>();
            try
            {
                CRegistroVacacionesD intermedioVacaciones = new CRegistroVacacionesD(contexto);

                var registro = intermedioVacaciones.ConsultaInconsistencias(fechaInicio, fechaFin, cedula);
                var lista = ((List<RegistroVacaciones>)registro.Contenido);
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        var dato = ConvertirDatosRegistroVacacionesADTO(item);
                        respuesta.Add(dato);
                        respuestaFuncionario.Add(intermedioFuncionario.DescargarFuncionario(cedula));
                    }
                    res.Add(respuesta);
                    res.Add(respuestaFuncionario);
                    return res;
                }
                else
                {
                    respuesta.Add(new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "No se encontraron inconsistencias."
                    });
                    res.Add(respuesta);
                    return res;
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message,
                    Mensaje = error.Message
                });
                res.Add(respuesta);
                return res;
            }
        }

        public CBaseDTO TrasladarRegistroVacaciones(int idRegistro, decimal dias, int periodoDestino)
        {
            try
            {
                CRegistroVacacionesD intermedio = new CRegistroVacacionesD(contexto);

                var respuesta = intermedio.TrasladarRegistroVacaciones(idRegistro, dias, periodoDestino);

                if (respuesta.Codigo > 0)
                {
                    return CPeriodoVacacionesL.ConvertirDatosPeriodoVacacionesADTO(((PeriodoVacaciones)respuesta.Contenido));
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    MensajeError = error.Message
            };
        }
    }

    #endregion
}
}
