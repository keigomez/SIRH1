using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SIRH.Datos.Helpers;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CFuncionarioD
    {
        #region Variables

        /// <summary>
        /// Contexto de la entidad funcionario
        /// <autor>Deivert Guiltrichs</autor>
        /// </summary>
        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la clase Funcionario
        /// </summary>
        /// <autor>Deivert Guiltrichs</autor>
        /// <param name="entidadGlobal"></param>
        public CFuncionarioD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Almacena datos del funcionario en la BD
        /// </summary>
        /// <autor>Deivert Guiltrichs</autor>
        /// <param name="funcionario"></param>
        /// <returns>Devuelve la llave primaria del registro insertado</returns>
        public int GuardarFuncionario(Funcionario funcionario)
        {
            if (entidadBase.Funcionario.Where(F => F.IdCedulaFuncionario == funcionario.IdCedulaFuncionario).Count() < 1)
            {
                entidadBase.Funcionario.Add(funcionario);
            }
            return funcionario.PK_Funcionario;
        }

        //22/11/2016 GUARDAR DATOS DE FUNCIONARIO.....le envío la cedula
        public CRespuestaDTO GuardarDatosFuncionario(string cedula, Funcionario funcionario)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Funcionario.Add(funcionario);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = funcionario
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
        public CRespuestaDTO BuscarFuncionarioProfesional(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                respuesta.Contenido = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.EstadoPuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Where(F => F.IdCedulaFuncionario == cedula && F.Nombramiento.Where(N => (N.FecVence == null
                                                                || N.FecVence >= DateTime.Now) && N.Puesto.IndNivelOcupacional == 1
                                                                    || N.Puesto.IndNivelOcupacional == 2
                                                                    || N.Puesto.IndNivelOcupacional == 3
                                                                    || N.Puesto.IndNivelOcupacional == 5).Count() > 0)
                                                   .FirstOrDefault();
                if ((Funcionario)respuesta.Contenido != null)
                {
                    if (((Funcionario)respuesta.Contenido).EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                    {
                        throw new Exception("El funcionario no se encuentra Activo, por lo que no puede mostrarse en el módulo.");
                    }
                }
                else
                {
                    throw new Exception("El funcionario no se encuentra registrado como profesional o bien no se encontró ningún funcionario asociado a la cédula indicada. Por favor revise los datos suministrados.");
                }

            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }
        public CRespuestaDTO BuscarFuncionarioPolicial(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                respuesta.Contenido = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.EstadoPuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Where(F => F.IdCedulaFuncionario == cedula && F.Nombramiento.Where(N => (N.FecVence == null
                                                                || N.FecVence >= DateTime.Now) && N.Puesto.IndNivelOcupacional == 1
                                                                    || N.Puesto.IndNivelOcupacional == 2
                                                                    || N.Puesto.IndNivelOcupacional == 3
                                                                    || N.Puesto.IndNivelOcupacional == 5).Count() > 0)
                                                   .FirstOrDefault();
                if ((Funcionario)respuesta.Contenido != null)
                {
                    if (((Funcionario)respuesta.Contenido).EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                    {
                        throw new Exception("El funcionario no se encuentra Activo, por lo que no puede mostrarse en el módulo.");
                    }
                }
                else
                {
                    throw new Exception("El funcionario no se encuentra registrado como profesional o bien no se encontró ningún funcionario asociado a la cédula indicada. Por favor revise los datos suministrados.");
                }

            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }

        public CRespuestaDTO BuscarFuncionarioPolicial(decimal codPolicial)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                respuesta.Contenido = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   //.Where(F => F.DetalleContratacion.FirstOrDefault().CodPolicial == codPolicial && F.Nombramiento.Where(N => (N.FecVence == null
                                                   //             || N.FecVence >= DateTime.Now) && N.Puesto.IndNivelOcupacional == 2
                                                   //                 || N.Puesto.IndNivelOcupacional == 3
                                                   //                 || N.Puesto.IndNivelOcupacional == 5).Count() > 0)
                                                   .Where(F => F.DetalleContratacion.FirstOrDefault().CodPolicial == codPolicial)
                                                   .ToList()
                                                   .FirstOrDefault();

                if ((Funcionario)respuesta.Contenido != null)
                {
                    //if (((Funcionario)respuesta.Contenido).EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                    //{
                    //    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede mostrarse.");
                    //}
                }
                else
                {
                    throw new Exception("El funcionario no se encuentra registrado como profesional o bien no se encontró ningún funcionario con ese Código Policial. Por favor revise los datos suministrados.");
                }

            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }


        public CRespuestaDTO ListarFuncionarioPolicial()
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                respuesta.Contenido = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   //.Where(F => F.DetalleContratacion.FirstOrDefault().CodPolicial != null && F.DetalleContratacion.FirstOrDefault().CodPolicial != 0 && F.Nombramiento.Where(N => (N.FecVence == null
                                                   //             || N.FecVence >= DateTime.Now) && N.Puesto.IndNivelOcupacional == 2
                                                   //                 || N.Puesto.IndNivelOcupacional == 3
                                                   //                 || N.Puesto.IndNivelOcupacional == 5).Count() > 0)
                                                   .Where(F => F.Nombramiento.Where(N => N.Puesto.DetallePuesto.Where(DP => DP.FK_Clase.Value >= 8303 && DP.FK_Clase.Value <= 8506 || DP.FK_Clase.Value >= 14765 && DP.FK_Clase.Value <= 14830).Count() > 0).Count() > 0)
                                                   .OrderBy(Q => Q.DetalleContratacion.FirstOrDefault().CodPolicial)
                                                   .ToList();
                if (respuesta.Contenido == null)
                    throw new Exception("No se encontraró ningún funcionario con Código Policial. Por favor revise los datos suministrados.");
            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }
        /// <summary>
        /// Actualiza los datos del funcionario en la BD
        /// </summary>
        /// <autor>Deivert Guiltrichs</autor>
        /// <param name="funcionario"></param>
        /// <returns>Número de filas afectadas</returns>
        /* public bool ActualizarFuncionario(Funcionario funcionario)
         {
             EntityKey llave;
             object objetoOriginal;

             using (entidadBase)
             {
                 llave = entidadBase.CreateEntityKey("Funcionario", funcionario);
                 if (entidadBase.TryGetObjectByKey(llave, out objetoOriginal))
                 {
                     entidadBase.ApplyPropertyChanges(llave.EntitySetName, funcionario);
                 }
             }

             int respuesta = entidadBase.SaveChanges();

             if (respuesta > 0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
         }*/



        public List<Funcionario> BuscarFuncionario(Funcionario funcionario)
        {
            List<Funcionario> resultado = entidadBase.Funcionario.Include("EstadoFuncionario").
                                                                    Include("Nombramiento").
                                                                    Include("Nombramiento.Puesto").
                                                                    Include("Nombramiento.Puesto.DetallePuesto").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").
                                                                    ToList();

            bool condicionCedula = funcionario.IdCedulaFuncionario != "" && funcionario.IdCedulaFuncionario != null;
            bool condicionNombre = funcionario.NomFuncionario != "" && funcionario.NomFuncionario != null;
            bool condicionApellido1 = funcionario.NomPrimerApellido != "" && funcionario.NomPrimerApellido != null;
            bool condicionApellido2 = funcionario.NomSegundoApellido != "" && funcionario.NomSegundoApellido != null;
            bool estado = funcionario.EstadoFuncionario != null && funcionario.EstadoFuncionario.DesEstadoFuncionario != "";

            if (condicionCedula)
            {
                resultado = resultado.Where(q => q.IdCedulaFuncionario.Contains(funcionario.IdCedulaFuncionario)).ToList();
            }
            if (estado)
            {
                switch (funcionario.EstadoFuncionario.DesEstadoFuncionario)
                {
                    case "Servidor Activo":
                        resultado = resultado.Where(q => q?.EstadoFuncionario?.PK_EstadoFuncionario == 1).ToList();
                        break;
                    case "Permiso sin salario":
                        resultado = resultado.Where(q => q?.EstadoFuncionario?.PK_EstadoFuncionario == 2).ToList();
                        break;
                    case "Suspensión Indefinida":
                        resultado = resultado.Where(q => q?.EstadoFuncionario?.PK_EstadoFuncionario == 3).ToList();
                        break;
                    case "Translado a otra Institución":
                        resultado = resultado.Where(q => q?.EstadoFuncionario?.PK_EstadoFuncionario == 4 || q?.EstadoFuncionario?.PK_EstadoFuncionario == 25).ToList();
                        break;
                    case "Permiso con Salario":
                        resultado = resultado.Where(q => q?.EstadoFuncionario?.PK_EstadoFuncionario == 6).ToList();
                        break;
                    case "Exfuncionario":
                        resultado = resultado.Where(q => q.EstadoFuncionario != null && q.EstadoFuncionario.PK_EstadoFuncionario == 7 ||
                        q.EstadoFuncionario.PK_EstadoFuncionario == 8 || q.EstadoFuncionario.PK_EstadoFuncionario == 9 || q.EstadoFuncionario.PK_EstadoFuncionario == 10 ||
                        q.EstadoFuncionario.PK_EstadoFuncionario == 11 || q.EstadoFuncionario.PK_EstadoFuncionario == 12 || q.EstadoFuncionario.PK_EstadoFuncionario == 13 ||
                        q.EstadoFuncionario.PK_EstadoFuncionario == 14 || q.EstadoFuncionario.PK_EstadoFuncionario == 17 || q.EstadoFuncionario.PK_EstadoFuncionario == 18).ToList();
                        break;
                }

            }
            if (condicionNombre)
            {
                resultado = resultado.Where(q => q.NomFuncionario.ToLower().Contains(funcionario.NomFuncionario.ToLower())).ToList();
            }
            if (condicionApellido1)
            {
                resultado = resultado.Where(q => q.NomPrimerApellido.ToLower().Contains(funcionario.NomPrimerApellido.ToLower())).ToList();
            }
            if (condicionApellido2)
            {
                resultado = resultado.Where(q => q.NomSegundoApellido.ToLower().Contains(funcionario.NomSegundoApellido.ToLower())).ToList();
            }

            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            return resultado;
        }

        public List<Funcionario> BuscarFuncionarioOriginal(Funcionario funcionario)
        {
            List<Funcionario> resultado = entidadBase.Funcionario.Include("EstadoFuncionario").
                                                                    Include("Nombramiento").
                                                                    Include("Nombramiento.Puesto").
                                                                    Include("Nombramiento.Puesto.DetallePuesto").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").ToList();

            bool condicionCedula = funcionario.IdCedulaFuncionario != "" && funcionario.IdCedulaFuncionario != null;
            bool condicionNombre = funcionario.NomFuncionario != "" && funcionario.NomFuncionario != null;
            bool condicionApellido1 = funcionario.NomPrimerApellido != "" && funcionario.NomPrimerApellido != null;
            bool condicionApellido2 = funcionario.NomSegundoApellido != "" && funcionario.NomSegundoApellido != null;

            if (condicionCedula)
            {
                resultado = resultado.Where(q => q.IdCedulaFuncionario.Contains(funcionario.IdCedulaFuncionario)).ToList();
            }
            if (condicionNombre)
            {
                resultado = resultado.Where(q => q.NomFuncionario.ToLower().Contains(funcionario.NomFuncionario.ToLower())).ToList();
            }
            if (condicionApellido1)
            {
                resultado = resultado.Where(q => q.NomPrimerApellido.ToLower().Contains(funcionario.NomPrimerApellido.ToLower())).ToList();
            }
            if (condicionApellido2)
            {
                resultado = resultado.Where(q => q.NomSegundoApellido.ToLower().Contains(funcionario.NomSegundoApellido.ToLower())).ToList();
            }

            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            return resultado;
        }

        private List<Funcionario> BuscarFuncionarioRestante(List<Funcionario> contextoRestante, Funcionario funcionario)
        {
            List<Funcionario> resultado = contextoRestante;

            bool condicionCedula = funcionario.IdCedulaFuncionario != "" && funcionario.IdCedulaFuncionario != null;
            bool condicionNombre = funcionario.NomFuncionario != "" && funcionario.NomFuncionario != null;
            bool condicionApellido1 = funcionario.NomPrimerApellido != "" && funcionario.NomPrimerApellido != null;
            bool condicionApellido2 = funcionario.NomSegundoApellido != "" && funcionario.NomSegundoApellido != null;


            if (condicionCedula)
            {
                var temp = resultado.Where(q => q.IdCedulaFuncionario.Contains(funcionario.IdCedulaFuncionario)).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (condicionNombre)
            {
                var temp = resultado.Where(q => q.NomFuncionario.ToLower().Contains(funcionario.NomFuncionario.ToLower())).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (condicionApellido1)
            {
                var temp = resultado.Where(q => q.NomPrimerApellido.ToLower().Contains(funcionario.NomPrimerApellido.ToLower())).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (condicionApellido2)
            {
                var temp = resultado.Where(q => q.NomSegundoApellido.ToLower().Contains(funcionario.NomSegundoApellido.ToLower())).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }

            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            return resultado;
        }

        public List<Funcionario> BuscarFuncionarioDetallePuesto(string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal)
        {
            List<Funcionario> resultadoTotal = new List<Funcionario>();
            List<Funcionario> resultado = entidadBase.Funcionario.Include("EstadoFuncionario").Include("Nombramiento").Include("Nombramiento.Puesto").
                                                Include("Nombramiento.Puesto.DetallePuesto").Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                Include("Nombramiento.Puesto.DetallePuesto.Especialidad").Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").ToList();

            resultadoTotal = resultado;

            if (codPuesto != null && codPuesto != "")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.CodPuesto.TrimEnd(' ').Equals(codPuesto)).Count() > 0).ToList();
            }
            if (codClase != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Clase != null && D.Clase.PK_Clase.Equals(codClase)).Count() > 0).Count() > 0).ToList();
            }
            if (codEspecialidad != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Especialidad != null &&
                                                    D.Especialidad.PK_Especialidad.Equals(codEspecialidad)).Count() > 0).Count() > 0).ToList();
            }
            if (codOcupacionReal != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.OcupacionReal != null &&
                                                    D.OcupacionReal.PK_OcupacionReal.Equals(codOcupacionReal)).Count() > 0).Count() > 0).ToList();
            }

            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            if (resultado.Count == resultadoTotal.Count)
            {
                resultadoTotal = new List<Funcionario>();
            }
            else
            {
                resultadoTotal = resultado;
            }

            return resultadoTotal;
        }

        private List<Funcionario> BuscarFuncionarioDetallePuestoContextoRestante(List<Funcionario> contextoRestante, string codPuesto, string codClase, string codEspecialidad, string codOcupacionReal)
        {
            List<Funcionario> resultado = contextoRestante;

            if (codPuesto != null && codPuesto != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.CodPuesto.Equals(codPuesto)).Count() > 1).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (codClase != null && codClase != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Clase != null
                                                        && D.Clase.DesClase.ToLower().Contains(codClase.ToLower())).Count() > 0).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (codEspecialidad != null && codEspecialidad != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Especialidad != null &&
                                                    D.Especialidad.DesEspecialidad.ToLower().Contains(codEspecialidad.ToLower())).Count() > 0).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (codOcupacionReal != null && codOcupacionReal != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.OcupacionReal != null &&
                                                    D.OcupacionReal.DesOcupacionReal.ToLower().Contains(codOcupacionReal.ToLower())).Count() > 0).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }

            // resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            return resultado;
        }

        public List<Funcionario> BuscarFuncionarioUbicacion(int codDivision, int codDireccion, int codDepartamento, int codSeccion, string codPresupuesto)
        {
            var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30, 33, 35, 36, 37, 38, 39 };

            List<Funcionario> resultadoTotal = new List<Funcionario>();

            List<Funcionario> resultado = entidadBase.Funcionario.Include("EstadoFuncionario").Include("Nombramiento").
                                                                    Include("Nombramiento.Puesto").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").ToList();


            resultadoTotal = resultado;

            if (codDivision != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division.PK_Division.Equals(codDivision)).Count() > 0).ToList();
            }
            if (codDireccion != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.Equals(codDireccion)).Count() > 0).ToList();
            }
            if (codDepartamento != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento.PK_Departamento.Equals(codDepartamento)).Count() > 0).ToList();
            }
            if (codSeccion != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion.Equals(codSeccion)).Count() > 0).ToList();
            }
            if (codPresupuesto != "0")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento)).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Presupuesto != null &&
                                                    N.Puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto.Equals(codPresupuesto)).Count() > 0).ToList();
            }

            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            if (resultado.Count == resultadoTotal.Count)
            {
                resultadoTotal = new List<Funcionario>();
            }
            else
            {
                resultadoTotal = resultado;
            }

            return resultadoTotal;
        }

        public List<Funcionario> BuscarFuncionarioDetallePuestoPreCargado(List<Funcionario> datosPrevios, string codPuesto, int codClase, int codEspecialidad, int codnivel)
        {
            List<Funcionario> resultadoTotal = new List<Funcionario>();
            List<Funcionario> resultado = new List<Funcionario>();
            if (datosPrevios.Count < 1)
            {
                resultado = entidadBase.Funcionario.Include("EstadoFuncionario").
                                                        Include("Nombramiento").
                                                        Include("Nombramiento.Puesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                        Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").ToList();
            }
            else
            {
                resultado = datosPrevios;
            }


            resultadoTotal = resultado;

            if (codPuesto != null && codPuesto != "")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.CodPuesto.TrimEnd(' ').Equals(codPuesto)).FirstOrDefault() != null).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimPuesto = ultimoNombramiento.Puesto;

                    if (ultimPuesto.CodPuesto.TrimEnd().Equals(codPuesto))
                    {
                        limpio.Add(item);
                    }
                }
                resultado = limpio;
            }
            if (codClase != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                    N.Puesto.DetallePuesto.Where(D => D.Clase != null && D.Clase.PK_Clase.Equals(codClase)).Count() > 0).FirstOrDefault() != null).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimoDetalle = ultimoNombramiento.Puesto.DetallePuesto.Where(Q => Q.Clase.PK_Clase.Equals(codClase)).FirstOrDefault();

                    if (ultimoDetalle != null)
                    {
                        if (ultimoNombramiento.Puesto == ultimoDetalle.Puesto)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }
            if (codEspecialidad != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Especialidad != null &&
                                                    D.Especialidad.PK_Especialidad.Equals(codEspecialidad)).Count() > 0).FirstOrDefault() != null).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimoDetalle = ultimoNombramiento.Puesto.DetallePuesto.Where(Q => Q.Especialidad.PK_Especialidad.Equals(codEspecialidad)).FirstOrDefault();

                    if (ultimoDetalle != null)
                    {
                        if (ultimoNombramiento.Puesto == ultimoDetalle.Puesto)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }
            if (codnivel != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.IndNivelOcupacional == codnivel).FirstOrDefault() != null).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimPuesto = ultimoNombramiento.Puesto;

                    if (ultimPuesto.IndNivelOcupacional.Equals(codnivel))
                    {
                        limpio.Add(item);
                    }
                }
                resultado = limpio;
            }


            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();


            if (resultado.Count == resultadoTotal.Count)
            {
                resultadoTotal = new List<Funcionario>();
            }
            else
            {
                resultadoTotal = resultado;
            }


            return resultadoTotal;

        }

        public List<Funcionario> BuscarFuncionarioDetallePuestoPreCargadoOriginal(List<Funcionario> datosPrevios, string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal)
        {
            List<Funcionario> resultadoTotal = new List<Funcionario>();
            List<Funcionario> resultado = new List<Funcionario>();
            if (datosPrevios.Count < 1)
            {
                resultado = entidadBase.Funcionario.Include("EstadoFuncionario").
                                                        Include("Nombramiento").
                                                        Include("Nombramiento.Puesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                        Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").ToList();
            }
            else
            {
                resultado = datosPrevios;
            }


            resultadoTotal = resultado;


            if (codPuesto != null && codPuesto != "")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.CodPuesto.TrimEnd(' ').Equals(codPuesto)).Count() > 0).ToList();
            }
            if (codClase != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Clase != null && D.Clase.PK_Clase.Equals(codClase)).Count() > 0).Count() > 0).ToList();
            }
            if (codEspecialidad != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.Especialidad != null &&
                                                    D.Especialidad.PK_Especialidad.Equals(codEspecialidad)).Count() > 0).Count() > 0).ToList();
            }
            if (codOcupacionReal != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.DetallePuesto != null &&
                                                    N.Puesto.DetallePuesto.Where(D => D.OcupacionReal != null &&
                                                    D.OcupacionReal.PK_OcupacionReal.Equals(codOcupacionReal)).Count() > 0).Count() > 0).ToList();
            }


            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();


            if (resultado.Count == resultadoTotal.Count)
            {
                resultadoTotal = new List<Funcionario>();
            }
            else
            {
                resultadoTotal = resultado;
            }


            return resultadoTotal;

        }

        public List<Funcionario> BuscarFuncionarioUbicacionPrecargado(List<Funcionario> datosPrevios, int codDivision, int codDireccion, int codDepartamento, int codSeccion, string codPresupuesto, string codcostos)
        {
            var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };


            List<Funcionario> resultadoTotal = new List<Funcionario>();


            List<Funcionario> resultado = new List<Funcionario>();




            if (datosPrevios.Count < 1)
            {
                resultado = entidadBase.Funcionario.Include("EstadoFuncionario").
                                                        Include("Nombramiento").
                                                        Include("Nombramiento.Puesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                        Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").ToList();
            }
            else
            {
                resultado = datosPrevios;
            }


            resultadoTotal = resultado;


            if (codDivision != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division.PK_Division.Equals(codDivision)).Count() > 0).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimaDivision = ultimoNombramiento.Puesto.UbicacionAdministrativa.Division;

                    if (ultimaDivision != null)
                    {
                        if (ultimoNombramiento.Puesto.UbicacionAdministrativa.Division == ultimaDivision)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }
            if (codDireccion != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.Equals(codDireccion)).Count() > 0).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimaDireccion = ultimoNombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral;

                    if (ultimaDireccion != null)
                    {
                        if (ultimoNombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral == ultimaDireccion)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }
            if (codDepartamento != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento.PK_Departamento.Equals(codDepartamento)).Count() > 0).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimoDepartamento = ultimoNombramiento.Puesto.UbicacionAdministrativa.Departamento;

                    if (ultimoDepartamento != null)
                    {
                        if (ultimoNombramiento.Puesto.UbicacionAdministrativa.Departamento == ultimoDepartamento)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;

            }
            if (codSeccion != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion.Equals(codSeccion)).Count() > 0).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimaSeccion = ultimoNombramiento.Puesto.UbicacionAdministrativa.Seccion;

                    if (ultimaSeccion != null)
                    {
                        if (ultimoNombramiento.Puesto.UbicacionAdministrativa.Seccion == ultimaSeccion)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }
            if (codPresupuesto != "")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Presupuesto != null &&
                                                    N.Puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto.Equals(codPresupuesto)).Count() > 0).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimoPresupuesto = ultimoNombramiento.Puesto.UbicacionAdministrativa.Presupuesto;

                    if (ultimoPresupuesto != null)
                    {
                        if (ultimoNombramiento.Puesto.UbicacionAdministrativa.Presupuesto == ultimoPresupuesto)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }
            if (codcostos != "")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.DesObservaciones == codcostos).Count() > 0).ToList();
                List<Funcionario> limpio = new List<Funcionario>();
                foreach (var item in resultado)
                {
                    var ultimoNombramiento = item.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige)
                                                               .FirstOrDefault();
                    var ultimaUbicacion = ultimoNombramiento.Puesto.UbicacionAdministrativa;

                    if (ultimaUbicacion != null)
                    {
                        if (ultimoNombramiento.Puesto.UbicacionAdministrativa.DesObservaciones == ultimaUbicacion.DesObservaciones)
                        {
                            limpio.Add(item);
                        }
                    }
                }
                resultado = limpio;
            }


            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();


            if (resultado.Count == resultadoTotal.Count)
            {
                resultadoTotal = new List<Funcionario>();
            }
            else
            {
                resultadoTotal = resultado;
            }


            return resultadoTotal;
        }

        public List<Funcionario> BuscarFuncionarioUbicacionPrecargado(List<Funcionario> datosPrevios, int codDivision, int codDireccion, int codDepartamento, int codSeccion, string codPresupuesto)
        {
            var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25 };


            List<Funcionario> resultadoTotal = new List<Funcionario>();


            List<Funcionario> resultado = new List<Funcionario>();




            if (datosPrevios.Count < 1)
            {
                resultado = entidadBase.Funcionario.Include("EstadoFuncionario").
                                                        Include("Nombramiento").
                                                        Include("Nombramiento.Puesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                        Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                        Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                        Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto").ToList();
            }
            else
            {
                resultado = datosPrevios;
            }


            resultadoTotal = resultado;


            if (codDivision != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division.PK_Division.Equals(codDivision)).Count() > 0).ToList();
            }
            if (codDireccion != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral.PK_DireccionGeneral.Equals(codDireccion)).Count() > 0).ToList();
            }
            if (codDepartamento != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento.PK_Departamento.Equals(codDepartamento)).Count() > 0).ToList();
            }
            if (codSeccion != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion.PK_Seccion.Equals(codSeccion)).Count() > 0).ToList();
            }
            if (codPresupuesto != "")
            {
                resultado = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => listaEstados.Contains(N.FK_EstadoNombramiento) && N.FecVence.HasValue == false).OrderByDescending(N => N.FecRige).Take(1)
                                                   .Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Presupuesto != null &&
                                                    N.Puesto.UbicacionAdministrativa.Presupuesto.IdPresupuesto.Equals(codPresupuesto)).Count() > 0).ToList();
            }


            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();


            if (resultado.Count == resultadoTotal.Count)
            {
                resultadoTotal = new List<Funcionario>();
            }
            else
            {
                resultadoTotal = resultado;
            }


            return resultadoTotal;
        }

        public List<Funcionario> BuscarExFuncionario(int mes, int anio)
        {
            List<Funcionario> resultado = new List<Funcionario>();

            resultado = entidadBase.Funcionario.Include("EstadoFuncionario").Include("Nombramiento")
                                    .Where(Q => Q.FK_EstadoFuncionario == 7 || Q.FK_EstadoFuncionario == 8 ||
                                                Q.FK_EstadoFuncionario == 9 || Q.FK_EstadoFuncionario == 10 ||
                                                Q.FK_EstadoFuncionario == 11 || Q.FK_EstadoFuncionario == 12 ||
                                                Q.FK_EstadoFuncionario == 17 || Q.FK_EstadoFuncionario == 18)
                                    .Where(Q => Q.Nombramiento.Count() > 0)
                                    .ToList();

            if (mes != 0)
            {
                resultado = resultado.Where(Q => Q.Nombramiento.LastOrDefault().FecVence.HasValue &&
                                                 Q.Nombramiento.LastOrDefault().FecVence.Value.Month == mes &&
                                                 Q.Nombramiento.LastOrDefault().FecVence.Value.Year == anio)
                                                 .ToList();

                resultado.AddRange(entidadBase.C_EMU_EXFUNCIONARIOS
                                               .Where(X => X.FECHA_CESE.Value.Month == mes && X.FECHA_CESE.Value.Year == anio)
                                               .ToList()
                                               .Select(X => new Funcionario
                                               {
                                                   IdCedulaFuncionario = X.CEDULA.TrimEnd(),
                                                   NomFuncionario = X.NOMBRE.TrimEnd(),
                                                   NomPrimerApellido = X.PRIMER_APELLIDO.TrimEnd(),
                                                   NomSegundoApellido = X.SEGUNDO_APELLIDO.TrimEnd()
                                               })
                                   );
            }
            else
            {
                resultado = resultado.Where(Q => Q.Nombramiento.LastOrDefault().FecVence.HasValue).ToList();

                resultado.AddRange(entidadBase.C_EMU_EXFUNCIONARIOS
                                                .Where(X => X.FECHA_CESE.Value.Month > mes)
                                                .ToList()
                                                .Select(X => new Funcionario
                                                {
                                                    IdCedulaFuncionario = X.CEDULA.TrimEnd(),
                                                    NomFuncionario = X.NOMBRE.TrimEnd(),
                                                    NomPrimerApellido = X.PRIMER_APELLIDO.TrimEnd(),
                                                    NomSegundoApellido = X.SEGUNDO_APELLIDO.TrimEnd()
                                                })
                                    );
            }

            return resultado;
        }


        public C_EMU_EXFUNCIONARIOS BuscarExFuncionarioCedula(string cedula)
        {
            var resultado = entidadBase.C_EMU_EXFUNCIONARIOS.Where(X => X.CEDULA == cedula).FirstOrDefault();
            return resultado;
        }


        public List<C_EMU_EXFUNCIONARIOS> BuscarExFuncionarioNombre(string nombre, string apellido1, string apellido2)
        {
            List<C_EMU_EXFUNCIONARIOS> resultado = new List<C_EMU_EXFUNCIONARIOS>();

            bool condicionNombre = nombre != "" && nombre != null;
            bool condicionApellido1 = apellido1 != "" && apellido1 != null;
            bool condicionApellido2 = apellido2 != "" && apellido2 != null;

            if (condicionNombre)
            {
                if (resultado.Count() > 0)
                    resultado = resultado.Where(q => q.NOMBRE.ToLower().Contains(nombre.ToLower())).ToList();
                else
                    resultado = entidadBase.C_EMU_EXFUNCIONARIOS.Where(q => q.NOMBRE.ToLower().Contains(nombre.ToLower())).ToList();
            }
            if (condicionApellido1)
            {
                if (resultado.Count() > 0)
                    resultado = resultado.Where(q => q.PRIMER_APELLIDO.ToLower().Contains(apellido1.ToLower())).ToList();
                else
                    resultado = entidadBase.C_EMU_EXFUNCIONARIOS.Where(q => q.PRIMER_APELLIDO.ToLower().Contains(apellido1.ToLower())).ToList();
            }
            if (condicionApellido2)
            {
                if (resultado.Count() > 0)
                    resultado = resultado.Where(q => q.SEGUNDO_APELLIDO.ToLower().Contains(apellido2.ToLower())).ToList();
                else
                    resultado = entidadBase.C_EMU_EXFUNCIONARIOS.Where(q => q.SEGUNDO_APELLIDO.ToLower().Contains(apellido2.ToLower())).ToList();
            }

            return resultado;
        }


        public List<C_EMU_EXFUNCIONARIOS> BuscarExFuncionarioOcupacionReal(string codOcupacionReal)
        {
            var resultado = entidadBase.C_EMU_EXFUNCIONARIOS.Where(X => X.OCUP_REAL == codOcupacionReal).ToList();
            return resultado;
        }

        public List<C_EMU_EXFUNCIONARIOS> BuscarExFuncionarioCodPolicial(string codPolicial)
        {
            var resultado = entidadBase.C_EMU_EXFUNCIONARIOS.Where(X => X.CODIGO_INSPECTORES.Contains(codPolicial)).ToList();
            return resultado;
        }

        private List<Funcionario> BuscarFuncionarioUbicacionContextoRestante(List<Funcionario> contextoRestante, string codDivision, string codDireccion, string codDepartamento, string codSeccion)
        {
            List<Funcionario> resultado = contextoRestante;

            if (codDivision != null && codDivision != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division != null &&
                                                    N.Puesto.UbicacionAdministrativa.Division.NomDivision.ToLower().Contains(codDivision.ToLower())).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (codDireccion != null && codDireccion != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral != null &&
                                                    N.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.ToLower().Contains(codDireccion.ToLower())).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (codDepartamento != null && codDepartamento != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento != null &&
                                                    N.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento.ToLower().Contains(codDepartamento.ToLower())).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }
            if (codSeccion != null && codSeccion != "")
            {
                var temp = resultado.Where(Q => Q.Nombramiento != null && Q.Nombramiento.Where(N => N.Puesto != null && N.Puesto.UbicacionAdministrativa != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion != null &&
                                                    N.Puesto.UbicacionAdministrativa.Seccion.NomSeccion.ToLower().Contains(codSeccion.ToLower())).Count() > 0).ToList();
                if (temp.Count > 0)
                {
                    resultado = temp;
                }
            }

            //resultado = resultado.OrderBy(Q => Q.NomPrimerApellido).ToList();

            return resultado;
        }

        public List<Funcionario> BusquedaGeneralFuncionarios(List<string> palabras)
        {
            List<Funcionario> resultado = new List<Funcionario>();
            var temp = entidadBase.Funcionario.Include("EstadoFuncionario").Include("Nombramiento").
                                                                    Include("Nombramiento.Puesto").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento").
                                                                    Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                                                    Include("Nombramiento.Puesto.DetallePuesto").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.Especialidad").
                                                                    Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal").ToList();

            var bandera = temp;

            temp = BuscarFuncionarioGeneral(temp, palabras);
            temp = BuscarFuncionarioPuestoGeneral(temp, palabras);
            temp = BuscarFuncionarioUbicacionGeneral(temp, palabras);

            if (temp.Count < bandera.Count)
            {
                resultado = temp;
            }

            return resultado;
        }

        private List<Funcionario> BuscarFuncionarioUbicacionGeneral(List<Funcionario> contextoRestante, List<string> palabras)
        {
            List<Funcionario> resultado = contextoRestante;

            foreach (var item in palabras)
            {
                resultado = BuscarFuncionarioUbicacionContextoRestante(resultado, item, "", "", "");
                resultado = BuscarFuncionarioUbicacionContextoRestante(resultado, "", item, "", "");
                resultado = BuscarFuncionarioUbicacionContextoRestante(resultado, "", "", item, "");
                resultado = BuscarFuncionarioUbicacionContextoRestante(resultado, "", "", "", item);
            }

            return resultado;
        }

        private List<Funcionario> BuscarFuncionarioPuestoGeneral(List<Funcionario> contextoRestante, List<string> palabras)
        {
            List<Funcionario> resultado = contextoRestante;

            foreach (var item in palabras)
            {
                resultado = BuscarFuncionarioDetallePuestoContextoRestante(resultado, item, "", "", "");
                resultado = BuscarFuncionarioDetallePuestoContextoRestante(resultado, "", item, "", "");
                resultado = BuscarFuncionarioDetallePuestoContextoRestante(resultado, "", "", item, "");
                resultado = BuscarFuncionarioDetallePuestoContextoRestante(resultado, "", "", "", item);
            }

            return resultado;
        }

        private List<Funcionario> BuscarFuncionarioGeneral(List<Funcionario> contextoRestante, List<string> palabras)
        {
            List<Funcionario> resultado = contextoRestante;
            foreach (var item in palabras)
            {
                resultado = BuscarFuncionarioRestante(resultado, new Funcionario { IdCedulaFuncionario = item });
                resultado = BuscarFuncionarioRestante(resultado, new Funcionario { NomFuncionario = item });
                var resultadoApellido1 = BuscarFuncionarioRestante(resultado, new Funcionario { NomPrimerApellido = item });
                var resultadoApellido2 = BuscarFuncionarioRestante(resultado, new Funcionario { NomSegundoApellido = item });
                foreach (var aux in resultadoApellido2)
                {
                    if (!(resultadoApellido1.Contains(aux)))
                    {
                        resultadoApellido1.Add(aux);
                    }
                }
                resultado = resultadoApellido1;
            }

            return resultado;
        }

        public Funcionario BuscarFuncionarioCedula(string cedula)
        {
            return entidadBase.Funcionario.Include("EstadoFuncionario").Include("Nombramiento").Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.DetallePuesto").Include("Nombramiento.Puesto.DetallePuesto.Clase").
                                            Include("Nombramiento.Puesto.DetallePuesto.Especialidad").Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion").
                                            Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral").
                                            Include("Nombramiento.Puesto.UbicacionAdministrativa.Division").
                                            Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial").
                                            Include("Nombramiento.Puesto.RelPuestoUbicacion").
                                            Include("Nombramiento.Puesto.RelPuestoUbicacion.UbicacionPuesto").
                                            Include("Nombramiento.Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito").
                                            Include("Nombramiento.Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton").
                                            Include("Nombramiento.Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia").
                                            Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial").
                                            Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto.Programa").Where(q => q.IdCedulaFuncionario == cedula).FirstOrDefault();
        }

        public Funcionario BuscarFuncionarioNombre(string nombre, string apellido1, string apellido2)
        {
            return entidadBase.Funcionario.Include("DetalleAcceso").Where(Q => Q.NomFuncionario.ToLower().Contains(nombre.ToLower())
                                                        && Q.NomPrimerApellido.ToLower().Contains(apellido1.ToLower())
                                                        && Q.NomSegundoApellido.ToLower().Contains(apellido2.ToLower())).FirstOrDefault();
        }

        public Funcionario BuscarFuncionarioPorCodigoPuesto(string codPuesto)
        {
            return entidadBase.Funcionario.Include("EstadoFuncionario")
                                          .Include("DetalleContratacion")
                                          .Include("Nombramiento")
                                          .Include("Nombramiento.EstadoNombramiento")
                                          .Include("Nombramiento.Puesto")
                                          .Where(R => R.Nombramiento.Where(N => N.Puesto.CodPuesto == codPuesto).Count() > 0).FirstOrDefault();
        }

        public Funcionario BuscarFuncionarioNombramiento(string cedula)
        {
            return entidadBase.Funcionario.Include("EstadoFuncionario")
                                         .Include("DetalleContratacion")
                                         .Include("Nombramiento")
                                         .Include("Nombramiento.EstadoNombramiento")
                                         .Include("Nombramiento.Puesto")
                                         .Where(R => R.IdCedulaFuncionario == cedula)
                                         .FirstOrDefault();

        }

        public CRespuestaDTO BuscarFuncionarioCedulaBase(string cedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Where(F => F.IdCedulaFuncionario == cedula)
                                                   .FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

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

        public CRespuestaDTO PruebaBuscarFuncionarioCedula(string cedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Where(F => F.IdCedulaFuncionario == cedula)
                                                   .FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

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

        // 21/11/2016, "GUARDAR FUNCIONARIO ESTADO CIVIL"
        public CRespuestaDTO GuardarDatosPersonalesFuncionario(Funcionario funcionario)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Funcionario.Add(funcionario);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = funcionario
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

        public CRespuestaDTO GuardarCursoGrado(CursoGrado cursoGrado)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.CursoGrado.Add(cursoGrado);
                entidadBase.SaveChanges();

                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = cursoGrado
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

        public bool ActualizarEstadoFuncionario(Funcionario func)
        {
            bool respuesta = false;
            Funcionario funcionarioActualizar = entidadBase.Funcionario
                                                    .Where(F => F.IdCedulaFuncionario == func.IdCedulaFuncionario)
                                                    .FirstOrDefault();
            funcionarioActualizar.EstadoFuncionario = func.EstadoFuncionario;
            int resultado = entidadBase.SaveChanges();
            if (resultado > 0)
            {
                respuesta = true;
            }
            return respuesta;
        }

        public CRespuestaDTO BuscarFuncionarioDesgloceSalarial(string cedula)
        {
            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.EstadoPuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Include("Nombramiento.DesgloseSalarial")
                                                   .Include("Nombramiento.DesgloseSalarial.DetalleDesgloseSalarial")
                                                   .Where(F => F.IdCedulaFuncionario == cedula)
                                                   .FirstOrDefault();
                if (resultado == null)
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }
                if (resultado.EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                {
                    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede registrar extras");
                }
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = resultado
                };

            }
            catch (Exception ex)
            {
                return new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = ex.Message }
                };
            }
        }

        public CRespuestaDTO BuscarFuncionarioDetallePuestoVacaciones(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var listaEstados = new List<int> { 1, 2, 5, 9, 13, 18, 19, 20, 21, 22, 23, 24, 25, 26 };

                respuesta.Contenido = entidadBase.Funcionario
                                                   .Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.EstadoPuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.DetallePuestoRubro")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Where(F => F.IdCedulaFuncionario == cedula).FirstOrDefault();
                //.Where(F => F.IdCedulaFuncionario == cedula
                //         && F.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige).Count() > 0)
                //&& F.Nombramiento.Where(N => N.EstadoNombramiento.PK_EstadoNombramiento == 1 || N.EstadoNombramiento.PK_EstadoNombramiento == 2
                //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 5 || N.EstadoNombramiento.PK_EstadoNombramiento == 9
                //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 13 || N.EstadoNombramiento.PK_EstadoNombramiento == 18
                //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 19 || N.EstadoNombramiento.PK_EstadoNombramiento == 20
                //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 21 || N.EstadoNombramiento.PK_EstadoNombramiento == 22
                //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 23 || N.EstadoNombramiento.PK_EstadoNombramiento == 24
                //                        ).Count() > 0)
                //&& F.Nombramiento.Where(N => N.Puesto.DetallePuesto.Where(D => (D.IndEstadoDetallePuesto ?? 1) == 1).Count() > 0).Count() > 0)
                //.Where(F => F.Nombramiento.FirstOrDefault().EstadoNombramiento.PK_EstadoNombramiento == 1)
                //.Where(F => F.Nombramiento.FirstOrDefault().Puesto.DetallePuesto.FirstOrDefault().IndEstadoDetallePuesto == 1)
                if ((Funcionario)respuesta.Contenido != null)
                {
                    //if (((Funcionario)respuesta.Contenido).EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                    //{
                    //    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede mostrarse en el módulo.");
                    //}
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }

        public CRespuestaDTO BuscarFuncionarioDetallePuesto(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                var listaEstados = new List<int> { 1, 2, 5, 9, 10, 13, 18, 19, 20, 21, 22, 23, 24, 25, 26, 28, 30, 33, 27, 35, 36, 37, 38, 39 };

                respuesta.Contenido = entidadBase.Funcionario
                                                   .Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.EstadoPuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.DetallePuestoRubro")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Where(F => F.IdCedulaFuncionario == cedula
                                                            && F.Nombramiento.Where(N => listaEstados.Contains(N.EstadoNombramiento.PK_EstadoNombramiento)).Count() > 0).FirstOrDefault();
                                                   //.Where(F => F.IdCedulaFuncionario == cedula
                                                   //         && F.Nombramiento.Where(N => (N.FecVence >= DateTime.Now || N.FecVence == null)).OrderByDescending(A => A.FecRige).Count() > 0)
                                                   //&& F.Nombramiento.Where(N => N.EstadoNombramiento.PK_EstadoNombramiento == 1 || N.EstadoNombramiento.PK_EstadoNombramiento == 2
                                                   //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 5 || N.EstadoNombramiento.PK_EstadoNombramiento == 9
                                                   //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 13 || N.EstadoNombramiento.PK_EstadoNombramiento == 18
                                                   //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 19 || N.EstadoNombramiento.PK_EstadoNombramiento == 20
                                                   //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 21 || N.EstadoNombramiento.PK_EstadoNombramiento == 22
                                                   //                          || N.EstadoNombramiento.PK_EstadoNombramiento == 23 || N.EstadoNombramiento.PK_EstadoNombramiento == 24
                                                   //                        ).Count() > 0)
                                                   //&& F.Nombramiento.Where(N => N.Puesto.DetallePuesto.Where(D => (D.IndEstadoDetallePuesto ?? 1) == 1).Count() > 0).Count() > 0)
                                                   //.Where(F => F.Nombramiento.FirstOrDefault().EstadoNombramiento.PK_EstadoNombramiento == 1)
                                                   //.Where(F => F.Nombramiento.FirstOrDefault().Puesto.DetallePuesto.FirstOrDefault().IndEstadoDetallePuesto == 1)
                if ((Funcionario)respuesta.Contenido != null)
                {
                    //if (((Funcionario)respuesta.Contenido).EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                    //{
                    //    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede mostrarse en el módulo.");
                    //}
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }

        public CRespuestaDTO BuscarFuncionarioBase(string cedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Where(F => F.IdCedulaFuncionario == cedula)
                                                   .FirstOrDefault();
                if (resultado != null)
                {
                    //if (resultado.EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo) ||
                    //    resultado.Nombramiento.Where(N => N.FecVence == null).Count() < 1)
                    //{
                    //    throw new Exception("El funcionario no se encuentra Activo, por lo que no se puede registrar.");
                    //}
                    //else
                    //{
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                    //}
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

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

        public CRespuestaDTO BuscarFuncionarioOferente(string cedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.C_EMU_EXFUNCIONARIOS
                                                   .Where(F => F.CEDULA == cedula)
                                                   .FirstOrDefault();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO
                    {
                        Codigo = -1,
                        MensajeError = error.Message
                    }
                };

                return respuesta;
            }
        }

        public CRespuestaDTO BuscarFuncionarioJornada(string cedula)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                    .Include("DetalleContratacion.TipoJornada")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Where(F => F.IdCedulaFuncionario == cedula)
                                                   .FirstOrDefault();
                if (resultado != null)
                {
                    if (resultado.EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo) ||
                        resultado.Nombramiento.Where(N => N.FecVence != null).Count() > 0)
                    {
                        throw new Exception("El funcionario no se encuentra Activo, por lo que no se puede registrar.");
                    }
                    else
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };

                        return respuesta;
                    }
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

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

        public CRespuestaDTO BuscarFuncionarioUsuario(string nombreUsuario)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                    .Include("EstadoFuncionario")
                                                    .Include("Nombramiento")
                                                    .Include("Nombramiento.DetalleNombramiento")
                                                    .Include("Nombramiento.Puesto")
                                                    .Include("Nombramiento.Puesto.DetallePuesto")
                                                    .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                    .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                    .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                    .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                    .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                    .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                    .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                    .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                    .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                    .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                    .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                    .Where(F => F.DetalleAcceso
                                                    .Where(D => D.Usuario.NomUsuario.Contains(nombreUsuario)).Count() > 0)
                                                    .FirstOrDefault();
                if (resultado != null)
                {
                    if (resultado.EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo) ||
                        resultado.Nombramiento.Where(N => N.FecVence != null).Count() > 0)
                    {
                        throw new Exception("El funcionario no se encuentra Activo, por lo que no se puede registrar.");
                    }
                    else
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };

                        return respuesta;
                    }
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

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

        public CRespuestaDTO FuncionariosConCauciones()
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Caucion")
                                                   .Include("Nombramiento.Caucion.MontoCaucion")
                                                   .Include("Nombramiento.Caucion.EntidadSeguros")
                                                   .Where(Q => Q.Nombramiento.Where(N => (N.FecVence == null || N.FecVence >= DateTime.Now) &&
                                                       N.Puesto.IndPuestoConfianza == true && N.EstadoNombramiento.PK_EstadoNombramiento != 15)
                                                       .Count() > 0).OrderBy(Q => Q.NomPrimerApellido).ToList();
                if (resultado != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron funcionarios que estén actualmente en puestos de confianza o suscritos a pólizas de caución.");
                }

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

        public CRespuestaDTO ObtenerFormacionAcademicaFuncionario(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var resultado = entidadBase.Funcionario.Include("Nombramiento")
                                                   .Include("EstadoFuncionario")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.OcupacionReal")
                                                   .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Presupuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Include("FormacionAcademica")
                                                   .Include("FormacionAcademica.CursoCapacitacion")
                                                   .Include("FormacionAcademica.CursoCapacitacion.Modalidad")
                                                   .Include("FormacionAcademica.CursoCapacitacion.EntidadEducativa")
                                                   .Include("FormacionAcademica.CursoGrado")
                                                   .Include("FormacionAcademica.CursoGrado.EntidadEducativa")
                                                   .Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();
                if (resultado != null)
                {
                    if (resultado.EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo) ||
                        resultado.Nombramiento.Where(N => N.FecVence != null).Count() > 0)
                    {
                        throw new Exception("El funcionario no se encuentra Activo, por lo que no se le puede registrar carrera profesional ni policial.");
                    }
                    else
                    {
                        respuesta = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };

                        return respuesta;
                    }
                }
                else
                {
                    throw new Exception("No se encontró ningún funcionario asociado a la cédula indicada.");
                }

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

        public CRespuestaDTO ObtenerFuncionariosPorDepartamento(CDepartamentoDTO departamento)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var datos = entidadBase.Funcionario.Include("DetalleContratacion").Where(F => F.Nombramiento
                                                   .Where(N => N.Puesto.UbicacionAdministrativa.Departamento.PK_Departamento == departamento.IdEntidad)
                                                   .Count() > 0).ToList();
                if (datos.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                    throw new Exception("No se encontró funcionarios para este departamento.");
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { Codigo = -1, MensajeError = error.Message }
                };
            }
            return respuesta;
        }

        public List<DesgloseSalarial> BuscarFuncionarioDesgloceSalarialPF(string cedula)
        {
            List<DesgloseSalarial> respuesta = new List<DesgloseSalarial>();

            try
            {
                Funcionario funcion = new Funcionario();
                funcion = entidadBase.Funcionario.Include("EstadoFuncionario")
                                                    .Where(F => F.IdCedulaFuncionario == cedula)
                                                    .FirstOrDefault();


                if (funcion.EstadoFuncionario.PK_EstadoFuncionario != Convert.ToInt32(EstadosFuncionario.Activo))
                {
                    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede registrar extras");
                }
                else
                {
                    Nombramiento respuesta2 = entidadBase.Nombramiento.Where(N => N.Funcionario.PK_Funcionario == funcion.PK_Funcionario).FirstOrDefault();
                    if (respuesta2 != null)
                    {
                        respuesta = entidadBase.DesgloseSalarial.Where(DS => DS.Nombramiento.PK_Nombramiento == respuesta2.PK_Nombramiento).ToList();
                    }
                    else
                    {
                        throw new Exception("No se encontró un desglose salarial para este funcionario");
                    }
                }

            }
            catch (Exception ex)
            {
                respuesta = new List<DesgloseSalarial>();
            }

            return respuesta;
        }

        public List<Puesto> BuscarFuncionarioPuestoPF(string cedula)
        {
            List<Puesto> respuesta = new List<Puesto>();

            try
            {
                Funcionario funcion = new Funcionario();
                funcion = entidadBase.Funcionario.Where(F => F.IdCedulaFuncionario == cedula).FirstOrDefault();


                if (funcion == null)
                {
                    throw new Exception("El funcionario no se encuentra Activo, por lo que no puede registrar extras");
                }
                else
                {
                    respuesta = entidadBase.Puesto.Where(P => P.Nombramiento.Where(N => N.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).ToList();
                    if (respuesta == null)
                    {
                        throw new Exception("El funcionario no tiene un historial de puestos");
                    }
                }

            }
            catch (Exception ex)
            {
                respuesta = new List<Puesto>();
            }

            return respuesta;
        }

        public CRespuestaDTO ListarFuncionariosActivos()
        {
            try
            {
                var datos = entidadBase.Funcionario.Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   .Where(Q => Q.FK_EstadoFuncionario == 1).ToList();

                if (datos != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("Ocurrió un error al descargar los datos de los funcionarios activos");
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

        public CRespuestaDTO ListarFuncionariosJefatura(string cedulaJefatura)
        {
            try
            {
                var datos = entidadBase.USP_LISTAR_FUNCIONARIOS_JEFATURA(cedulaJefatura).ToList();

                if (datos != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("Ocurrió un error al descargar los datos de los funcionarios activos");
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

        public CRespuestaDTO ObtenerJefaturaFuncionario(string numCedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                List<CFuncionarioDTO> listado = new List<CFuncionarioDTO>();

                var registro = entidadBase.USP_OBTENER_JEFATURA_FUNCIONARIO(numCedula,2021).ToList();

                if (registro != null)
                {
                    listado.Add(new CFuncionarioDTO
                    {
                        Cedula = registro[0].IdCedulaJefeInmediato,
                        Nombre = registro[0].NombreJefeInmediato
                    });

                    listado.Add(new CFuncionarioDTO
                    {
                        Cedula = registro[0].IdCedulaJefeSuperior,
                        Nombre = registro[0].NombreJefeSuperior
                    });

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = listado
                    };
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = "Ocurrió un error al leer los datos de la Jefatura"
                    };
                }
            }
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
            }

            return respuesta;
        }

        public CRespuestaDTO ListarFuncionariosActivos(bool guardasSeguridad, bool oficialesTransito)
        {
            try
            {
                var datos1 = entidadBase.ListaNombramientosActivos.Include("DetallePuesto.Clase").Include("Nombramiento.Funcionario").ToList();
                //var datos = entidadBase.Funcionario.Include("Nombramiento").Include("Nombramiento.Puesto").Include("Nombramiento.Puesto.DetallePuesto").Include("Nombramiento.Puesto.DetallePuesto.Clase").Where(Q => Q.FK_EstadoFuncionario == 1).ToList();
                if (guardasSeguridad)
                {
                    datos1 = datos1.Where(D => D.DetallePuesto.Clase.PK_Clase != 350 && D.DetallePuesto.Clase.PK_Clase != 351 && D.DetallePuesto.Clase.PK_Clase != 352).ToList();
                    //datos = datos.Where(F => F.Nombramiento.Where(N => N.Puesto.DetallePuesto.Where(P => P.FK_Clase != 350 && P.FK_Clase != 351 && P.FK_Clase != 352).Count() > 0).Count() > 0).ToList();
                }

                if (oficialesTransito)
                {
                    datos1 = datos1.Where(D => D.DetallePuesto.Clase.PK_Clase != 8412 && D.DetallePuesto.Clase.PK_Clase != 8414 && D.DetallePuesto.Clase.PK_Clase != 8416
                    && D.DetallePuesto.Clase.PK_Clase != 8418 && D.DetallePuesto.Clase.PK_Clase != 8420 && D.DetallePuesto.Clase.PK_Clase != 14812
                    && D.DetallePuesto.Clase.PK_Clase != 14814 && D.DetallePuesto.Clase.PK_Clase != 14816 && D.DetallePuesto.Clase.PK_Clase != 14818
                    && D.DetallePuesto.Clase.PK_Clase != 14822).ToList();
                   // datos = datos.Where(F => F.FK_EstadoFuncionario == 1 && F.Nombramiento.Where(N => N.Puesto.DetallePuesto.Where(P => P.FK_Clase != 8412 && P.FK_Clase != 8414 && P.FK_Clase != 8416 && P.FK_Clase != 8418
                   //&& P.FK_Clase != 8420 && P.FK_Clase != 14812 && P.FK_Clase != 14814 && P.FK_Clase != 14816 && P.FK_Clase != 14818 && P.FK_Clase != 8303 && P.FK_Clase != 8308
                   //&& P.FK_Clase != 8313 && P.FK_Clase != 8318 && P.FK_Clase != 8325 && P.FK_Clase != 8330 && P.FK_Clase != 8335 && P.FK_Clase != 8340 && P.FK_Clase != 8345
                   //&& P.FK_Clase != 8350 && P.FK_Clase != 8360 && P.FK_Clase != 8365 && P.FK_Clase != 8370 && P.FK_Clase != 8375 && P.FK_Clase != 8380 && P.FK_Clase != 8390
                   //&& P.FK_Clase != 8395 && P.FK_Clase != 8400 && P.FK_Clase != 8405 && P.FK_Clase != 8410 && P.FK_Clase != 8412 && P.FK_Clase != 8414 && P.FK_Clase != 8416
                   //&& P.FK_Clase != 8418 && P.FK_Clase != 8318 && P.FK_Clase != 8325 && P.FK_Clase != 8330 && P.FK_Clase != 8420 && P.FK_Clase != 14765 && P.FK_Clase != 14770
                   //&& P.FK_Clase != 14775 && P.FK_Clase != 14780 && P.FK_Clase != 14785 && P.FK_Clase != 14790 && P.FK_Clase != 14795
                   //&& P.FK_Clase != 14800 && P.FK_Clase != 14805 && P.FK_Clase != 14810 && P.FK_Clase != 14812 && P.FK_Clase != 14814
                   //&& P.FK_Clase != 14815 && P.FK_Clase != 14816 && P.FK_Clase != 14818 && P.FK_Clase != 14820).Count() > 0).Count() > 0).ToList();
                }
                if (datos1 != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos1
                    };
                }
                else
                {
                    throw new Exception("Ocurrió un error al descargar los datos de los funcionarios activos.");
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

        public List<C_EMU_EXFUNCIONARIOS> ListarExFuncionariosTotal()
        {
            // Comentario
            return entidadBase.C_EMU_EXFUNCIONARIOS.ToList();
        }

        public CRespuestaDTO ListarFuncionariosTotal()
        {
            try
            {
                var datos = entidadBase.Funcionario.Include("DetalleContratacion")
                                                   .Include("Nombramiento")
                                                   .Include("Nombramiento.Puesto")
                                                   .Include("Nombramiento.Puesto.DetallePuesto")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                   .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                                   //.Where(Q => Q.FK_EstadoFuncionario == 1)
                                                   .Where(Q => Q.FK_EstadoFuncionario != 20 && Q.FK_EstadoFuncionario != 21)
                                                   //20	Oferente registrado, 21  Oferente con informacion completa
                                                   .ToList();

                if (datos != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                }
                else
                {
                    throw new Exception("Ocurrió un error al descargar los datos de los funcionarios activos");
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

        public CRespuestaDTO ActualizarInformacionContacto(CInformacionContactoDTO dato, string cedula)
        {
            try
            {
                var informacion = entidadBase.InformacionContacto.FirstOrDefault(I => I.Funcionario.IdCedulaFuncionario == cedula
                                                                                        && I.FK_TipoContacto == dato.TipoContacto.IdEntidad);

                if (informacion != null)
                {
                    informacion.DesContenido = dato.DesContenido;
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = informacion.PK_InformacionContacto
                    };
                }
                else
                {
                    entidadBase.InformacionContacto.Add
                        (
                            new InformacionContacto
                            {
                                FK_TipoContacto = dato.TipoContacto.IdEntidad,
                                DesContenido = dato.DesContenido,
                                FK_Funcionario = entidadBase.Funcionario.FirstOrDefault(F => F.IdCedulaFuncionario == cedula).PK_Funcionario
                            }
                        );

                    int respuesta = entidadBase.SaveChanges();

                    if (respuesta > 0)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = respuesta
                        };
                    }
                    else
                    {
                        throw new Exception("No se pudo insertar la información de contacto");
                    }

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

        public CRespuestaDTO ActualizarEstadoCivil(HistorialEstadoCivil dato)
        {
            try
            {
                var anterior = entidadBase.HistorialEstadoCivil.FirstOrDefault(H => H.FK_Funcionario == dato.FK_Funcionario);
                anterior.FecFin = dato.FecInicio.AddDays(-1);
                entidadBase.HistorialEstadoCivil.Add(dato);
                entidadBase.SaveChanges();
                return new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = dato.PK_HistorialEstadoCivil
                };
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

        public CRespuestaDTO ActualizarDireccionDomicilio(CDireccionDTO dato, string cedula)
        {
            try
            {
                var direccion = entidadBase.Direccion.FirstOrDefault(D => D.Funcionario.IdCedulaFuncionario == cedula);

                if (direccion != null)
                {
                    direccion.DirExacta = dato.DirExacta;
                    direccion.FK_Distrito = dato.Distrito.IdEntidad;
                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = direccion.PK_Direccion
                    };
                }
                else
                {
                    throw new Exception("No se encontró la información de contacto a editar");
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

        public CRespuestaDTO BuscarInformacionBasicaFuncionario(string cedula)
        {
            try
            {
                var funcionario = entidadBase.Funcionario.Include("InformacionContacto").Include("HistorialEstadoCivil")
                                             .Include("Direccion").FirstOrDefault(F => F.IdCedulaFuncionario == cedula);

                if (funcionario != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = funcionario
                    };
                }
                else
                {
                    throw new Exception("No se encontró el funcionario con la cédula indicada");
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

        public List<C_EMU_EXFUNCIONARIOS> BuscarExfuncionarioFiltros(string cedula, string nombre, string primerApellido, string segundoApellido, string codPuesto)
        {
            List<C_EMU_EXFUNCIONARIOS> resultadoTotal = new List<C_EMU_EXFUNCIONARIOS>();
            try
            {
                var resultado = entidadBase.C_EMU_EXFUNCIONARIOS.ToList();
                resultadoTotal = resultado;

                if (resultado == null)
                {
                    throw new Exception();
                }
                else
                {
                    if (cedula != "")
                    {
                        resultado = resultado.Where(E => E.CEDULA != null && E.CEDULA.Trim().ToUpper().Equals(cedula.Trim().ToUpper())).ToList();
                    }
                    if (nombre != "")
                    {
                        resultado = resultado.Where(E => E.NOMBRE != null && E.NOMBRE.Trim().ToUpper().Contains(nombre.Trim().ToUpper())).ToList();
                    }
                    if (primerApellido != "")
                    {
                        resultado = resultado.Where(E => E.PRIMER_APELLIDO != null && E.PRIMER_APELLIDO.Trim().ToUpper().Contains(primerApellido.Trim().ToUpper())).ToList();
                    }
                    if (segundoApellido != "")
                    {
                        resultado = resultado.Where(E => E.SEGUNDO_APELLIDO != null && E.SEGUNDO_APELLIDO.Trim().ToUpper().Contains(segundoApellido.Trim().ToUpper())).ToList();
                    }
                    if (codPuesto != "")
                    {
                        resultado = resultado.Where(E => E.PUESTO_PROPIEDAD != null && E.PUESTO_PROPIEDAD.Trim().ToUpper().Contains(codPuesto.Trim().ToUpper())).ToList();
                    }

                    if (resultado.Count == resultadoTotal.Count)
                    {
                        resultadoTotal = new List<C_EMU_EXFUNCIONARIOS>();
                    }
                    else
                    {
                        resultadoTotal = resultado;
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoTotal = new List<C_EMU_EXFUNCIONARIOS>();
            }
            return resultadoTotal;
        }

        public List<Puesto> BuscarFuncionarioPuestoDetallePuesto(string codPuesto, int codClase, int codEspecialidad, int codOcupacionReal, string estadoPuesto, string confianza, int codNivel)
        {

            List<Puesto> resultadoTotal = new List<Puesto>();

            var resultado = entidadBase.Puesto.Include("EstadoPuesto")
                                                .Include("UbicacionAdministrativa.Division")
                                                .Include("UbicacionAdministrativa.DireccionGeneral")
                                                .Include("UbicacionAdministrativa.Departamento")
                                                .Include("UbicacionAdministrativa.Seccion")
                                                .Include("DetallePuesto")
                                                .Include("DetallePuesto.Clase")
                                                .Include("DetallePuesto.Especialidad")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Funcionario").ToList();
            resultadoTotal = resultado;

            if (codPuesto != null && codPuesto != "")
            {
                resultado = resultado.Where(P => P.CodPuesto.TrimEnd(' ').Equals(codPuesto)).ToList();
            }

            if (confianza != null && confianza != "")
            {
                if (confianza == "De confianza")
                {
                    resultado = resultado.Where(P => P.IndPuestoConfianza == true).ToList();
                }
                else if (confianza == "No de confianza")
                {
                    resultado = resultado.Where(P => P.IndPuestoConfianza != true).ToList();
                }
            }
            if (codNivel != 0)
            {
                resultado = resultado.Where(P => P.IndNivelOcupacional == codNivel).ToList();
            }

            if (estadoPuesto != null && estadoPuesto != "")
            {
                switch (estadoPuesto)
                {
                    case "Propiedad":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto == 2).ToList();
                        break;
                    case "Interino":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto == 3 || P.FK_EstadoPuesto == 9).ToList();
                        break;
                    case "Ascenso Interino":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto == 9).ToList();
                        break;
                    case "Puestos con nombramiento a plazo fijo":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 18 || N.FK_EstadoNombramiento == 19) && N.FecVence >= DateTime.Now).Count() > 0).ToList();
                        break;
                    case "Traslado Horizonal en Propiedad":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 24) && N.FecVence == null).Count() > 0).ToList();
                        break;
                    case "Traslado Interinstitucional Horizontal Interino":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 26) && N.FecVence >= DateTime.Now).Count() > 0).ToList();
                        break;
                    case "Traslado Interinstitucional Ascenso Interino":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 28) && N.FecVence >= DateTime.Now).Count() > 0).ToList();
                        break;
                    case "Traslado Interinstitucional Ascenso en Propiedad":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 30) && N.FecVence == null).Count() > 0).ToList();
                        break;
                    case "Traslado Interinstitucional Descenso en Propiedad":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 33) && N.FecVence == null).Count() > 0).ToList();
                        break;
                    case "Traslado Interinstitucional Descenso Interino":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 35) && N.FecVence >= DateTime.Now).Count() > 0).ToList();
                        break;
                    case "Traslado Horizontal Interino dentro del MOPT":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 36) && N.FecVence >= DateTime.Now).Count() > 0).ToList();
                        break;
                    case "Traslado Horizontal en Propiedad dentro del MOPT":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.FK_EstadoNombramiento == 37) && N.FecVence == null).Count() > 0).ToList();
                        break;
                    case "Permiso sin Salario":
                        resultado = resultado.Where(P => P.Nombramiento.Where(N => (N.Funcionario.FK_EstadoFuncionario == 2) && N.FecVence == null).Count() > 0).ToList();
                        break;
                    //case "Vacante":
                    //    resultado = resultado.Where(P => P.FK_EstadoPuesto == 4 || P.FK_EstadoPuesto == 5 || P.FK_EstadoPuesto == 6 || P.FK_EstadoPuesto == 7 || P.FK_EstadoPuesto == 8 || P.FK_EstadoPuesto == 10 || P.FK_EstadoPuesto == 14
                    //    || P.FK_EstadoPuesto == 19 || P.FK_EstadoPuesto == 20 || P.FK_EstadoPuesto == 22 || P.FK_EstadoPuesto == 24).ToList();
                    //    break;
                    case "Vacantes con interino y propietario":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto.Equals(3) || P.FK_EstadoPuesto.Equals(9)).ToList();
                        var interinocon = entidadBase.Nombramiento.Where(N => ((N.FecVence == null) || (N.FecVence >= DateTime.Now &&
                            (N.FK_EstadoNombramiento == 6 || N.FK_EstadoNombramiento == 7 || N.FK_EstadoNombramiento == 8
                            || N.FK_EstadoNombramiento == 10))
                            && N.FK_EstadoNombramiento != 15)).ToList();
                        resultado = (from l1 in resultado join l2 in interinocon on l1.PK_Puesto equals l2.FK_Puesto select l1).ToList();
                        break;
                    case "Vacantes con interino sin propietario":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto.Equals(3) || P.FK_EstadoPuesto.Equals(9)).ToList();
                        var interinosin = entidadBase.Nombramiento.Where(N => ((N.FecVence == null) || (N.FecVence >= DateTime.Now &&
                            (N.FK_EstadoNombramiento == 6 || N.FK_EstadoNombramiento == 7 || N.FK_EstadoNombramiento == 8
                            || N.FK_EstadoNombramiento == 10))
                            && N.FK_EstadoNombramiento != 15)).ToList();
                        resultado = (from l1 in resultado join l2 in interinosin on l1.PK_Puesto equals l2.FK_Puesto into g where !g.Any() select l1).ToList();
                        break;
                    case "Vacantes con propietario sin interino":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto.Equals(5) || P.FK_EstadoPuesto.Equals(6) || P.FK_EstadoPuesto.Equals(7)
                                                    || P.FK_EstadoPuesto.Equals(8) || P.FK_EstadoPuesto.Equals(10) || P.FK_EstadoPuesto.Equals(14)
                                                    || P.FK_EstadoPuesto.Equals(16) || P.FK_EstadoPuesto.Equals(19) || P.FK_EstadoPuesto.Equals(20)
                                                    || P.FK_EstadoPuesto.Equals(25)
                                                    || P.FK_EstadoPuesto.Equals(26)).ToList();
                        /*var nombramientospropsin = entidadBase.Nombramiento.Where(N => (N.FecVence == null) || (N.FecVence >= DateTime.Now && 
                                                    (N.FK_EstadoNombramiento == 6 || N.FK_EstadoNombramiento == 7 || N.FK_EstadoNombramiento == 8
                                                    || N.FK_EstadoNombramiento == 10)) 
                                                    && N.FK_EstadoNombramiento != 15).ToList();
                        resultado = (from l1 in resultado join l2 in nombramientospropsin on l1.PK_Puesto equals l2.FK_Puesto select l1).ToList();*/
                        var nombramientosConPropInterno = entidadBase.Nombramiento.Where(N => (N.FecVence == null) || (N.FecVence >= DateTime.Now &&
                                                        (N.FK_EstadoNombramiento == 6 || N.FK_EstadoNombramiento == 7 || N.FK_EstadoNombramiento == 8
                                                        || N.FK_EstadoNombramiento == 10)) 
                                                        && N.FK_EstadoNombramiento != 15).ToList();
                        resultado = (from l1 in resultado join l2 in nombramientosConPropInterno on l1.PK_Puesto equals l2.FK_Puesto select l1).ToList();
                        break;
                    case "Vacantes sin interino ni propietario":
                        //5,6,7,8,10,14,16,19,20,22,24,25,26
                        resultado = resultado.Where(P => P.FK_EstadoPuesto.Equals(5) || P.FK_EstadoPuesto.Equals(6) || P.FK_EstadoPuesto.Equals(7)
                                                    || P.FK_EstadoPuesto.Equals(8) || P.FK_EstadoPuesto.Equals(10) || P.FK_EstadoPuesto.Equals(14)
                                                    || P.FK_EstadoPuesto.Equals(16) || P.FK_EstadoPuesto.Equals(19) || P.FK_EstadoPuesto.Equals(20)
                                                    || P.FK_EstadoPuesto.Equals(22) || P.FK_EstadoPuesto.Equals(24) || P.FK_EstadoPuesto.Equals(25)
                                                    || P.FK_EstadoPuesto.Equals(26)).ToList();
                        var nombramientosProp = entidadBase.Nombramiento.Where(N => N.FecVence == null && N.FK_EstadoNombramiento != 15).ToList();
                        var nombramientosInt = entidadBase.Nombramiento.Where(N => N.FecVence >= DateTime.Now && N.FK_EstadoNombramiento != 15).ToList();
                        resultado = (from l1 in resultado join l2 in nombramientosProp on l1.PK_Puesto equals l2.FK_Puesto into g where !g.Any() select l1).ToList();
                        resultado = (from l1 in resultado join l2 in nombramientosInt on l1.PK_Puesto equals l2.FK_Puesto into g where !g.Any() select l1).ToList();
                        //resultado = resultado.Where((P, i) => P.PK_Puesto != nombramientosProp[i].FK_Puesto).ToList();
                        break;
                    case "Relacion de puestos completos":
                        resultado = resultado.Where(P => P.FK_EstadoPuesto != 23).ToList();
                        //resultado = resultado.Where(P => P.FK_EstadoPuesto == 2 || P.FK_EstadoPuesto == 3 || P.FK_EstadoPuesto == 4 || P.FK_EstadoPuesto == 5 ||
                        //P.FK_EstadoPuesto == 6 || P.FK_EstadoPuesto == 7 || P.FK_EstadoPuesto == 8 || P.FK_EstadoPuesto == 9 || P.FK_EstadoPuesto == 10 || P.FK_EstadoPuesto == 14
                        //|| P.FK_EstadoPuesto == 19 || P.FK_EstadoPuesto == 20 || P.FK_EstadoPuesto == 22 || P.FK_EstadoPuesto == 24
                        //|| P.FK_EstadoPuesto == 25 || P.FK_EstadoPuesto == 26).ToList();
                        break;
                }
            }

            if (codClase != 0)
            {
                resultado = resultado.Where(P => P.DetallePuesto != null && P.DetallePuesto.Where(D => D.Clase != null &&
                                                    (D.IndEstadoDetallePuesto ?? 1) == 1 &&
                                                    D.Clase.PK_Clase.Equals(codClase)).Count() > 0).ToList();
            }
            if (codEspecialidad != 0)
            {
                resultado = resultado.Where(P => P.DetallePuesto != null && P.DetallePuesto.Where(D => D.Especialidad != null &&
                                                    (D.IndEstadoDetallePuesto ?? 1) == 1 &&
                                                    D.Especialidad.PK_Especialidad.Equals(codEspecialidad)).Count() > 0).ToList();
            }
            if (codOcupacionReal != 0)
            {
                resultado = resultado.Where(P => P.DetallePuesto != null && P.DetallePuesto.Where(D => D.OcupacionReal != null &&
                                                    (D.IndEstadoDetallePuesto ?? 1) == 1 &&
                                                    D.OcupacionReal.PK_OcupacionReal.Equals(codOcupacionReal)).Count() > 0).ToList();
            }

            if (resultado.Count == resultadoTotal.Count && confianza != "Todos")
            {
                resultadoTotal = new List<Puesto>();
            }
            else
            {
                resultadoTotal = resultado;
            }

            return resultadoTotal;
        }

        public CRespuestaDTO CargarCalificacionActual(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();

            try
            {
                respuesta.Codigo = 1;
                respuesta.Contenido = entidadBase.Nombramiento.Include("Funcionario")
                                                   .Include("CalificacionNombramiento")
                                                   .Include("CalificacionNombramiento.Calificacion")
                                                   .Where(X => (X.FecVence == null || X.FecVence > DateTime.Now) && X.Funcionario.IdCedulaFuncionario == cedula)
                                                   .OrderByDescending(X => X.FecRige)
                                                   .FirstOrDefault();

                if ((Nombramiento)respuesta.Contenido == null)
                    throw new Exception("No se encontró ninguna calificación");
            }
            catch (Exception ex)
            {
                respuesta = new CRespuestaDTO();
                respuesta.Codigo = -1;
                respuesta.Contenido = new CErrorDTO { MensajeError = ex.Message };
            }

            return respuesta;
        }
        #endregion
    }
}