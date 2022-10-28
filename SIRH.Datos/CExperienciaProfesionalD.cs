using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CExperienciaProfesionalD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion


        #region Constructor

        public CExperienciaProfesionalD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }
        #endregion


        #region Metodos

        public CRespuestaDTO GuardarExperienciaProfesional(string cedula, ExperienciaProfesional experienciaProfesional)
        {
            CRespuestaDTO respuesta;
            try
            {
                //var significa que almacena cualquier tipo de dato. 
                //traer al funcionario de la entidadBase(BD), al que se le va a asignar la experiencia profesional.
                var funcionario = entidadBase.Funcionario.Include("ExperienciaProfesional").Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();
                //si el funcionario es nulo, o sea que no se encuentra registrado
                if (funcionario != null)
                {
                    //si la formacion académica no existe para el funcionario
                    if (funcionario.FormacionAcademica.Count < 1)
                    {
                        //entonces se va a registrar una nueva formacion academica,(por que no hay registros) en temp de Formacion Académica
                        FormacionAcademica temp = new FormacionAcademica
                            {
                                //se registra en la fecha actual
                                FecRegistro = DateTime.Now,
                            };
                        //en la nueva formacion academica registrada,se va a experiencia prof y se agrega la experiencia prof 
                        temp.ExperienciaProfesional.Add(experienciaProfesional);
                        //al funcionario en la formacion academica se le inserta la experiencia profesional.
                        funcionario.FormacionAcademica.Add(temp);
                        //guardar los cambios en la BD
                        entidadBase.SaveChanges();
                    }
                        
                    else
                    {
                        //Al funcionario qe contiene formacion academica, se le agregue la experiencia profesional y se guarda en la entidad base, se guardan los cambios.
                        funcionario.FormacionAcademica.FirstOrDefault().ExperienciaProfesional.Add(experienciaProfesional);
                        entidadBase.SaveChanges();
                    }

                    respuesta = new CRespuestaDTO
                    {
                        // el uno significa que no hubieron  errores.
                        Codigo = 1,
                        //que le devuelva el contenido de funcionario a lógica
                        Contenido = funcionario
                    };
                    //regresarme un respuesta
                    return respuesta;
                }
                else
                {    //da un error si no se encontro el num de ced, que de este mensaje
                    throw new Exception("No se encontró el funcionario con la cédula: " + cedula);
                }
            }
                //aqui cae el error 
            catch(Exception error)
            {
                respuesta = new CRespuestaDTO
                {  
                    //el -1 signigica que existen errores
                    Codigo = -1,
                    //devuelve a logica el error
                    Contenido = new CErrorDTO { Mensaje = error.Message }
                };
                return respuesta;
            }
        }
                                                        //cédula que digita el usuario 
        public CRespuestaDTO BuscarExperienciaProfesionalCedula(string cedula)
        {   //A CrespuestaDTO la nombramos como respuesta ¿para qué?
            CRespuestaDTO respuesta;
            try
            {
                // traer al funcionario de la entidadBase(BD), al que se está buscando por medio de la cedula, buscando por medio de las tablas que llevan a la exp. prof.
                var funcionario = entidadBase.Funcionario
                                                .Include("FormacionAcademica")
                                                .Include("FormacionAcademica.ExperienciaProfesional")
                                                .Include("Nombramiento")
                                                .Include("Nombramiento.Puesto")
                                                .Include("Nombramiento.Puesto.DetallePuesto")
                                                .Include("Nombramiento.Puesto.DetallePuesto.Clase")
                                                .Include("Nombramiento.Puesto.DetallePuesto.Especialidad")
                                                .Include("Nombramiento.Puesto.UbicacionAdministrativa.Division")
                                                .Include("Nombramiento.Puesto.UbicacionAdministrativa.DireccionGeneral")
                                                .Include("Nombramiento.Puesto.UbicacionAdministrativa.Departamento")
                                                .Include("Nombramiento.Puesto.UbicacionAdministrativa.Seccion")
                                              //donde Q es funcionario (en este caso)y busca en funionario la cedula que digita el usuario, y que se traiga el primer dato
                                             .Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();
                //si el funcionario es diferente de nulo, o sea que si tiene datos
                if (funcionario != null)
                {
                    //si el funcionario tiene formacion academica, y la experiencia profesional es mayor a 0, o sea que tiene datos. 
                    if (funcionario.FormacionAcademica.Count > 0)
                    {
                        if (funcionario.FormacionAcademica.FirstOrDefault().ExperienciaProfesional.Count > 0)
                        {  //la respuesta será una nueva respuesta en DTO
                            respuesta = new CRespuestaDTO
                            {
                                // el uno significa que no hubieron  errores.
                                Codigo = 1,
                                //que le devuelva el contenido de funcionario a la capa de lógica
                                Contenido = funcionario
                            };
                            //recibo la respuesta
                            return respuesta;
                        }
                        else
                        {// da un error si no se experiencia profesional
                            throw new Exception("No se encontró Experiencia Profesional asociada al funcionario con la cédula: " + cedula);
                        }
                    }
                    else
                    {// da un error si no se experiencia profesional
                        throw new Exception("No se encontró Experiencia Profesional asociada al funcionario con la cédula: " + cedula);
                    }
                }
                else
                {// da un error para el segundo if, o sea la segunda pregunta, si no se encuentra la cedula, 
                    throw new Exception("No se encontró el Funcionario asociado a la cédula: " + cedula);
                }
            } //aqui cae el error, Exception es el error, y error es el mj de error
            catch (Exception error)
            {
                respuesta = new CRespuestaDTO
                {  //el -1 signigica que existen errores
                    Codigo = -1,
                    //devuelve a logica el error
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                //me regresa respuesta de todo lo anterior
                return respuesta;
            }
        }

        #endregion
    }
}


