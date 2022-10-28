using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CContenidoPresupuestarioL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion
        
        #region Constructor	 
	
        public CContenidoPresupuestarioL()
        {
            contexto = new SIRHEntities();
        }            
           
        #endregion

        #region Metodos

        internal static CContenidoPresupuestarioDTO ConvertirDatosContenidoADTO(ContenidoPresupuestario item)
        {
            return new CContenidoPresupuestarioDTO
            {
                IdEntidad = item.PK_ContenidoPresupuestario,
                //FechaPago = Convert.ToDateTime(item.FecPago),
                FechaRige = Convert.ToDateTime(item.FecRige),                
                NumeroResolucion = item.NumResolucion,            
            };
        }

        //POR CODIGO DE PUESTO
        //Insertado en ICPuestoService y CPuestoService el 26/01/2017
        public CBaseDTO BuscarContenidoPresupuestario(CContenidoPresupuestarioDTO contenido)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {                
                CContenidoPresupuestarioD intermedio = new CContenidoPresupuestarioD(contexto);
                var datos = intermedio.BuscarContenidoPresupuestario(contenido.NumeroResolucion);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta = ConvertirDatosContenidoADTO(((ContenidoPresupuestario)datos.Contenido));
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
        //INSERTÉ ESTE METODO EN ICPuestoService y en CPuestoService el 25/01/2017...
        public CBaseDTO GuardarNumResolucion(CDetallePuestoDTO detallePuesto, CContenidoPresupuestarioDTO contenido)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CContenidoPresupuestarioD intermedio = new CContenidoPresupuestarioD(contexto);
                //SE LE ASIGNA A contendidoPresupuestario que es de datos, la palabra ContenidoF para ser usada mas adelante.
                ContenidoPresupuestario ContenidoP = new ContenidoPresupuestario
                {
                    NumResolucion = contenido.NumeroResolucion,
                    FecRige = contenido.FechaRige,
                    //FecPago = contenido.FechaPago                    
                };
                //a var datos se le asigna un intermedio para guardar el numresolucion, se guarda en detallePuesto y luego se le asigna el"ContenidoP"
                var datos = intermedio.BuscarNumResolucion(detallePuesto.IdEntidad, ContenidoP);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirDatosContenidoADTO((ContenidoPresupuestario)datos.Contenido);

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
