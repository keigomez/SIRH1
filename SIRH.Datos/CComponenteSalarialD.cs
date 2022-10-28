using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CComponenteSalarialD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CComponenteSalarialD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public List<ComponenteSalarial> CargarComponentesSalariales(CComponenteSalarialD ComponenteSalarial)
        {
            List<ComponenteSalarial> resultados = new List<ComponenteSalarial>();
            resultados = entidadBase.ComponenteSalarial.ToList();
            return resultados;
        }

        public ComponenteSalarial CargarComponenteSalarialId(int idComponenteSalarial)
        {
            ComponenteSalarial resultado = new ComponenteSalarial();
            resultado = entidadBase.ComponenteSalarial.Where(R => R.PK_ComponenteSalarial == idComponenteSalarial).FirstOrDefault();
            return resultado;
        }

        

        #endregion

    }
}
