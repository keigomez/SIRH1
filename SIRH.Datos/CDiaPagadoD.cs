using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDiaPagadoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDiaPagadoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca el día pagado que corresponde con el código que recibe por parámetros
        /// </summary>
        /// <returns>Retorna el registro del día pagado especificado</returns>
        public CRespuestaDTO BuscarDiaPagado(int codDiaPagado)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DiaPagado.Include("PagoFeriado").Include("PagoFeriado.PagoExtraordinario").Include("PagoFeriado.PagoExtraordinario.Funcionario")
                    .Include("CatalogoDia").Include("CatalogoDia.TipoDia").Where(T => T.PK_DiaPagado == codDiaPagado).FirstOrDefault();

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
                    throw new Exception("No se encontró el día pagado indicado");
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
        /// Busca todos los días pagados registrados en la base de datos
        /// </summary>
        /// <returns>Retorna todos los días pagados almaceenados en la BD</returns>
        public CRespuestaDTO ListarDiaPagado()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DiaPagado.ToList();

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
                    throw new Exception("No se encontró ningún día pagado registrado");
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
        /// Guarda un día pagado en la BD
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CRespuestaDTO AgregarDiaPagado(DiaPagado diaPagado, Funcionario funcionario, PagoExtraordinario pagoExtraordinario, PagoFeriado pagoFeriado)
        {
            CRespuestaDTO respuesta;
            try
            {

                funcionario = entidadBase.Funcionario
                     .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null)
                {  //existe el funcionario
                    pagoExtraordinario = entidadBase.PagoExtraordinario.
                        Where(PE => PE.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario &&
                            PE.Funcionario.PK_Funcionario == funcionario.PK_Funcionario).FirstOrDefault();

                    if (pagoExtraordinario != null)
                    {//existe un pago extraordinario
                        CatalogoDia catalogoDia = entidadBase.CatalogoDia.Where(CD => CD.PK_CatalogoDia == diaPagado.CatalogoDia.PK_CatalogoDia).FirstOrDefault();

                        var validarPE = entidadBase.Funcionario.Where(F => F.PagoExtraordinario.Where(PE =>
                              PE.PagoFeriado.Where(PF => PF.DiaPagado.Where(DP =>
                                  DP.AnioPago == diaPagado.AnioPago && DP.CatalogoDia.PK_CatalogoDia == catalogoDia.PK_CatalogoDia && DP.PagoFeriado.EstadoTramite.PK_EstadoTramite != 2).Count() > 0).Count() > 0).Count() > 0);


                        if (validarPE != null)
                        { //no existe un pago extraordinario para el dia especifico
                            pagoFeriado = entidadBase.PagoFeriado.Where(PF => PF.PK_PagoFeriado == pagoFeriado.PK_PagoFeriado
                               && PF.PagoExtraordinario.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario).FirstOrDefault();
                            if (pagoFeriado != null)
                            { //si existe un trámite de pago de feriado correspondiente

                                var validarDiaPagado = entidadBase.Funcionario.Where(F => F.PK_Funcionario == funcionario.PK_Funcionario &&
                                (F.PagoExtraordinario.Where(P => P.Funcionario.PK_Funcionario == F.PK_Funcionario &&
                                (P.PagoFeriado.Where(PF => (PF.DiaPagado.Where(DP => DP.CatalogoDia.PK_CatalogoDia == diaPagado.CatalogoDia.PK_CatalogoDia
                                    && DP.AnioPago == diaPagado.AnioPago && DP.PagoFeriado.EstadoTramite.PK_EstadoTramite != 2)).Count() > 0
                                 )).Count() > 0
                                ).Count() > 0));

                                if (validarDiaPagado.Count() == 0) //si no ha encontrado un pago para el día especifico
                                {
                                    pagoFeriado.DiaPagado.Add(diaPagado);
                                    catalogoDia.DiaPagado.Add(diaPagado);

                                    entidadBase.DiaPagado.Add(diaPagado);

                                    entidadBase.SaveChanges();
                                    respuesta = new CRespuestaDTO
                                    {
                                        Codigo = 1,
                                        Contenido = "Guardado"
                                    };
                                    return respuesta;
                                }
                                else
                                {
                                    throw new Exception("El día indicado ya fue pagado previamente");
                                }
                            }

                            else
                            {
                                throw new Exception("No existe el registro de pago de feriado indicado");
                            }
                        }
                        else
                        {
                            throw new Exception("Ya existe un trámite de pago para el día indicado");
                        }
                    }
                    else
                    {
                        throw new Exception("No existe el registro de pago extraordinario indicado");
                    }

                }
                else
                {
                    throw new Exception("No existe el funcionario indicado");
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
        /// Busca los días pagados al tramite de pago de feriado correspondiente
        /// </summary>
        /// <returns>Retorna el registro del catálogo especificado</returns>
        public CRespuestaDTO ListarDiasPagadosPorPago(int codigoPago)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DiaPagado.Include("CatalogoDia").Where(DP => DP.PagoFeriado.PK_PagoFeriado == codigoPago).ToList();

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
                    throw new Exception("No se encontró ningún día pagado correspondiente al código de la transacción indicado");
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
        /// Elimina de la base de datos el día pagado que corresponde con el código que recibe por parámetros
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CRespuestaDTO EliminarDiaPagado(DiaPagado diaPagado)
        {
            CRespuestaDTO respuesta;
            try
            {
                diaPagado = entidadBase.DiaPagado
                    .Where(F => F.PK_DiaPagado == diaPagado.PK_DiaPagado).FirstOrDefault();

                if (diaPagado != null)
                {
                    entidadBase.DiaPagado.Remove(diaPagado);
                    entidadBase.SaveChanges();


                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = "Eliminado"
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el dia pagado indicado");
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
        
        #endregion
    }
}
