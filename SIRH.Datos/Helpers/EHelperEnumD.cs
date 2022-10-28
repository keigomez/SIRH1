using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIRH.Datos.Helpers
{
    public enum EstadosPuesto
    {
        NombAscAutoridadPresupuestaria = 1,
        Propiedad = 2,
        InterinoAutoridadPresupuestaria = 3,
        Congelada = 4,
        SuspIndef = 5,
        VacantePSS = 6,
        VacAscensoInterino = 7,
        VacDescensoInterino = 8,
        AscensoInterino = 9,
        VacAprobAutoridadPresupuestaria = 10,
        Descongelar = 11,
        AgruparRebajar = 12,
        EnvAprobAutoridadPresupuestaria = 13,
        VacDenegada = 14,
        AprobAscensoAutoridadPresupuestaria = 15,
        VacEnviadaAutoridadPresupuestaria = 16,
        NombTramite = 17,
        EliminadaMovLaboral = 18,
        VacDE19887H = 19,
        VacProyModRelPuesto = 20,
        PuestoPropCompart = 21,
        VacanteReestructuracion = 22,
        PuestoEliminado = 23,
        Vacante = 24,
        EliminadoTrasladoCONAVI = 25,
        EliminadoTrasladoInstGeog = 26,
        EliminadoTrasladoAsigFam = 27
    }

    public enum EstadosFuncionario
    {
        Activo = 1,
        PSS = 2,
        SuspInd = 3,
        TrasInt = 4,
        AgotoSub = 5,
        PCS = 6,
        DespCausa = 7,
        DespResp = 8,
        PensEstado = 9,
        PensCCSS = 10,
        CesMuerte = 11,
        Renuncia = 12,
        CesInter = 13,
        TrasInst = 14,
        ExcSistem = 15,
        GestDesp = 16,
        MovLaboral = 17,
        DespReestr = 18,
        TrasladoConavi = 22
    }

    public enum EstadoCivil
    {
        Soltero = 1,
        Casado = 2,
        Divorciado = 3,
        UnionLibre = 4,
        Viudo = 5
    }

    public enum TiposContacto
    {
        TelDomicilio = 1,
        TelTrabajo = 2,
        TelCelular = 3,
        TelAdicional = 4,
        Email = 5
    }

    public enum EstadosNombramiento
    {
        Propiedad = 1,
        Interino = 2,
        Congelada = 3,
        VacSuspIndefinida = 4,
        NombramientoTramite = 5,
        VacantePSS = 6,
        VacanteAscInt = 7,
        VacanteDescInt = 8,
        AscensoInterino = 9,
        Vacante = 10,
        PedimentoPermanente = 11,
        PedimentoProvisional = 12,
        PeriodoPrueba = 13,
        PedimentoInconcluso = 14,
        NombramientoInactivo = 15
    }

    public enum Calificaciones
    {
        Excelente = 1,
        MuyBueno = 2,
        Bueno = 3,
        Regular = 4
    }

    public enum TiposUbicacion
    {
        Contrato = 1,
        Trabajo = 2
    }

    public enum Generos
    {
        Masculino = 1,
        Femenino = 2
    }
}
