using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
namespace SIRH.Logica
{
    public class CDetalleDeduccionGastoTransporteL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CDetalleDeduccionGastoTransporteL()
        {
            contexto = new SIRHEntities();
        }

        #endregion
        #region Métodos
        internal static CDetalleDeduccionGastoTransporteDTO ConvertirDetalleDeduccionGastoTransporteDatosaDTO(DetalleDeduccionGastoTransporte item)
        {
            return new CDetalleDeduccionGastoTransporteDTO
            {
                MovimientoGastoTransporteDTO = CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos(item.MovimientoGastoTransporte),
                DesMotivoDTO = item.DesMotivo,
                FecRigeDTO = item.FecRige,
                FecVenceDTO = item.FecVence,
                MontMontoBajarDTO = item.MontMontoBajar,
                MontMontoRebajarDTO = item.MontMontoRebajarConp,
                NumNoDiaDTO = Convert.ToInt32(item.NumNoDias),
                NumSolicitudAccionPDTO = item.NumAccionPersonal,
                TotRebajarDTO = item.TotRebajar,
                IdEntidad = item.PK_DetalleDeduccionGastoTransporte
            };
        }
        internal static DetalleDeduccionGastoTransporte ConvertirMovimientoGastoTransporteDTOaDatos(CDetalleDeduccionGastoTransporteDTO item)
        {
            return new DetalleDeduccionGastoTransporte
            {
                MovimientoGastoTransporte = CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDatosaDTO(item.MovimientoGastoTransporteDTO),
                DesMotivo = item.DesMotivoDTO,
                FecRige = item.FecRigeDTO,
                FecVence = item.FecVenceDTO,
                MontMontoBajar = item.MontMontoBajarDTO,
                MontMontoRebajarConp = item.MontMontoRebajarDTO,
                NumNoDias =  item.NumNoDiaDTO,
                NumAccionPersonal = item.NumSolicitudAccionPDTO,
                TotRebajar = item.TotRebajarDTO,
                PK_DetalleDeduccionGastoTransporte = item.IdEntidad
            };
        }
        /// <summary>
        /// Metodo encargado de hacerle logica a agregar deduccion de un Gasto Transporte
        /// </summary>
        /// <param name="detalleEVC"></param>
        /// <param name="movimientoVC"></param>
        /// <returns></returns>
        public List<CBaseDTO> AgregarDetalleDedcuccionGastoTransporte(List<CDetalleDeduccionGastoTransporteDTO> detalleDGT,
                                                      CMovimientoGastoTransporteDTO GTmovimientoDTO)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            // foreach 

            // si no tira error

            // guardado exitoso, envia respuesta a siguiente capa
            try
            {
                CMovimientoGastoTransporteD intermedioMovimientoGT = new CMovimientoGastoTransporteD(contexto);
                // Llena objeto movimientoGT
                MovimientoGastoTransporte movimientoGT = new MovimientoGastoTransporte
                {                    
                    NumTipo = GTmovimientoDTO.Nomtipo,
                    FecMovimiento = GTmovimientoDTO.FecMovimientoDTO,
                    ObsObservacion = GTmovimientoDTO.ObsObservacionesDTO,
                    PK_MovimientoGastosTransporte = GTmovimientoDTO.IdEntidad,
                    IndEstado = GTmovimientoDTO.EstadoDTO
                };
                movimientoGT.GastoTransporte = contexto.GastoTransporte.FirstOrDefault(Q => Q.PK_GastosTransporte == GTmovimientoDTO.GastoTransporteDTO.IdEntidad);
                var insertaMGT = intermedioMovimientoGT.AgregarMovimientoGastoTransporte(movimientoGT);

                if (insertaMGT.Codigo > 0)
                {
                    respuesta.Add(CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos((MovimientoGastoTransporte)insertaMGT.Contenido));

                    CDetalleDeduccionGastoTransporteD intermedioGT = new CDetalleDeduccionGastoTransporteD(contexto);
                    foreach (var item in detalleDGT)
                    {
                        var mtoBajar = ConvertirFormato(item.MontMontoBajarDTO);
                        //var mtoRebajar = ConvertirFormato(item.MontMontoRebajarDTO);
                        var mtoTotal = ConvertirFormato(item.TotRebajarDTO);

                        //construye objetos detalle GT
                        DetalleDeduccionGastoTransporte datosDetalle = new DetalleDeduccionGastoTransporte
                        {
                            PK_DetalleDeduccionGastoTransporte = item.IdEntidad,
                            DesMotivo = item.DesMotivoDTO,
                            FecRige = item.FecRigeDTO,
                            FecVence = item.FecVenceDTO,
                            NumNoDias = item.NumNoDiaDTO,
                            MontMontoBajar = mtoBajar.ToString(),
                            //MontMontoRebajarConp = mtoRebajar.ToString(),
                            TotRebajar = mtoTotal.ToString(),
                            NumAccionPersonal = item.NumSolicitudAccionPDTO
                        };
                        datosDetalle.MovimientoGastoTransporte = contexto.MovimientoGastoTransporte.FirstOrDefault(Q => Q.PK_MovimientoGastosTransporte == ((MovimientoGastoTransporte)insertaMGT.Contenido).PK_MovimientoGastosTransporte);

                        var insertarDetalleGB = intermedioGT.AgregarDetalleDeduccionGastoTransporte(datosDetalle, movimientoGT);
                        
                        if (insertarDetalleGB.Codigo > 0)
                        {
                            respuesta.Add(ConvertirDetalleDeduccionGastoTransporteDatosaDTO((DetalleDeduccionGastoTransporte)insertarDetalleGB.Contenido));
                        }
                        else
                        {
                            throw new Exception(((CErrorDTO)respuesta[1]).MensajeError);
                        }
                    }
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
                respuesta.Add((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido);
            }

            return respuesta;
        }

        /// <summary>
        /// Metodo encargado de Obtener deduccion de Gasto Transporte
        /// </summary>
        /// <returns></returns>
        public List<CBaseDTO> ObtenerDeduccionGastoTransporte(string codigo)
        {
            int cod = Convert.ToInt32(codigo);
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CDetalleDeduccionGastoTransporteD intermedio = new CDetalleDeduccionGastoTransporteD(contexto);

            var detalleDeduccionGT = intermedio.ObtenerDeduccionGastoTransporte(cod);

            if (detalleDeduccionGT.Codigo != -1)
            {
                foreach (var item in (List<DetalleDeduccionGastoTransporte>)detalleDeduccionGT.Contenido)
                {
                    respuesta.Add(new CDetalleDeduccionGastoTransporteDTO
                    {
                        IdEntidad = item.PK_DetalleDeduccionGastoTransporte,
                        DesMotivoDTO = item.DesMotivo,
                        FecRigeDTO = item.FecRige,
                        FecVenceDTO = item.FecVence,
                        MontMontoBajarDTO = item.MontMontoBajar,
                        MontMontoRebajarDTO = item.MontMontoRebajarConp,
                        MovimientoGastoTransporteDTO = CMovimientoGastoTransporteL.ConvertirMovimientoGastoTransporteDTOaDatos(item.MovimientoGastoTransporte),
                        NumNoDiaDTO = Convert.ToInt32(item.NumNoDias),
                        NumSolicitudAccionPDTO = item.NumAccionPersonal,
                        TotRebajarDTO = item.TotRebajar
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)detalleDeduccionGT.Contenido);
            }

            return respuesta;
        }

        private string ConvertirFormato(string s)
        {
            //decimal valor = 0;
            short result;
            string cadena = "";
            foreach (char c in s)
                if (Int16.TryParse(c.ToString(), out result) || c.ToString() == "," || c.ToString() == ".")
                    if (c.ToString() == ",")
                        cadena += ".";
                    else
                        cadena += c;

            //Decimal.TryParse(cadena, out valor);

            return cadena;
        }
        #endregion
    }
}
