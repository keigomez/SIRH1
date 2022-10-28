using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEntidadSegurosL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEntidadSegurosL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Métodos

        public List<CBaseDTO> ListarEntidadesSeguros()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CEntidadSegurosD intermedio = new CEntidadSegurosD(contexto);

            var entidadSeguros = intermedio.ListarEntidadSeguros();

            if (entidadSeguros.Codigo != -1)
            {
                foreach (var item in (List<EntidadSeguros>)entidadSeguros.Contenido)
                {
                    respuesta.Add(new CEntidadSegurosDTO
                    {
                        IdEntidad = item.PK_EntidadSeguros,
                        NombreEntidad = item.NomEntidadSeguros
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)entidadSeguros.Contenido);
            }

            return respuesta;
        }

        internal static CEntidadSegurosDTO ConvertirEntidadSegurosADto(EntidadSeguros item)
        {
            return new CEntidadSegurosDTO 
            {
                IdEntidad = item.PK_EntidadSeguros,
                NombreEntidad = item.NomEntidadSeguros
            };
        }

        #endregion
    }
}
