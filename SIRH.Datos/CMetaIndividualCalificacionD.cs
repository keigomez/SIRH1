using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMetaIndividualCalificacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CMetaIndividualCalificacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Métodos
        public CRespuestaDTO InsertarMetaIndividual(MetaIndividualCalificacion dato)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (dato != null)
                {
                    entidadBase.MetaIndividualCalificacion.Add(dato);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato
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

        public CRespuestaDTO ConsultarMetaIndividual(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var dato = entidadBase.MetaIndividualCalificacion
                                        .Include("CatMetaEstado")
                                        .Include("CatMetaPrioridad")
                                        .Include("PeriodoCalificacion")
                                        .Include("Funcionario")
                                        .Include("MetaObjetivoCalificacion")
                                        .Where(M => M.PK_Meta == codigo).FirstOrDefault();

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
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna Meta Individual" }
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

        public CRespuestaDTO BuscarMetaIndividual(List<MetaIndividualCalificacion> datosPrevios, object parametro, string elemento)
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
                        Contenido = datosPrevios.ToList()
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

        private List<MetaIndividualCalificacion> CargarDatos(string elemento, List<MetaIndividualCalificacion> datosPrevios, object parametro)
        {
            string param = "";
            int paramInt = 0;

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
                    case "Anio":
                        datosPrevios = entidadBase.MetaIndividualCalificacion
                                        .Include("CatMetaEstado")
                                        .Include("CatMetaPrioridad")
                                        .Include("PeriodoCalificacion")
                                        .Include("Funcionario")
                                        .Include("MetaObjetivoCalificacion")
                                        .Where(C => C.PeriodoCalificacion.PK_PeriodoCalificacion == paramInt).ToList();
                        break;

                    case "Cedula": //Cedula                        
                        datosPrevios = entidadBase.MetaIndividualCalificacion
                                                    .Include("CatMetaEstado")
                                                    .Include("CatMetaPrioridad")
                                                    .Include("PeriodoCalificacion")
                                                    .Include("Funcionario")
                                                    .Include("MetaObjetivoCalificacion")
                                                    .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "FechaInicio":
                        datosPrevios = entidadBase.MetaIndividualCalificacion
                                                    .Include("CatMetaEstado")
                                                    .Include("CatMetaPrioridad")
                                                    .Include("PeriodoCalificacion")
                                                    .Include("Funcionario")
                                                    .Include("MetaObjetivoCalificacion")
                                                    .Where(C => C.FecDesde >= paramFechaInicio && C.FecDesde <= paramFechaFinal).ToList();
                        break;
                    case "FechaFinal":
                        datosPrevios = entidadBase.MetaIndividualCalificacion
                                                    .Include("CatMetaEstado")
                                                    .Include("CatMetaPrioridad")
                                                    .Include("PeriodoCalificacion")
                                                    .Include("Funcionario")
                                                    .Include("MetaObjetivoCalificacion")
                                                    .Where(C => C.FecHasta >= paramFechaInicio && C.FecHasta <= paramFechaFinal).ToList();
                        break;
                    case "Estado":
                        if (paramInt != 0)
                            datosPrevios = entidadBase.MetaIndividualCalificacion
                                                    .Include("CatMetaEstado")
                                                    .Include("CatMetaPrioridad")
                                                    .Include("PeriodoCalificacion")
                                                    .Include("Funcionario")
                                                    .Include("MetaObjetivoCalificacion")
                                                    .Where(C => C.CatMetaEstado.PK_Estado == paramInt).ToList();
                        else
                            datosPrevios = entidadBase.MetaIndividualCalificacion
                                                    .Include("CatMetaEstado")
                                                    .Include("CatMetaPrioridad")
                                                    .Include("PeriodoCalificacion")
                                                    .Include("Funcionario")
                                                    .Include("MetaObjetivoCalificacion")
                                                    .Where(C => C.CatMetaEstado.PK_Estado == 1).ToList();
                        break;

                    case "MetaObjetivo":
                        datosPrevios = entidadBase.MetaIndividualCalificacion
                                                    .Include("CatMetaEstado")
                                                    .Include("CatMetaPrioridad")
                                                    .Include("PeriodoCalificacion")
                                                    .Include("Funcionario")
                                                    .Include("MetaObjetivoCalificacion")
                                                    .Where(C => C.MetaObjetivoCalificacion.PK_Meta != 4).ToList();  // Descartada
                        break;
                    default:
                        datosPrevios = new List<MetaIndividualCalificacion>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Anio":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.PeriodoCalificacion.PK_PeriodoCalificacion == paramInt).ToList();
                        break;

                    case "Cedula": //Cedula
                        datosPrevios = datosPrevios.Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "FechaInicio":
                        datosPrevios = datosPrevios.Where(C => C.FecDesde >= paramFechaInicio && C.FecDesde <= paramFechaFinal).ToList();
                        break;

                    case "FechaFinal":
                        datosPrevios = datosPrevios.Where(C => C.FecHasta >= paramFechaInicio && C.FecHasta <= paramFechaFinal).ToList();
                        break;

                    case "Estado":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.CatMetaEstado.PK_Estado == paramInt).ToList();
                        else
                            datosPrevios = datosPrevios.Where(C => C.CatMetaEstado.PK_Estado != 4).ToList();  // Descartada
                        break;

                    case "MetaObjetivo":
                        datosPrevios = datosPrevios.Where(C => C.MetaObjetivoCalificacion.PK_Meta == paramInt).ToList();
                        break;

                    default:
                        datosPrevios = new List<MetaIndividualCalificacion>();
                        break;
                }
            }

            return datosPrevios;
        }


        public CRespuestaDTO ModificarMetaIndividual(MetaIndividualCalificacion registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var MetaIndividualOld = entidadBase.MetaIndividualCalificacion.Where(I => I.PK_Meta == registro.PK_Meta).FirstOrDefault();

                if (MetaIndividualOld != null)
                {
                    if (registro.FecDesde.Year != 1)
                        MetaIndividualOld.FecDesde = registro.FecDesde;
                    if (registro.FecHasta.Year != 1)
                        MetaIndividualOld.FecHasta = registro.FecHasta;
                    MetaIndividualOld.FK_Prioridad = registro.FK_Prioridad;
                    MetaIndividualOld.FK_TipoIndicador = registro.FK_TipoIndicador;
                    MetaIndividualOld.NumIndicador = registro.NumIndicador;
                    MetaIndividualOld.DesIndicadorMensual = registro.DesIndicadorMensual;
                    MetaIndividualOld.IndEsTeletrabajable = registro.IndEsTeletrabajable;
                    MetaIndividualOld.PorPeso = registro.PorPeso;
                    MetaIndividualOld.FecRegistro = DateTime.Now;

                    if (registro.FK_MetaObjetivo != null)
                        MetaIndividualOld.FK_MetaObjetivo = registro.FK_MetaObjetivo;

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
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún MetaIndividual con el código especificado." }
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


        public CRespuestaDTO ModificarEstadoMetaIndividual(MetaIndividualCalificacion registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var MetaIndividualOld = entidadBase.MetaIndividualCalificacion.Where(I => I.PK_Meta == registro.PK_Meta).FirstOrDefault();

                if (MetaIndividualOld != null)
                {
                    MetaIndividualOld.FK_Estado = registro.FK_Estado;
                    MetaIndividualOld.FecFinalizado = registro.FecFinalizado;
                    MetaIndividualOld.DesObservaciones = registro.DesObservaciones;

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
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún MetaIndividual con el código especificado." }
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

        public CRespuestaDTO IniciarMeta(MetaIndividualCalificacion registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var MetaIndividualOld = entidadBase.MetaIndividualCalificacion.Where(I => I.PK_Meta == registro.PK_Meta).FirstOrDefault();

                if (MetaIndividualOld != null)
                {
                    MetaIndividualOld.FK_Estado = registro.FK_Estado;
                    
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
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún MetaIndividual con el código especificado." }
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
     
        public CRespuestaDTO HabilitarMeta(MetaIndividualCalificacion registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var MetaIndividualOld = entidadBase.MetaIndividualCalificacion.Where(I => I.PK_Meta == registro.PK_Meta).FirstOrDefault();

                if (MetaIndividualOld != null)
                {
                    MetaIndividualOld.IndModificar = registro.IndModificar;

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
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún MetaIndividual con el código especificado." }
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