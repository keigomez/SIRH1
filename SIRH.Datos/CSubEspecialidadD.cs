using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
    public class CSubEspecialidadD
   {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CSubEspecialidadD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        
        #region Metodos

        /// Obtiene la lista de las Subespecialidades de la BD       
        //Retorna una lista de subespecialidades</returns>        
        public List<SubEspecialidad> CargarSubEspecialidades()
        {
            List<SubEspecialidad> resultados = new List<SubEspecialidad>();

            resultados = entidadBase.SubEspecialidad.ToList();

            return resultados;
        }

        public CRespuestaDTO GuardarSubEspecialidad(string codPuesto, SubEspecialidad subEspecialidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.SubEspecialidad.Add(subEspecialidad);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = subEspecialidad
                };
                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return respuesta;
            }
        }


        /// <summary>
        /// Obtiene la carga de las SubEspecialidades de la BD
        /// </summary>
        /// <returns>Retorna las SubEspecialidades</returns>
        public SubEspecialidad CargarSubEspecialidadPorID(int idSubEspecialidad)
        {
            SubEspecialidad resultado = new SubEspecialidad();

            resultado = entidadBase.SubEspecialidad.Where(R => R.PK_SubEspecialidad == idSubEspecialidad).FirstOrDefault();

            return resultado;
        }
        //Por Parametros
        public SubEspecialidad CargarSubEspecialidadParam(string DescripcionSubEspecialidad)
        {
            SubEspecialidad resultado = new SubEspecialidad();

            resultado = entidadBase.SubEspecialidad.Where(R => R.DesSubEspecialidad.ToLower().Contains(DescripcionSubEspecialidad.ToLower())).FirstOrDefault();

            return resultado;
        }

        public CRespuestaDTO BuscarSubespecialidadParam(int codigo, string nombre)
        {
            try
            {
                List<SubEspecialidad> resultado;
                if (codigo != 0)
                {
                    resultado = entidadBase.SubEspecialidad.Where(S => S.PK_SubEspecialidad == codigo).ToList();
                }
                else
                {
                    resultado = entidadBase.SubEspecialidad.Where(S => S.DesSubEspecialidad.Contains(nombre)).ToList();
                }

                if (resultado != null || resultado.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se encontraron resultados para la búsqueda establecida");
                }
            }
            catch (Exception error)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }
        }

        #endregion

   }
}
