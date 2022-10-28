using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDesgloseSalarialL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDesgloseSalarialL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        public static CDesgloseSalarialDTO DesgloseSalarialGeneral(DesgloseSalarial item)
        {
            CDesgloseSalarialDTO respuesta = new CDesgloseSalarialDTO
            {
                IdEntidad = item.PK_DesgloseSalarial,
                MontoSalarioBruto = Convert.ToDecimal(item.DetalleDesgloseSalarial.Sum(Q => Q.MtoPagocomponenteSalarial))
            };

            return respuesta;
        }            

        #endregion
    }
}
