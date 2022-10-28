using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CFactorL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CFactorL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Metodos

        internal static CFactorDTO ConvertirDatosFactorADTO(Factor item)
        {
            return new CFactorDTO
            {
                IdEntidad = item.PK_Factor,
                DescripcionFactor = item.DesFactor
            };
        }

        //SE INSERTÓ EN ICPuestoService y CPuestoService el 27/01/2017
        public CBaseDTO BuscarFactorId(CFactorDTO factor)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CFactorD intermedio = new CFactorD(contexto);
                var datos = intermedio.BuscarFactorId(factor.IdEntidad);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta = ConvertirDatosFactorADTO(((Factor)datos.Contenido));
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
        //SE INSERTÓ EN ICPuestoService y CPuestoService el 26/01/2017
        public CBaseDTO GuardarFactor(CTituloFactorDTO titulo,CFactorDTO factor)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CFactorD intermedio = new CFactorD(contexto);
                //SE LE ASIGNA A Factor que es de datos, la palabra factorB para ser usada mas adelante.
                Factor factorB = new Factor
                {
                    DesFactor = factor.DescripcionFactor
                };
                //a var datos se le asigna un intermedio para guardar el Factor, se guarda en TituloFactor y luego se le asigna el"factorB"
                var datos = intermedio.GuardarFactor(titulo.IdEntidad, factorB);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirDatosFactorADTO((Factor)datos.Contenido);

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
