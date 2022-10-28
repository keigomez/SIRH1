using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;


namespace SIRH.Logica
{
    public class CCaracteristicasPuestoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCaracteristicasPuestoL()
        {
            contexto = new SIRHEntities();
        }            
        
        #endregion

        #region Metodos

        internal static CCaracteristicasPuestoDTO ConvertirDatosCaracteristicasPuestoADTO(CaracteristicasPuesto item)
        {
            return new CCaracteristicasPuestoDTO
            {
                IdEntidad = item.PK_CaracteristicasPuesto,
                NumEscala = Convert.ToInt32(item.IndEscala),
                Caracteristicas = item.DesCaracteristica
            };
        }

        //LO QUE NO SE PONE SON EL TITULO FACTOR NI EL FACTOR.
        //Se insertó en ICPuestoService y CPuestoService..25/01/2017.....
        public List<CBaseDTO> GuardarCaracteristicasPuesto(CPuestoDTO puesto, CCaracteristicasPuestoDTO caracteristicas, CTareasPuestoDTO tareas, CFactorDTO factor, CTituloFactorDTO titulo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                //ESCOGER ID....
                CCaracteristicasPuestoD intermedio = new CCaracteristicasPuestoD(contexto);
                CaracteristicasPuesto caracteristica = new CaracteristicasPuesto
                {
                    IndEscala = Convert.ToInt32(caracteristicas.IdEntidad),
                    DesCaracteristica = caracteristicas.Caracteristicas
                };
                var datos = intermedio.GuardarCaracteristicasPuesto(puesto.CodPuesto, caracteristica);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {   
                    respuesta.Add(caracteristicas);
                }
                else
                {
                    respuesta.Add((CErrorDTO)datos.Contenido);
                }
                return respuesta;
            }
            catch (Exception error)
            {
                //es una nueva lista con un solo elemento en este caso es el Error
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }
        }
        
        #endregion
    }
}
