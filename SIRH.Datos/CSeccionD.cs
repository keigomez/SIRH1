using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CSeccionD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CSeccionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Obtiene la lista de las Secciones de la BD
        /// </summary>
        /// <returns>Retorna una lista de secciones</returns>
        public List<Seccion> CargarSecciones()
        {
            List<Seccion> resultados = new List<Seccion>();

            resultados = entidadBase.Seccion.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de de las Secciones de la BD
        /// </summary>
        /// <returns>Retorna las Secciones</returns>
       public Seccion CargarSeccionPorID(int idSeccion)
        {
            Seccion resultado = new Seccion();

            resultado = entidadBase.Seccion.Where(R => R.PK_Seccion == idSeccion).FirstOrDefault();

            return resultado;
        }
        //POR PARAMETRO
        public Seccion CargarSeccionParam(string NombreSeccion)
        {
            Seccion resultado = new Seccion();

            resultado = entidadBase.Seccion.Where(R => R.NomSeccion.ToLower().Contains(NombreSeccion.ToLower())).FirstOrDefault();

            return resultado;
        }

        /// <summary>
        /// Obtiene y enlista las Secciones de la BD
        /// </summary>
        /// <param name="codSeccion"></param>
        /// <param name="nomSeccion"></param>
        /// <returns></returns>
        public List<Seccion> CargarSeccionesParam(int codSeccion, string nomSeccion)
        {
            List<Seccion> resultado = entidadBase.Seccion.ToList();

            if (codSeccion != 0)
            {
                resultado = resultado.Where(Q => Q.PK_Seccion.Equals(codSeccion)).ToList();
            }
            if (nomSeccion != null && nomSeccion != "")
            {
                resultado = resultado.Where(Q => Q.NomSeccion.ToLower().Contains(nomSeccion.ToLower())).ToList();
            }

            return resultado;
        }

        #endregion

   }
}
