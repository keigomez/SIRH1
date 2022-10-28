using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Logica
{
    public class CEntidadFinancieraL
    {
        #region Variables

        private SIRHEntities contexto;

        #endregion
              
        #region Constructor

        public CEntidadFinancieraL()
        {
            contexto = new SIRHEntities();
        }  
        
        #endregion

        #region Metodos

        internal static CEntidadFinancieraDTO ConvertirEntidadFinancieraADTO(EntidadFinanciera itemEntidadFin)
        {
            return new CEntidadFinancieraDTO
            {
                NomEntidad = itemEntidadFin.NomEntidad,
                CodEntidad = itemEntidadFin.CodEntidad
            };
        }       

        //Se insertó en FuncionarioService el 31/01/2017
        public CBaseDTO BuscarEntidadFinanciera(int codigo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CEntidadFinancieraD intermedio = new CEntidadFinancieraD(contexto);
                var datos = intermedio.BuscarEntidadFinanciera(codigo);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        respuesta = ConvertirEntidadFinancieraADTO(((EntidadFinanciera)datos.Contenido));
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

        //Se insertó en FuncionarioService el 31/01/2017
        public List<CBaseDTO> ListarEntidadesFinancieras()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CEntidadFinancieraD intermedio = new CEntidadFinancieraD(contexto);
                var datos = intermedio.ListarEntidadFinanciera();
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        foreach (var item in (List<EntidadFinanciera>)datos.Contenido)
                        {
                            respuesta.Add(ConvertirEntidadFinancieraADTO(item));
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

