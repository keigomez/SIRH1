using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CFactorD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();
        
        #endregion

        #region Constructor

        public CFactorD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos     
        
        //guardar el factor       
        public CRespuestaDTO GuardarFactor(int idTituloFactor, Factor factor)
        {
            CRespuestaDTO respuesta;
            try
            {
                var tituloFactor = entidadBase.TituloFactor.Include("Factor")
                                    .Where(N => N.PK_TituloFactor == idTituloFactor).FirstOrDefault();


                if (tituloFactor != null)
                {
                    //de tituloFactor a factor, se agrega el factor
                    tituloFactor.Factor.Add(factor);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO

                    {
                        Codigo = 1,
                        Contenido = tituloFactor
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún Factor dentro del Título de Factor");
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

        public CRespuestaDTO BuscarFactorId(int idFactor)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = entidadBase.Factor.Where(A => A.PK_Factor == idFactor).FirstOrDefault();

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
                    throw new Exception("No se encontró ningún factor solicitado");
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
