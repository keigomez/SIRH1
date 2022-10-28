using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CHistoricoPlanillaL
    {
        #region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CHistoricoPlanillaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Metodos

        internal CHistoricoPlanillaDTO ConvertirPlanillaADTO(C_EMU_HistoricoPlanilla item)
        {
            string nombre = "No registra nombre";
            C_EMU_EXFUNCIONARIOS exfunc;
            Funcionario func = contexto.Funcionario.Where(Q => Q.IdCedulaFuncionario == item.cedula).FirstOrDefault();
            if (func == null)
            {
                exfunc = contexto.C_EMU_EXFUNCIONARIOS.Where(Q => Q.CEDULA == item.cedula).FirstOrDefault();
                if (exfunc != null)
                {
                    nombre = exfunc.NOMBRE.TrimEnd() + " " + exfunc.PRIMER_APELLIDO.TrimEnd() + " " + exfunc.SEGUNDO_APELLIDO.TrimEnd();
                }
            }
            else
            {
                nombre = func.NomFuncionario.TrimEnd() + " " + func.NomPrimerApellido.TrimEnd() + " " + func.NomSegundoApellido.TrimEnd();
            }

            return new CHistoricoPlanillaDTO
            {
                IdEntidad = item.ID,
                Cedula = item.cedula,
                Nombre = nombre,
                Aumentos = item.aumentos,
                GradoAcademico = item.grado_academico,
                PorGradoAcademico = item.porc_grado_academico,
                Bonificacion = item.bonificacion,
                Categoria = item.categoria,
                ClasePuesto = item.clase_puesto,
                CodigoPresupuestario = item.codigo_presupuestario,
                ConsultaExterna = item.consulta_externa,
                CursoBasico = item.curso_basico,
                DedicacionExclusiva = item.dedicacion_exclusiva,
                PagoDesarraigo = item.desarraigo,
                Disponibilidad = item.disponibilidad,
                FechaCorrida = item.fecha_corrida.Substring(0,4) + "-" + item.fecha_corrida.Substring(4, 2) + "-" + item.fecha_corrida.Substring(6, 2),
                FechaPeriodo = item.fecha_periodo.Substring(0, 4) + "-" + item.fecha_periodo.Substring(4, 2) + "-" + item.fecha_periodo.Substring(6, 2),
                Grupo = item.grupo,
                InstruccionPolicial = item.instruccion_policial,
                OtrosIncentivos = item.otros_incentivos,
                Peligrosidad = item.peligrosidad,
                PorBonificacion = item.porc_bonificacion,
                PorConsultaExterna = item.porc_consulta_externa,
                PorCursoBasico = item.porc_curso_basico,
                PorDedicacionExclusiva = item.porc_dedicacion_exclusiva,
                PorDisponibilidad = item.porc_disponibilidad,
                PorInstruccionPolicial = item.porc_instruccion_policial,
                PorPeligrosidad = item.porc_peligrosidad,
                PorProhibicion = item.porc_prohibicion,
                PorQuinquenio = item.porc_quinquenio,
                PorRiesgoPolicial = item.porc_riesgo_policial,
                Prohibicion = item.prohibicion,
                NumeroPuesto = item.numero_puesto,
                Puntos = item.cant_puntos,
                Quinquenio = item.quinquenio,
                RiesgoPolicial = item.riesgo_policial,
                SalarioBase = item.@base,
                SalarioMensual = item.sal_mensual,
                Sobresueldos = item.sobresueldos
            };
        }

        public CBaseDTO ObtenerPagoID(int idPago)
        {
            try
            {
                CHistoricoPlanillaD intermedio = new CHistoricoPlanillaD(contexto);
                var resultado = intermedio.ObtenerPagoID(idPago);

                if (resultado.Codigo > 0)
                {
                    return ConvertirPlanillaADTO((C_EMU_HistoricoPlanilla)resultado.Contenido);
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                return new CErrorDTO { MensajeError = error.Message };
            }
        }

        public List<CBaseDTO> BuscarDatosPlanilla(string cedula, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                List<C_EMU_HistoricoPlanilla> planilla = new List<C_EMU_HistoricoPlanilla>();
                CHistoricoPlanillaD intermedio = new CHistoricoPlanillaD(contexto);

                if (cedula != null && cedula != "")
                {
                    var retorno = intermedio.BuscarDatosPlanilla(planilla, cedula, "cedula");
                    if (retorno.Codigo > 0)
                    {
                        planilla = ((List<C_EMU_HistoricoPlanilla>)retorno.Contenido);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)retorno.Contenido).MensajeError);
                    }
                }

                if (fechaInicio.Year != 1 && fechaFinal.Year != 1)
                {
                    List<DateTime> fechas = new List<DateTime> { fechaInicio, fechaFinal };
                    var retorno = intermedio.BuscarDatosPlanilla(planilla, fechas, "fechas");
                    if (retorno.Codigo > 0)
                    {
                        planilla = ((List<C_EMU_HistoricoPlanilla>)retorno.Contenido);
                    }
                    else
                    {
                        throw new Exception(((CErrorDTO)retorno.Contenido).MensajeError);
                    }
                }

                planilla = planilla.OrderBy(Q => Q.fecha_periodo).ToList();

                foreach (var item in planilla)
                {
                    var temp = ConvertirPlanillaADTO(item);
                    respuesta.Add(temp);
                }

                return respuesta;
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }
        }

        #endregion
    }
}
