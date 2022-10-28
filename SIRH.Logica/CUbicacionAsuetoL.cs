using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CUbicacionAsuetoL
    {
         #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CUbicacionAsuetoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Convierte una entidad de ubicación de asueto a DTO
        /// </summary>
        /// <returns>Retorna la ubicación en formato DTO</returns>
        internal static CUbicacionAsuetoDTO ConvertirUbicacionAsuetoADto(UbicacionAsueto item)
        {
            return new CUbicacionAsuetoDTO
            {
                IdEntidad = item.PK_UbicacionAsueto,
            };
        }

        /// <summary>
        /// aGREGA una ubicación de asueto especifica
        /// </summary>
        /// <returns>Retorna la ubicación almacenada</returns>
        public CBaseDTO AgregarUbicacionAsueto(CCantonDTO canton, CCatalogoDiaDTO asueto, CUbicacionAsuetoDTO ubicacionAsueto)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CUbicacionAsuetoD intermedio = new CUbicacionAsuetoD(contexto);

                CCatalogoDiaD intermedioDia = new CCatalogoDiaD(contexto);

                CCantonD intermedioCanton = new CCantonD(contexto);

               CatalogoDia datosCatalogoDia = new CatalogoDia {
                   PK_CatalogoDia = asueto.IdEntidad
                };

                var cantonT = intermedioCanton.BuscarCanton(canton.IdEntidad);

                if (cantonT.Codigo != -1)
                {
                    ubicacionAsueto.Canton = CCantonL.ConvertirCantonADto((Canton)cantonT.Contenido);
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)cantonT).Contenido;
                    throw new Exception();
                }

                Canton datosCanton = new Canton
                {
                    PK_Canton = canton.IdEntidad
                };

                UbicacionAsueto datosUbicacionAsueto = new UbicacionAsueto {
                    PK_UbicacionAsueto = ubicacionAsueto.IdEntidad,
                };

                respuesta = intermedio.AgregarUbicacionAsueto(datosCatalogoDia, datosUbicacionAsueto, datosCanton);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch(Exception e)
            {
                return respuesta;
            }
        }

        /// <summary>
        /// Realiza una búsqueda de asuetos por ubicación
        /// </summary>
        /// <returns>Retorna los asuetos encontrados</returns>
        public List<List<CBaseDTO>> ListarAsuetosPorUbicacion(string provincia, string canton)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CUbicacionAsuetoD intermedio = new CUbicacionAsuetoD(contexto);
                CCatalogoDiaD intermedioCatalogo = new CCatalogoDiaD(contexto);
                CTipoDiaD intermedioTipoDia = new CTipoDiaD(contexto);
               int  cantonAux = Convert.ToInt32(canton);
               var ubicacion = intermedio.BuscarAsuetoPorUbicacion(provincia, cantonAux);
          

                if (ubicacion.Codigo > 0)
                {
                    foreach (var dia in (List<CatalogoDia>)ubicacion.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoCatalogo = CCatalogoDiaL.ConvertirDatosCatalogoDADto(dia);
                        temporal.Add(datoCatalogo);

                        var tipoDia = intermedioTipoDia.BuscarTipoDia(dia.TipoDia.PK_TipoDia);
                        var tipoD = CTipoDiaL.ConvertirTipoDiaADto((TipoDia)tipoDia.Contenido);
                        temporal.Add(tipoD);

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)ubicacion.Contenido);
                    resultado.Add(temporal);
                }

            }
            catch (Exception error)
            {
                temporal.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                resultado.Add(temporal);
            }
            return resultado;
        }

        /// <summary>
        /// Busca los cantones almacenados en la BD
        /// </summary>
        /// <returns>Retorna los cantones</returns>
        public List<List<CBaseDTO>> ListarCantones()
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CUbicacionAsuetoD intermedio = new CUbicacionAsuetoD(contexto);
                CCantonD intermedioCanton = new CCantonD(contexto);
                CTipoDiaD intermedioTipoDia = new CTipoDiaD(contexto);

                var cantones = intermedioCanton.ListarCantones();


                if (cantones.Codigo > 0)
                {
                    foreach (var canton in (List<Canton>)cantones.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoCanton = CCantonL.ConvertirCantonADto(canton);
                        temporal.Add(datoCanton);

                        CProvinciaDTO provincia = new CProvinciaDTO
                        {
                            IdEntidad = canton.Provincia.PK_Provincia,
                            NomProvincia = canton.Provincia.NomProvincia
                        };
                        temporal.Add(provincia);

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)cantones.Contenido);
                    resultado.Add(temporal);
                }

            }
            catch (Exception error)
            {
                temporal.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                resultado.Add(temporal);
            }
            return resultado;
        }

        /// <summary>
        /// Busca un cantón específico
        /// </summary>
        /// <returns>Retorna el cantón</returns>
        public List<CBaseDTO> ObtenerCanton(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCantonD intermedio = new CCantonD(contexto);

                var canton = intermedio.BuscarCanton(codigo);

                if (canton.Codigo > 0)
                {
                    var cantonDTO = CCantonL.ConvertirCantonADto((Canton)canton.Contenido);
                    var datoCanton = cantonDTO;
                    respuesta.Add(datoCanton);
                    CProvinciaDTO provincia = new CProvinciaDTO
                    {
                        IdEntidad = ((Canton)canton.Contenido).Provincia.PK_Provincia,
                        NomProvincia = ((Canton)canton.Contenido).Provincia.NomProvincia
                    };
                    respuesta.Add(provincia);
                }
                else
                {
                    respuesta.Add((CErrorDTO)canton.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        #endregion
    }
}
