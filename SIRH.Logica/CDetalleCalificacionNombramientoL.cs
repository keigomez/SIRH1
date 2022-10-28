using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleCalificacionNombramientoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDetalleCalificacionNombramientoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Metodo encargado de Convertir los datos del Detalle CalificacionDTO NombramientoDTO.
        /// </summary>
        /// <param name="item"> de tipo </param>
        /// <returns></returns>
        internal static CDetalleCalificacionNombramientoDTO ConvertirDatosCDetalleCalificacionNombramientoADto(DetalleCalificacion item)
        {
            
            return new CDetalleCalificacionNombramientoDTO
            {

                IdEntidad = item.PK_DetalleCalificacion,
                CalificacionNombramientoDTO = CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(item.CalificacionNombramiento),
                CatalogoPreguntaDTO = CCatalogoPreguntaL.ConvertirDatosCCatalogoPreguntaADto(item.CatalogoPregunta),
                NumNotasPorPreguntaDTO = item.NumNotasPregunta
            };
        }

        internal static CDetalleCalificacionNombramientoDTO ConvertirDatosCDetalleCalificacionNombramientoModificadoADto(DetalleCalificacionModificada item)
        {

            return new CDetalleCalificacionNombramientoDTO
            {

                IdEntidad = item.PK_DetalleCalificacion,
                CalificacionNombramientoDTO = CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(item.CalificacionNombramiento),
                CatalogoPreguntaDTO = CCatalogoPreguntaL.ConvertirDatosCCatalogoPreguntaADto(item.CatalogoPregunta),
                NumNotasPorPreguntaDTO = item.NumNotasPregunta
            };
        }

        internal static CDetalleCalificacionNombramientoDTO ConvertirDatosCDetalleCalificacionModificadaADto(DetalleCalificacionModificada item)
        {

            return new CDetalleCalificacionNombramientoDTO
            {

                IdEntidad = item.PK_DetalleCalificacion,
                CalificacionNombramientoDTO = CCalificacionNombramientoL.ConvertirDatosCCalificacionNombramientoLADto(item.CalificacionNombramiento),
                CatalogoPreguntaDTO = CCatalogoPreguntaL.ConvertirDatosCCatalogoPreguntaADto(item.CatalogoPregunta),
                NumNotasPorPreguntaDTO = item.NumNotasPregunta.ToString()
            };
        }

        private static DetalleCalificacion ConvertirDTODetalleCalificacionNombramientoADatos(CDetalleCalificacionNombramientoDTO itemDCN)
        {
            return new DetalleCalificacion
            {
                PK_DetalleCalificacion = itemDCN.IdEntidad,
                CalificacionNombramiento = CCalificacionNombramientoL.ConvertirDTOCalificacionNombramientoADatos(itemDCN.CalificacionNombramientoDTO),
                CatalogoPregunta = CCatalogoPreguntaL.ConvertirDTOCatalogoPreguntaDatos(itemDCN.CatalogoPreguntaDTO),
                NumNotasPregunta = itemDCN.NumNotasPorPreguntaDTO
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCalificacion"></param>
        /// <param name="ursNombre"></param>
        /// <param name="fecha"></param>
        /// <param name="detalleNombramiento"></param>
        /// <returns></returns>
        //public CBaseDTO AgregarDetalleCalificacionNombramiento(int idCalificacion,  CDetalleCalificacionNombramientoDTO detalle)
        //{
        //    CBaseDTO respuesta = new CBaseDTO();

        //    try
        //    {
        //        CDetalleCalificacionNombramientoD intermedio = new CDetalleCalificacionNombramientoD(contexto);
                
        //        DetalleCalificacion datosDetalleCalificacionNombramientoBD = new DetalleCalificacion
        //        {
        //            PK_DetalleCalificacionNombramiento = detalleNombramiento.IdEntidad,
        //            CalificacionNombramiento = CCalificacionNombramientoL.ConvertirDTOCalificacionNombramientoADatos(detalleNombramiento.CalificacionNombramientoDTO),
        //            UsrEvaluador = detalleNombramiento.UsrEvaluadorDTO,
        //            FecCreacion = detalleNombramiento.FecCreacionDTO,
        //            IndEstado = detalleNombramiento.IndEstadoDTO,
        //            IdJefeInmediato = detalleNombramiento.JefeInmediato.IdEntidad,
        //            IdJefeSuperior = detalleNombramiento.JefeSuperior.IdEntidad
        //        };

        //        respuesta = intermedio.AgregarDetalleCalificacionNombramiento(datosDetalleCalificacionNombramientoBD);

        //        if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
        //        {
        //            respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
        //            throw new Exception();
        //        }
        //        else
        //        {
        //            return respuesta;
        //        }
        //    }
        //    catch
        //    {
        //        return respuesta;
        //    }
        //}


        //public CBaseDTO EditarDetalleCalificacionNombramiento(int detalleNombramiento)
        //{
        //    CBaseDTO respuesta = new CBaseDTO();

        //    try
        //    {
        //        CDetalleCalificacionNombramientoD intermedio = new CDetalleCalificacionNombramientoD(contexto);

        //        //DetalleCalificacionNombramiento datosDetalleCalificacionNombramientoBD = new DetalleCalificacionNombramiento
        //        //{
        //        //    PK_DetalleCalificacionNombramiento = detalleNombramiento.IdEntidad,
        //        //    IndEstado = detalleNombramiento.IndEstadoDTO
        //        //};

        //        var datosDetalleCalificacionNombramiento = intermedio.EditarDetalleCalificacionNombramiento(detalleNombramiento);

        //        if (datosDetalleCalificacionNombramiento.Codigo > 0)
        //        {
        //            respuesta = new CBaseDTO { IdEntidad = datosDetalleCalificacionNombramiento.IdEntidad };
        //        }
        //        else
        //        {
        //            respuesta = ((CErrorDTO)datosDetalleCalificacionNombramiento.Contenido);
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = (new CErrorDTO
        //        {
        //            Codigo = -1,
        //            MensajeError = error.Message
        //        });
        //    }

        //    return respuesta;
        //}
        
        #endregion
    }
}