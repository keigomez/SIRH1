using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CMovimientoViaticoCorridoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CMovimientoViaticoCorridoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        #region Metodos
        /// <summary>
        /// Metodo encargador de guardar en base de datos un viatico corrido y restorna un objeto de tipo Movimiento Viatico Corrido como respuesta DTO
        /// </summary>
        /// <param name="movimientoViaticoCorrido"></param>
        /// <returns></returns>
        public CRespuestaDTO AgregarMovimientoViaticoCorrido(MovimientoViaticoCorrido movimientoViaticoCorrido)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datoViaticoC = entidadBase.ViaticoCorrido
                    .Where(VC => VC.PK_ViaticoCorrido == movimientoViaticoCorrido.ViaticoCorrido.PK_ViaticoCorrido).FirstOrDefault();
                if (datoViaticoC != null)
                {
                    entidadBase.MovimientoViaticoCorrido.Add(movimientoViaticoCorrido);
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = movimientoViaticoCorrido
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
        public CRespuestaDTO AnularMovimientoViaticoCorrido(MovimientoViaticoCorrido mViaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var mViaticoCorridoOld = entidadBase.MovimientoViaticoCorrido.FirstOrDefault(D => D.PK_MovimientoViaticoCorrido == mViaticoC.PK_MovimientoViaticoCorrido);
                if (mViaticoCorridoOld != null)
                {
                    mViaticoCorridoOld.IndEstado = 3;
                    //mViaticoCorridoOld.ObsObservacion = mViaticoC.ObsViaticoCorrido;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = mViaticoC
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
        /// metodo encargado de optener un solo movimiento viatico corrido
        /// </summary>
        /// <param name="codViaticoC"></param>
        /// <returns></returns>
        public CRespuestaDTO ObtenerMovimientoViaticoCorrido(string codViaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(codViaticoC);
                //var datosBusqueda = codDesaraigo.Split('-'); // [año,consecutivo]
                //var annio = int.Parse(datosBusqueda[0]);
                //var consecutivo = int.Parse(datosBusqueda[1]) - 1;

                var viaticoCorrido = entidadBase.MovimientoViaticoCorrido.FirstOrDefault(D => D.PK_MovimientoViaticoCorrido == cod);
                //.Where(D => D.FecInicDesarraigo.Value.Year == annio)
                //.OrderBy(D => D.FecInicDesarraigo).ToList().ElementAtOrDefault(consecutivo);
                if (viaticoCorrido != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = viaticoCorrido,
                        // Mensaje = GenerarCodigoInforme(viaticoCorrido)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el viaticoCorrido");
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


        public CRespuestaDTO ObtenerMovimientoViaticoCorridoFecha(string codViaticoC)
        {
            CRespuestaDTO respuesta;
            try
            {
                var cod = int.Parse(codViaticoC);
                //var datosBusqueda = codDesaraigo.Split('-'); // [año,consecutivo]
                //var annio = int.Parse(datosBusqueda[0]);
                //var consecutivo = int.Parse(datosBusqueda[1]) - 1;

                var viaticoCorrido = entidadBase.MovimientoViaticoCorrido.FirstOrDefault(D => D.PK_MovimientoViaticoCorrido == cod);
                //.Where(D => D.FecInicDesarraigo.Value.Year == annio)
                //.OrderBy(D => D.FecInicDesarraigo).ToList().ElementAtOrDefault(consecutivo);
                if (viaticoCorrido != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = viaticoCorrido,
                        // Mensaje = GenerarCodigoInforme(viaticoCorrido)
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el viaticoCorrido");
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
        public List<MovimientoViaticoCorrido> BuscarMovimientoViaticoCorrido(Funcionario funcionario, String codigo)
        {
            List<MovimientoViaticoCorrido> resultado = new List<MovimientoViaticoCorrido>();

            try
            {
                if (codigo == "2") {
                    var datos = entidadBase.MovimientoViaticoCorrido.Where(C => C.ViaticoCorrido.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario && C.NumTipo == 2 ).ToList();
                    if (datos != null)
                    {
                        resultado = datos;
                        return resultado.OrderByDescending(Q => Q.PK_MovimientoViaticoCorrido).ToList();
                    }
                    else
                    {
                        throw new Exception("No se encontraron registros.");
                    }
                } else if (codigo == "3") {
                    var datos = entidadBase.MovimientoViaticoCorrido.Where(C => C.ViaticoCorrido.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario && C.NumTipo ==3).ToList();
                    if (datos != null)
                    {
                        resultado = datos;
                        return resultado.OrderByDescending(Q => Q.PK_MovimientoViaticoCorrido).ToList();
                    }
                    else
                    {
                        throw new Exception("No se encontraron registros.");
                    }
                }
            }
            catch (Exception error)
            {
                string r = error.ToString();
            }
            return resultado;
        }

        public List<ViaticoCorridoReintegroDias> BuscarSolicitudReintegro(Funcionario funcionario)
        {
            List<ViaticoCorridoReintegroDias> resultado = new List<ViaticoCorridoReintegroDias>();

            try
            {
                var datos = entidadBase.ViaticoCorridoReintegroDias.Where(C => C.ViaticoCorrido.Nombramiento.Funcionario.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).ToList();
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
                var mViaticoCorridoOld = entidadBase.ViaticoCorridoReintegroDias.FirstOrDefault(D => D.PK_Reintegro == idReintegro);
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
