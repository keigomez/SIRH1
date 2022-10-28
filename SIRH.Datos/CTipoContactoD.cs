using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
  public  class CTipoContactoD
  {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoContactoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Obtiene la lista de los Tipos de Contacto de la BD
        /// </summary>
        /// <returns>Retorna una lista los tipos de contacto</returns>
        public List<TipoContacto> CargarTiposDeContacto()
        {
            List<TipoContacto> resultados = new List<TipoContacto>();

            resultados = entidadBase.TipoContacto.ToList();

            return resultados;
        }

        #endregion
        /// <summary>
        /// Obtiene la carga de los Tipos de Contacto de la BD
        /// </summary>
        /// <returns>Retorna las Tipos de Contacto</returns>       
        public TipoContacto CargarTipoContactoPorID(int idTipoContacto)
        {
            TipoContacto resultado = new TipoContacto();

            resultado = entidadBase.TipoContacto.Where(R => R.PK_TipoContacto == idTipoContacto).FirstOrDefault();

            return resultado;
        }

        
  }
}
