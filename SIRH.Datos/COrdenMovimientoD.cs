using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;
using System.Data;

namespace SIRH.Datos
{
    public class COrdenMovimientoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public COrdenMovimientoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Métodos
        public CRespuestaDTO InsertarOrdenMovimiento(OrdenMovimiento orden, OrdenMovimientoDeclaracion declaracion)
        {
            CRespuestaDTO respuesta;
            try
            {
                if (orden != null)
                {
                    var annio = orden.FecRige.Year.ToString();
                    var catalogo= entidadBase.CatOrdenMovimiento.Where(Q => Q.AnioRige == annio).FirstOrDefault();
                    long numConsecutivo = 0;

                    if (catalogo == null)
                    {
                        catalogo = new CatOrdenMovimiento
                        {
                            AnioRige = annio,
                            NumConsecutivo = numConsecutivo
                        };
                        entidadBase.CatOrdenMovimiento.Add(catalogo);
                        entidadBase.SaveChanges();
                    }
                    else
                    {
                        numConsecutivo = catalogo.NumConsecutivo;
                    }

                    numConsecutivo += 1;
                    string fmt = "00000";
                    orden.NumOrden = annio + numConsecutivo.ToString(fmt);
                    entidadBase.OrdenMovimiento.Add(orden);
                    entidadBase.SaveChanges();

                    declaracion.FK_OrdenMovimiento = orden.PK_OrdenMovimiento;
                    entidadBase.OrdenMovimientoDeclaracion.Add(declaracion);
                    entidadBase.SaveChanges();

                    catalogo.NumConsecutivo = numConsecutivo;
                    entidadBase.Entry(catalogo).State = EntityState.Modified;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = orden.PK_OrdenMovimiento
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

        public CRespuestaDTO ConsultarOrdenMovimiento(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var orden = entidadBase.OrdenMovimiento
                                            .Include("OrdenMovimientoDeclaracion")
                                            .Include("Funcionario")
                                            .Include("PedimentoPuesto")
                                            .Include("DetallePuesto")
                                            .Include("DetallePuesto.Puesto")
                                            .Include("MotivoMovimiento")
                                            .Include("EstadoOrdenMovimiento")
                                            .Where(A => A.PK_OrdenMovimiento == codigo).FirstOrDefault();

                if (orden != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = orden
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna orden" }
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


        public CRespuestaDTO BuscarFuncionarioCodigo(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Where(F => F.PK_Funcionario == codigo)
                                                   .FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
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

        public CRespuestaDTO BuscarOrdenes(List<OrdenMovimiento> datosPrevios, object parametro, string elemento)
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

        private List<OrdenMovimiento> CargarDatos(string elemento, List<OrdenMovimiento> datosPrevios, object parametro)
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
                        datosPrevios = entidadBase.OrdenMovimiento
                                                    .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "NumOrden":
                        datosPrevios = entidadBase.OrdenMovimiento
                                                    .Where(C => C.NumOrden.Contains(param)).ToList();
                        break;

                    case "Puesto":
                        datosPrevios = entidadBase.OrdenMovimiento
                                                    .Where(C => C.DetallePuesto.Puesto.CodPuesto.Contains(param)).ToList();
                        break;

                    case "Estado":
                        datosPrevios = entidadBase.OrdenMovimiento
                                                       .Where(C => C.EstadoOrdenMovimiento.PK_Estado == paramInt).ToList();
                        break;

                    case "Tipo":
                        datosPrevios = entidadBase.OrdenMovimiento
                                                       .Where(C => C.FK_TipoMovimiento == paramInt).ToList();
                        break;

                    case "FechaInicio":
                        datosPrevios = entidadBase.OrdenMovimiento
                                                    .Where(C => C.FecRige >= paramFechaInicio &&
                                                        C.FecRige <= paramFechaFinal).ToList();
                        break;

                    case "FechaFinal":
                        datosPrevios = entidadBase.OrdenMovimiento
                                                    .Where(C => C.FecVence >= paramFechaInicio &&
                                                        C.FecVence <= paramFechaFinal).ToList();
                        break;

                    default:
                        datosPrevios = new List<OrdenMovimiento>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param)).ToList();
                        break;

                    case "NumOrden":
                        datosPrevios = datosPrevios.Where(C => C.NumOrden.Contains(param)).ToList();
                        break;

                    case "Puesto":
                        datosPrevios = datosPrevios.Where(C => C.DetallePuesto.Puesto.CodPuesto.Contains(param)).ToList();
                        break;

                    case "Estado":
                        datosPrevios = datosPrevios.Where(C => C.EstadoOrdenMovimiento.PK_Estado == paramInt).ToList();
                        break;

                    case "Tipo":
                        datosPrevios = datosPrevios.Where(C => C.FK_TipoMovimiento == paramInt).ToList();
                        break;

                    case "FechaInicio":
                        datosPrevios = datosPrevios.Where(C => C.FecRige >= paramFechaInicio &&
                                                        C.FecRige <= paramFechaFinal).ToList();
                        break;
                    case "FechaFinal":
                        datosPrevios = datosPrevios.Where(C => C.FecVence >= paramFechaInicio &&
                                                        C.FecVence <= paramFechaFinal).ToList();
                        break;
                    default:
                        datosPrevios = new List<OrdenMovimiento>();
                        break;
                }
            }

            return datosPrevios;
        }

        public CRespuestaDTO ActualizarOrden(OrdenMovimiento registro)
        {
            CRespuestaDTO respuesta;
            int dato;

            try
            {
                var orden = entidadBase.OrdenMovimiento.Where(A => A.PK_OrdenMovimiento == registro.PK_OrdenMovimiento).FirstOrDefault();

                if (orden != null)
                {
                    switch (registro.EstadoOrdenMovimiento.PK_Estado)
                    {
                        case 2: // Revisado
                            orden.IdFuncionarioRevision = registro.IdFuncionarioRevision;
                        break;

                        case 3: // Aprobar
                            orden.IdFuncionarioJefatura = registro.IdFuncionarioJefatura;
                            break;
                    }

                    orden.EstadoOrdenMovimiento = registro.EstadoOrdenMovimiento;
                    using (entidadBase)
                    {
                        entidadBase.Entry(orden).State = EntityState.Modified;
                        dato = entidadBase.SaveChanges();
                    }

                    if (dato > 0)
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = true
                        };
                    }
                    else
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = -1,
                            Contenido = false
                        };
                    }

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = orden
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna orden" }
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
            }
            return respuesta;
        }

        public CRespuestaDTO ListarOrdenEstados()
        {
            try
            {
                var estados = entidadBase.EstadoOrdenMovimiento.ToList();
                if (estados != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = estados
                    };
                }
                else
                {
                    throw new Exception("No se encontraron Estados de la Orden de movimiento");
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

        public CRespuestaDTO BuscarEstado(Int32 codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.EstadoOrdenMovimiento.FirstOrDefault(E => E.PK_Estado == codigo);
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún estado");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        #endregion
    }
}
