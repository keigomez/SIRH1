using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatMetaEstadoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatMetaEstadoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO AgregarEstado(CatMetaEstado estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.CatMetaEstado.Add(estado);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = estado
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

        public CatMetaEstado ObtenerEstado(int codigo)
        {
            var estado = entidadBase.CatMetaEstado.Where(Q => Q.PK_Estado == codigo).FirstOrDefault();
            return estado;
        }

        public CRespuestaDTO ListarEstados()
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.CatMetaEstado.ToList();

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
                    throw new Exception("No se encontraron estados registrados.");
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
