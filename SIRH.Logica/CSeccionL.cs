using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CSeccionL
    {
        #region Variables

        SIRHEntities contexto;
        CSeccionD SeccionDescarga;

        #endregion

        #region Constructor

        public CSeccionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        public List<CSeccionDTO> DescargarSeccions(int codigo, string nombre)
        {
            List<CSeccionDTO> resultado = new List<CSeccionDTO>();
            SeccionDescarga = new CSeccionD(contexto);
            var item = SeccionDescarga.CargarSeccionesParam(codigo, nombre);

            foreach (var aux in item)
            {
                CSeccionDTO Seccion = new CSeccionDTO();
                Seccion.IdEntidad = aux.PK_Seccion;
                Seccion.NomSeccion = aux.NomSeccion;
                Seccion.IndEstSeccion = Convert.ToInt32(aux.IndEstadoSeccion);
                resultado.Add(Seccion);
            }

            return resultado;
        }

        internal static CSeccionDTO ConvertirSeccionADTO(Seccion item)
        {
            CSeccionDTO respuesta = new CSeccionDTO
            {
                IdEntidad = item.PK_Seccion,
                IndEstSeccion = Convert.ToInt32(item.IndEstadoSeccion),
                NomSeccion = item.NomSeccion


            };
            return respuesta;
        }

        internal static Seccion ConvertirCSeccionDTOaDatos(CSeccionDTO item)
        {
            Seccion respuesta = new Seccion
            {
                PK_Seccion = item.IdEntidad,
                IndEstadoSeccion = Convert.ToInt32(item.IndEstSeccion),
                NomSeccion = item.NomSeccion


            };
            return respuesta;
        }

        public List<CBaseDTO> ListarSecciones()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            CSeccionD intermedio = new CSeccionD(contexto);
            var secciones = intermedio.CargarSecciones();
            if (secciones != null)
            {
                foreach (var item in secciones)
                {
                    respuesta.Add(new CSeccionDTO
                    {
                        IdEntidad = item.PK_Seccion,
                        NomSeccion = item.NomSeccion,
                    });
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron secciones" });
            }
            return respuesta;
        }

        #endregion
    }
}
