using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEntidadEducativaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEntidadEducativaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        //01/12/2016...
        
        public CRespuestaDTO GuardarEntidadEducativa(EntidadEducativa entidadEducativa)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (entidadEducativa.TipEntidad > 0)
                {
                    entidadBase.EntidadEducativa.Add(entidadEducativa);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidadEducativa.PK_EntidadEducativa
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Tipo de entidad inválido");
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

        public CRespuestaDTO AnularEntidadEducativa(int id)
        {
            CRespuestaDTO respuesta;
            try
            {
                var entidad = entidadBase.EntidadEducativa.Where(E => E.PK_EntidadEducativa == id).FirstOrDefault();
                if (entidad != null)
                {
                    entidad.Estado = 2;
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad.PK_EntidadEducativa
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró la entidad educativa especificada");
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

        public CRespuestaDTO BuscarEntidadEducativa(int codEntidadEducativa)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = entidadBase.EntidadEducativa.Where(M => M.PK_EntidadEducativa == codEntidadEducativa).FirstOrDefault();

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
                    throw new Exception("No se encontró ninguna Entidad Educativa");
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


        public CRespuestaDTO BuscarEntidadEducativa(string nombre, int tipo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = new List<EntidadEducativa>();
                if (nombre != null && tipo > 0)
                    datosEntidad = entidadBase.EntidadEducativa.Where(M => M.NomEntidad.Contains(nombre) && M.TipEntidad == tipo && M.Estado == 1).ToList();
                else if (tipo > 0)
                    datosEntidad = entidadBase.EntidadEducativa.Where(M => M.TipEntidad == tipo && M.Estado == 1).ToList();
                else if (nombre != null)
                    datosEntidad = entidadBase.EntidadEducativa.Where(M => M.NomEntidad.Contains(nombre) && M.Estado == 1).ToList();

                if (datosEntidad.Count() > 0)
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
                    throw new Exception("No se encontró ninguna Entidad Educativa");
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


        public CRespuestaDTO ListarEntidadEducativa()
        { 
            CRespuestaDTO respuesta;


            try
            {
                var datosEntidad = entidadBase.EntidadEducativa.ToList();

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
                    throw new Exception("No se encontraron entidades educativas");
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

        
     