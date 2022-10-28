using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class GenerarResolucionVM
    {
        public List<CursoResolucionVM> ListaCursos { get; set; }
        public List<CursoResolucionVM> CursosSeleccionados { get; set; }
        //public int TotalFuncionarios { get; set; }
        public CFuncionarioDTO FuncionarioBusqueda { get; set; }
        public int CursoSeleccionado { get; set; }

        [Required(ErrorMessage = "Debe ingresar el número de resolución")]
        [DisplayName ("Número de resolución")]     
        public string NumResolucion { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de rige")]
        [DisplayName("Fecha de rige")]
        public DateTime FechaRige { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de vence")]
        [DisplayName("Fecha de vence")]
        public DateTime FechaVence { get; set; }
        public CErrorDTO Error { get; set; }




    }
}