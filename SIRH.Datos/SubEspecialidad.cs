
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
    
public partial class SubEspecialidad
{

    public SubEspecialidad()
    {

        this.DetallePuesto = new HashSet<DetallePuesto>();

    }


    public int PK_SubEspecialidad { get; set; }

    public string DesSubEspecialidad { get; set; }

    public Nullable<int> IndEstadoSubEspecialidad { get; set; }



    public virtual ICollection<DetallePuesto> DetallePuesto { get; set; }

}

}
