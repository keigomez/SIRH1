using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CFormacionAcademicaL
    {
        #region variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CFormacionAcademicaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        //una lista de listas en baseDTO para buscar datos por curso, lista de string de elementos y lista de objetos para parámetros
        //Se insertó en CFormacionAcademicaService y ICFormacionAcademicaService el 26/01/2017 
        public List<List<CBaseDTO>> BuscarDatosCursos(CFuncionarioDTO funcionario, CDetalleContratacionDTO contratacion,
                                                       CBaseDTO curso, List<DateTime> fechas)
        {
            //una lista de listas la respuesta será la nueva lista de listas
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            try
            {
                string tipoCurso = "";
                //intermedio será donde pasen los datos de formacion académica, desde el contexto
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);
                if (curso != null)
                {
                    if (curso.GetType() == typeof(CCursoCapacitacionDTO))
                    {
                        tipoCurso = "capacitacion";
                    }
                    else
                    {
                        tipoCurso = "grado";
                    }
                }
                else
                {
                    tipoCurso = "todos";
                }
                //los datos estarán en intermedio, en el método de buscar datos por tipo de curso(me trae datos por medio de los parámetros, el primero que encuentre) convertido a string
                var datos = intermedio.BuscarDatosPorTipoCurso(tipoCurso, funcionario.Mensaje);
                // si los datos no tienen errores
                if (datos.Codigo != -1)
                {
                    //entonces la lista de FormacionAcademica estarán en los datos previos como nueva lista de formacion academica
                    List<FormacionAcademica> datosPrevios = new List<FormacionAcademica>();
                    //datos previos es igual a la lista de la formacion académica, con todos los datos.
                    datosPrevios = ((List<FormacionAcademica>)datos.Contenido);

                    if (funcionario.Cedula != null)
                    {
                        var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "cedula", funcionario.Cedula));

                        if (resultado.Codigo > 0)
                        {
                            datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                        }
                    }

                    if (fechas.Count > 0)
                    {
                        var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "rangoFechas", fechas));

                        if (resultado.Codigo > 0)
                        {
                            datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                        }
                    }

                    if (contratacion != null)
                        if (contratacion.CodigoPolicial > 0)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "codPolicial", contratacion.CodigoPolicial));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                    if (curso.GetType() == typeof(CCursoCapacitacionDTO))
                    {
                        if (((CCursoCapacitacionDTO)curso).Resolucion != null)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "numResolucion", ((CCursoCapacitacionDTO)curso).Resolucion));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                        if (((CCursoCapacitacionDTO)curso).DescripcionCapacitacion != null)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "nombreCurso", ((CCursoCapacitacionDTO)curso).DescripcionCapacitacion));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                        if (((CCursoCapacitacionDTO)curso).EntidadEducativa.IdEntidad != 0)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "entidadEducativa", ((CCursoCapacitacionDTO)curso).EntidadEducativa.IdEntidad));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                        if (((CCursoCapacitacionDTO)curso).Modalidad.IdEntidad >= 0)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "modalidad", ((CCursoCapacitacionDTO)curso).Modalidad.IdEntidad));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }
                    }
                    else
                    {
                        if (((CCursoGradoDTO)curso).Resolucion != null)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "numResolucion", ((CCursoGradoDTO)curso).Resolucion));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                        if (((CCursoGradoDTO)curso).CursoGrado != null)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "nombreCurso", ((CCursoGradoDTO)curso).CursoGrado));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                        if (((CCursoGradoDTO)curso).EntidadEducativa.DescripcionEntidad != null)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "entidadEducativa", ((CCursoGradoDTO)curso).EntidadEducativa.DescripcionEntidad));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }

                        if (((CCursoGradoDTO)curso).TipoGrado >= 0)
                        {
                            var resultado = ((CRespuestaDTO)intermedio.BuscarFormacionAcademicaParametros(datosPrevios, "grado", ((CCursoGradoDTO)curso).TipoGrado));

                            if (resultado.Codigo > 0)
                            {
                                datosPrevios = (List<FormacionAcademica>)resultado.Contenido;
                            }
                        }
                    }

                    if (datosPrevios.Count > 0)
                    {
                        //para cada item en datos previos
                        foreach (var item in datosPrevios)
                        {
                            //lista de CBaseDTO será un temp para la nueva lista de CBaseDTO
                            List<CBaseDTO> temp = new List<CBaseDTO>();
                            //Al temp (lista de CBaseDTO) agregarle el item, y convertir Formacion a DTO
                            temp.Add(ConvertirFormacionADTO(item));
                            CFuncionarioDTO fun = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Funcionario);
                            fun.Mensaje = item.DetallePuntosAdicional.Sum(Q => Q.CantPtos).ToString();
                            temp.Add(fun);
                            //Si el item en curso grado es mayor a cero....
                            List<CBaseDTO> tempCurso = new List<CBaseDTO>();
                            if (tipoCurso == "grado") // dentro de este foreach
                            {
                                if (item.CursoGrado.Count > 0)
                                {
                                    //para cada item de grado en el item de curso de grado
                                    foreach (var itemGrado in item.CursoGrado)
                                    {
                                        if (((CCursoGradoDTO)curso).TipoGrado >= 0)
                                        {

                                            if (itemGrado.TipCursoGrado == ((CCursoGradoDTO)curso).TipoGrado)
                                                //agregue al temp(lista de CBaseDTO) el item de grado, convertido a DTO
                                                tempCurso.Add(ConvertirGradoADTO(itemGrado));
                                            else if (((CCursoGradoDTO)curso).TipoGrado == 0)
                                            {
                                                tempCurso.Add(ConvertirGradoADTO(itemGrado));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (item.CursoCapacitacion.Count > 0)
                                {
                                    //para cada item de capacitacion en el item 
                                    foreach (var itemCapacitacion in item.CursoCapacitacion)
                                    {
                                        if (((CCursoCapacitacionDTO)curso).Modalidad != null)
                                        {
                                            if (itemCapacitacion.Modalidad.PK_Modalidad == ((CCursoCapacitacionDTO)curso).Modalidad.IdEntidad)

                                                //al temp(lista) agregar el item de capacitacion y convertirlo a DTO
                                                tempCurso.Add(ConvertirCapacitacionADTO(itemCapacitacion));
                                            else if (((CCursoCapacitacionDTO)curso).Modalidad.IdEntidad == 0)
                                            {
                                                tempCurso.Add(ConvertirCapacitacionADTO(itemCapacitacion));
                                            }
                                        }

                                    }
                                }
                            }

                            //Aplicado del filtro en capa lógica
                            if (curso.GetType() == typeof(CCursoCapacitacionDTO))
                            {
                                if (((CCursoCapacitacionDTO)curso).EntidadEducativa.IdEntidad != 0)
                                    tempCurso = tempCurso.Where(C => ((CCursoCapacitacionDTO)C).EntidadEducativa.IdEntidad == ((CCursoCapacitacionDTO)curso).EntidadEducativa.IdEntidad).ToList();
                                if (fechas.Count() > 0)
                                    tempCurso = tempCurso.Where(C => (((CCursoCapacitacionDTO)C).FechaInicio >= fechas[0]) && (((CCursoCapacitacionDTO)C).FechaFinal <= fechas[1])).ToList();
                            }
                            else
                            {
                                if (((CCursoGradoDTO)curso).EntidadEducativa.IdEntidad != 0)
                                    tempCurso = tempCurso.Where(C => ((CCursoGradoDTO)C).EntidadEducativa.IdEntidad == ((CCursoGradoDTO)curso).EntidadEducativa.IdEntidad).ToList();
                                if (fechas.Count() > 0)
                                    tempCurso = tempCurso.Where(C => (((CCursoGradoDTO)C).FechaEmision >= fechas[0]) && (((CCursoGradoDTO)C).FechaEmision <= fechas[1])).ToList();
                            }



                            if (tempCurso.Count() < 1)
                            {
                                string textoError = "No se encontraron resultados o bien los parámetros ingresados no corresponden al tipo de carrera seleccionado. Se seleccionó: ";
                                if (funcionario.Mensaje == "1")
                                {
                                    textoError = textoError + "Carrera Policial.";
                                }
                                else
                                {
                                    textoError = textoError + "Carrera Profesional.";
                                }
                                throw new Exception(textoError);
                            }


                            //si el item de curso de capacitacion es mayor a cero....

                            //Respuesta es lo que se le agregó(item) anteriormente a la lista (temp)
                            respuesta.Add(temp);
                            respuesta.Add(tempCurso);
                        }

                    }
                    else
                    {
                        string textoError = "No se encontraron resultados o bien los parámetros ingresados no corresponden al tipo de carrera seleccionado. Se seleccionó: ";
                        if (funcionario.Mensaje == "1")
                        {
                            textoError = textoError + "Carrera Policial.";
                        }
                        else
                        {
                            textoError = textoError + "Carrera Profesional.";
                        }
                        throw new Exception(textoError);
                    }
                    //regrese la respuesta
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                //respuesta se le agrega a la nueva lista de CBaseDTO un error de mensaje.
                respuesta.Add(new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } });
                return respuesta;
            }
        }
        //YA ESTA INSERTADO EN ICFormacionAcademicaService y CFormacionAcademica(Deivert)
        public List<CBaseDTO> GuardarFormacionAcademica(CBaseDTO curso, CFuncionarioDTO funcionario)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);
                var datos = intermedio.GuardarFormacionAcademica(curso, funcionario.Cedula);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        CBaseDTO cursoInsertado = new CBaseDTO();
                        if (datos.Contenido.GetType() == typeof(CursoCapacitacion))
                        {
                            cursoInsertado = ConvertirCapacitacionADTO((CursoCapacitacion)datos.Contenido);
                        }
                        else
                        {
                            cursoInsertado = ConvertirGradoADTO((CursoGrado)datos.Contenido);
                        }
                        respuesta.Add(funcionario);
                        respuesta.Add(cursoInsertado);
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado");
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        //son los métodos que se genera cuando se va a convertir información de Datos a DTO, como anteriormente
        //Se usa internal static para que se puedan ver entre las mismas clases de la misma capa, no se pueden ver en otras capas.
        internal static CCursoCapacitacionDTO ConvertirCapacitacionADTO(CursoCapacitacion itemCapacitacion)
        {
            return new CCursoCapacitacionDTO
            {
                IdEntidad = itemCapacitacion.PK_CursoCapacitacion,
                DescripcionCapacitacion = itemCapacitacion.DesCursoCapacitacion,
                EntidadEducativa = CEntidadEducativaL.ConvertirEntidadEducativaADTO(itemCapacitacion.EntidadEducativa),
                FechaFinal = Convert.ToDateTime(itemCapacitacion.FecFin),
                FechaInicio = Convert.ToDateTime(itemCapacitacion.FecInicio),
                Modalidad = CModalidadL.ConvertirModalidadADTO(itemCapacitacion.Modalidad),
                Estado = Convert.ToInt32(itemCapacitacion.Estado),
                TotalHoras = Convert.ToInt32(itemCapacitacion.IndTotalHoras),
                Resolucion = itemCapacitacion.NumResolucion,
                FecRegistro = Convert.ToDateTime(itemCapacitacion.FecRegistro),
                FecVence = Convert.ToDateTime(itemCapacitacion.FecVence),

            };
        }
        //se usa internal para que se puedan ver entre las mismas clases de la misma capa, no se pueden ver en otras capas.
        internal static CCursoGradoDTO ConvertirGradoADTO(CursoGrado itemGrado)
        {
            return new CCursoGradoDTO
            {
                IdEntidad = itemGrado.PK_CursoGrado,
                CursoGrado = itemGrado.DesCursoGrado,
                FechaEmision = Convert.ToDateTime(itemGrado.FecEmision),
                Incentivo = Convert.ToInt32(itemGrado.PorIncentivo),
                Resolucion = itemGrado.NumResolucion,
                TipoGrado = Convert.ToInt32(itemGrado.TipCursoGrado),
                EntidadEducativa = CEntidadEducativaL.ConvertirEntidadEducativaADTO(itemGrado.EntidadEducativa),
                Estado = Convert.ToInt32(itemGrado.Estado)
            };
        }

        internal static CFormacionAcademicaDTO ConvertirFormacionADTO(FormacionAcademica item)
        {
            return new CFormacionAcademicaDTO 
            {
                IdEntidad = item.PK_FormacionAcademica,
                Fecha = Convert.ToDateTime(item.FecRegistro),
                Funcionario = CFuncionarioL.FuncionarioGeneral(item.Funcionario)
            };
        }
        #endregion
    
        public List<CBaseDTO> BuscarCursoCapacitacionPorCodigo(CCursoCapacitacionDTO curso)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);

                var datos = intermedio.BuscarCursoCapacitacionPorCodigo(curso.IdEntidad);

                if (datos.Codigo != -1)
                {
                    respuesta.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(((CursoCapacitacion)datos.Contenido).FormacionAcademica.Funcionario));
                    respuesta.Add(CCursoCapacitacionL.ConvertirCursoCapacitacionADTO(((CursoCapacitacion)datos.Contenido)));
                    respuesta.Add(CEntidadEducativaL.ConvertirEntidadEducativaADTO(((CursoCapacitacion)datos.Contenido).EntidadEducativa));
                    respuesta.Add(CModalidadL.ConvertirModalidadADTO(((CursoCapacitacion)datos.Contenido).Modalidad));
                    respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(((CursoCapacitacion)datos.Contenido).FormacionAcademica.Funcionario.DetalleContratacion.FirstOrDefault()));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> BuscarCursoGradoPorCodigo(CCursoGradoDTO curso)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);

                var datos = intermedio.BuscarCursoGradoPorCodigo(curso.IdEntidad);

                if (datos.Codigo != -1)
                {
                    respuesta.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(((CursoGrado)datos.Contenido).FormacionAcademica.Funcionario));
                    respuesta.Add(CCursoGradoL.ConvertirDatosCursoGradoADTO(((CursoGrado)datos.Contenido)));
                    respuesta.Add(CEntidadEducativaL.ConvertirEntidadEducativaADTO(((CursoGrado)datos.Contenido).EntidadEducativa));
                    respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(((CursoGrado)datos.Contenido).FormacionAcademica.Funcionario.DetalleContratacion.FirstOrDefault()));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> BuscarDatosCarreraCedula(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);

                var datos = intermedio.BuscarDatosCarreraPorCedula(cedula);

                if (datos.Codigo != -1)
                {
                    CFuncionarioDTO fun = CFuncionarioL.ConvertirDatosFuncionarioADTO(((Funcionario)datos.Contenido));
                    if (((Funcionario)datos.Contenido).FormacionAcademica.Count() > 0)
                        fun.Mensaje = ((Funcionario)datos.Contenido).FormacionAcademica.FirstOrDefault().DetallePuntosAdicional.Sum(Q => Q.CantPtos).ToString();
                    else
                        fun.Mensaje = "0";
                    respuesta.Add(fun);
                    respuesta.Add(CPuestoL.ConstruirPuesto(((Funcionario)datos.Contenido).Nombramiento.FirstOrDefault().Puesto, new CPuestoDTO()));
                    //respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Funcionario)datos.Contenido).Nombramiento.FirstOrDefault().Puesto.DetallePuesto.FirstOrDefault()));
                    respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(((Funcionario)datos.Contenido).DetalleContratacion.FirstOrDefault()));
                    if (((Funcionario)datos.Contenido).FormacionAcademica.Count() > 0)
                    {
                        foreach (var grado in ((Funcionario)datos.Contenido).FormacionAcademica.FirstOrDefault().CursoGrado)
                        {
                            respuesta.Add(CCursoGradoL.ConvertirDatosCursoGradoADTO(grado));
                            respuesta.Add(CEntidadEducativaL.ConvertirEntidadEducativaADTO(grado.EntidadEducativa));
                        }
                        foreach (var capacitacion in ((Funcionario)datos.Contenido).FormacionAcademica.FirstOrDefault().CursoCapacitacion)
                        {
                            respuesta.Add(CCursoCapacitacionL.ConvertirCursoCapacitacionADTO(capacitacion));
                            respuesta.Add(CModalidadL.ConvertirModalidadADTO(capacitacion.Modalidad));
                        }
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public CBaseDTO AnularCurso(CBaseDTO curso)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);

                if (curso.GetType() == typeof(CCursoCapacitacionDTO))
                {
                    CursoCapacitacion cursoCapacitacion = new CursoCapacitacion
                    {
                        PK_CursoCapacitacion = curso.IdEntidad,
                        Observaciones = ((CCursoCapacitacionDTO)curso).Observaciones
                    };

                    var datosCurso = intermedio.AnularCursoCapacitacion(cursoCapacitacion);

                    if (datosCurso.Codigo > 0)
                    {
                        respuesta = new CBaseDTO { IdEntidad = curso.IdEntidad };
                    }
                    else
                    {
                        respuesta = ((CErrorDTO)datosCurso.Contenido);
                    }
                }
                else
                {
                    CursoGrado cursoGrado = new CursoGrado
                    {
                        PK_CursoGrado = curso.IdEntidad,
                        Observaciones = ((CCursoGradoDTO)curso).Observaciones
                    };

                    var datosCurso = intermedio.AnularCursoGrado(cursoGrado);

                    if (datosCurso.Codigo > 0)
                    {
                        respuesta = new CBaseDTO { IdEntidad = curso.IdEntidad };
                    }
                    else
                    {
                        respuesta = ((CErrorDTO)datosCurso.Contenido);
                    }
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public CBaseDTO EditarCurso(CBaseDTO curso)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);

                if (curso.GetType() == typeof(CCursoCapacitacionDTO))
                {
                    CursoCapacitacion cursoCapacitacion = new CursoCapacitacion
                    {
                        PK_CursoCapacitacion = curso.IdEntidad,
                        NumResolucion = ((CCursoCapacitacionDTO)curso).Resolucion,
                        ImgTitulo = ((CCursoCapacitacionDTO)curso).ImagenTitulo
                    };

                    var datosCurso = intermedio.EditarCursoCapacitacion(cursoCapacitacion);

                    if (datosCurso.Codigo > 0)
                    {
                        respuesta = new CBaseDTO { IdEntidad = curso.IdEntidad };
                    }
                    else
                    {
                        respuesta = ((CErrorDTO)datosCurso.Contenido);
                    }
                }
                else
                {
                    CursoGrado cursoGrado = new CursoGrado
                    {
                        PK_CursoGrado = curso.IdEntidad,
                        NumResolucion = ((CCursoGradoDTO)curso).Resolucion,
                        ImgTitulo = ((CCursoGradoDTO)curso).ImagenTitulo
                    };

                    var datosCurso = intermedio.EditarCursoGrado(cursoGrado);

                    if (datosCurso.Codigo > 0)
                    {
                        respuesta = new CBaseDTO { IdEntidad = curso.IdEntidad };
                    }
                    else
                    {
                        respuesta = ((CErrorDTO)datosCurso.Contenido);
                    }
                }
            }
            catch (Exception error)
            {
                respuesta = (new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public List<CBaseDTO> BuscarDatosCarreraFuncionario(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);

                var datos = intermedio.BuscarDatosCarreraPorCedula(cedula);

                if (datos.Codigo != -1)
                {
                    respuesta.Add(CFuncionarioL.ConvertirDatosFuncionarioADTO(((Funcionario)datos.Contenido)));
                    respuesta.Add(CPuestoL.ConstruirPuesto(((Funcionario)datos.Contenido).Nombramiento.FirstOrDefault().Puesto, new CPuestoDTO()));
                    //respuesta.Add(CDetallePuestoL.ConstruirDetallePuesto(((Funcionario)datos.Contenido).Nombramiento.FirstOrDefault().Puesto.DetallePuesto.FirstOrDefault()));
                    respuesta.Add(CDetalleContratacionL.ConvertirDetalleContratacionADTO(((Funcionario)datos.Contenido).DetalleContratacion.FirstOrDefault()));
                    if (((Funcionario)datos.Contenido).FormacionAcademica.Count() > 0)
                    {
                        foreach (var grado in ((Funcionario)datos.Contenido).FormacionAcademica.FirstOrDefault()?.CursoGrado)
                        {
                            respuesta.Add(CCursoGradoL.ConvertirDatosCursoGradoADTO(grado));
                            respuesta.Add(CEntidadEducativaL.ConvertirEntidadEducativaADTO(grado.EntidadEducativa));
                        }
                        foreach (var capacitacion in ((Funcionario)datos.Contenido).FormacionAcademica.FirstOrDefault()?.CursoCapacitacion)
                        {
                            respuesta.Add(CCursoCapacitacionL.ConvertirCursoCapacitacionADTO(capacitacion));
                            respuesta.Add(CModalidadL.ConvertirModalidadADTO(capacitacion.Modalidad));
                        }
                    }

                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public CBaseDTO AgregarPuntosAdicionales(string cedula, int puntos, string observaciones, string numDoc)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);
                var datos = intermedio.AgregarPuntosAdicionales(cedula, puntos, observaciones, numDoc);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        respuesta.IdEntidad = (int)datos.Contenido;
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado");
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        //Retorna el PeriodoEscalaSalarial con el valor del punto
        public CBaseDTO ObtenerValorPunto()
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);
                var datos = intermedio.ObtenerValorPunto();
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        respuesta = CPeriodoEscalaSalarialL.ConvertirPeriodoEscalaSalarialADTO((PeriodoEscalaSalarial)datos.Contenido);
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado");
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }



        public CBaseDTO CalcularPuntosFuncionario(string cedula)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);
                var datos = intermedio.CalcularPuntosFuncionario(cedula);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        FormacionAcademica formacion = (FormacionAcademica)datos.Contenido;

                        respuesta = CalcularPuntos(formacion);

                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado");
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        internal static CDetallePuntosDTO CalcularPuntos(FormacionAcademica formacion)
        {
            CDetallePuntosDTO puntos = new CDetallePuntosDTO();

            //var grado = formacion.CursoGrado.OrderByDescending(Q => Q.FecEmision).FirstOrDefault(); // Tomar grado academico más reciente (fecha de emision)
            var grado = formacion.CursoGrado.OrderByDescending(Q => Q.PorIncentivo).FirstOrDefault();    //Tomar grado academico que otorga más puntos

            //POLICIAL
            if (formacion.Funcionario.DetalleContratacion.FirstOrDefault().CodPolicial > 0)
            {

                //Aprovechamiento
                puntos.CursosAprovechamiento = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 1).Count();
                puntos.HorasAprovechamiento = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 1 && X.IndTotalHoras >= 12).Sum(S => S.IndTotalHoras);
                puntos.PuntosAprovechamiento = Math.Floor(puntos.HorasAprovechamiento / 40);
                puntos.HorasExcAprovechamiento = puntos.HorasAprovechamiento - (puntos.PuntosAprovechamiento * 40);

                //Participacion
                puntos.CursosParticipacion = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 2).Count();
                puntos.HorasParticipacion = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 2 && X.IndTotalHoras >= 30).Sum(S => S.IndTotalHoras);
                puntos.PuntosParticipacion = Math.Floor(puntos.HorasParticipacion / 80);
                puntos.HorasExcParticipacion = puntos.HorasParticipacion - (puntos.PuntosParticipacion * 80);

                //Aprovechamiento Ley 9635
                puntos.CursosAprovechamientoLey = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 10).Count();
                puntos.HorasAprovechamientoLey = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 10 && X.IndTotalHoras >= 12 && X.NumResolucion != null && X.FecVence > DateTime.Now).Sum(S => S.IndTotalHoras);
                puntos.PuntosAprovechamientoLey = Math.Floor(puntos.HorasAprovechamientoLey / 40);
                puntos.HorasExcAprovechamientoLey = puntos.HorasAprovechamientoLey - (puntos.PuntosAprovechamientoLey * 40);

                //Participacion Ley 9635
                puntos.CursosParticipacionLey = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 11).Count();
                puntos.HorasParticipacionLey = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 11 && X.IndTotalHoras >= 30 && X.NumResolucion != null && X.FecVence > DateTime.Now).Sum(S => S.IndTotalHoras);
                puntos.PuntosParticipacionLey = Math.Floor(puntos.HorasParticipacionLey / 80);
                puntos.HorasExcParticipacionLey = puntos.HorasParticipacionLey - (puntos.PuntosParticipacion * 80);

                //Grado

                puntos.PuntosGrado = (decimal)grado.PorIncentivo;
                int tipGrado = (int)grado.TipCursoGrado;
                switch (tipGrado)
                {
                    case 1: puntos.Grado = "Diplomado"; break;
                    case 2: puntos.Grado = "Bachiller"; break;
                    case 3: puntos.Grado = "Licenciatura"; break;
                    default: puntos.Grado = ""; break;     //Sin grado académico
                }

            }
            //ADMINISTRATIVO
            else
            {
                //Aprovechamiento
                puntos.CursosAprovechamiento = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 1).Count();
                puntos.HorasAprovechamiento = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 1 && X.IndTotalHoras >= 1).Sum(S => S.IndTotalHoras);
                puntos.PuntosAprovechamiento = Math.Floor(puntos.HorasAprovechamiento / 40);
                puntos.HorasExcAprovechamiento = puntos.HorasAprovechamiento - (puntos.PuntosAprovechamiento * 40);

                //Participacion
                puntos.CursosParticipacion = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 2).Count();
                puntos.HorasParticipacion = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 2 && X.IndTotalHoras >= 1).Sum(S => S.IndTotalHoras);
                puntos.PuntosParticipacion = Math.Floor(puntos.HorasParticipacion / 80);
                puntos.HorasExcParticipacion = puntos.HorasParticipacion - (puntos.PuntosParticipacion * 80);

                //Instruccion
                puntos.CursosInstruccion = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 3).Count();
                puntos.HorasInstruccion = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 3 && X.IndTotalHoras >= 1).Sum(S => S.IndTotalHoras);
                puntos.PuntosInstruccion = Math.Floor(puntos.HorasInstruccion / 24);

                //Aprovechamiento Ley 9635
                puntos.CursosAprovechamientoLey = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 10).Count();
                puntos.HorasAprovechamientoLey = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 10 && X.IndTotalHoras >= 1 && X.NumResolucion != null && X.FecVence > DateTime.Now).Sum(S => S.IndTotalHoras);
                puntos.PuntosAprovechamientoLey = Math.Floor(puntos.HorasAprovechamientoLey / 40);
                puntos.HorasExcAprovechamientoLey = puntos.HorasAprovechamientoLey - (puntos.PuntosAprovechamientoLey * 40);

                //Participacion Ley 9635
                puntos.CursosParticipacionLey = formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 11).Count();
                puntos.HorasParticipacionLey = (decimal)formacion.CursoCapacitacion.Where(X => (X.Estado == 1 || X.Estado == null) && X.FK_Modalidad == 11 && X.IndTotalHoras >= 1 && X.NumResolucion != null && X.FecVence > DateTime.Now).Sum(S => S.IndTotalHoras);
                puntos.PuntosParticipacionLey = Math.Floor(puntos.HorasParticipacionLey / 80);
                puntos.HorasExcParticipacionLey = puntos.HorasParticipacionLey - (puntos.PuntosParticipacion * 80);

                //Grado
                puntos.PuntosGrado = (decimal)grado.PorIncentivo;
                int tipGrado = (int)grado.TipCursoGrado;
                switch (tipGrado)
                {
                    case 1: puntos.Grado = "Bachiller"; break;
                    case 2: puntos.Grado = "Licenciatura"; break;
                    case 3: puntos.Grado = "Licenciatura adicional"; break;
                    case 4: puntos.Grado = "Maestría"; break;
                    case 5: puntos.Grado = "Maestría adicional"; break;
                    case 6: puntos.Grado = "Doctorado "; break;
                    case 7: puntos.Grado = "Doctorado adicional"; break;
                    case 8: puntos.Grado = "Especialidad con base a bachillerato"; break;
                    case 9: puntos.Grado = "Especialidad con base a licenciatura"; break;
                    case 10: puntos.Grado = "Especialidad adicional"; break;
                    default: puntos.Grado = ""; break;     //Sin grado académico
                }
            }

            puntos.PuntosAdicionales = (decimal)formacion.DetallePuntosAdicional.Sum(X => X.CantPtos);

            puntos.TotalPuntos = puntos.PuntosAprovechamiento + puntos.PuntosParticipacion + puntos.PuntosInstruccion + puntos.PuntosAdicionales;


            return puntos;

        }

        public List<CBaseDTO> AsignarResolucion(List<CCursoCapacitacionDTO> cursos, string numResolucion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFormacionAcademicaD intermedio = new CFormacionAcademicaD(contexto);
                var datos = intermedio.AsignarResolucion(cursos, numResolucion);

                if (datos.Codigo != -1)
                {
                    List<CursoCapacitacion> contenido = (List<CursoCapacitacion>)datos.Contenido;

                    foreach (CursoCapacitacion c in contenido)
                    {
                        respuesta.Add(ConvertirCapacitacionADTO(c));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }
    }
}
