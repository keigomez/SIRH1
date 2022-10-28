using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRH.DTO;
using SIRH.Web.ServicioTSE;

namespace SIRH.Web.Helpers
{
    public static class FuncionarioHelper
    {
        public static CFuncionarioDTO ConvertirPersonaTSEAFuncionario(Persona persona)
        {
            return new CFuncionarioDTO
            {
                Cedula = CedulaTSEAMOPT(persona.Identificacion),
                Nombre = persona.Nombre,
                PrimerApellido = persona.Apellido1,
                SegundoApellido = persona.Apellido2,
                FechaNacimiento = Convert.ToDateTime(persona.Fecha_Nacimiento),
                Sexo = persona.Genero == "FEMENINO" ? GeneroEnum.Femenino : GeneroEnum.Masculino
            };
        }

        internal static string CedulaEmulacionATSE(string cedula)
        {
            string cedulaTSE = "";
            if (cedula.Substring(0, 2).Equals("00"))
            {
                cedulaTSE = cedula.Substring(2, 1);
                cedulaTSE += "0" + cedula.Substring(3, 3);
                cedulaTSE += cedula.Substring(6, 4);
            }
            else
            {
                cedulaTSE = cedula.Substring(1, 1);
                cedulaTSE += cedula.Substring(2, 4);
                cedulaTSE += cedula.Substring(6, 4);
            }
            return cedulaTSE;
        }

        internal static string CedulaTSEAMOPT(string cedula)
        {
            if (cedula.Substring(1, 1).Equals("0"))
            {
                return "00" + cedula.Substring(0, 1) + cedula.Substring(2, cedula.Length - 2);
            }
            else
            {
                return "0" + cedula;
            }
        }

        internal static int CalcularEdadFuncionario(DateTime fechaNacimiento)
        {
            int edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (DateTime.Now.Month == fechaNacimiento.Month)
            {
                if (DateTime.Now.Day > fechaNacimiento.Day)
                {
                    edad--;
                }
            }
            else
            {
                if (DateTime.Now.Month < fechaNacimiento.Month)
                {
                    edad--;
                }
            }

            return edad;
        }
    }
}
