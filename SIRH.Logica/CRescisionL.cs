using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using System.Data;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CRescisionL
    {
        #region Variables
        
        SIRHEntities contexto;
        
        #endregion

        #region Constructor

        public CRescisionL()
        {
            contexto = new SIRHEntities();
        }
       
        #endregion

        #region Metodos

        internal static CRescisionDTO ConvertirDatosRescisionADTO(Rescision item)
        {
            return new CRescisionDTO
            {
                NumeroRescision = item.NumRescision,
                NumeroOficio = item.NumOficio,
                FechaDeOficio = Convert.ToDateTime(item.FecOficio),
                FechaRescision = Convert.ToDateTime(item.FecRescision)
            };
        }

        //Se insertó en ICPuestoService y CPuestoService el 27/01/2017
        public CBaseDTO GuardarNumeroRescision(CRescisionDTO rescisionP, CPrestamoPuestoDTO prestamo)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CRescisionD intermedio = new CRescisionD(contexto);
                //SE LE ASIGNA A RESCISION que es de datos, la palabra rescisionP para ser usada mas adelante.
                Rescision rescisionPuesto = new Rescision
                {
                    NumRescision = rescisionP.NumeroRescision,
                    NumOficio = rescisionP.NumeroOficio,
                    FecRescision = Convert.ToDateTime(rescisionP.FechaRescision),
                    FecOficio = Convert.ToDateTime(rescisionP.FechaDeOficio),
                };

                //a var datos se le asigna un intermedio para guardar LA RESCISION, se guarda en prestamo y luego se le asigna la "Rescision"
                var datos = intermedio.GuardarNumRescision(prestamo.IdEntidad, rescisionPuesto);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirDatosRescisionADTO((Rescision)datos.Contenido);

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
