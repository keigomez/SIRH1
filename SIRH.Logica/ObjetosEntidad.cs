using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SIRH.Logica
{
    public class ObjetosEntidad
    {
        private string encabezado;

        public string Encabezado
        {
            get
            {
                return encabezado;
            }
            set
            {
                encabezado = value;
            }
        }

        private object valor;

        public object Valor
        {
            get
            {
                return valor;
            }
            set
            {
                valor = value;
            }
        }
    }
}
