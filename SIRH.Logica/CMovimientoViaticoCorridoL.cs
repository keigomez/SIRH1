using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CMovimientoViaticoCorridoL
    {
        # region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CMovimientoViaticoCorridoL()
        {
            contexto = new SIRHEntities();
        }

        internal static CMovimientoViaticoCorridoDTO ConvertirMovimientoDatosaDTO(MovimientoViaticoCorrido item)
        {
            List<CDetalleDeduccionViaticoCorridoDTO> lista = new List<CDetalleDeduccionViaticoCorridoDTO>();

            foreach (var deduc in item.DetalleDeduccionViaticoCorrido)
            {
                lista.Add(new CDetalleDeduccionViaticoCorridoDTO
                {
                    IdEntidad = deduc.PK_DetalleDeduccionViaticoCorrido,
                    FecRigeDTO = deduc.FecRige,
                    FecVenceDTO = deduc.FecVence,
                    DesMotivoDTO = deduc.DesMotivo,
                    NumNoDiaDTO = deduc.NumNoDias.HasValue ? Convert.ToInt16(deduc.NumNoDias): 0,
                    MontMontoBajarDTO= deduc.MontMontoBajar,
                    MontMontoRebajarDTO = deduc.MontMontoRebajarConp,
                    TotRebajarDTO = deduc.TotRebajar,
                    NumSolicitudAccionPDTO = deduc.NumAccionPersonal != null ? deduc.NumAccionPersonal : "0"
                });
            }


            return new CMovimientoViaticoCorridoDTO
            {
                ViaticoCorridoDTO = CViaticoCorridoL.ConvertirViaticoCorridoDatosaDTO(item.ViaticoCorrido),
                FecMovimientoDTO = item.FecMovimiento,
                Nomtipo = Convert.ToInt32(item.NumTipo),
                ObsObservacionesDTO = item.ObsObservacion,
                IdEntidad = item.PK_MovimientoViaticoCorrido,
                EstadoDTO = Convert.ToInt32(item.IndEstado),
                Deducciones = lista
            };
        }

        internal static MovimientoViaticoCorrido ConvertirMovimientoDTOaDatos(CMovimientoViaticoCorridoDTO item)
        {
            return new MovimientoViaticoCorrido
            {
                ViaticoCorrido = CViaticoCorridoL.ConvertirViaticoCorridoDTOaDatos(item.ViaticoCorridoDTO),
                FecMovimiento = item.FecMovimientoDTO,
                NumTipo = item.Nomtipo,
                ObsObservacion = item.ObsObservacionesDTO,
                PK_MovimientoViaticoCorrido = item.IdEntidad,
                IndEstado = item.EstadoDTO
            };
        }

        #endregion
        #region Métodos
        /// <summary>
        /// Metodo encargado de guardar el movimiento de viatico corrido de tipo eliminación
        /// </summary>
        /// <param name="movimientoVC"></param>
        /// <returns></returns>
        public CBaseDTO AgregarMovimientoViaticoCorridoEliminacion(CMovimientoViaticoCorridoDTO movimientoVC)
        {
            CBaseDTO respuesta = new CBaseDTO();

            // foreach 

            // si no tira error

            // guardado exitoso, envia respuesta a siguiente capa
            try
            {

                //instancia a datos de CCalificacionNombramientoD
                CMovimientoViaticoCorridoD intermedioMovimiento = new CMovimientoViaticoCorridoD(contexto);
                // inserta en CcalificacionNombramientoD
                MovimientoViaticoCorrido datoMovimientoBD = new MovimientoViaticoCorrido
                {
                    NumTipo = movimientoVC.Nomtipo,
                    FecMovimiento = movimientoVC.FecMovimientoDTO,
                    ObsObservacion = movimientoVC.ObsObservacionesDTO,
                    PK_MovimientoViaticoCorrido = movimientoVC.IdEntidad,
                    IndEstado = movimientoVC.EstadoDTO
                };
                var insertaMVC = intermedioMovimiento.AgregarMovimientoViaticoCorrido(datoMovimientoBD);

                //pregunto si da error
                if (insertaMVC.Codigo > 0)
                {
                    respuesta = ConvertirMovimientoDatosaDTO((MovimientoViaticoCorrido)insertaMVC.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta = (((CErrorDTO)((CRespuestaDTO)respuesta).Contenido));
            }

            return respuesta;
        }


        /// <summary>
        /// Metodo encargado de anular viatico corrido
        /// </summary>
        /// <param name="mViaticoC"></param>
        /// <returns></returns>
        public CBaseDTO AnularMovimientoViaticoCorrido(CMovimientoViaticoCorridoDTO mViaticoC)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoViaticoCorridoD intermedio = new CMovimientoViaticoCorridoD(contexto);
                var mViaticoCorridoDB = new MovimientoViaticoCorrido
                {
                    PK_MovimientoViaticoCorrido = mViaticoC.IdEntidad,
                };
                var datosMovimientoViaticoCorrido = intermedio.AnularMovimientoViaticoCorrido(mViaticoCorridoDB);
                if (datosMovimientoViaticoCorrido.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = mViaticoC.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosMovimientoViaticoCorrido.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        /// <summary>
        /// encargado de obtener un Movimiento viatico corrido en especifico
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public CBaseDTO ObtenerMovimientoViaticoCorrido(string codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoViaticoCorridoD intermedio = new CMovimientoViaticoCorridoD(contexto);

                var viaticoC = intermedio.ObtenerMovimientoViaticoCorrido(codigo);

                if (viaticoC.Codigo > 0)
                {
                    var datoMovimientoViaticoCorrido = ConvertirMovimientoDatosaDTO((MovimientoViaticoCorrido)viaticoC.Contenido);
                    respuesta = datoMovimientoViaticoCorrido; //0 - 0
                }
                else
                {
                    respuesta=((CErrorDTO)viaticoC.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO { Codigo = -1, MensajeError = error.Message });
            }
            return respuesta;
        }

        public CBaseDTO ObtenerMovimientoViaticoCorridoFecha(int idViatico, DateTime fecha)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoViaticoCorridoD intermedio = new CMovimientoViaticoCorridoD(contexto);

                var viaticoC = intermedio.ObtenerMovimientoViaticoCorridoFecha(idViatico.ToString());

                if (viaticoC.Codigo > 0)
                {
                    var datoMovimientoViaticoCorrido = ConvertirMovimientoDatosaDTO((MovimientoViaticoCorrido)viaticoC.Contenido);
                    respuesta = datoMovimientoViaticoCorrido; //0 - 0
                }
                else
                {
                    respuesta = ((CErrorDTO)viaticoC.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO { Codigo = -1, MensajeError = error.Message });
            }
            return respuesta;
        }

        /// <summary>
        /// Metodo encargado de hacerle logica a la busqueda de movimientos segun el tipo
        /// </summary>
        /// <param name="funcionario"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public List<List<CBaseDTO>> BuscarMovimientoViaticoCorrido(CFuncionarioDTO funcionario,string codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultadoFuncionario = intermedio.PruebaBuscarFuncionarioCedula(funcionario.Cedula);
                if (resultadoFuncionario.Codigo != -1)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral((Funcionario)resultadoFuncionario.Contenido) });

                    CMovimientoViaticoCorridoD intermedioMVC = new CMovimientoViaticoCorridoD(contexto);
                    var datosFuncionario = new Funcionario
                    {
                        IdCedulaFuncionario = funcionario.Cedula
                    };

                    if (codigo != "4")  // Reintegros
                    {
                        var movimientos = intermedioMVC.BuscarMovimientoViaticoCorrido(datosFuncionario, codigo);
                        if (movimientos != null)
                        {
                            List<CBaseDTO> movimientosdata = new List<CBaseDTO>();
                            foreach (var item in movimientos)
                            {
                                movimientosdata.Add(new CMovimientoViaticoCorridoDTO
                                {
                                    IdEntidad = item.PK_MovimientoViaticoCorrido,
                                    EstadoDTO = Convert.ToInt32(item.IndEstado),
                                    FecMovimientoDTO = item.FecMovimiento,
                                    Nomtipo = Convert.ToInt32(item.NumTipo),
                                    ObsObservacionesDTO = item.ObsObservacion,
                                    ViaticoCorridoDTO = CViaticoCorridoL.ConvertirViaticoCorridoDatosaDTO(item.ViaticoCorrido)
                                });
                            }
                            respuesta.Add(movimientosdata);
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> { new CMovimientoViaticoCorridoDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
                        }

                    }
                    else // Reintegros
                    {
                        var movimientos = intermedioMVC.BuscarSolicitudReintegro(datosFuncionario);
                        if (movimientos != null)
                        {
                            List<CBaseDTO> movimientosdata = new List<CBaseDTO>();
                            foreach (var item in movimientos)
                            {
                                movimientosdata.Add(new CViaticoCorridoReintegroDTO
                                {
                                    IdEntidad = item.PK_Reintegro,
                                    EstadoDTO = Convert.ToInt32(item.IndEstado),
                                    FecDiaDTO = item.FecDia,
                                    MonReintegroDTO = Convert.ToDecimal(item.MonReintegro),
                                    ObsMotivoDTO = item.ObsMotivo,
                                    ViaticoCorridoDTO = CViaticoCorridoL.ConvertirViaticoCorridoDatosaDTO(item.ViaticoCorrido)
                                });
                            }
                            respuesta.Add(movimientosdata);
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> { new CViaticoCorridoReintegroDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
                        }
                    }
                   
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }

            return respuesta;
        }

        public CBaseDTO EditarReintegro(int idReintegro, int estado)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoViaticoCorridoD intermedio = new CMovimientoViaticoCorridoD(contexto);
                var datosMov = intermedio.EditarReintegro(idReintegro,estado);
                if (datosMov.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = idReintegro
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosMov.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }

        #endregion
    }
}
