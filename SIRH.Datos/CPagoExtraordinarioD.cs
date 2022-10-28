using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPagoExtraordinarioD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPagoExtraordinarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca el pago extraordinario en la BD que corresponde con el código que recibe por parámetros
        /// </summary>
        /// <returns>Retorna el registro del pago extraordinario especificado</returns>
        public CRespuestaDTO BuscarPagoExtraordinario(int codPagoExtraordinario)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.PagoExtraordinario.Include("Funcionario").Where(P => P.PK_PagoExtraordinario == codPagoExtraordinario).FirstOrDefault();

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
                    throw new Exception("No se encontró el trámite de pago extraordinario indicado");
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
        /// Busca todos los pagos extraordinarios almacenados en la BD
        /// </summary>
        /// <returns>Retorna TODOS los registros encontrados</returns>
        public CRespuestaDTO ListarPagoExtraordinario()
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
                    throw new Exception("No se encontró ningún trámite de pago extraordinario");
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
        /// Almacena un pago extraordinario en la BD
        /// </summary>
        /// <returns>Retorna la pk del trámite almacenado</returns>
        public CRespuestaDTO AgregarPagoExtraordinario(PagoExtraordinario pagoExtraordinario, Funcionario funcionario)
        {
            CRespuestaDTO respuesta;
            try
            {
                funcionario = entidadBase.Funcionario
                    .Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).FirstOrDefault();

                if (funcionario != null)
                {
                    funcionario.PagoExtraordinario.Add(pagoExtraordinario);
                    entidadBase.PagoExtraordinario.Add(pagoExtraordinario);
                    
                    entidadBase.SaveChanges();
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = pagoExtraordinario.PK_PagoExtraordinario
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el funcionario indicado");
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
        /// Elimina el registro de pago extraordinario indicado
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CRespuestaDTO EliminarPagoExtraordinario(PagoExtraordinario pagoExtraordinario)
        {
            CRespuestaDTO respuesta;
            try
            {
                pagoExtraordinario = entidadBase.PagoExtraordinario
                    .Where(F => F.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario).FirstOrDefault();

                if (pagoExtraordinario != null)
                {
                    entidadBase.PagoExtraordinario.Remove(pagoExtraordinario);
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
                    throw new Exception("No se encontró el pago extraordinario indicado");
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
