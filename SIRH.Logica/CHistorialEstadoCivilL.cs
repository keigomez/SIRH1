using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CHistorialEstadoCivilL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CHistorialEstadoCivilL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CHistorialEstadoCivilDTO ConvertirHistorialEstadoCivilADTO(HistorialEstadoCivil item)
        {
            return new CHistorialEstadoCivilDTO
            {
                IdEntidad = item.PK_HistorialEstadoCivil,
                FecIncio = Convert.ToDateTime(item.FecInicio),
                FecFin = Convert.ToDateTime(item.FecFin),
                CatEstadoCivil = new CCatEstadoCivilDTO
                {
                    DesEstadoCivil = item.CatEstadoCivil.DesEstadoCivil
                }
            };
        }

        //Se registró en CFuncionarioService y ICFuncionarioService el 30/01/2017
        public CBaseDTO GuardarHistEstadoCivil(CFuncionarioDTO funcionario, CHistorialEstadoCivilDTO historialEstadoCivil)
        {                   
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CHistorialEstadoCivilD intermedio = new CHistorialEstadoCivilD(contexto);
                //SE LE ASIGNA A HISTORIALESTADOCIVIL que es de datos, la palabra HistorialEC para ser usada mas adelante.
                HistorialEstadoCivil HistorialEC = new HistorialEstadoCivil
                {
                    FecInicio = Convert.ToDateTime(historialEstadoCivil.FecIncio),
                    FecFin = Convert.ToDateTime(historialEstadoCivil.FecIncio)
                };
                //a var datos se le asigna un intermedio para guardar el ESTADO CIVIL, se guarda en HistorialEstadoCivil y luego se le asigna el"HistorialEC"
                var datos = intermedio.GuardarHistorialEstadoCivil(funcionario.Cedula, HistorialEC);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {
                    //datos sin punto antes, el punto es la dependencia.
                    //en respuesta le asigno convertir los datos a DTO, y castear el tipo de dato entre parentesis y luego datos.contendio
                    respuesta = ConvertirHistorialEstadoCivilADTO((HistorialEstadoCivil)datos.Contenido);

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
