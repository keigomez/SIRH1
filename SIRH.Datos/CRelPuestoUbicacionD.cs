using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CRelPuestoUbicacionD
     {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CRelPuestoUbicacionD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Guarda las Relaciones de Puestos de Ubicacion en la BD
        /// </summary>
        /// <returns>Retorna las Relaciones de puestos de ubicacion</returns>

        public int GuardarRelPuestoUbicacion(RelPuestoUbicacion RelPuestoUbicacion)
        {
            entidadBase.RelPuestoUbicacion.Add(RelPuestoUbicacion);
            return RelPuestoUbicacion.PK_RelPuestoUbicacion;
        }
        /// <summary>
        /// Obtiene la lista de las relaciones de puesto de ubicacion de la BD
        /// </summary>
        /// <returns>Retorna una lista de relaciones de puesto de ubicacion</returns>
        public List<RelPuestoUbicacion> CargarRelPuestoUbicacion()
        {
            List<RelPuestoUbicacion> resultados = new List<RelPuestoUbicacion>();

            resultados = entidadBase.RelPuestoUbicacion.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga las relaciones de puesto de ubicacion de la BD
        /// </summary>
        /// <returns>Retorna los detalles de la relacion de puesto de ubicacion</returns>    
        public RelPuestoUbicacion CargarRelPuestoUbicacionPorID(int idRelPuestoUbicacion)
        {
            RelPuestoUbicacion resultado = new RelPuestoUbicacion();

            resultado = entidadBase.RelPuestoUbicacion.Where(R => R.PK_RelPuestoUbicacion == idRelPuestoUbicacion).FirstOrDefault();

            return resultado;
        }

        #endregion

   }
}

