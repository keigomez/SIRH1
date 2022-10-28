using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatMetaPrioridadD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatMetaPrioridadD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO AgregarPrioridad(CatMetaPrioridad prioridad)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.CatMetaPrioridad.Add(prioridad);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = prioridad
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

        public CatMetaPrioridad ObtenerPrioridad(int codigo)
        {
            var prioridad = entidadBase.CatMetaPrioridad.Where(Q => Q.PK_Prioridad == codigo).FirstOrDefault();
            return prioridad;
        }

        public CRespuestaDTO ListarPrioridades()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.CatMetaPrioridad.ToList();

                if (datos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron prioridades registrados.");
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
