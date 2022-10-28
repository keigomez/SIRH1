using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CRescisionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();
        
        #endregion

        #region Constructor

        public CRescisionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos

        public CRespuestaDTO BuscarRescisionPorNumRescision(string numRescision)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var rescision = entidadBase.Rescision.Include("PrestamoPuesto")
                                                     .Include("Puesto")   
                                                     .Where(R => R.NumRescision == numRescision).FirstOrDefault();

                if (rescision != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = rescision
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el número de rescisión");
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

        public CRespuestaDTO GuardarNumRescision(int idPrestamoPuesto, Rescision rescision)
        {
            CRespuestaDTO respuesta;
            try 
            {                
                var prestamoPuesto = entidadBase.PrestamoPuesto.Where(R => R.PK_PrestamoPuesto == idPrestamoPuesto).FirstOrDefault();

                if(prestamoPuesto != null)
                {
                    prestamoPuesto.Rescision.Add(rescision);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = rescision
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró la rescisión del préstamo especificado");
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
        #endregion
    }
}
