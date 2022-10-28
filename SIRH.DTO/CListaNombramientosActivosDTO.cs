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
    public class CListaNombramientosActivosDTO : CBaseDTO
    {
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }

        [DataMember]
        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        [DataMember]
        public CExpedienteFuncionarioDTO Expediente { get; set; }

        [DataMember]
        public CHistorialEstadoCivilDTO EstadoCivil { get; set; }

        [DataMember]
        public CInformacionContactoDTO Celular { get; set; }
        
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }

        [DataMember]
        public CPuestoDTO Puesto { get; set; }

        [DataMember]
        public CDetallePuestoDTO DetallePuesto { get; set; }

        [DataMember]
        public CDivisionDTO Division { get; set; }
        [DataMember]
        public CDireccionGeneralDTO DireccionGeneral { get; set; }

        [DataMember]
        public CDepartamentoDTO Departamento { get; set; }

        [DataMember]
        public CSeccionDTO Seccion { get; set; }

        [DataMember]
        public CPresupuestoDTO Presupuesto { get; set; }

        [DataMember]
        public CDesgloseSalarialDTO PrimerSalario { get; set; }

        [DataMember]
        public CDesgloseSalarialDTO UltimoSalario { get; set; }

        public CSalarioDTO Salario { get; set; }

        [DataMember]
        public CUbicacionPuestoDTO UbiContrato { get; set; }

        [DataMember]
        public CUbicacionPuestoDTO UbiTrabajo { get; set; }

        [DataMember]
        public CDireccionDTO Domicilio { get; set; }

        [DataMember]
        [DisplayName("En Propiedad")]
        public int IndPropiedad { get; set; }

        [DataMember]
        public List<CCalificacionNombramientoDTO> Calificaciones { get; set; }
    }
}