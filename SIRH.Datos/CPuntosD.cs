using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Datos
{
    public class CPuntosD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPuntosD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos


        /// <summary>
        /// Obtiene la lista de datos de puntos de la BD
        /// </summary>
        /// <returns>Retorna la lista de datos de punto</returns>
        public CRespuestaDTO CargarDatosPuntos()
        {
            CRespuestaDTO respuesta;

            try
            {
                var puntos = entidadBase.C_EMU_Puntos.ToList();              

                if (puntos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = puntos
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun registro de puntos" }
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
        /// Carga un registro de de los datos de puntos de la BD
        /// </summary>
        /// <returns>Retorna un registro de datos de puntos</returns>
        public CRespuestaDTO CargarDatosPuntosPorId(int id)
        {
            CRespuestaDTO respuesta;

            try
            {
                var puntos = entidadBase.C_EMU_Puntos.Where(R => R.ID == id).FirstOrDefault();

                if (puntos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = puntos
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun registro de puntos" }
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
        /// Carga registros de puntos de la BD por parametro
        /// </summary>
        /// <returns>Retorna registros de puntos por parametro</returns>
        public CRespuestaDTO BuscarDatosPuntos(List<C_EMU_Puntos> datosPrevios, object parametro, string elemento)
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

        private List<C_EMU_Puntos> CargarDatos(string elemento, List<C_EMU_Puntos> datosPrevios, object parametro)
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
                        datosPrevios = entidadBase.C_EMU_Puntos
                                                    .Where(C => C.Cedula.Contains(param)).ToList();
                        break;
                    case "Nombre":
                        datosPrevios = entidadBase.C_EMU_Puntos
                                                       .Where(C => C.Nombre.Contains(param)).ToList();
                        break;
                    default:
                        datosPrevios = new List<C_EMU_Puntos>();
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
                    case "Nombre":
                        datosPrevios = datosPrevios.Where(C => C.Nombre.Contains(param)).ToList();
                        break;
                    default:
                        datosPrevios = new List<C_EMU_Puntos>();
                        break;
                }
            }

            return datosPrevios;
        }

        #endregion
    }
}
