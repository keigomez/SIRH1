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
    public class CPrestacionLegalDTO : CBaseDTO
    {
        [DataMember]
        public CNombramientoDTO Nombramiento { get; set; }
        [DataMember]
        public CTipoPrestacionDTO TipoPrestacion { get; set; }

        [DataMember]
        public List<CDetallePrestacionDTO> Detalle { get; set; }

        [DataMember]
        public CExpedienteFuncionarioDTO Expediente { get; set; }

        [DataMember]
        public CDetalleContratacionDTO DetalleContratacion { get; set; }

        //[DataMember]
        //public List<CAccionPersonalDTO> Acciones { get; set; }

        [DataMember]
        public List<CPeriodoVacacionesDTO> Vacaciones { get; set; }

        [DataMember]
        public List<CDetallePrestacionCuadroDTO> ListaSalario { get; set; }

        [DataMember]
        public List<CDetallePrestacionCuadroDTO> ListaAguinaldo { get; set; }

        [DataMember]
        public List<CDetallePrestacionCuadroDTO> ListaSalarioEscolar { get; set; }

        [DataMember]
        public List<CDetallePrestacionCuadroDTO> ListaVacacionesAcumuladas { get; set; }

        [DataMember]
        public List<CDetallePrestacionCuadroDTO> ListaVacacionesProporcionales { get; set; }
        [DataMember]
        public List<CDetallePrestacionAfiliacionDTO> ListaAfiliacion { get; set; }

        /// DetallePrestacionCuadro

        [DataMember]
        [DisplayName("Fecha Creacion")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecCreacion { get; set; }
        [DataMember]
        public int IndEstado { get; set; }

        [DataMember]
        [DisplayName("Número Prestación")]
        public string NumPrestacion { get; set; }


        [DataMember]
        [DisplayName("Monto Total")]
        public decimal MtoTotal { get; set; }
        [DataMember]
        [DisplayName("Monto Cesantia")]
        public decimal MtoCesantia { get; set; }
        [DataMember]
        [DisplayName("Monto Vacaciones")]
        public decimal MtoVacaciones { get; set; }
        [DataMember]
        [DisplayName("Monto Quincenal")]
        public decimal MtoQuincenal { get; set; }
        [DataMember]
        [DisplayName("Monto Diario")]
        public decimal MtoDiario { get; set; }

        [DataMember]
        [DisplayName("Monto Escolar sin Rebajo")]
        public decimal MtoEscolarSinRebajo { get; set; }

        [DataMember]
        [DisplayName("Monto Escolar con Rebajo")]
        public decimal MtoEscolarConRebajo { get; set; }

        [DataMember]
        [DisplayName("Monto Aguinaldo")]
        public decimal MtoAguinaldo { get; set; }

        [DataMember]
        [DisplayName("Monto Extras")]
        public decimal MtoExtras { get; set; }

        [DataMember]
        [DisplayName("Fecha Pago")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecPago { get; set; }

        [DataMember]
        [DisplayName("Años Laborados")]
        public int AñosLaborados { get; set; }
    }
}
