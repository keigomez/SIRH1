using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using System.Data;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CProvinciaL
    {
        #region Variables

        SIRHEntities contexto;
        CDistritoD ProvinciaDescarga;

        #endregion
        
        #region Constructor           

        public CProvinciaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        //pendiente Deivert

        public List<CBaseDTO> ListarProvincias()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CProvinciaD intermedio = new CProvinciaD(contexto);

            var provincias = intermedio.CargarProvincias(null); //ojo aqui
            if (provincias != null)
            {
                foreach (var item in provincias)
                {
                    respuesta.Add(new CProvinciaDTO { IdEntidad = item.PK_Provincia, NomProvincia = item.NomProvincia });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron provincias" });
            }

            return respuesta;
        }

        internal static Provincia ConvertirProvinciaDatosaDTO(CProvinciaDTO item)
        {
            return new Provincia
            {
                PK_Provincia = item.IdEntidad,
                NomProvincia = item.NomProvincia
            };
        }

        internal static CProvinciaDTO ConvertirProvinciaDTOaDatos(Provincia item)
        {
            return new CProvinciaDTO
            {
                IdEntidad = item.PK_Provincia,
                NomProvincia = item.NomProvincia
            };
        }

        #endregion
    }
}
