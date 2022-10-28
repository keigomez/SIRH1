using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CActividadFuncionarioD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CActividadFuncionarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos

        public CRespuestaDTO InsertarActividadFuncionario(ActividadFuncionario actividad)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (actividad != null)
                {
                    entidadBase.ActividadFuncionario.Add(actividad);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = actividad
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

        public CRespuestaDTO ActualizarActividad(ActividadFuncionario actividad)
        {

            CRespuestaDTO respuesta;
            var actividadActualizar = entidadBase.ActividadFuncionario.
                                       Where(a => a.PK_Actividad == actividad.PK_Actividad).FirstOrDefault();

            actividadActualizar.DesObservacion = actividad.DesObservacion != null ? actividad.DesObservacion : "";
            actividadActualizar.IndEstado = actividad.IndEstado;

            int resultado = entidadBase.SaveChanges();
            if (resultado > 0)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = actividadActualizar.PK_Actividad
                };
            }
            else
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = "No se encontró la Actividad a modificar." }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ConsultarActividadFuncionario(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var actividad = entidadBase.ActividadFuncionario
                                            .Include("Funcionario")
                                            .Where(A => A.PK_Actividad == codigo).FirstOrDefault();
                if (actividad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = actividad
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna actividad" }
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

        public CRespuestaDTO BuscarActividadFuncionario(List<ActividadFuncionario> datosPrevios, object parametro, string elemento)
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

        private List<ActividadFuncionario> CargarDatos(string elemento, List<ActividadFuncionario> datosPrevios, object parametro)
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
                    case "Cedula": //Cedula                        
                        datosPrevios = entidadBase.ActividadFuncionario
                                                    .Include("Funcionario")
                                                    .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "Fecha":
                        datosPrevios = entidadBase.ActividadFuncionario
                                                    .Where(C => C.FecDesde >= paramFechaInicio &&
                                                        C.FecHasta <= paramFechaFinal).ToList();
                        break;
                                   
                    default:
                        datosPrevios = new List<ActividadFuncionario>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Funcionario": //Cedula
                        datosPrevios = datosPrevios.Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;
                    case "Fecha":
                        datosPrevios = datosPrevios.Where(C => C.FecDesde >= paramFechaInicio && C.FecHasta <= paramFechaFinal).ToList();
                        break;
                    default:
                        datosPrevios = new List<ActividadFuncionario>();
                        break;
                }
            }

            return datosPrevios;
        }

        #endregion
    }
}
