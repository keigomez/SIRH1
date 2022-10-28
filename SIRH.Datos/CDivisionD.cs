using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
   public class CDivisionD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDivisionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Guarda las Divisiones en la BD
        /// </summary>
        /// <returns>Retorna las Divisiones</returns>

        public int GuardarDivision(Division Division)
        {
            entidadBase.Division.Add(Division);
            return Division.PK_Division;
        }
        /// <summary>
        /// Obtiene la lista de las Divisiones de la BD
        /// </summary>
        /// <returns>Retorna una lista de Divisiones</returns>
        public List<Division> CargarDivisiones()
        {
            List<Division> resultados = new List<Division>();

            resultados = entidadBase.Division.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de las Divisiones de la BD
        /// </summary>
        /// <returns>Retorna las Divisiones</returns>
        public Division CargarDivisionPorID(int idDivision)
        {
            Division resultado = new Division();

            resultado = entidadBase.Division.Where(R => R.PK_Division == idDivision).FirstOrDefault();

            return resultado;
        }

        // DIVISION POR ID DIVISION
      public Division CargarDivisionParam(string NombreDivision)
        {
            Division resultado = new Division();

            resultado = entidadBase.Division.Where(R => R.NomDivision.ToLower().Contains(NombreDivision.ToLower())).FirstOrDefault();

            return resultado;
        }

       /// <summary>
       /// Enlista las Divisiones de la BD
       /// </summary>
       /// <param name="codDivision"></param>
       /// <param name="nomDivision"></param>
      /// <returns>Retorna las Divisiones</returns>
      public List<Division> CargarDivisionesParam(int codDivision, string nomDivision)
      {
          List<Division> resultado = entidadBase.Division.ToList();

          if (codDivision != 0)
          {
              resultado = resultado.Where(Q => Q.PK_Division.Equals(codDivision)).ToList();
          }
          if (nomDivision != null && nomDivision != "")
          {
              resultado = resultado.Where(Q => Q.NomDivision.ToLower().Contains(nomDivision.ToLower())).ToList();
          }

          return resultado;
      }

        #endregion

   }
}

