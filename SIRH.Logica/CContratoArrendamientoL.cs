using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CContratoArrendamientoL
    {
         # region Variables 
                
        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CContratoArrendamientoL() {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos 
        
        internal static CContratoArrendamientoDTO ConstruirContratoArrendamiento(ContratoArrendamiento item) {
            return new CContratoArrendamientoDTO {
                IdEntidad = item.PK_ContratoArrendamiento,
                EmisorContrato = item.EmisorContrato,
                CodigoContratoArrendamiento = item.CodContrato,
                MontoContrato = item.MonContrato,
                FechaInicio = Convert.ToDateTime(item.FecInicial),
                FechaFin = Convert.ToDateTime(item.FecFinal),
                Desarraigo = CDesarraigoL.ConvertirDesarraigoDatosaDTO(item.Desarraigo),
                ViaticoCorrido = (item.ViaticoCorrido != null) ? CViaticoCorridoL.ConstruirViaticoCorridoGeneral(item.ViaticoCorrido) : null
                //GastoTransporte = CGastoTransporteL.ConstruirGastoTransporteGeneral(item.GastoTransporte)

            };
        }
        internal static CContratoArrendamientoDTO ConstruirContratoArrendamientoViaticoCorrido(ContratoArrendamiento item)
        {
            return new CContratoArrendamientoDTO
            {
                IdEntidad = item.PK_ContratoArrendamiento,
                EmisorContrato = item.EmisorContrato,
                CodigoContratoArrendamiento = item.CodContrato,
                MontoContrato = item.MonContrato,
                FechaInicio = Convert.ToDateTime(item.FecInicial),
                FechaFin = Convert.ToDateTime(item.FecFinal),
                ViaticoCorrido = (item.ViaticoCorrido != null) ? CViaticoCorridoL.ConstruirViaticoCorridoGeneral(item.ViaticoCorrido) : null

            };
        }
        internal static CContratoArrendamientoDTO ConstruirContratoArrendamientoGastoTransporte(ContratoArrendamiento item)
        {
            return new CContratoArrendamientoDTO
            {
                IdEntidad = item.PK_ContratoArrendamiento,
                EmisorContrato = item.EmisorContrato,
                CodigoContratoArrendamiento = item.CodContrato,
                MontoContrato = item.MonContrato,
                FechaInicio = Convert.ToDateTime(item.FecInicial),
                FechaFin = Convert.ToDateTime(item.FecFinal),
               // GastoTransporte = CGastoTransporteL.ConstruirGastoTransporteGeneral(item.GastoTransporte)

            };
        }

        internal static List<CBaseDTO> ConstruirContratoArrendamiento(IEnumerable<ContratoArrendamiento> items) {
            var respuesta = new List<CBaseDTO>();
            foreach (var c in items)
            {
                respuesta.Add(ConstruirContratoArrendamiento(c));
            }
            return respuesta;
        }
        internal static List<CBaseDTO> ConstruirContratoArrendamientoViaticoCorrido(IEnumerable<ContratoArrendamiento> items)
        {
            var respuesta = new List<CBaseDTO>();
            foreach (var c in items)
            {
                respuesta.Add(ConstruirContratoArrendamientoViaticoCorrido(c));
            }
            return respuesta;
        }
        internal static List<CBaseDTO> ConstruirContratoArrendamientoGastoTransporte(IEnumerable<ContratoArrendamiento> items)
        {
            var respuesta = new List<CBaseDTO>();
            foreach (var c in items)
            {
                respuesta.Add(ConstruirContratoArrendamientoGastoTransporte(c));
            }
            return respuesta;
        }

        public CBaseDTO AgregarContratoArrendamiento(CDesarraigoDTO desarraigo,CContratoArrendamientoDTO contrato) {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                var intermedio = new CContratoArrendamientoD(contexto);
                var desarraigoDB = new Desarraigo{PK_Desarraigo = desarraigo.IdEntidad};
                var contratoDB = new ContratoArrendamiento{CodContrato = contrato.CodigoContratoArrendamiento,
                                                           EmisorContrato = contrato.EmisorContrato,
                                                           FecFinal = contrato.FechaFin,
                                                           FecInicial = contrato.FechaInicio,
                                                           MonContrato = contrato.MontoContrato};
                respuesta = intermedio.AgregarContrArrendamiento(desarraigoDB,contratoDB);
                if (((CRespuestaDTO)respuesta).Codigo < 0)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                return respuesta;
            }
            catch {
                return respuesta;
            }
        }
        public CBaseDTO AgregarContratoArrendamientoViaticoCorrido(CViaticoCorridoDTO viaticoC, CContratoArrendamientoDTO contrato)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                var intermedio = new CContratoArrendamientoD(contexto);
                var viaticoCorridoDB = new ViaticoCorrido { PK_ViaticoCorrido = viaticoC.IdEntidad };
                var contratoDB = new ContratoArrendamiento
                {
                    CodContrato = contrato.CodigoContratoArrendamiento,
                    EmisorContrato = contrato.EmisorContrato,
                    FecFinal = contrato.FechaFin,
                    FecInicial = contrato.FechaInicio,
                    MonContrato = contrato.MontoContrato
                };
                respuesta = intermedio.AgregarContrArrendamientoViaticoCorrido(viaticoCorridoDB, contratoDB);
                if (((CRespuestaDTO)respuesta).Codigo < 0)
                {
                    respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
                    throw new Exception();
                }
                return respuesta;
            }
            catch
            {
                return respuesta;
            }
        }
      

        public List<CBaseDTO> ObtenerContratosArrendamientos(CDesarraigoDTO desarraigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CContratoArrendamientoD intermedio = new CContratoArrendamientoD(contexto);
                var desarraigoBD = new Desarraigo { PK_Desarraigo = desarraigo.IdEntidad };
                var datos = intermedio.ObtenerContrArrendamientoDesarraigo(desarraigoBD);

                if (((CRespuestaDTO)datos).Codigo < 0)
                {
                    respuesta.Add((CErrorDTO)((CRespuestaDTO)datos).Contenido);
                    throw new Exception();
                }
                respuesta.AddRange(ConstruirContratoArrendamiento(((List<ContratoArrendamiento>)datos.Contenido)));
            }
            catch
            {
                return respuesta;
            }
            return respuesta;

        }
        public List<CBaseDTO> ObtenerContratosArrendamientosViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CContratoArrendamientoD intermedio = new CContratoArrendamientoD(contexto);
                var viaticoCorridoBD = new ViaticoCorrido { PK_ViaticoCorrido = viaticoC.IdEntidad };
                var datos = intermedio.ObtenerContrArrendamientoViaticoCorrido(viaticoCorridoBD);

                if (((CRespuestaDTO)datos).Codigo < 0)
                {
                    respuesta.Add((CErrorDTO)((CRespuestaDTO)datos).Contenido);
                    throw new Exception();
                }
                respuesta.AddRange(ConstruirContratoArrendamiento(((List<ContratoArrendamiento>)datos.Contenido)));
            }
            catch
            {
                return respuesta;
            }
            return respuesta;

        }
        

        #endregion

    }
}
