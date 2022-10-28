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
    public class CPerfilAccesoDTO : CBaseDTO
    {
        [DataMember]
        public CDetalleAccesoDTO DetalleAcceso { get; set; }
        [DataMember]
        [DisplayName("Fecha de Asignación")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FecAsignacion { get; set; }
        [DataMember]
        public CCatPermisoDTO CatPermiso { get; set; }
    }
}
