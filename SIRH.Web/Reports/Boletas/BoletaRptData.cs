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

namespace SIRH.Web.Reports.Boletas
{
    public class BoletaRptData
    {

        public string Solicitante { get; set; }
        public string Funcionario { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public string Departamento { get; set; }

        public string MotivoPrestamo { get; set; }

        public string NumeroExpediente { get; set; }

        public string FechaPrestamo { get; set; }

        public string FechaCaducidad { get; set; }

        public string NumeroBoleta { get; set; }

        public string Filtros { get; set; }


        internal static BoletaRptData GenerarDatosReporte(FormularioBoletaPrestamoVM dato, string filtros) {
            return new BoletaRptData
            {
                Solicitante = dato.BoletaPrestamo.CedulaSolicitante +" "+dato.BoletaPrestamo.NombreSolicitante +" "+ dato.BoletaPrestamo.ApellidoSolicitante,
                Funcionario = dato.BoletaPrestamo.CedulaFuncionario + " "+dato.BoletaPrestamo.NombreFuncionarioSolicitado +" "+ dato.BoletaPrestamo.ApellidoFuncionarioSolicitado,
                Telefono = dato.BoletaPrestamo.Telefonolicitante,
                Correo = dato.BoletaPrestamo.CorreoSolicitante,
                Departamento = dato.BoletaPrestamo.DepartamentoFuncionario,
                MotivoPrestamo = dato.BoletaPrestamo.MotivoPrestamo,
                NumeroExpediente = dato.BoletaPrestamo.NumeroExpediente,
                FechaPrestamo = dato.BoletaPrestamo.FechaPrestamo.ToShortDateString(),
                FechaCaducidad = dato.BoletaPrestamo.FechaCaducidad.ToShortDateString(),
                NumeroBoleta = dato.BoletaPrestamo.IdEntidad.ToString(),
                Filtros = filtros
            };
        }
    }
}