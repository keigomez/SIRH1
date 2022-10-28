using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoDetallePagoGastoDTO : CBaseDTO
    {
        /// <summary>
        /// Descripción del tipo de detalle, especificamente
        /// al detalle de pago de un gasto.
        /// Algunos tipos son: Incapacidad, vacaciones, permisos, 
        /// </summary>
        [DataMember]
        [DisplayName("Descripción del tipo")]
        public string DescripcionTipo { get; set; }
    }
}
