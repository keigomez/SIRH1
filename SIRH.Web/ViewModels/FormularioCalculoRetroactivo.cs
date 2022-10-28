using SIRH.DTO;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
using System;

namespace SIRH.Web.ViewModels
{
    public class FormularioCalculoRetroactivo
    {
        public CCartaPresentacionDTO Carta { get; set; }
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaICalculo { get; set; }
        [DisplayName("Fecha Final")]
        public DateTime FechaFCalculo { get; set; }
    }
}