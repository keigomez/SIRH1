
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
    
public partial class MovimientoBorradorAccionPersonal
{

    public int PK_MovimientoBorrador { get; set; }

    public int FK_Borrador { get; set; }

    public System.DateTime FecMovimiento { get; set; }

    public int CodMovimiento { get; set; }

    public string UsuarioEnvia { get; set; }

    public string UsuarioRecibe { get; set; }

    public string ObsMovimiento { get; set; }



    public virtual BorradorAccionPersonal BorradorAccionPersonal { get; set; }

}

}
