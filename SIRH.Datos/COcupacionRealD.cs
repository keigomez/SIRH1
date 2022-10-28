using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;

namespace SIRH.Datos
{
    public class COcupacionRealD
    {

        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public COcupacionRealD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        
        /// Obtiene la lista de las Ocupaciones Reales de la BD        
        /// <returns>Retorna una lista las ocupaciones reales</returns>
        public List<OcupacionReal> CargarOcupacionesReales()
        {
            List<OcupacionReal> resultados = new List<OcupacionReal>();

            resultados = entidadBase.OcupacionReal.ToList();

            return resultados;
        }

        public CRespuestaDTO GuardarOcupacionReal(string codPuesto, OcupacionReal ocupacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.OcupacionReal.Add(ocupacion);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = ocupacion
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
        /// Obtiene la carga de las Ocupaciones Reales de la BD
        /// </summary>
        /// <returns>Retorna las Ocupaciones Reales</returns>          
        public OcupacionReal CargarOcupacionRealPorID(int idOcupacionReal)
        {
            OcupacionReal resultado = new OcupacionReal();

            resultado = entidadBase.OcupacionReal.Where(R => R.PK_OcupacionReal == idOcupacionReal).FirstOrDefault();

            return resultado;
        }

        //Por Cedula
        public OcupacionReal CargarOcupacionReal(string cedula)
        {
            OcupacionReal resultado = new OcupacionReal();

            resultado = entidadBase.OcupacionReal.Where(R =>
                                                        R.DetallePuesto.Where(Q =>
                                                                                Q.Puesto.Nombramiento.Where(K =>
                                                                                                                K.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).Count() > 0).FirstOrDefault();
            return resultado;
        }
        //Por Parámetro
        public OcupacionReal CargarOcupacionRealParam(string DescripcionOcupacionReal)
        {
            OcupacionReal resultado = new OcupacionReal();

            resultado = entidadBase.OcupacionReal.Where(R => R.DesOcupacionReal.ToLower().Contains(DescripcionOcupacionReal.ToLower())).FirstOrDefault();

            return resultado;
        }

        /// <summary>
        /// Obtiene y Enlista las Ocupaciones Reales
        /// </summary>
        /// <param name="codOcupacionReal"></param>
        /// <param name="nomOcupacionReal"></param>
        /// <returns>Retorna las Ocupaciones Reales</returns>
        public List<OcupacionReal> CargarOcupacionRealesParams(int codOcupacionReal, string nomOcupacionReal)
        {
            List<OcupacionReal> resultado = entidadBase.OcupacionReal.ToList();

            if (codOcupacionReal != 0 && codOcupacionReal != null)
            {
                resultado = resultado.Where(Q => Q.PK_OcupacionReal == codOcupacionReal).ToList();
            }
            if (nomOcupacionReal != "" && nomOcupacionReal != null)
            {
                resultado = resultado.Where(Q => Q.DesOcupacionReal.ToLower().Contains(nomOcupacionReal.ToLower())).ToList();
            }

            return resultado;
        }

        #endregion

    }
}

