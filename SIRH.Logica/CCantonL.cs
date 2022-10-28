using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using System.Data;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCantonL
    {
        #region Variables

        SIRHEntities contexto;
        CDistritoD CantonDescarga;

        #endregion
        
        #region Constructor           

        public CCantonL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos        
        
        internal static CCantonDTO ConvertirCantonADto(Canton item)
        {
            return new CCantonDTO
            {
                IdEntidad = item.PK_Canton,
                NomCanton = item.NomCanton,
                CodPostalCanton = item.CodPostalCanton
            };
        }

        public List<CBaseDTO> ListarCantones()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CCantonD intermedio = new CCantonD(contexto);

            var cantones = intermedio.CargarCantones();
            if (cantones != null)
            {
                foreach (var item in cantones)
                {
                    respuesta.Add(new CCantonDTO { IdEntidad = item.PK_Canton, NomCanton = item.NomCanton });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron cantones" });
            }

            return respuesta;
        }

        internal static Canton ConvertirCantonDatosaDTO(CCantonDTO item)
        {
            return new Canton
            {
                PK_Canton = item.IdEntidad,
                Provincia = CProvinciaL.ConvertirProvinciaDatosaDTO(item.Provincia),
                CodPostalCanton = item.CodPostalCanton,
                NomCanton = item.NomCanton,
            };
        }

        internal static CCantonDTO ConvertirCantonDTOaDatos(Canton item)
        {
            return new CCantonDTO
            {
                IdEntidad = item.PK_Canton,
                Provincia = CProvinciaL.ConvertirProvinciaDTOaDatos(item.Provincia),
                CodPostalCanton = item.CodPostalCanton,
                NomCanton = item.NomCanton,
            };
        }


        #endregion

    }
}
