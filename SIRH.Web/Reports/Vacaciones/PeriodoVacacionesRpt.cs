using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Reports.Vacaciones
{
    public class PeriodoVacacionesRpt
    {
        public string periodo { get; set; }

        public string dias { get; set; }

        public string estado { get; set; }

        public string fechaCarga { get; set; }

        public string saldo { get; set; }

        public string proporcion { get; set; }

        public string proporcionMes { get; set; }

        internal static PeriodoVacacionesRpt GenerarDatosReporte(CPeriodoVacacionesDTO periodo)
        {
            var estadoPeriodo= "Vencido";
            if (periodo.Estado == 1)
            {
                estadoPeriodo = "Activo";
            }

            return new PeriodoVacacionesRpt
            {
                  periodo=periodo.Periodo,
                  dias=periodo.DiasDerecho.ToString(),
                  estado = estadoPeriodo,
                  fechaCarga=periodo.FechaCarga.ToShortDateString(),
                  saldo = periodo.Saldo.ToString(),
                  proporcion = periodo.Proporcion.ToString(),
                  proporcionMes = periodo.ProporcionMes.ToString()
            };
        }
    }
}
