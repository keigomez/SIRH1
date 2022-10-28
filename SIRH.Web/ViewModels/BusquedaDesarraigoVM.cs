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
    public class BusquedaDesarraigoVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CDesarraigoDTO Desarraigo { get; set; }
        public List<FormularioDesarraigoVM> DesarraigosRes { get; set; }
        //[DisplayName("N° de cédula del funcionario")]
        //[StringLength(15)]
        //public string NumeroCedulaFuncionario { get; set; }
        //[DisplayName("N° de registro del desarraigo")]
        //[RegularExpression("^[0-9]{4}-[0-9]*$",ErrorMessage="Formato invalido")]
        //public string NumeroDesarraigo { get; set; }

        [DisplayName("Estado del registro")]
        public string EstadoSeleccion { get; set; }

        public SelectList Estados { get; set; }

        [DisplayName("Inicio")]
        public DateTime FechaInicioDesarraigoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechaInicioDesarraigoF { get; set; }

        [DisplayName("Inicio")]
        public DateTime FechaFinalDesarraigoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechaFinalDesarraigoF { get; set; }

        [DisplayName("Distrito")]
        public string DistritoSeleccion { get; set; }
        [DisplayName("Cantón")]
        public string CantonSeleccion { get; set; }
        [DisplayName("Provincias")]
        public string ProvinciaSeleccion { get; set; }

        public SelectList Distritos { get; set; }
        public SelectList Cantones { get; set; }
        public SelectList Provincias { get; set; }

    }
}
