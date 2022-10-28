using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCursoGradoD
    {
        #region Variables

        private SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CCursoGradoD(SIRHEntities entidadGlobal)
        {
            contexto = entidadGlobal;
        }   

        #endregion

        #region Metodos
        //01/12/2016...
        public int GuardarCursoGrado(CursoGrado cursoGrado)
        {
            contexto.CursoGrado.Add(cursoGrado);
            return cursoGrado.PK_CursoGrado;
        }

        public CRespuestaDTO GuardarCursoGrado(int idFormacionEducativa, CursoGrado cursoGrado)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.CursoGrado.Add(cursoGrado);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = cursoGrado
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
