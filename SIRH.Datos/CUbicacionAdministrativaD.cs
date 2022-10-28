using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using SIRH.DTO;

namespace SIRH.Datos
{
     public class CUbicacionAdministrativaD
     {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CUbicacionAdministrativaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Metodos
        /// <summary>
        /// Guarda la Ubicacion Administrativa
        /// </summary>
        /// <returns>Retorna Ubicacion administrativa</returns>
        public int GuardarUbicacionAdministrtiva(UbicacionAdministrativa UbicacionAdministrativa)
        {
            entidadBase.UbicacionAdministrativa.Add(UbicacionAdministrativa);
            return UbicacionAdministrativa.PK_UbicacionAdministrativa;
        }

        /// <summary>
        /// Obtiene la lista de las Ubicaciones Administrativas de la BD
        /// </summary>
        /// <returns>Retorna una lista de Ubicaciones adm.</returns>
        
        public List<UbicacionAdministrativa> CargarUbicacionAdministrativa()
        {
            List<UbicacionAdministrativa> resultados = new List<UbicacionAdministrativa>();

            resultados = entidadBase.UbicacionAdministrativa.ToList();

            return resultados;
        }

        /// <summary>
        /// Carga las Ubicaciones Administrativas de la BD
        /// </summary>
        /// <returns>Retorna la carga de Ubicacion administrativa</returns>
        public UbicacionAdministrativa CargarUbicacionAdministrativaPorID(int idUbicacionAdministrativa)
        {
            UbicacionAdministrativa resultado = new UbicacionAdministrativa();

            resultado = entidadBase.UbicacionAdministrativa.Where(R => R.PK_UbicacionAdministrativa == idUbicacionAdministrativa).FirstOrDefault();

            return resultado;
        }

        private DbQuery<UbicacionAdministrativa> RetornarPresupuesto()
        {
            //return entidadBase.UbicacionAdministrativa.Include("Presupuesto").Include("Presupuesto.Programa").Include("Presupuesto.Area").Include("Presupuesto.Actividad");
            return entidadBase.UbicacionAdministrativa.Include("Presupuesto").Include("Presupuesto.Programa");
        }

        private DbQuery<UbicacionAdministrativa> RetornarSeccion()
        {
            return RetornarPresupuesto().Include("DireccionGeneral").Include("Seccion").Include("Departamento").Include("Division");
        }

         //POR CÉDULA
        public UbicacionAdministrativa CargarUbicacionAdministrativaCedula(string cedula)
        {
            UbicacionAdministrativa temp = new UbicacionAdministrativa();

            temp = RetornarSeccion().Where(Q => Q.Puesto.Where(
                R=> R.Nombramiento.Where(K => K.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).Count() > 0).FirstOrDefault();

            return temp;
        }

        public List<UbicacionAdministrativa> BuscarCentroCostosList(string centrocostos)
        {
            List<UbicacionAdministrativa> result = new List<UbicacionAdministrativa>();
            result = entidadBase.UbicacionAdministrativa.Where(U => U.DesObservaciones != null && U.DesObservaciones.Contains(centrocostos)).Distinct().ToList();
            return result;
        }

        public CRespuestaDTO UbicacionAdministrativaSeccion(int seccion)
        {
            try
            {
                var resultado = entidadBase.UbicacionAdministrativa.Include("Division")
                                                                   .Include("DireccionGeneral")
                                                                   .Include("Departamento")
                                                                   .Include("Seccion")
                                                                   .Include("Presupuesto")
                                                                   .Where(U => U.FK_Seccion == seccion).ToList();

                if (resultado.Count > 0)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se encontraron ubicaciones administrativas que coincidan con la sección ingresada");
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
       

       
    
