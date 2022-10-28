using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCatalogoDiaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCatalogoDiaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Convierte una entidad en un objeto DTO
        /// </summary>
        /// <returns>Retorna el objeto DTO</returns>
        internal static CCatalogoDiaDTO ConvertirDatosCatalogoDADto(CatalogoDia item)
        {
            string auxMes= item.FecDia.Substring(3, 2);
            auxMes = definirMes(auxMes);
            return new CCatalogoDiaDTO
            {
                IdEntidad = item.PK_CatalogoDia,
                DescripcionDia = item.DesDia,
                Mes = auxMes,
                Dia = item.FecDia.Substring(0, 2)
            };
        }

        /// <summary>
        /// Agrega un día de asueto a la BD
        /// </summary>
        /// <returns>Retorna el asueto registrado</returns>
        public CBaseDTO AgregarAsueto(CCatalogoDiaDTO asueto, CTipoDiaDTO tipoDia)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCatalogoDiaD intermedio = new CCatalogoDiaD(contexto);

                CTipoDiaD intermedioTipoDia = new CTipoDiaD(contexto);

                CatalogoDia datosAsueto = new CatalogoDia
                {
                    DesDia = asueto.DescripcionDia,
                    FecDia = asueto.Dia + "/" + asueto.Mes
                };

                var tipoDiaAsueto = intermedioTipoDia.BuscarTipoDia(tipoDia.IdEntidad);


                if (tipoDiaAsueto.Codigo != -1)
                {
                  datosAsueto.TipoDia = (TipoDia)(tipoDiaAsueto.Contenido);
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)tipoDiaAsueto).Contenido;
                    throw new Exception();
                }
                var datoTipo = datosAsueto.TipoDia = (TipoDia)(tipoDiaAsueto.Contenido); 

                respuesta = intermedio.AgregarDiaAsueto(datosAsueto, datoTipo);

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
        /// Obtiene un catalogo de día específico
        /// </summary>
        /// <returns>Retorna el catálogo de día</returns>
        public List<CBaseDTO> ObtenerCatalogoDia(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCatalogoDiaD intermedio = new CCatalogoDiaD(contexto);

                CTipoDiaD intermediotipoDia = new CTipoDiaD(contexto);


                var catalogoDia = intermedio.BuscarCatalogoDia(codigo);

                if (catalogoDia.Codigo > 0)
                {
                    var datoCatalogo = ConvertirDatosCatalogoDADto((CatalogoDia)catalogoDia.Contenido);

                    respuesta.Add(datoCatalogo);

                    var tipoDia = ((CatalogoDia)catalogoDia.Contenido).TipoDia;

                    respuesta.Add(new CTipoDiaDTO
                    {
                        DescripcionTipoDia = tipoDia.DesTipoDia,
                        IdEntidad = tipoDia.PK_TipoDia
                    });
                }
                else
                {
                    respuesta.Add((CErrorDTO)catalogoDia.Contenido);
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
        /// Obtiene una lista de días según el tipo: feriado o de asueto
        /// </summary>
        /// <returns>Retorna la lista de días</returns>
        public List<List<CBaseDTO>> ListarDiasPorTipo(int tipo)
        {
            List<CBaseDTO> temporal = new List<CBaseDTO>();
            List<List<CBaseDTO>> resultado = new List<List<CBaseDTO>>();

            try
            {
                CTipoDiaD intermedio = new CTipoDiaD(contexto);
                CCatalogoDiaD intermedioCatalogo = new CCatalogoDiaD(contexto);
               
                var tipoDia = intermedio.BuscarTipoDia(tipo);
                var catalogo = intermedioCatalogo.ListarCatalogoDiaPorTipo(((TipoDia)tipoDia.Contenido).PK_TipoDia);

                if (catalogo.Codigo > 0)
                {
                    foreach (var dia in (List<CatalogoDia>)catalogo.Contenido)
                    {
                        temporal = new List<CBaseDTO>();

                        var datoCatalogo = ConvertirDatosCatalogoDADto((CatalogoDia)dia);
                        temporal.Add(datoCatalogo);

                        var tipoD = CTipoDiaL.ConvertirTipoDiaADto((TipoDia)tipoDia.Contenido);
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
        /// Define el mes a partir del número
        /// </summary>
        /// <returns>Retorna el nombre del mes</returns>
        private static string definirMes(string mes)
        {
            switch (mes)
            {
                case "01":
                    return "Enero";
                case "02":
                    return "Febrero";
                case "03":
                    return "Marzo";
                case "04":
                    return "Abril";
                case "05":
                    return "Mayo";
                case "06":
                    return "Junio";
                case "07":
                    return "Julio";
                case "08":
                    return "Agosto";
                case "09":
                    return "Setiembre";
                case "10":
                    return "Octubre";
                case "11":
                    return "Noviembre";
                case "12":
                    return "Diciembre";
                default:
                    return " ";
            }
        }

        #endregion
    }
}
