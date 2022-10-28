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

namespace SIRH.Web.Reports.Datos
{
    public class DatosMEDRptDat
    {
        //------------------------------------------------------------------------------------------------------------------------//
        public string DatosGPerido { get; set; }
        public string DatosGNombreInstitucion { get; set; }
        public string DatosGCantidadPI { get; set; }
        public string DatosGOcupadosP { get; set; }
        public string DatosGInterinos { get; set; }
        public string DatosGSInterinos { get; set; }
        public string DatosGCantidadPFRSC { get; set; }
        public string DatosGCantidadPuestosFueraRSC { get; set; }
        public string DatosGPExcluidos { get; set; }
        public string DatosGConfianza { get; set; }
        public string DatosGExeptuados { get; set; }
        public string DatosGOposicon { get; set; }
        public string DatosGOtros { get; set; }
        public string DatosGFuncionariosDRSC { get; set; }
        public string DatosGFuncionariosE { get; set; }
        public string DatosGFuncionariosNE { get; set; }
        //------------------------------------------------------------------------------------------------------------------------//

        public string Estratos { get; set; }
        public string Excelente { get; set; }
        public string ExcelentePorc { get; set; }
        public string MuyBueno { get; set; }
        public string MuyBuenoPorc { get; set; }
        public string Bueno { get; set; }
        public string BuenoPorc { get; set; }
        public string Regular { get; set; }
        public string RegularPorc { get; set; }
        public string Deficiente { get; set; }
        public string DeficientePorc { get; set; }
        public string TotalPEC { get; set; }
        public string TotalPECPorc { get; set; }
        public string NoEvaluados { get; set; }
        public string NoEvaluadosPorc { get; set; }
        public string TotalInstitucional { get; set; }
        public string PorcTotalInstitucional { get; set; }
        

        internal static DatosMEDRptDat GenerarDatosReporte(FormularioDatosEvaluacionVM dato, string filtros)
        {
            if (dato.DatosEvaluacionCC.Count > 0 && dato.DatosGeneralesEvaluacion.Count() > 0)
            {
                return new DatosMEDRptDat
                {
                    DatosGPerido = dato.DatosGeneralesEvaluacion[0].Periodos,
                    DatosGNombreInstitucion = dato.DatosGeneralesEvaluacion[0].NombreInstitucion,
                    DatosGCantidadPI = dato.DatosGeneralesEvaluacion[0].CantPuestosInstitucionales,
                    DatosGOcupadosP = dato.DatosGeneralesEvaluacion[0].Propiedad,
                    DatosGInterinos = dato.DatosGeneralesEvaluacion[0].Interinos,
                    DatosGSInterinos = dato.DatosGeneralesEvaluacion[0].SinInterinos,
                    DatosGCantidadPFRSC = dato.DatosGeneralesEvaluacion[0].CantidadPFRSC,
                    DatosGCantidadPuestosFueraRSC = dato.DatosGeneralesEvaluacion[0].CantidadPuestosFueraRSC,
                    DatosGPExcluidos = dato.DatosGeneralesEvaluacion[0].Excluidos,
                    DatosGConfianza = dato.DatosGeneralesEvaluacion[0].PuestoConfianza,
                    DatosGExeptuados = dato.DatosGeneralesEvaluacion[0].Exceptuados,
                    DatosGOposicon = dato.DatosGeneralesEvaluacion[0].Oposicion,
                    DatosGOtros = dato.DatosGeneralesEvaluacion[0].Otros,
                    DatosGFuncionariosDRSC = dato.DatosGeneralesEvaluacion[0].FuncionariosDentroRSC,
                    DatosGFuncionariosE = dato.DatosGeneralesEvaluacion[0].Evaluados,
                    DatosGFuncionariosNE = dato.DatosGeneralesEvaluacion[0].NoEvaluados,
                    //----------------------------------------------------------------------//
                    Estratos = " " + dato.DatosEvaluacionCC[0].Estratos + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[1].Estratos + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[2].Estratos + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[3].Estratos + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[4].Estratos + "\n",
                    Excelente = " " + dato.DatosEvaluacionCC[0].AbsExcelente + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[1].AbsExcelente + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[2].AbsExcelente + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[3].AbsExcelente + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[4].AbsExcelente + " ",
                    ExcelentePorc = " " + dato.DatosEvaluacionCC[0].PorcExcelente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[1].PorcExcelente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[2].PorcExcelente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[3].PorcExcelente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[4].PorcExcelente + "\n",
                    MuyBueno = " " + dato.DatosEvaluacionCC[0].AbsMuyBueno + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[1].AbsMuyBueno + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[2].AbsMuyBueno + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[3].AbsMuyBueno + "\n" +
                            " " +
                            dato.DatosEvaluacionCC[4].AbsMuyBueno + " ",
                    MuyBuenoPorc = dato.DatosEvaluacionCC[0].PorcMuyBueno + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[1].PorcMuyBueno + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[2].PorcMuyBueno + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[3].PorcMuyBueno + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[4].PorcMuyBueno + "\n",
                    Bueno = " " + dato.DatosEvaluacionCC[0].AbsBueno + "\n" +
                         " " +
                         dato.DatosEvaluacionCC[1].AbsBueno + "\n" +
                         " " +
                         dato.DatosEvaluacionCC[2].AbsBueno + "\n" +
                         " " +
                         dato.DatosEvaluacionCC[3].AbsBueno + "\n" +
                         " " +
                         dato.DatosEvaluacionCC[4].AbsBueno + " ",
                    BuenoPorc = " " + dato.DatosEvaluacionCC[0].AbsMuyBueno + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[1].AbsMuyBueno + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[2].AbsMuyBueno + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[3].AbsMuyBueno + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[4].AbsMuyBueno + "\n",
                    Regular = " " + dato.DatosEvaluacionCC[0].AbsRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[1].AbsRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[2].AbsRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[3].AbsRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[4].AbsRegular + " ",
                    RegularPorc = dato.DatosEvaluacionCC[0].PorcRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[1].PorcRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[2].PorcRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[3].PorcRegular + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[4].PorcRegular + " ",
                    Deficiente = " " + dato.DatosEvaluacionCC[0].AbsDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[1].AbsDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[2].AbsDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[3].AbsDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[4].AbsDeficiente + " ",
                    DeficientePorc = dato.DatosEvaluacionCC[0].PorcDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[1].PorcDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[2].PorcDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[3].PorcDeficiente + "\n" +
                                 " " +
                                 dato.DatosEvaluacionCC[4].PorcDeficiente + "\n",
                    TotalPEC = " " + dato.DatosEvaluacionCC[0].TotalEvaluacion + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[1].TotalEvaluacion + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[2].TotalEvaluacion + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[3].TotalEvaluacion + "\n" +
                             " " +
                             dato.DatosEvaluacionCC[4].TotalEvaluacion + " ",
                    //TotalPECPorc = dato.DatosEvaluacionCC[0].PorcTotalEvalacion + "\n" +
                    //         " " +
                    //         dato.DatosEvaluacionCC[2].PorcTotalEvalacion + "\n" +
                    //         " " +
                    //         dato.DatosEvaluacionCC[3].PorcTotalEvalacion + "\n" +
                    //         " " +
                    //         dato.DatosEvaluacionCC[4].PorcTotalEvalacion + "\n" +
                    //         " " +
                    //         dato.DatosEvaluacionCC[5].PorcTotalEvalacion + "\n" +
                    //         " " +
                    //         dato.DatosEvaluacionCC[1].PorcTotalEvalacion + "\n",
                    //NoEvaluados = dato.DatosEvaluacionCC[0].TotalDatosNoEvaluados,
                    //NoEvaluadosPorc = dato.DatosEvaluacionCC[0].PorcTotalDatosNoEvaluados,
                    //TotalInstitucional = dato.DatosEvaluacionCC[0].TotalInstitucional,
                    //PorcTotalInstitucional = dato.DatosEvaluacionCC[0].PorcTotalInstitucional
                };
            }
            else {
                return new DatosMEDRptDat();
            }
             
        }
    }
}