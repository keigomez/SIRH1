
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
    
public partial class PeriodoVacaciones
{

    public PeriodoVacaciones()
    {

        this.RegistroVacaciones = new HashSet<RegistroVacaciones>();

    }


    public int PK_PeriodoVacaciones { get; set; }

    public Nullable<decimal> IndDiasDerecho { get; set; }

    public System.DateTime FecCarga { get; set; }

    public string IndPeriodo { get; set; }

    public int IndEstado { get; set; }

    public Nullable<int> FK_Nombramiento { get; set; }

    public Nullable<double> IndSaldo { get; set; }



    public virtual Nombramiento Nombramiento { get; set; }

    public virtual ICollection<RegistroVacaciones> RegistroVacaciones { get; set; }

}

}