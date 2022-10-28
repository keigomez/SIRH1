using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
   public class CTipoUbicacionD
   {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoUbicacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Obtiene la lista de los Tipo de Ubicacion de la BD
        /// </summary>
        /// <returns>Retorna una lista los tipos de ubicación</returns>
        public List<TipoUbicacion> CargarTiposDeUbicacion()
        {
            List<TipoUbicacion> resultados = new List<TipoUbicacion>();

            resultados = entidadBase.TipoUbicacion.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de los tipos de ubicacion de la BD
        /// </summary>
        /// <returns>Retorna los tipos de ubicacion</returns>                           
        public TipoUbicacion CargarTipoUbicacionPorID(int idTipoUbicacion)
        {
            TipoUbicacion resultado = new TipoUbicacion();

            resultado = entidadBase.TipoUbicacion.Where(R => R.PK_TipoUbicacion == idTipoUbicacion).FirstOrDefault();

            return resultado;
        }
        
        #endregion
   }
}

