using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.DTO;

namespace SIRH.Datos
{
   public class CDetalleContratacionD
   {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CDetalleContratacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Metodos
        /// <summary>
        /// Guarda los Detalles de Contratacion en la BD
        /// </summary>
        /// <returns>Retorna los Detalles de Contratacion</returns>
        
        public int GuardarDetalleContratacionFuncionario(DetalleContratacion DetalleContratacion)
        {
            entidadBase.DetalleContratacion.Add(DetalleContratacion);
            // devuelve un int, la PK_DetalleContratacion
            return DetalleContratacion.PK_DetalleContratacion;
        }

       //09/12/2016
        public CRespuestaDTO GuardarDetalleContratacion(DetalleContratacion detalleContratacion)
        {
            CRespuestaDTO respuesta; 
            try
            {
                entidadBase.DetalleContratacion.Add(detalleContratacion);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = detalleContratacion.PK_DetalleContratacion
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

        /// <summary>
        /// Obtiene la lista de Detalle de Contratacion de la BD
        /// </summary>
        /// <returns>Retorna una lista de Detalle de Contratacion</returns>
        
        public List<DetalleContratacion> CargarDetalleContratacion()
        {
            List<DetalleContratacion> resultados = new List<DetalleContratacion>();

            resultados = entidadBase.DetalleContratacion.ToList();

            return resultados;
        }

        /// <summary>
        /// Carga Detalles de Contratacion de la BD
        /// </summary>
        /// <returns>Retorna Detalle de Contratacion</returns>
        public DetalleContratacion CargarDetalleContratacionPorID(int idDetalleContratacion)
        {
            DetalleContratacion resultado = new DetalleContratacion();

            resultado = entidadBase.DetalleContratacion.Where(R => R.PK_DetalleContratacion == idDetalleContratacion).FirstOrDefault();

            return resultado;
        }
        /// <summary>
        /// POR CEDULA
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public DetalleContratacion CargarDetalleContratacionCedula(string cedula)
        {
            DetalleContratacion resultado = new DetalleContratacion();

            resultado = entidadBase.DetalleContratacion.Where(R => R.Funcionario.IdCedulaFuncionario == cedula).FirstOrDefault();

            return resultado;
        }

        public CRespuestaDTO ActualizarDetalleContratacion(DetalleContratacion dato)
        {
            try
            {
                var anterior = entidadBase.DetalleContratacion.Where(D => D.Funcionario.PK_Funcionario == dato.Funcionario.PK_Funcionario).FirstOrDefault();

                if (anterior != null)
                {
                    anterior.FecIngreso = dato.FecIngreso != null ? dato.FecIngreso : anterior.FecIngreso;
                    anterior.NumAnualidades = dato.NumAnualidades > -1 ? dato.NumAnualidades : anterior.NumAnualidades;
                    anterior.FecMesAumento = dato.FecMesAumento != null ? dato.FecMesAumento : anterior.FecMesAumento;
                    anterior.CodPolicial = dato.CodPolicial > 0 ? dato.CodPolicial : anterior.CodPolicial;
                    anterior.MarcaAsistencia = dato.MarcaAsistencia != anterior.MarcaAsistencia ? dato.MarcaAsistencia : anterior.MarcaAsistencia;
                    anterior.IndUbicacionReal = dato.IndUbicacionReal != null ? dato.IndUbicacionReal : anterior.IndUbicacionReal;
                    anterior.FecRegimenPolicial = dato.FecRegimenPolicial != null ? dato.FecRegimenPolicial : anterior.FecRegimenPolicial;
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = anterior.PK_DetalleContratacion
                    };
                }
                else
                {
                    throw new Exception("El funcionario no cuenta actualmente con un detalle de contratación asociado");
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
        
        public CRespuestaDTO CargarDetalleContratacionFuncionario(string cedula)
        {
            try
            {
                var dato = entidadBase.DetalleContratacion.Include("Funcionario")
                                                          .Include("Funcionario.ExpedienteFuncionario")
                                                          .FirstOrDefault(D => D.Funcionario.IdCedulaFuncionario == cedula);

                if (dato != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = dato
                    };
                }
                else
                {
                    throw new Exception("El funcionario no cuenta actualmente con un detalle de contratación asociado");
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
        /// Buscar funcionario policial por cedula / código policial / rango de fechas de ingreso al régimen policial
        /// </summary>
        /// <returns>Retorna una Lista de Funcionarios policiales</returns>
        public CRespuestaDTO BuscarFuncionarioPolicial(List<Funcionario> datosPrevios, object parametro, string elemento)
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

        private List<Funcionario> CargarDatos(string elemento, List<Funcionario> datosPrevios, object parametro)
        {
            string sParam = "";
            int iParam = 0;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                sParam = parametro.ToString();
            }
            else if (parametro.GetType().Name.Equals("Int32"))
            {
                iParam = Convert.ToInt32(parametro);
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
                        datosPrevios = entidadBase.Funcionario.Include("Nombramiento").Include("DetalleContratacion")
                                                              .Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.DetallePuesto")
                                                              .Include("Nombramiento.Puesto.DetallePuesto.Clase").Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                  .Where(A => A.FK_EstadoFuncionario == 1
                                                  && A.DetalleContratacion.Where(B => B.CodPolicial != null && B.FecCese == null).Count() > 0
                                                  && A.Nombramiento.Where(B => B.FecVence > DateTime.Now || B.FecVence == null)
                                                                   .OrderByDescending(B => B.FecVence).FirstOrDefault() != null
                                                  && A.IdCedulaFuncionario == sParam)
                                                  .ToList();
                        break;
                    case "CodigoPolicial":
                        datosPrevios = entidadBase.Funcionario.Include("Nombramiento").Include("DetalleContratacion")
                                                              .Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.DetallePuesto")
                                                              .Include("Nombramiento.Puesto.DetallePuesto.Clase").Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                  .Where(A => A.FK_EstadoFuncionario == 1
                                                  && A.DetalleContratacion.Where(B => B.CodPolicial == iParam && B.FecCese == null).Count() > 0
                                                  && A.Nombramiento.Where(B => B.FecVence > DateTime.Now || B.FecVence == null)
                                                                   .OrderByDescending(B => B.FecVence).FirstOrDefault() != null)
                                                  .ToList();
                        break;
                    case "Fechas":
                        datosPrevios = entidadBase.Funcionario.Include("Nombramiento").Include("DetalleContratacion")
                                                              .Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.DetallePuesto")
                                                              .Include("Nombramiento.Puesto.DetallePuesto.Clase").Include("Nombramiento.Puesto.DetallePuesto.Especialidad")

                                                  .Where(A => A.FK_EstadoFuncionario == 1
                                                  && A.DetalleContratacion.Where(B => B.CodPolicial != null && B.FecCese == null
                                                                          && B.FecRegimenPolicial > paramFechaInicio
                                                                          && B.FecRegimenPolicial < paramFechaFinal).Count() > 0
                                                  && A.Nombramiento.Where(B => B.FecVence > DateTime.Now || B.FecVence == null)
                                                                   .OrderByDescending(B => B.FecVence).FirstOrDefault() != null)
                                                  .ToList();
                        break;
                    default:
                        datosPrevios = new List<Funcionario>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(A => A.FK_EstadoFuncionario == 1
                                                   && A.DetalleContratacion.Where(B => B.CodPolicial != null && B.FecCese == null).Count() > 0
                                                   && A.Nombramiento.Where(B => B.FecVence > DateTime.Now || B.FecVence == null)
                                                                    .OrderByDescending(B => B.FecVence).FirstOrDefault() != null
                                                   && A.IdCedulaFuncionario == sParam)
                                                   .ToList();
                        break;
                    case "CodigoPolicial":
                        datosPrevios = datosPrevios.Where(A => A.FK_EstadoFuncionario == 1
                                                  && A.DetalleContratacion.Where(B => B.CodPolicial == iParam && B.FecCese == null).Count() > 0
                                                  && A.Nombramiento.Where(B => B.FecVence > DateTime.Now || B.FecVence == null)
                                                                   .OrderByDescending(B => B.FecVence).FirstOrDefault() != null
                                                  && A.IdCedulaFuncionario == sParam)
                                                  .ToList();
                        break;
                    case "Fechas":
                        datosPrevios = datosPrevios.Where(A => A.FK_EstadoFuncionario == 1
                                                  && A.DetalleContratacion.Where(B => B.CodPolicial != null
                                                                          && B.FecCese == null
                                                                          && B.FecRegimenPolicial > paramFechaInicio
                                                                          && B.FecRegimenPolicial < paramFechaFinal).Count() > 0
                                                  && A.Nombramiento.Where(B => B.FecVence > DateTime.Now || B.FecVence == null)
                                                                   .OrderByDescending(B => B.FecVence).FirstOrDefault() != null)
                                                  .ToList();
                        break;
                    default:
                        datosPrevios = new List<Funcionario>();
                        break;
                }
            }

            return datosPrevios;
        }

        #endregion
    }
}