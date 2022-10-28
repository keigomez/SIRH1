using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CProcAlmacenadoDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Estratos")]
        public string Estratos { get; set; }
        [DataMember]
        [DisplayName("AbsExcelente")]
        public string AbsExcelente { get; set; }
        [DataMember]
        [DisplayName("PorcExcelente")]
        public string PorcExcelente { get; set; }
        [DataMember]
        [DisplayName("AbsMuyBueno")]
        public string AbsMuyBueno { get; set; }
        [DataMember]
        [DisplayName("PorcMuyBueno")]
        public string PorcMuyBueno { get; set; }
        [DataMember]
        [DisplayName("AbsBueno")]
        public string AbsBueno { get; set; }
        [DataMember]
        [DisplayName("PorcBueno")]
        public string PorcBueno { get; set; }
        [DataMember]
        [DisplayName("AbsRegular")]
        public string AbsRegular { get; set; }
        [DataMember]
        [DisplayName("PorcRegular")]
        public string PorcRegular { get; set; }
        [DataMember]
        [DisplayName("AbsDeficiente")]
        public string AbsDeficiente { get; set; }
        [DataMember]
        [DisplayName("PorcDeficiente")]
        public string PorcDeficiente { get; set; }
        [DataMember]
        [DisplayName("TotalEvaluacion")]
        public string TotalEvaluacion { get; set; }
        [DataMember]
        [DisplayName("PorcTotalEvalacion")]
        public string PorcTotalEvalacion { get; set; }
        [DataMember]
        [DisplayName("TotalDatosNoEvaluados")]
        public string TotalDatosNoEvaluados { get; set; }
        [DataMember]
        [DisplayName("PorcTotalDatosNoEvaluados")]
        public string PorcTotalDatosNoEvaluados { get; set; }
        [DataMember]
        [DisplayName("TotalInstitucional")]
        public string TotalInstitucional { get; set; }
        [DataMember]
        [DisplayName("PorcTotalInstitucional")]
        public string PorcTotalInstitucional { get; set; }

        [DataMember]
        [DisplayName("error")]
        public string error { get; set; }


    }
}
