using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica{
    public class CEstadoViaticoCorridoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CEstadoViaticoCorridoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        internal static CEstadoViaticoCorridoDTO ConvertirEstadoViaticoCorridoDTOaDatos(EstadoViaticoCorrido item)
        {
            return new CEstadoViaticoCorridoDTO
            {
                NomEstadoDTO = item.NomEstado,
                IdEntidad = item.PK_EstadoViaticoCorrido
            };
        }
        internal static EstadoViaticoCorrido ConvertirEstadoViaticoCorridoDatosaDTO(CEstadoViaticoCorridoDTO item)
        {
            return new EstadoViaticoCorrido
            {
                NomEstado = item.NomEstadoDTO,
                PK_EstadoViaticoCorrido = item.IdEntidad
            };
        }
        #endregion
    }
}