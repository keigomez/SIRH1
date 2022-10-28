using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using System.Data;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDistritoL
    {
        #region Variables

        SIRHEntities contexto;
        CDistritoD DistritoDescarga;

        #endregion
        
        #region Constructor           

        public CDistritoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        //pendiente Deivert

        public List<CBaseDTO> ListarDistritos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CDistritoD intermedio = new CDistritoD(contexto);

            var distritos = intermedio.CargarDistritos();
            if (distritos != null)
            {
                foreach (var item in distritos)
                {
                    respuesta.Add(new CDistritoDTO { NomDistrito = item.NomDistrito });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron distritos" });
            }

            return respuesta;
        }

        internal static Distrito ConvertirDistritoDatosaDTO(CDistritoDTO item)
        {
            return new Distrito
            {
                PK_Distrito = item.IdEntidad,
                Canton = CCantonL.ConvertirCantonDatosaDTO(item.Canton),
                CodPostalDistrito = item.CodPostalDistrito,
                NomDistrito = item.NomDistrito
            };
        }
        internal static Distrito ConvertirDistritoDatosaDTOBasicos(CDistritoDTO item)
        {
            return new Distrito
            {
                PK_Distrito = item.IdEntidad,
                CodPostalDistrito = item.CodPostalDistrito,
                NomDistrito = item.NomDistrito
            };
        }

        internal static CDistritoDTO ConvertirViaticoCorridoDTOaDatos(Distrito item)
        {
            return new CDistritoDTO
            {
                IdEntidad = item.PK_Distrito,
                Canton = CCantonL.ConvertirCantonDTOaDatos(item.Canton),
                CodPostalDistrito = item.CodPostalDistrito,
                NomDistrito = item.NomDistrito,
            };
        }

        public CBaseDTO CargarDistritoId(int idDistrito)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDistritoD intermedio = new CDistritoD(contexto);

                var distrito = intermedio.CargarDistritoId(idDistrito);
                if (distrito != null)
                {
                    var dato= ConvertirViaticoCorridoDTOaDatos((Distrito)distrito);

                    respuesta =dato;
                }
                else
                {
                    respuesta = new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = "Error"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta=new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }
        #endregion


    }
}
