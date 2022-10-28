using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.Web.ViewModels
{
    public class ListaGastoTransporteVM
    {
        public CErrorDTO Error { get; set; }

        public List<CGastoTransporteDTO> Gastos { get; set; }

        public string ReservaRecurso { get; set; }

        public string MesSeleccion { get; set; }
        public List<SelectListItem> MesesViatico { get; set; }

        //Tipo SelectList para poder ponerlos en el dropdown en la vista (_FormularioAsignaciónGT)
        [DisplayName("Códigos Presupuestarios")]
        public SelectList CodigosPresupuestoList { get; set; }
        [DisplayName("Códigos Presupuestarios")]
        public string PresupuestoSelected { get; set; }

        public bool PagoRealizado { get; set; }
    }
}
