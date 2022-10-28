using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTituloFactorD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();
        
        #endregion

        #region Constructor

        public CTituloFactorD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos
        
        public CRespuestaDTO GuardarTituloFactor(TituloFactor tituloFactor)
        {
            CRespuestaDTO respuesta;
            try 
            {
                entidadBase.TituloFactor.Add(tituloFactor);
                entidadBase.SaveChanges();
                
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = tituloFactor

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

        public CRespuestaDTO BuscarTituloFactorId(int idTituloFactor)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.TituloFactor.Where(T => T.PK_TituloFactor == idTituloFactor).FirstOrDefault();

                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidadBase
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún Título de Factor");
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
        //Me trae la lista de los titulos factor del código de puesto.
        public CRespuestaDTO BuscarTitulosFactorPorPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                //al traerme una lista, hago foreach en Logica
                var tituloFactor = entidadBase.Puesto.Include("Factor")
                                                     .Include("CaracteristicasPuesto").Include("TituloFactor")
                                                     .Where(P => P.CodPuesto == codPuesto).ToList(); 
                                              
                if (tituloFactor != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tituloFactor
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("no se encontraron titulos de factor en el puesto solicitado");
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
