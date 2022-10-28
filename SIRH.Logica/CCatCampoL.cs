using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CCatCampoL
    {
        #region Variables

        SIRHEntities contexto;
        CCatMetaPrioridadD prioridadDescarga;

        #endregion

        #region constructor

        public CCatCampoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCatCampoDTO ConvertirCampoADTO(CatCampo item)
        {
            return new CCatCampoDTO
            {
                IdEntidad = item.PK_Campo,
                DesCampo = item.NomCampo,
                DesTabla = item.DesTabla,
                DesColumna = item.DesColumna,
                DesPantalla = item.DesTabla
            };
        }

        public List<CBaseDTO> ListarCampos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCatCampoD datos = new CCatCampoD(contexto);

                var campos = datos.ListarCampos();

                if (campos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    foreach (var item in ((List<CatCampo>)campos.Contenido))
                    {
                        respuesta.Add(ConvertirCampoADTO(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)campos.Contenido).MensajeError);
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

        
       

         