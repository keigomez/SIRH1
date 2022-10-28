using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMontoCaucionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CMontoCaucionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Métodos

        public CRespuestaDTO AgregarMontoCaucion(MontoCaucion monto)
        {
            CRespuestaDTO respuesta;

            try
            {
                entidadBase.MontoCaucion.Add(monto);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = monto.PK_MontoCaucion
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

        public CRespuestaDTO EditarMontoCaucion(MontoCaucion monto)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosMonto = entidadBase.MontoCaucion.Where(M => M.PK_MontoCaucion == monto.PK_MontoCaucion).FirstOrDefault();

                if (datosMonto != null)
                {
                    datosMonto.IndEstadoMonto = 2;
                    datosMonto.FecVencimiento = monto.FecVencimiento;
                    datosMonto.DesJustificacionInactivo = monto.DesJustificacionInactivo;

                    monto = datosMonto;

                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = monto.PK_MontoCaucion
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Montos de Caución asociados a la clave especificada.");
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


        public CRespuestaDTO ObtenerMontoCaucion(int codMontoCaucion)
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosMonto = entidadBase.MontoCaucion.Where(M => M.PK_MontoCaucion == codMontoCaucion).FirstOrDefault();

                if (datosMonto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosMonto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Montos de Caución asociados a la clave especificada.");
                }
            }
            catch(Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                
                return respuesta;
            }
        }

        public CRespuestaDTO ListarMontoCaucion()
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosMonto = entidadBase.MontoCaucion.Where(Q => Q.IndEstadoMonto == 1).ToList();

                if (datosMonto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosMonto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Montos de Caución asociados.");
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

        public CRespuestaDTO BuscarMontosCaucion(List<MontoCaucion> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var montosCaucion = BuscarPorParametro(datosPrevios, parametro, elemento);
                if (montosCaucion != null)
                {
                    respuesta.Codigo = 1;
                    respuesta.Contenido = montosCaucion;
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron resultados con los parámetros de búsqueda especificados.");
                }
            }
            catch (Exception error)
            {
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        private List<MontoCaucion> BuscarPorParametro(List<MontoCaucion> monto, object parametro, string elemento)
        {
            string paramString = "";
            int paramInt = 0;
            DateTime paramDateInicio = new DateTime();
            DateTime paramDateFin = new DateTime();
            decimal montoInicial = 0;
            decimal montofinal = 0;

            if(parametro.GetType() == typeof(String))
            {
                paramString = parametro.ToString();
            }

            if (parametro.GetType() == typeof(List<DateTime>))
            {
                paramDateInicio = ((List<DateTime>)parametro).ElementAt(0);
                paramDateFin = ((List<DateTime>)parametro).ElementAt(1);
            }

            if (parametro.GetType() == typeof(List<decimal>))
            {
                montoInicial = ((List<decimal>)parametro).ElementAt(0);
                montofinal = ((List<decimal>)parametro).ElementAt(1);
            }

            if (parametro.GetType() == typeof(Int32))
            {
                paramInt = Convert.ToInt32(parametro);
            }

            if (monto.Count > 0)
            {
                switch (elemento)
                {
                    case "Estado":
                        monto = monto.Where(M => M.IndEstadoMonto == paramInt).ToList();
                        break;
                    case "Puesto":
                        monto = monto.Where(M => M.DesMontoCaucion.Contains(paramString)).ToList();
                        break;
                    case "Fecha":
                        monto = monto.Where(M => M.FecRige >= paramDateInicio && M.FecRige <= paramDateFin).ToList();
                        break;
                    case "Monto":
                        monto = monto.Where(M => M.MtoColones >= montoInicial && M.MtoColones <= montofinal).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Estado":
                        monto = entidadBase.MontoCaucion.Where(M => M.IndEstadoMonto == paramInt).ToList();
                        break;
                    case "Puesto":
                        monto = entidadBase.MontoCaucion.Where(M => M.DesMontoCaucion.Contains(paramString)).ToList();
                        break;
                    case "Fecha":
                        monto = entidadBase.MontoCaucion.Where(M => M.FecRige >= paramDateInicio && M.FecRige <= paramDateFin).ToList();
                        break;
                    case "Monto":
                        monto = entidadBase.MontoCaucion.Where(M => M.MtoColones >= montoInicial && M.MtoColones <= montofinal).ToList();
                        break;
                    default:
                        break;
                }
            }

            return monto;
        }
        
        #endregion

    }
}
