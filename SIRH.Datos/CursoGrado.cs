
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
    
public partial class CursoGrado
{

    public int PK_CursoGrado { get; set; }

    public string DesCursoGrado { get; set; }

    public Nullable<int> PorIncentivo { get; set; }

    public Nullable<int> FK_FormacionAcademica { get; set; }

    public Nullable<int> TipCursoGrado { get; set; }

    public Nullable<int> FK_EntidadEducativa { get; set; }

    public Nullable<System.DateTime> FecEmision { get; set; }

    public string NumResolucion { get; set; }

    public byte[] ImgTitulo { get; set; }

    public Nullable<int> Estado { get; set; }

    public string Observaciones { get; set; }



    public virtual EntidadEducativa EntidadEducativa { get; set; }

    public virtual FormacionAcademica FormacionAcademica { get; set; }

}

}
