using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CDistritoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDistritoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion


        #region Metodos
        /// <summary>
        /// Guarda los Distritos en la BD
        /// </summary>
        /// <returns>Retorna las Distritos</returns>

        public int GuardarDistrito(Distrito Distrito)
        {
            entidadBase.Distrito.Add(Distrito);
            return Distrito.PK_Distrito;
        }
        /// <summary>
        /// Obtiene la lista de los Distritos de la BD
        /// </summary>
        /// <returns>Retorna una lista de Distritos</returns>
        public List<Distrito> CargarDistritos()
        {
            List<Distrito> resultados = new List<Distrito>();

            resultados = entidadBase.Distrito.Include("Canton").ToList();

            return resultados;
        }

        public List<Distrito> CargarDistritosPorCanton(int canton)
        {
            List<Distrito> resultados = new List<Distrito>();

            resultados = entidadBase.Distrito.Include("Canton").Where(Q => Q.Canton.PK_Canton == canton).ToList();

            return resultados;
        }

        /// <summary>
        /// Obtiene la carga de los Distritos de la BD
        /// </summary>
        /// <returns>Retorna las Distritos</returns>
        public Distrito CargarDistritoIdCanton(int idCanton)
        {
            Distrito resultado = new Distrito();

            resultado = entidadBase.Distrito.Where(R => R.Canton.PK_Canton == idCanton).FirstOrDefault();

            return resultado;
        }

        // DISTRITO POR ID DISTRITO
        public Distrito CargarDistritoId(int idDistrito) // ESTE IGUAL PARA CANTON Y DISTRITO
        {
            Distrito resultado = new Distrito();

            resultado = entidadBase.Distrito.Where(R => R.PK_Distrito == idDistrito).FirstOrDefault();

            return resultado;
        }

        #endregion

    }
}