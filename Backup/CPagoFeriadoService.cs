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
    // NOTE: If you change the class name "CPagoFeriadoService" here, you must also update the reference to "CPagoFeriadoService" in App.config.
    public class CPagoFeriadoService : ICPagoFeriadoService
    {
        public CBaseDTO AgregarPagoExtraordinario(CFuncionarioDTO funcionario, CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CPagoExtraordinarioL respuesta = new CPagoExtraordinarioL();

            return respuesta.AgregarPagoExtraordinario(funcionario, pagoExtraordinario);
        }

        public List<CBaseDTO> ObtenerPagoExtraordinario(int codigo)
        {
            CPagoExtraordinarioL respuesta = new CPagoExtraordinarioL();

            return respuesta.ObtenerPagoExtraordinario(codigo);
        }


        public CBaseDTO AgregarPagoFeriado(CPagoExtraordinarioDTO pagoExtraordinario, CPagoFeriadoDTO pagoFeriado,
                                      CEstadoTramiteDTO estadoTramite, CFuncionarioDTO funcionario)
        {
            CPagoFeriadoL respuesta = new CPagoFeriadoL();

            return respuesta.AgregarPagoFeriado(pagoExtraordinario, pagoFeriado, estadoTramite, funcionario);
        }

        public List<CBaseDTO> ObtenerPagoFeriado(int codigo)
        {

            CPagoFeriadoL respuesta = new CPagoFeriadoL();

            return respuesta.ObtenerPagoFeriado(codigo);
        }

        public List<List<CBaseDTO>> BuscarPagosFeriado(CFuncionarioDTO funcionario, CPagoFeriadoDTO tramite,
                                                       List<DateTime> fechasTramite, CEstadoTramiteDTO estadoTramite,
                                                       List<string> diasFeriados)
        {

            CPagoFeriadoL respuesta = new CPagoFeriadoL();

            return respuesta.BuscarPagosFeriado(funcionario, tramite, fechasTramite, estadoTramite, diasFeriados);
        }

        public CBaseDTO AnularPagoFeriado(CPagoFeriadoDTO tramite)
        {
            CPagoFeriadoL respuesta = new CPagoFeriadoL();

            return respuesta.AnularPagoFeriado(tramite);
        }


        public CBaseDTO AgregarDeduccion(CFuncionarioDTO funcionario, List<CDeduccionEfectuadaDTO> deducciones, List<CCatalogoDeduccionDTO> catalogoDeduccion,
                                           CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario)
        {

            CDeduccionEfectuadaL respuesta = new CDeduccionEfectuadaL();

            return respuesta.AgregarDeduccion(funcionario, deducciones, catalogoDeduccion, pagoFeriado, pagoExtraordinario);
        }

        public List<CBaseDTO> ObtenerDeduccionEfectuada(int codigo)
        {
            CDeduccionEfectuadaL respuesta = new CDeduccionEfectuadaL();

            return respuesta.ObtenerDeduccionEfectuada(codigo);
        }

        public List<List<CBaseDTO>> RetornarDeduccionesPorPagoFeriado(int codigo)
        {
            CDeduccionEfectuadaL respuesta = new CDeduccionEfectuadaL();

            return respuesta.RetornarDeduccionesPorPagoFeriado(codigo);
        }

        public List<CBaseDTO> ObtenerCatalogoDeduccion(int codigo)
        {

            CCatalogoDeduccionL respuesta = new CCatalogoDeduccionL();

            return respuesta.ObtenerCatalogoDeduccion(codigo);
        }

        public List<List<CBaseDTO>> ListarDeduccionesTipo(int tipo)
        {

            CCatalogoDeduccionL respuesta = new CCatalogoDeduccionL();

            return respuesta.ListarDeduccionesTipo(tipo);
        }


        public CBaseDTO AgregarDiaPagado(CFuncionarioDTO funcionario, List<CDiaPagadoDTO> dias, List<CCatalogoDiaDTO> catalogodias,
                                          CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CDiaPagadoL respuesta = new CDiaPagadoL();

            return respuesta.AgregarDiaPagado(funcionario, dias, catalogodias, pagoFeriado, pagoExtraordinario);
        }

        public List<CBaseDTO> ObtenerDiaPagado(int codigo)
        {

            CDiaPagadoL respuesta = new CDiaPagadoL();

            return respuesta.ObtenerDiaPagado(codigo);
        }

        public List<List<CBaseDTO>> RetornarDiasPorTramitePagado(int codigo)
        {

            CDiaPagadoL respuesta = new CDiaPagadoL();

            return respuesta.RetornarDiasPorTramitePagado(codigo);
        }

        public List<CBaseDTO> RetornarDiaPorTramitePagado(int codigo)
        {

            CDiaPagadoL respuesta = new CDiaPagadoL();

            return respuesta.RetornarDiaPorTramitePagado(codigo);
        }

        public CBaseDTO AgregarAsueto(CCatalogoDiaDTO asueto, CTipoDiaDTO tipoDia)
        {
            CCatalogoDiaL respuesta = new CCatalogoDiaL();

            return respuesta.AgregarAsueto(asueto, tipoDia);

        }

        public List<CBaseDTO> ObtenerCatalogoDia(int codigo)
        {
            CCatalogoDiaL respuesta = new CCatalogoDiaL();

            return respuesta.ObtenerCatalogoDia(codigo);
        }

        public List<List<CBaseDTO>> ListarDiasPorTipo(int tipo)
        {
            CCatalogoDiaL respuesta = new CCatalogoDiaL();

            return respuesta.ListarDiasPorTipo(tipo);
        }

        public CBaseDTO AgregarUbicacionAsueto(CCantonDTO canton, CCatalogoDiaDTO asueto, CUbicacionAsuetoDTO ubicacionAsueto)
        {
            CUbicacionAsuetoL respuesta = new CUbicacionAsuetoL();
            return respuesta.AgregarUbicacionAsueto(canton, asueto, ubicacionAsueto);
        }

        public List<List<CBaseDTO>> ListarAsuetosPorUbicacion(string provincia, string canton)
        {
            CUbicacionAsuetoL respuesta = new CUbicacionAsuetoL();
            return respuesta.ListarAsuetosPorUbicacion(provincia,canton);
        }
        public List<List<CBaseDTO>> ListarCantones()
        {
            CUbicacionAsuetoL respuesta = new CUbicacionAsuetoL();
            return respuesta.ListarCantones();
        }
        public List<CBaseDTO> ObtenerCanton(int codigo)
        {
            CUbicacionAsuetoL respuesta = new CUbicacionAsuetoL();
            return respuesta.ObtenerCanton(codigo);
        }

        public CBaseDTO BuscarDesgloceSalarialPF(string cedula)
        {
            CFuncionarioL respuesta = new CFuncionarioL();
            return respuesta.BuscarDesgloceSalarialPF(cedula);
        }

        public List<CBaseDTO> BuscarFuncionarioDetallePuesto(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarFuncionarioDetallePuesto(cedula);
        }
        public List<CBaseDTO> BuscarPuestoPF(string cedula)
        {
            CFuncionarioL logica = new CFuncionarioL();
            return logica.BuscarPuestoPF(cedula);
        }

        public CBaseDTO EliminarPagoExtraordinario(CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CPagoExtraordinarioL logica = new CPagoExtraordinarioL();
            return logica.EliminarPagoExtraordinario(pagoExtraordinario);
        }

        public CBaseDTO EliminarPagoFeriado(CPagoFeriadoDTO pagoFeriado)
        {
            CPagoFeriadoL logica = new CPagoFeriadoL();
            return logica.EliminarPagoFeriado(pagoFeriado);
        }

        public CBaseDTO EliminarDeduccionEfectuada(CPagoFeriadoDTO pagoFeriado)
        {
            CDeduccionEfectuadaL logica = new CDeduccionEfectuadaL();
            return logica.EliminarDeduccionEfectuada(pagoFeriado);
        }

        public CBaseDTO EliminarDiaPagado(CPagoFeriadoDTO pagoFeriado)
        {
            CDiaPagadoL logica = new CDiaPagadoL();
            return logica.EliminarDiaPagado(pagoFeriado);
        }

        public List<CBaseDTO> ObtenerSalarioEscolar()
        {
            CRemuneracionEfectuadaPFL logica = new CRemuneracionEfectuadaPFL();
            return logica.ObtenerSalarioEscolar();
        }

        public CBaseDTO AgregarBeneficio(CFuncionarioDTO funcionario, CRemuneracionEfectuadaPFDTO beneficio,
                                       CPagoFeriadoDTO pagoFeriado, CPagoExtraordinarioDTO pagoExtraordinario)
        {
            CRemuneracionEfectuadaPFL logica = new CRemuneracionEfectuadaPFL();
            return logica.AgregarBeneficio(funcionario, beneficio, pagoFeriado, pagoExtraordinario);
        }

        public CBaseDTO ActualizarSalarioEscolar(CCatalogoRemuneracionDTO remuneracionEfectuada)
        {
            CRemuneracionEfectuadaPFL logica = new CRemuneracionEfectuadaPFL();
            return logica.ActualizarSalarioEscolar(remuneracionEfectuada);
        }

        public List<List<CBaseDTO>> ListarDeduccionesPagoTipo(int tipo, int codigo)
        {
            CCatalogoDeduccionL logica = new CCatalogoDeduccionL();
            return logica.ListarDeduccionesPagoTipo(tipo, codigo);
        }

        public CBaseDTO ObtenerSalarioEscolarEfectuado(int codigoTramite)
        {
            CRemuneracionEfectuadaPFL logica = new CRemuneracionEfectuadaPFL();

            return logica.ObtenerSalarioEscolarEfectuado(codigoTramite);
        }

       
    }
}
