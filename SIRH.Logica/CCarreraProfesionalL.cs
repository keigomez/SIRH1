using SIRH.Datos;
using SIRH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRH.Logica
{
    public class CCarreraProfesionalL
    {
        #region variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CCarreraProfesionalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos


        public List<CBaseDTO> CargarCarreraProfesional()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();



            CCarreraProfesionalD intermedio = new CCarreraProfesionalD(contexto);

            var carreras = intermedio.CargarCarreraProfesional();
            
            if (carreras.Codigo > 0)
            {
                var datosCarrera = (List<C_EMU_CarreraProfesional>)carreras.Contenido;
                foreach (var carrera in datosCarrera) 
                {
                    respuesta.Add(ConvertirDatosCarreraADTO(carrera));
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontró ningún registro de carrera profesional" });
            }

            return respuesta;
        }


        public CBaseDTO CargarCarreraProfesionalPorID(int id)
        {
            CBaseDTO respuesta = new CBaseDTO();

            CCarreraProfesionalD intermedio = new CCarreraProfesionalD(contexto);

            var carrera = intermedio.CargarCarreraProfesionalPorID(id);

            if(carrera.Codigo > 0)
            {
                respuesta = ConvertirDatosCarreraADTO((C_EMU_CarreraProfesional)carrera.Contenido);
            }
            else
            {
                respuesta = new CErrorDTO { Codigo = -1, MensajeError = "No se encontró ningún registro de carrera profesional" };
            }

            return respuesta;
        }


        public List<CBaseDTO> BuscarCarreraProfesional(CCarreraProfesionalDTO carrera, List<DateTime> fechas) 
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            CCarreraProfesionalD intermedio = new CCarreraProfesionalD(contexto);

            List<C_EMU_CarreraProfesional> datosCarrera = new List<C_EMU_CarreraProfesional>();

            if (carrera.Cedula != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Cedula, "Cedula"));

                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }

            if (carrera.Nombre != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Nombre, "Nombre"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }

            //Test
            //List<DateTime> fechasTemp = new List<DateTime>();
            //fechasTemp.Add(new DateTime(2001, 4, 1));
            //fechasTemp.Add(new DateTime(2001, 4, 2));


            if (fechas != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, fechas, "Fecha"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }

            if (carrera.Puesto > 0)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Puesto, "Puesto"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }

            if (carrera.Curso1 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso1, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }
            if (carrera.Curso2 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso2, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }

            if (carrera.Curso3 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso3, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }
            if (carrera.Curso4 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso4, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }
            if (carrera.Curso5 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso5, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }
            if (carrera.Curso6 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso6, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }
            if (carrera.Curso7 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso7, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }
            if (carrera.Curso8 != null)
            {
                var resultado = ((CRespuestaDTO)intermedio.
                    BuscarCarreraProfesional(datosCarrera, carrera.Curso8, "Curso"));
                if (resultado.Codigo > 0)
                {
                    datosCarrera = (List<C_EMU_CarreraProfesional>)resultado.Contenido;
                }
            }



            if (datosCarrera.Count > 0)
            {
                foreach (var item in datosCarrera)
                {

                    var datoCarrera = ConvertirDatosCarreraADTO(item);                   
                    respuesta.Add(datoCarrera);
                    
                }
            }
            else
            {
                respuesta.Add(new CErrorDTO { Codigo = -1, MensajeError = "No se encontraron resultados para los parámetros especificados." });
            }

            return respuesta;
        }

        internal static CCarreraProfesionalDTO ConvertirDatosCarreraADTO(C_EMU_CarreraProfesional carrera)
        {
            CCarreraProfesionalDTO respuesta;
            respuesta = new CCarreraProfesionalDTO
            {
                ID = Convert.ToInt32(carrera.ID),
                Puesto = Convert.ToInt32(carrera.Puesto),
                Nombre = Convert.ToString(carrera.Nombre),
                Cedula = Convert.ToString(carrera.Cedula),
                Clase = Convert.ToInt32(carrera.Clase),
                Codigo = Convert.ToInt32(carrera.Codigo),
                Division = Convert.ToInt32(carrera.Division),
                Direccion = Convert.ToInt32(carrera.Direccion),
                Ubicacion = Convert.ToString(carrera.Ubicacion),
                TotalPuntos = Convert.ToDecimal(carrera.TotalPuntos),
                Departamento = Convert.ToInt32(carrera.Departamento),
                ValorPunto = Convert.ToDecimal(carrera.ValorPunto),
                AnnoExperienciaPR = Convert.ToInt32(carrera.AnnoExperienciaPR),
                Aprovechamiento = Convert.ToInt32(carrera.Aprovechamiento),
                Participacion = Convert.ToInt32(carrera.Participacion),
                ParticipacionInstruccion = Convert.ToInt32(carrera.ParticipacionInstruccion),
                CalificacionServicio = Convert.ToString(carrera.CalificacionServicio),
                HorasAprovechamiento = Convert.ToDecimal(carrera.HorasAprovechamiento),
                HorasParticipacion = Convert.ToDecimal(carrera.HorasParticipacion),
                HorasInstruccion = Convert.ToDecimal(carrera.HorasInstruccion),
                PtosGrado = Convert.ToDecimal(carrera.PtosGrado),
                PtosAprovechamiento = Convert.ToDecimal(carrera.PtosAprovechamiento),
                PtosExperienciaPR = Convert.ToDecimal(carrera.PtosExperienciaPR),
                PtosParticipacion = Convert.ToDecimal(carrera.PtosParticipacion),
                PtosInstruccion = Convert.ToDecimal(carrera.PtosInstruccion),
                PtosOtros = Convert.ToDecimal(carrera.PtosOtros),
                ExplicacionOtros = Convert.ToString(carrera.ExplicacionOtros),
                Observacion = Convert.ToString(carrera.Observacion),
                PSS1Feri = Convert.ToDateTime(carrera.PSS1Feri),
                PSS1Feve = Convert.ToDateTime(carrera.PSS1Feve),
                PSS2Feri = Convert.ToDateTime(carrera.PSS2Feri),
                PSS2Feve = Convert.ToDateTime(carrera.PSS2Feve),
                PSS3Feri = Convert.ToDateTime(carrera.PSS3Feri),
                PSS3Feve = Convert.ToDateTime(carrera.PSS3Feve),
                MesCarrera = Convert.ToInt32(carrera.MesCarrera),
                AnnoCarrera = Convert.ToInt32(carrera.AnnoCarrera),
                FecRigePago = Convert.ToDateTime(carrera.FecRigePago),
                HorasExcParticipacion = Convert.ToDecimal(carrera.HorasExcParticipacion),
                HorasExcAprovechamiento = Convert.ToDecimal(carrera.HorasExcAprovechamiento),
                NumResolucion = Convert.ToString(carrera.NumResolucion),
                FecResolucion = Convert.ToDateTime(carrera.FecResolucion),
                Periodo = Convert.ToString(carrera.Periodo),
                Grado = Convert.ToString(carrera.Grado),
                Nivel = Convert.ToString(carrera.Nivel),  
                AnnoExperienciaEst = Convert.ToInt32(carrera.AnnoExperienciaEst),
                AprovechamientoEst = Convert.ToInt32(carrera.AprovechamientoEst),
                ParticipacionEst = Convert.ToInt32(carrera.ParticipacionEst),
                ParticipacionInstruccionEst = Convert.ToInt32(carrera.ParticipacionInstruccionEst),
                HorasAprovechamientoEst = Convert.ToDecimal(carrera.HorasAprovechamientoEst),
                HorasParticipacionEst = Convert.ToDecimal(carrera.HorasParticipacionEst),
                HorasInstruccionEst = Convert.ToDecimal(carrera.HorasInstruccionEst),
                PtosGradoEst = Convert.ToDecimal(carrera.PtosGradoEst),
                PtosAprovechamientoEst = Convert.ToDecimal(carrera.PtosAprovechamientoEst),
                PtosExperienciaEst = Convert.ToDecimal(carrera.PtosExperienciaEst),
                PtosParticipacionEst = Convert.ToDecimal(carrera.PtosParticipacionEst),
                PtosInstruccionEst = Convert.ToDecimal(carrera.PtosInstruccionEst),
                PtosOtroEst = Convert.ToDecimal(carrera.PtosOtroEst),
                ExcParticipacionEst = Convert.ToDecimal(carrera.ExcParticipacionEst),
                ExcAprovechamientoEst = Convert.ToDecimal(carrera.ExcAprovechamientoEst),
                Marca = Convert.ToString(carrera.Marca),   
                Curso1 = Convert.ToString(carrera.Curso1),
                Curso2 = Convert.ToString(carrera.Curso2),
                Curso3 = Convert.ToString(carrera.Curso3),
                Curso4 = Convert.ToString(carrera.Curso4),
                Curso5 = Convert.ToString(carrera.Curso5),
                Curso6 = Convert.ToString(carrera.Curso6),
                Movimiento = Convert.ToString(carrera.Movimiento),
                Curso7 = Convert.ToString(carrera.Curso7),
                Curso8 = Convert.ToString(carrera.Curso8),
                Observacion1 = Convert.ToString(carrera.Observacion1),
                Observacion2 = Convert.ToString(carrera.Observacion2),
                Observacion3 = Convert.ToString(carrera.Observacion3),
                Fecha = Convert.ToDateTime(carrera.Fecha)
            };
            return respuesta;
        }



        #endregion
    }
}


//public CBaseDTO GuardarCarreraProfesional(CCarreraProfesionalDTO carrera)
//{
//    CBaseDTO respuesta = new CBaseDTO();

//    try
//    {
//        CCarreraProfesionalD intermedio = new CCarreraProfesionalD(contexto);

//        C_EMU_CarreraProfesional datosCarrera = convertirDTOACarrera(carrera);

//        respuesta = intermedio.GuardarCarreraProfesional(datosCarrera);

//        if (((CRespuestaDTO)respuesta).Contenido.GetType() == typeof(CErrorDTO))
//        {
//            respuesta = (CErrorDTO)((CRespuestaDTO)respuesta).Contenido;
//            throw new Exception();
//        }
//        else
//        {
//            return respuesta;
//        }
//    }
//    catch
//    {
//        return respuesta;
//    }
//}


//internal static C_EMU_CarreraProfesional convertirDTOACarrera(CCarreraProfesionalDTO carrera)
//{
//    C_EMU_CarreraProfesional datosCarrera = new C_EMU_CarreraProfesional
//    {
//        ID = Convert.ToInt32(carrera.ID),
//        Puesto = Convert.ToInt32(carrera.Puesto),
//        Nombre = Convert.ToString(carrera.Nombre),
//        Cedula = Convert.ToString(carrera.Cedula),
//        Clase = Convert.ToInt32(carrera.Clase),
//        Codigo = Convert.ToInt32(carrera.Codigo),
//        Division = Convert.ToInt32(carrera.Division),
//        Direccion = Convert.ToInt32(carrera.Direccion),
//        Ubicacion = Convert.ToString(carrera.Ubicacion),
//        TotalPuntos = Convert.ToDecimal(carrera.TotalPuntos),
//        Departamento = Convert.ToInt32(carrera.Departamento),
//        ValorPunto = Convert.ToDecimal(carrera.ValorPunto),
//        AnnoExperienciaPR = Convert.ToInt32(carrera.AnnoExperienciaPR),
//        Aprovechamiento = Convert.ToInt32(carrera.Aprovechamiento),
//        Participacion = Convert.ToInt32(carrera.Participacion),
//        ParticipacionInstruccion = Convert.ToInt32(carrera.ParticipacionInstruccion),
//        CalificacionServicio = Convert.ToString(carrera.CalificacionServicio),
//        HorasAprovechamiento = Convert.ToDecimal(carrera.HorasAprovechamiento),
//        HorasParticipacion = Convert.ToDecimal(carrera.HorasParticipacion),
//        HorasInstruccion = Convert.ToDecimal(carrera.HorasInstruccion),
//        PtosGrado = Convert.ToDecimal(carrera.PtosGrado),
//        PtosAprovechamiento = Convert.ToDecimal(carrera.PtosAprovechamiento),
//        PtosExperienciaPR = Convert.ToDecimal(carrera.PtosExperienciaPR),
//        PtosParticipacion = Convert.ToDecimal(carrera.PtosParticipacion),
//        PtosInstruccion = Convert.ToDecimal(carrera.PtosInstruccion),
//        PtosOtros = Convert.ToDecimal(carrera.PtosOtros),
//        ExplicacionOtros = Convert.ToString(carrera.ExplicacionOtros),
//        Observacion = Convert.ToString(carrera.Observacion),
//        //PSS1Feri = carrera.PSS1Feri,
//        //PSS1Feve = carrera.PSS1Feve,
//        //PSS2Feri = carrera.PSS2Feri,
//        //PSS2Feve = carrera.PSS2Feve,
//        //PSS3Feri = carrera.PSS3Feri,
//        //PSS3Feve = carrera.PSS3Feve,
//        //MesCarrera = carrera.MesCarrera,
//        //AnnoCarrera = carrera.AnnoCarrera,
//        //FecRigePago = carrera.FecRigePago,
//        //HorasExcParticipacion = carrera.HorasExcParticipacion,
//        //HorasExcAprovechamiento = carrera.HorasExcAprovechamiento,
//        //NumResolucion = carrera.NumResolucion,
//        //FecResolucion = carrera.FecResolucion,
//        //Periodo = carrera.Periodo,
//        //Grado = carrera.Grado,
//        //Nivel = carrera.Nivel.ToString(),
//        //AnnoExperienciaEst = carrera.AnnoExperienciaEst,
//        //AprovechamientoEst = carrera.AprovechamientoEst,
//        //ParticipacionEst = carrera.ParticipacionEst,
//        //ParticipacionInstruccionEst = carrera.ParticipacionInstruccionEst,
//        //HorasAprovechamientoEst = carrera.HorasAprovechamientoEst,
//        //HorasParticipacionEst = carrera.HorasParticipacionEst,
//        //HorasInstruccionEst = carrera.HorasInstruccionEst,
//        //PtosGradoEst = carrera.PtosGradoEst,
//        //PtosAprovechamientoEst = carrera.PtosAprovechamientoEst,
//        //PtosExperienciaEst = carrera.PtosExperienciaEst,
//        //PtosParticipacionEst = carrera.PtosParticipacionEst,
//        //PtosInstruccionEst = carrera.PtosInstruccionEst,
//        //PtosOtroEst = carrera.PtosOtroEst,
//        //ExcParticipacionEst = carrera.ExcParticipacionEst,
//        //ExcAprovechamientoEst = carrera.ExcAprovechamientoEst,
//        //Marca = carrera.Marca.ToString(),
//        //Curso1 = carrera.Curso1,
//        //Curso2 = carrera.Curso2,
//        //Curso3 = carrera.Curso3,
//        //Curso4 = carrera.Curso4,
//        //Curso5 = carrera.Curso5,
//        //Curso6 = carrera.Curso6,
//        //Movimiento = carrera.Movimiento,
//        //Curso7 = carrera.Curso7,
//        //Curso8 = carrera.Curso8,
//        //Observacion1 = carrera.Observacion1,
//        //Observacion2 = carrera.Observacion2,
//        //Observacion3 = carrera.Observacion3,
//        //Fecha = carrera.Fecha


//    };
//    return datosCarrera;
//}