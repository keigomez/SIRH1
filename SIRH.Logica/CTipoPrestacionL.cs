using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CTipoPrestacionL
    {


        #region Variables

        SIRHEntities entidadBase = new SIRHEntities();

        private string desTipoPrestacion;
        public string DescripcionTipoPrestacion
        {
            get
            {
                return desTipoPrestacion;
            }
            set
            {
                desTipoPrestacion = value;
            }
        }

        #endregion

        #region Metodos




        internal static CTipoPrestacionDTO ConvertirDatosTipoPrestacionADto(TipoPrestacion item)
        {
            return new CTipoPrestacionDTO
            {
                IdEntidad = item.PK_TipoPrestacion,

                DesPrestacion = item.DesPrestacion
            };
        }


        public List<CBaseDTO> RetornarTiposPrestaciones()
        {
            List<CBaseDTO> resultado = new List<CBaseDTO>();

            try
            {
                CTipoPrestacionD intermedio = new CTipoPrestacionD(entidadBase);

                var tipoPres = intermedio.RetornarTipoPrestaciones();

                if (tipoPres.Codigo > 0)
                {
                    foreach (var tipo in (List<TipoPrestacion>)tipoPres.Contenido)
                    {
                        var datoTipoInc = ConvertirDatosTipoPrestacionADto((TipoPrestacion)tipo);
                        resultado.Add(datoTipoInc);
                    }
                }
                else
                {
                    resultado.Add((CErrorDTO)tipoPres.Contenido);
                }
            }
            catch (Exception error)
            {
                resultado.Add(new CErrorDTO
                {
                    Codigo = -1,
                    MensajeError = error.Message
                });
            }
            return resultado;
        }



        #endregion

    }
}