using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleAsignacionGastoTransporteL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region constructor

        public CDetalleAsignacionGastoTransporteL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos
        internal static CDetalleAsignacionGastoTransporteDTO ConvertirDetalleAsignacionGastoTransporteDatosaDTO (DetalleAsignacionGastoTransporte item)
        {
            return new CDetalleAsignacionGastoTransporteDTO
            {
                Ruta = new CGastoTransporteRutasDTO { IdEntidad = item.FK_Ruta },
                NomRutaDTO = item.NomRuta,
                NomFraccionamientoDTO = item.NomFraccionamiento,
                MontTarifa = Convert.ToDecimal(item.MontTarifa),
                NumGaceta = item.NumGaceta
            };
        }
        internal static DetalleAsignacionGastoTransporte ConvertirDetalleAsignacionGastoTransporteDTOaDatos (CDetalleAsignacionGastoTransporteDTO item)
        {
            return new DetalleAsignacionGastoTransporte
            {
                FK_Ruta = item.Ruta.IdEntidad,
                NomRuta = item.NomRutaDTO,
                NomFraccionamiento = item.NomFraccionamientoDTO,
                MontTarifa = item.MontTarifa,
                NumGaceta = item.NumGaceta
            };
        }
        public List<CBaseDTO> ListarAsignacion(int codigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CDetalleAsignacionGastoTrasporteD intermedio = new CDetalleAsignacionGastoTrasporteD(contexto);
                var resultado = intermedio.ListarAsignacion(codigo);
                if (resultado != null)
                {
                    List<CBaseDTO> data = new List<CBaseDTO>();
                    foreach (var item in resultado)
                    {
                        data.Add(new CDetalleAsignacionGastoTransporteDTO
                        {
                            IdEntidad = item.PK_DetalleAsignacionGastoTransporte,
                            NomRutaDTO = item.NomRuta,
                            NomFraccionamientoDTO = item.NomFraccionamiento,
                            MontTarifa = Convert.ToDecimal(item.MontTarifa),
                            //Ruta = CGastoTransporteL.ConvertirRutaDatosaDTO(item.GastoTransporteRutas),
                            //NumGaceta = item.NumGaceta
                        });
                        respuesta = data;
                    }
                }
                else
                {
                    respuesta = new List<CBaseDTO> { new CCalificacionNombramientoDTO { Mensaje = "No se encontraron datos de contacto para este funcionario" } };
                }

                }
            
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }

            return respuesta;
        }
        #endregion
    }
}
