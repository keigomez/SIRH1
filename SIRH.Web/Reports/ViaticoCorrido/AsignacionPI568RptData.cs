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
    public class AsignacionPI568RptData
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
        public string HojaIndividualizada { get; set; }
        public string TipoNombramiento { get; set; }
        public string FechaInicioN { get; set; }
        public string FechaFinalN { get; set; }
        public string CabinasYorN { get; set; }
        public string Pernocte { get; set; }
        public string Hospedaje { get; set; }
        public string monto { get; set; }
        public string Mes { get; set; }
        public string Monto { get; set; }
        public string TotalM { get; set; }

        public string Observaciones { get; set; }

        internal static AsignacionPI568RptData GenerarDatosReporte(FormularioViaticoCorridoVM dato, string filtros, int i)
        {
            return new AsignacionPI568RptData
            {
                monto = "PI-568",
                CedulaFuncionario = dato.Funcionario.Cedula,
                Apell1Funcionario = dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Apell2Funcionario = dato.Funcionario.SegundoApellido.TrimEnd(),
                NombreFuncionario = dato.Funcionario.Nombre.TrimEnd(),

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

                FechaInicio = dato.ViaticoCorrido.FecInicioDTO.ToShortDateString(),
                FechaFinal = dato.ViaticoCorrido.FecFinDTO.ToShortDateString(),
                Carta = dato.NumCartaPresentacion,
                //HojaIndividualizada = dato.ViaticoCorrido.p.HojaIndividualizadaDTO,

                TipoNombramiento = dato.ViaticoCorrido.NombramientoDTO.EstadoNombramiento.DesEstado,
                FechaInicioN = dato.ViaticoCorrido.NombramientoDTO.FecRige.ToShortDateString(),
                FechaFinalN = dato.ViaticoCorrido.NombramientoDTO.FecVence.ToShortDateString() != "01/01/0001" ? dato.ViaticoCorrido.NombramientoDTO.FecVence.ToShortDateString() : "",

                CabinasYorN = dato.Cabinas,
                Pernocte = dato.ViaticoCorrido.PernocteDTO,
                Hospedaje = dato.ViaticoCorrido.HospedajeDTO,
                montoM = Convert.ToDecimal(dato.ViaticoCorrido.MontViaticoCorridoDTO).ToString("#,#.00#;(#,#.00#)"),

                Mes = dato.ViaticoCorridoList[i].FecInicioDTO.ToShortDateString() + " - " + dato.ViaticoCorridoList[i].FecFinDTO.ToShortDateString(),
                Monto = Convert.ToDecimal(dato.ViaticoCorridoList[i].MontViaticoCorridoDTO).ToString("#,#.00#;(#,#.00#)"),
                TotalM = dato.TotalMA.ToString("#,#.00#;(#,#.00#)"),

                Observaciones = dato.ViaticoCorrido.ObsViaticoCorridoDTO

            };
        }

    }
}
