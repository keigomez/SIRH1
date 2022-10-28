using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCargoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCargoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal static CCargoDTO ConvertirCargoDTOADatos(Cargo input)
        {
            return new CCargoDTO
            {
                IdEntidad = input.PK_Cargo,
                JefaturaInmediata = input.IndJefaturaInmediata,
                JefaturaSuperiorInmediata = input.IndJefaturaSuperiorInmediata,
                NombreCargo = input.NomCargo,
                ProcesoTrabajo = input.DesProcesoTrabajo,
                Proposito = input.DesProposito,
                ClaseCargo = input.IndClase != 0 ? new CClaseDTO { IdEntidad = Convert.ToInt32(input.IndClase) } : null,
                EspecialidadCargo = input.IndEspecialidad != 0 ? new CEspecialidadDTO { IdEntidad = Convert.ToInt32(input.IndEspecialidad) } : null,
                SubespecialidadCargo = input.IndSubespecialidad != 0 ? new CSubEspecialidadDTO { IdEntidad = Convert.ToInt32(input.IndSubespecialidad) } : null,
                SeccionCargo = input.IndSeccion != 0 ? new CSeccionDTO { IdEntidad = Convert.ToInt32(input.IndSeccion) } : null,
                UbicacionOrganizacional = input.DesUbicacionOrganizacional,
                Estrato = input.DesEstrato
            };
        }

        internal static CResultadoCargoDTO ConvertirResultadoCargoADTO(ResultadoCargo input)
        {
            return new CResultadoCargoDTO
            {
                IdEntidad = input.PK_ResultadoCargo,
                Cargo = new CCargoDTO { IdEntidad = input.Cargo.PK_Cargo, NombreCargo = input.Cargo.NomCargo },
                ResultadoCargo = input.DesResultadoCargo,
                ActividadesResultado = ConvertirListaActividadesResultado(input.ActividadResultadoCargo.ToList())
            };
        }

        internal static List<CActividadResultadoCargoDTO> ConvertirListaActividadesResultado(List<ActividadResultadoCargo> actividades)
        {
            List<CActividadResultadoCargoDTO> resultado = new List<CActividadResultadoCargoDTO>();

            foreach (var item in actividades)
            {
                resultado.Add(ConvertirActividadResultadoCargoADTO(item));
            }

            return resultado;
        }

        internal static CActividadResultadoCargoDTO ConvertirActividadResultadoCargoADTO(ActividadResultadoCargo input)
        {
            return new CActividadResultadoCargoDTO
            {
                IdEntidad = input.PK_ActividadResultadoCargo,
                ActividadResultadoCargo = input.DesActividadResultadoCargo,
            };
        }

        internal static CFactorClasificacionCargoDTO ConvertirFactoresADTO(FactorClasificacionCargo input)
        {
            return new CFactorClasificacionCargoDTO
            {
                IdEntidad = input.PK_FactorClasificacion,
                ActivosEquiposInsumos = input.DesActivosEquiposInsumos,
                Ambiente = input.DesAmbiente,
                Condiciones = input.DesCondiciones,
                Cargo = new CCargoDTO { IdEntidad = input.Cargo.PK_Cargo, NombreCargo = input.Cargo.NomCargo },
                ImpactoGestion = input.DesImpactoGestion,
                Independencia = input.DesIndependencia,
                Lugares = input.DesLugares,
                ModalidadTrabajo = input.DesModalidadTrabajo,
                RelacionesTrabajo = input.DesRelacionesTrabajo,
                SupervisionEjercida = input.DesSupervisionEjercida
            };
        }

        internal static CRequerimientoEspecificoCargoDTO ConvertirRequerimientosEspecificos(RequerimientoEspecificoCargo input)
        {
            return new CRequerimientoEspecificoCargoDTO
            {
                IdEntidad = input.PK_RequerimientoEspecificoCargo,
                Cargo = new CCargoDTO { IdEntidad = input.Cargo.PK_Cargo, NombreCargo = input.Cargo.NomCargo },
                Conocimientos = input.DesConocimientos,
                RequisitosEspecificos = input.DesRequisitosEspecificos
            };
        }

        internal static CCompetenciaTransversalCargoDTO ConvertirCompetenciaTransversal(CompetenciaTransversalCargo input)
        {
            return new CCompetenciaTransversalCargoDTO
            {
                IdEntidad = input.PK_CompetenciaTransversalCargo,
                Cargo = new CCargoDTO { IdEntidad = input.Cargo.PK_Cargo, NombreCargo = input.Cargo.NomCargo },
                NivelDominio = Convert.ToInt32(input.IntNivelDominio),
                TipoCompetencia = Convert.ToInt32(input.IndTipoCompetencia),
                ComportamientosTransversales = ConvertirListaComportamientoTransversal(input.ComportamientoTransversal.ToList())
            };
        }

        internal static List<CComportamientoTransversalDTO> ConvertirListaComportamientoTransversal(List<ComportamientoTransversal> comportamientos)
        {
            List<CComportamientoTransversalDTO> resultado = new List<CComportamientoTransversalDTO>();

            foreach (var item in comportamientos)
            {
                resultado.Add(ConvertirComportamientoTransversal(item));
            }

            return resultado;
        }

        internal static CComportamientoTransversalDTO ConvertirComportamientoTransversal(ComportamientoTransversal input)
        {
            return new CComportamientoTransversalDTO
            {
                IdEntidad = input.PK_ComportamientoTransversal,
                ComportamientoTransversal = input.DesComportamientoTransversal,
                EvidenciasComportamientoTransversal = ConvertirListaEvidenciaComportamientoTransversal(input.EvidenciaComportamientoTransversal.ToList())
            };
        }

        internal static List<CEvidenciaComportamientoTransversalDTO> ConvertirListaEvidenciaComportamientoTransversal(List<EvidenciaComportamientoTransversal> evidencias)
        {
            List<CEvidenciaComportamientoTransversalDTO> resultado = new List<CEvidenciaComportamientoTransversalDTO>();

            foreach (var item in evidencias)
            {
                resultado.Add(ConvertirEvidenciaComportamientoTransversal(item));
            }

            return resultado;
        }

        internal static CEvidenciaComportamientoTransversalDTO ConvertirEvidenciaComportamientoTransversal(EvidenciaComportamientoTransversal input)
        {
            return new CEvidenciaComportamientoTransversalDTO
            {
                IdEntidad = input.PK_EvidenciaComportamientoTransversal,
                Evidencia = input.DesEvidencia,

            };
        }

        internal static CCompetenciaGrupoOcupacionalDTO ConvertirCompetenciaGrupo(CompetenciaGrupoOcupacional input)
        {
            return new CCompetenciaGrupoOcupacionalDTO
            {
                IdEntidad = input.PK_CompetenciaGrupoOcupacional,
                TipoGrupoOcupacional = input.NomCompetencia,
                Nivel = input.IndNivel,
                ComportamientosGrupo = ConvertirListaComportamientosGrupo(input.ComportamientoGrupoOcupacional.ToList()),
                Cargo = new CCargoDTO { IdEntidad = input.Cargo.PK_Cargo, NombreCargo = input.Cargo.NomCargo }
            };
        }

        internal static List<CComportamientoGrupoOcupacionalDTO> ConvertirListaComportamientosGrupo(List<ComportamientoGrupoOcupacional> input)
        {
            List<CComportamientoGrupoOcupacionalDTO> resultado = new List<CComportamientoGrupoOcupacionalDTO>();

            foreach (var item in input)
            {
                resultado.Add(ConvertirComportamientoGrupo(item));
            }

            return resultado;
        }

        internal static CComportamientoGrupoOcupacionalDTO ConvertirComportamientoGrupo(ComportamientoGrupoOcupacional input)
        {
            return new CComportamientoGrupoOcupacionalDTO
            {
                IdEntidad = input.PK_ComportamientoGrupoOcupacional,
                Comportamiento = input.DesComportamiento,
                EvidenciasGrupo = ConvertirListaEvidenciasGrupo(input.EvidenciaGrupoOcupacional.ToList())
            };
        }

        internal static List<CEvidenciaGrupoOcupacionalDTO> ConvertirListaEvidenciasGrupo(List<EvidenciaGrupoOcupacional> input)
        {
            List<CEvidenciaGrupoOcupacionalDTO> resultado = new List<CEvidenciaGrupoOcupacionalDTO>();

            foreach (var item in input)
            {
                resultado.Add(ConvertirEvidenciaGrupo(item));
            }

            return resultado;
        }

        internal static CEvidenciaGrupoOcupacionalDTO ConvertirEvidenciaGrupo(EvidenciaGrupoOcupacional input)
        {
            return new CEvidenciaGrupoOcupacionalDTO
            {
                IdEntidad = input.PK_EvidenciaGrupoOcupacional,
                Evidencia = input.DesEvidencia
            };
        }

        public CBaseDTO RegistrarCargo(CCargoDTO cargo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                CRespuestaDTO resultado = new CRespuestaDTO();

                if (cargo.IdEntidad == 0)
                {
                    Cargo cargoRegistro = new Cargo
                    {
                        DesProcesoTrabajo = cargo.ProcesoTrabajo,
                        DesProposito = cargo.Proposito,
                        IndClase = cargo.ClaseCargo.IdEntidad,
                        IndEspecialidad = cargo.EspecialidadCargo.IdEntidad,
                        IndJefaturaInmediata = cargo.JefaturaInmediata,
                        IndJefaturaSuperiorInmediata = cargo.JefaturaSuperiorInmediata,
                        IndSeccion = cargo.SeccionCargo.IdEntidad,
                        IndSubespecialidad = cargo.SubespecialidadCargo.IdEntidad,
                        NomCargo = cargo.NombreCargo,
                        DesUbicacionOrganizacional = cargo.UbicacionOrganizacional,
                        DesEstrato = cargo.Estrato
                    };

                    resultado = intermedio.RegistrarCargo(cargoRegistro);
                }
                else
                {
                    resultado = intermedio.ActualizarDescripcionCargo(cargo);
                }

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO ObtenerCargoPorCodigo(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.ObtenerCargoPorCodigo(codigo);

                if (resultado.Codigo > 0)
                {
                    var datos = ConvertirCargoDTOADatos((Cargo)resultado.Contenido);

                    datos.ClaseCargo = (datos.ClaseCargo != null && datos.ClaseCargo.IdEntidad > 0) ? CClaseL.ConstruirClase(contexto.Clase.FirstOrDefault(C => C.PK_Clase == datos.ClaseCargo.IdEntidad)) : null;
                    datos.EspecialidadCargo = (datos.EspecialidadCargo != null && datos.EspecialidadCargo.IdEntidad > 0) ? CEspecialidadL.ConstruirEspecialidad(contexto.Especialidad.FirstOrDefault(C => C.PK_Especialidad == datos.EspecialidadCargo.IdEntidad)) : null;
                    datos.SubespecialidadCargo = (datos.SubespecialidadCargo != null && datos.SubespecialidadCargo.IdEntidad > 0) ? CSubEspecialidadL.ConvertirSubEspecialidadADTO(contexto.SubEspecialidad.FirstOrDefault(C => C.PK_SubEspecialidad == datos.SubespecialidadCargo.IdEntidad)) : null;
                    datos.SeccionCargo = (datos.SeccionCargo != null && datos.SeccionCargo.IdEntidad > 0) ? CSeccionL.ConvertirSeccionADTO(contexto.Seccion.FirstOrDefault(C => C.PK_Seccion == datos.SeccionCargo.IdEntidad)) : null;

                    return datos;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO RegistroResultadosActividades(List<CResultadoCargoDTO> resultados)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);
                foreach (var resultado in resultados)
                {
                    var resultadoTemp = new ResultadoCargo
                    {
                        FK_Cargo = resultado.Cargo.IdEntidad,
                        DesResultadoCargo = resultado.ResultadoCargo
                    };

                    foreach (var actividad in resultado.ActividadesResultado)
                    {
                        resultadoTemp.ActividadResultadoCargo.Add(new ActividadResultadoCargo
                        {
                            DesActividadResultadoCargo = actividad.ActividadResultadoCargo,
                        });
                    }

                    var respuesta = intermedio.RegistrarResultados(resultadoTemp);

                    if (respuesta.Codigo < 0)
                    {
                        throw new Exception(((CErrorDTO)respuesta.Contenido).MensajeError);
                    }
                }

                return resultados.ElementAt(0).Cargo;
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<CBaseDTO> ObtenerResultadosCargo(int codigo)
        {
            try
            {
                List<CBaseDTO> resultados = new List<CBaseDTO>();

                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.ObtenerResultadosCargo(codigo);

                if (resultado.Codigo > 0)
                {

                    foreach (var item in ((List<ResultadoCargo>)resultado.Contenido))
                    {
                        resultados.Add(ConvertirResultadoCargoADTO(item));
                    }

                    return resultados;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
            }
        }

        public CBaseDTO EliminarResultado(int codigo)
        {
            try
            { 
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.EliminarResultadoCargo(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message};
            }
        }

        public CBaseDTO EliminarActividad(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.EliminarActividadResultadoCargo(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO RegistrarFactorCargo(CFactorClasificacionCargoDTO factor)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);
                CRespuestaDTO resultado = new CRespuestaDTO();

                if (factor.IdEntidad == 0)
                {
                    var factorDatos = new FactorClasificacionCargo
                    {
                        FK_Cargo = factor.Cargo.IdEntidad,
                        DesActivosEquiposInsumos = factor.ActivosEquiposInsumos,
                        DesAmbiente = factor.Ambiente,
                        DesCondiciones = factor.Condiciones,
                        DesImpactoGestion = factor.ImpactoGestion,
                        DesIndependencia = factor.Independencia,
                        DesLugares = factor.Lugares,
                        DesModalidadTrabajo = factor.ModalidadTrabajo,
                        DesRelacionesTrabajo = factor.RelacionesTrabajo,
                        DesSupervisionEjercida = factor.SupervisionEjercida
                    };

                    resultado = intermedio.RegistrarFactorCargo(factorDatos);
                }
                else
                {
                    resultado = intermedio.ActualizarFactoresCargo(factor);
                }

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO ObtenerFactoresCargo(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.ObtenerFactoresCargo(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirFactoresADTO((FactorClasificacionCargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO RegistrarRequerimientosEspecificos(CRequerimientoEspecificoCargoDTO requerimiento)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);
                CRespuestaDTO resultado = new CRespuestaDTO();

                if (requerimiento.IdEntidad == 0)
                {
                    var requerimientoDatos = new RequerimientoEspecificoCargo
                    {
                        DesConocimientos = requerimiento.Conocimientos,
                        DesRequisitosEspecificos = requerimiento.RequisitosEspecificos,
                        FK_Cargo = requerimiento.Cargo.IdEntidad
                    };

                    resultado = intermedio.RegistrarRequerimientosEspecificos(requerimientoDatos);
                }
                else
                {
                    resultado = intermedio.ActualizarRequerimientoEspecifico(requerimiento);
                }


                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO ObtenerRequerimientosEspecificos(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.ObtenerRequerimientosEspecificos(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirRequerimientosEspecificos((RequerimientoEspecificoCargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO RegistrarCompetenciasTransversales(List<CCompetenciaTransversalCargoDTO> competenciasTransversales)
        {
            try
            {
                CBaseDTO respuesta = new CBaseDTO();
                CCargoD intermedio = new CCargoD(contexto);

                foreach (var item in competenciasTransversales)
                {
                    var competenciaTransversalDatos = new CompetenciaTransversalCargo
                    {
                        FK_Cargo = item.Cargo.IdEntidad,
                        IndTipoCompetencia = item.TipoCompetencia,
                        IntNivelDominio = item.NivelDominio
                    };

                    var listaComportamientos = new List<ComportamientoTransversal>();

                    foreach (var comportamiento in item.ComportamientosTransversales)
                    {
                        var comportamientoDatos = new ComportamientoTransversal
                        {
                            DesComportamientoTransversal = comportamiento.ComportamientoTransversal
                        };

                        var listaEvidencias = new List<EvidenciaComportamientoTransversal>();

                        foreach (var evidencia in comportamiento.EvidenciasComportamientoTransversal)
                        {
                            var evidenciaDatos = new EvidenciaComportamientoTransversal
                            {
                                DesEvidencia = evidencia.Evidencia
                            };

                            listaEvidencias.Add(evidenciaDatos);
                        }

                        comportamientoDatos.EvidenciaComportamientoTransversal = listaEvidencias;

                        listaComportamientos.Add(comportamientoDatos);
                    }

                    competenciaTransversalDatos.ComportamientoTransversal = listaComportamientos;

                    var resultado = intermedio.RegistrarCompetenciaTransversal(competenciaTransversalDatos);

                    if (resultado.Codigo > 0)
                    {
                        respuesta = ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                    }
                }

                return respuesta;
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<CBaseDTO> ObtenerCompetenciasTransversales(int codigo)
        {
            try
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.ObtenerCompetenciaTransversal(codigo);

                if (resultado.Codigo > 0)
                {
                    foreach (var item in ((List<CompetenciaTransversalCargo>)resultado.Contenido))
                    {
                        respuesta.Add(ConvertirCompetenciaTransversal(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
            }
        }

        public CBaseDTO EliminarCompetenciasTransversales(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.EliminarCompetenciaTransversal(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO RegistrarCompetenciasGrupo(List<CCompetenciaGrupoOcupacionalDTO> competenciasGrupo)
        {
            try
            {
                CBaseDTO respuesta = new CBaseDTO();
                CCargoD intermedio = new CCargoD(contexto);

                foreach (var item in competenciasGrupo)
                {
                    var competenciaGrupo = new CompetenciaGrupoOcupacional
                    {
                        FK_Cargo = item.Cargo.IdEntidad,
                        NomCompetencia = item.TipoGrupoOcupacional,
                        IndNivel = item.Nivel
                    };

                    var listaComportamientos = new List<ComportamientoGrupoOcupacional>();

                    foreach (var comportamiento in item.ComportamientosGrupo)
                    {
                        var comportamientoDatos = new ComportamientoGrupoOcupacional
                        {
                            DesComportamiento = comportamiento.Comportamiento
                        };

                        var listaEvidencias = new List<EvidenciaGrupoOcupacional>();

                        foreach (var evidencia in comportamiento.EvidenciasGrupo)
                        {
                            var evidenciaDatos = new EvidenciaGrupoOcupacional
                            {
                                DesEvidencia = evidencia.Evidencia
                            };

                            listaEvidencias.Add(evidenciaDatos);
                        }

                        comportamientoDatos.EvidenciaGrupoOcupacional = listaEvidencias;

                        listaComportamientos.Add(comportamientoDatos);
                    }

                    competenciaGrupo.ComportamientoGrupoOcupacional = listaComportamientos;

                    var resultado = intermedio.RegistrarCompetenciaGrupoOcupacional(competenciaGrupo);

                    if (resultado.Codigo > 0)
                    {
                        respuesta = ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                    }
                }

                return respuesta;
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<CBaseDTO> ObtenerCompetenciaGrupo(int codigo)
        {
            try
            {
                List<CBaseDTO> respuesta = new List<CBaseDTO>();
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.ObtenerCompetenciaGrupo(codigo);

                if (resultado.Codigo > 0)
                {
                    foreach (var item in ((List<CompetenciaGrupoOcupacional>)resultado.Contenido))
                    {
                        respuesta.Add(ConvertirCompetenciaGrupo(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
            }
        }

        public CBaseDTO EliminarCompetenciaGrupo(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.EliminarCompetenciaGrupo(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO EliminarComportamientoGrupo(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.EliminarComportamientoGrupo(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public CBaseDTO EliminarEvidenciaGrupo(int codigo)
        {
            try
            {
                CCargoD intermedio = new CCargoD(contexto);

                var resultado = intermedio.EliminarEvidenciaGrupo(codigo);

                if (resultado.Codigo > 0)
                {
                    return ConvertirCargoDTOADatos((Cargo)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        #endregion
    }
}
