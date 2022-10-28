using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CExperienciaProfesionalL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CExperienciaProfesionalL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        //insertado en ICFormacionAcademicaService y CFormacionAcademicaService(Deivert)
        public List<CBaseDTO> BuscarExperienciaProfesionalCedula(string cedula)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                //en Experiencia profesional de datos, en intermedio será igual a CExperiencia profesional desde en el contexto
                CExperienciaProfesionalD intermedio = new CExperienciaProfesionalD(contexto);
                var datos = intermedio.BuscarExperienciaProfesionalCedula(cedula);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        //funcionario es igual a la clase de Funcionario, en datos y dentro de contenido(BD)
                        var funcionario = (Funcionario)datos.Contenido;
                        //a la respuesta se agrega a Funcionario de lógica el funcionario general se trae a funcionario.
                        respuesta.Add(CFuncionarioL.FuncionarioGeneral(funcionario));
                        //para cada item del funcionario, se va a formacion academica y se trae el primer dato de experiencia profesional.
                        foreach (var item in funcionario.FormacionAcademica.FirstOrDefault().ExperienciaProfesional)
                        {
                            //a respuesta se agrega cada item de la experiencia profesional convertida a DTO.
                            respuesta.Add(ConvertirExperienciaProfesioalADTO(item));
                        }
                        return respuesta;

                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado");
           
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }

        }
        //Insertado en ICExperienciaProfesionalService y CExperienciaProfesionalService (DEIVERT)
        public List<CBaseDTO> GuardarExperienciaProfesional(CExperienciaProfesionalDTO experiencia, CFuncionarioDTO funcionario)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CExperienciaProfesionalD intermedio = new CExperienciaProfesionalD(contexto);
                ExperienciaProfesional experienciaDato = new ExperienciaProfesional
                {
                    FecRegistro = experiencia.FechaReg,
                    IndPuntosAsignados = experiencia.IndicadorPtsAsignados,
                    NumResolucion = experiencia.NumeroResoluc,
                    ObsExperienciaProfesional = experiencia.ObservacionesExp,
                    TipExperiencia = experiencia.TipoExp
                };
                var datos = intermedio.GuardarExperienciaProfesional(funcionario.Cedula, experienciaDato);
                if (datos != null)
                {
                    if (datos.Codigo != -1)
                    {
                        CExperienciaProfesionalDTO datoInsertado = ConvertirExperienciaProfesioalADTO(((Funcionario)datos.Contenido).FormacionAcademica.FirstOrDefault().ExperienciaProfesional.Last());
                        respuesta.Add(funcionario);
                        respuesta.Add(datoInsertado);
                        return respuesta;
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)datos.Contenido).MensajeError);
                    }
                }
                else
                {
                    throw new Exception("Ocurrió un error inesperado");
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;

            }
        }
        //interna es para que se pueda mostrar a las clases de la misma capa, se convierte cada dato de experiencia profesional a DTO.
        internal static CExperienciaProfesionalDTO ConvertirExperienciaProfesioalADTO(ExperienciaProfesional experienciaProfesional)
        {
            return new CExperienciaProfesionalDTO
            {
                IdEntidad = experienciaProfesional.PK_ExperienciaProfesional,
                TipoExp = Convert.ToInt32(experienciaProfesional.TipExperiencia),
                NumeroResoluc = experienciaProfesional.NumResolucion,
                ObservacionesExp = experienciaProfesional.ObsExperienciaProfesional,
                FechaReg = Convert.ToDateTime(experienciaProfesional.FecRegistro),
                IndicadorPtsAsignados = Convert.ToInt32(experienciaProfesional.IndPuntosAsignados),
            };
        }

        #endregion
    }
}

        

