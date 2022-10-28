using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CFacturaDesarraigoL
    {

        # region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CFacturaDesarraigoL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CFacturaDesarraigoDTO ConstruirFacturaDesarraigo(FacturaDesarraigo item)
        {
            return new CFacturaDesarraigoDTO
            {
                IdEntidad = item.PK_FacturaDesarraigo,
                Emisor = item.EmisorFactura,
                CodigoFactura = item.CodFactura,
                MontoFactura = item.MonFactura,
                FechaFacturacion = Convert.ToDateTime(item.FecFacturacion),
                ObsConcepto = item.ObsConcepto,
                Desarraigo = CDesarraigoL.ConvertirDesarraigoDatosaDTO(item.Desarraigo),
                ViaticoCorrido = (item.ViaticoCorrido != null) ? CViaticoCorridoL.ConstruirViaticoCorridoGeneral(item.ViaticoCorrido) : null
                //GastoTrasnporte = CGastoTransporteL.ConstruirGastoTransporteGeneral(item.GastoTransporte)
            };
        }
        internal static CFacturaDesarraigoDTO ConstruirFacturaViaticoCorrido(FacturaDesarraigo item)
        {
            return new CFacturaDesarraigoDTO
            {
                IdEntidad = item.PK_FacturaDesarraigo,
                Emisor = item.EmisorFactura,
                CodigoFactura = item.CodFactura,
                MontoFactura = item.MonFactura,
                FechaFacturacion = Convert.ToDateTime(item.FecFacturacion),
                ObsConcepto = item.ObsConcepto,
                ViaticoCorrido = (item.ViaticoCorrido != null) ? CViaticoCorridoL.ConstruirViaticoCorridoGeneral(item.ViaticoCorrido) : null
            };
        }
        internal static CFacturaDesarraigoDTO ConstruirFacturaGastoTransporte(FacturaDesarraigo item)
        {
            return new CFacturaDesarraigoDTO
            {
                IdEntidad = item.PK_FacturaDesarraigo,
                Emisor = item.EmisorFactura,
                CodigoFactura = item.CodFactura,
                MontoFactura = item.MonFactura,
                FechaFacturacion = Convert.ToDateTime(item.FecFacturacion),
                ObsConcepto = item.ObsConcepto,
                //GastoTrasnporte = CGastoTransporteL.ConstruirGastoTransporteGeneral(item.GastoTransporte)
            };
        }

        internal static List<CBaseDTO> ConstruirFacturaDesarraigo(IEnumerable<FacturaDesarraigo> items)
        {
            var respuesta = new List<CBaseDTO>();
            foreach (var f in items)
            {
                respuesta.Add(ConstruirFacturaDesarraigo(f));
            }
            return respuesta;
        }
        internal static List<CBaseDTO> ConstruirFacturaViaticoCorrido(IEnumerable<FacturaDesarraigo> items)
        {
            var respuesta = new List<CBaseDTO>();
            foreach (var f in items)
            {
                respuesta.Add(ConstruirFacturaViaticoCorrido(f));
            }
            return respuesta;
        }
        internal static List<CBaseDTO> ConstruirFacturaGastoTransporte(IEnumerable<FacturaDesarraigo> items)
        {
            var respuesta = new List<CBaseDTO>();
            foreach (var f in items)
            {
                respuesta.Add(ConstruirFacturaGastoTransporte(f));
            }
            return respuesta;
        }

        public CBaseDTO AgregarFactura(CDesarraigoDTO desarraigo, CFacturaDesarraigoDTO factura)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                var intermedio = new CFacturaDesarraigoD(contexto);
                var desarraigoDB = new Desarraigo { PK_Desarraigo = desarraigo.IdEntidad };
                var facturaDB = new FacturaDesarraigo
                {
                    CodFactura = factura.CodigoFactura,
                    EmisorFactura = factura.Emisor,
                    FecFacturacion = factura.FechaFacturacion,
                    MonFactura = factura.MontoFactura
                };
                respuesta = intermedio.AgregarFactura(desarraigoDB, facturaDB);
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
        public CBaseDTO AgregarFacturaViaticoCorrido(CViaticoCorridoDTO viaticoC, CFacturaDesarraigoDTO factura)
        {
            CBaseDTO respuesta = new CBaseDTO();
            try
            {
                var intermedio = new CFacturaDesarraigoD(contexto);
                var viaticoCorridoDB = new ViaticoCorrido { PK_ViaticoCorrido = viaticoC.IdEntidad };
                var facturaDB = new FacturaDesarraigo
                {
                    CodFactura = factura.CodigoFactura,
                    EmisorFactura = factura.Emisor,
                    FecFacturacion = factura.FechaFacturacion,
                    MonFactura = factura.MontoFactura
                };
                respuesta = intermedio.AgregarFacturaViaticoCorrido(viaticoCorridoDB, facturaDB);
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
        
        public List<CBaseDTO> ObtenerFacturasDesarraigo(CDesarraigoDTO desarraigo)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO> ();
            try
            {
                CFacturaDesarraigoD intermedio = new CFacturaDesarraigoD(contexto);
                var desarraigoBD = new Desarraigo { PK_Desarraigo = desarraigo.IdEntidad };
                var datos = intermedio.ObtenerFacturasDesarraigo(desarraigoBD);

                if (((CRespuestaDTO)datos).Codigo < 0)
                {
                    respuesta.Add((CErrorDTO)((CRespuestaDTO)datos).Contenido);
                    throw new Exception();
                }
                respuesta.AddRange(CFacturaDesarraigoL.ConstruirFacturaDesarraigo(((List<FacturaDesarraigo>)datos.Contenido)));
            }
            catch
            {
                return respuesta;
            }
            return respuesta;
        }
        public List<CBaseDTO> ObtenerFacturasViaticoCorrido(CViaticoCorridoDTO viaticoC)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CFacturaDesarraigoD intermedio = new CFacturaDesarraigoD(contexto);
                var viaticoCorridoBD = new ViaticoCorrido { PK_ViaticoCorrido = viaticoC.IdEntidad };
                var datos = intermedio.ObtenerFacturasViaticoCorrido(viaticoCorridoBD);

                if (((CRespuestaDTO)datos).Codigo < 0)
                {
                    respuesta.Add((CErrorDTO)((CRespuestaDTO)datos).Contenido);
                    throw new Exception();
                }
                respuesta.AddRange(CFacturaDesarraigoL.ConstruirFacturaDesarraigo(((List<FacturaDesarraigo>)datos.Contenido)));
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
