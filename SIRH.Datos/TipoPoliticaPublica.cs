
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
    
public partial class TipoPoliticaPublica
{

    public TipoPoliticaPublica()
    {

        this.CalificacionCapacitacion = new HashSet<CalificacionCapacitacion>();

    }


    public int PK_TipoPP { get; set; }

    public string DesTipoPP { get; set; }

    public string DesSiglas { get; set; }



    public virtual ICollection<CalificacionCapacitacion> CalificacionCapacitacion { get; set; }

}

}