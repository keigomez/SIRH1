using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEntidadGubernamentalL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region Constructor

         public CEntidadGubernamentalL()
        {
            contexto = new SIRHEntities();
        }    
        
        #endregion

        #region Metodos

        internal static CEntidadGubernamentalDTO ConvertirDatosEntidadGubernamentalADTO(EntidadGubernamental item)
        {
            return new CEntidadGubernamentalDTO
            {
                IdEntidad = item.PK_EntidadGubernamental,
                EntidadGubernamental = item.NomEntidad,
                TipoEntidad = Convert.ToInt32(item.TipEntidad)               
            };
        }

        //Se insertó en ICPuestoService y CPuestoService el 30/01/2017
        public CBaseDTO BuscarEntidadGubernamental(int codigo)
        {
            CBaseDTO respuesta;
            try
            {
                CEntidadGubernamentalD intermedio = new CEntidadGubernamentalD(contexto);
                var datos = intermedio.BuscarEntidadGubernamental(codigo);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        respuesta = ConvertirDatosEntidadGubernamentalADTO((EntidadGubernamental)datos.Contenido);
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
        public List<CBaseDTO> ListarEntidadesGubernamentales()
        {            
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CEntidadGubernamentalD intermedio = new CEntidadGubernamentalD(contexto);
                var datos = intermedio.ListarEntidadesGubernamentales();
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        foreach (var item in (List<EntidadGubernamental>)datos.Contenido)
                        {
                            respuesta.Add(ConvertirDatosEntidadGubernamentalADTO(item));
                        }

                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
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
