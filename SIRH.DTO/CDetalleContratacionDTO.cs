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
    public class CDetalleContratacionDTO : CBaseDTO
    {
        [DataMember]
        public CFuncionarioDTO Funcionario { get; set; }        
        [DataMember]
        [DisplayName("Fecha de Ingreso")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaIngreso { get; set; }
        [DataMember]
        [DisplayName("Fecha de Cese")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaCese { get; set; }
        [DataMember]
        [DisplayName("Cantidad de Anualidades")]
        public int NumeroAnualidades { get; set; }
        [DataMember]
        [DisplayName("Años de Servicio Público")]                   
        public int NumeroAnniosServicioPublico { get; set; }
        [DataMember]
        [DisplayName("Mes de Aumento")]
        public string FechaMesAumento { get; set; }
        [DataMember]
        [DisplayName("Código Policial")]
        public int CodigoPolicial { get; set; }
        [DataMember]
        [DisplayName("Estado de Marcación")]
        public bool? EstadoMarcacion { get; set; }

        [DataMember]
        [DisplayName("Fecha de ingreso al régimen policial")]
        public DateTime FechaRegimenPolicial { get; set; }

        [DataMember]
        [DisplayName("Fecha de vacaciones")]
        public DateTime FechaVacaciones { get; set; }

        [DataMember]
        [DisplayName("Ubicacion real")]
        public string UbicacionReal { get; set; }
    }
}