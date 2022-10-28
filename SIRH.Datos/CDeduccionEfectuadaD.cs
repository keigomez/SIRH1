using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDeduccionEfectuadaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDeduccionEfectuadaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Busca una deduccion efectuada según el párametro recibido
        /// </summary>
        /// <returns>Retorna la deducción</returns>
        public CRespuestaDTO BuscarDeduccionEfectuada(int codDeduccionEfectuada)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DeduccionEfectuada.Include("PagoFeriado").Include("PagoFeriado.PagoExtraordinario").Include("PagoFeriado.PagoExtraordinario.Funcionario")
                    .Include("CatalogoDeduccion").Include("CatalogoDeduccion.TipoDeduccion").Where(T => T.PK_DeduccionEfectuada == codDeduccionEfectuada).FirstOrDefault();

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
                    throw new Exception("No se encontró la deducción efectuada indicada");
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
        /// Busca todas las deducciones efectuadas registradas en la BD
        /// </summary>
        /// <returns>Retorna una lista de deducciones efectuadas</returns>
        public CRespuestaDTO ListarTipoDeduccion()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DeduccionEfectuada.ToList();

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
                    throw new Exception("No se encontró ninguna deducción efectuada");
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
        /// Registra una deducción efectuada en la BD
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CRespuestaDTO AgregarDeduccionEfectuada(DeduccionEfectuada deduccionEfectuada, Funcionario funcionario, PagoExtraordinario pagoExtraordinario, PagoFeriado pagoFeriado)
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
                        pagoFeriado = entidadBase.PagoFeriado.Where(PF => PF.PK_PagoFeriado == pagoFeriado.PK_PagoFeriado
                            && PF.PagoExtraordinario.PK_PagoExtraordinario == pagoExtraordinario.PK_PagoExtraordinario && PF.EstadoTramite.PK_EstadoTramite==1).FirstOrDefault();
                        if (pagoFeriado != null)
                        {//existe un pago de feriado
                            var deduccionEfctda = entidadBase.PagoFeriado.Where(PF=> PF.PK_PagoFeriado == pagoFeriado.PK_PagoFeriado && PF.DeduccionEfectuada.Where(DE => DE.CatalogoDeduccion.PK_CatalogoDeduccion == deduccionEfectuada.CatalogoDeduccion.PK_CatalogoDeduccion).Count()>0);
                            if(deduccionEfctda.Count()==0){
                                //no se ingreso la deduccion respectiva
                            CatalogoDeduccion catalogoDeduccion = entidadBase.CatalogoDeduccion.Where(CD => CD.PK_CatalogoDeduccion == deduccionEfectuada.CatalogoDeduccion.PK_CatalogoDeduccion).FirstOrDefault();

                            pagoFeriado.DeduccionEfectuada.Add(deduccionEfectuada);
                            catalogoDeduccion.DeduccionEfectuada.Add(deduccionEfectuada);

                            entidadBase.DeduccionEfectuada.Add(deduccionEfectuada);
                            entidadBase.SaveChanges();
                            respuesta = new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = "Guardado"
                            };
                            return respuesta;
                        }
                        else{
                        throw new Exception("Ya efectuó la deducción respectiva");
                        }
                        }

                        else
                        {
                            throw new Exception("No existe el registro de pago de feriado indicado");
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
        /// Busca las deducciones efectuadas a un trámite de pago de feriados específico
        /// </summary>
        /// <returns>Retorna la lista de deducciones efectuadas</returns>
        public CRespuestaDTO ListarDeduccionPorPago(int codigoPago)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.DeduccionEfectuada.Include("PagoFeriado").Include("PagoFeriado.PagoExtraordinario")
                    .Include("PagoFeriado.PagoExtraordinario.Funcionario").Include("CatalogoDeduccion").Include("CatalogoDeduccion.TipoDeduccion")
                    .Where(DE => DE.PagoFeriado.PK_PagoFeriado == codigoPago).ToList();

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
                    throw new Exception("No se encontró ninguna deducción efectuada correspondiente al número de trámite indicado");
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
        /// Elimina una deducción efectuada de la BD
        /// </summary>
        /// <returns>Retorna una confirmación</returns>
        public CRespuestaDTO EliminarDeduccionEfectuada(DeduccionEfectuada deduccionEfectuada)
        {
            CRespuestaDTO respuesta;
            try
            {
                deduccionEfectuada = entidadBase.DeduccionEfectuada
                    .Where(F => F.PK_DeduccionEfectuada == deduccionEfectuada.PK_DeduccionEfectuada).FirstOrDefault();

                if (deduccionEfectuada != null)
                {
                    entidadBase.DeduccionEfectuada.Remove(deduccionEfectuada);
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
                    throw new Exception("No se encontró la deducción efectuada indicada");
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
