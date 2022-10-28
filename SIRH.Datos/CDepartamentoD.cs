using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CDepartamentoD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDepartamentoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Guarda los departamentos en la BD
        /// </summary>
        /// <returns>Retorna los departamentos</returns>

        public int GuardarDepartamento(Departamento Departamento)
        {
            entidadBase.Departamento.Add(Departamento);
            return Departamento.PK_Departamento;
        }

        /// <summary>
        /// Obtiene la lista de Departamentos de la BD
        /// </summary>
        /// <returns>Retorna una lista de departamentos</returns>
        public List<Departamento> CargarDepartamentos()
        {
            List<Departamento> resultados = new List<Departamento>();

            resultados = entidadBase.Departamento.ToList();

            return resultados;
        }

        /// <summary>
        /// Obtiene la carga los Departamentos de la BD
        /// </summary>
        /// <returns>Retorna los Departamentos</returns>    
        public Departamento CargarDepartamentoPorID(int idDepartamento)
        {
            Departamento resultado = new Departamento();

            resultado = entidadBase.Departamento.Where(R => R.PK_Departamento == idDepartamento).FirstOrDefault();

            return resultado;
        }
   
        public Departamento CargarDepartamenotParam(string NombreDepartamento)
        {
            Departamento resultado = new Departamento();

            resultado = entidadBase.Departamento.Where(R => R.NomDepartamento.ToLower().Contains(NombreDepartamento.ToLower())).FirstOrDefault();

            return resultado;
        }

        /// <summary>
        /// Obtiene una lista de los departamentos
        /// </summary>
        /// <returns>Retorna los Departamentos</returns>
         public List<Departamento> CargarDepartamentoesParam(int codDepartamento, string nomDepartamento)
         {
             List<Departamento> resultado = entidadBase.Departamento.ToList();

             if (codDepartamento != 0)
             {
                 resultado = resultado.Where(Q => Q.PK_Departamento.Equals(codDepartamento)).ToList();
             }
             if (nomDepartamento != null && nomDepartamento != "")
             {
                 resultado = resultado.Where(Q => Q.NomDepartamento.ToLower().Contains(nomDepartamento.ToLower())).ToList();
             }

             return resultado;
         }

      #endregion

    }
}

