
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
    
public partial class CatalogoRemuneracion
{

    public CatalogoRemuneracion()
    {

        this.RemuneracionEfectuadaPF = new HashSet<RemuneracionEfectuadaPF>();

    }


    public int PK_CatalogoRemuneracion { get; set; }

    public decimal PorRemuneracion { get; set; }

    public string DesRemuneracion { get; set; }



    public virtual ICollection<RemuneracionEfectuadaPF> RemuneracionEfectuadaPF { get; set; }

}

}
