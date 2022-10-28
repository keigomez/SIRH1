using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
     public class CEstadoPuestoD
   {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public CRespuestaDTO GuardarEstadoPuesto(string codPuesto, EstadoPuesto estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.EstadoPuesto.Add(estado);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = estado
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

         // <summary>
         /// Obtiene y Enlista los estados de los Puestos
         /// </summary>
         /// <returns>Retorna los estados del Puesto</returns>
        public List<EstadoPuesto> CargarEstadosDePuestos()
        {
            List<EstadoPuesto> resultados = new List<EstadoPuesto>();

            resultados = entidadBase.EstadoPuesto.ToList();

            return resultados;
        }

         /// <summary>
         /// Obtiene la Carga de losEestados de los Puestos por ID
         /// </summary>
         /// <param name="idEstadoPuesto"></param>
        /// <returns>Retorna los estados del Puesto</returns>
       public EstadoPuesto CargarEstadoPuestoPorID(int idEstadoPuesto)
        {
            EstadoPuesto resultado = new EstadoPuesto();

            resultado = entidadBase.EstadoPuesto.Where(R => R.PK_EstadoPuesto == idEstadoPuesto).FirstOrDefault();

            return resultado;
        }

        #endregion
   }
}