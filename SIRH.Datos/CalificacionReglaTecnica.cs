
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
    
public partial class CalificacionReglaTecnica
{

    public int PK_CalificacionReglaTecnica { get; set; }

    public int FK_PeriodoCalificacion { get; set; }

    public int IndDireccionGeneral { get; set; }

    public int IndDirector { get; set; }

    public string DesCorreo { get; set; }

    public int NumFuncionarios { get; set; }

    public int NumFuncionariosExcelentes { get; set; }

    public int IndEstado { get; set; }

    public byte[] ImgDocumento { get; set; }



    public virtual PeriodoCalificacion PeriodoCalificacion { get; set; }

}

}
