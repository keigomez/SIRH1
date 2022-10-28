using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
   public class CRelPresupuestoExtraD
  {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CRelPresupuestoExtraD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos


        public List<RelPresupuestoExtra> RetornarRelPresupuestoExtra()
        {
            return contexto.RelPresupuestoExtra.ToList();
        }

        public RelPresupuestoExtra RetornarRelPresupuestoExtra(string Cedula)
        {
            return contexto.RelPresupuestoExtra.Where(A => A.Presupuesto.UbicacionAdministrativa.Where 
                                                                                                    (R => R.Puesto.Where
                                                                                                                        (M => M.Nombramiento.Where
                                                                                                                                                (F => F.Funcionario.IdCedulaFuncionario == Cedula).Count() > 0).Count() > 0).Count() > 0).FirstOrDefault();
        }
               /// <summary>
        /// Guarda las Relaciones de presupuesto de extra en la BD
        /// </summary>
        /// <returns>Retorna las Relaciones de presupuesto de extra</returns>

        public int GuardarRelPresupuestoExtra(RelPresupuestoExtra RelPresupuestoExtra)
        {
            contexto.RelPresupuestoExtra.Add(RelPresupuestoExtra);
            return RelPresupuestoExtra.PK_PresupuestoExtra;
        }
              
        /// <summary>
        /// Obtiene la carga las relaciones de presupuesto de Extras de la BD
        /// </summary>
        /// <returns>Retorna los detalles de la relacion del Presupuesto de las extras</returns>    
        public RelPresupuestoExtra CargarRelPresupuestoExtraPorID(string IdPresupuesto)
        {
            RelPresupuestoExtra resultado = new RelPresupuestoExtra();

            resultado = contexto.RelPresupuestoExtra.Where(R => R.Presupuesto.IdPresupuesto == IdPresupuesto).FirstOrDefault();

            return resultado;
        }

        #endregion

   }
}
