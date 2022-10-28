using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleDeduccionViaticoCorridoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CDetalleDeduccionViaticoCorridoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo Detalle Deduccion Viatico Corrido como respuesta DTO
        /// </summary>
        /// <param name="detalleDVC"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarDetalleDeduccionViaticoCorrido(DetalleDeduccionViaticoCorrido detalleDVC,  MovimientoViaticoCorrido movimientoVC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoMViatico = entidadBase.MovimientoViaticoCorrido
                    .Where(VC => VC.PK_MovimientoViaticoCorrido == movimientoVC.PK_MovimientoViaticoCorrido).FirstOrDefault();
                if (datoMViatico != null)
                {
                    entidadBase.DetalleDeduccionViaticoCorrido.Add(detalleDVC);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleDVC
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
        /// metodo encargado de Obtener una deduccion de viatico corrido
        /// </summary>
        /// <returns></returns>
        public CRespuestaDTO ObtenerDeduccionViaticoCorrido(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.DetalleDeduccionViaticoCorrido.Where(C => C.MovimientoViaticoCorrido.PK_MovimientoViaticoCorrido == codigo).ToList();
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
