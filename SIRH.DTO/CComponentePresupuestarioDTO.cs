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
    public class CComponentePresupuestarioDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Objeto Gasto")]
        public CObjetoGastoDTO ObjetoGasto { get; set; }

        [DataMember]
        public CProgramaDTO Programa { get; set; }

        [DataMember]
        [DisplayName("Año Presupuestario")]
        [Required(ErrorMessage = "Debe agregar el año")]
        public string AnioPresupuesto { get; set; }

        [DataMember]
        [DisplayName("Monto")]
        [DisplayFormat(DataFormatString = "{0:N2")]
        public decimal MontoComponente { get; set; }

        [DataMember]
        [DisplayName("Tipo de Movimiento")]
        public CCatMovimientoPresupuestoDTO TipoMovimiento { get; set; }

        [DataMember]
        [DisplayName("Detalle")]
        public string Detalle { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Debe ingresar el título")]
        [DisplayName("Titulo de decreto")]
        public string TituloComponente { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Debe agregar el número de decreto")]
        [DisplayName("Numero de decreto")]
        public string NumeroComponentePresupuestario { get; set; }

        [DataMember]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaDecreto { get; set; }


    }
}

