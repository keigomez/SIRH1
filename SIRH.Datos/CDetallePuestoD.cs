using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
    //Cometario para subir
    public class CDetallePuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDetallePuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// Guarda el detalle del puesto en la BD        
        //Retorna informacion del detalle del puesto        
        public int GuardarDetalle(DetallePuesto DetallePuesto)
        {
            entidadBase.DetallePuesto.Add(DetallePuesto);
            return DetallePuesto.PK_DetallePuesto;
        }

        public CRespuestaDTO ActualizarEscalaSalarial(int clase, int categoria, int periodo)
        {
            try
            {
                var resultado = entidadBase.DetallePuesto.Where(Q => Q.Clase.PK_Clase == clase && Q.IndEstadoDetallePuesto == 1).FirstOrDefault();

                if (resultado != null)
                {
                    resultado.EscalaSalarial = entidadBase.EscalaSalarial.Where(Q => Q.IndCategoria == categoria
                                                                                && Q.PeriodoEscalaSalarial.PK_Periodo == periodo).FirstOrDefault();
                    entidadBase.SaveChanges();

                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("El detalle de puesto a actualizar no pudo ser encontrado");
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

        public CRespuestaDTO GuardarDetallePuesto(DetallePuesto detalle)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.DetallePuesto.Add(detalle);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = detalle
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
        /// POR CEDULA
        /// </summary>
        /// <returns>Retorna Informacion detalle de puesto por cédula</returns>

        public DetallePuesto CargarDetallePuestoCedula(string cedula)
        {
            DetallePuesto resultado = new DetallePuesto();

            resultado = entidadBase.DetallePuesto.Where(R =>
                                                          R.Puesto.Nombramiento.Where(Q =>
                                                                                      Q.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).FirstOrDefault();


            return resultado;
        }

        public DetallePuesto CargarDetallePuestoCodigo(string codPuesto)
        {
            DetallePuesto resultado = new DetallePuesto();

            resultado = entidadBase.DetallePuesto.Include("Clase")
                                                    .Include("Especialidad")
                                                    .Include("SubEspecialidad")
                                                    .Include("OcupacionReal")
                                                    .Where(Q => Q.Puesto.CodPuesto == codPuesto).FirstOrDefault();

            return resultado;
        }


        public DetallePuesto CargarDetallePuesto(int indDetalle)
        {
            DetallePuesto resultado = new DetallePuesto();

            resultado = entidadBase.DetallePuesto.Include("Clase")
                                                    .Include("Especialidad")
                                                    .Include("SubEspecialidad")
                                                    .Include("OcupacionReal")
                                                    .Include("EscalaSalarial")
                                                    .Where(Q => Q.PK_DetallePuesto == indDetalle).FirstOrDefault();

            return resultado;
        }

        public CRespuestaDTO ActualizarDetallePuesto(string codPuesto, DetallePuesto detallePuesto)
        {
            try
            {
                var detallePuestoActual = entidadBase.DetallePuesto.FirstOrDefault(D => D.Puesto.CodPuesto == codPuesto && 
                                                                                  (D.IndEstadoDetallePuesto == null || D.IndEstadoDetallePuesto == 1));

                if (detallePuestoActual != null)
                {
                    detallePuestoActual.IndEstadoDetallePuesto = 2;
                    detallePuesto.IndEstadoDetallePuesto = 1;
                    detallePuesto.FecRige = DateTime.Now;
                    detallePuesto.EscalaSalarial = entidadBase.
                                                   EscalaSalarial.FirstOrDefault(E => E.IndCategoria == detallePuesto.Clase.IndCategoria && E.FK_Periodo == 3);
                    detallePuesto.PorProhibicion = detallePuestoActual.PorProhibicion;
                    detallePuesto.PorDedicacion = detallePuestoActual.PorDedicacion;
                    detallePuesto.FK_Nombramiento = detallePuestoActual.FK_Nombramiento;
                    var puesto = detallePuestoActual.Puesto;
                    puesto.DetallePuesto.Add(detallePuesto);

                    if (entidadBase.SaveChanges() > 0)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = puesto
                        };
                    }
                    else
                    {
                        throw new Exception("No se pudieron actualizar los registros del detalle de puesto");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el detalle de puesto a actualizar");
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
        
