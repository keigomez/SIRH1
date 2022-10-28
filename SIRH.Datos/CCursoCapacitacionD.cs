using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCursoCapacitacionD
    {
        #region Variables

        private SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CCursoCapacitacionD(SIRHEntities entidadGlobal)
        {
            contexto = entidadGlobal;
        }
        #endregion

        #region Metodos

        public int GuardarCursoCapacitacion(CursoCapacitacion cursoCapacitacion)        
        {
            contexto.CursoCapacitacion.Add(cursoCapacitacion);
            return cursoCapacitacion.PK_CursoCapacitacion;
        }

        public CRespuestaDTO GuardarCursoCapacitacion(int idFormacionAcademica, CursoCapacitacion cursoCapacitacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.CursoCapacitacion.Add(cursoCapacitacion);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = cursoCapacitacion
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