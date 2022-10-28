using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleEliminacionViaticoCorridoGastoTransporteD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CDetalleEliminacionViaticoCorridoGastoTransporteD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo Detalle Eliminacion Viatico Corrido como respuesta DTO
        /// </summary>
        /// <param name="detalleDVC"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarDetalleEliminacionViaticoCorrido(DetalleEliminacionViaticoCorridoGastoTransporte detalleEVCGT, MovimientoViaticoCorrido movimientoVC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoMViatico = entidadBase.MovimientoViaticoCorrido
                    .Where(VC => VC.PK_MovimientoViaticoCorrido == movimientoVC.PK_MovimientoViaticoCorrido).FirstOrDefault();
                if (datoMViatico != null)
                {
                    entidadBase.DetalleEliminacionViaticoCorridoGastoTransporte.Add(detalleEVCGT);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleEVCGT
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Error movimientoViaticoCorrido");
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
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo Detalle Eliminacion Gasto Transporte como respuesta DTO
        /// </summary>
        /// <param name="detalleDVC"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarDetalleEliminacionGastoTransporte(DetalleEliminacionViaticoCorridoGastoTransporte detalleEVCGT, MovimientoGastoTransporte movimientoGT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoMGasto = entidadBase.MovimientoGastoTransporte
                    .Where(GT => GT.PK_MovimientoGastosTransporte == movimientoGT.PK_MovimientoGastosTransporte).FirstOrDefault();
                if (datoMGasto != null)
                {
                    entidadBase.DetalleEliminacionViaticoCorridoGastoTransporte.Add(detalleEVCGT);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleEVCGT
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Error movimientoViaticoCorrido");
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
        /// <summary>
        /// metodo encargado de optener una sola Eliminacion
        /// <param name="codigo"></param>
        /// <returns></returns>
        public CRespuestaDTO ObtenerEliminacion(string codigo, string tipo)
        {
            CRespuestaDTO respuesta;
            try
            {
                DetalleEliminacionViaticoCorridoGastoTransporte eliminacionVC = new DetalleEliminacionViaticoCorridoGastoTransporte ();
                DetalleEliminacionViaticoCorridoGastoTransporte eliminacionGT = new DetalleEliminacionViaticoCorridoGastoTransporte(); 
                var cod = int.Parse(codigo);
                if (tipo == "VC") {
                    eliminacionVC = entidadBase.DetalleEliminacionViaticoCorridoGastoTransporte.Include("MovimientoViaticoCorrido")
                                                  .FirstOrDefault(D => D.MovimientoViaticoCorrido.PK_MovimientoViaticoCorrido == cod);
                } else if (tipo == "GT") {
                    eliminacionGT = entidadBase.DetalleEliminacionViaticoCorridoGastoTransporte
                                                  .FirstOrDefault(D => D.MovimientoGastoTransporte.PK_MovimientoGastosTransporte == cod);
                }
                if (eliminacionVC.PK_DetalleEliminacionViaticoCorrido != 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = eliminacionVC,
                        Mensaje = "VC"

                    };
                    return respuesta;
                }
                else if(eliminacionGT.PK_DetalleEliminacionViaticoCorrido != 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = eliminacionGT,
                        Mensaje ="GT"

                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Movimiento de Eliminación");
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
        #endregion
    }
}
