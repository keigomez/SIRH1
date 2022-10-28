using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCalificacionObservacionesD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCalificacionObservacionesD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos

        public CRespuestaDTO InsertarObservaciones(CalificacionObservaciones observacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (observacion != null)
                {
                    entidadBase.CalificacionObservaciones.Add(observacion);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = observacion
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

        public CRespuestaDTO ActualizarObservacion(CalificacionObservaciones observacion)
        {

            CRespuestaDTO respuesta;
            var observacionActualizar = entidadBase.CalificacionObservaciones.
                                       Where(e => e.PK_Observacion == observacion.PK_Observacion).FirstOrDefault();

            //observacionActualizar.DesObservacion = observacion.DesObservacion != null ? observacion.DesObservacion : "";
            observacionActualizar.IndEstado = observacion.IndEstado;

            int resultado = entidadBase.SaveChanges();
            if (resultado > 0)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = observacionActualizar.PK_Observacion
                };
            }
            else
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = "No se encontró la Observación a anular." }
                };
            }
            return respuesta;
        }

        public CRespuestaDTO ConsultarCalificacionObservaciones(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var observacion = entidadBase.CalificacionObservaciones.Where(E => E.PK_Observacion == codigo).FirstOrDefault();

                if (observacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = observacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna Observación" }
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

        public CRespuestaDTO BuscarCalificacionObservaciones(List<CalificacionObservaciones> datosPrevios, object parametro, string elemento)
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

        private List<CalificacionObservaciones> CargarDatos(string elemento, List<CalificacionObservaciones> datosPrevios, object parametro)
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

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "Cedula":
                        var funcionario = entidadBase.Funcionario.Where(Q => Q.IdCedulaFuncionario == param).FirstOrDefault();
                        if(funcionario != null)
                            datosPrevios = entidadBase.CalificacionObservaciones
                                                    .Where(C => C.CalificacionNombramientoFuncionarios.FK_Funcionario== funcionario.PK_Funcionario).ToList();
                        break;
                        
                    case "Descripcion":
                        datosPrevios = entidadBase.CalificacionObservaciones
                                                    .Where(C => C.DesObservacion.Contains(param)).ToList();
                        break;
                    case "Fecha":
                        datosPrevios = entidadBase.CalificacionObservaciones
                                                    .Where(C => C.FecRegistro >= paramFechaInicio &&
                                                        C.FecRegistro <= paramFechaFinal).ToList();
                        break;
                   
                    default:
                        datosPrevios = new List<CalificacionObservaciones>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        var funcionario = entidadBase.Funcionario.Where(Q => Q.IdCedulaFuncionario == param).FirstOrDefault();
                        if (funcionario != null)
                            datosPrevios = datosPrevios
                                                    .Where(C => C.CalificacionNombramientoFuncionarios.FK_Funcionario == funcionario.PK_Funcionario).ToList();
                        break;

                    case "Descripcion":
                        datosPrevios = datosPrevios.Where(C => C.DesObservacion.Contains(param)).ToList();
                        break;
                    case "Fecha":
                        datosPrevios = datosPrevios.Where(C => C.FecRegistro >= paramFechaInicio &&
                                                        C.FecRegistro <= paramFechaFinal).ToList();
                        break;
                    default:
                        datosPrevios = new List<CalificacionObservaciones>();
                        break;
                }
            }

            datosPrevios = datosPrevios.Where(C => C.IndEstado == 1).ToList();
            return datosPrevios;
        }

        #endregion
    }
}
