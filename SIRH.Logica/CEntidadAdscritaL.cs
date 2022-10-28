using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEntidadAdscritaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEntidadAdscritaL()
        {
            contexto = new SIRHEntities();
        }  
        
        #endregion

        #region Metodos

        internal static CEntidadAdscritaDTO ConvertirEntidadAdscritaADTO(EntidadAdscrita itemEntidadAds)
        {
            return new CEntidadAdscritaDTO
            {
                IdEntidad = itemEntidadAds.PK_EntidadAdscrita,
                EntidadAdscrita = itemEntidadAds.NomEntidad,
                TipoEntidad = Convert.ToInt32(itemEntidadAds.TipEntidad)
            };
        }

        //Se insertó en ICPuestoService y CPuestoService el 30/01/2017
        public CBaseDTO BuscarEntidadAdscrita(int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CEntidadAdscritaD intermedio = new CEntidadAdscritaD(contexto);
                var datos = intermedio.BuscarEntidadAdscrita(codigo);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        respuesta = ConvertirEntidadAdscritaADTO(((EntidadAdscrita)datos.Contenido));
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ha ocurrido en error inesperado");
                }
            }
            catch (Exception Error)
            {
                respuesta = new CErrorDTO { MensajeError = Error.Message };
                return respuesta;
            }
        }

        //Se insertó en ICPuestoService y CPuestoService el 30/01/2017
        public List<CBaseDTO> ListarEntidadesAdscritas()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try 
            {                
                CEntidadAdscritaD intermedio = new CEntidadAdscritaD(contexto);                
                var datos = intermedio.ListarEntidadesAdscritas();
                if(datos != null)
                {
                    if(datos.Codigo != -1)
                    {
                        foreach (var item in (List<EntidadAdscrita>)datos.Contenido)
                        {
                            respuesta.Add(ConvertirEntidadAdscritaADTO(item));
                        }

                        return respuesta;
                    }
                    else
                    {
                        throw new Exception (((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
	            }
            }
	        catch (Exception error)
            {
                respuesta.Add (new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        } 
        
        #endregion
    }
}
