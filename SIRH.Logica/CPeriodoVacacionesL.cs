using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPeriodoVacacionesL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPeriodoVacacionesL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        internal static CRegistroVacacionesDTO ConvertirC_EMU_Vacaciones_MovimientoADTO(C_EMU_Vacaciones_Movimiento periodo)
        {
            return new CRegistroVacacionesDTO
            {
                Periodo = new CPeriodoVacacionesDTO { Periodo = periodo.PERI_TRAN },
                TipoTransaccion = Convert.ToInt32(periodo.TRANSACCION),
                Dias = Convert.ToDecimal(periodo.DIAS.Replace('.',',')),
                NumeroTransaccion = periodo.DOCUMENTO,
                FechaRige = Convert.ToDateTime(periodo.FECHA_RIGE),
                FechaVence = Convert.ToDateTime(periodo.FECHA_VENCE),
                FechaActualizacion = Convert.ToDateTime(periodo.FECHA_ACT_K),
            };

        }

        internal static CPeriodoVacacionesDTO ConvertirDatosPeriodoVacacionesADTO(PeriodoVacaciones item)
        {
            return new CPeriodoVacacionesDTO
            {
                IdEntidad = item.PK_PeriodoVacaciones,
                Periodo = item.IndPeriodo,
                Saldo = Convert.ToDouble(item.IndSaldo),
                DiasDerecho = Convert.ToDecimal(item.IndDiasDerecho),
                Estado = item.IndEstado,
                FechaCarga = item.FecCarga
            };
        }

        public CBaseDTO AnularPeriodoVacaciones(int periodo)
        {
            try
            {
                CBaseDTO respuesta = new CBaseDTO();
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                var resultado = intermedio.AnularPeriodoVacaciones(periodo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirDatosPeriodoVacacionesADTO((PeriodoVacaciones)resultado.Contenido);
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

        public CBaseDTO ObtenerPeriodo(string cedFuncionario, string codPeriodo, int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CPeriodoVacacionesD intermedioPeriodo = new CPeriodoVacacionesD(contexto);
                CFuncionarioD intermediofuncionario = new CFuncionarioD(contexto);
                var funcionario = intermediofuncionario.BuscarFuncionarioCedula(cedFuncionario);
                if (funcionario != null)
                {
                    var periodo = intermedioPeriodo.ObtenerPeriodoCodigo(cedFuncionario, codPeriodo, codigo);
                    if (periodo.Codigo > 0)
                    {
                        var datoPeriodo = ConvertirDatos((PeriodoVacaciones)periodo.Contenido);
                        return datoPeriodo;
                    }
                    else
                    {
                        respuesta = new CErrorDTO
                        {
                            Codigo = -1,
                            MensajeError = "No se encontró el periodo digitado."
                        };
                    }
                }
                else
                {
                    respuesta = new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "No se encontró el funcionario digitado."
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO { Codigo = -1, MensajeError = error.Message });
            }
            return respuesta;
        }
        public CPeriodoVacacionesDTO DescargarPeriodo(string cedFuncionario, string codPeriodo)
        {

            CPeriodoVacacionesD intermedioPeriodo = new CPeriodoVacacionesD(contexto);


            var periodo = intermedioPeriodo.ObtenerPeriodo(cedFuncionario, codPeriodo);

            var datoPeriodo = ConvertirDatos((PeriodoVacaciones)periodo.Contenido);
            return datoPeriodo;
        }
        internal static CPeriodoVacacionesDTO ConvertirDatos(PeriodoVacaciones item)
        {
            return new CPeriodoVacacionesDTO
            {
                IdEntidad = item.PK_PeriodoVacaciones,
                DiasDerecho = Convert.ToDecimal(item.IndDiasDerecho),
                FechaCarga = DateTime.Now,
                Estado = item.IndEstado,
                Periodo = item.IndPeriodo,
                Saldo = Convert.ToDouble(item.IndSaldo)
            };
        }

        public List<CBaseDTO> ListarPeriodosActivos(string cedula)
        {
            List<CBaseDTO> respuesta;
            try
            {
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                var resultado = intermedio.ListarPeriodosActivos(cedula);

                if (resultado.Codigo != -1)
                {
                    respuesta = new List<CBaseDTO>();

                    var listaPeriodos = ((List<PeriodoVacaciones>)resultado.Contenido);

                    foreach (var item in listaPeriodos)
                    {
                        var dato = ConvertirDatosPeriodoVacacionesADTO(item);
                        dato.CantidadSolicitudes = item.RegistroVacaciones.Count;
                        respuesta.Add(dato);
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
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<CBaseDTO> ListarPeriodosNoActivos(string cedula)
        {
            List<CBaseDTO> respuesta;
            try
            {
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                var resultado = intermedio.ListarPeriodosNoActivos(cedula);

                if (resultado.Codigo != -1)
                {
                    respuesta = new List<CBaseDTO>();

                    var listaPeriodos = ((List<PeriodoVacaciones>)resultado.Contenido);

                    foreach (var item in listaPeriodos)
                    {
                        var dato = ConvertirDatosPeriodoVacacionesADTO(item);
                        dato.CantidadSolicitudes = item.RegistroVacaciones.Count;
                        respuesta.Add(dato);
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
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<string[]> ListarPeriodosSG(string cedula)
        {
            List<string[]> respuesta;
            try
            {
                var datos = ListarPeriodosActivos(cedula);
                respuesta = new List<string[]>();
                foreach (var item in datos)
                {
                    respuesta.Add(new string[]
                    {
                        ((CPeriodoVacacionesDTO)item).Periodo != null ? ((CPeriodoVacacionesDTO)item).Periodo : "SD",
                        ((CPeriodoVacacionesDTO)item).Saldo.ToString(),
                        ((CPeriodoVacacionesDTO)item).CantidadSolicitudes.ToString()
                    });
                }
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new List<string[]>();
                respuesta.Add(new string[] { error.Message });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> ObtenerDetalleVacaciones(string cedFuncionario)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            List<CBaseDTO> temp = new List<CBaseDTO>();
            try
            {
                CPeriodoVacacionesL intermedioPeriodo = new CPeriodoVacacionesL();
                CFuncionarioD intermediofuncionario = new CFuncionarioD(contexto);
                var funcionario = intermediofuncionario.BuscarFuncionarioCedula(cedFuncionario);
                if (funcionario != null)
                {
                    respuesta.Add(intermedioPeriodo.ListarPeriodosActivos(cedFuncionario));
                    respuesta.Add(intermedioPeriodo.ListarPeriodosNoActivos(cedFuncionario));
                    return respuesta;
                }
                else
                {
                    temp.Add(new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "No se encontró el periodo digitado."
                    });
                    respuesta.Add(temp);

                }

            }
            catch (Exception error)
            {
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                respuesta.Add(temp);
            }
            return respuesta;
        }

        public CBaseDTO GuardaRegistroPeriodo(CPeriodoVacacionesDTO registroPeriodo, string cedulaFuncionario)
        {
            CBaseDTO respuesta = new CBaseDTO();
            CFuncionarioD intermediofuncionario = new CFuncionarioD(contexto);
            var nombramiento = intermediofuncionario.BuscarFuncionarioCedula(cedulaFuncionario)
                                                    .Nombramiento.Where(Q => (Q.FecVence >= DateTime.Now
                                                    || Q.FecVence == null)).OrderByDescending(F => F.FecRige).FirstOrDefault();
            try
            {

                CPeriodoVacacionesD intermedioPeriodo = new CPeriodoVacacionesD(contexto);
                PeriodoVacaciones periodoVac = new PeriodoVacaciones
                {
                    FecCarga = DateTime.Now,
                    IndEstado = 1,
                    Nombramiento = nombramiento,
                    IndPeriodo = registroPeriodo.Periodo,
                    IndDiasDerecho = registroPeriodo.DiasDerecho,
                    FK_Nombramiento = nombramiento.PK_Nombramiento,
                    IndSaldo = Convert.ToDouble(registroPeriodo.DiasDerecho)
                };
                var resultado = intermedioPeriodo.RegistrarPeriodoVacaciones(cedulaFuncionario, periodoVac);
                return new CBaseDTO { IdEntidad = ((PeriodoVacaciones)resultado.Contenido).PK_PeriodoVacaciones };
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    IdEntidad = -1,
                    MensajeError = error.Message
                };
                return respuesta;
            }

        }

        public List<CBaseDTO> ListaPeriodos(string cedula)
        {
            List<CBaseDTO> respuesta;
            try
            {
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                var resultado = intermedio.ListaPeriodos(cedula);

                if (resultado.Codigo != -1)
                {
                    respuesta = new List<CBaseDTO>();

                    var listaPeriodos = ((List<PeriodoVacaciones>)resultado.Contenido);

                    foreach (var item in listaPeriodos)
                    {
                        var dato = ConvertirDatosPeriodoVacacionesADTO(item);
                        dato.CantidadSolicitudes = item.RegistroVacaciones.Count;
                        respuesta.Add(dato);
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
                    MensajeError = error.Message
                });
                return respuesta;
            }
        }

        public List<string[]> ConsultaEstadoVacaciones(string cedula, string nombre, string apellido1,
                                                            string apellido2, string seccion, string estado)
        {
            List<string[]> respuesta = new List<string[]>();

            try
            {
                List<PeriodoVacaciones> resultadosIntermedios = new List<PeriodoVacaciones>();
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                if (cedula != null && cedula != "")
                {
                    var datosBusqueda = intermedio.ConsultaEstadoVacaciones(resultadosIntermedios, "cedula", cedula);
                    if (datosBusqueda.Codigo > 0)
                    {
                        resultadosIntermedios = ((List<PeriodoVacaciones>)datosBusqueda.Contenido);
                    }
                }

                if (nombre != null && nombre != "")
                {
                    var datosBusqueda = intermedio.ConsultaEstadoVacaciones(resultadosIntermedios, "nombre", nombre);
                    if (datosBusqueda.Codigo > 0)
                    {
                        resultadosIntermedios = ((List<PeriodoVacaciones>)datosBusqueda.Contenido);
                    }
                }

                if (apellido1 != null && apellido1 != "")
                {
                    var datosBusqueda = intermedio.ConsultaEstadoVacaciones(resultadosIntermedios, "apellido1", apellido1);
                    if (datosBusqueda.Codigo > 0)
                    {
                        resultadosIntermedios = ((List<PeriodoVacaciones>)datosBusqueda.Contenido);
                    }
                }

                if (apellido2 != null && apellido2 != "")
                {
                    var datosBusqueda = intermedio.ConsultaEstadoVacaciones(resultadosIntermedios, "apellido2", apellido2);
                    if (datosBusqueda.Codigo > 0)
                    {
                        resultadosIntermedios = ((List<PeriodoVacaciones>)datosBusqueda.Contenido);
                    }
                }

                if (seccion != null && seccion != "")
                {
                    var datosBusqueda = intermedio.ConsultaEstadoVacaciones(resultadosIntermedios, "seccion", seccion);
                    if (datosBusqueda.Codigo > 0)
                    {
                        resultadosIntermedios = ((List<PeriodoVacaciones>)datosBusqueda.Contenido);
                    }
                }

                if (estado != null && estado != "")
                {
                    var datosBusqueda = intermedio.ConsultaEstadoVacaciones(resultadosIntermedios, "estado", estado);
                    if (datosBusqueda.Codigo > 0)
                    {
                        resultadosIntermedios = ((List<PeriodoVacaciones>)datosBusqueda.Contenido);
                    }
                }

                if (resultadosIntermedios.Count > 0)
                {

                    foreach (var item in resultadosIntermedios)
                    {
                        string datoEstado = "Indefinido";
                        if (resultadosIntermedios.Where(Q => Q.IndSaldo > 0 && Q.Nombramiento.Funcionario.IdCedulaFuncionario == item.Nombramiento.Funcionario.IdCedulaFuncionario).Count() == 1)
                        {
                            datoEstado = "Preventivo";
                        }
                        else
                        {
                            if (resultadosIntermedios.Where(Q => Q.IndSaldo > 0 && Q.Nombramiento.Funcionario.IdCedulaFuncionario == item.Nombramiento.Funcionario.IdCedulaFuncionario).Count() == 2)
                            {
                                datoEstado = "Crítico";
                            }
                            else
                            {
                                if (resultadosIntermedios.Where(Q => Q.IndSaldo < 0 && Q.Nombramiento.Funcionario.IdCedulaFuncionario == item.Nombramiento.Funcionario.IdCedulaFuncionario).Count() > 0)
                                {
                                    datoEstado = "Negativo";
                                }
                            }
                        }

                        string[] temp = new string[]
                        {
                            item.Nombramiento.Funcionario.IdCedulaFuncionario,
                            item.Nombramiento.Funcionario.NomFuncionario.TrimEnd() + " " + item.Nombramiento.Funcionario.NomPrimerApellido.TrimEnd() + " " + item.Nombramiento.Funcionario.NomSegundoApellido.TrimEnd(),
                            item.IndDiasDerecho.ToString(),
                            item.Nombramiento.Puesto.UbicacionAdministrativa.Seccion.NomSeccion,
                            item.IndPeriodo,
                            item.IndSaldo.ToString(),
                            datoEstado
                        };

                        respuesta.Add(temp);
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron resultados para los parámetros de búsqueda establecidos");
                }
            }
            catch (Exception error)
            {
                return new List<string[]> { new string[] { error.Message } };
            }
        }

        public CBaseDTO ActualizarDatosVacaciones(int idregistro, string cambio, decimal saldo)
        {
            try
            {
                CPeriodoVacacionesD intermedio = new CPeriodoVacacionesD(contexto);

                var resultado = intermedio.ActualizarDatosVacaciones(idregistro, cambio, saldo);

                if (resultado.Codigo > 0)
                {
                    return new CBaseDTO { Mensaje = "Listo" };
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

        public List<CBaseDTO> BuscarHistorialMovimientoVacaciones(string cedula, string periodo)
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPeriodoVacacionesD intemedio = new CPeriodoVacacionesD(contexto);

                var expediente = intemedio.BuscarHistorialMovimientoVacaciones(cedula,periodo);

                if (expediente.Codigo > 0)
                {
                    foreach (var item in ((List<C_EMU_Vacaciones_Movimiento>)expediente.Contenido))
                    {
                        respuesta.Add(ConvertirC_EMU_Vacaciones_MovimientoADTO(item));
                    }

                }
                else
                {
                    respuesta.Add((CErrorDTO)expediente.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return respuesta;
        }

        public List<CBaseDTO> BuscarHistorialPeriodoVacacionesSinActuales(string cedula, List<string> actuales)
        {

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<CRegistroVacacionesDTO> resultados = new List<CRegistroVacacionesDTO>();
            try
            {
                CPeriodoVacacionesD intemedio = new CPeriodoVacacionesD(contexto);

                var expediente = intemedio.BuscarHistorialPeriodoVacacionesSinActuales(cedula, actuales);

                if (expediente.Codigo > 0)
                {
                    foreach (var item in ((List<C_EMU_Vacaciones_Movimiento>)expediente.Contenido))
                    {
                        resultados.Add(ConvertirC_EMU_Vacaciones_MovimientoADTO(item));
                    }

                    var listaPeriodos = resultados.Select(R => R.Periodo.Periodo).Distinct();

                    foreach (var item in listaPeriodos)
                    {
                        respuesta.Add(new CPeriodoVacacionesDTO
                        {
                            Periodo = item,
                            Saldo = Convert.ToDouble(resultados.Where(R => R.Periodo.Periodo == item && R.TipoTransaccion != 99).Sum(R => R.Dias))
                        });
                    }

                }
                else
                {
                    respuesta.Add((CErrorDTO)expediente.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
            }

            return respuesta;
        }
    }
}
