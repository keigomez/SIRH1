using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    /// <summary>
    /// Clase que administrara los datos de la tabla DetalleCalificacionNombramientoDTO de la base de datos.
    /// </summary>
    public class CDetalleCalificacionNombramientoD
    {
        #region Variables
        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor
        public CDetalleCalificacionNombramientoD(SIRHEntities entidadGlobal)
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
        public CRespuestaDTO AgregarDetalleCalificacionNombramiento(DetalleCalificacion detalle)
        {
            CRespuestaDTO respuesta;
            try
            {
                var calificacionNombramiento = entidadBase.CalificacionNombramiento
                    .Where(C => C.PK_CalificacionNombramiento == detalle.CalificacionNombramiento.PK_CalificacionNombramiento).FirstOrDefault();

                if (calificacionNombramiento != null)
                {
                    entidadBase.DetalleCalificacion.Add(detalle);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró la calificación indicada");
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
        /// Metodo encargado de modificar el estado datosDetalleCalificacionNombramiento
        /// </summary>
        /// <param name="detalleNombramiento">Tipo DetalleCalificacionNombramientoDTO</param>
        /// <returns></returns>


        public CRespuestaDTO AgregarDetalleCalificacionModificada(DetalleCalificacionModificada detalle)
        {
            CRespuestaDTO respuesta;
            try
            {
                var calificacionNombramiento = entidadBase.CalificacionNombramiento
                    .Where(C => C.PK_CalificacionNombramiento == detalle.CalificacionNombramiento.PK_CalificacionNombramiento).FirstOrDefault();

                if (calificacionNombramiento != null)
                {
                    entidadBase.DetalleCalificacionModificada.Add(detalle);
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = detalle
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró la calificación indicada");
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

        public CRespuestaDTO ObtenerDetalleCalificacionN(int codigo)
        {
            CRespuestaDTO respuesta;

            try
            {
                var detalleCalificacion = entidadBase.DetalleCalificacion
                    .Where(C => C.CalificacionNombramiento.PK_CalificacionNombramiento == codigo).FirstOrDefault();
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
                        Contenido = new CErrorDTO { MensajeError = "No se encontró ninguna caución." }
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


        public CRespuestaDTO BuscarFuncionarioId(Int32 id)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Where(F => F.PK_Funcionario == id)
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

        #endregion
    }
}
