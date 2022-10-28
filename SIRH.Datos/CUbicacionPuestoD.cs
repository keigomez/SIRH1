using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using SIRH.DTO;
using System.Data.Entity.Infrastructure;

namespace SIRH.Datos
{
    public class CUbicacionPuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CUbicacionPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Metodos
        /// <summary>
        /// Guarda la Ubicacion de los Puestos
        /// </summary>
        /// <returns>Retorna la ubicacion del puesto</returns>
        public int GuardarUbicacionPuesto(UbicacionPuesto UbicacionPuesto)
        {
            entidadBase.UbicacionPuesto.Add(UbicacionPuesto);
            return UbicacionPuesto.PK_UbicacionPuesto;
        }

        /// <summary>
        /// Obtiene la lista de ubicaciones de los puestos de la BD
        /// </summary>
        /// <returns>Retorna una lista de Ubicaciones de puestos</returns>
        
        public List<UbicacionPuesto> CargarUbicacionPuesto()
        {
            List<UbicacionPuesto> resultados = new List<UbicacionPuesto>();

            resultados = entidadBase.UbicacionPuesto.ToList();

            return resultados;
        }

        /// <summary>
        /// Carga las Ubicaciones de puestos de la BD
        /// </summary>
        /// <returns>Retorna la carga de Ubicaciones de puestos</returns>
        public UbicacionPuesto CargarUbicacionPuestoPorID(int idUbicacionPuesto)
        {
            UbicacionPuesto resultado = new UbicacionPuesto();

            resultado = entidadBase.UbicacionPuesto.Where(R => R.PK_UbicacionPuesto == idUbicacionPuesto).FirstOrDefault();

            return resultado;
        }

        private DbQuery<UbicacionPuesto> RetornarDistritos()
        {
            return entidadBase.UbicacionPuesto.Include("Distrito");
        }

        //POR CÉDULA
        public List<UbicacionPuesto> CargarUbicacionPuestoCedula(string cedula)
        {
            List<UbicacionPuesto> resultado = new List<UbicacionPuesto>();

            resultado = RetornarDistritos().Include("TipoUbicacion").Where(Q => Q.RelPuestoUbicacion.Where(
                R => R.Puesto.Nombramiento.Where(K => K.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).Count() > 0).ToList();

            return resultado;
        }

        public CRespuestaDTO BuscarUbicacionTrabajoPedimento(string codigoPuesto)
        {
            try
            {
                var resultado = entidadBase.Puesto
                                    .Include("PedimentoPuesto")
                                    .Include("EstadoPuesto")
                                    .Include("DetallePuesto")
                                    .Include("DetallePuesto.ContenidoPresupuestario")
                                    .Include("DetallePuesto.Clase")
                                    .Include("DetallePuesto.Especialidad")
                                    .Include("DetallePuesto.OcupacionReal")
                                    .Include("DetallePuesto.SubEspecialidad")
                                    .Include("UbicacionAdministrativa")
                                    .Include("UbicacionAdministrativa.Division")
                                    .Include("UbicacionAdministrativa.DireccionGeneral")
                                    .Include("UbicacionAdministrativa.Departamento")
                                    .Include("UbicacionAdministrativa.Seccion")
                                    .Include("UbicacionAdministrativa.Presupuesto")
                                    .Include("RelPuestoUbicacion")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia")
                                    .FirstOrDefault(Q => Q.CodPuesto == codigoPuesto);

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
                    throw new Exception("No se encontró el puesto con el código suministrado, por favor verifique la información ingresada.");
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

        public CRespuestaDTO BuscarHistorialUbicacionTrabajo(string codigoPuesto)
        {
            try
            {
                var resultado = entidadBase.Puesto
                                    .Include("EstadoPuesto")
                                    .Include("DetallePuesto")
                                    .Include("DetallePuesto.Clase")
                                    .Include("DetallePuesto.Especialidad")
                                    .Include("DetallePuesto.OcupacionReal")
                                    .Include("DetallePuesto.SubEspecialidad")
                                    .Include("UbicacionAdministrativa")
                                    .Include("UbicacionAdministrativa.Division")
                                    .Include("UbicacionAdministrativa.DireccionGeneral")
                                    .Include("UbicacionAdministrativa.Departamento")
                                    .Include("UbicacionAdministrativa.Seccion")
                                    .Include("UbicacionAdministrativa.Presupuesto")
                                    .Include("RelPuestoUbicacion")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton")
                                    .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia")
                                    .FirstOrDefault(Q => Q.CodPuesto == codigoPuesto);

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
                    throw new Exception("No se encontró el puesto con el código suministrado, por favor verifique la información ingresada.");
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

        public CRespuestaDTO ModificarUbicacionPuesto(string codPuesto, CUbicacionPuestoDTO ubicacion)
        {
            try
            {
                var resultado = entidadBase.Puesto.Include("RelPuestoUbicacion")
                                                  .Include("RelPuestoUbicacion.UbicacionPuesto")
                                                  .Include("RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                                  .FirstOrDefault(Q => Q.CodPuesto == codPuesto);

                if (resultado != null)
                {
                    var ubicacionPuesto = entidadBase.UbicacionPuesto.FirstOrDefault(U => U.FK_Distrito == ubicacion.Distrito.IdEntidad
                                          && U.FK_TipoUbicacion == ubicacion.TipoUbicacion.IdEntidad);

                    if (resultado.RelPuestoUbicacion.Count < 1)
                    {
                        resultado.RelPuestoUbicacion.Add(
                            new RelPuestoUbicacion
                            {
                                FecCreacion = DateTime.Now,
                                Puesto = entidadBase.Puesto.FirstOrDefault(Q => Q.CodPuesto == resultado.CodPuesto),
                                UbicacionPuesto = ubicacionPuesto != null ? ubicacionPuesto : new UbicacionPuesto
                                {
                                    Distrito = entidadBase.Distrito.FirstOrDefault(Q => Q.PK_Distrito == ubicacion.Distrito.IdEntidad),
                                    IndEstadoUbicacionPuesto = 1,
                                    TipoUbicacion = entidadBase.TipoUbicacion.FirstOrDefault(Q => Q.PK_TipoUbicacion == ubicacion.TipoUbicacion.IdEntidad),
                                }
                            }
                        );

                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };
                    }
                    else
                    {
                        if (ubicacionPuesto != null)
                        {
                            resultado.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == ubicacion.TipoUbicacion.IdEntidad).
                                                         UbicacionPuesto = ubicacionPuesto;
                        }
                        else
                        {
                            resultado.RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == ubicacion.TipoUbicacion.IdEntidad).
                                                         UbicacionPuesto = new UbicacionPuesto
                                                         {
                                                             Distrito = entidadBase.Distrito.FirstOrDefault(Q => Q.PK_Distrito == ubicacion.Distrito.IdEntidad),
                                                             IndEstadoUbicacionPuesto = 1,
                                                             TipoUbicacion = entidadBase.TipoUbicacion.FirstOrDefault(Q => Q.PK_TipoUbicacion == ubicacion.TipoUbicacion.IdEntidad),
                                                         };
                        }

                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };
                    }
                }
                else
                {
                    throw new Exception("No se encontró el puesto con el número indicado, por favor revise los datos suministrados");
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

        public CRespuestaDTO BuscarUbicacionPuestoCodigo(string codPuesto, int codigo)
        {
            try
            {
                var resultado = entidadBase.Puesto.Include("EstadoPuesto")
                                                  .Include("DetallePuesto")
                                                  .Include("DetallePuesto.ContenidoPresupuestario")
                                                  .Include("DetallePuesto.Clase")
                                                  .Include("DetallePuesto.Especialidad")
                                                  .Include("DetallePuesto.OcupacionReal")
                                                  .Include("DetallePuesto.SubEspecialidad")
                                                  .Include("UbicacionAdministrativa")
                                                  .Include("UbicacionAdministrativa.Division")
                                                  .Include("UbicacionAdministrativa.DireccionGeneral")
                                                  .Include("UbicacionAdministrativa.Departamento")
                                                  .Include("UbicacionAdministrativa.Seccion")
                                                  .Include("UbicacionAdministrativa.Presupuesto")
                                                  .Include("RelPuestoUbicacion")
                                                  .Include("RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                                  .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                                  .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton")
                                                  .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia")
                                                  .FirstOrDefault(Q => Q.CodPuesto == codPuesto 
                                                                    && Q.RelPuestoUbicacion.Where(R => R.UbicacionPuesto.PK_UbicacionPuesto == codigo)
                                                                    .Count() > 0);
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
                    throw new Exception("No se pudo encontrar el detalle de ubicación determinado para el puesto dado, por favor revise los datos suministrados.");
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

       
    
