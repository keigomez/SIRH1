using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
   public class CEspecialidadD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CEspecialidadD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion

        #region Metodos
   
        //Guarda las Especialidades en la BD                
        public int GuardarEspecialidad(Especialidad Especialidad)
        {
            entidadBase.Especialidad.Add(Especialidad);
            return Especialidad.PK_Especialidad;
        }

        public CRespuestaDTO GuardarEspecialidad(String codPuesto, Especialidad especialidad)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.Especialidad.Add(especialidad);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = especialidad
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
   

      
        // Obtiene la carga de las Especialidades de la BD
      
       public Especialidad CargarEspecialidadPorID(int idEspecialidad)
        {
            Especialidad resultado = new Especialidad();

            resultado = entidadBase.Especialidad.Where(R => R.PK_Especialidad == idEspecialidad).FirstOrDefault();

            return resultado;
        }

       //POR CÉDULA
       public Especialidad CargarEspecialidadCedula(string cedula)
       {

           Especialidad resultado = new Especialidad();
           //Por Cédula
           resultado = entidadBase.Especialidad.Where(R => 
                                                        R.DetallePuesto.Where(Q => 
                                                                                Q.Puesto.Nombramiento.Where(K => 
                                                                                                                K.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).Count() > 0).FirstOrDefault();

           return resultado;
       }
            //PARAMETRO
         public Especialidad CargarEspecialidadParam(string DescripcionEspecialidad)
         {
             Especialidad resultado = new Especialidad();

             resultado = entidadBase.Especialidad.Where(R => R.DesEspecialidad.ToLower().Contains(DescripcionEspecialidad.ToLower())).FirstOrDefault();

             return resultado;
         }

       /// <summary>
       /// Enlista las Especialidades de la BD
       /// </summary>
       /// <param name="codEspecialidad"></param>
       /// <param name="nomEspecialidad"></param>
         /// <returns>Retorna las Especialidades</returns>
         public List<Especialidad> CargarEspecialidadesParams(int codEspecialidad, string nomEspecialidad)
         {
             List<Especialidad> resultado = entidadBase.Especialidad.ToList();

             if (codEspecialidad != 0 && codEspecialidad != null)
             {
                 resultado = resultado.Where(Q => Q.PK_Especialidad == codEspecialidad).ToList();
             }
             if (nomEspecialidad != "" && nomEspecialidad != null)
             {
                 resultado = resultado.Where(Q => Q.DesEspecialidad.ToLower().Contains(nomEspecialidad.ToLower())).ToList();
             }

             return resultado;
         }

        #endregion

    }
}