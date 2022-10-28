using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CModalidadD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();
        
        #endregion

        #region Constructor
                
        public CModalidadD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }        

        #endregion

        #region Metodos

        public int GuardarModalidad(Modalidad modalidad)
        {
            entidadBase.Modalidad.Add(modalidad);
            return modalidad.PK_Modalidad;
        }

        public CRespuestaDTO GuardarModalidad(int idCursoCapacitacion, Modalidad modalidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Modalidad.Add(modalidad);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = modalidad
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

        public CRespuestaDTO BuscarModalidad(int codModalidad)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = entidadBase.Modalidad.Where(M => M.PK_Modalidad == codModalidad).FirstOrDefault();               

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
                    throw new Exception("No se encontró ninguna modalidad");
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


        public CRespuestaDTO ListarModalidad()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.Modalidad.ToList();

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
                    throw new Exception("No se encontraron modalidades");
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
