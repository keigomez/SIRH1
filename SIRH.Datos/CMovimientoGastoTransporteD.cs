using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMovimientoGastoTransporteD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CMovimientoGastoTransporteD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo Movimiento Gasto Transporte como respuesta DTO
        /// </summary>
        /// <param name="movimientogastotransporte"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarMovimientoGastoTransporte(MovimientoGastoTransporte movimientogastotransporte)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoGastoT = entidadBase.GastoTransporte
                    .Where(VC => VC.PK_GastosTransporte == movimientogastotransporte.GastoTransporte.PK_GastosTransporte).FirstOrDefault();
                if (datoGastoT != null)
                {
                    entidadBase.MovimientoGastoTransporte.Add(movimientogastotransporte);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = movimientogastotransporte
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("Error movimientoViaticoCorrido");
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
        /// Metodo encargado de anular un viatico corrido
        /// </summary>
        /// <param name="viaticoC"></param>
        /// <returns></returns>
        public CRespuestaDTO AnularMovimientoGastoTransporte(MovimientoGastoTransporte mGastoT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var mGastoTransporteOld = entidadBase.MovimientoGastoTransporte.FirstOrDefault(D => D.PK_MovimientoGastosTransporte == mGastoT.PK_MovimientoGastosTransporte);
                if (mGastoTransporteOld != null)
                {
                    mGastoTransporteOld.IndEstado = 3;
                    //mViaticoCorridoOld.ObsObservacion = mViaticoC.ObsViaticoCorrido;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = mGastoT
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Movimiento");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }
        /// <summary>
        /// metodo encargado de optener un solo movimiento gasto trasnporte
        /// </summary>
        /// <param name="codGastoT"></param>
        /// <returns></returns>
        public CRespuestaDTO ObtenerMovimientoGastoTransporte(string codGastoT)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(codGastoT);
                //var datosBusqueda = codDesaraigo.Split('-'); // [año,consecutivo]
                //var annio = int.Parse(datosBusqueda[0]);
                //var consecutivo = int.Parse(datosBusqueda[1]) - 1;

                var gastoTransporte = entidadBase.MovimientoGastoTransporte.FirstOrDefault(D => D.PK_MovimientoGastosTransporte == cod);
                //.Where(D => D.FecInicDesarraigo.Value.Year == annio)
                //.OrderBy(D => D.FecInicDesarraigo).ToList().ElementAtOrDefault(consecutivo);
                if (gastoTransporte != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = gastoTransporte,
                        //Mensaje = GenerarCodigoInforme(gastoTransporte)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el gastoTransporte");
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
        /// Metodo encargado de listar historico de movimientos segundo el codigo o tipo que se eligio
        /// </summary>
        /// <param name="funcionario"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public List<MovimientoGastoTransporte> BuscarMovimientoGastoTransporte(Funcionario funcionario, String codigo)
        {
            List<MovimientoGastoTransporte> resultado = new List<MovimientoGastoTransporte>();

            try
            {
                if (codigo == "2")
                {
                    var datos = entidadBase.MovimientoGastoTransporte.Where(G => G.GastoTransporte.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario && G.NumTipo == 2).ToList();
                    if (datos != null)
                    {
                        resultado = datos;
                        return resultado.OrderByDescending(Q => Q.PK_MovimientoGastosTransporte).ToList();
                    }
                    else
                    {
                        throw new Exception("No se encontraron Preguntas asociadas al tipoFormulario.");
                    }
                }
                else if (codigo == "3")
                {
                    var datos = entidadBase.MovimientoGastoTransporte.Where(C => C.GastoTransporte.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario && C.NumTipo == 3).ToList();
                    if (datos != null)
                    {
                        resultado = datos;
                        return resultado.OrderByDescending(Q => Q.PK_MovimientoGastosTransporte).ToList();
                    }
                    else
                    {
                        throw new Exception("No se encontraron Preguntas asociadas al tipoFormulario.");
                    }
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        public List<GastoTransporteReintegroDias> BuscarSolicitudReintegro(Funcionario funcionario)
        {
            List<GastoTransporteReintegroDias> resultado = new List<GastoTransporteReintegroDias>();

            try
            {
                var datos = entidadBase.GastoTransporteReintegroDias.Where(C => C.GastoTransporte.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).ToList();
                if (datos != null)
                {
                    resultado = datos;
                    return resultado.OrderByDescending(Q => Q.PK_Reintegro).ToList();
                }
                else
                {
                    throw new Exception("No se encontraron registros.");
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        public CRespuestaDTO EditarReintegro(int idReintegro, int estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var mViaticoCorridoOld = entidadBase.GastoTransporteReintegroDias.FirstOrDefault(D => D.PK_Reintegro == idReintegro);
                if (mViaticoCorridoOld != null)
                {
                    mViaticoCorridoOld.IndEstado = estado;
                    //mViaticoCorridoOld.ObsObservacion = mViaticoC.ObsViaticoCorrido;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = mViaticoCorridoOld
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el Movimiento");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        #endregion
    }
}
