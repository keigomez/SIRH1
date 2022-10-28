using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CObjetivoCalificacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CObjetivoCalificacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos

        public CRespuestaDTO InsertarObjetivoCalificacion(ObjetivoCalificacion objetivo)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (objetivo != null)
                {
                    entidadBase.ObjetivoCalificacion.Add(objetivo);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = objetivo
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("El objetivo es inválido");
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

        public CRespuestaDTO ConsultarObjetivoCalificacion(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var objetivo = entidadBase.ObjetivoCalificacion.Where(O => O.PK_ObjetivoCalificacion == codigo).FirstOrDefault();

                if (objetivo != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = objetivo
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningún Objetivo" }
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

        public CRespuestaDTO BuscarObjetivoCalificacion(List<ObjetivoCalificacion> datosPrevios, object parametro, string elemento)
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

        private List<ObjetivoCalificacion> CargarDatos(string elemento, List<ObjetivoCalificacion> datosPrevios, object parametro)
        {
            string param = "";
            int iparam = 0;

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString();
            }
            else
            {
                iparam = Convert.ToInt32(parametro);
            }


            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "Anio":
                        datosPrevios = entidadBase.ObjetivoCalificacion.Where(C => C.PeriodoCalificacion.PK_PeriodoCalificacion == iparam && C.IndEstado == 1).ToList();
                        break;
                    case "Seccion":
                        datosPrevios = entidadBase.ObjetivoCalificacion.Where(C => C.Seccion.PK_Seccion == iparam && C.IndEstado == 1).ToList();
                        break;
                    case "Descripcion":
                        datosPrevios = entidadBase.ObjetivoCalificacion.Where(C => C.DesObjetivo.Contains(param) && C.IndEstado == 1).ToList();
                        break;
                    default:
                        datosPrevios = new List<ObjetivoCalificacion>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Anio":
                        datosPrevios = datosPrevios.Where(C => C.PeriodoCalificacion.PK_PeriodoCalificacion == iparam && C.IndEstado == 1).ToList();
                        break;
                    case "Seccion":
                        datosPrevios = datosPrevios.Where(C => C.Seccion.PK_Seccion == iparam && C.IndEstado == 1).ToList();
                        break;
                    case "Descripcion":
                        datosPrevios = datosPrevios.Where(C => C.DesObjetivo.Contains(param) && C.IndEstado == 1).ToList();
                        break;
                    default:
                        datosPrevios = new List<ObjetivoCalificacion>();
                        break;
                }
            }

            return datosPrevios;
        }

        #endregion
    }
}
