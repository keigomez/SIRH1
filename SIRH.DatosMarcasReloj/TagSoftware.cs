//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIRH.DatosMarcasReloj
{
    using System;
    using System.Collections.Generic;
    
    public partial class TagSoftware
    {
        public TagSoftware()
        {
            this.DispositivoAsistencia = new HashSet<DispositivoAsistencia>();
        }
    
        public int IdTag { get; set; }
        public string Descripcion { get; set; }
    
        public virtual ICollection<DispositivoAsistencia> DispositivoAsistencia { get; set; }
    }
}
