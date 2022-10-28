using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CModalidadL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CModalidadL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Metodos
        //Insertado en CFormacionAcademicaService y ICFormacionAcademicaService(Deivert)
        public CBaseDTO BuscarModalidad(int codigo)
        {
            CBaseDTO respuesta;
            try
            { 
                //el tipo modalidad de datos le asigno intermedio(flechas) a un nuevo modalidad de datos que viene de contexto.
                CModalidadD intermedio = new CModalidadD(contexto);
                //ir a intermedio para buscar en datos mediante el metodo BuscarModalidad el código de modalidad(cuando es codigo es solo UNA MODALIDAD)
                var datos = intermedio.BuscarModalidad(codigo);
                if (datos != null)
                {
                    //si el codigo que esta en datos trae errores
                    if (datos.Codigo != -1)
                    {  
                        //respuesta es modalidad convertido a DTO, que viene de datos en Contenido.
                        respuesta = ConvertirModalidadADTO((Modalidad)datos.Contenido);
                        return respuesta;
                    }
                    else
                    {  
                        //si viene error en el contenido, se debe de convertir datos.Contenido a CErrorDTO, y que muestre el mj de error 
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {  
                    //si viene null entonces no viene nada, y uno digita el mj de error.
                    throw new Exception("Ocurrió un error inesperado en la consulta");
                }
            }
            catch (Exception error)
            {
                //retorna el error que viene de la base de datos
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }
        //Insertado en CFormacionAcademicaService y ICFormacionAcademicaService(Deivert)
        public List<CBaseDTO> ListarModalidad()
        {
            //lista de BaseDTO la respuesta será igual a una nueva lista de BaseDTO
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            { 
                //
                CModalidadD intermedio = new CModalidadD(contexto);
                var datos = intermedio.ListarModalidad();
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        //Para cada item en la lista de modalidades, que viene de datos.Contenido, procesa cada dato
                        //lista   |1|2|3|4|5|
                        //foreach para cada uno de la lista
                        //item    |1| procesa, |2|procesa...etc. el item va pasando uno por uno.
                        foreach (var item in (List<Modalidad>)datos.Contenido)
                        {
                            //en la respuesta de lista se convierte cada item de Datos a DTO
                            respuesta.Add(ConvertirModalidadADTO(item));
                        }                        
                        return respuesta;
                    }
                    else
                    {  
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {  
                    throw new Exception("Ocurrió un error inesperado en la consulta");
                }
            }
            catch (Exception error)
            {
                //a la respuesta agrega el error (solo para listas se digita respuesta.Add)
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        internal static CModalidadDTO ConvertirModalidadADTO(Modalidad modalidad)
        {  //respuesta con la información de la modalidad
            return new CModalidadDTO
            {    //atributo = ruta
                IdEntidad = modalidad.PK_Modalidad,
                Descripcion = modalidad.DesModalidad,
                TopePuntos = Convert.ToInt32(modalidad.IndTopePuntos)
            };
        }
        
        #endregion
    }
}
