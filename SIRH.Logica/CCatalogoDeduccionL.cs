using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCatalogoDeduccionL
    {
         #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCatalogoDeduccionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CCatalogoDeduccionDTO ConvertirDatosCatalogoDeduccionesADto(CatalogoDeduccion item)
        {

            return new CCatalogoDeduccionDTO
            {
                IdEntidad = item.PK_CatalogoDeduccion,
                DescripcionDeduccion = item.DesDeduccion,
                PorcentajeDeduccion = item.PorDeduccion
            };
        }

        /// <summary>
        /// Busca todo el catalogo de deducción
        /// </summary>
        /// <returns>Retorna una lista que contiene todas las deducciones almacenadas en la BD</returns>
        public List<CBaseDTO> ObtenerCatalogoDeduccion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCatalogoDeduccionD intermedio = new CCatalogoDeduccionD(contexto);

                CTipoDeduccionD intermediotipoDeduccion = new CTipoDeduccionD(contexto);


                var catalogoDeduccion = intermedio.BuscarCatalogoDeduccion(codigo);

                if (catalogoDeduccion.Codigo > 0)
                {
                    var datoCatalogo = ConvertirDatosCatalogoDeduccionesADto((CatalogoDeduccion)catalogoDeduccion.Contenido);

                    respuesta.Add(datoCatalogo);

                    var tipoDeduccion = ((CatalogoDeduccion)catalogoDeduccion.Contenido).TipoDeduccion;
                    
                    respuesta.Add(new CTipoDeduccionDTO
                    {
                        DescripcionTipoDeduccion = tipoDeduccion.DesTipDeduccion,
                        IdEntidad = tipoDeduccion.PK_TipoDeduccion
                    });
                }
                else
                {
                    respuesta.Add((CErrorDTO)catalogoDeduccion.Contenido);
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

        /// <summary>
        /// Busca deducciones según el tipo que recibe por parámetros
        /// </summary>
        /// <returns>Retorna una lista con las deducciones encontradas</returns>
        public List<List<CBaseDTO>> ListarDeduccionesTipo(int tipo)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CTipoDeduccionD intermedio = new CTipoDeduccionD(contexto);
                CCatalogoDeduccionD intermedioCatalogo = new CCatalogoDeduccionD(contexto);

                var tipoDeduccion = intermedio.BuscarTipoDeduccion(tipo);
                var catalogo = intermedioCatalogo.ListarCatalogoDeduccionPorTipo(tipo);

                if (catalogo.Codigo > 0)
                {
                    foreach (var deduccion in (List<CatalogoDeduccion>)catalogo.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoCatalogo = ConvertirDatosCatalogoDeduccionesADto(deduccion);
                        temporal.Add(datoCatalogo);

                        var tipoD = CTipoDeduccionL.ConvertirTipoDiaADto((TipoDeduccion)tipoDeduccion.Contenido);
                        temporal.Add(tipoD);

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)catalogo.Contenido);
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
        /// Busca las deducciones de un tipo especifico, efectuadas a un trámite de pago especifico
        /// </summary>
        /// <returns>Retorna una lista con las deducciones encontradas</returns>
        public List<List<CBaseDTO>> ListarDeduccionesPagoTipo(int tipo, int codigo)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CTipoDeduccionD intermedio = new CTipoDeduccionD(contexto);
                CCatalogoDeduccionD intermedioCatalogo = new CCatalogoDeduccionD(contexto);

                var tipoDeduccion = intermedio.BuscarTipoDeduccion(tipo);
                var catalogo = intermedioCatalogo.ListarDeduccionPagoPorTipo(codigo,tipo);

                if (catalogo.Codigo > 0)
                {
                    foreach (var deduccion in (List<DeduccionEfectuada>)catalogo.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoCatalogo = CDeduccionEfectuadaL.ConvertirDeduccionEfectuadaADto(deduccion);
                        temporal.Add(datoCatalogo);

                        var datoDeduccion = ConvertirDatosCatalogoDeduccionesADto(deduccion.CatalogoDeduccion);
                        temporal.Add(datoDeduccion);

                        var tipoD = CTipoDeduccionL.ConvertirTipoDiaADto((TipoDeduccion)tipoDeduccion.Contenido);
                        temporal.Add(tipoD);

                        resultado.Add(temporal);
                    }
                }
                else
                {
                    temporal.Add((CErrorDTO)catalogo.Contenido);
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

        #endregion
    }
}
