using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleAsignacionGastoTrasporteModificadaD
    {
		#region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDetalleAsignacionGastoTrasporteModificadaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de guardar en base de datos un Detalle de Asiganacion de Gasto de Trasporte(detalles de ruta) Modificada
        /// cuando se hacen cambios en una ruta que ya se había creado para el gasto de transporte.
        /// </summary>
        /// <param name="detAsigGTModif"></param>
        /// <param name="idGastoTransp"></param>
        /// <returns>Un objeto de tipo 'DetalleAsiganacionGastoTrasporteModificada' como respuesta DTO</returns>
        public CRespuestaDTO AgregarDetalleAsignacionGTModificada(DetalleAsignacionGastoTransporteModificada detAsigGTModif, int idGastoTransp)
        {
            CRespuestaDTO respuesta;
            try
            {
                var GastoTransporte = entidadBase.GastoTransporte
                    .Where(D => D.PK_GastosTransporte == idGastoTransp).FirstOrDefault();

                if (GastoTransporte != null)
                {
                    detAsigGTModif.GastoTransporte = GastoTransporte;
                    entidadBase.DetalleAsignacionGastoTransporteModificada.Add(detAsigGTModif);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detAsigGTModif
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Error al agregar el detalle de asignación(rutas) modificada del gasto de transporte");
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
        /// Obtener de la bd una lista con los detalles de asignación modificada del gasto de transporte especificado.
        /// </summary>
        /// <param name="Codigo">PK del gasto de transporte.</param>
        /// <returns></returns>
        public List<DetalleAsignacionGastoTransporteModificada> ListarAsignacionModificada(int idgasto)
        {
            List<DetalleAsignacionGastoTransporteModificada> resultado = new List<DetalleAsignacionGastoTransporteModificada>();

            try
            {
                var datos = entidadBase.DetalleAsignacionGastoTransporteModificada.Where(C => C.GastoTransporte.PK_GastosTransporte == idgasto).ToList();
                if (datos != null)
                {
                    resultado = datos;
                    return resultado.OrderByDescending(Q => Q.PK_DetalleAsignacionGastoModificada).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron detalles de asignación modificadas, asociadas al gasto de transporte indicado.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }


        /// <summary>
        /// Hace update en la tabla DETALLEASIGNACIONMODIFICADA cuando quere insertar 
        /// una ruta modificada que ya se ha agregado antes
        /// </summary>
        /// <param name="idDetalle">PK del detalleAsignacionModificada de transporte</param>
        /// <param name="newTarifa">El nuevo valor total del gasto</param>
        /// <returns></returns>
        public CRespuestaDTO ActualizarTarifaRutaModificada(int idDetalle, string newTarifa)
        {
            CRespuestaDTO respuesta;
            try
            {
                var DetalleRutaOld = entidadBase.DetalleAsignacionGastoTransporteModificada
                                    .FirstOrDefault(D => D.PK_DetalleAsignacionGastoModificada == idDetalle);

                //var gastoEnModificadas = entidadBase.DetalleAsignacionGastoTransporteModificada.Where(d => d.FK_GastoTransporte == idgasto);

                if (DetalleRutaOld != null)
                {
                    DetalleRutaOld.MontTarifa = newTarifa;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Mensaje = "No se encontró el Detalle de Asignación Modificada requerido",
                        Contenido = new CErrorDTO { Codigo = -1, MensajeError = "No se encontró el Detalle de Asignación Modificada requerido" }

                    };
                    return respuesta;
                    throw new Exception(respuesta.Mensaje);
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        #endregion
    }
}
