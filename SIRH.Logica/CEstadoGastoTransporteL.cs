using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEstadoGastoTransporteL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CEstadoGastoTransporteL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos
        internal static CEstadoGastoTransporteDTO ConvertirEstadoGastoTransporteDatosaDTO(EstadoGastoTransporte item)
        {
            return new CEstadoGastoTransporteDTO
            {
                NomEstadoDTO = item.NomEstado,
                IdEntidad = item.PK_EstadoGastosTransporte
            };
        }
        internal static EstadoGastoTransporte ConvertirEstadoGastoTransporteDTOaDatos(CEstadoGastoTransporteDTO item)
        {
            return new EstadoGastoTransporte
            {
                NomEstado = item.NomEstadoDTO,
                PK_EstadoGastosTransporte = item.IdEntidad
            };
        }
        #endregion
    }
}