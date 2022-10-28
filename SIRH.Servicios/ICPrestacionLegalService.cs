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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICPrestacionLegalService" in both code and config file together.
    [ServiceContract]
    public interface ICPrestacionLegalService
    {

        [OperationContract]
        CBaseDTO AgregarPrestacion(CNombramientoDTO nombramiento, CTipoPrestacionDTO tipo, CPrestacionLegalDTO prestacion);

        [OperationContract]
        List<CBaseDTO> CalcularPrestacion(string cedula);

        [OperationContract]
        CBaseDTO ObtenerPrestacion(CPrestacionLegalDTO prestacion);


        [OperationContract]
        List<CBaseDTO> ObtenerTipoPrestacion();



        /////////////////////////////////////////
        /*
        [OperationContract]
        CBaseDTO AgregarPrestacionReal (CRegistroPrestacionRealDTO prestacion);

        

        [OperationContract]
        List<CBaseDTO> ObtenerDesgloseSalarial(string numCedula, int quincenas);

        [OperationContract]
        List<CBaseDTO> ObtenerPromedioSalarioBruto(string numCedula, int quincenas);
        

        [OperationContract]
       CBaseDTO ObtenerMontoCesantiaSimple(string numCedula);
        

        [OperationContract]
        List<CBaseDTO> ObtenerMesesLaborados(string numCedula);


        [OperationContract]
        List<CBaseDTO> ObtenerDiasVacacionesDerecho(string numCedula);
       

        [OperationContract]
        List<CBaseDTO> ObtenerMesesPosterioresCumplimientoVacaciones(string numCedula);

        [OperationContract]
        List<CBaseDTO> ObtenerRegistroExtras(string numCedula);

        [OperationContract]
         CBaseDTO ObtenerMontoSalarioEscolar(string numCedula);


        [OperationContract]
        CBaseDTO ObtenerMontoPromedioQuincenal(string numCedula);
        */
        
    }
}