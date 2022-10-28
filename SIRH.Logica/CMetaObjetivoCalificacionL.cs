using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CMetaObjetivoCalificacionL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CMetaObjetivoCalificacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CMetaObjetivoCalificacionDTO ConvertirDatosMetaADto(MetaObjetivoCalificacion item)
        {
            
            return new CMetaObjetivoCalificacionDTO
            {
                IdEntidad = item.PK_Meta,
                Descripcion = item.DesMeta,
                IndEstado = Convert.ToInt16(item.IndEstado),
                TipoMeta = Convert.ToInt16(item.TipMeta),
                FechaInicio = Convert.ToDateTime(item.FecInicio),
                FechaFinalizacion = Convert.ToDateTime(item.FecFinalizacion),
                Objetivo = CObjetivoCalificacionL.ConvertirDatosObjetivoADto(item.ObjetivoCalificacion),
                ProductoEspecifico = item.DesProductoEspecifico,
                FuenteDatos = item.DesFuenteDatos,
                Supuestos = item.DesSupuestos,
                NotasTecnicas = item.DesNotasTecnicas,
                TipoIndicador = item.DesTipoIndicador,
                Indicador = item.DesIndicador,
                Dimension = item.DesDimension,
                MetaAnual = item.DesMetaAnual
                //Prioridad = Convert.ToInt16(item.IndPrioridad),
                //ValorAsignado = Convert.ToDecimal(item.ValorAsignado),
                //ValorCumplido = Convert.ToDecimal(item.ValorCumplido),
                //Observaciones = item.DesObservaciones,
            };
        }

        internal static MetaObjetivoCalificacion ConvertirDTOMetaADatos(CMetaObjetivoCalificacionDTO item)
        {
            return new MetaObjetivoCalificacion
            {
                PK_Meta = item.IdEntidad,
                DesMeta = item.Descripcion,
                IndEstado = item.IndEstado,
                TipMeta = item.TipoMeta,
                FecInicio = item.FechaInicio,
                FecFinalizacion = item.FechaFinalizacion,
                DesProductoEspecifico = item.ProductoEspecifico,
                DesFuenteDatos = item.FuenteDatos,
                DesSupuestos = item.Supuestos,
                DesNotasTecnicas = item.NotasTecnicas,
                DesTipoIndicador = item.TipoIndicador,
                DesIndicador = item.Indicador,
                DesDimension = item.Dimension,
                DesMetaAnual = item.MetaAnual
                //DesObservaciones = item.Observaciones,
                //IndPrioridad = Convert.ToInt16(item.Prioridad),
                //ValorAsignado = Convert.ToDecimal(item.ValorAsignado),
                //ValorCumplido = Convert.ToDecimal(item.ValorCumplido),
            };
        }

        public List<CBaseDTO> GuardarMeta(CMetaObjetivoCalificacionDTO meta)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaObjetivoCalificacionD intermedio = new CMetaObjetivoCalificacionD(contexto);
                CObjetivoCalificacionD intermedioObj = new CObjetivoCalificacionD(contexto);
                //CFuncionarioD intermedioFun = new CFuncionarioD(contexto);
                //CUbicacionAdministrativaD intermedioUbic = new CUbicacionAdministrativaD(contexto);

                MetaObjetivoCalificacion datos = ConvertirDTOMetaADatos(meta);

                // Objetivo Institucional
                var entidadObj = intermedioObj.ConsultarObjetivoCalificacion(meta.Objetivo.IdEntidad);
                if (entidadObj.Codigo != -1)
                    datos.ObjetivoCalificacion = (ObjetivoCalificacion)entidadObj.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadObj).Contenido).MensajeError);

                var insertaMeta = intermedio.InsertarMetaObjetivoCalificacion(datos);

                if (insertaMeta.Codigo > 0)
                    respuesta.Add(meta);
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
                //respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerMeta(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaObjetivoCalificacionD intermedio = new CMetaObjetivoCalificacionD(contexto);
               
                var dato = intermedio.ConsultarMetaObjetivoCalificacion(codigo);
                if (dato.Codigo != -1)
                {
                    var datosMeta = ConvertirDatosMetaADto((MetaObjetivoCalificacion)dato.Contenido);
                                        
                    // 01 Meta
                    respuesta.Add(datosMeta);
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

        public List<List<CBaseDTO>> BuscarMetas(CFuncionarioDTO funcionario, CObjetivoCalificacionDTO objetivo,
                                                CMetaObjetivoCalificacionDTO meta,
                                                List<DateTime> fechasInicio, List<DateTime> fechasVencimiento, 
                                                CSeccionDTO seccion)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();

            CMetaObjetivoCalificacionD intermedio = new CMetaObjetivoCalificacionD(contexto);

            List<MetaObjetivoCalificacion> datosMeta = new List<MetaObjetivoCalificacion>();

            bool buscar = true;

            if (seccion != null && seccion.IdEntidad > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarMetaObjetivoCalificacion(datosMeta, seccion.IdEntidad, "Seccion"));
                if (resultado.Codigo > 0)
                {
                    datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                }
                else
                    buscar = false;
            }

            if (objetivo != null && objetivo.IdEntidad > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, objetivo.IdEntidad, "Objetivo"));
                if (resultado.Codigo > 0)
                {
                    datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                }
                else
                    buscar = false;
            }

            if (objetivo != null && objetivo.Periodo != null && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, objetivo.Periodo.IdEntidad, "Periodo"));
                if (resultado.Codigo > 0)
                {
                    datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                }
                else
                    buscar = false;
            }

            if (funcionario != null && funcionario.Cedula != null && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, funcionario.Cedula, "Cedula"));
                if (resultado.Codigo > 0)
                {
                    datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                }
                else
                    buscar = false;
            }


            if (fechasInicio != null && fechasInicio.Count > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, fechasInicio, "FechaInicio"));
                if (resultado.Codigo > 0)
                {
                    datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                }
                else
                    buscar = false;
            }

            if (fechasVencimiento != null && fechasVencimiento.Count > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, fechasVencimiento, "FechaFinal"));
                if (resultado.Codigo > 0)
                {
                    datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                }
                else
                    buscar = false;
            }

            if (meta != null && buscar)
            {
                if (meta.Observaciones != null && meta.Observaciones != "" && buscar)
                {
                    var resultado = ((CRespuestaDTO)intermedio.
                        BuscarMetaObjetivoCalificacion(datosMeta, meta.Observaciones, "Observaciones"));
                    if (resultado.Codigo > 0)
                    {
                        datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                    }
                    else
                        buscar = false;
                }

                if (meta.TipoMeta > 0 && buscar)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, meta.TipoMeta, "TipMeta"));
                    if (resultado.Codigo > 0)
                    {
                        datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                    }
                    else
                        buscar = false;
                }

                if (meta.IdEntidad > 0 && buscar)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarMetaObjetivoCalificacion(datosMeta, meta.IdEntidad, "Meta"));
                    if (resultado.Codigo > 0)
                    {
                        datosMeta = (List<MetaObjetivoCalificacion>)resultado.Contenido;
                    }
                    else
                        buscar = false;
                }
            }
                       

            if (datosMeta.Count > 0 && buscar)
            {
                foreach (var item in datosMeta)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    // [0] Meta
                    temp.Add(ConvertirDatosMetaADto(item));

                    //CFuncionarioDTO tempFuncionario = new CFuncionarioDTO
                    //{
                    //    Cedula = item.Funcionario.IdCedulaFuncionario,
                    //    Nombre = item.Funcionario.NomFuncionario.TrimEnd(),
                    //    PrimerApellido = item.Funcionario.NomPrimerApellido.TrimEnd(),
                    //    SegundoApellido = item.Funcionario.NomSegundoApellido.TrimEnd(),
                    //    Sexo = GeneroEnum.Indefinido
                    //};

                    //// [1] Funcionario
                    //temp.Add(tempFuncionario);
                    
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

        public List<CBaseDTO> ObtenerUbicacion(int idSeccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaObjetivoCalificacionD intermedio = new CMetaObjetivoCalificacionD(contexto);

                var dato = intermedio.ObtenerUbicacion(idSeccion);
                if (dato.Codigo != -1)
                {
                    var datosUbi = CUbicacionAdministrativaL.ConvertirUbicacionAdministrativaADTO((UbicacionAdministrativa)dato.Contenido);

                    // 01 Ubicación
                    respuesta.Add(datosUbi);
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

        public CBaseDTO AnularMeta(CMetaObjetivoCalificacionDTO meta)
        {
            try
            {
                CMetaObjetivoCalificacionD intermedio = new CMetaObjetivoCalificacionD(contexto);
                var resultado = intermedio.AnularMeta(meta.IdEntidad);

                if (resultado.Codigo > 0)
                    return ConvertirDatosMetaADto(((MetaObjetivoCalificacion)resultado.Contenido));
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO ModificarMeta(CMetaObjetivoCalificacionDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaObjetivoCalificacionD intermedio = new CMetaObjetivoCalificacionD(contexto);

                MetaObjetivoCalificacion metaBD = new MetaObjetivoCalificacion
                {
                    PK_Meta = registro.IdEntidad,
                    FecInicio = registro.FechaInicio,
                    FecFinalizacion = registro.FechaFinalizacion
                    //IndPrioridad = registro.Prioridad,
                    //ValorAsignado = registro.ValorAsignado,
                    //ValorCumplido = registro.ValorCumplido
                };

                respuesta = intermedio.ModificarMeta(metaBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }
                else
                {
                    return respuesta = new CBaseDTO { Mensaje = (((CRespuestaDTO)respuesta).Contenido).ToString() }; ;
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

        #endregion
    }
}