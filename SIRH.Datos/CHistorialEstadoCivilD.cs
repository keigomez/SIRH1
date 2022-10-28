using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CHistorialEstadoCivilD
    {
        #region Variables

        private SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CHistorialEstadoCivilD(SIRHEntities entidadGlobal)
        {
            contexto = entidadGlobal;
        }

        #endregion

        #region Metodos
         
        internal static CHistorialEstadoCivilDTO ConvertirDatosHistorialEstadoCivilADTO(HistorialEstadoCivil item)
        {
            return new CHistorialEstadoCivilDTO
            {
                IdEntidad = item.PK_HistorialEstadoCivil,
                FecIncio = Convert.ToDateTime(item.FecInicio),
                FecFin = Convert.ToDateTime(item.FecFin)
            };
        }

        /// <summary>
        /// Guarda el Estado Civil en la BD
        /// </summary>
        /// <param name="estadoCivil"></param>
        /// <returns>Retorna el Estado Civil</returns>
        public int GuardarHistorialEstadoCivil(HistorialEstadoCivil estadoCivil)
        {
            contexto.HistorialEstadoCivil.Add(estadoCivil);
            return estadoCivil.PK_HistorialEstadoCivil;
        }


        //22/11/2016, "GUARDAR HISTORIAL ESTADO CIVIL DEL FUNCIONARIO"   
                                                       //ESTOS PARAMETROS TIENE QUE ESTAR IGUAL EN CFUNCIONARIOL EN ESTE MISMO METODO
        public CRespuestaDTO GuardarHistorialEstadoCivil(string cedula, HistorialEstadoCivil historialEstadoCivil)
        {
            CRespuestaDTO respuesta;
            try
            {
                contexto.HistorialEstadoCivil.Add(historialEstadoCivil);
                contexto.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = historialEstadoCivil
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
        /// Obtiene la Carga del Estado Civil de la BD
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns>Retorna el Estado Civil</returns>
        public List<HistorialEstadoCivil> CargarCatEstadoCivilCedula(string cedula)
        {
            List<HistorialEstadoCivil> resultado = new List<HistorialEstadoCivil>();

            resultado = contexto.HistorialEstadoCivil.Include("CatEstadoCivil").Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula).ToList();
            return resultado;
        }

          // 22/11/2016 GUARDAR HISTORIAL DE ESTADO CIVIL......
        //public CRespuestaDTO GuardarHistorialEstadoCivil(HistorialEstadoCivil historialEstadoCivil)
        //{
        //    CRespuestaDTO respuesta;
        //    try
        //    {
        //        entidadBase.AddToHistorialEstadoCivil(historialEstadoCivil);
        //        entidadBase.SaveChanges();

        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = 1,
        //            Contenido = historialEstadoCivil
        //        };

        //        return respuesta;
        //    }
        //    catch (Exception error)
        //    {
        //        respuesta = new CRespuestaDTO
        //        {
        //            Codigo = -1,
        //            Contenido = new CErrorDTO { MensajeError = error.Message }
        //        };

        //        return respuesta;
        //    }
        //}

        #endregion

    }
}
