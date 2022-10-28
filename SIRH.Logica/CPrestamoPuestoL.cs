using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CPrestamoPuestoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CPrestamoPuestoL()

        {
            contexto = new SIRHEntities(); 
        }

        #endregion

        #region Metodos

        internal static CPrestamoPuestoDTO ConvertirDatosPrestamoPuestoADTO(PrestamoPuesto item)
        {
            return new CPrestamoPuestoDTO
            {
                IdEntidad = item.PK_PrestamoPuesto,
                FechaDeTraslado = Convert.ToDateTime(item.FecTraslado),
                FechaFinalConvenio = Convert.ToDateTime(item.FecFinConvenio),
                NumDeResolucion = item.NumResolucion,
                NumOficioDeRescision = item.NumOficioRescision,                
            };
        }

        //Buscar Lista de prestamos de un puesto
        //Insertado en ICPuestoService y CPuestoService el 26/01/2017
        public List<CBaseDTO> BuscarPrestamoPuestoPorPuesto(CPrestamoPuestoDTO prestamo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CPrestamoPuestoD intermedio = new CPrestamoPuestoD(contexto);
                var datos = intermedio.BuscarPrestamoPuestoPorPuesto(prestamo.Puesto.CodPuesto);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //se hace foreach, xq en D, hay un TOLIST (En este mismo método)
                    foreach (var item in ((List<PrestamoPuesto>)datos.Contenido))
                    {
                        //se pone ADD cuando es lista
                        respuesta.Add(ConvertirDatosPrestamoPuestoADTO(item));
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

        //Insertado en ICPuestoService y CPuestoService el 26/01/2017 
        public CBaseDTO GuardarPrestamoPuesto(CPuestoDTO puesto,CPrestamoPuestoDTO prestamo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CPrestamoPuestoD intermedio = new CPrestamoPuestoD(contexto);
                //SE LE ASIGNA A Factor que es de datos, la palabra factorB para ser usada mas adelante.
                PrestamoPuesto PrestamoP = new PrestamoPuesto
                {
                    FecTraslado = prestamo.FechaDeTraslado,
                    NumResolucion = prestamo.NumDeResolucion,
                    NumOficioRescision = prestamo.NumOficioDeRescision,
                    FecFinConvenio = prestamo.FechaFinalConvenio,
                    NumRescision = prestamo.NumDeRescision,
                };
                    
                //a var datos se le asigna un intermedio para guardar el prestamoPuesto, se guarda en Puesto y luego se le asigna el"prestamo"
                var datos = intermedio.GuardarPrestamoPuesto(puesto.CodPuesto,PrestamoP);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirDatosPrestamoPuestoADTO((PrestamoPuesto)datos.Contenido);

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
