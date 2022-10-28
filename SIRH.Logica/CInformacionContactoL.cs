using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CInformacionContactoL
    {
        #region Variables

        SIRHEntities contexto;
        CInformacionContactoD infoDescarga;

        #endregion

        #region constructor

        public CInformacionContactoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CInformacionContactoDTO ConvertirInformacionContactoADTO(InformacionContacto item)
        {
            return new CInformacionContactoDTO
            {
                IdEntidad = item.PK_InformacionContacto,
                DesAdicional = item.DesAdicional,
                DesContenido = item.DesContenido,
                TipoContacto = new CTipoContactoDTO
                {
                    IdEntidad = item.TipoContacto.PK_TipoContacto,
                    DesTipoContacto = item.TipoContacto.DesTipoContacto
                }
            };
        }

        //Se insertó en ICFuncionarioService y CFuncionarioService el 30/01/2017
        public List<CBaseDTO> DescargarInformacionContacto(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            infoDescarga = new CInformacionContactoD(contexto);

            foreach (var item in infoDescarga.DescargarInformacionContactoCedula(cedula))
            {
                CInformacionContactoDTO temp = new CInformacionContactoDTO();
                temp.IdEntidad = item.PK_InformacionContacto;
                temp.DesAdicional = item.DesAdicional;
                temp.DesContenido = item.DesContenido;
                //temp.Funcionario = new CFuncionarioDTO { IdEntidad = item.Funcionario.PK_Funcionario };
                temp.TipoContacto = new CTipoContactoDTO
                {
                    IdEntidad = item.TipoContacto.PK_TipoContacto,
                    DesTipoContacto = item.TipoContacto.DesTipoContacto
                };

                resultado.Add(temp);
            }

            return resultado;
        }

        //Se insertó en ICFuncionarioService y CFuncionarioService el 30/01/2017
        public CBaseDTO GuardarInformacionContacto(CFuncionarioDTO funcionario, CInformacionContactoDTO informacionContacto)
        {      
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CInformacionContactoD intermedio = new CInformacionContactoD(contexto);
                //SE LE ASIGNA A InformacionContacto que es de datos, la palabra InformacionC para ser usada mas adelante.
                InformacionContacto InformacionC = new InformacionContacto
                {
                    DesContenido = informacionContacto.DesContenido,
                    DesAdicional = informacionContacto.DesAdicional,
                };               
                var datos = intermedio.GuardarInformacionContacto(funcionario.Cedula, InformacionC);
                if (datos.Contenido.GetType() != typeof(CErrorDTO))
                {                    
                    respuesta = ConvertirInformacionContactoADTO((InformacionContacto)datos.Contenido);

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
        #endregion 
    }
}
