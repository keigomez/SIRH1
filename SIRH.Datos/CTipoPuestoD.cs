using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CTipoPuestoD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion
       
        #region Constructor

        public CTipoPuestoD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }
        
        #endregion

        #region Métodos

        public CRespuestaDTO GuardarTipoPuesto(string codPuesto, TipoPuesto tipoPuesto)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.TipoPuesto.Add(tipoPuesto);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = tipoPuesto
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            } 
        }

        /// <summary>
        /// Obtiene los tipos de Puesto de la BD
        /// </summary>
        /// <returns>Retorna Tipos de Puesto</returns>
        public List<TipoPuesto> RetornarTiposPuesto()
        {
            return contexto.TipoPuesto.ToList();
        }

        /// <summary>
        /// Obtiene un tipo de Puesto correspondiente a un código
        /// </summary>
        /// <param name="codPuesto"></param>
        /// <returns>Retorna un Tipo de Puesto</returns>
        public TipoPuesto RetornarTipoPuestoEspecifico(string codPuesto)
        {
            return contexto.TipoPuesto.Where(Q => Q.DetallePuesto.Where(R => R.Puesto.CodPuesto == codPuesto).Count() > 0).FirstOrDefault();
        }

        #endregion
    }
}
