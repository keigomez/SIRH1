using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CCatMetaEstadoL
    {
        #region Variables

        SIRHEntities contexto;
        CCatMetaEstadoD estadoDescarga;

        #endregion

        #region constructor

        public CCatMetaEstadoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCatMetaEstadoDTO ConvertirEstadoADTO(CatMetaEstado item)
        {
            return new CCatMetaEstadoDTO
            {
                IdEntidad = item.PK_Estado,
                DesEstado = item.DesEstado
            };
        }

        public CBaseDTO ObtenerEstado(int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCatMetaEstadoD datos = new CCatMetaEstadoD(contexto);

                var estado = datos.ObtenerEstado(codigo);

                if (estado != null)
                    respuesta = ConvertirEstadoADTO(estado);
                else
                    throw new Exception("No existe registro");
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
            }
            return respuesta;
        }

        public List<CBaseDTO> DescargarEstado()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCatMetaEstadoD datos = new CCatMetaEstadoD(contexto);

                var permisos = datos.ListarEstados();

                if (permisos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in ((List<CatMetaEstado>)permisos.Contenido))
                    {
                        respuesta.Add(ConvertirEstadoADTO(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)permisos.Contenido).MensajeError);
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

        
       

         