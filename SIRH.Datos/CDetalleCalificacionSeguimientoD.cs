using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    /// <summary>
    /// Clase que administrara los datos de la tabla CDetalleCalificacionSeguimientoD de la base de datos.
    /// </summary>
    public class CDetalleCalificacionSeguimientoD
    {
        #region Variables
        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor
        public CDetalleCalificacionSeguimientoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion
        #region Método
        /// <summary>
        /// Metodo que se encargaa de agregar preguntas Detalle CalificacionDTO NombramientoDTO
        /// </summary>
        /// <param name="idCalificacion">Tipo int</param>
        /// <param name="ursNombre">Tipo int</param>
        /// <param name="fecha">Tipo Datetime</param>
        /// <param name="detalleNombramiento">Tipo DetalleCalificacionNombramientoDTO</param>
        /// <returns></returns>
        public CRespuestaDTO AgregarDetalleSeguimiento(DetalleCalificacionSeguimiento detalle)
        {
            CRespuestaDTO respuesta;
            try
            {
                //var tablaDetalle = entidadBase.DetalleCalificacionSeguimiento
                //    .Where(C => C.FK_CalificacionNombramientoFuncionario == detalle.FK_CalificacionNombramientoFuncionario
                //             && C.FecRegistro.Day == detalle.FecRegistro.Day
                //             && C.FecRegistro.Month == detalle.FecRegistro.Month
                //             && C.FecRegistro.Year == detalle.FecRegistro.Year
                //             && C.FK_Estado != 5  )
                //    .FirstOrDefault();

                //if (tablaDetalle == null)
                //{
                //    entidadBase.DetalleCalificacionSeguimiento.Add(detalle);
                //    entidadBase.SaveChanges();

                //    respuesta = new CRespuestaDTO
                //    {
                //        Codigo = 1,
                //        Contenido = detalle.PK_Detalle
                //    };
                //}
                //else
                //{
                //    throw new Exception("Ya se registró un informe de seguimiento con fecha de hoy");
                //}

                entidadBase.DetalleCalificacionSeguimiento.Add(detalle);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = detalle.PK_Detalle
                };
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }

            return respuesta;
        }
        /// <summary>
        /// Metodo encargado de modificar el estado datosDetalleCalificacionNombramiento
        /// </summary>
        /// <param name="detalleNombramiento">Tipo DetalleCalificacionNombramientoDTO</param>
        /// <returns></returns>

        public CRespuestaDTO ObtenerDetalleSeguimiento(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var detalleCalificacion = entidadBase.DetalleCalificacionSeguimiento
                                            .Where(C => C.PK_Detalle == codigo).FirstOrDefault();
                if (detalleCalificacion != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalleCalificacion
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ningún dato de Seguimiento." }
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


        public CRespuestaDTO ModificarEstado(DetalleCalificacionSeguimiento registro)
        {
            CRespuestaDTO respuesta;

            try
            {
                var DetalleOld = entidadBase.DetalleCalificacionSeguimiento.Where(I => I.PK_Detalle == registro.PK_Detalle).FirstOrDefault();

                if (DetalleOld != null)
                {
                    DetalleOld.FK_Estado = registro.FK_Estado;
                   
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_Detalle
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningún MetaIndividual con el código especificado." }
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


        public List<DetalleCalificacionSeguimiento> CargarDatos(int idFuncionario, int idPeriodo)
        {
            var datosPrevios = entidadBase.DetalleCalificacionSeguimiento
                                            .Where(C => C.CalificacionNombramientoFuncionarios.FK_Funcionario == idFuncionario 
                                                && C.CalificacionNombramientoFuncionarios.FK_PeriodoCalificacion == idPeriodo)
                                            .ToList();

            return datosPrevios;
        }

        #endregion
    }
}
