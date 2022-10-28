using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class BusquedaVacacionesReportesVM
    {
        public CFuncionarioDTO Funcionario { get; set; }
        public CPeriodoVacacionesDTO Periodo { get; set; }
        public CRegistroVacacionesDTO Registro { get; set; }

        [DisplayName("Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaInicioVacaciones { get; set; }
        [DisplayName("Fin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FechaFinalVacaciones { get; set; }

        public SelectList Direcciones { get; set; }
        public SelectList Divisiones { get; set; }
        public SelectList Departamentos { get; set; }
        public SelectList Secciones { get; set; }
        public string Saldo1 { get; set; }
        public string Saldo2 { get; set; }
        [DisplayName("Numero de documento")]
        public string NumeroDocumento { get; set; }
        public SelectList Estados { get; set; }

        [DisplayName("Tipo de registro")]
        public string EstadoSeleccion { get; set; }
        public string DireccionSeleccion { get; set; }
        public string DivisionSeleccion { get; set; }
        public string DepartamentoSeleccion { get; set; }
        public string SeccionSeleccion { get; set; }
        public List<CRegistroVacacionesDTO> RegistroVacaciones { get; set; }
        public bool RegistroRebajoColectivo { get; set; }
        public List<CReintegroVacacionesDTO> ReintegrosVacaciones { get; set; }
        public List<CFuncionarioDTO> RegistroFuncionarios { get; set; }
        public List<CPeriodoVacacionesDTO> PeriodosVacaciones { get; set; }
        public string estadoPeriodo { get; set; }
        public SelectList tipoRegistroVacaciones { get; set; }
        [DisplayName ("Tipo registro de vacaciones")]
        public string SeleccionTipoVacaciones { get; set; }

        public List<CPeriodoVacacionesDTO> PeriodosVacacionesInactivos { get; set; }


    }
}