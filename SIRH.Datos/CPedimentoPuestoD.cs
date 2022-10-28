using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CPedimentoPuestoD
    {
        #region Variables

        SIRHEntities contexto = new SIRHEntities();

        #endregion

        #region Constructor

        public CPedimentoPuestoD(SIRHEntities contextoGlobal)
        {
            contexto = contextoGlobal;
        }   
     
        #endregion

        #region Metodos

        public CRespuestaDTO BuscarPedimentosPorPuesto(string codPuesto)
        {
            CRespuestaDTO respuesta;

            try
            {
                var puesto = contexto.Puesto.Include("PedimentoPuesto")
                                            .Include("EstadoPuesto")
                                            .Include("DetallePuesto")
                                            .Include("DetallePuesto.Clase")
                                            .Include("DetallePuesto.Especialidad")
                                            .Include("DetallePuesto.OcupacionReal")
                                            .Include("DetallePuesto.SubEspecialidad")
                                            .Include("UbicacionAdministrativa")
                                            .Include("UbicacionAdministrativa.Division")
                                            .Include("UbicacionAdministrativa.DireccionGeneral")
                                            .Include("UbicacionAdministrativa.Departamento")
                                            .Include("UbicacionAdministrativa.Seccion")
                                            .Include("UbicacionAdministrativa.Presupuesto")
                                            .Include("RelPuestoUbicacion")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia")
                                            .Where(P => P.CodPuesto == codPuesto).FirstOrDefault();

                if (puesto.PedimentoPuesto.Count() > 0)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = puesto
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se han registrado pedimentos para este puesto");
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

        public CRespuestaDTO BuscarPedimentoCodigo(int codPedimento)
        {
            CRespuestaDTO respuesta;

            try
            {
                var resultado = contexto.PedimentoPuesto.Include("Puesto")
                                            .Include("Puesto.EstadoPuesto")
                                            .Include("Puesto.DetallePuesto")
                                            .Include("Puesto.DetallePuesto.Clase")
                                            .Include("Puesto.DetallePuesto.Especialidad")
                                            .Include("Puesto.DetallePuesto.OcupacionReal")
                                            .Include("Puesto.DetallePuesto.SubEspecialidad")
                                            .Include("Puesto.UbicacionAdministrativa")
                                            .Include("Puesto.UbicacionAdministrativa.Division")
                                            .Include("Puesto.UbicacionAdministrativa.DireccionGeneral")
                                            .Include("Puesto.UbicacionAdministrativa.Departamento")
                                            .Include("Puesto.UbicacionAdministrativa.Seccion")
                                            .Include("Puesto.UbicacionAdministrativa.Presupuesto")
                                            .Include("Puesto.RelPuestoUbicacion")
                                            .Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                            .Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                            .Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton")
                                            .Include("Puesto.RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia")
                                            .Where(P => P.PK_PedimentoPuesto == codPedimento).FirstOrDefault();

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
                    throw new Exception("No se han registrado pedimentos para este puesto");
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

        public CRespuestaDTO GuardarPedimentoPuesto(string codPuesto, PedimentoPuesto pedimentoPuesto)
        {
            CRespuestaDTO respuesta;
            try
            {   
                // en el contexto en puesto, que me incluya a pedimento de puesto, donde en Puesto me trae codigo puesto que solicito.
                var puesto = contexto.Puesto.Include("PedimentoPuesto").Where(P => P.CodPuesto == codPuesto).FirstOrDefault();

                //si puesto no es null
                if (puesto != null)
                {
                    //en el puesto, si el numero de resolución del estudio de puesto es null o menor que uno, signigica que puesto esta en estudio.
                    if (puesto.EstudioPuesto.Where(E => E.NumResolucion == null).Count() < 1)
                    {
                        //en la tabla puesto pasa a la tabla pedimento de puesto y agregue el pedimento de puesto
                        puesto.PedimentoPuesto.Add(pedimentoPuesto);
                        contexto.SaveChanges();

                        respuesta = new CRespuestaDTO
                        {
                            IdEntidad = 1,
                            Contenido = pedimentoPuesto.PK_PedimentoPuesto
                        };
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception("Este puesto actualmente está bajo estudio.");
                    }
                }
                else
                {
                    throw new Exception("No se encontró el puesto asociado al número suministrado.");
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

        public CRespuestaDTO BuscarPedimentos(List<PedimentoPuesto> datosPrevios, object parametro, string elemento)
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
                        Contenido = datosPrevios
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

        private List<PedimentoPuesto> CargarDatos(string elemento, List<PedimentoPuesto> datosPrevios, object parametro)
        {
            string param = "";
            int iparam = 0;
            DateTime paramFechaInicio = new DateTime();
            DateTime paramFechaFinal = new DateTime();

            if (parametro.GetType().Name.Equals("String"))
            {
                param = parametro.ToString();
            }
            else
            {
                if (parametro.GetType() == typeof(int))
                {
                    iparam = Convert.ToInt32(parametro);
                }
                else
                {
                    paramFechaInicio = ((List<DateTime>)parametro).ElementAt(0);
                    paramFechaFinal = ((List<DateTime>)parametro).ElementAt(1);
                }
            }

            if (datosPrevios.Count < 1)
            {
                switch (elemento)
                {
                    case "Puesto":
                        datosPrevios = contexto.PedimentoPuesto
                                                    .Include("Puesto")
                                                    .Include("Puesto.DetallePuesto")
                                                    .Include("Puesto.UbicacionAdministrativa")
                                                    .Where(C => C.Puesto.CodPuesto
                                                        == param).ToList();
                        break;
                    case "FechaPedimento":
                        datosPrevios = contexto.PedimentoPuesto
                                                    .Include("Puesto")
                                                    .Include("Puesto.DetallePuesto")
                                                    .Include("Puesto.UbicacionAdministrativa")
                                                    .Where(C => C.FecPedimento >= paramFechaInicio &&
                                                        C.FecPedimento <= paramFechaFinal)
                                                    .ToList();
                        break;
                    case "NumeroPedimento":
                        datosPrevios = contexto.PedimentoPuesto
                                                    .Include("Puesto")
                                                    .Include("Puesto.DetallePuesto")
                                                    .Include("Puesto.UbicacionAdministrativa")
                                                    .Where(C => C.NumPedimento == param).ToList();
                        break;
                    default:
                        datosPrevios = new List<PedimentoPuesto>();
                        break;
                }
            }
            else
            {
                switch (elemento)
                {
                    case "Puesto":
                        datosPrevios = datosPrevios.Where(C => C.Puesto.CodPuesto == param).ToList();
                        break;
                    case "FechaPedimento":
                        datosPrevios = datosPrevios.Where(C => C.FecPedimento >= paramFechaInicio &&
                                                        C.FecPedimento <= paramFechaFinal).ToList();
                        break;
                    case "NumeroPedimento":
                        datosPrevios = datosPrevios.Where(C => C.NumPedimento == param).ToList();
                        break;
                    default:
                        datosPrevios = new List<PedimentoPuesto>();
                        break;
                }
            }

            return datosPrevios;
        }

        #endregion
    }
}
