using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SIRH.DTO
{
    [DataContract]
    public class CDesarraigoDTO:CBaseDTO
    {
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }
        [DataMember]
        public CEstadoDesarraigoDTO EstadoDesarraigo { get; set; }
        [DataMember]
        public CDistritoDTO DistritoContrato { get; set; }
        [DataMember]
        public CDistritoDTO DistritoTrabajo { get; set; }
        [DataMember]
        public CDistritoDTO DistritoPernocte { get; set; }
        [DataMember]
        public CSeccionDTO Seccion { get; set; }
        [DataMember]
        public CDepartamentoDTO Departamento { get; set; }
        [DataMember]
        [DisplayName("Presupuesto")]
        public CPresupuestoDTO Presupuesto { get; set; }

        [DataMember]
        [DisplayName("Jefe Inmediato")]
        public CFuncionarioDTO JefeInmediato { get; set; }

        [DataMember]
        [DisplayName("Registrado por")]
        public CFuncionarioDTO RegistradoPor { get; set; }

        [DataMember]
        [DisplayName("Analizado por")]
        public CFuncionarioDTO AnalizadoPor { get; set; }

        [DataMember]
        [DisplayName("Revisado por")]
        public CFuncionarioDTO RevisadoPor { get; set; }

        [DataMember]
        [DisplayName("Aprobado / Anulado por")]
        public CFuncionarioDTO AprobadoPor { get; set; }

        [DataMember]
        [DisplayName("Núm. Contrato")]
        public string NumContrato { get; set; }

        [DataMember]
        [DisplayName("40% Salario Base")]
        [DisplayFormat(DataFormatString = "#;(#)")]
        public decimal MontoDesarraigo { get; set; }
        [DataMember]
        [DisplayName("Monto Salario Base")]
        [DisplayFormat(DataFormatString = "#;(#)")]
        public decimal MontoSalario { get; set; }
        [DataMember]
        [DisplayName("Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        [DisplayName("Fecha de Vencimiento")]
        public DateTime FechaFin { get; set; }
        [DataMember]
        [DisplayName("Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
        [DataMember]
        [DisplayName("N° de registro del desarraigo")]
        [RegularExpression("^[0-9]{4}-[0-9]*$", ErrorMessage = "Formato invalido")] // nuevo
        public string CodigoDesarraigo { get; set; }

       

        [DataMember]
        [StringLength(300)]
        [DisplayName("Observaciones")]
        public string ObservacionesDesarraigo { get; set; }


        [DataMember]
        [StringLength(300)]
        [DisplayName("Observaciones Estado")]
        public string ObservacionesEstado { get; set; }

        [DataMember]
        [DisplayName("Nombre Proyecto")]
        public string NomProyecto { get; set; }

        [DataMember]
        [DisplayName("Lugar Pernocte")]
        public string LugarPernocte { get; set; }
        [DataMember]
        [DisplayName("Documento")]
        public byte[] DocAdjunto { get; set; }

        [DataMember]
        [DisplayName("Núm Acción Aprob.")]
        public string NumAccionAprobacion { get; set; }

        [DataMember]
        [DisplayName("Núm Acción Elimin.")]
        public string NumAccionEliminacion { get; set; }

        [DataMember]
        public List<CContratoArrendamientoDTO> Contrato { get; set; }

        [DataMember]
        public List<CFacturaDesarraigoDTO> Factura { get; set; }

        [DataMember]
        public List<CDetalleDesarraigoEliminacionDTO> DetalleEliminacion { get; set; }
        
    }
}
