using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Helpers
{
    public enum EAccionesPuestoHelper
    {
        GUARDAR_PEDIMENTO = 1,
        GUARDAR_UBICACION = 2
    }

    public enum EModulosHelper
    {
        Administrador = 1,
        General = 2,
        Usuarios = 3,
        Cauciones = 4,
        Desarraigo = 5,
        FeriadosPT = 6,
        Marcas = 7,
        BorradorAcciones = 8,
        Carrera = 9,
        Vacantes = 10,
        Calificacion = 11,
        AccionPersonal = 12,
        Incapacidades = 13,
        Planilla = 14,
        ViaticoCorrido = 15,
        TiempoExtra = 16,
        Vacaciones = 17,
        Archivo = 18,
        Incidencias = 19,
        Teletrabajo = 20,
        Prestaciones = 21,
        Planificacion = 22,
        RelacionesLaborales = 23,
        Consultorio = 24,
        SeleccionPersonal = 25,
        ComponentePresupuestario = 26
    }

    public enum ENivelesVacantes
    {
        Administrador = 30,
        AdministradorVacantesGeneral = 31,
        AdministradorVacantesPolicial = 32,
        AdministradorNominaGeneral = 33,
        AdministradorNominaPolicial = 34,
        AdministradorEstudio = 35,
        AdministradorPrestamo = 36,
        Consulta = 37
    }

    public enum ENivelesCarrera
    {
        Administrador = 25,
        Policial = 26,
        Profesional = 27,
        Experiencia = 28,
        Consulta = 29
    }

    public enum ENivelesCaucion
    {
        Administrador = 9,
        Operativo = 10,
        Consulta = 11
    }

    public enum ENivelesBorradorAccionPersonal
    {
        Administrador = 38,
        Oficio = 22,
        Tecnico = 23,
        Analista = 24,
        Registro = 39,
    }

    public enum ENivelesAccionPersonal
    {
        Administrador = 40,
        Operativo = 41,
        Consulta = 42
    }
    
    public enum ENivelesIncapacidades
    {
        Administrador = 43,
        Operativo = 44,
        Consulta = 45
    }

    public enum ENivelesPlanillas
    {
        Administrador = 46,
        Operativo = 47,
        Consulta = 48
    }

    public enum ENivelesViaticoCorrido
    {
        Administrador = 49,
        Operativo = 50,
        Consulta = 51,
        Aprobador = 68,
        GeneradorPago = 69,
        AprobadorMovimiento = 70,
        RegistrarMovimiento = 71
    }


    public enum ENivelesTiempoExtra
    {
        Administrador = 52,
        Operativo = 53,
        Consulta = 54
    }

    public enum ENivelesVacaciones
    {
        Administrador = 55,
        Operativo = 56,
        Consulta = 57
    }

    public enum ENivelesArchivo
    {
        Administrador = 58,
        Operativo = 59,
        Consulta = 60
    }

    public enum ENivelesCalificacion
    {
        Administrador = 61,
        Operativo = 62,
        Consulta = 63,
        FueraDominio = 64
    }

    public enum ENivelesTeletrabajo
    {
        Administrador = 65,
        Operativo = 66,
        Consulta = 67
    }
    public enum ENivelesPrestacion
    {
        Administrador = 74,
        Operativo = 75,
        Consulta = 76
    }

    public enum ENivelesPlanificacion
    {
        Administrador = 77,
        Operativo = 78,
        Consulta = 79
    }
    public enum EAccionesBitacora
    {
        Login = 0,
        Guardar = 1,
        Editar = 2,
        Notificar = 3,
        GenerarReporte = 4,
        Anular = 5,
        GeneracionResolucion = 6
    }

    public enum EErrorAcceso
    {
        Acceso = 1,
        Sistema = 2
    }

    public enum ENivelOcupacional
    {
        Superior = 1,
        Ejecutivo = 2,
        Profesional = 3,
        Tecnico = 4, 
        Administrativo = 5,
        Servicios = 6, 
        Policial = 7
    }


    public static class NivelOcupacionalHelper
    {
        public static string ObtenerNombre(int num)
        {
            // 4 = Nivel Técnico(revisar que los puestos en la clase inicien con palabras como tecnico en... tecnico del servicio civil)
            //5 = Nivel Administrativo(oficnitas, secretaria, recepcionista)
            switch (num)
            {
                case 1: return "Nivel Superior";
                case 2: return "Nivel Ejecutivo";
                case 3: return "Nivel Profesional";
                case 4: return "Nivel Técnico";
                case 5: return "Nivel Administrativo";
                case 6: return "Servicios";
                case 7: return "Puestos Policiales";
                default: return "";
            }
        }

        public static int ObtenerId(string nombre)
        {
            switch (nombre)
            {
                case "Nivel Superior": return 1;
                case "Nivel Ejecutivo": return 2;
                case "Nivel Profesional": return 3;
                case "Nivel Técnico": return 4;
                case "Nivel Administrativo": return 5;
                case "Servicios": return 6;
                case "Puestos Policiales": return 7;
                default: return 0;
            }
        }
    }
}