using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRH.DTO;
using SIRH.Logica;

namespace SIRH.Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CPrestacionLegalService" in both code and config file together.
    public class CPrestacionLegalService : ICPrestacionLegalService
    {

        public CBaseDTO AgregarPrestacion(CNombramientoDTO nombramiento, CTipoPrestacionDTO tipo, CPrestacionLegalDTO prestacion)
        {
            CPrestacionLegalL respuesta = new CPrestacionLegalL();
            return respuesta.AgregarPrestacionLegal(nombramiento, tipo, prestacion);
        }

        //public CBaseDTO AgregarPrestacionReal(CRegistroPrestacionRealDTO prestacion)
        //{
        //    CRegistroPrestacionRealL respuesta = new CRegistroPrestacionRealL();
        //    return respuesta.AgregarPagoPrestacionReal(prestacion);
        //}

        public List<CBaseDTO> CalcularPrestacion(string cedula)
        {
            CPrestacionLegalL respuesta = new CPrestacionLegalL();
            return respuesta.CalcularPrestacion(cedula);
        }

        public CBaseDTO ObtenerPrestacion(CPrestacionLegalDTO prestacion)
        {
            CPrestacionLegalL respuesta = new CPrestacionLegalL();
            return respuesta.ObtenerPrestacionLegal(prestacion);
        }


        public List<CBaseDTO> ObtenerTipoPrestacion()
        {
            CTipoPrestacionL respuesta = new CTipoPrestacionL();
            return respuesta.RetornarTiposPrestaciones();
        }

        
        //public List<CBaseDTO> ObtenerDesgloseSalarial(string numCedula, int quincenas)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerSalarioDesgloseSalarial(numCedula,quincenas);
        //}

        //public List<CBaseDTO> ObtenerPromedioSalarioBruto(string numCedula, int quincenas)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerPromedioSalarioBruto(numCedula,quincenas);
        //}


        //public CBaseDTO ObtenerMontoCesantiaSimple(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerMontoCesantiaSimple(numCedula);
        //}

     
        //public List<CBaseDTO> ObtenerMesesLaborados(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerMesesLaborados(numCedula);
        //}
        
        //public List<CBaseDTO> ObtenerDiasVacacionesDerecho(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerDiasVacacionesDerecho(numCedula);
        //}
        
        //public List<CBaseDTO> ObtenerMesesPosterioresCumplimientoVacaciones(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerMesesPosterioresCumplimientoVacaciones(numCedula);
        //}

        //public List<CBaseDTO> ObtenerRegistroExtras(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerRegistroExtras(numCedula);
        //}
        
        //public CBaseDTO ObtenerMontoSalarioEscolar(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerMontoSalarioEscolar(numCedula);
        //}

        //public CBaseDTO ObtenerMontoPromedioQuincenal(string numCedula)
        //{
        //    CPagoPrestacionL respuesta = new CPagoPrestacionL();
        //    return respuesta.ObtenerMontoPromedioQuincenal(numCedula);
        //}
    }
}