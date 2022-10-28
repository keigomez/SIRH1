using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCaracteristicasPuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CCaracteristicasPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        
        #endregion

        #region Metodos

        public CRespuestaDTO GuardarCaracteristicasPuesto(string CodPuesto, CaracteristicasPuesto caracteristicas)        
        {
            CRespuestaDTO respuesta; 
            try
            {
                entidadBase.CaracteristicasPuesto.Add(caracteristicas);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = caracteristicas
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        } 

        public CRespuestaDTO BuscarCaracteristicasPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var caracteristicasPuesto = entidadBase.Puesto.Where(P => P.CodPuesto == codPuesto).ToList();

                if (caracteristicasPuesto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = caracteristicasPuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("no se encontraron características para el puesto solicitado");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta; 
            }             
        }        
        #endregion

    }
}


    

