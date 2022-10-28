using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMetaIndividualInformeD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CMetaIndividualInformeD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Métodos

        public CRespuestaDTO InsertarInforme(MetaIndividualInforme Informe)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (Informe != null)
                {
                    entidadBase.MetaIndividualInforme.Add(Informe);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Informe
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

        public CRespuestaDTO ActualizarInforme(MetaIndividualInforme Informe)
        {

            CRespuestaDTO respuesta;
            var InformeActualizar = entidadBase.MetaIndividualInforme.
                                       Where(e => e.PK_Informe == Informe.PK_Informe).FirstOrDefault();

            InformeActualizar.DesObservaciones = Informe.DesObservaciones != null ? Informe.DesObservaciones : "";
            InformeActualizar.IndEstado = Informe.IndEstado;
           
            int resultado = entidadBase.SaveChanges();
            if (resultado > 0)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = InformeActualizar.PK_Informe
                };
            }
            else
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = "No se encontró la Informe a modificar." }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ConsultarInforme(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var Informe = entidadBase.MetaIndividualInforme.Where(E => E.PK_Informe == codigo).FirstOrDefault();

                if (Informe != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Informe
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna Informe" }
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

        public CRespuestaDTO BuscarInforme(List<MetaIndividualInforme> datosPrevios, object parametro, string elemento, bool busquedaAnterior)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatos(elemento, datosPrevios, parametro, busquedaAnterior);
                if (datosPrevios.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosPrevios
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

        private List<MetaIndividualInforme> CargarDatos(string elemento, List<MetaIndividualInforme> datosPrevios, object parametro, bool busquedaAnterior)
        {
            string param = "";
            decimal dparam = 0;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString().ToUpper();
            }
            else
            {
                if (parametro.GetType().Name.Equals("Decimal"))
                {
                    dparam = Convert.ToDecimal(parametro);
                }
                else
                {
                    paramFechaInicio = ((List<DateTime>)parametro).ElementAt(0);
                    paramFechaFinal = ((List<DateTime>)parametro).ElementAt(1);
                }
            }

            if (datosPrevios.Count < 1 && ! busquedaAnterior)
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = entidadBase.MetaIndividualInforme
                                                    .Include("MetaIndividualCalificacion")
                                                    .Include("MetaIndividualCalificacion.PeriodoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion.ObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.Funcionario")
                                                    .Where(C => C.MetaIndividualCalificacion.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Periodo":
                        datosPrevios = entidadBase.MetaIndividualInforme
                                                    .Include("MetaIndividualCalificacion")
                                                    .Include("MetaIndividualCalificacion.PeriodoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion.ObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.Funcionario")
                                                    .Where(C => C.MetaIndividualCalificacion.FK_PeriodoCalificacion == dparam).ToList();
                        break;

                    case "Meta":
                        datosPrevios = entidadBase.MetaIndividualInforme
                                                    .Include("MetaIndividualCalificacion")
                                                    .Include("MetaIndividualCalificacion.PeriodoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.MetaObjetivoCalificacion.ObjetivoCalificacion")
                                                    .Include("MetaIndividualCalificacion.Funcionario")
                                                    .Where(C => C.MetaIndividualCalificacion.PK_Meta == dparam).ToList();
                        break;

                    case "Descripcion":
                        datosPrevios = entidadBase.MetaIndividualInforme
                                                    .Where(C => C.DesInforme.Contains(param)).ToList();
                        break;

                    case "Fecha":
                        datosPrevios = entidadBase.MetaIndividualInforme
                                                    .Where(C => C.FecMes >= paramFechaInicio &&
                                                        C.FecMes <= paramFechaFinal).ToList();
                        break;

                    default:
                        datosPrevios = new List<MetaIndividualInforme>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.MetaIndividualCalificacion.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Periodo":
                        datosPrevios = datosPrevios.Where(C => C.MetaIndividualCalificacion.FK_PeriodoCalificacion == dparam).ToList();
                        break;

                    case "Meta":
                        datosPrevios = datosPrevios.Where(C => C.MetaIndividualCalificacion.PK_Meta == dparam).ToList();
                        break;

                    case "Descripcion":
                        datosPrevios = datosPrevios.Where(C => C.DesInforme.Contains(param)).ToList();
                        break;

                    case "Fecha":
                        datosPrevios = datosPrevios.Where(C => C.FecMes >= paramFechaInicio &&
                                                        C.FecMes <= paramFechaFinal).ToList();
                        break;
            
                    default:
                        datosPrevios = new List<MetaIndividualInforme>();
                        break;
                }
            }

            datosPrevios = datosPrevios.Where(C => C.IndEstado == 1).ToList();
            return datosPrevios;
        }

        #endregion
    }
}