using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CProvinciaD
    {

       #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion


        #region Constructor

        public CProvinciaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Obtiene la lista de las provincias de la BD
        /// </summary>
        /// <returns>Retorna una lista de las provincias</returns>
        public List<Provincia> CargarProvincias(CProvinciaD provincia)
            
        {
            List<Provincia> resultados = new List<Provincia>();

            resultados = entidadBase.Provincia.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de las Provincias de la BD
        /// </summary>
        /// <returns>Retorna las Provincias</returns>
        public Provincia CargarProvinciaId(int idProvincia) // ESTE IGUAL PARA CANTON Y DISTRITO
        {
            Provincia resultado = new Provincia();

            resultado = entidadBase.Provincia.Where(R => R.PK_Provincia == idProvincia).FirstOrDefault();

            return resultado;
        }
        #endregion
        
    }
}