using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleNombramientoL
    {
        #region Variables
        
        SIRHEntities contexto;

        #endregion

        #region Constructor
        
        public CDetalleNombramientoL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Metodos

        //internal static CDetalleNombramientoDTO ConvertirDatosDetalleNombramientoADTO(DetalleNombramiento item)
        //{

        //    string observaciones = "";
        //    if (!String.IsNullOrEmpty(item.ObsJornada)) {
        //        observaciones = item.ObsJornada;
        //    }
        //    return new CDetalleNombramientoDTO
        //    {
        //        IdEntidad = item.PK_DetalleNombramiento,
        //        FecCreacion = Convert.ToDateTime(item.FecCreacion),
        //        ObservacionesTipoJornada = observaciones
        //    };
        //}

        //internal static CDetalleNombramientoDTO ConvertirDetalleNombramientoADTO(DetalleNombramiento item)
        //{
        //    return new CDetalleNombramientoDTO
        //    {
        //        FecCreacion = Convert.ToDateTime(item.FecCreacion)
        //    };
        //}
        
        //06/01/17..REVISAR CON DEIVERT....
        //Insertado en ICFuncionarioService y CFuncionarioService el 25/01/2017
        public CBaseDTO GuardarDetalleNombramiento(CDetalleNombramientoDTO detalleNombramiento)
        {
            return new CBaseDTO();
            //CBaseDTO respuesta = new CBaseDTO();
            //try
            //{
            //    CDetalleNombramientoD intermedio = new CDetalleNombramientoD(contexto);
            //    CNombramientoD intermedioNombramiento = new CNombramientoD(contexto);
            //    CTipoJornadaD intermedioTipoJornada = new CTipoJornadaD(contexto);                
              
            //    DetalleNombramiento datos = new DetalleNombramiento                
            //    {  
            //        FecCreacion = Convert.ToDateTime(detalleNombramiento.FecCreacion),                    
            //        TipoJornada = intermedioTipoJornada.CargarTipoJornadaPorID(detalleNombramiento.IdEntidad),                   
            //    };

            //    intermedio.GuardarDetalleNombramiento(datos);

            //    return respuesta;
            //}
            //catch (Exception error)
            //{
            //    //en respuesta del catch, se digita el error 
            //    respuesta = new CErrorDTO { MensajeError = error.Message };
            //    return respuesta;
            //}
        }

        public CBaseDTO ObtenerDetalleNombramientoFuncionario(CFuncionarioDTO funcionario)
        {
            return new CBaseDTO();
            //CBaseDTO respuesta = new CBaseDTO();
            //try
            //{
            //    CDetalleNombramientoD intermedio = new CDetalleNombramientoD(contexto);

            //    Funcionario funcionarioAux = new Funcionario
            //    {
            //        IdCedulaFuncionario = funcionario.Cedula
            //    };

            //    var respuestaAUX = intermedio.ObtenerDetalleNombramientoFuncionario(funcionarioAux);

            //    if (respuestaAUX.Codigo > 0)
            //    {
            //        respuesta = ConvertirDatosDetalleNombramientoADTO((DetalleNombramiento)respuestaAUX.Contenido);
            //    }
            //    else {
            //        respuesta = (CErrorDTO)((CRespuestaDTO)respuestaAUX).Contenido;
            //        throw new Exception();
            //    }

            //    return respuesta;
            //}
            //catch (Exception error)
            //{
            //    respuesta = new CErrorDTO { MensajeError = ((CErrorDTO)respuesta).MensajeError };
            //    return respuesta;
            //}
        }      

        #endregion
    }
}
