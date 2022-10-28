using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Datos
{
    public class CCursoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCursoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos


        /// <summary>
        /// Obtiene la lista de Cursos de la BD
        /// </summary>
        /// <returns>Retorna la lista de Cursos</returns>
        public CRespuestaDTO CargarCursos()
        {
            CRespuestaDTO respuesta;

            try
            {
                var cursos = entidadBase.C_EMU_Cursos.ToList();     

                if (cursos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = cursos
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun registro de cursos" }
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


        /// <summary>
        /// Carga un registro de cursos de la BD por id
        /// </summary>
        /// <returns>Retorna un registro de cursos por id</returns>
        public CRespuestaDTO CargarCursoPorID(int id)
        {
            CRespuestaDTO respuesta;

            try
            {
                var curso = entidadBase.C_EMU_Cursos.Where(R => R.ID == id).FirstOrDefault();

                if (curso != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = curso
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun registro de cursos" }
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


        /// <summary>
        /// Carga registros de Cursos de la BD por parametro
        /// </summary>
        /// <returns>Retorna registros de Cursos por parametro</returns>
        public CRespuestaDTO BuscarCursos(List<C_EMU_Cursos> datosPrevios, object parametro, string elemento)
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

        private List<C_EMU_Cursos> CargarDatos(string elemento, List<C_EMU_Cursos> datosPrevios, object parametro)                                                           
        {
            string param = "";
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString().ToUpper();
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
                    case "Cedula":
                        datosPrevios = entidadBase.C_EMU_Cursos
                                                    .Where(C => C.Cedula.Contains(param)).ToList();
                        break;
                    case "Fecha":
                        datosPrevios = entidadBase.C_EMU_Cursos
                                                    .Where(C => C.FecRige >= paramFechaInicio &&
                                                        C.FecRige <= paramFechaFinal).ToList();
                        break;
                    case "Resolucion":
                        datosPrevios = entidadBase.C_EMU_Cursos
                                                        .Where(C => C.Resolucion == param).ToList();
                        break;
                    case "Nombre":
                        datosPrevios = entidadBase.C_EMU_Cursos
                                                       .Where(C => C.Nombre.Contains(param)).ToList();
                        break;
                    case "Curso":
                        datosPrevios = entidadBase.C_EMU_Cursos
                                                    .Where(C => C.NombreCurso.Contains(param)).ToList();
                        break;
                    case "TipoCurso":
                        datosPrevios = entidadBase.C_EMU_Cursos
                                                    .Where(C => C.TipoCurso.Contains(param)).ToList();
                        break;
                    default:
                        datosPrevios = new List<C_EMU_Cursos>();
                        break;
                }
            }
            else
            {
                
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.Cedula.Contains(param)).ToList();
                        break;
                    case "Fecha":
                        datosPrevios = datosPrevios.Where(C => C.FecRige >= paramFechaInicio &&
                                                        C.FecRige <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "Resolucion":
                        datosPrevios = datosPrevios.Where(C => C.Resolucion.Contains(param)).ToList();
                        break;
                    case "Nombre":
                        datosPrevios = datosPrevios.Where(C => C.Nombre.Contains(param)).ToList();
                        break;
                    case "Curso":
                        datosPrevios = datosPrevios.Where(C => C.NombreCurso.Contains(param)).ToList();
                        break;
                    case "TipoCurso":
                        datosPrevios = datosPrevios.Where(C => C.TipoCurso.Contains(param)).ToList();
                        break;
                    default:
                        datosPrevios = new List<C_EMU_Cursos>();
                        break;
                }
            }

            return datosPrevios;
        }

        #endregion
    }
}
