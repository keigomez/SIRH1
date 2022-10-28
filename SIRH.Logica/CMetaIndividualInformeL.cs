using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CMetaIndividualInformeL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CMetaIndividualInformeL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CMetaIndividualInformeDTO ConvertirDatosInformeADto(MetaIndividualInforme item)
        {
            
            return new CMetaIndividualInformeDTO
            {
                IdEntidad = item.PK_Informe,
                DesInforme = item.DesInforme,
                IndCompleto = item.IndCompleto,
                NumIndicador = item.NumIndicador,
                NumResultadoProduccion = item.NumResultadoProduccion,
                FecMes = Convert.ToDateTime(item.FecMes),
                IndEstado = item.IndEstado,
                Observaciones = item.DesObservaciones,
            };
        }

        internal static MetaIndividualInforme ConvertirDTOInformeADatos(CMetaIndividualInformeDTO item)
        {
            return new MetaIndividualInforme
            {
                PK_Informe = item.IdEntidad,
                FecMes = item.FecMes,
                IndCompleto = item.IndCompleto,
                NumIndicador = item.NumIndicador,
                NumResultadoProduccion = item.NumResultadoProduccion,
                DesInforme = item.DesInforme,
                IndEstado = item.IndEstado,
                DesObservaciones = item.Observaciones
            };
        }

        public List<CBaseDTO> GuardarInforme(CMetaIndividualInformeDTO Informe)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaIndividualInformeD intermedio = new CMetaIndividualInformeD(contexto);
                CMetaIndividualCalificacionD intermedioMeta = new CMetaIndividualCalificacionD(contexto);

                MetaIndividualInforme datos = ConvertirDTOInformeADatos(Informe);

                // Meta
                var entidadMeta = intermedioMeta.ConsultarMetaIndividual(Informe.Meta.IdEntidad);
                if (entidadMeta.Codigo != -1)
                    datos.MetaIndividualCalificacion = (MetaIndividualCalificacion)entidadMeta.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadMeta).Contenido).MensajeError);


                var insertaMeta = intermedio.InsertarInforme(datos);

                //pregunto si da error
                if (insertaMeta.Codigo > 0)
                    respuesta.Add(Informe);
                else
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
            }
            catch (Exception error)
            {              
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public CBaseDTO ModificarInforme(CMetaIndividualInformeDTO Informe)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaIndividualInformeD intermedio = new CMetaIndividualInformeD(contexto);

                MetaIndividualInforme InformeBD = new MetaIndividualInforme
                {
                    PK_Informe = Informe.IdEntidad,
                    DesObservaciones = Informe.Observaciones,
                    IndEstado = Informe.IndEstado
                };

                var datosInforme = intermedio.ActualizarInforme(InformeBD);

                if (datosInforme.Codigo > 0)
                    respuesta = new CBaseDTO { IdEntidad = Informe.IdEntidad };
                else
                    respuesta = ((CErrorDTO)datosInforme.Contenido);
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

        public List<CBaseDTO> ObtenerInforme(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaIndividualInformeD intermedio = new CMetaIndividualInformeD(contexto);
                
                var dato = intermedio.ConsultarInforme(codigo);
                if (dato.Codigo != -1)
                {
                    var datosInforme = ConvertirDatosInformeADto((MetaIndividualInforme)dato.Contenido);
                    
                    // [0] Informe
                    respuesta.Add(datosInforme);
                }
                else
                {
                    respuesta.Add((CErrorDTO)dato.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public List<List<CBaseDTO>> BuscarInformes(CFuncionarioDTO funcionario, CMetaIndividualInformeDTO Informe,
                                                       List<DateTime> fechasRegistro, CMetaIndividualCalificacionDTO meta,
                                                       CPeriodoCalificacionDTO periodo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CMetaIndividualInformeD intermedio = new CMetaIndividualInformeD(contexto);

            List<MetaIndividualInforme> datosInforme = new List<MetaIndividualInforme>();

            bool busquedaAnterior = false;
            
            if (funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarInforme(datosInforme, funcionario.Cedula, "Cedula", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosInforme = (List<MetaIndividualInforme>)resultado.Contenido;

                busquedaAnterior = true;
            }

            if (periodo != null && periodo.IdEntidad > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarInforme(datosInforme, Convert.ToDecimal(periodo.IdEntidad), "Periodo", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosInforme = (List<MetaIndividualInforme>)resultado.Contenido;
                else
                    datosInforme.Clear();
            }

            if (meta != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarInforme(datosInforme, meta.IdEntidad, "Meta", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosInforme = (List<MetaIndividualInforme>)resultado.Contenido;
                else
                    datosInforme.Clear();
            }

            if (fechasRegistro != null && fechasRegistro.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarInforme(datosInforme, fechasRegistro, "Fecha", busquedaAnterior));
                if (resultado.Codigo > 0)
                    datosInforme = (List<MetaIndividualInforme>)resultado.Contenido;
                else
                    datosInforme.Clear();
            }
            
                       
            if (Informe != null && Informe.DesInforme != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarInforme(datosInforme, Informe.DesInforme, "Descripcion", busquedaAnterior));
                if (resultado.Codigo > 0)
                    datosInforme = (List<MetaIndividualInforme>)resultado.Contenido;
                else
                    datosInforme.Clear();
            }

                     
            if (datosInforme.Count > 0)
            {
                foreach (var item in datosInforme)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(ConvertirDatosInformeADto(item));

                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                respuesta.Add(temp);
            }

            return respuesta;
        }

        #endregion
    }
}