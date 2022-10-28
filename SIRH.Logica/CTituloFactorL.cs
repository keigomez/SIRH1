using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CTituloFactorL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CTituloFactorL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Metodos

        internal static CTituloFactorDTO ConvertirDatosTituloFactorADTO(TituloFactor item)
        {
            return new CTituloFactorDTO
            {
                IdEntidad = item.PK_TituloFactor,
                DescripcionTitulo = item.DesTitulo                    
            }; 
        }
        
        //BUSCAR TITULO POR ID... 
        //Insertado en ICPuestoService y CPuestoService el 27/01/2017
        public CBaseDTO BuscarTituloFactorId(CTituloFactorDTO tituloFactor)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CTituloFactorD intermedio = new CTituloFactorD(contexto);             
                var datos = intermedio.BuscarTituloFactorId(tituloFactor.IdEntidad);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta = ConvertirDatosTituloFactorADTO(((TituloFactor)datos.Contenido));
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
      
          //TRAER LA LISTA DE TITULOS FACTOR POR PUESTO, LOS FACTORES Y CARACTERISTICS QUE SON DATOS QUE DEBEN DE VENIR LIGADOS A PUESTO.
        //Insertado en ICPuestoService y CPuestoService el 26/01/2017
        public List<CBaseDTO> BuscarTitulosFactorPorPuesto(CPuestoDTO puesto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CTituloFactorD intermedio = new CTituloFactorD(contexto);
                var datos = intermedio.BuscarTitulosFactorPorPuesto(puesto.CodPuesto);
                 
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //Para cada item en la lista de titulos factor, que viene de datos.Contenido, procesa cada dato
                    //lista   |1|2|3|4|5|
                    //foreach para cada uno de la lista
                    //item    |1| procesa, |2|procesa...etc. el item va pasando uno por uno. Contenido tiene un dato únicamente
                    // este primer foreach es para traer el primer dato, ejemplo Puesto 
                    foreach (var item in ((List<Puesto>)datos.Contenido))
                    {
                        //en respuesta se agrega en Puesto en Logica, el item que sería el puesto en este caso.
                        respuesta.Add(CPuestoL.PuestoGeneral(item));
                        //se hace OTRO foreach para la demás información que viene con puesto, o sea el dato del primer foreach.
                        foreach (var caracteristica in item.CaracteristicasPuesto)
                        {
                            //la respuesta será agregar en caracteristicasPuesto de logica las caracteristicas convertidas a DTO.
                            respuesta.Add(CCaracteristicasPuestoL.ConvertirDatosCaracteristicasPuestoADTO(caracteristica));
                            //Se agrega en factor de logica el factor convertido a DTO
                            respuesta.Add(CFactorL.ConvertirDatosFactorADTO(caracteristica.Factor));
                            //Se agrega en Titulo factor de logica el titulo factor convertido a DTO
                            respuesta.Add(ConvertirDatosTituloFactorADTO(caracteristica.Factor.TituloFactor));
                        }
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

        //Se insertó en ICPuestoService y CPuestoService el 26/01/2017
        public CBaseDTO GuardarTituloFactor(CTituloFactorDTO titulo)
        {
           //cuando no se tiene que guardar una lista, se pone CBaseDTO y NO List<CBaseDTO>
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CTituloFactorD intermedio = new CTituloFactorD(contexto);
                //SE LE ASIGNA A TITULOFACTOR que es de datos, la palabra tituloF para ser usada mas adelante.
                TituloFactor tituloF = new TituloFactor
                {
                    DesTitulo = titulo.DescripcionTitulo                    
                };
                //a var datos se le asigna un intermedio que guardar a titulo factor el titulo factor que se le asignó a "tituloF"
                var datos = intermedio.GuardarTituloFactor(tituloF);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {                                                                           //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirDatosTituloFactorADTO((TituloFactor)datos.Contenido);                   
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
                respuesta =  new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }              
                     
        #endregion
    }
}
            
 
           













