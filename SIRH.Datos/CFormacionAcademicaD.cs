using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;

namespace SIRH.Datos
{
    public class CFormacionAcademicaD
    {
        #region Variables

        private SIRHEntities entidadBase = new SIRHEntities();

        #endregion

        #region Constructor

        public CFormacionAcademicaD(SIRHEntities entidadGlobal)
        {
            entidadBase = entidadGlobal;
        }

        #endregion

        #region Metodos
        
        public int GuardarFormacionAcademica(FormacionAcademica formacionAcademica)
        {
            entidadBase.FormacionAcademica.Add(formacionAcademica);
            return formacionAcademica.PK_FormacionAcademica;
        }

        public CRespuestaDTO GuardarFormacionAcademicaPorCedula(string cedula, FormacionAcademica formacion)
        {
            CRespuestaDTO respuesta;
            try
            {
                entidadBase.FormacionAcademica.Add(formacion);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = formacion
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

        #region Buscar

        public CRespuestaDTO BuscarCursoCapacitacionPorCodigo(int codCurso)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.CursoCapacitacion
                                            .Include("FormacionAcademica")
                                            .Include("FormacionAcademica.Funcionario")
                                            .Include("FormacionAcademica.Funcionario.DetalleContratacion")
                                            .Include("EntidadEducativa")
                                            .Include("Modalidad")
                                            .Where(Q => Q.PK_CursoCapacitacion == codCurso)
                                            .FirstOrDefault();
                if (datos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron resultados.");
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

        public CRespuestaDTO BuscarCursoGradoPorCodigo(int codCurso)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.CursoGrado
                                            .Include("FormacionAcademica")
                                            .Include("FormacionAcademica.Funcionario")
                                            .Include("FormacionAcademica.Funcionario.DetalleContratacion")
                                            .Include("EntidadEducativa")
                                            .Where(Q => Q.PK_CursoGrado == codCurso)
                                            .FirstOrDefault();
                if (datos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron resultados.");
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

        public CRespuestaDTO BuscarDatosCarreraPorCedula(string cedula)
        {
            CRespuestaDTO respuesta;
            try
            {
                var datos = entidadBase.Funcionario
                                            .Include("DetalleContratacion")
                                            .Include("Nombramiento")
                                            .Include("Nombramiento.Puesto")
                                            .Include("Nombramiento.Puesto.DetallePuesto")
                                            .Include("FormacionAcademica")
                                            .Include("FormacionAcademica.CursoGrado")
                                            .Include("FormacionAcademica.CursoGrado.EntidadEducativa")
                                            .Include("FormacionAcademica.CursoCapacitacion")
                                            .Include("FormacionAcademica.CursoCapacitacion.EntidadEducativa")
                                            .Include("FormacionAcademica.CursoCapacitacion.Modalidad")
                                            .Include("FormacionAcademica.DetallePuntosAdicional")
                                            .Where(Q => Q.IdCedulaFuncionario == cedula)
                                            .FirstOrDefault();
                if (datos != null)
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = datos
                    };
                    return respuesta;
                }
                else
                {
                    throw new Exception("No se encontraron resultados.");
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

        //Lista de Formacion Academica, buscar por tipo de curso, con un parámetro de tipo string
        public CRespuestaDTO BuscarDatosPorTipoCurso(string parametro, string tipo)
        {   // lista de formación académica en datos previos, será una nueva lista de formación académica.
            CRespuestaDTO respuesta = new CRespuestaDTO();
            List<FormacionAcademica> datosPrevios = new List<FormacionAcademica>();
            try
            {
                //para hacer un menú con diferentes parámetros
                switch (parametro)
                {   //en caso de que sean todos, ir a datos previos que será ingresar primero a BD, en formac. acad. y me traiga curso grado, etc.en una lista.
                    case "todos":
                        datosPrevios = entidadBase.FormacionAcademica.Include("Funcionario")
                                                                         .Include("Funcionario.DetalleContratacion")
                                                                         .Include("CursoGrado")
                                                                         .Include("CursoCapacitacion")
                                                                         .Include("CursoGrado.EntidadEducativa")
                                                                         .Include("CursoCapacitacion.Modalidad")
                                                                         .Include("CursoCapacitacion.EntidadEducativa")
                                                                         .Include("DetallePuntosAdicional")
                                                                         .ToList();
                        break;
                    case "grado"://en caso de que haya elegido grado, ir a datosprevios, ir a BD, a formacion acad, y me traiga funcionario, etc en una lista.
                        datosPrevios = entidadBase.FormacionAcademica.Include("Funcionario")
                                                    .Include("DetallePuntosAdicional")
                                                    .Include("Funcionario.DetalleContratacion")
                                                    .Include("CursoGrado")
                                                    .Include("CursoGrado.EntidadEducativa").ToList();
                                                    //.Where(c => c.GetType() == typeof(CursoGrado)).ToList();
                        break;
                    case "capacitacion":
                        datosPrevios = entidadBase.FormacionAcademica.Include("Funcionario")
                                                                            .Include("DetallePuntosAdicional")
                                                                            .Include("Funcionario.DetalleContratacion")
                                                                            .Include("CursoCapacitacion")
                                                                            .Include("CursoCapacitacion.Modalidad")
                                                                            .Include("CursoCapacitacion.EntidadEducativa")
                                                                            .ToList();
                                                                            //.Where(c => c.GetType() == typeof(CursoCapacitacion)).ToList();
                        break;
                    default:
                        break;
                }
                if (tipo == "1")
                {
                    datosPrevios = datosPrevios.Where(Q => Q.Funcionario.DetalleContratacion.FirstOrDefault().CodPolicial != null).ToList();
                }
                else
                {
                    datosPrevios = datosPrevios.Where(Q => Q.Funcionario.DetalleContratacion.FirstOrDefault().CodPolicial == null).ToList();
                }
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
                    throw new Exception("No se encontraron resultados para los parámetros de búsqueda especificados");
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
        //Buscar Formacion academica mediante parametros y los muestre en una lista
        public CRespuestaDTO BuscarFormacionAcademicaParametros(List<FormacionAcademica> datosPrevios, string elemento, object parametro)
        {
            CRespuestaDTO respuesta;

            try
            {
                datosPrevios = CargarDatosFormacionAcademica(datosPrevios, elemento, parametro);
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = datosPrevios
                };
                return respuesta;
            }//aquí cae el error, 
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

        private List<FormacionAcademica> CargarDatosFormacionAcademica(List<FormacionAcademica> datosPrevios, string elemento, object parametro)
        {
            string sParametro = "";
            int horaInicio = 0;
            int horaFinal = 0;
            DateTime dtFechaInicio = new DateTime();
            DateTime dtFechaFin = new DateTime();
            int iParametro = 0;

            if (parametro.GetType() == typeof(string))
            {
                sParametro = parametro.ToString();
            }
            else
            {
                if (parametro.GetType() == typeof(List<int>))
                {
                    horaInicio = ((List<int>)parametro).ElementAt(0);
                    horaFinal = ((List<int>)parametro).ElementAt(1);
                }
                else
                {
                    if (parametro.GetType() == typeof(int))
                    {
                        iParametro = Convert.ToInt32(parametro);
                    }
                    else
                    {
                        dtFechaInicio = ((List<DateTime>)parametro).ElementAt(0);
                        dtFechaFin = ((List<DateTime>)parametro).ElementAt(1);
                    }
                }
            }

            switch (elemento)
            {
                case "cedula":
                    datosPrevios = datosPrevios.Where(Q => Q.Funcionario.IdCedulaFuncionario == sParametro)
                                         .ToList();
                    break;
                case "codPolicial":
                    datosPrevios = datosPrevios.Where(Q => Q.Funcionario.DetalleContratacion.Where(G => G.CodPolicial == iParametro).Count() > 0)
                                         .ToList();
                    break;
                case "rangoFechas":
                    datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.FecInicio >= dtFechaInicio && C.FecFin <= dtFechaFin).Count() > 0
                                                || Q.CursoGrado.Where(G => G.FecEmision >= dtFechaInicio && G.FecEmision <= dtFechaFin).Count() > 0)
                                         .ToList();
                    break;
                case "rangoHoras":
                    datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.IndTotalHoras >= horaInicio && C.IndTotalHoras <= horaFinal).Count() > 0)
                                        .ToList();
                    break;
                case "modalidad":
                    if (iParametro > 0)
                        datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.Modalidad.PK_Modalidad == iParametro).Count() > 0).ToList();
                    else
                        datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.Modalidad.PK_Modalidad > 0).Count() > 0).ToList();
                    break;
                case "numResolucion":
                    datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.NumResolucion != null && C.NumResolucion.Contains(sParametro)).Count() > 0
                                                    || Q.CursoGrado.Where(C => C.NumResolucion != null && C.NumResolucion.Contains(sParametro)).Count() > 0)
                                        .ToList();
                    break;
                case "nombreCurso":
                    datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.DesCursoCapacitacion != null && C.DesCursoCapacitacion.Contains(sParametro)).Count() > 0
                                                || Q.CursoGrado.Where(G => G.DesCursoGrado != null && G.DesCursoGrado.Contains(sParametro)).Count() > 0)
                                         .ToList();
                    break;
                case "entidadEducativa":
                    datosPrevios = datosPrevios.Where(Q => Q.CursoCapacitacion.Where(C => C.EntidadEducativa.PK_EntidadEducativa == iParametro).Count() > 0
                                                || Q.CursoGrado.Where(G => G.EntidadEducativa.PK_EntidadEducativa == iParametro).Count() > 0)
                                         .ToList();
                    break;
                case "grado":
                    if (iParametro > 0)
                        datosPrevios = datosPrevios.Where(Q => Q.CursoGrado.Where(G => G.TipCursoGrado == iParametro).Count() > 0).ToList();
                    else
                        datosPrevios = datosPrevios.Where(Q => Q.CursoGrado.Where(G => G.TipCursoGrado > 0).Count() > 0).ToList();
                    break;
                default:
                    break;
            }
            return datosPrevios;
        }

        #endregion        

        #region Guardar

        public CRespuestaDTO GuardarFormacionAcademica(CBaseDTO curso, string cedula)
        {
            CRespuestaDTO resultado = new CRespuestaDTO();
            try
            {
                var funcionario = entidadBase.Funcionario.Include("FormacionAcademica").Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();
                FormacionAcademica formacion;

                if (curso.GetType() == typeof(CCursoCapacitacionDTO))
                {
                    if (funcionario.FormacionAcademica.Count > 0)
                    {
                        formacion = funcionario.FormacionAcademica.FirstOrDefault();
                        //a formacion en el curso de capacitac. se agrega CursoCapacitacion, convertirlo de DTO a Datos
                        formacion.CursoCapacitacion.Add(ConvertirCursoCapacitacionDTOADatos((CCursoCapacitacionDTO)curso));
                        entidadBase.SaveChanges();
                        resultado = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = formacion.CursoCapacitacion.Last()
                        };
                        return resultado;
                    }
                    else
                    {
                        //formacion = new FormacionAcademica { FecRegistro = DateTime.Now, Funcionario = funcionario };
                        formacion = new FormacionAcademica { FecRegistro = DateTime.Now, PK_FormacionAcademica = funcionario.PK_Funcionario, Funcionario = funcionario};
                        entidadBase.FormacionAcademica.Add(formacion);
                        formacion.CursoCapacitacion.Add(ConvertirCursoCapacitacionDTOADatos((CCursoCapacitacionDTO)curso));
                        entidadBase.SaveChanges();
                        resultado = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = formacion.CursoCapacitacion.Last()
                        };
                        return resultado;
                    }
                }
                else
                {
                    if (funcionario.FormacionAcademica.Count > 0)
                    {
                        formacion = funcionario.FormacionAcademica.FirstOrDefault();
                        formacion.CursoGrado.Add(ConvertirCursoGradoDTOADatos((CCursoGradoDTO)curso));

                        if (funcionario.DetalleContratacion.FirstOrDefault().CodPolicial != null)
                        {
                            if (formacion.CursoGrado.Last().TipCursoGrado == 2 || formacion.CursoGrado.Last().TipCursoGrado == 3) {
                                if (formacion.CursoGrado.Where(c => c.TipCursoGrado == formacion.CursoGrado.Last().TipCursoGrado - 1).FirstOrDefault() != null) {
                                    formacion.CursoGrado.Where(c => c.TipCursoGrado == formacion.CursoGrado.Last().TipCursoGrado - 1).FirstOrDefault().Estado = 2;
                                }
                            }
                        }
                        else
                        {
                            switch (formacion.CursoGrado.Last().TipCursoGrado) {
                                case 2:
                                    if (formacion.CursoGrado.Where(c => c.TipCursoGrado == formacion.CursoGrado.Last().TipCursoGrado - 1).FirstOrDefault() != null)
                                    {
                                        formacion.CursoGrado.Where(c => c.TipCursoGrado == formacion.CursoGrado.Last().TipCursoGrado - 1).FirstOrDefault().Estado = 2;
                                    }
                                    break;
                                case 4: case 6:
                                    if (formacion.CursoGrado.Where(c => c.TipCursoGrado == formacion.CursoGrado.Last().TipCursoGrado - 2).FirstOrDefault() != null)
                                    {
                                        formacion.CursoGrado.Where(c => c.TipCursoGrado == formacion.CursoGrado.Last().TipCursoGrado - 2).FirstOrDefault().Estado = 2;
                                    }
                                    break;
                            }
                        }
                        

                        entidadBase.SaveChanges();
                        resultado = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = formacion.CursoGrado.Last()
                        };
                        return resultado;
                    }
                    else
                    {
                        //formacion = new FormacionAcademica { FecRegistro = DateTime.Now, Funcionario = funcionario };
                        formacion = new FormacionAcademica { FecRegistro = DateTime.Now, FK_Funcionario = funcionario.PK_Funcionario };
                        entidadBase.FormacionAcademica.Add(formacion);
                        formacion.CursoGrado.Add(ConvertirCursoGradoDTOADatos((CCursoGradoDTO)curso));
                        entidadBase.SaveChanges();
                        resultado = new CRespuestaDTO
                        {
                            Codigo = 1,
                            Contenido = formacion.CursoGrado.Last()
                        };
                        return resultado;
                    }
                }
            }
            catch (Exception error)
            {
                resultado = new CRespuestaDTO
                {
                    Codigo = -1,
                    Contenido = new CErrorDTO { MensajeError = error.Message }
                };
                return resultado;
            }
        }

        private CursoGrado ConvertirCursoGradoDTOADatos(CCursoGradoDTO curso)
        {
            return new CursoGrado
            {
                DesCursoGrado = curso.CursoGrado,
                EntidadEducativa = entidadBase.EntidadEducativa.Where(Q => Q.PK_EntidadEducativa == curso.EntidadEducativa.IdEntidad).FirstOrDefault(),
                PorIncentivo = curso.Incentivo,
                TipCursoGrado = curso.TipoGrado,
                FecEmision = curso.FechaEmision,
                NumResolucion = curso.Resolucion,
                ImgTitulo = curso.ImagenTitulo,
                Estado = 1
            };
        }

        private CursoCapacitacion ConvertirCursoCapacitacionDTOADatos(CCursoCapacitacionDTO curso)
        {
            DateTime fecha = DateTime.Now;
            DateTime fechaV = new DateTime(fecha.Year + 5, fecha.Month, fecha.Day);
            if (curso.Resolucion == null)
                return new CursoCapacitacion
                {
                    DesCursoCapacitacion = curso.DescripcionCapacitacion,
                    EntidadEducativa = entidadBase.EntidadEducativa.Where(Q => Q.PK_EntidadEducativa == curso.EntidadEducativa.IdEntidad).FirstOrDefault(),
                    IndTotalHoras = curso.TotalHoras,
                    IndTotalPuntos = curso.TotalPuntos,
                    Modalidad = entidadBase.Modalidad.Where(Q => Q.PK_Modalidad == curso.Modalidad.IdEntidad).FirstOrDefault(),
                    NumResolucion = curso.Resolucion,
                    FecInicio = curso.FechaInicio,
                    FecFin = curso.FechaFinal,
                    ImgTitulo = curso.ImagenTitulo,
                    Estado = 2,
                    FecRegistro = fecha,
                    FecVence = fechaV,
                };
            else
                return new CursoCapacitacion
                {
                    DesCursoCapacitacion = curso.DescripcionCapacitacion,
                    EntidadEducativa = entidadBase.EntidadEducativa.Where(Q => Q.PK_EntidadEducativa == curso.EntidadEducativa.IdEntidad).FirstOrDefault(),
                    IndTotalHoras = curso.TotalHoras,
                    IndTotalPuntos = curso.TotalPuntos,
                    Modalidad = entidadBase.Modalidad.Where(Q => Q.PK_Modalidad == curso.Modalidad.IdEntidad).FirstOrDefault(),
                    NumResolucion = curso.Resolucion,
                    FecInicio = curso.FechaInicio,
                    FecFin = curso.FechaFinal,
                    ImgTitulo = curso.ImagenTitulo,
                    Estado = 1,
                    FecRegistro = fecha,
                    FecVence = fechaV,
                };
        }


        public CRespuestaDTO AgregarPuntosAdicionales(string cedula, int puntos, string observaciones, string numDoc)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var temp = entidadBase.Funcionario.Include("FormacionAcademica").Where(Q => Q.IdCedulaFuncionario == cedula).FirstOrDefault();
                var formacion = temp.FormacionAcademica.FirstOrDefault();
                DetallePuntosAdicional detalle = new DetallePuntosAdicional
                {
                    CantPtos = puntos,
                    Observaciones = observaciones,
                    NumDoc = numDoc,
                    FecRegistro = DateTime.Now,
                };
                formacion.DetallePuntosAdicional.Add(detalle);
                entidadBase.SaveChanges();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = formacion.DetallePuntosAdicional.Last().PK_DetallePuntos
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

        #endregion

        #region Anular
        public CRespuestaDTO AnularCursoCapacitacion(CursoCapacitacion curso)
        {
            CRespuestaDTO respuesta;

            try
            {
                var cursoCapacitacionOld = entidadBase.CursoCapacitacion
                                                    .Where(C => C.PK_CursoCapacitacion == curso.PK_CursoCapacitacion).FirstOrDefault();

                if (cursoCapacitacionOld != null)
                {
                    cursoCapacitacionOld.Estado = 3;
                    cursoCapacitacionOld.Observaciones = curso.Observaciones;
                    curso = cursoCapacitacionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = curso.PK_CursoCapacitacion
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun curso con el código especificado." }
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

        public CRespuestaDTO AnularCursoGrado(CursoGrado curso)
        {
            CRespuestaDTO respuesta;

            try
            {
                var cursoGradoOld = entidadBase.CursoGrado
                                                    .Where(C => C.PK_CursoGrado == curso.PK_CursoGrado).FirstOrDefault();

                if (cursoGradoOld != null)
                {
                    cursoGradoOld.Estado = 3;
                    cursoGradoOld.Observaciones = curso.Observaciones;
                    curso = cursoGradoOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = curso.PK_CursoGrado
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun curso con el código especificado." }
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
        #endregion

        #region Editar
        public CRespuestaDTO EditarCursoCapacitacion(CursoCapacitacion curso)
        {
            CRespuestaDTO respuesta;

            try
            {
                var cursoCapacitacionOld = entidadBase.CursoCapacitacion
                                                    .Where(C => C.PK_CursoCapacitacion == curso.PK_CursoCapacitacion).FirstOrDefault();

                if (cursoCapacitacionOld != null)
                {
                    cursoCapacitacionOld.NumResolucion = curso.NumResolucion;
                    if (curso.ImgTitulo != null)
                    {
                        cursoCapacitacionOld.ImgTitulo = curso.ImgTitulo;
                    }
                    curso = cursoCapacitacionOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = curso.PK_CursoCapacitacion
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun curso con el código especificado." }
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

        public CRespuestaDTO EditarCursoGrado(CursoGrado curso)
        {
            CRespuestaDTO respuesta;

            try
            {
                var cursoGradoOld = entidadBase.CursoGrado
                                                    .Where(C => C.PK_CursoGrado == curso.PK_CursoGrado).FirstOrDefault();

                if (cursoGradoOld != null)
                {
                    cursoGradoOld.NumResolucion = curso.NumResolucion;
                    if (curso.ImgTitulo != null)
                    {
                        cursoGradoOld.ImgTitulo = curso.ImgTitulo;
                    }
                    curso = cursoGradoOld;
                    entidadBase.SaveChanges();

                    respuesta = new CRespuestaDTO
                    {
                        Codigo = 1,
                        Contenido = curso.PK_CursoGrado
                    };

                    return respuesta;
                }
                else
                {
                    respuesta = new CRespuestaDTO
                    {
                        Codigo = -1,
                        Contenido = new CErrorDTO { Mensaje = "No se encontró ningun curso con el código especificado." }
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
        #endregion

        //Retorna el PeriodoEscalaSalarial con el valor del punto
        public CRespuestaDTO ObtenerValorPunto()
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var dato = entidadBase.PeriodoEscalaSalarial.OrderByDescending(X => X.FecInicial).FirstOrDefault();
                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = dato
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

        public CRespuestaDTO CalcularPuntosFuncionario(string cedula)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            try
            {
                var dato = entidadBase.FormacionAcademica.Include("Funcionario")
                                                         .Include("Funcionario.DetalleContratacion")
                                                         .Include("DetallePuntosAdicional")
                                                         .Include("CursoCapacitacion")
                                                         .Include("CursoGrado")
                                                         .Include("ExperienciaProfesional")
                                                         .Where(X => X.Funcionario.IdCedulaFuncionario == cedula).FirstOrDefault();



                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = dato
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


        public CRespuestaDTO AsignarResolucion(List<CCursoCapacitacionDTO> cursos, string numResolucion)
        {
            CRespuestaDTO respuesta = new CRespuestaDTO();
            var temp = new List<CursoCapacitacion>();
            try
            {
                var datos = entidadBase.CursoCapacitacion.Where(X => X.Estado == 2).ToList();


                foreach (CursoCapacitacion c in datos)
                {
                    foreach (CCursoCapacitacionDTO cc in cursos)
                    {
                        if (c.PK_CursoCapacitacion == cc.IdEntidad)
                        {
                            c.NumResolucion = numResolucion;
                            c.Estado = 1;
                            temp.Add(c);
                        }
                    }
                }

                entidadBase.SaveChanges();


                respuesta = new CRespuestaDTO
                {
                    Codigo = 1,
                    Contenido = temp
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


        #endregion        
    }
}

