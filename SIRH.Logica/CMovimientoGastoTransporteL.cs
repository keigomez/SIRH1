using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CMovimientoGastoTransporteL
    {
        # region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CMovimientoGastoTransporteL()
        {
            contexto = new SIRHEntities();
        }

        #endregion
        #region Métodos
        internal static CMovimientoGastoTransporteDTO ConvertirMovimientoGastoTransporteDTOaDatos(MovimientoGastoTransporte item)
        {
            List<CDetalleDeduccionGastoTransporteDTO> lista = new List<CDetalleDeduccionGastoTransporteDTO>();

            foreach (var deduc in item.DetalleDeduccionGastoTransporte)
            {
                lista.Add(new CDetalleDeduccionGastoTransporteDTO
                {
                    IdEntidad = deduc.PK_DetalleDeduccionGastoTransporte,
                    FecRigeDTO = deduc.FecRige,
                    FecVenceDTO = deduc.FecVence,
                    DesMotivoDTO = deduc.DesMotivo,
                    NumNoDiaDTO = deduc.NumNoDias.HasValue ? Convert.ToInt16(deduc.NumNoDias) : 0,
                    MontMontoBajarDTO = deduc.MontMontoBajar,
                    MontMontoRebajarDTO = deduc.MontMontoRebajarConp,
                    TotRebajarDTO = deduc.TotRebajar,
                    NumSolicitudAccionPDTO = deduc.NumAccionPersonal != null ? deduc.NumAccionPersonal : "0"
                });
            }

            return new CMovimientoGastoTransporteDTO
            {
                GastoTransporteDTO = new CGastoTransporteDTO { IdEntidad = item.FK_GastosTransporte },
                Nomtipo = Convert.ToInt32(item.NumTipo),
                IdEntidad = item.PK_MovimientoGastosTransporte,
                ObsObservacionesDTO = item.ObsObservacion,
                EstadoDTO = Convert.ToInt32(item.IndEstado),
                Deducciones = lista
            };
        }
        internal static MovimientoGastoTransporte ConvertirMovimientoGastoTransporteDatosaDTO(CMovimientoGastoTransporteDTO item)
        {
            return new MovimientoGastoTransporte
            {
                GastoTransporte = CGastoTransporteL.ConvertirGastoTransporteDTOaDatos(item.GastoTransporteDTO),
                //DetalleDeduccionGastoTransporte = CDetalleDeduccionGastoTransporteL.ConvertirMovimientoGastoTransporteDatosaDTO(item.DetalleDeduccionGastoTransporteDTO),
                 NumTipo = item.Nomtipo,
                PK_MovimientoGastosTransporte = item.IdEntidad,
                ObsObservacion = item.ObsObservacionesDTO
            };
        }
        /// <summary>
        /// Metodo encargado de anular viatico corrido
        /// </summary>
        /// <param name="mGastoT"></param>
        /// <returns></returns>
        public CBaseDTO AnularMovimientoGastoTransporte(CMovimientoGastoTransporteDTO mGastoT)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoGastoTransporteD intermedio = new CMovimientoGastoTransporteD(contexto);
                var mViaticoCorridoDB = new MovimientoGastoTransporte
                {
                    PK_MovimientoGastosTransporte = mGastoT.IdEntidad,
                };
                var datosMovimientoGastoTransporte = intermedio.AnularMovimientoGastoTransporte(mViaticoCorridoDB);
                if (datosMovimientoGastoTransporte.Codigo > 0)
                {
                    respuesta = new CBaseDTO
                    {
                        IdEntidad = mGastoT.IdEntidad
                    };
                }
                else
                {
                    respuesta = (CErrorDTO)datosMovimientoGastoTransporte.Contenido;
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = error.Message };
            }
            return respuesta;
        }
        /// <summary>
        /// encargado de obtener un Movimiento gasto corrido en especifico
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>

        public CBaseDTO ObtenerMovimientoGastoTransporte(string codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CMovimientoGastoTransporteD intermedio = new CMovimientoGastoTransporteD(contexto);

                var gastoT = intermedio.ObtenerMovimientoGastoTransporte(codigo);

                if (gastoT.Codigo > 0)
                {
                    var datoMovimientoGastoTrasnporte = ConvertirMovimientoGastoTransporteDTOaDatos((MovimientoGastoTransporte)gastoT.Contenido);
                    respuesta = datoMovimientoGastoTrasnporte; //0 - 0
                }
                else
                {
                    respuesta = ((CErrorDTO)gastoT.Contenido);
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
        public List<List<CBaseDTO>> BuscarMovimientoGastoTransporte(CFuncionarioDTO funcionario, string codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CFuncionarioD intermedio = new CFuncionarioD(contexto);
                var resultadoFuncionario = intermedio.PruebaBuscarFuncionarioCedula(funcionario.Cedula);
                if (resultadoFuncionario.Codigo != -1)
                {
                    respuesta.Add(new List<CBaseDTO> { CFuncionarioL.FuncionarioGeneral((Funcionario)resultadoFuncionario.Contenido) });
                    CMovimientoGastoTransporteD intermedioMGT = new CMovimientoGastoTransporteD(contexto);
                    var datosFuncionario = new Funcionario
                    {
                        IdCedulaFuncionario = funcionario.Cedula
                    };

                    if (codigo != "4")  // Reintegros
                    {
                        var movimientos = intermedioMGT.BuscarMovimientoGastoTransporte(datosFuncionario, codigo);
                        if (movimientos != null)
                        {
                            List<CBaseDTO> movimientosdata = new List<CBaseDTO>();
                            foreach (var item in movimientos)
                            {
                                movimientosdata.Add(new CMovimientoGastoTransporteDTO
                                {
                                    IdEntidad = item.PK_MovimientoGastosTransporte,
                                    FecMovimientoDTO = item.FecMovimiento,
                                    EstadoDTO = Convert.ToInt32(item.IndEstado),
                                    Nomtipo = Convert.ToInt32(item.NumTipo),
                                    ObsObservacionesDTO = item.ObsObservacion,
                                    GastoTransporteDTO = CGastoTransporteL.ConvertirGastoTransporteDatosaDTO(item.GastoTransporte)
                                });
                            }
                            respuesta.Add(movimientosdata);
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> { new CMovimientoGastoTransporteDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
                        }
                    }
                    else // Reintegros
                    {
                        var movimientos = intermedioMGT.BuscarSolicitudReintegro(datosFuncionario);
                        if (movimientos != null)
                        {
                            List<CBaseDTO> movimientosdata = new List<CBaseDTO>();
                            foreach (var item in movimientos)
                            {
                                movimientosdata.Add(new CGastoTransporteReintegroDTO
                                {
                                    IdEntidad = item.PK_Reintegro,
                                    FecDiaDTO = item.FecDia,
                                    EstadoDTO = Convert.ToInt32(item.IndEstado),
                                    MonReintegroDTO = Convert.ToDecimal(item.MonReintegro),
                                    ObsMotivoDTO = item.ObsMotivo,
                                    GastoTransporteDTO = CGastoTransporteL.ConvertirGastoTransporteDatosaDTO(item.GastoTransporte)
                                });
                            }
                            respuesta.Add(movimientosdata);
                        }
                        else
                        {
                            respuesta.Add(new List<CBaseDTO> { new CGastoTransporteReintegroDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } });
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
                CMovimientoGastoTransporteD intermedio = new CMovimientoGastoTransporteD(contexto);
                var datosMov = intermedio.EditarReintegro(idReintegro, estado);
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
