
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
    
public partial class OrdenMovimientoDeclaracion
{

    public int PK_Declaracion { get; set; }

    public int FK_OrdenMovimiento { get; set; }

    public System.DateTime FecDeclaracion { get; set; }

    public string DesCondicionAcademica { get; set; }

    public string DesExperiencia { get; set; }

    public string DesCapacitacion { get; set; }

    public string DesLicencia { get; set; }

    public string DesColegiatura { get; set; }



    public virtual OrdenMovimiento OrdenMovimiento { get; set; }

}

}