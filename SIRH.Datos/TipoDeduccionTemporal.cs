
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
    
public partial class TipoDeduccionTemporal
{

    public TipoDeduccionTemporal()
    {

        this.DeduccionTemporal = new HashSet<DeduccionTemporal>();

    }


    public int PK_TipoDeduccionTemporal { get; set; }

    public string DesTipoDeduccionTemporal { get; set; }

    public int IndEstado { get; set; }

    public int IndConSalario { get; set; }



    public virtual ICollection<DeduccionTemporal> DeduccionTemporal { get; set; }

}

}
