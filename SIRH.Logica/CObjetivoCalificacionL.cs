using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CObjetivoCalificacionL
    {
        #region Variables

        SIRHEntities contexto;
        
        #endregion

        #region constructor

        public CObjetivoCalificacionL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CObjetivoCalificacionDTO ConvertirDatosObjetivoADto(ObjetivoCalificacion item)
        {
            return new CObjetivoCalificacionDTO
            {
                IdEntidad = item.PK_ObjetivoCalificacion,
                Descripcion = item.DesObjetivo,
                Periodo = CCalificacionNombramientoL.ConvertirDatosPeriodoDto(item.PeriodoCalificacion),
                Seccion = CSeccionL.ConvertirSeccionADTO(item.Seccion),
                ActividadPresupuestaria = item.DesActividadPresupuestaria,
                ProductoPrograma = item.DesProductoPrograma,
                IndEstado = item.IndEstado
            };
        }

        internal static ObjetivoCalificacion ConvertirDTOObjetivoADatos(CObjetivoCalificacionDTO item)
        {
            return new ObjetivoCalificacion
            {
                PK_ObjetivoCalificacion = item.IdEntidad,
                DesObjetivo = item.Descripcion,
                DesActividadPresupuestaria = item.ActividadPresupuestaria,
                DesProductoPrograma = item.ProductoPrograma,
                IndEstado = item.IndEstado
            };
        }

        public List<CBaseDTO> GuardarObjetivo(CObjetivoCalificacionDTO objetivo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CObjetivoCalificacionD intermedio = new CObjetivoCalificacionD(contexto);
                CSeccionD intermedioSeccion = new CSeccionD(contexto);
                CCalificacionNombramientoD intermedioCalificacionN = new CCalificacionNombramientoD(contexto);

                ObjetivoCalificacion datos = ConvertirDTOObjetivoADatos(objetivo);
                                
                // Periodo Evaluación
                var entidadPeriodo = intermedioCalificacionN.ObtenerPeriodoCalificacion(objetivo.Periodo.IdEntidad);

                if (entidadPeriodo.Codigo != -1)
                {
                    datos.PeriodoCalificacion = (PeriodoCalificacion)entidadPeriodo.Contenido;
                }
                else
                {
                    throw new Exception(((CErrorDTO)((CRespuestaDTO)entidadPeriodo).Contenido).MensajeError);
                }


                // Sección
                var entidadSeccion = intermedioSeccion.CargarSeccionPorID(objetivo.Seccion.IdEntidad);

                if (entidadSeccion != null)
                    datos.Seccion = entidadSeccion;
                else
                    throw new Exception("No existe la información de la Sección");

                var insertaCN = intermedio.InsertarObjetivoCalificacion(datos);
                
                //pregunto si da error
                if (insertaCN.Codigo > 0)
                {
                    respuesta.Add(objetivo);
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
        
        public List<CBaseDTO> ObtenerObjetivo(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CObjetivoCalificacionD intermedio = new CObjetivoCalificacionD(contexto);
                CCalificacionNombramientoD intermedioCalificacionN = new CCalificacionNombramientoD(contexto);

                var dato = intermedio.ConsultarObjetivoCalificacion(codigo);
                if (dato.Codigo != -1)
                {
                    var datosObjetivo = ConvertirDatosObjetivoADto((ObjetivoCalificacion)dato.Contenido);
                    
                    // 01 Objetivo
                    respuesta.Add(datosObjetivo);
                    
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


        public List<CBaseDTO> BuscarObjetivos(CObjetivoCalificacionDTO objetivo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CObjetivoCalificacionD intermedio = new CObjetivoCalificacionD(contexto);

            List<ObjetivoCalificacion> datosObjetivos = new List<ObjetivoCalificacion>();

            bool buscar = true;

            // Por Periodo
            if (objetivo.Periodo != null && objetivo.Periodo.IdEntidad > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarObjetivoCalificacion(datosObjetivos, objetivo.Periodo.IdEntidad, "Anio"));

                if (resultado.Codigo > 0)
                    datosObjetivos = (List<ObjetivoCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }

            // Por Sección
            if (objetivo.Seccion != null && objetivo.Seccion.IdEntidad > 0 && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarObjetivoCalificacion(datosObjetivos, objetivo.Seccion.IdEntidad, "Seccion"));

                if (resultado.Codigo > 0)
                    datosObjetivos = (List<ObjetivoCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }

            // Por Descripción
            if (objetivo.Descripcion != null && objetivo.Descripcion != "" && buscar)
            {
                var resultado = ((CRespuestaDTO)intermedio.BuscarObjetivoCalificacion(datosObjetivos, objetivo.Descripcion, "Descripcion"));

                if (resultado.Codigo > 0)
                    datosObjetivos = (List<ObjetivoCalificacion>)resultado.Contenido;
                else
                    buscar = false;
            }


            if (datosObjetivos.Count > 0 && buscar)
            {
                foreach (var item in datosObjetivos.Where(Q => Q.IndEstado == 1).OrderBy(Q => Q.Seccion.PK_Seccion).ToList())
                {
                    respuesta.Add(ConvertirDatosObjetivoADto(item));
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