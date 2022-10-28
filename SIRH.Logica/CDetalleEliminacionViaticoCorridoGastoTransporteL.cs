using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleEliminacionViaticoCorridoGastoTransporteL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region constructor

        public CDetalleEliminacionViaticoCorridoGastoTransporteL()
        {
            contexto = new SIRHEntities();
        }

        #endregion
        #region Métodos
        internal static CDetalleEliminacionViaticoCorridoGastoTransporteDTO ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTO(DetalleEliminacionViaticoCorridoGastoTransporte item)
        {

            return new CDetalleEliminacionViaticoCorridoGastoTransporteDTO
            {

             IdEntidad= item.PK_DetalleEliminacionViaticoCorrido,
             MovimientoViaticoCorridoDTO = CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO(item.MovimientoViaticoCorrido),
             MovimientoGastoTransporteDTO = CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos(item.MovimientoGastoTransporte),
             FecInicioDTO = Convert.ToDateTime(item.FecInicio),
             FecFinDTO = Convert.ToDateTime(item.FecFinal),
             ObsJustificacionDTO = item.ObsJustificacion

            };
        }
        internal static DetalleEliminacionViaticoCorridoGastoTransporte ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDTOaDatos(CDetalleEliminacionViaticoCorridoGastoTransporteDTO item)
        {

            return new DetalleEliminacionViaticoCorridoGastoTransporte
            {

                PK_DetalleEliminacionViaticoCorrido = item.IdEntidad ,
                MovimientoViaticoCorrido = CMovimientoViaticoCorridoL.ConvertirMovimientoDTOaDatos(item.MovimientoViaticoCorridoDTO),
                MovimientoGastoTransporte = CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDatosaDTO(item.MovimientoGastoTransporteDTO),
                FecInicio = Convert.ToDateTime(item.FecInicioDTO),
                FecFinal = Convert.ToDateTime(item.FecFinDTO),
                ObsJustificacion = item.ObsJustificacionDTO

            };
        }
        internal static CDetalleEliminacionViaticoCorridoGastoTransporteDTO ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTOVC(DetalleEliminacionViaticoCorridoGastoTransporte item)
        {

            return new CDetalleEliminacionViaticoCorridoGastoTransporteDTO
            {

                IdEntidad = item.PK_DetalleEliminacionViaticoCorrido,
                MovimientoViaticoCorridoDTO = CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO(item.MovimientoViaticoCorrido),
                FecInicioDTO = Convert.ToDateTime(item.FecInicio),
                FecFinDTO = Convert.ToDateTime(item.FecFinal),
                ObsJustificacionDTO = item.ObsJustificacion

            };
        }
        internal static CDetalleEliminacionViaticoCorridoGastoTransporteDTO ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTOGT(DetalleEliminacionViaticoCorridoGastoTransporte item)
        {

            return new CDetalleEliminacionViaticoCorridoGastoTransporteDTO
            {

                IdEntidad = item.PK_DetalleEliminacionViaticoCorrido,
                MovimientoGastoTransporteDTO = CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos(item.MovimientoGastoTransporte),
                FecInicioDTO = Convert.ToDateTime(item.FecInicio),
                FecFinDTO = Convert.ToDateTime(item.FecFinal),
                ObsJustificacionDTO = item.ObsJustificacion

            };
        }
        /// <summary>
        /// Metodo encargado de agregar un formulario de eliminacion a la base de datos de viatico corrido
        /// </summary>
        /// <param name="detalleEVC"></param>
        /// <param name="movimientoVC"></param>
        /// <returns></returns>
        public List<CBaseDTO> AgregarDetalleEliminacionViaticoCorrido(CDetalleEliminacionViaticoCorridoGastoTransporteDTO detalleEVC,
                                                              CMovimientoViaticoCorridoDTO movimientoVC)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            // foreach 

            // si no tira error

            // guardado exitoso, envia respuesta a siguiente capa
            try
            {

                //instancia a datos de CCalificacionNombramientoD
                CMovimientoViaticoCorridoD intermedioMovimientonVC = new CMovimientoViaticoCorridoD(contexto);
                // inserta en CcalificacionNombramientoD
                MovimientoViaticoCorrido datosMovimientonVCBD = new MovimientoViaticoCorrido
                {
                    NumTipo = movimientoVC.Nomtipo,
                    ObsObservacion = movimientoVC.ObsObservacionesDTO,
                    PK_MovimientoViaticoCorrido = movimientoVC.IdEntidad,
                    IndEstado = movimientoVC.EstadoDTO,
                    FecMovimiento = DateTime.Today
                };
                datosMovimientonVCBD.ViaticoCorrido = contexto.ViaticoCorrido.FirstOrDefault(Q => Q.PK_ViaticoCorrido == movimientoVC.ViaticoCorridoDTO.IdEntidad);
                var insertaMVC = intermedioMovimientonVC.AgregarMovimientoViaticoCorrido(datosMovimientonVCBD);

                //pregunto si da error
                if (insertaMVC.Codigo > 0)
                {
                    respuesta.Add(CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO((MovimientoViaticoCorrido)insertaMVC.Contenido));

                    // instancia de CDetalleCalificacionNombramientoD
                    CDetalleEliminacionViaticoCorridoGastoTransporteD intermedioVCGT = new CDetalleEliminacionViaticoCorridoGastoTransporteD(contexto);
                    // inserta CDetalleCalificacionNombramientoD
                    DetalleEliminacionViaticoCorridoGastoTransporte datosDetalleECVBD = new DetalleEliminacionViaticoCorridoGastoTransporte
                    {
                        PK_DetalleEliminacionViaticoCorrido = detalleEVC.IdEntidad,
                        FecInicio = detalleEVC.FecInicioDTO,
                        FecFinal = detalleEVC.FecFinDTO,
                        ObsJustificacion = detalleEVC.ObsJustificacionDTO

                    };
                    datosDetalleECVBD.MovimientoViaticoCorrido = contexto.MovimientoViaticoCorrido.FirstOrDefault(Q => Q.PK_MovimientoViaticoCorrido == ((MovimientoViaticoCorrido)insertaMVC.Contenido).PK_MovimientoViaticoCorrido);

                    var insertarDECVB = intermedioVCGT.AgregarDetalleEliminacionViaticoCorrido(datosDetalleECVBD,datosMovimientonVCBD);
                    // si no tira error
                    if (insertarDECVB.Codigo > 0)
                    {
                        respuesta.Add(CDetalleEliminacionViaticoCorridoGastoTransporteL.ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTOVC((DetalleEliminacionViaticoCorridoGastoTransporte)insertarDECVB.Contenido));
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)respuesta[1]).MensajeError);
                    }

                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
                }
            }
            catch (Exception error)
            {

                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));

            }

            return respuesta;

        }
        /// <summary>
        /// Metodo encargado de guardar una eliminacion de tipo GastoTransporte
        /// </summary>
        /// <param name="detalleEVC"></param>
        /// <param name="movimientoGT"></param>
        /// <returns></returns>
        public List<CBaseDTO> AgregarDetalleEliminacionGastoTransporte(CDetalleEliminacionViaticoCorridoGastoTransporteDTO detalleEVC,
                                                      CMovimientoGastoTransporteDTO movimientoGT)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            // foreach 

            // si no tira error

            // guardado exitoso, envia respuesta a siguiente capa
            try
            {

                //instancia a datos de CCalificacionNombramientoD
                CMovimientoGastoTransporteD intermedioMovimientonGT = new CMovimientoGastoTransporteD(contexto);
                // inserta en CcalificacionNombramientoD
                MovimientoGastoTransporte datosMovimientonGTBD = new MovimientoGastoTransporte
                {
                    NumTipo = movimientoGT.Nomtipo,
                    FecMovimiento = movimientoGT.FecMovimientoDTO,
                    ObsObservacion = movimientoGT.ObsObservacionesDTO,
                    PK_MovimientoGastosTransporte = movimientoGT.IdEntidad,
                    IndEstado = movimientoGT.EstadoDTO
                };
                datosMovimientonGTBD.GastoTransporte = contexto.GastoTransporte.FirstOrDefault(Q => Q.PK_GastosTransporte == movimientoGT.GastoTransporteDTO.IdEntidad);
                var insertaMGT = intermedioMovimientonGT.AgregarMovimientoGastoTransporte(datosMovimientonGTBD);

                //pregunto si da error
                if (insertaMGT.Codigo > 0)
                {
                    respuesta.Add(CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos((MovimientoGastoTransporte)insertaMGT.Contenido));

                    // instancia de CDetalleCalificacionNombramientoD
                    CDetalleEliminacionViaticoCorridoGastoTransporteD intermedioVCGT = new CDetalleEliminacionViaticoCorridoGastoTransporteD(contexto);
                    // inserta CDetalleCalificacionNombramientoD
                    DetalleEliminacionViaticoCorridoGastoTransporte datosDetalleECVBD = new DetalleEliminacionViaticoCorridoGastoTransporte
                    {
                        PK_DetalleEliminacionViaticoCorrido = detalleEVC.IdEntidad,
                        FecInicio = detalleEVC.FecInicioDTO,
                        FecFinal = detalleEVC.FecFinDTO,
                        ObsJustificacion = detalleEVC.ObsJustificacionDTO

                    };
                    datosDetalleECVBD.MovimientoGastoTransporte= contexto.MovimientoGastoTransporte.FirstOrDefault(Q => Q.PK_MovimientoGastosTransporte == ((MovimientoGastoTransporte)insertaMGT.Contenido).PK_MovimientoGastosTransporte);

                    var insertarDEGTB = intermedioVCGT.AgregarDetalleEliminacionGastoTransporte(datosDetalleECVBD, datosMovimientonGTBD);
                    // si no tira error
                    if (insertarDEGTB.Codigo > 0)
                    {
                        respuesta.Add(CDetalleEliminacionViaticoCorridoGastoTransporteL.ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTOGT((DetalleEliminacionViaticoCorridoGastoTransporte)insertarDEGTB.Contenido));
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)respuesta[1]).MensajeError);
                    }

                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
                }
            }
            catch (Exception error)
            {

                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));

            }

            return respuesta;

        }
        /// <summary>
        /// encargado de optener una sola Eliminacion de viatico corrido en especifico
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public CBaseDTO ObtenerEliminacion(string codigo, string tipo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CDetalleEliminacionViaticoCorridoGastoTransporteD intermedio = new CDetalleEliminacionViaticoCorridoGastoTransporteD(contexto);
                var datos = intermedio.ObtenerEliminacion(codigo, tipo);
                if (datos.Codigo > 0)
                {
                    if (datos.Mensaje == "VC")
                    {
                        respuesta = ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTOVC((DetalleEliminacionViaticoCorridoGastoTransporte)datos.Contenido);
                    }
                    else if (datos.Mensaje == "GT")
                    {
                        respuesta = ConvertirDatosCDetalleEliminacionViaticoCorridoGastoTransporteDatosaDTOGT((DetalleEliminacionViaticoCorridoGastoTransporte)datos.Contenido);
                    }
                }
                else
                {
                    respuesta = (CErrorDTO)datos.Contenido;
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
