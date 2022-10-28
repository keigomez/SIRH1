using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCursoCapacitacionL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCursoCapacitacionL()
        {
            contexto = new SIRHEntities();
        }
        #endregion

        #region Metodos

        internal static CCursoCapacitacionDTO ConvertirCursoCapacitacionADTO(CursoCapacitacion cursoCapacitacion)
        {
            return new CCursoCapacitacionDTO
            {
                IdEntidad = cursoCapacitacion.PK_CursoCapacitacion,
                FechaInicio = Convert.ToDateTime(cursoCapacitacion.FecInicio),
                FechaFinal = Convert.ToDateTime(cursoCapacitacion.FecFin),
                Resolucion = cursoCapacitacion.NumResolucion,
                DescripcionCapacitacion = cursoCapacitacion.DesCursoCapacitacion,
                TotalHoras = Convert.ToInt32(cursoCapacitacion.IndTotalHoras),
                TotalPuntos = Convert.ToInt32(cursoCapacitacion.IndTotalPuntos),
                ImagenTitulo = cursoCapacitacion.ImgTitulo,
                Estado = Convert.ToInt32(cursoCapacitacion.Estado),
                Modalidad =  new CModalidadDTO
                {
                    IdEntidad = cursoCapacitacion.Modalidad.PK_Modalidad,
                    Descripcion = cursoCapacitacion.Modalidad.DesModalidad
                }
            };
        }        
        #endregion
    }
}
