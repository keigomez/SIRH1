using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CActividadFuncionarioL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CActividadFuncionarioL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CActividadFuncionarioDTO ConvertirDatosActividadADto(ActividadFuncionario item)
        {
            
            return new CActividadFuncionarioDTO
            {
                IdEntidad = item.PK_Actividad,
                Descripcion = item.DesActividad,
                Observaciones = item.DesObservacion,
                Evidencia = item.DesEvidencia,
                FechaDesde = Convert.ToDateTime(item.FecDesde),
                FechaHasta = Convert.ToDateTime(item.FecHasta),
                FechaRegistro = Convert.ToDateTime(item.FecRegistro),
                IndEstado = Convert.ToInt16(item.IndEstado),
                IndTeletrabajo = Convert.ToInt16(item.IndTeletrabajo),
                Funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Funcionario)
            };
        }

        internal static ActividadFuncionario ConvertirDTOActividadADatos(CActividadFuncionarioDTO item)
        {
            return new ActividadFuncionario
            {
                DesActividad = item.Descripcion,
                DesObservacion = item.Observaciones,
                DesEvidencia = item.Evidencia,
                FecDesde = item.FechaDesde,
                FecHasta = item.FechaHasta,
                FecRegistro = item.FechaRegistro,
                IndEstado = item.IndEstado,
                IndTeletrabajo = item.IndTeletrabajo
            };
        }

        public List<CBaseDTO> GuardarActividad(CActividadFuncionarioDTO actividad)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CActividadFuncionarioD intermedio = new CActividadFuncionarioD(contexto);
                CFuncionarioD intermedioFun = new CFuncionarioD(contexto);

                ActividadFuncionario datos = ConvertirDTOActividadADatos(actividad);

                // Funcionario
                var entidadFun = intermedioFun.BuscarFuncionarioCedulaBase(actividad.Funcionario.Cedula);
                if (entidadFun.Codigo != -1)
                    datos.Funcionario = (Funcionario)entidadFun.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadFun).Contenido).MensajeError);


                var insertaAct = intermedio.InsertarActividadFuncionario(datos);

                if (insertaAct.Codigo > 0)
                    respuesta.Add(actividad);
                else
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                //respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerActividad(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CActividadFuncionarioD intermedio = new CActividadFuncionarioD(contexto);
                
                var dato = intermedio.ConsultarActividadFuncionario(codigo);
                if (dato.Codigo != -1)
                {
                    var datosActividad = ConvertirDatosActividadADto((ActividadFuncionario)dato.Contenido);
                    // [0] Actividad
                    respuesta.Add(datosActividad);
                }
                else
                {
                    respuesta.Add((CErrorDTO)dato.Contenido);
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

        public CBaseDTO ModificarActividad(CActividadFuncionarioDTO actividad)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CActividadFuncionarioD intermedio = new CActividadFuncionarioD(contexto);

                ActividadFuncionario actividadBD = new ActividadFuncionario
                {
                    PK_Actividad = actividad.IdEntidad,
                    DesObservacion = actividad.Observaciones,
                    IndEstado = actividad.IndEstado
                };

                var datosActividad = intermedio.ActualizarActividad(actividadBD);

                if (datosActividad.Codigo > 0)
                    respuesta = new CBaseDTO { IdEntidad = actividad.IdEntidad };
                else
                    respuesta = ((CErrorDTO)datosActividad.Contenido);
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

        public List<CBaseDTO> BuscarActividades(CActividadFuncionarioDTO actividad, CFuncionarioDTO funcionario,
                                                List<DateTime> fechas)
        {
            CActividadFuncionarioD intermedio = new CActividadFuncionarioD(contexto);

            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            List<ActividadFuncionario> datosActividades = new List<ActividadFuncionario>();

            bool buscar = true;
            
            // Funcionario
            if (funcionario != null && funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarActividadFuncionario(datosActividades, funcionario.Cedula, "Cedula"));
                if (resultado.Codigo > 0)
                    datosActividades = (List<ActividadFuncionario>)resultado.Contenido;
                else
                {
                    datosActividades.Clear();
                    buscar = false;
                }
            }

            
            // Fechas
            if (fechas != null && fechas.Count > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarActividadFuncionario(datosActividades, fechas, "Fecha"));

                if (resultado.Codigo > 0)
                    datosActividades = (List<ActividadFuncionario>)resultado.Contenido;
                else
                {
                    datosActividades.Clear();
                    buscar = false;
                }
            }


            // Estado
            if (actividad != null && actividad.IndEstado > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarActividadFuncionario(datosActividades, actividad.IndEstado, "Estado"));

                if (resultado.Codigo > 0)
                    datosActividades = (List<ActividadFuncionario>)resultado.Contenido;
            }
            else
                datosActividades = datosActividades.Where(Q => Q.IndEstado > 0).ToList();

            
            if (datosActividades.Count > 0)
            {
                datosActividades = datosActividades.OrderBy(Q => Q.Funcionario.IdCedulaFuncionario)
                                                    .ThenBy(Q => Q.FecDesde).ThenBy(Q => Q.FecHasta).ToList();
                foreach (var item in datosActividades)
                {
                    respuesta.Add(ConvertirDatosActividadADto(item));
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }

        #endregion
    }
}