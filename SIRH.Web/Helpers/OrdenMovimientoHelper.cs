using SIRH.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRH.Web.Helpers
{
    public class OrdenMovimientoHelper
    {
        public class JefeOrden
        {
            public int Id { get; set; }
            public string Cedula { get; set; }
            public string Nombre { get; set; }
            public string Correo { get; set; }
        }
        public static string[] datosOrden()
        {
            return new string[]
            {
               
               //[0] Observaciones
               "Cuenta con disponibilidad de uso, de conformidad con el Alcance No. 34 a la Gaceta No. 30, de fecha 12 de febrero del 2021, Segunda Modificación Legislativa de La Ley No. 9926 “Ley de Presupuesto Ordinario y Extraordinario de la República para el ejercicio económico 2021, de 01 de diciembre 2020”.",
               //[1] Partida
                "Cargos Fijos",
                //[2] Academica
               "CUMPLE CON EL REQUISITO ACADÉMICO DE LA CLASE Y ESPECIALIDAD CORRESPONDIENTE DE ACUERDO CON EL MANUAL DE CLASES DE LA DIRECCIÓN GENERAL DEL SERVICIO CIVIL.",
                //[3] Experiencia
               "CUMPLE CON EL REQUISITO DE EXPERIENCIA PARA LA CLASE Y ESPECIALIDAD CORRESPONDIENTE DE ACUERDO CON EL MANUAL DE CLASES DE LA DIRECCIÓN GENERAL DEL SERVICIO CIVIL.",
                //[4] Capacitacion
                "CUMPLE CON EL REQUISITO DE CAPACITACIÓN REQUERIDA PARA LA CLASE Y ESPECIALIDAD CORRESPONDIENTE DE ACUERDO CON EL MANUAL DE CLASES DE LA DIRECCIÓN GENERAL DEL SERVICIO CIVIL.",
                //[5] Licencias
                "CUMPLE CON EL REQUISITO DE LICENCIA O PERMISOS ESPECIALES REQUERIDOS PARA LA CLASE Y ESPECIALIDAD CORRESPONDIENTE DE ACUERDO CON EL MANUAL DE CLASES DE LA DIRECCIÓN GENERAL DEL SERVICIO CIVIL.",
                //[6] Colegiaturas
                "CUMPLE CON EL REQUISITO DE COLEGIATURA REQUERIDO PARA LA CLASE Y ESPECIALIDAD CORRESPONDIENTE DE ACUERDO CON EL MANUAL DE CLASES DE LA DIRECCIÓN GENERAL DEL SERVICIO CIVIL.",
             };
        }

        public static List<JefeOrden> ListaJefes()
        {
            List<JefeOrden> lista = new List<JefeOrden>();
            
            // REVISADO POR
            lista.Add(new JefeOrden
            {
                Id = 0,
                Cedula =  "0111350808",
                Nombre = "WENDY PÉREZ ROJAS",
                Correo = "orlando.sandoval@mopt.go.cr" //"wendy.perez@mopt.go.cr"
            });

            // APROBADO POR
            lista.Add(new JefeOrden
            {
                Id = 1,
                Cedula = "0019430125",
                Nombre = "OLGA ORTEGA ROMERO",
                Correo = "orlando.sandoval@mopt.go.cr" //"olga.ortega@mopt.go.cr"
            });


            // REALIZAR NOMBRAMIENTO
            lista.Add(new JefeOrden
            {
                Id = 2,
                Cedula = "",
                Nombre = "",
                Correo = "orlando.sandoval@mopt.go.cr"
            });
            return lista;
        }
    }
}
