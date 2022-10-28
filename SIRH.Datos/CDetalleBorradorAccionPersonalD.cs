using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CDetalleBorradorAccionPersonalD
    {

        #region Variables

        /// <summary>
        /// Contexto de la entidad funcionario
        /// </summary>
        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase Borrador Acción de Personal
        /// </summary>
        /// <param name="entidadGlobal"></param>
        public CDetalleBorradorAccionPersonalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Almacena datos del Detalle de Borrador de Acción de Personal en la BD
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Devuelve la llave primaria del registro insertado</returns>
        public CRespuestaDTO GuardarDetalle(DetalleBorradorAccionPersonal registro)
        {
            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.DetalleBorradorAccionPersonal
                                           .Include("BorradorAccionPersonal")
                                           .ToList();


                if (resultado.Where(R => R.PK_Detalle == registro.PK_Detalle).Count() < 1)
                {
                    entidadBase.DetalleBorradorAccionPersonal.Add(registro);
                    entidadBase.SaveChanges();
                    
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro.PK_Detalle
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ya existen los datos del Detalle de Borrador"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
            }
            return respuesta;
        }


        /// <summary>
        /// Actualiza los datos del Detalle de Borrador de Acción de Personal en la BD
        /// </summary>
        /// <param name="registro"></param>
        /// <returns>Número de filas afectadas</returns>
        public CRespuestaDTO ActualizarDetalle(DetalleBorradorAccionPersonal registro)
        {
            CRespuestaDTO respuesta;
            int dato;

            try
            {
                EntityKey llave;
                object objetoOriginal;

                using (entidadBase)
                {
                    /* llave = entidadBase.CreateEntityKey("DetalleBorradorAccionPersonal", registro);
                     if (entidadBase.TryGetObjectByKey(llave, out objetoOriginal))
                     {
                         entidadBase.ApplyPropertyChanges(llave.EntitySetName, registro);
                     }*/
                    entidadBase.Entry(registro).State= EntityState.Modified;

                    dato = entidadBase.SaveChanges();
                }

                if (dato > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = true
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = false
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
            return respuesta;
        }


        public CRespuestaDTO ObtenerDetalle(int codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var registro = entidadBase.DetalleBorradorAccionPersonal
                                            .Include("BorradorAccionPersonal")
                                            .Include("Programa")
                                            .Include("Seccion")
                                            .Where(R => R.BorradorAccionPersonal.PK_Borrador == codigo).FirstOrDefault();

                if (registro != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = registro
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos del Detalle de Borrador"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }

        
        #endregion
    }
}