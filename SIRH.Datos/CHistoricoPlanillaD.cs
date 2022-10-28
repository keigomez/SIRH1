using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CHistoricoPlanillaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CHistoricoPlanillaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO ObtenerPagoID(int idPago)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.C_EMU_HistoricoPlanilla.Where(Q => Q.ID == idPago).FirstOrDefault();
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

        public CRespuestaDTO BuscarDatosPlanilla(List<C_EMU_HistoricoPlanilla> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatosPlanilla(elemento, datosPrevios, parametro);
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

        private List<C_EMU_HistoricoPlanilla> CargarDatosPlanilla(string elemento, List<C_EMU_HistoricoPlanilla> datosPrevios, object parametro)
        {
            string sParametro = "";
            string FInicio = "";
            string FFin = "";
            string fmt = "00";
            DateTime FechaInicio = new DateTime();
            DateTime FechaFinal = new DateTime();

            if (elemento == "fechas")
            {
                List<DateTime> fechas = (List<DateTime>)parametro;
                FechaInicio = fechas.ElementAt(0);
                FechaFinal = fechas.ElementAt(1);
                FInicio = FechaInicio.Year.ToString() + FechaInicio.Month.ToString(fmt) + FechaInicio.Day.ToString(fmt);
                FFin = FechaFinal.Year.ToString() + FechaFinal.Month.ToString(fmt) + FechaFinal.Day.ToString(fmt);
            }
            else
            {
                sParametro = parametro.ToString();
            }

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "cedula":
                        datosPrevios = entidadBase.C_EMU_HistoricoPlanilla.Where(Q => Q.cedula == sParametro).ToList();
                        break;
                    case "fechas":
                            datosPrevios = entidadBase.C_EMU_HistoricoPlanilla.Where(Q => Convert.ToInt64(Q.fecha_periodo) >= Convert.ToInt64(FInicio)
                                                                                      && Convert.ToInt64(Q.fecha_periodo) <= Convert.ToInt64(FFin))
                                                                                     .ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "cedula":
                        datosPrevios = datosPrevios.Where(Q => Q.cedula == sParametro).ToList();
                        break;
                    case "fechas":
                            datosPrevios = datosPrevios.Where(Q => Convert.ToInt64(Q.fecha_periodo) >= Convert.ToInt64(FInicio)
                                                                && Convert.ToInt64(Q.fecha_periodo) <= Convert.ToInt64(FFin))
                                                                .ToList();
                        break;
                    default:
                        break;
                }
            }
            return datosPrevios;
        }

        #endregion
    }
}
