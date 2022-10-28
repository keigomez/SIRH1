using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleCalificacionSeguimientoL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CDetalleCalificacionSeguimientoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CDetalleCalificacionSeguimientoDTO ConvertirDatosSeguimientoADto(DetalleCalificacionSeguimiento item)
        {
            
            return new CDetalleCalificacionSeguimientoDTO
            {
                IdEntidad = item.PK_Detalle,
                DesEvidencia = item.DesEvidencia,
                DesComentario = item.DesComentario,
                DesOportunidadMejora = item.DesOportunidadMejora,
                DesPlanAccion = item.DesPlanAccion,
                FecRegistro = Convert.ToDateTime(item.FecRegistro),
                FecCierreAccion = Convert.ToDateTime(item.FecCierreAccion),
                JefeInmediato = CFuncionarioL.ConvertirDatosFuncionarioADTO(item.Funcionario),
                Estado = CCatMetaEstadoL.ConvertirEstadoADTO(item.CatMetaEstado),
            };
        }

        internal static DetalleCalificacionSeguimiento ConvertirDTOSeguimientoADatos(CDetalleCalificacionSeguimientoDTO item)
        {
            return new DetalleCalificacionSeguimiento
            {
                PK_Detalle = item.IdEntidad,
                DesEvidencia = item.DesEvidencia,
                DesComentario = item.DesComentario,
                DesOportunidadMejora = item.DesOportunidadMejora,
                DesPlanAccion = item.DesPlanAccion,
                FecRegistro = item.FecRegistro,
                FecCierreAccion = item.FecCierreAccion,
                IdJefatura = item.JefeInmediato.IdEntidad,
                FK_Estado = item.Estado.IdEntidad,
                FK_CalificacionNombramientoFuncionario = item.Calificacion.IdEntidad
            };
        }

        public CBaseDTO GuardarSeguimiento(CDetalleCalificacionSeguimientoDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDetalleCalificacionSeguimientoD intermedio = new CDetalleCalificacionSeguimientoD(contexto);
                CCalificacionNombramientoD intermedioCal = new CCalificacionNombramientoD(contexto);

                DetalleCalificacionSeguimiento datos = ConvertirDTOSeguimientoADatos(detalle);

                var calificacion = intermedioCal.ObtenerCalificacionFuncionario(detalle.Calificacion.IdEntidad);
                if (calificacion.Codigo != -1)
                    datos.CalificacionNombramientoFuncionarios = (CalificacionNombramientoFuncionarios)calificacion.Contenido;
                else
                    throw new Exception(((CErrorDTO)calificacion.Contenido).Mensaje);
                
                var insertaDetalle = intermedio.AgregarDetalleSeguimiento(datos);

                //pregunto si da error
                if (insertaDetalle.Codigo > 0)
                    respuesta = insertaDetalle;
                else
                    throw new Exception(((CErrorDTO)insertaDetalle.Contenido).Mensaje); 
            }
            catch (Exception error)
            {              
                respuesta = new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
            }

            return respuesta;
        }

        public CBaseDTO ModificarSeguimiento(CDetalleCalificacionSeguimientoDTO detalle)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CDetalleCalificacionSeguimientoD intermedio = new CDetalleCalificacionSeguimientoD(contexto);

                DetalleCalificacionSeguimiento seguimientoBD = new DetalleCalificacionSeguimiento
                {
                    PK_Detalle = detalle.IdEntidad,
                    FK_Estado = detalle.Estado.IdEntidad
                };

                var datosSeguimiento = intermedio.ModificarEstado(seguimientoBD);

                if (datosSeguimiento.Codigo > 0)
                    respuesta = new CBaseDTO { IdEntidad = detalle.IdEntidad };
                else
                    respuesta = ((CErrorDTO)datosSeguimiento.Contenido);
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

        public List<CBaseDTO> ObtenerDetalleSeguimiento(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CDetalleCalificacionSeguimientoD intermedio = new CDetalleCalificacionSeguimientoD(contexto);
                
                var dato = intermedio.ObtenerDetalleSeguimiento(codigo);
                if (dato.Codigo != -1)
                {
                    var datosSeguimiento = ConvertirDatosSeguimientoADto((DetalleCalificacionSeguimiento)dato.Contenido);
                    
                    // [0] DetalleSeguimiento
                    respuesta.Add(datosSeguimiento);
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

        public List<CBaseDTO> ListarSeguimientos(CFuncionarioDTO funcionario, CPeriodoCalificacionDTO periodo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CDetalleCalificacionSeguimientoD intermedio = new CDetalleCalificacionSeguimientoD(contexto);
            CFuncionarioD intermedioFuncionario = new CFuncionarioD(contexto);

            List<DetalleCalificacionSeguimiento> datosSeguimiento = new List<DetalleCalificacionSeguimiento>();

            if (funcionario.Cedula != "" && periodo.IdEntidad > 0)
            {
                var datoFuncionario = intermedioFuncionario.BuscarFuncionarioNombramiento(funcionario.Cedula);
                datosSeguimiento = intermedio.CargarDatos(datoFuncionario.PK_Funcionario, periodo.IdEntidad);
            }

            if (datosSeguimiento.Count > 0)
            {
                foreach (var item in datosSeguimiento)
                {
                    if(item.FK_Estado != 5)
                        respuesta.Add(ConvertirDatosSeguimientoADto(item));
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