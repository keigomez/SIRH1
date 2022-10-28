using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using SIRH.Logica;

namespace SIRH.Logica
{
    public class CIncidenciaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CIncidenciaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CIncidenciaDTO ConvertirDatosIncidenciaADto(Incidencia item)
        {
            return new CIncidenciaDTO
            {
                IdEntidad = item.PK_Incidencia,
                EstIncidencia = Convert.ToInt32(item.EstIncidencia),
                FecInicio = Convert.ToDateTime(item.FecInicio),
                FecFin = Convert.ToDateTime(item.FecFin),
                IpOrigen = item.IndIPOrigen,
                Error = item.DesError,
                ImagenError = item.ImgError,
                Motivo = item.DesJustificacion
            };
        }

        internal static string DefinirEstadoIncidencia(int codigo)
        {
            string respuesta;
            switch (codigo)
            {
                case 1:
                    respuesta = "Registrada";
                    break;
                case 2:
                    respuesta = "En Revision";
                    break;
                case 3:
                    respuesta = "Resuelta";
                    break;
                case 4:
                    respuesta = "Reabierta";
                    break;
                case 5:
                    respuesta = "Rechazada";
                    break;
                default:
                    respuesta = "El estado no esta definido";
                    break;
            }
            return respuesta;
        }

        //Se registró en ICIncidenciasService y CIncidenciasService
        public CBaseDTO AgregarIncidencia(CFuncionarioDTO funcionario, CIncidenciaDTO incidencia, CCatalogoIncidenciaDTO catalogoIncidencia)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CIncidenciaD intermedio = new CIncidenciaD(contexto);
                CCatalogoIncidenciaD intermedioCatalogo = new CCatalogoIncidenciaD(contexto);

                Funcionario datosFuncionario = new Funcionario
                {
                    IdCedulaFuncionario = funcionario.Cedula
                };

                Incidencia datosIncidencia = new Incidencia
                {
                    DesError = incidencia.Error,
                    EstIncidencia = incidencia.EstIncidencia,
                    FecInicio = Convert.ToDateTime(incidencia.FecInicio),
                    FecFin = Convert.ToDateTime("1900-01-01 00:00:00"),
                    ImgError = incidencia.ImagenError,
                    IndIPOrigen = incidencia.IpOrigen
                };

                Incidencia datosIncidencia1 = new Incidencia();

                datosIncidencia1.FecInicio = Convert.ToDateTime(incidencia.FecInicio);

                var catalogoIncidencias = intermedioCatalogo.ObtenerCatalogoIncidencia(catalogoIncidencia.IdEntidad);

                if (catalogoIncidencias.Codigo != -1)
                {
                    datosIncidencia.CatalogoIncidencia = (CatalogoIncidencia)catalogoIncidencias.Contenido;
                }
                else
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)catalogoIncidencias).Contenido;
                    throw new Exception();
                }

                respuesta = intermedio.AgregarIncidencia(datosFuncionario, datosIncidencia);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
                }
            }
            catch
            {
                return respuesta;
            }
        }

        public List<List<CBaseDTO>> BuscarIncidencias(CFuncionarioDTO usuario,
                                                       CIncidenciaDTO incidencia,
                                                       List<DateTime> fechasInicio,
                                                       List<DateTime> fechasFin)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CIncidenciaD intermedio = new CIncidenciaD(contexto);
            List<Incidencia> datosIncidencias = new List<Incidencia>();
            int searchflag = 0;

            if (usuario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, usuario.Cedula, "NombreUsuario", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEncargado.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEncargado.Cedula, "CedulaReceptor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEncargado.Nombre != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEncargado.Nombre, "NombreReceptor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEncargado.PrimerApellido != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                   BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEncargado.PrimerApellido, "PrimerApellidoReceptor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEncargado.SegundoApellido != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEncargado.SegundoApellido, "SegundoApellidoReceptor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEmisor.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEmisor.Cedula, "CedulaEmisor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEmisor.Nombre != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEmisor.Nombre, "NombreEmisor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEmisor.PrimerApellido != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                   BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEmisor.PrimerApellido, "PrimerApellidoEmisor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.FuncionarioEmisor.SegundoApellido != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.FuncionarioEmisor.SegundoApellido, "SegundoApellidoEmisor", searchflag));

                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (fechasInicio.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, fechasInicio, "FechaInicio", searchflag));
                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (fechasFin.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, fechasFin, "FechaFin", searchflag));
                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.Catalogo != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.Catalogo.Perfil.NomPerfil, "Catalogo", searchflag));
                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.DetalleEstIncidencia != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.DetalleEstIncidencia, "EstIncidencia", searchflag));
                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
                searchflag = 1;
            }

            if (incidencia.DetallePriIncidencia != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarIncidencias(datosIncidencias, incidencia.DetallePriIncidencia, "Prioridad", searchflag));
                if (resultado.Codigo > 0)
                {
                    datosIncidencias = (List<Incidencia>)resultado.Contenido;
                }
            }

            if (datosIncidencias.Count > 0)
            {
                foreach (var item in datosIncidencias)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    var datoIncidencia = ConvertirDatosIncidenciaADto(item);
                    datoIncidencia.DetalleEstIncidencia = DefinirEstadoIncidencia(datoIncidencia.EstIncidencia);
                    CIncidenciaDTO tempIncidencia = datoIncidencia;
                    temp.Add(tempIncidencia);

                    CFuncionarioDTO tempFuncionario = new CFuncionarioDTO
                    {
                        Cedula = item.Funcionario.IdCedulaFuncionario,
                        Nombre = item.Funcionario.NomFuncionario,
                        PrimerApellido = item.Funcionario.NomPrimerApellido,
                        SegundoApellido = item.Funcionario.NomSegundoApellido,
                        Sexo = GeneroEnum.Indefinido
                    };
                    temp.Add(tempFuncionario);

                    CCatalogoIncidenciaDTO tempCatalogo = CCatalogoIncidenciaL.ConvertirDatosCatalogoIncidenciaADto(item.CatalogoIncidencia);
                    temp.Add(tempCatalogo);

                    if (item.Funcionario1 != null)
                    {
                        CFuncionarioDTO tempFuncionario1 = new CFuncionarioDTO
                        {
                            Cedula = item.Funcionario1.IdCedulaFuncionario,
                            Nombre = item.Funcionario1.NomFuncionario,
                            PrimerApellido = item.Funcionario1.NomPrimerApellido,
                            SegundoApellido = item.Funcionario1.NomSegundoApellido,
                            Sexo = GeneroEnum.Indefinido
                        };

                        temp.Add(tempFuncionario1);
                    }
                    else
                    {
                        temp.Add(null);
                    }

                    respuesta.Add(temp);
                }
            }
            else
            {
                List<CBaseDTO> temp = new List<CBaseDTO>();
                temp.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
                respuesta.Add(temp);
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerIncidencia(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CIncidenciaD intermedio = new CIncidenciaD(contexto);
                CCatalogoIncidenciaD intermedioCatalogoIncidencia = new CCatalogoIncidenciaD(contexto);
                var incidencia = intermedio.ObtenerIncidencia(codigo);

                if (incidencia.Codigo > 0)
                {
                    var datoIncidencia = ConvertirDatosIncidenciaADto((Incidencia)incidencia.Contenido);
                    datoIncidencia.DetalleEstIncidencia = DefinirEstadoIncidencia(datoIncidencia.EstIncidencia);
                    respuesta.Add(datoIncidencia);

                    var funcionario = ((Incidencia)incidencia.Contenido).Funcionario;
                    respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario));

                    var catalogoIncidencia = intermedioCatalogoIncidencia.ObtenerCatalogoIncidencia(((Incidencia)incidencia.Contenido).CatalogoIncidencia.PK_CatalogoIncidencia);
                    respuesta.Add(CCatalogoIncidenciaL.ConvertirDatosCatalogoIncidenciaADto((CatalogoIncidencia)catalogoIncidencia.Contenido));

                    if (((Incidencia)incidencia.Contenido).Funcionario1 != null)
                    {
                        var funcionario1 = ((Incidencia)incidencia.Contenido).Funcionario1;
                        respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario1));

                    }
                    else
                    {
                        respuesta.Add(null);
                    }
                }
                else
                {
                    respuesta.Add((CErrorDTO)incidencia.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return respuesta;
        }

        public List<CBaseDTO> ListarPerfiles()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CIncidenciaD intermedio = new CIncidenciaD(contexto);

                var datos = intermedio.ListarPerfiles();
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        foreach (var item in (List<Perfil>)datos.Contenido)
                        {
                            respuesta.Add(CPerfilL.PerfilADto(item));
                        }

                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado en la consulta");
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        public List<CBaseDTO> ObtenerCatalogo(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCatalogoIncidenciaD intermedio = new CCatalogoIncidenciaD(contexto);

                var catalogo = intermedio.ObtenerCatalogoIncidencia(codigo);

                if (catalogo.Codigo > 0)
                {
                    respuesta.Add(CCatalogoIncidenciaL.ConvertirDatosCatalogoIncidenciaADto((CatalogoIncidencia)catalogo.Contenido));
                }
                else
                {
                    respuesta.Add((CErrorDTO)catalogo.Contenido);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }

            return respuesta;
        }

        public CBaseDTO ActualizarEstado(CIncidenciaDTO tramite)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CIncidenciaD intermedio = new CIncidenciaD(contexto);
                Funcionario datosFuncionario = new Funcionario();

                if (tramite.FuncionarioEncargado != null)
                {
                    datosFuncionario = new Funcionario
                    {
                        IdCedulaFuncionario = tramite.FuncionarioEncargado.Cedula
                    };
                }

                Incidencia tramiteBD = new Incidencia
                {
                    PK_Incidencia = tramite.IdEntidad,
                    EstIncidencia = tramite.EstIncidencia,
                    FecFin = tramite.FecFin,
                    DesJustificacion = tramite.Motivo
                };

                if (tramite.FuncionarioEncargado != null)
                {
                    tramiteBD.Funcionario1 = datosFuncionario;
                }

                respuesta = intermedio.ActualizarEstado(tramiteBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                else
                {
                    return respuesta;
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

        #endregion
    }
}