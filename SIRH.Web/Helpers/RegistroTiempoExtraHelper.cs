using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using SIRH.DTO;
//using SIRH.Web.RegistroTELocal;
using SIRH.Web.RegistroTEService;

namespace SIRH.Web.Helpers
{
    //Parasubir
    public static class RegistroTiempoExtraHelper
    {
        public static bool EsAdministrativo(string ocupacion)
        {
            return !ocupacion.StartsWith("GUARDA");
        }
        public static int DefinirHorasPorJornada(JornadaEnum jornada)
        {
            switch (jornada)
            {
                case JornadaEnum.D:
                case JornadaEnum.DD:
                    return 240;
                case JornadaEnum.M:
                case JornadaEnum.MD:
                    return 210;
                case JornadaEnum.N:
                case JornadaEnum.ND:
                    return 180;
                default: return 0;
            }
        }
        public static bool EsVacio(CDetalleTiempoExtraDTO detalle)
        {
            return (detalle.HoraInicio == null && detalle.MinutoInicio == null && detalle.HoraFinal == null && detalle.MinutoFinal == null);
        }
        public static bool EsVacioDoble(CDetalleTiempoExtraDTO detalle)
        {
            return (detalle.HoraInicio == null && detalle.MinutoInicio == null && detalle.HoraFinal == null && detalle.MinutoFinal == null
                && detalle.HoraTotalH0 == null && detalle.HoraTotalH1 == null && detalle.HoraTotalH2 == null && detalle.MinutoTotalH0 == null && detalle.MinutoTotalH1 == null && detalle.MinutoTotalH2 == null);
        }
        public static bool EsDiaLibre(CDetalleTiempoExtraDTO detalle)
        {
            return detalle.FechaInicio.DayOfWeek == DayOfWeek.Saturday || detalle.FechaInicio.DayOfWeek == DayOfWeek.Sunday || DiasFestivosHelper.EsFeriado(detalle.FechaInicio);
        }
        public static List<string> DeterminarMeses(int mesesAtras)
        {
            List<string> meses = new List<string>();
            for (int i = 1; i <= mesesAtras; i++)
            {
                DateTime mesAntN = DateTime.Today.AddMonths(-i);
                meses.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[mesAntN.Month - 1][0].ToString().ToUpper()
               + (System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[mesAntN.Month - 1]).Substring(1)
               + " " + mesAntN.Year.ToString());
            }
            return meses;
        }
        public static DateTime ConvertirFechaPago(string fecPago)
        {
            return new DateTime(DateTime.Now.Year, DateTime.ParseExact(fecPago.Substring(3, fecPago.Length - 3), "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month, fecPago.StartsWith("1Q") ? 14 : 28);
        }

        public static string CalcularPeriodoPago() {
            return (DateTime.Now.Day > 0 && DateTime.Now.Day < 9 ? "1Q " : "2Q ") + //Si diaActual esta entre 1 y 8 => 1 Quincena, si es despues 2 Quncena 
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[DateTime.Now.Month % 12][0].ToString().ToUpper() + //Si mesActual == 12, paga enero=0. Si mesActual < 12, paga mesActual+1
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[DateTime.Now.Month % 12].Substring(1);
        }
        public static string CalcularPeriodoPagoRegistrado(DateTime dateTime)
        {
            if ((dateTime.Month % 12) > 0)
            {
                return (dateTime.Day == 14 ? "1Q " : "2Q ") +
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[(dateTime.Month % 12) - 1][0].ToString().ToUpper() +
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[(dateTime.Month % 12) - 1].Substring(1);
            }
            else
            {
                return (dateTime.Day == 14 ? "1Q " : "2Q ") +
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[11][0].ToString().ToUpper() +
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[11].Substring(1);
            }
        }
        //public static List<decimal> CalcularHorasFila(int horas, CDetalleTiempoExtraDTO detalle)
        //{
        //    List<decimal> respuesta = new List<decimal>();
        //    switch (horas)
        //    {
        //        case 240:
        //            respuesta = CalcularHorasFila240(detalle);
        //            break;
        //        case 210:
        //            respuesta = CalcularHorasFila210(detalle);
        //            break;
        //        case 180:
        //            respuesta = CalcularHorasFila180(detalle);
        //            break;
        //        default:
        //            respuesta.Add(0);
        //            respuesta.Add(0);
        //            respuesta.Add(0);
        //            break;
        //    }
        //    return respuesta;
        //}

        public static List<decimal> CalcularHorasFilaGuardaReporte(CDetalleTiempoExtraDTO detalle)
        {
            List<decimal> respuesta = new List<decimal>();
            TimeSpan horasExtra;
            horasExtra = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
            int jornada;
            if (detalle.Jornada == JornadaEnum.D || detalle.Jornada == JornadaEnum.DD)
            {
                jornada = 8;
                if(detalle.FechaInicioEspecial || detalle.FechaFinalEspecial)
                {
                    if((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > jornada)
                    {
                        respuesta.Add(jornada);//240
                        respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 100);//210
                        respuesta.Add(0);//180
                    }
                    else
                    {
                        respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 60);//240
                        respuesta.Add(0);//210
                        respuesta.Add(0);//180
                    }
                }
                else if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > jornada)//No es día especial, pero es mayor a la jornada, el resto son extras
                {
                        respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 60);//240
                        respuesta.Add(0);//210
                        respuesta.Add(0);//180
                }
                else //Dijito menos de la jornada normal y no es especial, entonces no hay extras
                {
                    respuesta.Add(0);//240
                    respuesta.Add(0);//210
                    respuesta.Add(0);//180
                }
                return respuesta;
            }
            else if (detalle.Jornada == JornadaEnum.M || detalle.Jornada == JornadaEnum.MD)
            {
                jornada = 7;
                if (detalle.FechaInicioEspecial || detalle.FechaFinalEspecial)
                {
                    if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > jornada) //Si es dia especial, 
                    {
                        respuesta.Add(jornada);//240
                        respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 60);//210
                        respuesta.Add(0);//180
                    }
                    else
                    {
                        respuesta.Add(0);//240
                        respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 60);//210
                        respuesta.Add(0);//180
                    }
                }
                else if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > jornada)//No es día especial, pero es mayor a la jornada, el resto son extras
                {
                    respuesta.Add(0);//240
                    respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 60);//210
                    respuesta.Add(0);//180
                }
                else //Dijito menos de la jornada normal y no es especial, entonces no hay extras
                {
                    respuesta.Add(0);//240
                    respuesta.Add(0);//210
                    respuesta.Add(0);//180
                }
                return respuesta;
            }
            else if (detalle.Jornada == JornadaEnum.N || detalle.Jornada == JornadaEnum.ND)
            {
                jornada = 6;
                if (detalle.FechaInicioEspecial || detalle.FechaFinalEspecial)
                {
                    if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 100) > jornada) //Si es dia especial, 
                    {
                        respuesta.Add(jornada);//240
                        respuesta.Add(0);//210
                        respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 100);//180
                    }
                    else
                    {
                        respuesta.Add(0);//240
                        respuesta.Add(0);//210
                        respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 100);//180
                    }
                }
                else if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 100) > jornada) //No es día especial, pero es mayor a la jornada, el resto son extras
                {
                    respuesta.Add(0);//240
                    respuesta.Add(0);//210
                    respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 100);//180
                }
                else //Dijito menos de la jornada normal y no es especial, entonces no hay extras
                {
                    respuesta.Add(0);//240
                    respuesta.Add(0);//210
                    respuesta.Add(0);//180
                }
                return respuesta;
            }
            //Error en las jornadas
            respuesta.Add(0);
            respuesta.Add(0);
            respuesta.Add(0);
            return respuesta;
        }
        public static List<decimal> CalcularHorasFilaGuardaVista(CDetalleTiempoExtraDTO detalle, string reporte)
        {
            List<decimal> respuesta = new List<decimal>();
            TimeSpan horasExtra;
            horasExtra = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
            int jornada;
            switch ((JornadaEnum)detalle.Jornada)
            {
                case JornadaEnum.D: jornada = 8; break;
                case JornadaEnum.M: jornada = 7; break;
                case JornadaEnum.N: jornada = 6; break;
                default: jornada = 0; break;
            }
            if (detalle.FechaInicioEspecial || detalle.FechaFinalEspecial)//Si no funciona usar tipo extra
            {
                if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > jornada)
                {
                    respuesta.Add(jornada);//H0
                    respuesta.Add(0);//H1
                    respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 60);//H2
                }
                else //Si son menos de la jornada pero es feriado, cuenta
                {
                    respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 60);
                    respuesta.Add(0);
                    respuesta.Add(0);
                }
            }
            else if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > jornada)
            {
                if (reporte == "reporte" && jornada == 6)
                {
                    respuesta.Add(0);//H0
                    respuesta.Add(0);
                    respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 60);
                }
                else
                {
                    respuesta.Add(0);//H0
                    respuesta.Add((horasExtra.Hours - jornada) + (decimal)horasExtra.Minutes / 60);
                    respuesta.Add(0);
                }
            }
            else //Si es menor o igual a jornada y no es feriado, no hay extras
            {
                respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 60);
                respuesta.Add(0);
                respuesta.Add(0);
            }
            return respuesta;
        }
        //public static List<decimal> CalcularHorasFilaAdministrativo2(CDetalleTiempoExtraDTO detalle)
        //{
        //    List<decimal> respuesta = new List<decimal>();
        //    TimeSpan horasExtra;
        //    horasExtra = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
        //    if (EsDiaLibre(detalle))
        //    {
        //        if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 100) > 8)
        //        {
        //            respuesta.Add(8);//H0
        //            respuesta.Add(0);//H1
        //            respuesta.Add((horasExtra.Hours - 8) + (decimal)horasExtra.Minutes / 100);//H2
        //        }
        //        else //Si son menos de 8 pero es feriado, cuenta
        //        {
        //            respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 100);
        //            respuesta.Add(0);
        //            respuesta.Add(0);
        //        }
        //    }
        //    else if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 100) > 8)
        //    {
        //        //respuesta.Add(8);//H0
        //        respuesta.Add(0);//H0
        //        respuesta.Add((horasExtra.Hours - 8) + (decimal)horasExtra.Minutes / 100);
        //        respuesta.Add(0);
        //    }
        //    else //Si es menor o igual a 8 y no es feriado, no hay extras
        //    {
        //        //respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 100);
        //        respuesta.Add(0);
        //        respuesta.Add(0);
        //        respuesta.Add(0);
        //    }
        //    return respuesta;
        //}
        public static List<decimal> CalcularHorasFilaAdministrativo(CDetalleTiempoExtraDTO detalle)
        {
            List<decimal> respuesta = new List<decimal>();
            TimeSpan horasExtra;
            horasExtra = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
            if (EsDiaLibre(detalle))
            {
                if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > 8)
                {
                    respuesta.Add(8);//H0
                    respuesta.Add(0);//H1
                    respuesta.Add((horasExtra.Hours - 8) + (decimal)horasExtra.Minutes / 60);//H2
                }
                else //Si son menos de 8 pero es feriado, cuenta
                {
                    respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 60);
                    respuesta.Add(0);
                    respuesta.Add(0);
                }
            }
            else if ((horasExtra.Hours + (decimal)horasExtra.Minutes / 60) > 8)
            {
                //respuesta.Add(8);//H0
                respuesta.Add(0);//H0
                respuesta.Add((horasExtra.Hours - 8) + (decimal)horasExtra.Minutes / 60);
                respuesta.Add(0);
            }
            else //Si es menor o igual a 8, cuenta solo se ingreesaron extras
            {
                //respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 100);
                respuesta.Add(0);
                respuesta.Add((horasExtra.Hours) + (decimal)horasExtra.Minutes / 60);
                respuesta.Add(0);
            }
            return respuesta;
        }
        public static void FormatearFechas(CDetalleTiempoExtraDTO detalle)
        {
            detalle.HoraInicioDate = DateTime.Parse(detalle.HoraInicio + ":" + detalle.MinutoInicio);
            detalle.HoraFinalDate = DateTime.Parse(detalle.HoraFinal + ":" + detalle.MinutoFinal);
            if (detalle.HoraFinalDate <= detalle.HoraInicioDate)  //Identifica cambio de día
            {
                detalle.FechaFinal = detalle.FechaInicio.AddDays(1);
                detalle.HoraInicioDate = new DateTime(detalle.FechaInicio.Year, detalle.FechaInicio.Month, detalle.FechaInicio.Day, detalle.HoraInicioDate.Hour, detalle.HoraInicioDate.Minute, 0);
                detalle.HoraFinalDate = new DateTime(detalle.FechaFinal.Year, detalle.FechaFinal.Month, detalle.FechaFinal.Day, detalle.HoraFinalDate.Hour, detalle.HoraFinalDate.Minute, 0);
            }
        }
        public static bool ValidarHoraCompleta(int jornada, CDetalleTiempoExtraDTO detalle, bool guarda)
        {
            TimeSpan horasExtra = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
            if (!guarda && EsDiaLibre(detalle))
            {
                return horasExtra.Hours >= 1;
            }
            else if (guarda && (detalle.FechaInicioEspecial || detalle.FechaFinalEspecial))
            {
                return horasExtra.Hours >= 1;
            }
            else if (horasExtra.Hours + (decimal)horasExtra.Minutes / 60 > jornada)
            {
                horasExtra = horasExtra.Subtract(new TimeSpan(jornada, 0, 0));
            }
            return horasExtra.Hours > 0;
        }

        //public static string ValidarRegistroAdministrativo2(CDetalleTiempoExtraDTO detalle, CRegistroTEServiceClient servicioTiempoExtra, string cedula, ref bool hayHoraCompleta)
        //{
        //    if (detalle.HoraInicio == null || detalle.MinutoInicio == null) //Error en la hora o en el minuto de Incio
        //    {
        //        return "Error:1";
        //    }
        //    if (detalle.HoraFinal == null || detalle.MinutoFinal == null) //Error en la hora o en el minuto Final
        //    {
        //        return "Error:2";
        //    }
        //    FormatearFechas(detalle);//Para las siguientes validaciones y calculos es necesario tener las fechas establecidas
        //    TimeSpan tiempoTotal = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
        //    if (tiempoTotal.Hours + (decimal)tiempoTotal.Minutes / 100 <= 8 && !EsDiaLibre(detalle)) //Valida que registro mas tiempo que lo que dura la jornada normal
        //    {
        //        return "Error:H";//Si no completa la jornada normal, no hay extras.
        //    }
        //    DateTime fechaFin;
        //    DateTime fechaInicio = new DateTime(detalle.FechaInicio.Year, detalle.FechaInicio.Month, detalle.FechaInicio.Day,
        //        detalle.HoraInicioDate.Hour, detalle.HoraInicioDate.Minute, 0);
        //    if (detalle.FechaFinal == DateTime.MinValue)
        //    {
        //        fechaFin = detalle.FechaInicio;
        //    }
        //    else
        //    {
        //        fechaFin = new DateTime(detalle.FechaFinal.Year, detalle.FechaFinal.Month, detalle.FechaFinal.Day,
        //                                                            detalle.HoraFinalDate.Hour, detalle.HoraFinalDate.Minute, 0);
        //    }
        //    var respuesta = servicioTiempoExtra.EstaEnVacaciones(cedula, fechaInicio, fechaFin);
        //    if (respuesta.Codigo > 2)//Valida que el funcionario no se encontraba en vacaciones
        //    {
        //        return "Error:V";
        //    }
        //    respuesta = servicioTiempoExtra.EstaIncapacitado(cedula, fechaInicio, fechaFin);
        //    if (respuesta.Codigo > 2)//Valida que el funcionario no se encontraba incapacitado
        //    {
        //        return "Error:I";
        //    }
        //    if (!hayHoraCompleta)//Valida que hayan al menos 60 minutos de extras antes de registrar menos de una hora por día
        //    {
        //        hayHoraCompleta = ValidarHoraCompleta(8, detalle);
        //        if (!hayHoraCompleta)//Se pone dos veces, así la primera que lo encuentre no vuelve a llamar al metodo en otras iteraciones
        //        {
        //            return "Error:C";
        //        }
        //    }

        //    return "correcto";
        //}
        public static string ValidarRegistroAdministrativo(CDetalleTiempoExtraDTO detalle, DateTime fecNombVence, CRegistroTEServiceClient servicioTiempoExtra, string cedula, ref bool hayHoraCompleta)
        {
            if (detalle.HoraInicio == null || detalle.MinutoInicio == null) //Error en la hora o en el minuto de Incio
            {
                return "Error:1";
            }
            if (detalle.HoraFinal == null || detalle.MinutoFinal == null) //Error en la hora o en el minuto Final
            {
                return "Error:2";
            }
            if (!(Convert.ToInt32(detalle.HoraInicio) >= 0 && Convert.ToInt32(detalle.HoraInicio) <= 23) || !(Convert.ToInt32(detalle.MinutoInicio) >= 0 && Convert.ToInt32(detalle.MinutoInicio) <= 59)) //Error en la hora o en el minuto de Inicio
            {
                return "Error:3";
            }
            if (!(Convert.ToInt32(detalle.HoraFinal) >= 0 && Convert.ToInt32(detalle.HoraFinal) <= 23) || !(Convert.ToInt32(detalle.MinutoFinal) >= 0 && Convert.ToInt32(detalle.MinutoFinal) <= 59)) //Error en la hora o en el minuto Final
            {
                return "Error:4";
            }
            FormatearFechas(detalle);//Para las siguientes validaciones y calculos es necesario tener las fechas establecidas
            //TimeSpan tiempoTotal = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
            //if (tiempoTotal.Hours + (decimal)tiempoTotal.Minutes / 100 <= 8 && !EsDiaLibre(detalle)) //Valida que registro mas tiempo que lo que dura la jornada normal
            //{
            //    return "Error:H";//Si no completa la jornada normal, no hay extras.
            //}
            DateTime fechaFin;
            DateTime fechaInicio = new DateTime(detalle.FechaInicio.Year, detalle.FechaInicio.Month, detalle.FechaInicio.Day,
                detalle.HoraInicioDate.Hour, detalle.HoraInicioDate.Minute, 0);
            if (detalle.FechaFinal == DateTime.MinValue)
            {
                fechaFin = detalle.FechaInicio;
            }
            else
            {
                fechaFin = new DateTime(detalle.FechaFinal.Year, detalle.FechaFinal.Month, detalle.FechaFinal.Day,
                                                                    detalle.HoraFinalDate.Hour, detalle.HoraFinalDate.Minute, 0);
            }
            if(fecNombVence != DateTime.MinValue && detalle.FechaInicio >= fecNombVence)
            {
                //return "Error:F";
            }
            var respuesta = servicioTiempoExtra.EstaEnVacaciones(cedula, fechaInicio, fechaFin);
            if (respuesta.Codigo > 0)//Valida que el funcionario no se encontraba en vacaciones
            {
                return "Error:V";
            }
            respuesta = servicioTiempoExtra.EstaIncapacitado(cedula, fechaInicio, fechaFin);
            if (respuesta.Codigo > 0)//Valida que el funcionario no se encontraba incapacitado
            {
                return "Error:I";
            }
            if (!hayHoraCompleta)//Valida que hayan al menos 60 minutos de extras antes de registrar menos de una hora por día
            {
                hayHoraCompleta = ValidarHoraCompleta(8, detalle, false);
                if (!hayHoraCompleta)//Se pone dos veces, así la primera que lo encuentre no vuelve a llamar al metodo en otras iteraciones
                {
                    return "Error:C";
                }
            }
            return "correcto";
        }
        public static string ValidarRegistroGuarda(CDetalleTiempoExtraDTO detalle, DateTime fecNombVence, CRegistroTEServiceClient servicioTiempoExtra, string cedula, ref bool hayHoraCompleta, bool jornadaDoble = false, List<DateTime> detallesFecInicio = null)
        {
            if (jornadaDoble)
            {
                if (EsVacioDoble(detalle))
                {
                    return "Error:N";
                }
                if(detallesFecInicio != null)
                {
                    if (!detallesFecInicio.Contains(detalle.FechaInicio))
                    {
                        return "Error:X";
                    }
                }
            }
            if (detalle.HoraInicio == null || detalle.MinutoInicio == null) //Error en la hora o en el minuto de Incio
            {
                return "Error:1";
            }
            if (detalle.HoraFinal == null || detalle.MinutoFinal == null) //Error en la hora o en el minuto Final
            {
                return "Error:2";
            }
            if (!(Convert.ToInt32(detalle.HoraInicio) >= 0 && Convert.ToInt32(detalle.HoraInicio) <= 23) || !(Convert.ToInt32(detalle.MinutoInicio) >= 0 && Convert.ToInt32(detalle.MinutoInicio) <= 59)) //Error en la hora o en el minuto de Inicio
            {
                return "Error:3";
            }
            if (!(Convert.ToInt32(detalle.HoraFinal) >= 0 && Convert.ToInt32(detalle.HoraFinal) <= 23) || !(Convert.ToInt32(detalle.MinutoFinal) >= 0 && Convert.ToInt32(detalle.MinutoFinal) <= 59)) //Error en la hora o en el minuto Final
            {
                return "Error:4";
            }
            if (jornadaDoble)
            {
                if ((detalle.HoraTotalH0 != null || detalle.MinutoTotalH0 != null) && (detalle.HoraTotalH0 == null || detalle.MinutoTotalH0 == null)) //Error en la hora o en el minuto de H0, puede ser vacio, pero no a medias
                {
                    return "Error:5"; 
                }
                if ((detalle.HoraTotalH1 != null || detalle.MinutoTotalH1 != null) && (detalle.HoraTotalH1 == null || detalle.MinutoTotalH1 == null)) //Error en la hora o en el minuto de H1, puede ser vacio, pero no a medias
                {
                    return "Error:6";
                }
                if ((detalle.HoraTotalH2 != null || detalle.MinutoTotalH2 != null) && (detalle.HoraTotalH2 == null || detalle.MinutoTotalH2 == null)) //Error en la hora o en el minuto de H2, puede ser vacio, pero no a medias
                {
                    return "Error:7";
                }
                decimal totalH0 = Convert.ToInt32(detalle.HoraTotalH0) + (decimal)Convert.ToInt32(detalle.MinutoTotalH0) / 100;
                decimal totalH1 = Convert.ToInt32(detalle.HoraTotalH1) + (decimal)Convert.ToInt32(detalle.MinutoTotalH1) / 100;
                decimal totalH2 = Convert.ToInt32(detalle.HoraTotalH2) + (decimal)Convert.ToInt32(detalle.MinutoTotalH2) / 100;
                if ( totalH0 <= 0 && totalH1 <= 0 && totalH2 <= 0)
                {
                    return "Error:8";
                }
                if(totalH0 > 0 && totalH1 > 0 && totalH2 > 0)
                {
                    return "Error:9";
                }
                if (totalH1 > 0 && totalH2 > 0)
                {
                    return "Error:T";
                }
                if (detalle.Jornada != JornadaEnum.DD && detalle.Jornada != JornadaEnum.MD && detalle.Jornada != JornadaEnum.ND)//Error, no se selecciono una jornada
                {
                    return "Error:J";
                }
            }
            else 
            {
                if (detalle.Jornada != JornadaEnum.D && detalle.Jornada != JornadaEnum.M && detalle.Jornada != JornadaEnum.N)//Error, no se selecciono una jornada
                {
                    return "Error:J";
                } 
            }
            FormatearFechas(detalle);//Para las siguientes validaciones y calculos es necesario tener las fechas establecidas
            TimeSpan tiempoTotal = detalle.HoraFinalDate.Subtract(detalle.HoraInicioDate);
            //D = 8, diurna  |  M = 7, Mixta  |  N = 6, Noctura
            int jornada = 0;
            if (detalle.Jornada == JornadaEnum.D || detalle.Jornada == JornadaEnum.DD) jornada = 8;
            else if (detalle.Jornada == JornadaEnum.M || detalle.Jornada == JornadaEnum.MD) jornada = 7;
            else if (detalle.Jornada == JornadaEnum.N || detalle.Jornada == JornadaEnum.ND) jornada = 6;
            if (!jornadaDoble && tiempoTotal.Hours + (decimal)tiempoTotal.Minutes / 100 <= jornada && !detalle.FechaInicioEspecial && !detalle.FechaFinalEspecial) //Valida que lo que registro dure mas tiempo que lo que dura la jornada normal
            {
                return "Error:H";//Si no completa la jornada normal, no hay extras.
            }
            DateTime fechaFin;
            DateTime fechaInicio = new DateTime(detalle.FechaInicio.Year, detalle.FechaInicio.Month, detalle.FechaInicio.Day,
                detalle.HoraInicioDate.Hour, detalle.HoraInicioDate.Minute, 0);
            if (detalle.FechaFinal == DateTime.MinValue)
            {
                fechaFin = detalle.FechaInicio;
            }
            else
            {
                fechaFin = new DateTime(detalle.FechaFinal.Year, detalle.FechaFinal.Month, detalle.FechaFinal.Day,
                                                                    detalle.HoraFinalDate.Hour, detalle.HoraFinalDate.Minute, 0);
            }
            if(fecNombVence != DateTime.MinValue && detalle.FechaInicio >= fecNombVence)
            {
                return "Error:F";
            }
            var respuesta = servicioTiempoExtra.EstaEnVacaciones(cedula, fechaInicio, fechaFin);
            if (respuesta.Codigo > 0)//Valida que el funcionario no se encontraba en vacaciones
            {
                return "Error:V";
            }
            respuesta = servicioTiempoExtra.EstaIncapacitado(cedula, fechaInicio, fechaFin);
            if (respuesta.Codigo > 0)//Valida que el funcionario no se encontraba incapacitado
            {
                return "Error:I";
            }
            if (!jornadaDoble && !hayHoraCompleta)//Valida que hayan al menos 60 minutos de extras antes de registrar menos de una hora por día
            {//D = 8, diurna  |  M = 7, Mixta  |  N = 6, Noctura
                hayHoraCompleta = ValidarHoraCompleta(jornada, detalle, true);
                if (!hayHoraCompleta)//Se pone dos veces, así la primera que lo encuentre no vuelve a llamar al metodo en otras iteraciones
                {
                    return "Error:C";
                }
            }
            return "correcto";
        }
        public static int DefinirTipoExtra(CDetalleTiempoExtraDTO detalle, bool guarda)
        {
            if (guarda)
                return DefinirTipoExtraGuarda(detalle);
            switch (detalle.Jornada)
            {
                case JornadaEnum.D: return EsDiaLibre(detalle) ? 2 : 1;//Diurna: 2=>Diurna en dia libre, 1=>Diurna en dia normal
                case JornadaEnum.M: return EsDiaLibre(detalle) ? 4 : 3;//Diurna: 4=>Mixta en dia libre, 3=>Mixta en dia normal
                case JornadaEnum.N: return EsDiaLibre(detalle) ? 6 : 5;//Diurna: 6=>Noctura en dia libre, 5=>Nocturna en dia normal
                default:  return 0;//Error en los datos
            }
        }
        private static int DefinirTipoExtraGuarda(CDetalleTiempoExtraDTO detalle)
        {
            if (detalle.Jornada == JornadaEnum.D) return detalle.FechaInicioEspecial ? 2 : 1;
            if(detalle.Jornada == JornadaEnum.M) return detalle.FechaInicioEspecial ? 4 : 3;
            if(detalle.Jornada == JornadaEnum.N)
            {
                if (!detalle.FechaInicioEspecial && !detalle.FechaFinalEspecial) return 5;
                if (!detalle.FechaInicioEspecial && detalle.FechaFinalEspecial) return 6;
                if (detalle.FechaInicioEspecial && !detalle.FechaFinalEspecial) return 7;
                if (detalle.FechaInicioEspecial && detalle.FechaFinalEspecial) return 8;
            }
            return 0;
        }
        public static int DefinirTipoExtraDoble(CDetalleTiempoExtraDTO detalle)
        {
            if (detalle.Jornada == JornadaEnum.DD) return detalle.FechaInicioEspecial ? 10 : 9;
            if (detalle.Jornada == JornadaEnum.MD) return detalle.FechaInicioEspecial ? 12 : 11;
            if (detalle.Jornada == JornadaEnum.ND)
            {
                if (!detalle.FechaInicioEspecial && !detalle.FechaFinalEspecial) return 13;
                if (!detalle.FechaInicioEspecial && detalle.FechaFinalEspecial) return 14;
                if (detalle.FechaInicioEspecial && !detalle.FechaFinalEspecial) return 15;
                if (detalle.FechaInicioEspecial && detalle.FechaFinalEspecial) return 16;
            }
            return 0;
        }
        public static JornadaEnum DefinirJornada(int tipoExtra)
        {
            switch (tipoExtra)
            {
                case 1:
                case 2:
                    return JornadaEnum.D;
                case 3:
                case 4:
                    return JornadaEnum.M;
                case 5:
                case 6:
                case 7:
                case 8:
                    return JornadaEnum.N;
                case 9:
                case 10:
                    return JornadaEnum.DD;
                case 11:
                case 12:
                    return JornadaEnum.MD;
                case 13:
                case 14:
                case 15:
                case 16:
                    return JornadaEnum.ND;
                default: return JornadaEnum.D;
            }
        }
        public static List<bool> DefinirDiaLibre(CDetalleTiempoExtraDTO detalle, bool doble)
        {
            List<bool> libres = new List<bool>();
            if (doble)
            {
                if (detalle.Jornada == JornadaEnum.D || detalle.Jornada == JornadaEnum.DD ||
                    detalle.Jornada == JornadaEnum.M || detalle.Jornada == JornadaEnum.MD)
                {
                    libres.Add(true);
                    libres.Add(false);
                }
                else if (detalle.TipoExtra.IdEntidad == 6 || detalle.TipoExtra.IdEntidad == 14)
                {
                    libres.Add(false);
                    libres.Add(true);
                }
                else if (detalle.TipoExtra.IdEntidad == 7 || detalle.TipoExtra.IdEntidad == 15)
                {
                    libres.Add(true);
                    libres.Add(false);
                }
                else if (detalle.TipoExtra.IdEntidad == 8 || detalle.TipoExtra.IdEntidad == 16)
                {
                    libres.Add(true);
                    libres.Add(true);
                }
            }
            else
            {
                libres.Add(false);
                libres.Add(false);
            }
            return libres;
        }
        public static decimal CalcularMontoPagoPorHorasVista(decimal montoPorHora, decimal totalHoras, string tipoHora)
        {
            if (tipoHora == "H0")//Jornada normal en días feriados (x1), En H0 se identifican horas en dias libres
            {
                return montoPorHora * totalHoras;
            }
            if (tipoHora == "H1")//Tiempo extra en días regulares (x1.5)
            {
                return (montoPorHora * Convert.ToDecimal(1.5)) * totalHoras;
            }
            if(tipoHora == "H2")//Tiempo extra en feriados, como estos no estan incluidos en el pago, se multiplica por 2
            {
                return (montoPorHora) * totalHoras * 2;
            }
            return 0; //tipoHora tenia un valor incorrecto
        }
        public static decimal CalcularMontoPagoPorHorasGuardaReporte(decimal montoPorHora, decimal totalHoras, string tipoHora, bool feriado = false)
        {
            if (feriado)
            {
                if (tipoHora == "210" || tipoHora == "180")
                {
                    return montoPorHora * totalHoras * 2;
                }
                else if(tipoHora == "240")
                {
                    return montoPorHora * totalHoras;
                }
            }
            else if(tipoHora == "240" || tipoHora == "210" || tipoHora == "180")//Si no es feriado son extras normales
            {
                return montoPorHora * totalHoras * Convert.ToDecimal(1.5);
            }
            return 0;
        }
       /* public static decimal CalcularMontoLinea(decimal salario, int tipoExtra, List<decimal> calculo)
        {
            decimal respuesta = 0;

            switch (tipoExtra)
            {
                case 1:
                    respuesta = ((salario / 240) * Convert.ToDecimal(1.5)) * calculo[1];
                    break;
                case 2:
                    respuesta = ((salario / 240) * 2) * calculo[2];
                    break;
                case 3:
                    respuesta = ((salario / 210) * Convert.ToDecimal(1.5)) * calculo[1];
                    break;
                case 4:
                    respuesta = ((salario / 210) * 2) * calculo[2];
                    break;
                case 5:
                    respuesta = ((salario / 180) * 2) * calculo[1];
                    break;
                case 6:
                    respuesta = ((salario / 180) * 2) * calculo[2];
                    break;
                default:
                    break;
            }


            return respuesta;
        }*/

    }


}
