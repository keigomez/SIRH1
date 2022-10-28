using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Models
{
    public class RutasARESEPModel
    {
        public string CodRuta { get; set; }
        public string NomRuta { get; set; }
        public string NomFraccionamiento { get; set; }
        public string KmPorViaje { get; set; }
        public int TarifaRegular { get; set; }
        public int TarifaAdultoMayor { get; set; }
        public string Resolucion { get; set; }
        public string FecResolucion { get; set; }
        public string Gaceta { get; set; }
        public string Alcance { get; set; }
        public string FecGaceta { get; set; }
        public string FecVigencia { get; set; }
        public string Expediente { get; set; }
        public string Operadores { get; set; }
    }
}