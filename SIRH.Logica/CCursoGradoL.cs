using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCursoGradoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCursoGradoL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Metodos

        internal static CCursoGradoDTO ConvertirDatosCursoGradoADTO(CursoGrado item)
        {
            return new CCursoGradoDTO
            {
                IdEntidad = item.PK_CursoGrado,
                Resolucion = item.NumResolucion,
                FechaEmision = Convert.ToDateTime(item.FecEmision),
                TipoGrado = Convert.ToInt32(item.TipCursoGrado),
                CursoGrado = item.DesCursoGrado,
                Incentivo = Convert.ToInt32(item.PorIncentivo),
                ImagenTitulo = item.ImgTitulo
            };
        }
                 
        #endregion
    }
}
