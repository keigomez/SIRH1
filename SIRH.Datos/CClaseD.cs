using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
   public class CClaseD
    {
       #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CClaseD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// Guarda las clases en la BD
        /// </summary>
        /// <returns>Retorna las Clases</returns>

        public int GuardarClase(Clase Clase)
        {
            entidadBase.Clase.Add(Clase);
            return Clase.PK_Clase;
        }

        public CRespuestaDTO GuardarClase(string Codpuesto, Clase clase)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Clase.Add(clase);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = clase
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
        /// Obtiene la lista de Clases de la BD        
        public List<Clase> CargarClases()
        {
            List<Clase> resultados = new List<Clase>();

            resultados = entidadBase.Clase.ToList();

            return resultados;
        }
        /// <summary>
        /// Obtiene la carga de las Clases de la BD
        /// </summary>
        /// <returns>Retorna las clases</returns>
        public Clase CargarClasePorID(int idClase)
        {
            Clase resultado = new Clase();

            resultado = entidadBase.Clase.Where(R => R.PK_Clase == idClase).FirstOrDefault();

            return resultado;
           
        }
        
       /// <summary>
       /// Carga las clases por Parámetro
       /// </summary>
       /// <param name="nombreClase"></param>
       /// <returns>Retorna las clases</returns>
        public Clase CargarClaseParam(string nombreClase)
        {
            Clase resultado = new Clase();

            resultado = entidadBase.Clase.Where(R => R.DesClase.ToLower().Contains(nombreClase.ToLower())).FirstOrDefault();

            return resultado;
        }

       /// <summary>
       /// Genera una lista de Clases ordenadas por ID y por Parámetro
       /// </summary>
       /// <param name="idClase"></param>
       /// <param name="nombreClase"></param>
       /// <returns>Enlista las clases</returns>
        public List<Clase> CargarClasesParams(int idClase, string nombreClase)
        {
            List<Clase> resultado = new List<Clase>();
            resultado = entidadBase.Clase.ToList();

            if(idClase != 0 && idClase != null)
            {
                resultado = resultado.Where(Q => Q.PK_Clase == idClase && Q.IndEstadoClase == 1).ToList();
            }
            if (nombreClase != "" && nombreClase != null)
            {
                resultado = resultado.Where(Q => Q.DesClase.ToLower().Contains(nombreClase.ToLower()) && Q.IndEstadoClase == 1).ToList();
            }

            return resultado;
        }

        public CRespuestaDTO ListarClasesConFormato(string formato)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosClase = entidadBase.Clase.Where(c => c.DesClase.StartsWith(formato)).ToList();

                if (datosClase != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosClase
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Clases asociadas.");
                }

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };

                return respuesta;
            }
        }

        #endregion

    }
}

