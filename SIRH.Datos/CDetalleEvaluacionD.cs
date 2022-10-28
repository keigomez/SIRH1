using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Datos
{
    public class CDetalleEvaluacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDetalleEvaluacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos
         /// <summary>
         /// 
         /// </summary>
         /// <param name="idDetalleEvaluacion"></param>
         /// <param name="idPregunta"></param>
         /// <param name="detalle"></param>
         /// <returns></returns>
        public CRespuestaDTO AgregarDetalleEvaluacion(DetalleCalificacionNombramiento idDetalleCN,CatalogoPregunta idPregunta, DetalleEvaluacion detalle)
        {
            CRespuestaDTO respuesta;
            try
            {


                var CalificacionNombramiento = entidadBase.CalificacionNombramiento.
                    Where(CN => CN.PK_CalificacionNombramiento == detalle.DetalleCalificacionNombramiento.CalificacionNombramiento.PK_CalificacionNombramiento).FirstOrDefault();

                if (CalificacionNombramiento != null)
                { //si la calificacion NombramientoDTO Existe
                    detalle.CatalogoPregunta = entidadBase.CatalogoPregunta.Where(Q => Q.PK_CatalogoPregunta == idPregunta.PK_CatalogoPregunta).FirstOrDefault();
                    entidadBase.DetalleEvaluacion.Add(detalle);
                    //entidadBase.DetalleEvaluacion.Add(detalle);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle
                    };
                    return respuesta;

                }
               
            

                else
                {
                    throw new Exception("No se encontró el funcionario indicado");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }
        #endregion
    }
}
