using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CEspecialidadL
    {
        #region Variables

        SIRHEntities contexto;
        CEspecialidadD EspecialidadDescarga;

        #endregion

        #region Constructor

        public CEspecialidadL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        //Se registró en ICPuestoService y CPuestoService 30/01/2017
        public List<CEspecialidadDTO> DescargarEspecialidades(int codigo, string nombre)
        {
            List<CEspecialidadDTO> resultado = new List<CEspecialidadDTO>();
            EspecialidadDescarga = new CEspecialidadD(contexto);
            var item = EspecialidadDescarga.CargarEspecialidadesParams(codigo, nombre);

            foreach (var aux in item)
            {
                CEspecialidadDTO temp = new CEspecialidadDTO();

                temp.IdEntidad = aux.PK_Especialidad;
                temp.DesEspecialidad = aux.DesEspecialidad;
                temp.IndEstEspecialidad = Convert.ToInt32(aux.IndEstadoEspecialidad);
                     

                resultado.Add(temp);
            }
            return resultado;
        }

        internal static CEspecialidadDTO ConstruirEspecialidad(Especialidad entrada)
        {
            return new CEspecialidadDTO 
            {
                IdEntidad = entrada.PK_Especialidad,
                DesEspecialidad = entrada.DesEspecialidad
            };
        }
        
        #endregion
    }
}
