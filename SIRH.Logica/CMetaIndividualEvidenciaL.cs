using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    //
    public class CMetaIndividualEvidenciaL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CMetaIndividualEvidenciaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CMetaIndividualEvidenciaDTO ConvertirDatosEvidenciaADto(MetaIndividualEvidencia item)
        {
            
            return new CMetaIndividualEvidenciaDTO
            {
                IdEntidad = item.PK_Evidencia,
                DesEvidencia = item.DesEvidencia,
                DesEnlace = item.DesEnlace,
                DocAdjunto = item.DocAdjunto,
                FecRegistro = Convert.ToDateTime(item.FecRegistro),
                IndEstado = item.IndEstado,
                Observaciones = item.DesObservaciones,
                DesArchivo = item.DesArchivo
            };
        }

        internal static MetaIndividualEvidencia ConvertirDTOEvidenciaADatos(CMetaIndividualEvidenciaDTO item)
        {
            return new MetaIndividualEvidencia
            {
                PK_Evidencia = item.IdEntidad,
                FecRegistro = item.FecRegistro,
                DesEvidencia = item.DesEvidencia,
                DesEnlace = item.DesEnlace,
                DocAdjunto = item.DocAdjunto,
                IndEstado = item.IndEstado,
                DesObservaciones = item.Observaciones,
                DesArchivo = item.DesArchivo
            };
        }

        public List<CBaseDTO> GuardarEvidencia(CMetaIndividualEvidenciaDTO evidencia)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);
                CMetaIndividualCalificacionD intermedioMeta = new CMetaIndividualCalificacionD(contexto);

                MetaIndividualEvidencia datos = ConvertirDTOEvidenciaADatos(evidencia);

                // Meta
                var entidadMeta = intermedioMeta.ConsultarMetaIndividual(evidencia.Meta.IdEntidad);
                if (entidadMeta.Codigo != -1)
                    datos.MetaIndividualCalificacion = (MetaIndividualCalificacion)entidadMeta.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadMeta).Contenido).MensajeError);


                var insertaMeta = intermedio.InsertarEvidencia(datos);

                //pregunto si da error
                if (insertaMeta.Codigo > 0)
                    respuesta.Add(evidencia);
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

        public CBaseDTO ModificarEvidencia(CMetaIndividualEvidenciaDTO evidencia)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);

                MetaIndividualEvidencia evidenciaBD = new MetaIndividualEvidencia
                {
                    PK_Evidencia = evidencia.IdEntidad,
                    DesObservaciones = evidencia.Observaciones,
                    IndEstado = evidencia.IndEstado
                };

                var datosEvidencia = intermedio.ActualizarEvidencia(evidenciaBD);

                if (datosEvidencia.Codigo > 0)
                    respuesta = new CBaseDTO { IdEntidad = evidencia.IdEntidad };
                else
                    respuesta = ((CErrorDTO)datosEvidencia.Contenido);
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

        public List<CBaseDTO> ObtenerEvidencia(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);
                
                var dato = intermedio.ConsultarEvidencia(codigo);
                if (dato.Codigo != -1)
                {
                    var datosEvidencia = ConvertirDatosEvidenciaADto((MetaIndividualEvidencia)dato.Contenido);
                    
                    // [0] Evidencia
                    respuesta.Add(datosEvidencia);
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

        public List<List<CBaseDTO>> BuscarEvidencias(CFuncionarioDTO funcionario, CMetaIndividualEvidenciaDTO evidencia,
                                                       List<DateTime> fechasRegistro, CMetaIndividualCalificacionDTO meta,
                                                       CPeriodoCalificacionDTO periodo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);

            List<MetaIndividualEvidencia> datosEvidencia = new List<MetaIndividualEvidencia>();

            bool busquedaAnterior = false;
            
            if (funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarEvidencia(datosEvidencia, funcionario.Cedula, "Cedula", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosEvidencia = (List<MetaIndividualEvidencia>)resultado.Contenido;

                busquedaAnterior = true;
            }

            if (periodo != null && periodo.IdEntidad > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarEvidencia(datosEvidencia, Convert.ToDecimal(periodo.IdEntidad), "Periodo", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosEvidencia = (List<MetaIndividualEvidencia>)resultado.Contenido;
                else
                    datosEvidencia.Clear();
            }

            if (meta != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarEvidencia(datosEvidencia, meta.IdEntidad, "Meta", busquedaAnterior));

                if (resultado.Codigo > 0)
                    datosEvidencia = (List<MetaIndividualEvidencia>)resultado.Contenido;
                else
                    datosEvidencia.Clear();
            }

            if (fechasRegistro != null && fechasRegistro.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarEvidencia(datosEvidencia, fechasRegistro, "Fecha", busquedaAnterior));
                if (resultado.Codigo > 0)
                    datosEvidencia = (List<MetaIndividualEvidencia>)resultado.Contenido;
                else
                    datosEvidencia.Clear();
            }
            
                       
            if (evidencia != null && evidencia.DesEvidencia != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarEvidencia(datosEvidencia, evidencia.DesEvidencia, "Descripcion", busquedaAnterior));
                if (resultado.Codigo > 0)
                    datosEvidencia = (List<MetaIndividualEvidencia>)resultado.Contenido;
                else
                    datosEvidencia.Clear();
            }

                     
            if (datosEvidencia.Count > 0)
            {
                foreach (var item in datosEvidencia)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();
                    temp.Add(ConvertirDatosEvidenciaADto(item));

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


        public CBaseDTO InsertarPermiso(CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datoFuncionario = intermedioFuncionario.BuscarFuncionarioCedula(funcionario.Cedula);

                var permiso = new CatPermisoEvidencia
                {
                    FK_Funcionario = datoFuncionario.PK_Funcionario,
                    IndArchivo = 1
                };

                var dato = intermedio.InsertarPermiso(permiso);

                if (dato != null)
                {
                    if (dato.Codigo != -1)
                        respuesta = new CBaseDTO { IdEntidad = 1 };
                    else
                        respuesta = ((CErrorDTO)dato.Contenido);
                }
                else
                {
                    throw new Exception("No se encontró el funcionario");
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        public CBaseDTO AnularPermiso(CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datoFuncionario = intermedioFuncionario.BuscarFuncionarioCedula(funcionario.Cedula);

                var permiso = new CatPermisoEvidencia
                {
                    FK_Funcionario = datoFuncionario.PK_Funcionario,
                    IndArchivo = 0
                };

                var dato = intermedio.ModificarPermiso(permiso);

                if (dato != null)
                {
                    if (dato.Codigo != -1)
                        respuesta = dato;
                    else
                        respuesta = ((CErrorDTO)dato.Contenido);
                }
                else
                {
                    throw new Exception("No se encontró el funcionario");
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        public CBaseDTO ObtenerPermiso(CFuncionarioDTO funcionario)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaIndividualEvidenciaD intermedio = new CMetaIndividualEvidenciaD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

                var datoFuncionario = intermedioFuncionario.BuscarFuncionarioCedula(funcionario.Cedula);

                if (datoFuncionario != null)
                {
                    var dato = intermedio.ConsultarPermiso(datoFuncionario.PK_Funcionario);
                    if (dato.Codigo != -1)
                        respuesta = dato;
                    else
                        respuesta = ((CErrorDTO)dato.Contenido);
                }
                else
                {
                    throw new Exception("No se encontró el funcionario");
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        #endregion
    }
}