using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.ViewModels
{
    public class FuncionarioResolucionVM
    {
        public CFuncionarioDTO Funcionario { get; set; }

        public decimal PuntosActuales { get; set; }

        public decimal PuntosAdicionales { get; set; }

        public decimal PuntosTotales { get; set; }

        public decimal MontoPagar { get; set; }

        public decimal MontoTotal { get; set; }

        public decimal ValorPunto { get; set; }

        public string  NumResolucion { get; set; }

        public decimal SalarioBase { get; set; }

        public DateTime FecRige { get; set; }

        public DateTime FecVence { get; set; }
    }
}