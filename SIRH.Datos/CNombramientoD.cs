using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using SIRH.Datos;
using SIRH.DTO;
using System.Data.Entity.Infrastructure;

namespace SIRH.Datos
{
    public class CNombramientoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CNombramientoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        internal static CNombramientoDTO ConvertirDatosNombramientoADTO(Nombramiento nombramiento)
        {
            return new CNombramientoDTO
            {
                FecRige = Convert.ToDateTime(nombramiento.FecRige),
                FecVence = Convert.ToDateTime(nombramiento.FecVence),
                FecNombramiento = Convert.ToDateTime(nombramiento.FecNombramiento)                
            };
        }

        public CRespuestaDTO FuncionarioEnNombramiento(int codNombramiento)
        {
            try
            {
                var nombramiento = entidadBase.Nombramiento.Include("Funcionario")
                                                .Where(Q => Q.PK_Nombramiento == codNombramiento).FirstOrDefault();
                if (nombramiento != null)
                {
                    return new CRespuestaDTO 
                    {
                        Codigo = 1,
                        Contenido = nombramiento.Funcionario
                    };
                }
                else
                {
                    throw new Exception("No se encontró el nombramiento indicado por parámetros");
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

        /// <summary>
        /// Guarda los Nombramientos en la BD
        /// </summary>
        /// <returns>Retorna los Nombramientos</returns>
        public int GuardarNombramiento(Nombramiento nombramientoLocal)
        {
            //DateTime fechaMaxima = DateTime.Now.AddDays(-5);
            //if (nombramientoLocal.EstadoNombramiento.PK_EstadoNombramiento == 20 || nombramientoLocal.EstadoNombramiento.PK_EstadoNombramiento == 27)
            if (nombramientoLocal.FK_EstadoNombramiento == 20 || nombramientoLocal.FK_EstadoNombramiento == 27)
            {
                var nombramientosAdicionales = entidadBase.Nombramiento.Include("Funcionario").Include("Funcionario.EstadoFuncionario")
                                               .Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)
                                               && N.FK_EstadoNombramiento != 15
                                               && N.FK_Funcionario == nombramientoLocal.FK_Funcionario)
                                               .OrderByDescending(Q => Q.FecRige).ToList();
                if (nombramientosAdicionales.Count() > 0)
                {
                    foreach (var item in nombramientosAdicionales)
                    {
                        item.FecVence = nombramientoLocal.FecRige?.AddDays(-1);
                    }
                }
            }
            entidadBase.Nombramiento.Add(nombramientoLocal);
            entidadBase.SaveChanges();
            return nombramientoLocal.PK_Nombramiento;
        }    

        public CRespuestaDTO GuardarNombramientoPuesto(Nombramiento nombramiento)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Nombramiento.Add(nombramiento);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = nombramiento
                };
                return respuesta;
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

        private DbQuery<Nombramiento> RetornarPuesto()
        {
            return RetornarCalificaciones().Include("Puesto").Include("Puesto.EstadoPuesto");
        }

        private DbQuery<Nombramiento> RetornarCalificaciones()
        {
            return entidadBase.Nombramiento.Include("CalificacionNombramiento").Include("CalificacionNombramiento.Calificacion");
        }


        public Nombramiento CargarNombramiento(int idEntidad)
        {
            Nombramiento resultado = new Nombramiento();

            resultado = entidadBase.Nombramiento.Include("EstadoNombramiento").Where(R => R.PK_Nombramiento == idEntidad).FirstOrDefault();

            return resultado;
        }

        /// <summary>
        /// Obtiene la Carga de los Nombramientos de la BD
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns>Retorna los Nombramientos</returns>
        public Nombramiento CargarNombramientoCedula(string cedula)
        {
            Nombramiento resultado = new Nombramiento();
            // listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };
            resultado = RetornarPuesto().Include("EstadoNombramiento").Where(R => (R.FecVence >= DateTime.Now || R.FecVence == null)
             /*&& listaEstados.Contains(R.FK_EstadoNombramiento)*/ && R.Funcionario.IdCedulaFuncionario == cedula).OrderByDescending(Q=>Q.FecRige).ToList().FirstOrDefault();

            return resultado;
        }

        public Nombramiento CargarNombramientoActualCedula(string cedula)
        {
            try
            {
                Nombramiento resultado = new Nombramiento();
                var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25,
                                                    26, 27, 28, 30, 33, 35, 36,37, 38, 39};
                resultado = entidadBase.Nombramiento
                                //.Include("Funcionario")
                                //.Include("Funcionario.Direccion")
                                //.Include("Funcionario.Direccion.Distrito")
                                //.Include("Funcionario.Direccion.Canton")
                                //.Include("Funcionario.Direccion.Provincia")
                                //.Include("Puesto")
                                //.Include("Puesto.DetallePuesto")
                                //.Include("Puesto.DetallePuesto.Clase")
                                //.Include("Puesto.DetallePuesto.Especialidad")
                                //.Include("Puesto.UbicacionAdministrativa")
                                //.Include("Puesto.UbicacionAdministrativa.DireccionGeneral")
                                //.Include("Puesto.UbicacionAdministrativa.Division")
                                //.Include("Puesto.UbicacionAdministrativa.Departamento")
                                //.Include("Puesto.UbicacionAdministrativa.Seccion")
                                //.Include("Puesto.RelPuestoUbicacion")
                                //.Include("Puesto.RelPuestoUbicacion.UbicacionPuesto")
                                //.Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                //.Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.Canton")
                                //.Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.Provincia")
                                .Where(N => N.Funcionario.IdCedulaFuncionario == cedula && listaEstados.Contains(N.FK_EstadoNombramiento))
                                .Where(N => N.FecVence.HasValue == false || N.FecVence >= DateTime.Now)
                                .OrderByDescending(N => N.FecRige)
                                .ToList()
                                .LastOrDefault();
                return resultado;
            }
            catch
            {
                return null;
            }
        }


        public Nombramiento CargarNombramientoActualPuesto(string codPuesto)
        {
            Nombramiento resultado = new Nombramiento();
            var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24 };
            resultado = entidadBase.Nombramiento.Where(R => R.Puesto.CodPuesto == codPuesto && listaEstados.Contains(R.FK_EstadoNombramiento)).OrderByDescending(R => R.FecRige).FirstOrDefault();
            return resultado;
        }

        public bool ActualizarEstadoNombramiento(Nombramiento nomb)
        {
            bool resultado = false;

            Nombramiento nombramientoAct = entidadBase.Nombramiento
                                            .Where(N => N.Funcionario.IdCedulaFuncionario == nomb.Funcionario.IdCedulaFuncionario)
                                            .FirstOrDefault();
            nombramientoAct.EstadoNombramiento = nomb.EstadoNombramiento;
            int respuesta = entidadBase.SaveChanges();
            if (respuesta > 0)
            {
                resultado = true;
            }

            return resultado;
        }

        public CRespuestaDTO BuscarNombramientoCodigoPuesto(string codpuesto)
        {
            CRespuestaDTO respuesta;
            try
            {
                bool estadoValido = true;
                var nombramiento = entidadBase.Nombramiento.Include("EstadoNombramiento")
                                                     .Include("Puesto")
                                                     .Include("Puesto.DetallePuesto")
                                                     .Include("Puesto.DetallePuesto.Clase")
                                                     .Include("Puesto.DetallePuesto.Especialidad")
                                                     .Include("Puesto.UbicacionAdministrativa")
                                                     .Include("Puesto.UbicacionAdministrativa.Division")
                                                     .Include("Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                     .Include("Puesto.UbicacionAdministrativa.Departamento")
                                                     .Include("Puesto.UbicacionAdministrativa.Seccion")
                                                     .Include("Puesto.UbicacionAdministrativa.Presupuesto")
                                                     .Include("Puesto.EstadoPuesto")
                                                     .Include("Funcionario")
                                                     .Include("Funcionario.EstadoFuncionario")
                                                     .Include("Funcionario.DetalleContratacion")
                                                     .Where(N => N.Puesto.CodPuesto == codpuesto
                                                            && (N.FecVence == null || N.FecVence >= DateTime.Now) && N.FK_EstadoNombramiento != 15)
                                                     .OrderByDescending(O => O.FecRige).FirstOrDefault();

                if (nombramiento != null)
                {
                    if (nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 5
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 6
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 7
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 8
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 10
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 14
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 16
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 19
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 20
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 22
                        || nombramiento.Puesto.EstadoPuesto.PK_EstadoPuesto == 24)
                    {
                        estadoValido = false;
                    }
                }

                if (nombramiento != null && estadoValido)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = nombramiento,
                        Mensaje = "Nombramiento"
                    };
                }
                else
                {
                    var puesto = entidadBase.Puesto.Include("EstadoPuesto")
                                                .Include("DetallePuesto")
                                                .Include("DetallePuesto.Clase")
                                                .Include("DetallePuesto.Especialidad")
                                                .Include("UbicacionAdministrativa")
                                                .Include("UbicacionAdministrativa.Division")
                                                .Include("UbicacionAdministrativa.DireccionGeneral")
                                                .Include("UbicacionAdministrativa.Departamento")
                                                .Include("UbicacionAdministrativa.Seccion")
                                                .Include("UbicacionAdministrativa.Presupuesto")
                                                .Where(P => P.CodPuesto == codpuesto).FirstOrDefault();

                    if (puesto != null)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = puesto,
                            Mensaje = "Puesto"
                        };
                    }
                    else
                    {
                        throw new Exception("No se encontraron datos asociados al número de puesto ingresado");
                    }
                }

                return respuesta;
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

        public CRespuestaDTO BuscarNombramientoCedulaFuncionario(string cedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var nombramiento = entidadBase.Nombramiento.Include("EstadoNombramiento")
                                                     .Include("Puesto")
                                                     .Include("Puesto.DetallePuesto")
                                                     .Include("Puesto.DetallePuesto.Clase")
                                                     .Include("Puesto.DetallePuesto.Especialidad")
                                                     .Include("Puesto.UbicacionAdministrativa")
                                                     .Include("Puesto.UbicacionAdministrativa.Division")
                                                     .Include("Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                     .Include("Puesto.UbicacionAdministrativa.Departamento")
                                                     .Include("Puesto.UbicacionAdministrativa.Seccion")
                                                     .Include("Puesto.UbicacionAdministrativa.Presupuesto")
                                                     .Include("Puesto.EstadoPuesto")
                                                     .Include("Funcionario")
                                                     .Include("Funcionario.EstadoFuncionario")
                                                     .Include("Funcionario.DetalleContratacion")
                                                     .Where(N => N.Funcionario.IdCedulaFuncionario == cedula
                                                            && (N.FecVence == null || N.FecVence >= DateTime.Now) && N.FK_EstadoNombramiento != 15)
                                                     .OrderByDescending(O => O.FecRige).FirstOrDefault();

                if (nombramiento != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = nombramiento,
                        Mensaje = "Nombramiento"
                    };
                }
                else
                {
                    var funcionario = entidadBase.Funcionario.Include("EstadoFuncionario")
                                                .Include("DetalleContratacion")
                                                .Where(P => P.IdCedulaFuncionario == cedula).FirstOrDefault();

                    if (funcionario != null)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = funcionario,
                            Mensaje = "Funcionario"
                        };
                    }
                    else
                    {
                        throw new Exception("No se encontraron datos asociados al número de cédula ingresado");
                    }
                }

                return respuesta;
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

        public CRespuestaDTO ListarEstadosNombramiento()
        {
            CRespuestaDTO respuesta;
            try
            {
                var estados = entidadBase.EstadoNombramiento.ToList();
                if (estados != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = estados
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron estados de nombramiento");
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

        public CRespuestaDTO BuscarHistorialNombramiento(List<Nombramiento> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta; 
            try
            {
                datosPrevios = CargarDatosNombramiento(elemento, datosPrevios, parametro);
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

        private List<Nombramiento> CargarDatosNombramiento(string elemento, List<Nombramiento> datosPrevios, object parametro)
        {
            string sParametro = "";
            DateTime FechaInicio = new DateTime();
            DateTime FechaFinal = new DateTime();

            if (elemento == "fechas")
            {
                List<DateTime> fechas = (List<DateTime>)parametro;
                FechaInicio = fechas.ElementAt(0);
                FechaFinal = fechas.ElementAt(1);
            }
            else
            {
                sParametro = parametro.ToString();
            }

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "cedula":
                        datosPrevios = entidadBase.Nombramiento
                                                  .Include("Puesto")
                                                  .Include("Funcionario")
                                                  .Include("EstadoNombramiento")
                                                  .Where(Q => Q.Funcionario.IdCedulaFuncionario == sParametro).ToList();
                        break;
                    case "numeroPuesto":
                        datosPrevios = entidadBase.Nombramiento
                                                  .Include("Puesto")
                                                  .Include("Funcionario")
                                                  .Include("EstadoNombramiento")
                                                  .Where(Q => Q.Puesto.CodPuesto.Contains(sParametro)).ToList();
                        break;
                    case "fechas":
                        datosPrevios = entidadBase.Nombramiento
                                                  .Include("Puesto")
                                                  .Include("Funcionario")
                                                  .Include("EstadoNombramiento")
                                                  .Where(Q => Q.FecRige >= FechaInicio && Q.FecRige <= FechaFinal)
                                                  .ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "cedula":
                        datosPrevios = datosPrevios
                                                  .Where(Q => Q.Funcionario.IdCedulaFuncionario == sParametro).ToList();
                        break;
                    case "numeroPuesto":
                        datosPrevios = datosPrevios
                                                  .Where(Q => Q.Puesto.CodPuesto.Contains(sParametro)).ToList();
                        break;
                    case "fechas":
                        datosPrevios = datosPrevios
                                                  .Where(Q => Q.FecRige >= FechaInicio && Q.FecRige <= FechaFinal)
                                                  .ToList();
                        break;
                    default:
                        break;
                }
            }
            return datosPrevios;
        }

        public CRespuestaDTO NombramientoPorCodigo(int codNombramiento)
        {
            try
            {
                var resultado = entidadBase.Nombramiento.Include("Funcionario").Include("Puesto")
                                            .Include("Puesto.DetallePuesto").Include("Puesto.DetallePuesto.Clase")
                                            .Include("Puesto.DetallePuesto.Especialidad")
                                            .Where(Q => Q.PK_Nombramiento == codNombramiento).FirstOrDefault();

                if (resultado != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se encontró ningún nombramiento asociado al código suministrado");
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

        public CRespuestaDTO ListarNombramientosVence(DateTime fecha)
        {
            try
            {
                var nombramientos = entidadBase.Nombramiento.Include("Funcionario")
                                                            .Include("Puesto")
                                                            .Where(N => N.FecVence == fecha && N.FK_EstadoNombramiento != 10).ToList();

                if (nombramientos.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = nombramientos
                    };
                }
                else
                {
                    throw new Exception("No se encontraron nombramientos cuyo vencimiento se alcance en la fecha indicada");
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

        #endregion
    }
}