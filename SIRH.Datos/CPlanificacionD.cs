using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
   public class CPlanificacionD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        #region Constructor

        public CPlanificacionD(SIRHEntities entidadGlobal)
{
    entidadBase = entidadGlobal;
}

        #endregion
        #region Métodos
        /// <summary>
        /// Guarda las clases en la BD
        /// </summary>
        /// <returns>Retorna las Clases</returns>

        /// Obtiene la lista de Nombramientos de un Funcionario
        public List<Nombramiento> ListarNombramientosFuncionario(string cedula)
        {
            List<Nombramiento> resultados = new List<Nombramiento>();

            resultados = entidadBase.Nombramiento
                                    .Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula && Q.FK_EstadoNombramiento != 15) //Inactivo
                                    .OrderBy(Q => Q.FecRige)
                                    .ToList();

            return resultados;
        }

        public List<ListaNombramientosActivos> ListarNombramientosActivos()
        {
            List<ListaNombramientosActivos> resultado = new List<ListaNombramientosActivos>();

            //entidadBase.USP_LISTAR_NOMBRAMIENTOS_ACTIVOS();

            resultado = entidadBase.ListaNombramientosActivos                                    
                                    .OrderBy(Q => Q.Funcionario.NomPrimerApellido)
                                    .ThenBy(Q => Q.Funcionario.NomSegundoApellido)
                                    .ThenBy(Q => Q.Funcionario.NomFuncionario)
                                    .ToList();

            return resultado;
        }


        public List<EstadoFuncionario> CargarEstadosFuncionario()
        {
            List<EstadoFuncionario> resultados = new List<EstadoFuncionario>();

            resultados = entidadBase.EstadoFuncionario.ToList();

            return resultados;
        }

        #endregion
    }
}