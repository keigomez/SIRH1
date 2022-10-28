using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CEntidadFinancieraD
   {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CEntidadFinancieraD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        public List<EntidadFinanciera> CargarEntidadFinanciera()
        {
            List<EntidadFinanciera> resultados = new List<EntidadFinanciera>();

            resultados = contexto.EntidadFinanciera.ToList();

            return resultados;
        }

        //30/01/2017
        public CRespuestaDTO ListarEntidadFinanciera()
        {
            CRespuestaDTO respuesta;
            try            
            {
                var datosEntidad = contexto.EntidadFinanciera.ToList();

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
                    throw new Exception("No se encontraron entidades financieras");
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

        //31/01/17
        public CRespuestaDTO BuscarEntidadFinanciera(int codEntidadFinanciera)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datosEntidad = contexto.EntidadFinanciera.Where(M => M.PK_EntidadFinanciera == codEntidadFinanciera).FirstOrDefault();

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
                    throw new Exception("No se encontró ninguna Entidad financiera");
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
        
        public int GuardarEntidadFinanciera(EntidadFinanciera EntidadFinanciera)
        {
            contexto.EntidadFinanciera.Add(EntidadFinanciera);
            return EntidadFinanciera.PK_EntidadFinanciera;
        }

        public CRespuestaDTO GuardarEntidadFinanciera(string cedula, EntidadFinanciera entidadFinanciera)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.EntidadFinanciera.Add(entidadFinanciera);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = entidadFinanciera
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

        /// <summary>
        /// Obtiene la carga de entidades financieras de la BD
        /// </summary>
        /// <returns>Retorna la entidad financiera</returns>    
        public EntidadFinanciera CargarEntidadFinancieraPorID(string Cedula)
        {
            EntidadFinanciera resultado = new EntidadFinanciera();

            resultado = contexto.EntidadFinanciera.Where(A => 
                                                            A.CuentaBancaria.Where(R => 
                                                                                       R.DetalleContratacion.Funcionario.IdCedulaFuncionario == Cedula).Count() > 0).FirstOrDefault();

            return resultado;
        }

        #endregion
   }
}
