
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
    
public partial class Clase
{

    public Clase()
    {

        this.DetallePuesto = new HashSet<DetallePuesto>();

        this.RegistroTiempoExtra = new HashSet<RegistroTiempoExtra>();

    }


    public int PK_Clase { get; set; }

    public string DesClase { get; set; }

    public Nullable<int> IndEstadoClase { get; set; }

    public Nullable<int> IndCategoria { get; set; }



    public virtual ICollection<DetallePuesto> DetallePuesto { get; set; }

    public virtual ICollection<RegistroTiempoExtra> RegistroTiempoExtra { get; set; }

}

}
