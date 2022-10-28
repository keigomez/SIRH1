using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CTipoExtraD
  {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CTipoExtraD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Carga los Tipos Extra
        /// </summary>
        /// <returns>Retorna los tipos extra</returns>
        public List<TipoExtra> RetornarTipoExtra()
        {
            return contexto.TipoExtra.ToList();
        }

        /// <summary>
        /// Obtiene un Tipo Extra Específico
        /// </summary>
        /// <param name="TipoExtra"></param>
        /// <returns>Retorna un tipo extra</returns>
        /*public TipoExtra RetornarTipoExtraEspecifico(string TipoExtra)
        {
            return contexto.TipoExtra.Where(Q => Q.RegistroTiempoExtra.Where(A => A.TipoExtra.DesTipExtra == TipoExtra).Count() > 0).FirstOrDefault();
        }*/

        #endregion
    }
}