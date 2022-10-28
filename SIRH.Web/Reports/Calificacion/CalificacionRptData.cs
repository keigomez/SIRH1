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

namespace SIRH.Web.Reports.Calificacion
{
    public class CalificacionRptData
    {
        public string NombreFormulario { get; set; }
        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string Clasificacion { get; set; }
        public string UnidaddondeTrabaja { get; set; }
        public string NombreJefeInmediato { get; set; }
        public string NombreSuperioJefeInmediato { get; set; }
        public string Expediente { get; set; }

        public string TituloPregunta1 { get; set; }
        public string Pregunta1 { get; set; }
        public string Nota1 { get; set; }
        public string TituloPregunta2 { get; set; }
        public string Pregunta2 { get; set; }
        public string Nota2 { get; set; }
        public string TituloPregunta3 { get; set; }
        public string Pregunta3 { get; set; }
        public string Nota3 { get; set; }
        public string TituloPregunta4 { get; set; }
        public string Pregunta4 { get; set; }
        public string Nota4 { get; set; }
        public string TituloPregunta5 { get; set; }
        public string Pregunta5 { get; set; }
        public string Nota5 { get; set; }
        public string TituloPregunta6 { get; set; }
        public string Pregunta6 { get; set; }
        public string Nota6 { get; set; }
        public string TituloPregunta7 { get; set; }
        public string Pregunta7 { get; set; }
        public string Nota7 { get; set; }
        public string TituloPregunta8 { get; set; }
        public string Pregunta8 { get; set; }
        public string Nota8 { get; set; }
        public string TituloPregunta9 { get; set; }
        public string Pregunta9 { get; set; }
        public string Nota9 { get; set; }
        public string TituloPregunta10 { get; set; }
        public string Pregunta10 { get; set; }
        public string Nota10 { get; set; }

        public string DimensionModificada { get; set; }
        public string NivelOriginal { get; set; }
        public string NuevoNivel { get; set; }
        public string NuevaPuntuacion { get; set; }
        public string DiferenciaPuntos { get; set; }
        public string PuntuacionOriginal { get; set; }
        public string CategoriaOriginal { get; set; }
        public string PuntuacionNueva { get; set; }
        public string CategoriaNueva { get; set; }

        public string CalificacionFinalLetra { get; set; }
        public string PuntuacionFinal { get; set; }
        public string Autor { get; set; }
        public string Filtros { get; set; }
        public string IndRatificada { get; set; }
        public string Perido { get; set; }
        public string FecCreacion { get; set; }
        public string FecRatificacion { get; set; }
        public string ObsGeneral { get; set; }
        public string ObsCapacitacion { get; set; }
        public string ObsJustificacionCapacitacion { get; set; }


        internal static CalificacionRptData GenerarDatosReporte(FormularioCalificacionVM dato, string filtros)
        {
            CalificacionRptData datoModificado = new CalificacionRptData();
            var detalleModificado = dato.CalificacionNombramiento.DetalleCalificacionModificado != null ? dato.CalificacionNombramiento.DetalleCalificacionModificado : new List<DTO.CDetalleCalificacionNombramientoDTO>() ;

            datoModificado.DimensionModificada = "";
            datoModificado.NivelOriginal = "";
            datoModificado.NuevoNivel = "";
            datoModificado.NuevaPuntuacion = "";
            datoModificado.DiferenciaPuntos = "";
            datoModificado.PuntuacionOriginal = "";
            datoModificado.CategoriaOriginal = "";
            datoModificado.PuntuacionNueva = "";
            datoModificado.CategoriaNueva = "";

            int diferenciaNota = 0;
            decimal diferenciaTotal = 0;
            decimal notaNueva = 0;

            for (int i = 0; i < detalleModificado.Count; i++)
            {
                if (detalleModificado[i].NumNotasPorPreguntaDTO != dato.Detalle[i].NumNotasPorPreguntaDTO)
                {
                    datoModificado.DimensionModificada += (i + 1).ToString() + " " + "\n";
                    datoModificado.NivelOriginal += CalcularNivel(dato.Detalle[i].NumNotasPorPreguntaDTO).ToString() + " " + "\n";
                    datoModificado.NuevoNivel += CalcularNivel(detalleModificado[i].NumNotasPorPreguntaDTO).ToString() + " " + "\n";
                    diferenciaNota = CalcularNivel(detalleModificado[i].NumNotasPorPreguntaDTO) - CalcularNivel(dato.Detalle[i].NumNotasPorPreguntaDTO);
                    datoModificado.NuevaPuntuacion += diferenciaNota.ToString() + " " + "\n";
                    diferenciaTotal += Convert.ToDecimal(detalleModificado[i].NumNotasPorPreguntaDTO.Replace(",",".")) - Convert.ToDecimal(dato.Detalle[i].NumNotasPorPreguntaDTO.Replace(",", "."));
                }
            }

            if(diferenciaTotal != 0)
            {
                datoModificado.DiferenciaPuntos = diferenciaTotal.ToString();
                datoModificado.PuntuacionOriginal = dato.PuntuacionFinal.ToString();
                datoModificado.CategoriaOriginal = dato.CalificacionFinalLetra;
                notaNueva = dato.PuntuacionFinal + diferenciaTotal;
                datoModificado.PuntuacionNueva = notaNueva.ToString();
                if (notaNueva >= 95 && notaNueva <= 100)
                {
                    datoModificado.CategoriaNueva = "Excelente";
                }
                else if (notaNueva >= 85 && notaNueva < 95)
                {
                    datoModificado.CategoriaNueva = "Muy Bueno";
                }
                else if (notaNueva >= 75 && notaNueva < 85)
                {
                    datoModificado.CategoriaNueva = "Bueno";
                }
                else if (notaNueva < 75)
                {
                    datoModificado.CategoriaNueva = "Regular";
                }
                else if (notaNueva == 0)
                {
                    datoModificado.CategoriaNueva = "Deficiente";
                }
            }

            return new CalificacionRptData
            {
                NombreFormulario = dato.NombreFormulario,
                CedulaFuncionario = dato.Funcionario.Cedula,
                NombreFuncionario = dato.Funcionario.Nombre.TrimEnd() + " " +
                                dato.Funcionario.PrimerApellido.TrimEnd() + " " + dato.Funcionario.SegundoApellido.TrimEnd(),
                Clasificacion = dato.DetallePuesto.Clase.DesClase,
                Expediente = dato.Expediente.NumeroExpediente.ToString(),
                UnidaddondeTrabaja = (dato.Puesto.UbicacionAdministrativa.Division.NomDivision != null ? dato.Puesto.UbicacionAdministrativa.Division.NomDivision.TrimEnd() : "") + "-" +
                                        (dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion != null ? dato.Puesto.UbicacionAdministrativa.DireccionGeneral.NomDireccion.TrimEnd() : "") + "-" +
                                        (dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento != null ? dato.Puesto.UbicacionAdministrativa.Departamento.NomDepartamento.TrimEnd() : "") + "-" +
                                        (dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion != null ? dato.Puesto.UbicacionAdministrativa.Seccion.NomSeccion.TrimEnd() : ""),
                NombreJefeInmediato = dato.CalificacionNombramiento.JefeInmediato.Nombre,
                NombreSuperioJefeInmediato = dato.CalificacionNombramiento.JefeSuperior.Nombre,
                CalificacionFinalLetra = dato.CalificacionFinalLetra,
                PuntuacionFinal = dato.PuntuacionFinal.ToString() + " .pts",
                TituloPregunta1 = dato.Detalle[0].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta1 = dato.Detalle[0].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota1 = dato.Detalle[0].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta2 = dato.Detalle[1].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta2 = dato.Detalle[1].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota2 = dato.Detalle[1].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta3 = dato.Detalle[2].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta3 = dato.Detalle[2].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota3 = dato.Detalle[2].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta4 = dato.Detalle[3].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta4 = dato.Detalle[3].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota4 = dato.Detalle[3].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta5 = dato.Detalle[4].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta5 = dato.Detalle[4].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota5 = dato.Detalle[4].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta6 = dato.Detalle[5].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta6 = dato.Detalle[5].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota6 = dato.Detalle[5].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta7 = dato.Detalle[6].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta7 = dato.Detalle[6].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota7 = dato.Detalle[6].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta8 = dato.Detalle[7].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta8 = dato.Detalle[7].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota8 = dato.Detalle[7].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta9 = dato.Detalle[8].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta9 = dato.Detalle[8].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota9 = dato.Detalle[8].NumNotasPorPreguntaDTO.ToString(),
                TituloPregunta10 = dato.Detalle[9].CatalogoPreguntaDTO.DesTituloPDTO,
                Pregunta10 = dato.Detalle[9].CatalogoPreguntaDTO.DesPreguntaDTO,
                Nota10 = dato.Detalle[9].NumNotasPorPreguntaDTO.ToString(),

                DimensionModificada = datoModificado.DimensionModificada,
                NivelOriginal = datoModificado.NivelOriginal,
                NuevoNivel = datoModificado.NuevoNivel,
                NuevaPuntuacion = datoModificado.NuevaPuntuacion,

                DiferenciaPuntos = datoModificado.DiferenciaPuntos,
                PuntuacionOriginal = datoModificado.PuntuacionOriginal,
                CategoriaOriginal = datoModificado.CategoriaOriginal,
                PuntuacionNueva = datoModificado.PuntuacionNueva,
                CategoriaNueva = datoModificado.CategoriaNueva,

                Perido = dato.Periodos.ToString(),
                IndRatificada = dato.CalificacionNombramiento.IndRatificacionDTO.ToString(),
                ObsCapacitacion = dato.CalificacionNombramiento.ObsCapacitacionDTO,
                ObsGeneral = dato.CalificacionNombramiento.ObsGeneralDTO,
                ObsJustificacionCapacitacion = dato.CalificacionNombramiento.ObsJustificacionCapacitacionDTO,
                FecCreacion = dato.CalificacionNombramiento.FecCreacionDTO.ToShortDateString(),
                FecRatificacion = (dato.CalificacionNombramiento.FecRatificacionDTO != null) ?
                                            (dato.CalificacionNombramiento.FecRatificacionDTO.HasValue) ?
                                                    Convert.ToDateTime(dato.CalificacionNombramiento.FecRatificacionDTO).ToShortDateString() : ""
                                            : "",
                Autor = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()
                //Filtros = filtros
            };
        }

        internal static int CalcularNivel(string nota)
        {
            if(nota == "10")
                return 5;
            if (nota == "8,5")
                return 4;
            if (nota == "7,5")
                return 3;
            if (nota == "6")
                return 2;
            if (nota == "4")
                return 1;

            return 0;
        }
    }
}
