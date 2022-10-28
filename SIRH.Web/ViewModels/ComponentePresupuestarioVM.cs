using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class ComponentePresupuestarioVM
    {
        public CComponentePresupuestarioDTO ComponentePresupuestario { get; set; }

        public CErrorDTO Error { get; set; }

        public CObjetoGastoDTO ObjetoGasto { get; set; }

        public CProgramaDTO Programa { get; set; }

        [DisplayName("Año Presupuestario")]
        public string AnioPresupuesto { get; set; }

        [DisplayName("Monto")]
        public decimal MontoComponente { get; set; }

        public CCatMovimientoPresupuestoDTO TipoMovimiento { get; set; }

        [DisplayName("Detalle")]
        public string Detalle { get; set; }

        [DisplayName("Titulo de decreto")]
        public string TituloComponente { get; set; }

        [DisplayName("Numero de decreto")]
        public string NumeroComponentePresupuestario { get; set; }

        [DisplayName("Fecha")]
        public DateTime FechaDecreto { get; set; }
    }
}