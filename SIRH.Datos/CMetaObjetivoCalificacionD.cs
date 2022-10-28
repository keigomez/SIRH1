using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMetaObjetivoCalificacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CMetaObjetivoCalificacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos
        public CRespuestaDTO InsertarMetaObjetivoCalificacion(MetaObjetivoCalificacion meta)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (meta != null)
                {
                    entidadBase.MetaObjetivoCalificacion.Add(meta);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = meta
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Los parámetros son inválidos");
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

        public CRespuestaDTO ConsultarMetaObjetivoCalificacion(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var meta = entidadBase.MetaObjetivoCalificacion
                                        .Include("ObjetivoCalificacion")
                                        .Include("ObjetivoCalificacion.Seccion")
                                        .Where(M => M.PK_Meta == codigo).FirstOrDefault();

                if (meta != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = meta
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna Meta de Objetivo" }
                    };
                    return respuesta;
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

        public CRespuestaDTO ObtenerUbicacion(int idSeccion)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.UbicacionAdministrativa
                                        .Where(Q => Q.FK_Seccion == idSeccion).FirstOrDefault();

                if (dato != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna Ubicación" }
                    };
                    return respuesta;
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
        public CRespuestaDTO BuscarMetaObjetivoCalificacion(List<MetaObjetivoCalificacion> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatos(elemento, datosPrevios, parametro);
                if (datosPrevios.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosPrevios.Where(Q => Q.IndEstado == 1).ToList()
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontraron resultados para los parámetros de búsqueda establecidos" }
                    };
                    return respuesta;
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

        private List<MetaObjetivoCalificacion> CargarDatos(string elemento, List<MetaObjetivoCalificacion> datosPrevios, object parametro)
        {
            string param = "";
            int paramInt = 0;
            int idSeccion = 0;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString().ToUpper();
            }
            else if (parametro.GetType().Name.Equals("Int32"))
            {
                paramInt = Convert.ToInt32(parametro);
            }
            else
            {
                paramFechaInicio = ((List<DateTime>)parametro).ElementAt(0);
                paramFechaFinal = ((List<DateTime>)parametro).ElementAt(1);
            }

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "Objetivo":
                        if (paramInt != 0)
                            datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                        .Include("ObjetivoCalificacion")
                                                        .Include("ObjetivoCalificacion.Seccion")
                                                        .Include("ObjetivoCalificacion.Seccion.CalificacionEncargado")
                                                        .Where(C => C.ObjetivoCalificacion.PK_ObjetivoCalificacion == paramInt && C.ObjetivoCalificacion.IndEstado == 1).ToList();
                        break;

                    case "Periodo":
                        if (paramInt != 0)
                            datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                        .Include("ObjetivoCalificacion")
                                                        .Include("ObjetivoCalificacion.Seccion")
                                                        .Include("ObjetivoCalificacion.Seccion.CalificacionEncargado")
                                                        .Where(C => C.ObjetivoCalificacion.FK_PeriodoCalificacion == paramInt && C.ObjetivoCalificacion.IndEstado == 1).ToList();
                        break;

                    case "Seccion":
                        if (paramInt != 0)
                            datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                        .Include("ObjetivoCalificacion")
                                                        .Include("ObjetivoCalificacion.Seccion")
                                                        .Include("ObjetivoCalificacion.Seccion.CalificacionEncargado")
                                                        .Where(C => C.ObjetivoCalificacion.Seccion.PK_Seccion == paramInt).ToList();
                        break;

                    case "Cedula": //Cedula

                        var datosSeccion = entidadBase.CalificacionEncargado
                                                      .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param) && C.IndEstado == 1)
                                                      .Select(x => x.FK_Seccion);

                        //if(datosSeccion != null)
                        //    idSeccion = datosSeccion.FirstOrDefault();
                        
                        datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                       .Include("ObjetivoCalificacion")
                                                       .Include("ObjetivoCalificacion.Seccion")
                                                       .Include("ObjetivoCalificacion.Seccion.CalificacionEncargado")
                                                       .Where(C => datosSeccion.Contains(C.ObjetivoCalificacion.Seccion.PK_Seccion))
                                                       //.Where(C => C.ObjetivoCalificacion.Seccion.PK_Seccion == idSeccion)
                                                       .ToList();
                        break;

                    case "FechaInicio":
                        datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                    .Include("ObjetivoCalificacion")
                                                    .Include("ObjetivoCalificacion.Seccion")
                                                    .Where(C => C.FecInicio >= paramFechaInicio && C.FecInicio <= paramFechaFinal).ToList();
                        break;
                    case "FechaFinal":
                        datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                   .Include("ObjetivoCalificacion")
                                                   .Include("ObjetivoCalificacion.Seccion")
                                                   .Where(C => C.FecFinalizacion >= paramFechaInicio && C.FecFinalizacion <= paramFechaFinal).ToList();
                        break;
                    case "TipMeta":
                        if (paramInt != 0)
                            datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                       .Include("ObjetivoCalificacion")
                                                       .Include("ObjetivoCalificacion.Seccion")
                                                       .Where(C => C.TipMeta == paramInt).ToList();
                        break;
                    case "Estado":
                        datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                  .Include("ObjetivoCalificacion")
                                                  .Include("ObjetivoCalificacion.Seccion")
                                                  .Where(C => C.IndEstado == paramInt).ToList();
                        break;

                    case "Meta":
                        datosPrevios = entidadBase.MetaObjetivoCalificacion
                                                  .Include("ObjetivoCalificacion")
                                                  .Include("ObjetivoCalificacion.Seccion")
                                                  .Where(C => C.PK_Meta == paramInt).ToList();
                        break;
                    //case "Observaciones":
                    //    datosPrevios = entidadBase.MetaObjetivoCalificacion.Include("Funcionario").Include("ObjetivoInstitucional").Include("UbicacionAdministrativa")
                    //                                   .Where(C => C.DesObservaciones.Contains(param)).ToList();
                    //    break;

                    default:
                        datosPrevios = new List<MetaObjetivoCalificacion>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Objetivo":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.ObjetivoCalificacion.PK_ObjetivoCalificacion == paramInt && C.IndEstado == 1).ToList();
                        break;

                    case "Periodo":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.ObjetivoCalificacion.FK_PeriodoCalificacion == paramInt && C.IndEstado == 1).ToList();
                        break;

                    case "Seccion":
                        datosPrevios = datosPrevios.Where(C => C.ObjetivoCalificacion.Seccion.PK_Seccion == paramInt && C.IndEstado == 1).ToList();
                        break;

                    case "Cedula": //Cedula
                        var datosSeccion = entidadBase.CalificacionEncargado
                                                      .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param) && C.IndEstado == 1)
                                                      .Select(x => x.FK_Seccion);

                        //if (datosSeccion != null)
                        //    idSeccion = datosSeccion.FirstOrDefault();

                        datosPrevios = datosPrevios.Where(C => datosSeccion.Contains(C.ObjetivoCalificacion.Seccion.PK_Seccion)).ToList();
                        //datosPrevios = datosPrevios.Where(C => C.ObjetivoCalificacion.Seccion.PK_Seccion == idSeccion && C.IndEstado == 1).ToList();
                        break;

                    case "FechaInicio":
                        datosPrevios = datosPrevios.Where(C => C.FecInicio >= paramFechaInicio &&
                                                        C.FecInicio <= paramFechaFinal && C.IndEstado == 1).ToList();
                        break;
                    case "FechaFinal":
                        datosPrevios = datosPrevios.Where(C => C.FecFinalizacion >= paramFechaInicio &&
                                                        C.FecFinalizacion <= paramFechaFinal && C.IndEstado == 1).ToList();
                        break;
                    case "TipMeta":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.TipMeta == paramInt && C.IndEstado == 1).ToList();
                        break;
                    case "Estado":
                        datosPrevios = datosPrevios.Where(C => C.IndEstado == paramInt).ToList();
                        break;
                    case "Meta":
                        datosPrevios = datosPrevios.Where(C => C.PK_Meta == paramInt).ToList();
                        break;
                    //case "Observaciones":
                    //    datosPrevios = datosPrevios.Where(C => C.DesObservaciones.Contains(param)).ToList();
                    //    break;
                    default:
                        datosPrevios = new List<MetaObjetivoCalificacion>();
                        break;
                }
            }

            return datosPrevios;
        }


        public CRespuestaDTO AnularMeta(int id)
        {
            try
            {
                var meta = entidadBase.MetaObjetivoCalificacion.Where(Q => Q.PK_Meta == id).FirstOrDefault();
                if (meta != null)
                {
                    meta.IndEstado = 2;
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = meta
                    };
                }
                else
                {
                    throw new Exception("No se encontró la Meta que se intentó anular");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }
        
        public CRespuestaDTO ModificarMeta(MetaObjetivoCalificacion registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var MetaOld = entidadBase.MetaObjetivoCalificacion.Where(I => I.PK_Meta == registro.PK_Meta).FirstOrDefault();

                if (MetaOld != null)
                {
                    MetaOld.FecInicio = registro.FecInicio;
                    MetaOld.FecFinalizacion = registro.FecFinalizacion;
                    //MetaOld.IndPrioridad = registro.IndPrioridad;
                    //MetaOld.ValorAsignado = registro.ValorAsignado;
                    //MetaOld.ValorCumplido = registro.ValorCumplido;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_Meta
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna Meta con el código especificado." }
                    };
                    return respuesta;
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
