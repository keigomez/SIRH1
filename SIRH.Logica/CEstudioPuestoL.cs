using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEstudioPuestoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CEstudioPuestoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CEstudioPuestoDTO ConvertirDatosEstudioPuestoADTO(EstudioPuesto item)
        {
            return new CEstudioPuestoDTO
            {
                IdEntidad = item.PK_EstudioPuesto,
                NumeroOficio = item.NumOficio,
                NumeroResolucion = item.NumResolucion,
                FechaResolucion = Convert.ToDateTime(item.FecResolucion),
                ObsDeEstudioPuesto = item.ObsEstudioPuesto,
            };
        }
      
        //Se insertó en ICFuncionarioService y CFuncionarioService el 25/01/2017....
        public CBaseDTO GuardarEstudioPuesto(CPuestoDTO puesto, CEstudioPuestoDTO estudio)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CEstudioPuestoD intermedio = new CEstudioPuestoD(contexto);
                EstudioPuesto estudioP = new EstudioPuesto
                {
                    NumOficio = estudio.NumeroOficio,
                    NumResolucion = estudio.NumeroResolucion,
                    FecResolucion = Convert.ToDateTime(estudio.FechaResolucion),
                    ObsEstudioPuesto = estudio.ObsDeEstudioPuesto
                };                
                var datos = intermedio.GuardarEstudioPuesto(puesto.CodPuesto, estudioP );
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta = ConvertirDatosEstudioPuestoADTO((EstudioPuesto)datos.Contenido);
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

        //BUSCAR ESTUDIOS DE PUESTO POR PUESTO EN UNA LISTA...
        //INSERTE ESTE METODO EN ICPuestoService y CPuestoService el 25/01/2017....
        public List<CBaseDTO> BuscarEstudiosPorPuesto(CEstudioPuestoDTO estudio)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CEstudioPuestoD intermedio = new CEstudioPuestoD(contexto);
                var datos = intermedio.BuscarEstudioPuestoPorPuesto(estudio.Puesto.CodPuesto);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //se hace foreach, xq en D, hay un TOLIST (En este mismo método)
                    foreach (var item in ((List<EstudioPuesto>)datos.Contenido))
                    {
                        respuesta.Add(ConvertirDatosEstudioPuestoADTO(item));
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
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }
        }
        //BUSCAR ESTUDIO DE PUESTO POR PUESTO.....
        public CBaseDTO BuscarEstudioPuestoPorPuesto(CEstudioPuestoDTO estudio)
        {
            CBaseDTO respuesta = new CBaseDTO();
             try
            {
                CEstudioPuestoD intermedio = new CEstudioPuestoD(contexto);
                 var datos = intermedio.BuscarEstudioPuestoPorPuesto(estudio.Puesto.CodPuesto);
                 if (datos.Contenido.GetType() != typeof(CErrorDTO))
                 {
                     respuesta = ConvertirDatosEstudioPuestoADTO(((EstudioPuesto)datos.Contenido));
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

        #endregion
    }
}
