using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CUbicacionAsuetoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CUbicacionAsuetoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Retorna una ubicación de asueto especifica
        /// </summary>
        /// <returns>Retorna la ubicación</returns>
        public CRespuestaDTO BuscarUbicacionAsueto(int codUbicacion)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.UbicacionAsueto.Where(Q => Q.PK_UbicacionAsueto == codUbicacion).FirstOrDefault();

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
                    throw new Exception("No se encontró la ubicacion del asueto");
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
        /// Busca una lista de ubicaciones de asueto
        /// </summary>
        /// <returns>Retorna la ubicación</returns>
        public CRespuestaDTO ListarUbicacionAsueto()
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
                    throw new Exception("No se encontró ninguna ubicacion de asueto");
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
        /// Almacena una ubicación de asueto especifica
        /// </summary>
        /// <returns>Retorna la ubicación registrada</returns>
        public CRespuestaDTO AgregarUbicacionAsueto(CatalogoDia catalogoDia, UbicacionAsueto ubicacionAsueto, Canton canton)
        {
            CRespuestaDTO respuesta;
            try
            {
                catalogoDia = entidadBase.CatalogoDia.Where(CD => CD.PK_CatalogoDia == catalogoDia.PK_CatalogoDia).FirstOrDefault();

                if (catalogoDia != null)
                {
                    entidadBase.UbicacionAsueto.Add(ubicacionAsueto);
                    catalogoDia.UbicacionAsueto.Add(ubicacionAsueto);
                    entidadBase.Canton.Where(C => C.PK_Canton == canton.PK_Canton).FirstOrDefault()
                                              .UbicacionAsueto.Add(ubicacionAsueto);


                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = ubicacionAsueto.PK_UbicacionAsueto
                    };

                    return respuesta;
                }
                else {
                    throw new Exception("El día indicado no existe");
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
        /// Busca las ubicaciones de asueto según el nombre
        /// </summary>
        /// <returns>Retorna la ubicación</returns>
        public CRespuestaDTO BuscarAsuetoPorUbicacion(string nombre, int canton)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                if (!String.IsNullOrEmpty(nombre) && canton < 1) {

                    var datosEntidad = entidadBase.CatalogoDia.Include("TipoDia").Include("UbicacionAsueto").Include("UbicacionAsueto.Canton")
                        .Where(UA => UA.DesDia.Contains(nombre)).ToList();

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
                        throw new Exception("No se encontró un asueto en la ubicación indicada");
                    }
                }
                else if (String.IsNullOrEmpty(nombre) && canton > 0) {
                    var datosEntidad = entidadBase.CatalogoDia.Include("TipoDia")
                    .Where(UA =>( UA.UbicacionAsueto.Where(U => U.Canton.PK_Canton == canton).Count() > 0) && (UA.TipoDia.PK_TipoDia==2)).ToList();


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
                        throw new Exception("No se encontró un asueto en la ubicación indicada");
                    }
                }
                else if (!String.IsNullOrEmpty(nombre) && canton > 0)
                {
                    var datosEntidad = entidadBase.CatalogoDia.Include("TipoDia").Include("UbicacionAsueto").Include("UbicacionAsueto.Canton")
                       .Where(UA => UA.UbicacionAsueto.Where(U => U.Canton.PK_Canton == canton).Count() > 0).ToList();

                    if (datosEntidad != null)
                    {
                        datosEntidad = datosEntidad.Where(UA => UA.DesDia.Contains(nombre)).ToList();
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
                            throw new Exception("No se encontró un asueto en que coincida con el nombre indicado");
                        }
                    }
                    else
                    {
                        throw new Exception("No se encontró un asueto en la ubicación indicada");
                    }
                } 
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = "No se encontró nada"
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

        #endregion
    }
}
