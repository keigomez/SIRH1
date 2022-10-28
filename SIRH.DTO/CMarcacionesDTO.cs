using System;
using System.Runtime.Serialization;

namespace SIRH.DTO
{

    [DataContract(IsReference = true)]
    public class CMarcacionesDTO : CBaseDTO 
    {
        [DataMember]
        public short DispositivoMarcas { get; set; }

        [DataMember]
        public DateTime FechaHoraMarca { get; set; }

        [DataMember]
        public string CodigoEmpleado { get; set; }

        [DataMember]
        public short ProcesadoMarcas { get; set; } //tagSoftware

    }

}
