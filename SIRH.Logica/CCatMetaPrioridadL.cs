using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CCatMetaPrioridadL
    {
        #region Variables

        SIRHEntities contexto;
        CCatMetaPrioridadD prioridadDescarga;

        #endregion

        #region constructor

        public CCatMetaPrioridadL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCatMetaPrioridadDTO ConvertirPrioridadADTO(CatMetaPrioridad item)
        {
            return new CCatMetaPrioridadDTO
            {
                IdEntidad = item.PK_Prioridad,
                DesPrioridad = item.DesPrioridad
            };
        }

        public CBaseDTO ObtenerPrioridad(int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCatMetaPrioridadD datos = new CCatMetaPrioridadD(contexto);

                var prioridad = datos.ObtenerPrioridad(codigo);

                if (prioridad != null)
                    respuesta = ConvertirPrioridadADTO(prioridad);
                else
                    throw new Exception("No existe registro");
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
            }
            return respuesta;
        }

        public List<CBaseDTO> ListarPrioridades()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCatMetaPrioridadD datos = new CCatMetaPrioridadD(contexto);

                var prioridades = datos.ListarPrioridades();

                if (prioridades.Contenido.GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in ((List<CatMetaPrioridad>)prioridades.Contenido))
                    {
                        respuesta.Add(ConvertirPrioridadADTO(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)prioridades.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        #endregion
    }
}

        
       

         