﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SIRH.DTO
{
    [DataContract]
    public class CTipoIndicadorMetaDTO : CBaseDTO
    {
        [DataMember]
        [DisplayName("Tipo Indicador")]
        public string DesTipoIndicadorMeta { get; set; }
    }
}