using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
   public class CPresupuestoD
   {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CPresupuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Metodos
        /// <summary>
        /// Guarda el presupuesto
        /// </summary>
        /// <returns>Retorna el presupuesto</returns>
        public int GuardarPresupuesto(Presupuesto Presupuesto)
        {
            entidadBase.Presupuesto.Add(Presupuesto);
            return Presupuesto.PK_Presupuesto;
        }
       
        /// <summary>
        /// Carga los presupuestos de la BD
        /// </summary>
        /// <returns>Retorna presupuestos</returns>
        public Presupuesto CargarPresupuestoPorID(int idPresupuesto)
        {
            Presupuesto resultado = new Presupuesto();

            resultado = entidadBase.Presupuesto.Where(R => R.PK_Presupuesto == idPresupuesto).FirstOrDefault();

            return resultado;
        }
        
        /// <summary>
        /// POR CEDULA    
        /// </summary>
        /// <param name="cedula"></param>
        /// <returns></returns>
        public Presupuesto CargarPresupuestoCedula(string cedula)
        {
            Presupuesto resultado = new Presupuesto();

            resultado = entidadBase.Presupuesto.Where(R =>
                                                        R.UbicacionAdministrativa.Where(Q =>
                                                                                        Q.Puesto.Where(K =>
                                                                                                       K.Nombramiento.Where(Z =>
                                                                                                                            Z.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).Count() > 0).Count() > 0).FirstOrDefault();
                                                                                                                               
          
            return resultado;
        }

        public List<Presupuesto> CargarPresupuestosParam(string idpresupuesto)
        {
            List<Presupuesto> resultado = entidadBase.Presupuesto.ToList();

            if (idpresupuesto != "0")
            {
                resultado = resultado.Where(Q => Q.IdPresupuesto.Contains(idpresupuesto)).ToList();
            }

            return resultado;
        }

        public List<Programa> CargarCodigoProgramas()
        {
            List<Programa> resultado = entidadBase.Programa.ToList();

            return resultado;
        }

        public CRespuestaDTO ListarPresupuestos()
        {
            CRespuestaDTO respuesta;

            try
            {
                var datosPresupuesto = entidadBase.Presupuesto.ToList();

                if (datosPresupuesto != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosPresupuesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron Presupuestos asociados.");
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

        public CRespuestaDTO BuscarPresupXCodPresupuestario(string codPresupuestario)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datosEntidad = entidadBase.Presupuesto.FirstOrDefault(P => P.IdPresupuesto == codPresupuestario);
                if (datosEntidad != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosEntidad
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún estado de código presupuestario que coincida");
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
                return respuesta;
            }
        }

        public List<Presupuesto> BuscarCodigoPresupuestarioList(string codigopresupuesto)
        {
            List<Presupuesto> result = new List<Presupuesto>();
            result = entidadBase.Presupuesto.Where(U => U.IdPresupuesto != null && U.IdPresupuesto.Contains(codigopresupuesto)).Distinct().ToList();
            return result;
        }


        #endregion

    }
    
}

