using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CUbicacionPuestoL
    {
        #region Variables

        SIRHEntities contexto;
        CUbicacionPuestoD ubicacionDescarga;
        CProvinciaD provinciaDescarga;
        CCantonD cantonDescarga;
        CDistritoD distritoDescarga;

        #endregion

        #region Constructor

        public CUbicacionPuestoL()
        {
           contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        //hola
        public List<List<CBaseDTO>> GetLocalizacion(bool cantones, int canton, bool distritos, bool provincias, int provincia)
        {
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();
            try
            {
                if (cantones)
                {
                    if (provincia < 1)
                    {
                        var can = (new CCantonD(contexto)).CargarCantones()
                                                          .Select(C => (CBaseDTO)new CCantonDTO { IdEntidad = C.PK_Canton, NomCanton = C.NomCanton, Provincia = new CProvinciaDTO { IdEntidad = C.Provincia.PK_Provincia } })
                                                          .ToList();
                        resultado.Add(can);
                    }
                    else
                    {
                        var can = ((List<Canton>)(new CCantonD(contexto)).BuscarCantonProvincia(Convert.ToInt32(provincia)).Contenido)
                                  .Select(C => (CBaseDTO)new CCantonDTO { IdEntidad = C.PK_Canton, NomCanton = C.NomCanton, Provincia = new CProvinciaDTO { IdEntidad = C.Provincia.PK_Provincia } })
                                  .ToList();
                        resultado.Add(can);
                    }
                }
                if (distritos)
                {
                    if (canton < 1)
                    {
                        var dis = (new CDistritoD(contexto)).CargarDistritos()
                                                            .Select(D => (CBaseDTO)new CDistritoDTO { IdEntidad = D.PK_Distrito, NomDistrito = D.NomDistrito, Canton = new CCantonDTO { IdEntidad = D.Canton.PK_Canton } })
                                                            .ToList();
                        resultado.Add(dis);
                    }
                    else
                    {
                        var dis = (new CDistritoD(contexto)).CargarDistritosPorCanton(Convert.ToInt32(canton))
                                    .Select(D => (CBaseDTO)new CDistritoDTO { IdEntidad = D.PK_Distrito, NomDistrito = D.NomDistrito, Canton = new CCantonDTO { IdEntidad = D.Canton.PK_Canton } })
                                    .ToList();
                        resultado.Add(dis);
                    }
                }
                if (provincias)
                {
                    var prov = (new CProvinciaD(contexto)).CargarProvincias(null)
                                                          .Select(P => (CBaseDTO)new CProvinciaDTO { IdEntidad = P.PK_Provincia, NomProvincia = P.NomProvincia })
                                                          .ToList();
                    resultado.Add(prov);
                }
            }
            catch (Exception error)
            {
                resultado = new List<List<CBaseDTO>>();
                var aux = new List<CBaseDTO>();
                aux.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                resultado.Add(aux);
                return resultado;
            }
            return resultado;
        }

        //Se insertó en PuestoService el 30/01/2017
        public List<List<CBaseDTO>> CargarUbicacionPuesto(string cedula)
        {
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            ubicacionDescarga = new CUbicacionPuestoD(contexto);
            provinciaDescarga = new CProvinciaD(contexto);
            cantonDescarga = new CCantonD(contexto);
            distritoDescarga = new CDistritoD(contexto);

            foreach (var item in ubicacionDescarga.CargarUbicacionPuestoCedula(cedula))
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                if (item.Distrito != null)
                {
                    temp.Add(new CTipoUbicacionDTO { DesTipoUbicacion = item.TipoUbicacion.DesTipoUbicacion });
                    var tempDistrito = distritoDescarga.CargarDistritos().Where(Q => Q.CodPostalDistrito == item.Distrito.CodPostalDistrito && Q.PK_Distrito == item.Distrito.PK_Distrito).FirstOrDefault();
                    var tempCanton = cantonDescarga.CargarCantones().Where(Q => Q.CodPostalCanton == tempDistrito.Canton.CodPostalCanton && Q.PK_Canton == tempDistrito.Canton.PK_Canton).FirstOrDefault();
                    var tempProvincia = provinciaDescarga.CargarProvincias(provinciaDescarga).Where(Q => Q.PK_Provincia == tempCanton.Provincia.PK_Provincia).FirstOrDefault();
                    temp.Add(new CProvinciaDTO { IdEntidad = tempProvincia.PK_Provincia, NomProvincia = tempProvincia.NomProvincia });
                    temp.Add(new CCantonDTO { IdEntidad = tempCanton.PK_Canton, NomCanton = tempCanton.NomCanton });
                    temp.Add(new CDistritoDTO { IdEntidad = tempDistrito.PK_Distrito, NomDistrito = tempDistrito.NomDistrito });
                    temp.Add(new CUbicacionPuestoDTO { ObsUbicacionPuesto = item.ObsUbicacionPuesto });
                }
                else
                {
                    temp.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }

                resultado.Add(temp);
            }

            return resultado;
        }

        internal static CUbicacionPuestoDTO ConvertirUbicacionPuestoADTO(UbicacionPuesto item)
        {
            return new CUbicacionPuestoDTO
            {
                IdEntidad = item.PK_UbicacionPuesto,
                ObsUbicacionPuesto = item.ObsUbicacionPuesto,
                TipoUbicacion = new CTipoUbicacionDTO
                {
                    IdEntidad = item.TipoUbicacion.PK_TipoUbicacion,
                    DesTipoUbicacion = item.TipoUbicacion.DesTipoUbicacion
                },
                Distrito = new CDistritoDTO
                {
                    IdEntidad = item.Distrito.PK_Distrito,
                    NomDistrito = item.Distrito.NomDistrito,
                    Canton = new CCantonDTO
                    {
                        IdEntidad = item.Distrito.Canton.PK_Canton,
                        NomCanton = item.Distrito.Canton.NomCanton,
                        Provincia = new CProvinciaDTO 
                        {
                            IdEntidad = item.Distrito.Canton.Provincia.PK_Provincia,
                            NomProvincia = item.Distrito.Canton.Provincia.NomProvincia
                        }
                    }
                }
            };
        }

        public List<CBaseDTO> DescargarUbicacionTrabajoPedimento(string codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CUbicacionPuestoD intermedio = new CUbicacionPuestoD(contexto);

                var resultado = intermedio.BuscarUbicacionTrabajoPedimento(codigo);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(CPuestoL.ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()));
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()));
                    respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 1
                                                                                   && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto));
                    respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 2
                                                                                   && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto));
                    respuesta.Add(CPedimentoPuestoL.ConvertirDatosPedimentoPuestoADTO(((Puesto)resultado.Contenido).PedimentoPuesto.FirstOrDefault()));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> BuscarHistorialUbicacionTrabajo(string codigo)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                CUbicacionPuestoD intermedio = new CUbicacionPuestoD(contexto);

                var resultado = intermedio.BuscarHistorialUbicacionTrabajo(codigo);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(new List<CBaseDTO> { CPuestoL.ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()) });
                    respuesta.Add(new List<CBaseDTO> { CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()) });
                    respuesta.Add(new List<CBaseDTO> { CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 1
                                                                                   && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto) });
                    var ubicacionesTrabajo = ((Puesto)resultado.Contenido)
                                                .RelPuestoUbicacion.Where(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 2).ToList();

                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    foreach (var item in ubicacionesTrabajo)
                    {
                        temp.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(item.UbicacionPuesto));
                    }

                    respuesta.Add(temp);

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { Codigo = -1, MensajeError = error.Message } });
                return respuesta;
            }
        }

        public CBaseDTO ModificarUbicacionPuesto(CPuestoDTO puesto, CUbicacionPuestoDTO ubicacion)
        {
            try
            {
                var resultado = (new CUbicacionPuestoD(contexto)).ModificarUbicacionPuesto(puesto.CodPuesto, ubicacion);
                if (resultado.Codigo > 0)
                {
                    return resultado;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    MensajeError = error.Message
                };
            }
        }

        public List<CBaseDTO> BuscarUbicacionPuestoCodigo(string codPuesto, int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                var resultado = (new CUbicacionPuestoD(contexto)).BuscarUbicacionPuestoCodigo(codPuesto, codigo);

                if (resultado.Codigo > 0)
                {
                    respuesta.Add(CPuestoL.ConstruirPuesto(((Puesto)resultado.Contenido), new CPuestoDTO()));
                    respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Puesto)resultado.Contenido).DetallePuesto.FirstOrDefault()));
                    respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                                    .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 1
                                                                                                       && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto));
                    respuesta.Add(CUbicacionPuestoL.ConvertirUbicacionPuestoADTO(((Puesto)resultado.Contenido)
                                                .RelPuestoUbicacion.FirstOrDefault(Q => Q.UbicacionPuesto.TipoUbicacion.PK_TipoUbicacion == 2
                                                                                   && Q.UbicacionPuesto.IndEstadoUbicacionPuesto == 1).UbicacionPuesto));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = error.Message });
                return respuesta;
            }
        }
        
        #endregion
    }
}
