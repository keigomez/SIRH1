using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Globalization;

namespace SIRH.DTO
{
    [DataContract]
    public class CEMUExfuncionarioDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }
        [DataMember]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DataMember]
        [DisplayName("Primer Apellido")]
        public string PrimerApellido { get; set; }
        [DataMember]
        [DisplayName("Segundo Apellido")]
        public string SegundoApellido { get; set; }
        [DataMember]
        [DisplayName("Puesto Propiedad")]
        public string PuestoPropiedad { get; set; }
        [DataMember]
        [DisplayName("Último Período")]
        public string UltimoPeriodo { get; set; }
        [DataMember]
        [DisplayName("Estado Funcionario")]
        public int EstadoFunc { get; set; }
        [DataMember]
        [DisplayName("Estado Funcionario")]
        public string DescEstado { get; set; }
        [DataMember]
        [DisplayName("Programa")]
        public string Programa { get; set; }
        [DataMember]
        [DisplayName("Clase")]
        public string ClasePuesto { get; set; }
        [DataMember]
        [DisplayName("Fecha de Ingreso")]
        public DateTime? FechaIngreso { get; set; }
        [DataMember]
        [DisplayName("Fecha de Cese")]
        public DateTime? FechaCese { get; set; }
        [DataMember]
        [DisplayName("Fecha Cumple")]
        public DateTime? FechaCumple { get; set; }
        [DataMember]
        [DisplayName("Género")]
        public string Sexo { get; set; }
        [DataMember]
        [DisplayName("Estado Civil")]
        public string EstadoCivil { get; set; }
        [DataMember]
        [DisplayName("División")]
        public string Division { get; set; }
        [DataMember]
        [DisplayName("Subdivisión")]
        public string SubDivision { get; set; }
        [DataMember]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DataMember]
        [DisplayName("Departamento")]
        public string Departamento { get; set; }
        [DataMember]
        [DisplayName("Sección")]
        public string Seccion { get; set; }
        [DataMember]
        [DisplayName("Número de Expediente")]
        public string NumeroExpediente { get; set; }
        [DataMember]
        [DisplayName("Código de Inspectores")]
        public string CodInspectores { get; set; }
    }
}
