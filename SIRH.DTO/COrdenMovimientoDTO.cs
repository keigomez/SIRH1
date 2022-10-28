using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    [DataContract]
    public class COrdenMovimientoDTO : CBaseDTO
    {
        [DataMember]
        public CEstadoOrdenDTO Estado { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioOrden { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioSustituido { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioResponsable { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioRevision { get; set; }

        [DataMember]
        public CFuncionarioDTO FuncionarioJefatura { get; set; }

        [DataMember]
        public CDetallePuestoDTO DetallePuesto { get; set; }

        [DataMember]
        public CMotivoMovimientoDTO TipoMovimiento { get; set; }

        [DataMember]
        public CMotivoMovimientoDTO MotivoMovimiento { get; set; }

        [DataMember]
        public CPedimentoPuestoDTO Pedimento { get; set; }

        [DataMember]
        [DisplayName("Núm. Orden")]
        public string NumOrden { get; set; }

        [DataMember]
        [DisplayName("Cuenta Cliente")]
        public string CuentaCliente { get; set; }
               
        [DataMember]
        [DisplayName("Fecha Rige")]
        public DateTime FecRige { get; set; }

        [DataMember]
        [DisplayName("Fecha Vence")]
        public DateTime? FecVence { get; set; }

        [DataMember]
        [DisplayName("Observaciones")]
        public string DesObservaciones { get; set; }

        [DataMember]
        [DisplayName("Estado Observaciones")]
        public string DesEstadoObservaciones { get; set; }

        [DataMember]
        [DisplayName("Partida Presupuestaria")]
        public string DesPartidaPresupuestaria { get; set; }
    }
}
