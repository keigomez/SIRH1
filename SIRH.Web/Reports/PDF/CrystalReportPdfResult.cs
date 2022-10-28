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
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SIRH.Web.Reports.PDF
{
    public class CrystalReportPdfResult : ActionResult
    {
        private readonly byte[] _contentBytes;
        private string typeFile;

        public CrystalReportPdfResult(string reportPath, object dataSet)
        {
            ReportDocument reportDocument = new ReportDocument();
            typeFile = "";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataSet);
            //_contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
            _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.Excel));
        }

        public CrystalReportPdfResult(string reportPath, object dataSet, string file)
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataSet);
            if (file == "PDF")
            {
                typeFile = "PDF";
                _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
            }
            else if (file == "EXCEL")
            {
                typeFile = "EXCEL";
                _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.Excel));
            }
            else if (file == "WORD")
            {
                typeFile = "WORD";
                _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.WordForWindows));
            }
            reportDocument.Close();

        }

        /// <summary>
        /// Crea un reporte que soporta n subreportes del mismo tipo
        /// </summary>
        /// <param name="reportPath">La direccion del reporte</param>
        /// <param name="dataSet">Modelo principal de datos </param>
        /// <param name="subReport">N Modelos de los subreportes</param>
        public CrystalReportPdfResult(string reportPath, object dataSet, params object[] subReport)
        {
            ReportDocument reportDocument = new ReportDocument();
            typeFile = "PDF";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataSet);
            for (int i = 0; i < subReport.Length; i++)
                reportDocument.Subreports[i].SetDataSource(subReport[i]);
            _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
            //_contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.Excel));
        }


        public CrystalReportPdfResult(object dataSet, string reportPath, object group, object subreport)
        {
            typeFile = "PDF";
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(reportPath);
            reportDocument.Database.Tables[0].SetDataSource(dataSet);
            reportDocument.Database.Tables[1].SetDataSource(group);
            reportDocument.Subreports[0].SetDataSource(subreport);
            _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
            //_contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.Excel));
        }

        /// <summary>
        /// Crea un reporte que soporta dos modelos de datos en el mismo reporte
        /// </summary>
        /// <param name="dataSet">Primer modelo de datos</param>
        /// <param name="dataSet2">Segundo modelo de datos</param>
        /// <param name="reportPath">La ruta del reporte</param>
        public CrystalReportPdfResult(object dataSet, object dataSet2, string reportPath)
        {
            ReportDocument reportDocument = new ReportDocument();
            typeFile = "PDF";
            reportDocument.Load(reportPath);
            reportDocument.Database.Tables[0].SetDataSource(dataSet);
            reportDocument.Database.Tables[1].SetDataSource(dataSet2);
            _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
            //_contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.Excel));
        }

        /// <summary>
        /// Crea un reporte que soporta cuatro modelos de datos en el mismo reporte (PF)
        /// </summary>
        /// <param name="dataSet">Primer modelo de datos</param>
        /// <param name="dataSet2">Segundo modelo de datos</param>
        /// /// <param name="dataSet3">Tercer modelo de datos</param>
        /// /// <param name="dataSet4">Cuarto modelo de datos</param>
        /// <param name="reportPath">La ruta del reporte</param>
        public CrystalReportPdfResult(string reportPath, object dataSet, object dataSet2, object dataSet3, object dataSet4)
        {
            ReportDocument reportDocument = new ReportDocument();
            typeFile = "PDF";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataSet);
            if (dataSet3 != null)
            {
                reportDocument.Subreports[0].SetDataSource(dataSet3);
                reportDocument.Subreports[2].SetDataSource(dataSet2);
                reportDocument.Subreports[1].SetDataSource(dataSet4);
            }
            else
            {
                reportDocument.Subreports[0].SetDataSource(dataSet2);
            }
            _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
            //_contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.Excel));
        }

        public override void ExecuteResult(ControllerContext context)
        {

            var response = context.HttpContext.ApplicationInstance.Response;
            response.Clear();
            response.Buffer = false;
            response.ClearContent();
            response.ClearHeaders();
            response.Cache.SetCacheability(HttpCacheability.Public);

            if(typeFile == "PDF")
                response.ContentType = "application/pdf";
            else if (typeFile == "EXCEL")
                response.ContentType = "application/vnd.ms-excel";
            else
                response.ContentType = "application/msword";

            using (var stream = new MemoryStream(_contentBytes))
            {
                stream.WriteTo(response.OutputStream);
                stream.Flush();
            }
        }

        private static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
