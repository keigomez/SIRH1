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
    public class BusquedaBoletaRptData
    {
        public string NumeroBoleta { get; set; }

        public string NumeroExpediente { get; set; }

        public string FechaEntregaExpediente { get; set; }

        public string FechaCaducidadExpediente { get; set; }

        public string CedulaFuncionario { get; set; }

        public string NombreFuncionario {get; set;}

        public string ApellidoFuncionario { get; set; }


        internal static BusquedaBoletaRptData GenerarReporteBusquedaBoletas(BoletasRecuperadasVM dato, string filtros) {
            return new BusquedaBoletaRptData
            {
                NumeroBoleta = dato.ListaBoletas.FirstOrDefault().IdEntidad.ToString(),
                NumeroExpediente = dato.ListaBoletas.FirstOrDefault().NumeroExpediente,
                FechaEntregaExpediente = dato.ListaBoletas.FirstOrDefault().FechaPrestamo.ToShortDateString(),
                FechaCaducidadExpediente = dato.ListaBoletas.FirstOrDefault().FechaCaducidad.ToShortDateString(),
                CedulaFuncionario = dato.ListaBoletas.FirstOrDefault().CedulaFuncionario,
                NombreFuncionario = dato.ListaBoletas.FirstOrDefault().NombreFuncionarioSolicitado,
                ApellidoFuncionario = dato.ListaBoletas.FirstOrDefault().ApellidoFuncionarioSolicitado
            };
        }
    }
 }
