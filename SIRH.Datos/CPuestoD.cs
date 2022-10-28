using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using SIRH.Datos;
using SIRH.DTO;
using System.Data.Entity.Infrastructure;

//Cambio para subirle a Orlando.
namespace SIRH.Datos
{
    public class CPuestoD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CPuestoD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos

        public int GuardarPuesto(Puesto registro)
        {
            if (entidadBase.Puesto.Where(Q => Q.PK_Puesto == registro.PK_Puesto).Count() < 1)
            {
                entidadBase.Puesto.Add(registro);
            }
            return entidadBase.SaveChanges();
        }

        public CRespuestaDTO GuardarPuestoP(Puesto puesto)
        {            
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                entidadBase.Puesto.Add(puesto);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = puesto
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
        /// Obtiene la lista de los Puestos de la BD
        /// </summary>
        /// <returns>Retorna una lista de Puestos</returns>
        
        public List<Puesto> CargarPuestos()
        {
            List<Puesto> resultados = new List<Puesto>();

            resultados = entidadBase.Puesto.ToList();

            return resultados;
        }

        private DbQuery<Puesto> RetornarDetallePuesto()
        {
            return entidadBase.Puesto.Include("EstadoPuesto").Include("DetallePuesto");
        }

        private DbQuery<Puesto> RetornarSubespecialidad()
        {
            return RetornarDetallePuesto().Include("DetallePuesto.SubEspecialidad");
        }

        private DbQuery<Puesto> RetornarEspecialidad()
        {
            return RetornarSubespecialidad().Include("DetallePuesto.Especialidad");
        }

        private DbQuery<Puesto> RetornarOcupacionReal()
        {
            return RetornarEspecialidad().Include("DetallePuesto.OcupacionReal");
        }

        private DbQuery<Puesto> RetornarClase()
        {
            return RetornarOcupacionReal().Include("DetallePuesto.Clase").Include("Nombramiento")
                                            .Include("Nombramiento.EstadoNombramiento")
                                            .Include("Nombramiento.Funcionario")
                                            .Include("Nombramiento.Funcionario.EstadoFuncionario");
        }

        /// <summary>
        /// Obtiene la carga de los puestos de la BD
        /// </summary>
        /// <returns>Retorna los puestos</returns>
        public Puesto CargarPuestoActivo(string cedula)
        {
            Puesto resultado = new Puesto();

            resultado = RetornarClase().Where(Q =>
                                                   Q.Nombramiento.Where(K =>
                                                                          K.Funcionario.IdCedulaFuncionario == cedula 
                                                                          && (K.EstadoNombramiento.PK_EstadoNombramiento == 1 
                                                                          || K.EstadoNombramiento.PK_EstadoNombramiento == 2
                                                                          || K.EstadoNombramiento.PK_EstadoNombramiento == 5
                                                                          || K.EstadoNombramiento.PK_EstadoNombramiento == 9)).Count() > 0).FirstOrDefault();
            return resultado;
        }

        public Puesto DescargarPuesto(string cedula)
        {
            Puesto resultado = new Puesto();

            resultado = RetornarClase().Where(Q =>
                                                   Q.Nombramiento.Where(K =>
                                                                          K.Funcionario.IdCedulaFuncionario == cedula).Count() > 0).FirstOrDefault();
            return resultado;
        }

        public Puesto DescargarPuestoPropiedad(string cedula)
        {
            Puesto resultado = new Puesto();

            resultado = RetornarClase().Where(Q =>
                                                   Q.Nombramiento.Where(K =>
                                                                          K.Funcionario.IdCedulaFuncionario == cedula && K.FecVence == null).Count() > 0).FirstOrDefault();
            return resultado;
        }

        public Puesto DescargarPuestoCodigo(string codigoPuesto)
        {
            Puesto resultado = new Puesto();

            resultado = RetornarClase().Where(Q =>
                                                   Q.CodPuesto == codigoPuesto).FirstOrDefault();
            return resultado;
        }

        //Por Parametro
        public Puesto CargarPuestoParam(string CodigoDePuesto)
        {
            Puesto resultado = new Puesto();

            DbQuery<Puesto> temp = entidadBase.Puesto.Include("UbicacionAdministrativa").Include("UbicacionAdministrativa.Presupuesto");

            temp = temp.Include("EstadoPuesto");

            resultado = temp.Where(R => R.CodPuesto.Contains(CodigoDePuesto)).FirstOrDefault();

            return resultado;
        }

        public List<Puesto> CargarPuestosDetalle(string codigopuesto, int clase, int especialidad, int ocupacion)
        {
            List<Puesto> resultado = new List<Puesto>();

            var puesto = entidadBase.Puesto.Include("EstadoPuesto").Include("UbicacionAdministrativa.Presupuesto").Include("DetallePuesto").
                                            Include("DetallePuesto.Clase").Include("DetallePuesto.Especialidad").
                                            Include("DetallePuesto.OcupacionReal").ToList();
            puesto = puesto.Where(Q => Q.EstadoPuesto.PK_EstadoPuesto == 22).ToList();
            if (codigopuesto != "")
            {
                puesto = puesto.Where(Q => Q.CodPuesto == codigopuesto).ToList();
            }
            if (clase != 0)
            {
                puesto = puesto.Where(Q => Q.DetallePuesto.LastOrDefault().Clase.PK_Clase == clase).ToList();
            }
            if (especialidad != 0)
            {
                puesto = puesto.Where(Q => Q.DetallePuesto.LastOrDefault().Especialidad.PK_Especialidad == especialidad).ToList();
            }
            if (ocupacion != 0)
            {
                puesto = puesto.Where(Q => Q.DetallePuesto.LastOrDefault().OcupacionReal.PK_OcupacionReal == ocupacion).ToList();
            }

            foreach (var item in puesto)
            {
                resultado.Add(item);
            }

            return resultado;
        }

        public Puesto DetallePuestoPorCodigo(string codPuesto)
        {
            Puesto respuesta = new Puesto();

            respuesta = entidadBase.Puesto.Include("EstadoPuesto")
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
                                            .Where(Q => Q.CodPuesto == codPuesto).FirstOrDefault();

            return respuesta;
        }

        public Puesto DetallePuestoCedula(string cedula)
        {
            Puesto respuesta = new Puesto();

            respuesta = entidadBase.Puesto.Include("EstadoPuesto")
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
                                            .Include("UbicacionAdministrativa.Presupuesto.Programa")
                                            .Include("RelPuestoUbicacion")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.TipoUbicacion")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton")
                                            .Include("RelPuestoUbicacion.UbicacionPuesto.Distrito.Canton.Provincia")
                                            .Where(Q => Q.Nombramiento
                                            .Where(R => R.Funcionario.IdCedulaFuncionario == cedula && R.Puesto.PK_Puesto == Q.PK_Puesto).Count() > 0)
                                            .FirstOrDefault();

            return respuesta;
        }

        public bool ActualizarEstadoPuesto(Puesto puesto)
        {
            bool respuesta = false;

            Puesto puestoAct = entidadBase.Puesto.Include("EstadoPuesto").Where(P => P.CodPuesto == puesto.CodPuesto)
                                                    .FirstOrDefault();

            if (puestoAct.FK_EstadoPuesto == puesto.FK_EstadoPuesto)
            {
                respuesta = true;
            }
            else
            {
                puestoAct.FK_EstadoPuesto = puesto.FK_EstadoPuesto;
                if (entidadBase.SaveChanges() > 0)
                {
                    respuesta = true;
                }
            }

            return respuesta;
        }

        public CRespuestaDTO DescargarPuestoVacante(string codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.Puesto.Include("EstadoPuesto")
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
                                            .Where(Q => Q.CodPuesto == codigo && Q.EstadoPuesto.DesEstadoPuesto.ToLower().StartsWith("vac"))
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
                    throw new Exception("El puesto ingresado no existe o bien no se encuentra vacante, por favor verifique los datos suministrados");
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

        public CRespuestaDTO DescargarPuestoCompleto(string codigo)
        {
            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.Puesto.Include("EstadoPuesto")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Funcionario")
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
                                            .Where(Q => Q.CodPuesto == codigo)
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
                    throw new Exception("El puesto ingresado no existe, por favor verifique los datos suministrados");
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

        public CRespuestaDTO DescargarPuestoPedimento(string pedimento)
        {
            CRespuestaDTO respuesta;
            try
            {
                var resultado = entidadBase.Puesto.Include("EstadoPuesto")
                                            .Include("PedimentoPuesto")
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
                                            .Where(Q => Q.PedimentoPuesto.Where(P => P.NumPedimento == pedimento).Count() > 0)
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
                    throw new Exception("El número de pedimento ingresado no existe, por favor verifique los datos suministrados");
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
        public CRespuestaDTO DescargarPerfilPuestoAcciones(string codPuesto)
        {
            try
            {
                var resultado = entidadBase.Puesto
                         .Include("Nombramiento")
                         .Include("Nombramiento.EstadoNombramiento")
                         .Include("Nombramiento.Funcionario")
                         .Include("Nombramiento.Funcionario.EstadoFuncionario")
                         .Include("Nombramiento.Funcionario.DetalleContratacion")
                         .Include("EstudioPuesto")
                         .Include("PedimentoPuesto")
                         .Include("PrestamoPuesto")
                         .Include("EstadoPuesto")
                         .Include("DetallePuesto")
                         .Include("DetallePuesto.ContenidoPresupuestario")
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
                         .Where(N => N.CodPuesto == codPuesto)
                         .ToList();
                //FECHA LIMITE DE DÍAS PARA CERRAR EL NOMBRAMIENTO
                DateTime fechaLimite = DateTime.Now.AddDays(-5);
                var datoNombramiento = entidadBase.Nombramiento.Where(N => (N.FecVence >= fechaLimite
                            || N.FecVence == null) && N.Puesto.CodPuesto == codPuesto && N.FK_EstadoNombramiento != 6 && N.FK_EstadoNombramiento != 7 && N.FK_EstadoNombramiento != 8
                            && N.FK_EstadoNombramiento != 10 && N.FK_EstadoNombramiento != 15)
                            .OrderByDescending(A => A.FecRige).FirstOrDefault();

                if (datoNombramiento != null)
                {
                    int pkNombramiento = datoNombramiento.PK_Nombramiento;
                    resultado = resultado.Where(Q => Q.Nombramiento.Where(N => N.PK_Nombramiento == pkNombramiento).Count() > 0).ToList();
                }


                if (resultado != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado.FirstOrDefault()
                    };
                }
                else
                {
                    throw new Exception("No se encontró el puesto con el número suministrado");
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

        public CRespuestaDTO DescargarPerfilPuestoAccionesFuncionario(string cedula)
        {
            try
            {
                //FECHA LIMITE DE DÍAS PARA CERRAR EL NOMBRAMIENTO
                DateTime fechaLimite = DateTime.Now.AddDays(-5);
                var nombramiento = entidadBase.Nombramiento.Where(N => (N.FecVence >= fechaLimite
                        || N.FecVence == null) && N.Funcionario.IdCedulaFuncionario == cedula && N.FK_EstadoNombramiento != 6 && N.FK_EstadoNombramiento != 7 && N.FK_EstadoNombramiento != 8
                        && N.FK_EstadoNombramiento != 10 && N.FK_EstadoNombramiento != 15)
                        .OrderByDescending(A => A.FecRige).FirstOrDefault();

                if (nombramiento.Funcionario.FK_EstadoFuncionario != 1)
                {
                    throw new Exception("El funcionario no se encuentra activo. Si es un exfuncionario no es posible que libere ningún puesto y si es un funcionario que se encontraba trasladado o prestado a otra institución o dependencia debe realizarse primero un regreso al trabajo antes de que se libere el puesto.");
                }

                var resultado = new Puesto();

                if (nombramiento != null)
                {
                    int pkNombramiento = nombramiento.PK_Nombramiento;

                    resultado = entidadBase.Puesto
                                .Include("Nombramiento")
                                .Include("Nombramiento.Funcionario.ExpedienteFuncionario")
                                .Include("Nombramiento.EstadoNombramiento")
                                .Include("Nombramiento.Funcionario")
                                .Include("Nombramiento.Funcionario.EstadoFuncionario")
                                .Include("Nombramiento.Funcionario.DetalleContratacion")
                                .Include("Nombramiento.Funcionario.HistorialEstadoCivil")
                                .Include("Nombramiento.Funcionario.HistorialEstadoCivil.CatEstadoCivil")
                                .Include("Nombramiento.Funcionario.InformacionContacto")
                                .Include("Nombramiento.Funcionario.InformacionContacto.TipoContacto")
                                .Include("Nombramiento.Funcionario.Direccion")
                                .Include("Nombramiento.Funcionario.Direccion.Distrito")
                                .Include("Nombramiento.Funcionario.Direccion.Distrito.Canton")
                                .Include("Nombramiento.Funcionario.Direccion.Distrito.Canton.Provincia")
                                .Include("EstudioPuesto")
                                .Include("PedimentoPuesto")
                                .Include("PrestamoPuesto")
                                .Include("EstadoPuesto")
                                .Include("DetallePuesto")
                                .Include("Nombramiento.CalificacionNombramiento")
                                .Include("Nombramiento.CalificacionNombramiento.Calificacion")
                                .Include("Nombramiento.CalificacionNombramiento.PeriodoCalificacion")
                                .Include("DetallePuesto.ContenidoPresupuestario")
                                .Include("DetallePuesto.Clase")
                                .Include("DetallePuesto.Especialidad")
                                .Include("DetallePuesto.OcupacionReal")
                                .Include("DetallePuesto.SubEspecialidad")
                                .Include("DetallePuesto.EscalaSalarial")
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
                                .Where(Q => Q.Nombramiento.Where(N => N.PK_Nombramiento == pkNombramiento).Count() > 0)
                                .FirstOrDefault();
                }
                else
                {
                    throw new Exception("El funcionario no está ocupando ningún puesto actualmente por lo que no se le puede realizar ningún movimiento.");
                }

                if (resultado != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("El funcionario indicado no ha ocupado ningún puesto en el Ministerio.");
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

        public CRespuestaDTO AsignarPuestoConfianza(string codPuesto) {
            try
            {
                var resultado = entidadBase.Puesto
                            .Where(Q => Q.CodPuesto == codPuesto)
                            .FirstOrDefault();

                if (resultado != null)
                {

                    resultado.IndPuestoConfianza = true;
                    entidadBase.SaveChanges();

                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se encontró el puesto con el número suministrado");
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

        public CRespuestaDTO ActualizarObservacionesPuestoCaucion(string codPuesto, string observaciones)
        {
            try
            {
                var resultado = entidadBase.Puesto.Where(Q => Q.CodPuesto == codPuesto).FirstOrDefault();
                if (resultado != null)
                {
                    if (resultado.ObsPuesto != null)
                    {
                        if (resultado.ObsPuesto.TrimEnd() != observaciones.TrimEnd())
                        {
                            resultado.ObsPuesto = observaciones;
                            if (resultado.IndPuestoConfianza == true)
                                resultado.IndPuestoConfianza = false;
                            else
                                resultado.IndPuestoConfianza = true;

                            entidadBase.SaveChanges();
                            return new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = resultado.PK_Puesto
                            };
                        }
                        else
                        {
                            throw new Exception("Las observaciones del puesto no fueron modificadas");
                        }
                    }
                    else
                    {
                        resultado.ObsPuesto = observaciones;
                        if (resultado.IndPuestoConfianza == true)
                            resultado.IndPuestoConfianza = false;
                        else
                            resultado.IndPuestoConfianza = true;

                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado.PK_Puesto
                        };
                    }
                }
                else
                {
                    throw new Exception("No se encontró el número de puesto indicado");
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

        public CRespuestaDTO DescargarPerfilNombramientoAccionesFuncionario(string cedula)
        {
            try
            {
                var resultado = entidadBase.Nombramiento
                            .Include("EstadoNombramiento")
                            .Include("Funcionario")
                            .Include("Funcionario.ExpedienteFuncionario")
                            .Include("Funcionario.EstadoFuncionario")
                            .Include("Funcionario.DetalleContratacion")
                            .Include("Funcionario.HistorialEstadoCivil")
                            .Include("Funcionario.HistorialEstadoCivil.CatEstadoCivil")
                            .Include("Funcionario.InformacionContacto")
                            .Include("Funcionario.InformacionContacto.TipoContacto")
                            .Include("Funcionario.Direccion")
                            .Include("Funcionario.Direccion.Distrito")
                            .Include("Funcionario.Direccion.Distrito.Canton")
                            .Include("Funcionario.Direccion.Distrito.Canton.Provincia")
                            .Include("Puesto")
                            .Include("Puesto.EstudioPuesto")
                            .Include("Puesto.PedimentoPuesto")
                            .Include("Puesto.PrestamoPuesto")
                            .Include("Puesto.EstadoPuesto")
                            .Include("Puesto.DetallePuesto")
                            .Include("CalificacionNombramiento")
                            .Include("CalificacionNombramiento.Calificacion")
                            .Include("CalificacionNombramiento.PeriodoCalificacion")
                            .Include("Puesto.DetallePuesto.ContenidoPresupuestario")
                            .Include("Puesto.DetallePuesto.Clase")
                            .Include("Puesto.DetallePuesto.Especialidad")
                            .Include("Puesto.DetallePuesto.OcupacionReal")
                            .Include("Puesto.DetallePuesto.SubEspecialidad")
                            .Include("Puesto.DetallePuesto.EscalaSalarial")
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
                            .Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula && (Q.FecVence >= DateTime.Now || Q.FecVence == null)).OrderByDescending(P =>P.FecRige).ThenByDescending(O => O.FecNombramiento).FirstOrDefault();
                            
                if (resultado != null)
                {
                    return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };
                }
                else
                {
                    resultado = entidadBase.Nombramiento
                            .Include("EstadoNombramiento")
                            .Include("Funcionario")
                            .Include("Funcionario.ExpedienteFuncionario")
                            .Include("Funcionario.EstadoFuncionario")
                            .Include("Funcionario.DetalleContratacion")
                            .Include("Funcionario.HistorialEstadoCivil")
                            .Include("Funcionario.HistorialEstadoCivil.CatEstadoCivil")
                            .Include("Funcionario.InformacionContacto")
                            .Include("Funcionario.InformacionContacto.TipoContacto")
                            .Include("Funcionario.Direccion")
                            .Include("Funcionario.Direccion.Distrito")
                            .Include("Funcionario.Direccion.Distrito.Canton")
                            .Include("Funcionario.Direccion.Distrito.Canton.Provincia")
                            .Include("Puesto")
                            .Include("Puesto.EstudioPuesto")
                            .Include("Puesto.PedimentoPuesto")
                            .Include("Puesto.PrestamoPuesto")
                            .Include("Puesto.EstadoPuesto")
                            .Include("Puesto.DetallePuesto")
                            .Include("CalificacionNombramiento")
                            .Include("CalificacionNombramiento.Calificacion")
                            .Include("CalificacionNombramiento.PeriodoCalificacion")
                            .Include("Puesto.DetallePuesto.ContenidoPresupuestario")
                            .Include("Puesto.DetallePuesto.Clase")
                            .Include("Puesto.DetallePuesto.Especialidad")
                            .Include("Puesto.DetallePuesto.OcupacionReal")
                            .Include("Puesto.DetallePuesto.SubEspecialidad")
                            .Include("Puesto.DetallePuesto.EscalaSalarial")
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
                            .Where(Q => Q.Funcionario.IdCedulaFuncionario == cedula).OrderByDescending(O => O.FecRige).FirstOrDefault();
                    if (resultado != null)
                    {
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };
                    }
                    else
                    {
                        throw new Exception("El funcionario indicado no ha ocupado ningún puesto en el Ministerio.");
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

        public List<CRespuestaDTO> DescargarPuestoOcupantes(string codPuesto)
        {
            List<CRespuestaDTO> respuesta = new List<CRespuestaDTO>();
            try
            {
                var resultado = entidadBase.Puesto
                            .Include("Nombramiento")
                            .Include("Nombramiento.EstadoNombramiento")
                            .Include("Nombramiento.Funcionario")
                            .Include("Nombramiento.Funcionario.EstadoFuncionario")
                            .Include("Nombramiento.Funcionario.DetalleContratacion")
                            .Include("EstudioPuesto")
                            .Include("PedimentoPuesto")
                            .Include("PrestamoPuesto")
                            .Include("EstadoPuesto")
                            .Include("DetallePuesto")
                            .Include("DetallePuesto.ContenidoPresupuestario")
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
                            .Where(Q => Q.CodPuesto == codPuesto)
                            .FirstOrDefault();

                if (resultado != null)
                {
                    respuesta.Add(new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    });
                    var ocupante = entidadBase.Nombramiento.Include("Funcionario")
                                                                    .Include("EstadoNombramiento")
                                                                    .Include("Puesto")
                                                                    .Where(N => N.Puesto.CodPuesto == codPuesto && (N.FecVence >= DateTime.Now || N.FecVence == null))
                                                                    .OrderByDescending(T => T.FecRige).FirstOrDefault();
                    if (ocupante == null) // Ocupa el puesto actualmente
                    {
                        respuesta.Add(new CRespuestaDTO
                        {
                            Codigo = -1,
                            Contenido = new CErrorDTO { MensajeError = "No se encontro al ocupante del puesto" }
                        });
                        respuesta.Add(new CRespuestaDTO
                        {
                            Codigo = -1,
                            Contenido = new CErrorDTO { MensajeError = "No se encontro al propietario del puesto" }
                        });
                    }
                    else
                    {
                        if (ocupante.FecVence == null || ocupante.EstadoNombramiento.PK_EstadoNombramiento == 1) // Propietario
                        { 
                            // Busca si hay otro funcionario nombrado en ese puesto con fechas válidas.
                            var otroOcupante = entidadBase.Nombramiento.Include("Funcionario")
                                                                  .Include("EstadoNombramiento")
                                                                  .Include("Puesto")
                                                                  .Where(N => N.Puesto.CodPuesto == codPuesto && N.Funcionario.PK_Funcionario != ocupante.Funcionario.PK_Funcionario && (N.FecVence >= DateTime.Now || N.FecVence == null))
                                                                  .OrderByDescending(T => T.FecRige).FirstOrDefault();

                            respuesta.Add(new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = otroOcupante == null ? ocupante : otroOcupante
                            });
                            respuesta.Add(new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = ocupante
                            });
                        }
                        else
                        {
                            respuesta.Add(new CRespuestaDTO
                            {
                                Codigo = 1,
                                Contenido = ocupante
                            });

                            var propietario = entidadBase.Nombramiento.Include("Funcionario")
                                                           .Include("EstadoNombramiento")
                                                           .Include("Puesto")
                                                            .Where(N => N.Puesto.CodPuesto == codPuesto && N.FecVence == null)
                                                            .OrderByDescending(T => T.FecRige).FirstOrDefault();
                            if (propietario != null) // Propietario del puesto
                            {
                                respuesta.Add(new CRespuestaDTO
                                {
                                    Codigo = 1,
                                    Contenido = propietario
                                });
                            }
                            else
                            {
                                //1   Propiedad
                                //26  Trasl.Interinst.Horizontal Interino -interno
                                //28  Trasl.Interinstitucional Asc. Interino - interno
                                //35  Trasl.Interinstitucional Desc. Interino - interno
                                //36  Traslado horizontal interino dentro del MOPT
                                //var listaEstadoPropiedad = new List<int> { 1, 26, 28, 35, 36 };
                                var listaEstadoPropiedad = new List<int> { 6, 7, 8, 10 };

                                propietario = entidadBase.Nombramiento.Include("Funcionario")
                                                           .Include("EstadoNombramiento")
                                                           .Include("Puesto")
                                                           .Where(N => N.Puesto.CodPuesto == codPuesto
                                                                //&& N.FecVence >= DateTime.Today
                                                                && listaEstadoPropiedad.Contains(N.EstadoNombramiento.PK_EstadoNombramiento) && (N.FecVence == null || N.FecVence >= DateTime.Now))
                                                           .OrderByDescending(T => T.FecRige).FirstOrDefault();

                                if (propietario != null) // Propietario del puesto
                                {
                                    respuesta.Add(new CRespuestaDTO
                                    {
                                        Codigo = 1,
                                        Contenido = propietario
                                    });
                                }
                                else
                                {
                                    respuesta.Add(new CRespuestaDTO
                                    {
                                        Codigo = -1,
                                        Contenido = new CErrorDTO { MensajeError = "No se encontró el propietario del puesto" }
                                    });
                                }
                            }
                        }
                    }
                    //Verificar si el estado de mapache es en propiedad.

                    // No busco el anterior

                    //Si el mapache es interino, busca el anterior en propiedad
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontró el puesto con el número suministrado");
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                });
                return respuesta;
            }
        }

        public CRespuestaDTO DatosUbicacionDetallePuesto(string codpuesto)
        {
            try
            {
                var resultado = entidadBase.Puesto.Include("UbicacionAdministrativa")
                                                  .Include("UbicacionAdministrativa.Division")
                                                  .Include("UbicacionAdministrativa.DireccionGeneral")
                                                  .Include("UbicacionAdministrativa.Departamento")
                                                  .Include("UbicacionAdministrativa.Seccion")
                                                  .Include("DetallePuesto")
                                                  .Include("DetallePuesto.Clase")
                                                  .Include("DetallePuesto.Especialidad")
                                                  .Include("DetallePuesto.Subespecialidad")
                                                  .Include("DetallePuesto.OcupacionReal")
                                                  .Include("DetallePuesto.EscalaSalarial")
                                                  .Include("DetallePuesto.EscalaSalarial.PeriodoEscalaSalarial")
                                                  .Include("EstadoPuesto")
                                                  .Include("Nombramiento")
                                                  .Include("Nombramiento.Funcionario")
                                                  .FirstOrDefault(P => P.CodPuesto == codpuesto);
                if (resultado != null)
                {
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };
                }
                else
                {
                    throw new Exception("No se encontró el puesto con el código específicado");
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

        public CRespuestaDTO ActualizarUbicacionAdministrativa(string codPuesto, UbicacionAdministrativa ubicacionAdmin)
        {
            try
            {
                var resultado = entidadBase.Puesto.Where(Q => Q.CodPuesto == codPuesto).FirstOrDefault();
                if (resultado != null)
                {
                    if (ubicacionAdmin.PK_UbicacionAdministrativa != 0)
                    {
                        resultado.UbicacionAdministrativa = ubicacionAdmin;

                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };
                    }
                    else
                    {
                        entidadBase.UbicacionAdministrativa.Add(ubicacionAdmin);

                        resultado.UbicacionAdministrativa = ubicacionAdmin;

                        entidadBase.SaveChanges();
                        return new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = resultado
                        };

                    }

                }
                else
                {
                    throw new Exception("No se pudo actualizar la ubicación administrativa");
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

        public CRespuestaDTO ActualizarNivelOcupacional(string codPuesto, int nivelOcupacional)
        {
            try
            {
                var resultado = entidadBase.Puesto.Where(Q => Q.CodPuesto == codPuesto).FirstOrDefault();
                if (resultado != null)
                {

                    resultado.IndNivelOcupacional = nivelOcupacional;

                    entidadBase.SaveChanges();
                    return new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = resultado
                    };

                }
                else
                {
                    throw new Exception("No se pudo actualizar la ubicación administrativa");
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