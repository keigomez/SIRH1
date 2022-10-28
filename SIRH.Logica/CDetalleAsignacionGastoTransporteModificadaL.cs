using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
    public class CDetalleAsignacionGastoTransporteModificadaL
    {
		#region Variables

        SIRHEntities contexto;

        #endregion

        #region Constructor

        public CDetalleAsignacionGastoTransporteModificadaL()
        {
            contexto = new SIRHEntities();
        }

        #endregion

        #region Métodos

        internal static CDetalleAsignacionGastoTransporteModificadaDTO ConvertirDetalleAsigGTModificadaDTOaDatos(DetalleAsignacionGastoTransporteModificada item)
        {
            return new CDetalleAsignacionGastoTransporteModificadaDTO
            {
                GastoTransporteDTO = CGastoTransporteL.ConvertirGastoTransporteDatosaDTO(item.GastoTransporte),
                NomRutaDTO = item.NomRuta,
                NomFraccionamientoDTO = item.NomFraccionamiento,
                MontTarifa = item.MontTarifa,
                NumGaceta = item.NumGaceta
            };
        }

        /// <summary>
        /// Agregar en la BD la lista de rutas modificadas
        /// </summary>
        /// <param name="detalleAsigModif">Lista de rutas del gasto que se van a modificar</param>
        /// <param name="idgasto">PK del gasto de transporte al que se le modifican las rutas</param>
        /// <returns></returns>
        public List<CBaseDTO> AgregarDetalleAsignacionGTModificada(List<CDetalleAsignacionGastoTransporteModificadaDTO> detalleAsigModif, int idgasto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CDetalleAsignacionGastoTrasporteModificadaD intermedioAsignModif = new CDetalleAsignacionGastoTrasporteModificadaD(contexto);
                List<DetalleAsignacionGastoTransporteModificada> tempInsertar = new List<DetalleAsignacionGastoTransporteModificada>();//Para guardar las rutas que se insertan y hacerlo al final


                var modifExistentes = ListarAsignacionModificada(idgasto); // obtener las que ya se han modificado para ese gasto

                //Por cada una de las rutas modificadas
                foreach (var item in detalleAsigModif)
                {
                    bool nueva = false;

                    //Crear el detalle (las rutas del GT) modificado
                    DetalleAsignacionGastoTransporteModificada detalleRutasBDModif = new DetalleAsignacionGastoTransporteModificada
                    {
                        PK_DetalleAsignacionGastoModificada = item.IdEntidad,
                        MontTarifa = item.MontTarifa,
                        NomFraccionamiento = item.NomFraccionamientoDTO,
                        NomRuta = item.NomRutaDTO,
                        NumGaceta = item.NumGaceta
                    };
                    // si ya existe solo editela sino insertela

                    for (int k = 0; k < modifExistentes.Count(); k++)
                    {
                        CDetalleAsignacionGastoTransporteModificadaDTO rutaExistente =
                            (CDetalleAsignacionGastoTransporteModificadaDTO)modifExistentes[k];

                        //Si la ruta a agregar ya esta en las existentes
                        if ((item.NomRutaDTO.Equals(rutaExistente.NomRutaDTO) &&
                            item.NomFraccionamientoDTO.Equals(rutaExistente.NomFraccionamientoDTO)))
                        {
                            //editar
                            nueva = false;
                            var actualizaTarifa = intermedioAsignModif.ActualizarTarifaRutaModificada(rutaExistente.IdEntidad, item.MontTarifa);
                            if (actualizaTarifa.Codigo > 0)
                            {
                                respuesta.Add(actualizaTarifa);
                            }
                            else
                            {
                                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = "Ocurrió un error al actualizar la tarifa, por favor consulte con el personal correspondiente" } };
                                //throw new Exception(((CErrorDTO)respuesta[2]).MensajeError);
                            }
                            break;
                        }
                        else
                        {
                            //insertar
                            nueva = true;
                        }
                    }
                    //Solo se inserta si sí hay nuevas o si no habían en las existentes.
                    if (nueva)
                    {
                        tempInsertar.Add(detalleRutasBDModif);
                    }
                    else if (modifExistentes.Count == 0)
                    {
                        tempInsertar.Add(detalleRutasBDModif);
                    }
                }

                //Verificar si hay rutas nuevas para insertar
                if (tempInsertar.Count > 0)
                {
                    foreach (var cosa in tempInsertar)
                    {
                        var insertarDetalleRutas = intermedioAsignModif.AgregarDetalleAsignacionGTModificada(cosa, idgasto);
                        if (insertarDetalleRutas.Codigo > 0)
                        {
                            respuesta.Add(ConvertirDetalleAsigGTModificadaDTOaDatos((DetalleAsignacionGastoTransporteModificada)insertarDetalleRutas.Contenido));
                        }
                        else
                        {
                            respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = "Ocurrió un error al insertar la modificación de la ruta, por favor consulte al personal correspondiente" } };
                            //throw new Exception(((CErrorDTO)respuesta[2]).MensajeError);
                        }
                    }
                }

                return respuesta;
            }
            catch
            {
                return respuesta;
            }

        }

        public List<CBaseDTO> ListarAsignacionModificada(int idgasto)
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();
            try
            {
                CDetalleAsignacionGastoTrasporteModificadaD intermedio = new CDetalleAsignacionGastoTrasporteModificadaD(contexto);
                var resultado = intermedio.ListarAsignacionModificada(idgasto);
                if (resultado != null)
                {
                    List<CBaseDTO> data = new List<CBaseDTO>();
                    foreach (var item in resultado)
                    {
                        data.Add(new CDetalleAsignacionGastoTransporteModificadaDTO
                        {
                            IdEntidad = item.PK_DetalleAsignacionGastoModificada,
                            NomRutaDTO = item.NomRuta,
                            NomFraccionamientoDTO = item.NomFraccionamiento,
                            MontTarifa = item.MontTarifa,
                            GastoTransporteDTO = CGastoTransporteL.ConvertirGastoTransporteDatosaDTO(item.GastoTransporte),
                            NumGaceta = item.NumGaceta
                        });
                    }
                    respuesta = data;
                }
                else
                {
                    respuesta = new List<CBaseDTO> { new CDetalleAsignacionGastoTransporteModificadaDTO { Mensaje = "No se encontraron asignaciones de gasto modificadas para este gasto de transporte" } };
                }
            }
            catch (Exception error)
            {
                respuesta = new List<CBaseDTO> { new CErrorDTO { MensajeError = error.Message } };
                return respuesta;
            }

            return respuesta;
        }
        #endregion
    }
}
