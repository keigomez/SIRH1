using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos
{
    public class CDireccionGeneralD
   {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CDireccionGeneralD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Guarda las Direcciones Generales en la BD
        /// </summary>
        /// <returns>Retorna las Direcciones Generales</returns>
        
        public int GuardarDireccionGenneral(DireccionGeneral direccionLocal)
        {
            entidadBase.DireccionGeneral.Add(direccionLocal);
            return direccionLocal.PK_DireccionGeneral;
        }
        /// <summary>
        /// Obtiene la lista de las Direcciones Generales de la BD
        /// </summary>
        /// <returns>Retorna una lista de Direcciones Generales</returns>
        public List<DireccionGeneral> CargarDireccionesGenerales()
        {
            List<DireccionGeneral> resultados = new List<DireccionGeneral>();

            resultados = entidadBase.DireccionGeneral.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de las Direcciones Generales de la BD
        /// </summary>
        /// <returns>Retorna las Direcciones Generales</returns>
        public DireccionGeneral CargarDireccionGeneralPorID(int idDireccionGeneral)
        {
            DireccionGeneral resultado = new DireccionGeneral();

            resultado = entidadBase.DireccionGeneral.Where(R => R.PK_DireccionGeneral == idDireccionGeneral).FirstOrDefault();

            return resultado;
        }

        //POR PARAMETROS
        public DireccionGeneral CargarDireccionGeneralParam(string NomDireccion)
        {
            DireccionGeneral resultado = new DireccionGeneral();

            resultado = entidadBase.DireccionGeneral.Where(R => R.NomDireccion.ToLower().Contains(NomDireccion.ToLower())).FirstOrDefault();

            return resultado;
        }

        /// <summary>
        /// Enlista las Direcciones Generales de la BD
        /// </summary>
        /// <param name="codDireccionGeneral"></param>
        /// <param name="nomDireccionGeneral"></param>
        /// <returns>Retorna Direcciones Generales</returns>
        public List<DireccionGeneral> CargarDireccionesGeneralesParam(int codDireccionGeneral, string nomDireccionGeneral)
        {
            List<DireccionGeneral> resultado = entidadBase.DireccionGeneral.ToList();

            if (codDireccionGeneral != 0)
            {
                resultado = resultado.Where(Q => Q.PK_DireccionGeneral.Equals(codDireccionGeneral)).ToList();
            }
            if (nomDireccionGeneral != null && nomDireccionGeneral != "")
            {
                resultado = resultado.Where(Q => Q.NomDireccion.ToLower().Contains(nomDireccionGeneral.ToLower())).ToList();
            }

            return resultado;
        }

        #endregion

   }
}