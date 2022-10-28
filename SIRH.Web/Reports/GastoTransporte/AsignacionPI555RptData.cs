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

namespace SIRH.Web.Reports.GastoTransporte
{
    public class AsignacionPI555RptData
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
        public string CodigoGasto { get; set; }
        public string Carta { get; set; }
        public string TipoNombramiento { get; set; }
        public string FechaInicioN { get; set; }
        public string FechaFinalN { get; set; }

        public string monto { get; set; }
        public string Mes { get; set; }
        public string Monto { get; set; }
        public string TotalM { get; set; }

        public string Ruta { get; set; }
        public string Fraccion { get; set; }
        public string tarifa { get; set; }

        public string CodPresupuestario { get; set; }
        public string Programa { get; set; }
        public string Area { get; set; }
        public string Actividad { get; set; }

        public string MesesAPagar { get; set; }
        public string MontoMesesPagar { get; set; }

        internal static AsignacionPI555RptData GenerarDatosReporte(FormularioGastoTransporteVM dato, string mesesPagar, string montoMesesP, int i, int k, int h)
        {
            return new AsignacionPI555RptData
            {
                monto = "PI-555",
                CedulaFuncionario = dato.Funcionario.Cedula,
                Apell1Funcionario = dato.Funcionario.PrimerApellido,
                Apell2Funcionario = dato.Funcionario.SegundoApellido,
                NombreFuncionario = dato.Funcionario.Nombre,

                Provincia = dato.UbicacionTrabajo.Distrito.Canton.Provincia.NomProvincia,
                Canton = dato.UbicacionTrabajo.Distrito.Canton.NomCanton,
                Distrito = dato.UbicacionTrabajo.Distrito.NomDistrito,
                Division = dato.Puesto.UbicacionAdministrativa.Division.NomDivision,
                Direccion = dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion,
                Departamento = dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento,
                UbicacionReal = dato.DetallePuesto.OcupacionReal.DesOcupacionReal,
                Tel = "Pendiente",

                ProvinciaC = dato.UbicacionContrato.Distrito.Canton.Provincia.NomProvincia,
                CantonC = dato.UbicacionContrato.Distrito.Canton.NomCanton,
                DistritoC = dato.UbicacionContrato.Distrito.NomDistrito,
                ProvinciaD = dato.Direccion.Distrito.Canton.Provincia.NomProvincia,
                CantonD = dato.Direccion.Distrito.Canton.NomCanton,
                DistritoD = dato.Direccion.Distrito.NomDistrito,
                TelD = "Pendiente",
                OtrasSeñas = dato.Direccion.DirExacta,

                FechaInicio = dato.GastoTransporte.FecInicioDTO.ToShortDateString() == "01/01/0001" ? "" : dato.GastoTransporte.FecInicioDTO.ToShortDateString(),
                FechaFinal = dato.GastoTransporte.FecFinDTO.ToShortDateString() == "01/01/0001" ? "" : dato.GastoTransporte.FecFinDTO.ToShortDateString(),
                montoM = Convert.ToDouble(dato.GastoTransporte.MontGastoTransporteDTO).ToString("#,#.00#;(#,#.00#)"),
                CodigoGasto = dato.GastoTransporte.CodigoGastoTransporte,

                Carta = dato.NumCartaPresentacion,
                TipoNombramiento = dato.Nombramiento.EstadoNombramiento.DesEstado,
                FechaInicioN = dato.Nombramiento.FecRige.ToShortDateString() == "01/01/0001" ? "" : dato.Nombramiento.FecRige.ToShortDateString(),
                FechaFinalN = dato.Nombramiento.FecVence.ToShortDateString() == "01/01/0001" ? "" : dato.Nombramiento.FecVence.ToShortDateString(),

                Mes = h < dato.GastoTransporteList[i].Pagos.Count ? dato.GastoTransporteList[i].Pagos[h].FecPago.ToShortDateString() : "-",
                Monto = h < dato.GastoTransporteList[i].Pagos.Count ? dato.GastoTransporteList[i].Pagos[h].MonPago.ToString() : "-",
                TotalM = dato.TotalMA.ToString("#,#.00#;(#,#.00#)"),
                
                Ruta = k < dato.Rutas.Count ? dato.Rutas[k].NomRutaDTO : "--", // dato.Rutas[k] != null ? dato.Rutas[k].NomRutaDTO : "---",
                Fraccion = k < dato.Rutas.Count ? dato.Rutas[k].NomFraccionamientoDTO : "--", //dato.Rutas[k] != null ? dato.Rutas[k].NomFraccionamientoDTO : "Sin ruta",
                tarifa = k < dato.Rutas.Count ? Convert.ToDouble(dato.Rutas[k].MontTarifa).ToString("#,#.00#;(#,#.00#)") : "--", //dato.Rutas[k] != null ? dato.Rutas[k].MontTarifa : "0",   
                
                //Informacion Presupuesto
                CodPresupuestario = dato.GastoTransporteList[i].PresupuestoDTO.CodigoPresupuesto,
                Programa = dato.GastoTransporteList[i].PresupuestoDTO.Programa.DesPrograma,
                Area = dato.GastoTransporteList[i].PresupuestoDTO.Area,
                Actividad = dato.GastoTransporteList[i].PresupuestoDTO.Actividad,

                MesesAPagar = mesesPagar,
                MontoMesesPagar = montoMesesP
            };
        }
        internal static AsignacionPI555RptData llenaRuta(FormularioGastoTransporteVM dato, string filtros, int k)
        {
            return new AsignacionPI555RptData
            {
                Ruta = dato.Rutas[k].NomRutaDTO,
                Fraccion = dato.Rutas[k].NomFraccionamientoDTO,
                //tarifa = dato.Rutas[k].MontTarifa,
            };
        }
        
 
    }
}
