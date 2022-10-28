using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CObjetoGastoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CObjetoGastoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos

        public int GuardarObjetoGasto(ObjetoGasto ObjetoGasto)
        {
            entidadBase.ObjetoGasto.Add(ObjetoGasto);
            return ObjetoGasto.PK_ObjetoGasto;

        }

        public List<ObjetoGasto> CargarObjetosGasto()
        {
            List<ObjetoGasto> resultados = new List<ObjetoGasto>();
            resultados = entidadBase.ObjetoGasto.Include("SubPartida").ToList();
            return resultados;
        }

        public List<ObjetoGasto> CargarObjetosGastoPorSubPartida(int SubPartida)
        {
            List<ObjetoGasto> resultados = new List<ObjetoGasto>();
            resultados = entidadBase.ObjetoGasto.Include("SubPartida").Where(Q => Q.SubPartida.PK_SubPartida == SubPartida).ToList();
            return resultados;
        }

        public ObjetoGasto CargarObjetoGastoIDSubPartida(int idSubPartida)
        {
            ObjetoGasto resultado = new ObjetoGasto();
            resultado = entidadBase.ObjetoGasto.Where(R => R.SubPartida.PK_SubPartida == idSubPartida).FirstOrDefault();
            return resultado;
        }

        public ObjetoGasto CargarObjetoGastoId(int idObjetoGasto)
        {
            ObjetoGasto resultado = new ObjetoGasto();
            resultado = entidadBase.ObjetoGasto.Where(R => R.PK_ObjetoGasto == idObjetoGasto).FirstOrDefault();
            return resultado;
        }


        #endregion
    }
}
