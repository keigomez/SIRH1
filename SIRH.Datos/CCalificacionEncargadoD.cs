using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CCalificacionEncargadoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion
        
        #region Constructor

        public CCalificacionEncargadoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion
        
        #region Métodos
                
        public CRespuestaDTO GuardarEncargado(CalificacionEncargado Encargado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var encargadoOld = entidadBase.CalificacionEncargado
                                                .Where(Q => Q.FK_Funcionario == Encargado.Funcionario.PK_Funcionario
                                                        && Q.FK_Seccion == Encargado.Seccion.PK_Seccion)
                                                .FirstOrDefault();


                if (encargadoOld == null)
                    entidadBase.CalificacionEncargado.Add(Encargado);
                else
                {
                    encargadoOld.IndEstado = 1;
                    Encargado.PK_CalificacionEncargado = encargadoOld.PK_CalificacionEncargado;
                }

                entidadBase.SaveChanges();
               
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = Encargado.PK_CalificacionEncargado
                };
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

        public CRespuestaDTO ActualizarEncargado(CalificacionEncargado Encargado)
        {
            CRespuestaDTO respuesta;
            try
            {
                var encargadoOld = entidadBase.CalificacionEncargado
                                                .Where(Q => Q.FK_Funcionario == Encargado.FK_Funcionario
                                                        && Q.FK_Seccion == Encargado.FK_Seccion)
                                                .FirstOrDefault();

                if (encargadoOld != null)
                { 
                    encargadoOld.IndEstado = Encargado.IndEstado;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = Encargado.PK_CalificacionEncargado
                    };
                }
                else
                {
                    throw new Exception("No se encontró registro para actualizar");
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

        public CRespuestaDTO BuscarEncargado(List<CalificacionEncargado> datosPrevios, object parametro, string elemento)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatos(elemento, datosPrevios, parametro);
                if (datosPrevios.Count > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datosPrevios.Where(Q => Q.IndEstado == 1).ToList()
                    };
                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontraron resultados para los parámetros de búsqueda establecidos" }
                    };
                    return respuesta;
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


        private List<CalificacionEncargado> CargarDatos(string elemento, List<CalificacionEncargado> datosPrevios, object parametro)
        {
            string param = "";
            int paramInt = 0;
          
            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString().ToUpper();
            }
            if (parametro.GetType().Name.Equals("Int32"))
            {
                paramInt = Convert.ToInt32(parametro);
            }
            

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "Cedula": //Cedula

                        datosPrevios = entidadBase.CalificacionEncargado
                                                    .Include("Funcionario")
                                                    .Include("Seccion")
                                                    .Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param) && C.IndEstado == 1).ToList();
                        break;


                    case "Division":
                        if (paramInt != 0)
                        {
                            var datosSeccion = entidadBase.UbicacionAdministrativa
                                                     .Where(C => C.Division.PK_Division == paramInt)
                                                     .Select(x => x.FK_Seccion);

                            datosPrevios = entidadBase.CalificacionEncargado
                                                       .Include("Funcionario")
                                                       .Include("Seccion")
                                                       .Where(C => datosSeccion.Contains(C.Seccion.PK_Seccion) && C.IndEstado == 1).ToList();
                        }
                           
                        break;

                    case "Direccion":
                        if (paramInt != 0)
                        {
                            var datosSeccion = entidadBase.UbicacionAdministrativa
                                                     .Where(C => C.DireccionGeneral.PK_DireccionGeneral == paramInt)
                                                     .Select(x => x.FK_Seccion);

                            datosPrevios = entidadBase.CalificacionEncargado
                                                       .Include("Funcionario")
                                                       .Include("Seccion")
                                                       .Where(C => datosSeccion.Contains(C.Seccion.PK_Seccion) && C.IndEstado == 1).ToList();
                        }

                        break;

                    case "Departamento":
                        if (paramInt != 0)
                        {
                            var datosSeccion = entidadBase.UbicacionAdministrativa
                                                     .Where(C => C.Departamento.PK_Departamento == paramInt)
                                                     .Select(x => x.FK_Seccion);

                            datosPrevios = entidadBase.CalificacionEncargado
                                                       .Include("Funcionario")
                                                       .Include("Seccion")
                                                       .Where(C => datosSeccion.Contains(C.Seccion.PK_Seccion) && C.IndEstado == 1).ToList();
                        }

                        break;

                    case "Seccion":
                        if (paramInt != 0)
                            datosPrevios = entidadBase.CalificacionEncargado
                                                    .Include("Funcionario")
                                                    .Include("Seccion")
                                                    .Where(C => C.Seccion.PK_Seccion == paramInt && C.IndEstado == 1).ToList();
                        break;

                    default:
                        datosPrevios = new List<CalificacionEncargado>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Cedula":
                        datosPrevios = datosPrevios.Where(C => C.Funcionario.IdCedulaFuncionario.Contains(param) && C.IndEstado == 1).ToList();
                        break;

                    case "Division":
                        if (paramInt != 0)
                        {
                            var datosSeccion = entidadBase.UbicacionAdministrativa
                                                     .Where(C => C.Division.PK_Division == paramInt)
                                                     .Select(x => x.FK_Seccion);

                            datosPrevios = datosPrevios.Where(C => datosSeccion.Contains(C.Seccion.PK_Seccion) && C.IndEstado == 1).ToList();
                        }

                        break;

                    case "Direccion":
                        if (paramInt != 0)
                        {
                            var datosSeccion = entidadBase.UbicacionAdministrativa
                                                     .Where(C => C.DireccionGeneral.PK_DireccionGeneral == paramInt)
                                                     .Select(x => x.FK_Seccion);

                            datosPrevios = datosPrevios.Where(C => datosSeccion.Contains(C.Seccion.PK_Seccion) && C.IndEstado == 1).ToList();
                        }

                        break;

                    case "Departamento":
                        if (paramInt != 0)
                        {
                            var datosSeccion = entidadBase.UbicacionAdministrativa
                                                     .Where(C => C.Departamento.PK_Departamento == paramInt)
                                                     .Select(x => x.FK_Seccion);

                            datosPrevios = datosPrevios.Where(C => datosSeccion.Contains(C.Seccion.PK_Seccion) && C.IndEstado == 1).ToList();
                        }

                        break;

                    case "Seccion":
                        if (paramInt != 0)
                            datosPrevios = datosPrevios.Where(C => C.Seccion.PK_Seccion == paramInt && C.IndEstado == 1).ToList();
                        break;

                   

                    default:
                        datosPrevios = new List<CalificacionEncargado>();
                        break;
                }
            }

            return datosPrevios;
        }


        #endregion
    }
}