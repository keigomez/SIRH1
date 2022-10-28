using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;


namespace SIRH.Datos
{
    public class CAddendumPrestamoPuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CAddendumPrestamoPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
    
        #endregion

        #region Metodos

        public CRespuestaDTO BuscarAddendumPrestamoPuesto(string numAddendum)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var AddendumPrestamoPuesto = entidadBase.AddendumPrestamoPuesto.Where(A => A.NumAddendum == numAddendum).FirstOrDefault();

                if (AddendumPrestamoPuesto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = AddendumPrestamoPuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el número de addendum");
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

        public CRespuestaDTO BuscarAddendumIdPrestamo(int idPrestamoPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var PrestamoPuesto = entidadBase.PrestamoPuesto.Where(A => A.PK_PrestamoPuesto == idPrestamoPuesto).ToList();
                if (PrestamoPuesto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = PrestamoPuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron números de addendum en Préstamo de Puesto"); 
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
        // este Guardar es para cuando NO es una lista.
        public CRespuestaDTO GuardarNumAddendum(int idPrestamoPuesto, AddendumPrestamoPuesto addendumPrestamoPuesto) 
        {
            CRespuestaDTO respuesta;
            try
            {
               var prestamoPuesto = entidadBase.PrestamoPuesto.Include("Puesto").Include("AddendumPrestamoPuesto")
                                   .Where(N => N.PK_PrestamoPuesto == idPrestamoPuesto).FirstOrDefault();


                if(prestamoPuesto != null)
                {
                    //de prestamo puesto a addendumPrestamo, se agrega el addendumPrestamoPuesto
                    prestamoPuesto.AddendumPrestamoPuesto.Add(addendumPrestamoPuesto);                
                    entidadBase.SaveChanges(); 
                    
                    respuesta = new CRespuestaDTO
                
                    {
                        Codigo = 1,
                        Contenido = prestamoPuesto
                    };

                    return respuesta;
                }
                else 
                {
                    throw new Exception("No se encontró la resolución de préstamo especificada");
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
