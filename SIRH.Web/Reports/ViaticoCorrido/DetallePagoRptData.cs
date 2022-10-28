using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.Web.ViewModels;
using System.Collections.Generic;

namespace SIRH.Web.Reports.ViaticoCorrido
{
    public class DetallePagoRptData
    {
        public string montoM { get; set; }
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string Apell1Funcionario { get; set; }
        public string Apell2Funcionario { get; set; }

        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string Division { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string UbicacionReal { get; set; }
        public string Tel { get; set; }

        public string ProvinciaC { get; set; }
        public string CantonC { get; set; }
        public string DistritoC { get; set; }

        public string ProvinciaD { get; set; }
        public string CantonD { get; set; }
        public string DistritoD { get; set; }
        public string TelD { get; set; }
        public string OtrasSeñas { get; set; }

        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string Carta { get; set; }
        public string TipoNombramiento { get; set; }
        public string FechaInicioN { get; set; }
        public string FechaFinalN { get; set; }
        public string CabinasYorN { get; set; }
        public string Pernocte { get; set; }
        public string Hospedaje { get; set; }
        public string Mes { get; set; }
        public string MontoViatico { get; set; }
        public string MontoTotalRebaja { get; set; }
        public string MontoTotalReintegro { get; set; }
        public string MontoPago { get; set; }

        public string Observaciones { get; set; }

        public string HojaIndividualizada { get; set; }
        public string NumBoleta { get; set; }
        public string FecPago { get; set; }
        public string FecRebajo { get; set; }
        public string MotivoRebajo { get; set; }
        public string MontoRebajo { get; set; }

        public string CodViatico { get; set; }
        public string ReservaRecurso { get; set; }
        public string NumDiasRebajo { get; set; }
        public string NumDiasReintegro { get; set; }

        public string EsReintegro { get; set; }

        internal static DetallePagoRptData GenerarDatosReporteVC(FormularioViaticoCorridoVM dato, string filtros, int i)
        {
            return new DetallePagoRptData
            {
                CedulaFuncionario = dato.Funcionario.Cedula,
                NombreFuncionario = dato.Funcionario.Nombre.TrimEnd() + " " + dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),

                Provincia = dato.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia + " - " +
                            dato.UbicacionTrabajo.Distrito.Canton.NomCanton + " - " +
                            dato.UbicacionTrabajo.Distrito.NomDistrito,
                //Division = dato.Puesto.UbicacionAdministrativa.Division.NomDivision,
                //Direccion = dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion,
                //Departamento = dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento,
                //UbicacionReal = dato.DetallePuesto.OcupacionReal.DesOcupacionReal,
                //Tel = "Pendiente",

                ProvinciaC = dato.UbicacionContrato.Distrito.Canton.Provincia.NomProvincia + " - " +
                             dato.UbicacionContrato.Distrito.Canton.NomCanton + " - " +
                             dato.UbicacionContrato.Distrito.NomDistrito,
                //ProvinciaD = dato.Direccion.Distrito.Canton.Provincia.NomProvincia + " - " +
                //             dato.Direccion.Distrito.Canton.NomCanton + " - " +
                //             dato.Direccion.Distrito.NomDistrito,
                //TelD = "Pendiente",
                //OtrasSeñas = dato.Direccion.DirExacta,

                FechaInicio = dato.ViaticoCorrido.FecInicioDTO.ToShortDateString(),
                FechaFinal = dato.ViaticoCorrido.FecFinDTO.ToShortDateString(),
                Carta = dato.NumCartaPresentacion != null ? dato.NumCartaPresentacion : "",

                //TipoNombramiento = dato.ViaticoCorrido.NombramientoDTO.EstadoNombramiento.DesEstado,
                //FechaInicioN = dato.ViaticoCorrido.NombramientoDTO.FecRige.ToShortDateString(),
                //FechaFinalN = dato.ViaticoCorrido.NombramientoDTO.FecVence.ToShortDateString() != "01/01/0001" ? dato.ViaticoCorrido.NombramientoDTO.FecVence.ToShortDateString() : "",

                //CabinasYorN = dato.Cabinas,
                Pernocte = dato.ViaticoCorrido.PernocteDTO != null ? dato.ViaticoCorrido.PernocteDTO : "",
                Hospedaje = dato.ViaticoCorrido.HospedajeDTO != null ? dato.ViaticoCorrido.HospedajeDTO : "",
                montoM = Convert.ToDecimal(dato.ViaticoCorrido.MontViaticoCorridoDTO).ToString("#,#.00#;(#,#.00#)"),

                //Mes = dato.ViaticoCorridoList[i].FecInicioDTO.ToShortDateString() + " - " + dato.ViaticoCorridoList[i].FecFinDTO.ToShortDateString(),
                MontoViatico = Convert.ToDecimal(dato.ViaticoCorrido.MontViaticoCorridoDTO).ToString("#,#.00#;(#,#.00#)"),
                MontoTotalRebaja = dato.TotalRebajo.ToString("#,#.00#;(#,#.00#)"),
                MontoTotalReintegro = dato.TotalReintegro.ToString("#,#.00#;(#,#.00#)"),
                MontoPago = dato.ViaticoCorrido.Pagos[0].MonPago.ToString("#,#.00#;(#,#.00#)"),

                Observaciones = dato.ViaticoCorrido.ObsViaticoCorridoDTO != null ? dato.ViaticoCorrido.ObsViaticoCorridoDTO : "",

                FecPago = dato.ViaticoCorrido.Pagos[0].FecPago.ToShortDateString(),
                HojaIndividualizada = dato.ViaticoCorrido.Pagos[0].HojaIndividualizada != null ? dato.ViaticoCorrido.Pagos[0].HojaIndividualizada : "",
                NumBoleta = dato.ViaticoCorrido.Pagos[0].NumBoleta != null ? dato.ViaticoCorrido.Pagos[0].NumBoleta : "",
                ReservaRecurso = dato.ViaticoCorrido.Pagos[0].ReservaRecurso != null ? dato.ViaticoCorrido.Pagos[0].ReservaRecurso : "",

                NumDiasRebajo = dato.diasRebajo.ToString(),

                FecRebajo = dato.ViaticoCorrido.Pagos[0].Detalles != null ? dato.ViaticoCorrido.Pagos[0].Detalles[i].FecDiaPago : "",
                MotivoRebajo = dato.ViaticoCorrido.Pagos[0].Detalles != null ? dato.ViaticoCorrido.Pagos[0].Detalles[i].TipoDetalleDTO.DescripcionTipo : "",
                MontoRebajo = dato.ViaticoCorrido.Pagos[0].Detalles != null ? dato.ViaticoCorrido.Pagos[0].Detalles[i].MonPago.ToString("#,#.00#;(#,#.00#)") : "",

                NumDiasReintegro = dato.diasReintegro.ToString(),
                CodViatico = dato.ViaticoCorrido.CodigoViaticoCorrido,
                EsReintegro = dato.ViaticoCorrido.Pagos[0].Detalles != null ? 
                                (dato.ViaticoCorrido.Pagos[0].Detalles[i].TipoDetalleDTO.IdEntidad == 5) ? "1" : "0"
                                :"0"
            };
        }
    }
}
