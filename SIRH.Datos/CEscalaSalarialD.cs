using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEscalaSalarialD
    {
        #region Variables

        private SIRHEntities entidadBase;
        
        #endregion

        #region Constructor

        public CEscalaSalarialD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos

        public CRespuestaDTO GuardarEscalaSalarial(EscalaSalarial escala)
        {
            try
            {
                bool existeCategoria = entidadBase.EscalaSalarial.Where(Q => Q.IndCategoria == escala.IndCategoria
                                                                        && Q.PeriodoEscalaSalarial.NumResolucion == escala.PeriodoEscalaSalarial.NumResolucion)
                                                                        .Count() > 0 ? true : false;

                if (!existeCategoria)
                {
                    entidadBase.EscalaSalarial.Add(escala);
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO 
                    { 
                        Codigo = 1,
                        Contenido = escala
                    };
                }
                else
                {
                    throw new Exception("Ya existe una Escala Salarial con el mismo número de resolución y categoría");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        public CRespuestaDTO RetornarCategorias()
        {
            CRespuestaDTO respuesta;
            try
            {
                var entidad = entidadBase.EscalaSalarial.Select(q => q.IndCategoria).Distinct().ToList();

                if (entidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de las Categorías de las Escalas Salariales"
                    };
                }
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

        public CRespuestaDTO BuscarEscalaCategoria(int indCategoria)
        {
            CRespuestaDTO respuesta;
            try
            {
                var entidad = entidadBase.EscalaSalarial
                                        .Include("PeriodoEscalaSalarial")
                                        .Where(q => q.IndCategoria == indCategoria).FirstOrDefault();

                if (entidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontró la Categoría dentro de las Escalas Salariales"
                    };
                }
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


        public CRespuestaDTO BuscarEscalaCategoriaPeriodo(int indCategoria, int indPeriodo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var entidad = entidadBase.EscalaSalarial
                                        .Include("PeriodoEscalaSalarial")
                                        .Where(q => q.IndCategoria == indCategoria && q.FK_Periodo == indPeriodo).FirstOrDefault();

                if (entidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = entidad
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "No se encontró la Categoría dentro de las Escalas Salariales"
                    };
                }
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
        #endregion
    }
}
