
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
    
public partial class Partida
{

    public Partida()
    {

        this.SubPartida = new HashSet<SubPartida>();

    }


    public int PK_Partida { get; set; }

    public string CodPartida { get; set; }

    public string DesPartida { get; set; }



    public virtual ICollection<SubPartida> SubPartida { get; set; }

}

}
