using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CEstadoFuncionarioD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEstadoFuncionarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Obtiene y Enlista los Estados de Funcionario de la BD
        /// </summary>
        /// <returns>Retorna los estados de los funcionarios</returns>
        public List<EstadoFuncionario> CargarEstadosDeFuncionarios()
        {
            List<EstadoFuncionario> resultados = new List<EstadoFuncionario>();

            resultados = entidadBase.EstadoFuncionario.ToList();

            return resultados;
        }       

            /// <summary>
            /// Obtiene la lista de las Estados de Funcionarios de la BD
            /// </summary>
            /// <returns>Retorna una lista de Especialidades</returns>
        public List<EstadoFuncionario> CargarEstadoFuncionario()
        {          
            List<EstadoFuncionario> resultados = new List<EstadoFuncionario>();

            resultados = entidadBase.EstadoFuncionario.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de los Estados de los Funcionarios de la BD
        /// </summary>
        /// <returns>Retorna los Estados de los funcionarios</returns>
        public EstadoFuncionario CargarEstadoFuncionarioPorID(int idEstadoFuncionario)
        {
            EstadoFuncionario resultado = new EstadoFuncionario();

            resultado = entidadBase.EstadoFuncionario.Where(R => R.PK_EstadoFuncionario == idEstadoFuncionario).FirstOrDefault();

            return resultado;
        }
        #endregion
    }
}