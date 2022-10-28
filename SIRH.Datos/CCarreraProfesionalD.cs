using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCarreraProfesionalD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCarreraProfesionalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos



        /// <summary>
        /// Obtiene la lista de CarreraProfesional de la BD
        /// </summary>
        /// <returns>Retorna la lista de CarreraProfesional</returns>
        public CRespuestaDTO CargarCarreraProfesional()
        {
            CRespuestaDTO respuesta;

            try {
                var carrera = entidadBase.C_EMU_CarreraProfesional.ToList();               

                if (carrera != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = carrera
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun registro de carrera profesional" }
                    };
                    return respuesta;
                }

            }
            catch(Exception error)
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
        /// Carga un registro de CarreraProfesional de la BD
        /// </summary>
        /// <returns>Retorna un registro de CarreraProfesional</returns>
        public CRespuestaDTO CargarCarreraProfesionalPorID(int id)
        {
            CRespuestaDTO respuesta;

            try
            {
                var carrera = entidadBase.C_EMU_CarreraProfesional.Where(R => R.ID == id).FirstOrDefault();

                if (carrera != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = carrera
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningun registro de carrera profesional" }
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
        /// Carga registros de CarreraProfesional de la BD por parametro
        /// </summary>
        /// <returns>Retorna registros de CarreraProfesional por parametro</returns>
        public CRespuestaDTO BuscarCarreraProfesional(List<C_EMU_CarreraProfesional> datosPrevios, object parametro, string elemento)
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

        private List<C_EMU_CarreraProfesional> CargarDatos(string elemento, List<C_EMU_CarreraProfesional> datosPrevios, object parametro)                                                            
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
                    case "Cedula":
                        datosPrevios = entidadBase.C_EMU_CarreraProfesional
                                                    .Where(C => C.Cedula.Contains(param)).ToList();
                        break;
                    case "Fecha":
                        datosPrevios = entidadBase.C_EMU_CarreraProfesional
                                                    .Where(C => C.FecRigePago >= paramFechaInicio &&
                                                        C.FecRigePago <= paramFechaFinal).ToList();
                        break;
                    case "Puesto":
                        if(paramInt != 0)
                            datosPrevios = entidadBase.C_EMU_CarreraProfesional
                                                        .Where(C => C.Puesto == paramInt).ToList(); //Puesto es int
                        break;
                    case "Nombre":
                        datosPrevios = entidadBase.C_EMU_CarreraProfesional
                                                       .Where(C => C.Nombre.Contains(param)).ToList();
                        break;
                    case "Curso":
                        datosPrevios = entidadBase.C_EMU_CarreraProfesional
                                                    .Where(C => (C.Curso1 != null && C.Curso1.Contains(param)) || (C.Curso2 != null && C.Curso2.Contains(param))
                                                     || (C.Curso3 != null && C.Curso3.Contains(param)) || (C.Curso4 != null && C.Curso4.Contains(param))
                                                     || (C.Curso5 != null && C.Curso5.Contains(param)) || (C.Curso6 != null && C.Curso6.Contains(param))
                                                     || (C.Curso7 != null && C.Curso7.Contains(param)) || (C.Curso8 != null && C.Curso8.Contains(param))
                                                    ).ToList();
                        break;
                    default:
                        datosPrevios = new List<C_EMU_CarreraProfesional>();
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
                        datosPrevios = datosPrevios.Where(C => C.Fecha >= paramFechaInicio &&
                                                        C.Fecha <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "Puesto":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.Puesto == paramInt).ToList();
                        break;
                    case "Nombre":
                        datosPrevios = datosPrevios.Where(C => C.Nombre.Contains(param)).ToList();
                        break;
                    case "Curso":

                        datosPrevios = datosPrevios.Where(C =>  (C.Curso1 != null && C.Curso1.Contains(param)) || (C.Curso2 != null && C.Curso2.Contains(param))
                                                     || (C.Curso3 != null && C.Curso3.Contains(param)) || (C.Curso4 != null && C.Curso4.Contains(param)) 
                                                     || (C.Curso5 != null && C.Curso5.Contains(param)) || (C.Curso6 != null && C.Curso6.Contains(param))
                                                     || (C.Curso7 != null && C.Curso7.Contains(param)) || (C.Curso8 != null && C.Curso8.Contains(param))
                                                    ).ToList();
                        break;
                    default:
                        datosPrevios = new List<C_EMU_CarreraProfesional>();
                        break;
                }
            }

            return datosPrevios;
        }



        #endregion
    }
}


///// <summary>
///// Guardar CarreraProfesional
///// </summary>
///// <param name="CarreraProfesional"></param>
///// <returns>Devuelve la llave primaria del registro insertado</returns>
//public CRespuestaDTO GuardarCarreraProfesional(C_EMU_CarreraProfesional carrera)
//{
//    CRespuestaDTO respuesta;
//    try
//    {
//        entidadBase.C_EMU_CarreraProfesional.Add(carrera);
//        entidadBase.SaveChanges();
//        respuesta = new CRespuestaDTO
//        {
//            Codigo = 1,
//            Contenido = carrera
//        };
//        return respuesta;
//    }
//    catch (Exception error)
//    {
//        respuesta = new CRespuestaDTO
//        {
//            Codigo = -1,
//            Contenido = new CErrorDTO { MensajeError = error.Message }
//        };
//        return respuesta;
//    }
//}