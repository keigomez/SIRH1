using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTareasPuestoD
    {
        #region Variables
        
        private SIRHEntities entidadBase = new SIRHEntities(); 

        #endregion

        #region Constructor

        public CTareasPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos
        //buscar tareas por el codigo de puesto
        public CRespuestaDTO BuscarTareasCodPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {                                                                              
                                                                                       //al traerme una lista, hago foreach en Logica
                var tareasPuesto = entidadBase.TareasPuesto.Where(T => T.Puesto.CodPuesto == codPuesto).ToList();

                if (tareasPuesto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tareasPuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("no se encontraron tareas al código de puesto");
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

        //buscar por PK de TareasPuesto
        public CRespuestaDTO BuscarTareasId(int idTareasPuesto)
        {
            CRespuestaDTO respuesta;
            try
            {
                var tareasPuesto = entidadBase.TareasPuesto.Where(T => T.PK_TareasPuesto == idTareasPuesto).FirstOrDefault();
                if (tareasPuesto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tareasPuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron tareas del puesto solicitado");
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
        public CRespuestaDTO GuardarTareas(string CodPuesto,TareasPuesto tareasPuesto)
        {
            CRespuestaDTO respuesta;
            try
            {
                    entidadBase.TareasPuesto.Add(tareasPuesto);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = tareasPuesto
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
        
        #endregion
    }
}
