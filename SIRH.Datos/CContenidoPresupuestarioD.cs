using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CContenidoPresupuestarioD
    {
        #region Variables

        SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CContenidoPresupuestarioD(SIRHEntities ContextoGlobal)
        {
            entidadBase = ContextoGlobal;
        }
        #endregion

        #region Metodos

        public CRespuestaDTO GuardarContenidoPresupuestario(ContenidoPresupuestario contenido)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.ContenidoPresupuestario.Add(contenido); 
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = contenido
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

        public CRespuestaDTO ActualizarContenidoPresupuestario(DateTime FecVencimiento, int idContenido)
        {
            CRespuestaDTO respuesta;
            try
            {
                var contenido = entidadBase.ContenidoPresupuestario.Where(Q => Q.PK_ContenidoPresupuestario == idContenido);
                if (contenido != null)
                {
                    //Actualizar fecha vencimiento
                    //Actualizar fecha actualización
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = contenido
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el detalle de contenido prespuestario indicado");
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

        public CRespuestaDTO BuscarContenidoPresupuestario(string numResolucion)
        {
             CRespuestaDTO respuesta = new CRespuestaDTO();
             try
             {
                 var datosEntidad = entidadBase.ContenidoPresupuestario.Where(A => A.NumResolucion == numResolucion).FirstOrDefault();

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
                     throw new Exception("No se encontró el número de resolución de contenido presupuestario");
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

        public CRespuestaDTO BuscarNumResolucion(int idDetallePuesto, ContenidoPresupuestario contenidoPresupuestario)
        {
            CRespuestaDTO respuesta;
            try
            {
                var detallePuesto = entidadBase.DetallePuesto.Include("Puesto").Where(D => D.PK_DetallePuesto == idDetallePuesto).FirstOrDefault();


                if (detallePuesto != null)
                {
                    //de detalle de puesto a contenido presupuestario, se agrega el contenido presupuestario
                    detallePuesto.ContenidoPresupuestario = contenidoPresupuestario;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,                        
                        Contenido = detallePuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró la resolución de contenido presupuestario especificada");
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
