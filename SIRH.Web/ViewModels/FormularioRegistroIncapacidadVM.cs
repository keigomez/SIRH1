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
using System.Collections.Generic;

namespace SIRH.Web.ViewModels
{
    public class FormularioRegistroIncapacidadVM
    {
        public CRegistroIncapacidadDTO Incapacidad { get; set; }
        public CEntidadMedicaDTO EntidadMedica { get; set; }
        public CTipoIncapacidadDTO TipoIncapacidad { get; set; }
        public CDesgloseSalarialDTO Desglose { get; set; }

        //public List<CDetalleIncapacidadDTO> Detalles { get; set; }

        public CErrorDTO Error { get; set; }

        public SelectList Entidades { get; set; }

        [DisplayName("Entidad que emite")]
        public int EntidadSeleccionada { get; set; }

        public CFuncionarioDTO Funcionario { get; set; }
        public CPuestoDTO Puesto { get; set; }
        public CDetallePuestoDTO DetallePuesto { get; set; }
        public CExpedienteFuncionarioDTO Expediente { get; set; }
        
        public SelectList Tipos { get; set; }

        [DisplayName("Tipo Incapacidad")]
        public int TipoSeleccionado { get; set; }

        [DisplayName("Salario Bruto")]
        public decimal MontoSalarioBruto { get; set; }

        [DisplayName("Salario Diario")]
        public decimal MontoSalarioDiario { get; set; }

        public int IndProrroga { get; set; }
    }
}