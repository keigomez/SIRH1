using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class BusquedaDeduccionTemporalVM
    {
        public CFuncionarioDTO Funcionario { get; set; }

        public CPuestoDTO Puesto { get; set; }

        public CDeduccionTemporalDTO Deduccion { get; set; }

        public CTipoDeduccionTemporalDTO DatoTipoDeduccion { get; set; }

        public CBitacoraUsuarioDTO Usuario { get; set; }

        public List<FormularioDeduccionTemporalVM> Deducciones { get; set; }

        [DisplayName("Fecha de emisión desde")]
        public DateTime FechaEmisionDesde { get; set; }

        [DisplayName("Fecha de emisión hasta")]
        public DateTime FechaEmisionHasta { get; set; }

        [DisplayName("Fecha Bitácora desde")]
        public DateTime FechaBitacoraDesde { get; set; }

        [DisplayName("Fecha Bitácora hasta")]
        public DateTime FechaBitacoraHasta { get; set; }

        public SelectList Estados { get; set; }

        [DisplayName("Estado")]
        public string EstadoSeleccionado { get; set; }

        public SelectList Tipos { get; set; }

        [DisplayName("Tipo de deducción")]
        public string TipoSeleccionado { get; set; }

        public SelectList Usuarios { get; set; }

        [DisplayName("Usuario")]
        public string UsuarioSeleccionado { get; set; }

        public string CampoOrdenar { get; set; }

        public bool PermisoAdministrador { get; set; }

        public bool PermisoRegistrar { get; set; }
    }
}