using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CUbicacionAdministrativaL
    {
        #region Variables

        SIRHEntities contexto;
        CUbicacionAdministrativaD ubicacionDescarga;

        #endregion

        #region Constructor

        public CUbicacionAdministrativaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        //Se insertó en PuestoService el 30/01/2017
        public List<CBaseDTO> DescargarUbicacionAdministrativa(string cedula)
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();
            ubicacionDescarga = new CUbicacionAdministrativaD(contexto);
            var item = ubicacionDescarga.CargarUbicacionAdministrativaCedula(cedula);

            resultado = SetearPresupuesto(resultado, item);

            resultado = SetearUbicacionAdministrativa(resultado, item);

            return resultado;
        }

        internal static CUbicacionAdministrativaDTO ConvertirUbicacionAdministrativaADTO(UbicacionAdministrativa item)
        {
            CUbicacionAdministrativaDTO respuesta = new CUbicacionAdministrativaDTO
            {
                IdEntidad = item.PK_UbicacionAdministrativa,
                Departamento = item.Departamento != null ? CDepartamentoL.ConstruirDepartamentoDTO(item.Departamento) : new CDepartamentoDTO(),
                DesObservaciones = item.DesObservaciones,
                DireccionGeneral = item.DireccionGeneral != null ? CDireccionGeneralL.ConvertirDireccionGeneralADTO(item.DireccionGeneral) : new CDireccionGeneralDTO(),
                Division = item.Division != null ? CDivisionL.ConvertirDivisionADTO(item.Division) : new CDivisionDTO(),
                Seccion = item.Seccion != null ? CSeccionL.ConvertirSeccionADTO(item.Seccion) : new CSeccionDTO(),
                Presupuesto = item.Presupuesto != null ? 
                                    new CPresupuestoDTO {
                                        IdEntidad = item.Presupuesto.PK_Presupuesto,
                                        CodigoPresupuesto = item.Presupuesto.IdPresupuesto } 
                                    : new CPresupuestoDTO ()
            };
            return respuesta;
        }

        internal static UbicacionAdministrativa ConvertirDTOUbicacionAdministrativaADatos(CUbicacionAdministrativaDTO item)
        {
            return new UbicacionAdministrativa
            {
                PK_UbicacionAdministrativa = item.IdEntidad,
                Departamento = CDepartamentoL.ConvertirDepartamentoDTOaDatos(item.Departamento),
                DesObservaciones = item.DesObservaciones,
                DireccionGeneral = CDireccionGeneralL.ConvertirCDireccionGeneralDTOaDatos(item.DireccionGeneral),
                Division = CDivisionL.ConvertirCDivisionDTOaDatos(item.Division),
                Seccion = CSeccionL.ConvertirCSeccionDTOaDatos(item.Seccion),

            };
        }

        private static List<CBaseDTO> SetearUbicacionAdministrativa(List<CBaseDTO> resultado, UbicacionAdministrativa item)
        {
            if (item != null)
            {
                if (item.Division != null)
                {
                    resultado.Add(new CDivisionDTO { IdEntidad = item.Division.PK_Division, NomDivision = item.Division.NomDivision });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
                if (item.DireccionGeneral != null)
                {
                    resultado.Add(new CDireccionGeneralDTO { IdEntidad = item.DireccionGeneral.PK_DireccionGeneral, NomDireccion = item.DireccionGeneral.NomDireccion });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
                if (item.Departamento != null)
                {
                    resultado.Add(new CDepartamentoDTO { IdEntidad = item.Departamento.PK_Departamento, NomDepartamento = item.Departamento.NomDepartamento });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
                if (item.Seccion != null)
                {
                    resultado.Add(new CSeccionDTO { IdEntidad = item.Seccion.PK_Seccion, NomSeccion = item.Seccion.NomSeccion });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
            }

            return resultado;
        }

        private static List<CBaseDTO> SetearPresupuesto(List<CBaseDTO> resultado, UbicacionAdministrativa item)
        {
            if (item != null && item.Presupuesto != null)
            {
                resultado.Add(new CPresupuestoDTO
                {
                    IdEntidad = item.Presupuesto.PK_Presupuesto,
                    IdUnidadPresupuestaria = item.Presupuesto.IdUnidadPresupuestaria,
                    IdDireccionPresupuestaria = item.Presupuesto.IdDireccionPresupuestaria
                });

                if (item.Presupuesto.Programa != null)
                {
                    resultado.Add(new CProgramaDTO { IdEntidad = item.Presupuesto.Programa.PK_Programa, DesPrograma = item.Presupuesto.Programa.DesPrograma });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
            }
            else
            {
                resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
            }

            return resultado;
        }

        private static List<CBaseDTO> SetearPresupuestoFuncionario(List<CBaseDTO> resultado, UbicacionAdministrativa item)
        {
            if (item != null && item.Presupuesto != null)
            {
                resultado.Add(new CPresupuestoDTO
                {
                    IdEntidad = item.Presupuesto.PK_Presupuesto,
                    IdUnidadPresupuestaria = item.Presupuesto.IdUnidadPresupuestaria,
                    IdDireccionPresupuestaria = item.Presupuesto.IdDireccionPresupuestaria,
                    CodigoPresupuesto = item.Presupuesto.IdPresupuesto
                });
                if (item.Presupuesto.Programa != null)
                {
                    resultado.Add(new CProgramaDTO { IdEntidad = item.Presupuesto.Programa.PK_Programa, DesPrograma = item.Presupuesto.Programa.DesPrograma });
                }
                else
                {
                    resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
                }
            }
            else
            {
                resultado.Add(new CBaseDTO { Mensaje = "No se encontraron resultados" });
            }
            return resultado;
        }

        public List<CBaseDTO> BuscarCentroCostosList(string centroCostos)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CUbicacionAdministrativaD intermedio = new CUbicacionAdministrativaD(contexto);
                var datos = intermedio.BuscarCentroCostosList(centroCostos);
                if (datos != null)
                {
                    foreach (var item in datos)
                    {
                        respuesta.Add(new CBaseDTO
                        {
                            Mensaje = item.DesObservaciones
                        });
                    }
                    return respuesta;
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

        public List<CBaseDTO> UbicacionAdministrativaSeccion(int seccion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CUbicacionAdministrativaD intermedio = new CUbicacionAdministrativaD(contexto);

                var resultado = intermedio.UbicacionAdministrativaSeccion(seccion);

                if (resultado.Codigo > 0)
                {
                    foreach (var item in ((List<UbicacionAdministrativa>)resultado.Contenido))
                    {
                        respuesta.Add(ConvertirUbicacionAdministrativaADTO(item));
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
                respuesta.Clear();
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        #endregion


    }
}

