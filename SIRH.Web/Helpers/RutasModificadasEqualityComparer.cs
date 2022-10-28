using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;

namespace SIRH.Web.Helpers
{
    public class RutasModificadasEqualityComparer : IEqualityComparer<CDetalleAsignacionGastoTransporteModificadaDTO>
    {
        public bool Equals(CDetalleAsignacionGastoTransporteModificadaDTO x, CDetalleAsignacionGastoTransporteModificadaDTO y)
        {

            if (ReferenceEquals(x, y)) return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.NomRutaDTO == y.NomRutaDTO && x.NomFraccionamientoDTO == y.NomFraccionamientoDTO && x.MontTarifa == y.MontTarifa;

        }



        public int GetHashCode(CDetalleAsignacionGastoTransporteModificadaDTO rutaModificada)

        {

            if (ReferenceEquals(rutaModificada, null)) return 0;

            int hashRuta = rutaModificada.NomRutaDTO == null ? 0 : rutaModificada.NomRutaDTO.GetHashCode();
            int hashMonto = rutaModificada.MontTarifa == null ? 0 : rutaModificada.MontTarifa.GetHashCode();
            int hashFraccion = rutaModificada.NomFraccionamientoDTO.GetHashCode();



            return hashRuta ^ hashFraccion ^ hashMonto;

        }
    }
}