
//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SIRH.Datos
{

using System;
    using System.Collections.Generic;
    
public partial class TipoIndicadorMeta
{

    public TipoIndicadorMeta()
    {

        this.MetaIndividualCalificacion = new HashSet<MetaIndividualCalificacion>();

    }


    public int PK_TipoIndicador { get; set; }

    public string DesTipoIndicador { get; set; }



    public virtual ICollection<MetaIndividualCalificacion> MetaIndividualCalificacion { get; set; }

}

}
