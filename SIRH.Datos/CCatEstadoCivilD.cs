using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCatEstadoCivilD
    {

        #region Variables

        private SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CCatEstadoCivilD(SIRHEntities entidadGlobal)
        {
            contexto = entidadGlobal;
        }

        #endregion

        #region Metodos 
        //Obtiene la lista de Categorias de Estados Civiles de la BD        
        //Retorna una lista de categorias de Categorias Civiles</returns>
        public List<CatEstadoCivil> CargarCatEstadosCiviles()
        {
            List<CatEstadoCivil> resultados = new List<CatEstadoCivil>();

            resultados = contexto.CatEstadoCivil.ToList();

            return resultados;
        }
        
        //Carga las Categorias de Estados Civiles de la BD        
        //Retorna estados civiles</returns>
        public CatEstadoCivil CargarCatEstadoCivilPorID(int idCatEstadoCivil)
        {
            CatEstadoCivil resultado = new CatEstadoCivil();

            resultado = contexto.CatEstadoCivil.Where(R => R.PK_CatEstadoCivil == idCatEstadoCivil).FirstOrDefault();

            return resultado;
        }

        #endregion
    }
}
