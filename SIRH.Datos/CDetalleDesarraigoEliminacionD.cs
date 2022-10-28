using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using System.Globalization;

namespace SIRH.Datos
{
    public class CDetalleDesarraigoEliminacionD
    {
        #region Variables

        private SIRHEntities entidadesBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDetalleDesarraigoEliminacionD(SIRHEntities entidadGlobal)
        {
            entidadesBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        
        public CRespuestaDTO AgregarDetalleEliminacion(DetalleDesarraigoEliminacion detalle, Desarraigo desarraigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var desarraigoDB = entidadesBase.Desarraigo
                                            .Where( Q=> Q.PK_Desarraigo == desarraigo.PK_Desarraigo)
                                            .FirstOrDefault();
                if (desarraigoDB != null)
                {
                    desarraigoDB.FK_EstadoDesarraigo = 7; // Suspendido
                    detalle.Desarraigo = desarraigoDB;
                    entidadesBase.DetalleDesarraigoEliminacion.Add(detalle);
                    entidadesBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        IdEntidad = detalle.PK_Detalle
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró un Desarraigo válido para registrar la Eliminación");
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

        public CRespuestaDTO ModificarDetalleEliminacion(DetalleDesarraigoEliminacion detalle) {
            CRespuestaDTO respuesta;
            try {
                var detalleOld = entidadesBase.DetalleDesarraigoEliminacion
                                                 .FirstOrDefault(D => D.PK_Detalle == detalle.PK_Detalle);
                if (detalleOld != null)
                {
                    if (detalleOld.EstadoDesarraigo.PK_EstadoDesarraigo == 2) // "Espera"
                    {
                        detalleOld.FecEliminacion = detalle.FecEliminacion;
                        entidadesBase.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = detalle.PK_Detalle
                        };
                        return respuesta;
                    }
                    else {
                        throw new Exception("El desarraigo no es modificable");
                    }
                }
                else {
                    throw new Exception("No se encontró el desarraigo requerido");
                }
            }catch(Exception error){
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
       
        public CRespuestaDTO ObtenerDetalleEliminacion(int idDetalle)
        {
            CRespuestaDTO respuesta;
            try
            {
                var desarraigo = entidadesBase.DetalleDesarraigoEliminacion
                                                 .FirstOrDefault(D => D.PK_Detalle == idDetalle);

                if (desarraigo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = idDetalle,
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Detalle de la Eliminación");
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