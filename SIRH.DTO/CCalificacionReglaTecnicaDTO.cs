using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CCalificacionReglaTecnicaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Periodo")]
        public CPeriodoCalificacionDTO Periodo { get; set; }

        [DataMember]
        [DisplayName("DireccionGeneral")]
        public CDireccionGeneralDTO DireccionDTO { get; set; }

        [DataMember]
        [DisplayName("Director")]
        public CFuncionarioDTO DirectorDTO { get; set; }

        [DataMember]
        [DisplayName("Email")]
        public string EmailDTO { get; set; }
        
        [DataMember]
        [DisplayName("Número Funcionarios")]
        public int NumFuncionariosDTO { get; set; }
        
        [DataMember]
        [DisplayName("Número Excelentes")]
        public int NumExcelentesDTO { get; set; }

        [DataMember]
        [DisplayName("Porcentaje Excelentes")]
        public decimal PorcExcelentesDTO { get; set; }

        [DataMember]
        [DisplayName("IndEstado")]
        public int IndEstadoDTO { get; set; }
        
        [DataMember]
        [DisplayName("Documento")]
        public byte[] ImagenDocumento { get; set; }
    }
}