using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCatalogoPreguntaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCatalogoPreguntaL()
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
        internal static CCatalogoPreguntaDTO ConvertirDatosCCatalogoPreguntaADto(CatalogoPregunta item)
        {

            return new CCatalogoPreguntaDTO
            {

                IdEntidad = item.PK_CatalogoPregunta,
                DesPreguntaDTO = item.DesPregunta,
                IndTipoFormularioDTO = Convert.ToInt32(item.IndTipoFormulario),
                IndEstadoDTO = Convert.ToInt32(item.IndEstado),
                DesTituloPDTO = item.DesTituloP
            };
        }

        internal static CatalogoPregunta ConvertirDTOCatalogoPreguntaDatos(CCatalogoPreguntaDTO itemCP)
        {
            return new CatalogoPregunta
            {

                PK_CatalogoPregunta = itemCP.IdEntidad,
                DesPregunta = itemCP.DesPreguntaDTO,
                IndTipoFormulario = itemCP.IndTipoFormularioDTO,
                IndEstado = itemCP.IndEstadoDTO,
                DesTituloP = itemCP.DesTituloPDTO

            };
        }
        private static CatalogoPregunta ConvertirDTOCatalogoPreguntaADatos(CCatalogoPreguntaDTO pregunta)
        {
            return new CatalogoPregunta
            {
                PK_CatalogoPregunta = pregunta.IdEntidad,
                DesPregunta = pregunta.DesPreguntaDTO,
                IndTipoFormulario = pregunta.IndTipoFormularioDTO,
                IndEstado = pregunta.IndEstadoDTO,
                DesTituloP = pregunta.DesTituloPDTO

            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pregunta"></param>
        /// <returns></returns>
        public CBaseDTO AgregarPregunta(CCatalogoPreguntaDTO pregunta)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCatalogoPreguntaD intermedio = new CCatalogoPreguntaD(contexto);

                CatalogoPregunta datosCatalogoPreguntaBD = ConvertirDTOCatalogoPreguntaADatos(pregunta);

                respuesta = intermedio.AgregarPregunta(datosCatalogoPreguntaBD);

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



        /// <summary>
        /// 
        /// </summary>
        /// <param name="detalleNombramiento"></param>
        /// <returns></returns>
        public CBaseDTO EditarPregunta(CCatalogoPreguntaDTO pregunta)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCatalogoPreguntaD intermedio = new CCatalogoPreguntaD(contexto);


                //Cuando necesito pasar datos a la capa de Datos, no puedo volver a crear NUEVAS
                //entidades de esa capa, porque si no, vuelve a guardar el elemento
                //a menos que en datos, le caiga encima al mismo elemento creado
                CatalogoPregunta datosCatalogoPreguntaBD = new CatalogoPregunta
                {
                    PK_CatalogoPregunta = pregunta.IdEntidad,
                    IndEstado = pregunta.IndEstadoDTO
                };

                var datosCatalogoPregunta = intermedio.EditarPregunta(datosCatalogoPreguntaBD);

                if (datosCatalogoPregunta.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = datosCatalogoPregunta.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosCatalogoPregunta.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codPregunta"></param>
        /// <returns></returns>
        public CBaseDTO ObtenerPreguntas(int codPregunta)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCatalogoPreguntaD intermedio = new CCatalogoPreguntaD(contexto);

                var resultado = intermedio.ObtenerPreguntas(codPregunta);

                if (resultado.Codigo > 0)
                {
                    respuesta = ConvertirDatosCCatalogoPreguntaADto(((CatalogoPregunta)resultado.Contenido));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoFormulario"></param>
        /// <returns></returns>
        public List<CBaseDTO> ListarPreguntas(int tipoFormulario)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            CCatalogoPreguntaD catalogoPregunta = new CCatalogoPreguntaD(contexto);
            var pregunta = catalogoPregunta.ListarPreguntas(tipoFormulario);

            if (pregunta != null)
            {
                foreach (var item in pregunta)
                {
                    resultado.Add(new CCatalogoPreguntaDTO { IdEntidad = item.PK_CatalogoPregunta, DesPreguntaDTO = item.DesPregunta, IndTipoFormularioDTO = Convert.ToInt32(item.IndTipoFormulario),IndEstadoDTO =Convert.ToInt32(item.IndEstado), DesTituloPDTO=item.DesTituloP });
                }

            } else
            {
                resultado.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron Preguntas" });
            }
            
   
            return resultado;
        }





        #endregion
    }
}
