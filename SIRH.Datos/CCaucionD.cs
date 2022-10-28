using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos.Helpers;

namespace SIRH.Datos
{
    public class CCaucionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCaucionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CRespuestaDTO AgregarCaucion(Funcionario funcionario, Caucion caucion)
        {
            CRespuestaDTO respuesta;
            try
            {
                funcionario = entidadBase.Funcionario.Include("Nombramiento").Include("Nombramiento.Puesto")
                    .Include("Nombramiento.Caucion")
                    .FirstOrDefault(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario);

                if (funcionario != null)
                {
                   //datos nombramiento es igual a la tabla funcionario luego a tabla nombramiento en el atributo fecha vence es null.
                    var datosNombramiento = funcionario.
                                            Nombramiento.
                                            Where(N => N.FecVence == null || N.FecVence >= DateTime.Now).OrderByDescending(Q => Q.FecRige).FirstOrDefault();

                    //datos caucion es tabla datosnombramiento, luego tabla caucion en atributo indEstadoPoliza igual a 1
                    var datosCaucion = datosNombramiento.Caucion.ToList();

                    if (datosCaucion.Count() > 0)
                    {
                        var polizasRepetidas = datosCaucion.Where(C => C.NumPoliza.ToUpper() == caucion.NumPoliza.ToUpper() && C.IndEstadoPoliza != 3);
                        if (polizasRepetidas.Count() > 0)
                        {
                            throw new Exception("Ya existe una póliza activa, por activar o vencida con el mismo número suministrado.");
                        }
                        else
                        {
                            var polizasActivas = datosCaucion.Where(C => C.IndEstadoPoliza == 1);
                            if (polizasActivas.Count() > 0)
                            {
                                var datosPorActivar = datosCaucion.Where(C => C.IndEstadoPoliza == 2);
                                if (datosPorActivar.Count(Q => Q.FecEmision <= caucion.FecEmision && Q.FecVence >= caucion.FecVence) > 0 || polizasActivas.Count(Q => Q.FecEmision <= caucion.FecEmision && Q.FecVence >= caucion.FecVence) > 0)
                                {
                                    throw new Exception("Ya existe una póliza activa o por activar dentro de las mismas fechas suministradas, por favor revise los datos ingresados.");
                                }
                                else
                                {
                                    caucion.IndEstadoPoliza = 2;
                                    datosNombramiento.Caucion.Add(caucion);
                                    entidadBase.SaveChanges();
                                }
                            }
                            else
                            {
                                if (caucion.FecVence < DateTime.Today)
                                {
                                    caucion.IndEstadoPoliza = 4;
                                    datosNombramiento.Caucion.Add(caucion);
                                    entidadBase.SaveChanges();
                                }
                                else
                                {
                                    caucion.IndEstadoPoliza = 1;
                                    datosNombramiento.Caucion.Add(caucion);
                                    entidadBase.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (caucion.FecVence < DateTime.Today)
                        {
                            caucion.IndEstadoPoliza = 4;
                            datosNombramiento.Caucion.Add(caucion);
                            entidadBase.SaveChanges();
                        }
                        else
                        {
                            caucion.IndEstadoPoliza = 1;
                            datosNombramiento.Caucion.Add(caucion);
                            entidadBase.SaveChanges();
                        }
                    }

                    datosNombramiento.Caucion.Add(caucion);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO 
                    {
                        Codigo = 1,
                        Contenido = caucion.PK_Caucion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el funcionario indicado");
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

        public CRespuestaDTO ObtenerCaucion(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                //1.BD     //2.Tabla base y tablas asociadas
                var caucion = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                 .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                 //3 Acción      //4. Consulta y parametrización
                                                 .FirstOrDefault(K => K.PK_Caucion == codigo);


                if (caucion != null)
                {
                    respuesta = new CRespuestaDTO 
                    {
                        Codigo = 1,
                        Contenido = caucion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ninguna caución.");
                    //respuesta = new CRespuestaDTO
                    //{
                    //    Codigo = -1,
                    //    Contenido = new CErrorDTO { MensajeError =  }
                    //};
                    //return respuesta;
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

        public CRespuestaDTO BuscarCauciones(List<Caucion> datosPrevios, object parametro, string elemento)
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

        private List<Caucion> CargarDatos(string elemento, List<Caucion> datosPrevios, object parametro)
        {
            string param  =  "";
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString();
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
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario 
                                                        == param).ToList();
                        break;
                    case "FechaEmision":
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.FecEmision >= paramFechaInicio && 
                                                        C.FecEmision <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "FechaVence":
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.FecVence >= paramFechaInicio &&
                                                        C.FecVence <= paramFechaFinal).ToList();
                        break;
                    case "Numero":
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.NumPoliza == param).ToList();
                        break;
                    case "Estado":
                        int param1 = 0;
                        switch (param)
                        {
                            case "Activa":
                                param1 = 1;
                                break;
                            case "Por Activar":
                                param1 = 2;
                                break;
                            case "Anulada":
                                param1 = 3;
                                break;
                            case "Vencida":
                                param1 = 4;
                                break;
                            default:
                                break;
                        }
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.IndEstadoPoliza == param1).ToList();
                        break;
                    case "Puesto":
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.Nombramiento.Puesto.CodPuesto
                                                        == param).ToList();
                        break;
                    case "Nivel":
                        datosPrevios = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Include("Nombramiento.Puesto")
                                                    .Where(C => C.MontoCaucion.DesMontoCaucion.Contains(param)).ToList();
                        break;
                    default:
                        datosPrevios = new List<Caucion>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.Nombramiento.Funcionario.IdCedulaFuncionario == param).ToList();
                        break;
                    case "FechaEmision":
                        datosPrevios = datosPrevios.Where(C => C.FecEmision >= paramFechaInicio &&
                                                        C.FecEmision <= paramFechaFinal).ToList();
                        break;
                    case "FechaVence":
                        datosPrevios = datosPrevios.Where(C => C.FecVence >= paramFechaInicio &&
                                                        C.FecVence <= paramFechaFinal).ToList();
                        break;
                    case "Numero":
                        datosPrevios = datosPrevios.Where(C => C.NumPoliza == param).ToList();
                        break;
                    default:
                        datosPrevios = new List<Caucion>();
                        break;
                }
            }

            return datosPrevios;
        }

        public CRespuestaDTO AnularCaucion(Caucion caucion)
        {
            CRespuestaDTO respuesta;

            try
            {
                var caucionOld = entidadBase.Caucion.Include("EntidadSeguros").Include("MontoCaucion")
                                                    .Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Where(C => C.PK_Caucion == caucion.PK_Caucion).FirstOrDefault();

                if (caucionOld != null)
                {
                    caucionOld.IndEstadoPoliza = 3;
                    caucionOld.ObsPoliza = caucion.ObsPoliza;
                    caucion = caucionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = caucion.PK_Caucion
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ninguna caución con el código especificado." }
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

        public CRespuestaDTO ActualizarVencimientoPolizas(DateTime fechaVencimiento)
        {
            CRespuestaDTO respuesta;
            List<Caucion> caucionesActualizadas = new List<Caucion>();

            try
            {
                var caucionesVencidas = entidadBase.Caucion.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                                    .Where(C => C.FecVence <= fechaVencimiento && C.IndEstadoPoliza != 4 && C.IndEstadoPoliza != 3).ToList();
                if (caucionesVencidas.Count > 0)
                {
                    foreach (var item in caucionesVencidas)
                    {
                        item.IndEstadoPoliza = 4;
                        entidadBase.SaveChanges();
                        caucionesActualizadas.Add(item);
                    }
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = caucionesActualizadas
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "ATENCIÓN: No hay cauciones cuyo vencimiento se alcance el día de hoy" }
                    };

                    return respuesta;
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

        public CRespuestaDTO PolizasPorVencer(DateTime fechaVencimiento)
        {
            CRespuestaDTO respuesta;

            try
            {
                DateTime fechaQuince = fechaVencimiento.Date.AddDays(15);
                DateTime fechaTres = fechaVencimiento.Date.AddDays(3);

                var caucionesVencidas = entidadBase.Caucion.Include("Nombramiento").Include("Nombramiento.Funcionario")
                                            .Where(C => ((C.FecVence >= fechaVencimiento && C.FecVence <= fechaQuince) ||
                                                        C.FecVence == fechaTres) && C.IndEstadoPoliza == 1).ToList();
                if (caucionesVencidas.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = caucionesVencidas
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "ATENCIÓN: No hay cauciones cuyo vencimiento se alcance el día de hoy" }
                    };

                    return respuesta;
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

        #endregion


    }
}
