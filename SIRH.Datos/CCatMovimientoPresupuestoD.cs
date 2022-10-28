using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatMovimientoPresupuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatMovimientoPresupuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Métodos

        public CatMovimientoPresupuesto CargarMovimientoPresupuestoId(int idMovPresupuesto)
        {
            CatMovimientoPresupuesto resultado = new CatMovimientoPresupuesto();
            resultado = entidadBase.CatMovimientoPresupuesto.Where(R => R.PK_CatMovimientoPresupuesto == idMovPresupuesto).FirstOrDefault();
            return resultado;
        }



        /// <summary>
        /// Busca un tipo de movimiento de presupuesto especifico según el párametro recibido
        /// </summary>
        /// <returns>Retorna el tipo de movimiento</returns>
        public CRespuestaDTO BuscarCatMovimientoPresupuesto(int codCatMovimientoPresupuesto)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatMovimientoPresupuesto.Include("TipoMovimientoPresupuesto").Where(C => C.PK_CatMovimientoPresupuesto == codCatMovimientoPresupuesto).FirstOrDefault();
                    
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };

                }
                else
                {
                    throw new Exception("No se encontró el tipo de movimiento");
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
            return respuesta;
        }

        /// <summary>
        /// Retorna todas los moviemientos de presupuesto registrados en la BD
        /// </summary>
        /// <returns>Retorna los movimientos de presupuesto en la BD</returns>
        public CRespuestaDTO ListarCatalogoMovimientoPresupuesto()
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatMovimientoPresupuesto.ToList();

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
                    throw new Exception("No se encontró ningún movimiento de presupuesto registrado");
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
        /// Busca los movimientos de presupuesto según el tipo 1. Incial
        ///                                                    2. Planilla
        ///                                                    3. Decreto
        /// </summary>
        /// <returns>Retorna los movimientos de presupuesto</returns>
        public CRespuestaDTO ListarCatalogoMovimientosPresupuestoPorTipo(int tipo)
        {

            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var datosEntidad = entidadBase.CatMovimientoPresupuesto.Where(MP => MP.PK_CatMovimientoPresupuesto == tipo).ToList();
                    
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
                    throw new Exception("No se encontró ningún movimiento de presupuesto correspondiente al tipo indicado");
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
