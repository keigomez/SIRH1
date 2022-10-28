using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.Web.Mvc;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FormularioDeduccionTemporalVM
    {
        public CDeduccionTemporalDTO Deduccion { get; set; }

        public CTipoDeduccionTemporalDTO DatoTipoDeduccion { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }

        public CPuestoDTO Puesto { get; set; }

        public CDetallePuestoDTO DetallePuesto { get; set; }

        public CExpedienteFuncionarioDTO Expediente { get; set; }

        public CBitacoraUsuarioDTO Bitacora { get; set; }

        public CSalarioDTO Salario { get; set; }

        public SelectList Tipos { get; set; }

        [DisplayName("Tipo Permiso / Licencia")]
        public int TipoSeleccionado { get; set; }

        public bool MostrarDato { get; set; }

        public bool PermisoAprobar { get; set; }

        public bool PermisoRegistrar { get; set; }
    }
}