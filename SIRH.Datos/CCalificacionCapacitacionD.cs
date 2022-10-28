using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{//
    public class CCalificacionCapacitacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCalificacionCapacitacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos

        public CRespuestaDTO AgregarCapacitacion(CalificacionCapacitacion capacitacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.CalificacionCapacitacion.Add(capacitacion);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = capacitacion
                };
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

        public CRespuestaDTO ConsultarCapacitacion(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var capacitacion = entidadBase.CalificacionCapacitacion.Where(Q => Q.PK_CalificacionCapacitacion == codigo).FirstOrDefault();

                if (capacitacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = capacitacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningún registro de Capacitación" }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        public CRespuestaDTO EditarCapacitacion(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var capacitacion = entidadBase.CalificacionCapacitacion.Where(Q => Q.PK_CalificacionCapacitacion == codigo).FirstOrDefault();

                if (capacitacion != null)
                {
                    capacitacion.IndEstado = 2;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = capacitacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningún registro de Capacitación" }
                    };
                    return respuesta;
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }

        #endregion
    }
}
