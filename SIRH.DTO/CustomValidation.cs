using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SIRH.DTO
{
    class CustomValidationHora : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool respuesta = false;
            if (value != null)
            {
                string valor = Convert.ToString(value);
                if (Convert.ToInt32(valor) >= 0 && Convert.ToInt32(valor) <= 23)
                {
                    respuesta = true;
                }
            }
            else 
            {
                respuesta = true;
            }
            return respuesta;
        }
    }

    class CustomValidationMinuto : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool respuesta = false;
            if (value != null)
            {
                string valor = Convert.ToString(value);
                if (Convert.ToInt32(valor) >= 0 && Convert.ToInt32(valor) <= 59)
                {
                    respuesta = true;
                }
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }
    }

    class CustomValidationLength : ValidationAttribute
    {
        public int Size { get; set; }

        public CustomValidationLength(int size)
        {
            this.Size = size;
        }

        public override bool IsValid(object value)
        {
            bool respuesta = false;
            if (value != null)
            {
                string valor = Convert.ToString(value);
                if (valor.Length < Size)
                {
                    respuesta = true;
                }
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}
