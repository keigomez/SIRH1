using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPagoFeriadoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPagoFeriadoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca el pago feriado registrado que corresponde con el código que recibe por parámetros
        /// </summary>
        /// <returns>Retorna el registro completo</returns>
        public CRespuestaDTO BuscarPagoFeriado(int codPagoFeriado)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.PagoFeriado.Include("PagoExtraordinario").Where(P => P.PK_PagoFeriado == codPagoFeriado).FirstOrDefault();

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
                    throw new Exception("No se encontró el trámite de pago indicado");
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

        /// <summary>
        /// Busca todos los trámites registrados en la BD
        /// </summary>
        /// <returns>Retorna TODOS los trámites de feriado registrados</returns>
        public CRespuestaDTO ListarPagoFeriado()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.UbicacionAsueto.ToList();

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
                    throw new Exception("No se encontró ningún trámite de pago");
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

        /// <summary>
        /// Almacena el pago de feriado que recibe por parámetros
        /// </summary>
        /// <returns>Retorna el pk del registro almacenado</returns>
        public CRespuestaDTO AgregarPagoFeriado(PagoFeriado tramite, PagoExtraordinario pagoExtraordinario, Funcionario funcionario, EstadoTramite estadoTramite)
        {
            CRespuestaDTO respuesta;
            try
            {

                funcionario = entidadBase.Funcionario
                    .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null) //si existe el funcionario
                {
                    pagoExtraordinario = entidadBase.PagoExtraordinario.Where(PE => PE.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario).FirstOrDefault();

                    if (pagoExtraordinario != null)
                    { //si existe el pago extraordinario
                        estadoTramite = entidadBase.EstadoTramite.Where(PE => PE.PK_EstadoTramite == tramite.EstadoTramite.PK_EstadoTramite).FirstOrDefault();

                        if (estadoTramite != null && estadoTramite.PK_EstadoTramite != 2)
                        {


                            pagoExtraordinario.PagoFeriado.Add(tramite);
                            entidadBase.PagoFeriado.Add(tramite);
                            entidadBase.SaveChanges();

                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = tramite.PK_PagoFeriado
                            };
                            return respuesta;
                        }
                        else
                        {
                            throw new Exception("No se encontró el estado del trámite indicado");
                        }

                    }
                    else
                    {
                        throw new Exception("No se encontró el registro del pago extraordinario indicado");
                    }
                }

                else
                {
                    throw new Exception("No se encontró el funcionario indicado");
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
        /// Realiza una búsqueda de trámites
        /// </summary>
        /// <returns>Retorna el registro del catálogo especificado</returns>
        public CRespuestaDTO ListarPagoFeriado(List<PagoFeriado> datosPrevios, object parametro, string elemento)
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

        /// <summary>
        /// Busca y filtra trámites de pago
        /// </summary>
        /// <returns>Retorna los trámites encontrados</returns>
        private List<PagoFeriado> CargarDatos(string elemento, List<PagoFeriado> datosPrevios, object parametro)
        {
            string filtro = "";
            int aux;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();
            try
            {
                if (parametro.GetType().Name.Equals("String"))
                {
                    filtro = parametro.ToString();
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
                            datosPrevios = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario").Include("EstadoTramite").Include("DiaPagado").Include("DiaPagado.CatalogoDia")
                                                        .Where(PF => PF.PagoExtraordinario.Funcionario.IdCedulaFuncionario == filtro).ToList();
                            break;
                        case "FechaTramite":
                            datosPrevios = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario").Include("EstadoTramite").Include("DiaPagado.CatalogoDia")
                                                         .Where(PF => PF.PagoExtraordinario.FecTramite >= paramFechaInicio &&
                                                            PF.PagoExtraordinario.FecTramite <= paramFechaFinal)
                                                        .ToList();
                            break;
                        case "Consecutivo":
                            aux = Convert.ToInt32(filtro);
                            datosPrevios = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario").Include("EstadoTramite").Include("DiaPagado").Include("DiaPagado.CatalogoDia")
                                                       .Where(PF => PF.PK_PagoFeriado == aux).ToList();
                            break;
                        case "DiaFeriado":
                            aux = Convert.ToInt32(filtro);
                            datosPrevios = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario").Include("EstadoTramite").Include("DiaPagado").Include("DiaPagado.CatalogoDia")
                                .Where(PF => PF.DiaPagado.Where(DP => DP.CatalogoDia.PK_CatalogoDia == aux).Count() > 0).ToList();
                            break;
                        case "Estado":
                            aux = Convert.ToInt32(filtro);
                            datosPrevios = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario").Include("EstadoTramite").Include("DiaPagado").Include("DiaPagado.CatalogoDia")
                                                             .Where(PF => PF.EstadoTramite.PK_EstadoTramite == aux).ToList();
                            break;
                        default:
                            datosPrevios = new List<PagoFeriado>();
                            break;
                    }
                }
                else
                {
                    switch (elemento)
                    {
                        case "Cedula":
                            datosPrevios = datosPrevios.Where(PF => PF.PagoExtraordinario.Funcionario.IdCedulaFuncionario == filtro).ToList();
                            break;
                        case "FechaTramite":
                            datosPrevios = datosPrevios.Where(PF => PF.PagoExtraordinario.FecTramite >= paramFechaInicio &&
                                                            PF.PagoExtraordinario.FecTramite <= paramFechaFinal).ToList();
                            break;
                        case "Consecutivo":
                            aux = Convert.ToInt32(filtro);
                            datosPrevios = datosPrevios.Where(PF => PF.PK_PagoFeriado == aux).ToList();
                            break;
                        case "DiaFeriado":
                            aux = Convert.ToInt32(filtro);
                            datosPrevios = datosPrevios.Where(PF => PF.DiaPagado.Where(DP => DP.CatalogoDia.PK_CatalogoDia == aux).Count() > 0).ToList();
                            break;
                        case "Estado":
                            aux = Convert.ToInt32(filtro);
                            datosPrevios = datosPrevios.Where(PF => PF.EstadoTramite.PK_EstadoTramite == aux).ToList();
                            break;
                        default:
                            datosPrevios = new List<PagoFeriado>();
                            break;
                    }
                }

                return datosPrevios;
            }catch(Exception e){
                return datosPrevios;
            }
        }

        /// <summary>
        /// Anula un trámite de pago
        /// </summary>
        /// <returns>Retorna el trámite anulado</returns>
        public CRespuestaDTO AnularPagoFeriado(PagoFeriado tramite)
        {
            CRespuestaDTO respuesta;

            try
            {
                var tramiteOld = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario")
                                                    .Where(PK => PK.PK_PagoFeriado == tramite.PK_PagoFeriado).FirstOrDefault();
                EstadoTramite estado = entidadBase.EstadoTramite.Where(ET => ET.PK_EstadoTramite == 2).FirstOrDefault();

                if (tramiteOld != null)
                {
                    estado.PagoFeriado.Add(tramiteOld);
                    tramiteOld.ObsTramite =tramiteOld.ObsTramite +"\n"+tramite.ObsTramite;
                    tramite = tramiteOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                     {
                         Codigo = 1,
                         Contenido = tramite.PK_PagoFeriado
                     };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun trámite de pago de feriado con el código especificado." }
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
        /// Busca un trámite de pago completo
        /// </summary>
        /// <returns>Retorna el trámite encontrado</returns>
        public CRespuestaDTO BuscarPagoFeriadoCompleto(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var caucion = entidadBase.PagoFeriado.Include("PagoExtraordinario").Include("PagoExtraordinario.Funcionario").Include("EstadoTramite")
                                                    .Where(PF => PF.PK_PagoFeriado == codigo).FirstOrDefault();

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
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró el trámite respectivo." }
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

        /// <summary>
        /// Elimina un trámite de pago
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CRespuestaDTO EliminarPagoFeriado(PagoFeriado pagoFeriado)
        {
            CRespuestaDTO respuesta;
            try
            {
                pagoFeriado = entidadBase.PagoFeriado
                    .Where(F => F.PK_PagoFeriado == pagoFeriado.PK_PagoFeriado).FirstOrDefault();

                if (pagoFeriado != null)
                {
                    entidadBase.PagoFeriado.Remove(pagoFeriado);
                    entidadBase.SaveChanges();


                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "Eliminado"
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el pago de feriado indicado");
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
