using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CAddendumPrestamoPuestoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CAddendumPrestamoPuestoL()
        {
            contexto = new SIRHEntities();
        }    

        #endregion


        #region Metodos

        internal static CAddendumPrestamoPuestoDTO ConvertirDatosAddendumADTO(AddendumPrestamoPuesto itemAddendum)
        {
            return new CAddendumPrestamoPuestoDTO
            {
                IdEntidad = itemAddendum.PK_AddendumPrestamoPuesto,                  
                NumeroAddendum = itemAddendum.NumAddendum,
                FechaRige = Convert.ToDateTime(itemAddendum.FecRige),
                FechaFin = Convert.ToDateTime(itemAddendum.FecFin)
            };              
        }

        //Se insertó en ICPuestoService y CPuestoService el 27/01/2017
        public CBaseDTO BuscarAddendumPrestamoPuesto(CAddendumPrestamoPuestoDTO addendum)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CAddendumPrestamoPuestoD intermedio = new CAddendumPrestamoPuestoD(contexto);
                var datos = intermedio.BuscarAddendumPrestamoPuesto(addendum.NumeroAddendum);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    respuesta = ConvertirDatosAddendumADTO(((AddendumPrestamoPuesto)datos.Contenido));
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
        
        //BUSCAR num de addendum POR LISTA, hacer FOREACH
        //Se insertó en ICPuestoService y CPuestoService el 27/01/2017
        public List<CBaseDTO> BuscarAddendumIdPrestamo(CPrestamoPuestoDTO prestamo, CAddendumPrestamoPuestoDTO addendum)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CAddendumPrestamoPuestoD intermedio = new CAddendumPrestamoPuestoD(contexto);
                var datos = intermedio.BuscarAddendumIdPrestamo(prestamo.IdEntidad);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //se hace foreach, xq en D, hay un TOLIST (En este mismo método)
                    foreach (var item in ((List<AddendumPrestamoPuesto>)datos.Contenido))
                    {
                        respuesta.Add(ConvertirDatosAddendumADTO(item));

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
         //Guardar el num de Addendum, en prestamo Puesto  y en el addendum prestamo puesto
        //Se insertó en ICPuestoService y CPuestoService el 26/01/2017
        public CBaseDTO GuardarNumAddendum(CPrestamoPuestoDTO prestamo, CAddendumPrestamoPuestoDTO addendum)               
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CAddendumPrestamoPuestoD intermedio = new CAddendumPrestamoPuestoD(contexto);
                //SE LE ASIGNA A AddendumPrestamoPuesto que es de datos, la palabra addendumP para ser usada mas adelante.
                AddendumPrestamoPuesto addendumP = new AddendumPrestamoPuesto
                {
                    NumAddendum = addendum.NumeroAddendum,
                    FecRige = addendum.FechaRige,
                    FecFin = addendum.FechaFin
                };
                //a var datos se le asigna un intermedio para guardar el addendum, se guarda en prestamo y luego se le asigna el"addendumP"
                var datos = intermedio.GuardarNumAddendum(prestamo.IdEntidad, addendumP);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirDatosAddendumADTO((AddendumPrestamoPuesto)datos.Contenido);

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
