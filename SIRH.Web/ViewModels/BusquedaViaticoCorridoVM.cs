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
    public class BusquedaViaticoCorridoVM
    {
        
        public CFuncionarioDTO Funcionario { get; set; }
        public CViaticoCorridoDTO ViaticoCorrido { get; set; }
        public List<FormularioViaticoCorridoVM> ViaticoCorridoRes { get; set; }
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
        public DateTime FechaInicioViaticoCorridoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechaInicioViaticoCorridoF { get; set; }

        [DisplayName("Inicio")]
        public DateTime FechaFinalViaticoCorridoI { get; set; }
        [DisplayName("Fin")]
        public DateTime FechaFinalViaticoCorridoF { get; set; }

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
