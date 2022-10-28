using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract(IsReference = true)]

    public class CAguinaldoDTO : CBaseDTO
    {

        [DataMember]
        [DisplayName("Cédula")]
        [Required(ErrorMessage = "La cédula es requerida")]
        [RegularExpression(@"\d{10}", ErrorMessage = "El número cédula debe ser de al menos 10 dígitos")]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        public CDesgloseSalarialDTO Salario { get; set; }

        [DataMember]
        [DisplayName("Monto Aguinaldo")]
        public decimal MtoAguinaldo { get; set; }

        [DataMember]
        [DisplayName("Periodo")]
        public DateTime Periodo { get; set; }

    }
}

