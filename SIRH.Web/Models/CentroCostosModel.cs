using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIRH.Web.Models
{
    public class CentroCostosModel
    {
        public string CodigoSearch { get; set; }
        public SelectList CentrosCostosList { get; set; }
        public string CodigoCostos { get; set; }
    }
}