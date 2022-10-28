using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.DTO;
using SIRH.Datos;


namespace SIRH.Logica
{
    public class CMontoCaucionL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CMontoCaucionL()
        {
            contexto = new SIRHEntities();
        }
        
        #endregion

        #region Métodos
        //Se insertó en ICCaucionService y CCaucionService(DEIVERT)
        public CBaseDTO AgregarMontoCaucion(CMontoCaucionDTO monto)
        {
            CBaseDTO respuesta;

            try
            {
                CMontoCaucionD intermedio = new CMontoCaucionD(contexto);

                MontoCaucion montoBD = new MontoCaucion
                {
                    DesJustificacion = monto.Justificacion,
                    DesMontoCaucion = monto.Descripcion,
                    FecRige = monto.FechaRige,
                    IndEstadoMonto = monto.EstadoMonto,
                    IndNivel = monto.Nivel,
                    MtoColones = monto.MontoColones,
                    FecVencimiento = monto.FechaInactiva.Year > 1 ? monto.FechaInactiva : (DateTime?)null,
                    DesJustificacionInactivo = monto.JustificacionInactiva
                };

                var resultado = intermedio.AgregarMontoCaucion(montoBD);

                if (resultado.Codigo > 0)
                {
                    respuesta = new CMontoCaucionDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }
        //Se insertó en ICCaucionService y CCaucionService(DEIVERT)
        public CBaseDTO EditarMontoCaucion(CMontoCaucionDTO monto)
        {
            CBaseDTO respuesta;

            try
            {
                CMontoCaucionD intermedio = new CMontoCaucionD(contexto);

                MontoCaucion montoBD = new MontoCaucion
                {
                    PK_MontoCaucion = monto.IdEntidad,
                    FecVencimiento = monto.FechaInactiva,
                    DesJustificacionInactivo = monto.JustificacionInactiva
                };

                var resultado = intermedio.EditarMontoCaucion(montoBD);

                if (resultado.Codigo > 0)
                {
                    respuesta = new CMontoCaucionDTO { IdEntidad = Convert.ToInt32(resultado.Contenido) };
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }
        //Se insertó en ICCaucionService y CCaucionService(DEIVERT)
        public List<CBaseDTO> ListarMontosCaucion()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CMontoCaucionD intermedio = new CMontoCaucionD(contexto);

            var entidadMontos = intermedio.ListarMontoCaucion();

            if (entidadMontos.Codigo != -1)
            {
                foreach (var item in (List<MontoCaucion>)entidadMontos.Contenido)
                {
                    respuesta.Add(new CMontoCaucionDTO
                    {
                        IdEntidad = item.PK_MontoCaucion,
                        Nivel = item.IndNivel,
                        MontoColones = Convert.ToDecimal(item.MtoColones),
                        Descripcion = item.DesMontoCaucion,
                        FechaRige = Convert.ToDateTime(item.FecRige),
                        Justificacion = item.DesJustificacion,
                        EstadoMonto = Convert.ToInt32(item.IndEstadoMonto)
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)entidadMontos.Contenido);
            }

            return respuesta;
        }
        //Se insertó en ICCaucionService y CCaucionService(DEIVERT)
        public List<CBaseDTO> BuscarMontosCaucion(CMontoCaucionDTO caucion, List<DateTime> fechas, 
                                                    List<decimal> montos)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {

                CMontoCaucionD intermedio = new CMontoCaucionD(contexto);

                List<MontoCaucion> datos = new List<MontoCaucion>();

                if (caucion.DetalleEstadoMonto != null)
                {
                    caucion.EstadoMonto = DeterminarEstado(caucion.DetalleEstadoMonto);

                    var datosBD = ((CRespuestaDTO)intermedio.BuscarMontosCaucion(datos, caucion.EstadoMonto, "Estado"));

                    if (datosBD.Codigo > 0)
                    {
                        datos = (List<MontoCaucion>)datosBD.Contenido;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datosBD.Contenido).MensajeError);
                    }
                }

                if (caucion.Descripcion != null)
                {
                    var datosBD = ((CRespuestaDTO)intermedio.BuscarMontosCaucion(datos, caucion.Descripcion, "Puesto"));

                    if (datosBD.Codigo > 0)
                    {
                        datos = (List<MontoCaucion>)datosBD.Contenido;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datosBD.Contenido).MensajeError);
                    }
                }

                if (fechas.Count > 0)
                {
                    var datosBD = ((CRespuestaDTO)intermedio.BuscarMontosCaucion(datos, fechas, "Fecha"));

                    if (datosBD.Codigo > 0)
                    {
                        datos = (List<MontoCaucion>)datosBD.Contenido;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datosBD.Contenido).MensajeError);
                    }
                }

                if (montos.Count > 0)
                {
                    var datosBD = ((CRespuestaDTO)intermedio.BuscarMontosCaucion(datos, montos, "Monto"));

                    if (datosBD.Codigo > 0)
                    {
                        datos = (List<MontoCaucion>)datosBD.Contenido;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datosBD.Contenido).MensajeError);
                    }
                }

                if(datos.Count < 1)
                {
                    throw new Exception("No se encontraron datos asociados a la búsqueda determinada.");
                }

                foreach (var item in datos)
                {
                    respuesta.Add(ConvertirMontoCaucionADTO(item));
                }

                return respuesta;
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }
        //Se insertó en ICCaucionService y CCaucionService(DEIVERT)
        public CBaseDTO ObtenerMontoCaucion(int codMontoCaucion)
        {
            CBaseDTO respuesta = new CBaseDTO();

            try
            {
                CMontoCaucionD intermedio = new CMontoCaucionD(contexto);

                var resultado = intermedio.ObtenerMontoCaucion(codMontoCaucion);

                if (resultado.Codigo > 0)
                {
                    respuesta = ConvertirMontoCaucionADTO(((MontoCaucion)resultado.Contenido));
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }

            }
            catch (Exception error)
            {
                respuesta = new CErrorDTO { MensajeError = error.Message };
                return respuesta;
            }
        }

        private int DeterminarEstado(string estado)
        {
            int respuesta = 1;

            if (estado == "Inactivo")
            {
                respuesta = 2;
            }

            return respuesta;
        }

        internal static CMontoCaucionDTO ConvertirMontoCaucionADTO(MontoCaucion item)
        {
            return new CMontoCaucionDTO
            {
                IdEntidad = item.PK_MontoCaucion,
                Nivel = item.IndNivel,
                Descripcion = item.DesMontoCaucion,
                Justificacion = item.DesJustificacion,
                FechaRige = Convert.ToDateTime(item.FecRige),
                EstadoMonto = Convert.ToInt32(item.IndEstadoMonto),
                MontoColones = Convert.ToDecimal(item.MtoColones),
                DetalleEstadoMonto = Convert.ToInt32(item.IndEstadoMonto) == 1 ? "Activo" : "Inactivo",
                FechaInactiva = Convert.ToDateTime(item.FecVencimiento),
                JustificacionInactiva = item.DesJustificacionInactivo
            };
        }

        #endregion
    }
}
