using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleAsignacionGastoTrasporteD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CDetalleAsignacionGastoTrasporteD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargador de guardar en base de datos un Detalle de Asiganacion de Gasto de Trasporte y restorna un objeto de tipo DetalleAsiganacionGastoTrasporte como respuesta DTO
        /// </summary>
        /// <param name="detalleAGT"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarDetalleAsignacionGastoTransporte(DetalleAsignacionGastoTransporte detalleAGT, int idRuta)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporteRuta = entidadBase.GastoTransporteRutas
                    .Where(D => D.PK_Ruta == idRuta).FirstOrDefault();

                if (GastoTransporteRuta != null)
                {
                    detalleAGT.GastoTransporteRutas = GastoTransporteRuta;
                    entidadBase.DetalleAsignacionGastoTransporte.Add(detalleAGT);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleAGT
                    };
                }
                else
                {
                    throw new Exception("El Gasto Transporte");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Codigo"></param>
        /// <returns></returns>
        public List<DetalleAsignacionGastoTransporte> ListarAsignacion(int codigo)
        {
            List<DetalleAsignacionGastoTransporte> resultado = new List<DetalleAsignacionGastoTransporte>();

            try
            {
                var datos= entidadBase.DetalleAsignacionGastoTransporte.Where(C => C.GastoTransporteRutas.GastoTransporte.PK_GastosTransporte == codigo).ToList();
                if (datos != null)
                {
                    resultado = datos;
                    return resultado.OrderByDescending(Q => Q.PK_DetalleAsignacionGastoTransporte).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron Preguntas asociadas al tipoFormulario.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }
        #endregion
    }
}
