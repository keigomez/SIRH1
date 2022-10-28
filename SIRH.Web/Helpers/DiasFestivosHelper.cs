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

namespace SIRH.Web.Helpers
{
    public static class DiasFestivosHelper
    {
        public static bool EsFeriado(DateTime dia)
        {
            bool respuesta = false;

            if (EsJuevesOViernesDePascua(dia))
                respuesta = true;
            else if (dia.Day == 1 && dia.Month == 1)
                respuesta = true;
            else if (dia.Day == 11 && dia.Month == 4)
                respuesta = true;
            else if (dia.Day == 1 && dia.Month == 5)
                respuesta = true;
            else if (dia.Day == 26 && dia.Month == 7)
                respuesta = true;
            else if (dia.Day == 2 && dia.Month == 8)
                respuesta = true;
            else if (dia.Day == 15 && dia.Month == 8)
                respuesta = true;
            else if (dia.Day == 13 && dia.Month == 9)
                respuesta = true;
            else if (dia.Day == 29 && dia.Month == 11)
                respuesta = true;
            else if (dia.Day == 25 && dia.Month == 12)
                respuesta = true;
            else if (dia.DayOfWeek == DayOfWeek.Sunday)
                respuesta = true;

            return respuesta;
        }

        #region CalcularPascua

        private static bool EsJuevesOViernesDePascua(DateTime dia)
        {
            bool respuesta = false;

            if (dia.Month == 3 || dia.Month == 4)
            {
                if (dia.DayOfWeek == DayOfWeek.Thursday || dia.DayOfWeek == DayOfWeek.Friday)
                {
                    if (calculaDomingoPascua(dia.Year).AddDays(-2) == dia || calculaDomingoPascua(dia.Year).AddDays(-3) == dia)
                    {
                        respuesta = true;
                    }
                }
            }

            return respuesta;
        }

        #region ConstantesCálculo

        private struct ParConstantes
        {
            public int M { get; set; }
            public int N { get; set; }
        }

        private static ParConstantes getPar(int anio)
        {
            ParConstantes par = new ParConstantes();

            if (anio < 2100) { par.M = 24; par.N = 5; }
            else if (anio < 2200) { par.M = 24; par.N = 6; }
            else if (anio < 2299) { par.M = 25; par.N = 0; }
            else
            {
                throw
                    new ArgumentOutOfRangeException("El año deberá ser inferior a 2299");
            }
            return par;
        }

        #endregion

        #region CalcularDias

        private static DateTime calculaDomingoPascua(int anio)
        {
            ParConstantes p = getPar(anio);
            int a = anio % 19;
            int b = anio % 4;
            int c = anio % 7;
            int d = (19 * a + p.M) % 30;
            int e = (2 * b + 4 * c + 6 * d + p.N) % 7;
            DateTime pascuaResurreccion = new DateTime();

            if (d + e < 10)
            {
                pascuaResurreccion = new DateTime(anio, 3, d + e + 22);
            }
            else
            {
                pascuaResurreccion = new DateTime(anio, 4, d + e - 9);
            }

            // Excepciones
            if (pascuaResurreccion == new DateTime(anio, 4, 26))
                pascuaResurreccion = new DateTime(anio, 4, 19);

            if (pascuaResurreccion == new DateTime(anio, 4, 25) && d == 28 && e == 6 && a > 10)
                pascuaResurreccion = new DateTime(anio, 4, 18);

            return pascuaResurreccion;
        }


        #endregion

        #endregion
    }
}
