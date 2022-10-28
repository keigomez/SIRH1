using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
    public class CEstadoMovimientoPuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CEstadoMovimientoPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion

        #region Metodos

        public CRespuestaDTO BuscarEstadoMovimientoPuesto(int codEstadoMovimientoPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = entidadBase.EstadoMovimientoPuesto.Where(E => E.PK_EstadoMovimientoPuesto == codEstadoMovimientoPuesto).FirstOrDefault();
               
                if (datosEntidad != null)
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
                    throw new Exception("No se encontró ningún Estado de Movimiento de Puesto");
                }
            }
            catch(Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta; 
            }
        }

        public EstadoMovimientoPuesto CargarEstadoMovimientoPorPuesto(string NumeroPuesto)
        {
            EstadoMovimientoPuesto resultado = new EstadoMovimientoPuesto();

            resultado = entidadBase.EstadoMovimientoPuesto.Where(R => R.MovimientoPuesto.Where(P => P.Puesto.CodPuesto == NumeroPuesto).Count() > 0).FirstOrDefault();

            return resultado;
        }

        public CRespuestaDTO GuardarEstadoMovimientoPuesto(string puesto,EstadoMovimientoPuesto estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.EstadoMovimientoPuesto.Add(estado);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = estado
                };
                return respuesta;
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

        public CRespuestaDTO ListarEstadosMovimientoPuesto()
        {
            CRespuestaDTO respuesta;
            
            try
            {
                var datosEntidad = entidadBase.EstadoMovimientoPuesto.ToList();
                if(datosEntidad != null)
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
                    throw new Exception("No se encontraron estados de movimiento de puesto");
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
