using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CProcAlmacenadoDatosGeneralesDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Periodos")]
        public string Periodos { get; set; }
        [DataMember]
        [DisplayName("NombreInstitucion")]
        public string NombreInstitucion { get; set; }
        [DataMember]
        [DisplayName("CantPuestosInstitucionales")]
        public string CantPuestosInstitucionales { get; set; }
        [DataMember]
        [DisplayName("Propiedad")]
        public string Propiedad { get; set; }
        [DataMember]
        [DisplayName("Interinos")]
        public string Interinos { get; set; }
        [DataMember]
        [DisplayName("SinInterinos")]
        public string SinInterinos { get; set; }
        [DataMember]
        [DisplayName("CantidadPFRSC")]
        public string CantidadPFRSC { get; set; }
        [DataMember]
        [DisplayName("CantidadPuestosFueraRSC")]
        public string CantidadPuestosFueraRSC { get; set; }
        [DataMember]
        [DisplayName("Excluidos")]
        public string Excluidos { get; set; }
        [DataMember]
        [DisplayName("PuestoConfianza")]
        public string PuestoConfianza { get; set; }
        [DataMember]
        [DisplayName("CantidadPuestosFRSC")]
        public string CantidadPuestosFRSC { get; set; }
        [DataMember]
        [DisplayName("Exceptuados")]
        public string Exceptuados { get; set; }
        [DataMember]
        [DisplayName("Oposicion")]
        public string Oposicion { get; set; }
        [DataMember]
        [DisplayName("Otros")]
        public string Otros { get; set; }
        [DataMember]
        [DisplayName("FuncionariosDentroRSC")]
        public string FuncionariosDentroRSC { get; set; }
        [DataMember]
        [DisplayName("Evaluados")]
        public string Evaluados { get; set; }
        [DataMember]
        [DisplayName("NoEvaluados")]
        public string NoEvaluados { get; set; }
        
        [DataMember]
        [DisplayName("error")]
        public string error { get; set; }


    }
}
