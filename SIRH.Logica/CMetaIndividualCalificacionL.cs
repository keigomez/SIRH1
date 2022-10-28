using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CMetaIndividualCalificacionL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CMetaIndividualCalificacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CMetaIndividualCalificacionDTO ConvertirDatosMetaADto(MetaIndividualCalificacion item)
        {
            var metaObj = new CMetaObjetivoCalificacionDTO();
            if (item.MetaObjetivoCalificacion != null)
                metaObj = CMetaObjetivoCalificacionL.ConvertirDatosMetaADto(item.MetaObjetivoCalificacion);

            var listaEvidencia = new List<CMetaIndividualEvidenciaDTO>();
            var listaInforme = new List<CMetaIndividualInformeDTO>();

            // Evidencia
            foreach (var ev in item.MetaIndividualEvidencia)
                listaEvidencia.Add(new CMetaIndividualEvidenciaDTO
                {
                    IdEntidad = ev.PK_Evidencia,
                    DesEvidencia = ev.DesEvidencia,
                    IndEstado = ev.IndEstado,
                    FecRegistro = ev.FecRegistro,
                    DesEnlace = ev.DesEnlace,
                    DocAdjunto = ev.DocAdjunto,
                    DesArchivo = ev.DesArchivo,
                    Observaciones = ev.DesObservaciones != null ? ev.DesObservaciones :""
                });

            // Informe
            foreach (var inf in item.MetaIndividualInforme)
                listaInforme.Add(new CMetaIndividualInformeDTO
                {
                    IdEntidad = inf.PK_Informe,
                    DesInforme = inf.DesInforme,
                    IndEstado = inf.IndEstado,
                    FecMes = inf.FecMes,
                    IndCompleto = inf.IndCompleto,
                    NumIndicador = inf.NumIndicador,
                    NumResultadoProduccion = inf.NumResultadoProduccion,
                    Observaciones = inf.DesObservaciones != null ? inf.DesObservaciones : ""
                });
            
            return new CMetaIndividualCalificacionDTO
            {
                IdEntidad = item.PK_Meta,
                DesMeta = item.DesMeta,
                Periodo = CCalificacionNombramientoL.ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                Funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Funcionario),
                Prioridad = CCatMetaPrioridadL.ConvertirPrioridadADTO(item.CatMetaPrioridad),
                Estado = CCatMetaEstadoL.ConvertirEstadoADTO(item.CatMetaEstado), 
                FecDesde = Convert.ToDateTime(item.FecDesde),
                FecHasta = Convert.ToDateTime(item.FecHasta),
                FecRegistro = Convert.ToDateTime(item.FecRegistro),
                FecFinalizado = Convert.ToDateTime(item.FecFinalizado),
                DesIndicadorMensual = item.DesIndicadorMensual,
                NumIndicador = item.NumIndicador,
                TipoIndicador = CTipoIndicadorMetaL.ConvertirTipoADto(item.TipoIndicadorMeta),
                DesResultadoProduccion = item.DesResultadoProduccion,
                IndEsTeletrabajable = item.IndEsTeletrabajable,
                PorPeso = item.PorPeso,
                PorPesoNuevo = item.PorPeso,
                IndModificable = item.IndModificar == 1 ? true : false,
                DesObservaciones = item.DesObservaciones,
                MetaObjetivo = metaObj,
                JefeInmediato = new CFuncionarioDTO
                {
                    IdEntidad = Convert.ToInt32(item.IdJefeInmediato),
                    Sexo = GeneroEnum.Indefinido
                },
                ListaEvidencias = listaEvidencia,
                ListaInforme = listaInforme
            };
        }

        internal static MetaIndividualCalificacion ConvertirDTOMetaADatos(CMetaIndividualCalificacionDTO item)
        {
            return new MetaIndividualCalificacion
            {
                PK_Meta = item.IdEntidad,
                DesMeta = item.DesMeta,
                FecDesde = Convert.ToDateTime(item.FecDesde),
                FecHasta = Convert.ToDateTime(item.FecHasta),
                FecRegistro = Convert.ToDateTime(item.FecRegistro),
                FecFinalizado = Convert.ToDateTime(item.FecFinalizado),
                DesIndicadorMensual = item.DesIndicadorMensual,
                NumIndicador = item.NumIndicador,
                DesResultadoProduccion = item.DesResultadoProduccion,
                PorPeso = item.PorPeso,
                IndModificar = item.IndModificable ? 1 : 0,
                IndEsTeletrabajable = item.IndEsTeletrabajable,
                DesObservaciones = item.DesObservaciones,
                IdJefeInmediato = item.JefeInmediato.IdEntidad
            };
        }

        public List<CBaseDTO> GuardarMeta(CMetaIndividualCalificacionDTO meta)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);
                CMetaObjetivoCalificacionD intermedioObj = new CMetaObjetivoCalificacionD(contexto);
                CCalificacionNombramientoD intermedioCalificacionN = new CCalificacionNombramientoD(contexto);
                CFuncionarioD intermedioFun = new CFuncionarioD(contexto);
                CCatMetaEstadoD intermedioEstado= new CCatMetaEstadoD(contexto);
                CTipoIndicadorMetaD intermedioTipo = new CTipoIndicadorMetaD(contexto);
                CCatMetaPrioridadD intermedioPrioridad = new CCatMetaPrioridadD(contexto);

                MetaIndividualCalificacion datos = ConvertirDTOMetaADatos(meta);

                // Meta Objetivo
                if (meta.MetaObjetivo != null)
                {
                    var entidadObj = intermedioObj.ConsultarMetaObjetivoCalificacion(meta.MetaObjetivo.IdEntidad);
                    if (entidadObj.Codigo != -1)
                        datos.MetaObjetivoCalificacion = (MetaObjetivoCalificacion)entidadObj.Contenido;
                    else
                        throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadObj).Contenido).MensajeError);
                }


                // Periodo Evaluación
                var entidadPeriodo = intermedioCalificacionN.ObtenerPeriodoCalificacion(meta.Periodo.IdEntidad);

                if (entidadPeriodo.Codigo != -1)
                    datos.PeriodoCalificacion = (PeriodoCalificacion)entidadPeriodo.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadPeriodo).Contenido).MensajeError);


                // Funcionario
                var entidadFun = intermedioFun.BuscarFuncionarioCedulaBase(meta.Funcionario.Cedula);
                if (entidadFun.Codigo != -1)
                    datos.Funcionario = (Funcionario)entidadFun.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadFun).Contenido).MensajeError);


                // Prioridad
                var entidadPrio = intermedioPrioridad.ObtenerPrioridad(meta.Prioridad.IdEntidad);
                if (entidadPrio != null)
                    datos.CatMetaPrioridad = entidadPrio;
                else
                    throw new Exception("No existe registro de Prioridad");


                // Estado
                var entidadEstado = intermedioEstado.ObtenerEstado(meta.Estado.IdEntidad);
                if (entidadEstado != null)
                    datos.CatMetaEstado = entidadEstado;
                else
                    throw new Exception("No existe registro de Estado");

                // Tipo Indicador
                var entidadTipo = intermedioTipo.CargarTipoIndicadorMetaPorID(meta.TipoIndicador.IdEntidad);
                if (entidadTipo != null && entidadTipo.Codigo != -1)
                    datos.TipoIndicadorMeta = (TipoIndicadorMeta)entidadTipo.Contenido;
                else
                    throw new Exception("No existe registro de  Tipo Indicador");

                // Jefe Inmediato
                if (meta.JefeInmediato.IdEntidad == 0)
                {
                    var entidadJefe = intermedioFun.BuscarFuncionarioCedulaBase(meta.JefeInmediato.Cedula);
                    if (entidadJefe.Codigo != -1)
                        datos.Funcionario = (Funcionario)entidadJefe.Contenido;
                    else
                        throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadJefe).Contenido).MensajeError);
                }

                datos.IndModificar = 0;
                datos.DesObservaciones = "";
                datos.FecFinalizado = null;

                var insertaMeta = intermedio.InsertarMetaIndividual(datos);

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
                CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);
                CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

                var dato = intermedio.ConsultarMetaIndividual(codigo);
                if (dato.Codigo != -1)
                {
                    var datosMeta = ConvertirDatosMetaADto((MetaIndividualCalificacion)dato.Contenido);

                    // Jefe Inmediato
                    if (datosMeta.JefeInmediato.IdEntidad != null && datosMeta.JefeInmediato.IdEntidad != 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(Convert.ToInt32(datosMeta.JefeInmediato.IdEntidad));
                        datosMeta.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }

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

        public List<CBaseDTO> BuscarMetas(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo,
                                                CMetaIndividualCalificacionDTO meta,
                                                List<DateTime> fechasInicio, List<DateTime> fechasVencimiento)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);
            CDetalleCalificacionNombramientoD intermedioDCN = new CDetalleCalificacionNombramientoD(contexto);

            List<MetaIndividualCalificacion> datosMeta = new List<MetaIndividualCalificacion>();

            bool buscar = true;

            if (periodo != null && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaIndividual(datosMeta, periodo.IdEntidad, "Anio"));
                if (resultado.Codigo > 0)
                    datosMeta = (List<MetaIndividualCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }

            if (funcionario != null && funcionario.Cedula != null && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaIndividual(datosMeta, funcionario.Cedula, "Cedula"));
                if (resultado.Codigo > 0)
                    datosMeta = (List<MetaIndividualCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }


            if (fechasInicio != null && fechasInicio.Count > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaIndividual(datosMeta, fechasInicio, "FechaInicio"));
                if (resultado.Codigo > 0)
                    datosMeta = (List<MetaIndividualCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }

            if (fechasVencimiento != null && fechasVencimiento.Count > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarMetaIndividual(datosMeta, fechasVencimiento, "FechaFinal"));
                if (resultado.Codigo > 0)
                    datosMeta = (List<MetaIndividualCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }
            if (meta != null && buscar)
            {
                if (meta.Estado != null && meta.Estado.IdEntidad > 0 && buscar)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarMetaIndividual(datosMeta, meta.Estado.IdEntidad, "Estado"));
                    if (resultado.Codigo > 0)
                        datosMeta = (List<MetaIndividualCalificacion>)resultado.Contenido;
                    else
                        buscar = false;
                }

                if (meta.Prioridad != null && meta.Prioridad.IdEntidad > 0 && buscar)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarMetaIndividual(datosMeta, meta.Prioridad.IdEntidad, "Prioridad"));
                    if (resultado.Codigo > 0)
                        datosMeta = (List<MetaIndividualCalificacion>)resultado.Contenido;
                    else
                        buscar = false;
                }
            }
                        
            if (datosMeta.Count > 0 && buscar)
            {
                foreach (var item in datosMeta.Where(Q => Q.CatMetaEstado.PK_Estado != 5).ToList()) // 5 -Anulado
                {
                    // [0] Meta
                    var metaDTO = ConvertirDatosMetaADto(item);
                    metaDTO.Funcionario = CFuncionarioL.FuncionarioGeneral(item.Funcionario);

                    // Jefe Inmediato
                    if (item.IdJefeInmediato != 0)
                    {
                        var dJefatura = intermedioDCN.BuscarFuncionarioId(item.IdJefeInmediato);
                        metaDTO.JefeInmediato = (CFuncionarioL.FuncionarioGeneral((Funcionario)dJefatura.Contenido));
                    }

                    if (item.MetaObjetivoCalificacion != null)
                        metaDTO.MetaObjetivo = CMetaObjetivoCalificacionL.ConvertirDatosMetaADto(item.MetaObjetivoCalificacion);
                    else
                        metaDTO.MetaObjetivo = new CMetaObjetivoCalificacionDTO { IdEntidad = 0, Descripcion =""};
                    respuesta.Add(metaDTO);
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }

        public CBaseDTO ModificarEstadoMeta(CMetaIndividualCalificacionDTO meta, string desEstado)
        {
            try
            {
                int idEstado = 0;
                
                switch (desEstado)
                {
                    case "En Curso":
                        idEstado = 2;
                        break;
                    case "Completado":
                        idEstado = 3;
                        break;
                    case "Cerrado":
                        idEstado = 4;
                        break;
                    case "Anulado":
                        idEstado = 5;
                        break;

                    default:
                        throw new Exception("Valor de Estado no válido " + desEstado);
                }
                 
                CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);

                MetaIndividualCalificacion metaBD = new MetaIndividualCalificacion
                {
                    PK_Meta = meta.IdEntidad,
                    FK_Estado = idEstado,
                    FecFinalizado = meta.FecFinalizado,
                    DesObservaciones = meta.DesObservaciones,
                    IndModificar = 0
                };

                var resultado = intermedio.ModificarEstadoMetaIndividual(metaBD);

                if (resultado.Codigo > 0)
                    return meta;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO IniciarMeta(CMetaIndividualCalificacionDTO meta)
        {
            try
            {
                CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);

                MetaIndividualCalificacion metaBD = new MetaIndividualCalificacion
                {
                    PK_Meta = meta.IdEntidad,
                    FK_Estado = 2,
                };

                var resultado = intermedio.IniciarMeta(metaBD);

                if (resultado.Codigo > 0)
                    return meta;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO ModificarMeta(CMetaIndividualCalificacionDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);

                respuesta = intermedio.ConsultarMetaIndividual(registro.IdEntidad);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(respuesta.Mensaje);
                }

                var metaBD = (MetaIndividualCalificacion)((CRespuestaDTO)respuesta).Contenido;
                if (registro.FecDesde.Year > 1 && registro.FecDesde != null)
                    metaBD.FecDesde = registro.FecDesde;
                if (registro.FecHasta.Year > 1 && registro.FecHasta != null)
                    metaBD.FecHasta = registro.FecHasta;
                if (registro.Prioridad != null)
                    metaBD.FK_Prioridad = registro.Prioridad.IdEntidad;
                if (registro.TipoIndicador != null)
                    metaBD.FK_TipoIndicador = registro.TipoIndicador.IdEntidad;
                metaBD.DesMeta = registro.DesMeta;
                metaBD.NumIndicador = registro.NumIndicador;
                metaBD.DesIndicadorMensual = registro.DesIndicadorMensual;
                metaBD.IndEsTeletrabajable = registro.IndEsTeletrabajable;
                metaBD.PorPeso = registro.PorPeso;
                metaBD.IndModificar = 0;

                respuesta = intermedio.ModificarMetaIndividual(metaBD);

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

        public CBaseDTO HabilitarMeta(CMetaIndividualCalificacionDTO meta)
        {
            try
            {
                CMetaIndividualCalificacionD intermedio = new CMetaIndividualCalificacionD(contexto);

                MetaIndividualCalificacion metaBD = new MetaIndividualCalificacion
                {
                    PK_Meta = meta.IdEntidad,
                    IndModificar = meta.IndModificable ? 1 : 0,
                };

                var resultado = intermedio.HabilitarMeta(metaBD);

                if (resultado.Codigo > 0)
                    return meta;
                else
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<List<CBaseDTO>> CargarAsignarMeta(CPeriodoCalificacionDTO periodo,
                                                CFuncionarioDTO funcionario,
                                                CFuncionarioDTO jefatura)
        {
            List<CBaseDTO> dato = new List<CBaseDTO>();
            List<CBaseDTO> datoObjetivo = new List<CBaseDTO>();

            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CMetaIndividualCalificacionDTO datosMeta = new CMetaIndividualCalificacionDTO();
            CSeccionDTO datosSeccion = new CSeccionDTO { IdEntidad = -1, NomSeccion = "" };

            CFuncionarioL intermedioFuncionario = new CFuncionarioL();

            CMetaObjetivoCalificacionL intermedioObjetivo = new CMetaObjetivoCalificacionL();
            CCatMetaPrioridadL intermedioPrioridad = new CCatMetaPrioridadL();
            CTipoIndicadorMetaL intermedioTipoIndicador = new CTipoIndicadorMetaL();

            try
            {
                //  Funcionario
                var datoFuncionario = intermedioFuncionario.BuscarFuncionarioBase(funcionario.Cedula);
                datosMeta.Funcionario = (CFuncionarioDTO)datoFuncionario;

                // Jefe
                datoFuncionario = intermedioFuncionario.BuscarFuncionarioBase(jefatura.Cedula);
                datosMeta.JefeInmediato = (CFuncionarioDTO)datoFuncionario;

                // Metas del POI
                var objetivo = new CObjetivoCalificacionDTO { Periodo = periodo };
                var busqueda = intermedioObjetivo.BuscarMetas(datosMeta.JefeInmediato, objetivo, null, null, null, null);
                if (busqueda[0][0].GetType() != typeof(CErrorDTO) || busqueda.FirstOrDefault().GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in busqueda.ToList())
                    {
                        datoObjetivo.Add(item[0]);
                    }

                    datosSeccion = ((CMetaObjetivoCalificacionDTO)busqueda[0].FirstOrDefault()).Objetivo.Seccion;
                }

                //[0][0]   Meta Individual
                dato.Add(datosMeta);

                //[0][1]   Sección
                dato.Add(datosSeccion);

                respuesta.Add(dato);

                //[1][0]   Metas del POI
                respuesta.Add(datoObjetivo);

                //[2][0]   Listado de Prioridades
                respuesta.Add(intermedioPrioridad.ListarPrioridades());

                //[3][0]   Listado de Tipo de Indicador
                respuesta.Add(intermedioTipoIndicador.RetornarTipos());

            }
            catch (Exception error)
            {
                respuesta.Clear();
                dato.Add(new CErrorDTO
                {
                    IdEntidad = -1,
                    Mensaje = error.Message,
                    MensajeError = error.InnerException != null ? error.InnerException.Message : ""
                });
                respuesta.Add(dato);
            }

            return respuesta;
        }

        #endregion
    }
}