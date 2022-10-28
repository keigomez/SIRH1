using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SIRH.Logica
{
    public class CDetalleDeduccionViaticoCorridoL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion
        #region Constructor

        public CDetalleDeduccionViaticoCorridoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion
        #region Métodos
        internal static CDetalleDeduccionViaticoCorridoDTO ConvertirDetalleDeduccionViaticoCorridoDTOaDatos(DetalleDeduccionViaticoCorrido item)
        {
            return new CDetalleDeduccionViaticoCorridoDTO
            {
                MovimientoViaticoCorridoDTO = CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO(item.MovimientoViaticoCorrido),
                DesMotivoDTO = item.DesMotivo,
                FecRigeDTO = item.FecRige,
                FecVenceDTO = item.FecVence,
                MontMontoBajarDTO = item.MontMontoBajar,
                MontMontoRebajarDTO = item.MontMontoRebajarConp,
                NumNoDiaDTO = Convert.ToInt32(item.NumNoDias),
                NumSolicitudAccionPDTO = item.NumAccionPersonal,
                TotRebajarDTO = item.TotRebajar,
                IdEntidad = item.PK_DetalleDeduccionViaticoCorrido
            };
        }
        internal static DetalleDeduccionViaticoCorrido ConvertirDetalleDeduccionViaticoCorridoDatosaDTO(CDetalleDeduccionViaticoCorridoDTO item)
        {
            return new DetalleDeduccionViaticoCorrido
            {
                MovimientoViaticoCorrido = CMovimientoViaticoCorridoL.ConvertirMovimientoDTOaDatos(item.MovimientoViaticoCorridoDTO),
                DesMotivo = item.DesMotivoDTO,
                FecRige = item.FecRigeDTO,
                FecVence = item.FecVenceDTO,
                MontMontoBajar = item.MontMontoBajarDTO,
                MontMontoRebajarConp = item.MontMontoRebajarDTO,
                NumNoDias = item.NumNoDiaDTO,
                NumAccionPersonal = item.NumSolicitudAccionPDTO,
                TotRebajar = item.TotRebajarDTO,
                PK_DetalleDeduccionViaticoCorrido = item.IdEntidad
            };
        }
        /// <summary>
        /// Metodo encargado de hacerle logica a agregar deduccion de un viatico corrido
        /// </summary>
        /// <param name="detalleEVC"></param>
        /// <param name="movimientoVC"></param>
        /// <returns></returns>
        public List<CBaseDTO> AgregarDetalleDedcuccionViaticoCorrido(List<CDetalleDeduccionViaticoCorridoDTO> detalleDVC,
                                                      CMovimientoViaticoCorridoDTO movimientoVC)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            // foreach 

            // si no tira error

            // guardado exitoso, envia respuesta a siguiente capa

            try
            {
                CMovimientoViaticoCorridoD intermedioMovimientonVC = new CMovimientoViaticoCorridoD(contexto);

                MovimientoViaticoCorrido datosMovimientonVCBD = new MovimientoViaticoCorrido
                {
                    NumTipo = movimientoVC.Nomtipo,
                    FecMovimiento = movimientoVC.FecMovimientoDTO,
                    ObsObservacion = movimientoVC.ObsObservacionesDTO,
                    PK_MovimientoViaticoCorrido = movimientoVC.IdEntidad,
                    IndEstado = movimientoVC.EstadoDTO
                };

                datosMovimientonVCBD.ViaticoCorrido = contexto.ViaticoCorrido.FirstOrDefault(Q => Q.PK_ViaticoCorrido == movimientoVC.ViaticoCorridoDTO.IdEntidad);
                var insertaMVC = intermedioMovimientonVC.AgregarMovimientoViaticoCorrido(datosMovimientonVCBD);

                //pregunto si da error
                if (insertaMVC.Codigo > 0)
                {
                    respuesta.Add(CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO((MovimientoViaticoCorrido)insertaMVC.Contenido));

                    CDetalleDeduccionViaticoCorridoD intermedioVC = new CDetalleDeduccionViaticoCorridoD(contexto);
                    foreach (var item in detalleDVC)
                    {
                        var mtoBajar = ConvertirFormato(item.MontMontoBajarDTO);
                        var mtoRebajar = ConvertirFormato(item.MontMontoRebajarDTO);
                        var mtoTotal = ConvertirFormato(item.TotRebajarDTO);

                        DetalleDeduccionViaticoCorrido datosDetalle = new DetalleDeduccionViaticoCorrido
                        {
                            PK_DetalleDeduccionViaticoCorrido = item.IdEntidad,
                            DesMotivo = item.DesMotivoDTO,
                            FecRige = item.FecRigeDTO,
                            FecVence = item.FecVenceDTO,
                            NumNoDias = item.NumNoDiaDTO,
                            MontMontoBajar = mtoBajar.ToString(),
                            MontMontoRebajarConp = mtoRebajar.ToString(),
                            TotRebajar = mtoTotal.ToString(),
                            NumAccionPersonal = item.NumSolicitudAccionPDTO
                        };

                        datosDetalle.MovimientoViaticoCorrido = contexto.MovimientoViaticoCorrido.FirstOrDefault(Q => Q.PK_MovimientoViaticoCorrido == ((MovimientoViaticoCorrido)insertaMVC.Contenido).PK_MovimientoViaticoCorrido);

                        var insertarDECVB = intermedioVC.AgregarDetalleDeduccionViaticoCorrido(datosDetalle, datosMovimientonVCBD);
                        // si no tira error
                        if (insertarDECVB.Codigo > 0)
                        {
                            respuesta.Add(CDetalleDeduccionViaticoCorridoL.ConvertirDetalleDeduccionViaticoCorridoDTOaDatos((DetalleDeduccionViaticoCorrido)insertarDECVB.Contenido));
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
                respuesta.Add(((CErrorDTO)((CRespuestaDTO)respuesta[0]).Contenido));
            }

            return respuesta;
        }


        /// <summary>
        /// Metodo encargado de Obtener deduccion de viatico corrido
        /// </summary>
        /// <returns></returns>
        public List<CBaseDTO> ObtenerDeduccionViaticoCorrido(string codigo)
        {
            int cod = Convert.ToInt32(codigo);
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CDetalleDeduccionViaticoCorridoD intermedio = new CDetalleDeduccionViaticoCorridoD(contexto);

            var detalleDeduccionVC = intermedio.ObtenerDeduccionViaticoCorrido(cod);

            if (detalleDeduccionVC.Codigo != -1)
            {
                foreach (var item in (List<DetalleDeduccionViaticoCorrido>)detalleDeduccionVC.Contenido)
                {
                    respuesta.Add(new CDetalleDeduccionViaticoCorridoDTO
                    {
                        IdEntidad = item.PK_DetalleDeduccionViaticoCorrido,
                        DesMotivoDTO = item.DesMotivo,
                        FecRigeDTO = item.FecRige,
                        FecVenceDTO = item.FecVence,
                        MontMontoBajarDTO = item.MontMontoBajar,
                        MontMontoRebajarDTO = item.MontMontoRebajarConp,
                        MovimientoViaticoCorridoDTO = CMovimientoViaticoCorridoL.ConvertirMovimientoDatosaDTO(item.MovimientoViaticoCorrido),
                        NumNoDiaDTO = Convert.ToInt32(item.NumNoDias),
                        NumSolicitudAccionPDTO = item.NumAccionPersonal,
                        TotRebajarDTO = item.TotRebajar
                    });
                }
            }
            else
            {
                respuesta.Add((CErrorDTO)detalleDeduccionVC.Contenido);
            }

            return respuesta;
        }


        private string ConvertirFormato (string s)
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
