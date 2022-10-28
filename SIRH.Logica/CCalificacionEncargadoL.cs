using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CCalificacionEncargadoL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CCalificacionEncargadoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCalificacionEncargadoDTO ConvertirDatosEncargadoADto(CalificacionEncargado item)
        {
            
            return new CCalificacionEncargadoDTO
            {
                IdEntidad = item.PK_CalificacionEncargado,
                Seccion = CSeccionL.ConvertirSeccionADTO(item.Seccion),
                Funcionario = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Funcionario),
                IndEstado = Convert.ToInt16(item.IndEstado)
            };
        }

        internal static CalificacionEncargado ConvertirDTOEncargadoADatos(CCalificacionEncargadoDTO item)
        {
            return new CalificacionEncargado
            {
                PK_CalificacionEncargado = item.IdEntidad,
                IndEstado = item.IndEstado
            };
        }

        public List<CBaseDTO> GuardarEncargado(CCalificacionEncargadoDTO encargado)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionEncargadoD intermedio = new CCalificacionEncargadoD(contexto);
                CFuncionarioD intermedioFun = new CFuncionarioD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);

                CalificacionEncargado datos = ConvertirDTOEncargadoADatos(encargado);

                // Sección
                var entidadSeccion = intermedioSeccion.CargarSeccionPorID(encargado.Seccion.IdEntidad);

                if (entidadSeccion != null)
                    datos.Seccion = entidadSeccion;
                else
                    throw new Exception("No existe la información de la Sección");


                var entidadFun = intermedioFun.BuscarFuncionarioCedulaBase(encargado.Funcionario.Cedula);
                if (entidadFun.Codigo != -1)
                    datos.Funcionario = (Funcionario)entidadFun.Contenido;
                else
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadFun).Contenido).MensajeError);


                var insertaMeta = intermedio.GuardarEncargado(datos);

                if (insertaMeta.Codigo > 0)
                    respuesta.Add(encargado);
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
            }

            return respuesta;
        }
             

        public List<CBaseDTO> BuscarEncargado(CFuncionarioDTO funcionario, CUbicacionAdministrativaDTO ubicacion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CCalificacionEncargadoD intermedio = new CCalificacionEncargadoD(contexto);

            List<CalificacionEncargado> datosEncargado = new List<CalificacionEncargado>();


            if (funcionario != null && funcionario.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarEncargado(datosEncargado, funcionario.Cedula, "Cedula"));
                if (resultado.Codigo > 0)
                {
                    datosEncargado = (List<CalificacionEncargado>)resultado.Contenido;
                }
            }


            if (ubicacion != null)
            {
                // Por División
                if (ubicacion.Division != null && ubicacion.Division.IdEntidad > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarEncargado(datosEncargado, ubicacion.Division.IdEntidad, "Division"));
                    if (resultado.Codigo > 0)
                        datosEncargado = (List<CalificacionEncargado>)resultado.Contenido;
                }

                // Por Dirección
                if (ubicacion.DireccionGeneral != null && ubicacion.DireccionGeneral.IdEntidad > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarEncargado(datosEncargado, ubicacion.DireccionGeneral.IdEntidad, "Direccion"));
                    if (resultado.Codigo > 0)
                        datosEncargado = (List<CalificacionEncargado>)resultado.Contenido;
                }

                // Por Departamento
                if (ubicacion.Departamento != null && ubicacion.Departamento.IdEntidad > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarEncargado(datosEncargado, ubicacion.Departamento.IdEntidad, "Seccion"));
                    if (resultado.Codigo > 0)
                        datosEncargado = (List<CalificacionEncargado>)resultado.Contenido;
                }

                // Por Sección
                if (ubicacion.Seccion != null && ubicacion.Seccion.IdEntidad > 0)
                {
                    var resultado = ((CRespuestaDTO)intermedio.BuscarEncargado(datosEncargado, ubicacion.Seccion.IdEntidad, "Seccion"));
                    if (resultado.Codigo > 0)
                        datosEncargado = (List<CalificacionEncargado>)resultado.Contenido;
                }
            }

            if (datosEncargado.Count > 0)
            {
                foreach (var item in datosEncargado)            
                    respuesta.Add(ConvertirDatosEncargadoADto(item));
            }
            else
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });

            return respuesta;
        }


        public CBaseDTO ModificarEncargado(CCalificacionEncargadoDTO registro)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionEncargadoD intermedio = new CCalificacionEncargadoD(contexto);

                CalificacionEncargado encargadoBD = new CalificacionEncargado
                {
                    PK_CalificacionEncargado = registro.IdEntidad,
                    FK_Seccion = registro.Seccion.IdEntidad,
                    FK_Funcionario = registro.Funcionario.IdEntidad,
                    IndEstado = registro.IndEstado
                };

                respuesta = intermedio.ActualizarEncargado(encargadoBD);

                if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
                {
                    var mensaje = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception(mensaje.MensajeError);
                }     
                else
                    return respuesta = new CBaseDTO { Mensaje = (((CRespuestaDTO)respuesta).Contenido).ToString() }; ;
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