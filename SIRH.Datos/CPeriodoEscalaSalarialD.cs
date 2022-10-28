using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPeriodoEscalaSalarialD
    {
        #region Variables

        private SIRHEntities entidadBase;
        
        #endregion

        #region Constructor

        public CPeriodoEscalaSalarialD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos

        public CRespuestaDTO GuardarPeriodoEscalaSalarial(PeriodoEscalaSalarial periodo)
        {
            try
            {
                bool existePeriodo = entidadBase.PeriodoEscalaSalarial.Where(Q => Q.NumResolucion == periodo.NumResolucion).Count() > 0 ? true : false;

                if (!existePeriodo)
                {
                    entidadBase.PeriodoEscalaSalarial.Add(periodo);
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO 
                    { 
                        Codigo = 1,
                        Contenido = periodo
                    };
                }
                else
                {
                    throw new Exception("Ya existe un Periodo de Escala Salarial con el mismo número de Resolución");
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

        public CRespuestaDTO ObtenerPeriodoActual()
        {
            CRespuestaDTO respuesta;
            try
            {
                var periodo = entidadBase.PeriodoEscalaSalarial.OrderByDescending(Q => Q.FecCierre ?? DateTime.MaxValue).FirstOrDefault();

                if (periodo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = periodo
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró el Periodo de Escala" }
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
