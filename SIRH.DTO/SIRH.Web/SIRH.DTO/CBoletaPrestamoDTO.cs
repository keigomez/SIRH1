using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{

    [DataContract(IsReference = true)]
    public class CBoletaPrestamoDTO : CBaseDTO
    {


        [DataMember]
        public CUsuarioDTO Usuario { get; set; }

        [DataMember]
        public CExpedienteFuncionarioDTO ExpedienteFuncionario { get; set; }


        /*---------DATOS DE SOLICITANTE---------------*/

        [DataMember]
        [Required]
        [DisplayName("Cédula de Solicitante")]
        [RegularExpression(@"\d{10}", ErrorMessage = "El número cédula debe ser de al menos 10 dígitos")]
        public string CedulaSolicitante { get; set; }

        [DataMember]
        [DisplayName("Nombre Solicitante")]
        public string NombreSolicitante { get; set; }

        [DataMember]
        [DisplayName("Apellido Solicitante")]
        public string ApellidoSolicitante { get; set; }

        [DataMember]
        [DisplayName("Tipo de Usuario")]
        public TipoUsuarioEnum TipoUsuario { get; set; }


        [DataMember]
        [DisplayName("Teléfono")]
        public string Telefonolicitante { get; set; }

        [DataMember]
        [DisplayName("Correo")]
        public string CorreoSolicitante { get; set; }

        [DataMember]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha de Prestamo")]
        public DateTime FechaPrestamo { get; set; }

        [DataMember]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha de Devolución")]
        public DateTime FechaCaducidad { get; set; }


        [DataMember]
        [DisplayName("Motivo de Préstamo")]
        public string MotivoPrestamo { get; set; }



        [DataMember]
        [DisplayName("Lugar de Procedencia")]
        public string LugarDeProcedencia { get; set; }


        /*-------------------------------------------*/






        /*---------DATOS DE FUNCIONARIO---------------*/    

        [DataMember]
        [Required]
        [DisplayName("Cédula de Funcionario")]
        [RegularExpression(@"\d{10}", ErrorMessage = "El número cédula debe ser de al menos 10 dígitos")]
        public string CedulaFunionario { get; set; }


        [DataMember]
        public int ExpedienteUsuarioInterno { get; set; }

        [DataMember]
        [DisplayName("Nombre Funcionario")]
        public string NombreFuncionarioSolicitado { get; set; }

        [DataMember]
        [DisplayName("Apellidos Funcionario")]
        public string ApellidoFuncionarioSlicitado { get; set; }


        [DataMember]
        [DisplayName("Número de Expediente")]
        public string NumeroExpediente { get; set; }

        [DataMember]
        [DisplayName("División")]
        public string DivisiónFuncionario { get; set; }

        [DataMember]
        [DisplayName("Dirección")]
        public string DirecciónFuncionario { get; set; }

        [DataMember]
        [DisplayName("Departamento")]
        public string DepartamentoFuncionario { get; set; }

        [DataMember]
        public int NumeroBoleta { get; set; }

        /*-------------------------------------------*/

    }

}

[DataContract]
public enum TipoUsuarioEnum
{

    [EnumMember]
    Interno = 1,
    [EnumMember]
    Externo = 2
}
