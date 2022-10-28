using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDireccionGeneralL
    {
        #region Variables

        SIRHEntities contexto;
        CDireccionGeneralD direccionGeneralDescarga;

        #endregion

        #region Constructor

        public CDireccionGeneralL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        //Se insertó en PuestoService el 30/01/2017
        public List<CDireccionGeneralDTO> DescargarDireccionGenerals(int codigo, string nombre)
        {
            List<CDireccionGeneralDTO> resultado = new List<CDireccionGeneralDTO>();
            direccionGeneralDescarga = new CDireccionGeneralD(contexto);
            var item = direccionGeneralDescarga.CargarDireccionesGeneralesParam(codigo, nombre);

            foreach (var aux in item)
            {
                CDireccionGeneralDTO direccionGeneral = new CDireccionGeneralDTO();

                direccionGeneral.IdEntidad = aux.PK_DireccionGeneral;
                direccionGeneral.NomDireccion = aux.NomDireccion;

                resultado.Add(direccionGeneral);
            }

            return resultado;
        }

        public List<CBaseDTO> ListarDireccionGeneral()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CDireccionGeneralD intermedio = new CDireccionGeneralD(contexto);
            var direcciones = intermedio.CargarDireccionesGenerales();
            if (direcciones != null)
            {
                foreach (var item in direcciones)
                {
                    respuesta.Add(new CDireccionGeneralDTO
                    {
                        IdEntidad = item.PK_DireccionGeneral,
                        NomDireccion = item.NomDireccion
                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron direcciones generales" });
            }
            return respuesta;
        }

        internal static CDireccionGeneralDTO ConvertirDireccionGeneralADTO(DireccionGeneral item)
        {
            CDireccionGeneralDTO respuesta = new CDireccionGeneralDTO
            {
                IdEntidad = item.PK_DireccionGeneral,
                NomDireccion = item.NomDireccion,


            };
            return respuesta;
        }

        internal static DireccionGeneral ConvertirCDireccionGeneralDTOaDatos(CDireccionGeneralDTO item)
        {
            return new DireccionGeneral
            {
                PK_DireccionGeneral = item.IdEntidad,
                NomDireccion = item.NomDireccion


            };
        }

        #endregion
    }
}
