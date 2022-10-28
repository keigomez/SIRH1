using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleDeduccionGastoTransporteD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CDetalleDeduccionGastoTransporteD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo Detalle Deduccion Gasto Transporte como respuesta DTO
        /// </summary>
        /// <param name="detalleDT"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarDetalleDeduccionGastoTransporte(DetalleDeduccionGastoTransporte detalleDT, MovimientoGastoTransporte movimientoGT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoMGasto = entidadBase.MovimientoGastoTransporte
                    .Where(MG => MG.PK_MovimientoGastosTransporte == movimientoGT.PK_MovimientoGastosTransporte).FirstOrDefault();
                if (datoMGasto != null)
                {
                    entidadBase.DetalleDeduccionGastoTransporte.Add(detalleDT);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleDT
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Error movimientoGastoTransporte");
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
        /// metodo encargado de Obtener una deduccion de gasto transporte
        /// </summary>
        /// <returns></returns>
        public CRespuestaDTO ObtenerDeduccionGastoTransporte(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.DetalleDeduccionGastoTransporte.Where(C => C.MovimientoGastoTransporte.PK_MovimientoGastosTransporte == codigo).ToList();
                if (datosEntidad != null && datosEntidad.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró desarraigos");
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
