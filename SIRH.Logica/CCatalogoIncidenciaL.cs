using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCatalogoIncidenciaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCatalogoIncidenciaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCatalogoIncidenciaDTO ConvertirDatosCatalogoIncidenciaADto(CatalogoIncidencia item)
        {
            return new CCatalogoIncidenciaDTO
            {
                IdEntidad = item.PK_CatalogoIncidencia,
                NomCatalogo = item.Nombre,
                Prioridad = Convert.ToInt32(item.Prioridad),
                Perfil = CPerfilL.PerfilADto(item.Perfil),
                DetallePrioridad = DefinirPrioridadIncidencia(Convert.ToInt32(item.Prioridad))
            };
        }

        internal static string DefinirPrioridadIncidencia(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Alta";
                    break;
                case 2:
                    respuesta = "Media";
                    break;
                case 3:
                    respuesta = "Baja";
                    break;
                default:
                    respuesta = "Prioridad no definida";
                    break;
            }
            return respuesta;
        }

        #endregion
    }
}