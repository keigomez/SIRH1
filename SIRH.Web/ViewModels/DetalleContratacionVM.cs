using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using System.ComponentModel;
using System.Web.Mvc;

namespace SIRH.Web.ViewModels
{
    public class DetalleContratacionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }

        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        public CExpedienteFuncionarioDTO Expediente { get; set; }

        public bool EstadoMarca { get; set; }

        public CErrorDTO Error { get; set; }
    }
}