using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIRH.Datos;
using SIRH.DTO;

namespace SIRH.Logica
{
   public  class CMotivoMovimientoL
   
    {
        #region Variables

        private SIRHEntities contexto;

        private string desMotivo;
                
        
        public string DescripcionMotivo
        {
            get
            {
                return desMotivo;
            }
            set
            {
                desMotivo = value;
            }
        }
                 
        #endregion

        #region Constructor

        public CMotivoMovimientoL()
        {
             contexto = new SIRHEntities();
        }
         
        #endregion

        #region Metodos
               
        public int GuardarMotivoMovimiento(CMotivoMovimientoDTO MotivoMovimiento)
        {           
            MotivoMovimiento temp = new MotivoMovimiento();
                       
            return 0;
        }

       //se inserta en ICPuestoService y CPuestoService 27/01/2017
        public CMotivoMovimientoDTO CargarMotivoMovimientoPorPuesto(string NumeroPuesto)
        {
            CMotivoMovimientoDTO respuesta = new CMotivoMovimientoDTO();

            MotivoMovimiento temp = new MotivoMovimiento();                        
            CMotivoMovimientoD intermedio = new CMotivoMovimientoD(contexto);
            temp = intermedio.CargarMotivoMovimientoPorPuesto(NumeroPuesto);

            respuesta.IdEntidad = temp.PK_MotivoMovimiento;
            respuesta.DesMotivo = temp.DesMotivo;
            respuesta.IndEstMotivoMovimiento = Convert.ToInt32(temp.IndEstadoMotivoMovimiento);
            return respuesta;
        }

        //se inserta en ICPuestoService y CPuestoService 27/01/2017        
        public CMotivoMovimientoDTO CargarMotivoMovimientoPorCedula(string Cedula)
        {
            CMotivoMovimientoDTO respuesta = new CMotivoMovimientoDTO();

            MotivoMovimiento temp = new MotivoMovimiento();                        
            CMotivoMovimientoD intermedio = new CMotivoMovimientoD(contexto);
            temp = intermedio.CargarMotivoMovimientoPorCedula(Cedula);

            respuesta.IdEntidad = temp.PK_MotivoMovimiento;
            respuesta.DesMotivo = temp.DesMotivo;
            respuesta.IndEstMotivoMovimiento = Convert.ToInt32(temp.IndEstadoMotivoMovimiento);

            return respuesta;
        }
        //se inserta en ICPuestoService y CPuestoService 27/01/2017      
        public List<CMotivoMovimientoDTO> DescargarMotivoMovimientoCompleto()
        {
            List<CMotivoMovimientoDTO> respuesta = new List<CMotivoMovimientoDTO>();

            CMotivoMovimientoD intermedio = new CMotivoMovimientoD(contexto);
            foreach (var item in intermedio.DescargarMotivoMovimientoCompleto())
            {
                CMotivoMovimientoDTO temp = new CMotivoMovimientoDTO();
                temp.IdEntidad = item.PK_MotivoMovimiento;
                temp.DesMotivo = item.DesMotivo;
                temp.IndEstMotivoMovimiento = Convert.ToInt32(item.IndEstadoMotivoMovimiento);
                respuesta.Add(temp);
            }

            return respuesta;
        }

        public List<CBaseDTO> ListarMotivosMovimiento()
        {
            List<CBaseDTO> respuesta = new List<CBaseDTO>();

            try
            {
                CMotivoMovimientoD intermedio = new CMotivoMovimientoD(contexto);
                var resultado = intermedio.ListarMotivosMovimiento();

                if (resultado.Codigo > 0)
                {
                    var motivos = ((List<MotivoMovimiento>)resultado.Contenido);
                    foreach (var item in motivos)
                    {
                        respuesta.Add(ConvertirMotivoMovimientoADTO(item));
                    }
                    return respuesta;
                }
                else
                {
                    throw new Exception(((CErrorDTO)resultado.Contenido).MensajeError);
                }
            }
            catch (Exception error)
            {
                respuesta.Add(new CErrorDTO { MensajeError = error.Message });
                return respuesta;
            }
        }

        internal static CMotivoMovimientoDTO ConvertirMotivoMovimientoADTO(MotivoMovimiento item)
        {
            return new CMotivoMovimientoDTO
            {
                IdEntidad = item.PK_MotivoMovimiento,
                DesMotivo = item.DesMotivo
            };
        }

        #endregion
    }
}