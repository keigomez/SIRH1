using SIRH.Datos;
using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Logica
{
    public class CCursoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCursoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        public List<CBaseDTO> CargarCursos()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();



            CCursoD intermedio = new CCursoD(contexto);

            var cursos = intermedio.CargarCursos();

            if (cursos.Codigo > 0)
            {
                var datosCurso = (List<C_EMU_Cursos>)cursos.Contenido;
                foreach (var curso in datosCurso)
                {
                    respuesta.Add(ConvertirDatosCursoADTO(curso));
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraro ningún registro de cursos" });
            }

            return respuesta;
        }

        public CBaseDTO CargarCursoPorID(int id)
        {
            CBaseDTO respuesta;

            CCursoD intermedio = new CCursoD(contexto);

            var curso = intermedio.CargarCursoPorID(id);

            if (curso.Codigo > 0)
            {
                respuesta = ConvertirDatosCursoADTO((C_EMU_Cursos)curso.Contenido);
            }
            else
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = "No se encontraro ningún registro de cursos" };
            }

            return respuesta;
        }

        public List<CBaseDTO> BuscarCursos(CCursoDTO curso, List<DateTime> fechas)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CCursoD intermedio = new CCursoD(contexto);

            List<C_EMU_Cursos> datosCurso = new List<C_EMU_Cursos>();

            if (curso.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCursos(datosCurso, curso.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosCurso = (List<C_EMU_Cursos>)resultado.Contenido;
                }
            }

            if (curso.Nombre != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCursos(datosCurso, curso.Nombre, "Nombre"));
                if (resultado.Codigo > 0)
                {
                    datosCurso = (List<C_EMU_Cursos>)resultado.Contenido;
                }
            }

            //Test
            //List<DateTime> fechasTemp = new List<DateTime>();
            //fechasTemp.Add(new DateTime(2009, 8, 24));
            //fechasTemp.Add(new DateTime(2009, 8, 25));

            if (fechas != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCursos(datosCurso, fechas, "Fecha"));
                if (resultado.Codigo > 0)
                {
                    datosCurso = (List<C_EMU_Cursos>)resultado.Contenido;
                }
            }

            if (curso.NombreCurso != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCursos(datosCurso, curso.NombreCurso, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCurso = (List<C_EMU_Cursos>)resultado.Contenido;
                }
            }

            if (curso.TipoCurso != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCursos(datosCurso, curso.TipoCurso, "TipoCurso"));
                if (resultado.Codigo > 0)
                {
                    datosCurso = (List<C_EMU_Cursos>)resultado.Contenido;
                }
            }

            if (curso.Resolucion != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCursos(datosCurso, curso.Resolucion, "Resolucion"));
                if (resultado.Codigo > 0)
                {
                    datosCurso = (List<C_EMU_Cursos>)resultado.Contenido;
                }
            }




            if (datosCurso.Count > 0)
            {
                foreach (var item in datosCurso)
                {

                    var datoCurso = ConvertirDatosCursoADTO(item);
                    respuesta.Add(datoCurso);

                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }


        internal static CCursoDTO ConvertirDatosCursoADTO(C_EMU_Cursos curso)
        {
            CCursoDTO respuesta;
            respuesta = new CCursoDTO
            {
                ID = Convert.ToInt32(curso.ID),               
                Nombre = Convert.ToString(curso.Nombre),
                Cedula = Convert.ToString(curso.Cedula),
                NombreCurso = Convert.ToString(curso.NombreCurso),
                TotalHoras = Convert.ToDecimal(curso.TotalHoras),
                TipoCurso = Convert.ToString(curso.TipoCurso),
                ImpartidoEn = Convert.ToString(curso.ImpartidoEn),
                FecRige = Convert.ToDateTime(curso.FecRige),
                FecFinal = Convert.ToDateTime(curso.FecFinal),
                Resolucion = Convert.ToString(curso.Resolucion)
            };
            return respuesta;
        }


        #endregion
    }
}
