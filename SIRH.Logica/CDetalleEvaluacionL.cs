using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleEvaluacionL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDetalleEvaluacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        internal static CDetalleEvaluacionDTO ConvertirDatosCDetalleEvaluacionADto(DetalleEvaluacion item)
        {

            return new CDetalleEvaluacionDTO
            {

                IdEntidad = item.PK_DetalleEvaluacion,
                CatalogoPreguntaDTO = CCatalogoPreguntaL.ConvertirDatosCCatalogoPreguntaADto(item.CatalogoPregunta),
                NumNotasPorPreguntaDTO = item.NumNotasPregunta,
                DetalleCalificacionNombramientoDTO = CDetalleCalificacionNombramientoL.ConvertirDatosCDetalleCalificacionNombramientoADto(item.DetalleCalificacionNombramiento),

            };
        }

        internal static DetalleEvaluacion ConvertirDTOsCDetalleEvaluacionADatos(CDetalleEvaluacionDTO item)
        {

            return new DetalleEvaluacion
            {
                PK_DetalleEvaluacion = item.IdEntidad,
                CatalogoPregunta = CCatalogoPreguntaL.ConvertirDTOCatalogoPreguntaDatos(item.CatalogoPreguntaDTO),
                NumNotasPregunta = item.NumNotasPorPreguntaDTO,
                DetalleCalificacionNombramiento = CDetalleCalificacionNombramientoL.ConvertirDTODetalleCalificacionNombramientoADatos(item.DetalleCalificacionNombramientoDTO)
            };
        }
        public CBaseDTO AgregarDetalleEvaluacion(CDetalleCalificacionNombramientoDTO idDetalleCN, CCatalogoPreguntaDTO idPregunta, CDetalleEvaluacionDTO detalleE)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDetalleEvaluacionD intermedio = new CDetalleEvaluacionD(contexto);

                DetalleEvaluacion datosDetalleEvaluacionBD = ConvertirDTOsCDetalleEvaluacionADatos(detalleE);
                CatalogoPregunta datosDetalleCatalogoPreguntaBD = CCatalogoPreguntaL.ConvertirDTOCatalogoPreguntaDatos(idPregunta);
                DetalleCalificacionNombramiento datosDetalleCalificacionN = CDetalleCalificacionNombramientoL.ConvertirDTODetalleCalificacionNombramientoADatos(idDetalleCN);
                respuesta = intermedio.AgregarDetalleEvaluacion(datosDetalleCalificacionN, datosDetalleCatalogoPreguntaBD, datosDetalleEvaluacionBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }
        #endregion
    }
}
