using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SIRH.DTO;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.Web.ViewModels
{
    public class FormularioBorradorAccionPersonalVM
    {
        public CBorradorAccionPersonalDTO Borrador { get; set; }
        public CTipoAccionPersonalDTO TipoAccion { get; set; }
        public CEstadoBorradorDTO Estado { get; set; }
        public CDetalleBorradorAccionPersonalDTO Detalle { get; set; }   
        public CMovimientoBorradorAccionPersonalDTO Movimiento { get; set; }
 

        //public SelectList Estados { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }
        public CFuncionarioDTO Asignado { get; set; }

        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CNombramientoDTO Nombramiento { get; set; }
        public CDetalleContratacionDTO Contrato { get; set; }
        public CTipoJornadaDTO TipoJornada { get; set; }
        public CProgramaDTO Programa { get; set; }

        public CDireccionGeneralDTO Direccion { get; set; }
        public SelectList Direcciones { get; set; }
        public string DireccionSeleccionada { get; set; }

        public CSeccionDTO Seccion { get; set; }
        public SelectList Secciones { get; set; }
        public string SeccionSeleccionada { get; set; }

        public CClaseDTO Clase { get; set; }
        public SelectList Clases { get; set; }
        public string ClaseSeleccionada { get; set; }

        public SelectList Categorias { get; set; }
        public string CategoriaSeleccionada { get; set; }

        public SelectList Porcentajes { get; set; }

        public SelectList Tipos { get; set; }
        [DisplayName("Tipo Acción")]
        public int TipoSeleccionado { get; set; }

        //[DisplayName("Número de Oficio")]
        //public string NumOficio { get; set; }

        public SelectList Usuarios { get; set; }
        [DisplayName("Usuario a Asignar")]
        public string UsuarioAsignado { get; set; }

        public SelectList UsuariosDevolver { get; set; }
        [DisplayName("Usuario Devolución")]
        public string UsuarioDevolucion { get; set; }

        public decimal PuntosCarrera { get; set; }
        public CErrorDTO Error { get; set; }

        public string nombreBoton { get; set; }
    }
}