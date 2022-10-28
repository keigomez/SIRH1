using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
   public class CEstadoNombramientoD
   {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoNombramientoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion

        #region Metodos 

        public List<EstadoNombramiento> CargarEstadosdeNombramientos()
        {
            List<EstadoNombramiento> resultados = new List<EstadoNombramiento>();

            resultados = entidadBase.EstadoNombramiento.ToList();

            return resultados;
        }       
        /// <summary>
        /// Obtiene la lista de las Estados de Nombramientos de la BD
        /// </summary>
        /// <returns>Retorna una lista de Estados de Nombramientos</returns>
        public List<EstadoFuncionario> CargarEstadoFuncionario()
        {
            List<EstadoFuncionario> resultados = new List<EstadoFuncionario>();

            resultados = entidadBase.EstadoFuncionario.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de los Estados de los Nombramientos de la BD
        /// </summary>
        /// <returns>Retorna los Estados de los nombramientos</returns>
        public EstadoNombramiento CargarEstadoNombramientoPorID(int idEstadoNombramiento)
        {
            EstadoNombramiento resultado = new EstadoNombramiento();

            resultado = entidadBase.EstadoNombramiento.Where(R => R.PK_EstadoNombramiento == idEstadoNombramiento).FirstOrDefault();

            return resultado;
        }

        public CRespuestaDTO GuardarEstadoNombramiento(string cedula, EstadoNombramiento estado)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.EstadoNombramiento.Add(estado);
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

        #endregion
   }
}