using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{//
    public class CCalificacionCapacitacionL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CCalificacionCapacitacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CCalificacionCapacitacionDTO ConvertirDatosCapacitacionADto(CalificacionCapacitacion item)
        {
            return new CCalificacionCapacitacionDTO
            {
                IdEntidad = item.PK_CalificacionCapacitacion,
                DesCapacitacion = item.DesCapacitacion,
                DesTemas = item.DesTemas,
                DesObjetivos = item.DesObjetivos,
                DesLinea = item.DesLineaEstrategica,
                IndEstado = item.IndEstado,
                TipoPP = new CTipoPoliticaPublicaDTO
                {
                    IdEntidad = item.TipoPoliticaPublica.PK_TipoPP,
                    DescripcionTipoPP = item.TipoPoliticaPublica.DesTipoPP,
                    SiglaTipoPP = item.TipoPoliticaPublica.DesSiglas
                }
            };
        }

        internal static CalificacionCapacitacion ConvertirDTOCapacitacionADatos(CCalificacionCapacitacionDTO item)
        {
            return new CalificacionCapacitacion
            {
                PK_CalificacionCapacitacion  = item.IdEntidad,
                DesCapacitacion = item.DesCapacitacion,
                DesTemas = item.DesTemas,
                DesObjetivos = item.DesObjetivos,
                DesLineaEstrategica = item.DesLinea,
                IndEstado = item.IndEstado
            };
        }

        public List<CBaseDTO> GuardarCapacitacion(CCalificacionCapacitacionDTO Capacitacion)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionCapacitacionD intermedio = new CCalificacionCapacitacionD(contexto);
                CCalificacionNombramientoD intermedioCalificacionN = new CCalificacionNombramientoD(contexto);

                CalificacionCapacitacion datos = ConvertirDTOCapacitacionADatos(Capacitacion);
                                
                // Calificación
                var entidadCalificacion = intermedioCalificacionN.ObtenerCalificacionFuncionario(Capacitacion.CalificacionFuncionario.IdEntidad);

                if (entidadCalificacion.Codigo != -1)
                {
                    datos.CalificacionNombramientoFuncionarios = (CalificacionNombramientoFuncionarios)entidadCalificacion.Contenido;
                    datos.FK_CalificacionFuncionario = ((CalificacionNombramientoFuncionarios)entidadCalificacion.Contenido).PK_CalificacionNombramiento;
                }
                else
                {
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadCalificacion).Contenido).MensajeError);
                }

                // Tipo Política
                CTipoPoliticaPublicaD intermedioTipoPP = new CTipoPoliticaPublicaD(contexto);
                var entidadTipoPP = intermedioTipoPP.CargarTipoPorID(Capacitacion.TipoPP.IdEntidad);
                if (entidadTipoPP.Codigo != -1)
                {
                    datos.TipoPoliticaPublica = (TipoPoliticaPublica)entidadTipoPP.Contenido;
                }
                else
                {
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadTipoPP).Contenido).MensajeError);
                }

                datos.IndEstado = 1;
                var insertaCN = intermedio.AgregarCapacitacion(datos);
                
                //pregunto si da error
                if (insertaCN.Codigo > 0)
                {
                    respuesta.Add(Capacitacion);
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
               // respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public CBaseDTO AnularCapacitacion(CCalificacionCapacitacionDTO Capacitacion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CCalificacionCapacitacionD intermedio = new CCalificacionCapacitacionD(contexto);

                var modificar = intermedio.EditarCapacitacion(Capacitacion.IdEntidad);

                //pregunto si da error
                if (modificar.Codigo > 0)
                    respuesta = new CBaseDTO { IdEntidad = Capacitacion.IdEntidad };
                else
                    throw new Exception(((CErrorDTO)modificar.Contenido).MensajeError);
            }
            catch (Exception error)
            {
                return new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                };
                // respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }

        public List<CBaseDTO> ObtenerCapacitacion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CCalificacionCapacitacionD intermedio = new CCalificacionCapacitacionD(contexto);

                var dato = intermedio.ConsultarCapacitacion(codigo);
                if (dato.Codigo != -1)
                {
                    var datosCapacitacion = ConvertirDatosCapacitacionADto((CalificacionCapacitacion)dato.Contenido);
                    
                    // 01 Capacitacion
                    respuesta.Add(datosCapacitacion);
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

        #endregion
    }
}