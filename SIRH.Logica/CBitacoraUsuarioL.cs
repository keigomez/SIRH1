using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using System.Data.Objects;
using System.Data.EntityClient;

namespace SIRH.Logica
{
    public class CBitacoraUsuarioL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CBitacoraUsuarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CBitacoraUsuarioDTO ConvertirDatosBitacoraUsuarioADTO(BitacoraUsuario item)
        {
            return new CBitacoraUsuarioDTO
            {
                IdEntidad = item.PK_BitacoraUsuario,
                CodigoAccion = Convert.ToInt32(item.CodAccion),
                CodigoEntidad = Convert.ToInt32(item.CodEntidad),
                CodigoModulo = Convert.ToInt32(item.CodModulo),
                CodigoObjetoEntidad = Convert.ToInt32(item.CodObjetoEntidad),
                FechaEjecucion = Convert.ToDateTime(item.FecEjecucion),
                Usuario = CUsuarioL.UsuarioADto(item.Usuario)
            };
        }

        internal static BitacoraUsuario ConvertirDTOBitacoraUsuarioADatos(CBitacoraUsuarioDTO item)
        {
            return new BitacoraUsuario
            {
                CodAccion = Convert.ToInt32(item.CodigoAccion),
                CodModulo = Convert.ToInt32(item.CodigoModulo),
                CodObjetoEntidad = Convert.ToInt32(item.CodigoObjetoEntidad),
                FecEjecucion = DateTime.Now
            };
        }

      
        public CBaseDTO GuardarBitacora(CBitacoraUsuarioDTO bitacora)
        {
            try
            {
                string a = "";

                foreach (var item in bitacora.EntidadesAfectadas)
                {
                    a = "dto";
                    var bitacoraDatos = ConvertirDTOBitacoraUsuarioADatos(bitacora);
                    a = "usr";
                    bitacoraDatos.Usuario = contexto.Usuario.Where(Q => Q.NomUsuario == bitacora.Usuario.NombreUsuario).FirstOrDefault();
                    a = "guardar";
                    var resultado = new CBitacoraUsuarioD(contexto).GuardarBitacora(bitacoraDatos, item);
                    if (resultado.Contenido.GetType() == typeof(CErrorDTO))
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError 
                            + "," + bitacoraDatos.CodAccion 
                            + "," + bitacoraDatos.CodModulo
                            + "," + bitacoraDatos.CodObjetoEntidad
                            + "," + bitacoraDatos.Usuario.NomUsuario
                            + "," + bitacoraDatos.FecEjecucion
                            + "," + item
                            + "," + a
                            );
                    }
                }

                return bitacora;
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }


        public List<CBaseDTO> ListarBitacora(CBitacoraUsuarioDTO bitacora, List<DateTime> fechas)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CBitacoraUsuarioD intermedio = new CBitacoraUsuarioD(contexto);

            var resultado = intermedio.ListarBitacora(bitacora, fechas);

            if (resultado.Codigo > 0)
            {
                foreach (var item in (List<BitacoraUsuario>)resultado.Contenido)
                {
                    respuesta.Add(ConvertirDatosBitacoraUsuarioADTO(item));
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }
        #endregion
    }
}
