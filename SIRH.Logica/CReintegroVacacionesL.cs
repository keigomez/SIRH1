using SIRH.Datos;
using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Logica
{
    public class CReintegroVacacionesL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CReintegroVacacionesL()
        {
            contexto = new SIRHEntities();
        }
        internal static CReintegroVacacionesDTO ConvertirDatosReintegroVacacionesADTO(ReintegroVacaciones item)
        {
            return new CReintegroVacacionesDTO
            {
                 SolReintegro=item.NumSolicitudReintegro,
                 CantidadDias=Convert.ToDecimal(item.CntDias),
                 Observaciones=item.ObsReintegro,
                 FechaRige= Convert.ToDateTime( item.FecInicio), 
                 FechaVence=Convert.ToDateTime(item.FecFin)
            };
        }
        public List<CBaseDTO> ListarReintegrosPeriodos(string cedula, string periodo, int codigo)
        {
            List<CBaseDTO> respuesta;
            try
            {
                CReintegroVacacionesD intermedio = new CReintegroVacacionesD(contexto);
                var resultado = intermedio.ConsultaReintegroPeriodo(cedula, periodo, codigo);
                var resultadoHistorial = intermedio.ListarReintegrosHistorial(cedula, periodo);
                if (resultado.Codigo != -1)
                {
                    respuesta = new List<CBaseDTO>();

                    var listaPeriodos = ((List<ReintegroVacaciones>)resultado.Contenido);

                    foreach (var item in listaPeriodos)
                    {
                        var dato = ConvertirDatosReintegroVacacionesADTO(item);
                        dato.Fuente = "SIRH";
                        respuesta.Add(dato);
                    }

                    if (resultadoHistorial.Codigo > 0)
                    {
                        var listaHistorial = ((List<C_EMU_Vacaciones_Movimiento>)resultadoHistorial.Contenido);

                        foreach (var item in listaHistorial)
                        {
                            var dato = ConvertirDatosReintegroHistorialADTO(item);
                            dato.Fuente = "Emulación";
                            respuesta.Add(dato);
                        }
                    }

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

        private CReintegroVacacionesDTO ConvertirDatosReintegroHistorialADTO(C_EMU_Vacaciones_Movimiento item)
        {
            return new CReintegroVacacionesDTO
            {
                CantidadDias = Convert.ToDecimal(item.DIAS.Contains(".") ? item.DIAS.Replace('.', ',') : item.DIAS),
                FechaActualizacion = Convert.ToDateTime(item.FEC_ACT),
                FechaRige = Convert.ToDateTime(item.FECHA_RIGE),
                FechaVence = Convert.ToDateTime(item.FECHA_VENCE),
                Motivo = Convert.ToInt32(item.TIPO_REG),
                SolReintegro = item.DOCUMENTO
            };
        }

        public CBaseDTO RegistraReintegroVacaciones(string numeroDocumento, CReintegroVacacionesDTO reintegroVacaciones, string cedFuncionario)
        {
            CBaseDTO respuesta = new CBaseDTO();
            CReintegroVacacionesD intermedioReintegroV = new CReintegroVacacionesD(contexto);
            try
            {
                ReintegroVacaciones datosReintegro = new ReintegroVacaciones
                {
                    CntDias = reintegroVacaciones.CantidadDias,
                    FecFin = reintegroVacaciones.FechaVence,
                    FecInicio = reintegroVacaciones.FechaRige,
                    ObsReintegro = reintegroVacaciones.Observaciones,
                    IndMotivo = 88,
                    NumSolicitudReintegro = reintegroVacaciones.SolReintegro,
                    FecActualizacion=DateTime.Now
                };
                respuesta = intermedioReintegroV.RegistrarReintegroVacacionesModulo(numeroDocumento, datosReintegro, cedFuncionario, reintegroVacaciones.Mensaje);
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Mensaje=error.Message,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }
        public List<List<CBaseDTO>> BuscarReintegros(CFuncionarioDTO funcionario, CPeriodoVacacionesDTO periodoVacaciones, CRegistroVacacionesDTO registroVacaciones,
                                List<DateTime> fechas, string direccion, string seccion, string division, string departamento)
        {
            List<List<CBaseDTO>> res = new List<List<CBaseDTO>>();
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<CBaseDTO> respuestaFuncionario = new List<CBaseDTO>();
            List<CBaseDTO> respuestaPeriodo = new List<CBaseDTO>();
            CReintegroVacacionesD  intermedioReintegros = new CReintegroVacacionesD(contexto);
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
                    parametros.Add("Fechas");
                    parametros.Add(fechas);
                }
                var listaVacaciones = intermedioReintegros.Filtrar(parametros.ToArray());
                var lista = ((List<ReintegroVacaciones>)listaVacaciones.Contenido);
                if (lista != null)
                {
                    foreach (var item in lista)
                    {
                        var dato = ConvertirDatosReintegroVacacionesADTO(item);
                        var datoCed = new Funcionario { IdCedulaFuncionario = item.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario };
                        respuesta.Add(dato);
                        respuestaFuncionario.Add(intermedioFuncionario.DescargarFuncionario(datoCed.IdCedulaFuncionario));
                        respuestaPeriodo.Add(intermedioPeriodo.DescargarPeriodo(item.RegistroVacaciones.PeriodoVacaciones.Nombramiento.Funcionario.IdCedulaFuncionario, item.RegistroVacaciones.PeriodoVacaciones.IndPeriodo));
                    }
                    if (respuesta.Count() == 0 || respuestaFuncionario.Count() == 0 || respuestaPeriodo.Count()==0)
                    {
                        respuesta.Add(new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No se encontraron reintegros para los parametros ingresados!"
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

        public CBaseDTO RegistrarReintegroRegistro(CReintegroVacacionesDTO reintegro)
        {
            try
            {
                CReintegroVacacionesD intermedio = new CReintegroVacacionesD(contexto);

                var datoReintegro = new ReintegroVacaciones
                {
                    FecActualizacion = reintegro.FechaActualizacion,
                    CntDias = reintegro.CantidadDias,
                    FecFin = reintegro.FechaVence,
                    FecInicio = reintegro.FechaRige,
                    IndMotivo = reintegro.Motivo,
                    NumSolicitudReintegro = reintegro.SolReintegro,
                    ObsReintegro = reintegro.Observaciones
                };

                var resultado = intermedio.RegistrarReintegroRegistro(reintegro.RegistroVacaciones.IdEntidad, reintegro.Fuente, datoReintegro);

                if(resultado.Codigo > 0)
                {
                    return new CBaseDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.InnerException != null ? error.InnerException.Message : error.Message };
            }
        }

        #endregion
    }
}
