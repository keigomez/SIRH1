using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CTareasPuestoL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region Constructor

        public CTareasPuestoL()
        {
            contexto = new SIRHEntities(); 
        }
        
        #endregion

        #region Metodos

        internal static CTareasPuestoDTO ConvertirDatosTareasPuestoADTO(TareasPuesto item)
        {
            return new CTareasPuestoDTO
            {
                IdEntidad = item.PK_TareasPuesto,
                NumFrecuencia = Convert.ToInt32(item.IndFrecuencia),
                DescripcionComoHace = item.DesComoLoHace,
                DescripcionQueHace = item.DesQueHace,
            }; 
        }

        //BUSCAR TAREAS POR LISTA, hacer FOREACH
        //insertado en CIPuestoService y CPuestoService el 26/01/2017
        public List<CBaseDTO> BuscarTareasCodPuesto(CTareasPuestoDTO tareas)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CTareasPuestoD intermedio = new CTareasPuestoD(contexto);
                var datos = intermedio.BuscarTareasCodPuesto(tareas.Puesto.CodPuesto);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //se hace foreach, xq en D, hay un TOLIST (En este mismo método)
                    foreach (var item in ((List<TareasPuesto>)datos.Contenido))
                    {
                       // se pone ADD, CUANDO ES UNA LISTA.
                        respuesta.Add(ConvertirDatosTareasPuestoADTO(item));
                     
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } } ;
                return respuesta;
            }
        }

        //BUSCAR TAREAS POR ID....
        //insertado en CIPuestoService y CPuestoService el 26/01/2017
        public CBaseDTO BuscarTareasId(CTareasPuestoDTO tareasPuesto)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CTareasPuestoD intermedio = new CTareasPuestoD(contexto);
                var datos = intermedio.BuscarTareasId(tareasPuesto.IdEntidad);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta = ConvertirDatosTareasPuestoADTO(((TareasPuesto)datos.Contenido));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;                
            }
        }

        //insertado en CIPuestoService y CPuestoService el 26/01/2017
        public CBaseDTO GuardarTareasPuesto(CTareasPuestoDTO tareas)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CTareasPuestoD intermedio = new CTareasPuestoD(contexto);                
                TareasPuesto tareasPuesto = new TareasPuesto
                {
                    DesQueHace = tareas.DescripcionQueHace,
                    DesComoLoHace = tareas.DescripcionComoHace
                };                
                var datos = intermedio.GuardarTareas(tareas.Puesto.CodPuesto,tareasPuesto);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {                    
                    respuesta = ConvertirDatosTareasPuestoADTO((TareasPuesto)datos.Contenido);

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                //en respuesta del catch, se digita el error 
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }
      
        #endregion
    }
}
