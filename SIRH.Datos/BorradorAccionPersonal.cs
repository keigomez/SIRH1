
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
    
public partial class BorradorAccionPersonal
{

    public BorradorAccionPersonal()
    {

        this.MovimientoBorradorAccionPersonal = new HashSet<MovimientoBorradorAccionPersonal>();

        this.DetalleBorradorAccionPersonal = new HashSet<DetalleBorradorAccionPersonal>();

    }


    public int PK_Borrador { get; set; }

    public int FK_EstadoBorrador { get; set; }

    public string NumOficio { get; set; }

    public string UsuarioAsignado { get; set; }

    public string ObsJustificacion { get; set; }



    public virtual ICollection<MovimientoBorradorAccionPersonal> MovimientoBorradorAccionPersonal { get; set; }

    public virtual EstadoBorrador EstadoBorrador { get; set; }

    public virtual ICollection<DetalleBorradorAccionPersonal> DetalleBorradorAccionPersonal { get; set; }

}

}
