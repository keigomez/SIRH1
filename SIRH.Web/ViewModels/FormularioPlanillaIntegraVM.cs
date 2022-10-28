using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.ViewModels
{
    public class FormularioPlanillaIntegraVM
    {
        public HttpPostedFileBase Archivo { get; set; }

        public DateTime Periodo { get; set; }

    }
}