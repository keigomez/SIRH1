
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
    
public partial class Perfil
{

    public Perfil()
    {

        this.CatalogoIncidencia = new HashSet<CatalogoIncidencia>();

        this.CatPermiso = new HashSet<CatPermiso>();

    }


    public int PK_Perfil { get; set; }

    public string NomPerfil { get; set; }

    public string DesPerfil { get; set; }

    public Nullable<int> IndEstadoPerfil { get; set; }



    public virtual ICollection<CatalogoIncidencia> CatalogoIncidencia { get; set; }

    public virtual ICollection<CatPermiso> CatPermiso { get; set; }

}

}
