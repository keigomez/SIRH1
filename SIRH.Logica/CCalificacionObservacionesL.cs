using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCalificacionObservacionesL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CCalificacionObservacionesL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCalificacionObservacionesDTO ConvertirDatosObservacionesADto(CalificacionObservaciones item)
        {
            
            return new CCalificacionObservacionesDTO
            {
                IdEntidad = item.PK_Observacion,
                Observacion = item.DesObservacion,
                FecRegistro = Convert.ToDateTime(item.FecRegistro),
                IndEstado = item.IndEstado,
                CalificacionFuncionarioDTO = CCalificacionNombramientoL.ConvertirDatosCCalificacionFuncionarioADto(item.CalificacionNombramientoFuncionarios)
            };
        }

        internal static CalificacionObservaciones ConvertirDTOObservacionesADatos(CCalificacionObservacionesDTO item)
        {
            return new CalificacionObservaciones
            {
                PK_Observacion = item.IdEntidad,
                FecRegistro = item.FecRegistro,
                DesObservacion = item.Observacion,
                IndEstado = item.IndEstado
            };
        }

        public List<CBaseDTO> GuardarObservacion(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo, CCalificacionObservacionesDTO observacion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionObservacionesD intermedio = new CCalificacionObservacionesD(contexto);
                CCalificacionNombramientoD intermedioCal = new CCalificacionNombramientoD(contexto);
                CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);
                
                CalificacionObservaciones datos = ConvertirDTOObservacionesADatos(observacion);

                // Funcionario
                var entidadFunc = intermedioFuncionario.BuscarFuncionarioBase(funcionario.Cedula);
                if (entidadFunc.Codigo != -1)
                {
                    var datoFunc = (Funcionario)entidadFunc.Contenido;
                    var entidadCal = intermedioCal.ObtenerCalificacionNombramientoFuncionario(datoFunc.PK_Funcionario, periodo.IdEntidad);
                    datos.CalificacionNombramientoFuncionarios = (CalificacionNombramientoFuncionarios)entidadCal.Contenido;
                }    
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadFunc).Contenido).MensajeError);


                var insertaObs = intermedio.InsertarObservaciones(datos);

                //pregunto si da error
                if (insertaObs.Codigo > 0)
                {
                    respuesta.Add(observacion);
                }
                else
                {
                    throw new Exception(((CErrorDTO)respuesta[0]).MensajeError);
                }
            }
            catch (Exception error)
            {              
                respuesta.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
                respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public CBaseDTO ModificarObservacion(CCalificacionObservacionesDTO observacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionObservacionesD intermedio = new CCalificacionObservacionesD(contexto);

                CalificacionObservaciones observacionBD = new CalificacionObservaciones
                {
                    PK_Observacion = observacion.IdEntidad,
                    IndEstado = observacion.IndEstado
                };

                var datosEvidencia = intermedio.ActualizarObservacion(observacionBD);

                if (datosEvidencia.Codigo > 0)
                {
                    respuesta = new CBaseDTO { IdEntidad = observacion.IdEntidad };
                }
                else
                {
                    respuesta = ((CErrorDTO)datosEvidencia.Contenido);
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

        public List<CBaseDTO> ObtenerObservacion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionObservacionesD intermedio = new CCalificacionObservacionesD(contexto);
               // CEjecutorActividadD intermedioEjec = new CEjecutorActividadD(contexto);


                var dato = intermedio.ConsultarCalificacionObservaciones(codigo);
                if (dato.Codigo != -1)
                {
                    var datosObservacion = ConvertirDatosObservacionesADto((CalificacionObservaciones)dato.Contenido);
                    
                    // [0] Observacion
                    respuesta.Add(datosObservacion);
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

        public List<List<CBaseDTO>> BuscarCalificacionObservaciones(CPeriodoCalificacionDTO periodo, CFuncionarioDTO funcionario,
                                                                    CCalificacionObservacionesDTO detalle, List<DateTime> fechasRegistro)
        {
            List<List<CBaseDTO>> respuesta = new List<List<CBaseDTO>>();
            CCalificacionObservacionesD intermedio = new CCalificacionObservacionesD(contexto);

            List<CalificacionObservaciones> datosObservaciones = new List<CalificacionObservaciones>();

            if (funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCalificacionObservaciones(datosObservaciones, funcionario.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosObservaciones = (List<CalificacionObservaciones>)resultado.Contenido;
                }
            }

            if (fechasRegistro != null && fechasRegistro.Count > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarCalificacionObservaciones(datosObservaciones, fechasRegistro, "Fecha"));
                if (resultado.Codigo > 0)
                {
                    datosObservaciones = (List<CalificacionObservaciones>)resultado.Contenido;
                }
            }
            
                       
            if (detalle != null && detalle.Observacion != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCalificacionObservaciones(datosObservaciones, detalle.Observacion, "Descripcion"));
                if (resultado.Codigo > 0)
                {
                    datosObservaciones = (List<CalificacionObservaciones>)resultado.Contenido;
                }
            }
            
            if (datosObservaciones.Count > 0)
            {
                foreach (var item in datosObservaciones)
                {
                    List<CBaseDTO> temp = new List<CBaseDTO>();

                    if (periodo != null && periodo.IdEntidad > 0)
                    {
                        if (item.CalificacionNombramientoFuncionarios.FK_PeriodoCalificacion == periodo.IdEntidad)
                        {
                            temp.Add(ConvertirDatosObservacionesADto(item));
                            respuesta.Add(temp);
                        }
                    }
                    else
                    {
                        temp.Add(ConvertirDatosObservacionesADto(item));
                        respuesta.Add(temp);
                    }     
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

        #endregion
    }
}