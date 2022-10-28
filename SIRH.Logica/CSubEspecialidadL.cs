using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CSubEspecialidadL
    {
        #region Variables

        SIRHEntities contexto;
        CSubEspecialidadD SubEspecialidadDescarga;

        #endregion

        #region Constructor

        public CSubEspecialidadL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CSubEspecialidadDTO ConvertirSubEspecialidadADTO(SubEspecialidad item)
        {
            return new CSubEspecialidadDTO
            {
                IdEntidad = item.PK_SubEspecialidad,
                DesSubEspecialidad = item.DesSubEspecialidad,
                IndEstSubEspecialidad = Convert.ToInt32(item.IndEstadoSubEspecialidad),
            };
        }

        public List<CSubEspecialidadDTO> DescargarSubespecialidades(string nombre)
        {
            List<CSubEspecialidadDTO> resultado = new List<CSubEspecialidadDTO>();
            SubEspecialidadDescarga = new CSubEspecialidadD(contexto);
            var item = SubEspecialidadDescarga.CargarSubEspecialidades();

            foreach (var aux in item)
            {
                CSubEspecialidadDTO temp = new CSubEspecialidadDTO();

                temp.IdEntidad = aux.PK_SubEspecialidad;
                temp.DesSubEspecialidad = aux.DesSubEspecialidad;
                temp.IndEstSubEspecialidad = Convert.ToInt32(aux.IndEstadoSubEspecialidad);

                resultado.Add(temp);
            }
            return resultado;
        }

        public List<CBaseDTO> BuscarSubespecialidadParam(int codigo, string nombre)
        {
            try
            {
                List<CBaseDTO> resultado = new List<CBaseDTO>();
                var datos = new CSubEspecialidadD(contexto);
                var respuesta = datos.BuscarSubespecialidadParam(codigo, nombre);

                if (respuesta.Codigo != -1)
                {
                    foreach (var aux in (List<SubEspecialidad>)respuesta.Contenido)
                    {
                        CSubEspecialidadDTO temp = new CSubEspecialidadDTO();
                        temp.IdEntidad = aux.PK_SubEspecialidad;
                        temp.DesSubEspecialidad = aux.DesSubEspecialidad;
                        temp.IndEstSubEspecialidad = Convert.ToInt32(aux.IndEstadoSubEspecialidad);
                        resultado.Add(temp);
                    }

                    return resultado;
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO>
                {
                    new CErrorDTO { MensajeError = error.Message }
                };
            }

        }

        #endregion
    }
}
